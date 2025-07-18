using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Nels.Controllers;

[Route("")]
public class RootController : AbpController
{
    public ActionResult Index()
    {
        return Redirect("/index.html");
    }
}