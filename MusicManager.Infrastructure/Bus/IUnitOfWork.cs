namespace MusicManager.Infrastructure.Bus
{
    public interface IUnitOfWork
    {
        void Commit();
        void Rollback();
    }
}
