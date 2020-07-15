using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Net.Http;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace apiDemoUWP3
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string choiceSelected = "";
        private int locationID;
        public MainPage()
        {
            this.InitializeComponent();
        }
        private async void getID_CLick(object sender, RoutedEventArgs e)
        {
            try
            {
                var client = new HttpClient();

                choiceSelected = cityName.Text.ToLower();

                // Request parameters
                var url = "https://www.metaweather.com/api/location/search/?query=" + choiceSelected;

                // Asynchronously call the REST API method.
                var response = await client.GetAsync(url);

                // Asynchronously get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();
                client.Dispose();

                Location location = new Location();

                weatherForecast.Text = "";
                List<Location> locationObj = JsonConvert.DeserializeObject
                    <List<Location>>(contentString);
                weatherForecast.Text = String.Format("{0}- {1}", locationObj[0].title, locationObj[0].woeid);
                locationID = Int32.Parse(locationObj[0].woeid);
            }
            catch (Exception exObj)
            {
                weatherForecast.Text += "  Error Occured-- " + exObj.Message;
            }
        }
        private async void showWeather_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var client = new HttpClient();

                choiceSelected = cityName.Text.ToLower();

                // Request parameters
                string url = "https://www.metaweather.com/api/location/" + locationID.ToString();

                // Asynchronously call the REST API method.
                var response = await client.GetAsync(url);

                // Asynchronously get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();
                client.Dispose();

                Weather weather = new Weather();

                weatherForecast.Text += "\n";
                Weather weatherObj = JsonConvert.DeserializeObject
                    <Weather>(contentString);

                var day1Weather = weatherObj.ConsolidatedWeather[0];
                weatherForecast.Text += "--------------------------";
                weatherForecast.Text += "Day1 Weather: \n";
                weatherForecast.Text += "\nWeather State: " + day1Weather.WeatherStateName;
                weatherForecast.Text += "\nTemprature: " + day1Weather.TheTemp;
                weatherForecast.Text += "\nAir Pressure: " + day1Weather.AirPressure;
                weatherForecast.Text += "\nHumidity: " + day1Weather.Humidity;

            }
            catch (Exception exObj)
            {
                weatherForecast.Text += "  Error Occured-- " + exObj.Message;
            }
        }
    }
}
