namespace cloudscribe.Core.Web.Mvc.Components
{
    public class DefaultErrorResponseTypeDecider : IDecideErrorResponseType
    {
        public bool ShouldReturnJson(string originalPath, int statusCode)
        {
            if (originalPath.Contains("/api/"))
            {
                if(statusCode >= 400 && statusCode < 600)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
