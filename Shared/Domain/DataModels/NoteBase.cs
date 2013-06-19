using System;

namespace Blink.Shared.Domain.DataModels
{
    /// <summary>
    /// Blueprint of the Note type.
    /// </summary>
    internal abstract class NoteBase
    {
        internal abstract Guid Id { get; set; }
        internal abstract String Title { get; set; }
        internal abstract Timestamp Time { get; set; }
        internal abstract Content Content { get; set; }

        protected virtual Category Category { get; set; }
        protected virtual Priority Priority { get; set; }
        internal virtual Progress Progress { get; set; }
        internal virtual Schedules Schedules { get; set; }

        internal virtual Sources Sources { get; set; }
        internal virtual Keywords Keywords { get; set; }
        internal virtual Relations Relations { get; set; }
        internal virtual Sharing Sharing { get; set; }
    }

    
}
