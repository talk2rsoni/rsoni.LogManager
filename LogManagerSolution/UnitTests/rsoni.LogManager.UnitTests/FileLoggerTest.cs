using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using rsoni.LogManager.Common;

namespace rsoni.LogManager.UnitTests
{
    [TestClass]
    public class FileLoggerTest
    {
        ILogger _logger;
        [TestInitialize]
        public void Initialization()
        {
            _logger = new FileLogger("LogTest");
        }


        [TestMethod]
        public void LogEntryTest()
        {
            _logger.LogEntry(Enums.LogType.Information, "Test Message");
        }
    }
}
