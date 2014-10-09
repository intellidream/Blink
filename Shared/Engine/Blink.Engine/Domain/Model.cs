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
        ElementTypes Type { get; }
        IProgress Progress { get; }
    }

    #endregion

    #region Concretes

    #region Foundation

    public abstract class Concrete : IElement, IGroupable, INotable, IAggregate
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

        public abstract ElementTypes Type { get; }

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

        public override ElementTypes Type { get { return ElementTypes.Text; } }

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

        public override ElementTypes Type { get { return ElementTypes.Tweet; } }

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

        public override ElementTypes Type { get { return ElementTypes.File; } }

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
                return _progress ?? (_internalProgress = new InternalProgress<T>(this.Data.Select(e => e.Progress)));
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
            _internalProgress = new InternalProgress<T>(this.Data.Select(e => e.Progress));
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

        public virtual ElementTypes Type { get { return ElementTypes.None; } }

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

    public class Valuable<T> : Keepable<T>, IAggregateRoot where T : IElement
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

        public override IProgress Progress { get { return Values.Progress; } }

        #endregion

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
                return this.First(l => l.Name.Equals(name));
            }
            set
            {
                var element = this.First(l => l.Name.Equals(name));
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

        public override ElementTypes Type { get { return ElementTypes.Folder; } }

        #endregion
    }

    #endregion

    #region Rootables

    public interface IRootable : IElement { }

    public class RootElement : Keepable<IRootable>
    {
        #region IElement Members

        public override Guid Id { get { return Guid.Empty; } }

        public override Guid ParentId { get { return Guid.Empty; } }

        public override ElementTypes Type { get { return ElementTypes.Root; } }

        #endregion
    }

    #endregion

    #endregion
}