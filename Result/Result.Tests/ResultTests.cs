using System.Collections.Generic;
using System.Linq;
using Result.Errors;
using Xunit;

namespace Result.Tests;

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

        Result resultWithData = Result<SuccessDto>.Error(Error.CreateError(null, null));

        Assert.False(resultWithData.IsSuccess);

        resultWithData = Result<SuccessDto>.Error("error123", "detail error");

        Assert.False(resultWithData.IsSuccess);
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
}