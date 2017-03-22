using Common;
using Common.Enums;
using Common.log4net;
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
	public class RoleController : BaseController
	{
		IExtLog log = ExtLogManager.GetLogger("dblog");
		IModuleManage ModuleManage { get; set; }
		IPermissionManage PermissionManage { get; set; }
		IRolePermissionManage RolePermissionManage { get; set; }
		IRoleManage RoleManage { get; set; }
		public ActionResult Index()
		{
			return View();
		}
		/// <summary>
		/// 获取查询数据
		/// </summary>
		/// <returns></returns>
		public string GetList()
		{
			try
			{
				var entity = this.RoleManage.LoadAll(null).Select(p => new
				{
					p.ID,
					p.ROLENAME,
					p.ISCUSTOM
				});
				var result = JsonConverter.Serialize(entity);
				return result;
			}
			catch (Exception e)
			{
				log.Info(Common.Enums.enumOperator.Select + "加载角色列表：" + e);
				throw e.InnerException;
			}
		}
		/// <summary>
		/// 角色对应的权限信息
		/// </summary>
		/// <param name="RoleId"></param>
		/// <returns></returns>
		public ActionResult PerAllocation(int RoleId)
		{
			ViewData["RoleId"] = RoleId;//保存角色ID
			var moduleList = new List<Domain.SYS_MODULE>();
			var Role = RoleManage.Get(p => p.ID == RoleId);
			//获取角色所属系统模块
			moduleList = this.ModuleManage.RecursiveModule(this.ModuleManage.LoadAll(p => p.FK_BELONGSYSTEM == "1").ToList());
			ViewData["ModuleList"] = JsonConverter.JsonClass(moduleList.Select(p => new { p.ID, MODULENAME = ModuleManage.GetModuleName(p.NAME, p.LEVELS), p.ICON, p.PARENTID, p.LEVELS }));
			//获取权限
			var moduleId = moduleList.Select(p => p.ID).ToList();
			ViewData["PermissionList"] = this.PermissionManage.LoadAll(p => moduleId.Any(e => e == p.MODULEID)).ToList();
			//根据类型获取用户/角色已选中的权限
			var selectper = new List<string>();
			selectper =
				this.RolePermissionManage.LoadAll(p => p.ROLEID == RoleId)
					.Select(p => p.PERMISSIONID)
					.Cast<string>()
					.ToList();
			ViewData["selectper"] = selectper;
			return View();
		}

		/// <summary>
		/// 设置角色、用户权限
		/// </summary>
		public ActionResult SaveAllocation()
		{
			var json = new JsonHelper() { Msg = "分配权限完毕", Status = "y" };
			string roleId = Request.Form["roleId"];
			string perid = Request.Form["perid"];
			if (!this.RolePermissionManage.SetRolePermission(int.Parse(roleId), perid, "1"))
			{
				json.Msg = "保存失败";
				json.Status = "n";
				log.Info(Common.Enums.enumOperator.Allocation + "设置角色权限，结果：" + json.Msg + Common.Enums.enumLog4net.ERROR);
				return Json(json);
			}
			return Json(json);
		}
		/// <summary>
		/// 新增和编辑操作
		/// </summary>
		public int SaveOrUpdate(SYS_ROLE module)
		{
			if (RoleManage.SaveOrUpdate(module))
				return 1;
			return 0;
		}
		/// <summary>
		/// 删除操作
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public int Delete(List<SYS_ROLE> entityList)
		{
			//不能删除管理员权限
			if (entityList.Where(m => m.ID == ClsDic.DicRole["超级管理员"]).Count() > 0)
				return 0;
			var ids = entityList.Select(m => m.ID).ToList();
			return this.RoleManage.DeleteList(ids);
		}
	}
}