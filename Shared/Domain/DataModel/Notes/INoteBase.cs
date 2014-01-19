using System;

namespace Blink.Shared.Domain.DataModel.Notes
{
    /// <summary>
    /// Contract of a note
    /// </summary>
    internal interface INoteBase
    {
        Guid Id { get; set; }
        String Title { get; set; }
        TimeStamp Time { get; set; }
        Content Content { get; set; }
        Category Category { get; set; }
    }
}