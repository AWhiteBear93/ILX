using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using UnityEditor.Build.Reporting;

public class BuildScript : MonoBehaviour
{
    const string OUTPUT_FILENAME = "-outputname";
    const string OUTPUT_PATH = "-outputpath";
    const string BUILD_OPTION = "-buildoption";



    private static Dictionary<string, string> GetComnandLineArguments()
    {
        string[] args = System.Environment.GetCommandLineArgs();
        Dictionary<string, string> arguments = new Dictionary<string, string>();
        for (int i = 0; i < args.Length; i++)
        {
            var nextIndex = i + 1;
            if (nextIndex > args.Length - 1)
            {
                break;
            }

            var argName = args[i];
            var argValue = args[++i];
            Debug.LogFormat("ArgName {0}, ArgValue {1}", argName, argValue);
            arguments.Add(argName, argValue);
        }
        return arguments;
    }

    static string[] GetLevelsFromBuildSetting()
    {
        List<string> levels = new List<string>();
        for (int i = 0; i < EditorBuildSettings.scenes.Length; ++i)
        {
            if (EditorBuildSettings.scenes[i].enabled)
            {
                levels.Add(EditorBuildSettings.scenes[i].path);
            }
        }

        return levels.ToArray();
    }

    public static void PerformBuildWindowsPlayer()
    {
        try
        {
            string[] levels = GetLevelsFromBuildSetting();
            var buildArguments = GetComnandLineArguments();

            var buildOption = BuildOptions.None;
            BuildReport buildReport;
            if (buildArguments[BUILD_OPTION] == "dev")
            {
                buildOption = BuildOptions.Development;
            }

            buildReport = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = levels,
                target = EditorUserBuildSettings.activeBuildTarget,
                locationPathName = Path.Combine(buildArguments[OUTPUT_PATH], buildArguments[OUTPUT_FILENAME]),
                targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup,
                options = buildOption,
            });

            switch (buildReport.summary.result)
            {
                case BuildResult.Succeeded:
                    EditorApplication.Exit(0);
                    break;
                default:
                    EditorApplication.Exit(1);
                    break;
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Build Failed: " + ex.Message);
            EditorApplication.Exit(1);
        }
    }
}
