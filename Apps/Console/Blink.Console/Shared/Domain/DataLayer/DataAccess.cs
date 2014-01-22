using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blink.Shared.Domain.DataModel.Notes;

namespace Blink.Console.Shared.Domain.DataLayer
{
    class DataAccess
    {
    }

    internal class Repository : Blink.Shared.Domain.DataLayer.IRepository<BlinkNote>
    {
        public Task<BlinkNote> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<BlinkNote>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task Create(BlinkNote item)
        {
            throw new NotImplementedException();
        }

        public Task Update(BlinkNote item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(BlinkNote item)
        {
            throw new NotImplementedException();
        }
    }
}
