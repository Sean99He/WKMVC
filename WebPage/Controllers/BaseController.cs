using Common;
using Service;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebPage.Controllers
{
	public class BaseController : Controller
	{
		#region 公用变量
		/// <summary>
		/// 查询关键词
		/// </summary>
		public string keywords { get; set; }
		/// <summary>
		/// 视图传递的分页页码
		/// </summary>
		public int page { get; set; }
		/// <summary>
		/// 视图传递的分页条数
		/// </summary>
		public int pagesize { get; set; }
		/// <summary>
		/// 用户容器，公用
		/// </summary>
		public IUserManage UserManage = Spring.Context.Support.ContextRegistry.GetContext().GetObject("Service.User") as IUserManage;
		#endregion
		#region 用户对象
		/// <summary>
		/// 获取当前用户对象
		/// </summary>
		public Account CurrentUser
		{
			get
			{
				//从Session中获取用户对象
				if (SessionHelper.GetSession("CurrentUser") != null)
				{
					return SessionHelper.GetSession("CurrentUser") as Account;
				}
				//Session过期 通过Cookies中的信息 重新获取用户对象 并存储于Session中
				var account = UserManage.GetAccountByCookie();
				SessionHelper.SetSession("CurrentUser", account);
				return account;
			}
		}
		#endregion
		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (filterContext.HttpContext.Session == null)
			{
				filterContext.HttpContext.Response.Write("<script type='text/javascript'> alert('~登录已过期，请重新登录');window.top.location='/';</script>");
				filterContext.HttpContext.Response.End();
				filterContext.Result = new EmptyResult();
				return;
			}
			if (this.CurrentUser == null)
			{
				filterContext.HttpContext.Response.Write("<script type='text/javascript'> alert('~登录已过期，请重新登录');window.top.location='/';</script>");
				filterContext.HttpContext.Response.End();
				filterContext.Result = new EmptyResult();
				return;
			}
			#region 公共Get变量
			//分页页码
			object p = filterContext.HttpContext.Request["page"];
			if (p == null || p.ToString() == "") { page = 1; } else { page = int.Parse(p.ToString()); }

			//搜索关键词
			string search = filterContext.HttpContext.Request.QueryString["Search"];
			if (!string.IsNullOrEmpty(search)) { keywords = search; }
			//显示分页条数
			string size = filterContext.HttpContext.Request.QueryString["example_length"];
			if (!string.IsNullOrEmpty(size) && System.Text.RegularExpressions.Regex.IsMatch(size.ToString(), @"^\d+$")) { pagesize = int.Parse(size.ToString()); } else { pagesize = 10; }
			#endregion
		}
	}
}