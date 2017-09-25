using HomeManager.DataAccess;
using HomeManager.DataAccess.Interfaces;
using HomeManager.Entities;

namespace HomeManager.Services
{
    public class CategoryUsageService : RepositoryObserver<Operation>
    {
        public CategoryUsageService(IRepository<Operation> operations)
            : base(operations)
        {

        }

        protected override void OnAdded(object sender, Operation e)
        {
            e.Category.Usage++;
        }

        protected override void OnRemoveed(object sender, Operation e)
        {
            if (e.Category.Usage > 0)
                e.Category.Usage--;
        }
    }
}
