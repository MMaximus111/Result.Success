using System;
using System.Collections.Generic;
using System.Linq;
using ResultSuccess.Errors;
using ResultSuccess.Extensions;
using Xunit;

namespace ResultSuccess.Tests;

public class ExtensionsTests
{
    [Fact]
    public void GetDetailMessages_ShouldReturnDetailMessagesFromError_Always()
    {
        string[] detailMessages = new[] { "deatil1", "detail2", "detail3"};
        
        Error error = Error.CreateError("message", detailMessages);

        IEnumerable<string> messagesEnumerable = error.GetDetailMessages();

        string[] messages = messagesEnumerable.ToArray();
        
        Assert.True(detailMessages.Length == messages.Length);

        foreach (string detailMessage in detailMessages)
        {
            Assert.Contains(detailMessage, messages);
        }
    }
    
    [Fact]
    public void GetDetailMessages_ShouldReturnNull_WhenDetailsIsNull()
    {
        Error error = Error.CreateError("message", detailErrorMessages: null);

        IEnumerable<string> messagesEnumerable = error.GetDetailMessages();
        
        Assert.Null(messagesEnumerable);
    }

    [Fact]
    public void Concat_ShouldSetGeneralErrorMessageAndMergeAllDetails_Always()
    {
        const string generalErrorMessage = "concated message";

        Error error1 = Error.CreateError("message1", "1", "1");
        
        Error error2 = Error.CreateError("message2", "2", "2");

        Error error = error1.Concat(error2, generalErrorMessage);
        
        Assert.True(error.Details.Count == 4);
        
        Assert.Equal(generalErrorMessage, error.ErrorMessage);
    }
    
    [Fact]
    public void Concat_ShouldThrowExceptionWithNullArguments_Always()
    {
        const string generalErrorMessage = "concated message";

        Error error1 = Error.CreateError("message1", "1", "1");
        
        Assert.Throws<ArgumentNullException>(() => error1.Concat(null, generalErrorMessage));

        error1 = null;
        
        Assert.Throws<ArgumentNullException>(() => error1.Concat(Error.CreateError("qwerty"), generalErrorMessage));
    }
    
    [Fact]
    public void Concat_ShouldNotThrowException_WhenDetailsIsNull()
    {
        Error.CreateError(null, detailErrorMessages: null);
        Error.CreateError(null, detailErrorMessages: null);
    }
    
    [Fact]
    public void Concat_ShouldSetErrorIdFromFirstError_Always()
    {
        const string generalErrorMessage = "concated message";
        const int errorId = 123;
        
        Error error1 = Error.CreateError("message1", errorId);
        
        Error error2 = Error.CreateError("message2", "2", "2");

        Error error = error1.Concat(error2, generalErrorMessage);
        
        Assert.Equal(errorId, error.ErrorId);
    }

    [Fact]
    public void CastError_ShouldCastResultToGeneric_Always()
    {
        Result result = Result.Error("error text");
        
        Result<SuccessDto> castedResult = result.CastError<SuccessDto>();
        
        Assert.False(castedResult.IsSuccess);
        
        Assert.Equal(result.ErrorObj, castedResult.ErrorObj);
    }
}