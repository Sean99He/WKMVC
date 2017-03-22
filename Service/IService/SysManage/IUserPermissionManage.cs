using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IService
{
	public interface IUserPermissionManage:IRepository<SYS_USER_PERMISSION>
	{
		/// <summary>
		/// 设置用户权限
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="newper"></param>
		/// <param name="sysId"></param>
		/// <returns></returns>
		bool SetUserPermission(int userId, string newper, string sysId);
	}
}
