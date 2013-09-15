using System;
using System.Threading.Tasks;

namespace Blink.Shared.Domain.DataModel.Notes
{
    /// <summary>
    /// A default Blink note
    /// </summary>
    public sealed class BlinkNote : NoteBase
    {
        #region Base Fields

        private Guid _id;
        private string _title;
        private TimeStamp _time;
        private Guid _contentId;

        private Guid _categoryId;

        #endregion

        #region Base Properties

        public override Guid Id
        {
            get { return _id; }
            set { _id = (value != Guid.Empty) ? value : Guid.NewGuid(); }
        }

        public override string Title
        {
            get { return _title; }
            set { _title = (!String.IsNullOrWhiteSpace(value)) ? value : @"Unknown"; }
        }

        public override TimeStamp Time
        {
            get { return _time; }
            set { _time = (value.Equals(null) || value.Equals(TimeStamp.Default)) ? TimeStamp.Now : value; }
        }

        public override Guid ContentId
        {
            get { return _contentId; }
        }

        protected override Guid CategoryId { get; set; }

        #endregion

        public BlinkNote()
        {

        }

        public override Content LoadContent()
        {
            //return (HasContent ? ContentRepository.LoadContent(_contentId) : Content.Empty);
            throw new NotImplementedException();
        }
    }
}
