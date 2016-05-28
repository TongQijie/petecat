using Microsoft.VisualStudio.TestTools.UnitTesting;
using Petecat.Data.Ini;
using System;
using System.Linq;
using System.Text;

namespace Petecat.Test.Data.Ini
{
    [TestClass]
    public class StringFormatterTest
    {
        [TestMethod]
        public void ConvertFromFile()
        {
            var elements = StringFormatter.ConvertFromFile("AppSettings.ini", Encoding.UTF8);

            var httpsConfigElement = elements.FirstOrDefault(x => x.Key.Equals("httpsConfig", StringComparison.OrdinalIgnoreCase));
            var httpsConfig = httpsConfigElement.ReadObject<Configuration.HttpsConfig>();

            var appSettingsElement = elements.FirstOrDefault(x => x.Key.Equals("appSettings", StringComparison.OrdinalIgnoreCase));
            var appSettings = appSettingsElement.ReadObject<Configuration.AppSettings>();
        }
    }
}
