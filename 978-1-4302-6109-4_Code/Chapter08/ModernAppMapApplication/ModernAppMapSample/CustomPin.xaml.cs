using System;
using System.Windows;
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
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(UserControl), new PropertyMetadata(0, new PropertyChangedCallback(TextPropertyChanged)));

        public string Text
        {
            get;
            set;
        }
				
        public CustomPin()
        {
            this.InitializeComponent();
        }

        private static void TextPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var self = source as CustomPin;
            if (self != null)
            {
                self.text.Text = (string)e.NewValue;
            }
        }
    }
}
