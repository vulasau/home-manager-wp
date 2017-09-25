
namespace HomeManager.Infrastructure.Interfaces
{
    public interface IContainer
    {
        void RegisterInstance<T>(T instance);
        T GetInstance<T>();
    }
}
