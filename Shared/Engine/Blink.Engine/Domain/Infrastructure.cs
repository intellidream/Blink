using System;
using System.Collections.Generic;
using System.Linq;
using Wintellect.Sterling.Core.Serialization;

namespace Blink.Data.Domain.Infrastructure
{
    #region Timestamping

    public struct Timestamp
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public DateTime Accessed { get; set; }
    }

    #endregion

    #region Progressing

    public enum ProgressTypes : int
    {
        Manual,
        DateTime,
        Location,
        Internal
    }

    public interface IProgress
    {
        Guid Id { get; set; }

        ProgressTypes ProgressType { get; }

        int Percentage { get; }

        bool IsCompleted();
    }

    public abstract class ProgressBase : IProgress
    {
        #region IProgress Members

        public Guid Id { get; set; }

        public abstract ProgressTypes ProgressType { get; }

        public virtual int Percentage
        {
            get
            {
                return IsCompleted() ? 100 : 0;
            }
        }

        public abstract bool IsCompleted();

        #endregion
    }

    [SterlingIgnore]
    public class InternalProgress<T> : IProgress
    {
        #region Private Members

        private IList<IProgress> _values = null;

        private bool _HasValues
        {
            get { return ((_values != null) && (_values.Count > 0)); }
        }

        private int _Total { get { return _HasValues ? _values.Count : 0; } }

        private int _Completed
        {
            get
            {
                return (this._Total > 0)
                        ? _values.Count(p => p != null && p.IsCompleted())
                        : 0;
            }
        }

        #endregion

        public InternalProgress() { this.Id = Guid.Empty; }

        public InternalProgress(IEnumerable<IProgress> values) : this()
        {
            _values = values != null ? values.ToList() : null;
        }

        #region IProgress Members

        public Guid Id { get; set; }

        public ProgressTypes ProgressType
        {
            get { return ProgressTypes.Internal; }
        }

        public int Percentage
        {
            get
            {
                var completed = this._Completed;

                return (completed > 0)
                        ? (int)Math.Round((double)(100 * completed) / _Total)
                        : 0;
            }
        }

        public bool IsCompleted()
        {
            return _HasValues && (_Completed == _Total);
        }

        #endregion
    }

    public class ManualProgress : ProgressBase
    {
        #region Public Members

        public bool Completed { get; set; }

        #endregion

        #region IProgress Members

        public override ProgressTypes ProgressType
        {
            get { return ProgressTypes.Manual; }
        }

        public override bool IsCompleted()
        {
            return Completed;
        }

        #endregion
    }

    public class DateTimeProgress : ProgressBase
    {
        #region Public Members

        public DateTime Completion { get; set; }

        #endregion

        #region IProgress Members

        public override ProgressTypes ProgressType
        {
            get { return ProgressTypes.DateTime; }
        }

        public override bool IsCompleted()
        {
            return Completion.ToUniversalTime().Equals(DateTime.UtcNow);
        }

        #endregion
    }
    public class LocationProgress : ProgressBase
    {
        #region Public Members

        public Tuple<double, double> Current { get; set; }
        public Tuple<double, double> Destination { get; set; }

        #endregion

        #region IProgress Members

        public override ProgressTypes ProgressType
        {
            get { return ProgressTypes.Location; }
        }

        public override bool IsCompleted()
        {
            return Current.Equals(Destination);
        }

        #endregion
    }

    #endregion

    #region Scheduling

    public interface ISchedule
    {
        Guid Id { get; set; }
        Guid ParentId { get; set; }
        DateTime Value { get; set; }
    }

    #endregion
}