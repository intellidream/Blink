using System;
using System.Linq;
using Blink.Shared.Domain.DataModel.Notes;
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
        public const string AccessToken = "EwA4Aq1DBAAUGCCXc8wU%2fzFu9QnLdZXy%2bYnElFkAAaW%2fZONOx1lHwSrE1l0Ia4mqA3VLlafvxvz74cgP8E5QLGAVTYFuczmb9MbazHnJtQd1ePpUGVzYaqVrcTzxwpmQ8IbCKxSsFDFyCSbo%2fKs9sFzJ73t10mq6l2QOzRSTMjNA9l43aRSBo8sqoW7PmnkRXMxktn4QJIBisD0p9lHIGc4jv0k1t6x%2bK%2b4oXi4GBC656aRjUiexlN%2fCxgnx4hVPLLDM2OXxEbAWoAXOg6kbRu5uGiXfNRgyQdZD20kuPRak5U0KbdX0SZGMWFCQ2ogKqV8PXRc9f8KhMeWpBbW4EGM87X02zJ52l6Or4fP%2fh8xb6l8qRJZzFi7l6zSRc5oDZgAACJeCVu9HOslWCAGT8BH0I%2f3mC8ge6UoYSJPDSiJ6SiAmh%2fH%2fdMLldbonE5EYEKbG9GvGav3TjddTCu0ONS4bMy6Ie5S0XnZ3qH3MGC%2fkFbis4YPUolG3UXkdAWpVX9pxypWsDfQBikPchqwXNrSe0NHmI5mdt8YDkMekn40NGcxf0sWnXRg2VCIeszLLdfI0KaHQ2czhXiyflaqkYafZegNnJ5l7vfagXY5Z9w33xGoVM0cXULCY6QQBZJrSL1Zu9aOar0zVCyH2nYWK4Ddf5D86sI2FcnE1MBRgawlq2TNK1oKn0AWvHaXJgjV4y8CiI3jt6zbr6i8V5ldcmRGNtbmxrR4%2bGb%2bgTFQCxEFziqRPS0EAAA%3d%3d";
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

            var contents = client.GetContents(Folder.Root).ToList();
            if (contents.Any(f => f.Name.Equals("Public")))
            {
                System.Console.WriteLine("Got {0} values.", contents.Count);
            }
        }
    }
}
