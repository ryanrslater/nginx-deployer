using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API Title", Version = "v1" });
});

// Set the URLs conditionally based on the environment
if (builder.Environment.IsProduction())
{
    builder.WebHost.UseUrls("http://+:5001");
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
    });
    app.Map("/api", app =>
    {
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    });
    app.UseSpa(spa =>
    {
        spa.UseProxyToSpaDevelopmentServer("http://localhost:5173");
    });
}
else
{
    app.UseRouting();
    app.UseAuthorization();
    app.UseHttpsRedirection();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}

app.Run();