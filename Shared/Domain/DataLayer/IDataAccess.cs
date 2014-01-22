using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blink.Shared.Domain.DataModel;

namespace Blink.Shared.Domain.DataLayer
{
    internal interface IDataAccess
    {
        //Task<List<Note>> GetNotes();
        //Task UpdateNote(Note note);
        //Task<bool> DeleteNote(Note note);

        //Task<Note> GetNote(Guid noteId);
    }

    internal interface IDataAccess<T> where T : class, new()
    {
        Task<T> GetById(Guid id);
        Task<List<T>> GetAll();
        Task Create(T item);
        Task Update(T item);
        Task<bool> Delete(T item);
    }
}
