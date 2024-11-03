using System.Data;

namespace CourseTech.Domain.Helpers
{
    /// <summary>
    /// Сравниватель двух DataTable.
    /// Для проверки ответа пользователя на вопрос практического типы.
    /// Для того чтобы сравнивать результат запроса пользователя, и результат корректного запроса.
    /// </summary>
    public class DataTableComparer : IComparer<DataTable>
    {
        /// <summary>
        /// Сравнить результат запроса пользователя, и результат корректного запроса.
        /// Для вопроса практического типа.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(DataTable x, DataTable y)
        {
            if (x.Rows.Count != y.Rows.Count || x.Columns.Count != y.Columns.Count)
            {
                return -1;
            }

            for (int i = 0; i < x.Rows.Count; i++)
            {
                for (int j = 0; j < x.Columns.Count; j++)
                {
                    if (!x.Rows[i][j].Equals(y.Rows[i][j]))
                    {
                        return -1;
                    }
                }
            }

            return 0;
        }
    }
}
