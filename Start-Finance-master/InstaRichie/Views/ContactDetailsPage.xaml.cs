// **************************************************************************
//Start Finance - An to manage your personal finances.

//Start Finance is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//Start Finance is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//GNU General Public License for more details.

//You shousssld have received a copy of the GNU General Public License
//along with Start Finance.If not, see<http://www.gnu.org/licenses/>.
// ***************************************************************************

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
using Windows.UI.Popups;
using StartFinance.Models;
using SQLite.Net;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StartFinance.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContactDetailsPage : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");

        public string AccountSelection { get; private set; }

        public ContactDetailsPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            /// Initializing a database
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);



            // Creating table
            Results();
        }

        public void Results()
        {
            // Creating table
            conn.CreateTable<Accounts>();
            var query = conn.Table<Accounts>();
            // TransactionList.ItemsSource = query.ToList();
            AccountsListSel.ItemsSource = query.ToList();

            conn.CreateTable<ContactDetails>();


        }


        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            string AccountSelection = ((Accounts)AccountsListSel.SelectedItem).AccountName;



            // checks if account name is null
            if (AccountSelection == "")
            {
                MessageDialog dialog = new MessageDialog("Select an account", "Oops..!");
                await dialog.ShowAsync();
            }
            else if (FName.Text.ToString() == "FirstName" || LName.Text.ToString() == "Name")
            {
                MessageDialog variableerror = new MessageDialog("You cannot use this name", "Oops..!");
            }
            else
            {
                // String ContactDetailAccounts = conn.Query<ContactDetails>("SELECT * FROM ContactDetails WHERE Account = " + AccountSelection).ToString();
                string accountSel = ((Accounts)AccountsListSel.SelectedItem).AccountName;
                List<ContactDetails> query1 = conn.Query<ContactDetails>("SELECT * FROM ContactDetails WHERE Account ='" + accountSel + "'");

                if (query1.Count == 0)
                {

                    conn.Insert(new ContactDetails()
                    {
                        FirstName = FName.Text,
                        LastName = LName.Text,
                        CompanyName = CName.Text,
                        MobilePhone = MobilePhone.Text,
                        Account = AccountSelection


                        //InitialAmount = Convert.ToDouble(MoneyIn.Text),
                        //OverDraft = Drafting()
                    });
                    Results();
                }
                else
                {
                    List<ContactDetails> query2 = conn.Query<ContactDetails>("UPDATE ContactDetails SET Account = '" + accountSel + "',FirstName = '" + FName.Text + "',LastName = '" + LName.Text + "',CompanyName = '" + CName.Text + "',MobilePhone = '" + "' WHERE Account = '" + accountSel + "'");

                    Results();

                }
            }

        }






        private void AccountsListSel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            string accountSel = "";
            try
            {
                FName.Text = "";
                LName.Text = "";
                CName.Text = "";
                MobilePhone.Text = "";

                accountSel = ((Accounts)AccountsListSel.SelectedItem).AccountName;
                List<ContactDetails> query1 = conn.Query<ContactDetails>("SELECT * FROM ContactDetails WHERE Account ='" + accountSel + "'");

                FName.Text = query1.ElementAt(0).FirstName.ToString();
                LName.Text = query1.ElementAt(0).LastName.ToString();
                CName.Text = query1.ElementAt(0).CompanyName.ToString();
                MobilePhone.Text = query1.ElementAt(0).MobilePhone.ToString();


            }
            catch
            {


            }
        }
        // Clears the fields
        private async void ClearFileds_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog ClearDialog = new MessageDialog("Cleared", "information");
            await ClearDialog.ShowAsync();
        }

        // Displays the data when navigation between pages
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Results();
        }

        private async void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog ShowConf = new MessageDialog("Deleting contact details associated with this account?", "Important");
            ShowConf.Commands.Add(new UICommand("Yes, Delete")
            {
                Id = 0
            });
            ShowConf.Commands.Add(new UICommand("Cancel")
            {
                Id = 1
            });
            ShowConf.DefaultCommandIndex = 0;
            ShowConf.CancelCommandIndex = 1;

            var result = await ShowConf.ShowAsync();
            if ((int)result.Id == 0)
            {
                // checks if data is null else inserts
                try
                {
                    string AccountsLabel = ((Accounts)TransactionList.SelectedItem).AccountName;
                    var querydel = conn.Query<ContactDetails>("DELETE FROM ContactDetails WHERE Account='" + AccountsLabel + "'");
                    Results();
                    conn.CreateTable<Transactions>();
                    //var querytable = conn.Query<Transactions>("DELETE FROM Transactions WHERE Account='" + AccountsLabel + "'");
                }
                catch (NullReferenceException)
                {
                    MessageDialog ClearDialog = new MessageDialog("Please select the item to Delete", "Oops..!");
                    await ClearDialog.ShowAsync();
                }
            }
            else
            {
                //
            }
        }

    }
}
