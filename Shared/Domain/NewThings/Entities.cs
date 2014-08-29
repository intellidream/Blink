using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blink.Shared.Domain.NewThings
{
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

    public interface IConcreteEntity { }

    public interface IConcreteEntity<T> : IElementEntity
        where T : IConcreteEntity
    {
        ConcreteEntity ToConcreteBaseEntity();

        T ToConcreteTypeEntity();
    }

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

    public class TextEntity : IConcreteEntity
    {
        public Guid Id { get; set; }

        public string Text { get; set; }
    }

    public class FileEntity : IConcreteEntity
    {
        public Guid Id { get; set; }

        public string FileName { get; set; }

        public Blink.Shared.Domain.NewThings.FileElement.FileTypes FileType { get; set; }

        public string FilePath { get; set; }

        public byte[] FileData { get; set; }
    }
}
