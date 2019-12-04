
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;


namespace Microsoft.AspNetCore.Mvc.Rendering{
    public static partial  class MyHtmlHelperExtensions{
        public static IHtmlContent AutomaticTableTyped<TModel>(this IHtmlHelper htmlHelper, List<TModel> data) where TModel: class,new()
        {
            var table = new TagBuilder("table");
            table.Attributes.Add("class","table table-hover");
            var columns =typeof(TModel).GetProperties();

            var headerRow = new TagBuilder("thead");
            foreach (var col in columns)
            {
                var headerCell = new TagBuilder("th");
                headerCell.InnerHtml.Append(col.Name);
                headerRow.InnerHtml.AppendHtml(headerCell);
            }
            table.InnerHtml.AppendHtml(headerRow);
            
            foreach(var d in data)
            {
                var dataRow = new TagBuilder("tr");
                var dType=d.GetType();
                var dProperties = dType.GetProperties();
                foreach(var col in dProperties)
                {
                    var dataCell = new TagBuilder("td");
                    var dValue=col.GetValue(d);
                    dataCell.InnerHtml.Append(dValue.ToString());
                    dataRow.InnerHtml.AppendHtml(dataCell);
                }
                table.InnerHtml.AppendHtml(dataRow);
            }
            return table;
        }
    }
}