using System.Text.RegularExpressions;

namespace PocketCounter.Domain.Extensions
{
    public static class StringExtensions
    {
        public static bool IsValidHumanName(this string name)
        {
            // имя должно содержать только буквы и пробелы
            var regex = new Regex(@"^[A-Za-zА-Яа-яЁёs-]+$");
            return regex.IsMatch(name);
        }
    }
}
