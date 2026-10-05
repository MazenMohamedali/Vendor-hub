using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendorHub.Application.Features.Products.CreateProduct
{
    public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(200).WithMessage("Product name must not exceed 200 characters.");                                                                           


            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Product price must be greater than zero.");

            RuleFor(x => x.Currency)
                .NotEmpty().WithMessage("Currency code is required.")
                .Length(3).WithMessage("Currency code must be exactly 3 characters (e.g. EGP, USD).");                                                                            


            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("Initial quantity cannot be negative.");                                                                                      


            RuleFor(x => x.VendorId)
                .NotEmpty().WithMessage("A valid vendor is required.");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("A valid category is required.");
        }
    }
}