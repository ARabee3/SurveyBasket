using SurveyBasket.Api;
using SurveyBasket.Api.Persistence;
var builder = WebApplication.CreateBuilder(args);

// you can read configurations from Appsettings (As Dictionary) or environment variables in LaunchSettings 
// Environment Variables ===> Secrets ===> AppSettings Dev ===> AppSettings
builder.Services.AddDependencies(builder.Configuration);
//builder.Services.AddIdentityApiEndpoints<ApplicationUser>().AddEntityFrameworkStores<ApplicationDbContext>();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
var logger = app.Logger;

app.UseHttpsRedirection();

//must be Before Authorization (checking CORS then Authorize)
app.UseCors(); // uses default policy

app.UseAuthorization();
//app.MapIdentityApi<ApplicationUser>();

app.MapControllers();

app.Run();
