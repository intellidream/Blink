using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Blink.Shared.Domain.NewThings
{
    #region Entities

    public interface IElementEntity
    {
        ElementEntity ToElementEntity();
    }

    public interface IValuableEntity : IElementEntity
    {
        ValuableEntity ToValuableEntity();
    }

    public interface IConcreteEntity { }

    public interface IConcreteEntity<T> : IElementEntity 
        where T : IConcreteEntity
    {
        ConcreteEntity ToConcreteBaseEntity();

        T ToConcreteTypeEntity();
    }

    public class ElementEntity
    {
        public Guid Id { get; set; }

        public Guid ParentId { get; set; }

        public int Position { get; set; }

        public Guid TimestampId { get; set; }

        public ElementTypes Type { get; set; }
    }

    public class ValuableEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }

    public class ConcreteEntity
    {
        public Guid Id { get; set; }

        public Guid ProgressId { get; set; }
    }

    public class TextEntity : IConcreteEntity
    {
        public Guid Id { get; set; }

        public string Text { get; set; }
    }

    public class FileEntity : IConcreteEntity
    {
        public Guid Id { get; set; }

        public string FileName { get; set; }

        public Blink.Shared.Domain.NewThings.FileElement.FileTypes FileType { get; set; }

        public string FilePath { get; set; }
        
        public byte[] FileData { get; set; }
    }

    #endregion

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

    public class Keepable<T> : Collection<T>, IElement where T : IElement
    {
        private InternalProgress _Progress { get; private set; }

        public Keepable() 
        {
            //Progress = new ProgressCollection();
        }

        #region IElement Members

        public virtual Guid Id { get; set; }

        public virtual Guid ParentId { get; set; }

        public virtual int Position { get; set; }

        public virtual Timestamp Timestamp { get; set; }

        public virtual ElementTypes Type { get { return ElementTypes.None; } }

        public IProgress Progress { get; }

        #endregion

        #region Collection Overrides

        protected override void SetItem(int index, T item)
        {
            item.ParentId = this.Id;
            item.Position = index;
            Progress[index] = item.Progress;
            base.SetItem(index, item);
        }

        protected override void InsertItem(int index, T item)
        {
            item.ParentId = this.Id;
            item.Position = index;
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

    public class Valuable<T> : Keepable<T>, IValuableEntity where T : IElement
    {
        public virtual string Name { get; set; }
        
        #region IValuableEntity Members

        public ValuableEntity ToValuableEntity()
        {
            return new ValuableEntity
            {
                Id = this.Id,
                Name = this.Name
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
                Type = this.Type
            };
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

        //public override ProgressCollection Progress { get { return Values.Progress; } }

        #endregion
    }

    #endregion

    #region Concretes

    public abstract class ConcreteBase : IElement, IGroupable, INotable
    {
        public abstract ProgressBase Progress { get; set; }

        #region IElement Members

        public virtual Guid Id { get; set; }

        public virtual Guid ParentId { get; set; }

        public virtual int Position { get; set; }

        public virtual Timestamp Timestamp { get; set; }

        public abstract ElementTypes Type { get; }

        IProgress IElement.Progress { get { return Progress; } }

        #endregion
    }

    public class TextElement : ConcreteBase, IConcreteEntity<TextEntity>
    {
        public string Text { get; set; }

        #region ConcreteBase Members

        public override ProgressBase Progress { get; set; }

        #endregion

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

        public override ProgressBase Progress { get; set; }

        #endregion

        #region IElement Members

        public override ElementTypes Type { get { return ElementTypes.Tweet; } }

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

        public override ProgressBase Progress { get; set; }

        #endregion

        #region IElement Members

        public override ElementTypes Type { get { return ElementTypes.File; } }

        #endregion
    }

    #endregion

    #region Containers

    public class ListElement : Valuable<ConcreteBase>, IGroupable, INotable
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

    public class TreeElement : Selfable<ConcreteBase>, IGroupable, INotable
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

    public class Timestamp
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

        bool IsCompleted();
    }

    public abstract class ProgressBase : IProgress
    {
        #region IProgress Members

        public Guid Id { get; set; }

        public abstract ProgressTypes ProgressType { get; }

        public abstract bool IsCompleted();

        #endregion
    }

    public class InternalProgress : IProgress
    {
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

        public Keepable<IElement> Parent { get; set; }

        #region IProgress Members

        public Guid Id { get; set; }

        public ProgressTypes ProgressType 
        {
            get { return ProgressTypes.Internal; } 
        }

        public bool IsCompleted()
        {
            return _Completed > 0;
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
