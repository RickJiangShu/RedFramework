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
using DG.Tweening;

/// <summary>
/// head-up display 头顶显示的文本（如飘学）
/// </summary>
public class HUDText : MonoBehaviour
{
    public const string NAME = "HUDText";

    private Text text;
    private Shadow shadow;

    private int step = 0;//阶段 0 未开始 1 缩放出场 2 上移消失

    void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("UI");

        text = gameObject.AddComponent<Text>();
        text.alignment = TextAnchor.MiddleCenter;

        ContentSizeFitter fitter = gameObject.AddComponent<ContentSizeFitter>();
        fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        shadow = gameObject.AddComponent<Shadow>();
    }
    // Use this for initialization
    void Start()
    {

    }

    public void Set(string content, int fontSize, Color color, Vector3 target, HUDSettings settings)
    {
        //设置配置
        text.font = settings.font;
        text.fontSize = fontSize;
        text.color = color;

        //设置文本
        text.text = content;
    }

    public void Play(HUDSettings settings)
    {
        //出现变大->恢复
        Sequence queue = DOTween.Sequence();

        queue.Append( transform.DOScale(settings.enterScale, settings.enterDuration * 0.5f));
        queue.Append( transform.DOScale(1, settings.enterDuration * 0.5f));

        //向上飘渐隐
        Vector3 exitPos = transform.localPosition;
        exitPos.x += settings.exitOffset.x;
        exitPos.y += settings.exitOffset.y;
        queue.Append(transform.DOLocalMove(exitPos, settings.exitDuration));
        queue.Join(text.DOFade(0f, settings.exitDuration));

        queue.OnComplete(OnCompelte);
    }

    private void OnCompelte()
    {
        Warehouser.Push(gameObject);
    }
}
