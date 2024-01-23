using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.API.IRepo;
using Order.API.KafkaConsumer;
using Order.API.Models;

namespace Order.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public readonly IOrderRepo _orderRepo;
        public readonly IKafkaConsumer _kafkaConsumer;

        public OrderController(IOrderRepo orderRepo, IKafkaConsumer kafkaConsumer)
        {
            _orderRepo = orderRepo;
            _kafkaConsumer = kafkaConsumer;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                //_kafkaConsumer.ConsumeMessages();
                return Ok(await _orderRepo.GetOrders());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            try
            {
                return Ok(await _orderRepo.GetOrder(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(Orders order)
        {
            try
            {
                return Ok(await _orderRepo.CreateOrder(order));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                return Ok(await _orderRepo.DeleteOrder(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
