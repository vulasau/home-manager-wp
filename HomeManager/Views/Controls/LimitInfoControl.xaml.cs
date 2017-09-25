using System.Windows;
using System.Windows.Controls;

namespace HomeManager.Views.Controls
{
    public partial class LimitInfoControl : UserControl
    {
        #region Dependency properties
        public static readonly DependencyProperty LimitProperty =
            DependencyProperty.Register("Limit", typeof(object), typeof(LimitInfoControl), new PropertyMetadata(null));

        public static readonly DependencyProperty DisplayLimitNameProperty =
            DependencyProperty.Register("DisplayLimitName", typeof(bool), typeof(LimitInfoControl), new PropertyMetadata(false));
        #endregion

        #region Public fields
        public object Limit
        {
            get { return GetValue(LimitProperty); }
            set { SetValue(LimitProperty, value); }
        }

        public bool DisplayLimitName
        {
            get { return (bool)GetValue(DisplayLimitNameProperty); }
            set { SetValue(DisplayLimitNameProperty, value); }
        }
        #endregion

        public LimitInfoControl()
        {
            InitializeComponent();
        }
    }
}
