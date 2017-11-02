/*
 * Author:  Rick
 * Create:  2017/11/2 13:39:32
 * Email:   rickjiangshu@gmail.com
 * Follow:  https://github.com/RickJiangShu
 */
using UnityEngine;

/// <summary>
/// HUDSettings
/// </summary>
public struct HUDSettings
{
    //字体设置
    public Font font;
    public int fontSize;
    public Color color;

    //出场（缩放）设置
    public Vector2 enterOffset;//头顶偏移（像素）
    public float enterScale;

    //退出（移动）设置

    private static HUDSettings defaultSettings;
    public static HUDSettings GetDefault()
    {
        /*
        if(defaultSettings == null)
        {
            defaultSettings = new HUDSettings();

        }
         */
        defaultSettings = new HUDSettings()
        {
            font = Resources.GetBuiltinResource<Font>("Arial.ttf"),
            fontSize = 14,
            color = Color.black,

            enterOffset = new Vector3(0f,100f),
            enterScale = 2,
        };

        return defaultSettings;
    }
}
