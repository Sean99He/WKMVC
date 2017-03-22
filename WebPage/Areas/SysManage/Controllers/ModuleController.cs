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
	public class ModuleController : BaseController
	{
		IModuleManage ModuleManage { get; set; }
		public ActionResult Index()
		{
			return View();
		}
		/// <summary>
		/// 获取数据
		/// </summary>
		/// <returns></returns>
		public string GetList()
		{
			var entity = ModuleManage.RecursiveModule(ModuleManage.LoadListAll(null)).Select(p => new
			{
				p.ID,
				NAME =ModuleManage.GetModuleName(p.NAME, p.LEVELS),
				p.ALIAS,
				p.MODULEPATH,
				p.SHOWORDER,
				p.ISSHOW,
				p.PARENTID
			});
			var result = JsonConverter.Serialize(entity);
			return result;
		}		
		[HttpPost]
		public bool SaveOrUpdate(string data)
		{
			var list = JsonConverter.Deserialize<List<SYS_MODULE>>(data);
			return ModuleManage.SaveOrUpdate(list);
		}
		/// <summary>
		/// 编辑和新增
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public int SaveOrUpdate(SYS_MODULE model)
		{
			if (model.MODULETYPE == 0)
			{
				model.FK_BELONGSYSTEM = "1";
				model.MODULETYPE = 2;
				model.LEVELS = 2;
				model.IsVillage = false;
			}
			var result = ModuleManage.SaveOrUpdate(model);
			if (result)
				return 1;
			return 0;
		}
		[HttpPost]
		public int Delete(List<SYS_MODULE> entityList)
		{
			if (entityList.Count > 0)
			{
				List<int> ids = entityList.Select(m => m.ID).ToList();
				return ModuleManage.DeleteList(ids);
			}
			return 0;
		}
	}
}