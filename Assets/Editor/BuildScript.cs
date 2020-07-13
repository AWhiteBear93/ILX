using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BuildScript : MonoBehaviour
{
    [MenuItem("Tool/Build")]
    public static void BuildFunctionTest()
    {
        string[] includeScenes = new string[EditorBuildSettings.scenes.Length];
        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            includeScenes[i] = EditorBuildSettings.scenes[i].path;
        }

        BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = includeScenes,
            target = EditorUserBuildSettings.activeBuildTarget,
            locationPathName = "F:\\GitLib\\gitLib_1\\Build\\UnityBuildTest.exe",
            targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup,
            options = BuildOptions.Development,

        });
    }
}
