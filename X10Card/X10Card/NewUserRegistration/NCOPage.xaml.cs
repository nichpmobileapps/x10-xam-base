using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using X10Card.Models;
using X10Card.Models.NewUserRegistration;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace X10Card.NewUserRegistration
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NCOPage : ContentPage
    {
        NCODatabase nCODatabase = new NCODatabase();
        List<NCO> nCOlist;
        string ncoid;

        string registrationdateselected;
        string RegNo;

        GetNCODetailsDatabase getNCODetailsDatabase = new GetNCODetailsDatabase();
        List<GetNCODetails> getNCODetailslist, getNCODetailseditlist;
        DateTime Dateofbirth;
        string PersonalDetailsYN, ContactDetailsYN;
        public NCOPage()
        {
            InitializeComponent();
            lbl_navigation_header.Text = App.AppName;
            loaddata();
        }

        private async void loaddata()
        {
            
                RegNo = Preferences.Get("RegNo", "");
            
            string dob = Preferences.Get("DOB", "");
            if (!string.IsNullOrEmpty(dob))
            {
                try
                {
                    Dateofbirth = DateTime.ParseExact(Preferences.Get("DOB", ""), "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
                }
                catch
                {
                    Dateofbirth = DateTime.ParseExact(Preferences.Get("DOB", ""), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                }
            }
            var service = new UserRegistrationApi();
            btn_header.Text = "NCO Details" + "\nRegistration No : " + RegNo + "\nName : " + service.getusername(RegNo); ;
            Loading_activity.IsVisible = true;

            int response_GetNCO = await service.GetNCO();
            if (response_GetNCO == 200)
            {
                nCOlist = nCODatabase.GetNCO("Select * from NCO").ToList();
                listview_NCOname.ItemsSource = nCOlist;
            }

            Loading_activity.IsVisible = false;

            if (!string.IsNullOrEmpty(RegNo))
            {
                Loading_activity.IsVisible = true;
                await loadsubmittedforms();
                int response_GetNCODetails = await service.GetNCODetails(RegNo);
                if (response_GetNCODetails == 200)
                {
                    Loading_activity.IsVisible = false;
                    Loadpreviousdata();
                }
                else
                {

                    if (PersonalDetailsYN.ToUpper().Equals("Y") && ContactDetailsYN.ToUpper().Equals("Y"))
                    {
                        btn_finalsubmit.IsVisible = true;
                    }
                    else
                    {
                        btn_finalsubmit.IsVisible = false;
                    }
                }
                Loading_activity.IsVisible = false;
            }
        }

        async Task loadsubmittedforms()
        {
            SubmittedFormsDatabase submittedFormsDatabase = new SubmittedFormsDatabase();
            List<SubmittedFormsDetails> submittedFormsDetailslist;
            var service = new UserRegistrationApi();
            int reposne_GetRegDetailsLabels = await service.GetRegDetailsLabels(RegNo);
            if (reposne_GetRegDetailsLabels == 200)
            {
                submittedFormsDetailslist = submittedFormsDatabase.GetSubmittedFormsDetails("Select * from SubmittedFormsDetails").ToList();
                PersonalDetailsYN = submittedFormsDetailslist.ElementAt(0).PersonalDetailsYN;
                ContactDetailsYN = submittedFormsDetailslist.ElementAt(0).ContactDetailsYN;
            }
        }

        private void Loadpreviousdata()
        {
            string query = $"Select *, (case when Stat in ('-1', '3') then 'true' else 'false' end)imgeditdeletevisibility from getNCODetails where RegistrationNo= '{RegNo}'";
            getNCODetailslist = getNCODetailsDatabase.GetGetNCODetails(query).ToList();
            listview_NCOdetails.ItemsSource = getNCODetailslist;

            string userstatus = getNCODetailslist.ElementAt(0).Stat;
            if (userstatus.Equals("-1") || userstatus.Equals("3"))
            {

                if (PersonalDetailsYN.ToUpper().Equals("Y") && ContactDetailsYN.ToUpper().Equals("Y"))
                {
                    btn_finalsubmit.IsVisible = true;
                }
                else
                {
                    btn_finalsubmit.IsVisible = false;
                }

                btn_AddNCO.IsVisible = true;
                lbl_maxnco.IsVisible = true;
                //btn_next.IsVisible = false;
            }
            else if (userstatus.Equals("0") || userstatus.Equals("1"))
            {
                btn_finalsubmit.IsVisible = false;
                btn_AddNCO.IsVisible = false;
                lbl_maxnco.IsVisible = false;
                //btn_next.IsVisible = true;
            }

            else if (userstatus.Equals("2"))
            {
                btn_finalsubmit.IsVisible = false;
                btn_AddNCO.IsVisible = false;
                lbl_maxnco.IsVisible = false;
                //btn_next.IsVisible = true;
            }
        }

        private void btn_AddNCO_Clicked(object sender, EventArgs e)
        {
            string query = $"Select *, (case when Stat in ('-1', '3') then 'true' else 'false' end)imgeditdeletevisibility from getNCODetails where RegistrationNo= '{RegNo}'";
            getNCODetailslist = getNCODetailsDatabase.GetGetNCODetails(query).ToList();

            if (getNCODetailslist.Count == 3)
            {
                DisplayAlert(App.AppName, "You can add only 3 NCOS.", App.close);
            }
            else
            {
                popupaddNCO.IsVisible = true;
            }
        }

        private void editor_NCOName_Focused(object sender, FocusEventArgs e)
        {
            editor_NCOName.Unfocus();
            popupNCOname.IsVisible = true;
        }


        private void listview_NCOname_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var currentrecord = e.Item as NCO;
            editor_NCOName.Text = currentrecord.NCOName;
            ncoid = currentrecord.NCOId;

            popupNCOname.IsVisible = false;
        }

        private void SearchBar_NCOname_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(SearchBar_NCOname.Text))
            {
                string texttosearch = SearchBar_NCOname.Text.ToLower().Trim();
                listview_NCOname.ItemsSource = nCOlist.Where(t =>
                               t.NCOId.ToLower().Contains(texttosearch) ||
                               t.NCOName.ToLower().Contains(texttosearch)
                               ).ToList();
            }
            else
            {
                nCOlist = nCODatabase.GetNCO("Select * from NCO").ToList();
                listview_NCOname.ItemsSource = nCOlist;
            }
        }

        private void popupNCOnameCancel_Clicked(object sender, EventArgs e)
        {
            popupNCOname.IsVisible = false;
        }



        private void btn_cancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void btn_submit_Clicked(object sender, EventArgs e)
        {

        }

        /*  private void picker_NCO_SelectedIndexChanged(object sender, EventArgs e)
          {
              if (picker_NCO.SelectedIndex != -1)
              {

                  ncoid = nCOlist.ElementAt(picker_NCO.SelectedIndex).NCOId;
              }
          }*/

        private void btn_popupNCOcancel_Clicked(object sender, EventArgs e)
        {
            popupaddNCO.IsVisible = false;
        }

        private async void btn_popupNCOsubmit_Clicked(object sender, EventArgs e)
        {
            var service = new UserRegistrationApi();
            if (await checkvalidation())
            {
                Loading_activity.IsVisible = true;
                int response_SaveNCO = await service.SaveNCO(RegNo, ncoid, entry_EXPERIENCE.Text, registrationdateselected);
                if (response_SaveNCO == 200)
                {
                    popupaddNCO.IsVisible = false;
                    Loading_activity.IsVisible = false;
                    Application.Current.MainPage = new NewUserRegistrationMasterPage(9);

                    //await Navigation.PushAsync(new NCOPage()); ;
                    //Application.Current.MainPage = new NavigationPage(new NCOPage());
                }
                Loading_activity.IsVisible = false;
            }

        }
        private void Entry_RegistrationDate_Focused(object sender, FocusEventArgs e)
        {
            Entry_RegistrationDate.Unfocus();
            Device.BeginInvokeOnMainThread(() =>
            {
                datepicker_RegistrationDate.MinimumDate = Dateofbirth;

                datepicker_RegistrationDate.MaximumDate = DateTime.Now;
                datepicker_RegistrationDate.Focus();
            });
        }

        private void datepicker_RegistrationDate_DateSelected(object sender, DateChangedEventArgs e)
        {
            registrationdateselected = e.NewDate.ToString("dd/MM/yyyy");
            Entry_RegistrationDate.Text = registrationdateselected.Replace('-', '/');
        }

        private void img_edit_Clicked(object sender, EventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            var NCOCd = b.CommandParameter.ToString();

            string query = $"Select * from getNCODetails where NCOCd= '{NCOCd}'";
            getNCODetailseditlist = getNCODetailsDatabase.GetGetNCODetails(query).ToList();
            editor_NCOName.Text = getNCODetailseditlist.ElementAt(0).NCONm;
            entry_EXPERIENCE.Text = getNCODetailseditlist.ElementAt(0).experience;
            Entry_RegistrationDate.Text = getNCODetailseditlist.ElementAt(0).regdt;
            popupaddNCO.IsVisible = true;
        }

        private async void img_delete_Clicked(object sender, EventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            var NCOCd = b.CommandParameter.ToString();

            string query = $"Select * from getNCODetails where NCOCd= '{NCOCd}'";
            getNCODetailslist = getNCODetailsDatabase.GetGetNCODetails(query).ToList();
            string srno = getNCODetailslist.ElementAt(0).SrNo;

            Loading_activity.IsVisible = true;
            var service = new UserRegistrationApi();
            int response_finalsubmit = await service.DelNCODetails(RegNo, NCOCd, srno);
            if (response_finalsubmit == 200)
            {
                Loading_activity.IsVisible = false;
                Application.Current.MainPage = new NewUserRegistrationMasterPage(9);

                //await Navigation.PushAsync(new NCOPage());
            }
            Loading_activity.IsVisible = false;
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new PostLoginDashboardPage());
        }

        private async void btn_finalsubmit_Clicked(object sender, EventArgs e)
        {
            Loading_activity.IsVisible = true;
            var service = new UserRegistrationApi();
            int response_finalsubmit = await service.FinalSubmit(RegNo);
            if (response_finalsubmit == 200)
            {
                Loading_activity.IsVisible = false;
                Application.Current.MainPage = new NavigationPage(new PostLoginDashboardPage());
            }
            Loading_activity.IsVisible = false;
        }

        async Task<bool> checkvalidation()
        {
            try
            {

                if (string.IsNullOrEmpty(editor_NCOName.Text))
                {
                    await DisplayAlert(App.AppName, "ENTER NCO", App.close);
                    return false;
                }
                if (string.IsNullOrEmpty(entry_EXPERIENCE.Text))
                {
                    await DisplayAlert(App.AppName, "ENTER Experience", App.close);
                    return false;
                }
                else if (!App.isNumeric(entry_EXPERIENCE.Text))
                {
                    await DisplayAlert(App.AppName, "Only Numeric characters are allowed in Experience", App.close);
                    return false;
                }
                if (string.IsNullOrEmpty(Entry_RegistrationDate.Text))
                {
                    await DisplayAlert(App.AppName, "Enter Registration Date", App.close);
                    return false;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(App.AppName, ex.Message, "OK");
                return false;
            }
            return true;
        }

    }
}