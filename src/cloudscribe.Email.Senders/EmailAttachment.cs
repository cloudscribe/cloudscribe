using System;
using System.IO;

namespace cloudscribe.Email
{
    public class EmailAttachment
    {
        public EmailAttachment(Stream stream, string fileName)
        {
            Stream = stream ?? throw new ArgumentNullException(nameof(stream));
            FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
        }

        public Stream Stream { get; private set; }
        public string FileName { get; private set; }
    }
}
