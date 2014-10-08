using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Blink.Data.Domain.Model;
using Blink.Data.Engine; // will use access.repos here instead
using Blink.Shared.Engine.SkyDrive;
using Microsoft.Live;
using SkyNet.Client;
using SkyNet.Model;
using Wintellect.Sterling.Core.Database;
using Wintellect.Sterling.Core;

namespace Blink.Classic
{
    public partial class BlinkForm : Form
    {
        public BlinkForm()
        {
            InitializeComponent();
        }

        Guid _folderElementId;

        private async void button1_Click(object sender, EventArgs e)
        {
            var folderElement = new FolderElement();
            folderElement.Name = "My New Folder";
            _folderElementId = Guid.NewGuid();
            folderElement.Id = _folderElementId;
            folderElement.ParentId = Guid.Empty;

            var subFolderElement = new FolderElement();
            subFolderElement.Name = "My New SubFolder";
            subFolderElement.Id = Guid.NewGuid();
            subFolderElement.ParentId = Guid.Empty;

            var pageElement = new PageElement();
            pageElement.Id = Guid.NewGuid();

            var noteElement = new NoteElement();
            noteElement.Id = Guid.NewGuid();
            noteElement.Name = "My New Note";

            var noteElementTwo = new NoteElement();
            noteElementTwo.Id = Guid.NewGuid();
            noteElementTwo.Name = "My New Note Two";

            var groupElement = new GroupElement();
            groupElement.Name = "My New Group";
            groupElement.Id = Guid.NewGuid();

            var mp1 = new ManualProgress();
            mp1.Id = Guid.NewGuid();
            mp1.Completed = true;
            var textElement = new TextElement();
            textElement.Id = Guid.NewGuid();
            textElement.Text = "Nico";
            textElement.Progress = mp1;

            var mp3 = new ManualProgress();
            mp3.Id = Guid.NewGuid();
            mp3.Completed = false;
            var textElementTwo = new TextElement();
            textElementTwo.Id = Guid.NewGuid();
            textElementTwo.Progress = mp3;

            var mp2 = new ManualProgress();
            mp2.Id = Guid.NewGuid();
            mp2.Completed = true;
            var fileElement = new FileElement();
            fileElement.Id = Guid.NewGuid();
            fileElement.FileData = System.Text.Encoding.ASCII.GetBytes("Mihai");
            fileElement.FilePath = "C:\\Text.txt";
            fileElement.FileType = FileTypes.Other;
            fileElement.Progress = mp2;

            groupElement.Add(fileElement);
            groupElement.Add(textElement);

            noteElement.Add(textElementTwo);
            noteElement.Add(groupElement);

            pageElement.Add(noteElement);

            var pep = pageElement.Progress;

            noteElementTwo.Add(textElementTwo);

            subFolderElement.Values.Add(pageElement);
            subFolderElement.Values.Add(noteElementTwo);

            folderElement.Values.Add(pageElement);
            folderElement.Values.Add(noteElementTwo);
            
            folderElement.Add(subFolderElement);

            var treeElement = new TreeElement();
            treeElement.Id = Guid.NewGuid();
            treeElement.Values.Add(fileElement);

            var treeElementTwo = new TreeElement();
            treeElementTwo.Id = Guid.NewGuid();
            treeElement.Add(treeElementTwo);

            Default.Add(folderElement);

            var save = await Sterling.Database.SaveAsync(Default); // enforce "Default" policy via RootRepository 
            await Sterling.Database.FlushAsync();
        }

        // this will be in root repo
        private static RootElement _default = null;

        public static RootElement Default
        {
            get { return _default ?? (_default = new RootElement()); }
            set { _default = value; }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            Default = await Sterling.Database.LoadAsync<RootElement>(true); // enforce "Default" policy via RootRepository 
        }

        #region SkyDrive
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

                // REGEX with Format/Guid/Format here!!!

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





            // 4.0 vs. 4.5 - debug SkyNet/RestSharp escaping issues

            // Synchronization Strategy & Sharing(viaAzureTables) & Users/Friends



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
        #endregion
    }
}