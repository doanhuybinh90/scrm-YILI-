namespace Pb.Wechat.MpMessages.Dto
{
    public class MessageResponseModel
    {
        public string TaskID { get; set; }
        public int Count { get; set; }

        public string MessageType { get; set; }

        public string Content { get; set; }
        public int? ArticleID { get; set; }

        public string ArticleName { get; set; }

        public string ArticleMediaID { get; set; }
        public int? ArticleGroupID { get; set; }

        public string ArticleGroupName { get; set; }

        public string ArticleGroupMediaID { get; set; }
        public int? ImageID { get; set; }

        public string ImageName { get; set; }

        public string ImageMediaID { get; set; }
        public int? VideoID { get; set; }

        public string VideoName { get; set; }

        public string VideoMediaID { get; set; }
        public int? VoiceID { get; set; }

        public string VoiceName { get; set; }

        public string VoiceMediaID { get; set; }
        public string[] OpenIDs { get; set; }
    }
}
