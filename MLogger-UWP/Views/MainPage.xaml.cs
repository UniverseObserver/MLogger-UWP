using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MLogger.Views {
    public sealed partial class MainPage : Page {

        ApplicationDataContainer AppData = ApplicationData.Current.LocalSettings;
        int BottomBar_Tapped_Counter = 0;
        public MainPage() {

            InitializeComponent();
            SetUIBlack();
            InitAppData();
            UrlTextBox.Text = AppData.Values["apiurl"].ToString();
            if (FakeUsernameTextBox.Text != "" && PasswordTextBox.Password != "") {
                Login();
            }
        }

        void SetUIBlue() {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = Windows.UI.Color.FromArgb(255, 0, 99, 177);
            titleBar.ForegroundColor = Windows.UI.Colors.White;
            titleBar.ButtonBackgroundColor = Windows.UI.Color.FromArgb(255, 0, 99, 177);
            titleBar.ButtonForegroundColor = Windows.UI.Colors.White;
            titleBar.ButtonPressedBackgroundColor = Windows.UI.Color.FromArgb(0, 25, 114, 184);
            titleBar.ButtonPressedForegroundColor = Windows.UI.Colors.White;
            titleBar.ButtonInactiveBackgroundColor = Windows.UI.Color.FromArgb(255, 0, 99, 177);
            titleBar.ButtonInactiveForegroundColor = Windows.UI.Colors.LightGray;
            titleBar.InactiveBackgroundColor = Windows.UI.Color.FromArgb(255, 0, 99, 177);
            titleBar.InactiveForegroundColor = Windows.UI.Colors.LightGray;
            titleBar.ButtonHoverBackgroundColor = Windows.UI.Color.FromArgb(0, 25, 114, 184);
            titleBar.ButtonHoverForegroundColor = Windows.UI.Colors.White;
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.XamlCompositionBrushBase")) {
                CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            }
        }
        void SetUIPink() {
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
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.XamlCompositionBrushBase")) {
                CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            }
        }
        void SetUIBlack() {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = Windows.UI.Color.FromArgb(255, 0, 0, 0);
            titleBar.ForegroundColor = Windows.UI.Colors.White;
            titleBar.ButtonBackgroundColor = Windows.UI.Color.FromArgb(255, 0, 0, 0);
            titleBar.ButtonForegroundColor = Windows.UI.Colors.White;
            titleBar.ButtonPressedBackgroundColor = Windows.UI.Color.FromArgb(125, 0, 0, 0);
            titleBar.ButtonPressedForegroundColor = Windows.UI.Colors.White;
            titleBar.ButtonInactiveBackgroundColor = Windows.UI.Color.FromArgb(255, 0, 0, 0);
            titleBar.ButtonInactiveForegroundColor = Windows.UI.Colors.LightGray;
            titleBar.InactiveBackgroundColor = Windows.UI.Color.FromArgb(255, 0, 0, 0);
            titleBar.InactiveForegroundColor = Windows.UI.Colors.LightGray;
            titleBar.ButtonHoverBackgroundColor = Windows.UI.Color.FromArgb(125, 0, 0, 0);
            titleBar.ButtonHoverForegroundColor = Windows.UI.Colors.White;
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.XamlCompositionBrushBase")) {
                CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            }
        }

        void InitAppData() {

            try {
                UrlTextBox.Text = AppData.Values["apiurl"].ToString();
            } catch (System.NullReferenceException) {
                AppData.Values["apiurl"] = "";
                UrlTextBox.Text = AppData.Values["apiurl"].ToString();
            }

            try {
                FakeUsernameTextBox.Text = AppData.Values["fakeUsername"].ToString();
            } catch (System.NullReferenceException) {
                AppData.Values["fakeUsername"] = "";
                FakeUsernameTextBox.Text = AppData.Values["fakeUsername"].ToString();
            }

            try {
                string s = AppData.Values["realUsername"].ToString();
            } catch (System.NullReferenceException) {
                AppData.Values["realUsername"] = "";
            }

            //realUsername

            try {
                PasswordTextBox.Password = AppData.Values["passwd"].ToString();
            } catch (System.NullReferenceException) {
                AppData.Values["passwd"] = "";
                PasswordTextBox.Password = AppData.Values["passwd"].ToString();
            }

        }

        void Save() {
            AppData.Values["apiurl"] = UrlTextBox.Text.ToString();
            AppData.Values["fakeUsername"] = FakeUsernameTextBox.Text.ToString();
            AppData.Values["passwd"] = PasswordTextBox.Password.ToString();
        }

        async void Login() {

            SetUIBlue();
            Views.Busy.SetBusy(true, "Kissing");

            if (FakeUsernameTextBox.Text != "" && PasswordTextBox.Password != "") {
                try {
                    string url = UrlTextBox.Text;
                    string passwd = PasswordTextBox.Password;
                    string username;

                    bool hasAFakeName = (AppData.Values["realUsername"].ToString() != "");
                    if (hasAFakeName) {
                        username = AppData.Values["realUsername"].ToString();
                    } else {
                        username = FakeUsernameTextBox.Text;
                    }

                    Save();

                    string result = await Kiss(url, username, passwd);
                    result += await Kiss(url, username, passwd);
                    Debug.WriteLine(result);

                    await Task.Delay(800);

                    if (result.Contains("认证成功") || result.Contains("Microsoft Connect Test")) {
                        InfoTextBlock.Text = "Kissed!";
                    } else if (result.Contains("该IP已登录，请先注销")) {
                        InfoTextBlock.Text = "Kissed.";
                    } else if (result.Contains("被冻结")) {
                        InfoTextBlock.Text = "This account is forbidden.";
                    } else {
                        InfoTextBlock.Text = "016 is dead. Mismatch. ";
                    }
                } catch (Exception) {
                    ContentDialog ErrorContentDialog = new ContentDialog {
                        Title = "ERROR",
                        Content = "Please connect to Maple Leaf and enter correct API URL. ",
                        CloseButtonText = "哦。",
                    }; ContentDialogResult result = await ErrorContentDialog.ShowAsync();
                }
            } 
            else {
                ContentDialog ErrorContentDialog = new ContentDialog {
                    Title = "ERROR",
                    Content = "Please enter username and/or password",
                    CloseButtonText = "哦。",
                }; ContentDialogResult result = await ErrorContentDialog.ShowAsync();
            }

            Views.Busy.SetBusy(false);
            SetUIBlack();
        }

        private void OnLoginButtonClick(object sender, Windows.UI.Xaml.RoutedEventArgs e) {
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

        private async void OnBottomBarTap(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e) {
            BottomBar_Tapped_Counter++;
            if (BottomBar_Tapped_Counter == 13) {
                BottomBar_Tapped_Counter = 0;
                MLogger.Views.FakeUsernameDialog dialog = new MLogger.Views.FakeUsernameDialog();
                await dialog.ShowAsync();
                FakeUsernameTextBox.Text = AppData.Values["fakeUsername"].ToString();
            }
        }

        private void OnGridLoad(object sender, RoutedEventArgs e) {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.XamlCompositionBrushBase")) {
                Grid grid = sender as Grid;
                Windows.UI.Xaml.Media.AcrylicBrush acrylicBrush = new Windows.UI.Xaml.Media.AcrylicBrush();
                acrylicBrush.BackgroundSource = Windows.UI.Xaml.Media.AcrylicBackgroundSource.HostBackdrop;
                acrylicBrush.TintColor = Color.FromArgb(255, 0, 0, 0);
                acrylicBrush.FallbackColor = Color.FromArgb(255, 0, 0, 0);
                acrylicBrush.TintOpacity = 0.6;
                grid.Background = acrylicBrush;

                CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
                ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            } 
        }

        private void OnBorderLoad(object sender, RoutedEventArgs e) {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.XamlCompositionBrushBase")) {
                Border border = sender as Border;
                Windows.UI.Xaml.Media.AcrylicBrush acrylicBrush = new Windows.UI.Xaml.Media.AcrylicBrush();
                acrylicBrush.BackgroundSource = Windows.UI.Xaml.Media.AcrylicBackgroundSource.HostBackdrop;
                acrylicBrush.TintColor = Color.FromArgb(255, 0, 99, 177);
                acrylicBrush.FallbackColor = Color.FromArgb(255, 0, 99, 177);
                acrylicBrush.TintOpacity = 0.6;
                border.Background = acrylicBrush;
            }
        }

        private void OnButtonLoad(object sender, RoutedEventArgs e) {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.XamlCompositionBrushBase")) {
                Button button= sender as Button;
                Windows.UI.Xaml.Media.AcrylicBrush acrylicBrush = new Windows.UI.Xaml.Media.AcrylicBrush();
                acrylicBrush.BackgroundSource = Windows.UI.Xaml.Media.AcrylicBackgroundSource.HostBackdrop;
                acrylicBrush.TintColor = Color.FromArgb(255, 0, 99, 177);
                acrylicBrush.FallbackColor = Color.FromArgb(255, 0, 99, 177);
                acrylicBrush.TintOpacity = 0.6;
                button.Background = acrylicBrush;
            }
        }
    }
}