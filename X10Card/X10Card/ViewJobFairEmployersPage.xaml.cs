using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X10Card.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace X10Card
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewJobFairEmployersPage : ContentPage
    {
        public Label[] Footer_Labels;
        public string[] Footer_Image_Source;
        public Image[] Footer_Images;

        UserDetailsDatabase userDetailsDatabase = new UserDetailsDatabase();
        List<UserDetails> userDetailslist;
        JobfairsEmployersDatabase jobfairsEmployersDatabase = new JobfairsEmployersDatabase();
        List<JobfairsEmployers> jobfairsEmployerslists;
        string distt, exchange;
        public ViewJobFairEmployersPage(string district, string xchangename)
        {
            InitializeComponent();
            userDetailslist = userDetailsDatabase.GetUserDetails("Select * from UserDetails").ToList();
            distt = district;
            exchange = xchangename;

            string Registration_No = userDetailslist.ElementAt(0).RegNo;
            string username = userDetailslist.ElementAt(0).CandiName;
            lbl_header.Text = "Job Fair Details - " + username;

            Footer_Labels = new Label[4] { Tab_Home_Label, Tab_UpdateEmpStatus_Label, Tab_ViewAllowances_Label, Tab_Settings_Label };
            Footer_Images = new Image[4] { Tab_Home_Image, Tab_UpdateEmpStatus_Image, Tab_ViewAllowances_Image, Tab_Settings_Image };
            Footer_Image_Source = new string[4] { "ic_home.png", "ic_update.png", "ic_allowance.png", "ic_more.png" };

            lbl_header.Text = "Job Fair Details \nDistrict - "+ distt + "\nExchange - "+ exchange;
            jobfairsEmployerslists = jobfairsEmployersDatabase.GetJobfairsEmployers("Select *,(EmpID ||'$'||JobfairID)EmpidJobfairID from JobfairsEmployers").ToList();
            listview_jobfairdetails.ItemsSource = jobfairsEmployerslists;
        }
        private async void btn_viewvacancies_Clicked(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            string EmpidJobfairID = b.CommandParameter.ToString();
            string[] splitEmpidJobfairID = EmpidJobfairID.Split('$');
            string empid = splitEmpidJobfairID[0];
            string jobfairid = splitEmpidJobfairID[1];
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                Loading_activity.IsVisible = true;
                int reposnse_GetJobFairEmployersVacancies = await App.GetJobFairEmployersVacancies(jobfairid, empid);
                if (reposnse_GetJobFairEmployersVacancies == 200)
                {
                    Loading_activity.IsVisible = false;
                    JobFairEmployersVacanciesDatabase jobFairEmployersVacanciesDatabase = new JobFairEmployersVacanciesDatabase();
                    List<JobFairEmployersVacancies> jobfairsEmployersVacancieslists = jobFairEmployersVacanciesDatabase.GetJobFairEmployersVacancies("Select * from JobFairEmployersVacancies").ToList();
                    if (jobfairsEmployersVacancieslists.Any())
                    {
                        string employername = jobfairsEmployerslists.ElementAt(0).EmployerName;
                        await Navigation.PushAsync(new ViewJobFairEmployersVacanciesPage(distt,exchange, employername));
                    }
                    else
                    {
                        await DisplayAlert(App.AppName, "No Records Found", "Close");
                    }
                }
                Loading_activity.IsVisible = false;
            }
            else
            {
                await DisplayAlert(App.AppName, App.nointernet, "Close");
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

       

    }
}