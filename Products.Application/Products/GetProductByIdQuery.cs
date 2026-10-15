
using MediatR;
using Products.Application.DTOs;
using Products.Application.Interfaces;

namespace Products.Application.Products
{
    public sealed record GetProductByIdQuery(int Id) : IRequest<ProductDto?>;

    public sealed class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);

            if (product is null)
                return null;

            return new ProductDto(product.Id, product.Name, product.Price);
        }
    }
}
