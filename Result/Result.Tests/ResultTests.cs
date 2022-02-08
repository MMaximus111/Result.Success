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
}