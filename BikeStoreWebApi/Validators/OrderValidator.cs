using BikeStoreWebApi.DTOs.Order;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeStoreWebApi.Validators
{
    public class OrderValidator: AbstractValidator<SaveOrderDto>
    {
        public OrderValidator()
        {
            RuleFor(b => b.BikeId)
                .NotEmpty().WithMessage("'Bike Id' must not be 0.");

            RuleFor(b => b.UserId)
                .NotEmpty().WithMessage("'User Id' must not be 0.");
        }
    }
}
