using HomeManager.PhoneMemoryAccess.Interfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.Serialization;

namespace HomeManager.PhoneMemoryAccess
{
    public class PhoneMemoryService: IPhoneMemoryService
    {
        private IsolatedStorageFile _storage;
        private IsolatedStorageSettings _settings;

        public PhoneMemoryService()
        {
            _storage = IsolatedStorageFile.GetUserStoreForApplication();
            _settings = IsolatedStorageSettings.ApplicationSettings;
        }

        #region File operations
        public Stream ReadFile(string fileName)
        {
            if (_storage.FileExists(fileName))
                return _storage.OpenFile(fileName, FileMode.Open);
            return null;
        }

        public void WriteFile(string fileName, string content)
        {
            DeleteFile(fileName);
            using (var stream = _storage.CreateFile(fileName))
            {
                using (var sw = new StreamWriter(stream))
                {
                    sw.Write(content);
                }
            }
        }

        public void WriteFile(string fileName, string directory, string content, bool sdCard)
        {
            var d = new DirectoryInfo(@"D:\");
            
            if(!Directory.Exists(d + "\\" + directory))
                d.CreateSubdirectory(directory);
            var dir = new DirectoryInfo(d + "\\" + directory);
            
            using (var file = File.CreateText(dir.FullName + "\\" + fileName))
            {
                file.Write(content);
            }
        }

        public void DeleteFile(string fileName)
        {
            if (_storage.FileExists(fileName))
                _storage.DeleteFile(fileName);
        }

        public void MoveFile(string source, string destination)
        {
            if (_storage.FileExists(destination))
                _storage.DeleteFile(destination);
            _storage.MoveFile(source, destination);
        }
        #endregion

        #region Save/Load
        public void SaveCollection<T>(ObservableCollection<T> collection, string fileName)
        {
            if (!collection.Any() && _storage.FileExists(fileName))
            {
                _storage.DeleteFile(fileName);
                return;
            }
            using (var stream = new IsolatedStorageFileStream(fileName, FileMode.Create, _storage))
            {
                var serializer = new DataContractSerializer(typeof(ObservableCollection<T>));
                serializer.WriteObject(stream, collection);
            }
        }

        public ObservableCollection<T> LoadCollection<T>(string fileName)
        {
            if (!_storage.FileExists(fileName))
                return new ObservableCollection<T>();

            try
            {
                using (var stream = new IsolatedStorageFileStream(fileName, System.IO.FileMode.Open, _storage))
                {
                    var serializer = new DataContractSerializer(typeof(ObservableCollection<T>));
                    return (ObservableCollection<T>)serializer.ReadObject(stream);
                }
            }
            catch
            {
                using (var stream = new IsolatedStorageFileStream(fileName, System.IO.FileMode.Open, _storage))
                {
                    var reader = new StreamReader(stream);
                    Debug.WriteLine(reader.ReadToEnd());
                    return new ObservableCollection<T>();
                }
            }
        }

        public void SaveValue(string key, object value)
        {
            if (_settings.Contains(key))
                _settings[key] = value;
            else
                _settings.Add(key, value);

            _settings.Save();
        }

        public T LoadValue<T>(string key)
        {
            if (_settings.Contains(key))
                return (T)_settings[key];
            return default(T);
        }

        public void Clear()
        {
            _settings.Clear();
        }
        #endregion
    }
}
