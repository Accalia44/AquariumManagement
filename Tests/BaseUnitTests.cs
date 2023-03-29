using Utils;
using Serilog;

namespace Tests
{
public class BaseUnitTests
    {
        protected ILogger log = Logger.ContextLog<BaseUnitTests>();

        [OneTimeSetUp]
        public void Setup()
        //async entfernt 
        {
            Logger.InitLogger();
        }

        [Test]
        public void MyFirstLog()
        {
            log.Information("My first try");
            Assert.IsTrue(true);
        }
    }

}