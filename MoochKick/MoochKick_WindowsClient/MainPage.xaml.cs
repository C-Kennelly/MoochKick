using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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
        private List<Player> listOfPlayers = new List<Player>(100);
        private const string devKey = "";

        public MainPage()
        {
            this.InitializeComponent();
            


        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            //Call API here
            //http://ec2-35-167-65-201.us-west-2.compute.amazonaws.com/api/moochers/Creative Force/3/200/[devkey]
            
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://ec2-35-167-65-201.us-west-2.compute.amazonaws.com");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var serializer = new DataContractJsonSerializer(typeof(string[]));

                var streamTask = client.GetStreamAsync("api/moochers/Creative Force/3/200/" + devKey);
                var players = serializer.ReadObject(await streamTask) as string[];
                
                foreach (string gamertag in players)
                {
                    listOfPlayers.Add(new Player(gamertag));
                }
            }

            playerList.ItemsSource = listOfPlayers;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Clear
            listOfPlayers = new List<Player>(100);
            companyTextBox1.Text = "";
        }

    }
}
