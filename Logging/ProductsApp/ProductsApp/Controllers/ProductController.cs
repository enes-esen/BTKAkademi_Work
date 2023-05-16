using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsApp.Models;

namespace ProductsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAllProduct()
        {
            var product = new List<Product>()
            {
                new Product() {Id = 1, ProductName = "Computer"},
                new Product() {Id = 2, ProductName = "Keyboard"},
                new Product() {Id = 3, ProductName = "Mouse"}
            };

            //Log bilgisi düşeriz.
            _logger.LogInformation("GetAllProduct action has been called.");
            var test = product;
            return Ok(product);
        }

    }
}
