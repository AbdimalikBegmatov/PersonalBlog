using Microsoft.EntityFrameworkCore;
using PersonalBlog.Data;
using PersonalBlog.Interface;
using PersonalBlog.Models;

namespace PersonalBlog.Repository
{
    public class MembershipRepository : IMembership
    {
        private readonly ApplicationDbContext _context;

        public MembershipRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddMembershipAsync(Membership membership)
        {
            _context.Memberships.Add(membership);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMembershipAsync(Membership membership)
        {
            _context.Memberships.Remove(membership);
            await _context.SaveChangesAsync();
        }

        public async Task DisableMembershipAsync(string code)
        {
            var currentMembership = await _context.Memberships.FirstOrDefaultAsync(m=>m.Code.Equals(code));
            if (currentMembership != null)
            {
                currentMembership.IsEnable = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> EnableMembershipAsync(string code)
        {
            var currentMembership = await _context.Memberships.FirstOrDefaultAsync(m => m.Code.Equals(code));
            if (currentMembership != null)
            {
                return currentMembership.IsEnable;
            }
            return false;
        }

        public async Task<bool> ExistsMembershipAsync(string code)
        {
            return await _context.Memberships.AnyAsync(m=>m.Code.Equals(code));
        }

        public async Task<IEnumerable<Membership>> GetAllMembershipsAsync()
        {
            return await _context.Memberships.OrderByDescending(m=>m.Id).ToListAsync();
        }

        public async Task<Membership> GetMembershipAsync(int id)
        {
            return await _context.Memberships.FirstOrDefaultAsync(m=>m.Id == id);
        }
    }
}
