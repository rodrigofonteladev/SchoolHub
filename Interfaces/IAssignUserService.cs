namespace SchoolHub.Interfaces
{
    public interface IAssignUserService
    {
        Task CreateAssignUserAccountAsync(string userCode, int personId, string userId);
    }
}