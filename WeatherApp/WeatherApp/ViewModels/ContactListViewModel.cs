using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherApp.DependencyServices;
using WeatherApp.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WeatherApp.ViewModels
{
    public class ContactListViewModel : BaseModel
    {
        public ICommand SelectContactCommand { get; private set; }
        public ICommand ShareCommand { get; private set; }  
        private Weather _weather { get; }

        public ContactListViewModel(Weather weather)
        {
            _weather = weather;
            GetContacts();
            //SelectContactCommand = new Command(() => SelectContact());
            ShareCommand = new Command(async () => await ShareWeather());
        }

        ObservableCollection<PhoneContact> _contactList;
        public ObservableCollection<PhoneContact> ContactList
        {
            get
            {
                return _contactList;
            }
            set
            {
                _contactList = value;
                OnPropertyChanged("ContactList");
            }
        }

        IEnumerable<PhoneContact> _searchContactList;
        public IEnumerable<PhoneContact> SearchContactList
        {
            get
            {
                return _searchContactList;
            }
            set
            {
                _searchContactList = value;
                OnPropertyChanged("SearchContactList");
            }
        }

        string _searchString;
        public string SearchString
        {
            get
            {
                return _searchString;
            }
            set
            {
                _searchString = value;
                SearchContactList =
                    string.IsNullOrEmpty(value) ? ContactList :
                          ContactList.Where(i => i.LocalName.ToLower().Contains(value.ToLower()));
                OnPropertyChanged("SearchString");
            }
        }

        public async void GetContacts()
        {
            var _contactService = DependencyService.Get<IContactService>();
            var contacts = await _contactService.GetAllContacts();
            ContactList = new ObservableCollection<PhoneContact>(contacts);
            SearchContactList = ContactList;
        }

        //void SelectContact()
        //{
        //    foreach (var item in ContactList)
        //    {
        //        item.IsVisible = true;
        //    }
        //}

        async Task ShareWeather()
        {
            //Crashes.GenerateTestCrash();
            try
            {
                var selectedContactsList = ContactList.Where(x => x.IsSelected == true).ToList();
                if (selectedContactsList.Count > 0)
                {
                    await SendSms(
                        string.Format("Current weather in {0} is: {1}, current temperature is: {2}",
                        _weather.CityName,
                        _weather.WeatherDescription, _weather.Temperature),
                        selectedContactsList.Select(c => c.PhoneNumbers.ElementAt(0).Digits).ToArray());
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Info", "Please select at least one contact.", "Ok");
                }
            }
            catch (Exception ex)
            {
                // Other error has occurred.
                var properties = new Dictionary<string, string> {
                            { "Where", "Share weather command." },
                            { "Issue", "Unknown error occurred." }
                        };
                Crashes.TrackError(ex, properties);
            }
        }

        private async Task SendSms(string messageText, string[] recipients)
        {
            try
            {
                var message = new SmsMessage(messageText, recipients);
                await Sms.ComposeAsync(message);

                Analytics.TrackEvent("Send to SMS", new Dictionary<string, string> {
                            { "Message", messageText }
                        });
            }
            catch (FeatureNotSupportedException ex)
            {
                // Sms is not supported on this device.
                var properties = new Dictionary<string, string> {
                            { "Where", "SendSMS method." },
                            { "Issue", "SMS not supported in device." }
                        };
                Crashes.TrackError(ex, properties);
            }
            catch (Exception ex)
            {
                // Other error has occurred.
                var properties = new Dictionary<string, string> {
                            { "Where", "SendSMS method." },
                            { "Issue", "Unknown error occurred." }
                        };
                Crashes.TrackError(ex, properties);
            }
        }
    }
}
