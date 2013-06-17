using System;

namespace Blink.Shared.Domain.DataModels
{
    internal abstract class NoteBase
    {
        internal Guid Identity { get; set; }
        internal DateTime Timestamp { get; set; }
        internal String Title { get; set; }

        internal Category Category { get; set; }
        internal Content Content { get; set; }
    }
}
