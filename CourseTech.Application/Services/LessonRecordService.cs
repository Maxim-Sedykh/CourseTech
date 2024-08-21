using CourseTech.Domain.Dto.UserProfile;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Services
{
    public class LessonRecordService : ILessonRecordService
    {
        public Task<CollectionResult<LessonRecordDto>> GetLessonRecordsAsync(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
