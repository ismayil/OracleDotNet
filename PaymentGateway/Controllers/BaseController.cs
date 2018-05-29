using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PaymentGateway.Repositories;

namespace PaymentGateway.Controllers
{
    public class BaseController<T> : Controller
    {
        public ModelRepository<T> modelRepository;
        public BaseController(IConfiguration config)
        {
            modelRepository = new ModelRepository<T>(config);
        }

    }
}