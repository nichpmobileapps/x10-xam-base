using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using X10Card.Models.NewUserRegistration;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace X10Card.Models
{
    public class UserRegistrationApi
    {

        DistrictMasterDatabase districtMasterDatabase = new DistrictMasterDatabase();
        ExchangeNameDatabase exchangeNameDatabase = new ExchangeNameDatabase();
        ReligionMasterDatabase religionMasterDatabase = new ReligionMasterDatabase();
        MaritalStatusMasterDatabase maritalStatusMasterDatabase = new MaritalStatusMasterDatabase();
        CategoryMasterDatabase categoryMasterDatabase = new CategoryMasterDatabase();
        TehsilMasterDatabase tehsilMasterDatabase = new TehsilMasterDatabase();
        VillageMasterDatabase villageMasterDatabase = new VillageMasterDatabase();
        QualificationMasterDatabase qualificationMasterDatabase = new QualificationMasterDatabase();
        BoardMasterDatabase boardMasterDatabase = new BoardMasterDatabase();
        SectorofInterestMasterDatabase sectorofInterestMasterDatabase = new SectorofInterestMasterDatabase();
        LanguageMasterDatabase languageMasterDatabase = new LanguageMasterDatabase();
        EmploymentStatusDatabase employmentStatusDatabase = new EmploymentStatusDatabase();
        SubEmploymentStatusDatabase subEmploymentStatusDatabase = new SubEmploymentStatusDatabase();
        OrganisationMasterDatabase organisationMasterDatabase = new OrganisationMasterDatabase();
        SubCategoryDatabase subCategoryDatabase = new SubCategoryDatabase();
        PhysicallyHandicappedDatabase physicallyHandicappedDatabase = new PhysicallyHandicappedDatabase();
        ForceMasterDatabase forceMasterDatabase = new ForceMasterDatabase();
        RankMasterDatabase rankMasterDatabase = new RankMasterDatabase();
        MedicalMasterDatabase medicalMasterDatabase = new MedicalMasterDatabase();
        CharacterMasterDatabase characterMasterDatabase = new CharacterMasterDatabase();
        ReasonMasterDatabase reasonMasterDatabase = new ReasonMasterDatabase();
        NCODatabase ncoDatabase = new NCODatabase();
        ApplicantDashboardDatabase applicantDashboardDatabase = new ApplicantDashboardDatabase();
        ExistingUserDataDatabase existingUserDataDatabase = new ExistingUserDataDatabase();
        AlreadyRegisteredDatabase alreadyRegisteredDatabase = new AlreadyRegisteredDatabase();
        string UserID;

        PersonalDetailsDatabase personalDetailsDatabase = new PersonalDetailsDatabase();
        ContactDetailsDatabase contactDetailsDatabase = new ContactDetailsDatabase();
        QualficationDetailsDatabase qualficationDetailsDatabase = new QualficationDetailsDatabase();
        MiscellaneousDetailsDatabase miscellaneousDetailsDatabase = new MiscellaneousDetailsDatabase();
        GetLangDetailsDatabase getLangDetailsDatabase = new GetLangDetailsDatabase();
        SubCategoryDetailsDatabase subCategoryDetailsDatabase = new SubCategoryDetailsDatabase();
        EmployedDetailsDatabase employedDetailsDatabase = new EmployedDetailsDatabase();
        PHDetailsDatabase pHDetailsDatabase = new PHDetailsDatabase();
        XservicemenDetailsDatabase xservicemenDetailsDatabase = new XservicemenDetailsDatabase();
        GetNCODetailsDatabase getNCODetailsDatabase = new GetNCODetailsDatabase();
        SubmittedFormsDatabase submittedFormsDatabase = new SubmittedFormsDatabase();


        public async Task<int> GetDistrict()
        {
            try
            {
                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"Districtslist?";

                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    districtMasterDatabase = new DistrictMasterDatabase();
                    districtMasterDatabase.DeleteDistrictMaster();

                    foreach (var pair in parsed)
                    {
                        if (pair.Key == "DistrictsList")
                        {
                            var nodes = pair.Value;
                            var item = new DistrictMaster();
                            foreach (var node in nodes)
                            {
                                item.DistrictID = node["DistrictID"].ToString();
                                item.DistrictName = node["DistrictName"].ToString();
                                districtMasterDatabase.AddDistrictMaster(item);
                            }
                        }
                    }
                }
                return (int)response.StatusCode;
            }
            catch
            {
                //await Application.Current.MainPage.DisplayAlert("Exception", ey.Message, App.close);
                return 500;
            }
        }

        public async Task<int> GetExchange(string DisCd)
        {
            try
            {
                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"GetExchangeList?"
                + $"DistCd={HttpUtility.UrlEncode(DisCd)}";
                //+$"&packageid={HttpUtility.UrlEncode(CommonClass.Encrypt(AppInfo.PackageName))}";

                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);

                    exchangeNameDatabase.DeleteExchangeNameMaster();

                    foreach (var pair in parsed)
                    {

                        if (pair.Key == "ExchangeList")
                        {
                            var nodes = pair.Value;
                            var item = new ExchangeNameMaster();
                            foreach (var node in nodes)
                            {
                                item.ExchangeID = node["ExchangeID"].ToString();
                                item.ExchangeName = node["ExchangeName"].ToString();
                                exchangeNameDatabase.AddExchangeNameMaster(item);
                            }
                        }
                    }

                }
                return (int)response.StatusCode;

            }
            catch
            {
                //await Application.Current.MainPage.DisplayAlert("Exception", ey.Message, App.close);
                return 500;
            }
        }

        public async Task<int> GetReligion()
        {
            try
            {
                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"GetReligionListMaster?";


                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    religionMasterDatabase.DeleteReligionMaster();

                    foreach (var pair in parsed)
                    {

                        if (pair.Key == "ReligionList")
                        {
                            var nodes = pair.Value;
                            var item = new ReligionMaster();
                            foreach (var node in nodes)
                            {
                                item.ReligionID = node["ReligionID"].ToString();
                                item.ReligionName = node["ReligionName"].ToString();
                                religionMasterDatabase.AddReligionMaster(item);
                            }
                        }
                    }

                }
                return (int)response.StatusCode;

            }
            catch
            {
                //await Application.Current.MainPage.DisplayAlert("Exception", ey.Message, App.close);
                return 500;
            }
        }

        public async Task<int> GetMaritalStatus()
        {
            try
            {
                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"GetMaritalStatusMaster?";


                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    maritalStatusMasterDatabase.DeleteMaritalStatusMaster();

                    foreach (var pair in parsed)
                    {

                        if (pair.Key == "MaritalStatus")
                        {
                            var nodes = pair.Value;
                            var item = new MaritalStatusMaster();
                            foreach (var node in nodes)
                            {
                                item.MaritalStatusListID = node["MaritalStatusListID"].ToString();
                                item.MaritalStatusListName = node["MaritalStatusListName"].ToString();
                                maritalStatusMasterDatabase.AddMaritalStatusMaster(item);
                            }
                        }
                    }

                }
                return (int)response.StatusCode;

            }
            catch
            {
                //await Application.Current.MainPage.DisplayAlert("Exception", ey.Message, App.close);
                return 500;
            }
        }

        public async Task<int> GetCategory()
        {
            try
            {
                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"GetCategoryMaster?";


                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    categoryMasterDatabase.DeleteCategoryMaster();

                    foreach (var pair in parsed)
                    {

                        if (pair.Key == "Category")
                        {
                            var nodes = pair.Value;
                            var item = new CategoryMaster();
                            foreach (var node in nodes)
                            {
                                item.CategoryID = node["CategoryID"].ToString();
                                item.CategoryName = node["CategoryName"].ToString();
                                categoryMasterDatabase.AddCategoryMaster(item);
                            }
                        }
                    }

                }
                return (int)response.StatusCode;

            }
            catch
            {
                //await Application.Current.MainPage.DisplayAlert("Exception", ey.Message, App.close);
                return 500;
            }
        }

        public async Task<int> GetTehsil(string DisCd)
        {
            try
            {
                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"GetTehsilMaster?"
                + $"DistCd={HttpUtility.UrlEncode(DisCd)}";

                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);

                    tehsilMasterDatabase.DeleteTehsilMaster();

                    foreach (var pair in parsed)
                    {

                        if (pair.Key == "Tehsil")
                        {
                            var nodes = pair.Value;
                            var item = new TehsilMaster();
                            foreach (var node in nodes)
                            {
                                item.TehsilID = node["TehsilID"].ToString();
                                item.TehsilName = node["TehsilName"].ToString();
                                tehsilMasterDatabase.AddTehsilMaster(item);
                            }
                        }
                    }

                }
                return (int)response.StatusCode;

            }
            catch
            {
                //await Application.Current.MainPage.DisplayAlert("Exception", ey.Message, App.close);
                return 500;
            }
        }

        public async Task<int> GetVillageMaster(string DisCd, string TehsilCd)
        {
            try
            {
                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"GetVillageMaster?"
                + $"DistCd={HttpUtility.UrlEncode(DisCd)}"
                + $"&TehsilCd={HttpUtility.UrlEncode(TehsilCd)}";

                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);

                    villageMasterDatabase.DeleteVillageMaster();

                    foreach (var pair in parsed)
                    {

                        if (pair.Key == "Village")
                        {
                            var nodes = pair.Value;
                            var item = new VillageMaster();
                            foreach (var node in nodes)
                            {
                                item.VillageID = node["VillageID"].ToString();
                                item.VillageName = node["VillageName"].ToString();
                                villageMasterDatabase.AddVillageMaster(item);
                            }
                        }
                    }

                }
                return (int)response.StatusCode;

            }
            catch
            {
                //await Application.Current.MainPage.DisplayAlert("Exception", ey.Message, App.close);
                return 500;
            }
        }

        public async Task<int> GetQualificationMaster()
        {
            try
            {
                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"GetQualificationMaster?";


                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    qualificationMasterDatabase.DeleteQualificationMaster();

                    foreach (var pair in parsed)
                    {

                        if (pair.Key == "QualificationList")
                        {
                            var nodes = pair.Value;
                            var item = new QualificationMaster();
                            foreach (var node in nodes)
                            {
                                item.QualificationID = node["QualificationID"].ToString();
                                item.QualificationName = node["QualificationName"].ToString();
                                qualificationMasterDatabase.AddQualificationMaster(item);
                            }
                        }
                    }

                }
                return (int)response.StatusCode;

            }
            catch
            {
                //await Application.Current.MainPage.DisplayAlert("Exception", ey.Message, App.close);
                return 500;
            }
        }

        public async Task<int> GetBoardMaster()
        {
            try
            {
                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"GetBoardMaster?";


                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    boardMasterDatabase.DeleteBoardMaster();

                    foreach (var pair in parsed)
                    {

                        if (pair.Key == "BoardList")
                        {
                            var nodes = pair.Value;
                            var item = new BoardMaster();
                            foreach (var node in nodes)
                            {
                                item.BoardID = node["BoardID"].ToString();
                                item.BoardName = node["BoardName"].ToString();
                                boardMasterDatabase.AddBoardMaster(item);
                            }
                        }
                    }

                }
                return (int)response.StatusCode;

            }
            catch
            {
                //await Application.Current.MainPage.DisplayAlert("Exception", ey.Message, App.close);
                return 500;
            }
        }

        public async Task<int> GetSectorofInterestMaster()
        {
            try
            {
                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"GetSectorofInterestMaster?";


                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    sectorofInterestMasterDatabase.DeleteSectorofInterestMaster();

                    foreach (var pair in parsed)
                    {

                        if (pair.Key == "SectorofInterestList")
                        {
                            var nodes = pair.Value;
                            var item = new SectorofInterestMaster();
                            foreach (var node in nodes)
                            {
                                item.SectorID = node["SectorID"].ToString();
                                item.SectorName = node["SectorName"].ToString();
                                sectorofInterestMasterDatabase.AddSectorofInterestMaster(item);
                            }
                        }
                    }

                }
                return (int)response.StatusCode;

            }
            catch
            {
                //await Application.Current.MainPage.DisplayAlert("Exception", ey.Message, App.close);
                return 500;
            }
        }

        public async Task<int> GetLanguagesKnownMaster()
        {
            try
            {
                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"GetLanguagesKnownMaster?";


                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    languageMasterDatabase.DeleteLanguageMaster();

                    foreach (var pair in parsed)
                    {

                        if (pair.Key == "LanguageKnownList")
                        {
                            var nodes = pair.Value;
                            var item = new LanguageMaster();
                            foreach (var node in nodes)
                            {
                                item.LanguageID = node["LanguageID"].ToString();
                                item.LanguageName = node["LanguageName"].ToString();
                                languageMasterDatabase.AddLanguageMaster(item);
                            }
                        }
                    }

                }
                return (int)response.StatusCode;

            }
            catch
            {
                //await Application.Current.MainPage.DisplayAlert("Exception", ey.Message, App.close);
                return 500;
            }
        }

        public async Task<int> GetEmploymentStatus()
        {
            try
            {
                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"GetEmploymentStatus?";


                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    employmentStatusDatabase.DeleteEmploymentStatus();

                    foreach (var pair in parsed)
                    {

                        if (pair.Key == "EmploymentStatus")
                        {
                            var nodes = pair.Value;
                            var item = new EmploymentStatus();
                            foreach (var node in nodes)
                            {
                                item.EmpStatCd = node["EmpStatCd"].ToString();
                                item.EmpStatDesc = node["EmpStatDesc"].ToString();
                                employmentStatusDatabase.AddEmploymentStatus(item);
                            }
                        }
                    }

                }
                return (int)response.StatusCode;

            }
            catch
            {
                //await Application.Current.MainPage.DisplayAlert("Exception", ey.Message, App.close);
                return 500;
            }
        }

        public async Task<int> GetSubEmploymentStatus()
        {
            try
            {
                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"GetSubEmploymentStatus?";


                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    subEmploymentStatusDatabase.DeleteSubEmploymentStatus();

                    foreach (var pair in parsed)
                    {

                        if (pair.Key == "SubEmploymentStatus")
                        {
                            var nodes = pair.Value;
                            var item = new SubEmploymentStatus();
                            foreach (var node in nodes)
                            {
                                item.SubEmpStatCd = node["SubEmpStatCd"].ToString();
                                item.SubEmpStatDesc = node["SubEmpStatDesc"].ToString();
                                item.SSubEmpStatCd = node["SSubEmpStatCd"].ToString();
                                item.SSubEmpStatDesc = node["SSubEmpStatDesc"].ToString();
                                subEmploymentStatusDatabase.AddSubEmploymentStatus(item);
                            }
                        }
                    }

                }
                return (int)response.StatusCode;

            }
            catch
            {
                //await Application.Current.MainPage.DisplayAlert("Exception", ey.Message, App.close);
                return 500;
            }
        }

        public async Task<int> GetOrganisationName()
        {
            try
            {
                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"GetOrganisationName?";

                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    foreach (var pair in parsed)
                    {

                        if (pair.Key == "OrganisationNamelist")
                        {
                            organisationMasterDatabase.DeleteOrganisationMaster();
                            /*int i = 0;
                            string insertintorg = null;*/

                            var nodes = pair.Value;
                            var item = new OrganisationMaster();
                            foreach (var node in nodes)
                            {
                                item.OrgId = node["OrgId"].ToString();
                                item.OrgName = node["OrgName"].ToString();
                                organisationMasterDatabase.AddOrganisationMaster(item);
                                /* if (string.IsNullOrEmpty(insertintorg))
                                 {
                                     insertintorg = $"('{item.OrgId}','{item.OrgName}')";
                                 }
                                 else
                                 {
                                     insertintorg += $" ,('{item.OrgId}','{item.OrgName}')";
                                 }

                                 if (i % 500 == 0)
                                 {
                                     organisationMasterDatabase.InsertOrgNameList(insertintorg);
                                     insertintorg = "";
                                 }*/
                            }
                            // organisationMasterDatabase.InsertOrgNameList(insertintorg);
                        }
                    }

                }
                return (int)response.StatusCode;

            }
            catch 
            {
                //await Application.Current.MainPage.DisplayAlert("Exception", ey.Message, App.close);
                return 500;
            }
        }

        public async Task<int> GetSubCategory()
        {
            try
            {
                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"GetSubCategory?";


                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    subCategoryDatabase.DeleteSubCategory();

                    foreach (var pair in parsed)
                    {

                        if (pair.Key == "SubCategorylist")
                        {
                            var nodes = pair.Value;
                            var item = new SubCategory();
                            foreach (var node in nodes)
                            {
                                item.SubCategoryId = node["SubCategoryId"].ToString();
                                item.SubCategoryName = node["SubCategoryName"].ToString();
                                subCategoryDatabase.AddSubCategory(item);
                            }
                        }
                    }

                }
                return (int)response.StatusCode;

            }
            catch
            {
                //await Application.Current.MainPage.DisplayAlert("Exception", ey.Message, App.close);
                return 500;
            }
        }

        public async Task<int> GetPH()
        {
            try
            {
                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"GetPH?";


                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    physicallyHandicappedDatabase.DeletePhysicallyHandicapped();

                    foreach (var pair in parsed)
                    {
                        if (pair.Key == "PHlist")
                        {
                            var nodes = pair.Value;
                            var item = new PhysicallyHandicapped();
                            foreach (var node in nodes)
                            {
                                item.PHId = node["PHId"].ToString();
                                item.PHName = node["PHName"].ToString();
                                physicallyHandicappedDatabase.AddPhysicallyHandicapped(item);
                            }
                        }
                    }
                }
                return (int)response.StatusCode;
            }
            catch
            {
                //await Application.Current.MainPage.DisplayAlert("Exception", ey.Message, App.close);
                return 500;
            }
        }

        public async Task<int> GetNCO()
        {
            try
            {
                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"GetNCO?";


                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    ncoDatabase.DeleteNCO();

                    foreach (var pair in parsed)
                    {
                        if (pair.Key == "NCOlist")
                        {
                            var nodes = pair.Value;
                            var item = new NCO();
                            foreach (var node in nodes)
                            {
                                item.NCOId = node["NCOId"].ToString();
                                item.NCOName = node["NCOName"].ToString();
                                ncoDatabase.AddNCO(item);
                            }
                        }
                    }
                }
                return (int)response.StatusCode;
            }
            catch
            {
                //await Application.Current.MainPage.DisplayAlert("Exception", ey.Message, App.close);
                return 500;
            }
        }

        public async Task<int> GetForce()
        {
            try
            {
                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"GetForce?";


                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    forceMasterDatabase.DeleteForceMaster();

                    foreach (var pair in parsed)
                    {
                        if (pair.Key == "Forcelist")
                        {
                            var nodes = pair.Value;
                            var item = new ForceMaster();
                            foreach (var node in nodes)
                            {
                                item.ForceId = node["ForceId"].ToString();
                                item.ForceName = node["ForceName"].ToString();
                                forceMasterDatabase.AddForceMaster(item);
                            }
                        }
                    }
                }
                return (int)response.StatusCode;
            }
            catch
            {
                //await Application.Current.MainPage.DisplayAlert("Exception", ey.Message, App.close);
                return 500;
            }
        }

        public async Task<int> GetRank(string ForceCd)
        {
            try
            {
                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"GetRank?"
                + $"ForceCd={HttpUtility.UrlEncode(ForceCd)}";

                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    rankMasterDatabase.DeleteRankMaster();

                    foreach (var pair in parsed)
                    {
                        if (pair.Key == "Ranklist")
                        {
                            var nodes = pair.Value;
                            var item = new RankMaster();
                            foreach (var node in nodes)
                            {
                                item.RankId = node["RankId"].ToString();
                                item.RankName = node["RankName"].ToString();
                                rankMasterDatabase.AddRankMaster(item);
                            }
                        }
                    }
                }
                return (int)response.StatusCode;
            }
            catch
            {
                //await Application.Current.MainPage.DisplayAlert("Exception", ey.Message, App.close);
                return 500;
            }
        }

        public async Task<int> GetMedical()
        {
            try
            {
                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"GetMedical?";


                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    medicalMasterDatabase.DeleteMedicalMaster();

                    foreach (var pair in parsed)
                    {
                        if (pair.Key == "Medicallist")
                        {
                            var nodes = pair.Value;
                            var item = new MedicalMaster();
                            foreach (var node in nodes)
                            {
                                item.MedicalId = node["MedicalId"].ToString();
                                item.MedicalName = node["MedicalName"].ToString();
                                medicalMasterDatabase.AddMedicalMaster(item);
                            }
                        }
                    }
                }
                return (int)response.StatusCode;
            }
            catch
            {
                //await Application.Current.MainPage.DisplayAlert("Exception", ey.Message, App.close);
                return 500;
            }
        }

        public async Task<int> GetCharacter()
        {
            try
            {
                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"GetCharacter?";


                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    characterMasterDatabase.DeleteCharacterMaster();

                    foreach (var pair in parsed)
                    {
                        if (pair.Key == "Characterlist")
                        {
                            var nodes = pair.Value;
                            var item = new CharacterMaster();
                            foreach (var node in nodes)
                            {
                                item.CharacterId = node["CharacterId"].ToString();
                                item.CharacterName = node["CharacterName"].ToString();
                                characterMasterDatabase.AddCharacterMaster(item);
                            }
                        }
                    }
                }
                return (int)response.StatusCode;
            }
            catch
            {
                //await Application.Current.MainPage.DisplayAlert("Exception", ey.Message, App.close);
                return 500;
            }
        }

        public async Task<int> GetReason()
        {
            try
            {
                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"GetReason?";


                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    reasonMasterDatabase.DeleteReasonMaster();

                    foreach (var pair in parsed)
                    {
                        if (pair.Key == "Reasonlist")
                        {
                            var nodes = pair.Value;
                            var item = new ReasonMaster();
                            foreach (var node in nodes)
                            {
                                item.ReasonId = node["ReasonId"].ToString();
                                item.ReasonName = node["ReasonName"].ToString();
                                reasonMasterDatabase.AddReasonMaster(item);
                            }
                        }
                    }
                }
                return (int)response.StatusCode;
            }
            catch
            {
                //await Application.Current.MainPage.DisplayAlert("Exception", ey.Message, App.close);
                return 500;
            }
        }

        public async Task<int> SavePersonalDetails(string RegNo, string XchangeCode, string UserName,
            string FAtherHusbandSelection, string TXTFHNAME, string txtmother, string ddlmarital,
            string GENDER, string DOB, string ddlreligion, string Category, string issuedate, string certificateno, string _file, string UserID)
        {

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                int status = 0;
                try
                {
                    string ipaddress = await App.GetIPAddress();
                    if (string.IsNullOrEmpty(ipaddress))
                    {
                        ipaddress = "192.168.1.1";
                    }

                    var jsonData = new
                    {
                        RegNo = RegNo != null ? CommonClass.Encrypt(RegNo) : "",
                        XchangeCode = CommonClass.Encrypt(XchangeCode),
                        UserName = CommonClass.Encrypt(UserName),
                        FAtherHusbandSelection = CommonClass.Encrypt(FAtherHusbandSelection),
                        TXTFHNAME = CommonClass.Encrypt(TXTFHNAME),
                        txtmother = txtmother != null ? CommonClass.Encrypt(txtmother) : "",
                        ddlmarital = CommonClass.Encrypt(ddlmarital),
                        GENDER = CommonClass.Encrypt(GENDER),
                        DOB = CommonClass.Encrypt(DOB),
                        ddlreligion = CommonClass.Encrypt(ddlreligion),
                        Category = CommonClass.Encrypt(Category),
                        GetIPAddress = CommonClass.Encrypt(ipaddress),
                        issuedate = issuedate != null ? CommonClass.Encrypt(issuedate) : "",
                        certificateno = certificateno != null ? CommonClass.Encrypt(certificateno) : "",

                        document = _file != null ? _file : "",
                        UserID = UserID != null ? CommonClass.Encrypt(UserID) : "",
                    };

                    string json = JsonConvert.SerializeObject(jsonData);

                    var client = new HttpClient();

                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(App.baseurluserregapi + $"PersonsalInsUpd", content);

                    JObject parsed = new JObject();
                    if ((int)response.StatusCode == 200)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        parsed = JObject.Parse(result);
                        if (parsed.HasValues)
                        {
                            string msg = string.Empty;
                            var message = parsed["message"];
                            if (message.Any())
                            {

                                status = int.Parse(message["status"].ToString());
                                msg = message["message"].ToString();
                                if (status == 200)
                                {
                                    string regno = message["RegNo"].ToString();
                                    
                                        Preferences.Set("RegNo", regno);
                                    
                                    await App.Current.MainPage.DisplayAlert(App.AppName, msg, App.close);
                                }
                                else
                                {
                                    await App.Current.MainPage.DisplayAlert(App.AppName, msg, App.close);
                                }
                            }
                            else
                            {
                                await App.Current.MainPage.DisplayAlert(App.AppName, message.ToString(), App.close);
                            }
                        }

                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(App.AppName, parsed["Message"].ToString(), App.close);
                    }

                    return status;

                }
                catch (Exception ex)
                {
#if DEBUG
                    await App.Current.MainPage.DisplayAlert("Exception", ex.Message, App.close);
                    return 500;
#else
                    await App.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try after some time.", App.close);
                    return 500;
#endif
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(App.AppName, App.nointernet, App.close);
                return 101;
            }
        }

        public async Task<int> SaveContactDetails(string regno, string mobileno, string email,
         string district, string areatype, string tehsil, string village,
         string PO, string street, string pincode, string alterphone, string permntaddress, string corresaddress,
         string issuedate, string certificateno, string _file)
        {

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {

                int status = 0;
                try
                {
                    var jsonData = new
                    {
                        Regno = CommonClass.Encrypt(regno),
                        txtmobile = CommonClass.Encrypt(mobileno),
                        txtemail = CommonClass.Encrypt(email),
                        DistrictCode = CommonClass.Encrypt(district),
                        Areatype = CommonClass.Encrypt(areatype),
                        ddltehsilcode = CommonClass.Encrypt(tehsil),
                        ddlvillage = CommonClass.Encrypt(village),
                        txtpo = PO != null ? CommonClass.Encrypt(PO) : "",
                        txtstreet = street != null ? CommonClass.Encrypt(street) : "",
                        txtpincode = CommonClass.Encrypt(pincode),
                        txtphone = alterphone != null ? CommonClass.Encrypt(alterphone) : "",
                        addressmain = CommonClass.Encrypt(permntaddress),
                        txtaddressco = CommonClass.Encrypt(corresaddress),
                        IssueDt = issuedate != null ? CommonClass.Encrypt(issuedate) : "",
                        DocCertificateNo = certificateno != null ? CommonClass.Encrypt(certificateno) : "",
                        document = _file != null ? _file : "",
                    };

                    string json = JsonConvert.SerializeObject(jsonData);

                    var client = new HttpClient();

                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(App.baseurluserregapi + $"ContactInfoInsUpd", content);

                    JObject parsed = new JObject();
                    if ((int)response.StatusCode == 200)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        parsed = JObject.Parse(result);
                        if (parsed.HasValues)
                        {
                            string msg = string.Empty;
                            var message = parsed["message"];
                            status = int.Parse(message["status"].ToString());

                            msg = message["message"].ToString();
                            if (status == 200)
                            {
                                await App.Current.MainPage.DisplayAlert(App.AppName, msg, App.close);
                            }
                            else
                            {
                                await App.Current.MainPage.DisplayAlert(App.AppName, msg, App.close);
                            }
                        }

                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(App.AppName, parsed["Message"].ToString(), App.close);
                    }

                    return status;

                }
                catch (Exception ex)
                {
#if DEBUG
                    await App.Current.MainPage.DisplayAlert("Exception", ex.Message, App.close);
                    return 500;
#else
                    await App.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try after some time.", App.close);
                    return 500;
#endif
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(App.AppName, App.nointernet, App.close);
                return 101;
            }
        }

        public async Task<int> SavEducationDetails(string regno, string QualCd, string board,
         string tmarks, string omarks, string perMarks, string year, string regisdt, string issuedt, string certificateno, string _file)
        {

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                if (string.IsNullOrEmpty(issuedt))
                {
                    issuedt = "";
                    certificateno = "";
                    _file = "";

                }

                int status = 0;
                try
                {
                    var jsonData = new
                    {
                        RegistrationNo = CommonClass.Encrypt(regno),
                        QualCd = CommonClass.Encrypt(QualCd),
                        ddlboard = CommonClass.Encrypt(board),
                        year = CommonClass.Encrypt(year),
                        omarks = CommonClass.Encrypt(omarks),
                        tmarks = CommonClass.Encrypt(tmarks),
                        perMarks = CommonClass.Encrypt(perMarks),
                        issuedt = issuedt != null ? CommonClass.Encrypt(issuedt) : "",
                        validuptodt = CommonClass.Encrypt(""),
                        DocCertNo = certificateno != null ? CommonClass.Encrypt(certificateno) : "",
                        RegDt = CommonClass.Encrypt(regisdt),
                        document = _file != null ? _file : "",

                    };

                    string json = JsonConvert.SerializeObject(jsonData);

                    var client = new HttpClient();

                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(App.baseurluserregapi + $"EducationInsUpd", content);

                    JObject parsed = new JObject();
                    if ((int)response.StatusCode == 200)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        parsed = JObject.Parse(result);
                        if (parsed.HasValues)
                        {
                            string msg = string.Empty;
                            var message = parsed["message"];
                            status = int.Parse(message["status"].ToString());

                            msg = message["message"].ToString();
                            if (status == 200)
                            {
                                //get qualificationdetails
                                await App.Current.MainPage.DisplayAlert(App.AppName, msg, App.close);
                            }
                            else
                            {
                                await App.Current.MainPage.DisplayAlert(App.AppName, msg, App.close);
                            }
                        }

                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(App.AppName, parsed["Message"].ToString(), App.close);
                    }

                    return status;

                }
                catch (Exception ex)
                {
#if DEBUG
                    await App.Current.MainPage.DisplayAlert("Exception", ex.Message, App.close);
                    return 500;
#else
                    await App.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try after some time.", App.close);
                    return 500;
#endif
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(App.AppName, App.nointernet, App.close);
                return 101;
            }
        }

        public async Task<int> SaveMiscaleneousDetails(string regno, string EYESIGHT, string HEIGHT, string WEIGHT,
          string CHESTNORMAL, string CHESTEXPANDED, string SALARYINHOMEDISTRICT, string SALARYINHP, string SALARYOUTSIDEHP, string SectorOfInterestId)
        {

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                int status = 0;
                try
                {
                    var jsonData = new
                    {
                        RegistrationNo = CommonClass.Encrypt(regno),
                        Eyesight = EYESIGHT != null ? CommonClass.Encrypt(EYESIGHT) : CommonClass.Encrypt("0.00"),
                        Height = HEIGHT != null ? CommonClass.Encrypt(HEIGHT) : CommonClass.Encrypt("0.00"),
                        Weight = WEIGHT != null ? CommonClass.Encrypt(WEIGHT) : CommonClass.Encrypt("0.00"),
                        ChestNormal = CHESTNORMAL != null ? CommonClass.Encrypt(CHESTNORMAL) : CommonClass.Encrypt("0.00"),
                        ChastExpended = CHESTEXPANDED != null ? CommonClass.Encrypt(CHESTEXPANDED) : CommonClass.Encrypt("0.00"),
                        SalaryHomeDist = SALARYINHOMEDISTRICT != null ? CommonClass.Encrypt(SALARYINHOMEDISTRICT) : CommonClass.Encrypt("0.00"),
                        SalaryInHP = SALARYINHP != null ? CommonClass.Encrypt(SALARYINHP) : CommonClass.Encrypt("0.00"),
                        SalaryOutHP = SALARYOUTSIDEHP != null ? CommonClass.Encrypt(SALARYOUTSIDEHP) : CommonClass.Encrypt("0.00"),
                        Sector = SectorOfInterestId != null ? CommonClass.Encrypt(SectorOfInterestId) : CommonClass.Encrypt("0"),
                    };

                    string json = JsonConvert.SerializeObject(jsonData);

                    var client = new HttpClient();

                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(App.baseurluserregapi + $"MiscellaneousInsUpd", content);

                    JObject parsed = new JObject();
                    if ((int)response.StatusCode == 200)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        parsed = JObject.Parse(result);
                        if (parsed.HasValues)
                        {
                            string msg = string.Empty;
                            var message = parsed["message1"];
                            status = int.Parse(message["status"].ToString());

                            msg = message["message"].ToString();
                            if (status == 200)
                            {
                                //get qualificationdetails
                                await App.Current.MainPage.DisplayAlert(App.AppName, msg, App.close);
                            }
                            else
                            {
                                await App.Current.MainPage.DisplayAlert(App.AppName, msg, App.close);
                            }
                        }

                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(App.AppName, parsed["Message"].ToString(), App.close);
                    }

                    return status;

                }
                catch (Exception ex)
                {
#if DEBUG
                    await App.Current.MainPage.DisplayAlert("Exception", ex.Message, App.close);
                    return 500;
#else
                    await App.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try after some time.", App.close);
                    return 500;
#endif
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(App.AppName, App.nointernet, App.close);
                return 101;
            }
        }

        public async Task<int> SaveMiscaleneousLanguages(string regno, string Langcd, string Read, string Write, string Speak)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                int status = 0;
                try
                {
                    var jsonData = new
                    {
                        RegistrationNo = CommonClass.Encrypt(regno),
                        Langcd = CommonClass.Encrypt(Langcd),
                        Read = CommonClass.Encrypt(Read),
                        Write = CommonClass.Encrypt(Write),
                        Speak = CommonClass.Encrypt(Speak),
                    };

                    string json = JsonConvert.SerializeObject(jsonData);

                    var client = new HttpClient();
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(App.baseurluserregapi + $"AddLanguage", content);

                    JObject parsed = new JObject();
                    if ((int)response.StatusCode == 200)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        parsed = JObject.Parse(result);
                        if (parsed.HasValues)
                        {
                            string msg = string.Empty;
                            var message = parsed["message1"];
                            status = int.Parse(message["status"].ToString());
                            msg = message["message"].ToString();
                            await App.Current.MainPage.DisplayAlert(App.AppName, msg, App.close);
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(App.AppName, parsed["Message"].ToString(), App.close);
                    }

                    return status;

                }
                catch (Exception ex)
                {
#if DEBUG
                    await App.Current.MainPage.DisplayAlert("Exception", ex.Message, App.close);
                    return 500;
#else
                    await App.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try after some time.", App.close);
                    return 500;
#endif
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(App.AppName, App.nointernet, App.close);
                return 101;
            }
        }

        public async Task<int> SaveEmploymentStatus(string regno, string EmploymentStatusCode, string EmploymentSectorCode, string EmploymenttypeCode, string organisationid,
           string OrganizationName, string RegisteredOrganisationName)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                int status = 0;
                try
                {
                    var jsonData = new
                    {
                        RegNo = CommonClass.Encrypt(regno),
                        EmploymentStatus = EmploymentStatusCode != null ? CommonClass.Encrypt(EmploymentStatusCode) : "",
                        EmploymentSector = EmploymentSectorCode != null ? CommonClass.Encrypt(EmploymentSectorCode) : "",
                        EmploymentType = EmploymenttypeCode != null ? CommonClass.Encrypt(EmploymenttypeCode) : "",
                        RegisteredOrganisationID = organisationid != null ? CommonClass.Encrypt(organisationid) : "",
                        OrganisationName = OrganizationName != null ? CommonClass.Encrypt(OrganizationName) : "",
                        RegisteredOrganisationName = RegisteredOrganisationName != null ? CommonClass.Encrypt(RegisteredOrganisationName) : "",
                    };


                    string json = JsonConvert.SerializeObject(jsonData);

                    var client = new HttpClient();
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(App.baseurluserregapi + $"EmploymentStausInsUpd", content);

                    JObject parsed = new JObject();
                    if ((int)response.StatusCode == 200)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        parsed = JObject.Parse(result);
                        if (parsed.HasValues)
                        {
                            string msg = string.Empty;
                            var message = parsed["message"];
                            status = int.Parse(message["status"].ToString());
                            msg = message["message"].ToString();
                            await App.Current.MainPage.DisplayAlert(App.AppName, msg, App.close);
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(App.AppName, parsed["Message"].ToString(), App.close);
                    }

                    return status;

                }
                catch (Exception ex)
                {
#if DEBUG
                    await App.Current.MainPage.DisplayAlert("Exception", ex.Message, App.close);
                    return 500;
#else
                    await App.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try after some time.", App.close);
                    return 500;
#endif
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(App.AppName, App.nointernet, App.close);
                return 101;
            }
        }

        public async Task<int> SaveSubCategory(string regno, string UserID, string SubCategory, string issuedt, string validuptodt, string DocCertNo, string _file)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                int status = 0;
                string ipaddress = await App.GetIPAddress();
                if (string.IsNullOrEmpty(ipaddress))
                {
                    ipaddress = "192.168.1.1";
                }

                try
                {
                    var jsonData = new
                    {
                        RegNo = CommonClass.Encrypt(regno),
                        UserID = UserID != null ? CommonClass.Encrypt(UserID) : "",
                        SubCategory = SubCategory != null ? CommonClass.Encrypt(SubCategory) : "",
                        issuedt = issuedt != null ? CommonClass.Encrypt(issuedt) : "",
                        validuptodt = validuptodt != null ? CommonClass.Encrypt(validuptodt) : "",
                        DocCertNo = DocCertNo != null ? CommonClass.Encrypt(DocCertNo) : "",
                        IPAddress = ipaddress != null ? CommonClass.Encrypt(ipaddress) : "",
                        document = _file != null ? _file : "",
                    };

                    string json = JsonConvert.SerializeObject(jsonData);

                    var client = new HttpClient();
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(App.baseurluserregapi + $"SubCategoryInsUpd", content);

                    JObject parsed = new JObject();
                    if ((int)response.StatusCode == 200)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        parsed = JObject.Parse(result);
                        if (parsed.HasValues)
                        {
                            string msg = string.Empty;
                            var message = parsed["message"];
                            status = int.Parse(message["status"].ToString());
                            msg = message["message"].ToString();
                            await App.Current.MainPage.DisplayAlert(App.AppName, msg, App.close);
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(App.AppName, parsed["Message"].ToString(), App.close);
                    }

                    return status;

                }
                catch (Exception ex)
                {
#if DEBUG
                    await App.Current.MainPage.DisplayAlert("Exception", ex.Message, App.close);
                    return 500;
#else
                    await App.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try after some time.", App.close);
                    return 500;
#endif
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(App.AppName, App.nointernet, App.close);
                return 101;
            }
        }

        public async Task<int> SavePhysicallyHandicappedDetails(string regno, string PHType, string Percentage, string Remarks, string PhRegDate,
            string issuedt, string validuptodt, string DocCertNo, string _file)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                int status = 0;
                string ipaddress = await App.GetIPAddress();
                if (string.IsNullOrEmpty(ipaddress))
                {
                    ipaddress = "192.168.1.1";
                }

                try
                {
                    var jsonData = new
                    {
                        RegNo = CommonClass.Encrypt(regno),
                        UserID = UserID != null ? CommonClass.Encrypt(UserID) : "",
                        PHType = PHType != null ? CommonClass.Encrypt(PHType) : "",
                        Percentage = Percentage != null ? CommonClass.Encrypt(Percentage) : "",
                        Remarks = Remarks != null ? CommonClass.Encrypt(Remarks) : "",
                        PhRegDate = PhRegDate != null ? CommonClass.Encrypt(PhRegDate) : "",
                        issuedt = issuedt != null ? CommonClass.Encrypt(issuedt) : "",
                        validuptodt = validuptodt != null ? CommonClass.Encrypt(validuptodt) : "",
                        DocCertNo = DocCertNo != null ? CommonClass.Encrypt(DocCertNo) : "",
                        document = _file != null ? _file : "",
                    };

                    string json = JsonConvert.SerializeObject(jsonData);

                    var client = new HttpClient();
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(App.baseurluserregapi + $"PHInsUpd", content);

                    JObject parsed = new JObject();
                    if ((int)response.StatusCode == 200)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        parsed = JObject.Parse(result);
                        if (parsed.HasValues)
                        {
                            string msg = string.Empty;
                            var message = parsed["message"];
                            status = int.Parse(message["status"].ToString());
                            msg = message["message"].ToString();
                            await App.Current.MainPage.DisplayAlert(App.AppName, msg, App.close);
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(App.AppName, parsed["Message"].ToString(), App.close);
                    }

                    return status;

                }
                catch (Exception ex)
                {
#if DEBUG
                    await App.Current.MainPage.DisplayAlert("Exception", ex.Message, App.close);
                    return 500;
#else
                    await App.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try after some time.", App.close);
                    return 500;
#endif
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(App.AppName, App.nointernet, App.close);
                return 101;
            }
        }

        public async Task<int> SaveExServicemenDetails(string regno, string ForceCd, string RankCd, string EnrollDate, string DischargeDate,
           string RegimentName, string ServiceNo, string MedicalCatCd, string CharacterCd, string RemarkS, string ReasonCd, string issuedt, string validuptodt, string certno, string _file)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                int status = 0;
                string ipaddress = await App.GetIPAddress();
                if (string.IsNullOrEmpty(ipaddress))
                {
                    ipaddress = "192.168.1.1";
                }

                try
                {
                    var jsonData = new
                    {
                        RegNo = CommonClass.Encrypt(regno),
                        ForceCd = ForceCd != null ? CommonClass.Encrypt(ForceCd) : "",
                        RankCd = RankCd != null ? CommonClass.Encrypt(RankCd) : "",
                        EnrollDate = EnrollDate != null ? CommonClass.Encrypt(EnrollDate) : "",
                        DischargeDate = DischargeDate != null ? CommonClass.Encrypt(DischargeDate) : "",
                        RegimentName = RegimentName != null ? CommonClass.Encrypt(RegimentName) : "",
                        ServiceNo = ServiceNo != null ? CommonClass.Encrypt(ServiceNo) : "",
                        MedicalCatCd = MedicalCatCd != null ? CommonClass.Encrypt(MedicalCatCd) : "",
                        CharacterCd = CharacterCd != null ? CommonClass.Encrypt(CharacterCd) : "",
                        RemarkS = RemarkS != null ? CommonClass.Encrypt(RemarkS) : "",
                        ReasonCd = ReasonCd != null ? CommonClass.Encrypt(ReasonCd) : "",
                        issuedt = issuedt != null ? CommonClass.Encrypt(issuedt) : "",
                        validuptodt = validuptodt != null ? CommonClass.Encrypt(validuptodt) : "",
                        certno = certno != null ? CommonClass.Encrypt(certno) : "",
                        document = _file != null ? _file : "",
                    };

                    string json = JsonConvert.SerializeObject(jsonData);

                    var client = new HttpClient();
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(App.baseurluserregapi + $"EXServicemenInsUpd", content);

                    JObject parsed = new JObject();
                    if ((int)response.StatusCode == 200)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        parsed = JObject.Parse(result);
                        if (parsed.HasValues)
                        {
                            string msg = string.Empty;
                            var message = parsed["message"];
                            status = int.Parse(message["status"].ToString());
                            msg = message["message"].ToString();
                            await App.Current.MainPage.DisplayAlert(App.AppName, msg, App.close);
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(App.AppName, parsed["Message"].ToString(), App.close);
                    }

                    return status;

                }
                catch (Exception ex)
                {
#if DEBUG
                    await App.Current.MainPage.DisplayAlert("Exception", ex.Message, App.close);
                    return 500;
#else
                    await App.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try after some time.", App.close);
                    return 500;
#endif
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(App.AppName, App.nointernet, App.close);
                return 101;
            }
        }

        public async Task<int> SaveNCO(string regno, string NCOCd, string Period, string RegDt)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                int status = 0;
                string ipaddress = await App.GetIPAddress();
                if (string.IsNullOrEmpty(ipaddress))
                {
                    ipaddress = "192.168.1.1";
                }
                string Remark = string.Empty;
                string SrNo = string.Empty;

                try
                {
                    var jsonData = new
                    {
                        RegNo = CommonClass.Encrypt(regno),
                        NCOCd = NCOCd != null ? CommonClass.Encrypt(NCOCd) : "",
                        Period = Period != null ? CommonClass.Encrypt(Period) : "",
                        RegDt = RegDt != null ? CommonClass.Encrypt(RegDt) : "",
                        Remark = Remark != null ? CommonClass.Encrypt(Remark) : "",
                        SrNo = SrNo != null ? CommonClass.Encrypt(SrNo) : "",
                        UserCd = UserID != null ? CommonClass.Encrypt(UserID) : "",

                    };

                    string json = JsonConvert.SerializeObject(jsonData);

                    var client = new HttpClient();
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(App.baseurluserregapi + $"NCOInsUpd", content);

                    JObject parsed = new JObject();
                    if ((int)response.StatusCode == 200)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        parsed = JObject.Parse(result);
                        if (parsed.HasValues)
                        {
                            string msg = string.Empty;
                            var message = parsed["message"];
                            status = int.Parse(message["status"].ToString());
                            msg = message["message"].ToString();
                            await App.Current.MainPage.DisplayAlert(App.AppName, msg, App.close);
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(App.AppName, parsed["Message"].ToString(), App.close);
                    }

                    return status;

                }
                catch (Exception ex)
                {
#if DEBUG
                    await App.Current.MainPage.DisplayAlert("Exception", ex.Message, App.close);
                    return 500;
#else
                    await App.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try after some time.", App.close);
                    return 500;
#endif
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(App.AppName, App.nointernet, App.close);
                return 101;
            }
        }

        public async Task<int> Signup(string MobileNo, string Email, string Password, string ConfirmPassword)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                int status = 0;
                string ipaddress = await App.GetIPAddress();
                if (string.IsNullOrEmpty(ipaddress))
                {
                    ipaddress = "192.168.1.1";
                }
                try
                {
                    var jsonData = new
                    {
                        MobileNo = MobileNo != null ? CommonClass.Encrypt(MobileNo) : "",
                        Email = Email != null ? CommonClass.Encrypt(Email) : "",
                        Ipaddress = ipaddress != null ? CommonClass.Encrypt(ipaddress) : "",
                        Password = CommonClass.GetSha256FromString(Password),
                        ConfirmPassword = CommonClass.GetSha256FromString(ConfirmPassword),
                    };

                    string json = JsonConvert.SerializeObject(jsonData);

                    var client = new HttpClient();
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(App.baseurluserregapi + $"Signup", content);

                    JObject parsed = new JObject();
                    if ((int)response.StatusCode == 200)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        parsed = JObject.Parse(result);
                        if (parsed.HasValues)
                        {
                            string msg = string.Empty;
                            var message = parsed["message"];
                            status = int.Parse(message["status"].ToString());
                            msg = message["message"].ToString();
                            await App.Current.MainPage.DisplayAlert(App.AppName, msg, App.close);
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(App.AppName, parsed["Message"].ToString(), App.close);
                    }

                    return status;

                }
                catch (Exception ex)
                {
#if DEBUG
                    await App.Current.MainPage.DisplayAlert("Exception", ex.Message, App.close);
                    return 500;
#else
                    await App.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try after some time.", App.close);
                    return 500;
#endif
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(App.AppName, App.nointernet, App.close);
                return 101;
            }
        }

        public async Task<int> Login(string userid, string password)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                int status = 0;

                try
                {
                    var jsonData = new
                    {
                        Userid = CommonClass.Encrypt(userid),
                        Password = CommonClass.GetSha256FromString(password),
                    };

                    string json = JsonConvert.SerializeObject(jsonData);

                    var client = new HttpClient();
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(App.baseurluserregapi + $"Login", content);

                    JObject parsed = new JObject();
                    if ((int)response.StatusCode == 200)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        parsed = JObject.Parse(result);
                        if (parsed.HasValues)
                        {
                            string msg = string.Empty;

                            var message = parsed["message"];
                            status = int.Parse(message["status"].ToString());
                            msg = message["message"].ToString();

                            string regno = message["RegNo"].ToString();
                            string UserID = message["UserID"].ToString();
                           
                                Preferences.Set("UserID", UserID);
                                //UserID Preferences.Set("RegNoLogin", regno);
                                Preferences.Set("RegNo", regno);
                            
                            if (status ==500 || status==201)
                            {
                                await App.Current.MainPage.DisplayAlert(App.AppName, msg, App.close);
                            }
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(App.AppName, "Error while retrieving data.", App.close);
                    }

                    return status;

                }
                catch (Exception ex)
                {
#if DEBUG
                    await App.Current.MainPage.DisplayAlert("Exception", ex.Message, App.close);
                    return 500;
#else
                    await App.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try after some time.", App.close);
                    return 500;
#endif
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(App.AppName, App.nointernet, App.close);
                return 101;
            }
        }

        public async Task<int> ResendOTP(string mobileno)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                int status = 0;

                try
                {
                    var jsonData = new
                    {
                        MobileNo = CommonClass.Encrypt(mobileno),

                    };

                    string json = JsonConvert.SerializeObject(jsonData);

                    var client = new HttpClient();
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(App.baseurluserregapi + $"OtpResend", content);

                    JObject parsed = new JObject();
                    if ((int)response.StatusCode == 200)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        parsed = JObject.Parse(result);
                        if (parsed.HasValues)
                        {
                            string msg = string.Empty;

                            var message = parsed["message"];
                            status = int.Parse(message["status"].ToString());
                            msg = message["message"].ToString();


                            await App.Current.MainPage.DisplayAlert(App.AppName, msg, App.close);
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(App.AppName, parsed["Message"].ToString(), App.close);
                    }

                    return status;

                }
                catch (Exception ex)
                {
#if DEBUG
                    await App.Current.MainPage.DisplayAlert("Exception", ex.Message, App.close);
                    return 500;
#else
                    await App.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try after some time.", App.close);
                    return 500;
#endif
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(App.AppName, App.nointernet, App.close);
                return 101;
            }
        }

        public async Task<int> ReSendActivationLink(string Email)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                int status = 0;

                try
                {
                    var jsonData = new
                    {
                        Email = CommonClass.Encrypt(Email),

                    };

                    string json = JsonConvert.SerializeObject(jsonData);

                    var client = new HttpClient();
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(App.baseurluserregapi + $"ReSendActivationLink", content);

                    JObject parsed = new JObject();
                    if ((int)response.StatusCode == 200)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        parsed = JObject.Parse(result);
                        if (parsed.HasValues)
                        {
                            string msg = string.Empty;

                            var message = parsed["message"];
                            status = int.Parse(message["status"].ToString());
                            msg = message["message"].ToString();


                            await App.Current.MainPage.DisplayAlert(App.AppName, msg, App.close);
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(App.AppName, parsed["Message"].ToString(), App.close);
                    }

                    return status;

                }
                catch (Exception ex)
                {
#if DEBUG
                    await App.Current.MainPage.DisplayAlert("Exception", ex.Message, App.close);
                    return 500;
#else
                    await App.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try after some time.", App.close);
                    return 500;
#endif
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(App.AppName, App.nointernet, App.close);
                return 101;
            }
        }

        public async Task<int> ActivateOTP(string userid, string otp)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                int status = 0;

                try
                {
                    var jsonData = new
                    {
                        Userid = CommonClass.Encrypt(userid),
                        Otp = CommonClass.Encrypt(otp),
                    };

                    string json = JsonConvert.SerializeObject(jsonData);

                    var client = new HttpClient();
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(App.baseurluserregapi + $"ActivateOTP", content);

                    JObject parsed = new JObject();
                    if ((int)response.StatusCode == 200)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        parsed = JObject.Parse(result);
                        if (parsed.HasValues)
                        {
                            string msg = string.Empty;

                            var message = parsed["message"];
                            status = int.Parse(message["status"].ToString());
                            msg = message["message"].ToString();


                            await App.Current.MainPage.DisplayAlert(App.AppName, msg, App.close);
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(App.AppName, parsed["Message"].ToString(), App.close);
                    }

                    return status;

                }
                catch (Exception ex)
                {
#if DEBUG
                    await App.Current.MainPage.DisplayAlert("Exception", ex.Message, App.close);
                    return 500;
#else
                    await App.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try after some time.", App.close);
                    return 500;
#endif
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(App.AppName, App.nointernet, App.close);
                return 101;
            }
        }

        public async Task<int> GetApplicantDashboardData(string UserID)
        {
            try
            {
                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"GetApplicantDashboardData?"
                + $"UserID={HttpUtility.UrlEncode(UserID)}";

                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    applicantDashboardDatabase.DeleteApplicantDashboard();

                    foreach (var pair in parsed)
                    {
                        if (pair.Key == "DashboardList")
                        {
                            var nodes = pair.Value;
                            var item = new ApplicantDashboard();
                            foreach (var node in nodes)
                            {
                                item.UserImage = node["DocFileLink"].ToString();
                                item.EmailID = node["EmailID"].ToString();
                                item.Exchange = node["Exchange"].ToString();
                                item.MobileNo = node["MobileNo"].ToString();
                                item.Name = node["Name"].ToString();
                                item.RegistrationNo = node["RegistrationNo"].ToString();
                                item.userStatus = node["Stat"].ToString();
                                item.Submissiondate = node["date"].ToString();
                                item.ValidUptodate = node["renewaldate"].ToString();
                                applicantDashboardDatabase.AddApplicantDashboard(item);
                            }

                        }
                    }
                }
                return (int)response.StatusCode;
            }
            catch
            {
                //await Application.Current.MainPage.DisplayAlert("Exception", ey.Message, App.close);
                return 500;
            }
        }

        public async Task<int> GetExistUserData(string DOB, string FHNameDesc, string UserNM)
        {
            try
            {
                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"GetExistUserData?"
                + $"DOB={HttpUtility.UrlEncode(CommonClass.Encrypt(DOB))}"
                + $"&FHNameDesc={HttpUtility.UrlEncode(CommonClass.Encrypt(FHNameDesc))}"
                + $"&UserNM={HttpUtility.UrlEncode(CommonClass.Encrypt(UserNM))}";

                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    existingUserDataDatabase.DeleteExistingUserData();

                    foreach (var pair in parsed)
                    {
                        if (pair.Key == "GetExistUserList")
                        {
                            var nodes = pair.Value;
                            var item = new ExistingUserData();
                            foreach (var node in nodes)
                            {
                                item.Address = CommonClass.Decrypt(node["Address"].ToString());
                                item.DOB = CommonClass.Decrypt(node["DOB"].ToString());
                                item.DistrictCode = CommonClass.Decrypt(node["DistrictCode"].ToString());
                                item.Exchange = CommonClass.Decrypt(node["Exchange"].ToString());
                                item.FAtherHusbandSelection = CommonClass.Decrypt(node["FAtherHusbandSelection"].ToString());
                                item.RegistrationNo = CommonClass.Decrypt(node["RegistrationNo"].ToString());
                                item.XchangeCode = CommonClass.Decrypt(node["XchangeCode"].ToString());
                                item.UserName = CommonClass.Decrypt(node["UserName"].ToString());

                                existingUserDataDatabase.AddExistingUserData(item);
                            }

                        }
                    }
                }
                return (int)response.StatusCode;
            }
            catch
            {
                //await Application.Current.MainPage.DisplayAlert("Exception", ey.Message, App.close);
                return 500;
            }
        }

        public async Task<int> SaveUserMapping(string XchCd, string DOB, string RegNo, string UserID)
        {

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                int status = 0;
                try
                {
                    string ipaddress = await App.GetIPAddress();
                    if (string.IsNullOrEmpty(ipaddress))
                    {
                        ipaddress = "192.168.1.1";
                    }

                    var jsonData = new
                    {
                        XchCd = CommonClass.Encrypt(XchCd),
                        DOB = CommonClass.Encrypt(DOB),
                        RegNo = CommonClass.Encrypt(RegNo),
                        UserID = CommonClass.Encrypt(UserID),
                        IPAddress = CommonClass.Encrypt(ipaddress),
                    };

                    string json = JsonConvert.SerializeObject(jsonData);
                    var client = new HttpClient();
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(App.baseurluserregapi + $"MapRegistration", content);

                    JObject parsed = new JObject();
                    if ((int)response.StatusCode == 200)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        parsed = JObject.Parse(result);
                        if (parsed.HasValues)
                        {
                            string msg = string.Empty;
                            var message = parsed["message"];
                            if (message.Any())
                            {
                                status = int.Parse(message["status"].ToString());
                                msg = message["message"].ToString();
                                await App.Current.MainPage.DisplayAlert(App.AppName, msg, App.close);
                            }
                            else
                            {
                                await App.Current.MainPage.DisplayAlert(App.AppName, message.ToString(), App.close);
                            }
                        }

                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(App.AppName, parsed["Message"].ToString(), App.close);
                    }

                    return status;

                }
                catch (Exception ex)
                {
#if DEBUG
                    await App.Current.MainPage.DisplayAlert("Exception", ex.Message, App.close);
                    return 500;
#else
                    await App.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try after some time.", App.close);
                    return 500;
#endif
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(App.AppName, App.nointernet, App.close);
                return 101;
            }
        }

        public async Task<int> GetAlreadyRegisteredUserData(string DOB, string ExchangeID, string REGNO, string Districtcd, string UserID)
        {
            int status = 0;
            try
            {
                string ipaddress = await App.GetIPAddress();
                if (string.IsNullOrEmpty(ipaddress))
                {
                    ipaddress = "192.168.1.1";
                }

                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"AlreadyRegisteredUserData?"
                + $"RegNo={HttpUtility.UrlEncode(CommonClass.Encrypt(REGNO))}"
                + $"&DOB={HttpUtility.UrlEncode(CommonClass.Encrypt(DOB))}"
                + $"&XchCd={HttpUtility.UrlEncode(CommonClass.Encrypt(ExchangeID))}"
                //+ $"&DistrictCode={HttpUtility.UrlEncode(CommonClass.Encrypt(Districtcd))}"
                + $"&UserID={HttpUtility.UrlEncode(CommonClass.Encrypt(UserID))}"
                + $"&ipaddress={HttpUtility.UrlEncode(CommonClass.Encrypt(ipaddress))}"
                ;

                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    if (parsed.HasValues)
                    {
                        string msg = string.Empty;
                        var message = parsed["message"];
                        status = int.Parse(message["status"].ToString());

                        if (status != 200)
                        {
                            msg = message["message"].ToString();
                            await App.Current.MainPage.DisplayAlert(App.AppName, msg, App.close);
                        }
                        else
                        {
                            foreach (var pair in parsed)
                            {
                                if (pair.Key == "GetAlreadyregisteredList")
                                {
                                    alreadyRegisteredDatabase.DeleteAlreadyRegistered();
                                    var nodes = pair.Value;
                                    var item = new AlreadyRegistered();
                                    foreach (var node in nodes)
                                    {
                                        item.RegistrationNo = REGNO;
                                        item.DOB = DOB;
                                        item.XchangeCode = ExchangeID;
                                        item.RegName = CommonClass.Decrypt(node["RegName"].ToString());
                                        item.F_HNameCd = CommonClass.Decrypt(node["F_HNameCd"].ToString());
                                        item.F_HNameDesc = CommonClass.Decrypt(node["F_HNameDesc"].ToString());
                                        item.RegDt = CommonClass.Decrypt(node["RegDt"].ToString());
                                        item.RenewalDt = CommonClass.Decrypt(node["RenewalDt"].ToString());
                                        item.StatusDesc = CommonClass.Decrypt(node["StatusDesc"].ToString());
                                        item.Email = CommonClass.Decrypt(node["Email"].ToString());
                                        item.MobileNo = CommonClass.Decrypt(node["MobileNo"].ToString());
                                        item.ActivationStatus = CommonClass.Decrypt(node["ActivationStatus"].ToString());
                                        alreadyRegisteredDatabase.AddAlreadyRegistered(item);
                                    }
                                }
                            }
                        }

                    }
                }

                return status;
            }
            catch (Exception ex)
            {
#if DEBUG
                await App.Current.MainPage.DisplayAlert("Exception", ex.Message, App.close);
                return 500;
#else
                    await App.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try after some time.", App.close);
                    return 500;
#endif

            }
        }

        //get saved user data details
        public async Task<int> GetPersonalDetails(string RegNo)
        {
            try
            {
                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"GetPersonalDetails?"
                + $"RegNo={HttpUtility.UrlEncode(CommonClass.Encrypt(RegNo))}";

                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    personalDetailsDatabase.DeletePersonalDetails();

                    foreach (var pair in parsed)
                    {
                        if (pair.Key == "PersonalDetailsList")
                        {
                            var nodes = pair.Value;
                            if (nodes.Any())
                            {
                                var item = new PersonalDetails();
                                foreach (var node in nodes)
                                {
                                    item.DistrictCode = CommonClass.Decrypt(node["DistrictCode"].ToString());
                                    item.XchangeCode = CommonClass.Decrypt(node["XchangeCode"].ToString());
                                    item.UserName = CommonClass.Decrypt(node["UserName"].ToString());
                                    item.FAtherHusbandSelection = CommonClass.Decrypt(node["FAtherHusbandSelection"].ToString());
                                    item.TXTFHNAME = CommonClass.Decrypt(node["TXTFHNAME"].ToString());
                                    item.txtmother = CommonClass.Decrypt(node["txtmother"].ToString());
                                    item.GENDER = CommonClass.Decrypt(node["GENDER"].ToString());
                                    item.ddlmarital = CommonClass.Decrypt(node["ddlmarital"].ToString());
                                    item.DOB = CommonClass.Decrypt(node["DOB"].ToString());
                                    item.Category = CommonClass.Decrypt(node["Category"].ToString());
                                    item.Stat = CommonClass.Decrypt(node["Stat"].ToString());
                                    item.RegistrationNo = CommonClass.Decrypt(node["RegistrationNo"].ToString());
                                    item.issuedt = CommonClass.Decrypt(node["issuedt"].ToString());
                                    item.DocCertNo = CommonClass.Decrypt(node["DocCertNo"].ToString());
                                    item.ddlreligion = CommonClass.Decrypt(node["ddlreligion"].ToString());
                                    item.StatusDesc = CommonClass.Decrypt(node["StatusDesc"].ToString());
                                    item.RejectionRemarks = CommonClass.Decrypt(node["RejectionRemarks"].ToString());
                                    item.DocFileNm = CommonClass.Decrypt(node["DocFileNm"].ToString());
                                    item.DocFileLink = node["DocFileLink"].ToString();
                                    item.RegDate = CommonClass.Decrypt(node["RegDate"].ToString());
                                    item.RenewalMonth = CommonClass.Decrypt(node["RenewalMonth"].ToString());
                                    item.RenewalDate = CommonClass.Decrypt(node["RenewalDate"].ToString());
                                    personalDetailsDatabase.AddPersonalDetails(item);
                                }
                                return 200;
                            }
                            else
                            {
                                return 300;
                            }


                        }
                    }
                }
                return (int)response.StatusCode;
            }
            catch
            {

                return 500;
            }
        }

        public async Task<int> GetContactDetails(string RegNo)
        {
            try
            {
                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"GetContactDetails?"
                + $"RegNo={HttpUtility.UrlEncode(CommonClass.Encrypt(RegNo))}";

                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    contactDetailsDatabase.DeleteContactDetails();

                    foreach (var pair in parsed)
                    {
                        if (pair.Key == "ContactDetailsList")
                        {
                            var nodes = pair.Value;
                            if (nodes.Any())
                            {
                                var item = new ContactDetails();
                                foreach (var node in nodes)
                                {
                                    item.RegistrationNo = RegNo;
                                    item.ddltehsilcode = CommonClass.Decrypt(node["ddltehsilcode"].ToString());
                                    item.ddlvillage = CommonClass.Decrypt(node["ddlvillage"].ToString());
                                    item.txtpo = CommonClass.Decrypt(node["txtpo"].ToString());
                                    item.txtstreet = CommonClass.Decrypt(node["txtstreet"].ToString());
                                    item.txtpincode = CommonClass.Decrypt(node["txtpincode"].ToString());
                                    item.addressmain = CommonClass.Decrypt(node["addressmain"].ToString());
                                    item.txtaddressco = CommonClass.Decrypt(node["txtaddressco"].ToString());
                                    item.txtmobile = CommonClass.Decrypt(node["txtmobile"].ToString());
                                    item.txtphone = CommonClass.Decrypt(node["txtphone"].ToString());
                                    item.txtemail = CommonClass.Decrypt(node["txtemail"].ToString());
                                    item.DocCertificateNo = CommonClass.Decrypt(node["DocCertificateNo"].ToString());
                                    item.IssueDt = CommonClass.Decrypt(node["IssueDt"].ToString());
                                    item.Stat = CommonClass.Decrypt(node["Stat"].ToString());
                                    item.DistrictCode = CommonClass.Decrypt(node["DistrictCode"].ToString());
                                    item.Areatype = CommonClass.Decrypt(node["Areatype"].ToString());
                                    item.DocFileNm = CommonClass.Decrypt(node["DocFileNm"].ToString());
                                    item.DocFileLink = node["DocFileLink"].ToString();
                                    item.sameasabove = CommonClass.Decrypt(node["sameasabove"].ToString());
                                    item.DistrictCode = CommonClass.Decrypt(node["DistrictCode"].ToString());

                                    contactDetailsDatabase.AddContactDetails(item);
                                }
                                return 200;
                            }
                            else
                            {
                                return 300;

                            }


                        }
                    }
                }
                return (int)response.StatusCode;
            }
            catch
            {

                return 500;
            }
        }

        public async Task<int> GetEducationDetails(string RegNo)
        {
            try
            {
                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"GetEducationDetails?"
                + $"RegNo={HttpUtility.UrlEncode(CommonClass.Encrypt(RegNo))}";

                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    qualficationDetailsDatabase.DeleteQualficationDetails();

                    foreach (var pair in parsed)
                    {
                        if (pair.Key == "EducationDetailsList")
                        {
                            var nodes = pair.Value;
                            if (nodes.Any())
                            {
                                var item = new QualficationDetails();
                                foreach (var node in nodes)
                                {
                                    item.RegistrationNo = RegNo;
                                    item.Stat = CommonClass.Decrypt(node["Stat"].ToString());
                                    item.QualNm = CommonClass.Decrypt(node["QualNm"].ToString());
                                    item.ddlboard = CommonClass.Decrypt(node["ddlboard"].ToString());
                                    item.omarks = CommonClass.Decrypt(node["omarks"].ToString());
                                    item.tmarks = CommonClass.Decrypt(node["tmarks"].ToString());
                                    item.perMarks = CommonClass.Decrypt(node["perMarks"].ToString());
                                    item.year = CommonClass.Decrypt(node["year"].ToString());
                                    item.issuedt = CommonClass.Decrypt(node["issuedt"].ToString());
                                    item.validuptodt = CommonClass.Decrypt(node["validuptodt"].ToString());
                                    item.DocCertNo = CommonClass.Decrypt(node["DocCertNo"].ToString());
                                    item.RegDt = CommonClass.Decrypt(node["RegDt"].ToString());
                                    item.QualCd = CommonClass.Decrypt(node["QualCd"].ToString());
                                    item.VerifyStatus = CommonClass.Decrypt(node["VerifyStatus"].ToString());
                                    item.StatusDesc = CommonClass.Decrypt(node["StatusDesc"].ToString());
                                    item.VerifiedDt = CommonClass.Decrypt(node["VerifiedDt"].ToString());
                                    item.Remarks = CommonClass.Decrypt(node["Remarks"].ToString());
                                    item.DocFileNm = CommonClass.Decrypt(node["DocFileNm"].ToString());
                                    item.DocFileLink = node["DocFileLink"].ToString();

                                    qualficationDetailsDatabase.AddQualficationDetails(item);
                                }
                                return 200;

                            }
                            else { return 300; }



                        }
                    }
                }
                return (int)response.StatusCode;
            }
            catch
            {

                return 500;
            }
        }

        public async Task<int> GetMiscelleaneousDetails(string RegNo)
        {
            try
            {
                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"GetMiscelleaneousDetails?"
                + $"RegNo={HttpUtility.UrlEncode(CommonClass.Encrypt(RegNo))}";

                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    miscellaneousDetailsDatabase.DeleteMiscellaneousDetails();

                    foreach (var pair in parsed)
                    {
                        if (pair.Key == "MiscelleaneousDetailsList")
                        {
                            var nodes = pair.Value;
                            if (nodes.Any())
                            {
                                var item = new MiscellaneousDetails();
                                foreach (var node in nodes)
                                {
                                    item.RegistrationNo = RegNo;
                                    item.EyeSight = CommonClass.Decrypt(node["EyeSight"].ToString());
                                    item.Height = CommonClass.Decrypt(node["Height"].ToString());
                                    item.Weight = CommonClass.Decrypt(node["Weight"].ToString());
                                    item.ChestNormal = CommonClass.Decrypt(node["ChestNormal"].ToString());
                                    item.ChastExpended = CommonClass.Decrypt(node["ChastExpended"].ToString());
                                    item.SalaryHomeDist = CommonClass.Decrypt(node["SalaryHomeDist"].ToString());
                                    item.SalaryInHP = CommonClass.Decrypt(node["SalaryInHP"].ToString());
                                    item.SalaryOutHP = CommonClass.Decrypt(node["SalaryOutHP"].ToString());
                                    item.Stat = CommonClass.Decrypt(node["Stat"].ToString());
                                    item.Sector = CommonClass.Decrypt(node["Sector"].ToString());
                                    miscellaneousDetailsDatabase.AddMiscellaneousDetails(item);
                                }
                                return 200;

                            }
                            else { return 300; }



                        }
                    }
                }
                return (int)response.StatusCode;
            }
            catch
            {

                return 500;
            }
        }

        public async Task<int> GetLangDetails(string RegNo)
        {
            try
            {
                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"GetLangDetails?"
                + $"RegNo={HttpUtility.UrlEncode(CommonClass.Encrypt(RegNo))}";

                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    getLangDetailsDatabase.DeleteGetLangDetails();

                    foreach (var pair in parsed)
                    {
                        if (pair.Key == "LangDetailsList")
                        {
                            var nodes = pair.Value;
                            if (nodes.Any())
                            {
                                var item = new GetLangDetails();
                                foreach (var node in nodes)
                                {
                                    item.RegistrationNo = RegNo;
                                    item.Langcd = CommonClass.Decrypt(node["Langcd"].ToString());
                                    item.LangName = CommonClass.Decrypt(node["LangName"].ToString());
                                    item.Read = CommonClass.Decrypt(node["Read"].ToString());
                                    item.Write = CommonClass.Decrypt(node["Write"].ToString());
                                    item.Speak = CommonClass.Decrypt(node["Speak"].ToString());
                                    getLangDetailsDatabase.AddGetLangDetails(item);
                                }
                                return 200;

                            }
                            else { return 300; }



                        }
                    }
                }
                return (int)response.StatusCode;
            }
            catch
            {

                return 500;
            }
        }

        public async Task<int> GetSubcategoryDetails(string RegNo)
        {
            try
            {
                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"GetSubcategoryDetails?"
                + $"RegNo={HttpUtility.UrlEncode(CommonClass.Encrypt(RegNo))}";

                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    subCategoryDetailsDatabase.DeleteSubCategoryDetails();

                    foreach (var pair in parsed)
                    {
                        if (pair.Key == "SubcategoryDetailsList")
                        {
                            var nodes = pair.Value;
                            if (nodes.Any())
                            {
                                var item = new SubCategoryDetails();
                                foreach (var node in nodes)
                                {
                                    item.RegistrationNo = RegNo;
                                    item.Stat = CommonClass.Decrypt(node["Stat"].ToString());
                                    item.SubCategory = CommonClass.Decrypt(node["SubCategory"].ToString());
                                    item.SubCategoryNm = CommonClass.Decrypt(node["SubCategoryNm"].ToString());
                                    item.issuedt = CommonClass.Decrypt(node["issuedt"].ToString());
                                    item.validuptodt = CommonClass.Decrypt(node["validuptodt"].ToString());
                                    item.DocCertNo = CommonClass.Decrypt(node["DocCertNo"].ToString());
                                    item.VerifyStatus = CommonClass.Decrypt(node["VerifyStatus"].ToString());
                                    item.StatusDesc = CommonClass.Decrypt(node["StatusDesc"].ToString());
                                    item.VerifiedDt = CommonClass.Decrypt(node["VerifiedDt"].ToString());
                                    item.DocFileNm = CommonClass.Decrypt(node["DocFileNm"].ToString());
                                    item.DocFileLink = node["DocFileLink"].ToString();

                                    subCategoryDetailsDatabase.AddSubCategoryDetails(item);
                                }
                                return 200;

                            }
                            else { return 300; }
                        }
                    }
                }
                return (int)response.StatusCode;
            }
            catch
            {

                return 500;
            }
        }

        public async Task<int> GetEmployedDetails(string RegNo)
        {
            try
            {
                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"GetEmployedDetails?"
                + $"RegNo={HttpUtility.UrlEncode(CommonClass.Encrypt(RegNo))}";

                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    employedDetailsDatabase.DeleteEmployedDetails();

                    foreach (var pair in parsed)
                    {
                        if (pair.Key == "EmployedDetailsList")
                        {
                            var nodes = pair.Value;
                            if (nodes.Any())
                            {
                                var item = new EmployedDetails();
                                foreach (var node in nodes)
                                {
                                    item.RegistrationNo = RegNo;
                                    item.EmploymentStatus = CommonClass.Decrypt(node["EmploymentStatus"].ToString());
                                    item.EmploymentSector = CommonClass.Decrypt(node["EmploymentSector"].ToString());
                                    item.EmploymentType = CommonClass.Decrypt(node["EmploymentType"].ToString());
                                    item.OrganisationName = CommonClass.Decrypt(node["OrganisationName"].ToString());
                                    item.RegisteredOrganisationName = CommonClass.Decrypt(node["RegisteredOrganisationName"].ToString());
                                    item.RegisteredOrganisationID = CommonClass.Decrypt(node["RegisteredOrganisationID"].ToString());
                                    item.Stat = CommonClass.Decrypt(node["Stat"].ToString());
                                    employedDetailsDatabase.AddEmployedDetails(item);
                                }
                                return 200;

                            }
                            else { return 300; }
                        }
                    }
                }
                return (int)response.StatusCode;
            }
            catch
            {

                return 500;
            }
        }

        public async Task<int> GetPHDetails(string RegNo)
        {
            try
            {
                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"GetPHDetails?"
                + $"RegNo={HttpUtility.UrlEncode(CommonClass.Encrypt(RegNo))}";

                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    pHDetailsDatabase.DeletePHDetails();

                    foreach (var pair in parsed)
                    {
                        if (pair.Key == "PHDetailsList")
                        {
                            var nodes = pair.Value;
                            if (nodes.Any())
                            {
                                var item = new PHDetails();
                                foreach (var node in nodes)
                                {
                                    item.RegistrationNo = RegNo;
                                    item.Stat = CommonClass.Decrypt(node["Stat"].ToString());
                                    item.PHType = CommonClass.Decrypt(node["PHType"].ToString());
                                    item.PHTypeNm = CommonClass.Decrypt(node["PHTypeNm"].ToString());
                                    item.Percentage = CommonClass.Decrypt(node["Percentage"].ToString());
                                    item.Remarks = CommonClass.Decrypt(node["Remarks"].ToString());
                                    item.PhRegDate = CommonClass.Decrypt(node["PhRegDate"].ToString());
                                    item.issuedt = CommonClass.Decrypt(node["issuedt"].ToString());
                                    item.validuptodt = CommonClass.Decrypt(node["validuptodt"].ToString());
                                    item.DocCertNo = CommonClass.Decrypt(node["DocCertNo"].ToString());
                                    item.StatusDesc = CommonClass.Decrypt(node["StatusDesc"].ToString());
                                    item.VerifiedDt = CommonClass.Decrypt(node["VerifiedDt"].ToString());
                                    item.VerifyStatus = CommonClass.Decrypt(node["VerifyStatus"].ToString());
                                    item.DocFileNm = CommonClass.Decrypt(node["DocFileNm"].ToString());
                                    item.DocFileLink = node["DocFileLink"].ToString();
                                    pHDetailsDatabase.AddPHDetails(item);
                                }
                                return 200;

                            }
                            else { return 300; }
                        }
                    }
                }
                return (int)response.StatusCode;
            }
            catch
            {

                return 500;
            }
        }

        public async Task<int> GetXservicemenDetails(string RegNo)
        {
            try
            {
                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"GetXservicemenDetails?"
                + $"RegNo={HttpUtility.UrlEncode(CommonClass.Encrypt(RegNo))}";

                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    xservicemenDetailsDatabase.DeleteXservicemenDetails();

                    foreach (var pair in parsed)
                    {
                        if (pair.Key == "XservicemenDetailsList")
                        {
                            var nodes = pair.Value;
                            if (nodes.Any())
                            {
                                var item = new XservicemenDetails();
                                foreach (var node in nodes)
                                {
                                    item.RegistrationNo = RegNo;
                                    item.ddlforce = CommonClass.Decrypt(node["ddlforce"].ToString());
                                    item.ddlrank = CommonClass.Decrypt(node["ddlrank"].ToString());
                                    item.txtservicenumber = CommonClass.Decrypt(node["txtservicenumber"].ToString());
                                    item.txtregimentname = CommonClass.Decrypt(node["txtregimentname"].ToString());
                                    item.ddlmedical = CommonClass.Decrypt(node["ddlmedical"].ToString());
                                    item.ddlcharacter = CommonClass.Decrypt(node["ddlcharacter"].ToString());
                                    item.txtdischargedate = CommonClass.Decrypt(node["txtdischargedate"].ToString());
                                    item.txtenrolmentdate = CommonClass.Decrypt(node["txtenrolmentdate"].ToString());
                                    item.ddlreason = CommonClass.Decrypt(node["ddlreason"].ToString());
                                    item.txtremarks = CommonClass.Decrypt(node["txtremarks"].ToString());
                                    item.Stat = CommonClass.Decrypt(node["Stat"].ToString());
                                    item.ValidUptoDt = CommonClass.Decrypt(node["ValidUptoDt"].ToString());
                                    item.IssueDt = CommonClass.Decrypt(node["IssueDt"].ToString());
                                    item.DocCertificateNo = CommonClass.Decrypt(node["DocCertificateNo"].ToString());
                                    item.DocFileNm = CommonClass.Decrypt(node["DocFileNm"].ToString());
                                    item.DocFileLink = node["DocFileLink"].ToString();
                                    xservicemenDetailsDatabase.AddXservicemenDetails(item);
                                }
                                return 200;

                            }
                            else { return 300; }
                        }
                    }
                }
                return (int)response.StatusCode;
            }
            catch
            {

                return 500;
            }
        }

        public async Task<int> GetNCODetails(string RegNo)
        {
            try
            {
                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"GetNCODetails?"
                + $"RegNo={HttpUtility.UrlEncode(CommonClass.Encrypt(RegNo))}";

                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    getNCODetailsDatabase.DeleteGetNCODetails();

                    foreach (var pair in parsed)
                    {
                        if (pair.Key == "NCODetailsList")
                        {
                            var nodes = pair.Value;
                            if (nodes.Any())
                            {
                                var item = new GetNCODetails();
                                foreach (var node in nodes)
                                {
                                    item.RegistrationNo = RegNo;
                                    item.Stat = CommonClass.Decrypt(node["Stat"].ToString());
                                    item.NCOCd = CommonClass.Decrypt(node["NCOCd"].ToString());
                                    item.NCONm = CommonClass.Decrypt(node["NCONm"].ToString());
                                    item.SrNo = CommonClass.Decrypt(node["SrNo"].ToString());
                                    item.StatusDesc = CommonClass.Decrypt(node["StatusDesc"].ToString());
                                    item.VerifiedDt = CommonClass.Decrypt(node["VerifiedDt"].ToString());
                                    item.VerifyStatus = CommonClass.Decrypt(node["VerifyStatus"].ToString());
                                    item.experience = CommonClass.Decrypt(node["experience"].ToString());
                                    item.regdt = CommonClass.Decrypt(node["regdt"].ToString());
                                    item.remarks = CommonClass.Decrypt(node["remarks"].ToString());

                                    getNCODetailsDatabase.AddGetNCODetails(item);
                                }
                                return 200;

                            }
                            else { return 300; }
                        }
                    }
                }
                return (int)response.StatusCode;
            }
            catch
            {

                return 500;
            }
        }

        public async Task<int> GetRegDetailsLabels(string RegNo)
        {
            try
            {
                var client = new HttpClient();
                string parameters = App.baseurluserregapi + $"GetRegFormsData?"
                + $"RegistrationNo={HttpUtility.UrlEncode(CommonClass.Encrypt(RegNo))}";

                HttpResponseMessage response = await client.GetAsync(parameters);
                if ((int)response.StatusCode == 200)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);

                    foreach (var pair in parsed)
                    {
                        if (pair.Key == "SubmittedForms")
                        {
                            submittedFormsDatabase.DeleteSubmittedFormsDetails();

                            string query = "Select * from SubmittedFormsDetails";
                            var m = submittedFormsDatabase.GetSubmittedFormsDetails(query).ToList();
                            var nodes = pair.Value;
                            if (nodes.Any())
                            {
                                var item = new SubmittedFormsDetails();
                                foreach (var node in nodes)
                                {
                                    item.RegistrationNo = RegNo;

                                    item.ContactDetailsYN = CommonClass.Decrypt(node["ContactDetails"].ToString());
                                    item.PersonalDetailsYN = CommonClass.Decrypt(node["PersonalDetails"].ToString());
                                    item.PersonalDetails = CommonClass.Decrypt(node["PersonalDetails"].ToString()) == "N" ? "#c0c0c0" : "#FF006400";
                                    item.ContactDetails = CommonClass.Decrypt(node["ContactDetails"].ToString()) == "N" ? "#c0c0c0" : "#FF006400";
                                    item.EducationDetails = CommonClass.Decrypt(node["EducationDetails"].ToString()) == "N" ? "#c0c0c0" : "#FF006400";
                                    item.EmployedDetails = CommonClass.Decrypt(node["EmployedDetails"].ToString()) == "N" ? "#c0c0c0" : "#FF006400";
                                    item.ExDetails = CommonClass.Decrypt(node["ExDetails"].ToString()) == "N" ? "#c0c0c0" : "#FF006400";
                                    item.NCODetails = CommonClass.Decrypt(node["NCODetails"].ToString()) == "N" ? "#c0c0c0" : "#FF006400";
                                    item.MiscDetails = CommonClass.Decrypt(node["OtherDetails"].ToString()) == "N" ? "#c0c0c0" : "#FF006400";
                                    item.PH = CommonClass.Decrypt(node["PH"].ToString()) == "N" ? "#c0c0c0" : "#FF006400";
                                    item.SubCat = CommonClass.Decrypt(node["SubCat"].ToString()) == "N" ? "#c0c0c0" : "#FF006400";
                                    item.ActiveForm = CommonClass.Decrypt(node["ActiveForm"].ToString());
                                    item.Stat = CommonClass.Decrypt(node["Stat"].ToString());

                                    submittedFormsDatabase.AddSubmittedFormsDetails(item);
                                }
                                return 200;

                            }
                            else { return 300; }
                        }
                    }
                }
                return (int)response.StatusCode;
            }
            catch
            {

                return 500;
            }
        }
              
        public async Task<int> FinalSubmit(string RegNo)
        {

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                int status = 0;
                try
                {
                    string ipaddress = await App.GetIPAddress();
                    if (string.IsNullOrEmpty(ipaddress))
                    {
                        ipaddress = "192.168.1.1";
                    }
                    var jsonData = new
                    {
                        RegNo = RegNo != null ? CommonClass.Encrypt(RegNo) : "",
                        IPAddress = CommonClass.Encrypt(ipaddress),
                    };
                    string json = JsonConvert.SerializeObject(jsonData);

                    var client = new HttpClient();
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(App.baseurluserregapi + $"SubmitRegistration", content);

                    JObject parsed = new JObject();
                    if ((int)response.StatusCode == 200)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        parsed = JObject.Parse(result);
                        if (parsed.HasValues)
                        {
                            string msg = string.Empty;
                            var message = parsed["message"];
                            status = int.Parse(message["status"].ToString());
                            msg = message["message"].ToString();
                            await App.Current.MainPage.DisplayAlert(App.AppName, msg, App.close);
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(App.AppName, parsed["Message"].ToString(), App.close);
                    }
                    return status;

                }
                catch (Exception ex)
                {
#if DEBUG
                    await App.Current.MainPage.DisplayAlert("Exception", ex.Message, App.close);
                    return 500;
#else
                    await App.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try after some time.", App.close);
                    return 500;
#endif
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(App.AppName, App.nointernet, App.close);
                return 101;
            }
        }

        public async Task<int> TakeLive(string RegNo, string MappedByUser, string XchangeCode)
        {

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                int status = 0;
                try
                {
                    string ipaddress = await App.GetIPAddress();
                    if (string.IsNullOrEmpty(ipaddress))
                    {
                        ipaddress = "192.168.1.1";
                    }
                    var jsonData = new
                    {
                        RegNo = RegNo != null ? CommonClass.Encrypt(RegNo) : "",
                        MappedByUser = MappedByUser != null ? CommonClass.Encrypt(MappedByUser) : "",
                        MappedByIP = CommonClass.Encrypt(ipaddress),
                        XchangeCode = XchangeCode != null ? CommonClass.Encrypt(XchangeCode) : "",
                    };
                    string json = JsonConvert.SerializeObject(jsonData);

                    var client = new HttpClient();
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(App.baseurluserregapi + $"TakeLiveReg", content);

                    JObject parsed = new JObject();
                    if ((int)response.StatusCode == 200)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        parsed = JObject.Parse(result);
                        if (parsed.HasValues)
                        {
                            string msg = string.Empty;
                            var message = parsed["message"];
                            status = int.Parse(message["status"].ToString());
                            msg = message["message"].ToString();
                            await App.Current.MainPage.DisplayAlert(App.AppName, msg, App.close);
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(App.AppName, parsed["Message"].ToString(), App.close);
                    }
                    return status;

                }
                catch (Exception ex)
                {
#if DEBUG
                    await App.Current.MainPage.DisplayAlert("Exception", ex.Message, App.close);
                    return 500;
#else
                    await App.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try after some time.", App.close);
                    return 500;
#endif
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(App.AppName, App.nointernet, App.close);
                return 101;
            }
        }

        public async Task<int> DelEducationDetails(string RegNo, string QualCd)
        {

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                int status = 0;
                try
                {
                    var jsonData = new
                    {
                        RegNo = RegNo != null ? CommonClass.Encrypt(RegNo) : "",
                        QualCd = QualCd != null ? CommonClass.Encrypt(QualCd) : "",

                    };
                    string json = JsonConvert.SerializeObject(jsonData);

                    var client = new HttpClient();
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(App.baseurluserregapi + $"DelEducationDetails", content);

                    JObject parsed = new JObject();
                    if ((int)response.StatusCode == 200)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        parsed = JObject.Parse(result);
                        if (parsed.HasValues)
                        {
                            string msg = string.Empty;
                            var message = parsed["message"];
                            status = int.Parse(message["status"].ToString());
                            msg = message["message"].ToString();
                            await App.Current.MainPage.DisplayAlert(App.AppName, msg, App.close);
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(App.AppName, parsed["Message"].ToString(), App.close);
                    }
                    return status;

                }
                catch (Exception ex)
                {
#if DEBUG
                    await App.Current.MainPage.DisplayAlert("Exception", ex.Message, App.close);
                    return 500;
#else
                    await App.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try after some time.", App.close);
                    return 500;
#endif
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(App.AppName, App.nointernet, App.close);
                return 101;
            }
        }

        public async Task<int> DelSubcatDetails(string RegNo, string SubcatCd)
        {

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                int status = 0;
                try
                {
                    var jsonData = new
                    {
                        RegNo = RegNo != null ? CommonClass.Encrypt(RegNo) : "",
                        SubcatCd = SubcatCd != null ? CommonClass.Encrypt(SubcatCd) : "",

                    };
                    string json = JsonConvert.SerializeObject(jsonData);

                    var client = new HttpClient();
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(App.baseurluserregapi + $"DelSubcatDetails", content);

                    JObject parsed = new JObject();
                    if ((int)response.StatusCode == 200)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        parsed = JObject.Parse(result);
                        if (parsed.HasValues)
                        {
                            string msg = string.Empty;
                            var message = parsed["message"];
                            status = int.Parse(message["status"].ToString());
                            msg = message["message"].ToString();
                            await App.Current.MainPage.DisplayAlert(App.AppName, msg, App.close);
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(App.AppName, parsed["Message"].ToString(), App.close);
                    }
                    return status;

                }
                catch (Exception ex)
                {
#if DEBUG
                    await App.Current.MainPage.DisplayAlert("Exception", ex.Message, App.close);
                    return 500;
#else
                    await App.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try after some time.", App.close);
                    return 500;
#endif
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(App.AppName, App.nointernet, App.close);
                return 101;
            }
        }

        public async Task<int> DelPHDetails(string RegNo, string PHCd)
        {

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                int status = 0;
                try
                {
                    var jsonData = new
                    {
                        RegNo = RegNo != null ? CommonClass.Encrypt(RegNo) : "",
                        PHCd = PHCd != null ? CommonClass.Encrypt(PHCd) : "",

                    };
                    string json = JsonConvert.SerializeObject(jsonData);

                    var client = new HttpClient();
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(App.baseurluserregapi + $"DelPHDetails", content);

                    JObject parsed = new JObject();
                    if ((int)response.StatusCode == 200)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        parsed = JObject.Parse(result);
                        if (parsed.HasValues)
                        {
                            string msg = string.Empty;
                            var message = parsed["message"];
                            status = int.Parse(message["status"].ToString());
                            msg = message["message"].ToString();
                            await App.Current.MainPage.DisplayAlert(App.AppName, msg, App.close);
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(App.AppName, parsed["Message"].ToString(), App.close);
                    }
                    return status;

                }
                catch (Exception ex)
                {
#if DEBUG
                    await App.Current.MainPage.DisplayAlert("Exception", ex.Message, App.close);
                    return 500;
#else
                    await App.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try after some time.", App.close);
                    return 500;
#endif
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(App.AppName, App.nointernet, App.close);
                return 101;
            }
        }

        public async Task<int> DelNCODetails(string RegNo, string NCONo, string SrNo)
        {

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                int status = 0;
                try
                {
                    var jsonData = new
                    {
                        RegNo = RegNo != null ? CommonClass.Encrypt(RegNo) : "",
                        NCONo = NCONo != null ? CommonClass.Encrypt(NCONo) : "",
                        SrNo = SrNo != null ? CommonClass.Encrypt(SrNo) : "",

                    };
                    string json = JsonConvert.SerializeObject(jsonData);

                    var client = new HttpClient();
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(App.baseurluserregapi + $"DelNCODetails", content);

                    JObject parsed = new JObject();
                    if ((int)response.StatusCode == 200)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        parsed = JObject.Parse(result);
                        if (parsed.HasValues)
                        {
                            string msg = string.Empty;
                            var message = parsed["message"];
                            status = int.Parse(message["status"].ToString());
                            msg = message["message"].ToString();
                            await App.Current.MainPage.DisplayAlert(App.AppName, msg, App.close);
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(App.AppName, parsed["Message"].ToString(), App.close);
                    }
                    return status;

                }
                catch (Exception ex)
                {
#if DEBUG
                    await App.Current.MainPage.DisplayAlert("Exception", ex.Message, App.close);
                    return 500;
#else
                    await App.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try after some time.", App.close);
                    return 500;
#endif
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(App.AppName, App.nointernet, App.close);
                return 101;
            }
        }

        public async Task<int> UploadImageInsUpd(string _file, string UserID)
        {

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                int status = 0;
                try
                {

                    var jsonData = new
                    {

                        document = _file != null ? _file : "",
                        UserID = UserID != null ? CommonClass.Encrypt(UserID) : "",
                    };

                    string json = JsonConvert.SerializeObject(jsonData);

                    var client = new HttpClient();

                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(App.baseurluserregapi + $"UploadImageInsUpd", content);

                    JObject parsed = new JObject();
                    if ((int)response.StatusCode == 200)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        parsed = JObject.Parse(result);
                        if (parsed.HasValues)
                        {
                            string msg = string.Empty;
                            var message = parsed["message"];

                            status = int.Parse(message["status"].ToString());
                            msg = message["message"].ToString();
                            await App.Current.MainPage.DisplayAlert(App.AppName, msg, App.close);
                        }

                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(App.AppName, parsed["Message"].ToString(), App.close);
                    }

                    return status;

                }
                catch (Exception ex)
                {
#if DEBUG
                    await App.Current.MainPage.DisplayAlert("Exception", ex.Message, App.close);
                    return 500;
#else
                    await App.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try after some time.", App.close);
                    return 500;
#endif
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(App.AppName, App.nointernet, App.close);
                return 101;
            }
        }

        public async Task<int> RenewReg(string RegNo, string UserID)
        {

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                string ipaddress = await App.GetIPAddress();
                if (string.IsNullOrEmpty(ipaddress))
                {
                    ipaddress = "192.168.1.1";
                }

                int status = 0;
                try
                {
                    var jsonData = new
                    {
                        RegNo = RegNo != null ? CommonClass.Encrypt(RegNo) : "",
                        UserID = UserID != null ? CommonClass.Encrypt(UserID) : "",
                        ModifiedByIP = ipaddress != null ? CommonClass.Encrypt(ipaddress) : "",

                    };
                    string json = JsonConvert.SerializeObject(jsonData);

                    var client = new HttpClient();
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(App.baseurluserregapi + $"RenewReg", content);

                    JObject parsed = new JObject();
                    if ((int)response.StatusCode == 200)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        parsed = JObject.Parse(result);
                        if (parsed.HasValues)
                        {
                            string msg = string.Empty;
                            var message = parsed["message"];
                            status = int.Parse(message["status"].ToString());
                            msg = message["message"].ToString();
                            await Application.Current.MainPage.DisplayAlert(App.AppName, msg, App.close);
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(App.AppName, parsed["Message"].ToString(), App.close);
                    }
                    return status;

                }
                catch (Exception ex)
                {
#if DEBUG
                    await App.Current.MainPage.DisplayAlert("Exception", ex.Message, App.close);
                    return 500;
#else
                    await App.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try after some time.", App.close);
                    return 500;
#endif
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(App.AppName, App.nointernet, App.close);
                return 101;
            }
        }

        public async Task<int> ForgotPasslink(string LoginId)
        {

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                string ipaddress = await App.GetIPAddress();
                if (string.IsNullOrEmpty(ipaddress))
                {
                    ipaddress = "192.168.1.1";
                }

                int status = 0;
                try
                {
                    var jsonData = new
                    {
                        LoginId = LoginId != null ? CommonClass.Encrypt(LoginId) : "",
                        IpAddress = ipaddress != null ? CommonClass.Encrypt(ipaddress) : "",

                    };
                    string json = JsonConvert.SerializeObject(jsonData);

                    var client = new HttpClient();
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(App.baseurluserregapi + $"ForgotPasslink", content);

                    JObject parsed = new JObject();
                    if ((int)response.StatusCode == 200)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        parsed = JObject.Parse(result);
                        if (parsed.HasValues)
                        {
                            string msg = string.Empty;
                            var message = parsed["message"];
                            status = int.Parse(message["status"].ToString());
                            string RecoveryId = message["RegNo"].ToString();
                            
                                Preferences.Set("RecoveryId", RecoveryId);
                            


                            msg = message["message"].ToString();
                            await Application.Current.MainPage.DisplayAlert(App.AppName, msg, App.close);
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(App.AppName, parsed["Message"].ToString(), App.close);
                    }
                    return status;

                }
                catch (Exception ex)
                {
#if DEBUG
                    await App.Current.MainPage.DisplayAlert("Exception", ex.Message, App.close);
                    return 500;
#else
                    await App.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try after some time.", App.close);
                    return 500;
#endif
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(App.AppName, App.nointernet, App.close);
                return 101;
            }
        }

        public async Task<int> VerifyForgotPassCd(string LoginId, string RecoveryId, string OTP)
        {

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                string ipaddress = await App.GetIPAddress();
                if (string.IsNullOrEmpty(ipaddress))
                {
                    ipaddress = "192.168.1.1";
                }

                int status = 0;
                try
                {
                    var jsonData = new
                    {
                        LoginId = LoginId != null ? CommonClass.Encrypt(LoginId) : "",
                        RecoveryId = RecoveryId != null ? CommonClass.Encrypt(RecoveryId) : "",
                        OTP = OTP != null ? CommonClass.Encrypt(OTP) : "",
                        IpAddress = ipaddress != null ? CommonClass.Encrypt(ipaddress) : "",

                    };
                    string json = JsonConvert.SerializeObject(jsonData);

                    var client = new HttpClient();
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(App.baseurluserregapi + $"VerifyForgotPassCd", content);

                    JObject parsed = new JObject();
                    if ((int)response.StatusCode == 200)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        parsed = JObject.Parse(result);
                        if (parsed.HasValues)
                        {
                            string msg = string.Empty;
                            var message = parsed["message"];
                            status = int.Parse(message["status"].ToString());



                            msg = message["message"].ToString();
                            await Application.Current.MainPage.DisplayAlert(App.AppName, msg, App.close);
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(App.AppName, parsed["Message"].ToString(), App.close);
                    }
                    return status;

                }
                catch (Exception ex)
                {
#if DEBUG
                    await App.Current.MainPage.DisplayAlert("Exception", ex.Message, App.close);
                    return 500;
#else
                    await App.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try after some time.", App.close);
                    return 500;
#endif
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(App.AppName, App.nointernet, App.close);
                return 101;
            }
        }

        public async Task<int> ChangePassword(string LoginId, string RecoveryId, string Password, string ConfirmPassword)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                string ipaddress = await App.GetIPAddress();
                if (string.IsNullOrEmpty(ipaddress))
                {
                    ipaddress = "192.168.1.1";
                }

                int status = 0;
                try
                {
                    var jsonData = new
                    {
                        LoginId = LoginId != null ? CommonClass.Encrypt(LoginId) : "",
                        RecoveryId = RecoveryId != null ? CommonClass.Encrypt(RecoveryId) : "",
                        Password = Password != null ? CommonClass.Encrypt(CommonClass.GetSha256FromString(Password)) : "",
                        ConfirmPassword = ConfirmPassword != null ? CommonClass.Encrypt(CommonClass.GetSha256FromString(ConfirmPassword)) : "",
                        IpAddress = ipaddress != null ? CommonClass.Encrypt(ipaddress) : "",

                    };
                    string json = JsonConvert.SerializeObject(jsonData);

                    var client = new HttpClient();
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(App.baseurluserregapi + $"ChangePassword", content);

                    JObject parsed = new JObject();
                    if ((int)response.StatusCode == 200)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        parsed = JObject.Parse(result);
                        if (parsed.HasValues)
                        {
                            string msg = string.Empty;
                            var message = parsed["message"];
                            status = int.Parse(message["status"].ToString());



                            msg = message["message"].ToString();
                            await Application.Current.MainPage.DisplayAlert(App.AppName, msg, App.close);
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(App.AppName, parsed["Message"].ToString(), App.close);
                    }
                    return status;

                }
                catch (Exception ex)
                {
#if DEBUG
                    await App.Current.MainPage.DisplayAlert("Exception", ex.Message, App.close);
                    return 500;
#else
                    await App.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try after some time.", App.close);
                    return 500;
#endif
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(App.AppName, App.nointernet, App.close);
                return 101;
            }
        }

        public void getpdf(string RegNo, string filefor, string fileforcode)
        {        
            string url1 = App.pdfurluserregapi
            + $"RegNo={HttpUtility.UrlEncode(CommonClass.Encrypt(RegNo))}"
            + $"&filefor={HttpUtility.UrlEncode(CommonClass.Encrypt(filefor))}"
            + $"&fileforcode={HttpUtility.UrlEncode(CommonClass.Encrypt(fileforcode))}";
            Launcher.OpenAsync(url1);
        }

        public string getusername(string RegNo)
        {
            string username = string.Empty;
            string query = $"Select * from PersonalDetails where RegistrationNo= '{RegNo}'";
            var m = personalDetailsDatabase.GetPersonalDetails(query).ToList();
            if (m.Count > 0)
            {
                username = m.ElementAt(0).UserName;
            }

            return username;
        }

    }
}
