using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Constants.LearningProcess
{
    public static class UserQueryAnalyzeRemarks
    {
        public static readonly char[] SqlQuerySplitters = [' ', ',', '.', '(', ')', ';'];

        public const string KeywordsIncorrectOrderRemark = "Служебные слова расположены не в правильном порядке";
        public const string MissingKeywordRemark = "Вы не добавили в свой запрос служебное слово {0}";
    }
}
