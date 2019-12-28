namespace EGN_Validator.Validators
{
    using System;

    public class ControlDigitValidator : Validator
    {
        public override void Validate(string input)
        {
            var controlDigit = int.Parse(input[^1].ToString());
            var calculatedResult = CalculateResult(input[0..8]);

            if (controlDigit != calculatedResult)
            {
                throw new ArgumentException("Последната цифра от ЕГН не е валидна!");
            }
        }

        private int CalculateResult(string input)
        {
            var EGNWeights = new int[] { 2, 4, 8, 5, 10, 9, 7, 3, 6 };
            var result = 0;

            for (int i = 0; i < 9; i++)
            {
                result += int.Parse(input[i].ToString()) * EGNWeights[i];
            }

            result -= ((result / 11) * 11);
            return result == 10 ? 0 : result;
        }
    }
}
