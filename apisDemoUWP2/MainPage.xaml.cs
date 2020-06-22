using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Net.Http;
using System;
using Newtonsoft.Json;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace apisDemoUWP2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>   
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var client = new HttpClient();

            string choiceSelected = cityName.Text.ToLower();

            // Request parameters
            var url = "https://www.metaweather.com/api/location/search/?query=" + choiceSelected;


            // Asynchronously call the REST API method.  
            var response = await client.GetAsync(url);

            // Asynchronously get the JSON response.

            string contentString = await response.Content.ReadAsStringAsync();

            coordinatesTxtBox.Text = contentString;

            client.Dispose();
        }
        private void jsonConvert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Weather person = new Weather();

                string json = JsonConvert.SerializeObject(person, Formatting.Indented);
                jsonTxtBox.Text += "\n" + json + "--END";

                Weather person2 = JsonConvert.DeserializeObject<Weather>(json);
                jsonTxtBox.Text += "\n" + person2.title +
                                          person2.location_type+ 
                                          person2.woeid + 
                                          person2.latt_long + 
                                          "-------END";
            }
            catch (Exception exObj)
            {
                jsonTxtBox.Text += "\n" + exObj.Message;
            }
        }
    }
}
