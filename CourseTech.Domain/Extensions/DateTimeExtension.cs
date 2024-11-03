using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Extensions
{
    /// <summary>
    /// Расширение для DateTime.
    /// </summary>
    public static class DateTimeExtension
    {
        /// <summary>
        /// Вычислить количество лет с текущей даты до dateTime.
        /// </summary>
        /// <param name="dateTime">Дата</param>
        /// <returns>Количество полных лет</returns>
        public static int GetYearsByDateToNow(this DateTime dateTime)
        {
            var today = DateTime.Today;
            int age = today.Year - dateTime.Year;

            // Проверка, был ли день рождения в этом году
            if (dateTime > today.AddYears(-age)) age--;

            return age;
        }
    }
}
