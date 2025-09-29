var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

/*

// [1] Middleware do nothing.
app.Use((RequestDelegate next) => next);

// [2] Middleware intercepts HttpContext.
app.Use((RequestDelegate next) =>
{
    return async (HttpContext context) =>
    {
        await context.Response.WriteAsync("[2] Middleware.");
        await next(context);
    };
});

app.Use(async (HttpContext context, RequestDelegate next) =>
{
    await context.Response.WriteAsync("[3] Middleware.");
    await next(context);
});

app.Use(async (HttpContext context, RequestDelegate next) =>
{
    await context.Response.WriteAsync("[4] Middleware.");
});

app.Use(async (HttpContext context, RequestDelegate next) =>
{
    await context.Response.WriteAsync("[5] Middleware.");
    await next(context);
});

app.Run(async (HttpContext context) =>
{
    await context.Response.WriteAsync("This is an end of the pipeline.");
});

app.Use(async (HttpContext context, RequestDelegate next) =>
{
    await context.Response.WriteAsync("After the pipeline end.");
    await next(context);
});

app.Use(async (context, next) =>
{
    // context.Response.StatusCode = StatusCodes.Status206PartialContent;
    context.Response.ContentLength = 20;

    await context.Response.WriteAsync("Middleware [1]. \n");
    
    // context.Response.StatusCode = StatusCodes.Status206PartialContent;
    
    // context.Response.Headers.Append("X-Hdr1", "Val1");

    await next();
});

app.Use(async (context, next) =>
{
    // context.Response.StatusCode = StatusCodes.Status206PartialContent;

    await context.Response.WriteAsync("abc");
    
    await next();
});

app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("Middleware [3]. \n");
    
    await next();
});

app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("Before middleware [1]. \n");

    await next();

    await context.Response.WriteAsync("After middleware [1]. \n");
});

app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("     Before middleware [2]. \n");

    await next();

    await context.Response.WriteAsync("     After middleware [2]. \n");
});

app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("         Before middleware [3]. \n");

    await next();

    await context.Response.WriteAsync("         After middleware [3]. \n");
});

// [1] Built-in middlewares.
app.UseExceptionHandler();
app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

// [2] Custom middlewares
app.Use((RequestDelegate next) => next);

// [3] Endpoints.
app.MapGet("/", () => "Hello World!!");

app.Map("/branch1", GetBranch1);
app.Map("/branch2", GetBranch2);

app.Run(async context =>
{
    await context.Response.WriteAsync("Terminal middleware. \n");
});

app.MapWhen((context) =>

    context.Request.Path.Equals("/checkout", StringComparison.OrdinalIgnoreCase) &&
    context.Request.Query["mode"] == "new",
    b =>
    {
        b.Run(async context =>
        {
            await context.Response.WriteAsync("Modern checkout processing pipeline. \n");
        });
    }
);

app.Map("/checkout", b =>
{
    b.Run(async context =>
    {
        await context.Response.WriteAsync("Legacy checkout processing pipeline. \n");
    });
});

*/

app.UseWhen(context =>
    context.Request.Path.Equals("/branch1", StringComparison.OrdinalIgnoreCase),
    b =>
    {
        b.Use(async (context, next) =>
        {
            await context.Response.WriteAsync("Middleware in branch [1]");
            await next();
        });
    });

app.Run(async context =>
{
    await context.Response.WriteAsync("Terminal middleware. \n");
});


app.Run();

/*

static void GetCommonBranch(IApplicationBuilder app)
{
    app.Use(async (context, next) =>
    {
        await context.Response.WriteAsync("Middleware [1]. \n");
        await next();
    });

    app.Use(async (context, next) =>
    {
        await context.Response.WriteAsync("Middleware [2]. \n");
        await next();
    });
}

static void GetBranch1(IApplicationBuilder app)
{
    GetCommonBranch(app);

    app.Use(async (context, next) =>
    {
        await context.Response.WriteAsync("Middleware [3]. \n");
        await next();
    });

    app.Run(async (context) =>
    {
        await context.Response.WriteAsync("Middleware [4]. \n");
    });
}

static void GetBranch2(IApplicationBuilder app)
{
    GetCommonBranch(app);

    app.Use(async (context, next) =>
    {
        await context.Response.WriteAsync("Middleware [5]. \n");
        await next();
    });

    app.Run(async (context) =>
    {
        await context.Response.WriteAsync("Middleware [6]. \n");
    });
}

*/
