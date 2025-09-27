using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using X10Card.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace X10Card
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPasswordPage : ContentPage
    {
        string RecoveryId;
        public ForgotPasswordPage()
        {
            InitializeComponent();
            lbl_navigation_header.Text = App.AppName;
        }

        private async void btn_submit_Clicked(object sender, EventArgs e)
        {
            if (await checkvalidationuserid())
            {
                Loading_activity.IsVisible = true;
                var service = new UserRegistrationApi();
                int response_forgotpassword = await service.ForgotPasslink(entry_userid.Text);
                if (response_forgotpassword == 200)
                {
                    Loading_activity.IsVisible = false;
                    entry_userid.IsReadOnly = true;
                    stack_otp.IsVisible = true;
                    btn_submit.IsVisible = false;
                    btn_submitotp.IsVisible = true;
                }
                Loading_activity.IsVisible = false;
            }
        }
        private  void getRecoveryIds()
        {
           
                RecoveryId = Preferences.Get("RecoveryId", "");
            
        }


        private async void btn_submitotp_Clicked(object sender, EventArgs e)
        {
            if (await checkvalidationotp())
            {
                getRecoveryIds();
                Loading_activity.IsVisible = true;
                var service = new UserRegistrationApi();
                int response_VerifyForgotPassCd = await service.VerifyForgotPassCd(entry_userid.Text, RecoveryId, entry_otp.Text);
                if (response_VerifyForgotPassCd == 200)
                {
                    Loading_activity.IsVisible = false;
                    stack_otp.IsVisible = false;
                    btn_submit.IsVisible = false;
                    btn_submitotp.IsVisible = false;
                    stack_password.IsVisible = true;
                    btn_submitpassword.IsVisible = true;
                }
                Loading_activity.IsVisible = false;
            }
        }

        private async void btn_submitpassword_Clicked(object sender, EventArgs e)
        {
            if (await checkvalidationpassword())
            {
                getRecoveryIds();
                Loading_activity.IsVisible = true;
                var service = new UserRegistrationApi();
                int response_ChangePassword = await service.ChangePassword(entry_userid.Text, RecoveryId,entry_password.Text, entry_confirmpassword.Text);
                if (response_ChangePassword == 200)
                {
                    Loading_activity.IsVisible = false;
                    Application.Current.MainPage = new NavigationPage(new LoginPage());
                }
                Loading_activity.IsVisible = false;

            }
        }

        async Task<bool> checkvalidationuserid()
        {
            int moblen;

            try
            {

                if (!string.IsNullOrEmpty(entry_userid.Text))
                {
                    moblen = entry_userid.Text.Length;
                    if (moblen == 0)
                    {
                        await DisplayAlert(App.AppName, "Enter Login Id", App.close);
                        return false;
                    }

                    if (!App.isAlphaNumeric(entry_userid.Text))
                    {
                        await DisplayAlert(App.AppName, "Only Alpha Numeric Characters Are Allowed In Login Id", App.close);
                        return false;
                    }

                }
                else
                {
                    entry_otp.Focus();
                    await DisplayAlert(App.AppName, "Enter Login Id", App.close);
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

        async Task<bool> checkvalidationotp()
        {
            int moblen;

            try
            {

                if (!string.IsNullOrEmpty(entry_otp.Text))
                {
                    moblen = entry_otp.Text.Length;
                    if (moblen == 0)
                    {
                        await DisplayAlert(App.AppName, "Enter OTP", App.close);
                        return false;
                    }
                    if (moblen < 6)
                    {
                        await DisplayAlert(App.AppName, "Enter 6 digit OTP", App.close);
                        return false;
                    }
                    if (!App.isNumeric(entry_otp.Text))
                    {
                        await DisplayAlert(App.AppName, "Only Numeric Characters Are Allowed In OTP", App.close);
                        return false;
                    }

                }
                else
                {
                    entry_otp.Focus();
                    await DisplayAlert(App.AppName, "Enter OTP", App.close);
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

        async Task<bool> checkvalidationpassword()
        {
            int moblen;

            try
            {

                if (!string.IsNullOrEmpty(entry_password.Text))
                {
                    moblen = entry_password.Text.Length;
                    if (moblen == 0)
                    {
                        await DisplayAlert(App.AppName, "Enter Password", App.close);
                        return false;
                    }
                    if (!App.isvalidpassword(entry_password.Text))
                    {
                        await DisplayAlert(App.AppName, "Password must contain an uppercase, lowercase, special character(@,$,!,%,*,?,&), number & should be between 8 to 15 digits.", App.close);
                        return false;
                    }
                    if (string.IsNullOrEmpty(entry_confirmpassword.Text))
                    {
                        await DisplayAlert(App.AppName, "Enter Confirm Password", App.close);
                        return false;
                    }
                    if (!entry_password.Text.Trim().Equals(entry_confirmpassword.Text.Trim()))
                    {
                        await DisplayAlert(App.AppName, "Password and Confirm Password doesn't match.", App.close);
                        return false;
                    }
                }
                else
                {
                    entry_password.Focus();
                    await DisplayAlert(App.AppName, "Enter Password", App.close);
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

      
    }
}