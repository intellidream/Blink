using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blink.Data.Domain.Model;
using Blink.Data.Engine;
using Wintellect.Sterling.Core.Keys;

#region Older

//namespace Blink.Data.Access
//{
//    internal interface ISterlingRepository
//    {
//        Task<T> LoadAsync<T>(object key) where T : class, IElement, new();
//        Task<object> SaveAsync<T>(T instance) where T : class, IElement, new();
//        Task DeleteAsync<T>(T instance) where T : class, IElement;
//        List<TableKey<T, TKey>> Query<T, TKey>() where T : class, IElement, new();
//    }

//    internal class SterlingRepository : ISterlingRepository
//    {
//        #region ISterlingRepository Members

//        public async Task<T> LoadAsync<T>(object key) where T : class, IElement, new()
//        {
//            return await Sterling.Database.LoadAsync<T>(key);
//        }

//        public async Task<object> SaveAsync<T>(T instance) where T : class, IElement, new()
//        {
//            var result = await Sterling.Database.SaveAsync<T>(instance);

//            if (result != null)
//            {
//                await Sterling.Database.FlushAsync();
//            }

//            return result;
//        }

//        public async Task DeleteAsync<T>(T instance) where T : class, IElement
//        {
//            await Sterling.Database.DeleteAsync<T>(instance);
//        }

//        public List<TableKey<T, TKey>> Query<T, TKey>() where T : class, IElement, new() 
//        {
//            return Sterling.Database.Query<T, TKey>();
//        }

//        #endregion
//    }

//    public static class Repository 
//    {
//        private static readonly SterlingRepository _repo;

//        public static RootElement Root { get; private set; }

//        static Repository() 
//        {
//            _repo = new SterlingRepository();
//            Root = new RootElement();
//        }

//        public static async Task<T> LoadAsync<T>(object key) where T : class, IElement, new()
//        {
//            return await _repo.LoadAsync<T>(key);
//        }

//        public static async Task<object> SaveAsync<T>(T instance) where T : class, IElement, new() 
//        {
//            return await _repo.SaveAsync<T>(instance);
//        }

//        public static async Task DeleteAsync<T>(T instance) where T : class, IElement 
//        {
//            await _repo.DeleteAsync<T>(instance);
//        }

//        public static Lazy<T> Query<T, TKey>() where T : class, IElement, new()
//        {
//            return (from tableKey in _repo.Query<T, TKey>() select tableKey.LazyValue).FirstOrDefault();
//        }
//    }
//}

//namespace Blink.Data.Domain.Model 
//{
//    using Blink.Data.Access;

//    public static class RootExtensions 
//    {
//        public static async Task<object> SaveAsync(this RootElement instance) 
//        {
//            //var d = instance.Data;
//            return await Repository.SaveAsync<RootElement>(instance);
//        }

//        public static async Task LoadAsync(this RootElement instance)
//        {
//            //var d = instance.Data;
//            var a = await Repository.LoadAsync<RootElement>(RootElement.RootId);
//            instance = a;
//        }

//        public static async Task DeleteAsync(this RootElement instance)
//        {
//            await Repository.DeleteAsync<RootElement>(instance);
//        }

//        public static Lazy<RootElement> Query(this RootElement instance)
//        {
//            return Repository.Query<RootElement, bool>();
//        }
//    }
//}

#endregion