using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

using Petecat.Configuration;

namespace Petecat.Test.Configuration
{
    [TestClass]
    public class FileConfigurationManagerTest
    {
        [TestMethod]
        public void ReadXml()
        {
            var manager = new FileConfigurationManagerBase(true);
            try
            {
                manager.LoadFromXml<AppSettings>("AppSettings.config", "AppSettings");
            }
            catch (Exception)
            {
                Assert.Fail();
                return;
            }

            var appSettings = manager.Get<AppSettings>("AppSettings", null);
            Assert.IsNotNull(appSettings);
        }

        [TestMethod]
        public void ReadIni()
        {
            var manager = new FileConfigurationManagerBase(true);
            try
            {
                manager.LoadFromIni<HttpsConfig>("AppSettings.ini", "HttpsConfig");
            }
            catch (Exception)
            {
                Assert.Fail();
                return;
            }

            var httpsConfig = manager.Get<HttpsConfig>("HttpsConfig", null);
            Assert.IsNotNull(httpsConfig);
        }
    }
}