/*
 * Author:  Rick
 * Create:  2017/11/8 13:58:11
 * Email:   rickjiangshu@gmail.com
 * Follow:  https://github.com/RickJiangShu
 */

/// <summary>
/// PanelSettings
/// </summary>
[System.Serializable]
public class PanelSettings
{
    public string name;
    public string[] preloadAtlases;//加载面板之前预加载的图集
    public bool recyclable;//是否可回收
}
