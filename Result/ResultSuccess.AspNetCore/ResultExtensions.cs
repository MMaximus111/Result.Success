using Microsoft.AspNetCore.Mvc;

namespace ResultSuccess.AspNetCore;

public static class ResultExtensions
{
    /// <summary>
    /// Convert result to IActionResult.
    /// </summary>
    /// <param name="result">Result object.</param>
    /// <param name="successStatusCode">Success status code. By default 200.</param>
    /// <param name="errorStatusCode">Error status code. By default 400.</param>
    /// <returns>IActionResult.</returns>
    public static IActionResult ToActionResult(this Result result, int successStatusCode = 200, int errorStatusCode = 400)
    {
        IActionResult actionResult = result.IsSuccess
            ? new JsonResult(result) { StatusCode = successStatusCode }
            : new JsonResult(result.ErrorObj) { StatusCode = errorStatusCode };

        return actionResult;
    }
}