/*
 * Author:  Rick
 * Create:  2017/10/21 17:15:54
 * Email:   rickjiangshu@gmail.com
 * Follow:  https://github.com/RickJiangShu
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UIManager
/// </summary>
public class UIManager : MonoBehaviour
{
    /// <summary>
    /// 名字
    /// </summary>
    public const string NAME = "Canvas(UIManager)";
    
    /// <summary>
    /// GameObject的Layer
    /// </summary>
    private const int UNITY_UI_LAYER = 5;

    /// <summary>
    /// 层级之间的距离（米）
    /// </summary>
    private const float LAYER_DISTANCE = 5f;

    private static Camera camera;
    private static Canvas canvas;
    private static CanvasScaler scaler;

    /// <summary>
    /// 分辨率
    /// </summary>
    public static Vector2 resolution = Vector2.zero;

    /// <summary>
    /// 层级（0 UI层 1 Panel层）
    /// </summary>
    private static RectTransform[] layers = new RectTransform[2];
    
    void Awake()
    {
        //设置层级
        gameObject.layer = UNITY_UI_LAYER;

        //Canvas
        canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;

        scaler = gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.matchWidthOrHeight = 1f;
        scaler.referenceResolution = resolution;

        //Camera
        camera = new GameObject("UICamera", typeof(Camera)).GetComponent<Camera>();
        AddChild(camera.gameObject, transform);
        camera.orthographic = true;
        camera.orthographicSize = resolution.y * 0.005f;
        camera.nearClipPlane = 0f;
        camera.farClipPlane = LAYER_DISTANCE * layers.Length + 0.3f;
        camera.transform.localPosition = new Vector3(0f, 0f, -LAYER_DISTANCE * layers.Length * 100f);
        canvas.worldCamera = camera;

        //Instantiate Containers
        for (int i = 0, l = layers.Length; i < l; i++)
        {
            GameObject container = new GameObject(name, typeof(RectTransform));
            AddChild(container, transform);

            container.name = "Layer" + i;

            RectTransform rect = (RectTransform)container.transform;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.localPosition = new Vector3(0f, 0f, -LAYER_DISTANCE * i * 100f);
            layers[i] = rect;
        }
    }

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void Instantiate(float width = 768f, float height = 1366f)
    {
        resolution = new Vector2(width, height);

        GameObject go = new GameObject(NAME, typeof(RectTransform), typeof(UIManager_));
        go.layer = UNITY_UI_LAYER;
    }


    /// <summary>
    /// 显示HUDText
    /// </summary>
    /// <param name="content"></param>
    /// <param name="?"></param>
    public static void ShowHUDText(string content,Vector3 worldPosition)
    {
        GameObject go = Warehouser.Pull(HUDText.NAME);
        HUDText hudText;
        if (go == null)
        {
            hudText = HUDText.Instantiate();
        }
        else
        {
            hudText = go.GetComponent<HUDText>();
        }

        AddChild(hudText.gameObject, 2);
       // hudText.SetData(content);
    }

    /// <summary>
    /// 添加到层级
    /// </summary>
    /// <param name="go"></param>
    /// <param name="layer"></param>
    public static void AddChild(GameObject go, int layer = 1)
    {
        AddChild(go, layers[layer]);
    }

    public static void AddChild(GameObject go, Transform parent)
    {
        go.transform.SetParent(parent, false);
        go.layer = UNITY_UI_LAYER;
    }
}
