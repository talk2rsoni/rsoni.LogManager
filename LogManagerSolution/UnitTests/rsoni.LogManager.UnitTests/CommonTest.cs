using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using rsoni.Common;

namespace rsoni.LogManager.UnitTests
{
    [TestClass]
    public class CommonTest
    {
        ILogger _logger;
        [TestInitialize]
        public void Initialization()
        {
            _logger = new AppInsightLogger("LogTest");
        }

        [TestCleanup()]
        public void CleanUp()
        {
            // Clean up All Messages
        }


        [TestMethod]
        public void GetConfigEntryTest()
        {
            string configValue = string.Empty;
            configValue = ConfigManager.GetConfigEntry("LogError");

            Assert.IsTrue(configValue.ToLower() == "true");
        }



        [TestMethod]
        public void GetFullExceptionTest()
        {
            string configValue = string.Empty;
            try
            {
                throw new Exception("Test exception ");
            }
            catch (Exception ex)
            {
                configValue = ex.FullException();
                _logger.LogError(configValue);
            }

            Assert.IsTrue(configValue != string.Empty);
        }

    }
}
