/*
 * Author:  Rick
 * Create:  2017/11/2 13:39:32
 * Email:   rickjiangshu@gmail.com
 * Follow:  https://github.com/RickJiangShu
 */
using UnityEngine;

/// <summary>
/// HUD面板配置
/// </summary>
public struct HUDSettings
{
    public int layer;//层级

    //字体设置
    public Font font;

    //出场（缩放）设置
    public float enterScale;
    public float enterDuration;

    //退出（移动）设置
    public Vector2 exitOffset;
    public float exitDuration;
}
