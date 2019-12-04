using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using GlobalCityManager.Extensions;
using System.Reflection;

namespace Microsoft.AspNetCore.Mvc.Rendering{
    public static partial  class MyHtmlHelperExtensions{
        //This is work in progress. Apply SOLID and DRY because it is too long and complex
        //Future modifications:
        //1. Done (Send the buttons as modiffiers, so they can be attached only when needed)
        //2. Done (Image modifier to wrap a specific column, just as NationalFlag one)
        //3. Done (String formatter to separate camel-case in different words)
        //4. Icons on buttons
        //5. Exception handling here and in the related classes
        public static IHtmlContent AutomaticTableTypedWithActions<TModel,TResult>(this IHtmlHelper htmlHelper, 
                Expression<Func<TModel,TResult>> idProperty, IEnumerable<TModel> data,
                object tableHtmlAttributes,
                string detailLinkTargetAction, string detailLinkTargetController,
                string editLinkText, string editTargetAction,string editTargetController,
                object editHtmlAttributes,
                string deleteLinkText, string deleteTargetAction,string deleteTargetController,
                object deleteHtmlAttributes,Expression<Func<TModel,TResult>> imageProperty) 
                where TModel: class,new()
        {
            //Starting the table and adding attributes to it
            var table = new TagBuilder("table");
            table.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(tableHtmlAttributes));
            
            //Create a new row for the header
            var headerRow = TableParts.CreateTableHeaderRowWithActions<TModel>(new string[]{editLinkText,deleteLinkText});
            table.InnerHtml.AppendHtml(headerRow);

            //Getting the name and value of the expression received to identify which one is the ID
            var bodyId = idProperty.Body  as MemberExpression;
            var idPropertyName = bodyId.Member.Name;

            //Getting the name and value of the expression received to identify which one is the ID
            var bodyImg=imageProperty.Body as MemberExpression;
            var imgPropertyName=bodyImg.Member.Name;

            bool firstTime = true;
            Type dType = null;
            PropertyInfo modelIdPropety = null;
            PropertyInfo modelImgPropety = null;
            foreach(var d in data)
            {   
                //Getting the value of model's ID property that belongs to the object "d"
                if(firstTime)
                {
                    //avoiding the use of reflection all the time
                    dType=d.GetType();
                    modelIdPropety=dType.GetProperty(idPropertyName);
                    modelImgPropety=dType.GetProperty(imgPropertyName);
                    firstTime=false;
                }
                //Getting the values of the desired properties
                object modelIdPropertyValue= modelIdPropety.GetValue(d);
                object modelImgPropertyValue = modelImgPropety.GetValue(d);

                //Making a new row for the table
                var dataRow = TableParts.CreateTableRowWithDetailAndImage(idPropertyName,modelIdPropertyValue,d,dType,
                                                                          detailLinkTargetAction,detailLinkTargetController,
                                                                          imgPropertyName,modelImgPropertyValue);
                table.InnerHtml.AppendHtml(dataRow);

                //Creating a link button for the Edit action
                LinkButtonCreation buttons = new LinkButtonCreation();
                var editButton = buttons.LinkButtonCreate(editTargetAction,editTargetController,editLinkText,
                                                          idPropertyName.ToLower(),modelIdPropertyValue.ToString(),
                                                          editHtmlAttributes);
                //Creating a link button for the Delete action
                var deleteButton = buttons.LinkButtonCreate(deleteTargetAction,deleteTargetController,deleteLinkText,
                                                          idPropertyName.ToLower(),modelIdPropertyValue.ToString(),
                                                          deleteHtmlAttributes);
                //Appending buttons to row
                RowModificator.AppendCustomCellToRow(ref dataRow,editButton);
                RowModificator.AppendCustomCellToRow(ref dataRow,deleteButton);
            }
            return table;
        }

    }
}