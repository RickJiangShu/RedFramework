/*
 * Author:  Rick
 * Create:  2017/10/22 13:52:10
 * Email:   rickjiangshu@gmail.com
 * Follow:  https://github.com/RickJiangShu
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// head-up display 头顶显示的文本（如飘学）
/// </summary>
public class HUDText : MonoBehaviour
{
    public const string NAME = "HUDText";

    private Text text;
    private Shadow shadow;


    void Awake()
    {
        text = gameObject.AddComponent<Text>();
        shadow = gameObject.AddComponent<Shadow>();
    }
    // Use this for initialization
    void Start()
    {

    }

    public void SetData(string content)
    {

    }

    public void Play()
    {
        //出现变大->恢复

        //向上飘渐隐
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 实例化
    /// </summary>
    /// <returns></returns>
    public static HUDText Instantiate()
    {
        GameObject go = new GameObject(NAME,typeof(RectTransform));

#if OBSERVER
        go.AddComponent<Plugins.Warehouser.Observer.Observer>();
#endif
        return go.AddComponent<HUDText>();
    }
}
