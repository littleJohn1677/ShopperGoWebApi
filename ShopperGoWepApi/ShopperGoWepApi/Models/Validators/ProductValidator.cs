// ===============================================================
// File name: ProductValidator.cs
// Copyright (c) 2022 - ShopperGoWepApi - Ivan Vanogi
// Creation date: 2022.11.28
// ===============================================================

using FluentValidation;

using ShopperGoWepApi.Models.Entities;

namespace ShopperGoWepApi.Models.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        //public const string CF_REGEX = "[A-Za-z]{6}[0-9LMNPQRSTUV]{2}[A-Za-z]{1}[0-9LMNPQRSTUV]{2}[A-Za-z]{1}[0-9LMNPQRSTUV]{3}[A-Za-z]{1}";
        //public const string PI_REGEX = "[0-9]{11}";

        public ProductValidator() 
        {
            RuleFor(product => product.Name).NotNull().NotEmpty().Length(1, 255)
                .WithMessage("Deve essere inserito il nome del prodotto, che non deve essere maggiore di 255 caratteri.");

            RuleFor(product => product.Description).NotNull().NotEmpty().Length(1, 1024)
                .WithMessage("Deve essere una descrizione del prodotto, che non deve essere maggiore di 1024 caratteri.");

            RuleFor(product => product.Price).NotNull() 
                .WithMessage($"Deve essere inserito il prezzo.")
                .Must(money => money.Amount > 0.10M)
                .WithMessage($"Il prezzo che deve essere maggiore o uguale a 10 centesimi");

            RuleFor(product => product.Quantity).NotNull()
                .WithMessage($"Deve essere inserita la quantità.")
                .Must(quatity => quatity > 9)
                .WithMessage($"La quantita minima deve essere di 10 pezzi.");

        }
    }
}
