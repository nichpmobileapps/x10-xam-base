using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X10Card.Models;
using X10Card.Models.NewUserRegistration;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace X10Card
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AlreadyRegisteredPage : ContentPage
    {
        DistrictMasterDatabase districtMasterDatabase = new DistrictMasterDatabase();
        List<DistrictMaster> districtMasterslist;
        string DistrictCode;

        ExchangeNameDatabase exchangeNameDatabase = new ExchangeNameDatabase();
        List<ExchangeNameMaster> exchangeMasterslist;
        string ExchangeID;

        AlreadyRegisteredDatabase alreadyRegisteredDatabase = new AlreadyRegisteredDatabase();
        List<AlreadyRegistered> alreadyRegisteredlist;
        string UserID, dobdateselected;

        public AlreadyRegisteredPage()
        {
            InitializeComponent();
            lbl_navigation_header.Text = App.AppName;
            loaddata();
            getvalues();
        }

        private async void loaddata()
        {
            Loading_activity.IsVisible = true;
            var service = new UserRegistrationApi();
            int response_getdistrict = await service.GetDistrict();

            if (response_getdistrict == 200)
            {
                districtMasterslist = districtMasterDatabase.GetDistrictMaster("Select * from DistrictMaster").ToList();
                picker_district.ItemsSource = districtMasterslist;
                picker_district.ItemDisplayBinding = new Binding("DistrictName");
                picker_district.SelectedIndex = 0;
            }

            Loading_activity.IsVisible = false;

        }

        private async void picker_district_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (picker_district.SelectedIndex != -1)
            {

                DistrictCode = districtMasterslist.ElementAt(picker_district.SelectedIndex).DistrictID;
                // EmploymentStatusName = employmentStatuslist.ElementAt(picker_district.SelectedIndex).EmpStatDesc;
                Loading_activity.IsVisible = true;
                var service = new UserRegistrationApi();
                int response_GetExchange = await service.GetExchange(DistrictCode);
                if (response_GetExchange == 200)
                {
                    exchangeMasterslist = exchangeNameDatabase.GetExchangeNameMaster("Select * from ExchangeNameMaster").ToList();
                    picker_exchangename.ItemsSource = exchangeMasterslist;
                    picker_exchangename.ItemDisplayBinding = new Binding("ExchangeName");
                    picker_exchangename.SelectedIndex = 0;
                }
                Loading_activity.IsVisible = false;
            }
        }

        private void picker_exchangename_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (picker_exchangename.SelectedIndex != -1)
            {

                ExchangeID = exchangeMasterslist.ElementAt(picker_exchangename.SelectedIndex).ExchangeID;

            }
        }

        private void Entry_dob_Focused(object sender, FocusEventArgs e)
        {
            Entry_dob.Unfocus();
            Device.BeginInvokeOnMainThread(() =>
            {
                datepicker_dob.MaximumDate = DateTime.Now;
                datepicker_dob.Focus();
            });
        }

        private void datepicker_dob_DateSelected(object sender, DateChangedEventArgs e)
        {
            dobdateselected = e.NewDate.ToString("dd/MM/yyyy");
            Entry_dob.Text = dobdateselected.Replace('-', '/');
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new PostLoginDashboardPage());
        }

        private async void btn_submit_Clicked(object sender, EventArgs e)
        {
            if (await checkvalidation())
            {

                Loading_activity.IsVisible = true;
                var service = new UserRegistrationApi();
                int response_GetAlreadyRegisteredUserData = await service.GetAlreadyRegisteredUserData(Entry_dob.Text, ExchangeID, Entry_REGNO.Text, DistrictCode, UserID);
                if (response_GetAlreadyRegisteredUserData == 200)
                {
                    string query = $"Select  * from AlreadyRegistered";
                    alreadyRegisteredlist = alreadyRegisteredDatabase.GetAlreadyRegistered(query).ToList();
                    listview_ExistingUser.ItemsSource = alreadyRegisteredlist;
                    popupExistingUser.IsVisible = true;
                }

            }
            Loading_activity.IsVisible = false;
        }

        private void popupExistingUserCancel_Clicked(object sender, EventArgs e)
        {
            popupExistingUser.IsVisible = false;
        }

        private async void popupExistingUserMap_Clicked(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string regno = b.CommandParameter.ToString();

            List<AlreadyRegistered> alreadyRegistereddetailist; ;
            alreadyRegistereddetailist = alreadyRegisteredDatabase.GetAlreadyRegistered($"Select * from AlreadyRegistered where RegistrationNo='{regno}'").ToList();
            string XchangeCode = alreadyRegistereddetailist.ElementAt(0).XchangeCode;
            string dob = alreadyRegistereddetailist.ElementAt(0).DOB;

            Loading_activity.IsVisible = true;
            var service = new UserRegistrationApi();
            int response_usermap = await service.SaveUserMapping(XchangeCode, dob, regno, UserID);
            if (response_usermap == 200)
            {
                Loading_activity.IsVisible = false;

            }
            Loading_activity.IsVisible = false;

            popupExistingUser.IsVisible = false;
        }

        private  void getvalues()
        {
           
                UserID = Preferences.Get("UserID", "");
                //RegNo = Preferences.Get("RegNoLogin", "");
           
        }

        async Task<bool> checkvalidation()
        {
            try
            {
                if (picker_district.SelectedIndex == -1)
                {
                    await DisplayAlert(App.AppName, "Select District", App.close);
                    return false;
                }
                if (picker_exchangename.SelectedIndex == -1)
                {
                    await DisplayAlert(App.AppName, "Select Exchange Name", App.close);
                    return false;
                }
                if (string.IsNullOrEmpty(Entry_dob.Text))
                {
                    await DisplayAlert(App.AppName, "Select Date Of Birth", App.close);
                    return false;
                }
                if (string.IsNullOrEmpty(Entry_REGNO.Text))
                {
                    await DisplayAlert(App.AppName, "Enter Registration No.", App.close);
                    return false;
                }
                if (!App.isNumeric(Entry_REGNO.Text))
                {
                    await DisplayAlert(App.AppName, "Only Numeric Characters Are Allowed In Registration No.", App.close);
                    return false;
                }
                if (Entry_REGNO.Text.Length <11)
                {
                    await DisplayAlert(App.AppName, "Enter 11 Digit Registration No.", App.close);
                    return false;
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert(App.AppName, ex.Message, App.close);
                return false;
            }
            return true;
        }
    }
}