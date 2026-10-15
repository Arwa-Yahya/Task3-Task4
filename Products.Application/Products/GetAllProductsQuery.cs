
using MediatR;
using Products.Application.DTOs;
using Products.Application.Interfaces;

namespace Products.Application.Products
{
    public sealed record GetAllProductsQuery : IRequest<IReadOnlyList<ProductDto>>;

    public sealed class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IReadOnlyList<ProductDto>>
    {
        private readonly IProductRepository _productRepository;

        public GetAllProductsQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IReadOnlyList<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllAsync(cancellationToken);

            return products
                .Select(product => new ProductDto(product.Id, product.Name, product.Price))
                .ToList();
        }
    }
}
