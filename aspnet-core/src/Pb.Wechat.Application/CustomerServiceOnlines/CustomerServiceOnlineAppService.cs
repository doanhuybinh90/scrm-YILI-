using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Pb.Wechat.Auditing.Exporting;
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerServiceOnlines.Dto;
using Pb.Wechat.UserMps;
using System.Linq;
using System.Threading.Tasks;
using Pb.Wechat.WxMedias;
using Abp.Authorization;
using Pb.Wechat.Authorization;
using System;
using Abp.Domain.Uow;
using Pb.Wechat.CustomerServiceConversations;
using Pb.Wechat.CustomerServiceConversations.Dto;
using System.Collections.Generic;
using Newtonsoft.Json;
using Abp.UI;

namespace Pb.Wechat.CustomerServiceOnlines
{
    //[AbpAuthorize(AppPermissions.Pages_CustomerService_CustomerServiceOnline)]
    public class CustomerServiceOnlineAppService : AsyncCrudAppService<CustomerServiceOnline, CustomerServiceOnlineDto, int, GetCustomerServiceOnlinesInput, CustomerServiceOnlineDto, CustomerServiceOnlineDto>, ICustomerServiceOnlineAppService
    {
        private readonly IWxMediaAppService _wxMediaAppService;
        private readonly IUserMpAppService _userMpAppService;
        private readonly ICustomerServiceOnlineListExcelExporter _CustomerServiceOnlineListExcelExporter;
        private readonly IRepository<CustomerServiceConversation, long> _conversationReposity;
        public CustomerServiceOnlineAppService(IRepository<CustomerServiceOnline, int> repository, ICustomerServiceOnlineListExcelExporter CustomerServiceOnlineListExcelExporter, IUserMpAppService userMpAppService, IWxMediaAppService wxMediaAppService, IRepository<CustomerServiceConversation, long> conversationReposity) : base(repository)
        {
            _CustomerServiceOnlineListExcelExporter = CustomerServiceOnlineListExcelExporter;
            _userMpAppService = userMpAppService;
            _wxMediaAppService = wxMediaAppService;
            _conversationReposity = conversationReposity;
        }

        protected override IQueryable<CustomerServiceOnline> CreateFilteredQuery(GetCustomerServiceOnlinesInput input)
        {

            return Repository.GetAll()
                .Where(c=>c.KfType=="YL")
                .WhereIf(input.MpID!=0,c => c.MpID == input.MpID)
                .WhereIf(!string.IsNullOrWhiteSpace(input.KfId), c => c.KfId.Contains(input.KfId))
                .WhereIf(!string.IsNullOrWhiteSpace(input.KfNick), c => c.KfNick.Contains(input.KfNick))
                .WhereIf(!string.IsNullOrWhiteSpace(input.KfWx), c => c.KfWx.Contains(input.KfWx))
                .WhereIf(!string.IsNullOrWhiteSpace(input.OpenID), c => c.OpenID == input.OpenID)
                .WhereIf(input.OnlineState.HasValue, c => c.OnlineState == input.OnlineState)
                .WhereIf(input.ConnectState.HasValue, c => c.ConnectState == input.ConnectState);
        }

        public override async Task<CustomerServiceOnlineDto> Update(CustomerServiceOnlineDto input)
        {
            CheckUpdatePermission();
            if ((await Repository.CountAsync(m => m.OpenID == input.OpenID && m.IsDeleted == false && m.Id != input.Id)) > 0)
                throw new UserFriendlyException("对不起，OpenID已绑定另外的账号");
            var entity =await Repository.UpdateAsync(input.Id,async m =>
            {
                m.KfAccount = input.KfAccount;
                m.KfHeadingUrl = input.KfHeadingUrl;
                m.KfNick = input.KfNick;
                m.KfWx = input.KfWx;
                m.LastModificationTime = DateTime.Now;
                m.PreKfAccount = input.PreKfAccount;
                m.LocalHeadingUrl = input.LocalHeadingUrl;
                m.LocalHeadFilePath = input.LocalHeadFilePath;
            });

            MapToEntity(input, entity);
            return MapToEntityDto(entity);
        }

        public override Task Delete(EntityDto<int> input)
        {
            var data = Repository.Get(input.Id);
            _wxMediaAppService.DeleteCustom(data.MpID, data.KfAccount);
            return base.Delete(input);
        }
        public async Task<FileDto> GetListToExcel(GetCustomerServiceOnlinesInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _CustomerServiceOnlineListExcelExporter.ExportToFile(dtos);
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task GetKfList()
        {
            var wxType = KFType.WX.ToString();
           await  Repository.DeleteAsync(m => m.KfType == wxType);
            var mpid = await _userMpAppService.GetDefaultMpId();
            var kflist = await _wxMediaAppService.GetCurstomerList(mpid);
            foreach (var kf in kflist.kf_list)
            {
                var item = Repository.FirstOrDefault(m => m.KfAccount == kf.kf_account);
                if (item == null)
                {
                    CustomerServiceOnline model = new CustomerServiceOnline()
                    {
                        KfAccount = kf.kf_account,
                        KfNick = kf.kf_nick,
                        KfId = kf.kf_id.ToString(),
                        KfHeadingUrl = kf.kf_headimgurl,
                        PreKfAccount = kf.kf_account.Substring(0, kf.kf_account.IndexOf("@")),
                        PublicNumberAccount = kf.kf_account.Substring(kf.kf_account.IndexOf("@") + 1, kf.kf_account.Length - kf.kf_account.IndexOf("@") - 1),
                        MpID = mpid,
                        LastModificationTime = DateTime.Now,
                        AutoJoin = false,
                        AutoJoinCount = 0,
                        ConnectState = 1,
                        CreationTime = DateTime.Now,
                        IsDeleted = false,
                        KfType = KFType.WX.ToString(),
                        OnlineState = 1,
                        LocalHeadingUrl = kf.kf_headimgurl,
                        KfWx = kf.kf_wx
                    };
                    Repository.Insert(model);
                }
                else
                {
                    Repository.Update(item.Id, m =>
                    {
                        m.KfHeadingUrl = kf.kf_headimgurl;
                        m.LastModificationTime = DateTime.Now;
                        m.KfId = kf.kf_id.ToString();
                        m.KfWx = kf.kf_wx;
                    });
                }
            }
        }

        public async Task<CustomerServiceOnlineDto> GetFirstOrDefault(GetCustomerServiceOnlinesInput input)
        {
            return MapToEntityDto(await AsyncQueryableExecuter.FirstOrDefaultAsync(CreateFilteredQuery(input)));
        }

        /// <summary>
        /// 根据所有在线客服的接入比进行分配首选客服
        /// </summary>
        /// <param name="mpId"></param>
        /// <returns></returns>
        public async Task<CustomerServiceOnlineDto> GetAutoJoinCustomer(int mpId)
        {
            Logger.Info($"自动分配判定，条件：MPID为{mpId}");
            var state = (int)CustomerServiceConversationState.Asking;
            var kfType = KFType.YL.ToString();
            var onlineState = (int)OnlineState.OnLine;
            var connectState = (int)ConnectState.Connect;
            var conversationDicnary = _conversationReposity.GetAll().Where(m => m.State == state && m.MpID == mpId).GroupBy(m => m.CustomerId).Select(m => new { m.Key, Count = m.Count() }).ToList();
            Logger.Info($"自动分配判定，统计会话DIC：{JsonConvert.SerializeObject(conversationDicnary)}");
            var cusList = ObjectMapper.Map<List<AutoJoinCustomerDto>>(await Repository.GetAllListAsync(m => m.MpID == mpId && m.IsDeleted == false && m.AutoJoin == true && m.KfType == kfType && m.OnlineState == onlineState && m.ConnectState == connectState));
            Logger.Info($"自动分配判定，获取在线客服列表：{JsonConvert.SerializeObject(cusList)}");
            foreach (var item in cusList)
            {

                item.ConversationCount = conversationDicnary.Where(m => m.Key == item.Id).FirstOrDefault() == null ? 0 : conversationDicnary.Where(m => m.Key == item.Id).FirstOrDefault().Count;
                if (item.AutoJoinCount != 0)
                    item.JoinPercent = Math.Round((Convert.ToDecimal(item.AutoJoinCount) - Convert.ToDecimal(item.ConversationCount)) / Convert.ToDecimal(item.AutoJoinCount), 4);
                else
                    item.JoinPercent = 0;
            }
            Logger.Info($"自动分配判定，重置在线客服列表：{JsonConvert.SerializeObject(cusList)}");
            var result = cusList.Where(m => m.JoinPercent > 0).OrderByDescending(m => m.JoinPercent).FirstOrDefault();
            if (result != null)
            {
                var resultModel = new CustomerServiceOnlineDto
                {
                    Id = result.Id,
                    AutoJoin = result.AutoJoin,
                    AutoJoinCount = result.AutoJoinCount,
                    ConnectId = result.ConnectId,
                    ConnectState = result.ConnectState,
                    KfAccount = result.KfAccount,
                    KfHeadingUrl = result.KfHeadingUrl,
                    KfNick = result.KfNick,
                    OnlineState = result.OnlineState,
                    KfPassWord = result.KfPassWord,
                    KfType = result.KfType,
                    KfWx = result.KfWx,
                    LastModificationTime = result.LastModificationTime,
                    LocalHeadFilePath = result.LocalHeadFilePath,
                    LocalHeadingUrl = result.LocalHeadingUrl,
                    MessageToken = result.MessageToken,
                    MpID = result.MpID,
                    OpenID = result.OpenID,
                    PreKfAccount = result.PreKfAccount,
                    PublicNumberAccount = result.PublicNumberAccount
                };
                return resultModel;
            }
            else
                return null;
        }
        /// <summary>
        /// 获取上次接待客服客服信息
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<CustomerServiceOnlineDto> GetLastCustomerService(int customerId)
        {
            var askingStatus = (int)CustomerServiceConversationState.Asking;
            var askingCount = await _conversationReposity.CountAsync(m => m.CustomerId == customerId && m.State == askingStatus);
            var result = await Repository.FirstOrDefaultAsync(m => m.IsDeleted == false && m.OnlineState == 1 && m.ConnectState == 1 && m.AutoJoinCount > askingCount);
            if (result != null)
                return ObjectMapper.Map<CustomerServiceOnlineDto>(result);
            else
                return null;
        }
        public async Task<CustomerServiceOnline> GetByMessageToken(string messageToken)
        {
            return await Repository.FirstOrDefaultAsync(m => m.IsDeleted == false && m.MessageToken == messageToken);
        }
        //public async Task SyncCustomerList(int mpId)
        //{
        //    var result = await _wxMediaAppService.GetCurstomerList(mpId);
        //    var kfList = result.kf_list;
        //    var kfAccounts=kfList.Select(m => m.kf_account).ToList();
        //    var datas = Repository.GetAll().Where(m => kfAccounts.Contains(m.KfAccount)).ToList();
        //    foreach(var data in datas)
        //    {
        //        var kf = kfList.Where(m => m.kf_account == data.KfAccount).FirstOrDefault();
        //    }
        //}
    }
}
