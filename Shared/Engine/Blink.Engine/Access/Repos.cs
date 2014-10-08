using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blink.Data.Domain.Model;
using Blink.Data.Engine;

namespace Blink.Data.Access.Repos
{
    public static class RootRepository 
    {
        public static RootElement Instance { get; private set; }

        static RootRepository() 
        {
            Instance = new RootElement();
        }

        public static async Task<object> SaveAsync()
        {
            return await Sterling.Database.SaveAsync(Instance);
        }

        public static async Task LoadAsync() 
        {
            Instance = await Sterling.Database.LoadAsync<RootElement>(true);
        }
    }
}