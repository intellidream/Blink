using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wintellect.Sterling.Core;
using Wintellect.Sterling.Core.Database;
using Blink.Shared.Domain;
using Blink.Shared.Domain.NewThings;
using Wintellect.Sterling.Core.Serialization;

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

            //Engine.SterlingDatabase.RegisterSerializer<FolderSerializer>();
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
                //CreateTableDefinition<Domain.DataModel.Notes.BlinkNote, Guid>(i => i.Id),
                //CreateTableDefinition<Domain.DataModel.Notes.Content, Guid>(i => i.Id),
                //CreateTableDefinition<Domain.DataModel.Notes.Category, Guid>(i => i.Id)




                // Progress
                CreateTableDefinition<Domain.NewThings.ManualProgress, Guid>(e => e.Id),
                CreateTableDefinition<Domain.NewThings.DateTimeProgress, Guid>(e => e.Id),
                CreateTableDefinition<Domain.NewThings.LocationProgress, Guid>(e => e.Id),

                // Concretes
                CreateTableDefinition<Domain.NewThings.TextElement, Guid>(e => e.Id),
                CreateTableDefinition<Domain.NewThings.TweetElement, Guid>(e => e.Id),
                CreateTableDefinition<Domain.NewThings.FileElement, Guid>(e => e.Id),
                // Containers
                CreateTableDefinition<Domain.NewThings.ListElement, Guid>(e => e.Id),
                CreateTableDefinition<Domain.NewThings.GridElement, Guid>(e => e.Id),
                CreateTableDefinition<Domain.NewThings.TreeElement, Guid>(e => e.Id),
                // Groupables
                CreateTableDefinition<Domain.NewThings.GroupElement, Guid>(e => e.Id),
                // Notables
                CreateTableDefinition<Domain.NewThings.NoteElement, Guid>(e => e.Id),
                // Pageables
                CreateTableDefinition<Domain.NewThings.PageElement, Guid>(e => e.Id),
                // Foldables
                CreateTableDefinition<Domain.NewThings.FolderElement, Guid>(e => e.Id),




                




                //CreateTableDefinition<Domain.NewThings.ElementEntity, Guid>(e => e.Id),
                //CreateTableDefinition<Domain.NewThings.ConcreteEntity, Guid>(e => e.Id),
                //CreateTableDefinition<Domain.NewThings.ValuableEntity, Guid>(e => e.Id),
                //CreateTableDefinition<Domain.NewThings.TextEntity, Guid>(e => e.Id),
                //CreateTableDefinition<Domain.NewThings.FileEntity, Guid>(e => e.Id)
            };
        }
    }

    public class AuditTrigger<T> : BaseSterlingTrigger<T, Guid> where T : ElementRecord, new()
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

    //public class FolderSerializer : BaseSerializer 
    //{
    //    public override bool CanSerialize(Type targetType)
    //    {
    //        return targetType.Equals(typeof(FolderElement));
    //    }

    //    public override void Serialize(object target, System.IO.BinaryWriter writer)
    //    {
    //        var data = (FolderElement)target;
    //        //writer.Write(data.Id);
    //        writer.Write(data.Name);
    //    }

    //    public override object Deserialize(Type type, System.IO.BinaryReader reader)
    //    {
    //        return new FolderElement
    //                            {
    //                                //Id = ,
    //                                Name = reader.ReadString()
    //                            };
    //    }
    //}

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

    // use own entities or SerializeAs BaseElement, Concrete/Valuable, actual entity's serialization, etc???
}
