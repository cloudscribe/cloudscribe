using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace cloudscribe.QueryTool.Web
{
    public class QueryToolViewModel
    {
        /// <summary>
        /// The list of tables in the database
        /// </summary>
        public SelectList? TableNames { get; set; }

        /// <summary>
        /// The name of the table selected by the user
        /// </summary>
        public string Table { get; set; } = string.Empty;

        /// <summary>
        /// The list of columns in the selected table
        /// </summary>
        public SelectList? ColumnNames { get; set; }

        /// <summary>
        /// The name(s) of the column(s) selected by the user
        /// </summary>
        public List<string>? Columns { get; set; } = new List<string>();

        /// <summary>
        /// The SQL Query string from the Query input box
        /// </summary>
        public string Query { get; set; } = string.Empty;

        /// <summary>
        /// An optional list of query parameters to mimic the behaviour of the Query API
        /// If parameters are supplied here then the SQL query will need to be parameterised
        /// </summary>
        public string QueryParameters { get; set; } = string.Empty;

        /// <summary>
        /// The hightlighted/selected SQL Query string from the Query input box
        /// </summary>
        public string HighlightText { get; set; } = string.Empty;
        public int HighlightStart { get; set; } = 0;
        public int HighlightEnd { get; set; } = 0;

        /// <summary>
        /// Do we have a query to run?
        /// </summary>
        public bool HasQuery { get; set; } = false;

        /// <summary>
        /// Is the query valid?
        /// </summary>
        public bool QueryIsValid { get; set; } = false;

        /// <summary>
        /// Is the query an API query?
        /// </summary>
        public bool QueryIsAPI { get; set; } = false;

        /// <summary>
        /// The result of the query
        /// </summary>
        public DataTable? Data { get; set; }

        /// <summary>
        /// The command button pressed by the user
        /// </summary>
        public string Command { get; set; } = string.Empty;

        /// <summary>
        /// The name we give to a saved query
        /// </summary>
        public string SaveName { get; set; } = string.Empty;

        /// <summary>
        /// Do we want to save the query as an API Query?
        /// </summary>
        public bool SaveNameAsApi { get; set; } = false;

        /// <summary>
        /// The list of saved queries available to the user
        /// </summary>
        public SelectList? SavedQueryNames { get; set; }

        /// <summary>
        /// The name of the saved query selected by the user
        /// </summary>
        public string SavedQueryName { get; set; } = string.Empty;

        /// <summary>
        /// The number of affected rows from the query
        /// </summary>
        public int? RowsAffected { get; set; }

        /// <summary>
        /// Any SQL or runtime errors go here
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Any informational messages go here
        /// </summary>
        public string? InformationMessage { get; set; }

        /// <summary>
        /// Any warning messages go here
        /// </summary>
        public string? WarningMessage { get; set; }

        /// <summary>
        /// Do we enable tooltips on the page?
        /// </summary>
        public bool EnableTooltips { get; set; } = false;

    }
}
