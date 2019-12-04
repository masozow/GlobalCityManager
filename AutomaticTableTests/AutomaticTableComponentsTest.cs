using System.Collections;
using System.IO;
using Xunit;
using GlobalCityManager.Extensions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace AutomaticTableTests
{
    public class UnitTest1
    {
        [Fact]        
        public void LinkButtonCreate()
        {
            //Arrange
            LinkButtonCreation buttonCreation = new LinkButtonCreation();
            string action = "Action";
            string controller = "Controller";
            string text = "Text";
            string name = "Name";
            string value = "Value";
            var param = new { @class = "btn" };

            //Act
            var button = buttonCreation.LinkButtonCreate(action, controller, text, name, value, param);
            string htmlOutput = "";
            using (var writer = new StringWriter())
            {
                button.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                htmlOutput = writer.ToString();
            }
            string expectedString = CreateStringTag(action, controller, text, name, value, param);

            //Assert
            Assert.Equal(htmlOutput.ToLower().Trim(), expectedString.ToLower().Trim());
        }
        private string CreateStringTag(string action, string controller, string text, string name, string value, object param)
        {
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(param);
            string firstPart = "<a";
            foreach (var atr in attributes)
            {
                firstPart += $" {atr.Key}=\"{atr.Value}\"";
            }
            string endPart = $" href=\"/{controller}/{action}?{name}={value}\" id=\"{action}_{name}_{value}\">{text}</a>";
            return firstPart + endPart;
        }
    }
}