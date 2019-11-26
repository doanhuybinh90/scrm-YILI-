using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.MultiTenancy;
using Microsoft.AspNetCore.Mvc;
using Pb.Wechat.Authorization;
using Pb.Wechat.Web.Controllers;

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize]
    public class HomeController : AbpZeroTemplateControllerBase
    {
        public async Task<ActionResult> Index()
        {
            if (AbpSession.MultiTenancySide == MultiTenancySides.Host)
            {
                if (await IsGrantedAsync(AppPermissions.Pages_Administration_Host_Dashboard))
                {
                   
                    //return RedirectToAction("Index", "HostDashboard");
                    return RedirectToAction("Index", "MpMediaImages");
                }

                //if (await IsGrantedAsync(AppPermissions.Pages_Tenants))
                //{
                //    //return RedirectToAction("Index", "Tenants");
                //    return RedirectToAction("Index", "MpMediaImages");
                //}
            }
            //else
            //{
            //    if (await IsGrantedAsync(AppPermissions.Pages_Tenant_Dashboard))
            //    {
            //        //return RedirectToAction("Index", "Dashboard");
            //        return RedirectToAction("Index", "MpMediaImages");
             
            //    }
            //}

            //Default page if no permission to the pages above
            return RedirectToAction("Index", "Welcome");
        }
    }
}