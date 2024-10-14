using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Constants.LearningProcess
{
    public static class AnalysParts
    {
        public const string UndefinedOverall = "Извините, данные вашего анализа были утеряны";
        public const string SatisfactoryOverall = "Данный курс вы прошли удовлетворительно";
        public const string GoodOverall = "Вы очень хорошо прошли курс";
        public const string ExcellentOverall = "У вас отличный результат! Так держать!!!";
        public const string UnsatisfactoryOverall = @"Вы прошли этот тест неудовлетворительно. Но не сдавайтесь! Вы всегда рано
                                                или поздно достигните успеха, если будете стараться!";

        public const string LessonsAnalys = @"Ваша средняя оценка по обычным занятиям {0}
                        из 15 возможных. Вы набрали максимальное количество баллов по уроку {1} - {2} баллов.
                        Минимальное по уроку {3} баллов,
                        вы набрали {4} баллов по экзамену из 40б. 
                        За одно тестовое задание можно было получить 1.5 балла, за открытое 3.5.";
    }
}
