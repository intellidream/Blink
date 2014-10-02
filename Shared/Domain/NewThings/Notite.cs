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
    // TODO: rename file to Model.cs

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
        Guid ParentId { get; set; }
        int Position { get; set; }
        Timestamp Timestamp { get; set; }
        ElementTypes ElementType { get; }
        IProgress Progress { get; }
    }

    #endregion

    #region Valuables

    public class Keepable<T> : IElement, IEnumerable<T>, IEnumerable where T : IElement
    {
        #region Private Members

        #region Internal Collection

        private Collection<T> _data;

        #endregion

        #region IElement Members

        private Guid _id;
        private Guid _parentId;
        private int _position;
        private Timestamp _timestamp;
        private IProgress _progress;

        #endregion

        #region Internal Progress

        private InternalProgress<T> _InternalProgress { get; set; }

        #endregion

        #endregion

        #region Public Members

        #region Internal Progress

        public virtual IProgress Progress
        {
            get { return _progress ?? _InternalProgress; }
            set
            {
                if (value != _progress)
                {
                    _progress = value;
                    NotifyPropertyChanged();
                }
            }
        }

        #endregion

        #endregion

        public Keepable()
        {
            _data = new Collection<T>();
            _InternalProgress = new InternalProgress<T>(this);
        }

        #region Collection Indexer
        
        public T this[int i]
        {
            get
            {
                return _data[i];
            }
            set
            {
                value.ParentId = this.Id;
                value.Position = i;
                _data[i] = value;
                //NotifyPropertyChanged("Data");
            }
        }

        #endregion

        #region Collection Methods

        public void Insert(int index, T item)
        {
            item.ParentId = this.Id;
            item.Position = index;
            _data.Insert(index, item);
            //NotifyPropertyChanged("Data");
        }

        public void Add(T item)
        {
            _data.Add(item);
            //NotifyPropertyChanged("Data");
        }

        public void Remove(int index)
        {
            _data.RemoveAt(index);
            //NotifyPropertyChanged("Data");
        }

        public void Remove(T item)
        {
            _data.Remove(item);
            //NotifyPropertyChanged("Data");
        }

        public void Clear()
        {
            _data.Clear();
            //NotifyPropertyChanged("Data");
        }

        public bool Contains(T item)
        {
            return _data.Contains(item);
        }

        public int IndexOf(T item)
        {
            return _data.IndexOf(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        public void CopyTo(T[] array, int index)
        {
            _data.CopyTo(array, index);
        }

        public int Count { get { return _data.Count; } }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        #endregion
        
        #region IElement Members

        public virtual Guid Id
        {
            get { return _id; }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    //NotifyPropertyChanged();
                }
            }
        }

        public virtual Guid ParentId
        {
            get { return _parentId; }
            set
            {
                if (value != _parentId)
                {
                    _parentId = value;
                    //NotifyPropertyChanged();
                }
            }
        }

        public virtual int Position
        {
            get { return _position; }
            set
            {
                if (value != _position)
                {
                    _position = value;
                    //NotifyPropertyChanged();
                }
            }
        }

        public virtual Timestamp Timestamp
        {
            get { return _timestamp; }
            set
            {
                if (!value.Equals(_timestamp))
                {
                    _timestamp = value;
                    //NotifyPropertyChanged();
                }
            }
        }

        public virtual ElementTypes ElementType { get { return ElementTypes.None; } }

        IProgress IElement.Progress { get { return Progress; } }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }

    public class Valuable<T> : Keepable<T>, IValuableEntity<T> where T : IElement
    {
        #region Public Members

        public virtual string Name { get; set; }

        #endregion

        public Valuable() : base() { }

        #region IValuableEntity<T> Members

        public ValuableRecord ToValuableEntity()
        {
            return new ValuableRecord
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

        public ElementRecord ToElementEntity()
        {
            return new ElementRecord
            {
                Id = this.Id,
                ParentId = this.ParentId,
                Position = this.Position,
                TimestampId = this.Timestamp.Id,
                Type = this.ElementType
            };
        }

        public IElement FromElementEntity()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class Selfable<T> : Valuable<T> where T : IElement
    {
        public Keepable<T> Values { get; set; }

        public Selfable()
            : base()
        {
            Values = new Keepable<T>();
        }

        #region Helper Methods

        /// <summary>
        /// http://stackoverflow.com/questions/11830174/how-to-flatten-tree-via-linq/20335369?stw=2#20335369
        /// </summary>
        //public IEnumerable<Selfable<T>> Flatten()
        //{
        //    var stack = new Stack<Selfable<T>>();

        //    stack.Push(this);

        //    while (stack.Count > 0)
        //    {
        //        var current = stack.Pop();

        //        yield return current;

        //        foreach (var child in current)
        //        {
        //            stack.Push(child);
        //        }
        //    }
        //}

        #endregion

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

    public abstract class Concrete : IElement, MGroupable, MNotable, IConcreteEntity
    {
        #region Private Members

        private Guid _id;
        private Guid _parentId;
        private int _position;
        private Timestamp _timestamp;
        private ProgressBase _progress;

        #endregion

        #region Public Members

        public ProgressBase Progress 
        {
            get 
            {
                return _progress; 
            }
            set 
            {
                if (value != _progress)
                {
                    _progress = value;
                    NotifyPropertyChanged();
                }
            } 
        }

        #endregion

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

        public Guid ParentId
        {
            get { return _parentId; }
            set
            {
                if (value != _parentId)
                {
                    _parentId = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public int Position
        {
            get { return _position; }
            set
            {
                if (value != _position)
                {
                    _position = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Timestamp Timestamp
        {
            get { return _timestamp; }
            set
            {
                if (!value.Equals(_timestamp))
                {
                    _timestamp = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public abstract ElementTypes ElementType { get; }

        IProgress IElement.Progress { get { return Progress; } }

        #endregion

        #region IConcreteEntity Members

        public ConcreteRecord ToConcreteEntity()
        {
            return new ConcreteRecord
            {
                Id = this.Id,
                ProgressId = this.Progress.Id
            };
        }

        public Concrete FromConcreteEntity()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IElementEntity Members

        public ElementRecord ToElementEntity()
        {
            return new ElementRecord
            {
                Id = this.Id,
                ParentId = this.ParentId,
                Position = this.Position,
                TimestampId = this.Timestamp.Id,
                Type = this.ElementType
            };
        }

        public IElement FromElementEntity()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }

    public class TextElement : Concrete, ITextEntity
    {
        #region Private Members

        private string _text;

        #endregion

        #region Public Members

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

        #endregion

        #region IElement Members

        public override ElementTypes ElementType { get { return ElementTypes.Text; } }

        #endregion

        #region ITextEntity Members

        public TextRecord ToTextEntity()
        {
            return new TextRecord
            {
                Id = this.Id,
                Text = this.Text
            };
        }

        public TextElement FromTextEntity()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class TweetElement : Concrete
    {
        #region Public Members

        #region Commented Tweet Implementation
        //public TweetSource Source { get; set; } //name, twitterid and icon
        //public TweetContent Content { get; set; } //text and image
        //public DateTime TweetTime { get; set; }
        #endregion

        #endregion

        #region IElement Members

        public override ElementTypes ElementType { get { return ElementTypes.Tweet; } }

        #endregion
    }

    public enum FileTypes
    {
        Other = 0,
        Image = 1,
        Audio = 2,
        Video = 3
    }

    public class FileElement : Concrete, IFileEntity
    {
        #region Public Members

        public string FileName { get; set; }
        public FileTypes FileType { get; set; }
        public string FilePath { get; set; }
        public byte[] FileData { get; set; }

        #endregion

        #region IElement Members

        public override ElementTypes ElementType { get { return ElementTypes.File; } }

        #endregion

        #region IFileEntity Members

        public FileRecord ToFileEntity()
        {
            return new FileRecord
            {
                FileName = this.FileName,
                FileType = this.FileType,
                FilePath = this.FilePath,
                FileData = this.FileData
            };
        }

        public FileElement FromFileEntity()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    #endregion

    #region Containers

    public class ListElement : Valuable<Concrete>, MGroupable, MNotable
    {
        #region IElement Members

        public override ElementTypes ElementType { get { return ElementTypes.List; } }

        #endregion
    }

    public class GridElement : Valuable<ListElement>, MGroupable, MNotable
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

        public override ElementTypes ElementType { get { return ElementTypes.Grid; } }

        #endregion
    }

    public class TreeElement : Selfable<Concrete>, MGroupable, MNotable
    {
        #region IElement Members

        public override ElementTypes ElementType { get { return ElementTypes.Tree; } }

        #endregion
    }

    #endregion

    #region Groupables

    public interface MGroupable : IElement { }

    public class GroupElement : Valuable<MGroupable>, MNotable
    {
        #region IElement Members

        public override ElementTypes ElementType { get { return ElementTypes.Group; } }

        #endregion
    }

    #endregion

    #region Notables

    public interface MNotable : IElement { }

    public class NoteElement : Valuable<MNotable>, MPageable, MFoldable
    {
        #region IElement Members

        public override ElementTypes ElementType { get { return ElementTypes.Note; } }

        #endregion
    }

    #endregion

    #region Pageables

    public interface MPageable : IElement { }

    public class PageElement : Valuable<MPageable>, MFoldable
    {
        #region IElement Members

        public override ElementTypes ElementType { get { return ElementTypes.Page; } }

        #endregion
    }

    #endregion

    #region Foldables

    public interface MFoldable : IElement { }

    public class FolderElement : Selfable<MFoldable>, MFoldable, IRootable
    {
        #region IElement Members

        private ElementTypes? type;
        new public ElementTypes ElementType
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
        new ElementTypes ElementType { get; set; }
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

    public abstract class ProgressBase : IProgress, IProgressEntity
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

        #region IProgressEntity Members

        public ProgressRecord ToProgressEntity()
        {
            return new ProgressRecord
            {
                Id = this.Id,
                ProgressType = this.ProgressType
            };
        }

        public IProgress FromProgressEntity()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class InternalProgress<T> : IProgress where T : IElement
    {
        #region Private Members

        public Keepable<T> Parent { get; set; }

        private IList<IProgress> _Values 
        {
            get { return (Parent != null) ? Parent.Select(e => e.Progress).ToList() : null; } 
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

        #endregion

        public InternalProgress() { }

        public InternalProgress(Keepable<T> parent)
        {
            Parent = parent;
        }
        
        #region IProgress Members

        public Guid Id { get; set; }

        public ProgressTypes ProgressType 
        {
            get { return ProgressTypes.Internal; } 
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

        public bool IsCompleted()
        {
            return _HasValues && (_Completed == _Total);
        }

        #endregion
    }

    public class ManualProgress : ProgressBase, IManualProgressEntity
    {
        #region Public Members

        public bool Completed { get; set; }

        #endregion

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

        #region IManualProgressEntity Members

        public ManualProgressRecord ToManualProgressEntity()
        {
            return new ManualProgressRecord 
            { 
                Id = this.Id, 
                Completed = this.Completed 
            };
        }

        public ManualProgress FromManualProgressEntity()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class DateTimeProgress : ProgressBase, IDateTimeProgressEntity
    {
        #region Public Members

        public DateTime Completion { get; set; }

        #endregion

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

        #region IDateTimeProgressEntity Members

        public DateTimeProgressRecord ToDateTimeProgressEntity()
        {
            return new DateTimeProgressRecord 
            {
                Id = this.Id,
                Completion = this.Completion
            };
        }

        public DateTimeProgress FromDateTimeProgressEntity()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    public class LocationProgress : ProgressBase, ILocationProgressEntity
    {
        #region Public Members

        public Tuple<double, double> Current { get; set; }
        public Tuple<double, double> Destination { get; set; }

        #endregion

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

        #region ILocationProgressEntity Members

        public LocationProgressRecord ToLocationProgressEntity()
        {
            return new LocationProgressRecord
            {
                Id = this.Id,
                Current = this.Current,
                Destination = this.Destination
            };
        }

        public LocationProgress FromLocationProgressEntity()
        {
            throw new NotImplementedException();
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