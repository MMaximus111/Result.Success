using System;
using System.Collections.Generic;
using System.Linq;
using Result.Errors;
using Xunit;

namespace Result.Tests;

public class ErrorTests
{
    [Fact]
    public void CreateError_ShouldCorrectCreateErrorWithErrorMessages_Always()
    {
        const string generalMessage = "gm";
        const string detailMessage1 = "dm1";
        const string detailMessage2 = "dm2";

        Error error = Error.CreateError(generalMessage, detailMessage1, detailMessage2);

        Assert.Equal(generalMessage, error.ErrorMessage);

        Assert.True(error.Details.Any(x => string.Equals(x.ErrorMessage, detailMessage1)));
        Assert.True(error.Details.Any(x => string.Equals(x.ErrorMessage, detailMessage2)));
    }

    [Fact]
    public void AddErrorDetail_ShouldAddErrorDetail_Always()
    {
        Error error = Error.CreateError("general message");

        Error detailError = Error.CreateError("datail error message");

        error.AddErrorDetail(detailError);

        Assert.True(error.Details.Contains(detailError));
    }

    [Theory]
    [InlineData(1, "gm", "dm")]
    [InlineData(999, "", "qwerty")]
    [InlineData(null, null, null)]
    [InlineData(-100, "Some info about error", "detail")]
    public void CreateErrorlWithErrorId_ShouldInitializeAllCorrect_Always(int? errorId, string generalMessage, string detailErrorMessage)
    {
        Dictionary<string, object> messageParams = new ()
        {
            { "text", new object() },
            { "ho-ho-ho", new List<DateTime>() }
        };

        Error error = Error.CreateError(generalMessage, detailErrorMessage, errorId, messageParams);

        Assert.Equal(error.ErrorMessage, generalMessage);
        Assert.Equal(error.ErrorId, errorId);
        Assert.True(error.Details.Count == 1 && error.Details.First().ErrorMessage == detailErrorMessage);
        Assert.Equal(error.Details.First().MessageParams, messageParams);
    }
}