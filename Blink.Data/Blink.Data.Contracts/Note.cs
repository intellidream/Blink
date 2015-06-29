using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blink.Data.Contracts
{
    public class Folder
    {
        public string Hash { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        //public string Hash()
        //{
        //    return new MD5CryptoProvider()
        //}
    }


    public class Note
    {
        public Guid NoteId { get; set; }
        public Guid ParentId { get; set; }

    }

    public class NoteValue
    {

    }
}
