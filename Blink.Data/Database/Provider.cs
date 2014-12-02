using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blink.Data.Database
{
    public interface IProvider 
    {
        void CreateDatabase(string databasePath);
    }

    public class Provider : IProvider
    {
        #region IProvider Members

        public async void CreateDatabase(string databasePath)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
