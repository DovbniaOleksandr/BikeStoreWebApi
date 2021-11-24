using BikeStoreWebApi.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeStoreWebApi.Validators
{
    public class BikeValidator: AbstractValidator<SaveBikeDto>
    {
        public BikeValidator()
        {
            RuleFor(b => b.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(b => b.CategoryId)
                .NotEmpty()
                .WithMessage("'Category Id' must not be 0.");

            RuleFor(b => b.BrandId)
                .NotEmpty()
                .WithMessage("'Brand Id' must not be 0.");
        }
    }
}
