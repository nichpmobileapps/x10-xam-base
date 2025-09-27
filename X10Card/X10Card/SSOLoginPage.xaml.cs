using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace X10Card
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SSOLoginPage : ContentPage
    {
        string Action_Logout;
       

        public SSOLoginPage(string Logout = null)
        {
            InitializeComponent();
            Action_Logout = Logout;

            if (string.IsNullOrEmpty(Action_Logout))
            {                               
                SSO_browser.Source = $"https://eemis.hp.nic.in/?qs=h/yhm1mKjnC9mpzNJfOfLU13yC1EEy/qBrdXfVKfFytKActC6I+8tw==";
            }
            else
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("Logout", "Please Logout your SSO account from here manually!", "OK");
                });
               
                SSO_browser.Source = $"https://parichay.nic.in";
            }

        }

        async void SSO_browser_Navigating(object sender, WebNavigatingEventArgs e)
        {
            if (string.IsNullOrEmpty(Action_Logout))
            {
                if (e.Url.StartsWith(App.Redirectionuri))
                {
                    Uri uri = new Uri(e.Url);
                    string code_from_parichay = HttpUtility.ParseQueryString(uri.Query).Get("code");
                    string state_from_parichay = HttpUtility.ParseQueryString(uri.Query).Get("state");

                    Console.WriteLine(code_from_parichay);
                    Console.WriteLine(state_from_parichay);
                    e.Cancel = true;
                   // await GetToken(code_from_parichay);
                   
                }
            }
            else
            {                
                if (e.Url.StartsWith("https://parichay.nic.in/Accounts/Services?service="))
                {
                   Application.Current.MainPage = new NavigationPage(new SSOLoginPage());
                }
            }

        }
    }
}