using Microsoft.EntityFrameworkCore;
using UsersService.DbContexts;
using UsersService.Entities;
using UsersService.Interfaces.RepositoriesInterfaces;

namespace UsersService.Repository
{
    public class JobOfferUserJoinRepository : IJobOfferUserJoinRepository
    {

        private readonly UsersDbContext _context;

        public JobOfferUserJoinRepository(UsersDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UserJobOfferExistAsync(Guid userId , Guid jobId)
        {
            return await _context.JobOfferUsers
                .Where(j => j.JobOfferId == jobId && j.UserId == userId)
                .AnyAsync();
        }

        public async Task<JobOfferUser?> GetUserJobOfferJoinAsync(Guid userId , Guid jobId)
        {
            return await _context.JobOfferUsers
                .Where(e => e.UserId == userId && e.JobOfferId == jobId)
                .FirstOrDefaultAsync();
        }

        public void RemoveUserJobOffer(JobOfferUser entity)
        {
            _context.JobOfferUsers.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();  
        }
    }
}
