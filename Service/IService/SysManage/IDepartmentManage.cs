using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IService
{
	public interface IDepartmentManage : IRepository<SYS_DEPARTMENT>
	{
		/// <summary>
		/// 递归部门列表，返回按级别排序
		/// add yuangang by 2016-05-19
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		List<Domain.SYS_DEPARTMENT> RecursiveDepartment(List<Domain.SYS_DEPARTMENT> list);

		/// <summary>
		/// 根据部门ID递归部门列表，返回子部门+本部门的对象集合
		/// add yuangang by 2016-05-19
		/// </summary>
		List<Domain.SYS_DEPARTMENT> RecursiveDepartment(string parentId);
		/// <summary>
		/// 自动创建部门编号
		/// add yuangang by 2016-05-19
		/// </summary>
		string CreateCode(string parentCode);

		/// <summary>
		/// 部门是否存在下级部门
		/// add huafg by 2015-06-03
		/// </summary>
		bool DepartmentIsExists(string idlist);

		/// <summary>
		/// 根据部门ID获取部门名称，不存在返回空
		/// </summary>
		string GetDepartmentName(string id);
		/// <summary>
		/// 显示错层方法
		/// </summary>
		object GetDepartmentName(string name, decimal? level);
		/// <summary>
		/// 获取部门父级列表
		/// </summary>
		System.Collections.IList GetDepartmentByDetail();
		/// <summary>
		/// 编辑或者删除操作 created by Sean.He
		/// </summary>
		/// <param name="module"></param>
		/// <returns></returns>
		bool SaveOrUpdate(SYS_DEPARTMENT module);
		/// <summary>
		/// 批量删除 created by Sean.He
		/// </summary>
		/// <param name="ids"></param>
		/// <returns></returns>
		int DeleteByList(List<string> ids);
		/// <summary>
		/// created by Sean.He
		/// </summary>
		/// <returns></returns>
		string GetFirstLevelDepartment();
		/// <summary>
		/// 根据部门级别排序 created by Sean.He 2016-11-28 17:00:07
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		List<SYS_DEPARTMENT> SortDepartmentList(List<SYS_DEPARTMENT> list);
	}
}
