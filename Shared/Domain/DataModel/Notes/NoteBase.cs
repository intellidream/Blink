using System;
using System.Threading.Tasks;

namespace Blink.Shared.Domain.DataModel.Notes
{
    /// <summary>
    /// Blueprint of a note
    /// </summary>
    public abstract class NoteBase : INoteBase
    {
        #region Properties

        // Contract properties
        public abstract Guid Id { get; set; }
        public abstract String Title { get; set; }
        public abstract TimeStamp Time { get; set; }
        public abstract Guid ContentId { get; }

        // Linked properties
        protected abstract Guid CategoryId { get; set; }

        // Optional properties
        internal virtual Guid? PriorityId { get; set; }
        internal virtual Guid? ProgressId { get; set; }
        internal virtual Guid? SchedulesId { get; set; }
        internal virtual Guid? SourcesId { get; set; }
        internal virtual Guid? KeywordsId { get; set; }
        internal virtual Guid? RelationsId { get; set; }
        internal virtual Guid? SharingId { get; set; }

        #endregion

        #region Methods

        public virtual bool HasContent
        {
            get { return !ContentId.Equals(Guid.Empty); }
        }

        public abstract Content LoadContent();

        #endregion
    }
}