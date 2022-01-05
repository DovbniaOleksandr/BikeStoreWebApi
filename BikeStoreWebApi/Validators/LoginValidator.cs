using BikeStore.Core.Models;
using BikeStoreWebApi.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeStoreWebApi.Validators
{
    public class LoginValidator: AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(l => l.Email)
                .NotEmpty()
                .MaximumLength(50)
                .EmailAddress();

            RuleFor(l => l.Password)
                .NotEmpty();
        }
    }
}
