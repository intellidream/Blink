using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Blink.Data.Domain.Infrastructure;

namespace Blink.Data.Domain.Model
{
    #region Elements

    public enum ElementTypes : int
    {
        None = 0,
        Text = 1,
        Byte = 2,
        Twit = 3,
        File = 4,
        List = 5,
        Grid = 6,
        Tree = 7,
        Group = 8,
        Note = 9,
        Page = 10,
        Folder = 11,
        Root = 12
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

    #region Concretes

    #region Foundation

    public abstract class Concrete : IElement, IGroupable, INotable
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

        #region IGroupable Members

        public abstract GroupableTypes GroupableType { get; }

        #endregion

        #region INotable Members

        public abstract NotableTypes NotableType { get; }

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

    #endregion

    public class TextElement : Concrete
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

        #region IGroupable Members

        public override GroupableTypes GroupableType { get { return GroupableTypes.Text; } }

        #endregion
        
        #region INotable Members

        public override NotableTypes NotableType { get { return NotableTypes.Text; } }

        #endregion
    }

    public class ByteElement : Concrete
    {
        #region Private Members

        private byte[] _data;

        #endregion

        #region Public Members

        public byte[] Data
        {
            get { return _data; }
            set
            {
                if (value != _data)
                {
                    _data = value;
                    NotifyPropertyChanged();
                }
            }
        }

        #endregion

        #region IElement Members

        public override ElementTypes ElementType { get { return ElementTypes.Byte; } }

        #endregion

        #region IGroupable Members

        public override GroupableTypes GroupableType { get { return GroupableTypes.Byte; } }

        #endregion

        #region INotable Members

        public override NotableTypes NotableType { get { return NotableTypes.Byte; } }

        #endregion
    }

    public class TwitElement : Concrete
    {
        #region Public Members

        #region Commented Tweet Implementation
        //public TweetSource Source { get; set; } //name, twitterid and icon
        //public TweetContent Content { get; set; } //text and image
        //public DateTime TweetTime { get; set; }
        #endregion

        #endregion

        #region IElement Members

        public override ElementTypes ElementType { get { return ElementTypes.Twit; } }

        #endregion

        #region IGroupable Members

        public override GroupableTypes GroupableType { get { return GroupableTypes.Twit; } }

        #endregion

        #region INotable Members

        public override NotableTypes NotableType { get { return NotableTypes.Twit; } }

        #endregion
    }

    public enum FileTypes
    {
        Other = 0,
        Image = 1,
        Audio = 2,
        Video = 3
    }

    public class FileElement : Concrete
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

        #region IGroupable Members

        public override GroupableTypes GroupableType { get { return GroupableTypes.File; } }

        #endregion

        #region INotable Members

        public override NotableTypes NotableType { get { return NotableTypes.File; } }

        #endregion
    }

    #endregion

    #region Valuables

    #region Foundation

    public class Keepable<T> : IElement, IEnumerable<T>, IEnumerable where T : IElement
    {
        #region Private Members

        #region IElement Members

        private Guid _id;
        private Guid _parentId;
        private int _position;
        private Timestamp _timestamp;
        private IProgress _progress;

        #endregion

        #region Internal Progress

        private InternalProgress<T> _internalProgress;

        #endregion

        #endregion

        #region Public Members

        #region Wrapped Collection

        public Collection<T> Data { get; set; }

        #endregion

        #region Keepable's Progress

        public virtual IProgress Progress
        {
            get
            {
                return _progress ?? (_internalProgress = GetInternalProgress());
            }
            set
            {
                if (value.ProgressType == ProgressTypes.Internal) return;
                if (value == _progress) return;

                _progress = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #endregion

        public Keepable()
        {
            Data = new Collection<T>();
            _internalProgress = GetInternalProgress();
        }

        #region Collection Indexer

        [System.Runtime.CompilerServices.IndexerName("Item")]
        public T this[int index]
        {
            get
            {
                return this.Data[index];
            }
            private set
            {
                Set(index, value);
            }
        }

        #region Collection Setter

        private void Set(int index, T item)
        {
            item.ParentId = this.Id;
            item.Position = index;
            Data[index] = item;
            NotifyPropertyChanged("Data");
        }

        #endregion

        #endregion

        #region Collection Methods

        public void Insert(int index, T item)
        {
            item.ParentId = this.Id;
            item.Position = index;
            Data.Insert(index, item);
            NotifyPropertyChanged("Data");
        }

        public void Add(T item)
        {
            item.ParentId = this.Id;
            item.Position = Data.Count;
            Data.Add(item);
            NotifyPropertyChanged("Data");
        }

        public void Remove(int index)
        {
            Data.RemoveAt(index);
            NotifyPropertyChanged("Data");
        }

        public bool Remove(T item)
        {
            if (Data.Remove(item))
            {
                NotifyPropertyChanged("Data");
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Clear()
        {
            Data.Clear();
            NotifyPropertyChanged("Data");
        }

        public bool Contains(T item)
        {
            return Data.Contains(item);
        }

        public int IndexOf(T item)
        {
            return Data.IndexOf(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        public void CopyTo(T[] array, int index)
        {
            Data.CopyTo(array, index);
        }

        public int Count { get { return Data.Count; } }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Data.GetEnumerator();
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
                    NotifyPropertyChanged();
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
                    NotifyPropertyChanged();
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
                    NotifyPropertyChanged();
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
                    NotifyPropertyChanged();
                }
            }
        }

        public virtual ElementTypes ElementType { get { return ElementTypes.None; } }

        IProgress IElement.Progress { get { return Progress; } }

        #endregion

        #region Helper Methods

        private InternalProgress<T> GetInternalProgress()
        {
            return new InternalProgress<T>(this.Data.Select(e => e.Progress));
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

    public class Valuable<T> : Keepable<T> where T : IElement
    {
        #region Public Members

        public virtual string Name { get; set; }

        #endregion
    }

    public class Selfable<T> : Valuable<Selfable<T>> where T : IElement
    {
        #region Public Members

        public Keepable<T> Values { get; set; }

        #endregion

        public Selfable()
        {
            Values = new Keepable<T>();
        }

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

        public override IProgress Progress { get { return Values.Progress; } }

        #endregion

        #region Helper Methods

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

        #endregion
    }

    #endregion

    #region Containers

    public class ListElement : Valuable<Concrete>, IGroupable, INotable
    {
        #region IElement Members

        public override ElementTypes ElementType { get { return ElementTypes.List; } }

        #endregion

        #region IGroupable Members

        public GroupableTypes GroupableType { get { return GroupableTypes.List; } }

        #endregion

        #region INotable Members

        public NotableTypes NotableType { get { return NotableTypes.List; } }

        #endregion
    }

    public class GridElement : Valuable<ListElement>, IGroupable, INotable
    {
        #region Public Indexer

        public ListElement this[string name]
        {
            get
            {
                return this.First(l => l.Name.Equals(name));
            }
            set
            {
                var element = this.First(l => l.Name.Equals(name));
                element = value;
            }
        }

        #endregion

        #region IElement Members

        public override ElementTypes ElementType { get { return ElementTypes.Grid; } }

        #endregion

        #region IGroupable Members

        public GroupableTypes GroupableType { get { return GroupableTypes.Grid; } }

        #endregion

        #region INotable Members

        public NotableTypes NotableType { get { return NotableTypes.Grid; } }

        #endregion
    }

    public class TreeElement : Selfable<Concrete>, IGroupable, INotable
    {
        #region IElement Members

        public override ElementTypes ElementType { get { return ElementTypes.Tree; } }

        #endregion

        #region IGroupable Members

        public GroupableTypes GroupableType { get { return GroupableTypes.Tree; } }

        #endregion

        #region INotable Members

        public NotableTypes NotableType { get { return NotableTypes.Tree; } }

        #endregion
    }

    #endregion

    #region Groupables

    public enum GroupableTypes : int
    {
        Text = ElementTypes.Text,
        Byte = ElementTypes.Byte,
        Twit = ElementTypes.Twit,
        File = ElementTypes.File,
        List = ElementTypes.List,
        Grid = ElementTypes.Grid,
        Tree = ElementTypes.Tree
    }

    public interface IGroupable : IElement 
    {
        GroupableTypes GroupableType { get; }
    }

    public class GroupElement : Valuable<IGroupable>, INotable
    {
        #region IElement Members

        public override ElementTypes ElementType { get { return ElementTypes.Group; } }

        #endregion

        #region INotable Members

        public NotableTypes NotableType { get { return NotableTypes.Group; } }

        #endregion
    }

    #endregion

    #region Notables

    public enum NotableTypes : int
    {
        Text = ElementTypes.Text,
        Byte = ElementTypes.Byte,
        Twit = ElementTypes.Twit,
        File = ElementTypes.File,
        List = ElementTypes.List,
        Grid = ElementTypes.Grid,
        Tree = ElementTypes.Tree,
        Group = ElementTypes.Group
    }

    public interface INotable : IElement
    {
        NotableTypes NotableType { get; }
    }

    public class NoteElement : Valuable<INotable>, IFoldable
    {
        #region IElement Members

        public override ElementTypes ElementType { get { return ElementTypes.Note; } }

        #endregion

        #region IFoldable Members

        public FoldableTypes FoldableType { get { return FoldableTypes.Note; } }

        #endregion
    }

    #endregion

    #region Pageables

    public class PageElement : Valuable<NoteElement>, IFoldable
    {
        #region IElement Members

        public override ElementTypes ElementType { get { return ElementTypes.Page; } }

        #endregion

        #region IFoldable Members

        public FoldableTypes FoldableType { get { return FoldableTypes.Page; } }

        #endregion
    }

    #endregion

    #region Foldables

    public enum FoldableTypes : int
    {
        Note = ElementTypes.Note,
        Page = ElementTypes.Page
    }

    public interface IFoldable : IElement 
    {
        FoldableTypes FoldableType { get; }
    }

    public class FolderElement : Selfable<IFoldable>
    {
        #region IElement Members

        public override ElementTypes ElementType { get { return ElementTypes.Folder; } }

        #endregion
    }

    #endregion

    #region Rootables

    public class RootElement : Valuable<FolderElement>
    {
        #region Public Members

        public override string Name { get { return String.Empty; } }

        #endregion

        #region IElement Members

        public override Guid Id { get { return Guid.Empty; } }

        public override Guid ParentId { get { return Guid.Empty; } }

        public override ElementTypes ElementType { get { return ElementTypes.Root; } }

        #endregion
    }

    #endregion

    #endregion
}