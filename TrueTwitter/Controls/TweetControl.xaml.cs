using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TrueTwitter.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace TrueTwitter.Controls
{
    public sealed partial class TweetControl : UserControl
    {
       public static readonly DependencyProperty ModelProperty =
        DependencyProperty.Register(
            "Model", 
            typeof(Tweet), 
            typeof(TweetControl), 
            new PropertyMetadata(null,null));

        public Tweet Model
        {
            get { return (Tweet)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

           /* public Tweet Model { get; set; }*/

        public TweetControl()
        {
            this.InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;
        }

        private async void openButton_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(this.Model.URL));
        }
    }
}
