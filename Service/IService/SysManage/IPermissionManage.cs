using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IService
{
	public interface IPermissionManage : IRepository<SYS_PERMISSION>
	{
		/// <summary>
		/// 根据系统ID获取所有模块的权限ID集合
		/// </summary>
		List<int> GetPermissionIdBySysId(string sysId);
		/// <summary>
		/// create or update data created by Sean.He 2016年11月24日10:48:18
		/// </summary>
		/// <param name="module"></param>
		/// <returns></returns>
		bool SaveOrUpdate(SYS_PERMISSION module);
		/// <summary>
		/// delete by ids created by Sean.He 2016-11-24 11:09:08
		/// </summary>
		/// <param name="ids"></param>
		/// <returns></returns>
		int DeleteList(List<int> ids);
	}
}
