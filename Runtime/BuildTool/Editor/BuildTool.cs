using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class BuildTool : MonoBehaviour
{
#if UNITY_EDITOR
    public static string RootFolder
    {
        get
        {
            string[] root = Application.dataPath.Split('/');
            string str = "";

            int len = root.Length - 1;
            for (int i = 0; i < len; i++)
                str += root[i] + "/";


            return str;
        }
    }


    public static string SettingsPath
    {
        get
        {
            string path = Application.dataPath + "/TJ/BuildTool/BuildSettings.asset";


            return path;
        }
    }


    public static string BuildPath
    {
        get
        {
            string path = RootFolder + "Build";


            return path;
        }
    }

    public static string GetEtentionPerPlatform(BuildTarget target)
    {
        switch (target)
        {
            case BuildTarget.iOS:
                return "";
            case BuildTarget.Android:
                return "apk";
            case BuildTarget.StandaloneWindows:
                return "exe";

        }

        return "";
    }

    public static BuildSettingsAsset BuildSettings
    {
        get
        {
            return (BuildSettingsAsset)AssetDatabase.LoadAssetAtPath("Assets/TJ/BuildTool/BuildSettings.asset", typeof(BuildSettingsAsset));
        }
    }

    public static void ChangeProjectSettings(BuildSettingsAsset settings)
    {
        PlayerSettings.bundleVersion = settings.major + "." + settings.minor + "." + settings.patch;
        PlayerSettings.companyName = "Taylor James";
        PlayerSettings.productName = settings.projectName;
    }


    [MenuItem("TJ/Build/Create Build Asset", false, 100)]
    public static void CreateBuildAsset()
    {
        BuildSettingsAsset asset = ScriptableObject.CreateInstance<BuildSettingsAsset>();

        asset.targets = new BuildSettingsAsset.Target[1] { new BuildSettingsAsset.Target()};

        string path = Application.dataPath + "/TJ/BuildTool";

        CreateDirectory(path);

        if (File.Exists(SettingsPath))
        {
            bool value = EditorUtility.DisplayDialog("Build Warning", "You already have a Build Settings file. Are you sure you want to replave it?", "Yes", "No");
            if (!value) return;
        }


        AssetDatabase.CreateAsset(asset, "Assets/TJ/BuildTool/BuildSettings.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;

        string[] projectName = Application.dataPath.Split('/');
        asset.projectName = projectName[projectName.Length - 2];
    }

    [MenuItem("TJ/Build/Build Main Platform  %#F1", false, 101)]
    public static void BuildMain()
    {
        CreateFileForPlatform(BuildSettings.targets[0].target);
        Process.Start(BuildPath + "/" + BuildSettings.targets[0].target.ToString());
    }

    [MenuItem("TJ/Build/Build All Platforms %#F2", false, 102)]
    public static void BuildAll()
    {
        foreach (BuildSettingsAsset.Target t in BuildSettings.targets)
            CreateFileForPlatform(t.target);

        Process.Start(BuildPath);

        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildSettings.targets[0].group, BuildSettings.targets[0].target);
    }

   
    private static void CreateFileForPlatform(BuildTarget t)
    {
            string platformPath = BuildPath + "/" + t.ToString();
            CreateDirectory(platformPath);

            string version = "v" + BuildSettings.major + "." + BuildSettings.minor + "." + BuildSettings.patch;

            string versionPath = platformPath + "/" + version;

            CreateDirectory(versionPath, true);

            string[] levels = new string[BuildSettings.scenes.Length];
            int len = levels.Length;

            for (int i = 0; i < len; i++)
            {
                levels[i] = AssetDatabase.GetAssetPath(BuildSettings.scenes[i]);
            }

            BuildPipeline.BuildPlayer(levels, versionPath + "/" + BuildSettings.projectName + "." + GetEtentionPerPlatform(t), t, BuildOptions.None);
    }

    private static void CreateDirectory(string path , bool deleteIfExist = false)
    {
        if (deleteIfExist)
        {
            if (Directory.Exists(path))
                Directory.Delete(path , true);
        }

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }

#endif

}
