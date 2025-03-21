using Microsoft.AspNetCore.Components;

namespace ClientApp.Utils;

public static class QueryParser
{
    public static Dictionary<string, string> ParseQueryParameters(NavigationManager navigationManager)
    {
        var uri = navigationManager.ToAbsoluteUri(navigationManager.Uri);
        return Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString());
    }
}