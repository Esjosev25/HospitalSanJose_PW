namespace HospitalSanJose.Config
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Response.StatusCode == 401)
            {
                context.Response.Redirect("/Error/401");
            }
            await _next(context);

        }
    }
}
