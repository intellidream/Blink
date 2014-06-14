using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Blink.Shared.Domain.NewThings
{
    public class Timestamp 
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public DateTime Accessed { get; set; }
    }

    enum ElementTypes : int
    {
        Text,
        Tweet,
        File,
        List,
        Grid,
        Tree,
        Group,
        Note,
        Page,
        Folder
    }

    enum ValuableTypes { }
    enum SelfableTypes { }

    #region Elements

    public interface IElement
    {
        Guid Id { get; set; }
        Guid ParentId { get; set; }
        Timestamp Timestamp { get; set; }
        int Index { get; set; }

        //Type
        //Index
        IProgress Progress { get; }
    }

    public interface IElementCollection : IElement
    {
        ProgressCollection CollectionProgress { get; } 
    }

    #endregion

    #region Valuables

    public interface IValuable : IElementCollection
    {
        string Name { get; set; }
    }

    public interface ISelfable<T> : IValuable where T : IElement
    {
        Valuable<T> Values { get; set; }
    }

    public class Valuable<T> : Collection<T>, IValuable where T : IElement
    {
        public Valuable()
        {
            CollectionProgress = new ProgressCollection();
        }

        #region IElement Members

        public virtual Guid Id { get; set; }
        public virtual Guid ParentId { get; set; }

        private IProgress progress;
        public virtual IProgress Progress 
        {
            get { return progress ?? CollectionProgress; }
            protected set { progress = value; } 
        }

        #endregion

        #region IElementCollection Members

        public virtual ProgressCollection CollectionProgress { get; protected set; }

        #endregion

        #region Collection Overrides

        protected override void SetItem(int index, T item)
        {
            item.ParentId = this.Id;
            CollectionProgress[index] = item.Progress;
            base.SetItem(index, item);
        }

        protected override void InsertItem(int index, T item)
        {
            item.ParentId = this.Id;
            CollectionProgress.Insert(index, item.Progress);
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            CollectionProgress.RemoveAt(index);
            base.RemoveItem(index);
        }

        protected override void ClearItems()
        {
            CollectionProgress.Clear();
            base.ClearItems();
        }

        #endregion
    }

    // Progress of a Folder/TreeNode shoulod be the sum of it's Values + it's subFolders/TreeNodes...!!!
    // So I must treat progress correctly on a collection analisys... see Notes...!!

    // Implement CombinedProgress (with/Completed(IsCompleted()) property in IProgress?!)

    public class Selfable<T> : Valuable<Selfable<T>>, ISelfable<T> where T : IElement
    {
        public Selfable() : base()
        {
            Values = new Valuable<T>();
        }

        #region IElement Members

        private Guid id;

        public override Guid Id 
        { 
            get { return id; }
            set { id = value; Values.Id = id; }
        }

        private ProgressBase progress;
        public override IProgress Progress
        {
            get { return progress ?? Values.Progress; }
            protected set { progress = (ProgressBase)value; } 
        }

        #endregion

        #region IElement Members

        private ProgressCollection collectionProgress;
        public override ProgressCollection CollectionProgress
        {
            get { return collectionProgress ?? Values.CollectionProgress; }
            protected set { collectionProgress = value; }
        }

        #endregion

        #region IValuable Members

        public virtual string Name { get; set; }

        public virtual Valuable<T> Values { get; set; }

        #endregion
    }

    public static class SelfableExtensions 
    {
        /// <summary>
        /// http://stackoverflow.com/questions/11830174/how-to-flatten-tree-via-linq/20335369?stw=2#20335369
        /// </summary>
        public static IEnumerable<Selfable<T>> Flatten<T>(this Selfable<T> root) where T : IElement
        {
            var stack = new Stack<Selfable<T>>();
            
            stack.Push(root);

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
    }

    #endregion

    #region Concretes

    public enum ConcreteTypes 
    {
        Text = 0,
        Tweet = 1,
        File = 2
    }

    public interface IConcrete : IGroupable
    {
        ConcreteTypes Type { get; }

        new Guid ParentId { get; set; } 
    }

    public class TextElement : IConcrete
    {
        public string Text { get; set; }

        #region IElement Members

        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public IProgress Progress { get; set; }

        #endregion

        #region IConcrete Members

        public ConcreteTypes Type { get { return ConcreteTypes.Text; } }

        #endregion
    }

    public class TweetElement : IConcrete 
    {
        public string Source { get; set; } //name, twitterid and icon
        //public TweetContent Content { get; set; } //text and image
        public DateTime Timestamp { get; set; }

        //public class TweetContent : List<IConcrete>
        //{

        //}

        #region IElement Members

        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public IProgress Progress { get; set; }

        #endregion

        #region IConcrete Members

        public ConcreteTypes Type { get { return ConcreteTypes.Tweet; } }

        #endregion
    }

    public class FileElement : IConcrete
    {
        public string Name { get; set; }
        public FileTypes Type { get; set; }

        public string Path { get; set; }
        public byte[] Data { get; set; }

        public enum FileTypes 
        {
            Other = 0,
            Image = 1,
            Audio = 2,
            Video = 3
        }

        #region IElement Members

        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public IProgress Progress { get; set; }

        #endregion

        #region IConcrete Members

        ConcreteTypes IConcrete.Type { get { return ConcreteTypes.File; } }

        #endregion
    }

    #endregion

    #region Containers

    public enum ContainerTypes 
    {
        List = 0,
        Grid = 1,
        Tree = 2
    }

    public interface IContainer : IGroupable
    {
        ContainerTypes Type { get; }

        new Guid ParentId { get; set; } 
    }

    public interface IListable : IList, IContainer 
    {
        string Name { get; set; }
        new Guid ParentId { get; set; } 
    }

    public class ListElement : Valuable<IConcrete>, IListable
    {
        public string Name { get; set; }

        #region IContainer Members

        public ContainerTypes Type { get { return ContainerTypes.List; } }

        #endregion
    }

    public class GridElement : Valuable<IListable>, IContainer
    {
        public string Name { get; set; }

        public IListable this[string name]
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

        #region IContainer Members

        public ContainerTypes Type { get { return ContainerTypes.Grid; } }

        #endregion
    }

    public class TreeElement : Selfable<IConcrete>, IContainer 
    {
        #region IContainer Members

        public ContainerTypes Type { get { return ContainerTypes.Tree; } }

        #endregion
    }

    #endregion

    #region Groupables

    public interface IGroupable : INotable
    {
        new Guid ParentId { get; set; }
    }

    public class GroupElement : Valuable<IGroupable>, INotable
    {
        public string Name { get; set; }
    }

    #endregion

    #region Notables

    public interface INotable : IElement
    {
        new Guid ParentId { get; set; }
    }

    public class NoteElement : Valuable<INotable>, IPageable
    {
        public string Name { get; set; }
    }

    #endregion

    #region Pageables

    public interface IPageable : IFoldable
    {
        new Guid ParentId { get; set; }
    }

    public class PageElement : Valuable<IPageable>, IFoldable { }

    #endregion

    #region Foldables

    public interface IFoldable : IElement
    {
        new Guid ParentId { get; set; }
    }

    public class FolderElement : Selfable<IFoldable>, IFoldable { }

    #endregion

    #region Locating

    public interface ILocation
    {
        double Latitude { get; set; }
        double Longitude { get; set; }
    }

    public class Location : ILocation
    {
        #region ILocation Members

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        #endregion
    }

    #endregion

    #region Progressing

    public interface IProgress
    {
        Guid Id { get; set; }
        bool IsCompleted();
    }

    public abstract class ProgressBase : IProgress
    {
        public virtual Guid Id { get; set; }
        public abstract bool IsCompleted();
    }

    public class ProgressCollection : Collection<IProgress>, IProgress
    {
        #region IProgress Members

        public Guid Id { get; set; }

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
        public Location Current { get; set; }
        public Location Destination { get; set; }
        
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
        DateTime Value { get; set; } 
    }

    #endregion
}
