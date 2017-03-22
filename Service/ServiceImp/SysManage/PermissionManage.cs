using Domain;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ServiceImp
{
	public class PermissionManage : RepositoryBase<SYS_PERMISSION>, IPermissionManage
	{
		/// <summary>
		/// 根据系统ID获取所有模块的权限ID集合
		/// </summary>
		public List<int> GetPermissionIdBySysId(string sysId)
		{
			try
			{
				string sql = "select p.id from sys_permission p where exists(select 1 from sys_module t where t.fk_belongsystem=@sysid and t.id=p.moduleid)";
				DbParameter para = new SqlParameter("@sysid", sysId);
				return this.SelectBySql<int>(sql, para);
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		/// <summary>
		/// create or update data created by Sean.He 2016年11月24日10:47:28
		/// </summary>
		/// <param name="module"></param>
		/// <returns></returns>
		public bool SaveOrUpdate(SYS_PERMISSION module)
		{
			if (module.ID == 0)
				return this.SaveOrUpdate(module, false);
			return this.SaveOrUpdate(module, true);
		}
		/// <summary>
		/// delete by ids created by Sean.He 2016年11月24日11:08:27
		/// </summary>
		/// <param name="ids"></param>
		/// <returns></returns>
		public int DeleteList(List<int> ids)
		{
			return this.Delete(m => ids.Contains(m.ID));
		}
	}
}
