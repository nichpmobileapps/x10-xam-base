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
    public partial class ViewAllVacanciesPage : ContentPage
    {
        public Label[] Footer_Labels;
        public string[] Footer_Image_Source;
        public Image[] Footer_Images;
        UserDetailsDatabase userDetailsDatabase;
        List<UserDetails> userDetailslist;

        AllVacancyDetailsDatabase allVacancyDetailsDatabase = new AllVacancyDetailsDatabase();
        List<AllVacancyDetails> allVacancyDetailslist, allVacancyDetailslistpopup;
        bool isRowEven;
        public ViewAllVacanciesPage()
        {
            InitializeComponent();
            lbl_navigation_header.Text = App.AppName;
            userDetailsDatabase = new UserDetailsDatabase();
          


            Footer_Labels = new Label[4] { Tab_Home_Label, Tab_UpdateEmpStatus_Label, Tab_ViewAllowances_Label, Tab_Settings_Label };
            Footer_Images = new Image[4] { Tab_Home_Image, Tab_UpdateEmpStatus_Image, Tab_ViewAllowances_Image, Tab_Settings_Image };
            Footer_Image_Source = new string[4] { "ic_home.png", "ic_update.png", "ic_allowance.png", "ic_more.png" };

            userDetailslist = userDetailsDatabase.GetUserDetails("Select * from UserDetails").ToList();
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
            lbl_header.Text = userDetailslist.ElementAt(0).CandiName;

            allVacancyDetailslist = allVacancyDetailsDatabase.GetAllVacancyDetails("Select EmpID,EmployerName,count(*) as TotalVacancy from AllVacancyDetails group by EmployerName").ToList();
            DetailedList.ItemsSource = allVacancyDetailslist;
        }
        private void DetailedList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            
            var currentRecord = e.Item as AllVacancyDetails;
            string emplyrid = currentRecord.EmpID;
            //EmpID
            allVacancyDetailslistpopup = allVacancyDetailsDatabase.GetAllVacancyDetails($"Select * from AllVacancyDetails where EmpID = '{emplyrid}'").ToList();
            listview_vacancy.ItemsSource = allVacancyDetailslistpopup;
            lbl_popupvacancy.Text = "Vacancy Details -" + allVacancyDetailslistpopup.ElementAt(0).EmployerName;
            popupvacancy.IsVisible = true;
        }
        private void popupvacancyCancel_Clicked(object sender, EventArgs e)
        {
            popupvacancy.IsVisible = false;
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

       
        private void ViewCell_Appearing(object sender, EventArgs e)
        {

            var viewCell = (ViewCell)sender;
            if (viewCell.View != null && viewCell.View.BackgroundColor == default(Color))
            {
                if (isRowEven)
                {
                    viewCell.View.BackgroundColor = Color.FromHex("#ffffff");
                }
                else
                {
                    viewCell.View.BackgroundColor = Color.FromHex("#6fd9fc");
                }
            }
            isRowEven = !isRowEven;
        }

       
    }
}