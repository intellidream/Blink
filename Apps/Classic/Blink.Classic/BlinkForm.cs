using System;
using System.Linq;
using System.Windows.Forms;
using Blink.Shared.Domain;
using Blink.Shared.Domain.DataModel.Notes;
using Blink.Shared.Engine;
using Blink.Shared.Engine.SkyDrive;
using Microsoft.Live;
using SkyNet.Model;

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

        public const string ApiKey = "0000000048111E64";
        public const string ApiSecret = "er-ARwUcGUjG4kPbiUVktzAclNoOhZQz";
        public const string CallbackUrl = "https://login.live.com/oauth20_desktop.srf";
        public const string AccessToken = "EwA4Aq1DBAAUGCCXc8wU%2fzFu9QnLdZXy%2bYnElFkAAaW%2fZONOx1lHwSrE1l0Ia4mqA3VLlafvxvz74cgP8E5QLGAVTYFuczmb9MbazHnJtQd1ePpUGVzYaqVrcTzxwpmQ8IbCKxSsFDFyCSbo%2fKs9sFzJ73t10mq6l2QOzRSTMjNA9l43aRSBo8sqoW7PmnkRXMxktn4QJIBisD0p9lHIGc4jv0k1t6x%2bK%2b4oXi4GBC656aRjUiexlN%2fCxgnx4hVPLLDM2OXxEbAWoAXOg6kbRu5uGiXfNRgyQdZD20kuPRak5U0KbdX0SZGMWFCQ2ogKqV8PXRc9f8KhMeWpBbW4EGM87X02zJ52l6Or4fP%2fh8xb6l8qRJZzFi7l6zSRc5oDZgAACJeCVu9HOslWCAGT8BH0I%2f3mC8ge6UoYSJPDSiJ6SiAmh%2fH%2fdMLldbonE5EYEKbG9GvGav3TjddTCu0ONS4bMy6Ie5S0XnZ3qH3MGC%2fkFbis4YPUolG3UXkdAWpVX9pxypWsDfQBikPchqwXNrSe0NHmI5mdt8YDkMekn40NGcxf0sWnXRg2VCIeszLLdfI0KaHQ2czhXiyflaqkYafZegNnJ5l7vfagXY5Z9w33xGoVM0cXULCY6QQBZJrSL1Zu9aOar0zVCyH2nYWK4Ddf5D86sI2FcnE1MBRgawlq2TNK1oKn0AWvHaXJgjV4y8CiI3jt6zbr6i8V5ldcmRGNtbmxrR4%2bGb%2bgTFQCxEFziqRPS0EAAA%3d%3d";
        public const string RefreshToken = "";

        private void button3_Click(object sender, EventArgs e)
        {
            var client = new SkyNet.Client.Client(ApiKey, ApiSecret, CallbackUrl, AccessToken, RefreshToken);

            var contents = client.GetContents(Folder.Root);

            //var skyHelper = new SkyDriveHelper(client);

            ////var contents = client.GetContents(String.Empty);

            ////var actual = contents.SingleOrDefault(f => f.Name.Equals(""));

            //skyHelper.EnsureFolder(Folder.Root + "/test01/test02/test03/test04");






            //try
            //{
            //    webBrowser1.Navigated += (o, args) =>
            //    {
            //        if (args.Url.Fragment.Contains("access_token"))
            //        {
            //            //if (App.Current.Properties.Contains("responseData"))
            //            //{
            //            //    App.Current.Properties.Clear();
            //            //}
            //            //App.Current.Properties.Add("responseData", 1);
            //            //string[] responseAll = Regex.Split(args.Url.Fragment.Remove(0, 1), "&");

            //            //for (int i = 0; i < responseAll.Count(); i++)
            //            //{
            //            //    string[] nvPair = Regex.Split(responseAll[i], "=");
            //            //    App.Current.Properties.Add(nvPair[0], responseAll[i]);
            //            //}
            //            //this.Close();
            //        }
            //    };


            //    var auth = new LiveAuthClient("0000000048111E64");


            //    var loginUrl = auth.GetLoginUrl(new [] { "wl.skydrive_update", "wl.offline_access" });

            //    //var loginResult = await auth.IntializeAsync(new[] {"wl.skydrive_update", "wl.offline_access"});

            //    //https://login.live.com/oauth20_authorize.srf?client_id=0000000048111E64&redirect_uri=https%3A%2F%2Flogin.live.com%2Foauth20_desktop.srf&scope=wl.skydrive_update&response_type=code&display=windesktop&locale=en-GB&state=&theme=win7
            //    //https://login.live.com/oauth20_authorize.srf?client_id=0000000048111E64&redirect_uri=https%3A%2F%2Flogin.live.com%2Foauth20_desktop.srf&scope=wl.skydrive_update%20wl.offline_access&response_type=code&display=windesktop&locale=en-GB&state=&theme=win7
            //    //{https://login.live.com/oauth20_desktop.srf?code=839ea71f-8824-b13f-fbaf-bfa638394cbd&lc=2057}

            //    webBrowser1.Navigate(loginUrl);
            //}
            //catch (LiveAuthException exception)
            //{
            //}
        }
    }
}
