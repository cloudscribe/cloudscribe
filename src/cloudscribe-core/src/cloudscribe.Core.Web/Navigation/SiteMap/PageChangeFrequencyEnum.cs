

namespace cloudscribe.Core.Web.Navigation
{
    public enum PageChangeFrequency
    {
        /// <summary>
        /// Page content aways changes.
        /// </summary>
        Always,
        /// <summary>
        /// Page content changes hourly.
        /// </summary>
        Hourly,
        /// <summary>
        /// Page content changes daily.
        /// </summary>
        Daily,
        /// <summary>
        /// Page content changes weekly.
        /// </summary>
        Weekly,
        /// <summary>
        /// Page content changes monthly.
        /// </summary>
        Monthly,
        /// <summary>
        /// Page content changes yearly.
        /// </summary>
        Yearly,
        /// <summary>
        /// Page content never changes.
        /// </summary>
        Never
    }
}
