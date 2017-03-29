using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MoochKick_WindowsClient.Classes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MoochKick_WindowsClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        List<Player> result = new List<Player>(100);

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            string company = "Hah!"; //companyTextBox1.Text;

            //Call API here


            result.Add(company);
            result.Add("Thing!!");

            listView.ItemsSource = result;

            

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Clear
            listView.Items.Clear();
            companyTextBox1.Text = "";
        }

    }
}
