//using cloudscribe.FileManager.Web.Models;
//using System.Collections.Generic;
//using System.Text;

//namespace cloudscribe.FileManager.Web.Services
//{
//    public class FileExtensionValidationRegexBuilder : IFileExtensionValidationRegexBuilder
//    {
//        public string BuildRegex(string pipeSeparatedExtensions)
//        {
//            StringBuilder regex = new StringBuilder();

//            // (([^.;]*[.])+(jpg|gif|png|JPG|GIF|PNG); *)*(([^.;]*[.])+(jpg|gif|png|JPG|GIF|PNG))?$

//            //regex.Append("(([^.;]*[.])+(");
//            regex.Append(@"/(\.|\/)(");
//            //regex.Append(@"*\.(");

//            List<string> allowedExtensions = SplitOnPipes(pipeSeparatedExtensions);
//            string pipe = string.Empty;
//            foreach (string ext in allowedExtensions)
//            {
//                regex.Append(pipe + ext.Replace(".", string.Empty));
//                pipe = "|";
//                regex.Append(pipe + ext.Replace(".", string.Empty).ToUpper());
//            }

//            //regex.Append("); *)*(([^.;]*[.])+(");

//            //pipe = string.Empty;
//            //foreach (string ext in allowedExtensions)
//            //{
//            //    regex.Append(pipe + ext.Replace(".", string.Empty));
//            //    pipe = "|";
//            //    regex.Append(pipe + ext.Replace(".", string.Empty).ToUpper());
//            //}

//            //regex.Append("))?$");
//            regex.Append(")$/i");

//            return regex.ToString();

//        }

//        private List<string> SplitOnPipes(string s)
//        {
//            List<string> list = new List<string>();
//            if (string.IsNullOrEmpty(s)) { return list; }

//            string[] a = s.Split('|');
//            foreach (string item in a)
//            {
//                if (!string.IsNullOrEmpty(item)) { list.Add(item); }
//            }


//            return list;
//        }
//    }
//}
