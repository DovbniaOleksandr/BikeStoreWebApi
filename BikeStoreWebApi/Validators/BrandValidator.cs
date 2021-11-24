using BikeStoreWebApi.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeStoreWebApi.Validators
{
    public class BrandValidator: AbstractValidator<SaveBrandDto>
    {
        public BrandValidator()
        {
            RuleFor(b => b.BrandName)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
