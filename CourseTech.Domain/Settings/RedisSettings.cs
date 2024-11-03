using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Settings
{
    /// <summary>
    /// Настройка кэша redis
    /// </summary>
    public class RedisSettings
    {
        public string Url { get; set; }

        public string InstanceName { get; set; }
    }
}
