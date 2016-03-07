using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.BO
{
    public abstract class BOBase
    {
        protected int _ID;
        public virtual int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }
    }
}
