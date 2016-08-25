namespace Petecat.Data.Formatters
{
    public static class ObjectFormatterFactory
    {
        public static IObjectFormatter GetFormatter(ObjectFormatterType objectFormatterType)
        {
            switch (objectFormatterType)
            {
                case ObjectFormatterType.DataContractXml: return new DataContractJsonFormatter();
                case ObjectFormatterType.DataContractJson: return new DataContractJsonFormatter();
                case ObjectFormatterType.Xml: return new XmlFormatter();
                case ObjectFormatterType.Binary: return new BinaryFormatter();
                case ObjectFormatterType.Ini: return new IniFormatter();
                default: return null;
            }
        }
    }
}
