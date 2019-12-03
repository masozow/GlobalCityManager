using Microsoft.AspNetCore.Html;

namespace Microsoft.AspNetCore.Mvc.Rendering{
    public static partial class MyHtmlHelperExtensions{
        public static IHtmlContent ColorfulHeading(this IHtmlHelper htmlHelper, int level, 
                                                   string color, string content){            
            level= level < 1 ? 1 : level;
            level= level > 6 ? 6 : level;
            var tagName=$"h{level}";
            var tagBuilder = new TagBuilder(tagName);
            tagBuilder.Attributes.Add("style",$"color:{color??"green"}");
            tagBuilder.InnerHtml.Append(content??string.Empty);
            return tagBuilder;
        }
    }
}