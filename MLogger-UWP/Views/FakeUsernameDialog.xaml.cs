using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MLogger.Views {
    public sealed partial class FakeUsernameDialog : ContentDialog {

        ApplicationDataContainer AppData = ApplicationData.Current.LocalSettings;

        public FakeUsernameDialog() {
            InitializeComponent();
            // InitAppData();
            RealUsernameTextBox.Text = AppData.Values["realUsername"].ToString();
            FakeUsernameTextBox.Text = AppData.Values["fakeUsername"].ToString();

        }

        private void ContentDialog_ButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            Save();
            Hide();
        }

        void InitAppData() {
            FakeUsernameTextBox.Text = AppData.Values["fakeUsername"].ToString();
            try {
                RealUsernameTextBox.Text = AppData.Values["realUsername"].ToString();
            } catch (System.NullReferenceException) {
                AppData.Values["realUsername"] = "";
                RealUsernameTextBox.Text = AppData.Values["realUsername"].ToString();
            }
        }

        void Save() {
            AppData.Values["realUsername"] = RealUsernameTextBox.Text.ToString();
            AppData.Values["fakeUsername"] = FakeUsernameTextBox.Text.ToString();
        }
    }
}
