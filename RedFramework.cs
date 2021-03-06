﻿/*
 * Author:  Rick
 * Create:  2017/11/1 14:01:03
 * Email:   rickjiangshu@gmail.com
 * Follow:  https://github.com/RickJiangShu
 */
using UnityEngine;

/// <summary>
/// 框架静态类
/// </summary>
public class RedFramework
{
    /// <summary>
    /// 配置文件
    /// </summary>
    public static RedFrameworkSettings settings;

    /// <summary>
    /// 框架入口
    /// </summary>
    public static void Start()
    {
        //加载配置
        settings = Resources.Load<RedFrameworkSettings>("RedFrameworkSettings");

        //启动Warehouser
        Warehouser.Start();

        //启动ConfigManager
        SerializableSet set = Warehouser.GetAsset<SerializableSet>("SerializableSet");
        Deserializer.Deserialize(set);
        Warehouser.Unload("base/config.ab", true);

        //创建摄像机
        GameObject cameraController = Warehouser.NewObject("CameraController");
        cameraController.AddComponent<CameraController>();
        GameObject.DontDestroyOnLoad(cameraController);

        //创建Canvas
        GameObject canvas = Warehouser.NewObject(UIManager.NAME);
        canvas.AddComponent<UIManager>();
        GameObject.DontDestroyOnLoad(canvas);

    #if UNITY_EDITOR || DEVELOPMENT_BUILD
        //创建GM命令输入
        GameObject gmInput = Warehouser.NewObject("GMInput");
        gmInput.AddComponent<GMInput>();
        GameObject.DontDestroyOnLoad(gmInput);
    #endif
    }
}
