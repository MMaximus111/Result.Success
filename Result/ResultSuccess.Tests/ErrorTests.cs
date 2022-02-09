using System;
using System.Collections.Generic;
using System.Linq;
using ResultSuccess.Errors;
using Xunit;

namespace ResultSuccess.Tests;

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

        Assert.Contains(error.Details, x => string.Equals(x.ErrorMessage, detailMessage1));
        Assert.Contains(error.Details, x => string.Equals(x.ErrorMessage, detailMessage2));
    }

    [Fact]
    public void AddErrorDetail_ShouldAddErrorDetail_WhenErrorIsNotNull()
    {
        Error error = Error.CreateError("general message");

        Error detailError = Error.CreateError("datail error message");

        error.AddErrorDetail(detailError);

        Assert.Contains(detailError, error.Details);
    }

    [Fact]
    public void AddErrorDetail_ShouldThrowException_WhenErrorIsNull()
    {
        Error error = Error.CreateError("general message");

        Assert.Throws<ArgumentNullException>(() => error.AddErrorDetail(null));
    }

    [Theory]
    [InlineData(1, "gm", "dm")]
    [InlineData(999, "", "qwerty")]
    [InlineData(null, null, null)]
    [InlineData(-100, "Some info about error", "detail")]
    [InlineData(1234567, "           ", "              ")]
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