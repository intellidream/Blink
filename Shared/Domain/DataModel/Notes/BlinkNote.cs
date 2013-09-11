
using System;

namespace Blink.Shared.Domain.DataModel.Notes
{
    /// <summary>
    /// A default Blink note
    /// </summary>
    internal sealed class BlinkNote : NoteBase
    {
        #region Base Fields

        private Guid _id;
        private string _title;
        private Timestamp _time;
        private Content _content;

        #endregion

        #region Base Properties

        public override Guid Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = (value != Guid.Empty) ? value : Guid.NewGuid();
            }
        }

        public override string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = (!String.IsNullOrWhiteSpace(value)) ? value : @"Unknown";
            }
        }

        public override Timestamp Time
        {
            get { return _time; }
            set { _time = (value.Equals(Timestamp.Empty) || value.Equals(Timestamp.Default)) ? Timestamp.Now : value; }
        }

        public override Content Content
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        private BlinkNote()
        {
            //DateTime Default???
        }
    }
}
