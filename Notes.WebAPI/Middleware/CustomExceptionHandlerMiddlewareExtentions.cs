namespace Notes.WebAPI.Middleware;

/// <summary>
/// данный класс нужен для того, чтобы мы могли включать middleware в конвеер
/// </summary>
public static class CustomExceptionHandlerMiddlewareExtentions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this
        IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
    }
}
