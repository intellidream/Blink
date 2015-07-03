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
        public string Name { get; set; }
        public string Path { get; set; }

        public List<Folder> Childs { get; set; }
        public List<Note> Notes { get; set; }
    }


    public class Note
    {
        public Guid Id { get; set; }
        public Guid FolderId { get; set; }

        public List<NoteValue> Values { get; set; }
    }

    public class NoteValue
    {
        public ValueTypes Type { get; set; }

        public ValueSummary Summary { get; set; }
        public ValueContent Content { get; set; }
    }

    public enum ValueTypes
    {
        Text = 0,
        Tweet = 1,
        Image = 2,
        Audio = 3,
        Video = 4,
    }

    public class ValueSummary
    {
        public TextSummary Text { get; set; }
        public TweetSummary Tweet { get; set; }
        public ImageSummary Image { get; set; }
        public AudioSummary Audio { get; set; }
        public VideoSummary Video { get; set; }
    }

    public class ValueContent { }

    public class TextSummary { public string SummaryText { get; set; } }
    public class TweetSummary { }
    public class ImageSummary { public string SummaryText { get; set; } public byte[] SummaryData { get; set; } }
    public class AudioSummary { }
    public class VideoSummary { }

    public class TextContent { }
    public class TweetContent { }
    public class ImageContent { }
    public class AudioContent { }
    public class VideoContent { }
}