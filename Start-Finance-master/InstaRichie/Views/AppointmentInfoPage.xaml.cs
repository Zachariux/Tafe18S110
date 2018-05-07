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

//You should have received a copy of the GNU General Public License
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
    public sealed partial class AppointmentInfoPage : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");

        public AppointmentInfoPage()
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
            conn.CreateTable<Appointments>();
            conn.CreateTable<Accounts>();
            var query = conn.Table<Accounts>();
            AccountsListSel.ItemsSource = query.ToList();



        }
        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            string accountSel = null;


            // checks if account name is null
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
                List<Appointments> query1 = conn.Query<Appointments>("SELECT * FROM Appointments WHERE Account ='" + accountSel + "'");

                if (((Appointments)TransactionList.SelectedItem) == null)
                {
                    
                conn.Insert(new Appointments()
                {
                    EventName = EvntName.Text,
                    Location = Locate.Text,
                    EventDate = EvntDate.Text,
                    StartTime = STime.Text,
                    EndTime = ETime.Text,
                    Account = accountSel

                    //EventDate = EvntDate.Date,
                    // StartTime = STime.Date,
                    //EndTime = Etime.Date



                });
                    ((Accounts)AccountsListSel.SelectedItem).AccountName = "";
                    TransactionList.ItemsSource = null;
                Results();
            }
                else
                {

                  
                    List<Appointments> query2 = conn.Query<Appointments>("UPDATE Appointments SET Account = '" + accountSel + "',EventName = '" + EvntName.Text + "',Location = '" + Locate.Text + "',EventDate = '" + EvntDate.Text + "',StartTime = '" + STime.Text + "',EndTime = '" + ETime.Text + "' WHERE ID = '" + ((Appointments)TransactionList.SelectedItem).ID + "' ");

                    Results();

                }
            }
        private void AccountsListSel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string accountSel = "";
            try
            {
                EvntName.Text = "";
                Locate.Text = "";
                EvntDate.Text = "";
                STime.Text = "";
                ETime.Text = "";


                accountSel = ((Accounts)AccountsListSel.SelectedItem).AccountName;
                List<Appointments> query1 = conn.Query<Appointments>("SELECT * FROM Appointments WHERE Account ='" + accountSel + "'");


               
            }
            catch { }
            conn.CreateTable<Appointments>();
            List<Appointments> query2 = conn.Query<Appointments>("SELECT * FROM Appointments WHERE Account ='" + accountSel + "'");



            TransactionList.ItemsSource = query2.ToList();





        }
        private void TransactionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            string accountSel = "";
            try
            {
                EvntName.Text = "";
                Locate.Text = "";
                EvntDate.Text = "";
                STime.Text = "";
                ETime.Text = "";

                accountSel = ((Accounts)AccountsListSel.SelectedItem).AccountName;


                List<Appointments> query1 = conn.Query<Appointments>("SELECT * FROM Appointments WHERE Account ='" + accountSel + "'");

                int ID = ((Appointments)TransactionList.SelectedItem).ID;

                EvntName.Text = ((Appointments)TransactionList.SelectedItem).EventName;
                Locate.Text = ((Appointments)TransactionList.SelectedItem).Location;
                EvntDate.Text = ((Appointments)TransactionList.SelectedItem).EventDate;
                STime.Text = ((Appointments)TransactionList.SelectedItem).StartTime;
                ETime.Text = ((Appointments)TransactionList.SelectedItem).EndTime;


            }
            catch { }
        }

        private void ClearItem_Click(object sender, RoutedEventArgs e)
        {
            TransactionList.SelectedItem = null;
            EvntName.Text = "";
            Locate.Text = "";
            EvntDate.Text = "";
            STime.Text = "";
            ETime.Text = "";
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Results();
        }
        private async void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog ShowConf = new MessageDialog("Delete Appointment associated with this account?", "Important");
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
                    String accountSel = ((Accounts)AccountsListSel.SelectedItem).AccountName;

                    List<Appointments> query1 = conn.Query<Appointments>("DELETE FROM Appointments WHERE Account ='" + accountSel + "' AND ID = '" + ((Appointments)TransactionList.SelectedItem).ID + "' ");
                    Results();
                    conn.CreateTable<Appointments>();
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


