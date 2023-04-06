using Clean.Auth.WebApp.Config;
using Clean.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.


builder.Services.SetupAuthentication(configuration);
//builder.Services.SetAuthorization();


// Clean DB for application data
builder.Services.SetupCleanDb(configuration);

// Identity DB for Identity data
builder.Services.SetupIdentityDatabase(configuration);


//setup Services
builder.Services.SetupServices(configuration);

//setup Email
builder.Services.SetupEmail(configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.SetupSwagger();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();

//use CORS
app.UseCors(
  options => options.WithOrigins("*").AllowAnyMethod().AllowAnyHeader()
      );

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.EnsureIdentityDbIsCreated();
    //app.SeedIdentityDataAsync().Wait();

    app.UseSwagger();
    app.UseSwaggerUI(x =>
    {
        x.SwaggerEndpoint("/swagger/v1/swagger.json", "Clean Authentication Api v2");
    });
    app.UseSwaggerUI();


}

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});


app.Run();
