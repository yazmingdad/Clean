using Clean.Collector.WebApp.Config;
using Clean.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


// Add services to the container.


builder.Services.SetupAuthentication(configuration);


// Clean DB for application data
builder.Services.SetupCleanDb(configuration);

// Identity DB for Identity data
builder.Services.SetupIdentityDatabase(configuration);

//setup Mapper

builder.Services.SetupMapper(configuration);

//setup Services
builder.Services.SetupServices(configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.SetupSwagger();

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
    app.EnsureCleanDbIsCreated();
    app.SeedCleanDataAsync().Wait();

    app.UseSwagger();
    app.UseSwaggerUI(x =>
    {
        x.SwaggerEndpoint("/swagger/v1/swagger.json", "Clean Collector Api v2");
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

