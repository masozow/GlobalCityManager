using Microsoft.AspNetCore.Mvc.Rendering;

namespace GlobalCityManager.Extensions{
    public static class RowModificator{
        public static void AppendCustomCellToRow(ref TagBuilder row, TagBuilder appendableObject)
        {
            TagBuilder cell = new TagBuilder("td");
            cell.InnerHtml.AppendHtml(appendableObject);
            row.InnerHtml.AppendHtml(cell);
        }
    }
}