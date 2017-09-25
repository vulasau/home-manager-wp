
namespace HomeManager.Entities
{
    public class Icon: EntityBase
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private string _source;
        public string Source
        {
            get { return _source; }
            set
            {
                _source = value;
                OnPropertyChanged("Source");
            }
        }

        public Icon()
        {
            Name = string.Empty;
            Source = string.Empty;
        }

        public Icon(int id, string name, string source)
        {
            Id = id;
            Name = name;
            Source = source;
        }

        public override void Update(EntityBase entity)
        {
            var current = entity as Icon;
            Name = current.Name;
            Source = current.Source;
        }
    }
}
