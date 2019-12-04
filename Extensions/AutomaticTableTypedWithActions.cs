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
        //1. Send the buttons as modiffiers, so they can be attached only when needed
        //2. Image modifier to wrap an specific column, just as NationalFlag one
        //3. String formatter to separate camel-case in different words
        //4. Icons on buttons
        //5. Exception handling here and in the related classes
        public static IHtmlContent AutomaticTableTypedWithActions<TModel,TResult>(this IHtmlHelper htmlHelper, 
                Expression<Func<TModel,TResult>> idProperty, IEnumerable<TModel> data,
                object tableHtmlAttributes,
                string detailLinkTargetAction, string detailLinkTargetController,
                string editLinkText, string editTargetAction,string editTargetController,
                object editHtmlAttributes,
                string deleteLinkText, string deleteTargetAction,string deleteTargetController,
                object deleteHtmlAttributes) 
                where TModel: class,new()
        {
            //Starting the table and adding attributes to it
            var table = new TagBuilder("table");
            table.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(tableHtmlAttributes));
            
            //Create a new row for the header
            var headerRow = TableParts.CreateTableHeaderRowWithActions<TModel>(new string[]{editLinkText,deleteLinkText});
            table.InnerHtml.AppendHtml(headerRow);

            //Getting the name and value of the expression received to identify which one is the ID
            var body = idProperty.Body  as MemberExpression;
            var idPropertyName = body.Member.Name;
            bool firstTime = true;
            Type dType = null;
            foreach(var d in data)
            {   
                //Getting the value of model's ID property that belongs to the object "d"
                if(firstTime)
                {
                    //avoiding the use of reflection all the time
                    dType=d.GetType();
                    firstTime=false;
                }
                PropertyInfo modelIdPropety=dType.GetProperty(idPropertyName);
                object modelIdPropertyValue= modelIdPropety.GetValue(d);

                //Making a new row for the table
                var dataRow = TableParts.CreateTableRowWithDetail(idPropertyName,modelIdPropertyValue,d,dType,detailLinkTargetAction,detailLinkTargetController);
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