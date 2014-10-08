using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wintellect.Sterling.Core;
using Wintellect.Sterling.Core.Database;
using Blink.Data.Domain.Model;
using Wintellect.Sterling.Core.Serialization;
using Blink.Data.Domain.Support;

namespace Blink.Data.Engine
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

            Engine.SterlingDatabase.RegisterSerializer<TupleOfTwoDoubleSerializer>();

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
                // Progress
                CreateTableDefinition<ManualProgress, Guid>(e => e.Id),
                CreateTableDefinition<DateTimeProgress, Guid>(e => e.Id),
                CreateTableDefinition<LocationProgress, Guid>(e => e.Id),

                // Concretes
                CreateTableDefinition<TextElement, Guid>(e => e.Id),
                CreateTableDefinition<TweetElement, Guid>(e => e.Id),
                CreateTableDefinition<FileElement, Guid>(e => e.Id),
                // Containers
                CreateTableDefinition<ListElement, Guid>(e => e.Id),
                CreateTableDefinition<GridElement, Guid>(e => e.Id),
                CreateTableDefinition<TreeElement, Guid>(e => e.Id),
                // Groupables
                CreateTableDefinition<GroupElement, Guid>(e => e.Id),
                // Notables
                CreateTableDefinition<NoteElement, Guid>(e => e.Id),
                // Pageables
                CreateTableDefinition<PageElement, Guid>(e => e.Id),
                // Foldables
                CreateTableDefinition<FolderElement, Guid>(e => e.Id),
                // Rootables
                CreateTableDefinition<RootElement, bool>(c => true)
            };
        }
    }

    public class TupleOfTwoDoubleSerializer : BaseSerializer
    {
        public override bool CanSerialize(Type targetType)
        {
            return targetType.Equals(typeof(Tuple<double, double>));
        }

        public override void Serialize(object target, System.IO.BinaryWriter writer)
        {
            var data = (Tuple<double, double>)target;
            writer.Write(data.Item1);
            writer.Write(data.Item2);
        }

        public override object Deserialize(Type type, System.IO.BinaryReader reader)
        {
            return new Tuple<double, double>(reader.ReadDouble(), reader.ReadDouble());
        }
    }

    //public class AuditTrigger<T> : BaseSterlingTrigger<T, Guid> where T : IElement, new()
    //{
    //    public AuditTrigger(ISterlingDatabaseInstance database)
    //    {
    //    }

    //    public override bool BeforeSave(T instance)
    //    {
    //        return true;
    //    }

    //    public override void AfterSave(T instance)
    //    {
    //        return;
    //    }

    //    public override bool BeforeDelete(Guid key)
    //    {
    //        return true;
    //    }
    //}
}