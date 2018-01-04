using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace cloudscribe.Web.Common.TagHelpers
{
    [HtmlTargetElement("head")]
    [HtmlTargetElement("body")]
    [HtmlTargetElement("footer")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class CompositeTagHelper : TagHelper
    {
        public CompositeTagHelper(
            IEnumerable<ICompositeContentProvider> contentProviders,
            IHtmlHelper htmlHelper
            )
        {
            _contentProviders = contentProviders;
            _htmlHelper = htmlHelper;
        }

        private IEnumerable<ICompositeContentProvider> _contentProviders;
        private readonly IHtmlHelper _htmlHelper;
        
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var completed = new List<string>();

            foreach (var processor in _contentProviders)
            {
                if (!completed.Contains(processor.Name))
                {
                    if (processor.TargetTagName == output.TagName)
                    {
                        await processor.ProcessAsync(context, output, ViewContext);
                        completed.Add(processor.Name);
                    }
                }

            }

        }

    }

    public interface ICompositeContentProvider
    {
        string Name { get; }
        string TargetTagName { get; }
        Task ProcessAsync(TagHelperContext context, TagHelperOutput output, ViewContext viewContext);
    }
}
