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

        [TestCleanup()]
        public void CleanUp()
        {
            // Clean up All Messages
        }


        [TestMethod]
        public void LogEntryTest()
        {
            _logger.LogEntry(Enums.LogType.Information, "Test Message");
        }

        #region Unity
        [TestMethod]
        [TestCategory("Logging")]
        public void ObjectFromUnityTest()
        {
            DependencyInjector.Init();
            ILogger logger;
            logger = DependencyInjector.GetOtherClasses<ILogger>("alogger"
                , "instrumentationKey", "22222", "logError", "true", "logInfo", "true"
                , "logWarning", "true", "logTrack", "true");
            Assert.IsNotNull(logger);
        }

        [TestMethod]
        [TestCategory("Logging")]
        public void ObjectFromUnityTest1()
        {
            DependencyInjector.Init();
            ILogger logger;
            logger = DependencyInjector.GetOtherClasses<ILogger>("flogger"
                , "logName", "22222");
            logger.LogInfo("Test Method");
            Assert.IsNotNull(logger);
        }

        [TestMethod]
        [TestCategory("Logging")]
        public void FileLoggerwithDifferentparameters()
        {
            DependencyInjector.Init();
            ILogger logger;
            logger = DependencyInjector.GetOtherClasses<ILogger>("flogger"
                , "instrumentationKey", "22222", "logError", "true", "logInfo", "true"
                , "logWarning", "true", "logTrack", "true");
            Assert.IsNotNull(logger);
        }


        [TestMethod]
        [TestCategory("Logging")]
        public void FileLoggerwithConfigParameters()
        {
            DependencyInjector.Init();
            ILogger logger;
            IConfiguration configuration;
            configuration = new config();
            logger = DependencyInjector.GetOtherClasses<ILogger>("f1logger"
                , "configuration", configuration);
            Assert.IsNotNull(logger);
        }

        #endregion

    }


    public class config : IConfiguration
    {
        public TType GetAppSettingEntry<TType>(string AppSettingKey)
        {
            return default(TType);
        }
    }
}
