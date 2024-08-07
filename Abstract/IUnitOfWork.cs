namespace UserManagementAPI.Abstract
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
        void BeginTransaction();
        void Commit();
        void Rollback();
    }

}
