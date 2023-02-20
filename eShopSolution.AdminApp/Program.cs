using eShopSolution.AdminApp.LocalizationResources;
using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Mutiple Language
var cultures = new[]
{
                new CultureInfo("en"),
                new CultureInfo("vi"),
                

            };
// Mutiple Language End
builder.Services.AddControllersWithViews()
    //.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>())
             // Mutiple Language
             .AddExpressLocalization<ExpressLocalizationResource, ViewLocalizationResource>(ops =>
             {
               
                 // Uncomment and set to true to use only route culture provider
                 ops.UseAllCultureProviders = false;
                 ops.ResourcesPath = "LocalizationResources";
                 ops.RequestLocalizationOptions = o =>
                 {
                     o.SupportedCultures = cultures;
                     o.SupportedUICultures = cultures;
                     o.DefaultRequestCulture = new RequestCulture("vi");
                 };
             });
//Mutiple Language End


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseRequestLocalization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{culture=vi}/{controller=Home}/{action=Index}/{id?}");
});

app.Run();
