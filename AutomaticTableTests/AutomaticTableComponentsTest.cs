using System.Collections;
using System.IO;
using Xunit;
using GlobalCityManager.Extensions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using Xunit.Extensions;

namespace AutomaticTableTests
{
    public class UnitTest1
    {
        [Theory,MemberData(nameof(LinkButtonTestCases))]        
        public void LinkButtonCreate(string action, string controller, string text, string name, string value, object param)
        {
            //Arrange
            LinkButtonCreation buttonCreation = new LinkButtonCreation();
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(param);
            //Act
            var button = buttonCreation.LinkButtonCreate(action, controller, text, name, value, param);
            string htmlOutput = "";
            using (var writer = new StringWriter())
            {
                button.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                htmlOutput = writer.ToString();
            }

            //Assert
            Assert.StartsWith("<a",htmlOutput);
            Assert.Contains($"href=\"/{controller}/{action}?{name}={value}\"".Trim().ToLower(),htmlOutput.Trim().ToLower());
            Assert.Contains($"id=\"{action}_{name}_{value}\"".Trim().ToLower(),htmlOutput.Trim().ToLower());
            foreach (var atr in attributes)
            {
                Assert.Contains($"{atr.Key}=\"{atr.Value}\"".Trim().ToLower(),htmlOutput.Trim().ToLower());
            }
            Assert.EndsWith($">{text}</a>".Trim().ToLower(),htmlOutput.Trim().ToLower());
        }
       
        public static IEnumerable<object[]> LinkButtonTestCases
        {
            get
            {
                return new[]
                {
                new object[] { "Action", "Controller", "Text", "Name", "Value", new { @class = "btn" } },
                new object[] { "Action", "Controller", "Text", "Name", "Value", new { @class = "btn btn-info", @style="font-size:1;" } }
            };
            }
        }
    }
}
