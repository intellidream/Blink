using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blink.Shared.Domain.NewThings
{
    #region Elements

    public interface IElement 
    {
        Guid Id { get; set; }
        IProgress Progress { get; set; }
    }

    #endregion

    #region Concretes

    public enum ConcreteTypes 
    {
        Text = 0,
        File = 1
    }

    public interface IConcrete : IGroupable
    {
        ConcreteTypes Type { get; }
    }

    public class TextElement : IConcrete
    {
        public string Text { get; set; }

        #region IElement Members

        public Guid Id { get; set; }

        public IProgress Progress { get; set; }

        #endregion

        #region IConcrete Members

        public ConcreteTypes Type { get { return ConcreteTypes.Text; } }

        #endregion

        #region IGroupable Members

        public Guid GroupId { get; set; }

        #endregion

        #region INotable Members

        public Guid NoteId { get; set; }

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

        public IProgress Progress { get; set; }

        #endregion

        #region IConcrete Members

        ConcreteTypes IConcrete.Type { get { return ConcreteTypes.File; } }

        #endregion

        #region IGroupable Members

        public Guid GroupId { get; set; }

        #endregion

        #region INotable Members

        public Guid NoteId { get; set; }

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
    }

    public class ListElement : List<IConcrete>, IContainer
    {
        public string Name { get; set; }

        #region IContainer Members

        public ContainerTypes Type { get { return ContainerTypes.List; } }

        #endregion

        #region IGroupable Members

        public Guid GroupId { get; set; }

        #endregion

        #region IElement Members

        public Guid Id { get; set; }

        public IProgress Progress { get; set; }

        #endregion

        #region INotable Members

        public Guid NoteId { get; set; }

        #endregion
    }

    public class GridElement : List<ListElement>, IContainer
    {
        public string Name { get; set; }

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

        #region IContainer Members

        public ContainerTypes Type { get { return ContainerTypes.Grid; } }

        #endregion

        #region IGroupable Members

        public Guid GroupId { get; set; }

        #endregion

        #region IElement Members

        public Guid Id { get; set; }

        public IProgress Progress { get; set; }

        #endregion

        #region INotable Members

        public Guid NoteId { get; set; }

        #endregion
    }

    public class TreeElement : List<TreeElement>, IContainer 
    {
        public string Name { get; set; }

        public List<IConcrete> Values { get; set; }

        #region IContainer Members

        public ContainerTypes Type { get { return ContainerTypes.Tree; } }

        #endregion

        #region IGroupable Members

        public Guid GroupId { get; set; }

        #endregion

        #region IElement Members

        public Guid Id { get; set; }

        public IProgress Progress { get; set; }

        #endregion

        #region INotable Members

        public Guid NoteId { get; set; }

        #endregion
    }

    #endregion

    #region Groupables

    public interface IGroupable : INotable
    {
        Guid GroupId { get; set; }
    }

    public class GroupElement : List<IGroupable>, IElement, INotable
    {
        public string Name { get; set; }

        #region IElement Members

        public Guid Id { get; set; }

        public IProgress Progress { get; set; }

        #endregion

        #region INotable Members

        public Guid NoteId { get; set; }

        #endregion
    }

    #endregion

    #region Notables

    public interface INotable : IElement
    {
        Guid NoteId { get; set; }
    }

    public class NoteElement : List<INotable>, IFoldable
    {
        public string Name { get; set; }

        #region IElement Members

        public Guid Id { get; set; }

        public IProgress Progress { get; set; }

        #endregion

        #region IFoldable Members

        public Guid FolderId { get; set; }

        #endregion
    }

    #endregion

    #region Pageables

    public class PageElement : List<NoteElement>, IFoldable 
    {
        #region IElement Members

        public Guid Id { get; set; }

        public IProgress Progress { get; set; }

        #endregion

        #region IFoldable Members

        public Guid FolderId { get; set; }

        #endregion
    }

    #endregion

    #region Foldables

    public interface IFoldable : IElement
    {
        Guid FolderId { get; set; }
    }

    public interface IFolder //?? IFoldable??
    {

    }

    public class FolderElement : List<FolderElement> //List<IFolder>
    {
        public string Name { get; set; }

        #region IElement Members

        public Guid Id { get; set; }

        public IProgress Progress { get; set; }

        #endregion
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

    public class ProgressCollection : List<IProgress>, IProgress
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
