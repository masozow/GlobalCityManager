using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GlobalCityManager.Extensions{
    public static class TableParts{
        public static TagBuilder CreateTableHeaderRowWithActions<TModel>(string[] additionalHeaders){
             //Making the headers for the table
            var columns =typeof(TModel).GetProperties();
            var headerRow = new TagBuilder("thead");
            TagBuilder headerCell;
            foreach (var col in columns)
            {
                headerCell = new TagBuilder("th");
                headerCell.InnerHtml.Append(Regex.Replace(col.Name, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 "));
                headerRow.InnerHtml.AppendHtml(headerCell);
            }
            //Adding headers for the action buttons
            foreach (var header in additionalHeaders)
            {
                headerCell = new TagBuilder("th");
                headerCell.InnerHtml.Append(header);
                headerRow.InnerHtml.AppendHtml(headerCell);    
            }
            return headerRow;
        }
        public static TagBuilder CreateTableRowWithDetail(
                                 string idPropertyName, object modelIdPropertyValue,
                                 object modelInstance, Type modelType,
                                 string linkTargetAction, string linkTargetController
                                 )
        {
            //Getting the properties of the model instance
            var dProperties = modelType.GetProperties();

            //Making a new row for the table
            var dataRow = new TagBuilder("tr");

            //Initializing a custom link creation
            LinkButtonCreation newLink = new LinkButtonCreation();
            //Passing through the properties. With this we're going to make columns
            var modelProperties = modelInstance.GetType().GetProperties();
            foreach (var col in modelProperties)
            {
                //Making a new cell to attach later to the table
                var dataCell = new TagBuilder("td");
                //If we are passing throug the model's ID
                if (col.Name == idPropertyName)
                {
                    //Create a Link to direct us to this registry's detail
                    var detailLink = newLink.LinkButtonCreate(linkTargetAction, linkTargetController, "", idPropertyName.ToLower(), modelIdPropertyValue.ToString());
                    dataCell.InnerHtml.AppendHtml(detailLink);
                }
                else
                {
                    //Adding the value of any other property that isn't the ID
                    var dValue = col.GetValue(modelInstance);
                    dataCell.InnerHtml.Append(dValue.ToString());
                }
                //Appending the cell to the table row
                dataRow.InnerHtml.AppendHtml(dataCell);
            }
            return dataRow;
        }

        public static TagBuilder CreateTableRowWithDetailAndImage(
                                 string idPropertyName, object modelIdPropertyValue,
                                 object modelInstance, Type modelType,
                                 string linkTargetAction, string linkTargetController,
                                 string imgPropertyName, object modelImgPropertyValue
                                 )
        {
            //Getting the properties of the model instance
            var dProperties = modelType.GetProperties();

            //Making a new row for the table
            var dataRow = new TagBuilder("tr");

            //Initializing a custom link creation
            LinkButtonCreation newLink = new LinkButtonCreation();
            //Passing through the properties. With this we're going to make columns
            var modelProperties = modelInstance.GetType().GetProperties();
            foreach (var col in modelProperties)
            {
                //Making a new cell to attach later to the table
                var dataCell = new TagBuilder("td");
                //If we are passing throug the model's ID
                if (col.Name == idPropertyName)
                {
                    //Create a Link to direct us to this registry's detail
                    var detailLink = newLink.LinkButtonCreate(linkTargetAction, linkTargetController, "", idPropertyName.ToLower(), modelIdPropertyValue.ToString());
                    dataCell.InnerHtml.AppendHtml(detailLink);
                }
                else if(col.Name == imgPropertyName)
                {
                    var image = new TagBuilder("img");
                    image.MergeAttribute("src","..\\"+modelImgPropertyValue.ToString());
                    dataCell.InnerHtml.AppendHtml(image);
                }
                else
                {
                    //Adding the value of any other property that isn't the ID
                    var dValue = col.GetValue(modelInstance);
                    dataCell.InnerHtml.Append(dValue.ToString());
                }
                //Appending the cell to the table row
                dataRow.InnerHtml.AppendHtml(dataCell);
            }
            return dataRow;
        }
    }
}
