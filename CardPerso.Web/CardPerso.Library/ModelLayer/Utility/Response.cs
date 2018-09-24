using CardPerso.Library.ModelLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardPerso.Library.ModelLayer.Utility
{
    public class Response
    {
        public string SuccessMsg { get; set; }
        public string ErrorMsg { get; set; }
        public object DynamicList { get; set; }
        public int BranchId { get; set; }
    }
}
