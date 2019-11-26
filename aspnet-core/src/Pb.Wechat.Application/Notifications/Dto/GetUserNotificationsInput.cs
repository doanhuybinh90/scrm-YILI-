using Abp.Notifications;
using Pb.Wechat.Dto;

namespace Pb.Wechat.Notifications.Dto
{
    public class GetUserNotificationsInput : PagedInputDto
    {
        public UserNotificationState? State { get; set; }
    }
}