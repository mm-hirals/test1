namespace MidCapERP.Admin.Middleware
{
    /// <summary>
    /// Middleware for exception handling
    /// </summary>
    public static class ExceptionHandlerMiddlewareExtensions
    {
        /// <summary>
        /// Use exception handler to handle the exception
        /// </summary>
        /// <param name="app"></param>
        public static void UseExceptionHandlerMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<UseExceptionHandlerMiddleware>();
        }
    }
}