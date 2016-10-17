using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ModernAppMapSample
{
    public sealed partial class CustomPin : UserControl
    {
		public string Latitude {
			get { return latitude.Text; }
			set { latitude.Text = value; }
		}
		
		public string Longitude {
			get { return longitude.Text; }
			set { longitude.Text = value; }
		}
		
        public CustomPin()
        {
            this.InitializeComponent();
        }
    }
}
