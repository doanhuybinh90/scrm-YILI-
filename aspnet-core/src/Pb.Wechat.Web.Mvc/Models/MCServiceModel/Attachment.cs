namespace Pb.Wechat.Web.Models.MCServiceModel
{
    public class Attachment
    {
        private string attNameField;

        private string extNameField;

        private System.Guid gUIDField;

        private System.String uploadTimeField;

        private int fileSizeField;

        private bool isFirstPictureField;


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


        public System.String UploadTime
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
}
