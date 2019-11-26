//------------------------------------------------------------------------------
// <自动生成>
//     此代码由工具生成。
//     //
//     对此文件的更改可能导致不正确的行为，并在以下条件下丢失:
//     代码重新生成。
// </自动生成>
//------------------------------------------------------------------------------

namespace yilibabyMember
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://mmp.meichis.com/DataInterface/", ConfigurationName="yilibabyMember.MemberServiceSoap")]
    public interface MemberServiceSoap
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/ExportProductInfoXML", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<System.Xml.Linq.XElement> ExportProductInfoXMLAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/HelloWorld", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> HelloWorldAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetServerSyncTime", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<System.DateTime> GetServerSyncTimeAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/MemberCartGetList", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<yilibabyMember.MemberCart[]> MemberCartGetListAsync(string AuthKey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/MemberCartGetListJson", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> MemberCartGetListJsonAsync(string AuthKey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/MemberCartAddJson", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> MemberCartAddJsonAsync(string AuthKey, string Cart);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/MemberCartRemoveJson", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> MemberCartRemoveJsonAsync(string AuthKey, string Cart);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/MemberCartUpdateJson", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> MemberCartUpdateJsonAsync(string AuthKey, string Cart);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetMyMemberInfo", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<yilibabyMember.Member> GetMyMemberInfoAsync(string AuthKey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetMyMemberInfoJson", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetMyMemberInfoJsonAsync(string AuthKey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetMemberByMobile", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<yilibabyMember.Member> GetMemberByMobileAsync(string Mobile);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetMemberByMobileJson", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetMemberByMobileJsonAsync(string AuthKey, string Mobile);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/UpdateMemberInfoJson", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> UpdateMemberInfoJsonAsync(string AuthKey, string Member);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/SendUserRegisterMail", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> SendUserRegisterMailAsync(string AuthKey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/MemberBindWeChat", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> MemberBindWeChatAsync(string AuthKey, string UserWeChat, string CompanyWeChat, string Remark);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetPointsBalance", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<decimal> GetPointsBalanceAsync(string AuthKey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetPointsInfo", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<yilibabyMember.MemberPointsInfo> GetPointsInfoAsync(string AuthKey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetPointsInfoJson", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetPointsInfoJsonAsync(string AuthKey);
        
        // CODEGEN: 正在生成消息协定，因为操作具有多个返回值。
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/AddProductPoints", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<yilibabyMember.AddProductPointsResponse> AddProductPointsAsync(yilibabyMember.AddProductPointsRequest request);
        
        // CODEGEN: 正在生成消息协定，因为操作具有多个返回值。
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/AddProductPointsEx", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<yilibabyMember.AddProductPointsExResponse> AddProductPointsExAsync(yilibabyMember.AddProductPointsExRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/AddMemberPoints", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> AddMemberPointsAsync(string AuthKey, int Source, int Points, string Remark, string Key);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/AddMemberPointsEx", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> AddMemberPointsExAsync(string AuthKey, int Source, int Points, string Remark, string Key);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/AddMemberPointsEx2", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> AddMemberPointsEx2Async(string AuthKey, int CRMID, int Source, int Points, string Remark, string Key);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetPointsChangeDetailJson", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetPointsChangeDetailJsonAsync(string AuthKey, System.DateTime BeginDate, System.DateTime EndDate);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/VerifyProductCode", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> VerifyProductCodeAsync(string AuthKey, string ProductCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetDeliveryAddressJson", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetDeliveryAddressJsonAsync(string AuthKey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/DeliveryAddressAddJson", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> DeliveryAddressAddJsonAsync(string AuthKey, string Addr);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/DeliveryAddressUpdateJson", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> DeliveryAddressUpdateJsonAsync(string AuthKey, string Addr);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/DeliveryAddressSetDefault", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> DeliveryAddressSetDefaultAsync(string AuthKey, int AddrID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/DeliveryAddressDel", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> DeliveryAddressDelAsync(string AuthKey, int AddrID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetMyExchangeOrderList", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<yilibabyMember.ExchangeOrder[]> GetMyExchangeOrderListAsync(string AuthKey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetMyExchangeOrderListJson", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetMyExchangeOrderListJsonAsync(string AuthKey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetMyExchangeOrderJson", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetMyExchangeOrderJsonAsync(string AuthKey, int OrderID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/ChangeExchangeOrderAddressJson", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> ChangeExchangeOrderAddressJsonAsync(string AuthKey, int OrderID, string NewAddress);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/SignInExchangeOrder", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> SignInExchangeOrderAsync(string AuthKey, int OrderID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/CancelExchangeOrder", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> CancelExchangeOrderAsync(string AuthKey, int OrderID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/CancelExchangeOrderEx", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> CancelExchangeOrderExAsync(string AuthKey, int OrderID, string CancelRemark);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/ReturnExchangeOrder", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> ReturnExchangeOrderAsync(string AuthKey, int OrderID, string CancelReason);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetGiftCatalogs", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<yilibabyMember.GiftCatalog[]> GetGiftCatalogsAsync(string AuthKey, int SuperID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetGiftCatalogsJson", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetGiftCatalogsJsonAsync(string AuthKey, int SuperID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetGiftsByCatalog", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<yilibabyMember.Gift[]> GetGiftsByCatalogAsync(string AuthKey, int Catalog);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetGiftsByCatalogJson", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetGiftsByCatalogJsonAsync(string AuthKey, int Catalog);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetGiftInventory", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> GetGiftInventoryAsync(string AuthKey, int GiftID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetHotGifts", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<yilibabyMember.Gift[]> GetHotGiftsAsync(string AuthKey, int TopCount, int MaxPoints, System.DateTime BeginDate);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetHotGiftsJson", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetHotGiftsJsonAsync(string AuthKey, int TopCount, int MaxPoints, System.DateTime BeginDate);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetGiftInfo", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<yilibabyMember.Gift> GetGiftInfoAsync(string AuthKey, string GiftCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetGiftInfoJson", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetGiftInfoJsonAsync(string AuthKey, string GiftCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/CustomerExchangeOrderApplyEx3", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> CustomerExchangeOrderApplyEx3Async(string AuthKey, string CityName, string Consignee, string Address, string Mobile, string AcceptRemark, string ProductCodes, string Quantitys, int Source);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/CustomerExchangeOrderApplyEx", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> CustomerExchangeOrderApplyExAsync(string AuthKey, int OfficialCity, string Consignee, string Address, string Mobile, string AcceptRemark, string ProductIDs, string Quantitys, int Source);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/CustomerExchangeOrderApplyEx2", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> CustomerExchangeOrderApplyEx2Async(string AuthKey, int OfficialCity, string Consignee, string Address, string Mobile, string AcceptRemark, string ProductIDs, string Quantitys, int Source);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/CustomerExchangeOrderApply2", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> CustomerExchangeOrderApply2Async(string AuthKey, int OfficialCity, string Consignee, string Address, string Mobile, string AcceptRemark, string ProductIDs, string Quantitys, int Source);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/ChangeExchangeOrderAddress", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> ChangeExchangeOrderAddressAsync(string AuthKey, int OrderID, int OfficialCity, string Consignee, string Address, string Mobile, string AcceptRemark);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/IsJinLingGuanNewCustomer", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> IsJinLingGuanNewCustomerAsync(string AuthKey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/IsHaveRecommendQualifications", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> IsHaveRecommendQualificationsAsync(string AuthKey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/RecommendInfoAdd", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> RecommendInfoAddAsync(string AuthKey, string Name, string Mobile, string Email);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetClientInfoForNewClientActivity", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetClientInfoForNewClientActivityAsync(string AuthKey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetClientDSInfoForNewClientActivity", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetClientDSInfoForNewClientActivityAsync(string AuthKey, int CRMID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetClientInfoForNewClientActivityByCRMID", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetClientInfoForNewClientActivityByCRMIDAsync(string AuthKey, int CRMID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetClientInfoForNewClientActivityByMobile", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetClientInfoForNewClientActivityByMobileAsync(string AuthKey, string Mobile);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetMostlyProductList", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<yilibabyMember.DicDataItem[]> GetMostlyProductListAsync(string AuthKey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetMostlyProductListJson", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetMostlyProductListJsonAsync(string AuthKey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetBrandList", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetBrandListAsync(string authKey, int type);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetBrandListJson", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetBrandListJsonAsync(string AuthKey, int type);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetClientByGeo", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<yilibabyMember.ClientInfo[]> GetClientByGeoAsync(string AuthKey, string lat, string lng);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetRetailerListByOfficialCity", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<yilibabyMember.ClientInfo[]> GetRetailerListByOfficialCityAsync(string AuthKey, int OfficialCity);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetRetailerListByOfficialCityJson", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetRetailerListByOfficialCityJsonAsync(string AuthKey, int OfficialCity);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetNearRetailerList", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<yilibabyMember.ClientInfo[]> GetNearRetailerListAsync(string AuthKey, int OfficialCity, float Latitude, float Longitude, int Distance);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetNearRetailerListJson", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetNearRetailerListJsonAsync(string AuthKey, int OfficialCity, float Latitude, float Longitude, int Distance);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/CustomerServiceAccept", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> CustomerServiceAcceptAsync(string AuthKey, string Topic, string Content, string ServiceStaff, int AcceptSource);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/CustomerRecommandAccept", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> CustomerRecommandAcceptAsync(string AuthKey, string RecommandName, string RecommandMobile, string RecommandEmail);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetCustomerRecommandList", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<yilibabyMember.RecommendCustomerInfo[]> GetCustomerRecommandListAsync(string AuthKey, string BeginTime, string EndTime);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetCustomerRecommandListJson", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetCustomerRecommandListJsonAsync(string AuthKey, string BeginTime, string EndTime);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetCustomerRecommandPoint", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> GetCustomerRecommandPointAsync(string AuthKey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/FindWeChatMemberByAdvCondition", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string[]> FindWeChatMemberByAdvConditionAsync(string AuthKey, string MemberAdvConditionJson);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetCountByClientBrithday", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> GetCountByClientBrithdayAsync(string AuthKey, string BeginTime, string EndTime);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/JudgeGuardTrialPackApplyConditon", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> JudgeGuardTrialPackApplyConditonAsync(string AuthKey, string StartTime);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/MicroBlog_GetAuthorizeAccess", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> MicroBlog_GetAuthorizeAccessAsync(string client_id, string callbackUrl, string scope, string state, string display, string forcelogin, string language);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/MicroBlog_AccessToken", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> MicroBlog_AccessTokenAsync(string client_id, string client_secret, string grant_type, string code, string callbackUrl);
        
        // CODEGEN: 正在生成消息协定，因为操作具有多个返回值。
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/MicroBlog_AuthLoginEx", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<yilibabyMember.MicroBlog_AuthLoginExResponse> MicroBlog_AuthLoginExAsync(yilibabyMember.MicroBlog_AuthLoginExRequest request);
        
        // CODEGEN: 正在生成消息协定，因为操作具有多个返回值。
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/MicroBlog_AuthLogin", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<yilibabyMember.MicroBlog_AuthLoginResponse> MicroBlog_AuthLoginAsync(yilibabyMember.MicroBlog_AuthLoginRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/MicroBlog_GetUser", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> MicroBlog_GetUserAsync(string access_token, string uid, string source, string screen_name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/MicroBlog_GetUserEmail", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> MicroBlog_GetUserEmailAsync(string access_token, string souece);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/MicroBlog_GetNameByAddressCode", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> MicroBlog_GetNameByAddressCodeAsync(string access_token, string codes, string source);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/MicroBlog_GetTokenInfo", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> MicroBlog_GetTokenInfoAsync(string access_token);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/MemberGetCardJson", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> MemberGetCardJsonAsync(string AuthKey, string CardID, string CardCode, int Source, string Remark);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetFansSourceData", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<yilibabyMember.ArrayOfXElement> GetFansSourceDataAsync(System.DateTime startTime, System.DateTime endTime);
        
        // CODEGEN: 正在生成消息协定，因为操作具有多个返回值。
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/WechatSendTemplateMsg", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<yilibabyMember.WechatSendTemplateMsgResponse> WechatSendTemplateMsgAsync(yilibabyMember.WechatSendTemplateMsgRequest request);
        
        // CODEGEN: 正在生成消息协定，因为操作具有多个返回值。
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/CY_GetCard", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<yilibabyMember.CY_GetCardResponse> CY_GetCardAsync(yilibabyMember.CY_GetCardRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/StampActivity_MemberJoinJudge", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> StampActivity_MemberJoinJudgeAsync(string AuthKey, int Member);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/StampActivity_CanGetGiftList", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<yilibabyMember.StampGift[]> StampActivity_CanGetGiftListAsync(string AuthKey, int Member);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/StampActivity_CanGetGiftListJson", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> StampActivity_CanGetGiftListJsonAsync(string AuthKey, int Member);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/StampActivityMemberGetGiftViaExpress", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> StampActivityMemberGetGiftViaExpressAsync(string AuthKey, int Activity, int crmid, string GetGiftCode, int OfficialCity, string Address, string Consignee, string Mobile, int SourceInfo, string Remark);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetGift", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> GetGiftAsync(string Member, int InfoSource);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/getNotReceiveGift", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> getNotReceiveGiftAsync(string MemberID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/PointsClearOrder", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> PointsClearOrderAsync(string Member, int OfficialCity1, string Address, string Consignee, string Mobile, int SourceInfo, int Award, string Remark);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetMemberType_MBabyH5", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> GetMemberType_MBabyH5Async(string authkey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetLastBuyProduct_Activity", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetLastBuyProduct_ActivityAsync(string authkey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/CustomerJoinActivitySign", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> CustomerJoinActivitySignAsync(string AuthKey, int ClientID, int ActivityID, int InfoSource);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/UpdateMemberInfoCollectActivity", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> UpdateMemberInfoCollectActivityAsync(string AuthKey, int InfoCollectActivity);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetMemberInfoServiceCMClientCode", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetMemberInfoServiceCMClientCodeAsync(string AuthKey, string Mobile);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/UpdateMemberInfoServiceCMClientCode", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> UpdateMemberInfoServiceCMClientCodeAsync(string AuthKey, string Mobile, string ServiceCMClientCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetUserInfoByMobileJson", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetUserInfoByMobileJsonAsync(string AuthKey, string Mobile);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/IsHaveRecommendQualifications_xh", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> IsHaveRecommendQualifications_xhAsync(string AuthKey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/RecommendInfoAdd_xh", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> RecommendInfoAdd_xhAsync(string AuthKey, string Name, string Mobile, string Email);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/StampActivity_MemberJoinJudge2", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> StampActivity_MemberJoinJudge2Async(string authkey, int crmid, int activity);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/StampActivity_GetParams", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> StampActivity_GetParamsAsync(string authkey, int crmid, int activity);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://mmp.meichis.com/DataInterface/")]
    public partial class MemberCart
    {
        
        private int idField;
        
        private int giftIDField;
        
        private int quantityField;
        
        private string giftNameField;
        
        private string giftImageUrlField;
        
        private decimal actPriceField;
        
        private decimal actPointsField;
        
        private int sourceField;
        
        private string insertTimeField;
        
        private string lastBroserTimeField;
        
        private string lastUpdateTimeField;
        
        private int statusField;
        
        private string buyTimeField;
        
        private int buyQuantityField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public int GiftID
        {
            get
            {
                return this.giftIDField;
            }
            set
            {
                this.giftIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public int Quantity
        {
            get
            {
                return this.quantityField;
            }
            set
            {
                this.quantityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string GiftName
        {
            get
            {
                return this.giftNameField;
            }
            set
            {
                this.giftNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string GiftImageUrl
        {
            get
            {
                return this.giftImageUrlField;
            }
            set
            {
                this.giftImageUrlField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public decimal ActPrice
        {
            get
            {
                return this.actPriceField;
            }
            set
            {
                this.actPriceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public decimal ActPoints
        {
            get
            {
                return this.actPointsField;
            }
            set
            {
                this.actPointsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=7)]
        public int Source
        {
            get
            {
                return this.sourceField;
            }
            set
            {
                this.sourceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=8)]
        public string InsertTime
        {
            get
            {
                return this.insertTimeField;
            }
            set
            {
                this.insertTimeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=9)]
        public string LastBroserTime
        {
            get
            {
                return this.lastBroserTimeField;
            }
            set
            {
                this.lastBroserTimeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=10)]
        public string LastUpdateTime
        {
            get
            {
                return this.lastUpdateTimeField;
            }
            set
            {
                this.lastUpdateTimeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=11)]
        public int Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=12)]
        public string BuyTime
        {
            get
            {
                return this.buyTimeField;
            }
            set
            {
                this.buyTimeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=13)]
        public int BuyQuantity
        {
            get
            {
                return this.buyQuantityField;
            }
            set
            {
                this.buyQuantityField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://mmp.meichis.com/DataInterface/")]
    public partial class StampGift
    {
        
        private int idField;
        
        private string codeField;
        
        private string nameField;
        
        private int catalogField;
        
        private string descriptionField;
        
        private string contentField;
        
        private string barCodeField;
        
        private string specField;
        
        private decimal stdPriceField;
        
        private decimal stdPointsField;
        
        private decimal actPriceField;
        
        private decimal actPointsField;
        
        private System.DateTime syncTimeField;
        
        private string enabledFlagField;
        
        private System.DateTime insertTimeField;
        
        private int recommendField;
        
        private int buyCountsField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public int Catalog
        {
            get
            {
                return this.catalogField;
            }
            set
            {
                this.catalogField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public string Content
        {
            get
            {
                return this.contentField;
            }
            set
            {
                this.contentField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public string BarCode
        {
            get
            {
                return this.barCodeField;
            }
            set
            {
                this.barCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=7)]
        public string Spec
        {
            get
            {
                return this.specField;
            }
            set
            {
                this.specField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=8)]
        public decimal StdPrice
        {
            get
            {
                return this.stdPriceField;
            }
            set
            {
                this.stdPriceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=9)]
        public decimal StdPoints
        {
            get
            {
                return this.stdPointsField;
            }
            set
            {
                this.stdPointsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=10)]
        public decimal ActPrice
        {
            get
            {
                return this.actPriceField;
            }
            set
            {
                this.actPriceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=11)]
        public decimal ActPoints
        {
            get
            {
                return this.actPointsField;
            }
            set
            {
                this.actPointsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=12)]
        public System.DateTime SyncTime
        {
            get
            {
                return this.syncTimeField;
            }
            set
            {
                this.syncTimeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=13)]
        public string EnabledFlag
        {
            get
            {
                return this.enabledFlagField;
            }
            set
            {
                this.enabledFlagField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=14)]
        public System.DateTime InsertTime
        {
            get
            {
                return this.insertTimeField;
            }
            set
            {
                this.insertTimeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=15)]
        public int Recommend
        {
            get
            {
                return this.recommendField;
            }
            set
            {
                this.recommendField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=16)]
        public int BuyCounts
        {
            get
            {
                return this.buyCountsField;
            }
            set
            {
                this.buyCountsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://mmp.meichis.com/DataInterface/")]
    public partial class RecommendCustomerInfo
    {
        
        private int clientIDField;
        
        private string recommendNameField;
        
        private string recommendMobileField;
        
        private string recommendEmailField;
        
        private System.DateTime recommendTimeField;
        
        private int recommendStateField;
        
        private string recommendStateNameField;
        
        private System.DateTime recommendSuccessTimeField;
        
        private int rewardScoresField;
        
        private string remarkField;
        
        private int statusField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public int ClientID
        {
            get
            {
                return this.clientIDField;
            }
            set
            {
                this.clientIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string RecommendName
        {
            get
            {
                return this.recommendNameField;
            }
            set
            {
                this.recommendNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string RecommendMobile
        {
            get
            {
                return this.recommendMobileField;
            }
            set
            {
                this.recommendMobileField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string RecommendEmail
        {
            get
            {
                return this.recommendEmailField;
            }
            set
            {
                this.recommendEmailField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public System.DateTime RecommendTime
        {
            get
            {
                return this.recommendTimeField;
            }
            set
            {
                this.recommendTimeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public int RecommendState
        {
            get
            {
                return this.recommendStateField;
            }
            set
            {
                this.recommendStateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public string RecommendStateName
        {
            get
            {
                return this.recommendStateNameField;
            }
            set
            {
                this.recommendStateNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=7)]
        public System.DateTime RecommendSuccessTime
        {
            get
            {
                return this.recommendSuccessTimeField;
            }
            set
            {
                this.recommendSuccessTimeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=8)]
        public int RewardScores
        {
            get
            {
                return this.rewardScoresField;
            }
            set
            {
                this.rewardScoresField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=9)]
        public string Remark
        {
            get
            {
                return this.remarkField;
            }
            set
            {
                this.remarkField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=10)]
        public int Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://mmp.meichis.com/DataInterface/")]
    public partial class ClientInfo
    {
        
        private int idField;
        
        private string clientCodeField;
        
        private string fullNameField;
        
        private string shortNameField;
        
        private int officialCityField;
        
        private int organizeCityField;
        
        private string teleNumField;
        
        private string addressField;
        
        private string postCodeField;
        
        private int activeFlagField;
        
        private int supplierField;
        
        private int clientTypeField;
        
        private float latitudeField;
        
        private float longitudeField;
        
        private int distanceField;
        
        private Attachment[] attsField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string ClientCode
        {
            get
            {
                return this.clientCodeField;
            }
            set
            {
                this.clientCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string FullName
        {
            get
            {
                return this.fullNameField;
            }
            set
            {
                this.fullNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string ShortName
        {
            get
            {
                return this.shortNameField;
            }
            set
            {
                this.shortNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public int OfficialCity
        {
            get
            {
                return this.officialCityField;
            }
            set
            {
                this.officialCityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public int OrganizeCity
        {
            get
            {
                return this.organizeCityField;
            }
            set
            {
                this.organizeCityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public string TeleNum
        {
            get
            {
                return this.teleNumField;
            }
            set
            {
                this.teleNumField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=7)]
        public string Address
        {
            get
            {
                return this.addressField;
            }
            set
            {
                this.addressField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=8)]
        public string PostCode
        {
            get
            {
                return this.postCodeField;
            }
            set
            {
                this.postCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=9)]
        public int ActiveFlag
        {
            get
            {
                return this.activeFlagField;
            }
            set
            {
                this.activeFlagField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=10)]
        public int Supplier
        {
            get
            {
                return this.supplierField;
            }
            set
            {
                this.supplierField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=11)]
        public int ClientType
        {
            get
            {
                return this.clientTypeField;
            }
            set
            {
                this.clientTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=12)]
        public float Latitude
        {
            get
            {
                return this.latitudeField;
            }
            set
            {
                this.latitudeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=13)]
        public float Longitude
        {
            get
            {
                return this.longitudeField;
            }
            set
            {
                this.longitudeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=14)]
        public int Distance
        {
            get
            {
                return this.distanceField;
            }
            set
            {
                this.distanceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order=15)]
        public Attachment[] Atts
        {
            get
            {
                return this.attsField;
            }
            set
            {
                this.attsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://mmp.meichis.com/DataInterface/")]
    public partial class Attachment
    {
        
        private string attNameField;
        
        private string extNameField;
        
        private System.Guid gUIDField;
        
        private System.DateTime uploadTimeField;
        
        private int fileSizeField;
        
        private bool isFirstPictureField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string AttName
        {
            get
            {
                return this.attNameField;
            }
            set
            {
                this.attNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string ExtName
        {
            get
            {
                return this.extNameField;
            }
            set
            {
                this.extNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public System.Guid GUID
        {
            get
            {
                return this.gUIDField;
            }
            set
            {
                this.gUIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public System.DateTime UploadTime
        {
            get
            {
                return this.uploadTimeField;
            }
            set
            {
                this.uploadTimeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public int FileSize
        {
            get
            {
                return this.fileSizeField;
            }
            set
            {
                this.fileSizeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public bool IsFirstPicture
        {
            get
            {
                return this.isFirstPictureField;
            }
            set
            {
                this.isFirstPictureField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://mmp.meichis.com/DataInterface/")]
    public partial class DicDataItem
    {
        
        private int idField;
        
        private string nameField;
        
        private string valueField;
        
        private string remarkField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string Remark
        {
            get
            {
                return this.remarkField;
            }
            set
            {
                this.remarkField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://mmp.meichis.com/DataInterface/")]
    public partial class Gift
    {
        
        private int idField;
        
        private string codeField;
        
        private string nameField;
        
        private int catalogField;
        
        private string descriptionField;
        
        private string contentField;
        
        private string barCodeField;
        
        private string specField;
        
        private decimal stdPriceField;
        
        private decimal stdPointsField;
        
        private decimal actPriceField;
        
        private decimal actPointsField;
        
        private System.DateTime insertTimeField;
        
        private bool recommendField;
        
        private int buyCountsField;
        
        private string brandField;
        
        private string deliveryAreaField;
        
        private System.Guid imageGUIDField;
        
        private Attachment[] attsField;
        
        private int suitAgeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public int Catalog
        {
            get
            {
                return this.catalogField;
            }
            set
            {
                this.catalogField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public string Content
        {
            get
            {
                return this.contentField;
            }
            set
            {
                this.contentField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public string BarCode
        {
            get
            {
                return this.barCodeField;
            }
            set
            {
                this.barCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=7)]
        public string Spec
        {
            get
            {
                return this.specField;
            }
            set
            {
                this.specField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=8)]
        public decimal StdPrice
        {
            get
            {
                return this.stdPriceField;
            }
            set
            {
                this.stdPriceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=9)]
        public decimal StdPoints
        {
            get
            {
                return this.stdPointsField;
            }
            set
            {
                this.stdPointsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=10)]
        public decimal ActPrice
        {
            get
            {
                return this.actPriceField;
            }
            set
            {
                this.actPriceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=11)]
        public decimal ActPoints
        {
            get
            {
                return this.actPointsField;
            }
            set
            {
                this.actPointsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=12)]
        public System.DateTime InsertTime
        {
            get
            {
                return this.insertTimeField;
            }
            set
            {
                this.insertTimeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=13)]
        public bool Recommend
        {
            get
            {
                return this.recommendField;
            }
            set
            {
                this.recommendField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=14)]
        public int BuyCounts
        {
            get
            {
                return this.buyCountsField;
            }
            set
            {
                this.buyCountsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=15)]
        public string Brand
        {
            get
            {
                return this.brandField;
            }
            set
            {
                this.brandField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=16)]
        public string DeliveryArea
        {
            get
            {
                return this.deliveryAreaField;
            }
            set
            {
                this.deliveryAreaField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=17)]
        public System.Guid ImageGUID
        {
            get
            {
                return this.imageGUIDField;
            }
            set
            {
                this.imageGUIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order=18)]
        public Attachment[] Atts
        {
            get
            {
                return this.attsField;
            }
            set
            {
                this.attsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=19)]
        public int SuitAge
        {
            get
            {
                return this.suitAgeField;
            }
            set
            {
                this.suitAgeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://mmp.meichis.com/DataInterface/")]
    public partial class GiftCatalog
    {
        
        private int idField;
        
        private string nameField;
        
        private string descriptionField;
        
        private System.Guid imageGUIDField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public System.Guid ImageGUID
        {
            get
            {
                return this.imageGUIDField;
            }
            set
            {
                this.imageGUIDField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://mmp.meichis.com/DataInterface/")]
    public partial class ExchangeProcessHistory
    {
        
        private System.DateTime processTimeField;
        
        private string descriptionField;
        
        private string processStaffField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public System.DateTime ProcessTime
        {
            get
            {
                return this.processTimeField;
            }
            set
            {
                this.processTimeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string ProcessStaff
        {
            get
            {
                return this.processStaffField;
            }
            set
            {
                this.processStaffField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://mmp.meichis.com/DataInterface/")]
    public partial class ExchangeOrderDetail
    {
        
        private int productIDField;
        
        private string productNameField;
        
        private int quantityField;
        
        private decimal pointsField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public int ProductID
        {
            get
            {
                return this.productIDField;
            }
            set
            {
                this.productIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string ProductName
        {
            get
            {
                return this.productNameField;
            }
            set
            {
                this.productNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public int Quantity
        {
            get
            {
                return this.quantityField;
            }
            set
            {
                this.quantityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public decimal Points
        {
            get
            {
                return this.pointsField;
            }
            set
            {
                this.pointsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://mmp.meichis.com/DataInterface/")]
    public partial class ExchangeOrder
    {
        
        private int orderIDField;
        
        private int officialCityField;
        
        private string officialCityNameField;
        
        private string addressField;
        
        private string consigneeField;
        
        private string mobileField;
        
        private string teleNumberField;
        
        private string acceptRemarkField;
        
        private int stateField;
        
        private string stateNameField;
        
        private string deliveryCompanyField;
        
        private string deliverySheetCodeField;
        
        private System.DateTime acceptDateField;
        
        private System.DateTime preArrivalDateField;
        
        private System.DateTime beginDeliveryDateField;
        
        private System.DateTime signInDateField;
        
        private string signInManField;
        
        private string remarkField;
        
        private ExchangeOrderDetail[] itemsField;
        
        private ExchangeProcessHistory[] processHistoryField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public int OrderID
        {
            get
            {
                return this.orderIDField;
            }
            set
            {
                this.orderIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public int OfficialCity
        {
            get
            {
                return this.officialCityField;
            }
            set
            {
                this.officialCityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string OfficialCityName
        {
            get
            {
                return this.officialCityNameField;
            }
            set
            {
                this.officialCityNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string Address
        {
            get
            {
                return this.addressField;
            }
            set
            {
                this.addressField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string Consignee
        {
            get
            {
                return this.consigneeField;
            }
            set
            {
                this.consigneeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public string Mobile
        {
            get
            {
                return this.mobileField;
            }
            set
            {
                this.mobileField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public string TeleNumber
        {
            get
            {
                return this.teleNumberField;
            }
            set
            {
                this.teleNumberField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=7)]
        public string AcceptRemark
        {
            get
            {
                return this.acceptRemarkField;
            }
            set
            {
                this.acceptRemarkField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=8)]
        public int State
        {
            get
            {
                return this.stateField;
            }
            set
            {
                this.stateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=9)]
        public string StateName
        {
            get
            {
                return this.stateNameField;
            }
            set
            {
                this.stateNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=10)]
        public string DeliveryCompany
        {
            get
            {
                return this.deliveryCompanyField;
            }
            set
            {
                this.deliveryCompanyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=11)]
        public string DeliverySheetCode
        {
            get
            {
                return this.deliverySheetCodeField;
            }
            set
            {
                this.deliverySheetCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=12)]
        public System.DateTime AcceptDate
        {
            get
            {
                return this.acceptDateField;
            }
            set
            {
                this.acceptDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=13)]
        public System.DateTime PreArrivalDate
        {
            get
            {
                return this.preArrivalDateField;
            }
            set
            {
                this.preArrivalDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=14)]
        public System.DateTime BeginDeliveryDate
        {
            get
            {
                return this.beginDeliveryDateField;
            }
            set
            {
                this.beginDeliveryDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=15)]
        public System.DateTime SignInDate
        {
            get
            {
                return this.signInDateField;
            }
            set
            {
                this.signInDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=16)]
        public string SignInMan
        {
            get
            {
                return this.signInManField;
            }
            set
            {
                this.signInManField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=17)]
        public string Remark
        {
            get
            {
                return this.remarkField;
            }
            set
            {
                this.remarkField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order=18)]
        public ExchangeOrderDetail[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order=19)]
        public ExchangeProcessHistory[] ProcessHistory
        {
            get
            {
                return this.processHistoryField;
            }
            set
            {
                this.processHistoryField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://mmp.meichis.com/DataInterface/")]
    public partial class MemberPointsInfo
    {
        
        private decimal totalAddUpPointsField;
        
        private decimal totalExchangePointsField;
        
        private decimal balancePointsField;
        
        private decimal willExpirePointsField;
        
        private System.DateTime willExpireDateField;
        
        private decimal thisMonthPointsField;
        
        private System.DateTime lastAddUpDateField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public decimal TotalAddUpPoints
        {
            get
            {
                return this.totalAddUpPointsField;
            }
            set
            {
                this.totalAddUpPointsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public decimal TotalExchangePoints
        {
            get
            {
                return this.totalExchangePointsField;
            }
            set
            {
                this.totalExchangePointsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public decimal BalancePoints
        {
            get
            {
                return this.balancePointsField;
            }
            set
            {
                this.balancePointsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public decimal WillExpirePoints
        {
            get
            {
                return this.willExpirePointsField;
            }
            set
            {
                this.willExpirePointsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public System.DateTime WillExpireDate
        {
            get
            {
                return this.willExpireDateField;
            }
            set
            {
                this.willExpireDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public decimal ThisMonthPoints
        {
            get
            {
                return this.thisMonthPointsField;
            }
            set
            {
                this.thisMonthPointsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public System.DateTime LastAddUpDate
        {
            get
            {
                return this.lastAddUpDateField;
            }
            set
            {
                this.lastAddUpDateField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://mmp.meichis.com/DataInterface/")]
    public partial class Member
    {
        
        private System.Guid idField;
        
        private string realNameField;
        
        private int memberTypeField;
        
        private string memberTypeNameField;
        
        private string mobileField;
        
        private string emailField;
        
        private bool emailVerifyFlagField;
        
        private int sexField;
        
        private System.DateTime birthdayField;
        
        private int homeRoleField;
        
        private string homeRoleNameField;
        
        private string babyNameField;
        
        private string hobbyField;
        
        private int officialCityField;
        
        private string officialCityNameField;
        
        private string addressField;
        
        private System.DateTime activeDateField;
        
        private int registerSourceField;
        
        private string registerSourceNameField;
        
        private string collectProductField;
        
        private string collectProductNameField;
        
        private string firstBuyProductNameField;
        
        private int cRMIDField;
        
        private string maMaGroupField;
        
        private string serviceCMClientCodeField;
        
        private string serviceCMClientNameField;
        
        private string servicePromotorCodeField;
        
        private string servicePromotorNameField;
        
        private string servicePromotorMobileField;
        
        private int preBrandField;
        
        private string preBrandOtherField;
        
        private int infoCollectActivityField;
        
        private int activityInfoSourceField;
        
        private Attachment[] attsField;
        
        private string xHFlagField;
        
        private int xHUseField;
        
        private string xHUseNameField;
        
        private System.DateTime xHBirthdayField;
        
        private int xHPreBrandField;
        
        private string xHPreBrandNameField;
        
        private string xHRegProductField;
        
        private string xHRegProductNameField;
        
        private string xHProductField;
        
        private string xHProductNameField;
        
        private int xHMemberTypeField;
        
        private int xHHealthConcernsField;
        
        private string xHHealthConcernsNameField;
        
        private int xHRoleField;
        
        private int xHSexField;
        
        private string xHUserNameField;
        
        private int xHUserSexField;
        
        private string xHUserMobileField;
        
        private System.DateTime xHUserBrithdayField;
        
        private decimal balancePointsField;
        
        private decimal eatQuantityOneMonthField;
        
        private int eatTimesOneDayField;
        
        private int feedModeField;
        
        private int servicePreferencesField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public System.Guid ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string RealName
        {
            get
            {
                return this.realNameField;
            }
            set
            {
                this.realNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public int MemberType
        {
            get
            {
                return this.memberTypeField;
            }
            set
            {
                this.memberTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string MemberTypeName
        {
            get
            {
                return this.memberTypeNameField;
            }
            set
            {
                this.memberTypeNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string Mobile
        {
            get
            {
                return this.mobileField;
            }
            set
            {
                this.mobileField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public string Email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public bool EmailVerifyFlag
        {
            get
            {
                return this.emailVerifyFlagField;
            }
            set
            {
                this.emailVerifyFlagField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=7)]
        public int Sex
        {
            get
            {
                return this.sexField;
            }
            set
            {
                this.sexField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=8)]
        public System.DateTime Birthday
        {
            get
            {
                return this.birthdayField;
            }
            set
            {
                this.birthdayField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=9)]
        public int HomeRole
        {
            get
            {
                return this.homeRoleField;
            }
            set
            {
                this.homeRoleField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=10)]
        public string HomeRoleName
        {
            get
            {
                return this.homeRoleNameField;
            }
            set
            {
                this.homeRoleNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=11)]
        public string BabyName
        {
            get
            {
                return this.babyNameField;
            }
            set
            {
                this.babyNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=12)]
        public string Hobby
        {
            get
            {
                return this.hobbyField;
            }
            set
            {
                this.hobbyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=13)]
        public int OfficialCity
        {
            get
            {
                return this.officialCityField;
            }
            set
            {
                this.officialCityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=14)]
        public string OfficialCityName
        {
            get
            {
                return this.officialCityNameField;
            }
            set
            {
                this.officialCityNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=15)]
        public string Address
        {
            get
            {
                return this.addressField;
            }
            set
            {
                this.addressField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=16)]
        public System.DateTime ActiveDate
        {
            get
            {
                return this.activeDateField;
            }
            set
            {
                this.activeDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=17)]
        public int RegisterSource
        {
            get
            {
                return this.registerSourceField;
            }
            set
            {
                this.registerSourceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=18)]
        public string RegisterSourceName
        {
            get
            {
                return this.registerSourceNameField;
            }
            set
            {
                this.registerSourceNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=19)]
        public string CollectProduct
        {
            get
            {
                return this.collectProductField;
            }
            set
            {
                this.collectProductField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=20)]
        public string CollectProductName
        {
            get
            {
                return this.collectProductNameField;
            }
            set
            {
                this.collectProductNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=21)]
        public string FirstBuyProductName
        {
            get
            {
                return this.firstBuyProductNameField;
            }
            set
            {
                this.firstBuyProductNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=22)]
        public int CRMID
        {
            get
            {
                return this.cRMIDField;
            }
            set
            {
                this.cRMIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=23)]
        public string MaMaGroup
        {
            get
            {
                return this.maMaGroupField;
            }
            set
            {
                this.maMaGroupField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=24)]
        public string ServiceCMClientCode
        {
            get
            {
                return this.serviceCMClientCodeField;
            }
            set
            {
                this.serviceCMClientCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=25)]
        public string ServiceCMClientName
        {
            get
            {
                return this.serviceCMClientNameField;
            }
            set
            {
                this.serviceCMClientNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=26)]
        public string ServicePromotorCode
        {
            get
            {
                return this.servicePromotorCodeField;
            }
            set
            {
                this.servicePromotorCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=27)]
        public string ServicePromotorName
        {
            get
            {
                return this.servicePromotorNameField;
            }
            set
            {
                this.servicePromotorNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=28)]
        public string ServicePromotorMobile
        {
            get
            {
                return this.servicePromotorMobileField;
            }
            set
            {
                this.servicePromotorMobileField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=29)]
        public int PreBrand
        {
            get
            {
                return this.preBrandField;
            }
            set
            {
                this.preBrandField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=30)]
        public string PreBrandOther
        {
            get
            {
                return this.preBrandOtherField;
            }
            set
            {
                this.preBrandOtherField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=31)]
        public int InfoCollectActivity
        {
            get
            {
                return this.infoCollectActivityField;
            }
            set
            {
                this.infoCollectActivityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=32)]
        public int ActivityInfoSource
        {
            get
            {
                return this.activityInfoSourceField;
            }
            set
            {
                this.activityInfoSourceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order=33)]
        public Attachment[] Atts
        {
            get
            {
                return this.attsField;
            }
            set
            {
                this.attsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=34)]
        public string XHFlag
        {
            get
            {
                return this.xHFlagField;
            }
            set
            {
                this.xHFlagField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=35)]
        public int XHUse
        {
            get
            {
                return this.xHUseField;
            }
            set
            {
                this.xHUseField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=36)]
        public string XHUseName
        {
            get
            {
                return this.xHUseNameField;
            }
            set
            {
                this.xHUseNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=37)]
        public System.DateTime XHBirthday
        {
            get
            {
                return this.xHBirthdayField;
            }
            set
            {
                this.xHBirthdayField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=38)]
        public int XHPreBrand
        {
            get
            {
                return this.xHPreBrandField;
            }
            set
            {
                this.xHPreBrandField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=39)]
        public string XHPreBrandName
        {
            get
            {
                return this.xHPreBrandNameField;
            }
            set
            {
                this.xHPreBrandNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=40)]
        public string XHRegProduct
        {
            get
            {
                return this.xHRegProductField;
            }
            set
            {
                this.xHRegProductField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=41)]
        public string XHRegProductName
        {
            get
            {
                return this.xHRegProductNameField;
            }
            set
            {
                this.xHRegProductNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=42)]
        public string XHProduct
        {
            get
            {
                return this.xHProductField;
            }
            set
            {
                this.xHProductField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=43)]
        public string XHProductName
        {
            get
            {
                return this.xHProductNameField;
            }
            set
            {
                this.xHProductNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=44)]
        public int XHMemberType
        {
            get
            {
                return this.xHMemberTypeField;
            }
            set
            {
                this.xHMemberTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=45)]
        public int XHHealthConcerns
        {
            get
            {
                return this.xHHealthConcernsField;
            }
            set
            {
                this.xHHealthConcernsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=46)]
        public string XHHealthConcernsName
        {
            get
            {
                return this.xHHealthConcernsNameField;
            }
            set
            {
                this.xHHealthConcernsNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=47)]
        public int XHRole
        {
            get
            {
                return this.xHRoleField;
            }
            set
            {
                this.xHRoleField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=48)]
        public int XHSex
        {
            get
            {
                return this.xHSexField;
            }
            set
            {
                this.xHSexField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=49)]
        public string XHUserName
        {
            get
            {
                return this.xHUserNameField;
            }
            set
            {
                this.xHUserNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=50)]
        public int XHUserSex
        {
            get
            {
                return this.xHUserSexField;
            }
            set
            {
                this.xHUserSexField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=51)]
        public string XHUserMobile
        {
            get
            {
                return this.xHUserMobileField;
            }
            set
            {
                this.xHUserMobileField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=52)]
        public System.DateTime XHUserBrithday
        {
            get
            {
                return this.xHUserBrithdayField;
            }
            set
            {
                this.xHUserBrithdayField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=53)]
        public decimal BalancePoints
        {
            get
            {
                return this.balancePointsField;
            }
            set
            {
                this.balancePointsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=54)]
        public decimal EatQuantityOneMonth
        {
            get
            {
                return this.eatQuantityOneMonthField;
            }
            set
            {
                this.eatQuantityOneMonthField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=55)]
        public int EatTimesOneDay
        {
            get
            {
                return this.eatTimesOneDayField;
            }
            set
            {
                this.eatTimesOneDayField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=56)]
        public int FeedMode
        {
            get
            {
                return this.feedModeField;
            }
            set
            {
                this.feedModeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=57)]
        public int ServicePreferences
        {
            get
            {
                return this.servicePreferencesField;
            }
            set
            {
                this.servicePreferencesField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="AddProductPoints", WrapperNamespace="http://mmp.meichis.com/DataInterface/", IsWrapped=true)]
    public partial class AddProductPointsRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=0)]
        public string AuthKey;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=1)]
        public string ProductCode;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=2)]
        public string Retailer;
        
        public AddProductPointsRequest()
        {
        }
        
        public AddProductPointsRequest(string AuthKey, string ProductCode, string Retailer)
        {
            this.AuthKey = AuthKey;
            this.ProductCode = ProductCode;
            this.Retailer = Retailer;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="AddProductPointsResponse", WrapperNamespace="http://mmp.meichis.com/DataInterface/", IsWrapped=true)]
    public partial class AddProductPointsResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=0)]
        public decimal AddProductPointsResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=1)]
        public string Product;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=2)]
        public decimal AddPoints;
        
        public AddProductPointsResponse()
        {
        }
        
        public AddProductPointsResponse(decimal AddProductPointsResult, string Product, decimal AddPoints)
        {
            this.AddProductPointsResult = AddProductPointsResult;
            this.Product = Product;
            this.AddPoints = AddPoints;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="AddProductPointsEx", WrapperNamespace="http://mmp.meichis.com/DataInterface/", IsWrapped=true)]
    public partial class AddProductPointsExRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=0)]
        public string AuthKey;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=1)]
        public string ProductCode;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=2)]
        public string Retailer;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=3)]
        public int Channel;
        
        public AddProductPointsExRequest()
        {
        }
        
        public AddProductPointsExRequest(string AuthKey, string ProductCode, string Retailer, int Channel)
        {
            this.AuthKey = AuthKey;
            this.ProductCode = ProductCode;
            this.Retailer = Retailer;
            this.Channel = Channel;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="AddProductPointsExResponse", WrapperNamespace="http://mmp.meichis.com/DataInterface/", IsWrapped=true)]
    public partial class AddProductPointsExResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=0)]
        public decimal AddProductPointsExResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=1)]
        public string Product;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=2)]
        public decimal AddPoints;
        
        public AddProductPointsExResponse()
        {
        }
        
        public AddProductPointsExResponse(decimal AddProductPointsExResult, string Product, decimal AddPoints)
        {
            this.AddProductPointsExResult = AddProductPointsExResult;
            this.Product = Product;
            this.AddPoints = AddPoints;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="MicroBlog_AuthLoginEx", WrapperNamespace="http://mmp.meichis.com/DataInterface/", IsWrapped=true)]
    public partial class MicroBlog_AuthLoginExRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=0)]
        public string access_token;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=1)]
        public string uid;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=2)]
        public string DeviceCode;
        
        public MicroBlog_AuthLoginExRequest()
        {
        }
        
        public MicroBlog_AuthLoginExRequest(string access_token, string uid, string DeviceCode)
        {
            this.access_token = access_token;
            this.uid = uid;
            this.DeviceCode = DeviceCode;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="MicroBlog_AuthLoginExResponse", WrapperNamespace="http://mmp.meichis.com/DataInterface/", IsWrapped=true)]
    public partial class MicroBlog_AuthLoginExResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=0)]
        public string MicroBlog_AuthLoginExResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=1)]
        public string username;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=2)]
        public string userid;
        
        public MicroBlog_AuthLoginExResponse()
        {
        }
        
        public MicroBlog_AuthLoginExResponse(string MicroBlog_AuthLoginExResult, string username, string userid)
        {
            this.MicroBlog_AuthLoginExResult = MicroBlog_AuthLoginExResult;
            this.username = username;
            this.userid = userid;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="MicroBlog_AuthLogin", WrapperNamespace="http://mmp.meichis.com/DataInterface/", IsWrapped=true)]
    public partial class MicroBlog_AuthLoginRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=0)]
        public string authkey;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=1)]
        public string access_token;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=2)]
        public string uid;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=3)]
        public string deviceCode;
        
        public MicroBlog_AuthLoginRequest()
        {
        }
        
        public MicroBlog_AuthLoginRequest(string authkey, string access_token, string uid, string deviceCode)
        {
            this.authkey = authkey;
            this.access_token = access_token;
            this.uid = uid;
            this.deviceCode = deviceCode;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="MicroBlog_AuthLoginResponse", WrapperNamespace="http://mmp.meichis.com/DataInterface/", IsWrapped=true)]
    public partial class MicroBlog_AuthLoginResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=0)]
        public string MicroBlog_AuthLoginResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=1)]
        public string username;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=2)]
        public string userid;
        
        public MicroBlog_AuthLoginResponse()
        {
        }
        
        public MicroBlog_AuthLoginResponse(string MicroBlog_AuthLoginResult, string username, string userid)
        {
            this.MicroBlog_AuthLoginResult = MicroBlog_AuthLoginResult;
            this.username = username;
            this.userid = userid;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="WechatSendTemplateMsg", WrapperNamespace="http://mmp.meichis.com/DataInterface/", IsWrapped=true)]
    public partial class WechatSendTemplateMsgRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=0)]
        public string AuthKey;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=1)]
        public string ReqJsonString;
        
        public WechatSendTemplateMsgRequest()
        {
        }
        
        public WechatSendTemplateMsgRequest(string AuthKey, string ReqJsonString)
        {
            this.AuthKey = AuthKey;
            this.ReqJsonString = ReqJsonString;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="WechatSendTemplateMsgResponse", WrapperNamespace="http://mmp.meichis.com/DataInterface/", IsWrapped=true)]
    public partial class WechatSendTemplateMsgResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=0)]
        public int WechatSendTemplateMsgResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=1)]
        public string resstr;
        
        public WechatSendTemplateMsgResponse()
        {
        }
        
        public WechatSendTemplateMsgResponse(int WechatSendTemplateMsgResult, string resstr)
        {
            this.WechatSendTemplateMsgResult = WechatSendTemplateMsgResult;
            this.resstr = resstr;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="CY_GetCard", WrapperNamespace="http://mmp.meichis.com/DataInterface/", IsWrapped=true)]
    public partial class CY_GetCardRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=0)]
        public string authkey;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=1)]
        public int crmid;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=2)]
        public int source;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=3)]
        public string remark;
        
        public CY_GetCardRequest()
        {
        }
        
        public CY_GetCardRequest(string authkey, int crmid, int source, string remark)
        {
            this.authkey = authkey;
            this.crmid = crmid;
            this.source = source;
            this.remark = remark;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="CY_GetCardResponse", WrapperNamespace="http://mmp.meichis.com/DataInterface/", IsWrapped=true)]
    public partial class CY_GetCardResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=0)]
        public int CY_GetCardResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=1)]
        public string cardcode;
        
        public CY_GetCardResponse()
        {
        }
        
        public CY_GetCardResponse(int CY_GetCardResult, string cardcode)
        {
            this.CY_GetCardResult = CY_GetCardResult;
            this.cardcode = cardcode;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    public interface MemberServiceSoapChannel : yilibabyMember.MemberServiceSoap, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    public partial class MemberServiceSoapClient : System.ServiceModel.ClientBase<yilibabyMember.MemberServiceSoap>, yilibabyMember.MemberServiceSoap
    {
        
    /// <summary>
    /// 实现此分部方法，配置服务终结点。
    /// </summary>
    /// <param name="serviceEndpoint">要配置的终结点</param>
    /// <param name="clientCredentials">客户端凭据</param>
    static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public MemberServiceSoapClient(EndpointConfiguration endpointConfiguration) : 
                base(MemberServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), MemberServiceSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public MemberServiceSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(MemberServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public MemberServiceSoapClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(MemberServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public MemberServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.Threading.Tasks.Task<System.Xml.Linq.XElement> ExportProductInfoXMLAsync()
        {
            return base.Channel.ExportProductInfoXMLAsync();
        }
        
        public System.Threading.Tasks.Task<string> HelloWorldAsync()
        {
            return base.Channel.HelloWorldAsync();
        }
        
        public System.Threading.Tasks.Task<System.DateTime> GetServerSyncTimeAsync()
        {
            return base.Channel.GetServerSyncTimeAsync();
        }
        
        public System.Threading.Tasks.Task<yilibabyMember.MemberCart[]> MemberCartGetListAsync(string AuthKey)
        {
            return base.Channel.MemberCartGetListAsync(AuthKey);
        }
        
        public System.Threading.Tasks.Task<string> MemberCartGetListJsonAsync(string AuthKey)
        {
            return base.Channel.MemberCartGetListJsonAsync(AuthKey);
        }
        
        public System.Threading.Tasks.Task<int> MemberCartAddJsonAsync(string AuthKey, string Cart)
        {
            return base.Channel.MemberCartAddJsonAsync(AuthKey, Cart);
        }
        
        public System.Threading.Tasks.Task<int> MemberCartRemoveJsonAsync(string AuthKey, string Cart)
        {
            return base.Channel.MemberCartRemoveJsonAsync(AuthKey, Cart);
        }
        
        public System.Threading.Tasks.Task<int> MemberCartUpdateJsonAsync(string AuthKey, string Cart)
        {
            return base.Channel.MemberCartUpdateJsonAsync(AuthKey, Cart);
        }
        
        public System.Threading.Tasks.Task<yilibabyMember.Member> GetMyMemberInfoAsync(string AuthKey)
        {
            return base.Channel.GetMyMemberInfoAsync(AuthKey);
        }
        
        public System.Threading.Tasks.Task<string> GetMyMemberInfoJsonAsync(string AuthKey)
        {
            return base.Channel.GetMyMemberInfoJsonAsync(AuthKey);
        }
        
        public System.Threading.Tasks.Task<yilibabyMember.Member> GetMemberByMobileAsync(string Mobile)
        {
            return base.Channel.GetMemberByMobileAsync(Mobile);
        }
        
        public System.Threading.Tasks.Task<string> GetMemberByMobileJsonAsync(string AuthKey, string Mobile)
        {
            return base.Channel.GetMemberByMobileJsonAsync(AuthKey, Mobile);
        }
        
        public System.Threading.Tasks.Task<int> UpdateMemberInfoJsonAsync(string AuthKey, string Member)
        {
            return base.Channel.UpdateMemberInfoJsonAsync(AuthKey, Member);
        }
        
        public System.Threading.Tasks.Task<int> SendUserRegisterMailAsync(string AuthKey)
        {
            return base.Channel.SendUserRegisterMailAsync(AuthKey);
        }
        
        public System.Threading.Tasks.Task<int> MemberBindWeChatAsync(string AuthKey, string UserWeChat, string CompanyWeChat, string Remark)
        {
            return base.Channel.MemberBindWeChatAsync(AuthKey, UserWeChat, CompanyWeChat, Remark);
        }
        
        public System.Threading.Tasks.Task<decimal> GetPointsBalanceAsync(string AuthKey)
        {
            return base.Channel.GetPointsBalanceAsync(AuthKey);
        }
        
        public System.Threading.Tasks.Task<yilibabyMember.MemberPointsInfo> GetPointsInfoAsync(string AuthKey)
        {
            return base.Channel.GetPointsInfoAsync(AuthKey);
        }
        
        public System.Threading.Tasks.Task<string> GetPointsInfoJsonAsync(string AuthKey)
        {
            return base.Channel.GetPointsInfoJsonAsync(AuthKey);
        }
        
        public System.Threading.Tasks.Task<yilibabyMember.AddProductPointsResponse> AddProductPointsAsync(yilibabyMember.AddProductPointsRequest request)
        {
            return base.Channel.AddProductPointsAsync(request);
        }
        
        public System.Threading.Tasks.Task<yilibabyMember.AddProductPointsExResponse> AddProductPointsExAsync(yilibabyMember.AddProductPointsExRequest request)
        {
            return base.Channel.AddProductPointsExAsync(request);
        }
        
        public System.Threading.Tasks.Task<int> AddMemberPointsAsync(string AuthKey, int Source, int Points, string Remark, string Key)
        {
            return base.Channel.AddMemberPointsAsync(AuthKey, Source, Points, Remark, Key);
        }
        
        public System.Threading.Tasks.Task<int> AddMemberPointsExAsync(string AuthKey, int Source, int Points, string Remark, string Key)
        {
            return base.Channel.AddMemberPointsExAsync(AuthKey, Source, Points, Remark, Key);
        }
        
        public System.Threading.Tasks.Task<int> AddMemberPointsEx2Async(string AuthKey, int CRMID, int Source, int Points, string Remark, string Key)
        {
            return base.Channel.AddMemberPointsEx2Async(AuthKey, CRMID, Source, Points, Remark, Key);
        }
        
        public System.Threading.Tasks.Task<string> GetPointsChangeDetailJsonAsync(string AuthKey, System.DateTime BeginDate, System.DateTime EndDate)
        {
            return base.Channel.GetPointsChangeDetailJsonAsync(AuthKey, BeginDate, EndDate);
        }
        
        public System.Threading.Tasks.Task<int> VerifyProductCodeAsync(string AuthKey, string ProductCode)
        {
            return base.Channel.VerifyProductCodeAsync(AuthKey, ProductCode);
        }
        
        public System.Threading.Tasks.Task<string> GetDeliveryAddressJsonAsync(string AuthKey)
        {
            return base.Channel.GetDeliveryAddressJsonAsync(AuthKey);
        }
        
        public System.Threading.Tasks.Task<int> DeliveryAddressAddJsonAsync(string AuthKey, string Addr)
        {
            return base.Channel.DeliveryAddressAddJsonAsync(AuthKey, Addr);
        }
        
        public System.Threading.Tasks.Task<int> DeliveryAddressUpdateJsonAsync(string AuthKey, string Addr)
        {
            return base.Channel.DeliveryAddressUpdateJsonAsync(AuthKey, Addr);
        }
        
        public System.Threading.Tasks.Task<int> DeliveryAddressSetDefaultAsync(string AuthKey, int AddrID)
        {
            return base.Channel.DeliveryAddressSetDefaultAsync(AuthKey, AddrID);
        }
        
        public System.Threading.Tasks.Task<int> DeliveryAddressDelAsync(string AuthKey, int AddrID)
        {
            return base.Channel.DeliveryAddressDelAsync(AuthKey, AddrID);
        }
        
        public System.Threading.Tasks.Task<yilibabyMember.ExchangeOrder[]> GetMyExchangeOrderListAsync(string AuthKey)
        {
            return base.Channel.GetMyExchangeOrderListAsync(AuthKey);
        }
        
        public System.Threading.Tasks.Task<string> GetMyExchangeOrderListJsonAsync(string AuthKey)
        {
            return base.Channel.GetMyExchangeOrderListJsonAsync(AuthKey);
        }
        
        public System.Threading.Tasks.Task<string> GetMyExchangeOrderJsonAsync(string AuthKey, int OrderID)
        {
            return base.Channel.GetMyExchangeOrderJsonAsync(AuthKey, OrderID);
        }
        
        public System.Threading.Tasks.Task<int> ChangeExchangeOrderAddressJsonAsync(string AuthKey, int OrderID, string NewAddress)
        {
            return base.Channel.ChangeExchangeOrderAddressJsonAsync(AuthKey, OrderID, NewAddress);
        }
        
        public System.Threading.Tasks.Task<int> SignInExchangeOrderAsync(string AuthKey, int OrderID)
        {
            return base.Channel.SignInExchangeOrderAsync(AuthKey, OrderID);
        }
        
        public System.Threading.Tasks.Task<int> CancelExchangeOrderAsync(string AuthKey, int OrderID)
        {
            return base.Channel.CancelExchangeOrderAsync(AuthKey, OrderID);
        }
        
        public System.Threading.Tasks.Task<int> CancelExchangeOrderExAsync(string AuthKey, int OrderID, string CancelRemark)
        {
            return base.Channel.CancelExchangeOrderExAsync(AuthKey, OrderID, CancelRemark);
        }
        
        public System.Threading.Tasks.Task<int> ReturnExchangeOrderAsync(string AuthKey, int OrderID, string CancelReason)
        {
            return base.Channel.ReturnExchangeOrderAsync(AuthKey, OrderID, CancelReason);
        }
        
        public System.Threading.Tasks.Task<yilibabyMember.GiftCatalog[]> GetGiftCatalogsAsync(string AuthKey, int SuperID)
        {
            return base.Channel.GetGiftCatalogsAsync(AuthKey, SuperID);
        }
        
        public System.Threading.Tasks.Task<string> GetGiftCatalogsJsonAsync(string AuthKey, int SuperID)
        {
            return base.Channel.GetGiftCatalogsJsonAsync(AuthKey, SuperID);
        }
        
        public System.Threading.Tasks.Task<yilibabyMember.Gift[]> GetGiftsByCatalogAsync(string AuthKey, int Catalog)
        {
            return base.Channel.GetGiftsByCatalogAsync(AuthKey, Catalog);
        }
        
        public System.Threading.Tasks.Task<string> GetGiftsByCatalogJsonAsync(string AuthKey, int Catalog)
        {
            return base.Channel.GetGiftsByCatalogJsonAsync(AuthKey, Catalog);
        }
        
        public System.Threading.Tasks.Task<int> GetGiftInventoryAsync(string AuthKey, int GiftID)
        {
            return base.Channel.GetGiftInventoryAsync(AuthKey, GiftID);
        }
        
        public System.Threading.Tasks.Task<yilibabyMember.Gift[]> GetHotGiftsAsync(string AuthKey, int TopCount, int MaxPoints, System.DateTime BeginDate)
        {
            return base.Channel.GetHotGiftsAsync(AuthKey, TopCount, MaxPoints, BeginDate);
        }
        
        public System.Threading.Tasks.Task<string> GetHotGiftsJsonAsync(string AuthKey, int TopCount, int MaxPoints, System.DateTime BeginDate)
        {
            return base.Channel.GetHotGiftsJsonAsync(AuthKey, TopCount, MaxPoints, BeginDate);
        }
        
        public System.Threading.Tasks.Task<yilibabyMember.Gift> GetGiftInfoAsync(string AuthKey, string GiftCode)
        {
            return base.Channel.GetGiftInfoAsync(AuthKey, GiftCode);
        }
        
        public System.Threading.Tasks.Task<string> GetGiftInfoJsonAsync(string AuthKey, string GiftCode)
        {
            return base.Channel.GetGiftInfoJsonAsync(AuthKey, GiftCode);
        }
        
        public System.Threading.Tasks.Task<int> CustomerExchangeOrderApplyEx3Async(string AuthKey, string CityName, string Consignee, string Address, string Mobile, string AcceptRemark, string ProductCodes, string Quantitys, int Source)
        {
            return base.Channel.CustomerExchangeOrderApplyEx3Async(AuthKey, CityName, Consignee, Address, Mobile, AcceptRemark, ProductCodes, Quantitys, Source);
        }
        
        public System.Threading.Tasks.Task<int> CustomerExchangeOrderApplyExAsync(string AuthKey, int OfficialCity, string Consignee, string Address, string Mobile, string AcceptRemark, string ProductIDs, string Quantitys, int Source)
        {
            return base.Channel.CustomerExchangeOrderApplyExAsync(AuthKey, OfficialCity, Consignee, Address, Mobile, AcceptRemark, ProductIDs, Quantitys, Source);
        }
        
        public System.Threading.Tasks.Task<int> CustomerExchangeOrderApplyEx2Async(string AuthKey, int OfficialCity, string Consignee, string Address, string Mobile, string AcceptRemark, string ProductIDs, string Quantitys, int Source)
        {
            return base.Channel.CustomerExchangeOrderApplyEx2Async(AuthKey, OfficialCity, Consignee, Address, Mobile, AcceptRemark, ProductIDs, Quantitys, Source);
        }
        
        public System.Threading.Tasks.Task<int> CustomerExchangeOrderApply2Async(string AuthKey, int OfficialCity, string Consignee, string Address, string Mobile, string AcceptRemark, string ProductIDs, string Quantitys, int Source)
        {
            return base.Channel.CustomerExchangeOrderApply2Async(AuthKey, OfficialCity, Consignee, Address, Mobile, AcceptRemark, ProductIDs, Quantitys, Source);
        }
        
        public System.Threading.Tasks.Task<int> ChangeExchangeOrderAddressAsync(string AuthKey, int OrderID, int OfficialCity, string Consignee, string Address, string Mobile, string AcceptRemark)
        {
            return base.Channel.ChangeExchangeOrderAddressAsync(AuthKey, OrderID, OfficialCity, Consignee, Address, Mobile, AcceptRemark);
        }
        
        public System.Threading.Tasks.Task<int> IsJinLingGuanNewCustomerAsync(string AuthKey)
        {
            return base.Channel.IsJinLingGuanNewCustomerAsync(AuthKey);
        }
        
        public System.Threading.Tasks.Task<int> IsHaveRecommendQualificationsAsync(string AuthKey)
        {
            return base.Channel.IsHaveRecommendQualificationsAsync(AuthKey);
        }
        
        public System.Threading.Tasks.Task<int> RecommendInfoAddAsync(string AuthKey, string Name, string Mobile, string Email)
        {
            return base.Channel.RecommendInfoAddAsync(AuthKey, Name, Mobile, Email);
        }
        
        public System.Threading.Tasks.Task<string> GetClientInfoForNewClientActivityAsync(string AuthKey)
        {
            return base.Channel.GetClientInfoForNewClientActivityAsync(AuthKey);
        }
        
        public System.Threading.Tasks.Task<string> GetClientDSInfoForNewClientActivityAsync(string AuthKey, int CRMID)
        {
            return base.Channel.GetClientDSInfoForNewClientActivityAsync(AuthKey, CRMID);
        }
        
        public System.Threading.Tasks.Task<string> GetClientInfoForNewClientActivityByCRMIDAsync(string AuthKey, int CRMID)
        {
            return base.Channel.GetClientInfoForNewClientActivityByCRMIDAsync(AuthKey, CRMID);
        }
        
        public System.Threading.Tasks.Task<string> GetClientInfoForNewClientActivityByMobileAsync(string AuthKey, string Mobile)
        {
            return base.Channel.GetClientInfoForNewClientActivityByMobileAsync(AuthKey, Mobile);
        }
        
        public System.Threading.Tasks.Task<yilibabyMember.DicDataItem[]> GetMostlyProductListAsync(string AuthKey)
        {
            return base.Channel.GetMostlyProductListAsync(AuthKey);
        }
        
        public System.Threading.Tasks.Task<string> GetMostlyProductListJsonAsync(string AuthKey)
        {
            return base.Channel.GetMostlyProductListJsonAsync(AuthKey);
        }
        
        public System.Threading.Tasks.Task<string> GetBrandListAsync(string authKey, int type)
        {
            return base.Channel.GetBrandListAsync(authKey, type);
        }
        
        public System.Threading.Tasks.Task<string> GetBrandListJsonAsync(string AuthKey, int type)
        {
            return base.Channel.GetBrandListJsonAsync(AuthKey, type);
        }
        
        public System.Threading.Tasks.Task<yilibabyMember.ClientInfo[]> GetClientByGeoAsync(string AuthKey, string lat, string lng)
        {
            return base.Channel.GetClientByGeoAsync(AuthKey, lat, lng);
        }
        
        public System.Threading.Tasks.Task<yilibabyMember.ClientInfo[]> GetRetailerListByOfficialCityAsync(string AuthKey, int OfficialCity)
        {
            return base.Channel.GetRetailerListByOfficialCityAsync(AuthKey, OfficialCity);
        }
        
        public System.Threading.Tasks.Task<string> GetRetailerListByOfficialCityJsonAsync(string AuthKey, int OfficialCity)
        {
            return base.Channel.GetRetailerListByOfficialCityJsonAsync(AuthKey, OfficialCity);
        }
        
        public System.Threading.Tasks.Task<yilibabyMember.ClientInfo[]> GetNearRetailerListAsync(string AuthKey, int OfficialCity, float Latitude, float Longitude, int Distance)
        {
            return base.Channel.GetNearRetailerListAsync(AuthKey, OfficialCity, Latitude, Longitude, Distance);
        }
        
        public System.Threading.Tasks.Task<string> GetNearRetailerListJsonAsync(string AuthKey, int OfficialCity, float Latitude, float Longitude, int Distance)
        {
            return base.Channel.GetNearRetailerListJsonAsync(AuthKey, OfficialCity, Latitude, Longitude, Distance);
        }
        
        public System.Threading.Tasks.Task<int> CustomerServiceAcceptAsync(string AuthKey, string Topic, string Content, string ServiceStaff, int AcceptSource)
        {
            return base.Channel.CustomerServiceAcceptAsync(AuthKey, Topic, Content, ServiceStaff, AcceptSource);
        }
        
        public System.Threading.Tasks.Task<int> CustomerRecommandAcceptAsync(string AuthKey, string RecommandName, string RecommandMobile, string RecommandEmail)
        {
            return base.Channel.CustomerRecommandAcceptAsync(AuthKey, RecommandName, RecommandMobile, RecommandEmail);
        }
        
        public System.Threading.Tasks.Task<yilibabyMember.RecommendCustomerInfo[]> GetCustomerRecommandListAsync(string AuthKey, string BeginTime, string EndTime)
        {
            return base.Channel.GetCustomerRecommandListAsync(AuthKey, BeginTime, EndTime);
        }
        
        public System.Threading.Tasks.Task<string> GetCustomerRecommandListJsonAsync(string AuthKey, string BeginTime, string EndTime)
        {
            return base.Channel.GetCustomerRecommandListJsonAsync(AuthKey, BeginTime, EndTime);
        }
        
        public System.Threading.Tasks.Task<int> GetCustomerRecommandPointAsync(string AuthKey)
        {
            return base.Channel.GetCustomerRecommandPointAsync(AuthKey);
        }
        
        public System.Threading.Tasks.Task<string[]> FindWeChatMemberByAdvConditionAsync(string AuthKey, string MemberAdvConditionJson)
        {
            return base.Channel.FindWeChatMemberByAdvConditionAsync(AuthKey, MemberAdvConditionJson);
        }
        
        public System.Threading.Tasks.Task<int> GetCountByClientBrithdayAsync(string AuthKey, string BeginTime, string EndTime)
        {
            return base.Channel.GetCountByClientBrithdayAsync(AuthKey, BeginTime, EndTime);
        }
        
        public System.Threading.Tasks.Task<int> JudgeGuardTrialPackApplyConditonAsync(string AuthKey, string StartTime)
        {
            return base.Channel.JudgeGuardTrialPackApplyConditonAsync(AuthKey, StartTime);
        }
        
        public System.Threading.Tasks.Task<string> MicroBlog_GetAuthorizeAccessAsync(string client_id, string callbackUrl, string scope, string state, string display, string forcelogin, string language)
        {
            return base.Channel.MicroBlog_GetAuthorizeAccessAsync(client_id, callbackUrl, scope, state, display, forcelogin, language);
        }
        
        public System.Threading.Tasks.Task<string> MicroBlog_AccessTokenAsync(string client_id, string client_secret, string grant_type, string code, string callbackUrl)
        {
            return base.Channel.MicroBlog_AccessTokenAsync(client_id, client_secret, grant_type, code, callbackUrl);
        }
        
        public System.Threading.Tasks.Task<yilibabyMember.MicroBlog_AuthLoginExResponse> MicroBlog_AuthLoginExAsync(yilibabyMember.MicroBlog_AuthLoginExRequest request)
        {
            return base.Channel.MicroBlog_AuthLoginExAsync(request);
        }
        
        public System.Threading.Tasks.Task<yilibabyMember.MicroBlog_AuthLoginResponse> MicroBlog_AuthLoginAsync(yilibabyMember.MicroBlog_AuthLoginRequest request)
        {
            return base.Channel.MicroBlog_AuthLoginAsync(request);
        }
        
        public System.Threading.Tasks.Task<string> MicroBlog_GetUserAsync(string access_token, string uid, string source, string screen_name)
        {
            return base.Channel.MicroBlog_GetUserAsync(access_token, uid, source, screen_name);
        }
        
        public System.Threading.Tasks.Task<string> MicroBlog_GetUserEmailAsync(string access_token, string souece)
        {
            return base.Channel.MicroBlog_GetUserEmailAsync(access_token, souece);
        }
        
        public System.Threading.Tasks.Task<string> MicroBlog_GetNameByAddressCodeAsync(string access_token, string codes, string source)
        {
            return base.Channel.MicroBlog_GetNameByAddressCodeAsync(access_token, codes, source);
        }
        
        public System.Threading.Tasks.Task<string> MicroBlog_GetTokenInfoAsync(string access_token)
        {
            return base.Channel.MicroBlog_GetTokenInfoAsync(access_token);
        }
        
        public System.Threading.Tasks.Task<int> MemberGetCardJsonAsync(string AuthKey, string CardID, string CardCode, int Source, string Remark)
        {
            return base.Channel.MemberGetCardJsonAsync(AuthKey, CardID, CardCode, Source, Remark);
        }
        
        public System.Threading.Tasks.Task<yilibabyMember.ArrayOfXElement> GetFansSourceDataAsync(System.DateTime startTime, System.DateTime endTime)
        {
            return base.Channel.GetFansSourceDataAsync(startTime, endTime);
        }
        
        public System.Threading.Tasks.Task<yilibabyMember.WechatSendTemplateMsgResponse> WechatSendTemplateMsgAsync(yilibabyMember.WechatSendTemplateMsgRequest request)
        {
            return base.Channel.WechatSendTemplateMsgAsync(request);
        }
        
        public System.Threading.Tasks.Task<yilibabyMember.CY_GetCardResponse> CY_GetCardAsync(yilibabyMember.CY_GetCardRequest request)
        {
            return base.Channel.CY_GetCardAsync(request);
        }
        
        public System.Threading.Tasks.Task<int> StampActivity_MemberJoinJudgeAsync(string AuthKey, int Member)
        {
            return base.Channel.StampActivity_MemberJoinJudgeAsync(AuthKey, Member);
        }
        
        public System.Threading.Tasks.Task<yilibabyMember.StampGift[]> StampActivity_CanGetGiftListAsync(string AuthKey, int Member)
        {
            return base.Channel.StampActivity_CanGetGiftListAsync(AuthKey, Member);
        }
        
        public System.Threading.Tasks.Task<string> StampActivity_CanGetGiftListJsonAsync(string AuthKey, int Member)
        {
            return base.Channel.StampActivity_CanGetGiftListJsonAsync(AuthKey, Member);
        }
        
        public System.Threading.Tasks.Task<int> StampActivityMemberGetGiftViaExpressAsync(string AuthKey, int Activity, int crmid, string GetGiftCode, int OfficialCity, string Address, string Consignee, string Mobile, int SourceInfo, string Remark)
        {
            return base.Channel.StampActivityMemberGetGiftViaExpressAsync(AuthKey, Activity, crmid, GetGiftCode, OfficialCity, Address, Consignee, Mobile, SourceInfo, Remark);
        }
        
        public System.Threading.Tasks.Task<int> GetGiftAsync(string Member, int InfoSource)
        {
            return base.Channel.GetGiftAsync(Member, InfoSource);
        }
        
        public System.Threading.Tasks.Task<int> getNotReceiveGiftAsync(string MemberID)
        {
            return base.Channel.getNotReceiveGiftAsync(MemberID);
        }
        
        public System.Threading.Tasks.Task<int> PointsClearOrderAsync(string Member, int OfficialCity1, string Address, string Consignee, string Mobile, int SourceInfo, int Award, string Remark)
        {
            return base.Channel.PointsClearOrderAsync(Member, OfficialCity1, Address, Consignee, Mobile, SourceInfo, Award, Remark);
        }
        
        public System.Threading.Tasks.Task<int> GetMemberType_MBabyH5Async(string authkey)
        {
            return base.Channel.GetMemberType_MBabyH5Async(authkey);
        }
        
        public System.Threading.Tasks.Task<string> GetLastBuyProduct_ActivityAsync(string authkey)
        {
            return base.Channel.GetLastBuyProduct_ActivityAsync(authkey);
        }
        
        public System.Threading.Tasks.Task<int> CustomerJoinActivitySignAsync(string AuthKey, int ClientID, int ActivityID, int InfoSource)
        {
            return base.Channel.CustomerJoinActivitySignAsync(AuthKey, ClientID, ActivityID, InfoSource);
        }
        
        public System.Threading.Tasks.Task<int> UpdateMemberInfoCollectActivityAsync(string AuthKey, int InfoCollectActivity)
        {
            return base.Channel.UpdateMemberInfoCollectActivityAsync(AuthKey, InfoCollectActivity);
        }
        
        public System.Threading.Tasks.Task<string> GetMemberInfoServiceCMClientCodeAsync(string AuthKey, string Mobile)
        {
            return base.Channel.GetMemberInfoServiceCMClientCodeAsync(AuthKey, Mobile);
        }
        
        public System.Threading.Tasks.Task<int> UpdateMemberInfoServiceCMClientCodeAsync(string AuthKey, string Mobile, string ServiceCMClientCode)
        {
            return base.Channel.UpdateMemberInfoServiceCMClientCodeAsync(AuthKey, Mobile, ServiceCMClientCode);
        }
        
        public System.Threading.Tasks.Task<string> GetUserInfoByMobileJsonAsync(string AuthKey, string Mobile)
        {
            return base.Channel.GetUserInfoByMobileJsonAsync(AuthKey, Mobile);
        }
        
        public System.Threading.Tasks.Task<int> IsHaveRecommendQualifications_xhAsync(string AuthKey)
        {
            return base.Channel.IsHaveRecommendQualifications_xhAsync(AuthKey);
        }
        
        public System.Threading.Tasks.Task<int> RecommendInfoAdd_xhAsync(string AuthKey, string Name, string Mobile, string Email)
        {
            return base.Channel.RecommendInfoAdd_xhAsync(AuthKey, Name, Mobile, Email);
        }
        
        public System.Threading.Tasks.Task<int> StampActivity_MemberJoinJudge2Async(string authkey, int crmid, int activity)
        {
            return base.Channel.StampActivity_MemberJoinJudge2Async(authkey, crmid, activity);
        }
        
        public System.Threading.Tasks.Task<string> StampActivity_GetParamsAsync(string authkey, int crmid, int activity)
        {
            return base.Channel.StampActivity_GetParamsAsync(authkey, crmid, activity);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.MemberServiceSoap))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.MemberServiceSoap12))
            {
                System.ServiceModel.Channels.CustomBinding result = new System.ServiceModel.Channels.CustomBinding();
                System.ServiceModel.Channels.TextMessageEncodingBindingElement textBindingElement = new System.ServiceModel.Channels.TextMessageEncodingBindingElement();
                textBindingElement.MessageVersion = System.ServiceModel.Channels.MessageVersion.CreateVersion(System.ServiceModel.EnvelopeVersion.Soap12, System.ServiceModel.Channels.AddressingVersion.None);
                result.Elements.Add(textBindingElement);
                System.ServiceModel.Channels.HttpTransportBindingElement httpBindingElement = new System.ServiceModel.Channels.HttpTransportBindingElement();
                httpBindingElement.AllowCookies = true;
                httpBindingElement.MaxBufferSize = int.MaxValue;
                httpBindingElement.MaxReceivedMessageSize = int.MaxValue;
                result.Elements.Add(httpBindingElement);
                return result;
            }
            throw new System.InvalidOperationException(string.Format("找不到名称为“{0}”的终结点。", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.MemberServiceSoap))
            {
                return new System.ServiceModel.EndpointAddress("http://t.jifen.yilibabyclub.com/MClubIF2/MemberService.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.MemberServiceSoap12))
            {
                return new System.ServiceModel.EndpointAddress("http://t.jifen.yilibabyclub.com/MClubIF2/MemberService.asmx");
            }
            throw new System.InvalidOperationException(string.Format("找不到名称为“{0}”的终结点。", endpointConfiguration));
        }
        
        public enum EndpointConfiguration
        {
            
            MemberServiceSoap,
            
            MemberServiceSoap12,
        }
    }
    
    [System.Xml.Serialization.XmlSchemaProviderAttribute(null, IsAny=true)]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    public partial class ArrayOfXElement : object, System.Xml.Serialization.IXmlSerializable
    {
        
        private System.Collections.Generic.List<System.Xml.Linq.XElement> nodesList = new System.Collections.Generic.List<System.Xml.Linq.XElement>();
        
        public ArrayOfXElement()
        {
        }
        
        public virtual System.Collections.Generic.List<System.Xml.Linq.XElement> Nodes
        {
            get
            {
                return this.nodesList;
            }
        }
        
        public virtual System.Xml.Schema.XmlSchema GetSchema()
        {
            throw new System.NotImplementedException();
        }
        
        public virtual void WriteXml(System.Xml.XmlWriter writer)
        {
            System.Collections.Generic.IEnumerator<System.Xml.Linq.XElement> e = nodesList.GetEnumerator();
            for (
            ; e.MoveNext(); 
            )
            {
                ((System.Xml.Serialization.IXmlSerializable)(e.Current)).WriteXml(writer);
            }
        }
        
        public virtual void ReadXml(System.Xml.XmlReader reader)
        {
            for (
            ; (reader.NodeType != System.Xml.XmlNodeType.EndElement); 
            )
            {
                if ((reader.NodeType == System.Xml.XmlNodeType.Element))
                {
                    System.Xml.Linq.XElement elem = new System.Xml.Linq.XElement("default");
                    ((System.Xml.Serialization.IXmlSerializable)(elem)).ReadXml(reader);
                    Nodes.Add(elem);
                }
                else
                {
                    reader.Skip();
                }
            }
        }
    }
}
