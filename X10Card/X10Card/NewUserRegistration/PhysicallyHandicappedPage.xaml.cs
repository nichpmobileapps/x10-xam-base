using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
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
    public partial class PhysicallyHandicappedPage : ContentPage
    {
        PhysicallyHandicappedDatabase physicallyHandicappedDatabase = new PhysicallyHandicappedDatabase();
        List<PhysicallyHandicapped> physicallyHandicappedlist;
        string physicallyHandicappedID, doc1;
        string issuedateselected, validuptodateselected, registrationdateselected;
        string RegNo;
        PHDetailsDatabase pHDetailsDatabase = new PHDetailsDatabase();
        List<PHDetails> pHDetailslist, pHDetailseditlist;

        string PHTypecd;
        DateTime Dateofbirth;


        public PhysicallyHandicappedPage()
        {
            InitializeComponent();
            lbl_navigation_header.Text = App.AppName;
            loadData();
        }

        async void loadData()
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
            btn_header.Text = "Physically Handicapped Details" + "\nRegistration No : " + RegNo + "\nName : " + service.getusername(RegNo); ;
            Loading_activity.IsVisible = true;
          
            int response_GetPH = await service.GetPH();
            Loading_activity.IsVisible = false;
            if (response_GetPH == 200)
            {
                physicallyHandicappedlist = physicallyHandicappedDatabase.GetPhysicallyHandicapped("Select * from PhysicallyHandicapped").ToList();

                picker_PHTYPE.ItemsSource = physicallyHandicappedlist;
                picker_PHTYPE.ItemDisplayBinding = new Binding("PHName");
            }

            if (!string.IsNullOrEmpty(RegNo))
            {

                Loading_activity.IsVisible = true;

                pHDetailsDatabase.DeletePHDetails();
                int response_GetPHDetails = await service.GetPHDetails(RegNo);
                if (response_GetPHDetails == 200)
                {
                    Loading_activity.IsVisible = false;
                    Loadpreviousdata();
                }else if (response_GetPHDetails == 300)
                {
                    btn_submit.IsVisible = true;
                    btn_AddPhysicallyHandicapped.IsVisible = true;
                    btn_next.IsVisible = false;
                }
                Loading_activity.IsVisible = false;
            }
        }
        private void Loadpreviousdata()
        {
            string query = $"Select *" +
                $", (case when DocFileLink <> '' then  'true' else 'false' end)pdfimgvisibility " +
                $", (case when Stat in ('-1', '3') then 'true' else 'false' end)imgeditdeletevisibility " +
                $" from PHDetails where RegistrationNo= '{RegNo}'";
            pHDetailslist = pHDetailsDatabase.GetPHDetails(query).ToList();
            listview_PhysicallyHandicapped.ItemsSource = pHDetailslist;
            string userstatus = pHDetailslist.ElementAt(0).Stat;

            if (userstatus.Equals("-1") || userstatus.Equals("3"))
            {
                btn_submit.IsVisible = true;
                btn_AddPhysicallyHandicapped.IsVisible = true;
                btn_next.IsVisible = false;
            }
            else if (userstatus.Equals("0") || userstatus.Equals("1"))
            {
                btn_submit.IsVisible = false;
                btn_AddPhysicallyHandicapped.IsVisible = false;
                btn_next.IsVisible = true;
            }

            else if (userstatus.Equals("2"))
            {
                btn_submit.IsVisible = false;
                btn_AddPhysicallyHandicapped.IsVisible = false;
                btn_next.IsVisible = true;
            }

            string DocFileLink = pHDetailslist.ElementAt(0).DocFileLink;
            if (!string.IsNullOrEmpty(DocFileLink))
            {
                doc1 = DocFileLink;
            }
            else
            {
                doc1 = "";
            }
        }


        private void btn_AddPhysicallyHandicapped_Clicked(object sender, EventArgs e)
        {

            picker_PHTYPE.SelectedIndex = 0;
            popupaddPhysicallyHandicapped.IsVisible = true;
        }

        private void picker_PHTYPE_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (picker_PHTYPE.SelectedIndex != -1)
            {

                physicallyHandicappedID = physicallyHandicappedlist.ElementAt(picker_PHTYPE.SelectedIndex).PHId;
            }
        }

        private void img_edit_Clicked(object sender, EventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            PHTypecd = b.CommandParameter.ToString();

            string query = $"Select *" +
               $" from PHDetails where PHType= '{PHTypecd}'";
            pHDetailseditlist = pHDetailsDatabase.GetPHDetails(query).ToList();

            int phtypeindex = physicallyHandicappedlist.FindIndex(s => s.PHId == pHDetailseditlist.ElementAt(0).PHType);
            if (phtypeindex != -1)
            {
                picker_PHTYPE.SelectedIndex = phtypeindex;
                physicallyHandicappedID = physicallyHandicappedlist.ElementAt(phtypeindex).PHId;
            }
            entry_PERCENTAGE.Text = pHDetailseditlist.ElementAt(0).Percentage;
            Entry_RegistrationDate.Text = pHDetailseditlist.ElementAt(0).PhRegDate;
            Entry_issuedate.Text = pHDetailseditlist.ElementAt(0).issuedt;
            Entry_validuptodate.Text = pHDetailseditlist.ElementAt(0).validuptodt;
            entry_certificateno.Text = pHDetailseditlist.ElementAt(0).DocCertNo;
            entry_REMARKS.Text = pHDetailseditlist.ElementAt(0).Remarks;

            string userstatus = pHDetailseditlist.ElementAt(0).Stat;
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


            string DocFileLink = pHDetailseditlist.ElementAt(0).DocFileLink;
            if (!string.IsNullOrEmpty(DocFileLink))
            {
                img_pdfedit.IsVisible = true;
                doc1 = DocFileLink;
            }
            else
            {
                img_pdfedit.IsVisible = false;
                doc1 = "";
            }
            popupaddPhysicallyHandicapped.IsVisible = true;
        }

        private async void img_delete_Clicked(object sender, EventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            var PHType = b.CommandParameter.ToString();

            Loading_activity.IsVisible = true;
            var service = new UserRegistrationApi();
            int response_finalsubmit = await service.DelPHDetails(RegNo, PHType);
            if (response_finalsubmit == 200)
            {
                Loading_activity.IsVisible = false;
               // await Navigation.PushAsync(new PhysicallyHandicappedPage());
                Application.Current.MainPage = new NewUserRegistrationMasterPage(7);

            }
            Loading_activity.IsVisible = false;
        }

        private void btn_cancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();

        }
        
        private void btn_popupPhysicallyHandicappedcancel_Clicked(object sender, EventArgs e)
        {
            popupaddPhysicallyHandicapped.IsVisible = false;
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

        private void Entry_issuedate_Focused(object sender, FocusEventArgs e)
        {
            Entry_issuedate.Unfocus();
            Device.BeginInvokeOnMainThread(() =>
            {
                datepicker_issuedate.MinimumDate = Dateofbirth;
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
            Entry_validuptodate.Unfocus();
            Device.BeginInvokeOnMainThread(() =>
            {
                //datepicker_validupto.MaximumDate = DateTime.Now;
                datepicker_validupto.Focus();
            });
        }

        private void datepicker_validupto_DateSelected(object sender, DateChangedEventArgs e)
        {
            validuptodateselected = e.NewDate.ToString("dd/MM/yyyy");
            Entry_validuptodate.Text = validuptodateselected.Replace('-', '/');
        }

        private async void btn_uploaddoc1_Clicked(object sender, EventArgs e)
        {
            //    await PickAndShow(PickOptions.Default, 1);
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

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new PostLoginDashboardPage());
        }
             
        private void img_pdf_Clicked(object sender, EventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            PHTypecd = b.CommandParameter.ToString();

            var service = new UserRegistrationApi();
            service.getpdf(RegNo, "PH", PHTypecd);
        }

        private void img_pdfedit_Clicked(object sender, EventArgs e)
        {
            var service = new UserRegistrationApi();
            service.getpdf(RegNo, "PH", PHTypecd);
        }


        async Task<bool> checkvalidation()
        {
            try
            {
                if (picker_PHTYPE.SelectedIndex == -1)
                {
                    await DisplayAlert(App.AppName, "Select Sub-Category", App.close);
                    return false;
                }
                if (string.IsNullOrEmpty(entry_PERCENTAGE.Text))
                {
                    await DisplayAlert(App.AppName, "ENTER PERCENTAGE", App.close);
                    return false;
                }
                else if (!App.isDecimal(entry_PERCENTAGE.Text))
                {
                    await DisplayAlert(App.AppName, "ENTER PERCENTAGE IN DECIMAL(UPTO 2 DECIMAL)", App.close);
                    return false;
                }
                if (string.IsNullOrEmpty(Entry_RegistrationDate.Text))
                {
                    await DisplayAlert(App.AppName, "Select Registration Date", App.close);
                    return false;
                }

                if (string.IsNullOrEmpty(Entry_issuedate.Text))
                {
                    await DisplayAlert(App.AppName, "Select Issue Date", App.close);
                    return false;
                }

                if (datepicker_issuedate.Date < datepicker_RegistrationDate.Date)
                {
                    await DisplayAlert(App.AppName, "Issue date cannot be less than registration date.", App.close);
                    return false;
                }

                if (string.IsNullOrEmpty(Entry_validuptodate.Text))
                {
                    await DisplayAlert(App.AppName, "Select Valid Date", App.close);
                    return false;
                }

                if (datepicker_validupto.Date < datepicker_issuedate.Date)
                {
                    await DisplayAlert(App.AppName, "Valid date cannot be less than issue date.", App.close);
                    return false;
                }
                if (string.IsNullOrEmpty(entry_certificateno.Text))
                {
                    await DisplayAlert(App.AppName, "Enter Certificate No.", App.close);
                    return false;
                }
                else if (!App.isAlphaNumeric(entry_certificateno.Text.ToString().Trim()))
                {
                    await DisplayAlert(App.AppName, "Only Alphanumeric characters are allowed in Certificate No.", App.close);
                    return false;
                }

                if (string.IsNullOrEmpty(entry_REMARKS.Text))
                {
                    await DisplayAlert(App.AppName, "Enter Remarks", App.close);
                    return false;
                }
                if (!App.isAlphabetonly(entry_REMARKS.Text))
                {
                    await DisplayAlert(App.AppName, "Only alphabets are allowed in Remarks", App.close);
                    return false;
                }

                if (string.IsNullOrEmpty(doc1))
                {
                    await DisplayAlert(App.AppName, "Upload Document", App.close);
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

        private void btn_submit_Clicked(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new ExServiceManPage());
            Application.Current.MainPage = new NewUserRegistrationMasterPage(8);

        }

        private async void btn_popupPhysicallyHandicappedsubmit_Clicked(object sender, EventArgs e)
        {
            var service = new UserRegistrationApi();
            if (await checkvalidation())
            {
                Loading_activity.IsVisible = true;
                int response_SavePhysicallyHandicappedDetails = await service.SavePhysicallyHandicappedDetails(RegNo, physicallyHandicappedID, entry_PERCENTAGE.Text,
                    entry_REMARKS.Text, registrationdateselected, issuedateselected, validuptodateselected, entry_certificateno.Text, doc1);

                if (response_SavePhysicallyHandicappedDetails == 200)
                {
                    Loading_activity.IsVisible = false;
                   // await Navigation.PushAsync(new PhysicallyHandicappedPage()); ;
                       Application.Current.MainPage = new NewUserRegistrationMasterPage(7);
                }
                Loading_activity.IsVisible = false;
            }
        }

        private  void btn_next_Clicked(object sender, EventArgs e)
        {
            // await Navigation.PushAsync(new ExServiceManPage());
            Application.Current.MainPage = new NewUserRegistrationMasterPage(8);

        }
    }
}