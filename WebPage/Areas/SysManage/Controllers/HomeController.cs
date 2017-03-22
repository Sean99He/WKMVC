using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPage.Controllers;

namespace WebPage.Areas.SysManage.Controllers
{
	public class HomeController : BaseController
	{
		IModuleManage ModuleManage { get; set; }
		public ActionResult Index()
		{
			ViewData["Module"] = ModuleManage.GetModule(this.CurrentUser.Id, this.CurrentUser.Permissions, this.CurrentUser.System_Id);
			return View(this.CurrentUser);
		}
	}
}