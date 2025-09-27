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
    public partial class ViewAllowances : ContentPage
    {
        public Label[] Footer_Labels;
        public string[] Footer_Image_Source;
        public Image[] Footer_Images;

        UserDetailsDatabase userDetailsDatabase;
        List<UserDetails> userDetailslist;
        string Registration_No;

        AllowanceDetailsDatabase allowanceDetailsDatabase;
        List<AllowanceDetails> allowanceDetailslist;


        public ViewAllowances()
        {

            InitializeComponent();
            userDetailsDatabase = new UserDetailsDatabase();
            allowanceDetailsDatabase = new AllowanceDetailsDatabase();

            userDetailslist = userDetailsDatabase.GetUserDetails("Select * from UserDetails").ToList();

            lbl_navigation_header.Text = App.AppName;
            Registration_No = userDetailslist.ElementAt(0).RegNo;
            string username = userDetailslist.ElementAt(0).CandiName;
            lbl_header.Text = "Allowance Details - " + username;

            Footer_Labels = new Label[4] { Tab_Home_Label, Tab_UpdateEmpStatus_Label, Tab_ViewAllowances_Label, Tab_Settings_Label };
            Footer_Images = new Image[4] { Tab_Home_Image, Tab_UpdateEmpStatus_Image, Tab_ViewAllowances_Image, Tab_Settings_Image };
            Footer_Image_Source = new string[4] { "ic_home.png", "ic_update.png", "ic_allowance.png", "ic_more.png" };
            loaddata(Registration_No);
        }

        void loaddata(string Registration_No)
        {
            //allowanceDetailslist = allowanceDetailsDatabase.GetAllowanceDetails("Select *, (case when ApplicationStatus <>'Processed/ Completed' then '#f00000' else '#0d691c' end)ApplicationStatusTexcolor from AllowanceDetails").ToList();
            allowanceDetailslist = allowanceDetailsDatabase.GetAllowanceDetails($"Select * from AllowanceDetails where RegNo='{Registration_No}'").ToList();
            if (allowanceDetailslist.Any())
            {
                lblviedeatils.IsVisible = true;
                listview_allowancedetails.ItemsSource = allowanceDetailslist;
            }
            else
            {
                lblviedeatils.IsVisible = false;
                lblnorecords.IsVisible=true;
                searchbar_allowances.IsVisible = false;
                listview_allowancedetails.IsVisible = false;
            }
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

        private void listview_allowancedetails_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var currentRecord = e.Item as AllowanceDetails;
            string ApplicationNo = currentRecord.ApplicationNo.ToString();
            string AllowanceId = currentRecord.AllowanceId.ToString();
            string AllowanceName = currentRecord.AllowanceDesc.ToString();
            string ApplicationStatus = currentRecord.ApplicationStatus.ToString();
            int installmentpaid = int.Parse(currentRecord.InstallmentsPaid);
            if (installmentpaid != 0)
            {
                Navigation.PushAsync(new AllowanceTypeDetailsPage(AllowanceId, ApplicationNo, AllowanceName));
            }
            else
            {
                DisplayAlert(App.AppName, "No Transaction Details Found as the allowance is '" + ApplicationStatus + "'", "Close");
            }
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

        private void searchbar_allowances_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(searchbar_allowances.Text))
            {
                string texttosearch = searchbar_allowances.Text.ToLower().Trim();

                listview_allowancedetails.ItemsSource = allowanceDetailslist.Where(t =>
                               t.RegNo.ToLower().Contains(texttosearch)
                               || t.AllowanceDesc.ToLower().Contains(texttosearch)
                               || t.ApplicationNo.ToLower().Contains(texttosearch)
                               || t.XchNm.ToLower().Contains(texttosearch)
                               || t.ApplicationStatus.ToLower().Contains(texttosearch)
                               || t.InstallmentAmt.ToLower().Contains(texttosearch)
                               || t.StartDt.ToLower().Contains(texttosearch)
                               || t.EndDt.ToLower().Contains(texttosearch)
                               || t.TotInstallments.ToLower().Contains(texttosearch)
                               || t.InstallmentsPaid.ToLower().Contains(texttosearch)
                               || t.AmtPaid.ToLower().Contains(texttosearch)
                               || t.LastInstallmentPaidOn.ToLower().Contains(texttosearch)
                               ).ToList();
            }
            else
            {
                loaddata(Registration_No);
            }
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new PostLoginDashboardPage());
        }
    }
}