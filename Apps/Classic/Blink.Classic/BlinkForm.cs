using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Blink.Shared.Domain.DataModel.Notes;
using Blink.Shared.Engine;

namespace Blink.Classic
{
    public partial class BlinkForm : Form
    {
        private Guid _noteId;
        private Guid _categoryId;
        private Guid _contentId;

        public BlinkForm()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            _noteId = Guid.NewGuid();
            _categoryId = Guid.NewGuid();
            _contentId = Guid.NewGuid();

            var newCategory = new Category
            {
                Id = _categoryId,
                ParentId = null,
                Time = TimeStamp.UtcNow,
                Title = "New Category"
            };

            var newContent = new Content
            {
                Id = _contentId,
                NoteId = _noteId,
                Text = "New note content."
            };

            var newNote = new BlinkNote
            {
                Id = _noteId,
                Time = TimeStamp.UtcNow,
                Title = "New Note", 
                ContentId = _contentId,
                CategoryId = _categoryId
            };

            await Sterling.Database.SaveAsync(newCategory);
            await Sterling.Database.SaveAsync(newContent);
            await Sterling.Database.SaveAsync(newNote);
            await Sterling.Database.FlushAsync();

            MessageBox.Show(this, @"New Category/Content/Note saved at: " + newNote.Time, @"Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            var newNote = await Sterling.Database.LoadAsync<BlinkNote>(_noteId);
            var newCategory = await Sterling.Database.LoadAsync<Category>(newNote.CategoryId);
            var newContent = await Sterling.Database.LoadAsync<Content>(newNote.ContentId);

            var localTime = newNote.Time.ToLocalTime();

            MessageBox.Show(this, @"New Category/Content/Note loaded.", @"Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
