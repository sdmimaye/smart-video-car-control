using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartCarControl {
    /// <summary>
    /// Interaction logic for Steering.xaml
    /// </summary>
    public partial class Steering {
        public Steering() {
            InitializeComponent();
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
    }
}
