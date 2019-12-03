
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;

namespace Microsoft.AspNetCore.Mvc.Rendering{
    public static class MyHtmlHelperExtensions{
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

        //this is work in progress. Apply SOLID and DRY because it is too long
        public static IHtmlContent AutomaticTableTypedWithActions<TModel,TResult>(this IHtmlHelper htmlHelper, 
                Expression<Func<TModel,TResult>> idProperty, IEnumerable<TModel> data,
                string editLinkText, string editTargetAction,string editTargetController, 
                string deleteLinkText, string deleteTargetAction,string deleteTargetController) 
                where TModel: class,new()
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
            var headerCell2 = new TagBuilder("th");
            headerCell2.InnerHtml.Append("Update");
            headerRow.InnerHtml.AppendHtml(headerCell2);
            table.InnerHtml.AppendHtml(headerRow);
            
            foreach(var d in data)
            {
                //Making a new row for the table
                var dataRow = new TagBuilder("tr");
                //Getting the type of the object whose properties will go individually in every
                //cell inside the table
                var dType=d.GetType();
                //Getting the actual properties
                var dProperties = dType.GetProperties();
                //Passing through the properties. With this we're going to make columns
                foreach(var col in dProperties)
                {
                    //Making a tagBuilder to store every value
                    var dataCell = new TagBuilder("td");
                    //Getting the value of the property
                    var dValue=col.GetValue(d);
                    //Appending the value to the cell of the row
                    dataCell.InnerHtml.Append(dValue.ToString());
                    //Appending the cell to the table row
                    dataRow.InnerHtml.AppendHtml(dataCell);
                }

                //Making the cell where the Edit link will go
                var dataCell2 = new TagBuilder("td");
                //Getting the body of the expression redeived to identify which one
                //is the ID
                var body = idProperty.Body  as MemberExpression;
                //Getting the name of the property
                var idPropertyName = body.Member.Name;
                //we need to get the value of the property, not the property, just a D.ID will work,
                //but we need to get the property of "d" whose name is the same as idPropertyName 
                //and with that get idValue, something like this what we want:
                //var realPropertyID=dProperties.Find(idPropertyName);

                //Getting the value of the property that is the ID
                var idValue = dType.GetProperty(idPropertyName).GetValue(d);
                
                //Making the URL that the actionlink will redirect to
                var URL =$"/{editTargetController}/{editTargetAction}/{idValue.ToString()}";
                var editButton = new TagBuilder("a");
                editButton.MergeAttribute("href",URL);
                editButton.InnerHtml.AppendHtml(editLinkText);
                
                editButton.Attributes.Add("id",idPropertyName.ToLower());
                editButton.Attributes.Add("class","btn btn-warning");
                dataCell2.InnerHtml.AppendHtml(editButton);
                dataRow.InnerHtml.AppendHtml(dataCell2);
                table.InnerHtml.AppendHtml(dataRow);
            }
            return table;
        }
    }
}