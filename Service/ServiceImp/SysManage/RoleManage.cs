using Domain;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ServiceImp
{
	public class RoleManage : RepositoryBase<SYS_ROLE>, IRoleManage
	{
		/// <summary>
		/// 删除操作
		/// created by Sean.He 2016-11-25 10:51:13
		/// </summary>
		/// <param name="ids"></param>
		/// <returns></returns>
		public int DeleteList(List<int> ids)
		{
			return this.Delete(m => ids.Contains(m.ID));
		}

		/// <summary>
		/// 新增或编辑操作
		/// created by Sean.He 2016-11-25 10:43:56
		/// </summary>
		/// <param name="module"></param>
		/// <returns></returns>
		public bool SaveOrUpdate(SYS_ROLE module)
		{
			bool edit = module.ID != 0;
			return this.SaveOrUpdate(module, edit);
		}
	}
}
