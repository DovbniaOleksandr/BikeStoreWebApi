﻿using BikeStore.Core.Enums;
using BikeStoreWebApi.DTOs.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeStoreWebApi.Validators
{
    public class AdminRegistationValidator : AbstractValidator<RegistrationUserDto>
    {
        List<string> adminRoles = new List<string>() { Roles.Admin };

        public AdminRegistationValidator()
        {
            RuleFor(l => l.UserName)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(l => l.Email)
                .NotEmpty()
                .MaximumLength(50)
                .EmailAddress();

            RuleFor(l => l.Password)
                .NotEmpty().WithMessage("Your password cannot be empty")
                .MinimumLength(8).WithMessage("Your password length must be at least 8.")
                .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
                .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.");

            RuleFor(l => l.Role)
                .NotEmpty()
                .Must(x => adminRoles.Contains(x));
        }
    }
}
