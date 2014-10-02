using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blink.Shared.Domain.NewThings
{
    // TODO: rename file to Data.cs

    // TODO: see Lazy<T> on deserializing nested stuff!

    // TODO: see if using AutoMapper to map between entities and data is an option!

    // TODO: collection + properties composing wrappers instead of derived collections and observable collections in PCL!

    #region Contracts

    public interface IElementEntity
    {
        ElementRecord ToElementEntity();
        IElement FromElementEntity();
    }

    public interface IValuableEntity<T> : IElementEntity where T : IElement
    {
        ValuableRecord ToValuableEntity();
        Valuable<T> FromValuableEntity();
    }

    public interface IConcreteEntity : IElementEntity
    {
        ConcreteRecord ToConcreteEntity();
        Concrete FromConcreteEntity();
    }

    public interface ITextEntity 
    {
        TextRecord ToTextEntity();
        TextElement FromTextEntity();
    }

    public interface IFileEntity
    {
        FileRecord ToFileEntity();
        FileElement FromFileEntity();
    }

    public interface IProgressEntity
    {
        ProgressRecord ToProgressEntity();
        IProgress FromProgressEntity();
    }

    public interface IManualProgressEntity
    {
        ManualProgressRecord ToManualProgressEntity();
        ManualProgress FromManualProgressEntity();
    }

    public interface IDateTimeProgressEntity
    {
        DateTimeProgressRecord ToDateTimeProgressEntity();
        DateTimeProgress FromDateTimeProgressEntity();
    }

    public interface ILocationProgressEntity
    {
        LocationProgressRecord ToLocationProgressEntity();
        LocationProgress FromLocationProgressEntity();
    }

    #endregion

    #region Entities

    public class ElementRecord
    {
        public Guid Id { get; set; }

        public Guid ParentId { get; set; }

        public int Position { get; set; }

        public Guid TimestampId { get; set; }

        public ElementTypes Type { get; set; }
    }

    public class ValuableRecord
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }

    public class ConcreteRecord
    {
        public Guid Id { get; set; }

        public Guid ProgressId { get; set; }
    }

    public class TextRecord
    {
        public Guid Id { get; set; }

        public string Text { get; set; }
    }

    public class FileRecord
    {
        public Guid Id { get; set; }

        public string FileName { get; set; }

        public FileTypes FileType { get; set; }

        public string FilePath { get; set; }

        public byte[] FileData { get; set; }
    }

    public class ProgressRecord 
    {
        public Guid Id { get; set; }

        public ProgressTypes ProgressType { get; set; }
    }

    public class ManualProgressRecord 
    {
        public Guid Id { get; set; }

        public bool Completed { get; set; }
    }

    public class DateTimeProgressRecord
    {
        public Guid Id { get; set; }

        public DateTime Completion { get; set; }
    }

    public class LocationProgressRecord
    {
        public Guid Id { get; set; }

        public Tuple<double, double> Current { get; set; }
        public Tuple<double, double> Destination { get; set; }
    }

    #endregion
}





//namespace ClassLibrary1
//{
//    public enum ProgressTypes : int
//    {
//        Manual,
//        DateTime,
//        Location,
//        Internal
//    }

//    public interface IProgress
//    {
//        Guid Id { get; set; }

//        ProgressTypes ProgressType { get; }

//        int Percentage { get; }

//        bool IsCompleted();
//    }

//    public struct Timestamp
//    {
//        public Guid Id { get; set; }
//        public DateTime Created { get; set; }
//        public DateTime Modified { get; set; }
//        public DateTime Accessed { get; set; }
//    }

//    public enum ElementTypes : int
//    {
//        None,
//        Text,
//        Tweet,
//        File,
//        List,
//        Grid,
//        Tree,
//        Group,
//        Note,
//        Page,
//        Folder,
//        Root
//    }

//    public interface IElement
//    {
//        Guid Id { get; set; }
//        Guid ParentId { get; set; }
//        int Position { get; set; }
//        Timestamp Timestamp { get; set; }
//        ElementTypes ElementType { get; }
//        IProgress Progress { get; }
//    }

//    public class Keepable<T> : IElement where T : IElement
//    {
//        public Collection<T> Values { get; private set; }

//        public Keepable()
//        {
//            Values = new Collection<T>();
//        }

//        #region Collection Indexer

//        public T this[int i]
//        {
//            get
//            {
//                return Values[i];
//            }
//            set
//            {
//                value.ParentId = this.Id;
//                value.Position = i;
//                Values[i] = value;
//                //NotifyPropertyChanged("Items");
//            }
//        }

//        #endregion

//        #region Collection Methods

//        public void Insert(int index, T item)
//        {
//            item.ParentId = this.Id;
//            item.Position = index;
//            Values.Insert(index, item);
//            //NotifyPropertyChanged("Items");
//        }

//        public void Add(T item)
//        {
//            Values.Add(item);
//            //NotifyPropertyChanged("Items");
//        }

//        public void Remove(int index)
//        {
//            Values.RemoveAt(index);
//            //NotifyPropertyChanged("Items");
//        }

//        public void Remove(T item)
//        {
//            Values.Remove(item);
//            //NotifyPropertyChanged("Items");
//        }

//        public void Clear()
//        {
//            Values.Clear();
//            //NotifyPropertyChanged("Items");
//        }

//        public bool Contains(T item)
//        {
//            return Values.Contains(item);
//        }

//        public int IndexOf(T item)
//        {
//            return Values.IndexOf(item);
//        }

//        public IEnumerator<T> GetEnumerator()
//        {
//            return Values.GetEnumerator();
//        }

//        public void CopyTo(T[] array, int index)
//        {
//            Values.CopyTo(array, index);
//        }

//        #endregion

//        #region IElement Members

//        public virtual Guid Id { get; set; }

//        public Guid ParentId { get; set; }

//        public int Position { get; set; }

//        public Timestamp Timestamp { get; set; }

//        public ElementTypes ElementType { get { return ElementTypes.None; } }

//        public virtual IProgress Progress
//        {
//            get { throw new NotImplementedException(); }
//        }

//        #endregion
//    }

//    public class Valuable<T> : Keepable<T> where T : IElement
//    {
//        #region Public Members

//        public virtual string Name { get; set; }

//        #endregion
//    }

//    public class Selfable<T> : Valuable<Selfable<T>> where T : IElement
//    {
//        public Keepable<T> Values { get; set; } //???

//        public Selfable()
//            : base()
//        {
//            Values = new Keepable<T>();
//        }

//        #region Helper Methods

//        /// <summary>
//        /// http://stackoverflow.com/questions/11830174/how-to-flatten-tree-via-linq/20335369?stw=2#20335369
//        /// </summary>
//        public IEnumerable<Selfable<T>> Flatten()
//        {
//            var stack = new Stack<Selfable<T>>();

//            stack.Push(this);

//            while (stack.Count > 0)
//            {
//                var current = stack.Pop();

//                yield return current;

//                foreach (var child in current)
//                {
//                    stack.Push(child);
//                }
//            }
//        }

//        #endregion

//        #region Keepable Members

//        #region IElement Members

//        public override Guid Id
//        {
//            get { return base.Id; }
//            set
//            {
//                Values.Id = value;
//                base.Id = value;
//            }
//        }

//        #endregion

//        public override IProgress Progress { get { return Values.Progress; } }

//        #endregion
//    }
//}