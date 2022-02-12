using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using ResultSuccess.AspNetCore;
using ResultSuccess.Errors;
using Xunit;

namespace ResultSuccess.Tests;

public class ResultTests
{
    [Fact]
    public void Success_ShouldCreateSuccessResult_Always()
    {
        Result result = Result.Success();

        Assert.True(result.IsSuccess);

        result = Result.Success("warning1", "warning2");

        Assert.True(result.IsSuccess);

        Result resultWithData = Result<SuccessDto>.Success(new SuccessDto());

        Assert.True(resultWithData.IsSuccess);

        resultWithData = Result<SuccessDto>.Success(new SuccessDto(), "warning");

        Assert.True(resultWithData.IsSuccess);
    }

    [Fact]
    public void Error_ShouldCreateErrorResult_Always()
    {
        Result result = Result.Error(null, null);

        Assert.False(result.IsSuccess);

        result = Result.Error("general error", "detailed error");

        Assert.False(result.IsSuccess);

        Result resultWithData = Result<SuccessDto>.Error(Error.CreateError(null, detailErrorMessages: null));

        Assert.False(resultWithData.IsSuccess);
        
        resultWithData = Result<SuccessDto>.Error(Error.CreateError(null, errorId: null));

        Assert.False(resultWithData.IsSuccess);

        resultWithData = Result<SuccessDto>.Error("error123", "detail error");

        Assert.False(resultWithData.IsSuccess);
    }

    [Fact]
    public void Error_ShouldNotCreateDataObject_Always()
    {
        Result<SuccessDto> result = Result<SuccessDto>.Error(Error.CreateError(null, detailErrorMessages: null));

        Assert.Null(result.Data);
        
        result = Result<SuccessDto>.Error(Error.CreateError(null, errorId: null));

        Assert.Null(result.Data);

        result = Result<SuccessDto>.Error("error123", "detail error");

        Assert.Null(result.Data);
    }

    [Fact]
    public void Success_ShouldNotCreateErrorObj_Always()
    {
        Result result = Result.Success();

        Assert.Null(result.ErrorObj);

        result = Result.Success("warning1", "warning2");

        Assert.Null(result.ErrorObj);

        Result resultWithData = Result<SuccessDto>.Success(new SuccessDto());

        Assert.Null(resultWithData.ErrorObj);

        resultWithData = Result<SuccessDto>.Success(new SuccessDto(), "warning");

        Assert.Null(resultWithData.ErrorObj);
    }

    [Fact]
    public void AddWarnings_ShouldAddWarnings_Always()
    {
        const string warning1 = "warning1";
        const string warning2 = "warning2";

        Result result = Result.Success();

        result.AddWarnings();
        result.AddWarnings(warning1, warning2);

        Assert.True(result.Warnings.Count == 2);

        Assert.Contains(result.Warnings, x => x == warning1);
        Assert.Contains(result.Warnings, x => x == warning2);
    }

    [Fact]
    public void Success_ShouldSetData_IfDataPassed()
    {
        SuccessDto successDto = new ();

        Result<SuccessDto> result = Result<SuccessDto>.Success(successDto);

        result.AddWarnings();

        Assert.Equal(successDto, result.Data);

        result = Result<SuccessDto>.Success(successDto, "warning1", "warning2");

        Assert.Equal(successDto, result.Data);
    }

    [Fact]
    public void Success_ShouldNotSetData_IfDataNotPassed()
    {
        Result<SuccessDto> result = Result<SuccessDto>.Success(null);

        result.AddWarnings();

        Assert.Null(result.Data);
    }

    [Fact]
    public void Error_ShouldSetErrorObj_Always()
    {
        const string generalError = "general";
        const string detailError = "detail";

        Dictionary<string, object> messageParams = new()
        {
            {"text", new object()},
            {"key", new decimal()}
        };

        Error error = Error.CreateError(generalError, detailError);

        Result<SuccessDto> result = Result<SuccessDto>.Error(error);

        Assert.Equal(result.ErrorObj, error);

        result = Result<SuccessDto>.Error(generalError, detailError, 7, messageParams);

        Assert.Equal(result.ErrorObj.ErrorMessage, generalError);
        Assert.Equal(result.ErrorObj.Details.First().MessageParams, messageParams);
        Assert.Contains(result.ErrorObj.Details, x => x.ErrorMessage == detailError);

        Result errorResult = Result.Error(Error.CreateError(generalError, detailError));

        Assert.Equal(generalError, errorResult.ErrorObj.ErrorMessage);
        Assert.Contains(errorResult.ErrorObj.Details, x => x.ErrorMessage == detailError);
    }

    [Fact]
    public void ResultSuccess_ShouldNotFallWithNullValues_Always()
    {
        Result.Success(null, null, null, null);

        Result<SuccessDto>.Success(null, null, null);
    }

    [Fact]
    public void ResultError_ShouldThrowException_WhenErrorParameterIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => Result.Error(error: null));
        Assert.Throws<ArgumentNullException>(() => Result<SuccessDto>.Error(error: null));
    }

    [Fact]
    public void ToActionResult_ShouldReturnSuccessStatusCodeByDefault_WhenResultSuccess()
    {
        Result successResult = Result.Success();

        IActionResult actionResult = successResult.ToActionResult();

        HttpStatusCode httpStatusCode = GetStatusCodeFromActionResult(actionResult);
        
        Assert.Equal(HttpStatusCode.OK, httpStatusCode);
        
        Result<SuccessDto> successResultWithData = Result<SuccessDto>.Success(null);
        
        actionResult = successResultWithData.ToActionResult();

        httpStatusCode = GetStatusCodeFromActionResult(actionResult);
        
        Assert.Equal(HttpStatusCode.OK, httpStatusCode);

    }
    
    [Fact]
    public void ToActionResult_ShouldReturnErrorStatusCodeByDefault_WhenResultError()
    {
        Result errorResult = Result.Error("Error message.");

        IActionResult actionResult = errorResult.ToActionResult();

        HttpStatusCode httpStatusCode = GetStatusCodeFromActionResult(actionResult);
        
        Assert.Equal(HttpStatusCode.BadRequest, httpStatusCode);
        
        Result<SuccessDto> errorResultWithData = Result<SuccessDto>.Error("error text");
        
        actionResult = errorResultWithData.ToActionResult();

        httpStatusCode = GetStatusCodeFromActionResult(actionResult);
        
        Assert.Equal(HttpStatusCode.BadRequest, httpStatusCode);
    }

    [Fact]
    public void ToActionResult_ShouldReturnOverridenStatusCode_Always()
    {
        Result result = Result.Success();

        IActionResult actionResult = result.ToActionResult((int)HttpStatusCode.Created);
        
        HttpStatusCode httpStatusCode = GetStatusCodeFromActionResult(actionResult);
        
        Assert.Equal(HttpStatusCode.Created, httpStatusCode);
        
        result = Result.Error("error");

        actionResult = result.ToActionResult(errorStatusCode: (int)HttpStatusCode.Conflict);
        
        httpStatusCode = GetStatusCodeFromActionResult(actionResult);
        
        Assert.Equal(HttpStatusCode.Conflict, httpStatusCode);
    } 

    private static HttpStatusCode GetStatusCodeFromActionResult(IActionResult actionResult)
    {
        return (HttpStatusCode)actionResult.GetType().GetProperty("StatusCode").GetValue(actionResult, null);
    }
}