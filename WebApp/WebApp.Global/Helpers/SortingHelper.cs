using WebApp.Global.Shared.DTOs;

namespace WebApp.Global.Helpers
{
    public class SortingHelper
    {
        private string UpIcon = "fa fa-arrow-up";
        private string DownIcon = "fa fa-arrow-down";

        public string OrderBy { get; set; }
        public string Direction { get; set; }
        private List<SortableColumnDto> sortableColumns = new List<SortableColumnDto>();

        public void AddColumn(string column, bool isDefaultColumn = false)
        {
            SortableColumnDto? columnDto = sortableColumns.Where(c => c.ColumnName.ToLower() == column.ToLower()).SingleOrDefault();
            if (columnDto == null)
            {
                sortableColumns.Add(columnDto = new SortableColumnDto() { ColumnName = column });
            }

            if (isDefaultColumn == true || sortableColumns.Count == 1)
            {
                OrderBy = $"{column}_Desc";
                Direction = "Desc";
            }
        }

        public SortableColumnDto GetColumn(string column)
        {
            SortableColumnDto? columnDto = sortableColumns.Where(c => c.ColumnName.ToLower() == column.ToLower()).SingleOrDefault();
            if (columnDto == null)
            {
                sortableColumns.Add(columnDto = new SortableColumnDto() { ColumnName = column });
            }
            return columnDto;
        }

        public void ApplySort(string sortExpression)
        {
            if (string.IsNullOrWhiteSpace(sortExpression))
            {
                sortExpression = this.OrderBy;
            }

            sortExpression = sortExpression.ToLower();

            foreach (var sortableColumn in this.sortableColumns)
            {
                sortableColumn.SortIcon = "";
                sortableColumn.SortExpression = sortableColumn.ColumnName;

                if (sortExpression == sortableColumn.ColumnName.ToLower())
                {
                    this.OrderBy = sortableColumn.ColumnName;
                    this.Direction = "asc";

                    sortableColumn.SortIcon = DownIcon;
                    sortableColumn.SortExpression = $"{sortableColumn.ColumnName}_desc";
                }

                if (sortExpression == $"{sortableColumn.ColumnName.ToLower()}_desc")
                {
                    this.OrderBy = sortableColumn.ColumnName;
                    this.Direction = "desc";
                    sortableColumn.SortIcon = UpIcon;
                    sortableColumn.SortExpression = sortableColumn.ColumnName;
                }
            }
        }
    }
}
