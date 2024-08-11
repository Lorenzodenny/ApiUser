using System.Data;

namespace UserManagementAPI.Abstract
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        void Commit();
        void Rollback();
    }

}
