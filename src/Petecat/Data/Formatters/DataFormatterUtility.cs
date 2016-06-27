using System;
using System.Collections.Generic;

namespace Petecat.Data.Formatters
{
    public static class DataFormatterUtility
    {
        private static Dictionary<DataFormatterContent, IDataFormatter> _FormatterMappings = null;

        static DataFormatterUtility()
        {
            _FormatterMappings = new Dictionary<DataFormatterContent, IDataFormatter>();
            _FormatterMappings.Add(DataFormatterContent.DataContractXml, new DataContractXmlFormatter());
            _FormatterMappings.Add(DataFormatterContent.DataContractJson, new DataContractJsonFormatter());
            _FormatterMappings.Add(DataFormatterContent.Xml, new XmlFormatter());
        }

        public static IDataFormatter Get(DataFormatterContent formatterContentType)
        {
            if (!_FormatterMappings.ContainsKey(formatterContentType))
            {
                throw new Errors.FormatterNotFoundException(formatterContentType.ToString());
            }

            return _FormatterMappings[formatterContentType];
        }
    }
}
