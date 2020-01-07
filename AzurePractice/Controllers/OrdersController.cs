using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzurePractice.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzurePractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        [HttpPost]
        public async Task PutOrder([FromBody] Order order)
        {
            var orderRepository = new OrderRepository();
            await orderRepository.PutOrder(order);
        }

    }
}