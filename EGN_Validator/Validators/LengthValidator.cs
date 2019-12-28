namespace EGN_Validator.Validators
{
    using System;

    public class LengthValidator : IValidator
    {
        public void Validate(string input)
        {
            if (input.Length != 10)
            {
                throw new ArgumentException("Дължината на ЕГН трябва да е точно 10 символа!");
            }
        }
    }
}
