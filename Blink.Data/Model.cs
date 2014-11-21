using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blink.Data
{
    public enum ElementTypes
    {
        Composite,
        Material
    }

    public enum CompositeTypes 
    {
        List,
        Tree,
        Table,
        Group,
        Note,
        Page,
        Folder,
        Root
    }

    public enum MaterialTypes 
    {
        Text,
        Link,
        Byte,
        File,
        Drawing,
        Social,
        Media
    }

    public abstract class Element
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public abstract ElementTypes ElementType { get; }
        public int Position { get; set; }
    }

    public class Composite<T> : Element where T : Element
    {
        private List<Composite<T>> _children = new List<Composite<T>>();

        public List<Composite<T>> Children
        {
            get { return _children; }
        }

        public T Value { get; set; }

        public CompositeTypes CompositeType { get; set; }

        public override ElementTypes ElementType
        {
            get { return ElementTypes.Composite; }
        }

        public Composite<T> Add(T child)
        {
            var entity = new Composite<T> { Value = child };
            _children.Add(entity);
            return entity;
        }

        public void Remove(Guid id)
        {
            _children.RemoveAll(e => e.Id == id);
        }
    }

    public class Container { }

    public class Material<T> : Element
    {
        public T Value { get; set; }

        public MaterialTypes MaterialType { get; private set; }

        public override ElementTypes ElementType
        {
            get { return ElementTypes.Material; }
        }

        public Material(MaterialTypes type) { MaterialType = type; }
    }

    //public abstract class Material<T> : Element
    //{
    //    public abstract T Value { get; set; }

    //    public abstract MaterialTypes MaterialType { get; }
    //}

    //public class TextMaterial : Material<string>
    //{
    //    public override string Value { get; set; }

    //    public override MaterialTypes MaterialType
    //    {
    //        get { return MaterialTypes.Text; }
    //    }
    //}

    //public class ByteMaterial : Material<byte[]>
    //{
    //    public override byte[] Value { get; set; }

    //    public override MaterialTypes MaterialType
    //    {
    //        get { return MaterialTypes.Byte; }
    //    }
    //}

    // timestamp and progress only on ome composites

    // where is elementtype, in base or note/list etc?!

    // check-out composite pattern for folder/note/content...

    // Large file storage/Share VIA Blink from other Apps (fwd. ex.: Twitter)/Support both Sterling (see Azure Tables/Blobs but also OneDrive via 365 API) and PCLSQLite with new Model.

    // Summit: Polyglot persistence - iQuark GitHub - NoSQL Distilled (Fowler)

    // Summit: VSIX office 365 api -> VS -> Add connected service (see PCL version via nuget)

    // Summit: .Net as a .nuget package - watch Connect keynote.

    // Summit: Genisoft MiniEF for SQLite Xamarin-based with sync (Genisoft GitHub).
}
