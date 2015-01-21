#region Minimod
// <MiniMod>
//   <Name>Temporary Folder</Name>
//   <Author>Tobias Kiertscher &lt;dev@mastersign.de&gt;</Author>
//   <LastChanged>2015-01-21</LastChanged>
//   <Version>1.1.0</Version>
//   <Url>https://gist.github.com/mastersign/15cf94ebcf356d15da59/raw/Mastersign.Minimod.TemporaryFolder.cs</Url>
//   <Description>
//     This minimod contains a class for managing temporary folders in the file system.
//   </Description>
// </MiniMod>
#endregion

using System;
using System.IO;
using System.Linq;

namespace Mastersign.Minimods
{
    /// <summary>
    /// Represents a temporary folder in the file system.
    /// Creates a folder with a unique name at initialization 
    /// and deletes the folder with all its contents at disposal.
    /// </summary>
    public class TemporaryFolder : IDisposable
    {
        /// <summary>
        /// Gets the path of the temporary folder.
        /// </summary>
        public string TemporaryPath { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="TemporaryFolder"/>.
        /// </summary>
        /// <param name="basePath">The absolute path of a directory to create the temporary folder in or <c>null</c>.</param>
        /// <remarks>If <c>null</c> is given for <paramref name="basePath"/>
        /// <see cref="Path.GetTempPath()"/> is used to retrieve a base path.</remarks>
        public TemporaryFolder(string basePath = null)
        {
            if (basePath == null) basePath = Path.GetTempPath();
            TemporaryPath = GenerateNonExistingPath(basePath);
            Directory.CreateDirectory(TemporaryPath);
        }

        /// <summary>
        /// Deletes the temporary folder with all its content.
        /// </summary>
        public void Dispose()
        {
            if (Directory.Exists(TemporaryPath))
            {
                Directory.Delete(TemporaryPath, true);
            }
        }

        #region Static Helper

        private static readonly Random RAND = new Random();

        private const string PREFIX = "tmp_";

        private static string GenerateRandomName()
        {
            var buffer = new byte[4];
            RAND.NextBytes(buffer);
            return string.Join(string.Empty, buffer.Select(v => v.ToString("X2")));
        }

        private static string GeneratePath(string basePath)
        {
            return Path.Combine(basePath, PREFIX + GenerateRandomName());
        }

        private static string GenerateNonExistingPath(string basePath)
        {
            string path;
            do
            {
                path = GeneratePath(basePath);
            } while (Directory.Exists(path));
            return path;
        }

        #endregion
    }
}