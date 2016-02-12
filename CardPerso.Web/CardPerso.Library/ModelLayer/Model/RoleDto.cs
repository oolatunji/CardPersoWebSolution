using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardPerso.Library.ModelLayer.Model
{
    public class RoleDto : BaseModel
    {
        public List<Function> Functions { get; set; }
    }
}
