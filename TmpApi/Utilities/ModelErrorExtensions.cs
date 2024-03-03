using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq.Expressions;
using TmpApi.Services;

namespace TmpApi.Utilities;

public static class ModelErrorExtensions
{
    /*
    public static void AddAllErrors<T>(this ServiceResponse<T> response, ModelStateDictionary modelState)
        where T : class
    {
        foreach (var item in response.Errors)
        {
            modelState.AddModelError(item.Key, item.Value);
        }
    }*/

    public static void AddAllErrors(this ModelStateDictionary modelState, IServiceResponse serviceResponse)
    {
        foreach (var item in serviceResponse.Errors)
        {
            modelState.AddModelError(item.Key, item.Value);
        }
    }

    public static void AddError<T>(this ServiceResponse<T> response, object keySource, string errorMessage)
        where T : class
    {
        response.AddError(nameof(keySource), errorMessage);
    }
}
