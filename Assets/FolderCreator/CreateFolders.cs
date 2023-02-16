using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEngine.WSA;

public class CreateFolders : EditorWindow
{
    private static string projectName = "ProjectName";

    [MenuItem("Assets/Create Default Folders")]
    private static void SetUpFolders() { 
        CreateFolders window = ScriptableObject.CreateInstance<CreateFolders>();
        window.position = new Rect(Screen.width / 2 + 400, Screen.height / 2 + 150, 400, 150);
        window.ShowPopup();
    }

    private static void CreateAllFolders() {
        List<string> folders = new List<string>() {
            "Art","Audio","Code","Docs","Level"
        };

        string rootPath = "Assets/";

        foreach (string folder in folders)
        {
            if (!Directory.Exists(rootPath+folder)) {
                Directory.CreateDirectory(rootPath + projectName + "/" + folder);
            }

            CreateSubFolders(rootPath + projectName, "/Art/", new List<string>() { 
                "Materials","Models","Textures"
            });

            CreateSubFolders(rootPath + projectName, "/Audio/", new List<string>() {
                "Music","Sound"
            });

            CreateSubFolders(rootPath + projectName, "/Code/", new List<string>() {
                "Scripts","Shaders"
            });

            CreateSubFolders(rootPath + projectName, "/Level/", new List<string>() {
                "Prefabs","Scenes","UI"
            });
        }

        AssetDatabase.Refresh();
    }

    private static void CreateSubFolders(string rootPath,string folderName,List<string> subFolders) {
        foreach (string subFolder in subFolders)
        {
            if(!Directory.Exists(rootPath+folderName+subFolder))
            {
                Directory.CreateDirectory(rootPath + folderName + subFolder);
            }
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Insert the project name used as the root folder",EditorStyles.boldLabel);
        GUILayout.Space(20);
        projectName = EditorGUILayout.TextField(projectName);
        GUILayout.Space(50);
        GUILayout.BeginHorizontal();
        GUILayout.Space(10);
        if (GUILayout.Button("Cancel"))
        {
            this.Close();
        }
        GUILayout.Space(10);
        if (GUILayout.Button("Generate")) {
            CreateAllFolders();
            this.Close();
        }
        GUILayout.Space(10);
        GUILayout.EndHorizontal();
    }
}
