using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface ISchoolRepository
    {
        Task<bool> AddSchoolAsync(School school);
        Task<School> GetSchoolByIdAsync(long schoolId);
        Task<bool> UpdateSchoolAsync(School updatedSchool);
        Task<bool> DeleteSchoolAsync(School school);
    }
}
