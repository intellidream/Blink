using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wintellect.Sterling.Core;

namespace Blink.Engine
{
    public static class Sterling
    {
        public static ISterlingDatabaseInstance Database { get; private set; }
        public static SterlingEngine Engine { get; private set; }
        public static SterlingDefaultLogger Logger { get; private set; }

        static Sterling()
        {
            Database = null;
            Logger = null;
            Engine = null;
        }

        public static void ActivateEngine(Func<ISterlingDatabaseInstance> registerDatabases)
        {
            Engine = new SterlingEngine(Engine.PlatformAdapter);
            Logger = new SterlingDefaultLogger(Engine.SterlingDatabase, SterlingLogLevel.Information);
            Database = registerDatabases();//?!?-listof?!?
            Engine.Activate();
        }


    }
}
