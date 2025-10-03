using CourseTech.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Interfaces.Services
{
    public interface ISessionService
    {
        Task<Session> StartSessionAsync(Guid userId, SessionConfig config);
        Task<Question> GetNextQuestionAsync(Guid sessionId, Guid userId);
        Task<Session> FinishSessionAsync(Guid sessionId, Guid userId);
        Task<Session> GetSessionByIdAsync(Guid sessionId, Guid userId);
    }
}
