using eShopSolution.AdminApp.LocalizationResources;
using eShopSolution.ApiItergration;
using eShopSolution.ViewModel.Validator;
using FluentValidation.AspNetCore;
using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Login
builder.Services.AddHttpClient();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.AccessDeniedPath = "/User/Forbidden/";
    });

// Add services to the container.
//Mutiple Language
var cultures = new[]
{
                new CultureInfo("en"),
                new CultureInfo("vi"),
                

            };
// Mutiple Language End
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation()
             //Vadilator
             //.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValaditor>())
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
             })
             .AddFluentValidation();

//Mutiple Language End
//Add API
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<ILoginApiClient, LoginApiClient>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.UseRequestLocalization();
app.UseEndpoints(endpoints =>
{

 

    endpoints.MapControllerRoute(
        name: "Login",
        pattern: "{culture=vi}/{controller=Login}/{action=Index}/{id?}");

    //endpoints.MapControllerRoute(
    //    name: "default",
    //    pattern: "{culture=vi}/{controller=Home}/{action=Index}/{id?}");
});

app.Run();
