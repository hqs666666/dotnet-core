using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Core.Base.DTOS
{
    public class Email
    {
        public List<string> Receiver { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public byte[] Attachments { get; set; }
    }
}
