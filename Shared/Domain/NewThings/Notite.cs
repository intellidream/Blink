using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blink.Shared.Domain.NewThings
{
    class Test 
    {
        GridElement g = new GridElement();

        Test() 
        {
            g.Add("", new ListElement());

            var l = g[0].Value;

            l.Add(new TextElement());

            if (l[0].Type == ElementTypes.Concrete) { }

            #region Question
            //?? maybe lists or dictionaries should only contain Concrete types
            #endregion
        }
    }

    #region Elements

    enum ElementTypes 
    {
        Concrete = 0,
        Container = 1
    }

    interface IElement 
    {
        Guid Id { get; set; }
        Guid? Parent { get; set; }
        ElementTypes Type { get; }
        IProgress Progress { get; set; }
    }

    #endregion

    #region Concrete

    enum ConcreteTypes 
    {
        Text = 0,
        File = 1
    }

    interface IConcrete : IElement
    {
        new ConcreteTypes Type { get; }
    }

    class TextElement : IConcrete
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

    class FileElement : IConcrete
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

    enum ContainerTypes 
    {
        List = 0,
        Grid = 1
    }

    interface IContainer : IElement
    {
        new ContainerTypes Type { get; }
    }

    class ListElement : List<IElement>, IContainer 
    {
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

    class GridElement : Dictionary<string, ListElement>, IContainer 
    {
        public KeyValuePair<string, ListElement> this[int index]
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

    interface IGroup : IElement
    {
        string Name { get; set; }
        IList<Guid> Children { get; set; }
    }

    class Group : IGroup
    {
        #region IGroup Members

        public string Name { get; set; }

        public IList<Guid> Children { get; set; }

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

    interface IProgress
    {
        Guid Id { get; set; }
        bool IsCompleted();
    }

    class ProgressCollection : List<IProgress>, IProgress
    {
        #region IProgress Members

        public Guid Id { get; set; }

        public bool IsCompleted()
        {
            return this.All(p => p.IsCompleted());
        }

        #endregion
    }

    class InternalProgress : IProgress
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

    class DateTimeProgress : IProgress
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
    class LocationProgress : IProgress
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

    interface ISchedule 
    {
        Guid Id { get; set; }
        DateTime Value { get; set; } 
    }

    #endregion
}
