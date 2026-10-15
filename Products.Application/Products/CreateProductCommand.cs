
using MediatR;
using Products.Application.DTOs;
using Products.Application.Interfaces;
using Products.Domain.Entities;

namespace Products.Application.Products
{
    public sealed record CreateProductCommand(string Name, decimal Price) : IRequest<ProductDto>;

    public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product(request.Name, request.Price);

            await _productRepository.AddAsync(product, cancellationToken);

            return new ProductDto(product.Id, product.Name, product.Price);
        }
    }
}
