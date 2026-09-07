using Microsoft.AspNetCore.Mvc;

namespace ContosoUniversity.WebAPI.Controllers
{
    public class BaseController(IConfiguration configuration) : ControllerBase
    {
        protected readonly int _defaultPageSize = configuration.GetValue("PageSize", 3);
    }
}
