using System;
using System.Collections.Generic;
using System.Linq;
using X10Card.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace X10Card
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MorePage : ContentPage
    {
        public Label[] Footer_Labels;
        public string[] Footer_Image_Source;
        public Image[] Footer_Images;
        public MorePage()
        {
            InitializeComponent();
            Footer_Labels = new Label[4] { Tab_Home_Label, Tab_UpdateEmpStatus_Label, Tab_ViewAllowances_Label, Tab_Settings_Label };
            Footer_Images = new Image[4] { Tab_Home_Image, Tab_UpdateEmpStatus_Image, Tab_ViewAllowances_Image, Tab_Settings_Image };
            Footer_Image_Source = new string[4] { "ic_home.png", "ic_update.png", "ic_allowance.png", "ic_more.png" };

            VersionTracking.Track();
            var currentVersion = VersionTracking.CurrentVersion;
            lbl_appversion.Text = "Version " + currentVersion;

            lbl_navigation_header.Text = App.AppName;
            lbl_appname.Text = App.AppName;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (Preferences.Get("Active", 0) == 0)
            {
                Footer_Image_Source = new string[4] { "ic_homeselected.png", "ic_update.png", "ic_allowance.png", "ic_more.png" };
                App.pages = new Page[] { new HomePage(), new UpdateEmpStatusPage(), new ViewAllowances(), new MorePage() };
            }
            else if (Preferences.Get("Active", 0) == 1)
            {
                Footer_Image_Source = new string[4] { "ic_home.png", "ic_update.png", "ic_allowance.png", "ic_more.png" };
                App.pages = new Page[] { new HomePage(), new UpdateEmpStatusPage(), new ViewAllowances(), new MorePage() };

            }
            else if (Preferences.Get("Active", 0) == 2)
            {
                Footer_Image_Source = new string[4] { "ic_home.png", "ic_update.png", "ic_allowanceselected.png", "ic_more.png" };
                App.pages = new Page[] { new HomePage(), new UpdateEmpStatusPage(), new ViewAllowances(), new MorePage() };

            }
            else if (Preferences.Get("Active", 0) == 3)
            {
                Footer_Image_Source = new string[4] { "ic_home.png", "ic_update.png", "ic_allowance.png", "ic_moreselected.png" };
                App.pages = new Page[] { new HomePage(), new UpdateEmpStatusPage(), new ViewAllowances(), new MorePage() };

            }

            Footer_Images[Preferences.Get("Active", 0)].Source = Footer_Image_Source[Preferences.Get("Active", 0)];
            Footer_Labels[Preferences.Get("Active", 0)].TextColor = Color.FromHex("#337ab7");

        }


        private void Tab_Home_Tapped(object sender, EventArgs e)
        {
            Preferences.Set("Active", 0);
            Application.Current.MainPage = new NavigationPage(new HomePage());
        }

        private void Tab_UpdateEmpStatus_Tapped(object sender, EventArgs e)
        {
            Preferences.Set("Active", 1);
            Application.Current.MainPage = new NavigationPage(new UpdateEmpStatusPage());

        }

        private void Tab_ViewAllowances_Tapped(object sender, EventArgs e)
        {
            Preferences.Set("Active", 2);
            Application.Current.MainPage = new NavigationPage(new ViewAllowances());
        }

        private void Tab_Settings_Tapped(object sender, EventArgs e)
        {
            Preferences.Set("Active", 3);
            Application.Current.MainPage = new NavigationPage(new MorePage());
        }

        private void Deptt_Call(object sender, EventArgs e)
        {

            Launcher.OpenAsync("tel:+911772625277");
        }

        private void deptt_WebSite(object sender, EventArgs e)
        {
            Launcher.OpenAsync("https://eemis.hp.nic.in/");
        }

        private async void Deptt_email(object sender, EventArgs e)
        {
            await Clipboard.SetTextAsync("lep-hp@nic.in");
            if (Clipboard.HasText)
            {

                //var text = await Clipboard.GetTextAsync();
                // await DisplayAlert("Email", string.Format("Email Id  ' {0} ' Copied ", text), "OK");
                await Launcher.OpenAsync(new Uri($"mailto:{"lep-hp@nic.in"}"));
            }
        }

        private void policytapped(object sender, EventArgs e)
        {

            Navigation.PushAsync(new LoadWebViewPage(""));
        }

        private async void logoutTapped(object sender, EventArgs e)
        {
            UserDetailsDatabase userDetailsDatabase = new UserDetailsDatabase();
            List<UserDetails> userDetailslist;
            userDetailslist = userDetailsDatabase.GetUserDetails("Select * from userdetails").ToList();
            var name = userDetailslist.ElementAt(0).CandiName;
            var mobile = userDetailslist.ElementAt(0).MobileNo;

            bool m;
            if (string.IsNullOrEmpty(mobile))
            {
                m = await DisplayAlert(App.AppName, "Are you sure " + name + " you want to logout.", "Logout", "Cancel");
            }
            else
            {
                m = await DisplayAlert(App.AppName, "Are you sure " + name + " (" + mobile.Trim() + ")"
                + " you want to logout.", "Logout", "Cancel");
            }

            // bool m = await DisplayAlert(App.GetLabelByKey("lbl_navigation_header"), "Are you sure you want to logout.", "Logout", "Cancel");

            if (m)
            {
                SecureStorage.RemoveAll();
                Preferences.Clear();
              

                userDetailsDatabase.DeleteUserDetails();
                AllowanceDetailsDatabase allowanceDetailsDatabase = new AllowanceDetailsDatabase();
                allowanceDetailsDatabase.DeleteAllowanceDetails();
                AllowanceTransactionsDatabase allowanceTransactionsDatabase =new AllowanceTransactionsDatabase();
                allowanceTransactionsDatabase.DeleteAllowanceTransactions();
                Preferences.Set("LastUpdated", "");
                Preferences.Set("Active", 0);
                Application.Current.MainPage = new NavigationPage(new MainPage());
            }

        }
        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new PostLoginDashboardPage());
        }
    }
}