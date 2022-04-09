using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer.UserModel
{
    public class UserDataViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string FirstName { get; set; }
        public DateTime createdOn { get; set; }
    }
}
