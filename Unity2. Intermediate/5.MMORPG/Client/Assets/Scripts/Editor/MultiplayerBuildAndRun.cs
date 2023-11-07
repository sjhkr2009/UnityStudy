using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class MultiplayerBuildAndRun {
    [MenuItem("Custom Tools/Run MultiPlayer/2 Players")]
    static void PerformWin64BuildFor2Player() => PerformWin64Build(2);
    
    [MenuItem("Custom Tools/Run MultiPlayer/3 Players")]
    static void PerformWin64BuildFor3Player() => PerformWin64Build(3);
    
    [MenuItem("Custom Tools/Run MultiPlayer/4 Players")]
    static void PerformWin64BuildFor4Player() => PerformWin64Build(4);
    
    static void PerformWin64Build(int playerCount) {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);

        for (int i = 0; i < playerCount; i++) {
            BuildPipeline.BuildPlayer(
                GetScenePaths(),
                $"Builds/Win64/{GetProjectName()}{i}/{GetProjectName()}{i}.exe",
                BuildTarget.StandaloneWindows64,
                BuildOptions.AutoRunPlayer
            );
        }
    }

    static string GetProjectName() {
        var appPath = Application.dataPath.Split('/');
        return appPath[appPath.Length - 2];
    }

    static string[] GetScenePaths() {
        return EditorBuildSettings.scenes.Select(s => s.path).ToArray();
    }
}
