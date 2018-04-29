using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;
namespace StartFinance.Models
{
    class ShoppingList2
    {
        [PrimaryKey, AutoIncrement]
        public int ShoppingItemID { get; set; }
        
        [NotNull]
        public string Account { get; set; }

        [NotNull]
        public string ShopName { get; set; }

        [NotNull]
        public string NameOfItem { get; set; }

        [NotNull]
        public string ShoppingDate { get; set; }

        [NotNull]
        public string PriceQuoted { get; set; }
    }
}
