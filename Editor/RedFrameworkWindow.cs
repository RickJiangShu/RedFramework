/*
 * Author:  Rick
 * Create:  2017/11/7 15:14:08
 * Email:   rickjiangshu@gmail.com
 * Follow:  https://github.com/RickJiangShu
 */
using UnityEngine;
using UnityEditor;

/// <summary>
/// RedFrameworkWindow
/// </summary>
public class RedFrameworkWindow : EditorWindow
{
    /// <summary>
    /// 配置文件路径
    /// </summary>
    public const string SETTINGS_PATH = "Assets/Resources/RedFrameworkSettings.asset";

    /// <summary>
    /// 配置文件
    /// </summary>
    private RedFrameworkSettings settings;
    private SerializedObject serializedSettings;

    [MenuItem("Window/RedFramework")]
    public static RedFrameworkWindow Get()
    {
        return EditorWindow.GetWindow<RedFrameworkWindow>("RedFramework");
    }

    void Awake()
    {
        LoadSettings();
    }

    void OnGUI()
    {
        //Camera
        GUILayout.Label("Camera", EditorStyles.boldLabel);

        SerializedProperty camera = serializedSettings.FindProperty("camera");
        EditorGUILayout.PropertyField(camera, true);

        if (GUI.changed)
        {
            serializedSettings.ApplyModifiedProperties();
        }
    }

    /// <summary>
    /// 加载Setting
    /// </summary>
    private void LoadSettings()
    {
        settings = AssetDatabase.LoadAssetAtPath<RedFrameworkSettings>(SETTINGS_PATH);
        if (settings == null)
        {
            settings = new RedFrameworkSettings();
            AssetDatabase.CreateAsset(settings, SETTINGS_PATH);
        }
        serializedSettings = new SerializedObject(settings);
    }
}