using API;
using Application;
using Infraestructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddApplication();
builder.Services.AddInfraestructure(builder.Configuration);
builder.Services.AddJwtWithCookies(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor",
        policy =>
        {
            policy
            .WithOrigins("https://localhost:7145")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandlingMiddleware();

app.UseHttpsRedirection();

app.UseMiddleware<RefreshTokenMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowBlazor");

//Llenado de datos inicial
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await IdentityDataSeeder.SeedAsync(services);
}

app.Run();
