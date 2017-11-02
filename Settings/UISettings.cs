/*
 * Author:  Rick
 * Create:  2017/11/1 15:49:44
 * Email:   rickjiangshu@gmail.com
 * Follow:  https://github.com/RickJiangShu
 */
using UnityEngine;
/// <summary>
/// UI设置
/// </summary>
public struct UISettings 
{
    public Vector2 resolution;
    public Vector3 offset;

    public static UISettings GetDefault()
    {
        UISettings settings = new UISettings();
        settings.resolution = new Vector2(768f, 1366f);
        settings.offset = Vector3.zero;
        return settings;
    }
}
