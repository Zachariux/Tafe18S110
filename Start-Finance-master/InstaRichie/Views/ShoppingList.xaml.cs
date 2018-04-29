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
    public sealed partial class ShoppingList : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");

        public string AccountSelection { get; private set; }
  

        public ShoppingList()
        {
            this.InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            /// Initializing a database
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            conn.CreateTable<ShoppingList2>();
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


        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {


            string shopname = TShopName.Text;
            string nameofitem = TNameOfItem.Text;
            string shoppingdate = TShoppingDate.Text;
            string pricequoted = TPriceQuoted.Text;
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

            List<ShoppingList2> query1 = conn.Query<ShoppingList2>("SELECT * FROM ShoppingList2 WHERE Account ='" + accountSel + "'");

            if (((ShoppingList2)TransList.SelectedItem) == null)
            {
                conn.Insert(new ShoppingList2()
                {
                    Account = accountSel,
                    ShopName = shopname,
                    NameOfItem = nameofitem,
                    ShoppingDate = shoppingdate,
                    PriceQuoted = pricequoted

                });
                ((Accounts)AccountsListSel.SelectedItem).AccountName = "";
                TransList.ItemsSource = null;
                Results();
            }
            else
            {

                String TShopName = ((ShoppingList2)TransList.SelectedItem).ShopName;
                String TNameOfItem = ((ShoppingList2)TransList.SelectedItem).NameOfItem;
                String TShoppingDate = ((ShoppingList2)TransList.SelectedItem).ShoppingDate;
                String TPriceQuoted = ((ShoppingList2)TransList.SelectedItem).PriceQuoted;
                List<ShoppingList2> query2 = conn.Query<ShoppingList2>("UPDATE ShoppingList2 SET Account = '" + accountSel + "',ShopName = '" + shopname + "',NameOfItem = '" + nameofitem + "',ShoppingDate = '" + shoppingdate + "',PriceQuoted = '" + pricequoted + "' WHERE ShoppingItemID = '" + ((ShoppingList2)TransList.SelectedItem).ShoppingItemID + "' ");

                Results();

            }
        }


        // Clears the fields
        private void ClearFileds_Click(object sender, RoutedEventArgs e)
        {

        }

        // Displays the data when navigation between pages
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Results();
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
            List<ShoppingList2> query1 = conn.Query<ShoppingList2>("DELETE FROM ShoppingList2 WHERE Account ='" + accountSel + "' AND ShoppingItemID = '" + ((ShoppingList2)TransList.SelectedItem).ShoppingItemID + "' ");
            Results();
        }



        private void AccountsListSel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string accountSel = "";
            try
            {
                TShopName.Text = "";

                TNameOfItem.Text = "";
                TShoppingDate.Text = "";
                TPriceQuoted.Text = "";



                accountSel = ((Accounts)AccountsListSel.SelectedItem).AccountName;
                List<ShoppingList2> query1 = conn.Query<ShoppingList2>("SELECT * FROM ShoppingList2 WHERE Account ='" + accountSel + "'");





           
            }
            catch { }
            conn.CreateTable<ShoppingList2>();
            List<ShoppingList2> query2 = conn.Query<ShoppingList2>("SELECT * FROM ShoppingList2 WHERE Account ='" + accountSel + "'");




            TransList.ItemsSource = query2.ToList();





        }

        private void TransList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            string accountSel = "";
            try
            {
                TShopName.Text = "";

                TNameOfItem.Text = "";
                TShoppingDate.Text = "";
                TPriceQuoted.Text = "";

                accountSel = ((Accounts)AccountsListSel.SelectedItem).AccountName;


                List<ShoppingList2> query1 = conn.Query<ShoppingList2>("SELECT * FROM ShoppingList2 WHERE Account ='" + accountSel + "'");

                int ShoppingItemID = ((ShoppingList2)TransList.SelectedItem).ShoppingItemID;

                TShopName.Text = ((ShoppingList2)TransList.SelectedItem).ShopName;
                TNameOfItem.Text = ((ShoppingList2)TransList.SelectedItem).NameOfItem;
                TShoppingDate.Text = ((ShoppingList2)TransList.SelectedItem).ShoppingDate;
                TPriceQuoted.Text = ((ShoppingList2)TransList.SelectedItem).PriceQuoted;

            }
            catch { }
        }

        private void ClearItem_Click(object sender, RoutedEventArgs e)
        {
            TransList.SelectedItem = null;
            TShopName.Text = "";

            TNameOfItem.Text = "";
            TShoppingDate.Text = "";
            TPriceQuoted.Text = "";
        }
    }
}
