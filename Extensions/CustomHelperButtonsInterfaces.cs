using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

public interface ILinkButtonQueryStringHtmlAttr
{
    TagBuilder LinkButtonCreate(string action, string controller, string text, string queryStringName, string queryStringValue, object htmlAttributes);
}
public interface ILinkButtonQueryString
{
    TagBuilder LinkButtonCreate(string action, string controller, string text, string queryStringName, string queryStringValue);    
}

public interface ILinkButton
{
    TagBuilder LinkButtonCreate(string action, string controller, string text);
}