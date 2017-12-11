/*
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
    public static void Start()
    {
        Start(defaultUI);
    }

    /// <summary>
    /// 配置文件
    /// </summary>
    public static RedFrameworkSettings settings;

    /// <summary>
    /// 框架入口
    /// </summary>
    public static void Start(UISettings ui)
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
        GameObject cameraController = new GameObject("CameraController");
        cameraController.AddComponent<CameraController>();
        GameObject.DontDestroyOnLoad(cameraController);

        //创建Canvas
        GameObject canvas = new GameObject(UIManager.NAME);
        canvas.AddComponent<UIManager>().Set(ui);
        GameObject.DontDestroyOnLoad(canvas);

    #if UNITY_EDITOR || DEVELOPMENT_BUILD
        //创建GM命令输入
        GameObject gmInput = new GameObject("GMInput");
        gmInput.AddComponent<GMInput>();
        GameObject.DontDestroyOnLoad(gmInput);
    #endif
    }

    #region 默认配置
    private static UISettings? _defaultUI;
    public static UISettings defaultUI
    {
        get
        {
            if (_defaultUI == null)
            {
                _defaultUI = new UISettings();
            }
            return _defaultUI.Value;
        }
    }

    private static HUDSettings? _defaultHUD;
    public static HUDSettings defaultHUD
    {
        get
        {
            if (_defaultHUD == null)
            {
                _defaultHUD = new HUDSettings();
            }
            return _defaultHUD.Value;
        }
    }
    #endregion
}
