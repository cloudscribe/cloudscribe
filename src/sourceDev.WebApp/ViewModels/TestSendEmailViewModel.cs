using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sourceDev.WebApp.ViewModels
{
    public class TestSendEmailViewModel
    {
        
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string ReplyToEmail { get; set; }
        public string ReplyToName { get; set; }
        public string ToEmailCsv { get; set; }
        public string ToAliasCsv { get; set; }
        public string CcEmailCsv { get; set; }
        public string CcAliasCsv { get; set; }
        public string BccEmailCsv { get; set; }
        public string BccAliasCsv { get; set; }

        public string Subject { get; set; }
        public string Message { get; set; }

        public string AttachmentFilePathsCsv { get; set; }
        public string ConfigLookupKey { get; set; }

        
    }
}
