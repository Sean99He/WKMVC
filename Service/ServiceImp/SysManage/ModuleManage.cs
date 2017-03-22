using Domain;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ServiceImp
{
	public class ModuleManage : RepositoryBase<SYS_MODULE>, IModuleManage
	{
		/// <summary>
		/// 获取用户权限模块集合
		/// 
		/// </summary>
		/// <param name="userId">用户ID</param>
		/// <param name="permission">用户授权集合</param>
		/// <param name="siteId">站点ID</param>
		/// <returns></returns>
		public List<Domain.SYS_MODULE> GetModule(int userId, List<Domain.SYS_PERMISSION> permission, string siteId)
		{
			//返回模块
			var retmodule = new List<Domain.SYS_MODULE>();
			var permodule = new List<Domain.SYS_MODULE>();
			//权限转模块
			if (permission != null)
			{
				permodule.AddRange(permission.Select(p => p.SYS_MODULE));
				//去重
				permodule = permodule.Distinct(new ModuleDistinct()).ToList();
			}
			//检索显示与系统
			//permodule = permodule.Where(p => p.ISSHOW == 1 && p.FK_BELONGSYSTEM.ToString() == siteId).ToList();
			//商城系统融入本系统不再区分系统
			permodule = permodule.Where(p => p.ISSHOW == 1).ToList();
			//构造上级导航模块
			//var prevModule = this.LoadListAll(p => p.FK_BELONGSYSTEM.ToString() == siteId);
			//商城系统融入本系统不再区分系统
			var prevModule = this.LoadListAll(null);
			//反向递归算法构造模块带上级上上级模块
			if (permodule.Count > 0)
			{
				foreach (var item in permodule)
				{
					RecursiveModule(prevModule, retmodule, item.PARENTID);
					retmodule.Add(item);
				}
			}
			//去重
			retmodule = retmodule.Distinct(new ModuleDistinct()).ToList();
			//返回模块集合
			return retmodule.OrderBy(p => p.LEVELS).ThenBy(p => p.SHOWORDER).ToList();
		}

		/// <summary>
		/// 反向递归模块集合，可重复模块数据，最后去重
		/// </summary>
		/// <param name="PrevModule">总模块</param>
		/// <param name="retmodule">返回模块</param>
		/// <param name="parentId">上级ID</param>
		private void RecursiveModule(List<Domain.SYS_MODULE> PrevModule, List<Domain.SYS_MODULE> retmodule, int? parentId)
		{
			var result = PrevModule.Where(p => p.ID == parentId);
			if (result != null)
			{
				foreach (var item in result)
				{
					retmodule.Add(item);
					RecursiveModule(PrevModule, retmodule, item.PARENTID);
				}
			}
		}

		/// <summary>
		/// 递归模块列表，返回按级别排序
		/// 
		/// </summary>
		public List<Domain.SYS_MODULE> RecursiveModule(List<Domain.SYS_MODULE> list)
		{
			List<Domain.SYS_MODULE> result = new List<Domain.SYS_MODULE>();
			if (list != null && list.Count > 0)
			{
				ChildModule(list, result, 0);
			}
			return result;
		}
		/// <summary>
		/// 递归模块列表
		/// 
		/// </summary>
		private void ChildModule(List<Domain.SYS_MODULE> list, List<Domain.SYS_MODULE> newlist, int parentId)
		{
			var result = list.Where(p => p.PARENTID == parentId).OrderBy(p => p.LEVELS).OrderBy(p => p.SHOWORDER).ToList();
			if (result.Count() > 0)
			{
				for (int i = 0; i < result.Count(); i++)
				{
					newlist.Add(result[i]);
					ChildModule(list, newlist, result[i].ID);
				}
			}
		}

		/// <summary>
		/// 批量变更下级模块的级别
		/// </summary>
		public bool MoreModifyModule(int moduleId, int levels)
		{
			//根据当前模块ID获取下级模块的集合
			var ChildModule = this.LoadAll(p => p.PARENTID == moduleId).ToList();
			if (ChildModule.Any())
			{
				foreach (var item in ChildModule)
				{
					item.LEVELS = levels + 1;
					this.Update(item);
					MoreModifyModule(item.ID, item.LEVELS);
				}
			}
			return true;
		}

		/// <summary>
		/// 获取模板列表
		/// </summary>
		public dynamic LoadModuleInfo(int id)
		{
			return Common.JsonConverter.JsonClass(this.LoadAll(p => p.PARENTID == id).OrderBy(p => p.ID).Select(p => new { p.ID, p.NAME }).ToList());
		}
		/// <summary>
		/// update entity create by Buddy 2016年11月22日19:58:11
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public bool SaveOrUpdate(SYS_MODULE entity)
		{
			entity.NAME = entity.NAME.Replace("&nbsp;", "").Replace("|--", "").Trim();
			if (entity.ID == 0)
				return base.SaveOrUpdate(entity, false);
			else {
				return base.SaveOrUpdate(entity, true);
			}
		}
		/// <summary>
		/// update entityList created by Buddy 2016年11月22日19:58:31
		/// </summary>
		/// <param name="entityList"></param>
		/// <returns></returns>
		public bool SaveOrUpdate(List<SYS_MODULE> entityList)
		{
			var flag = true;
			foreach (var item in entityList)
			{
				flag = SaveOrUpdate(item);
				if (!flag)
					return flag;
			}
			return flag;
		}
		/// <summary>
		/// 根据ID删除 create by Sean.He
		/// </summary>
		/// <param name="ids"></param>
		public int DeleteList(List<int> ids)
		{
			return this.Delete(m => ids.Contains(m.ID));
		}
		/// <summary>
		/// 模块名称样式调整
		/// </summary>
		/// <param name="name"></param>
		/// <param name="level"></param>
		/// <returns></returns>
		public string GetModuleName(string name, decimal? level)
		{
			if (level > 0)
			{
				string nbsp = "&nbsp;&nbsp;";
				for (int i = 0; i < level; i++)
				{
					nbsp += "&nbsp;&nbsp;";
				}
				name = nbsp + "|--" + name;
			}
			return name;
		}
	}
	/// <summary>
	/// 模型去重，非常重要
	/// 
	/// </summary>
	public class ModuleDistinct : IEqualityComparer<Domain.SYS_MODULE>
	{
		public bool Equals(Domain.SYS_MODULE x, Domain.SYS_MODULE y)
		{
			return x.ID == y.ID;
		}

		public int GetHashCode(Domain.SYS_MODULE obj)
		{
			return obj.ToString().GetHashCode();
		}
	}
}
