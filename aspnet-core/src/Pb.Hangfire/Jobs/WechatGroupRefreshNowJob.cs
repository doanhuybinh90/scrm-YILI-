using Abp.BackgroundJobs;
using Abp.Dependency;
using Castle.Core.Logging;
using Pb.Hangfire.Tool;
using Pb.Wechat.MpAccessTokenClib;
using Pb.Wechat.MpAccounts;
using Pb.Wechat.MpAccounts.Dto;
using Senparc.Weixin.Helpers;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.User;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Pb.Hangfire.Jobs
{
    public class WechatGroupRefreshNowJob : BackgroundJob<bool>, ITransientDependency
    {
        private readonly ILogger LogWriter;
        private readonly IMpAccountAppService _mpAccountAppService;
        private readonly IAccessTokenContainer _accessTokenContainer;
        public WechatGroupRefreshNowJob(ILogger logger, IMpAccountAppService mpAccountAppService, IAccessTokenContainer accessTokenContainer)
        {
            LogWriter = logger;
            _mpAccountAppService = mpAccountAppService;
            _accessTokenContainer = accessTokenContainer;
        }

        public override void Execute(bool args)
        {
            Task.WaitAll(DoExecute());
        }

        private async Task DoExecute() {

            try
            {
                var mpList =await _mpAccountAppService.GetList();
                foreach (var mp in mpList)
                {
                    try
                    {
                        LogWriter.Info(string.Format("同步公众号{0}[{1}]的用户开始", mp.Name, mp.Id));
                        await ReFreshFans(mp);
                        LogWriter.Info(string.Format("同步公众号{0}[{1}]的用户结束", mp.Name, mp.Id));
                    }
                    catch (Exception ex)
                    {

                        LogWriter.Error(string.Format("同步公众号{0}[{1}]的分组和用户出现错误,错误信息:{2}", mp.Name, mp.Id, ex));
                    }

                }
            }
            catch (Exception e)
            {

                LogWriter.Error(string.Format("同步公众号出现错误,错误信息:{0}", e));
            }
        }

        private async Task ReFreshFans(MpAccountDto model)
        {
            var mpId = model.Id;
            #region 初始化
            var sb = new StringBuilder();
            int stoptick = 0, stopcount = 100;
            var fanssavestr = @"
if not exists(select * from MpFans where MpID='{0}' and OpenID='{1}')
insert into MpFans ([MpID],[OpenID],[NickName],[Sex],[Language],[City],[Province],[Country],[HeadImgUrl],[SubscribeTime],[UnionID],[WxGroupID],[IsFans],[UpdateTime],CreationTime,IsDeleted) values ({0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}',getdate(),getdate(),0)
else
update MpFans SET [MpID]='{0}',[OpenID]='{1}',[NickName]='{2}',[Sex]='{3}',[Language]='{4}',[City]='{5}',[Province]='{6}',[Country]='{7}',[HeadImgUrl]='{8}',[SubscribeTime]='{9}',[UnionID]='{10}',[WxGroupID]='{12}',[IsFans]='{13}',[UpdateTime]=getdate(),IsDeleted=0 WHERE MpID='{0}' and OpenID='{1}'
";
            #endregion

            #region 更新用户
            #region 更新前10000个用户
            OpenIdResultJson fansopenids = null;
            try
            {

                fansopenids = UserApi.Get(await _accessTokenContainer.TryGetAccessTokenAsync(model.AppId, model.AppSecret), "");
            }
            catch (Exception ex)
            {
                LogWriter.Error(string.Format("获取MpID为{0}的openid报错,错误信息：{1}", mpId, ex));
                fansopenids = UserApi.Get(await _accessTokenContainer.TryGetAccessTokenAsync(model.AppId, model.AppSecret, true), "");
            }
            DateTime now = DateTime.Now;
            if (fansopenids.count > 0)
            {
                foreach (var id in fansopenids.data.openid)
                {
                    UserInfoJson wxinfo = null;
                    try
                    {
                        wxinfo = UserApi.Info(await _accessTokenContainer.TryGetAccessTokenAsync(model.AppId, model.AppSecret), id);
                    }
                    catch (Exception ex)
                    {
                        LogWriter.Error(string.Format("获取MpID为{0}，openid为{1}的用户信息报错,错误信息：{2}", mpId.ToString(), id, ex));
                        wxinfo = UserApi.Info(await _accessTokenContainer.TryGetAccessTokenAsync(model.AppId, model.AppSecret, true), id);
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
                            await StaticObjects.db.ExcuteNonQueryAsync(sb.ToString(), null, false);
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
                    fansopenids = UserApi.Get(await _accessTokenContainer.TryGetAccessTokenAsync(model.AppId, model.AppSecret), fansopenids.next_openid);
                }
                catch (Exception ex)
                {
                    LogWriter.Error(string.Format("获取MpID为{0}的openid报错，nextopenid为{1},错误信息：{2}", mpId, fansopenids.next_openid, ex));
                    fansopenids = UserApi.Get(await _accessTokenContainer.TryGetAccessTokenAsync(model.AppId, model.AppSecret, true), fansopenids.next_openid);
                }
                if (fansopenids.count > 0)
                {
                    foreach (var id in fansopenids.data.openid)
                    {
                        UserInfoJson wxinfo = null;
                        try
                        {
                            wxinfo = UserApi.Info(await _accessTokenContainer.TryGetAccessTokenAsync(model.AppId, model.AppSecret), id);
                        }
                        catch (Exception ex)
                        {
                            LogWriter.Error(string.Format("获取MpID为{0}，openid为{1}的用户信息报错,错误信息：{2}", mpId, id, ex));
                            wxinfo = UserApi.Info(await _accessTokenContainer.TryGetAccessTokenAsync(model.AppId, model.AppSecret, true), id);
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
                                await StaticObjects.db.ExcuteNonQueryAsync(sb.ToString(), null, false);
                            sb.Clear();
                        }
                    }
                }
            }
            #endregion

#warning 该方法不够严谨
            //没更新到的用户设为取消关注
            sb.AppendFormat("update MpFans set IsFans='0' where MpID='{0}' and UpdateTime<'{1}'", mpId, now.AddDays(-1));
            #endregion

            LogWriter.Info(sb.ToString());
            if (sb.Length > 0)
                await StaticObjects.db.ExcuteNonQueryAsync(sb.ToString(), null, false);
            LogWriter.Info(string.Format("更新数量{0},更新完毕", stoptick));
        }
    }
}
