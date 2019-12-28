using System;

namespace EGN_Validator.Validators
{
    public class Validator : IValidator
    {
        private LengthValidator lengthValidator;
        private SymbolsValidator symbolsValidator;
        private MonthValidator monthValidator;
        private DayValidator dayValidator;
        private ControlDigitValidator controlDigitValidator;

        public Validator()
        {
            this.lengthValidator = new LengthValidator();
            this.symbolsValidator = new SymbolsValidator();
            this.monthValidator = new MonthValidator();
            this.dayValidator = new DayValidator();
            this.controlDigitValidator = new ControlDigitValidator();
        }

        public virtual void Validate(string input)
        {
            this.lengthValidator.Validate(input);
            this.symbolsValidator.Validate(input);
            this.monthValidator.Validate(input);
            this.dayValidator.Validate(input);
            this.controlDigitValidator.Validate(input);

        }
    }
}
