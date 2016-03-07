using BLL.BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public abstract class BLLBase
    {
        public abstract List<SqlParameter> fillParams(BO.BOBase u);

        #region Update
        public abstract void Update(BO.BOBase u);
        #endregion

        #region Insert
        public abstract void Create(BO.BOBase u);
        #endregion

        #region Delete
        public abstract void Delete(BO.BOBase u);
        #endregion
        
        //in 1 methode iets teruggeven dat gecontroleerd is op dbnull
        //--> Generics
        public virtual T IsDbNull<T>(object o)
        {
            if (o != DBNull.Value)
            {
                if(o is T)
                {
                    return (T)o;
                }
            }
            return default(T);
        }
    }
    
}
