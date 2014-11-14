using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blink.Data
{
    //public enum ElementTypes 
    //{
    //    Valuable,
    //    Concrete
    //}

    public enum ValuableTypes 
    {
        List,
        Group,
        Note,
        Page,
        Folder,
        Root
    }

    public enum ConcreteTypes 
    {
        Text,
        File,
        Tweet,
        Drawing
    }


    // TreeTypes - NodeTypes & LeafTypes

    public enum ElementTypes // Tree (Element)
    {
        Folder, // Node (Valuable)
        Content // Leaf (Concrete)
    }

    public class ElementBase
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public int Position { get; set; }
        public ElementTypes Type { get; set; }
    }

    public enum ContentTypes 
    {
        Text,
        Tweet,
        File,
        Link,
        Drawing
    }

    public class ContentBase 
    {
        private ElementBase _Base { get; set; }

        public Guid Id { get { return _Base.Id; } set { _Base.Id = value; } }
        public Guid ParentId { get { return _Base.ParentId; } set { _Base.ParentId = value; } }
        public int Position { get { return _Base.Position; } set { _Base.Position = value; } }
        public ElementTypes Type { get { return ElementTypes.Content; } }
    }

    public class TextContent
    {
        public ContentBase Base { get; set; }
        public ContentTypes Type { get { return ContentTypes.Text; } }
        public string Text { get; set; }
    }
    
    public class FileContent
    {
        public ContentBase Base { get; set; }
        public ContentTypes Type { get { return ContentTypes.File; } }
        public string Name { get; set; }
        public byte[] Data { get; set; }
        public string Source { get; set; }
        public string Extension { get; set; }
    }

    public struct Timestamp 
    {
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public DateTime Synchronized { get; set; }
    }

    public class ListElement 
    {
        public ElementBase Base { get; set; }

    }

    public class NoteElement
    {
        public ElementBase Base { get; set; }
        public ElementTypes Type { get { return ElementTypes.Folder; } }
        public Timestamp Timestamp { get; set; }
        public List<ElementBase> Children { get; set; }
        public List<ContentBase> Content { get; set; }
        public string Name { get; set; }
    }

    // where is elementtype, in base or note/list etc?!

    // check-out composite pattern for folder/note/content...

    // Large file storage/Share VIA Blink from other Apps (fwd. ex.: Twitter)/Support both Sterling (see Azure Tables/Blobs but also OneDrive via 365 API) and PCLSQLite with new Model.

    // Summit: Polyglot persistence - iQuark GitHub - NoSQL Distilled (Fowler)

    // Summit: VSIX office 365 api -> VS -> Add connected service (see PCL version via nuget)

    
    
    // Buy new big SSD and install windows 10 and VS 2015 and test VS 13/15 Community in VM.

    // Vodafone new Lumia and contract upgrade to 4G in Feb. 2015.

    // .Net as a .nuget package - watch Connect keynote.

    // Genisoft MiniEF for SQLite Xamarin-based with sync (Genisoft GitHub).

    public class NoteIdentity
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        //type
    }

    public class TextNote 
    {
        public NoteIdentity Identity { get; set; }
        public string Text { get; set; }
    }
}
