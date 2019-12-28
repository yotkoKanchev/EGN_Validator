namespace EGN_Validator.Validators
{
    using System;

    public class DayValidator : Validator
    {
        public override void Validate(string input)
        {
            var dayAsInt = int.Parse(input[4..6]);

            if (dayAsInt <= 0 || dayAsInt > 31)
            {
                throw new ArgumentException("Невалиден ден!");
            }
        }
    }
}
