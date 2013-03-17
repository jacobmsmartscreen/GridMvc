﻿using GridMvc.Columns;
using GridMvc.Resources;
using GridMvc.Utility;

namespace GridMvc.Filtering
{
    /// <summary>
    ///     Renderer for sortable column
    /// </summary>
    internal class QueryStringFilterColumnHeaderRenderer : IGridColumnRenderer
    {
        private const string FilteredButtonCssClass = "filtered";

        private const string FilterContent =
            @" <span 
                                            data-type=""{1}"" 
                                            data-name=""{2}""
                                            data-filtertype=""{3}""
                                            data-filtervalue=""{4}""
                                            data-url=""{5}""
                                            class=""grid-filter"">
                                        <span class=""grid-filter-btn {6}"" title=""{0}""></span>
                                    </span>";

        private readonly QueryStringFilterSettings _settings;

        public QueryStringFilterColumnHeaderRenderer(QueryStringFilterSettings settings)
        {
            _settings = settings;
        }

        #region IGridColumnRenderer Members

        public string Render(IGridColumn column, string content)
        {
            if (!column.FilterEnabled)
                return string.Empty;

            IGridFilterSettings filterSettings = _settings.IsInitState && column.InitialFilterSettings != null
                                                     ? column.InitialFilterSettings
                                                     : _settings;

            var filterType = GridFilterType.Equals;
            string value = string.Empty;
            bool isColumnFiltered = false;
            if (column.Name == filterSettings.ColumnName)
            {
                //filter on this column:
                filterType = filterSettings.Type;
                value = filterSettings.Value;
                isColumnFiltered = true;
            }
            //determine current url:
            var builder = new CustomQueryStringBuilder(_settings.Context.Request.QueryString);
            string url =
                builder.GetQueryStringExcept(new[]
                    {
                        column.ParentGrid.Pager.ParameterName,
                        _settings.FilterInitQueryParameterName,
                        _settings.TypeQueryParameterName,
                        _settings.ColumnQueryParameterName,
                        _settings.ValueQueryParameterName
                    });

            return string.Format(FilterContent,
                                 Strings.FilterButtonTooltipText,
                                 column.FilterWidgetTypeName,
                                 column.Name,
                                 (int) filterType,
                                 value,
                                 url,
                                 isColumnFiltered ? FilteredButtonCssClass : string.Empty);
        }

        #endregion
    }
}