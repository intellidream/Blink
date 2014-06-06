using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blink.Shared.Domain.NewThings
{
    #region Elements

    public enum ElementTypes 
    {
        Concrete = 0,
        Container = 1
    }

    public interface IElement 
    {
        Guid Id { get; set; }
        Guid? Parent { get; set; }
        ElementTypes Type { get; }
        IProgress Progress { get; set; }
    }

    #endregion

    #region Concrete

    public enum ConcreteTypes 
    {
        Text = 0,
        File = 1
    }

    public interface IConcrete : IElement
    {
        new ConcreteTypes Type { get; }
    }

    public class TextElement : IConcrete
    {
        public string Text { get; set; }
        //public bool IsLink { get; set; } - not important in backend/ui matter
                
        #region IConcrete Members

        public ConcreteTypes Type { get { return ConcreteTypes.Text; } }

        #endregion

        #region IElement Members

        public Guid Id { get; set; }

        public Guid? Parent { get; set; }

        ElementTypes IElement.Type { get { return ElementTypes.Concrete; } }

        public IProgress Progress { get; set; }

        #endregion
    }

    //public class HtmlElement : IConcrete { }

    public class FileElement : IConcrete
    {
        #region Suggestions
        // if Path has value, object is linked externally
        // if Data has value, object is stored internally

        // if the user chooses to show a stored or non-stored object as an image, audio or video, 
        // just try to load from Data or Path in the appropriate Type-determined internal viewer
        //
        // if internal loading fails, just display an "Error" loading the object and a "Refresh" link
        //
        // if external loading fails (maybe object is not there temporarily), just display an error in viewer and offer the possibility
        // to display the object as link (see below), and the possibility to refresh the viewer later and display correctly then
        //
        // if actual viewer loading fails, just display an "Error" loading the object and a "Refresh" link

        // if the user chooses to show a stored object as link, just show "Name" underlined - non-clickable to see in external viewer - and the possibility to still show the internal viewer (a "Show" link)
        // if the user chooses to show a non-stored object as link, just show "Path" underlined - clickable to see in external viewer - and the possibility to still show the internal viewer (a "Show" link)
        #endregion

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

        #region IConcrete Members

        ConcreteTypes IConcrete.Type { get { return ConcreteTypes.File; } }

        #endregion

        #region IElement Members

        public Guid Id { get; set; }

        public Guid? Parent { get; set; }

        ElementTypes IElement.Type { get { return ElementTypes.Concrete; } }

        public IProgress Progress { get; set; }

        #endregion
    }

    #endregion

    #region Containers

    public enum ContainerTypes 
    {
        List = 0,
        Grid = 1
    }

    public interface IContainer : IElement
    {
        new ContainerTypes Type { get; }
    }

    public class ListElement : List<IElement>, IContainer 
    {
        public string Name { get; set; }

        #region IContainer Members

        public ContainerTypes Type { get { return ContainerTypes.List; } }

        #endregion

        #region IElement Members

        public Guid Id { get; set; }

        public Guid? Parent { get; set; }

        ElementTypes IElement.Type { get { return ElementTypes.Container; } }

        public IProgress Progress { get; set; }

        #endregion
    }

    public class GridElement : Dictionary<string, List<IElement>>, IContainer 
    {
        public string Name { get; set; }

        public KeyValuePair<string, List<IElement>> this[int index]
        {
            get
            {
                return this.ElementAt(index);
            }
            set
            {
                var element = this.ElementAt(index);
                element = value;
            }
        }

        #region IContainer Members

        public ContainerTypes Type { get { return ContainerTypes.Grid; } }

        #endregion

        #region IElement Members

        public Guid Id { get; set; }

        public Guid? Parent { get; set; }

        ElementTypes IElement.Type { get { return ElementTypes.Container; } }

        public IProgress Progress { get; set; }

        #endregion
    }

    #endregion

    #region Grouping

    public interface IGroup : IElement
    {
        string Name { get; set; }
        IList<IElement> Children { get; set; }
    }

    public class Group : IGroup
    {
        #region IGroup Members

        public string Name { get; set; }

        public IList<IElement> Children { get; set; }

        #endregion

        #region IElement Members

        public Guid Id { get; set; }

        public Guid? Parent { get; set; }

        public ElementTypes Type { get; set; }

        public IProgress Progress { get; set; }

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
        //public Location Completion { get; set; }
        
        #region IProgress Members

        public Guid Id { get; set; }

        public bool IsCompleted()
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
        DateTime Value { get; set; } 
    }

    #endregion
}
