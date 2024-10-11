using AutoMapper;
using CourseTech.Application.Resources;
using CourseTech.Domain.Dto.LessonRecord;
using CourseTech.Domain.Dto.User;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Repositories;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Services
{
    public class LessonRecordService(IBaseRepository<LessonRecord> lessonRecordRepository, IMapper mapper) : ILessonRecordService
    {
        public async Task<CollectionResult<LessonRecordDto>> GetLessonRecordsAsync(Guid userId)
        {
            var userLessonRecords = await lessonRecordRepository.GetAll()
                .Where(x => x.UserId == userId)
                .Include(x => x.Lesson)
                .Select(x => mapper.Map<LessonRecordDto>(x))
                .ToArrayAsync();

            if (!userLessonRecords.Any())
            {
                return CollectionResult<LessonRecordDto>.Failure((int)ErrorCodes.LessonRecordsNotFound, ErrorMessage.LessonRecordsNotFound);
            }

            return CollectionResult<LessonRecordDto>.Success(userLessonRecords);
        }
    }
}
