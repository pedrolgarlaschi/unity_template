using UnityEngine;
using UnityEditor;

public class BuildSettingsAsset : ScriptableObject
{
#if UNITY_EDITOR
    [System.Serializable]
    public class Target
    {
        public BuildTargetGroup group = BuildTargetGroup.Standalone;
        public BuildTarget target = BuildTarget.StandaloneWindows;
    }


    public string projectName;
    public Target[] targets;
    public SceneAsset[] scenes;
    public int major = 1;
    public int minor = 0;
    public int patch = 0;
#endif
}
