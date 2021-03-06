﻿namespace EGN_Validator.Validators
{
    using System;
    using System.Text.RegularExpressions;

    public class SymbolsValidator : IValidator
    {
        public void Validate(string input)
        {
            var match = Regex.Match(input, @"^[0-9]{10}$"); // if using LINQ validation coment this line !

            if (!match.Success)  /*(!input.All(char.IsDigit)) */
            {
                throw new ArgumentException("EGN трябва да съдържа само неотрицателни цифри!");
            }
        }
    }
}
