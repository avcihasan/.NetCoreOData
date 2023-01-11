using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using NetCoreOData.API.Models;

namespace NetCoreOData.API.Controllers
{

    public class ProductsController : ODataController
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [EnableQuery]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Products);
        }

        //[EnableQuery]
        //[HttpGet]
        //public IActionResult Get([FromODataUri] int key)
        //{
        //    return Ok(_context.Products.Where(x=>x.Id==key));
        //}

        [ODataRoute("Products({productId})")]
        [EnableQuery]
        [HttpGet]
        public IActionResult CustomGetId([FromODataUri] int productId)
        {
            return Ok(_context.Products.Where(x => x.Id == productId));
        }

        [EnableQuery]
        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return Ok(product);
        }

        [EnableQuery]
        [HttpPut]
        public IActionResult Put([FromODataUri]int key,[FromBody] Product product)
        {
            product.Id = key;
            _context.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return NoContent();
        }

        [EnableQuery]
        [HttpDelete]
        public IActionResult Delete([FromODataUri] int key)
        {
            Product p = _context.Products.Find(key);
            if (p==null)
            {
                return NotFound();
            }
            _context.Entry(p).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            _context.SaveChanges();
            return NoContent();
        }
    }
}
