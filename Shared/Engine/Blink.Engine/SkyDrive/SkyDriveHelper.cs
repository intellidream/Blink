using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkyNet.Model;

namespace Blink.Shared.Engine.SkyDrive
{
    /// <summary>
    ///     This class is used to assist with managing the SkyDrive references
    /// </summary>
    public class SkyDriveHelper
    {
        private static SkyNet.Client.Client _client;

        private static readonly List<string> _paths = new List<string>();
        private static readonly List<string> _files = new List<string>();

        private SkyDriveHelper() { }

        public SkyDriveHelper(SkyNet.Client.Client client)
        {
            if (client == null) throw new ArgumentNullException("client");

            _client = client;
        }

        ///// <summary>
        /////     Gets an isolated storage reader
        ///// </summary>
        ///// <param name="path">The path</param>
        ///// <returns>The reader</returns>
        //public BinaryReader GetReader(string path)
        //{
        //    try
        //    {
        //        return new BinaryReader(File.OpenRead(path));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new SterlingFileSystemException(ex);
        //    }
        //}

        ///// <summary>
        /////     Get an isolated storage writer
        ///// </summary>
        ///// <param name="path">The path</param>
        ///// <returns>The writer</returns>
        //public BinaryWriter GetWriter(string path)
        //{
        //    try
        //    {
        //        var stream = File.Open(path, FileMode.Create, FileAccess.Write);
        //        return new BinaryWriter(stream);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new SterlingFileSystemException(ex);
        //    }
        //}

        ///// <summary>
        /////     Delete a file based on its path
        ///// </summary>
        ///// <param name="path">The path</param>
        //public void Delete(string path)
        //{
        //    try
        //    {
        //        if (File.Exists(path))
        //        {
        //            File.Delete(path);
        //            if (_files.Contains(path))
        //            {
        //                _paths.Remove(path);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new SterlingFileSystemException(ex);
        //    }
        //}

        /// <summary>
        ///     Ensure that a folder exists
        /// </summary>
        /// <param name="path">the path</param>
        public void EnsureFolder(string path)
        {
            if (!path.StartsWith(Folder.Root)) throw new SterlingSkyDriveException(new ArgumentException("path"));

            if (path.EndsWith("/"))
            {
                path = path.Substring(0, path.Length - 1);
            }

            if (_paths.Contains(path)) return;

            try
            {
                var result = Folder.Root;

                var local = path.Replace(Folder.Root, String.Empty);

                var segments = local.Split(new[] { @"\", @"/" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var segment in segments)
                {
                    var contents = _client.GetContents(result);

                    var actual = contents.SingleOrDefault(f => f.Name.Equals(segment));

                    if (actual == null)
                    {
                        _client.CreateFolder(result, segment);
                    }

                    result += "/";
                    result += segment;
                }

                _paths.Add(result);
            }
            catch (Exception ex)
            {
                throw new SterlingSkyDriveException(ex);
            }
        }
    }
}
