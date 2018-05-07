using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SQLite;
using StartFinance.Models;
using Windows.UI.Popups;
using SQLite.Net;
using System.Linq;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace StartFinance.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PersonalInfo : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");

        public string AccountSelection { get; private set; }









        public PersonalInfo()
        {
            this.InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            /// Initializing a database
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            conn.CreateTable<PersonalDetails>();
            // Creating table
            Results();
        }



        public void Results()
        {
            // Creating table
            conn.CreateTable<Accounts>();
            var query = conn.Table<Accounts>();
            AccountsListSel.ItemsSource = query.ToList();



        }
        private void AddData(object sender, RoutedEventArgs e)
        {

            string AccountSelection = ((Accounts)AccountsListSel.SelectedItem).AccountName;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }


        private void AccountsListSel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string accountSel = "";
            try
            {
                TFirstName.Text = "";
                TLastName.Text = "";
                TDOB.Text = "";
                TEmailAddress.Text = "";
                TMobilePhone.Text = "";


                accountSel = ((Accounts)AccountsListSel.SelectedItem).AccountName;
                List<PersonalDetails> query1 = conn.Query<PersonalDetails>("SELECT * FROM PersonalDetails WHERE Account ='" + accountSel + "'");


                TFirstName.Text = query1.ElementAt(0).FirstName.ToString();
                TLastName.Text = query1.ElementAt(0).LastName.ToString();
                TDOB.Text = query1.ElementAt(0).DOB.ToString();
                TEmailAddress.Text = query1.ElementAt(0).EmailAddress.ToString();
                TMobilePhone.Text = query1.ElementAt(0).MobilePhone.ToString();


                if (query1.ElementAt(0).Gender.ToString() == "Male")
                {
                    Rmale.IsChecked = true;
                }
                else
                {
                    Rfemale.IsChecked = true;
                }

            }
            catch { }





        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {


            string firstname = TFirstName.Text;
            string lastname = TLastName.Text;
            string dob = TDOB.Text;
            string gender;
            if (Rmale.IsChecked == true)
            {
                gender = "Male";
            }
            else
            {
                gender = "Female";
            }

            string emailAddress = TEmailAddress.Text;
            string mobilePhone = TMobilePhone.Text;

            string accountSel = null;
            if (((Accounts)AccountsListSel.SelectedItem) != null)

            {
                accountSel = ((Accounts)AccountsListSel.SelectedItem).AccountName;
            }
            else
            {
                MessageDialog dialog = new MessageDialog("No account selected", "Oops..!");
                await dialog.ShowAsync();
                return;
            }

            List<PersonalDetails> query1 = conn.Query<PersonalDetails>("SELECT * FROM PersonalDetails WHERE Account ='" + accountSel + "'");
            if (query1.Count == 0)
            {
                conn.Insert(new PersonalDetails()
                {
                    Account = accountSel,
                    FirstName = firstname,
                    LastName = lastname,
                    DOB = dob,
                    Gender = gender,
                    EmailAddress = emailAddress,
                    MobilePhone = mobilePhone

                });
                Results();
            }
            else
            {
                List<PersonalDetails> query2 = conn.Query<PersonalDetails>("UPDATE PersonalDetails SET Account = '" + accountSel + "',FirstName = '" + firstname + "',LastName = '" + lastname + "',DOB = '" + dob + "',Gender = '" + gender + "',EmailAddress = '" + emailAddress + "',MobilePhone = '" + mobilePhone + "' WHERE Account = '" + accountSel + "' ");

                Results();
            }



        }

        private async void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            string accountSel = null;
            if (((Accounts)AccountsListSel.SelectedItem) != null)

            {
                accountSel = ((Accounts)AccountsListSel.SelectedItem).AccountName;

            }
            else
            {
                MessageDialog dialog = new MessageDialog("No account selected", "Oops..!");
                await dialog.ShowAsync();
                return;
            }
            List<PersonalDetails> query1 = conn.Query<PersonalDetails>("DELETE FROM PersonalDetails WHERE Account ='" + accountSel + "'");
            Results();
        }
    }
}
