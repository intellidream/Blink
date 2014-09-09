using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wintellect.Sterling.Core;
using Wintellect.Sterling.Core.Database;
using Blink.Shared.Domain;
using Blink.Shared.Domain.NewThings;

namespace Blink.Shared.Engine
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

        public static void Activate(Func<ISterlingPlatformAdapter> provideAdapter, Func<ISterlingDriver> provideDriver)
        {
            var platformAdapter = provideAdapter();
            if (platformAdapter == null) throw new ArgumentNullException("provideAdapter");

            var sterlingDriver = provideDriver();
            if (sterlingDriver == null) throw new ArgumentNullException("provideDriver");

            Engine = new SterlingEngine(platformAdapter);
            Logger = new SterlingDefaultLogger(Engine.SterlingDatabase, SterlingLogLevel.Information);
            
            Engine.Activate();

            Database = Engine.SterlingDatabase.RegisterDatabase<BlinkDatabase>("BlinkDatabase", sterlingDriver);
        }

        public static void Deactivate()
        {
            Logger.Detach();
            Engine.Dispose();
            Database = null;
            Engine = null;
        }
    }

    public class BlinkDatabase : BaseDatabaseInstance
    {
        protected override List<ITableDefinition> RegisterTables()
        {
            return new List<ITableDefinition>
            {
                //CreateTableDefinition<Domain.DataModel.Notes.BlinkNote, Guid>(i => i.Id),
                //CreateTableDefinition<Domain.DataModel.Notes.Content, Guid>(i => i.Id),
                //CreateTableDefinition<Domain.DataModel.Notes.Category, Guid>(i => i.Id)

                //CreateTableDefinition<Domain.NewThings.FolderElement, Guid>(f => f.Id),

                CreateTableDefinition<Domain.NewThings.ElementEntity, Guid>(e => e.Id),
                CreateTableDefinition<Domain.NewThings.ConcreteEntity, Guid>(e => e.Id),
                CreateTableDefinition<Domain.NewThings.ValuableEntity, Guid>(e => e.Id),
                CreateTableDefinition<Domain.NewThings.TextEntity, Guid>(e => e.Id),
                CreateTableDefinition<Domain.NewThings.FileEntity, Guid>(e => e.Id)
            };
        }
    }

    public class AuditTrigger<T> : BaseSterlingTrigger<T, Guid> where T : ElementEntity, new()
    {
        public AuditTrigger(ISterlingDatabaseInstance database)
        {
        }

        public override bool BeforeSave(T instance)
        {
            return true;
        }

        public override void AfterSave(T instance)
        {
            return;
        }

        public override bool BeforeDelete(Guid key)
        {
            return true;
        }
    }



    // use own entities or SerializeAs BaseElement, Concrete/Valuable, actual entity's serialization, etc???
}
