using System;
using System.Linq;
using Blink.Shared.Engine;
using SkyNet.Model;
using Wintellect.Sterling.Core;
using Wintellect.Sterling.Server;

namespace Blink.Console
{
    static class Program
    {
        public const string ApiKey = "0000000048111E64";
        public const string ApiSecret = "er-ARwUcGUjG4kPbiUVktzAclNoOhZQz";
        public const string CallbackUrl = "https://login.live.com/oauth20_desktop.srf";
        public const string AccessToken = "EwA4Aq1DBAAUGCCXc8wU%2fzFu9QnLdZXy%2bYnElFkAAaW%2fZONOx1lHwSrE1l0Ia4mqA3VLlafvxvz74cgP8E5QLGAVTYFuczmb9MbazHnJtQd1ePpUGVzYaqVrcTzxwpmQ8IbCKxSsFDFyCSbo%2fKs9sFzJ73t10mq6l2QOzRSTMjNA9l43aRSBo8sqoW7PmnkRXMxktn4QJIBisD0p9lHIGc4jv0k1t6x%2bK%2b4oXi4GBC656aRjUiexlN%2fCxgnx4hVPLLDM2OXxEbAWoAXOg6kbRu5uGiXfNRgyQdZD20kuPRak5U0KbdX0SZGMWFCQ2ogKqV8PXRc9f8KhMeWpBbW4EGM87X02zJ52l6Or4fP%2fh8xb6l8qRJZzFi7l6zSRc5oDZgAACLExdyyc76h6CAExGdNABCtB9oTAyxTQ68rtwAuE6Fbh0lu6NQ1PwgsL4b9omuy%2bqLlDQP9Mabk3%2bOvHW6BMCB%2br%2bCRARikV9y5ujMLPnSUaj47KoB97rsEEXppNK7mibinnp721Se3KWCYGITQveyGyZycLsQE6%2f4%2fNk%2fy7Gy9%2fi0bXXei%2fR%2fuSM2Ul9UJLUnCs9hTkTmQIOKlcnV7eDd9WLhVSLbMy0uekrNR50ze3%2b1Ne6TqxKW%2bTuY2xyXW4yKlqwtHhB23wva%2fUJkXvOn7lk6N9Zq203fKqdI6%2f4hYpcGpeZBL3qDeGpfMcp919AtXQx%2fv0noqCI3ot8MjUEpjEaxZfm18QBShet5Q6H1taOSAAAA%3d%3d";
        public const string RefreshToken = "";

        static void Main(string[] args)
        {
            //Sterling.Activate(() => new PlatformAdapter(), () => new MemoryDriver());

            //System.Console.WriteLine("Sterling activated...");

            //while (System.Console.ReadKey().Key != ConsoleKey.Escape)
            //{
            //}

            //Sterling.Deactivate();


            var client = new SkyNet.Client.Client(ApiKey, ApiSecret, CallbackUrl, AccessToken, RefreshToken);

            var contents = client.GetContents("/me/skydrive").ToList();
            if (contents.Any(f => f.Name.Equals("Public")))
            {
                System.Console.WriteLine("Got {0} values.", contents.Count);
            }
        }
    }
}
