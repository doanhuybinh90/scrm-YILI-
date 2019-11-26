using System.Collections.Generic;

namespace Pb.Wechat.Web.Models.ChunYuModel
{
    public class CreateProblem_Request: BaseRequestModel
    {
        public List<BaseContent> content { get; set; }
        public string clinic_no { get; set; }
    }

    public abstract class BaseContent
    {
        public string type { get; set; }
    }
    public class TextContent : BaseContent
    {
        public string text { get; set; }
    }
    public class ImageOrAudioContent : BaseContent
    {
        public string file { get; set; }
    }
    public class MetaContent : BaseContent
    {
        public string age { get; set; }
        public string sex { get; set; }
    }
}
