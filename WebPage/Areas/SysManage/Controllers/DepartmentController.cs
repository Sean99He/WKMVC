using Common;
using Domain;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebPage.Areas.SysManage.Controllers
{
	public class DepartmentController : Controller
	{
		IDepartmentManage DepartmentManage { get; set; }
		// GET: SysManage/Default
		public ActionResult Index()
		{
			ViewData["dicPDepartment"] = this.DepartmentManage.GetFirstLevelDepartment();
			return View();
		}
		/// <summary>
		/// 获取数据
		/// </summary>
		/// <returns></returns>
		public string GetList()
		{
			var entity = this.DepartmentManage.SortDepartmentList(this.DepartmentManage.LoadListAll(null)).Select(p => new
			{
				p.ID,
				p.CODE,
				NAME = this.DepartmentManage.GetDepartmentName(p.NAME, p.BUSINESSLEVEL),
				p.SHOWORDER,
				p.PARENTID,
				p.BUSINESSLEVEL
			});
			var result = JsonConverter.Serialize(entity);
			return result;
		}
		/// <summary>
		/// 新增或者编辑
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public int SaveOrUpdate(SYS_DEPARTMENT model)
		{
			if (this.DepartmentManage.SaveOrUpdate(model))
				return 1;
			return 0;
		}
		/// <summary>
		/// 删除操作
		/// </summary>
		/// <param name="entityList"></param>
		/// <returns></returns>
		[HttpPost]
		public int Delete(List<SYS_DEPARTMENT> entityList)
		{
			var ids = entityList.Select(m => m.ID).ToList();
			return this.DepartmentManage.DeleteByList(ids);
		}
	}
}