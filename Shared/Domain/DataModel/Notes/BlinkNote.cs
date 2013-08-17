
using System;

namespace Blink.Shared.Domain.DataModel.Notes
{
    /// <summary>
    /// A default Blink note
    /// </summary>
    internal sealed class BlinkNote : NoteBase, IAggregateRoot
    {
        #region Base Fields

        private Guid _id;
        private string _title;

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
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
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

        #region Aggregate Interface

        bool IAggregateRoot.CanBeSaved
        {
            get { return true; }
        }

        bool IAggregateRoot.CanBeDeleted
        {
            get { return true; }
        }

        #endregion
    }
}
