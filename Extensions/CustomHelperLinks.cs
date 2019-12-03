using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.WebUtilities;

namespace GlobalCityManager.Extensions{
    public class LinkCreation:ILinkButtonQueryString
    {
        public TagBuilder LinkButtonCreate(string action, string controller, string text, 
                                                    string queryStringName,
                                                    string queryStringValue)
        {
            TagBuilder linkButton = new TagBuilder("a");
            var queryString= new Dictionary<string,string>(){{queryStringName.ToLower(),queryStringValue}};
            var urlProperty= QueryHelpers.AddQueryString($"/{controller}/{action}", queryString);
            linkButton.MergeAttribute("href",urlProperty);
            linkButton.InnerHtml.Append(queryStringValue);
            var idPropertyName=queryString.Keys.OfType<string>().FirstOrDefault();
            return linkButton;
        }
    }
}