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
    public class WarehousesController : ControllerBase
    {
        private IWarehouseService _service;
        
        public WarehousesController(IWarehouseService service){
            _service = service;
        }

        [HttpPost]
        public IActionResult CompleteOrder(Order order)
        {
            if(!_service.DoesProductExist(order.IdProduct).Result){
                return NotFound("Wrong Product id");
            }
            if(!_service.DoesWarehouseExist(order.IdProduct).Result){
                return NotFound("Wrong Warehouse id");
            }
            if(!_service.DoesOrderExist(order).Result){
                return NotFound("No Order");
            }
            if(_service.DoesOrderDone(order).Result){
                 return BadRequest("Order already done");
            }
            _service.UpdateFulfilledAt(order);
            var id = _service.InsertProduct(order);
           
            return Ok(id);
        }
    }
}