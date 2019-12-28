namespace EGN_Validator.Validators
{
    using System;

    public class LengthValidator : Validator
    {
        public override void Validate(string input)
        {
            if (input.Length != 10)
            {
                throw new ArgumentException("Дължината на ЕГН трябва да е точно 10 символа!");
            }
        }
    }
}
