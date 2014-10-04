using System;
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
using Blink.Shared.Domain.NewThings;
using Wintellect.Sterling.Core.Database;
using Wintellect.Sterling.Core;

namespace Blink.Classic
{
    public partial class BlinkForm : Form
    {
        public BlinkForm()
        {
            InitializeComponent();

            #region Commented
            //    var folderElement = new FolderElement();
            //    folderElement.Name = "My New Category";
            //    folderElement.Id = Guid.NewGuid();
            //    folderElement.ParentId = Guid.Empty;

            //    var subFolderElement = new FolderElement();
            //    subFolderElement.Name = "My New SubCategory";
            //    subFolderElement.Id = Guid.NewGuid();
            //    subFolderElement.ParentId = Guid.Empty;

            //    var pageElement = new PageElement();
            //    pageElement.Id = Guid.NewGuid();

            //    var noteElement = new NoteElement();
            //    noteElement.Id = Guid.NewGuid();
            //    noteElement.Name = "My New Note";

            //    var noteElementTwo = new NoteElement();
            //    noteElementTwo.Id = Guid.NewGuid();
            //    noteElementTwo.Name = "My New Note Two";

            //    var groupElement = new GroupElement();
            //    groupElement.Name = "My New Group";
            //    groupElement.Id = Guid.NewGuid();

            //    var locationProgress = new LocationProgress();
            //    locationProgress.Id = Guid.NewGuid();
            //    var currentLocation = new Shared.Domain.NewThings.Location();
            //    currentLocation.Latitude = 10.00;
            //    currentLocation.Longitude = 10.01;
            //    var destinationLocation = new Shared.Domain.NewThings.Location();
            //    destinationLocation.Latitude = 20.02;
            //    destinationLocation.Longitude = 20.03;
            //    locationProgress.Current = currentLocation;
            //    locationProgress.Destination = destinationLocation;
            //    var textElement = new TextElement();
            //    textElement.Id = Guid.NewGuid();
            //    textElement.Progress = locationProgress;

            //    var dateTimeProgress = new DateTimeProgress();
            //    dateTimeProgress.Id = Guid.NewGuid();
            //    dateTimeProgress.Completion = DateTime.UtcNow.Subtract(new TimeSpan(100000));
            //    var fileElement = new FileElement();
            //    fileElement.Id = Guid.NewGuid();
            //    fileElement.Data = null;
            //    fileElement.Path = "C:\\Text.txt";
            //    fileElement.Type = FileElement.FileTypes.Other;
            //    fileElement.Progress = dateTimeProgress;

            //    var dateTimeProgressTwo = new DateTimeProgress();
            //    dateTimeProgressTwo.Id = Guid.NewGuid();
            //    dateTimeProgressTwo.Completion = DateTime.UtcNow.Subtract(new TimeSpan(100000));
            //    var textElementTwo = new TextElement();
            //    textElementTwo.Id = Guid.NewGuid();
            //    textElementTwo.Progress = dateTimeProgressTwo;

            //    groupElement.Add(fileElement);

            //    //noteElement.Add(textElement);
            //    noteElement.Add(groupElement);

            //    pageElement.Add(noteElement);

            //    //noteElementTwo.Add(textElementTwo);

            //    subFolderElement.Values.Add(pageElement);
            //    //subFolderElement.Values.Add(noteElementTwo);

            //    folderElement.Add(subFolderElement);



            //    folderElement.Add(new FolderElement());
            //    folderElement[0].Add(new FolderElement());
            //    folderElement[0][0].Add(new FolderElement());
            //    folderElement[0][0][0].Add(new FolderElement());
            //    folderElement[0][0][0][0].Add(new FolderElement());
            //    folderElement[0][0][0][0][0].Add(new FolderElement());

            //    var t = folderElement.Flatten().ToList().Count;

            //    var p = folderElement.Progress.IsCompleted();

            //    var c = folderElement.CollectionProgress.Percentage;




            //    var d = new FolderElement();
            //    var e = d.CollectionProgress.Percentage;
            #endregion
        }

        Guid _treeElementId;
        Guid _noteElementId;
        Guid _folderElementId;

        //private void button1_Click(object sender, EventArgs e)
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
            _noteElementId = noteElement.Id;
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

            //noteElementTwo.Add(textElementTwo);

            subFolderElement.Values.Add(pageElement);
            //subFolderElement.Values.Add(noteElementTwo);

            //folderElement.Values.Add(pageElement);
            //folderElement.Values.Add(noteElementTwo);
            
            folderElement.Add(subFolderElement);



            var treeElement = new TreeElement();
            treeElement.Id = Guid.NewGuid();
            _treeElementId = treeElement.Id;
            treeElement.Values.Add(fileElement);

            var treeElementTwo = new TreeElement();
            treeElementTwo.Id = Guid.NewGuid();
            treeElement.Add(treeElementTwo);


            //folderElement.ElementType = ElementTypes.Root;
            //RootElement.Instance.Add(folderElement);

            // everything saved or deleted as element-entity will be audited for synchronization

            // my entities or sterling's custom serializers - also consider switching to another db (sterling's custom serializers will not be good there...)?!

            //await Sterling.Database.SaveAsync(folderElement.ToElementEntity());
            //await Sterling.Database.SaveAsync(folderElement.ToValuableEntity());

            //var folderType = typeof(FolderElement);




            await Sterling.Database.SaveAsync(folderElement);

            var folderProgress = folderElement[0].Progress.Percentage;

            await Sterling.Database.FlushAsync();

            //MessageBox.Show(this, @"folderElement saved at: " + DateTime.Now /*newNote.UtcTime.ToLocal()*/, @"Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //private void button2_Click(object sender, EventArgs e)
        private async void button2_Click(object sender, EventArgs e)
        {
            //var elementEntity = await Sterling.Database.LoadAsync<ElementEntity>(_folderElementId);
            //var valuableEntity = await Sterling.Database.LoadAsync<ValuableEntity>(_folderElementId);


            //var folderElement = await Sterling.Database.LoadAsync<ValuableEntity>(_folderElementId);




            var folderElement = await Sterling.Database.LoadAsync<FolderElement>(_folderElementId);

            var folderProgress = folderElement[0].Progress.Percentage;

            //MessageBox.Show(this, @"folderElement loaded.", @"Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public const string ApiKey = "0000000048111E64";
        public const string ApiSecret = "er-ARwUcGUjG4kPbiUVktzAclNoOhZQz";
        public const string CallbackUrl = "https://login.live.com/oauth20_desktop.srf";
        public string AccessToken = "";
        public string RefreshToken = "";

        #region SkyDrive
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