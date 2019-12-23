namespace EGN_Validator
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class StartUp
    {
        public static void Main()
        {
            var regions = new Dictionary<int, string>()
            {
                [43] = "Благоевград",           /* от 000 до 043 */
                [93] = "Бургас",                /* от 044 до 093 */
                [139] = "Варна",                /* от 094 до 139 */
                [169] = "Велико Търново",       /* от 140 до 169 */
                [183] = "Видин",                /* от 170 до 183 */
                [217] = "Враца",                /* от 184 до 217 */
                [233] = "Габрово",              /* от 218 до 233 */
                [281] = "Кърджали",             /* от 234 до 281 */
                [301] = "Кюстендил",            /* от 282 до 301 */
                [319] = "Ловеч",                /* от 302 до 319 */
                [341] = "Монтана",              /* от 320 до 341 */
                [377] = "Пазарджик",            /* от 342 до 377 */
                [395] = "Перник",               /* от 378 до 395 */
                [435] = "Плевен",               /* от 396 до 435 */
                [501] = "Пловдив",              /* от 436 до 501 */
                [527] = "Разград",              /* от 502 до 527 */
                [555] = "Русе",                 /* от 528 до 555 */
                [575] = "Силистра",             /* от 556 до 575 */
                [601] = "Сливен",               /* от 576 до 601 */
                [623] = "Смолян",               /* от 602 до 623 */
                [721] = "София - град",         /* от 624 до 721 */
                [751] = "София - окръг",        /* от 722 до 751 */
                [789] = "Стара Загора",         /* от 752 до 789 */
                [821] = "Добрич (Толбухин)",    /* от 790 до 821 */
                [843] = "Търговище",            /* от 822 до 843 */
                [871] = "Хасково",              /* от 844 до 871 */
                [903] = "Шумен",                /* от 872 до 903 */
                [925] = "Ямбол",                /* от 904 до 925 */
                [999] = "Друг/Неизвестен",      /* от 926 до 999 - Такъв регион понякога се ползва при
                                                                родени преди 1900, за родени в чужбина
                                                                или ако в даден регион се родят повече
                                                                деца от предвиденото. */
            };

            Console.WriteLine("Въведете ЕГН:");
            var input = Console.ReadLine();

            for (int i = 0; i < 5; i++)
            {
                if (i == 4)
                {
                    Console.WriteLine("Въведохте 5 невалидни ЕГН-та. Програмата приключва!");
                    return;
                }

                try
                {
                    ValidateInput(input);     
                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                    if (i == 3)
                    {
                        Console.WriteLine("Последен опит!");
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

                    Console.WriteLine("Въведете ново ЕГН: ");
                    input = Console.ReadLine();
                }
            }

            var currentDate = GetDateAsDateTime(input);

            var dayInBulgarian = currentDate.ToString("dddd", new CultureInfo("bg-BG"));
            var monthInBulgarian = currentDate.ToString("MMMM", new CultureInfo("bg-BG"));
            var currentYear = currentDate.Year;
            var sex = int.Parse(input[8].ToString()) % 2 == 0 ? "Мъж" : "Жена";

            var regionNumber = int.Parse(input.Substring(6, 3));
            var region = "";

            foreach (var area in regions)
            {
                if (regionNumber < area.Key)
                {
                    region = area.Value;
                    break;
                }
            }

            var postfix = sex == "Жена" ? "а" : "";

            Console.WriteLine();
            Console.WriteLine($"Информация за ЕГН: * {input} *");
            Console.WriteLine(new string('-', 60));

            var result = $"{sex}, роден{postfix} на {currentDate.Day} {monthInBulgarian} {currentYear}г.({dayInBulgarian}) в регион: {region}";

            Console.WriteLine(result);
            Console.WriteLine(new string('-', 60));
            Console.WriteLine();
        }

        private static DateTime GetDateAsDateTime(string input)
        {
            var year = int.Parse(input.Substring(0, 2));
            var month = int.Parse(input.Substring(2, 2));
            var day = int.Parse(input.Substring(4, 2));

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

        private static void ValidateInput(string input)
        {
            ValidateInputLength(input);
            ValidateInputSymbols(input);
            ValidateMonth(input);
            ValidateDay(input);
            
            var date = GetDateAsDateTime(input); // validating specific dates - as 30days months and February
                                                 // if is invalid will throw invalid system exc.
            ValidateControlDigit(input);
        }

        private static void ValidateControlDigit(string input)
        {
            var controlDigit = int.Parse(input[^1].ToString());
            var calculatedResult = CalculateResult(input.Substring(0, 9));

            if (controlDigit != calculatedResult)
            {
                throw new ArgumentException("Последната цифра от ЕГН не е валидна!");
            }
        }

        private static int CalculateResult(string v)
        {
            var EGNWeights = new int[] { 2, 4, 8, 5, 10, 9, 7, 3, 6 };
            var result = 0;

            for (int i = 0; i < 9; i++)
            {
                result += int.Parse(v[i].ToString()) * EGNWeights[i];
            }

            result -= ((result / 11) * 11);
            return result == 10 ? 0 : result;
        }

        private static void ValidateDay(string input)
        {
            var dayAsInt = int.Parse(input.Substring(4, 2));

            if (dayAsInt < 0 || dayAsInt > 31)
            {
                throw new ArgumentException("Невалиден ден!");
            }
        }

        private static void ValidateMonth(string input)
        {
            var monthAsInt = int.Parse(input.Substring(2, 2));

            if ((monthAsInt > 12 && monthAsInt < 21) || (monthAsInt > 32 && monthAsInt < 41) || monthAsInt > 52)
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
