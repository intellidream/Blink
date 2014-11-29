using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blink.Data.Model
{
    #region Enums

    public enum ElementTypes
    {
        Text,
        Link,
        Byte,
        File,
        Drawing,
        Social,
        Media,
        List,
        Table,
        Group,
        Note,
        Page,
        Tree,
        Folder,
        Root
    }

    #endregion

    #region Bases

    public abstract class Element
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public int Position { get; set; }
        public abstract ElementTypes Type { get; }
    }

    public abstract class Material<T> : Element
    {
        public T Value { get; set; }
    }

    public abstract class Container<T> : Element 
        where T : Element
    {
        public List<T> Values { get; set; }

        public Container() 
        {
            Values = new List<T>(); 
        }
    }

    public abstract class Composite<T, U> : Element 
        where T : Composite<T, U>
        where U : Element
    {
        public string Name { get; set; }

        public List<T> Children { get; set; }

        public List<U> Values { get; set; }

        public Composite()
        {
            Children = new List<T>();
            Values = new List<U>();
        }
    }

    #endregion

    #region Types

    public class Text : Material<string>
    {
        public override ElementTypes Type
        {
            get { return ElementTypes.Text; }
        }
    }

    public class List : Container<Element>
    {
        public override ElementTypes Type
        {
            get { return ElementTypes.List; }
        }
    }

    public class Tree : Composite<Tree, Element>
    {
        public override ElementTypes Type
        {
            get { return ElementTypes.Tree; }
        }
    }

    public class Note : Container<Element>
    {
        public override ElementTypes Type
        {
            get { return ElementTypes.Note; }
        }
    }

    public class Folder : Composite<Folder, Note>
    {
        public override ElementTypes Type
        {
            get { return ElementTypes.Folder; }
        }
    }

    public class Root : Composite<Folder, Note>
    {
        public override ElementTypes Type
        {
            get { return ElementTypes.Root; }
        }
    }

    #endregion
}