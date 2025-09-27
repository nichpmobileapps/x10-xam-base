using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using X10Card.Models;
using X10Card.Models.NewUserRegistration;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace X10Card
{
    public partial class App : Application
    {
        public static Page[] pages;
        public static string access_key = "1E0762F5768C727AC462C80967AA80F4";
        public static string access_id = "NICHPSC";
        public static string AppName = "X10 (Registration Card)";
        public static string close = "Close";
        public static string nointernet = "No Internet Connection Found!";
        public static string privacypolicyurl = "https://himachal.nic.in/mobile-app-privacy-policy/X10.html";
        public static string GetAppVersionDetailsUrl = "https://mobileappshp.nic.in/MyDiary/MobileAppVersions.svc/GetAppVersion?";
        public static string Redirectionuri = "";


        public static string baseurlx10api = "http://10.146.2.67/eemis/X10Service.svc/";
        public static string baseurluserregapi = "http://10.146.2.67/eemis/NewUserRegistration.svc/";
        public static string pdfurluserregapi = "http://10.146.2.67/eemis/ViewPDF/ViewPDF.aspx?";
 

        /*public static string baseurlx10api = "https://eemis.hp.nic.in/X10Service.svc/";
        public static string baseurluserregapi = "https://eemis.hp.nic.in/NewUserRegistration.svc/";
        public static string pdfurluserregapi = "https://eemis.hp.nic.in/ViewPDF/ViewPDF.aspx?";*/

        //old

        /*     public static string LoginUrl = "https://eemis.hp.nic.in/X10Service.svc/ValidateApplicant2024?";
             public static string UpdEmpStatUrl = "https://eemis.hp.nic.in/X10Service.svc/UpdEmpStat?";
             public static string AllowanceDetailsUrl = "https://eemis.hp.nic.in/X10Service.svc/GetAllowanceDetails?";
             public static string AllVacanciesUrl = "https://eemis.hp.nic.in/X10Service.svc/GetPublishedVacancies?";
             public static string GetPublishedjobfairsUrl = "https://eemis.hp.nic.in/X10Service.svc/GetPublishedjobfairs?";
             public static string GetPublishedJOBFAIRVacanciesUrl = "https://eemis.hp.nic.in/X10Service.svc/GetPublishedJOBFAIRVacancies?";
            public static string baseurluserregapi = "https://eemis.hp.nic.in/NewUserRegistration.svc/";
             public static string pdfurluserregapi = "https://eemis.hp.nic.in/ViewPDF/ViewPDF.aspx?";
         */

        /*  public static string LoginUrl = "http://10.146.2.89/eemis/X10Service.svc/ValidateApplicant2024?";
          public static string UpdEmpStatUrl = "http://10.146.2.89/eemis/X10Service.svc/UpdEmpStat?";
          public static string AllowanceDetailsUrl = "http://10.146.2.89/eemis/X10Service.svc/GetAllowanceDetails?";
          public static string AllVacanciesUrl = "http://10.146.2.89/eemis/X10Service.svc/GetPublishedVacancies?";
          public static string GetPublishedjobfairsUrl = "http://10.146.2.89/eemis/X10Service.svc/GetPublishedjobfairs?";
          public static string GetPublishedJOBFAIRVacanciesUrl = "http://10.146.2.89/eemis/X10Service.svc/GetPublishedJOBFAIRVacancies?";*/

        public static UserDetailsDatabase userDetailsDatabase;
        public static List<UserDetails> userDetailslist;
        public static NCODetailsDatabase nCODetailsDatabase;
        public static QualificationDetailsDatabase qualificationDetailsDatabase;
        public static EmploymentStatusDatabase employmentStatusDatabase;
        public static SubEmploymentStatusDatabase subEmploymentStatusDatabase;
        public static AllowanceDetailsDatabase allowanceDetailsDatabase;
        public static AllowanceTransactionsDatabase allowanceTransactionsDatabase;
        public static AllVacancyDetailsDatabase allVacancyDetailsDatabase = new AllVacancyDetailsDatabase();
        public static SponsorshipDetailsDatabase sponsorshipDetailsDatabase = new SponsorshipDetailsDatabase();
        public static JobFairDetailsDatabase jobFairDetailsDatabase = new JobFairDetailsDatabase();
        public static JobfairsEmployersDatabase jobfairsEmployersDatabase = new JobfairsEmployersDatabase();
        public static JobFairEmployersVacanciesDatabase jobFairEmployersVacanciesDatabase = new JobFairEmployersVacanciesDatabase();
        public static PersonalDetailsDatabase personalDetailsDatabase = new PersonalDetailsDatabase();
        public static List<PersonalDetails> personalDetailsList;

        protected static string SecretKey = "%&2022$X10%Registration$$1309cardKey%";
       /* protected static byte[] keybytes = Encoding.UTF8.GetBytes("e8ffc7e56311679f12b6fc91aa77a5eb");
        protected static byte[] iv = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };*/
        string loggedin;
        public static string PhotoBase64;
        public static MediaFile imagememroystreamsource;


        public App()
        {
            InitializeComponent();
            userDetailsDatabase = new UserDetailsDatabase();
            nCODetailsDatabase = new NCODetailsDatabase();
            qualificationDetailsDatabase = new QualificationDetailsDatabase();
            employmentStatusDatabase = new EmploymentStatusDatabase();
            subEmploymentStatusDatabase = new SubEmploymentStatusDatabase();
            allowanceDetailsDatabase = new AllowanceDetailsDatabase();
            allowanceTransactionsDatabase = new AllowanceTransactionsDatabase();
            //MainPage = new NavigationPage(new MainPage());
            //return;
           
                
                    loggedin = Preferences.Get("LoggedIn", "");
               

                if (!string.IsNullOrEmpty(loggedin))
                {
                    if (loggedin.Equals("Y"))
                    {
                        MainPage = new NavigationPage(new PostLoginDashboardPage());
                    }
                    else
                    {
                       
                        MainPage = new NavigationPage(new MainPage());
                    }
                }
                else
                {
                    
                    MainPage = new NavigationPage(new MainPage());
                }

           



            /* userDetailslist = userDetailsDatabase.GetUserDetails("Select * from UserDetails").ToList();

             if (userDetailslist.Any())
             {
                 Preferences.Set("Active", 0);
                 MainPage = new NavigationPage(new HomePage());
             }
             else
             {
                 MainPage = new NavigationPage(new LoginPage());
             }*/
        }

        public static async Task<string> GetIPAddress()
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                try
                {
                    var client = new HttpClient();
                    var response = await client.GetAsync("https://api.myip.com");
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    var m = parsed["ip"];
                    return parsed["ip"].ToString();
                }
                catch
                {
                    return Dns.GetHostAddresses(Dns.GetHostName()).FirstOrDefault().ToString();
                }
            }
            else
            {
                return Dns.GetHostAddresses(Dns.GetHostName()).FirstOrDefault().ToString();
            }
        }

        public static string ConvertToBase64(Stream stream)
        {
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }

            string base64 = Convert.ToBase64String(bytes);
            return base64;
        }

        public static async Task<int> validatelogin(string regno/*, string password, string dob*/)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                try
                {
                    var client = new HttpClient();
                    string jsonData = JsonConvert.SerializeObject(new
                    {
                        key = Encrypt(access_key),
                        id = Encrypt(access_id),
                        RegNo = Encrypt(regno.Trim()),
                        /* NM = Encrypt(password.Trim()),
                         DOB = Encrypt(dob.Trim()),*/
                    });
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    //var responce = await client.PostAsync($"{LoginUrl}", content);
                    HttpResponseMessage responce = await client.PostAsync(baseurlx10api + $"ValidateApplicant2024", content);
                    if (!responce.IsSuccessStatusCode)
                    {
                        await Current.MainPage.DisplayAlert(AppName, $"[{responce.StatusCode}] Something went wrong\nPlease try again!", "Close");
                        return 500;
                    }
                    else
                    {
                        var result = await responce.Content.ReadAsStringAsync();
                        JObject parsed = JObject.Parse(result);
                        var message = parsed["message"];
                        string msg = message["message"].ToString();
                        string status = message["status"].ToString();
                        if (status.Equals("True"))
                        {
                            foreach (var pair in parsed)
                            {
                                if (pair.Key == "applicantDetails")
                                {
                                    var nodes = pair.Value;
                                    userDetailsDatabase.DeleteUserDetails();
                                    foreach (var node in nodes)
                                    {
                                        var item = new UserDetails();
                                        item.CandiName = Decrypt(node["CandiName"].ToString());
                                        item.RegNo = Decrypt(node["RegNo"].ToString());
                                        item.RegDt = Decrypt(node["RegDt"].ToString());
                                        item.XchName = Decrypt(node["XchName"].ToString());
                                        item.RenewalDt = Decrypt(node["RenewalDt"].ToString());
                                        item.Address = Decrypt(node["Address"].ToString());
                                        item.Category = Decrypt(node["Category"].ToString());
                                        item.DOB = Decrypt(node["DOB"].ToString());
                                        item.MaritalStatus = Decrypt(node["MaritalStatus"].ToString());
                                        item.MobileNo = Decrypt(node["MobileNo"].ToString());
                                        item.EmpStatus = Decrypt(node["EmpStatus"].ToString());
                                        item.eMailId = Decrypt(node["eMailId"].ToString());
                                        item.islogin = "Y";
                                        item.lastupdated = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                                        userDetailsDatabase.AddUserDetails(item);
                                    }
                                }
                                if (pair.Key == "nCODetails")
                                {
                                    var nodes = pair.Value;
                                    nCODetailsDatabase.DeleteNCODetails();
                                    foreach (var node in nodes)
                                    {
                                        var item = new NCODetails();
                                        item.NCODesc = Decrypt(node["NCODesc"].ToString());
                                        nCODetailsDatabase.AddNCODetails(item);
                                    }
                                }
                                if (pair.Key == "qualificationDetails")
                                {
                                    var nodes = pair.Value;
                                    qualificationDetailsDatabase.DeleteQualificationDetails();
                                    foreach (var node in nodes)
                                    {
                                        var item = new QualificationDetails();
                                        item.QualDesc = Decrypt(node["QualDesc"].ToString());
                                        qualificationDetailsDatabase.AddQualificationDetails(item);
                                    }
                                }
                                /*if (pair.Key == "vacancyDetails")
                                {
                                    var nodes = pair.Value;
                                    vacancyDetailsDatabase.DeleteVacancyDetails();
                                    foreach (var node in nodes)
                                    {
                                        var item = new VacancyDetails();
                                        item.InterviewDateTime = Decrypt(node["InterviewDateTime"].ToString());
                                        item.Post = Decrypt(node["Post"].ToString());
                                        vacancyDetailsDatabase.AddVacancyDetails(item);
                                    }
                                }*/
                                if (pair.Key == "employmentStatus")
                                {
                                    var nodes = pair.Value;
                                    employmentStatusDatabase.DeleteEmploymentStatus();
                                    foreach (var node in nodes)
                                    {
                                        var item = new EmploymentStatus();
                                        item.EmpStatCd = Decrypt(node["EmpStatCd"].ToString());
                                        item.EmpStatDesc = Decrypt(node["EmpStatDesc"].ToString());
                                        employmentStatusDatabase.AddEmploymentStatus(item);

                                    }
                                }
                                if (pair.Key == "subEmploymentStatus")
                                {
                                    var nodes = pair.Value;
                                    subEmploymentStatusDatabase.DeleteSubEmploymentStatus();
                                    foreach (var node in nodes)
                                    {
                                        var item = new SubEmploymentStatus();
                                        item.SubEmpStatCd = Decrypt(node["SubEmpStatCd"].ToString());
                                        item.SubEmpStatDesc = Decrypt(node["SubEmpStatDesc"].ToString());
                                        item.SSubEmpStatCd = Decrypt(node["SSubEmpStatCd"].ToString());
                                        item.SSubEmpStatDesc = Decrypt(node["SSubEmpStatDesc"].ToString());
                                        subEmploymentStatusDatabase.AddSubEmploymentStatus(item);
                                    }
                                }
                            }
                            return 200;
                        }
                        else
                        {
                            await Current.MainPage.DisplayAlert(AppName, msg, "Close");
                            return 300;
                        }
                    }
                }
                catch
                {
                    await Current.MainPage.DisplayAlert("Exception", "Something went wrong. Pls try after some time", "Close");
                    return 400;
                }
            }
            return 500;
        }

        public static async Task UpdateEmployementstatus(string RegNo, string EmpStatus, string SubEmpStatus, string SSubEmpStatus)
        {

            userDetailslist = userDetailsDatabase.GetUserDetails("Select * from UserDetails").ToList();

            string Registration_No = userDetailslist.ElementAt(0).RegNo;
            /* string CandiName = userDetailslist.ElementAt(0).CandiName.Substring(0, 3);
             string DOB = userDetailslist.ElementAt(0).DOB;*/

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                try
                {

                    string jsonData = JsonConvert.SerializeObject(new
                    {
                        RegNo = Encrypt(RegNo),
                        EmpStatus = Encrypt(EmpStatus),
                        SubEmpStatus = Encrypt(SubEmpStatus),
                        SSubEmpStatus = Encrypt(SSubEmpStatus),

                    });
                    var client = new HttpClient();
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    //var response = await client.PostAsync($"{UpdEmpStatUrl}", content);
                    HttpResponseMessage response = await client.PostAsync(baseurlx10api + $"UpdEmpStat", content);

                    if (!response.IsSuccessStatusCode)
                    {
                        await Current.MainPage.DisplayAlert(AppName, $"[{response.StatusCode}] Something went wrong\nPlease try again!", "Close");
                        return;
                    }
                    else
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        JObject parsed = JObject.Parse(result);
                        if (parsed.HasValues)
                        {
                            var message = parsed["message"];
                            string msg = message["message"].ToString();
                            string status = message["status"].ToString();
                            //int status = Convert.ToInt32(message["status"]);
                            if (status.Equals("True"))
                            {
                                await validatelogin(Registration_No/*, CandiName, DOB*/);
                                await Current.MainPage.DisplayAlert(AppName, "Employment Status Successfully Updated.", "Close");

                            }
                            else
                            {
                                await Current.MainPage.DisplayAlert(AppName, msg, "Close");
                                return;
                            }
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    await Current.MainPage.DisplayAlert("Operation Cancelled", "Could not connect to server. Please try again later", "Close");

                }

                catch (TimeoutException)
                {
                    await Current.MainPage.DisplayAlert("Request Timeout", "Could not connect to server. Please try again later", "Close");

                }
                catch (SocketException)
                {
                    await Current.MainPage.DisplayAlert("Socket Closed", "Could not connect to server. Please try again later", "Close");

                }
                catch
                {
                    await Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try after some time.", "Close");
                }
            }
            else
            {
                await Current.MainPage.DisplayAlert(AppName, nointernet, "Close");
            }
        }

        public static async Task<int> GetAllowanceDetails(string RegNo)
        {
            try
            {
                var client = new HttpClient();
                string jsonData = JsonConvert.SerializeObject(new
                {
                    RegNo = Encrypt(RegNo),
                });
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                //var responce = await client.PostAsync($"{AllowanceDetailsUrl}", content);
                HttpResponseMessage responce = await client.PostAsync(baseurlx10api + $"GetAllowanceDetails", content);

                if (!responce.IsSuccessStatusCode)
                {
                    await Current.MainPage.DisplayAlert(AppName, $"[{responce.StatusCode}] Something went wrong\nPlease try again!", "OK");
                    return 500;
                }
                else
                {
                    var result = await responce.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    var message = parsed["message"];
                    string msg = message["message"].ToString();
                    int status = Convert.ToInt32(message["status"]);
                    if (status == 200)
                    {
                        allowanceDetailsDatabase.DeleteAllowanceDetails();
                        allowanceTransactionsDatabase.DeleteAllowanceTransactions();
                        foreach (var pair in parsed)
                        {
                            if (pair.Key == "allowanceDetails")
                            {

                                var nodes = pair.Value;
                                foreach (var node in nodes)
                                {
                                    var item = new AllowanceDetails();
                                    item.AllowanceDesc = Decrypt(node["AllowanceDesc"].ToString());
                                    item.AllowanceId = Decrypt(node["AllowanceId"].ToString());
                                    item.AmtPaid = Decrypt(node["AmtPaid"].ToString());
                                    item.ApplicationNo = Decrypt(node["ApplicationNo"].ToString());
                                    item.ApplicationStatus = Decrypt(node["ApplicationStatus"].ToString());
                                    item.ApplicationStatusCd = Decrypt(node["ApplicationStatusCd"].ToString());
                                    item.EndDt = Decrypt(node["EndDt"].ToString());
                                    item.InstallmentAmt = Decrypt(node["InstallmentAmt"].ToString());
                                    item.InstallmentsPaid = Decrypt(node["InstallmentsPaid"].ToString());
                                    item.LastInstallmentPaidOn = Decrypt(node["LastInstallmentPaidOn"].ToString());
                                    item.RegNo = Decrypt(node["RegNo"].ToString());
                                    item.StartDt = Decrypt(node["StartDt"].ToString());
                                    item.TotInstallments = Decrypt(node["TotInstallments"].ToString());
                                    item.XchNm = Decrypt(node["XchNm"].ToString());
                                    item.ApplicationStatusTexcolor = Decrypt(node["ApplicationStatusTexcolor"].ToString());
                                    allowanceDetailsDatabase.AddAllowanceDetails(item);
                                }
                            }

                            if (pair.Key == "allowanceTransactions")
                            {

                                var nodes = pair.Value;
                                foreach (var node in nodes)
                                {
                                    var item = new AllowanceTransactions();
                                    item.AccountNo = Decrypt(node["AccountNo"].ToString());
                                    item.AllowanceId = Decrypt(node["AllowanceId"].ToString());
                                    item.AllowanceDesc = Decrypt(node["AllowanceDesc"].ToString());
                                    item.ApplicationNo = Decrypt(node["ApplicationNo"].ToString());
                                    item.Finyear = Decrypt(node["Finyear"].ToString());
                                    item.GrossAmount = Decrypt(node["GrossAmount"].ToString());
                                    item.IFSC = Decrypt(node["IFSC"].ToString());
                                    item.MonthYY = Decrypt(node["MonthYY"].ToString());
                                    item.PayeeCode = Decrypt(node["PayeeCode"].ToString());
                                    item.TreaBillStatus = Decrypt(node["TreaBillStatus"].ToString());
                                    item.XBillNo = Decrypt(node["XBillNo"].ToString());
                                    item.XTBillNo = Decrypt(node["XTBillNo"].ToString());
                                    item.YearNm = Decrypt(node["YearNm"].ToString());
                                    try
                                    {
                                        item.paymentdate = Decrypt(node["paymentdate"].ToString());
                                        item.YearNm = Decrypt(node["voucherno"].ToString());
                                    }
                                    catch { }
                                    allowanceTransactionsDatabase.AddAllowanceTransactions(item);
                                }
                            }
                        }
                        return 200;
                    }
                    else
                    {
                        return 300;
                    }
                }
            }
            catch
            {
                await Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try after some time!", "OK");
                return 500;
            }
        }

        public static async Task<int> GetAllVacancies()
        {
            try
            {
                var client = new HttpClient();
                string jsonData = JsonConvert.SerializeObject(new
                {
                    RegNo = Encrypt(""),
                });
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                //var responce = await client.PostAsync($"{AllVacanciesUrl}", content);
                HttpResponseMessage responce = await client.PostAsync(baseurlx10api + $"GetPublishedVacancies", content);

                if (!responce.IsSuccessStatusCode)
                {
                    await Current.MainPage.DisplayAlert(AppName, $"[{responce.StatusCode}] Something went wrong\nPlease try again!", "OK");
                    return 500;
                }
                else
                {
                    var result = await responce.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    var message = parsed["message"];
                    string msg = message["message"].ToString();
                    int status = Convert.ToInt32(message["status"]);
                    if (status == 200)
                    {

                        foreach (var pair in parsed)
                        {
                            if (pair.Key == "GetPublishedVacanciesRequestData")
                            {
                                allVacancyDetailsDatabase.DeleteAllVacancyDetails();
                                var nodes = pair.Value;
                                foreach (var node in nodes)
                                {
                                    var item = new AllVacancyDetails();
                                    item.RegNo = Decrypt(node["RegNo"].ToString());
                                    item.EmployerName = Decrypt(node["EmployerName"].ToString());
                                    item.vacdesc = Decrypt(node["vacdesc"].ToString());
                                    item.NoofPost = Decrypt(node["NoofPost"].ToString());
                                    item.VacDesgOfPost = Decrypt(node["VacDesgOfPost"].ToString());
                                    item.agebw = Decrypt(node["agebw"].ToString());
                                    item.intdttm = Decrypt(node["intdttm"].ToString());
                                    item.IntPlace = Decrypt(node["IntPlace"].ToString());
                                    item.PublishDt = Decrypt(node["PublishDt"].ToString());
                                    item.JobfairID = Decrypt(node["JobfairID"].ToString());
                                    item.EmpID = Decrypt(node["EmpID"].ToString());

                                    allVacancyDetailsDatabase.AddAllVacancyDetails(item);
                                }
                            }
                        }
                        return 200;
                    }
                    else
                    {
                        await Current.MainPage.DisplayAlert("Exception", msg, "OK");
                        return 300;
                    }
                }
            }
            catch
            {
                await Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try again after some time.", "OK");
                return 500;
            }
        }

        public static async Task<int> GetSponsorship(string RegNo)
        {
            try
            {
                var client = new HttpClient();
                string jsonData = JsonConvert.SerializeObject(new
                {
                    RegNo = Encrypt(RegNo),
                });
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                // var responce = await client.PostAsync($"{AllVacanciesUrl}", content);
                HttpResponseMessage responce = await client.PostAsync(baseurlx10api + $"GetPublishedVacancies", content);
                if (!responce.IsSuccessStatusCode)
                {
                    await Current.MainPage.DisplayAlert(AppName, $"[{responce.StatusCode}] Something went wrong\nPlease try again!", "OK");
                    return 500;
                }
                else
                {
                    var result = await responce.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    var message = parsed["message"];
                    string msg = message["message"].ToString();
                    int status = Convert.ToInt32(message["status"]);
                    if (status == 200)
                    {

                        foreach (var pair in parsed)
                        {
                            if (pair.Key == "GetPublishedVacanciesRequestData")
                            {
                                sponsorshipDetailsDatabase.DeleteSponsorshipDetails();
                                var nodes = pair.Value;
                                foreach (var node in nodes)
                                {
                                    var item = new SponsorshipDetails();
                                    item.RegNo = Decrypt(node["RegNo"].ToString());
                                    item.EmployerName = Decrypt(node["EmployerName"].ToString());
                                    item.vacdesc = Decrypt(node["vacdesc"].ToString());
                                    item.NoofPost = Decrypt(node["NoofPost"].ToString());
                                    item.VacDesgOfPost = Decrypt(node["VacDesgOfPost"].ToString());
                                    item.agebw = Decrypt(node["agebw"].ToString());
                                    item.intdttm = Decrypt(node["intdttm"].ToString());
                                    item.IntPlace = Decrypt(node["IntPlace"].ToString());
                                    item.PublishDt = Decrypt(node["PublishDt"].ToString());
                                    item.JobfairID = Decrypt(node["JobfairID"].ToString());
                                    item.EmpID = Decrypt(node["EmpID"].ToString());
                                    item.ApplyDt = Decrypt(node["ApplyDt"].ToString());
                                    item.Attend_Dt = Decrypt(node["Attend_Dt"].ToString());
                                    item.Selected_Dt = Decrypt(node["Selected_Dt"].ToString());
                                    item.joindt = Decrypt(node["joindt"].ToString());

                                    sponsorshipDetailsDatabase.AddSponsorshipDetails(item);
                                }
                            }
                        }
                        return 200;
                    }
                    else
                    {
                        await Current.MainPage.DisplayAlert("Exception", msg, "OK");
                        return 300;
                    }
                }
            }
            catch
            {
                await Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try again after some time.", "OK");
                return 500;
            }
        }

        public static async Task<int> GetPublishedjobfairs()
        {
            try
            {
                var client = new HttpClient();
                string jsonData = JsonConvert.SerializeObject(new
                {
                    JobfairID = Encrypt(""),
                });
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                //var responce = await client.PostAsync($"{GetPublishedjobfairsUrl}", content);
                HttpResponseMessage responce = await client.PostAsync(baseurlx10api + $"GetPublishedjobfairs", content);

                if (!responce.IsSuccessStatusCode)
                {
                    await Current.MainPage.DisplayAlert(AppName, $"[{responce.StatusCode}] Something went wrong\nPlease try again!", "OK");
                    return 500;
                }
                else
                {
                    var result = await responce.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    var message = parsed["message"];
                    string msg = message["message"].ToString();
                    int status = Convert.ToInt32(message["status"]);
                    if (status == 200)
                    {

                        foreach (var pair in parsed)
                        {
                            if (pair.Key == "GetPublishedVacanciesRequestData")
                            {

                                jobFairDetailsDatabase.DeleteJobFairDetails();
                                var nodes = pair.Value;
                                foreach (var node in nodes)
                                {
                                    var item = new JobFairDetails();
                                    item.Distt = Decrypt(node["Distt"].ToString());
                                    item.XchName = Decrypt(node["XchName"].ToString());
                                    item.JobfairDate = Decrypt(node["JobfairDate"].ToString());
                                    item.venue = Decrypt(node["venue"].ToString());
                                    item.remarks = Decrypt(node["remarks"].ToString());
                                    item.officer_incharge = Decrypt(node["officer_incharge"].ToString());
                                    item.officer_mobile = Decrypt(node["officer_mobile"].ToString());
                                    item.officer_email = Decrypt(node["officer_email"].ToString());
                                    item.PublishDt = Decrypt(node["PublishDt"].ToString());
                                    item.JobfairID = Decrypt(node["JobfairID"].ToString());

                                    jobFairDetailsDatabase.AddJobFairDetails(item);
                                }
                            }
                        }
                        return 200;
                    }
                    else
                    {
                        await Current.MainPage.DisplayAlert("Exception", msg, "OK");
                        return 300;
                    }
                }
            }
            catch
            {
                await Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try again after some time.", "OK");
                return 500;
            }
        }

        public static async Task<int> GetPublishedjobfairsEmployers(string JobfairID)
        {
            try
            {
                var client = new HttpClient();
                string jsonData = JsonConvert.SerializeObject(new
                {
                    JobfairID = Encrypt(JobfairID),
                });
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                //var responce = await client.PostAsync($"{GetPublishedjobfairsUrl}", content);
                HttpResponseMessage responce = await client.PostAsync(baseurlx10api + $"GetPublishedjobfairs", content);

                if (!responce.IsSuccessStatusCode)
                {
                    await Current.MainPage.DisplayAlert(AppName, $"[{responce.StatusCode}] Something went wrong\nPlease try again!", "OK");
                    return 500;
                }
                else
                {
                    var result = await responce.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    var message = parsed["message"];
                    string msg = message["message"].ToString();
                    int status = Convert.ToInt32(message["status"]);
                    if (status == 200)
                    {

                        foreach (var pair in parsed)
                        {
                            if (pair.Key == "GetPublishedVacanciesRequestData")
                            {
                                jobfairsEmployersDatabase.DeleteJobfairsEmployers();
                                var nodes = pair.Value;
                                foreach (var node in nodes)
                                {
                                    var item = new JobfairsEmployers();
                                    item.EmployerName = Decrypt(node["EmployerName"].ToString());
                                    item.EmpID = Decrypt(node["EmpID"].ToString());
                                    item.NatureofWork = Decrypt(node["NatureofWork"].ToString());
                                    item.EmployerAddress = Decrypt(node["EmployerAddress"].ToString());
                                    item.EmployereMail = Decrypt(node["EmployereMail"].ToString());
                                    item.EmployerTel = Decrypt(node["EmployerTel"].ToString());
                                    item.JobfairID = Decrypt(node["JobfairID"].ToString());

                                    jobfairsEmployersDatabase.AddJobfairsEmployers(item);
                                }
                            }
                        }
                        return 200;
                    }
                    else
                    {
                        await Current.MainPage.DisplayAlert("Exception", msg, "OK");
                        return 300;
                    }
                }
            }
            catch
            {
                await Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try again after some time.", "OK");
                return 500;
            }
        }

        public static async Task<int> GetJobFairEmployersVacancies(string JobfairID, string EmpID)
        {
            try
            {
                var client = new HttpClient();
                string jsonData = JsonConvert.SerializeObject(new
                {
                    JobfairID = Encrypt(JobfairID),
                    EmpID = Encrypt(EmpID),
                });
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                //var responce = await client.PostAsync($"{GetPublishedJOBFAIRVacanciesUrl}", content);
                HttpResponseMessage responce = await client.PostAsync(baseurlx10api + $"GetPublishedJOBFAIRVacancies", content);

                
                if (!responce.IsSuccessStatusCode)
                {
                    await Current.MainPage.DisplayAlert(AppName, $"[{responce.StatusCode}] Something went wrong\nPlease try again!", "OK");
                    return 500;
                }
                else
                {
                    var result = await responce.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    var message = parsed["message"];
                    string msg = message["message"].ToString();
                    int status = Convert.ToInt32(message["status"]);
                    if (status == 200)
                    {

                        foreach (var pair in parsed)
                        {
                            if (pair.Key == "GetPublishedVacanciesRequestData")
                            {
                                jobFairEmployersVacanciesDatabase.DeleteJobFairEmployersVacancies();
                                var nodes = pair.Value;
                                foreach (var node in nodes)
                                {
                                    var item = new JobFairEmployersVacancies();
                                    item.EmployerName = Decrypt(node["EmployerName"].ToString());
                                    item.vacdesc = Decrypt(node["vacdesc"].ToString());
                                    item.NoofPost = Decrypt(node["NoofPost"].ToString());
                                    item.VacDesgOfPost = Decrypt(node["VacDesgOfPost"].ToString());
                                    item.agebw = Decrypt(node["agebw"].ToString());
                                    item.intdttm = Decrypt(node["intdttm"].ToString());
                                    item.IntPlace = Decrypt(node["IntPlace"].ToString());
                                    item.PublishDt = Decrypt(node["PublishDt"].ToString());
                                    item.JobfairID = Decrypt(node["JobfairID"].ToString());
                                    item.EmpID = Decrypt(node["EmpID"].ToString());
                                    item.XchName = Decrypt(node["XchName"].ToString());
                                    item.NcoCd = Decrypt(node["NcoCd"].ToString());

                                    item.ApplyDt = Decrypt(node["ApplyDt"].ToString());
                                    item.Attend_Dt = Decrypt(node["Attend_Dt"].ToString());
                                    item.Selected_Dt = Decrypt(node["Selected_Dt"].ToString());
                                    item.joindt = Decrypt(node["joindt"].ToString());
                                    /* item.ApplyDt = DateTime.Now.ToString();
                                     item.Attend_Dt = DateTime.Now.ToString();
                                     item.Selected_Dt = DateTime.Now.ToString();
                                     item.joindt = DateTime.Now.ToString();*/


                                    jobFairEmployersVacanciesDatabase.AddJobFairEmployersVacancies(item);
                                }
                            }
                        }
                        return 200;
                    }
                    else
                    {
                        await Current.MainPage.DisplayAlert("Exception", msg, "OK");
                        return 300;
                    }
                }
            }
            catch
            {
                await Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try again after some time.", "OK");
                return 500;
            }
        }

        public async static void update()
        {
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    double installedVersionNumber = double.Parse(VersionTracking.CurrentVersion);
                    double latestVersionNumber = installedVersionNumber;

                    var client = new HttpClient();
                    string CurrentPlateform = "A";
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        CurrentPlateform = "I";
                    }

                    var responce = await client.GetAsync(GetAppVersionDetailsUrl + $"&Platform={CurrentPlateform}&packageid={AppInfo.PackageName}");
                    var MyJson = await responce.Content.ReadAsStringAsync();

                    JObject parsed = JObject.Parse(MyJson);
                    var ServiceStatusCode = parsed["message"]["status"].ToString();
                    if (ServiceStatusCode == "200")
                    {
                        if (MyJson.Contains("Mandatory"))
                        {
                            latestVersionNumber = double.Parse(parsed["appVersionDetails"][0]["VersionNumber"].ToString());
                            if (installedVersionNumber < latestVersionNumber)
                            {
                                if (parsed["appVersionDetails"][0]["Mandatory"].ToString() == "Y")
                                {
                                    await Current.MainPage.DisplayAlert("New Version", $"There is a new version (v{parsed["appVersionDetails"][0]["VersionNumber"].ToString()}) of this app available.\nWhatsNew: {parsed["appVersionDetails"][0]["WhatsNew"].ToString()}", "Update");
                                    await Launcher.OpenAsync(parsed["appVersionDetails"][0]["Url"].ToString());
                                    return;
                                }
                                else
                                {
                                    var updat = await Current.MainPage.DisplayAlert("New Version", $"There is a new version (v{parsed["appVersionDetails"][0]["VersionNumber"].ToString()}) of this app available.\nWhatsNew: {parsed["appVersionDetails"][0]["WhatsNew"].ToString()}\nWould you like to update now?", "Yes", "No");
                                    if (updat)
                                    {
                                        await Launcher.OpenAsync(parsed["appVersionDetails"][0]["Url"].ToString());
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }


        public static bool isAlphabetonly(string strtocheck)
        {
            Regex rg = new Regex(@"^[a-zA-Z\s]+$");
            return rg.IsMatch(strtocheck);
        }
        public static bool isAlphaNumeric(string strToCheck)
        {
            Regex rg = new Regex(@"^[a-zA-Z0-9\s,.@()/-]*$");
            return rg.IsMatch(strToCheck);
        }
        public static bool isNumeric(string strToCheck)
        {
            Regex rg = new Regex("^[0-9]+$");
            return rg.IsMatch(strToCheck);
        }

        public static bool isDecimal(string strToCheck)
        {
            Regex rg = new Regex("^\\d+(\\.\\d{1,2})?$");
            return rg.IsMatch(strToCheck);
        }

        public static bool ValidateEmail(string strToCheck)
        {

            Regex rg = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            return rg.IsMatch(strToCheck);
        }

        public static bool isvalidpassword(string strToCheck)
        {
            Regex rg = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
            return rg.IsMatch(strToCheck);
        }

        public static async Task<string[]> UploadPhoto(Image image)
        {

            try
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await Application.Current.MainPage.DisplayAlert(AppName, "No Camera Found", close);
                    return new string[3] { null, null, null };
                }
                Preferences.Set("INeedCamera", "Y");
                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    Name = $"Camera{DateTime.Now:yyyyMMddHHmmss}.jpg",
                    SaveToAlbum = true,
                    CompressionQuality = 70,
                    CustomPhotoSize = 50,
                    PhotoSize = PhotoSize.Custom,
                    MaxWidthHeight = 800,
                    DefaultCamera = CameraDevice.Rear,
                    SaveMetaData = true,

                });
                if (file == null)
                {
                    return new string[0];
                }
                else
                {
                    imagememroystreamsource = file;
                    var MyArray = ConvertImageToByteArray(file.Path);
                    //     var size = DependencyService.Get<IImageManager>().GetDimensionsFrom(MyArray);
                    string Extension = "";

                    PhotoBase64 = Convert.ToBase64String(ConvertImageToByteArray(file.Path));
                    Extension = (file.Path).Substring((file.Path).Length - 3);
                    image.Source = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();

                        return stream;
                    });
                    return new string[3] { PhotoBase64, Extension, "Camera" };
                }

            }
            catch (Exception ey)
            {
                await Application.Current.MainPage.DisplayAlert(AppName, ey.Message, close);
                return new string[0];
            }
        }
        public static byte[] ConvertImageToByteArray(string imagePath)
        {
            byte[] imageByteArray = null;
            FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            using (BinaryReader reader = new BinaryReader(fileStream))
            {
                imageByteArray = new byte[reader.BaseStream.Length];
                for (int i = 0; i < reader.BaseStream.Length; i++)
                    imageByteArray[i] = reader.ReadByte();
            }
            var MyArray = imageByteArray;
            var mydataofarray = Encoding.UTF8.GetString(MyArray);
            return imageByteArray;
        }

        protected override void OnStart()
        {
        }
        protected override void OnSleep()
        {
        }
        protected override void OnResume()
        {
        }
       /* public static string AESEncryption(string inputText)
        {
            byte[] encStr = EncryptStringToBytes_Aes(inputText, keybytes, iv);
            string encString = Convert.ToBase64String(encStr);
            return encString;
        }
        public static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;
        }
        public static string DecryptStringAES(string cipherText)
        {
            var encrypted = Convert.FromBase64String(cipherText);
            var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
            return string.Format(decriptedFromJavascript);
        }
        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            string plaintext = null;
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;
                rijAlg.Key = key;
                rijAlg.IV = iv;
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
                try
                {
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new System.IO.StreamReader(csDecrypt))
                            {
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }
            return plaintext;
        }*/
        public static string Encrypt(string plainText)
        {
            var plainBytes = Encoding.UTF8.GetBytes(plainText);

            try
            {
                return Convert.ToBase64String(Encrypt(plainBytes, GetRijndaelManaged(SecretKey)));
            }
            catch
            {
                return "";
            }
        }
        public static string Decrypt(string encryptedText)
        {
            try
            {
                var encryptedBytes = Convert.FromBase64String(encryptedText);
                return Encoding.UTF8.GetString(Decrypt(encryptedBytes, GetRijndaelManaged(SecretKey)));
            }
            catch
            {
                return "";
            }
        }


        public static byte[] Encrypt(byte[] plainBytes, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateEncryptor().TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        }

        public static byte[] Decrypt(byte[] encryptedData, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
        }

        public static RijndaelManaged GetRijndaelManaged(string secretKey)
        {
            var keyBytes = new byte[16];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));
            return new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 128,
                BlockSize = 128,
                Key = keyBytes,
                IV = keyBytes
            };
        }


    }
}


