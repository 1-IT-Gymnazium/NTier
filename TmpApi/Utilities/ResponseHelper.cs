using TmpApi.Services;

namespace TmpApi.Utilities;

public static class ResponseHelper
{
    public static ServiceResponse<T> Create<T>()
        where T : class
        => new();
}
