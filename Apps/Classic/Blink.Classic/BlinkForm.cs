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
        public const string AccessToken = "EwA4Aq1DBAAUGCCXc8wU%2fzFu9QnLdZXy%2bYnElFkAAd9bNgjwKxTYTzxFgY3hVzTOFtCvkolM%2feO5jxVNuBRZ1GTjgryq59OsUhKzQFTSjNvrMA4h5LPZCyryzPxYbglCuaMC2uKFNs733eRDbnAh%2fRiGviQboYJ1w1jXud8SPU1tZU58qOuslfARZMzDcyYhlxYCheBNJoj4tLUL3M1c%2bKTkSf7rIGBachmysf5cYEhs%2fvxLH%2f2RNRleOZHBzchDoXfaomcBDWgl3luL8VKmcpQ3XOXrOrUZ0IRAm%2bdI5y9T4AjNo5t%2fjmAOlYIoDc5etG%2fSCWzyKoc5nj9BC%2fQRx83VUMmnoejVZ7AJgJstbBa%2bmxEjEs8HQ8HX93UhbmwDZgAACN%2fgWfUKXOZaCAE%2byKiZzxQYdUq%2fHhjpGXHcHDO%2bohSH9NGCDPp%2bdOsDJR38TYSc9L2f71v36DDc2sFJ9x8lNzCkenISIDJga5%2fPJb63Wa2MtUXw5M6P5e%2fLX7m87SAbGT2npzH69MpYcf3CndxBn8bKMfGbiNPrbZ5D6Vu4YXCv4ooZXe8Ad208vzeTYW6YbDucpCPiHi8v%2bJ669%2bBfa%2fz5mdgxmqWt%2bzKCrwp%2bi1bR2FCygDBAoHbmlWlIrmNCul7%2bZK1Ytji1rAOYW18gTzoas%2fIGfNk0XbwT4Uk1dlFinUNo2EeRzz4ORVIGtHawWZfKWELiavU%2bE%2brorowDg4vWI2zu4Qq09TKyQ6pX77PHc3wAAA%3d%3d";
        public const string RefreshToken = "";

        private void button3_Click(object sender, EventArgs e)
        {
            var client = new SkyNet.Client.Client(ApiKey, ApiSecret, CallbackUrl, AccessToken, RefreshToken);

            var skyHelper = new SkyDriveHelper(client);

            //var contents = client.GetContents(String.Empty);

            //var actual = contents.SingleOrDefault(f => f.Name.Equals(""));

            skyHelper.EnsureFolder(Folder.Root + "/test01/test02/test03/test04");






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
