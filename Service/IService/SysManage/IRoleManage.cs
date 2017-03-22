using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IService
{
	public interface IRoleManage : IRepository<SYS_ROLE>
	{
		/// <summary>
		/// 新增或编辑操作 created by Sean.He 2016-11-25 10:43:15
		/// </summary>
		/// <param name="module"></param>
		/// <returns></returns>
		bool SaveOrUpdate(SYS_ROLE module);
		/// <summary>
		/// 删除操作
		/// created by Sean.He 2016-11-25 10:50:33
		/// </summary>
		/// <param name="ids"></param>
		/// <returns></returns>
		int DeleteList(List<int> ids);
	}
}
