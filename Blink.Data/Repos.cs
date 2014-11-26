using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blink.Data.Model;

namespace Blink.Data.Repos
{
    public interface IRepository<T>
    {
        Task<T> GetById(Guid id);
        Task<bool> Save(T element);
        Task<bool> Delete(Guid id);
    }

    public interface IElementRepository : IRepository<Element> { }

    public interface IMaterialRepository<T> : IElementRepository where T : Material<T>
    {
        T GetValueById(Guid id);
        bool SaveValue(T value);
        bool DeleteValue(Guid id);
    }

    //public class ElementRepository : IRepository<Element> 
    //{
    //    #region IRepository<Element> Members

    //    public async Task<Element> GetById(Guid id)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public async Task<bool> Save(Element element)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public async Task<bool> Delete(Guid id)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    #endregion
    //}
}
