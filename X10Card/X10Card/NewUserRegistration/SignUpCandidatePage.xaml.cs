using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using X10Card.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace X10Card.NewUserRegistration
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpCandidatePage : ContentPage
    {
        public SignUpCandidatePage()
        {
            InitializeComponent();
            lbl_navigation_header.Text = App.AppName;

        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new LoginPage());
        }

        private async void btn_submit_Clicked(object sender, EventArgs e)
        {
           
            if (await checkvalidationpassword())
            {
                var service = new UserRegistrationApi();
                Loading_activity.IsVisible = true;
                int response_signup = await service.Signup(entry_mobileno.Text, entry_email.Text, entry_password.Text, entry_Confirmpassword.Text);
                if (response_signup == 200)
                {
                    Application.Current.MainPage = new NavigationPage(new LoginPage());
                }
                Loading_activity.IsVisible = false;
            }

        }


        async Task<bool> checkvalidationpassword()
        {
            int moblen;

            try
            {

                if(string.IsNullOrEmpty(entry_mobileno.Text) & string.IsNullOrEmpty(entry_email.Text))
                {
                    await DisplayAlert(App.AppName, "Enter Mobile Number/Email-Id or Both", "Close");
                    return false;
                }
                if (!string.IsNullOrEmpty(entry_mobileno.Text))
                {
                    if (!App.isNumeric(entry_mobileno.Text.ToString().Trim()))
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
                }

                if (!string.IsNullOrEmpty(entry_email.Text))
                {
                    if (!App.ValidateEmail(entry_email.Text))
                    {
                        await DisplayAlert(App.AppName, "Enter Valid Email-Id", "Close");
                        return false;
                    }
                }

                if (!string.IsNullOrEmpty(entry_password.Text))
                {
                    moblen = entry_password.Text.Length;
                    if (moblen == 0)
                    {
                        await DisplayAlert(App.AppName, "Enter Password", "Close");
                        return false;
                    }
                    if (!App.isvalidpassword(entry_password.Text))
                    {
                        await DisplayAlert(App.AppName, "Password must contain an uppercase, lowercase, special character (@,$,!,%,*,?,&), number & should be between 8 to 15 digits.", "Close");
                        return false;
                    }
                    if (string.IsNullOrEmpty(entry_Confirmpassword.Text))
                    {
                        await DisplayAlert(App.AppName, "Enter Confirm Password", "Close");
                        return false;
                    }
                    if (!entry_Confirmpassword.Text.Trim().Equals(entry_Confirmpassword.Text.Trim()))
                    {
                        await DisplayAlert(App.AppName, "Password and Confirm Password doesn't match.", "Close");
                        return false;
                    }
                }
                else
                {
                    entry_password.Focus();
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

       

       /* private void rb_mobile_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (rb_mobile.IsChecked)
            {
                stack_mobile.IsVisible = true;
                stack_email.IsVisible = false;
            }
            else
            {
                stack_mobile.IsVisible = false;
                stack_email.IsVisible = true;

            }
        }*/

        private void btn_popupotpcancel_Clicked(object sender, EventArgs e)
        {
            popupotp.IsVisible = false;

        }

        private void btn_popupotpsubmit_Clicked(object sender, EventArgs e)
        {
            popupotp.IsVisible = false;

        }
    }
}