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

    public class NoteValue
    {
        public Guid Id { get; set; }
        public Guid NoteId { get; set; }

        public ValueTypes Type { get; set; }

        public TextValue Text { get; set; }
        public TweetValue Tweet { get; set; }
        public MediaValue Media { get; set; }
    }

    public enum ValueTypes
    {
        Text = 0,
        Tweet = 1,
        Media = 2,
    }

    public class TextValue 
    {
        public Guid Id { get; set; }
        public Guid ValueId { get; set; }

        public TextSummary Summary { get; set; }
        public TextContent Content { get; set; }
    }

    public class TweetValue
    {
        public Guid Id { get; set; }
        public Guid ValueId { get; set; }

        public TweetSummary Summary { get; set; }
        public TweetContent Content { get; set; }
    }

    public class MediaValue
    {
        public Guid Id { get; set; }
        public Guid ValueId { get; set; }

        public MediaTypes Type { get; set; }
        public MediaSummary Summary { get; set; }
        public MediaContent Content { get; set; }
    }

    public class TextSummary 
    {
        public Guid Id { get; set; }
        public Guid TextId { get; set; }

        public string Text { get; set; } 
    }

    public class TextContent
    {
        public Guid Id { get; set; }
        public Guid TextId { get; set; }

        public string Text { get; set; }
    }
    
    public class TweetSummary
    {
        public Guid Id { get; set; }
        public Guid TweetId { get; set; }

        public string User { get; set; }
        public string Text { get; set; } 
    }

    public class TweetContent
    {
        public Guid Id { get; set; }
        public Guid TweetId { get; set; }

        public string User { get; set; }
        public string Text { get; set; }
    }

    public enum MediaTypes
    {
        Other = 0,
        Image = 1,
        Audio = 2,
        Video = 3,
    }

    public class MediaSummary 
    {
        public Guid Id { get; set; }
        public Guid MediaId { get; set; }

        public MediaTypes Type { get; set; }
        public string Text { get; set; } 
        public byte[] Data { get; set; }
    }

    public class Summary
    {
        public Guid Id { get; set; }//?
        public Guid ValueId { get; set; }//?

        public MediaTypes Type { get; set; }//?

        public string Text { get; set; }
        public byte[] Data { get; set; }
    }

    public class MediaContent 
    {
        public Guid Id { get; set; }
        public Guid MediaId { get; set; }

        public MediaTypes Type { get; set; }
        public byte[] Data { get; set; }
    }
}