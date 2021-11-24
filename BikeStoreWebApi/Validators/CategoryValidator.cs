using BikeStoreWebApi.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeStoreWebApi.Validators
{
    public class CategoryValidator: AbstractValidator<SaveCategoryDto>
    {
        public CategoryValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
