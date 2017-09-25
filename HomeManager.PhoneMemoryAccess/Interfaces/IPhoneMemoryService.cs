using System.Collections.ObjectModel;
using System.IO;

namespace HomeManager.PhoneMemoryAccess.Interfaces
{
    public interface IPhoneMemoryService
    {
        Stream ReadFile(string fileName);
        void WriteFile(string fileName, string content);
        void WriteFile(string fileName, string directory, string content, bool sdCard);
        void DeleteFile(string fileName);
        void MoveFile(string source, string destination);

        void SaveCollection<T>(ObservableCollection<T> collection, string fileName);
        ObservableCollection<T> LoadCollection<T>(string fileName);

        void SaveValue(string key, object value);
        T LoadValue<T>(string key);

        void Clear();
    }
}
