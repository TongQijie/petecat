using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

using Petecat.Configuration;

namespace Petecat.Test.Configuration
{
    [TestClass]
    public class RuntimeConfigurationManagerTest
    {
        [TestMethod]
        public void SetAndGet()
        {
            var appSettings = new AppSettings();
            appSettings.EnableHttps = true;
            appSettings.HttpsConfig = new HttpsConfig() { Url = "https://github.com/tongqijie", Port = 10000 };

            var manager = new RuntimeConfigurationManagerBase();
            manager.Set("AppSettings", appSettings, null);

            var got = manager.Get<AppSettings>("AppSettings", null);
            Assert.IsNotNull(got);
        }
    }
}