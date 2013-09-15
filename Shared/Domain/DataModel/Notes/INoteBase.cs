using System;
using System.Threading.Tasks;

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
        Guid ContentId { get; }

        bool HasContent { get; }
        Content LoadContent();
    }
}