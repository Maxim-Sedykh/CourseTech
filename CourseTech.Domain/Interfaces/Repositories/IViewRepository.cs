using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Репозиторий для работы с представлениями (Views)
    /// </summary>
    /// <typeparam name="TView"></typeparam>
    public interface IViewRepository<TView>
    {
        Task<List<TView>> GetAllFromViewAsync();
    }
}
