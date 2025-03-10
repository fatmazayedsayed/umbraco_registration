using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Web.Website.Controllers;
using Umbraco.Cms.Infrastructure.Scoping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using umbraco_registration.Models;

public class RegistrationController : SurfaceController
{
    private readonly IScopeProvider _scopeProvider;

    public RegistrationController(
        IScopeProvider scopeProvider,
        IUmbracoContextAccessor umbracoContextAccessor,
       IUmbracoDatabaseFactory databaseFactory,
       ServiceContext services,
       AppCaches appCaches,
       IProfilingLogger profilingLogger,
       IPublishedUrlProvider publishedUrlProvider)
       : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
    {
        _scopeProvider = scopeProvider;

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
     public IActionResult Register (RegistrationModel model)
    {


        try
        {
            using (var scope = _scopeProvider.CreateScope())
            {
                var database = scope.Database;
                database.Execute(
     "INSERT INTO Registration (Name, Email, ProductId, CreatedDate) VALUES (@Name, @Email, @ProductId, @CreatedDate)",
     new { model.Name, model.Email, model.ProductId, CreatedDate = DateTime.Now });


                scope.Complete();
            }

            TempData["Success"] = "Registration successful!";
            return RedirectToCurrentUmbracoPage(); // Correct for POST
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "An error occurred. Please try again.");
            return CurrentUmbracoPage();
        }
    }
    public IActionResult ThankYou()
    {
        return View(); // Display a thank-you page
    }



[HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Submit(RegistrationModel model)
    {
        

        try
        {
            using (var scope = _scopeProvider.CreateScope())
            {
                var database = scope.Database;
                database.Execute(
                    "INSERT INTO Registration (Name, Email,productId) VALUES (@Name, @Email,@productId)",
                    new { model.Name, model.Email,model.ProductId });

                scope.Complete();
            }

            TempData["Success"] = "Registration successful!";
            return RedirectToCurrentUmbracoPage();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "An error occurred. Please try again.");
            return CurrentUmbracoPage();
        }
    }


    public IActionResult Register0(string name, string email, string password)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ModelState.AddModelError("", "All fields are required.");
            return CurrentUmbracoPage();
        }

        try
        {
            using (var scope = _scopeProvider.CreateScope())
            {
                var database = scope.Database;
                database.Execute(
                    "INSERT INTO Registration (Name, Email, Password) VALUES (@Name, @Email, @Password)",
                    new { name, email, password });

                scope.Complete();
            }

            TempData["Success"] = "Registration successful!";
            return RedirectToCurrentUmbracoPage();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "An error occurred. Please try again.");
            return CurrentUmbracoPage();
        }
    }

}
