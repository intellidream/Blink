using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blink.Shared.Domain.DataModel;

namespace Blink.Shared.Domain.DataLayer
{
    public interface IRepository<T>
    {
        Task<T> GetById(Guid id);
        Task<List<T>> GetAll();
        Task Create(T item);
        Task Update(T item);
        Task<bool> Delete(T item);
    }
}
