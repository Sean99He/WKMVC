using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service.ServiceImp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ServiceImp.Tests
{
	[TestClass()]
	public class UserManageTests
	{
		[TestMethod()]
		public void UserLoginTest()
		{
			var aa = new AESCrypt().Encrypt("123456");
			Assert.Fail();
		}
	}
}