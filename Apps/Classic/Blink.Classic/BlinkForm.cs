﻿using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Blink.Shared.Domain;
using Blink.Shared.Domain.DataModel.Notes;
using Blink.Shared.Engine;
using Blink.Shared.Engine.SkyDrive;
using Microsoft.Live;
using SkyNet.Client;
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
        public string AccessToken = "";
        public string RefreshToken = "";

        private void button3_Click(object sender, EventArgs e)
        {
            var client = new Client(ApiKey, ApiSecret, CallbackUrl, AccessToken, RefreshToken);

            var authRequestUrl = client.GetAuthorizationRequestUrl(new[] {Scope.SkyDriveUpdate});

            webBrowser1.Navigated += (o, args) =>
            {
                if (!args.Url.Query.Contains("code")) return;

                var codeArray = args.Url.Query.Split(new[] {"=", "&"}, StringSplitOptions.RemoveEmptyEntries);
                var code = codeArray[1];

                Guid guid;
                if (!Guid.TryParse(code, out guid)) return;

                var userToken = client.GetAccessToken(code);

                AccessToken = userToken.Access_Token;
                RefreshToken = userToken.Refresh_Token;

                var contents = client.GetContents(Folder.Root);

                webBrowser1.Stop();
            };

            webBrowser1.Navigate(authRequestUrl);

            
            








            //var contents = client.GetContents(Folder.Root);

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
