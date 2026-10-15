using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Products.Application.DTOs;
using Products.Application.Products;

namespace Products.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ISender sender, ILogger<ProductsController> logger)
        {
            _sender = sender;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetAll(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving all products.");

            var products = await _sender.Send(new GetAllProductsQuery(), cancellationToken);

            _logger.LogInformation("Retrieved {ProductCount} products.", products.Count);

            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetById(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving product with ID {ProductId}.", id);

            var product = await _sender.Send(new GetProductByIdQuery(id), cancellationToken);

            if (product is null)
            {
                _logger.LogWarning("Product with ID {ProductId} was not found.", id);
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var product = await _sender.Send(command, cancellationToken);

                _logger.LogInformation("Product with ID {ProductId} was created successfully.", product.Id);

                return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
            }
            catch (ValidationException exception)
            {
                _logger.LogWarning("Product creation failed validation.");

                var errors = exception.Errors
                    .GroupBy(error => error.PropertyName)
                    .ToDictionary(group => group.Key, group => group.Select(error => error.ErrorMessage).ToArray());

                return BadRequest(new { Errors = errors });
            }
            catch (ArgumentException exception)
            {
                _logger.LogWarning("Product creation failed because the product data was invalid.");

                return BadRequest(new { Error = exception.Message });
            }
        }
    }

}
