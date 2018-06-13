using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MLogger_UWP.Views {
    public sealed partial class MainPage : Page {

        ApplicationDataContainer AppData = ApplicationData.Current.LocalSettings;
        
        public MainPage() {
            SetUI();
            InitializeComponent();
            InitAppData();
            // LoginButton.Focus(FocusState.Programmatic);
            UrlTextBox.Text = AppData.Values["apiurl"].ToString();
            if (UsernameTextBox.Text != "" && PasswordTextBox.Password != "") {
                Login();
            }
        }

        void SetUI() {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = Windows.UI.Color.FromArgb(255, 255, 64, 129);
            titleBar.ForegroundColor = Windows.UI.Colors.White;
            titleBar.ButtonBackgroundColor = Windows.UI.Color.FromArgb(255, 255, 64, 129);
            titleBar.ButtonForegroundColor = Windows.UI.Colors.White;
            titleBar.ButtonPressedBackgroundColor = Windows.UI.Color.FromArgb(125, 242, 61, 122);
            titleBar.ButtonPressedForegroundColor = Windows.UI.Colors.White;
            titleBar.ButtonInactiveBackgroundColor = Windows.UI.Color.FromArgb(255, 255, 64, 129);
            titleBar.ButtonInactiveForegroundColor = Windows.UI.Colors.LightGray;
            titleBar.InactiveBackgroundColor = Windows.UI.Color.FromArgb(255, 255, 64, 129);
            titleBar.InactiveForegroundColor = Windows.UI.Colors.LightGray;
            titleBar.ButtonHoverBackgroundColor = Windows.UI.Color.FromArgb(125, 242, 61, 122);
            titleBar.ButtonHoverForegroundColor = Windows.UI.Colors.White;
        }

        void InitAppData() {

            try {
                UrlTextBox.Text = AppData.Values["apiurl"].ToString();
            } catch (System.NullReferenceException) {
                AppData.Values["apiurl"] = "";
                UrlTextBox.Text = AppData.Values["apiurl"].ToString();
            }

            try {
                UsernameTextBox.Text = AppData.Values["usrname"].ToString();
            } catch (System.NullReferenceException) {
                AppData.Values["usrname"] = "";
                UsernameTextBox.Text = AppData.Values["usrname"].ToString();
            }

            try {
                PasswordTextBox.Password = AppData.Values["usrname"].ToString();
            } catch (System.NullReferenceException) {
                AppData.Values["passwd"] = "";
                PasswordTextBox.Password = AppData.Values["usrname"].ToString();
            }

        }

        void Save() {
            AppData.Values["apiurl"] = UrlTextBox.Text.ToString();
            AppData.Values["usrname"] = UsernameTextBox.Text.ToString();
            AppData.Values["passwd"] = PasswordTextBox.Password.ToString();
        }

        async void Login() {

            Views.Busy.SetBusy(true, "Kissing");

            if (UsernameTextBox.Text != "" && PasswordTextBox.Password != "") {
                try {
                    Save();

                    string result = await Kiss(UrlTextBox.Text, UsernameTextBox.Text, PasswordTextBox.Password);
                    Debug.WriteLine(result);

                    await Task.Delay(800);

                    if (result.Contains("认证成功")) {
                        InfoTextBlock.Text = "Kissed!";
                    } else if (result.Contains("该IP已登录，请先注销")) {
                        InfoTextBlock.Text = "Kissed.";
                    } else if (result.Contains("被冻结")) {
                        InfoTextBlock.Text = "This account is forbidden.";
                    } else {
                        InfoTextBlock.Text = "Something went wrong.";
                    }
                } catch (Exception) {
                    ContentDialog ErrorContentDialog = new ContentDialog {
                        Title = "ERROR",
                        Content = "Please connect to Maple Leaf and enter correct API URL. ",
                        CloseButtonText = "哦。",
                    }; ContentDialogResult result = await ErrorContentDialog.ShowAsync();
                }
            } else {
                ContentDialog ErrorContentDialog = new ContentDialog {
                    Title = "ERROR",
                    Content = "Please enter username and/or password",
                    CloseButtonText = "哦。",
                }; ContentDialogResult result = await ErrorContentDialog.ShowAsync();
            }

            Views.Busy.SetBusy(false);
        }

        private void LoginButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) {
            Login();
        }

        public static async Task<string> Kiss(string apiurl, string username, string password) {
            if (apiurl == "" || apiurl == "default") {
                apiurl = "http://172.16.1.38/webAuth/";
            }
            HttpClient client = new HttpClient();
            var content = new FormUrlEncodedContent(
                new Dictionary<string, string> {
                    { "une", username },
                    { "passwd", password },
                    { "username", username },
                    { "pwd", password }
                }
            );
            var response = await client.PostAsync(apiurl, content);
            return await response.Content.ReadAsStringAsync();
        }

    }
}