using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace StartFinance.Models
{
    class PersonalDetails
    {
        [PrimaryKey, AutoIncrement]
        public int PersonalID { get; set; }

        [NotNull]
        public string Account { get; set; }

        [NotNull]
        public string FirstName { get; set; }

        [NotNull]
        public string LastName { get; set; }

        [NotNull]
        public string DOB { get; set; }

        [NotNull]
        public string Gender { get; set; }

        [NotNull]
        public string EmailAddress { get; set; }

        [NotNull]
        public string MobilePhone { get; set; }
    }
}
