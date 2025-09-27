using System;
using System.Collections.Generic;
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
    public partial class MiscellaneousDetailsPage : ContentPage
    {
        SectorofInterestMasterDatabase sectorofInterestMasterDatabase = new SectorofInterestMasterDatabase();
        List<SectorofInterestMaster> sectorofInterestMasterlist;
        LanguageMasterDatabase languageMasterDatabase = new LanguageMasterDatabase();
        List<LanguageMaster> languageMasterlist;
        string languageid;
   
        string SectorOfInterestId;
        string langRead;
        string langWrite;
        string langSpeak;
        string RegNo;

        MiscellaneousDetailsDatabase miscellaneousDetailsDatabase = new MiscellaneousDetailsDatabase();
        List<MiscellaneousDetails> miscellaneousDetailslist;
        GetLangDetailsDatabase langDetailsDatabase = new GetLangDetailsDatabase();
        List<GetLangDetails> langDetailslist;   
        public MiscellaneousDetailsPage()
        {
            InitializeComponent();
            lbl_navigation_header.Text = App.AppName;
            loaddata();
        }

        private async void loaddata()
        {
           
                RegNo = Preferences.Get("RegNo", "");
            

            var service = new UserRegistrationApi();
            btn_header.Text = "Miscellaneous Details" + "\nRegistration No : " + RegNo + "\nName : " + service.getusername(RegNo);
           
            int response_GetSectorofInterestMaster = await service.GetSectorofInterestMaster();
            if (response_GetSectorofInterestMaster == 200)
            {
                sectorofInterestMasterlist = sectorofInterestMasterDatabase.GetSectorofInterestMaster("Select * from SectorofInterestMaster order by SectorName Desc").ToList();
                picker_SectorOfInterest.ItemsSource = sectorofInterestMasterlist;
                picker_SectorOfInterest.ItemDisplayBinding = new Binding("SectorName");
                picker_SectorOfInterest.Title = "Select Sector Of Interest";
               // picker_SectorOfInterest.SelectedIndex = 0;
            }
            int response_GetMiscelleaneousDetails = await service.GetMiscelleaneousDetails(RegNo);
            int response_GetLangDetails = await service.GetLangDetails(RegNo);
            if (response_GetMiscelleaneousDetails == 200)
            {
                miscellaneousDetailslist = miscellaneousDetailsDatabase.GetMiscellaneousDetails($"Select * from MiscellaneousDetails where RegistrationNo='{RegNo}'").ToList();
                Entry_EYESIGHT.Text=miscellaneousDetailslist.ElementAt(0).EyeSight;
                Entry_HEIGHT.Text=miscellaneousDetailslist.ElementAt(0).Height;
                Entry_WEIGHT.Text=miscellaneousDetailslist.ElementAt(0).Weight;
                Entry_CHESTNORMAL.Text=miscellaneousDetailslist.ElementAt(0).ChestNormal;
                Entry_CHESTEXPANDED.Text=miscellaneousDetailslist.ElementAt(0).ChastExpended;
                Entry_SALARYINHOMEDISTRICT.Text=miscellaneousDetailslist.ElementAt(0).SalaryHomeDist;
                Entry_SALARYINHP.Text=miscellaneousDetailslist.ElementAt(0).SalaryInHP;
                Entry_SALARYOUTSIDEHP.Text=miscellaneousDetailslist.ElementAt(0).SalaryOutHP;

                int sectorofintindex = sectorofInterestMasterlist.FindIndex(s => s.SectorID == miscellaneousDetailslist.ElementAt(0).Sector);
                if (sectorofintindex != -1)
                {
                    picker_SectorOfInterest.SelectedIndex = sectorofintindex;
                    SectorOfInterestId = sectorofInterestMasterlist.ElementAt(sectorofintindex).SectorID;
                }

               
                string langquery = $"Select LangName " +
                    $" ,(case when Read='1' then 'ic_tickmark.png' else 'ic_cancel.png' end)Readimg " +
                    $" ,(case when Write='1' then 'ic_tickmark.png' else 'ic_cancel.png' end)Writeimg " +
                    $" ,(case when Speak='1' then 'ic_tickmark.png' else 'ic_cancel.png' end)Speakimg " +
                    $" from GetLangDetails where RegistrationNo= '{RegNo}' ";
                langDetailslist = langDetailsDatabase.GetGetLangDetails(langquery).ToList();
                if (langDetailslist.Any())
                {
                    string read = langDetailslist.ElementAt(0).Read;
                    listview_language.ItemsSource = langDetailslist;
                    listview_language.HeightRequest = langDetailslist.Count * 50;
                    listview_language.IsVisible = true;

                }
                else
                {
                    listview_language.ItemsSource = null;
                    listview_language.IsVisible = false;
                }
                string userstatus = miscellaneousDetailslist.ElementAt(0).Stat;

                if (userstatus.Equals("-1") || userstatus.Equals("3"))
                {
                    btn_submit.IsVisible = true;
                    btn_addlanguage.IsVisible = true;
                    btn_next.IsVisible = false;
                }
                else if (userstatus.Equals("0") || userstatus.Equals("1"))
                {
                    btn_submit.IsVisible = false;
                    btn_next.IsVisible = true;
                    btn_addlanguage.IsVisible = false;
                }
                else if (userstatus.Equals("2"))
                {
                    btn_submit.IsVisible = false;
                    btn_addlanguage.IsVisible = false;
                    btn_next.IsVisible = true;
                }
            }
        }

        private void picker_SectorOfInterest_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (picker_SectorOfInterest.SelectedIndex != -1)
            {

                SectorOfInterestId = sectorofInterestMasterlist.ElementAt(picker_SectorOfInterest.SelectedIndex).SectorID;

            }
        }

    
        private void btn_cancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

      

        private async void btn_addlanguage_Clicked(object sender, EventArgs e)
        {
            Loading_activity.IsVisible = true;
            var service = new UserRegistrationApi();
            int response_getlangugaemaster = await service.GetLanguagesKnownMaster();
            if (response_getlangugaemaster == 200)
            {
                Loading_activity.IsVisible = false;

                languageMasterlist = languageMasterDatabase.GetLanguageMaster("Select * from LanguageMaster").ToList();
                picker_popuplanguage.ItemsSource = languageMasterlist;
                picker_popuplanguage.ItemDisplayBinding = new Binding("LanguageName");
                picker_popuplanguage.SelectedIndex = 0;

            }
            Loading_activity.IsVisible = false;
            popupaddlanguage.IsVisible = true;
        }

        private void picker_popuplanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (picker_popuplanguage.SelectedIndex != -1)
            {

                languageid = languageMasterlist.ElementAt(picker_popuplanguage.SelectedIndex).LanguageID;
               // languagename = languageMasterlist.ElementAt(picker_popuplanguage.SelectedIndex).LanguageName;

            }
        }

        private void btn_popuplanguagecancel_Clicked(object sender, EventArgs e)
        {
            popupaddlanguage.IsVisible = false;
        }

        //save languages
        private async void btn_popuplanguagesubmit_Clicked(object sender, EventArgs e)
        {
            /*if (chkbx_read.IsChecked)
            {
                langRead = true;
            }
            else
            {
                langRead = false;
            }
            if (chkbx_Write.IsChecked)
            {
                langWrite = true;
            }
            else
            {
                langWrite = false;
            }

            if (chkbx_Speak.IsChecked)
            {
                langSpeak = true;
            }
            else
            {
                langSpeak = false;
            }*/
            if (chkbx_read.IsChecked)
            {
                langRead = "1";
            }
            else
            {
                langRead = "0";
            }
            if (chkbx_Write.IsChecked)
            {
                langWrite = "1";
            }
            else
            {
                langWrite = "0";
            }

            if (chkbx_Speak.IsChecked)
            {
                langSpeak = "1";
            }
            else
            {
                langSpeak = "0";
            }


            /*   var item = new LanguagesSaved();
               item.LanguageID = languageid;
               item.Read = langRead;   
               item.Write = langWrite;
               item.Speak = langSpeak;
               item.LanguageName = languagename    ;
               languagesSavedDatabase.AddLanguagesSaved(item);*/

            var service = new UserRegistrationApi();
            //if (await checkvalidation()){
            Loading_activity.IsVisible = true;
            int response_SaveEducationDetails = await service.SaveMiscaleneousLanguages(RegNo, languageid, langRead, langWrite, langSpeak);


            if (response_SaveEducationDetails == 200)
            {
                Loading_activity.IsVisible = false;
                popupaddlanguage.IsVisible = false;
                Application.Current.MainPage = new NavigationPage(new MiscellaneousDetailsPage());

            }
            Loading_activity.IsVisible = false;

        }
   
        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new PostLoginDashboardPage());
        }

        async Task<bool> checkvalidation()
        {
            try
            {
                if (!string.IsNullOrEmpty(Entry_EYESIGHT.Text))
                {
                    if (!App.isDecimal(Entry_EYESIGHT.Text))
                    {
                        await DisplayAlert(App.AppName, "ENTER EYESIGHT IN DECIMAL(UPTO 2 DECIMAL)", App.close);
                        return false;
                    }
                } 
                
                if (!string.IsNullOrEmpty(Entry_HEIGHT.Text))
                {
                    if (!App.isDecimal(Entry_HEIGHT.Text))
                    {
                        await DisplayAlert(App.AppName, "ENTER HEIGHT IN DECIMAL(UPTO 2 DECIMAL)", App.close);
                        return false;
                    }
                }
                
                if (!string.IsNullOrEmpty(Entry_WEIGHT.Text))
                {
                    if (!App.isDecimal(Entry_WEIGHT.Text))
                    {
                        await DisplayAlert(App.AppName, "ENTER WEIGHT IN DECIMAL(UPTO 2 DECIMAL)", App.close);
                        return false;
                    }
                } 
                if (!string.IsNullOrEmpty(Entry_CHESTNORMAL.Text))
                {
                    if (!App.isDecimal(Entry_CHESTNORMAL.Text))
                    {
                        await DisplayAlert(App.AppName, "ENTER CHEST NORMAL IN DECIMAL(UPTO 2 DECIMAL)", App.close);
                        return false;
                    }
                }
                if (!string.IsNullOrEmpty(Entry_CHESTEXPANDED.Text))
                {
                    if (!App.isDecimal(Entry_CHESTEXPANDED.Text))
                    {
                        await DisplayAlert(App.AppName, "ENTER CHEST EXPANDED IN DECIMAL(UPTO 2 DECIMAL)", App.close);
                        return false;
                    }
                }

                if (!string.IsNullOrEmpty(Entry_SALARYINHOMEDISTRICT.Text))
                {
                    if (!App.isDecimal(Entry_SALARYINHOMEDISTRICT.Text))
                    {
                        await DisplayAlert(App.AppName, "Only numeric characters are allowed in Salary In Home District", App.close);
                        return false;
                    }
                }
                
                if (!string.IsNullOrEmpty(Entry_SALARYINHP.Text))
                {
                    if (!App.isDecimal(Entry_SALARYINHP.Text))
                    {
                        await DisplayAlert(App.AppName, "Only numeric characters are allowed in Salary In HP", App.close);
                        return false;
                    }
                }  
                
                if (!string.IsNullOrEmpty(Entry_SALARYOUTSIDEHP.Text))
                {
                    if (!App.isDecimal(Entry_SALARYOUTSIDEHP.Text))
                    {
                        await DisplayAlert(App.AppName, "Only numeric characters are allowed in Salary Outside HP", App.close);
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert(App.AppName, ex.Message, "OK");
                return false;
            }
            return true;
        }

        private async void btn_submit_Clicked(object sender, EventArgs e)
        {
            var service = new UserRegistrationApi();
            if (await checkvalidation())
            {
                Loading_activity.IsVisible = true;
                int response_SaveMiscaleneousDetails = await service.SaveMiscaleneousDetails(RegNo, Entry_EYESIGHT.Text, Entry_HEIGHT.Text, Entry_WEIGHT.Text,
                          Entry_CHESTNORMAL.Text, Entry_CHESTEXPANDED.Text, Entry_SALARYINHOMEDISTRICT.Text, Entry_SALARYINHP.Text,
                          Entry_SALARYOUTSIDEHP.Text, SectorOfInterestId);


                if (response_SaveMiscaleneousDetails == 200)
                {
                    Loading_activity.IsVisible = false;
                    //await Navigation.PushAsync(new EmploymentDetailsPage());
                    Application.Current.MainPage = new NewUserRegistrationMasterPage(5);

                }
                Loading_activity.IsVisible = false;
            }
        }

        private  void btn_next_Clicked(object sender, EventArgs e)
        {
           // await Navigation.PushAsync(new EmploymentDetailsPage());
            Application.Current.MainPage = new NewUserRegistrationMasterPage(5);

        }

    }
}