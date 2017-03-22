using Common;
using Domain;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPage.Controllers;

namespace WebPage.Areas.SysManage.Controllers
{
	public class PermissionController : BaseController
	{
		IModuleManage ModuleManage { get; set; }
		IPermissionManage PermissionManage { get; set; }
		Common.log4net.IExtLog log = Common.log4net.ExtLogManager.GetLogger("dblog");
		// GET: SysManage/Permission
		public ActionResult Index(int moduleId)
		{
			ViewData["ModuleId"] = moduleId;
			return View();
		}
		public ActionResult Home()
		{
			ViewData["Systemlist"] = null;
			return View();
		}
		/// <summary>
		/// 获取模块树形菜单
		/// </summary>
		public ActionResult GetTree()
		{
			var json = new JsonHelper() { Msg = "Success", Status = "y" };
			//获取系统ID
			//var sysId = Request.Form["sysId"];
			var sysId = "1";
			//判断系统ID是否传入
			if (string.IsNullOrEmpty(sysId))
			{
				json.Status = "n";
				json.Msg = "获取模块失败！";
				return Json(json);
			}
			try
			{
				//获取系统下的模块列表 按照 SHOWORDER字段 升序排列
				var query = this.ModuleManage.LoadAll(p => p.FK_BELONGSYSTEM == sysId).OrderBy(p => p.SHOWORDER).ToList();
				//这里就是按照jsTree的格式 输出一下 模块信息
				var result = query.Select(m => new
				{
					id = m.ID,
					parent = m.PARENTID > 0 ? m.PARENTID.ToString() : "#",
					text = m.NAME,
					icon = m.LEVELS == 0 ? "fa fa-circle text-danger" : "fa fa-circle text-navy"
				}).ToList();
				json.Data = result;
			}
			catch (Exception e)
			{
				json.Status = "n";
				json.Msg = "服务器忙，请稍后再试！";
				log.Error(Common.Enums.enumOperator.Select + "权限管理，获取模块树：" + e);
			}
			return Json(json);
		}
		/// <summary>
		/// 查询数据
		/// </summary>
		/// <param name="moduleId"></param>
		/// <returns></returns>
		public string GetList(int moduleId)
		{
			var list = PermissionManage.LoadListAll(m => m.MODULEID == moduleId).Select(p => new
			{
				p.ID,
				p.NAME,
				p.PERVALUE,
				p.MODULEID
			});
			var result = JsonConverter.Serialize(list);
			return result;
		}
		/// <summary>
		/// save or update data
		/// </summary>
		/// <returns></returns>
		public int SaveOrUpdate(SYS_PERMISSION module)
		{
			bool result = PermissionManage.SaveOrUpdate(module);
			if (result)
				return 1;
			return 0;
		}
		/// <summary>
		/// delete by ids created by Sean.He 2016-11-24 11:11:02
		/// </summary>
		/// <param name="entityList"></param>
		/// <returns></returns>
		public int Delete(List<SYS_PERMISSION> entityList)
		{
			if (entityList.Count > 0)
			{
				var ids = entityList.Select(m => m.ID).ToList();
				return PermissionManage.DeleteList(ids);
			}
			return 0;
		}
	}
}