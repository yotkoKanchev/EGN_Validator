namespace EGN_Validator
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;

    using Validators;

    public class StartUp
    {
        public static void Main()
        {
            var validator = new Validator();
            var regions = new RegionsRepository();

            Console.WriteLine("Въведете ЕГН:");
            var input = Console.ReadLine();

            for (int i = 0; i < 5; i++)
            {
                if (i == 5)
                {
                    Console.WriteLine("Въведохте 5 невалидни ЕГН-та. Програмата приключва!");
                    return;
                }

                try
                {
                    validator.Validate(input);

                    //validating valid day and month via DateTime:
                    var currentDate = GetDateAsDateTime(input);
                    var result = GenerateOutput(currentDate, input, regions);

                    Console.WriteLine(result);
                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);

                    if (i == 3)
                    {
                        Console.WriteLine("Последен опит!");
                    }

                    if (i == 4)
                    {
                        Console.WriteLine("Въведохте 5 невалидни ЕГН-та. Програмата приключва!");
                        return;
                    }

                    Console.WriteLine("Въведете ново ЕГН: ");
                    input = Console.ReadLine();
                }
                catch (SystemException)
                {
                    Console.WriteLine("Несъществуваща дата!");

                    if (i == 3)
                    {
                        Console.WriteLine("Последен опит!");
                    }

                    if (i == 4)
                    {
                        Console.WriteLine("Въведохте 5 невалидни ЕГН-та. Програмата приключва!");
                        return;
                    }

                    Console.WriteLine("Въведете ново ЕГН: ");
                    input = Console.ReadLine();
                }
            }
            // waits for input to keep the console on when run .exe file
            Console.ReadKey();
        }

        private static string GenerateOutput(DateTime currentDate, string input, RegionsRepository regions)
        {
            var sb = new StringBuilder();
            var dayInBulgarian = currentDate.ToString("dddd", new CultureInfo("bg-BG"));
            var monthInBulgarian = currentDate.ToString("MMMM", new CultureInfo("bg-BG"));
            var currentYear = currentDate.Year;
            var sex = int.Parse(input[8].ToString()) % 2 == 0 ? "Мъж" : "Жена";
            var region = GetRegion(input, regions);
            var postfix = sex == "Жена" ? "а" : "";

            sb.AppendLine();
            sb.AppendLine($"Информация за ЕГН: * {input} *");
            sb.AppendLine(new string('-', 60));

            var result = $"{sex}, роден{postfix} на {currentDate.Day} {monthInBulgarian} " +
                $"{currentYear}г.({dayInBulgarian}) в регион: {region}";

            sb.AppendLine(result);
            sb.AppendLine(new string('-', 60));

            return sb.ToString();
        }

        private static string GetRegion(string input, RegionsRepository regions)
        {
            var regionNumber = int.Parse(input[6..9]);

            foreach (KeyValuePair<int, string> area in regions)
            {
                if (regionNumber < area.Key)
                {
                    return area.Value;
                }
            }

            return null;
        }

        private static DateTime GetDateAsDateTime(string input)
        {
            var year = int.Parse(input[0..2]);
            var month = int.Parse(input[2..4]);
            var day = int.Parse(input[4..6]);

            if (month <= 12)
            {
                year += 1900;
            }
            else if (month <= 32)
            {
                year += 1800;
                month -= 20;
            }
            else if (month <= 52)
            {
                year += 2000;
                month -= 40;
            }

            return new DateTime(year, month, day);
        }
    }
}
