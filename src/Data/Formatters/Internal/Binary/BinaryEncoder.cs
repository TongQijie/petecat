namespace Petecat.Data.Formatters.Internal.Binary
{
    internal class BinaryEncoder
    {
        public const byte ObjectHeader = 0xFF;

        public const byte CollectionHeader = 0xFE;

        public const byte PropertyNameHeader = 0xF1;

        public const byte PropertyValueHeader = 0xF0;

        public const int PropertyNameMaxLength = 128;

        public const int PropertyValueMaxLength = 0xFFFF;


    }
}
