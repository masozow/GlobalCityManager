
using Microsoft.AspNetCore.Html;

namespace Microsoft.AspNetCore.Mvc.Rendering{
    public static partial class MyHtmlHelperExtensions{
        //We create a Custom Html Helper, non typed version, because the condition is 
        //to use arrays, not a specific model
        public static IHtmlContent AutomaticTable(this IHtmlHelper htmlHelper, string[] columns, string[,] content){
            //First we create the table wich will contain all the tag builders
            var table= new TagBuilder("table");
            //Adding an attribute to see the border of the table
            table.Attributes.Add("border","1");

            //We create a new row that will be the first of the table and will be used to 
            //contain the table headers
            var headerRow = new TagBuilder("tr");
            //We iterate through all the column names we got
            foreach (var col in columns)
            {
                //We go from inside out, first we create a new table header
                var headerCell = new TagBuilder("th");
                //Then we append the column name to it
                headerCell.InnerHtml.Append(col);
                //In the end we append the new table header to the row
                headerRow.InnerHtml.AppendHtml(headerCell);
            }
            //Here we append the new row to the table, so now we have a table with a single row
            //containing all the headers with its text content
            table.InnerHtml.AppendHtml(headerRow);
            //We get the number of columns
            var columnLength = columns.Length;
            //We get the total lenght of the bidimiensional array, which represents all the data
            //contained in the array, then we divide it by the number of columns so we get the
            //rows number or lenght
            var rowsLength = content.Length/columnLength;
            //As we may now we first run through the rows of a bidimiensional array
            for (int y = 0; y < rowsLength; y++)
            {
                //We create a new table row for each row in the bidimiensional array
                var contentRow = new TagBuilder("tr");
                //Then we iterate between all the columns inside this row of the bidimiensional array
                for (int x = 0; x < columnLength; x++)
                {
                    //We create a new table data for each column
                    var contentCell = new TagBuilder("td");
                    //We append the data from the bidimensional array corresponding to this row/column combination
                    contentCell.InnerHtml.Append(content[y,x]);
                    //We append the new table data to the table row
                    contentRow.InnerHtml.AppendHtml(contentCell);
                }
                //Every table row is appended to the table
                table.InnerHtml.AppendHtml(contentRow);
            }
            //We return the new table with all of its elements
            return table;
        }
    }
}