using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HomeManager.Views.Controls
{
    public partial class QuickAccessControl : UserControl
    {
        #region Dependency properties
        public static readonly DependencyProperty ItemsHeaderProperty = 
            DependencyProperty.Register("ItemsHeader", typeof(string), typeof(QuickAccessControl), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ValueHeaderProperty =
            DependencyProperty.Register("ValueHeader", typeof(string), typeof(QuickAccessControl), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(QuickAccessControl), new PropertyMetadata(null));

        public static readonly DependencyProperty FullModeItemTemplateProperty =
            DependencyProperty.Register("FullModeItemTemplate", typeof(DataTemplate), typeof(QuickAccessControl), new PropertyMetadata(null));

        public static readonly DependencyProperty InputTypeProperty =
            DependencyProperty.Register("InputType", typeof(InputScope), typeof(QuickAccessControl), new PropertyMetadata(null));
        #endregion

        #region Public fields
        public string ItemsHeader
        {
            get { return (string)GetValue(ItemsHeaderProperty); }
            set { SetValue(ItemsHeaderProperty, value); }
        }

        public string ValueHeader
        {
            get { return (string)GetValue(ValueHeaderProperty); }
            set { SetValue(ValueHeaderProperty, value); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public DataTemplate FullModeItemTemplate
        {
            get { return (DataTemplate)GetValue(FullModeItemTemplateProperty); }
            set { SetValue(FullModeItemTemplateProperty, value); }
        }

        public InputScope InputType
        {
            get { return (InputScope)GetValue(InputTypeProperty); }
            set { SetValue(InputTypeProperty, value); }
        }
        #endregion

        public QuickAccessControl()
        {
            InitializeComponent();
        }

        #region Events
        public event RoutedEventHandler AddClick;

        private void OnAddClick(object sender, RoutedEventArgs e)
        {
            if (AddClick != null)
                AddClick(sender, e);
        }
        #endregion
    }
}
