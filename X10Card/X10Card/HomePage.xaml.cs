using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using X10Card.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace X10Card
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public Label[] Footer_Labels;
        public string[] Footer_Image_Source;
        public Image[] Footer_Images;
        UserDetailsDatabase userDetailsDatabase;
        List<UserDetails> userDetailslist;

        NCODetailsDatabase nCODetailsDatabase;
        List<NCODetails> nCODetailslist;
        QualificationDetailsDatabase qualificationDetailsDatabase;
        List<QualificationDetails> qualificationDetailslist;


        AllVacancyDetailsDatabase allVacancyDetailsDatabase = new AllVacancyDetailsDatabase();
        List<AllVacancyDetails> allVacancyDetailslist;

        SponsorshipDetailsDatabase sponsorshipDetailsDatabase = new SponsorshipDetailsDatabase();
        List<SponsorshipDetails> sponsorshipDetailslist;
        bool isRowEven;

        ApplicantDashboardDatabase applicantDashboardDatabase = new ApplicantDashboardDatabase();
        List<ApplicantDashboard> applicantDashboardlist;
        string Registration_No;
        string UserID, RegNo;

        public HomePage()
        {
            InitializeComponent();
            lbl_navigation_header.Text = App.AppName;
            userDetailsDatabase = new UserDetailsDatabase();
            nCODetailsDatabase = new NCODetailsDatabase();
            qualificationDetailsDatabase = new QualificationDetailsDatabase();
            loadphoto();

            Footer_Labels = new Label[4] { Tab_Home_Label, Tab_UpdateEmpStatus_Label, Tab_ViewAllowances_Label, Tab_Settings_Label };
            Footer_Images = new Image[4] { Tab_Home_Image, Tab_UpdateEmpStatus_Image, Tab_ViewAllowances_Image, Tab_Settings_Image };
            Footer_Image_Source = new string[4] { "ic_home.png", "ic_update.png", "ic_allowance.png", "ic_more.png" };

            userDetailslist = userDetailsDatabase.GetUserDetails("Select * from UserDetails").ToList();


        }
        private void ViewCell_Appearing(object sender, EventArgs e)
        {

            var viewCell = (ViewCell)sender;
            if (viewCell.View != null && viewCell.View.BackgroundColor == default(Color))
            {
                if (isRowEven)
                {
                    viewCell.View.BackgroundColor = Color.FromHex("#ff0000");
                }
                else
                {
                    viewCell.View.BackgroundColor = Color.FromHex("#e6f3fc");
                }
            }
            isRowEven = !isRowEven;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Footer_Image_Source = new string[4] { "ic_homeselected.png", "ic_update.png", "ic_allowance.png", "ic_more.png" };

            Footer_Images[Preferences.Get("Active", 0)].Source = Footer_Image_Source[Preferences.Get("Active", 0)];
            Footer_Labels[Preferences.Get("Active", 0)].TextColor = Color.FromHex("#337ab7");

            loaddata();
        }

        void loaddata()
        {
            userDetailslist = userDetailsDatabase.GetUserDetails("Select * from UserDetails").ToList();

            Registration_No = userDetailslist.ElementAt(0).RegNo;
            string RegDate = userDetailslist.ElementAt(0).RegDt;
            string RenewalDate = userDetailslist.ElementAt(0).RenewalDt;
            string Address = userDetailslist.ElementAt(0).Address;
            string XchName = userDetailslist.ElementAt(0).XchName;
            string Category = userDetailslist.ElementAt(0).Category;
            string DOB = userDetailslist.ElementAt(0).DOB;
            string EmpStatus = userDetailslist.ElementAt(0).EmpStatus;
            string MaritalStatus = userDetailslist.ElementAt(0).MaritalStatus;
            string MobileNo = userDetailslist.ElementAt(0).MobileNo;

            lbl_header.Text = userDetailslist.ElementAt(0).CandiName;
            Label_RenewalDate.Text = RenewalDate;
            Label_registrationnumber.Text = Registration_No;
            Label_XchName.Text = XchName;
            Label_RegDate.Text = RegDate;
            Label_Address.Text = Address;
            Label_Category.Text = Category;
            Label_DOB.Text = DOB;
            Label_EmpStatus.Text = EmpStatus;
            Label_MaritalStatus.Text = MaritalStatus;
            Label_MobileNo.Text = MobileNo;
            lbl_lastupdated.Text = "Last Update On : " + userDetailslist.ElementAt(0).lastupdated;

            DateTime parsedrenewdate = DateTime.ParseExact(RenewalDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            //int ab = parsedrenewdate.CompareTo(currentdate);
            string diff2 = (parsedrenewdate - DateTime.Now).TotalDays.ToString();
            int ThreeMonths = -61;
            if (double.Parse(diff2) >= 0)
            {
                Label_RenewalDatetext.TextColor = Color.FromHex("#21610B");
                Label_RenewalDate.TextColor = Color.FromHex("#21610B");
                lbl_renewtext.IsVisible = true;
            }
            else if (double.Parse(diff2) >= double.Parse(ThreeMonths.ToString()))
            {
                Label_RenewalDatetext.TextColor = Color.FromHex("#FF9933");
                Label_RenewalDate.TextColor = Color.FromHex("#FF9933");
                btn_renew.IsVisible = true;
            }
            else if (double.Parse(diff2) <= double.Parse(ThreeMonths.ToString()))
            {
                Label_RenewalDatetext.TextColor = Color.Red;
                Label_RenewalDate.TextColor = Color.Red;
                lbl_expired.IsVisible = true;
                lbl_expired.Text = "Kindly Contact " + XchName;
                btn_renew.IsVisible = false;
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

        private void Btn_home_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new PostLoginDashboardPage());
        }

        private async void Btn_refresh_Clicked(object sender, EventArgs e)
        {
            userDetailslist = userDetailsDatabase.GetUserDetails("Select * from UserDetails").ToList();
            RegNo = userDetailslist.ElementAt(0).RegNo;
            /*    string CandiName = userDetailslist.ElementAt(0).CandiName.Substring(0, 3);
                string DOB = userDetailslist.ElementAt(0).DOB;*/

            Loading_activity.IsVisible = true;
            Lbl_PleaseWait.Text = "Please wait...\nRefreshing Data...";

            await App.validatelogin(RegNo/*, CandiName, DOB*/);
            await App.GetAllVacancies();
            await App.GetSponsorship(RegNo);
            await App.GetAllowanceDetails(RegNo);
            Loading_activity.IsVisible = false;
            Preferences.Set("Active", 0);
            Application.Current.MainPage = new NavigationPage(new HomePage());

        }

        private void btn_qualification_Clicked(object sender, EventArgs e)
        {

            string query = $"Select  (' * '|| QualDesc)QualDesc from QualificationDetails";
            qualificationDetailslist = qualificationDetailsDatabase.GetQualificationDetails(query).ToList();
            if (qualificationDetailslist.Any())
            {
                popupqualification.IsVisible = true;
                listview_qualification.ItemsSource = qualificationDetailslist;
            }
            else
            {
                DisplayAlert(App.AppName, "No Records Found", App.close);
            }
        }

        private void popupqualificationCancel_Clicked(object sender, EventArgs e)
        {
            popupqualification.IsVisible = false;
        }

        private void btn_Vacancy_Clicked(object sender, EventArgs e)
        {
            allVacancyDetailslist = allVacancyDetailsDatabase.GetAllVacancyDetails("Select * from AllVacancyDetails").ToList();
            listview_vacancy.ItemsSource = allVacancyDetailslist;
            if (allVacancyDetailslist.Any())
            {
                //popupvacancy.IsVisible = true;
                Navigation.PushAsync(new ViewAllVacanciesPage());
            }
            else
            {
                DisplayAlert(App.AppName, "No Records Found", App.close);
            }
        }

        private void popupvacancyCancel_Clicked(object sender, EventArgs e)
        {
            popupvacancy.IsVisible = false;
        }

        private void btn_occupation_Clicked(object sender, EventArgs e)
        {
            string query = $"Select  (' * '|| NCODesc)NCODesc from NCODetails";
            nCODetailslist = nCODetailsDatabase.GetNCODetails(query).ToList();
            if (nCODetailslist.Any())
            {
                popupOccupation.IsVisible = true;
                listview_Occupation.ItemsSource = nCODetailslist;
            }
            else
            {
                DisplayAlert(App.AppName, "No Records Found", App.close);
            }
        }

        private void popupOccupationCancel_Clicked(object sender, EventArgs e)
        {
            popupOccupation.IsVisible = false;
        }

        private void btn_sponsorship_Clicked(object sender, EventArgs e)
        {
            string query = $"Select *" +
                            $", (case when ApplyDt <> '' then 'true' else 'false' end)ApplyDtVisibility " +
                            $", (case when Attend_Dt <> '' then 'true' else 'false' end)Attend_DtVisibility " +
                            $", (case when Selected_Dt <> '' then 'true' else 'false' end)Selected_DtVisibility " +
                            $", (case when joindt <> '' then 'true' else 'false' end)joindtVisibility " +
                            $" from SponsorshipDetails";

            sponsorshipDetailslist = sponsorshipDetailsDatabase.GetSponsorshipDetails(query).ToList();


            if (sponsorshipDetailslist.Any())
            {
                listview_sponsorship.ItemsSource = sponsorshipDetailslist;
                popupsponsorship.IsVisible = true;

            }
            else
            {
                DisplayAlert(App.AppName, "No Records Found", App.close);
            }
            // DisplayAlert(App.AppName, "Data to be available", App.close);
        }

        private void popupsponsorshipCancel_Clicked(object sender, EventArgs e)
        {
            popupsponsorship.IsVisible = false;
        }

        private async void btn_renew_Clicked(object sender, EventArgs e)
        {
            Loading_activity.IsVisible = true;
            var service = new UserRegistrationApi();
            int response_RenewAccount = await service.RenewReg(Registration_No, UserID);
            if (response_RenewAccount == 200)
            {
                Loading_activity.IsVisible = false;
                Application.Current.MainPage = new NavigationPage(new PostLoginDashboardPage());
            }
            Loading_activity.IsVisible=false;
            // Launcher.OpenAsync(new Uri("https://eemis.hp.nic.in/"));
        }

        private async void btn_JobFair_Clicked(object sender, EventArgs e)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                int reposnse_GetPublishedjobfairs = await App.GetPublishedjobfairs();
                if (reposnse_GetPublishedjobfairs == 200)
                {
                    JobFairDetailsDatabase jobFairDetailsDatabase = new JobFairDetailsDatabase();
                    List<JobFairDetails> jobFairDetailslist = jobFairDetailsDatabase.GetJobFairDetails("Select * from JobFairDetails").ToList();
                    if (jobFairDetailslist.Any())
                    {
                        await Navigation.PushAsync(new ViewPublishedjobfairsPage());
                    }
                    else
                    {
                        await DisplayAlert(App.AppName, "No Records Found", App.close);

                    }
                }
            }
            else
            {
                await DisplayAlert(App.AppName, App.nointernet, App.close);
            }

        }

        private void loadphoto()
        {
            try
            {
                applicantDashboardlist = applicantDashboardDatabase.GetApplicantDashboard("Select * from ApplicantDashboard").ToList();

                string empphoto = applicantDashboardlist.ElementAt(0).UserImage;
                if (!string.IsNullOrEmpty(empphoto))
                {
                    var bytes = Convert.FromBase64String(empphoto);
                    candidateimage.Source = ImageSource.FromStream(() => new MemoryStream(bytes));
                }
                else
                {
                    candidateimage.Source = ImageSource.FromFile("ic_usericon.png");
                }
            }
            catch
            {
                candidateimage.Source = ImageSource.FromFile("ic_usericon.png");
            }
        }


        private  void getvalues()
        {

            
                UserID = Preferences.Get("UserID", "");
                RegNo = Preferences.Get("RegNo", "");
            
        }
    }
}