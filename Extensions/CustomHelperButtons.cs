using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.WebUtilities;

namespace GlobalCityManager.Extensions{
    public partial class LinkButtonCreation
    {
        public TagBuilder LinkButtonCreate(string action, string controller, string text, 
                                                    string queryStringName,
                                                    string queryStringValue,
                                                    object htmlAttributes)
        {
            TagBuilder linkButton = new TagBuilder("a");
            var queryString= new Dictionary<string,string>(){{queryStringName.ToLower(),queryStringValue}};
            var urlProperty= QueryHelpers.AddQueryString($"/{controller}/{action}", queryString);
            linkButton.MergeAttribute("href",urlProperty);
            linkButton.InnerHtml.Append(text);
            var idPropertyName=queryString.Keys.OfType<string>().FirstOrDefault();
            linkButton.Attributes.Add("id",$"{action}_{idPropertyName.ToLower()}_{queryString[idPropertyName]}");
            var edtHtmlAttr = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            linkButton.MergeAttributes(edtHtmlAttr);
            return linkButton;
        }
    }
}