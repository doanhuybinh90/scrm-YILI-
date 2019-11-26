using Hangfire;
using Pb.Hangfire.Tool;
using Pb.Wechat.MpAccounts;
using Senparc.Weixin.Helpers;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.User;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Pb.Hangfire.Jobs
{
    public class WechatGroupRefreshJob : IJob

    {
        [AutomaticRetry(Attempts = 1)]
        public void Run() {
            Task.WaitAll(DoRun());
        }

        private async Task DoRun() {
            IDBHelper db = new MssqlHelper(JobConfig.ConnectionStrings["YiliscrmDb"]);
            var mpList = db.FindToList<MpAccount>("SELECT * FROM MpAccounts WHERE ISDELETED=0 ", null, false);
            foreach (var mp in mpList)
            {
                await ReFreshFans(mp);
            }
        }

        private async Task ReFreshFans(MpAccount model)
        {
            var mpId = model.Id;
            #region 初始化
            var sb = new StringBuilder();
            int stoptick = 0, stopcount = 100;
            var fanssavestr = @"
if not exists(select * from MpFans where MpID='{0}' and OpenID='{1}')
insert into MpFans ([MpID],[OpenID],[NickName],[Sex],[Language],[City],[Province],[Country],[HeadImgUrl],[SubscribeTime],[UnionID],[WxGroupID],[IsFans],[UpdateTime],CreationTime,IsDeleted) values ({0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{12}','{13}',getdate(),getdate(),0)
else
update MpFans SET [MpID]='{0}',[OpenID]='{1}',[NickName]='{2}',[Sex]='{3}',[Language]='{4}',[City]='{5}',[Province]='{6}',[Country]='{7}',[HeadImgUrl]='{8}',[SubscribeTime]='{9}',[UnionID]='{10}',[WxGroupID]='{12}',[IsFans]='{13}',[UpdateTime]=getdate(),IsDeleted=0 WHERE MpID='{0}' and OpenID='{1}'
";
            #endregion

            #region 更新用户
            #region 更新前10000个用户
            OpenIdResultJson fansopenids = null;
            string _accesstoken = "";
            try
            {
                _accesstoken = (await StaticObjects.GetAccessToken(model.TaskAccessToken)).access_token;
                fansopenids = UserApi.Get(_accesstoken, "");
            }
            catch(Exception e)
            {
                _accesstoken = (await StaticObjects.GetAccessToken(model.TaskAccessToken,1)).access_token;
                //LogWriter.Error(string.Format("获取MpID为{0}的openid报错,错误信息：{1}", mpId, ex));
                fansopenids = UserApi.Get(_accesstoken, "");
            }
            DateTime now = DateTime.Now;
            if (fansopenids.count > 0)
            {
                foreach (var id in fansopenids.data.openid)
                {
                    UserInfoJson wxinfo = null;
                    try
                    {
                        wxinfo = UserApi.Info((await StaticObjects.GetAccessToken(model.TaskAccessToken)).access_token, id);
                    }
                    catch
                    {
                        //LogWriter.Error(string.Format("获取MpID为{0}，openid为{1}的用户信息报错,错误信息：{2}", mpId.ToString(), id, ex));
                        wxinfo = UserApi.Info((await StaticObjects.GetAccessToken(model.TaskAccessToken, 1)).access_token, id);
                    }

                    sb.AppendFormat(fanssavestr, mpId, id,
                        DbTool.ToSqlParamString(wxinfo.nickname), wxinfo.sex, DbTool.ToSqlParamString(wxinfo.language),
                        DbTool.ToSqlParamString(wxinfo.city), DbTool.ToSqlParamString(wxinfo.province),
                        DbTool.ToSqlParamString(wxinfo.country), DbTool.ToSqlParamString(wxinfo.headimgurl),
                        DateTimeHelper.GetDateTimeFromXml(wxinfo.subscribe_time), DbTool.ToSqlParamString(wxinfo.unionid),
                        DbTool.ToSqlParamString(wxinfo.remark), wxinfo.groupid, wxinfo.subscribe);
                    stoptick++;
                    if (stoptick % stopcount == 0 || stoptick == fansopenids.count)
                    {
                        if (sb.Length > 0)
                            StaticObjects.db.ExcuteNonQuery(sb.ToString(),null,false);
                        sb.Clear();
                    }
                }
            }
            #endregion

            #region while循环更新后续所有用户
            while (!string.IsNullOrEmpty(fansopenids.next_openid))
            {
                try
                {
                    fansopenids = UserApi.Get((await StaticObjects.GetAccessToken(model.TaskAccessToken)).access_token, fansopenids.next_openid);
                }
                catch
                {
                    //LogWriter.Error(string.Format("获取MpID为{0}的openid报错，nextopenid为{1},错误信息：{2}", mpId, fansopenids.next_openid, ex));
                    fansopenids = UserApi.Get((await StaticObjects.GetAccessToken(model.TaskAccessToken,1)).access_token, fansopenids.next_openid);
                }
                if (fansopenids.count > 0)
                {
                    foreach (var id in fansopenids.data.openid)
                    {
                        UserInfoJson wxinfo = null;
                        try
                        {
                            wxinfo = UserApi.Info((await StaticObjects.GetAccessToken(model.TaskAccessToken)).access_token, id);
                        }
                        catch
                        {
                            //LogWriter.Error(string.Format("获取MpID为{0}，openid为{1}的用户信息报错,错误信息：{2}", mpId, id, ex));
                            wxinfo = UserApi.Info((await StaticObjects.GetAccessToken(model.TaskAccessToken,1)).access_token, id);
                        }

                        sb.AppendFormat(fanssavestr, mpId, id,
                            DbTool.ToSqlParamString(wxinfo.nickname), wxinfo.sex, DbTool.ToSqlParamString(wxinfo.language),
                            DbTool.ToSqlParamString(wxinfo.city), DbTool.ToSqlParamString(wxinfo.province),
                            DbTool.ToSqlParamString(wxinfo.country), DbTool.ToSqlParamString(wxinfo.headimgurl),
                            DateTimeHelper.GetDateTimeFromXml(wxinfo.subscribe_time), DbTool.ToSqlParamString(wxinfo.unionid),
                            DbTool.ToSqlParamString(wxinfo.remark), wxinfo.groupid, wxinfo.subscribe);
                        stoptick++;
                        if (stoptick % stopcount == 0 || stoptick == fansopenids.count)
                        {
                            if (sb.Length > 0)
                                StaticObjects.db.ExcuteNonQuery(sb.ToString(), null, false);
                            sb.Clear();
                        }
                    }
                }
            }
            #endregion


            //没更新到的用户设为取消关注
            sb.AppendFormat("update MpFans set IsFans='0' where MpID='{0}' and UpdateTime<'{1}'", mpId, now.AddDays(-1));
            #endregion

            if (sb.Length>0)
            StaticObjects.db.ExcuteNonQuery(sb.ToString(), null, false);
            //LogWriter.Info(string.Format("更新数量{0},更新完毕", stoptick));
        }

        #region 注入代码
        //: PeriodicBackgroundWorkerBase, ISingletonDependency
        //private readonly ILogger LogWriter;
        //private readonly IMpAccountAppService _mpAccountAppService;
        //private readonly IAccessTokenContainer _accessTokenContainer;
        //public WechatGroupRefreshJob(AbpTimer timer, ILogger logger, IMpApiTokenAppService mpApiTokenAppService, IMpAccountAppService mpAccountAppService, IAccessTokenContainer accessTokenContainer) : base(timer)
        //{
        //    LogWriter = logger;
        //    _mpAccountAppService = mpAccountAppService;
        //    _accessTokenContainer = accessTokenContainer;
        //    Timer.Period = 86400000;
        //    //Timer.Period = 60000;
        //}

        //        [UnitOfWork]
        //        protected override void DoWork()
        //        {
        //            try
        //            {
        //                var mpList = _mpAccountAppService.GetList().Result;
        //                //var mpList = StaticObjects.db.FindToList<MpModel>("SELECT Id,Name FROM MpAccounts WHERE ISDELETED=0");
        //                foreach (var mp in mpList)
        //                {
        //                    try
        //                    {
        //                        LogWriter.Info(string.Format("同步公众号{0}[{1}]的用户开始", mp.Name, mp.Id));
        //                        ReFreshFans(mp);
        //                        LogWriter.Info(string.Format("同步公众号{0}[{1}]的用户结束", mp.Name, mp.Id));
        //                    }
        //                    catch (Exception ex)
        //                    {

        //                        LogWriter.Error(string.Format("同步公众号{0}[{1}]的分组和用户出现错误,错误信息:{2}", mp.Name, mp.Id, ex));
        //                    }

        //                }
        //            }
        //            catch (Exception e)
        //            {

        //                LogWriter.Error(string.Format("同步公众号出现错误,错误信息:{0}", e));
        //            }
        //        }

        //        private void ReFreshFans(MpAccount model)
        //        {
        //            var mpId = model.Id;
        //            #region 初始化
        //            var sb = new StringBuilder();
        //            int stoptick = 0, stopcount = 100; 
        //            var fanssavestr = @"
        //if not exists(select * from MpFans where MpID='{0}' and OpenID='{1}')
        //insert into MpFans ([MpID],[OpenID],[NickName],[Sex],[Language],[City],[Province],[Country],[HeadImgUrl],[SubscribeTime],[UnionID],[Remark],[WxGroupID],[IsFans],[UpdateTime],CreationTime,IsDeleted) values ({0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}',getdate(),getdate(),0)
        //else
        //update MpFans SET [MpID]='{0}',[OpenID]='{1}',[NickName]='{2}',[Sex]='{3}',[Language]='{4}',[City]='{5}',[Province]='{6}',[Country]='{7}',[HeadImgUrl]='{8}',[SubscribeTime]='{9}',[UnionID]='{10}',[Remark]='{11}',[WxGroupID]='{12}',[IsFans]='{13}',[UpdateTime]=getdate(),IsDeleted=0 WHERE MpID='{0}' and OpenID='{1}'
        //";
        //            #endregion

        //            #region 更新用户
        //            #region 更新前10000个用户
        //            OpenIdResultJson fansopenids = null;
        //            try
        //            {

        //                fansopenids = UserApi.Get(_accessTokenContainer.TryGetAccessTokenAsync(model.AppId,model.AppSecret).Result, "");
        //            }
        //            catch (Exception ex)
        //            {
        //                LogWriter.Error( string.Format("获取MpID为{0}的openid报错,错误信息：{1}", mpId, ex));
        //                fansopenids = UserApi.Get(_accessTokenContainer.TryGetAccessTokenAsync(model.AppId, model.AppSecret,true).Result, "");
        //            }
        //            DateTime now = DateTime.Now;
        //            if (fansopenids.count > 0)
        //            {
        //                foreach (var id in fansopenids.data.openid)
        //                {
        //                    UserInfoJson wxinfo = null;
        //                    try
        //                    {
        //                        wxinfo = UserApi.Info(_accessTokenContainer.TryGetAccessTokenAsync(model.AppId, model.AppSecret).Result, id);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        LogWriter.Error(string.Format("获取MpID为{0}，openid为{1}的用户信息报错,错误信息：{2}", mpId.ToString(), id, ex));
        //                        wxinfo = UserApi.Info(_accessTokenContainer.TryGetAccessTokenAsync(model.AppId, model.AppSecret,true).Result, id);
        //                    }

        //                    sb.AppendFormat(fanssavestr,  mpId, id,
        //                        DbTool.ToSqlParamString(wxinfo.nickname), wxinfo.sex, DbTool.ToSqlParamString(wxinfo.language),
        //                        DbTool.ToSqlParamString(wxinfo.city), DbTool.ToSqlParamString(wxinfo.province),
        //                        DbTool.ToSqlParamString(wxinfo.country), DbTool.ToSqlParamString(wxinfo.headimgurl),
        //                        DateTimeHelper.GetDateTimeFromXml(wxinfo.subscribe_time), DbTool.ToSqlParamString(wxinfo.unionid),
        //                        DbTool.ToSqlParamString(wxinfo.remark), wxinfo.groupid, wxinfo.subscribe);
        //                    stoptick++;
        //                    if (stoptick % stopcount == 0)
        //                    {
        //                        StaticObjects.db.ExcuteNonQuery(sb.ToString());
        //                        sb.Clear();
        //                    }
        //                }
        //            }
        //            #endregion

        //            #region while循环更新后续所有用户
        //            while (!string.IsNullOrEmpty(fansopenids.next_openid))
        //            {
        //                try
        //                {
        //                    fansopenids = UserApi.Get(_accessTokenContainer.TryGetAccessTokenAsync(model.AppId, model.AppSecret).Result, fansopenids.next_openid);
        //                }
        //                catch (Exception ex)
        //                {
        //                    LogWriter.Error( string.Format("获取MpID为{0}的openid报错，nextopenid为{1},错误信息：{2}", mpId, fansopenids.next_openid, ex));
        //                    fansopenids = UserApi.Get(_accessTokenContainer.TryGetAccessTokenAsync(model.AppId, model.AppSecret,true).Result, fansopenids.next_openid);
        //                }
        //                if (fansopenids.count > 0)
        //                {
        //                    foreach (var id in fansopenids.data.openid)
        //                    {
        //                        UserInfoJson wxinfo = null;
        //                        try
        //                        {
        //                            wxinfo = UserApi.Info(_accessTokenContainer.TryGetAccessTokenAsync(model.AppId, model.AppSecret).Result, id);
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            LogWriter.Error(string.Format("获取MpID为{0}，openid为{1}的用户信息报错,错误信息：{2}", mpId, id, ex));
        //                            wxinfo = UserApi.Info(_accessTokenContainer.TryGetAccessTokenAsync(model.AppId, model.AppSecret,true).Result, id);
        //                        }

        //                        sb.AppendFormat(fanssavestr, mpId, id,
        //                            DbTool.ToSqlParamString(wxinfo.nickname), wxinfo.sex, DbTool.ToSqlParamString(wxinfo.language),
        //                            DbTool.ToSqlParamString(wxinfo.city), DbTool.ToSqlParamString(wxinfo.province),
        //                            DbTool.ToSqlParamString(wxinfo.country), DbTool.ToSqlParamString(wxinfo.headimgurl),
        //                            DateTimeHelper.GetDateTimeFromXml(wxinfo.subscribe_time), DbTool.ToSqlParamString(wxinfo.unionid),
        //                            DbTool.ToSqlParamString(wxinfo.remark), wxinfo.groupid, wxinfo.subscribe);
        //                        stoptick++;
        //                        if (stoptick % stopcount == 0)
        //                        {
        //                            StaticObjects.db.ExcuteNonQuery(sb.ToString());
        //                            sb.Clear();
        //                        }
        //                    }
        //                }
        //            }
        //            #endregion

        //#warning 该方法不够严谨
        //            //没更新到的用户设为取消关注
        //            sb.AppendFormat("update MpFans set IsFans='0' where MpID='{0}' and UpdateTime<'{1}'", mpId, now.AddDays(-1));
        //            #endregion


        //            StaticObjects.db.ExcuteNonQuery(sb.ToString());
        //            LogWriter.Info(string.Format("更新数量{0},更新完毕", stoptick));
        //        }
        #endregion
    }

    public class MpModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class MpGroupModel
    {
        public int Id { get; set; }
        public int WxGroupID { get; set; }
        public int Length { get; set; }
    }

    public class ApiTokenResult
    {
        public string access_token { get; set; }
    }
}
