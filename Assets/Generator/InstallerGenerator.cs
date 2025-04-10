#if UNITY_EDITOR
using System.IO;
using System.Linq;
using System.Text;
using Generator.Utils.ScriptHandler.Impl;
using Generator.Utils.ScriptsParser;
using UnityEditor;
using UnityEngine;

namespace Generator
{
    public class InstallerGenerator
    {
        private const string PATH = "Scripts/Game/Generated";

        [MenuItem("Tools/Generate ControllerInstaller &g")]
        public static void GenerateManual()
        {
            var installerTemplates = Generate();
            var directoryPath = Path.Combine(Application.dataPath, PATH);
                
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            installerTemplates.SaveToFile(directoryPath);
                
            Debug.Log($"{nameof(InstallerGenerator)} | Generated controllers: {installerTemplates.Counter}");

            AssetDatabase.Refresh();
        }

        private static InstallerTemplate Generate()
        {
            var scriptsHandler = new ControllersScriptsHandler();
            ScriptsTraveler.Run(Application.dataPath, scriptsHandler);
            
            var collectedTemplates = scriptsHandler.GetParsedInstallersTemplates();

            var paths = PATH.Split("/").ToList();
            var nameSpaceBuilder = new StringBuilder();

            for (var index = 1; index < paths.Count; index++)
            {
                var path = paths[index];
                nameSpaceBuilder.Append(path);
                
                if (index < paths.Count - 1)
                    nameSpaceBuilder.Append(".");
            }

            var generatedInstallerCode = ControllerInstallerGenerator.GenerateInstaller(InstallerTemplate.Name,
                collectedTemplates.Container, collectedTemplates.Namespaces, nameSpaceBuilder.ToString());
            
            collectedTemplates.GeneratedInstallerCode = generatedInstallerCode;
            
            return collectedTemplates;
        }
    }
}
#endif