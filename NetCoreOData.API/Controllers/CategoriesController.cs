using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using NetCoreOData.API.Models;

namespace NetCoreOData.API.Controllers
{

    public class CategoriesController : ODataController
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        [EnableQuery]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Categories);
        }


        [EnableQuery]
        [HttpPost]
        public IActionResult TotalPrice([FromODataUri] int key)
        {
            var total = _context.Products.Where(x => x.CategoryId == key).Sum(x => x.Price);

            return Ok(total);
        }

        [EnableQuery]
        [HttpPost]
        public IActionResult TotalPrice1()
        {
            var total = _context.Products.Sum(x => x.Price);

            return Ok(total);
        }


        [EnableQuery]
        [HttpPost]
        public IActionResult TotalWithParameters(ODataActionParameters parameters)
        {
            int id = (int)parameters["categoryId"];
            var total = _context.Products.Where(x => x.CategoryId == id).Sum(x => x.Price);

            return Ok(total);
        }


        [EnableQuery]
        [HttpPost]
        public IActionResult Toplama(ODataActionParameters parameters)
        {
            int a = (int)parameters["a"];
            int b = (int)parameters["b"];
            int c = (int)parameters["c"];

            return Ok(a + b + c);
        }

        [EnableQuery]
        [HttpPost]
        public IActionResult LoginUser(ODataActionParameters parameters)
        {
            Login login = parameters["login"] as Login;

            return Ok(login.Email + " - " + login.Password);
        }


        [EnableQuery]
        [HttpGet("odata/Categories/CategoryCount")]
        public IActionResult CategoryCount()
        {
            var total = _context.Categories.Count();
            return Ok(total);
        }


        [EnableQuery]
        [HttpGet]
        public IActionResult Carpma([FromODataUri]int a, [FromODataUri] int b, [FromODataUri] int c)
        {
            return Ok(a * b * c);
        }

        [ODataRoute("Kdv")]
        [EnableQuery]
        [HttpGet("odata/Kdv")]
        public IActionResult Kdv()
        {
            return Ok(10);
        }

    }
}
