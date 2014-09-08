using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blink.Shared.Domain.NewThings
{
    #region Contracts

    public interface IElementEntity
    {
        ElementEntity ToElementEntity();
        IElement FromElementEntity();
    }

    public interface IValuableEntity<T> : IElementEntity where T : IElement
    {
        ValuableEntity ToValuableEntity();
        Valuable<T> FromValuableEntity();
    }

    public interface IConcreteEntity : IElementEntity
    {
        ConcreteEntity ToConcreteEntity();
        Concrete FromConcreteEntity();
    }

    public interface ITextEntity 
    {
        TextEntity ToTextEntity();
        TextElement FromTextEntity();
    }

    public interface IFileEntity
    {
        FileEntity ToFileEntity();
        FileElement FromFileEntity();
    }

    public interface IProgressEntity
    {
        ProgressEntity ToProgressEntity();
        IProgress FromProgressEntity();
    }

    public interface IManualProgressEntity
    {
        ManualProgressEntity ToManualProgressEntity();
        ManualProgress FromManualProgressEntity();
    }

    public interface IDateTimeProgressEntity
    {
        DateTimeProgressEntity ToDateTimeProgressEntity();
        DateTimeProgress FromDateTimeProgressEntity();
    }

    public interface ILocationProgressEntity
    {
        LocationProgressEntity ToLocationProgressEntity();
        LocationProgress FromLocationProgressEntity();
    }

    #endregion

    #region Entities

    public class ElementEntity
    {
        public Guid Id { get; set; }

        public Guid ParentId { get; set; }

        public int Position { get; set; }

        public Guid TimestampId { get; set; }

        public ElementTypes Type { get; set; }
    }

    public class ValuableEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }

    public class ConcreteEntity
    {
        public Guid Id { get; set; }

        public Guid ProgressId { get; set; }
    }

    public class TextEntity
    {
        public Guid Id { get; set; }

        public string Text { get; set; }
    }

    public class FileEntity
    {
        public Guid Id { get; set; }

        public string FileName { get; set; }

        public FileTypes FileType { get; set; }

        public string FilePath { get; set; }

        public byte[] FileData { get; set; }
    }

    public class ProgressEntity 
    {
        public Guid Id { get; set; }

        public ProgressTypes ProgressType { get; set; }
    }

    public class ManualProgressEntity 
    {
        public Guid Id { get; set; }

        public bool Completed { get; set; }
    }

    public class DateTimeProgressEntity
    {
        public Guid Id { get; set; }

        public DateTime Completion { get; set; }
    }

    public class LocationProgressEntity
    {
        public Guid Id { get; set; }

        public Tuple<double, double> Current { get; set; }
        public Tuple<double, double> Destination { get; set; }
    }

    #endregion
}
