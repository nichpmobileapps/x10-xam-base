using System;
using System.Collections.Generic;
using System.Globalization;
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
    public partial class SubCategoryPage : ContentPage
    {
        SubCategoryDatabase subCategoryDatabase = new SubCategoryDatabase();
        List<SubCategory> subCategorieslist;
        string catgeoryid, doc1;

        string issuedateselected, validuptodateselected;
        string UserID, RegNo;

        SubCategoryDetailsDatabase subCategoryDetailsDatabase = new SubCategoryDetailsDatabase();
        List<SubCategoryDetails> subCategoryDetailslist, subCategoryDetailseditlist;
        string SubCategorycd;
        DateTime Dateofbirth;
        public SubCategoryPage()
        {
            InitializeComponent();
            lbl_navigation_header.Text = App.AppName;
            loadData();
        }

        async void loadData()
        {
           
                UserID = Preferences.Get("UserID", "");
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
            btn_header.Text = "Sub-Category Details" + "\nRegistration No : " + RegNo + "\nName : " + service.getusername(RegNo); ;

            Loading_activity.IsVisible = true;
         
            int response_getsubcat = await service.GetSubCategory();
            Loading_activity.IsVisible = false;
            if (response_getsubcat == 200)
            {
                subCategorieslist = subCategoryDatabase.GetSubCategory("Select * from SubCategory").ToList();

                picker_SUBCATEGORY.ItemsSource = subCategorieslist;
                picker_SUBCATEGORY.ItemDisplayBinding = new Binding("SubCategoryName");
            }

            if (!string.IsNullOrEmpty(RegNo))
            {
                int response_GetSubcategoryDetails = await service.GetSubcategoryDetails(RegNo);
                if (response_GetSubcategoryDetails == 200)
                {
                    subCategoryDetailslist = subCategoryDetailsDatabase.GetSubCategoryDetails($"Select *, " +
                        $"(case when DocFileLink <> '' then  'true' else 'false' end)pdfimgvisibility " +
                         $", (case when Stat in ('-1', '3') then 'true' else 'false' end)imgeditdeletevisibility " +
                        $" from SubCategoryDetails where RegistrationNo='{RegNo}'").ToList();
                    if (subCategoryDetailslist.Any())
                    {
                        string subcategory = subCategoryDetailslist.ElementAt(0).SubCategoryNm;
                        if (!string.IsNullOrEmpty(subcategory))
                        {
                            listview_SubCategory.ItemsSource = subCategoryDetailslist;

                            //listview_SubCategory.IsVisible = true;
                        }
                        else
                        {
                            listview_SubCategory.ItemsSource = null;

                            //listview_SubCategory.IsVisible= false;
                        }
                    }

                    string DocFileLink = subCategoryDetailslist.ElementAt(0).DocFileLink;
                    if (!string.IsNullOrEmpty(DocFileLink))
                    {

                        doc1 = DocFileLink;
                    }
                    else
                    {
                        doc1 = "";
                    }

                    string userstatus = subCategoryDetailslist.ElementAt(0).Stat;
                    if (userstatus.Equals("-1") || userstatus.Equals("3"))
                    {
                        btn_submit1.IsVisible = true;
                        btn_next.IsVisible = false;
                        btn_AddSubcatgeory.IsVisible = true;
                    }
                    else if (userstatus.Equals("0") || userstatus.Equals("1"))
                    {
                        btn_submit1.IsVisible = false;
                        btn_AddSubcatgeory.IsVisible = false;
                        btn_next.IsVisible = true;
                    }

                    else if (userstatus.Equals("2"))
                    {
                        btn_AddSubcatgeory.IsVisible = false;
                        btn_submit1.IsVisible = false;
                        btn_next.IsVisible = true;
                    }
                }
            }
        }

        private void btn_AddSubcatgeory_Clicked(object sender, EventArgs e)
        {
            picker_SUBCATEGORY.SelectedIndex = 0;
            popupaddsubcategory.IsVisible = true;
        }

        private void picker_SUBCATEGORY_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (picker_SUBCATEGORY.SelectedIndex != -1)
            {

                catgeoryid = subCategorieslist.ElementAt(picker_SUBCATEGORY.SelectedIndex).SubCategoryId;
            }
        }

        private void img_edit_Clicked(object sender, EventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            SubCategorycd = b.CommandParameter.ToString();

            subCategoryDetailseditlist = subCategoryDetailsDatabase.GetSubCategoryDetails($"Select * from SubCategoryDetails " +
                $"where SubCategory='{SubCategorycd}'").ToList();

            int subcategoryindex = subCategorieslist.FindIndex(s => s.SubCategoryId == subCategoryDetailseditlist.ElementAt(0).SubCategory);
            if (subcategoryindex != -1)
            {
                picker_SUBCATEGORY.SelectedIndex = subcategoryindex;
                catgeoryid = subCategorieslist.ElementAt(subcategoryindex).SubCategoryId;
            }

            Entry_issuedate.Text = subCategoryDetailseditlist.ElementAt(0).issuedt;
            Entry_validuptodate.Text = subCategoryDetailseditlist.ElementAt(0).validuptodt;
            entry_certificateno.Text = subCategoryDetailseditlist.ElementAt(0).DocCertNo;
            string DocFileLink = subCategoryDetailseditlist.ElementAt(0).DocFileLink;
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
            popupaddsubcategory.IsVisible = true;
        }

        private async void img_delete_Clicked(object sender, EventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            var SubCategory = b.CommandParameter.ToString();

            Loading_activity.IsVisible = true;
            var service = new UserRegistrationApi();
            int response_finalsubmit = await service.DelSubcatDetails(RegNo, SubCategory);
            if (response_finalsubmit == 200)
            {
                Loading_activity.IsVisible = false;
                Application.Current.MainPage = new NewUserRegistrationMasterPage(6);
            }
            Loading_activity.IsVisible = false;
        }

        private void btn_cancel1_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
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
            doc1 = "";
            img_pdfedit.IsVisible=false;
            //await PickAndShow(PickOptions.Default, 1);
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

        private void btn_popupsubcategorycancel_Clicked(object sender, EventArgs e)
        {
            popupaddsubcategory.IsVisible = false;
        }
                   
        private void img_pdf_Clicked(object sender, EventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            SubCategorycd = b.CommandParameter.ToString();

            var service = new UserRegistrationApi();
            service.getpdf(RegNo, "subcategory", SubCategorycd);
        }

        private void img_pdfedit_Clicked(object sender, EventArgs e)
        {
            var service = new UserRegistrationApi();
            service.getpdf(RegNo, "subcategory", SubCategorycd);
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new PostLoginDashboardPage());
        }

        async Task<bool> checkvalidation()
        {
            try
            {
                if (picker_SUBCATEGORY.SelectedIndex == -1)
                {
                    await DisplayAlert(App.AppName, "Select Sub-Category", App.close);
                    return false;
                }
                if (string.IsNullOrEmpty(Entry_issuedate.Text))
                {
                    await DisplayAlert(App.AppName, "Enter Issue Date", App.close);
                    return false;
                }
                if (string.IsNullOrEmpty(Entry_validuptodate.Text))
                {
                    await DisplayAlert(App.AppName, "Enter Valid Date", App.close);
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

        private async void btn_popupsubcategorysubmit_Clicked(object sender, EventArgs e)
        {
            if (await checkvalidation())
            {

                var service = new UserRegistrationApi();
                Loading_activity.IsVisible = true;
                int response_SaveSubCategory = await service.SaveSubCategory(RegNo, UserID, catgeoryid, issuedateselected,
                    validuptodateselected, entry_certificateno.Text, doc1);

                if (response_SaveSubCategory == 200)
                {
                    Loading_activity.IsVisible = false;
                    // Application.Current.MainPage = new NavigationPage(new SubCategoryPage());
                    Application.Current.MainPage = new NewUserRegistrationMasterPage(6);
                }
                Loading_activity.IsVisible = false;
            }
        }
   
        private void btn_submit1_Clicked(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new PhysicallyHandicappedPage());
            Application.Current.MainPage = new NewUserRegistrationMasterPage(7);

        }

        private void btn_next_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new PhysicallyHandicappedPage());
            Application.Current.MainPage = new NewUserRegistrationMasterPage(7);

        }

    }
}