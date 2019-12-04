using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

public interface ILinkButton
{
    TagBuilder LinkButtonCreate(string action, string controller, string text, string queryStringName, string queryStringValue, object htmlAttributes);
    TagBuilder LinkButtonCreate(string action, string controller, string text, string queryStringName, string queryStringValue);    
    TagBuilder LinkButtonCreate(string action, string controller, string text);
}