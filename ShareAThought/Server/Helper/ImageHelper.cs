using Server.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Server.Helper
{
    public class ImageHelper
    {
        public static string GetUserAvatarOrDefault(string path, string username)
        {
            string result = null;
            DirectoryInfo dInfo = new DirectoryInfo(path);
            if (dInfo.Exists && dInfo.GetFiles().Length != 0)
            {
                var fullFilename = Directory
                    .GetFiles(path, "*", SearchOption.AllDirectories)[0];
                string[] splits = fullFilename.Split('\\');
                var filename = splits[splits.Length - 1];

                result = String.Format(ServerPathConstants.imagePath, username) + "/" + filename;
            }
            else
            {
                result = ServerPathConstants.defaultImagePath;
            }

            return result;
        }
    }
}