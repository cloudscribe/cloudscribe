namespace cloudscribe.Core.Web.Mvc.Components
{
    /// <summary>
    /// defines a method used on the oops/error controller
    /// to decide whehter to return a json response instead of html.
    /// The Default implementation will return json if the url contains /api/
    /// </summary>
    public interface IDecideErrorResponseType
    {
        bool ShouldReturnJson(string originalPath, int statusCode);
    }
}
