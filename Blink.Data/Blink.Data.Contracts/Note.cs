using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blink.Data.Contracts
{
    public class Folder
    {
        public string Id { get; set; }
        public string ParentId { get; set; }

        public string Name { get; set; }
        public string Path { get; set; }

        public List<Folder> Childs { get; set; }
        public List<Note> Notes { get; set; }
    }


    public class Note
    {
        public Guid Id { get; set; }
        public Guid FolderId { get; set; }

        public string Title { get; set; }

        public List<NoteValue> Values { get; set; }
    }

    public class Summary
    {
        public string Text { get; set; }
        public byte[] Data { get; set; }
    }

    public class NoteValue
    {
        public Guid Id { get; set; }
        public Guid NoteId { get; set; }

        public Summary Summary { get; set; }
        public Content Content { get; set; }
    }
    public enum ContentTypes
    {
        Text = 0,
        Tweet = 1,
        Media = 2,
    }


    public class Content 
    {
        public Guid Id { get; set; }
        public Guid ValueId { get; set; }

        public ContentTypes Type { get; set; }

        public Text Text { get; set; }
        public Tweet Tweet { get; set; }
        public Media Media { get; set; }
    }

    public class Text
    {
        public Guid Id { get; set; }
        public Guid ContentId { get; set; }

        public string Text { get; set; }
    }

    public class Tweet
    {
        public Guid Id { get; set; }
        public Guid ContentId { get; set; }

        public string Name { get; set; }
        public string User { get; set; }
        public string Text { get; set; }
        public string Time { get; set; }
        public string Client { get; set; }
    }

    public enum MediaTypes
    {
        Other = 0,
        Image = 1,
        Audio = 2,
        Video = 3,
    }

    public class Media
    {
        public Guid Id { get; set; }
        public Guid ContentId { get; set; }

        public MediaTypes Type { get; set; }
        public byte[] Data { get; set; }
    }
}