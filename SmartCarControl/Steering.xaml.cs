using System.Windows.Controls;
using System.Windows.Shapes;

namespace SmartCarControl {
    /// <summary>
    /// Interaction logic for AltSterring.xaml
    /// </summary>
    public partial class Steering : UserControl {
        public double ElementMargin
        {
            get { return ActualWidth / 13; }
        }

        public bool IsTopActive
        {
            set
            {
                UpdateColor(value, Top);
            }
        }

        public bool IsBottomActive
        {
            set
            {
                UpdateColor(value, Bottom);
            }
        }

        public bool IsLeftActive
        {
            set
            {
                UpdateColor(value, Left);
            }
        }

        public bool IsRightActive
        {
            set
            {
                UpdateColor(value, Right);
            }
        }

        private void UpdateColor(bool isActive, Rectangle rectangle) {
            rectangle.Fill.Opacity = isActive ? 1.0 : 0.5;
        }

        public Steering() {
            InitializeComponent();
        }

        private void UserControl_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e) {
            Top.Margin = new System.Windows.Thickness(e.NewSize.Width / 13);
            Bottom.Margin = new System.Windows.Thickness(e.NewSize.Width / 13);
            Left.Margin = new System.Windows.Thickness(e.NewSize.Width / 13);
            Right.Margin = new System.Windows.Thickness(e.NewSize.Width / 13);
        }
    }
}
