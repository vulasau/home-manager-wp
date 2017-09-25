using HomeManager.Entities;
using HomeManager.Services.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManager.Services
{
    public class IconsService : IIconsService
    {
        public ObservableCollection<Icon> Icons { get; set; }

        public void Load()
        {
            var currentDir = Environment.CurrentDirectory;
            var iconsDir = "\\Icons\\Categories";
            DirectoryInfo dinfo = new DirectoryInfo(currentDir + iconsDir);
            var files = dinfo.GetFiles();

            Icons = new ObservableCollection<Icon>();
            var id = 0;
            foreach (var file in files)
            {
                var name = Path.GetFileNameWithoutExtension(file.Name);
                var icon = new Icon(id, name, file.FullName.Remove(0, currentDir.Length));
                Icons.Add(icon);
                id++;
            }

            OnLoaded();
        }

        public async void LoadAsynk()
        {
            await Task.Run(() => Load());
        }

        public Icon Get(int id)
        {
            return Icons.FirstOrDefault(x => x.Id.Equals(id));
        }

        #region Events
        public event EventHandler Loaded;

        protected void OnLoaded()
        {
            if (Loaded != null)
                Loaded(this, new EventArgs());
        }
        #endregion
    }
}
