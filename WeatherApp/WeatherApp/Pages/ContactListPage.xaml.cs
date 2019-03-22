using WeatherApp.Models;
using WeatherApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ContactListPage : ContentPage
	{
		public ContactListPage (Weather weather)
		{
			InitializeComponent ();
            BindingContext = new ContactListViewModel(weather);

            listView.ItemSelected += (sender, e) =>
            {
                var itemSelected = e.SelectedItem as PhoneContact;
                if (itemSelected == null)
                {
                    return;
                }
                  ((ListView)sender).SelectedItem = null;
            };
        }
	}
}