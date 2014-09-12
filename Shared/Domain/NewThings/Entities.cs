using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blink.Shared.Domain.NewThings
{
    // TODO: rename file to Data.cs

    // TODO: see Lazy<T> on deserializing nested stuff!

    // TODO: see if using AutoMapper to map between entities and data is an option!

    // TODO: collection + properties composing wrappers instead of derived collections and observable collections in PCL!

    #region Contracts

    public interface IElementEntity
    {
        ElementRecord ToElementEntity();
        IElement FromElementEntity();
    }

    public interface IValuableEntity<T> : IElementEntity where T : IElement
    {
        ValuableRecord ToValuableEntity();
        Valuable<T> FromValuableEntity();
    }

    public interface IConcreteEntity : IElementEntity
    {
        ConcreteRecord ToConcreteEntity();
        Concrete FromConcreteEntity();
    }

    public interface ITextEntity 
    {
        TextRecord ToTextEntity();
        TextElement FromTextEntity();
    }

    public interface IFileEntity
    {
        FileRecord ToFileEntity();
        FileElement FromFileEntity();
    }

    public interface IProgressEntity
    {
        ProgressRecord ToProgressEntity();
        IProgress FromProgressEntity();
    }

    public interface IManualProgressEntity
    {
        ManualProgressRecord ToManualProgressEntity();
        ManualProgress FromManualProgressEntity();
    }

    public interface IDateTimeProgressEntity
    {
        DateTimeProgressRecord ToDateTimeProgressEntity();
        DateTimeProgress FromDateTimeProgressEntity();
    }

    public interface ILocationProgressEntity
    {
        LocationProgressRecord ToLocationProgressEntity();
        LocationProgress FromLocationProgressEntity();
    }

    #endregion

    #region Entities

    public class ElementRecord
    {
        public Guid Id { get; set; }

        public Guid ParentId { get; set; }

        public int Position { get; set; }

        public Guid TimestampId { get; set; }

        public ElementTypes Type { get; set; }
    }

    public class ValuableRecord
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }

    public class ConcreteRecord
    {
        public Guid Id { get; set; }

        public Guid ProgressId { get; set; }
    }

    public class TextRecord
    {
        public Guid Id { get; set; }

        public string Text { get; set; }
    }

    public class FileRecord
    {
        public Guid Id { get; set; }

        public string FileName { get; set; }

        public FileTypes FileType { get; set; }

        public string FilePath { get; set; }

        public byte[] FileData { get; set; }
    }

    public class ProgressRecord 
    {
        public Guid Id { get; set; }

        public ProgressTypes ProgressType { get; set; }
    }

    public class ManualProgressRecord 
    {
        public Guid Id { get; set; }

        public bool Completed { get; set; }
    }

    public class DateTimeProgressRecord
    {
        public Guid Id { get; set; }

        public DateTime Completion { get; set; }
    }

    public class LocationProgressRecord
    {
        public Guid Id { get; set; }

        public Tuple<double, double> Current { get; set; }
        public Tuple<double, double> Destination { get; set; }
    }

    #endregion
}
