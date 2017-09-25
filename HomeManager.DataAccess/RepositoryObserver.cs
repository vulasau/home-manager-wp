using HomeManager.DataAccess.Interfaces;

namespace HomeManager.DataAccess
{
    public abstract class RepositoryObserver<T> where T : Entities.EntityBase
    {
        protected RepositoryObserver(IRepository<T> repository)
        {
            repository.Added += OnAdded;
            repository.Removed += OnRemoveed;
        }

        protected abstract void OnAdded(object sender, T e);
        protected abstract void OnRemoveed(object sender, T e);
    }
}
