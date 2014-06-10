﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Blink.Shared.Domain.NewThings
{
    #region Elements

    public interface IElement 
    {
        Guid Id { get; set; }
        Guid ParentId { get; set; }
        IProgress Progress { get; }
    }

    #endregion

    #region Valuables

    public interface IValuable<T> : IElement where T : IElement
    {
        string Name { get; set; }
        Valuable<T> Values { get; set; }
    }

    public class Valuable<T> : Collection<T>, IElement where T : IElement
    {
        public Valuable()
        {
            Progress = new ProgressCollection();
        }

        #region IElement Members

        public virtual Guid Id { get; set; }
        public virtual Guid ParentId { get; set; }
        public virtual IProgress Progress { get; private set; }

        #endregion

        #region Collection Overrides

        protected override void SetItem(int index, T item)
        {
            item.ParentId = this.Id;
            base.SetItem(index, item);
            ((ProgressCollection)Progress)[index] = item.Progress;
        }

        protected override void InsertItem(int index, T item)
        {
            item.ParentId = this.Id;
            base.InsertItem(index, item);
            ((ProgressCollection)Progress).Insert(index, item.Progress);
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
            ((ProgressCollection)Progress).RemoveAt(index);
        }

        protected override void ClearItems()
        {
            base.ClearItems();
            ((ProgressCollection)Progress).Clear();
        }

        #endregion
    }

    public class Selfable<T> : Valuable<Selfable<T>>, IValuable<T> where T : IElement
    {
        public Selfable(Guid id)
            : base()
        {
            this.Id = id;

            Values = new Valuable<T>();
            Values.Id = this.Id;
        }

        #region IValuable Members

        public virtual string Name { get; set; }

        public virtual Valuable<T> Values { get; set; }

        #endregion
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
        public TreeElement(Guid id) : base(id) { }

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

    public class FolderElement : Selfable<IFoldable>, IFoldable 
    {
        public FolderElement(Guid id) : base(id) { }
    }

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

    public class ProgressCollection : Collection<IProgress>, IProgress
    {
        #region IProgress Members

        public Guid Id { get; set; }

        public bool IsCompleted()
        {
            return this.All(p => p.IsCompleted());
        }

        #endregion
    }

    public class InternalProgress : IProgress
    {
        public bool Completed { get; set; }

        #region IProgress Members

        public Guid Id { get; set; }

        public bool IsCompleted()
        {
            return Completed;
        }

        #endregion
    }

    public class DateTimeProgress : IProgress
    {
        public DateTime Completion { get; set; }
        
        #region IProgress Members

        public Guid Id { get; set; }

        public bool IsCompleted()
        {
            return Completion.ToUniversalTime().Equals(DateTime.UtcNow);
        }

        #endregion
    }
    public class LocationProgress : IProgress
    {
        public Location Current { get; set; }
        public Location Destination { get; set; }
        
        #region IProgress Members

        public Guid Id { get; set; }

        public bool IsCompleted()
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
