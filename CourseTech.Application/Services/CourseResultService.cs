using CourseTech.Domain.Dto.FinalResult;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;

namespace CourseTech.Application.Services
{
    public class CourseResultService : ICourseResultService
    {
        public Task<BaseResult<CourseResultDto>> GetCourseResultAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult<UserAnalysDto>> GetUserAnalys(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
