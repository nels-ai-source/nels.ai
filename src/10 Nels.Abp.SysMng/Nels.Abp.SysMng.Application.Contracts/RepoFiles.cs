// Copyright (c) Microsoft. All rights reserved.

using System.IO;
using System.Reflection;
using Volo.Abp;

namespace Nels.Abp.SysMng;

public static class RepoFiles
{
    /// <summary>
    /// Scan the local folders from the repo, looking for "Plugins" folder.
    /// </summary>
    /// <returns>The full path to Plugins folder.</returns>
    public static string SamplePluginsPath()
    {
        const string Folder = "Plugins";

        static bool SearchPath(string pathToFind, out string result, int maxAttempts = 10)
        {
            var currDir = Path.GetFullPath(typeof(RepoFiles).GetTypeInfo().Assembly.Location);
            bool found;
            do
            {
                result = Path.Join(currDir, pathToFind);
                found = Directory.Exists(result);
                currDir = Path.GetFullPath(Path.Combine(currDir, ".."));
            } while (maxAttempts-- > 0 && !found);

            return found;
        }

        if (!SearchPath(Folder, out var path))
        {
            throw new BusinessException("Plugins directory not found. The app needs the plugins from the repo to work.");
        }

        return path;
    }
}
