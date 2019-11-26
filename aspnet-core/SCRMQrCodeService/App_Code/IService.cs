using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;

// 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IService”。
[ServiceContract]
public interface IService
{
    /// <summary>
    /// 生成临时二维码(有大效期30天)(微信)
    /// </summary>
    /// <param name="OpenId">公众号ID</param>
    /// <param name="scene_id">参数值（妈妈班活动ID+110000）</param>
    /// <param name="expire_seconds">二维码有效期</param>
    /// <returns>二维码对应Ticket值</returns>
    [OperationContract]
    Task<string> GetQRCodeTicket(string OpenId, int scene_id, int expire_seconds);
    /// <summary>
    /// 根据门店ID或导购ID生成永久二维码
    /// </summary>
    /// <param name="OpenId">公众号ID</param>
    /// <param name="">申请来源（1：会员中心；2：商家中心）</param>
    /// <param name="">分配类型（1:门店；2：导购；3：代表）</param>
    /// <param name="">分配关联ID(门店或导购ID)</param>
    /// <returns>二维码对应Ticket值</returns>
    [OperationContract]
    Task<string> GetQRCodeLimitTicketEx(string OpenId, int ApplySource, int AssignType, int AssignRelateID);
   
	// TODO: 在此添加您的服务操作
}


// 使用下面示例中说明的数据约定将复合类型添加到服务操作。
[DataContract]
public class CompositeType
{
	bool boolValue = true;
	string stringValue = "Hello ";

	[DataMember]
	public bool BoolValue
	{
		get { return boolValue; }
		set { boolValue = value; }
	}

	[DataMember]
	public string StringValue
	{
		get { return stringValue; }
		set { stringValue = value; }
	}
}
