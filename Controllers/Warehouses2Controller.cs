using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cw5.Models;
using cw5.services;
using Microsoft.AspNetCore.Mvc;

namespace cw5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Warehouses2Controller : ControllerBase
    {
        private IWarehouseService _service;
        
        public Warehouses2Controller(IWarehouseService service){
            _service = service;
        }

        [HttpPost]
        public IActionResult CompleteOrder(Order order)
        {
            var id = _service.StoredProcedure(order);
            return Ok(id);
        }
    }
}