using Microsoft.EntityFrameworkCore;
using UsersService.DbContexts;
using UsersService.Interfaces;

namespace UsersService.Repository
{
    public class JobOfferUserJoinRepository : IJobOfferUserJoinRepository
    {

        private readonly UsersDbContext _context;

        public JobOfferUserJoinRepository(UsersDbContext context)
        {
            _context = context;
        }


        public async Task<bool> UserJobOfferExist(Guid userId , Guid jobId)
        {
            return await _context.JobOfferUsers
                .Where(j => j.JobOfferId == jobId && j.UserId == userId)
                .AnyAsync();
        }
    }
}
