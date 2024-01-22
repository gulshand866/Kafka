using Microsoft.AspNetCore.Mvc;
using Product.API.IRepo;
using Product.API.KafkaProducer;
using Product.API.Models;

namespace Product.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        public readonly IProductRepo _productRepo;
        public readonly IKafkaProducer _kafkaProducer;

        public ProductController(IProductRepo productRepo, IKafkaProducer kafkaProducer)
        {
            _productRepo = productRepo;
            _kafkaProducer = kafkaProducer;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                return Ok(await _productRepo.GetProducts());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                return Ok(await _productRepo.GetProduct(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Products product)
        {
            try
            {
               return Ok(await _productRepo.CreateProduct(product));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(Products product)
        {
            try
            {
                return Ok(await _productRepo.UpdateProduct(product));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                return Ok(await _productRepo.DeleteProduct(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PostOrder/{id}")]
        public async Task<IActionResult> AddOrder(int id)
        {
            var product = await _productRepo.GetProduct(id);
            await _kafkaProducer.ProduceMessage(new {  product.Id, product.Price, product.Quantity });
            return Ok("Creating Order...");
        }

    }
}
