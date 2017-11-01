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
    #region 常量

    #endregion

    /// <summary>
    /// 框架入口
    /// </summary>
    public static void Start(float resolutionX = 768f ,float resolutionY = 1366f)
    {
        //启动Warehouser

        //启动ConfigManager

        //创建Canvas
        UIManager.resolution.x = resolutionX;
        UIManager.resolution.y = resolutionY;
        GameObject canvas = new GameObject(UIManager.NAME, typeof(RectTransform));
        canvas.AddComponent<UIManager>();
    }
}