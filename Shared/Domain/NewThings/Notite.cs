using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace Blink.Shared.Domain.NewThings
{
    #region Elements

    public enum ElementTypes : int
    {
        None,
        Text,
        Tweet,
        File,
        List,
        Grid,
        Tree,
        Group,
        Note,
        Page,
        Folder,
        Root
    }

    public interface IElement : INotifyPropertyChanged
    {
        Guid Id { get; set; }
        Guid SyncId { get; set; }
        Guid ParentId { get; set; }
        int Position { get; set; }
        Timestamp Timestamp { get; set; }
        ElementTypes Type { get; }
        IProgress Progress { get; }
    }

    #endregion

    #region Valuables

    public class Keepable<T> : Collection<T>, IElement where T : IElement
    {
        #region Private Members

        private Guid _id;
        private Guid _parentId;
        private int _position;
        private Timestamp _timestamp;
        private ProgressBase _progress;

        #endregion

        private InternalProgress<T> _InternalProgress { get; set; }
        private IProgress _Progress { get; set; }
        public virtual IProgress Progress
        {
            get { return _Progress ?? _InternalProgress; }
            set { _Progress = value; }
        }

        public Keepable() 
        {
            _InternalProgress = new InternalProgress<T>(this);
        }

        #region IElement Members

        public virtual Guid Id
        {
            get { return _id; }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Guid SyncId { get; set; }

        public Guid ParentId { get; set; }

        public int Position { get; set; }

        public Timestamp Timestamp { get; set; }

        public virtual ElementTypes Type { get { return ElementTypes.None; } }

        IProgress IElement.Progress { get { return Progress; } }

        #endregion

        #region Collection Overrides

        protected override void SetItem(int index, T item)
        {
            item.ParentId = this.Id;
            item.Position = index;
            base.SetItem(index, item);
            NotifyPropertyChanged("Items");
        }

        protected override void InsertItem(int index, T item)
        {
            item.ParentId = this.Id;
            item.Position = index;
            base.InsertItem(index, item);
            NotifyPropertyChanged("Items");
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
            NotifyPropertyChanged("Items");
        }

        protected override void ClearItems()
        {
            base.ClearItems();
            NotifyPropertyChanged("Items");
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                SyncId = Guid.Empty; //Device SyncId here
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }

    public class Valuable<T> : Keepable<T>, IValuableEntity<T> where T : IElement
    {
        public virtual string Name { get; set; }
        
        #region IValuableEntity<T> Members

        public ValuableEntity ToValuableEntity()
        {
            return new ValuableEntity
            {
                Id = this.Id,
                Name = this.Name
            };
        }

        public Valuable<T> FromValuableEntity()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IElementEntity Members

        public ElementEntity ToElementEntity()
        {
            return new ElementEntity
            {
                Id = this.Id,
                ParentId = this.ParentId,
                Position = this.Position,
                TimestampId = this.Timestamp.Id,
                Type = this.Type
            };
        }

        public IElement FromElementEntity()
        {
            throw new NotImplementedException();
        }

        #endregion
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

        public override IProgress Progress { get { return Values.Progress; } }

        #endregion
    }

    #endregion

    #region Concretes

    public abstract class Concrete : IElement, IGroupable, INotable
    {
        #region Private Members

        private Guid _id;
        private Guid _parentId;
        private int _position;
        private Timestamp _timestamp;
        private ProgressBase _progress;

        #endregion

        public ProgressBase Progress { get; set; }

        #region IElement Members

        public Guid Id 
        {
            get { return _id; }
            set 
            {
                if (value != _id)
                {
                    _id = value;
                    NotifyPropertyChanged();
                }
            } 
        }

        public Guid SyncId { get; set; }

        public Guid ParentId { get; set; }

        public int Position { get; set; }

        public Timestamp Timestamp { get; set; }

        public abstract ElementTypes Type { get; }

        IProgress IElement.Progress { get { return Progress; } }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                SyncId = Guid.Empty; //Device SyncId here
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }

    public class TextElement : Concrete, IConcreteEntity<TextEntity>
    {
        #region Private Members

        private string _text;

        #endregion

        public string Text
        {
            get { return _text; }
            set
            {
                if (value != _text)
                {
                    _text = value;
                    NotifyPropertyChanged();
                }
            }
        }

        #region IElement Members

        public override ElementTypes Type { get { return ElementTypes.Text; } }

        #endregion

        #region IConcreteEntity<TextEntity> Members

        public ConcreteEntity ToConcreteBaseEntity()
        {
            return new ConcreteEntity 
            {
                Id = this.Id,
                ProgressId = this.Progress.Id
            };
        }

        public TextEntity ToConcreteTypeEntity()
        {
            return new TextEntity 
            {
                Id = this.Id,
                Text = this.Text
            };
        }

        #endregion

        #region IElementEntity Members

        public ElementEntity ToElementEntity()
        {
            return new ElementEntity
            {
                Id = this.Id,
                ParentId = this.ParentId,
                Position = this.Position,
                TimestampId = this.Timestamp.Id,
                Type = ElementTypes.Text
            };
        }

        public IElement FromElementEntity()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class TweetElement : Concrete
    {
        #region Commented Tweet Implementation
        //public TweetSource Source { get; set; } //name, twitterid and icon
        //public TweetContent Content { get; set; } //text and image
        //public DateTime TweetTime { get; set; }
        #endregion

        #region IElement Members

        public override ElementTypes Type { get { return ElementTypes.Tweet; } }

        #endregion
    }

    public class FileElement : Concrete
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

        #region IElement Members

        public override ElementTypes Type { get { return ElementTypes.File; } }

        #endregion
    }

    #endregion

    #region Containers

    public class ListElement : Valuable<Concrete>, IGroupable, INotable
    {
        #region IElement Members

        public override ElementTypes Type { get { return ElementTypes.List; } }

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

        #region IElement Members

        public override ElementTypes Type { get { return ElementTypes.Grid; } }

        #endregion
    }

    public class TreeElement : Selfable<Concrete>, IGroupable, INotable
    {
        #region IElement Members

        public override ElementTypes Type { get { return ElementTypes.Tree; } }

        #endregion
    }

    #endregion

    #region Groupables

    public interface IGroupable : IElement { }

    public class GroupElement : Valuable<IGroupable>, INotable
    {
        #region IElement Members

        public override ElementTypes Type { get { return ElementTypes.Group; } }

        #endregion
    }

    #endregion

    #region Notables

    public interface INotable : IElement { }

    public class NoteElement : Valuable<INotable>, IPageable, IFoldable
    {
        #region IElement Members

        public override ElementTypes Type { get { return ElementTypes.Note; } }

        #endregion
    }

    #endregion

    #region Pageables

    public interface IPageable : IElement { }

    public class PageElement : Valuable<IPageable>, IFoldable
    {
        #region IElement Members

        public override ElementTypes Type { get { return ElementTypes.Page; } }

        #endregion
    }

    #endregion

    #region Foldables

    public interface IFoldable : IElement { }

    public class FolderElement : Selfable<IFoldable>, IRootable
    {
        #region IElement Members

        private ElementTypes? type;
        new public ElementTypes Type
        {
            get
            {
                return type.HasValue ? type.Value : ElementTypes.Folder;
            }

            set { type = value; }
        }

        #endregion
    }

    #endregion

    #region Rootables

    public interface IRootable : IElement 
    {
        new ElementTypes Type { get; set; }
    }

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

    public struct Timestamp
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public DateTime Accessed { get; set; }
    }

    #endregion

    #region Progressing

    public enum ProgressTypes : int
    {
        Manual,
        DateTime,
        Location,
        Internal
    }

    public interface IProgress
    {
        Guid Id { get; set; }

        ProgressTypes ProgressType { get; }

        int Percentage { get; }

        bool IsCompleted();
    }

    public abstract class ProgressBase : IProgress
    {
        #region IProgress Members

        public Guid Id { get; set; }

        public abstract ProgressTypes ProgressType { get; }

        public virtual int Percentage 
        {
            get 
            {
                return IsCompleted() ? 100 : 0;
            } 
        }

        public abstract bool IsCompleted();

        #endregion
    }

    public class InternalProgress<T> : IProgress where T : IElement
    {
        private IList<IProgress> _Values 
        {
            get { return (_Parent != null) ? _Parent.Select(e => e.Progress).ToList() : null; } 
        }

        private bool _HasValues 
        {
            get { return ((_Values != null) && (_Values.Count > 0)); } 
        }

        private int _Total { get { return _HasValues ? _Values.Count : 0; } }

        private int _Completed
        {
            get
            {
                return (this._Total > 0)
                        ? _Values.Count(p => p != null && p.IsCompleted())
                        : 0;
            }
        }

        public int Percentage
        {
            get
            {
                var completed = this._Completed;

                return (completed > 0)
                        ? (int)Math.Round((double)(100 * completed) / _Total)
                        : 0;
            }
        }

        private Keepable<T> _Parent { get; set; }

        private InternalProgress() { }

        public InternalProgress(Keepable<T> parent)
        {
            _Parent = parent;
        }
        
        #region IProgress Members

        public Guid Id { get; set; }

        public ProgressTypes ProgressType 
        {
            get { return ProgressTypes.Internal; } 
        }

        public bool IsCompleted()
        {
            return _HasValues && (_Completed == _Total);
        }

        #endregion
    }

    public class ManualProgress : ProgressBase
    {
        public bool Completed { get; set; }

        #region IProgress Members

        public override ProgressTypes ProgressType
        {
            get { return ProgressTypes.Manual; }
        }

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

        public override ProgressTypes ProgressType
        {
            get { return ProgressTypes.DateTime; }
        }

        public override bool IsCompleted()
        {
            return Completion.ToUniversalTime().Equals(DateTime.UtcNow);
        }

        #endregion
    }
    public class LocationProgress : ProgressBase
    {
        public Tuple<double, double> Current { get; set; }
        public Tuple<double, double> Destination { get; set; }
        
        #region IProgress Members

        public override ProgressTypes ProgressType
        {
            get { return ProgressTypes.Location; }
        }

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
