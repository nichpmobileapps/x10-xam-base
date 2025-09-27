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
    public partial class EmploymentDetailsPage : ContentPage
    {
        EmploymentStatusDatabase employmentStatusDatabase = new EmploymentStatusDatabase();
        List<EmploymentStatus> employmentStatuslist;
        SubEmploymentStatusDatabase subEmploymentStatusDatabase = new SubEmploymentStatusDatabase();
        List<SubEmploymentStatus> empsectorlist;
        List<SubEmploymentStatus> emptypelist;    
        string  EmploymentStatusCode, EmploymentSectorCode, EmploymenttypeCode;

        OrganisationMasterDatabase organizationMasterDatabase = new OrganisationMasterDatabase();
        List<OrganisationMaster> organisationMasterlist;
        string organisationid;
        string RegNo;

        EmployedDetailsDatabase employedDetailsDatabase = new EmployedDetailsDatabase();
        List<EmployedDetails> employedDetailslist;




        public EmploymentDetailsPage()
        {
            InitializeComponent();
            lbl_navigation_header.Text = App.AppName;
            loadData();
        }

        async void loadData()
        {
            
                RegNo = Preferences.Get("RegNo", "");
             
            
            var service = new UserRegistrationApi();
            btn_header.Text = "Employment Details" + "\nRegistration No : " + RegNo + "\nName : " + service.getusername(RegNo);

            Loading_activity.IsVisible = true;
           
            int response_getempstatus = await service.GetEmploymentStatus();
            int response_getsubempstatus = await service.GetSubEmploymentStatus();
            //int response_getorganisations = await service.GetOrganisationName();
            Loading_activity.IsVisible = false;

            if (response_getempstatus == 200)
            {
                employmentStatuslist = employmentStatusDatabase.GetEmploymentStatus("Select * from EmploymentStatus order by EmpStatDesc Asc").ToList();
                Picker_EmploymentStatus.Title = "Select Employment Status";
                Picker_EmploymentStatus.ItemsSource = employmentStatuslist;
                Picker_EmploymentStatus.ItemDisplayBinding = new Binding("EmpStatDesc");
                //Picker_EmploymentStatus.SelectedIndex = 0;
            }

            if (!string.IsNullOrEmpty(RegNo))
            {

                Loading_activity.IsVisible = true;
            
                int response_GetContactDetails = await service.GetEmployedDetails(RegNo);
                if (response_GetContactDetails == 200)
                {
                    Loading_activity.IsVisible = false;                  
                  

                    Loadpreviousdata();
                }
                Loading_activity.IsVisible = false;
            }
        }

        private void Loadpreviousdata()
        {
            employedDetailslist = employedDetailsDatabase.GetEmployedDetails($"Select * from EmployedDetails where RegistrationNo= '{RegNo}'").ToList();
            organisationMasterlist = organizationMasterDatabase.GetOrganisationMaster("Select * from OrganisationMaster").ToList();
            string userstatus = employedDetailslist.ElementAt(0).Stat;
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

            string empstatuscd = employedDetailslist.ElementAt(0).EmploymentStatus;

            int EmploymentStatusindex = employmentStatuslist.FindIndex(s => s.EmpStatCd == empstatuscd);
            if (EmploymentStatusindex != -1)
            {
                Picker_EmploymentStatus.SelectedIndex = EmploymentStatusindex;
                EmploymentStatusCode = employmentStatuslist.ElementAt(EmploymentStatusindex).EmpStatCd;
            }

            if (empstatuscd.Equals("4"))
            {
                stack_sector.IsVisible = true;
                stack_type.IsVisible = true;
                stack_sector.IsVisible = true;
                stack_regorganisationname.IsVisible = true;
                stack_organisationr.IsVisible = true;                

                empsectorlist = subEmploymentStatusDatabase.GetSubEmploymentStatus("Select distinct SubEmpStatDesc,SubEmpStatCd  from SubEmploymentStatus order by SubEmpStatDesc Asc").ToList();
                int empsectorindex = empsectorlist.FindIndex(s => s.SubEmpStatCd == employedDetailslist.ElementAt(0).EmploymentSector);
                if (empsectorindex != -1)
                {
                    Picker_EmploymentSector.SelectedIndex = empsectorindex;
                    EmploymentSectorCode = empsectorlist.ElementAt(empsectorindex).SubEmpStatCd;
                }

                emptypelist = subEmploymentStatusDatabase.GetSubEmploymentStatus($"Select distinct SSubEmpStatDesc ,SSubEmpStatCd " +
                    $"from SubEmploymentStatus where SubEmpStatCd='{EmploymentSectorCode}' order by SSubEmpStatDesc ASC").ToList();
                int emptypeindex = emptypelist.FindIndex(s => s.SSubEmpStatCd == employedDetailslist.ElementAt(0).EmploymentType);
                if (emptypeindex != -1)
                {
                    Picker_employmentype.SelectedIndex = emptypeindex;
                    EmploymenttypeCode = emptypelist.ElementAt(emptypeindex).SSubEmpStatCd;
                }

                editor_organisation.Text = employedDetailslist.ElementAt(0).OrganisationName;
                editor_reOrganizationName.Text = employedDetailslist.ElementAt(0).RegisteredOrganisationName;
            }
            else
            {
                stack_sector.IsVisible = false;
                stack_type.IsVisible = false;
                stack_sector.IsVisible = false;
                stack_organisationr.IsVisible = false;
                stack_regorganisationname.IsVisible = false;
                EmploymentSectorCode = string.Empty;
                EmploymenttypeCode = string.Empty;
                organisationid = string.Empty;
                editor_organisation.Text = "";
            }
        }

        private  void Picker_EmploymentStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Picker_EmploymentStatus.SelectedIndex != -1)
            {

                EmploymentStatusCode = employmentStatuslist.ElementAt(Picker_EmploymentStatus.SelectedIndex).EmpStatCd;
               // EmploymentStatusName = employmentStatuslist.ElementAt(Picker_EmploymentStatus.SelectedIndex).EmpStatDesc;

                if (EmploymentStatusCode.Equals("4"))
                {
                    empsectorlist = subEmploymentStatusDatabase.GetSubEmploymentStatus("Select distinct SubEmpStatDesc,SubEmpStatCd  from SubEmploymentStatus order by SubEmpStatDesc Asc").ToList();
                    Picker_EmploymentSector.Title = "Select Employment Sector";
                    Picker_EmploymentSector.ItemsSource = empsectorlist;
                    Picker_EmploymentSector.ItemDisplayBinding = new Binding("SubEmpStatDesc");

                    organisationMasterlist = organizationMasterDatabase.GetOrganisationMaster("Select * from OrganisationMaster").ToList();
                    listview_organisationname.ItemsSource = organisationMasterlist;
                   /* Loading_activity.IsVisible = true;
                    var service = new UserRegistrationApi();
                    int response_getorganisations = await service.GetOrganisationName();
                    Loading_activity.IsVisible = false;*/

                    /* picker_organisationname.ItemsSource = organisationMasterlist;
                     picker_organisationname.ItemDisplayBinding = new Binding("OrgName");
                     picker_organisationname.SelectedIndex = 0;*/

                    stack_sector.IsVisible = true;
                    stack_type.IsVisible = true;
                    stack_sector.IsVisible = true;
                    stack_regorganisationname.IsVisible = true;
                    stack_organisationr.IsVisible = true;
                }
                else
                {
                    stack_sector.IsVisible = false;
                    stack_type.IsVisible = false;
                    stack_sector.IsVisible = false;
                    stack_organisationr.IsVisible = false;
                    stack_regorganisationname.IsVisible = false;
                    EmploymentSectorCode = string.Empty;
                    EmploymenttypeCode = string.Empty;
                    organisationid = string.Empty;
                    editor_organisation.Text = "";
                }
            }
        }

        private void Picker_EmploymentSector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Picker_EmploymentSector.SelectedIndex != -1)
            {

                EmploymentSectorCode = empsectorlist.ElementAt(Picker_EmploymentSector.SelectedIndex).SubEmpStatCd;
                emptypelist = subEmploymentStatusDatabase.GetSubEmploymentStatus($"Select distinct SSubEmpStatDesc ,SSubEmpStatCd " +
                    $"from SubEmploymentStatus where SubEmpStatCd='{EmploymentSectorCode}' order by SSubEmpStatDesc ASC").ToList();
                Picker_employmentype.Title = "Select Employment Type";
                Picker_employmentype.ItemsSource = emptypelist;
                Picker_employmentype.ItemDisplayBinding = new Binding("SSubEmpStatDesc");
            }
        }

        private void Picker_employmentype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Picker_employmentype.SelectedIndex != -1)
            {

                EmploymenttypeCode = emptypelist.ElementAt(Picker_employmentype.SelectedIndex).SSubEmpStatCd;
            }
        }

      

        private void listview_organisationname_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var currentrecord = e.Item as OrganisationMaster;
            editor_reOrganizationName.Text = currentrecord.OrgName;
            organisationid = currentrecord.OrgId;
            popuporganisationname.IsVisible = false;
        }

        private void popuporganisationnameCancel_Clicked(object sender, EventArgs e)
        {
            popuporganisationname.IsVisible = false;
        }

        private async void editor_OrganizationName_Focused(object sender, FocusEventArgs e)
        {
            editor_reOrganizationName.Unfocus();
            Loading_activity.IsVisible = true;
            var service = new UserRegistrationApi();
            int response_getorganisations = await service.GetOrganisationName();
            Loading_activity.IsVisible = false;
            if (response_getorganisations == 200)
            {
                popuporganisationname.IsVisible = true;
            }
            else
            {
                await DisplayAlert(App.AppName, "Error Fetching Organisation Name", App.close);
            }
        }

        private void SearchBar_organisationname_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(SearchBar_organisationname.Text))
            {
                string texttosearch = SearchBar_organisationname.Text.ToLower().Trim();
                listview_organisationname.ItemsSource = organisationMasterlist.Where(t =>
                               t.OrgId.ToLower().Contains(texttosearch) ||
                               t.OrgName.ToLower().Contains(texttosearch)
                               ).ToList();
            }
            else
            {
                organisationMasterlist = organizationMasterDatabase.GetOrganisationMaster("Select * from OrganisationMaster").ToList();
                listview_organisationname.ItemsSource = organisationMasterlist;
            }
        }

        private void btn_cancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

      

        async Task<bool> checkvalidation()
        {
            try
            {

                if (Picker_EmploymentStatus.SelectedIndex == -1)
                {
                    await DisplayAlert(App.AppName, "Select Employment Status", "Close");
                    Picker_EmploymentStatus.Focus();
                    return false;
                }
                if (EmploymentStatusCode.Equals("4"))
                {

                    if (Picker_EmploymentSector.SelectedIndex == -1)
                    {
                        await DisplayAlert(App.AppName, "Select Employment Sector", "Close");
                        Picker_EmploymentSector.Focus();
                        return false;
                    }


                    if (Picker_employmentype.SelectedIndex == -1)
                    {
                        await DisplayAlert(App.AppName, "Select Employment Type", "Close");
                        Picker_employmentype.Focus();
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert(App.AppName, ex.Message, "Close");
                return false;
            }
            return true;
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new PostLoginDashboardPage());
        }

        private async void btn_submit_Clicked(object sender, EventArgs e)
        {
            var service = new UserRegistrationApi();
            Loading_activity.IsVisible = true;
            int response_SaveEmploymentStatus = await service.SaveEmploymentStatus(RegNo, EmploymentStatusCode, EmploymentSectorCode,
                EmploymenttypeCode, organisationid, editor_organisation.Text, editor_reOrganizationName.Text);

            if (response_SaveEmploymentStatus == 200)
            {
                Loading_activity.IsVisible = false;
               // await Navigation.PushAsync(new SubCategoryPage());
                Application.Current.MainPage = new NewUserRegistrationMasterPage(6);

            }
            Loading_activity.IsVisible = false;
        }

        private  void btn_next_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new SubCategoryPage());
            Application.Current.MainPage = new NewUserRegistrationMasterPage(6);

        }
    }
}