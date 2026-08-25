using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VNTextPatch.Shared.Scripts
{
    public class FolderScriptCollection : IScriptCollection
    {
        private static readonly IScript[] TemporaryScripts;

        static FolderScriptCollection()
        {
            TemporaryScripts =
                new IScript[]
                {
                    new QlieScript(),
                    new JsonScript()
                };
        }

        public FolderScriptCollection(string folderPath, string extension, string format = null)
        {
            if (!Directory.Exists(folderPath))
                throw new DirectoryNotFoundException($"{folderPath} does not exist");

            FolderPath = folderPath;
            Extension = extension ?? string.Empty;
            Format = format;
        }

        public string Name
        {
            get { return FolderPath; }
        }

        public string FolderPath
        {
            get;
        }

        public string Extension
        {
            get;
        }

        public string Format
        {
            get;
        }

        public IScript GetTemporaryScript()
        {
            IScript script;
            if (Format != null)
            {
                string typeName = Format + "Script";
                script = TemporaryScripts.FirstOrDefault(f => f.GetType().Name.Equals(typeName, StringComparison.InvariantCultureIgnoreCase));
                if (script == null)
                    throw new NotSupportedException($"Format {Format} is not supported");
            }
            else
            {
                script = TemporaryScripts.FirstOrDefault(f => !string.IsNullOrEmpty(f.Extension) && f.Extension.Equals(Extension, StringComparison.InvariantCultureIgnoreCase));
                if (script == null)
                    throw new NotSupportedException($"Extension {Extension} is not supported");
            }
            return script;
        }

        public IEnumerable<string> Scripts
        {
            get
            {
                int folderPathLength = FolderPath.Length;
                if (!Name.EndsWith("\\"))
                    folderPathLength++;

                return Directory.EnumerateFiles(Name, "*" + Extension, SearchOption.AllDirectories)
                                .Select(f => f.Substring(folderPathLength));
            }
        }

        public bool Exists(string scriptName)
        {
            return File.Exists(Path.Combine(FolderPath, scriptName));
        }

        public void Add(string scriptName)
        {
            string filePath = Path.Combine(FolderPath, scriptName);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            File.Create(filePath).Close();
        }

        public void Add(string scriptName, ScriptLocation copyFrom)
        {
            string sourceFilePath = copyFrom.ToFilePath();
            string destFilePath = Path.Combine(FolderPath, copyFrom.ScriptName);
            Directory.CreateDirectory(Path.GetDirectoryName(destFilePath));
            File.Copy(sourceFilePath, destFilePath, true);
        }

        public override string ToString()
        {
            return FolderPath;
        }
    }
}
