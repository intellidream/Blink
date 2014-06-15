using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Blink.Shared.Domain.NewThings
{
    #region Elements

    public enum ElementTypes : int
    {
        Concrete,
        Valuable,
        Rootable
    }

    public interface IElement
    {
        Guid Id { get; set; }
        Guid ParentId { get; set; }
        int Position { get; set; }
        Timestamp Timestamp { get; set; }
        ElementTypes Type { get; }
        IProgress Progress { get; }
    }

    #endregion

    #region Valuables

    public enum ValuableTypes : int
    {
        List,
        Grid,
        Tree,
        Group,
        Note,
        Page,
        Folder
    }

    public class Keepable<T> : Collection<T>, IElement where T : IElement
    {
        public virtual ProgressCollection Progress { get; private set; }

        #region IElement Members

        public virtual Guid Id { get; set; }

        public virtual Guid ParentId { get; set; }

        public virtual int Position { get; set; }

        public virtual Timestamp Timestamp { get; set; }

        ElementTypes IElement.Type { get { return ElementTypes.Valuable; } }

        IProgress IElement.Progress { get { return Progress; } }

        #endregion

        #region Collection Overrides

        protected override void SetItem(int index, T item)
        {
            item.ParentId = this.Id;
            Progress[index] = item.Progress;
            base.SetItem(index, item);
        }

        protected override void InsertItem(int index, T item)
        {
            item.ParentId = this.Id;
            Progress.Insert(index, item.Progress);
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            Progress.RemoveAt(index);
            base.RemoveItem(index);
        }

        protected override void ClearItems()
        {
            Progress.Clear();
            base.ClearItems();
        }

        #endregion
    }

    public class Valuable<T> : Keepable<T> where T : IElement
    {
        public virtual string Name { get; set; }

        public virtual ValuableTypes Type { get; private set; }
    }

    public class Selfable<T> : Valuable<Selfable<T>> where T : IElement
    {
        public Keepable<T> Values { get; set; }

        public Selfable()
            : base()
        {
            Values = new Keepable<T>();
        }

        /// <summary>
        /// http://stackoverflow.com/questions/11830174/how-to-flatten-tree-via-linq/20335369?stw=2#20335369
        /// </summary>
        public IEnumerable<Selfable<T>> Flatten()
        {
            var stack = new Stack<Selfable<T>>();

            stack.Push(this);

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                yield return current;

                foreach (var child in current)
                {
                    stack.Push(child);
                }
            }
        }

        #region Keepable Members

        #region IElement Members

        public override Guid Id
        {
            get { return base.Id; }
            set
            {
                Values.Id = value;
                base.Id = value;
            }
        }

        #endregion

        public override ProgressCollection Progress { get { return Values.Progress; } }

        #endregion
    }

    #endregion

    #region Concretes

    public enum ConcreteTypes : int
    {
        Text,
        Tweet,
        File
    }

    public abstract class ConcreteBase : IElement, IGroupable, INotable
    {
        public abstract ConcreteTypes Type { get; }

        public abstract ProgressBase Progress { get; set; }

        #region IElement Members

        public virtual Guid Id { get; set; }

        public virtual Guid ParentId { get; set; }

        public virtual int Position { get; set; }

        public virtual Timestamp Timestamp { get; set; }

        ElementTypes IElement.Type { get { return ElementTypes.Concrete; } }

        IProgress IElement.Progress { get { return Progress; } }

        #endregion
    }

    public class TextElement : ConcreteBase
    {
        public string Text { get; set; }

        #region ConcreteBase Members

        public override ConcreteTypes Type { get { return ConcreteTypes.Text; } }

        public override ProgressBase Progress { get; set; }

        #endregion
    }

    public class TweetElement : ConcreteBase
    {
        #region Commented Tweet Implementation
        //public TweetSource Source { get; set; } //name, twitterid and icon
        //public TweetContent Content { get; set; } //text and image
        //public DateTime TweetTime { get; set; }
        #endregion

        #region ConcreteBase Members

        public override ConcreteTypes Type { get { return ConcreteTypes.Text; } }

        public override ProgressBase Progress { get; set; }

        #endregion
    }

    public class FileElement : ConcreteBase
    {
        public string FileName { get; set; }
        public FileTypes FileType { get; set; }

        public string FilePath { get; set; }
        public byte[] FileData { get; set; }

        public enum FileTypes
        {
            Other = 0,
            Image = 1,
            Audio = 2,
            Video = 3
        }

        #region ConcreteBase Members

        public override ConcreteTypes Type { get { return ConcreteTypes.Text; } }

        public override ProgressBase Progress { get; set; }

        #endregion
    }

    #endregion

    #region Containers

    public class ListElement : Valuable<ConcreteBase>, IGroupable, INotable
    {
        #region Valuable Members

        public override ValuableTypes Type
        {
            get
            {
                return ValuableTypes.List;
            }
        }

        #endregion
    }

    public class GridElement : Valuable<ListElement>, IGroupable, INotable
    {
        public ListElement this[string name]
        {
            get
            {
                return this.First(l => l.Name == name);
            }
            set
            {
                var element = this.First(l => l.Name == name);
                element = value;
            }
        }

        #region Valuable Members

        public override ValuableTypes Type
        {
            get
            {
                return ValuableTypes.Grid;
            }
        }

        #endregion
    }

    public class TreeElement : Selfable<ConcreteBase>, IGroupable, INotable
    {
        #region Valuable Members

        public override ValuableTypes Type
        {
            get
            {
                return ValuableTypes.Tree;
            }
        }

        #endregion
    }

    #endregion

    #region Groupables

    public interface IGroupable : IElement { }

    public class GroupElement : Valuable<IGroupable>, INotable
    {
        #region Valuable Members

        public override ValuableTypes Type
        {
            get
            {
                return ValuableTypes.Group;
            }
        }

        #endregion
    }

    #endregion

    #region Notables

    public interface INotable : IElement { }

    public class NoteElement : Valuable<INotable>, IPageable, IFoldable
    {
        #region Valuable Members

        public override ValuableTypes Type
        {
            get
            {
                return ValuableTypes.Note;
            }
        }

        #endregion
    }

    #endregion

    #region Pageables

    public interface IPageable : IElement { }

    public class PageElement : Valuable<IPageable>, IFoldable
    {
        #region Valuable Members

        public override ValuableTypes Type
        {
            get
            {
                return ValuableTypes.Page;
            }
        }

        #endregion
    }

    #endregion

    #region Foldables

    public interface IFoldable : IElement { }

    public class FolderElement : Selfable<IFoldable>, IRootable
    {
        #region Valuable Members

        public override ValuableTypes Type
        {
            get
            {
                return ValuableTypes.Folder;
            }
        }

        #endregion
    }

    #endregion

    #region Rootables

    public interface IRootable : IElement { }

    public sealed class RootElement : Valuable<IRootable>
    {
        private static readonly Lazy<RootElement> lazy =
            new Lazy<RootElement>(() => new RootElement());

        public static RootElement Instance { get { return lazy.Value; } }

        public bool IsReady { get { return lazy.IsValueCreated; } }

        private RootElement() { }
    } 

    #endregion

    #region Timestamping

    public class Timestamp
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public DateTime Accessed { get; set; }
    }

    #endregion

    #region Progressing

    public interface IProgress
    {
        bool IsCompleted();
    }

    public abstract class ProgressBase : IProgress
    {
        #region IProgress Members

        public abstract bool IsCompleted();

        #endregion

        public virtual Guid Id { get; set; }

        public virtual Guid ParentId { get; set; }
    }

    public class ProgressCollection : Collection<IProgress>, IProgress
    {
        #region IProgress Members

        public bool IsCompleted()
        {
            return (this.Total > 0)
                        ? this.All(p => p.IsCompleted())
                        : false;
        }

        #endregion

        public int Total { get { return this.Count; } }
        public int Completed
        {
            get
            {
                return (this.Total > 0)
                        ? this.Count(p => p.IsCompleted())
                        : 0;
            }
        }

        public int Percentage 
        {
            get 
            {
                return (this.Total > 0)
                        ? (int)Math.Round((double)(100 * Completed) / Total)
                        : 0;
            }
        }
    }

    public class InternalProgress : ProgressBase
    {
        public bool Completed { get; set; }

        #region IProgress Members

        public override bool IsCompleted()
        {
            return Completed;
        }

        #endregion
    }

    public class DateTimeProgress : ProgressBase
    {
        public DateTime Completion { get; set; }
        
        #region IProgress Members

        public override bool IsCompleted()
        {
            return Completion.ToUniversalTime().Equals(DateTime.UtcNow);
        }

        #endregion
    }
    public class LocationProgress : ProgressBase
    {
        Tuple<double, double> Current { get; set; }
        Tuple<double, double> Destination { get; set; }
        
        #region IProgress Members

        public override bool IsCompleted()
        {
            return Current.Equals(Destination);
        }

        #endregion
    }

    #endregion

    #region Scheduling

    public interface ISchedule 
    {
        Guid Id { get; set; }
        Guid ParentId { get; set; }
        DateTime Value { get; set; } 
    }

    #endregion
}
