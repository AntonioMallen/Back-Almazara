using AutoMapper;

namespace Back_Almazara.Utility
{
    public class TypeConverter
    {

        public class StringToByteArrayConverter : ITypeConverter<string, byte[]>
        {
            public byte[] Convert(string source, byte[] destination, ResolutionContext context)
            {
                return string.IsNullOrEmpty(source ?? "") ? new byte[0] : System.Convert.FromBase64String(source);
            }
        }

        public class ByteArrayToStringConverter : ITypeConverter<byte[], string>
        {
            public string Convert(byte[] source, string destination, ResolutionContext context)
            {
                return source == null ? "" : System.Convert.ToBase64String(source);
            }
        }
}
}
