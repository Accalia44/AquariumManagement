using System;
using DAL;

namespace Tests.DBTests
{
	public class DBConnectionTest : BaseUnitTests
	{
		[Test]
		public async Task TestDBConnection()
		{
			UnitOfWork uow = new UnitOfWork();

			Assert.IsTrue(uow.Context.IsConnected);
		}
	}
}

