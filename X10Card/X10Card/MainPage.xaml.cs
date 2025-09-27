using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace X10Card
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void btn_submit_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage=new NavigationPage(new LoginPage());
            //Application.Current.MainPage=new NavigationPage(new SSOLoginPage());
        }
    }
}