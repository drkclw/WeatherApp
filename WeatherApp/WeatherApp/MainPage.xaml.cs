using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.DependencyServices;
using WeatherApp.Pages;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
	{
        private Weather _weather { get; set; }
		public MainPage ()
		{
			InitializeComponent ();

            var searchTypeList = new List<string>();
            searchTypeList.Add("Current location.");
            searchTypeList.Add("Search by zip code.");
            searchType.ItemsSource = searchTypeList;
        }

        private void SearchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            if (picker.SelectedIndex == 1)
            {
                zipcodeSearch.IsVisible = true;

            }
            else
            {
                zipcodeSearch.IsVisible = false;
            }
        }

        private async void GetWeather_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (searchType.SelectedIndex == 0)
                {
                    try
                    {
                        var request = new GeolocationRequest(GeolocationAccuracy.Best);
                        var location = await Geolocation.GetLocationAsync(request);

                        if (location != null)
                        {
                            Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                        }

                        _weather = await Core.GetWeatherByCoordinates(location.Latitude, location.Longitude);
                        temperatureText.Text = _weather.Temperature;
                        cityText.Text = _weather.CityName;
                        weatherIcon.Source = _weather.IconUrl;
                        weatherDescriptionText.Text = _weather.WeatherDescription;
                    }
                    catch (FeatureNotSupportedException fnsEx)
                    {
                        await App.Current.MainPage.DisplayAlert("Info", "Location services not supported in device.", "OK");
                        var properties = new Dictionary<string, string> {
                            { "Where", "Get weather by location." },
                            { "Issue", "Location services not supported in device." }
                        };
                        Crashes.TrackError(fnsEx);
                    }
                    catch (FeatureNotEnabledException fneEx)
                    {
                        await App.Current.MainPage.DisplayAlert("Info", "Location services not enabled in device.", "OK");
                        var properties = new Dictionary<string, string> {
                            { "Where", "Get weather by location." },
                            { "Issue", "Location services not enabled in device." }
                        };
                        Crashes.TrackError(fneEx);
                    }
                    catch (PermissionException pEx)
                    {
                        await App.Current.MainPage.DisplayAlert("Info",
                            "Please check that the app has permissions to access location.", "OK");
                        var properties = new Dictionary<string, string> {
                            { "Where", "Get weather by location." },
                            { "Issue", "Permissions issue." }
                        };
                        Crashes.TrackError(pEx);
                    }
                    catch (Exception ex)
                    {
                        var properties = new Dictionary<string, string> {
                            { "Where", "Get weather by location." },
                            { "Issue", "Unable to get current location." }
                        };
                        // Unable to get location
                        await App.Current.MainPage.DisplayAlert("Info", "Unable to get current location.", "OK");
                        Crashes.TrackError(ex, properties);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(zipCode.Text))
                    {
                        _weather = await Core.GetWeatherByZipcode(zipCode.Text);
                        temperatureText.Text = _weather.Temperature;
                        cityText.Text = _weather.CityName;
                        weatherIcon.Source = _weather.IconUrl;
                        weatherDescriptionText.Text = _weather.WeatherDescription;
                    }
                }
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string> {
                            { "Where", "Get weather by zipcode." }
                };
                Crashes.TrackError(ex);
            }            
        }

        private async void ShareWeather_Clicked(object sender, EventArgs e)
        {
            try
            {                
                var contactListPage = new ContactListPage(_weather);
                await Navigation.PushAsync(contactListPage);                
            }catch(Exception ex)
            {
                var properties = new Dictionary<string, string> {
                            { "Where", "Share weather button." }
                };
                Crashes.TrackError(ex);
            }
        }
    }
}