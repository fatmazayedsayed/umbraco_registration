using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Cms.Web.Common.Controllers;

 public class RegistrationApiController : UmbracoApiController
{
    private readonly IScopeProvider _scopeProvider;

    public RegistrationApiController(IScopeProvider scopeProvider)
    {
        _scopeProvider = scopeProvider;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        using (var scope = _scopeProvider.CreateScope())
        {
            var database = scope.Database;
            var registrations = database.Fetch<dynamic>("SELECT Name, Email, CreatedDate FROM Registration");
            return Ok(registrations);
        }
    }
}
