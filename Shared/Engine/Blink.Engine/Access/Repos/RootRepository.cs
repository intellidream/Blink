using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blink.Data.Domain.Model;
using Blink.Data.Engine;

namespace Blink.Data.Access.Repos
{
    internal interface IRepository
    {
        Task<T> LoadAsync<T>(object key) where T : class, IElement, new();
        Task<object> SaveAsync<T>(T instance) where T : class, IElement, new();
        Task DeleteAsync<T>(T instance) where T : class, IElement;
        //findById
        //loadAll
    }

    internal class BaseRepository : IRepository
    {
        #region IRepository Members

        public async Task<T> LoadAsync<T>(object key) where T : class, IElement, new()
        {
            return await Sterling.Database.LoadAsync<T>(key);
        }

        public async Task<object> SaveAsync<T>(T instance) where T : class, IElement, new()
        {
            var result = await Sterling.Database.SaveAsync<T>(instance);

            if (result != null)
            {
                await Sterling.Database.FlushAsync();
            }

            return result;
        }

        public async Task DeleteAsync<T>(T instance) where T : class, IElement
        {
            await Sterling.Database.DeleteAsync<T>(instance);
        }

        #endregion
    }

    //internal class BaseRepository : 
    //{
    //    public static Repository Repo { get; private set; }

    //    static BaseRepository() 
    //    {
    //        Repo = new Repository();
    //    }
    //}

    public static class RootRepository 
    {
        private static readonly BaseRepository _repo;

        public static RootElement Root { get; private set; }

        static RootRepository() 
        {
            _repo = new BaseRepository();
            Root = new RootElement();
        }

        internal static async Task<object> SaveAsync()
        {
            return await _repo.SaveAsync<RootElement>(Root);
        }

        internal static async Task LoadAsync() 
        {
            Root = await _repo.LoadAsync<RootElement>(true);
        }

        internal static async Task DeleteAsync()
        {
            await _repo.DeleteAsync<RootElement>(Root);
        }
    }
}

namespace Blink.Data.Domain.Model 
{
    using Blink.Data.Access.Repos;

    public static class RootExtensions 
    {
        public static async Task<object> SaveAsync(this RootElement instance) 
        {
            return await RootRepository.SaveAsync();
        }

        public static async Task LoadAsync(this RootElement instance)
        {
            await RootRepository.LoadAsync();
        }

        public static async Task DeleteAsync(this RootElement instance)
        {
            await RootRepository.DeleteAsync();
        }
    }

    public static class FolderExtensions
    {
        private static readonly BaseRepository _repo;

        static FolderExtensions() 
        {
            _repo = new BaseRepository();
        }

        public static async Task<object> SaveAsync(this FolderElement instance)
        {
            return await _repo.SaveAsync<FolderElement>(instance);
        }

        public static async Task<FolderElement> LoadAsync(this FolderElement instance)
        {
            return await _repo.LoadAsync<FolderElement>(instance.Id);
        }

        public static async Task DeleteAsync(this FolderElement instance)
        {
            await _repo.DeleteAsync<FolderElement>(instance);
        }
    }
}