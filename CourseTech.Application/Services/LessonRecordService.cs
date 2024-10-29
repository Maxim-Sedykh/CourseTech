using AutoMapper;
using CourseTech.Application.Queries.LessonRecordQueries;
using CourseTech.Application.Resources;
using CourseTech.Domain.Constants.Cache;
using CourseTech.Domain.Dto.LessonRecord;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Cache;
using CourseTech.Domain.Interfaces.Repositories;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.Services
{
    public class LessonRecordService(ICacheService cacheService, IMediator mediator) : ILessonRecordService
    {
        public async Task<CollectionResult<LessonRecordDto>> GetUserLessonRecordsAsync(Guid userId)
        {
            var userLessonRecords = await cacheService.GetOrAddToCache(
                $"{CacheKeys.UserLessonRecords}{userId}",
                async () => await mediator.Send(new GetLessonRecordDtosByUserIdQuery(userId)));

            if (!userLessonRecords.Any())
            {
                return CollectionResult<LessonRecordDto>.Failure((int)ErrorCodes.LessonRecordsNotFound, ErrorMessage.LessonRecordsNotFound);
            }

            return CollectionResult<LessonRecordDto>.Success(userLessonRecords);
        }
    }
}
