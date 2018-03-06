using System.IO;

namespace cloudscribe.Email
{
    public static class StreamExtensions
    {
        public static byte[] ToByteArray(this Stream input)
        {
            if (input is MemoryStream)
            {
                return ((MemoryStream)input).ToArray();
            }

            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
