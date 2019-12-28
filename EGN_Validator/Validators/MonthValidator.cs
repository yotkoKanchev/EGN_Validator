namespace EGN_Validator.Validators
{
    using System;

    public class MonthValidator : Validator
    {
        public override void Validate(string input)
        {
            var monthAsInt = int.Parse(input[2..4]);

            if ((monthAsInt > 12 && monthAsInt < 21) ||
                (monthAsInt > 32 && monthAsInt < 41) ||
                monthAsInt > 52 ||
                monthAsInt <= 0)
            {
                throw new ArgumentException("Невалиден месец!");
            }
        }
    }
}
