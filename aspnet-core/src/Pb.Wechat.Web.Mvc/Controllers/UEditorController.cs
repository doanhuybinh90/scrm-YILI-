using Microsoft.AspNetCore.Mvc;
using UEditorNetCore;
using Abp.Web.Models;

namespace Pb.Wechat.Web.Controllers
{
    [DontWrapResult]
    [Route("api/[controller]")]
    public class UEditorController : Controller
    {
        private UEditorService ue;
        public UEditorController(UEditorService ue)
        {
            this.ue = ue;
        }


        public void Do()
        {
            Response.Redirect("http://www.baidu.com");
            //ue.DoAction(HttpContext);
        }
    }
}
