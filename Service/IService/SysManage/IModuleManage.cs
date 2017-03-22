using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IService
{
	public interface IModuleManage : IRepository<SYS_MODULE>
	{
		/// <summary>
		/// 获取用户权限模块集合
		/// 
		/// </summary>
		/// <param name="userId">用户ID</param>
		/// <param name="permission">用户授权集合</param>
		/// <param name="siteId">站点ID</param>
		/// <returns></returns>
		List<Domain.SYS_MODULE> GetModule(int userId, List<Domain.SYS_PERMISSION> permission, string siteId);
		/// <summary>
		/// 递归模块列表，返回按级别排序
		/// add by Sean.He 2016-11-25 10:15:45
		/// </summary>
		List<Domain.SYS_MODULE> RecursiveModule(List<Domain.SYS_MODULE> list);

		/// <summary>
		/// 批量变更当前模块下其他模块的级别
		/// </summary>
		bool MoreModifyModule(int moduleId, int levels);
		/// <summary>
		/// update entity created by Buddy 2016年11月22日19:59:35
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		bool SaveOrUpdate(SYS_MODULE entity);
		/// <summary>
		/// update entityList created by Buddy 2016年11月22日19:59:59
		/// </summary>
		/// <param name="entityList"></param>
		/// <returns></returns>
		bool SaveOrUpdate(List<SYS_MODULE> entityList);
		/// <summary>
		/// delelte by ids created by Sean.He 2016年11月23日14:33:41
		/// </summary>
		/// <param name="ids"></param>
		/// <returns></returns>
		int DeleteList(List<int> ids);
		/// <summary>
		/// 模块名称样式调整
		/// </summary>
		/// <param name="name"></param>
		/// <param name="level"></param>
		/// <returns></returns>
		string GetModuleName(string name, decimal? level);
	}
}
