using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X10Card.Models;
using X10Card.Models.NewUserRegistration;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace X10Card.NewUserRegistration
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExServiceManPage : ContentPage
    {
        ForceMasterDatabase forceMasterDatabase = new ForceMasterDatabase();
        RankMasterDatabase rankMasterDatabase = new RankMasterDatabase();
        MedicalMasterDatabase medicalMasterDatabase = new MedicalMasterDatabase();
        CharacterMasterDatabase characterMasterDatabase = new CharacterMasterDatabase();
        ReasonMasterDatabase reasonMasterDatabase = new ReasonMasterDatabase();

        List<ForceMaster> forceMasterlist;
        List<RankMaster> rankMasterlist;
        List<MedicalMaster> medicalMasterlist;
        List<CharacterMaster> characterMasterlist;
        List<ReasonMaster> reasonMasterlist;

        string forceId, rankid, medicalid, characterid, reasonid;

        string issuedateselected, validuptodateselected, dischargedateselected, ENROLMENTdateselected;
        string doc1, RegNo;

        XservicemenDetailsDatabase xservicemenDetailsDatabase = new XservicemenDetailsDatabase();
        List<XservicemenDetails> xservicemenDetailsList;

        public ExServiceManPage()
        {
            InitializeComponent();
            lbl_navigation_header.Text = App.AppName;
            loaddata();
        }

        private async void loaddata()
        {
            
                RegNo = Preferences.Get("RegNo", "");
             
            
            var service = new UserRegistrationApi();
            btn_header.Text = "Ex-ServiceMen Details" + "\nRegistration No : " + RegNo + "\nName : " + service.getusername(RegNo);
            Loading_activity.IsVisible = true;
          
            var F = await service.GetForce();
            // var R = await service.GetRank();
            var m = await service.GetMedical();
            var c = await service.GetCharacter();
            var e = await service.GetReason();

            Loading_activity.IsVisible = false;

            forceMasterlist = forceMasterDatabase.GetForceMaster("Select * from ForceMaster").ToList();
            picker_FORCENAME.ItemsSource = forceMasterlist;
            picker_FORCENAME.ItemDisplayBinding = new Binding("ForceName");
            picker_FORCENAME.Title = "Select Force";
            //picker_FORCENAME.SelectedIndex = 0;

            medicalMasterlist = medicalMasterDatabase.GetMedicalMaster("Select * from MedicalMaster").ToList();
            picker_MEDICALCATEGORY.ItemsSource = medicalMasterlist;
            picker_MEDICALCATEGORY.ItemDisplayBinding = new Binding("MedicalName");
            picker_MEDICALCATEGORY.Title = "Select Medical";

            // picker_MEDICALCATEGORY.SelectedIndex = 0;

            characterMasterlist = characterMasterDatabase.GetCharacterMaster("Select * from CharacterMaster").ToList();
            picker_CHARACTER.ItemsSource = characterMasterlist;
            picker_CHARACTER.ItemDisplayBinding = new Binding("CharacterName");
            picker_CHARACTER.Title = "Select Character";

            // picker_CHARACTER.SelectedIndex = 0;

            reasonMasterlist = reasonMasterDatabase.GetReasonMaster("Select * from ReasonMaster").ToList();
            picker_REASON.ItemsSource = reasonMasterlist;
            picker_REASON.ItemDisplayBinding = new Binding("ReasonName");
            picker_REASON.Title = "Select Reason";

            // picker_REASON.SelectedIndex = 0;

            if (!string.IsNullOrEmpty(RegNo))
            {
                Loading_activity.IsVisible = true;

                int response_GetXservicemenDetails = await service.GetXservicemenDetails(RegNo);
                if (response_GetXservicemenDetails == 200)
                {
                    Loading_activity.IsVisible = false;
                    Loadpreviousdata();
                }
                Loading_activity.IsVisible = false;
            }

        }
        private async void Loadpreviousdata()
        {
            string query = $"Select *" +
                $", (case when DocFileLink <> '' then  'true' else 'false' end)pdfimgvisibility " +
                $" from XservicemenDetails where RegistrationNo= '{RegNo}'";
            xservicemenDetailsList = xservicemenDetailsDatabase.GetXservicemenDetails(query).ToList();


            string userstatus = xservicemenDetailsList.ElementAt(0).Stat;
            if (userstatus.Equals("-1") || userstatus.Equals("3"))
            {
                btn_submit.IsVisible = true;
                btn_next.IsVisible = false;
            }
            else if (userstatus.Equals("0") || userstatus.Equals("1"))
            {
                btn_submit.IsVisible = false;
                btn_next.IsVisible = true;
            }

            else if (userstatus.Equals("2"))
            {
                btn_submit.IsVisible = false;
                btn_next.IsVisible = true;
            }

            if (!string.IsNullOrEmpty(xservicemenDetailsList.ElementAt(0).DocFileLink))
            {
                btn_viewdoc.IsVisible = true;
                doc1 = xservicemenDetailsList.ElementAt(0).DocFileLink;
            }
            int Forceindex = forceMasterlist.FindIndex(s => s.ForceId == xservicemenDetailsList.ElementAt(0).ddlforce);
            if (Forceindex != -1)
            {
                picker_FORCENAME.SelectedIndex = Forceindex;
                forceId = forceMasterlist.ElementAt(Forceindex).ForceId;
                var service = new UserRegistrationApi();
                var R = await service.GetRank(forceId);
                rankMasterlist = rankMasterDatabase.GetRankMaster("Select * from RankMaster").ToList();

                int rankindex = rankMasterlist.FindIndex(s => s.RankId == xservicemenDetailsList.ElementAt(0).ddlrank);
                if (rankindex != -1)
                {
                    picker_RANK.SelectedIndex = rankindex;
                    rankid = rankMasterlist.ElementAt(rankindex).RankId;
                }
            }

            entry_REGIMENTNAME.Text = xservicemenDetailsList.ElementAt(0).txtregimentname;
            entry_SERVICENUMBER.Text = xservicemenDetailsList.ElementAt(0).txtservicenumber;

            int medicalindex = medicalMasterlist.FindIndex(s => s.MedicalId == xservicemenDetailsList.ElementAt(0).ddlmedical);
            if (medicalindex != -1)
            {
                picker_MEDICALCATEGORY.SelectedIndex = medicalindex;
                medicalid = medicalMasterlist.ElementAt(medicalindex).MedicalId;
            }
            int characterindex = characterMasterlist.FindIndex(s => s.CharacterId == xservicemenDetailsList.ElementAt(0).ddlcharacter);
            if (characterindex != -1)
            {
                picker_CHARACTER.SelectedIndex = characterindex;
                characterid = characterMasterlist.ElementAt(characterindex).CharacterId;
            }
            string enrolmentdate = xservicemenDetailsList.ElementAt(0).txtenrolmentdate;
            if (enrolmentdate.Contains("1900"))
            {
                Entry_ENROLMENTdate.Text = "";
            }
            else
            {
                Entry_ENROLMENTdate.Text = enrolmentdate;
            }

            string dischargedate = xservicemenDetailsList.ElementAt(0).txtdischargedate;
            if (dischargedate.Contains("1900"))
            {
                Entry_DISCHARGEdate.Text = "";
            }
            else
            {
                Entry_DISCHARGEdate.Text = dischargedate;
            }


            int reasonindex = reasonMasterlist.FindIndex(s => s.ReasonId == xservicemenDetailsList.ElementAt(0).ddlreason);
            if (reasonindex != -1)
            {
                picker_REASON.SelectedIndex = reasonindex;
                reasonid = reasonMasterlist.ElementAt(reasonindex).ReasonId;
            }
            editor_REMARKS.Text = xservicemenDetailsList.ElementAt(0).txtremarks;
            Entry_issuedate.Text = xservicemenDetailsList.ElementAt(0).IssueDt;
            Entry_VALIDUPTOdate.Text = xservicemenDetailsList.ElementAt(0).ValidUptoDt;
            entry_certificateno.Text = xservicemenDetailsList.ElementAt(0).DocCertificateNo;

        }

        private async void picker_FORCENAME_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (picker_FORCENAME.SelectedIndex != -1)
            {

                forceId = forceMasterlist.ElementAt(picker_FORCENAME.SelectedIndex).ForceId;

                Loading_activity.IsVisible = true;
                var service = new UserRegistrationApi();

                var R = await service.GetRank(forceId);
                rankMasterlist = rankMasterDatabase.GetRankMaster("Select * from RankMaster").ToList();
                picker_RANK.ItemsSource = rankMasterlist;
                picker_RANK.ItemDisplayBinding = new Binding("RankName");
                picker_RANK.Title = "Select Rank";


                Loading_activity.IsVisible = false;
            }
        }

        private void picker_RANK_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (picker_RANK.SelectedIndex != -1)
            {

                rankid = rankMasterlist.ElementAt(picker_RANK.SelectedIndex).RankId;
            }
        }

        private void picker_MEDICALCATEGORY_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (picker_MEDICALCATEGORY.SelectedIndex != -1)
            {

                medicalid = medicalMasterlist.ElementAt(picker_MEDICALCATEGORY.SelectedIndex).MedicalId;
            }
        }

        private void picker_CHARACTER_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (picker_CHARACTER.SelectedIndex != -1)
            {

                characterid = characterMasterlist.ElementAt(picker_CHARACTER.SelectedIndex).CharacterId;
            }
        }

        private void picker_REASON_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (picker_REASON.SelectedIndex != -1)
            {

                reasonid = reasonMasterlist.ElementAt(picker_REASON.SelectedIndex).ReasonId;
            }
        }

        private void Entry_ENROLMENTdate_Focused(object sender, FocusEventArgs e)
        {
            Entry_ENROLMENTdate.Unfocus();
            Device.BeginInvokeOnMainThread(() =>
            {
                datepicker_ENROLMENTdate.MaximumDate = DateTime.Now;
                datepicker_ENROLMENTdate.Focus();
            });
        }

        private void datepicker_ENROLMENTdate_DateSelected(object sender, DateChangedEventArgs e)
        {
            ENROLMENTdateselected = e.NewDate.ToString("dd/MM/yyyy");
            Entry_ENROLMENTdate.Text = ENROLMENTdateselected.Replace('-', '/');
        }

        private void Entry_DISCHARGEdate_Focused(object sender, FocusEventArgs e)
        {
            Entry_DISCHARGEdate.Unfocus();
            Device.BeginInvokeOnMainThread(() =>
            {
                datepicker_DISCHARGEdate.MaximumDate = DateTime.Now;
                datepicker_DISCHARGEdate.Focus();
            });
        }

        private void datepicker_DISCHARGEdate_DateSelected(object sender, DateChangedEventArgs e)
        {
            dischargedateselected = e.NewDate.ToString("dd/MM/yyyy");
            Entry_DISCHARGEdate.Text = dischargedateselected.Replace('-', '/');
        }

        private void Entry_issuedate_Focused(object sender, FocusEventArgs e)
        {
            Entry_issuedate.Unfocus();
            Device.BeginInvokeOnMainThread(() =>
            {
                datepicker_issuedate.MaximumDate = DateTime.Now;
                datepicker_issuedate.Focus();
            });
        }

        private void datepicker_issuedate_DateSelected(object sender, DateChangedEventArgs e)
        {
            issuedateselected = e.NewDate.ToString("dd/MM/yyyy");
            Entry_issuedate.Text = issuedateselected.Replace('-', '/');
        }

        private void Entry_validuptodate_Focused(object sender, FocusEventArgs e)
        {
            Entry_VALIDUPTOdate.Unfocus();
            Device.BeginInvokeOnMainThread(() =>
            {
                //datepicker_validupto.MaximumDate = DateTime.Now;
                datepicker_VALIDUPTOdate.Focus();
            });
        }

        private void datepicker_validupto_DateSelected(object sender, DateChangedEventArgs e)
        {
            validuptodateselected = e.NewDate.ToString("dd/MM/yyyy");
            Entry_VALIDUPTOdate.Text = validuptodateselected.Replace('-', '/');
        }


        private async void btn_uploaddoc1_Clicked(object sender, EventArgs e)
        {
            var pickOptions = new PickOptions
            {
                PickerTitle = "Please select a PDF file",
                FileTypes = FilePickerFileType.Pdf // Use built-in PDF file filter
            };
            var result = await PickAndShow(pickOptions, 1);
        }

        async Task<FileResult> PickAndShow(PickOptions options, int docnumber)
        {
            try
            {
                var result = await FilePicker.PickAsync(options);
                if (result != null)
                {
                    var Text_ = $"File Name: {result.FileName}";
                    if (result.FileName.EndsWith("pdf", StringComparison.OrdinalIgnoreCase))
                    {
                        var stream = await result.OpenReadAsync();

                        long length = stream.Length / 1024;
                        if (length >= 1024)
                        {
                            await DisplayAlert(App.AppName, "File cannot be more than 1 Mb.", App.close);
                        }
                        else
                        {
                            if (docnumber == 1)
                            {
                                doc1 = App.ConvertToBase64(stream).ToString();
                                lbl_pdf1.IsVisible = false;
                                btn_uploaddoc1.BackgroundColor = Color.Green;
                                btn_uploaddoc1.TextColor = Color.White;
                                btn_uploaddoc1.Text = ("Successfully Uploaded");
                            }

                        }
                    }
                    else
                    {
                        await DisplayAlert(App.AppName, "Only PDF files are allowed", App.close);
                    }
                }

                return result;
            }
            catch
            {

            }

            return null;
        }

        private void btn_cancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

       

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new PostLoginDashboardPage());
        }
        private void btn_viewdoc_Clicked(object sender, EventArgs e)
        {
            // App.ConvertBase64ToPdf(xservicemenDetailsList.ElementAt(0).DocFileLink);
            var service = new UserRegistrationApi();
            service.getpdf(RegNo, "xServiceMan", "");
        }

        async Task<bool> checkvalidation()
        {
            try
            {
                if (!string.IsNullOrEmpty(entry_REGIMENTNAME.Text))
                {
                    if (!App.isAlphaNumeric(entry_REGIMENTNAME.Text))
                    {
                        await DisplayAlert(App.AppName, "Only Alphabets are allowed in Regiment Name", App.close);
                        return false;
                    }

                }

                if (!string.IsNullOrEmpty(entry_SERVICENUMBER.Text))
                {
                    if (!App.isAlphaNumeric(entry_SERVICENUMBER.Text))
                    {
                        await DisplayAlert(App.AppName, "Only Alphanumeric characters are allowed in Service Number", App.close);
                        return false;
                    }
                }

                if (!string.IsNullOrEmpty(editor_REMARKS.Text))
                {
                    if (!App.isAlphabetonly(editor_REMARKS.Text))
                    {
                        await DisplayAlert(App.AppName, "Only Alphabets are allowed in Remarks", App.close);
                    }
                }

                if (!string.IsNullOrEmpty(entry_certificateno.Text))
                {
                    if (!App.isAlphaNumeric(entry_certificateno.Text))
                    {
                        await DisplayAlert(App.AppName, "Only Alphanumeric characters are allowed in Certificate Number", App.close);
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
            Loading_activity.IsVisible = true;
            int response_SaveExServicemenDetails = await service.SaveExServicemenDetails(RegNo, forceId, rankid, ENROLMENTdateselected, dischargedateselected,
                entry_REGIMENTNAME.Text, entry_SERVICENUMBER.Text, medicalid, characterid, editor_REMARKS.Text, reasonid, issuedateselected, validuptodateselected, entry_certificateno.Text, doc1);

            if (response_SaveExServicemenDetails == 200)
            {
                Loading_activity.IsVisible = false;
               // await Navigation.PushAsync(new NCOPage()); ;
                Application.Current.MainPage = new NewUserRegistrationMasterPage(9);

            }
            Loading_activity.IsVisible = false;
        }

        private void btn_next_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new NCOPage());
            Application.Current.MainPage = new NewUserRegistrationMasterPage(9);

        }
    }
}