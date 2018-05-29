using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PaymentGateway.Models;
using PaymentGateway.Repositories;

namespace PaymentGateway.Controllers
{
    [Produces("application/json")]
    public class ProductController : BaseController<Product>
    {
        public ProductController(IConfiguration config) : base(config)
        {
           
        }
        [Route("api/GetEmployeeList")]
        public ActionResult GetAll()
        {                     
            var result = modelRepository.GetAll();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

    }
}