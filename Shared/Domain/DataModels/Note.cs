using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blink.Shared.Domain.DataModels
{
    internal sealed class Note : NoteBase
    {
        internal override Guid Id
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

        internal override string Title
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

        internal override Timestamp Time
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

        internal override Content Content
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

        protected override Category Category
        {
            get
            {
                return base.Category;
            }
            set
            {
                base.Category = value;
            }
        }

        protected override Priority Priority
        {
            get
            {
                return base.Priority;
            }
            set
            {
                base.Priority = value;
            }
        }
    }
}
