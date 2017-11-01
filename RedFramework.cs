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
    /// <summary>
    /// 框架入口
    /// </summary>
    public static void Start(UISettings uiSettings)
    {
        //启动Warehouser
        Warehouser.Start();

        //启动ConfigManager
        SerializableSet set = Warehouser.GetAsset<SerializableSet>("SerializableSet");
        Deserializer.Deserialize(set);
        Warehouser.UnloadAsset(set);

        //创建Canvas
        GameObject canvas = new GameObject(UIManager.NAME);
        canvas.AddComponent<UIManager>().Set(uiSettings);
        GameObject.DontDestroyOnLoad(canvas);

#if TEST
        //创建GM命令输入
        GameObject gmInput = new GameObject("GMInput");
        gmInput.AddComponent<GMInput>();
        GameObject.DontDestroyOnLoad(gmInput);
#endif
    }
}
