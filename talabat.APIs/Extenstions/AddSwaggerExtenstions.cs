namespace talabat.APIs.Extenstions
{
    public static class AddSwaggerExtenstions
    {
        public static WebApplication UseSwaggerMidleWares( this WebApplication app )
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }

    }
}
