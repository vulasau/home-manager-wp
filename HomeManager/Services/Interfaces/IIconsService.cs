using HomeManager.Entities;
using System;
using System.Collections.ObjectModel;

namespace HomeManager.Services.Interfaces
{
    public interface IIconsService
    {
        ObservableCollection<Icon> Icons { get; set; }

        void Load();
        void LoadAsynk();
        Icon Get(int id);

        event EventHandler Loaded;
    }
}
