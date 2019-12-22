namespace EGN_Validator
{
    using System;
    using System.Globalization;
    using System.Linq;

    public class StartUp
    {
        public static void Main()
        {
            Console.WriteLine("Въведете ЕГН: ");

            var input = Console.ReadLine();

            for (int i = 0; i < 5; i++)
            {
                if (i == 4)
                {
                    return;
                }

                if (i == 3)
                {
                    Console.WriteLine("Последен опит!");
                }

                try
                {
                    ValidateInputLength(input);
                    ValidateInputSymbols(input);

                    var month = input.Substring(2, 2);
                    var day = input.Substring(4, 2);
                    var areaPart = input.Substring(6, 3);
                    var controlDigit = input[input.Length - 1];


                    ValidateMonth(month);
                    ValidateDay(day);
                    var date = DateTime.ParseExact(input.Substring(0, 6), "yyMMdd", CultureInfo.InvariantCulture);
                    ValidateControlDigit(input);

                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                    input = Console.ReadLine();
                }
                catch (SystemException)
                {
                    Console.WriteLine("Невалиден ден!");
                    input = Console.ReadLine();
                }
            }

            var currentDate = DateTime.ParseExact(input.Substring(0, 6), "yyMMdd", CultureInfo.InvariantCulture);

            var dayInBulgarian = currentDate.ToString("dddd", new CultureInfo("bg-BG"));
            var monthInBulgarian = currentDate.ToString("MMMM", new CultureInfo("bg-BG"));
            var currentYear = currentDate.Year;
            var sex = int.Parse(input[9].ToString()) % 2 == 0 ? "Жена" : "Мъж";
            var result = $"{sex}, роден на {currentDate.Day}({dayInBulgarian}) {monthInBulgarian} {currentYear}г. в ...";
            Console.WriteLine($"Информация за ЕГН: * {input} *");
            Console.WriteLine(new string('-', 80));
            Console.WriteLine(result);
            Console.WriteLine(new string('-', 80));

        }

        private static void ValidateControlDigit(string input)
        {
            var controlDigit = int.Parse(input[input.Length - 1].ToString());

            var calculatedResult = CalculateResult(input.Substring(0, 9));

            if (controlDigit != calculatedResult)
            {
                throw new ArgumentException("Последната цифра от ЕГН не е валидна!");
            }
        }

        private static int CalculateResult(string v)
        {
            var result = int.Parse(v[0].ToString()) * 2;
            result += int.Parse(v[1].ToString()) * 4;
            result += int.Parse(v[2].ToString()) * 8;
            result += int.Parse(v[3].ToString()) * 5;
            result += int.Parse(v[4].ToString()) * 10;
            result += int.Parse(v[5].ToString()) * 9;
            result += int.Parse(v[6].ToString()) * 7;
            result += int.Parse(v[7].ToString()) * 3;
            result += int.Parse(v[8].ToString()) * 6;

            result = result - ((result / 11) * 11);

            return result == 10 ? 0 : result;
        }

        private static void ValidateDay(string day)
        {
            var dayAsInt = int.Parse(day);

            if (dayAsInt < 0 || dayAsInt > 31)
            {
                throw new ArgumentException("Невалиден ден!");
            }
        }

        private static void ValidateMonth(string month)
        {
            var monthAsInt = int.Parse(month);

            if ((monthAsInt > 12 && monthAsInt < 21) || (monthAsInt > 32 && monthAsInt < 41) || monthAsInt > 42)
            {
                throw new ArgumentException("Невалиден месец!");
            }
        }

        private static void ValidateInputSymbols(string input)
        {
            if (!input.All(char.IsDigit))
            {
                throw new ArgumentException("EGN трябва да съдъдржа само цифри!");
            }
        }

        private static void ValidateInputLength(string input)
        {
            if (input.Length != 10)
            {
                throw new ArgumentException("Дължината на ЕГН трябва да е точно 10 символа!");
            }
        }
    }
}
