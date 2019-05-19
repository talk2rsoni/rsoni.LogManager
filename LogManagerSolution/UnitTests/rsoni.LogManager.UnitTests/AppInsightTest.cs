using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using rsoni.LogManager.Common;

namespace rsoni.LogManager.UnitTests
{
    [TestClass]
    public class AppInsightTest
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
    }
}
