using Common;
using Common.log4net;
using Service.IService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebPage.Areas.SysManage.Controllers
{
	public class AccountController : Controller
	{
		#region 声明容器
		/// <summary>
		/// 用户管理
		/// 
		/// </summary>
		IUserManage UserManage { get; set; }
		/// <summary>
		/// 日志记录
		/// </summary>
		IExtLog log = ExtLogManager.GetLogger("dblog");
		#endregion

		#region 基本视图
		public ActionResult Index()
		{
			return View();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		[ValidateAntiForgeryToken]
		public ActionResult Login(Domain.SYS_USER item)
		{
			var json = new JsonHelper() { Msg = "登录成功", Status = "n" };
			var code = Request.Form["code"];
			if (Session["gif"] != null)
			{
				if (!string.IsNullOrEmpty(code) && code.ToLower() == Session["gif"].ToString().ToLower())
				{
					var user = this.UserManage.UserLogin(item.ACCOUNT.Trim(), item.PASSWORD.Trim());
					if (user != null)
					{
						//是否锁定
						if (user.ISCANLOGIN == 1)
						{
							json.Msg = "账户已经被锁定";
							log.Error(Utils.GetIP(), item.ACCOUNT, Request.Url.ToString(), "Login", "系统登录，登录结果：" + json.Msg);
						}
						else
						{
							var account = this.UserManage.GetAccountByUser(user);
							//把当前用户信息写入Session
							SessionHelper.SetSession("CurrentUser", account);
							//记录用户信息到cookie
							string cookievalue = "{\"id\":\"" + account.Id + "\",\"username\":\"" + account.LogName + "\",\"password\":\"" + account.PassWord + "\",\"Token\":\"" + Session.SessionID + "\"}";
							CookieHelper.SetCookie("cookie_rememberme", cookievalue, null);
							//更新用户最后一次登录ip
							user.LastLoginIP = Utils.GetIP();
							this.UserManage.Update(user);
							json.Status = "y";
							json.ReUrl = "/SysManage/Home/Index";
							//暂时注释掉登录成功日志
							//log.Info(Utils.GetIP(), item.ACCOUNT, Request.Url.ToString(), "Login", "系统登录，登录结果：" + json.Msg);
						}
					}
					else
					{
						json.Msg = "用户名或密码不正确";
						log.Error(Utils.GetIP(), item.ACCOUNT, Request.Url.ToString(), "Login", "系统登录，登录结果：" + json.Msg);
					}
				}
				else
				{
					json.Msg = "验证码不正确";
					log.Error(Utils.GetIP(), item.ACCOUNT, Request.Url.ToString(), "Login", "系统登录，登录结果：" + json.Msg);
				}
			}
			else
			{
				json.Msg = "验证码已过期，请刷新验证码";
				log.Error(Utils.GetIP(), item.ACCOUNT, Request.Url.ToString(), "Login", "系统登录，登录结果：" + json.Msg);
			}
			return Json(json, JsonRequestBehavior.AllowGet);
		}
		#endregion

		#region 帮助方法
		/// <summary>
		/// 验证码
		/// </summary>
		/// <returns></returns>
		public FileContentResult ValidateCode()
		{
			string code = string.Empty;
			MemoryStream ms = new VerifyCode().Create(out code);
			Session["gif"] = code;
			Response.ClearContent();
			return File(ms.ToArray(), @"image/png");
		}
		#endregion
	}
}