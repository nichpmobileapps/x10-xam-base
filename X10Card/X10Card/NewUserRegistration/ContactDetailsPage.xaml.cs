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
    public partial class ContactDetailsPage : ContentPage
    {
        DistrictMasterDatabase districtMasterDatabase = new DistrictMasterDatabase();
        List<DistrictMaster> districtMasterslist;
        string DistrictCode;
        string doc1;

        TehsilMasterDatabase tehsilMasterDatabase = new TehsilMasterDatabase();
        List<TehsilMaster> tehsilMasterslist;
        string tehsilid;
        VillageMasterDatabase villageMasterDatabase = new VillageMasterDatabase();
        List<VillageMaster> villageslist;
        string villageid;

        string issuedateselected;
        string RegNo;
        string ruralurbanid;
        DateTime Dateofbirth;

        ContactDetailsDatabase contactDetailsDatabase = new ContactDetailsDatabase();
        List<ContactDetails> contactDetailsList;

        public ContactDetailsPage()
        {
            InitializeComponent();
            lbl_navigation_header.Text = App.AppName;
            ruralurbanid = "R";
            
            Loading_activity.IsVisible = true;  
            loadata();
            Loading_activity.IsVisible = false;
            
        }

        private async void loadata()
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
            btn_header.Text = "Contact Details" + "\nRegistration No : " + RegNo + "\nName : " + service.getusername(RegNo); ;

           
            
            int response_getdistrict = await service.GetDistrict();

            if (response_getdistrict == 200)
            {
                districtMasterslist = districtMasterDatabase.GetDistrictMaster("Select * from DistrictMaster").ToList();
                picker_district.ItemsSource = districtMasterslist;
                picker_district.ItemDisplayBinding = new Binding("DistrictName");
                picker_district.SelectedIndex = 0;
            }

            if (!string.IsNullOrEmpty(RegNo))
            {
                Loading_activity.IsVisible=true;
                int response_GetContactDetails = await service.GetContactDetails(RegNo);
                if (response_GetContactDetails == 200)
                {
                    btn_header.Text = "Contact Details" + "\nRegistration No : " + RegNo + "\nName : " + service.getusername(RegNo);
                   

                    Loading_activity.IsVisible = true;
                    lbl_PleaseWait.Text = "Please Wait...";
                    Loadpreviousdata();
                    Loading_activity.IsVisible = false;
                }
            }
            Loading_activity.IsVisible = false;
        }

        private async void Loadpreviousdata()
        {

            contactDetailsList = contactDetailsDatabase.GetContactDetails($"Select * from ContactDetails where RegistrationNo= '{RegNo}'").ToList();
            string userstatus = contactDetailsList.ElementAt(0).Stat;

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

           
            var service = new UserRegistrationApi();
            int districtindex = districtMasterslist.FindIndex(s => s.DistrictID == contactDetailsList.ElementAt(0).DistrictCode);
            if (districtindex != -1)
            {
                picker_district.SelectedIndex = districtindex;
                DistrictCode = districtMasterslist.ElementAt(districtindex).DistrictID;
                int response_GetTehsil = await service.GetTehsil(DistrictCode);
                if (response_GetTehsil == 200)
                {
                    tehsilMasterslist = tehsilMasterDatabase.GetTehsilMaster("Select * from TehsilMaster").ToList();
                }
            }

            int tehsilindex = tehsilMasterslist.FindIndex(s => s.TehsilID == contactDetailsList.ElementAt(0).ddltehsilcode);
            if (tehsilindex != -1)
            {
                picker_tehsil.SelectedIndex = tehsilindex;
                tehsilid = tehsilMasterslist.ElementAt(tehsilindex).TehsilID;

                
                int response_GetVillageMaster = await service.GetVillageMaster(DistrictCode, tehsilid);
                if (response_GetVillageMaster == 200)
                {
                 
                    villageid = contactDetailsList.ElementAt(0).ddlvillage;
                    villageslist = villageMasterDatabase.GetVillageMaster($"Select * from VillageMaster where VillageID='{villageid}'").ToList();

                    if (villageslist.Any())
                    {
                        string VillageName = villageslist.ElementAt(0).VillageName;
                        if (!string.IsNullOrEmpty(VillageName))
                        {
                            editor_VillageName.Text = villageslist.ElementAt(0).VillageName;
                        }
                    }
                  
                }
              
            }


            entry_mobileno.Text = contactDetailsList.ElementAt(0).txtmobile;
            entry_email.Text = contactDetailsList.ElementAt(0).txtemail;
            string areatype = contactDetailsList.ElementAt(0).Areatype;
            if (areatype.Equals("R"))
            {
                rd_RURAL.IsChecked = true;
                rd_URBAN.IsChecked = false;
                ruralurbanid = "R";
            }
            else
            {
                rd_RURAL.IsChecked = false;
                rd_URBAN.IsChecked = true;
                ruralurbanid = "U";
            }
            entry_PO.Text = contactDetailsList.ElementAt(0).txtpo;
            editor_streetbuilding.Text = contactDetailsList.ElementAt(0).txtstreet;
            entry_pincode.Text = contactDetailsList.ElementAt(0).txtpincode;
            entry_phoneno.Text = contactDetailsList.ElementAt(0).txtphone;
            editor_permanentaddress.Text = contactDetailsList.ElementAt(0).addressmain;
            editor_corresponsenceaddress.Text = contactDetailsList.ElementAt(0).txtaddressco;
            if (!string.IsNullOrEmpty(contactDetailsList.ElementAt(0).sameasabove))
            {
                chkbx_address.IsChecked = true;
            }
            Entry_issuedate.Text = contactDetailsList.ElementAt(0).IssueDt;

            

            entry_certificateno.Text = contactDetailsList.ElementAt(0).DocCertificateNo;
            string pdfifle = contactDetailsList.ElementAt(0).DocFileLink;
            if (!string.IsNullOrEmpty(pdfifle))
            {
                btn_viewdoc.IsVisible = true;
                doc1 = pdfifle;
            }
            else
            {
                btn_viewdoc.IsVisible = false;
                doc1 = "";
            }
           
        }

        private void btn_viewdoc_Clicked(object sender, EventArgs e)
        {
            var service = new UserRegistrationApi();
            service.getpdf(RegNo,"Contact","");
            //App.ConvertBase64ToPdf(contactDetailsList.ElementAt(0).DocFileLink);

        }

        private void rd_RURAL_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (rd_RURAL.IsChecked)
            {
                ruralurbanid = "R";
            }
            else
            {
                ruralurbanid = "U";
            }
        }

        private async void picker_district_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (picker_district.SelectedIndex != -1)
            {

                DistrictCode = districtMasterslist.ElementAt(picker_district.SelectedIndex).DistrictID;
                // EmploymentStatusName = employmentStatuslist.ElementAt(picker_district.SelectedIndex).EmpStatDesc;
                Loading_activity.IsVisible = true;
                var service = new UserRegistrationApi();
                int response_GetTehsil = await service.GetTehsil(DistrictCode);
                if (response_GetTehsil == 200)
                {
                    tehsilMasterslist = tehsilMasterDatabase.GetTehsilMaster("Select * from TehsilMaster").ToList();
                    picker_tehsil.ItemsSource = tehsilMasterslist;
                    picker_tehsil.ItemDisplayBinding = new Binding("TehsilName");
                    picker_tehsil.SelectedIndex = 0;
                }
                Loading_activity.IsVisible = false;
            }

        }

        private async void picker_tehsil_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (picker_tehsil.SelectedIndex != -1)
            {

                tehsilid = tehsilMasterslist.ElementAt(picker_tehsil.SelectedIndex).TehsilID;
               
                Loading_activity.IsVisible = true;
                var service = new UserRegistrationApi();
                int response_GetVillageMaster = await service.GetVillageMaster(DistrictCode, tehsilid);
                if (response_GetVillageMaster == 200)
                {
                    villageslist = villageMasterDatabase.GetVillageMaster("Select * from VillageMaster").ToList();
                    listview_villagename.ItemsSource = villageslist;
                    /* picker_village.ItemsSource = villageslist;
                     picker_village.ItemDisplayBinding = new Binding("VillageName");
                     picker_village.SelectedIndex = 0;*/
                }
                Loading_activity.IsVisible = false;
            }
        }

        /* private void picker_village_SelectedIndexChanged(object sender, EventArgs e)
         {
             if (picker_village.SelectedIndex != -1)
             {
                 villageid = villageslist.ElementAt(picker_village.SelectedIndex).VillageID;
             }
         }*/

        private void chkbx_address_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (chkbx_address.IsChecked)
            {
                editor_corresponsenceaddress.Text = editor_permanentaddress.Text;
            }
            else
            {
                editor_corresponsenceaddress.Text = "";
            }
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
            //  await PickAndShow(PickOptions.Default, 1);
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
                            await DisplayAlert(App.AppName, "File size cannot be more than 1 MB.", App.close);
                        }
                        else
                        {
                            if (docnumber == 1)
                            {
                                doc1 = App.ConvertToBase64(stream).ToString();
                                lbl_pdf1.IsVisible = false;
                                btn_uploaddoc1.BackgroundColor = Color.Green;
                                btn_uploaddoc1.TextColor = Color.White;
                                btn_uploaddoc1.Text = "Successfully Uploaded";
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
            //App.Current.MainPage=new NavigationPage(new PersonalDetailsPage());
            Navigation.PopAsync();
        }
         
        async Task<bool> checkvalidation()
        {
            try
            {
                if (string.IsNullOrEmpty(entry_mobileno.Text))
                {
                    await DisplayAlert(App.AppName, "Enter Mobile No.", "Close");
                    return false;
                }
                else if (!App.isNumeric(entry_mobileno.Text.ToString().Trim()))
                {
                    await DisplayAlert(App.AppName, "Only Numeric characters are allowed in Mobile No.", "Close");
                    return false;
                }
                if (entry_mobileno.Text.Length < 10)
                {
                    await DisplayAlert(App.AppName, "Enter 10 digit mobile no.", App.close);
                    return false;
                }
                if (long.Parse(entry_mobileno.Text) < 6000000000)
                {
                    await DisplayAlert(App.AppName, "Invalid Mobile No.\nMobile should start with 6,7,8,9.", App.close);
                    return false;
                }
                if (string.IsNullOrEmpty(entry_email.Text))
                {
                    await DisplayAlert(App.AppName, "Enter Email Id.", "Close");
                    return false;
                }
                if (!App.ValidateEmail(entry_email.Text))
                {
                    await DisplayAlert(App.AppName, "Enter Valid Email-Id", "Close");
                    return false;
                }
                if (picker_district.SelectedIndex == -1)
                {
                    await DisplayAlert(App.AppName, "Select District", "Close");
                    return false;
                }



                if (picker_tehsil.SelectedIndex == -1)
                {
                    await DisplayAlert(App.AppName, "Select Tehsil", "Close");
                    return false;
                }

                if (string.IsNullOrEmpty(editor_VillageName.Text))
                {
                    await DisplayAlert(App.AppName, "Select Village", "Close");
                    return false;
                }

                if (string.IsNullOrEmpty(entry_PO.Text))
                {
                    await DisplayAlert(App.AppName, "Enter PO", "Close");
                    return false;
                }

                if (string.IsNullOrEmpty(entry_pincode.Text))
                {
                    await DisplayAlert(App.AppName, "Enter Pincode", "Close");
                    return false;
                }
                else if (!App.isNumeric(entry_pincode.Text.ToString().Trim()))
                {
                    await DisplayAlert(App.AppName, "Only Numeric characters are allowed in pincode", "Close");
                    return false;
                }

                if (string.IsNullOrEmpty(editor_permanentaddress.Text))
                {
                    await DisplayAlert(App.AppName, "Enter Permanent Address", "Close");
                    return false;
                }

                if (!App.isAlphaNumeric(editor_permanentaddress.Text))
                {
                    await DisplayAlert(App.AppName, "Only AlphaNumeric characters are allowed in Permanent Address", "Close");

                    return false;
                }

                if (string.IsNullOrEmpty(editor_corresponsenceaddress.Text))
                {
                    await DisplayAlert(App.AppName, "Enter Correspondence Address", "Close");
                    return false;
                }

                if (!App.isAlphaNumeric(editor_corresponsenceaddress.Text))
                {
                    await DisplayAlert(App.AppName, "Only AlphaNumeric characters are allowed in Correspondence Address", "Close");
                    return false;
                }

                if (!string.IsNullOrEmpty(Entry_issuedate.Text))
                {

                    if (string.IsNullOrEmpty(entry_certificateno.Text))
                    {
                        await DisplayAlert(App.AppName, "Enter Certificate No.", "Close");
                        return false;
                    }
                    else if (!App.isAlphaNumeric(entry_certificateno.Text.ToString().Trim()))
                    {
                        await DisplayAlert(App.AppName, "Only Alphanumeric characters are allowed in Certificate No.", "Close");
                        return false;
                    }
                    if (string.IsNullOrEmpty(doc1))
                    {
                        await DisplayAlert(App.AppName, "Upload Document", "Close");
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

        private void editor_VillageName_Focused(object sender, FocusEventArgs e)
        {
            editor_VillageName.Unfocus();
            popupvillagename.IsVisible = true;
        }

        private void listview_villagename_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var currentrecord = e.Item as VillageMaster;
            editor_VillageName.Text = currentrecord.VillageName;
            villageid = currentrecord.VillageID;
            popupvillagename.IsVisible = false;
        }

        private void popupvillagenameCancel_Clicked(object sender, EventArgs e)
        {
            popupvillagename.IsVisible = false;
        }
        private void SearchBar_villagename_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(SearchBar_villagename.Text))
            {
                string texttosearch = SearchBar_villagename.Text.ToLower().Trim();
                listview_villagename.ItemsSource = villageslist.Where(t =>
                               t.VillageID.ToLower().Contains(texttosearch) ||
                               t.VillageName.ToLower().Contains(texttosearch)
                               ).ToList();
            }
            else
            {
                villageslist = villageMasterDatabase.GetVillageMaster("Select * from VillageMaster").ToList();
                listview_villagename.ItemsSource = villageslist;
            }
        }


        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new PostLoginDashboardPage());
        }

        private async void btn_submit_Clicked(object sender, EventArgs e)
        {
            var service = new UserRegistrationApi();
            if (await checkvalidation())
            {
                Loading_activity.IsVisible = true;
                int response_SaveContactDetails = await service.SaveContactDetails(RegNo, entry_mobileno.Text, entry_email.Text, DistrictCode, ruralurbanid, tehsilid,
                            villageid, entry_PO.Text, editor_streetbuilding.Text, entry_pincode.Text, entry_phoneno.Text,
                            editor_permanentaddress.Text, editor_corresponsenceaddress.Text, Entry_issuedate.Text, entry_certificateno.Text, doc1);

                if (response_SaveContactDetails == 200)
                {
                    Loading_activity.IsVisible = false;  
                    Application.Current.MainPage = new NewUserRegistrationMasterPage(3);

                    //await Navigation.PushAsync(new QualficationDetailsPage());
                }
                Loading_activity.IsVisible = false;
            }

        }


        private  void btn_next_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NewUserRegistrationMasterPage(3);

            //await Navigation.PushAsync(new QualficationDetailsPage());
        }
    }
}