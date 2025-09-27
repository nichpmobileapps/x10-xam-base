using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
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
    public partial class QualficationDetailsPage : ContentPage
    {
        QualificationMasterDatabase qualificationMasterDatabase = new QualificationMasterDatabase();
        List<QualificationMaster> qualificationMasterslist;
        string qualificationid;
        BoardMasterDatabase boardMasterDatabase = new BoardMasterDatabase();
        List<BoardMaster> boardMasterslist;
        string boardid;

        string registrationdateselected, issuedateselected;
        string doc1;
        string RegNo;
        DateTime Dateofbirth;
        QualficationDetailsDatabase qualficationDetailsDatabase = new QualficationDetailsDatabase();
        List<QualficationDetails> qualficationDetailsList, qualficationDetailseditList;
        string QualCd;
        int percentComplete;
        public QualficationDetailsPage()
        {
            InitializeComponent();
            lbl_navigation_header.Text = App.AppName;

            loadata();
        }

        private async void loadata()
        {
            // DOB = DateTime.ParseExact(Preferences.Get("DOB", ""), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
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
            
                RegNo = Preferences.Get("RegNo", "");
            


            Loading_activity.IsVisible = true;
            var service = new UserRegistrationApi();

            int response_GetQualificationMaster = await service.GetQualificationMaster();
            int response_GetBoardMaster = await service.GetBoardMaster();
            Loading_activity.IsVisible = false;

            if (response_GetQualificationMaster == 200)
            {
                qualificationMasterslist = qualificationMasterDatabase.GetQualificationMaster("Select * from QualificationMaster order by QualificationName Asc").ToList();
                listview_Qualificationname.ItemsSource=qualificationMasterslist;
               /* picker_QUALIFICATION.ItemsSource = qualificationMasterslist;
                picker_QUALIFICATION.ItemDisplayBinding = new Binding("QualificationName");*/
            }

            if (response_GetBoardMaster == 200)
            {
                boardMasterslist = boardMasterDatabase.GetBoardMaster("Select * from BoardMaster order by BoardName Asc").ToList();
                picker_BOARD.ItemsSource = boardMasterslist;
                picker_BOARD.ItemDisplayBinding = new Binding("BoardName");
            }


            btn_header.Text = "Education Details" + "\nRegistration No : " + RegNo + "\nName : " + service.getusername(RegNo);
            if (!string.IsNullOrEmpty(RegNo))
            {

                Loading_activity.IsVisible = true;

                int response_GetEducationDetails = await service.GetEducationDetails(RegNo);
                if (response_GetEducationDetails == 200)
                {
                    Loadpreviousdata();
                    Loading_activity.IsVisible = false;
                }
                else if (response_GetEducationDetails == 300)
                {
                    btn_submit1.IsVisible = true;
                    btn_AddQualification.IsVisible = true;
                    btn_next.IsVisible = false;
                }
                Loading_activity.IsVisible = false;
            }

        }

        private void Loadpreviousdata()
        {
            string query = $"Select *" +
                $", (VerifyStatus ||' ' ||VerifiedDt)as VerifyStatus" +
                $", (case when DocFileLink <> '' then  'true' else 'false' end)pdfimgvisibility " +
                 $", (case when Stat in ('-1', '3') then 'true' else 'false' end)imgeditdeletevisibility " +
                $" from QualficationDetails where RegistrationNo= '{RegNo}'";
            qualficationDetailsList = qualficationDetailsDatabase.GetQualficationDetails(query).ToList();
            listview_Qualificationdetails.ItemsSource = qualficationDetailsList;

            string userstatus = qualficationDetailsList.ElementAt(0).Stat;

            if (userstatus.Equals("-1") || userstatus.Equals("3"))
            {
                btn_submit1.IsVisible = true;
                btn_AddQualification.IsVisible = true;
                btn_next.IsVisible = false;
            }
            else if (userstatus.Equals("0") || userstatus.Equals("1"))
            {
                btn_submit1.IsVisible = false;
                btn_AddQualification.IsVisible = false;
                btn_next.IsVisible = true;
            }

            else if (userstatus.Equals("2"))
            {
                btn_submit1.IsVisible = false;
                btn_AddQualification.IsVisible = false;
                btn_next.IsVisible = true;
            }


            string DocFileLink = qualficationDetailsList.ElementAt(0).DocFileLink;
            if (!string.IsNullOrEmpty(DocFileLink))
            {
                doc1 = DocFileLink;
            }
            else
            {
                doc1 = "";
            }
        }

        private void btn_AddQualification_Clicked(object sender, EventArgs e)
        {
            //picker_QUALIFICATION.SelectedIndex = 0;
            picker_BOARD.SelectedIndex = 0;

            popupaddqualification.IsVisible = true;
        }

     /*   private void picker_QUALIFICATION_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (picker_QUALIFICATION.SelectedIndex != -1)
            {
                qualificationid = qualificationMasterslist.ElementAt(picker_QUALIFICATION.SelectedIndex).QualificationID;
            }
        }*/

        private void picker_BOARD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (picker_BOARD.SelectedIndex != -1)
            {

                boardid = boardMasterslist.ElementAt(picker_BOARD.SelectedIndex).BoardID;

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

        public async void btn_uploaddoc1_Clicked(object sender, EventArgs e)
        {
            doc1 = "";
            img_pdfedit.IsVisible = false;
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
            popupaddqualification.IsVisible = false;
        }

        private void btn_cancel1_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void img_edit_Clicked(object sender, EventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            QualCd = b.CommandParameter.ToString();

            string query = $"Select * " +
                 $", (case when DocFileLink <> '' then  'true' else 'false' end)pdfimgvisibility " +
                $" from QualficationDetails where QualCd= '{QualCd}'";
            qualficationDetailseditList = qualficationDetailsDatabase.GetQualficationDetails(query).ToList();

            /* int qualificationindex = qualificationMasterslist.FindIndex(s => s.QualificationID == qualficationDetailseditList.ElementAt(0).QualCd);
             if (qualificationindex != -1)
             {
                 picker_QUALIFICATION.SelectedIndex = qualificationindex;
                 qualificationid = qualificationMasterslist.ElementAt(qualificationindex).QualificationID;
             }*/
            editor_QualificationName.Text = qualficationDetailseditList.ElementAt(0).QualNm;
            qualificationid = QualCd;

            int boardindex = boardMasterslist.FindIndex(s => s.BoardID == qualficationDetailseditList.ElementAt(0).ddlboard);
            if (boardindex != -1)
            {
                picker_BOARD.SelectedIndex = boardindex;
                boardid = boardMasterslist.ElementAt(boardindex).BoardID;
            }

            entry_TOTALMARKS.Text = qualficationDetailseditList.ElementAt(0).tmarks;
            entry_MARKSOBTAINED.Text = qualficationDetailseditList.ElementAt(0).omarks;


            btn_calclulatepercent.IsVisible = false;
            entry_Percentage.Text = qualficationDetailseditList.ElementAt(0).perMarks;
            stack_percentage.IsVisible = true;

            entry_YEAROFPASSING.Text = qualficationDetailseditList.ElementAt(0).year;
            Entry_RegistrationDate.Text = qualficationDetailseditList.ElementAt(0).RegDt;
            registrationdateselected = Entry_RegistrationDate.Text;
            Entry_issuedate.Text = qualficationDetailseditList.ElementAt(0).issuedt;
            issuedateselected = Entry_issuedate.Text;
            entry_certificateno.Text = qualficationDetailseditList.ElementAt(0).DocCertNo;

            string DocFileLink = qualficationDetailseditList.ElementAt(0).DocFileLink;
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

            popupaddqualification.IsVisible = true;
        }

        private async void img_delete_Clicked(object sender, EventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            var QualCd = b.CommandParameter.ToString();

            Loading_activity.IsVisible = true;
            var service = new UserRegistrationApi();
            int response_finalsubmit = await service.DelEducationDetails(RegNo, QualCd);
            if (response_finalsubmit == 200)
            {
                Loading_activity.IsVisible = false;
                Application.Current.MainPage = new NewUserRegistrationMasterPage(3);

               // await Navigation.PushAsync(new QualficationDetailsPage());
            }
            Loading_activity.IsVisible = false;
        }

        private void img_pdf_Clicked(object sender, EventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            QualCd = b.CommandParameter.ToString();

            var service = new UserRegistrationApi();
            service.getpdf(RegNo, "qualification", QualCd);
        }

        private void img_pdfedit_Clicked(object sender, EventArgs e)
        {

            var service = new UserRegistrationApi();
            service.getpdf(RegNo, "qualification", QualCd);
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new PostLoginDashboardPage());
        }

        async Task<bool> checkvalidation()
        {
            try
            {
                if (!string.IsNullOrEmpty(editor_QualificationName.Text))
                {
                    await DisplayAlert(App.AppName, "Select Qualification", App.close);
                    return false;
                }

                if (picker_BOARD.SelectedIndex == -1)
                {
                    await DisplayAlert(App.AppName, "Select Board", App.close);
                    return false;
                }

                if (!string.IsNullOrEmpty(entry_TOTALMARKS.Text))
                {

                    /* if (!App.isDecimal(entry_TOTALMARKS.Text.ToString().Trim()))
                     {
                         await DisplayAlert(App.AppName, "Only Numeric characters are allowed in TOTAL MARKS", "Close");
                         return false;
                     }*/

                    if (string.IsNullOrEmpty(entry_MARKSOBTAINED.Text))
                    {
                        await DisplayAlert(App.AppName, "ENTER MARKS OBTAINED", App.close);
                        return false;
                    }
                    /* else if (!App.isDecimal(entry_TOTALMARKS.Text.ToString().Trim()))
                     {
                         await DisplayAlert(App.AppName, "Only Numeric characters are allowed in MARKS OBTAINED", "Close");
                         return false;
                     }*/
                    double value1 = Convert.ToDouble(entry_TOTALMARKS.Text);
                    double value2 = Convert.ToDouble(entry_MARKSOBTAINED.Text);
                    int status = value1.CompareTo(value2);
                    if (status < 0)
                    {
                        await DisplayAlert(App.AppName, "MARKS OBTAINED CANNOT BE GREATER THAN TOTAL MARKS", App.close);
                        return false;
                    }
                }

                if (string.IsNullOrEmpty(entry_Percentage.Text))
                {
                    await DisplayAlert(App.AppName, "CALCULATE PERCENTAGE", App.close);
                    return false;
                }
                /*else if (!App.isDecimal(entry_Percentage.Text))
                {
                    await DisplayAlert(App.AppName, "ENTER PERCENTAGE IN DECIMAL(UPTO 2 DECIMAL)", App.close);
                    return false;
                }*/

                if (string.IsNullOrEmpty(entry_YEAROFPASSING.Text))
                {
                    await DisplayAlert(App.AppName, "ENTER YEAR OF PASSING", App.close);
                    return false;
                }
                else if (!App.isNumeric(entry_YEAROFPASSING.Text.ToString().Trim()))
                {
                    await DisplayAlert(App.AppName, "Only Numeric characters are allowed in YEAR OF PASSING", "Close");
                    return false;
                }
                else if (int.Parse(entry_YEAROFPASSING.Text) > DateTime.Now.Year)
                {
                    await DisplayAlert(App.AppName, "YEAR OF PASSING CANNOT BE GREATER THAN CURRENT YEAR", "Close");
                    return false;
                }
                if (string.IsNullOrEmpty(Entry_RegistrationDate.Text))
                {
                    await DisplayAlert(App.AppName, "Enter Registration Date", App.close);
                    return false;
                } if (string.IsNullOrEmpty(Entry_issuedate.Text))
                {
                    await DisplayAlert(App.AppName, "Enter Issue Date", App.close);
                    return false;
                }

                if (datepicker_issuedate.Date < datepicker_RegistrationDate.Date)
                {
                    await DisplayAlert(App.AppName, "Issue date cannot be less than registration  date.", App.close);
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

        private async void btn_submit_Clicked(object sender, EventArgs e)
        {

            var service = new UserRegistrationApi();
            if (await checkvalidation())
            {
                Loading_activity.IsVisible = true;
                int response_SaveEducationDetails = await service.SavEducationDetails(RegNo, qualificationid, boardid,
                          entry_TOTALMARKS.Text, entry_MARKSOBTAINED.Text, entry_Percentage.Text, entry_YEAROFPASSING.Text,
                          registrationdateselected, issuedateselected, entry_certificateno.Text, doc1);


                if (response_SaveEducationDetails == 200)
                {
                    Loading_activity.IsVisible = false;
                    // await Navigation.PushAsync(new QualficationDetailsPage());
                    Application.Current.MainPage = new NewUserRegistrationMasterPage(3);

                }
                Loading_activity.IsVisible = false;
            }

        }

        private void btn_calclulatepercent_Clicked(object sender, EventArgs e)
        {
            int totalmarks = int.Parse(entry_TOTALMARKS.Text);
            int obtmarks = int.Parse(entry_MARKSOBTAINED.Text);
            percentComplete = (int)Math.Round((double)(100 * obtmarks) / totalmarks);
            entry_Percentage.Text = percentComplete.ToString();
            stack_percentage.IsVisible = true;
        }

        private void editor_QualificationName_Focused(object sender, FocusEventArgs e)
        {
            editor_QualificationName.Unfocus();
            popupQualificationname.IsVisible = true;
        }

        private void listview_Qualificationname_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var currentrecord = e.Item as QualificationMaster;
            editor_QualificationName.Text = currentrecord.QualificationName;
            qualificationid = currentrecord.QualificationID;
            popupQualificationname.IsVisible = false;
        }

        private void popupQualificationnameCancel_Clicked(object sender, EventArgs e)
        {
            popupQualificationname.IsVisible = false;
        }

        private void SearchBar_Qualificationname_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(SearchBar_Qualificationname.Text))
            {
                string texttosearch = SearchBar_Qualificationname.Text.ToLower().Trim();
                listview_Qualificationname.ItemsSource = qualificationMasterslist.Where(t =>
                               t.QualificationID.ToLower().Contains(texttosearch) ||
                               t.QualificationName.ToLower().Contains(texttosearch)
                               ).ToList();
            }
            else
            {
                qualificationMasterslist = qualificationMasterDatabase.GetQualificationMaster("Select * from QualificationMaster order by QualificationName Asc").ToList();

                listview_Qualificationname.ItemsSource = qualificationMasterslist;
            }
        }

        private void btn_submit1_Clicked(object sender, EventArgs e)
        {

           // Navigation.PushAsync(new MiscellaneousDetailsPage());
            Application.Current.MainPage = new NewUserRegistrationMasterPage(4);

        }

        private  void btn_next_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new MiscellaneousDetailsPage());
            Application.Current.MainPage = new NewUserRegistrationMasterPage(4);

        }

    }
}