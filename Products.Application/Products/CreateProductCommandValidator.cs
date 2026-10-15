
using FluentValidation;

namespace Products.Application.Products
{
    public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(product => product.Name)
                .NotEmpty()
                .WithMessage("Product name is required.");

            RuleFor(product => product.Price)
                .GreaterThan(0)
                .WithMessage("Product price must be greater than zero.");
        }
    }
}
