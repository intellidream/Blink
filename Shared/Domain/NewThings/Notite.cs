using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blink.Shared.Domain.NewThings
{
    interface IElement 
    {
        Guid Id { get; set; }
        Guid? Parent { get; set; }
        IProgress Progress { get; set; }
    }

    class ElementBase : IElement
    {
        #region IElement
        public virtual Guid Id { get; set; }
        public virtual Guid? Parent { get; set; }
        public virtual IProgress Progress { get; set; }
        #endregion
    }

    class TextElement : ElementBase
    {
        string Text { get; set; }
    }

    class ListElement : ElementBase 
    {
        IList<IElement> Items { get; set; }
    }

    class GridElement : ElementBase 
    {
        IList<IList<IElement>> Data { get; set; }

        public IList<IElement> this[int index] 
        {
            get { return Data[index]; }
        }
    }

    class TableElement : ElementBase 
    {
        //IElement[][] Data { get; set; }

        //LIST(LIST=1'stIsColumnsList)

        public TableElement() 
        {
            Data = new IElement[5][5];

            Data.
        }

        class TableColumn : List<IElement>
        {

        }
    }

    interface IGroup : IElement
    {
        string Name { get; set; }
        IList<Guid> Children { get; set; }
    }

    class Group : IGroup
    {
        #region IElement
        public Guid Id { get; set; }
        public Guid? Parent { get; set; }
        public IProgress Progress { get; set; }
        #endregion

        #region IGroup
        public string Name { get; set; }
        public IList<Guid> Children { get; set; }
        #endregion
    }

    interface IProgress
    {
        Guid Id { get; set; }
        bool Completed();
    }

    interface ISchedule 
    {
        Guid Id { get; set; }
        DateTime Value { get; set; } 
    }
}
