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
[System.Serializable]
public struct UISettings 
{
    /// <summary>
    /// 分辨率
    /// </summary>
    public Vector2 resolution;

    /// <summary>
    /// 位置
    /// </summary>
    public Vector3 offset;

    /// <summary>
    /// 面板层
    /// </summary>
    public int panelLayer;

    /// <summary>
    /// 面板配置
    /// </summary>
    public PanelSettings[] panels;

    /// <summary>
    /// HUD层
    /// </summary>
    public int hudLayer;

    /// <summary>
    /// 多种HUD配置
    /// </summary>
    public HUDSettings[] HUD;
}
