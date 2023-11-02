using PersonalBlog.Models;

namespace PersonalBlog.Interface
{
    public interface IMembership
    {
        Task<IEnumerable<Membership>> GetAllMembershipsAsync();
        Task<Membership> GetMembershipAsync(int id);
        Task<bool> ExistsMembershipAsync(string code);
        Task<bool> EnableMembershipAsync(string code);
        Task DisableMembershipAsync(string code);
        Task AddMembershipAsync(Membership membership);
        Task DeleteMembershipAsync(Membership membership);

    }
}
