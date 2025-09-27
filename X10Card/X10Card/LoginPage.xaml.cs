using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X10Card.Models;
using X10Card.NewUserRegistration;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace X10Card
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        string activationmobileno;
        string UserID, RegNo;
        string loginusing = "M";
        InvalidLoginAttemptDatabase invalidLoginAttemptDatabase = new InvalidLoginAttemptDatabase();
        List<InvalidLoginAttempt> invalidLoginAttemptslist;
        public LoginPage()
        {
            InitializeComponent();
            entry_userid.Placeholder = "Enter Mobile No.";
            entry_userid.MaxLength = 10;
            entry_userid.Keyboard = Keyboard.Numeric;

            Device.BeginInvokeOnMainThread(() =>
            {
                App.update();
            });
        }

        private void rd_Mobile_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (rd_Mobile.IsChecked)
            {
                loginusing = "M";
                entry_userid.Text = string.Empty;
                entry_userid.Placeholder = "Enter Mobile No.";
                entry_userid.MaxLength = 10;
                entry_userid.Keyboard = Keyboard.Numeric;
            }
            else
            {
                loginusing = "E";
                entry_userid.Text = string.Empty;
                entry_userid.Placeholder = "Enter Email Id.";
                entry_userid.MaxLength = 50;
                entry_userid.Keyboard = Keyboard.Text;
            }
        }

        private async void btn_submit_Clicked(object sender, EventArgs e)
        {
            if (await checkvalidation())
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {

                    Loading_activity.IsVisible = true;

                    var service = new UserRegistrationApi();
                    int response_login = await service.Login(entry_userid.Text, entry_password.Text);
                    if (response_login == 200)
                    {

                        Preferences.Set("LoggedIn", "Y");
                        getvalues();

                        Application.Current.MainPage = new NavigationPage(new PostLoginDashboardPage());
                    }
                    else if (response_login == 700)
                    {
                        entry_userid.Text = string.Empty;
                        entry_password.Text = string.Empty;
                        getvalues();
                        lbl_popupotpheader.Text = "Enter OTP To Activate Account\nMobile No. : " + entry_userid.Text;
                        popupotp.IsVisible = true;
                    }
                    else if (response_login == 600)
                    {

                        entry_userid.Text = string.Empty;
                        entry_password.Text = string.Empty;
                        //SecureStorage.RemoveAll();
                        Preferences.Clear();
                    }
                    else
                    {
                        entry_userid.Text = string.Empty;
                        entry_password.Text = string.Empty;
                    }

                    Loading_activity.IsVisible = false;

                }
                else
                {
                    await DisplayAlert(App.AppName, App.nointernet, "Close");
                }
            }
        }

        bool checkaccountlockedornot()
        {
            string query = $"Select * from InvalidLoginAttempt " +
                           $" where userid='{entry_userid.Text}' " +
                           $" and AttemptedDateTime > '{DateTime.Now.AddMinutes(-15).ToString("yyyy-MM-dd HH:mm:ss")}' ";

            invalidLoginAttemptslist = invalidLoginAttemptDatabase.GetInvalidLoginAttempt(query).ToList();
            if (invalidLoginAttemptslist.Count >= 3)
            {
                DateTime attemptedatetime = DateTime.Parse(invalidLoginAttemptslist.Last().AttemptedDateTime);
                DateTime currentdatetime = DateTime.Now;
                TimeSpan span = currentdatetime.Subtract(attemptedatetime);
                var minutes = span.Minutes * 60;
                var second = span.Seconds;
                var totaltime = minutes + second;
                if (totaltime < 900)
                {
                    var remainedLockedInSeconds = 900 - totaltime;
                    TimeSpan span1 = TimeSpan.FromSeconds(remainedLockedInSeconds);
                    DisplayAlert(App.AppName, "Due to multiple unsuccessfull login attempts, your account is locked. " +
                        //"Kindly login after " + span1 + " minutes", "Close");
                        "Kindly login after 15 minutes", "Close");
                    entry_userid.Text = "";
                    entry_password.Text = "";
                    return true;
                }
                else
                {
                    invalidLoginAttemptDatabase.DeleteInvalidLoginAttempt();
                    return false;
                }

            }
            return false;
        }

        private void getvalues()
        {
            UserID = Preferences.Get("UserID", "");
            RegNo = Preferences.Get("RegNo", "");
        }

        private void btn_popupotpcancel_Clicked(object sender, EventArgs e)
        {
            popupotp.IsVisible = false;

        }

        private async void btn_popupotpsubmit_Clicked(object sender, EventArgs e)
        {
            if (await checkvalidationactivate())
            {

                Loading_activity.IsVisible = true;
                var service = new UserRegistrationApi();
                int resposnse_activateotp = await service.ActivateOTP(UserID, entry_otp.Text);
                if (resposnse_activateotp == 200)
                {
                    Loading_activity.IsVisible = false;
                    popupotp.IsVisible = false;
                }
                Loading_activity.IsVisible = false;

            }

        }

        async Task<bool> checkvalidationactivate()
        {
            try
            {

                if (string.IsNullOrEmpty(entry_otp.Text))
                {
                    await DisplayAlert(App.AppName, "Enter OTP", "Close");
                    return false;
                }
                else if (!App.isNumeric(entry_otp.Text.ToString().Trim()))
                {
                    await DisplayAlert(App.AppName, "Only Numeric characters are allowed in OTP", "Close");
                    return false;
                }
                if (entry_otp.Text.Length < 6)
                {
                    await DisplayAlert(App.AppName, "Enter 6 digit OTP", App.close);
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


        private void TapGestureRecognizer_NewUserTapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SignUpCandidatePage());
        }

        private void TapGestureRecognizer_ForgotPasswordTapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ForgotPasswordPage());
        }


        private async void btn_popupresendotp_Clicked(object sender, EventArgs e)
        {
            string mobileno = entry_userid.Text;
            Loading_activity.IsVisible = true;
            var service = new UserRegistrationApi();
            int response_resendotp = await service.ResendOTP(mobileno);
            Loading_activity.IsVisible = false;
        }



        async Task<bool> checkvalidation()
        {
            try
            {
                if (string.IsNullOrEmpty(entry_userid.Text))
                {
                    await DisplayAlert(App.AppName, "Enter User Id", "Close");
                    return false;
                }
                if (loginusing.Equals("M"))
                {
                    if (App.isNumeric(entry_userid.Text))
                    {
                        if (entry_userid.Text.Length < 10 || entry_userid.Text.Length > 10)
                        {
                            await DisplayAlert(App.AppName, "Enter 10 digit Mobile No.", "Close");
                            entry_userid.Text = string.Empty;
                            return false;
                        }

                        if (long.Parse(entry_userid.Text) < 6000000000)
                        {
                            await DisplayAlert(App.AppName, "Mobile No. must start from 6/7/8/9", "Close");
                            entry_userid.Text = string.Empty;
                            return false;
                        }
                    }
                    else
                    {
                        await DisplayAlert(App.AppName, "Only numeric characters are allowed in Mobile No.", "Close");
                        entry_userid.Text = string.Empty;
                        return false;
                    }
                }
                else if (loginusing.Equals("E"))
                {
                    if (!App.isAlphaNumeric(entry_userid.Text))
                    {
                        await DisplayAlert(App.AppName, "Only Alphanumeric characters are allowed in email id", "Close");
                        entry_userid.Text = string.Empty;
                        return false;
                    }

                    if (!App.ValidateEmail(entry_userid.Text))
                    {
                        await DisplayAlert(App.AppName, "Enter Valid Email", "Close");
                        entry_userid.Text = string.Empty;
                        return false;
                    }
                }

                if (string.IsNullOrEmpty(entry_password.Text))
                {
                    await DisplayAlert(App.AppName, "Enter Password", "Close");
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

        private void btn_register_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new PersonalDetailsPage());
        }

        private void btn_signup_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new SignUpCandidatePage());
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            stack_email.IsVisible = false;
            rb_mobile.IsChecked = true;
            lbl_popupactivationlinkheader.Text = "Resend OTP";
            entry_activationmobile.Text = string.Empty;
            entry_activationlinkemail.Text = string.Empty;
            popupactivationlink.IsVisible = true;
        }

        private void rb_mobile_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (rb_mobile.IsChecked)
            {
                stack_mobile.IsVisible = true;
                stack_email.IsVisible = false;
                lbl_popupactivationlinkheader.Text = "Resend OTP";

            }
            else
            {
                stack_mobile.IsVisible = false;
                stack_email.IsVisible = true;
                lbl_popupactivationlinkheader.Text = "Resend Activation link";
            }
        }

        private void btn_popupactivationlinkcancel_Clicked(object sender, EventArgs e)
        {
            popupactivationlink.IsVisible = false;

        }

        private async void btn_popupactivationlinksubmit_Clicked(object sender, EventArgs e)
        {
            string id;


            if (rb_mobile.IsChecked)
            {
                if (await checkvalidationpopupactivationlink())
                {
                    id = entry_activationmobile.Text;
                    activationmobileno = entry_activationmobile.Text;
                    popupactivationlink.IsVisible = false;
                    lbl_popupotpheader.Text = "Mobile No.: " + id;
                    Loading_activity.IsVisible = true;
                    var service = new UserRegistrationApi();
                    int response_resendotp = await service.ResendOTP(activationmobileno);
                    if (response_resendotp == 200)
                    {
                        Loading_activity.IsVisible = false;

                        popupotpactivation.IsVisible = true;
                    }
                    Loading_activity.IsVisible = false;

                }
            }
            else
            {
                if (await checkvalidationpopupactivationlink())
                {
                    id = entry_activationlinkemail.Text;
                    popupactivationlink.IsVisible = false;
                    popupotpactivation.IsVisible = false;

                    Loading_activity.IsVisible = true;
                    var service = new UserRegistrationApi();
                    int response_resendotp = await service.ReSendActivationLink(entry_activationlinkemail.Text);
                    if (response_resendotp == 200)
                    {
                        Loading_activity.IsVisible = false;
                    }
                    Loading_activity.IsVisible = false;


                }
            }


            //service to send otp/link pending

        }

        async Task<bool> checkvalidationpopupactivationlink()
        {
            try
            {
                if (rb_mobile.IsChecked)
                {
                    if (string.IsNullOrEmpty(entry_activationmobile.Text))
                    {
                        await DisplayAlert(App.AppName, "Enter Mobile No.", "Close");
                        return false;
                    }
                    else if (!App.isNumeric(entry_activationmobile.Text.ToString().Trim()))
                    {
                        await DisplayAlert(App.AppName, "Only Numeric characters are allowed in Mobile No.", "Close");
                        return false;
                    }
                    if (entry_activationmobile.Text.Length < 10)
                    {
                        await DisplayAlert(App.AppName, "Enter 10 digit mobile no.", App.close);
                        return false;
                    }
                    if (long.Parse(entry_activationmobile.Text) < 6000000000)
                    {
                        await DisplayAlert(App.AppName, "Invalid Mobile No.\nMobile should start with 6,7,8,9.", App.close);
                        return false;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(entry_activationlinkemail.Text))
                    {
                        await DisplayAlert(App.AppName, "Enter Email Id.", "Close");
                        return false;
                    }
                    if (!App.ValidateEmail(entry_activationlinkemail.Text))
                    {
                        await DisplayAlert(App.AppName, "Enter Valid Email-Id", "Close");
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

        async Task<bool> checkvalidationpopupactivationlinkOTP()
        {
            try
            {

                if (string.IsNullOrEmpty(entry_popupotpactivation.Text))
                {
                    await DisplayAlert(App.AppName, "Enter OTP", "Close");
                    return false;
                }
                else if (!App.isNumeric(entry_popupotpactivation.Text.ToString().Trim()))
                {
                    await DisplayAlert(App.AppName, "Only Numeric characters are allowed in OTP", "Close");
                    return false;
                }
                if (entry_popupotpactivation.Text.Length < 6)
                {
                    await DisplayAlert(App.AppName, "Enter 6 digit OTP", App.close);
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

        private void btn_popupotpactivationcancel_Clicked(object sender, EventArgs e)
        {
            popupotpactivation.IsVisible = false;
        }

        private async void btn_popupotpactivationsubmit_Clicked(object sender, EventArgs e)
        {
            if (await checkvalidationpopupactivationlinkOTP())
            {
                //check first parameter to be sent --service used accepts user id instead of mobile
                Loading_activity.IsVisible = true;
                var service = new UserRegistrationApi();
                int resposnse_activateotp = await service.ActivateOTP(activationmobileno, entry_popupotpactivation.Text);
                if (resposnse_activateotp == 200)
                {
                    Loading_activity.IsVisible = false;
                    popupotpactivation.IsVisible = false;
                }
                Loading_activity.IsVisible = false;
                popupotpactivation.IsVisible = false;



            }
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            string url = "https://eemis.hp.nic.in/Home/ViewFileReg";
            Launcher.OpenAsync(url);
            // Navigation.PushAsync(new LoadWebViewPage(url));
        }

        private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            string url = "https://eemis.hp.nic.in/Home/ViewFileRenew";
            Launcher.OpenAsync(url);
            // Navigation.PushAsync(new LoadWebViewPage(url));
        }



        private async void btn_popupotpactivationresendotp_Clicked(object sender, EventArgs e)
        {

            Loading_activity.IsVisible = true;
            var service = new UserRegistrationApi();
            int response_resendotp = await service.ResendOTP(activationmobileno);
            Loading_activity.IsVisible = false;
        }


    }
}