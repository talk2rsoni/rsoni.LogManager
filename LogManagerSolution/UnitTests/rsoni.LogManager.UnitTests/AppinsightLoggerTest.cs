using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using rsoni.LogManager.Common;

namespace rsoni.LogManager.UnitTests
{
    [TestClass]
    public class AppinsightLoggerTest
    {
        ILogger _logger;
        [TestInitialize]
        public void Initialization()
        {
            _logger = new AppInsightLogger("LogTest");
        }


        [TestMethod]
        public void LogEntryTest()
        {
            _logger.LogEntry(Enums.LogType.Information, "Test Message");
        }

        [TestMethod]
        public void LogErrorTest()
        {
            _logger.LogError("Test Error");
        }

        [TestMethod]
        public void LogExceptionTest()
        {
            _logger.LogError(new Exception("Test Log Exception"));
        }
    }
}
