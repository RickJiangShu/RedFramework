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

    /// <summary>
    /// 根节点（自身）
    /// </summary>
    private static RectTransform root;
    private static Camera camera;
    private static Canvas canvas;
    private static CanvasScaler scaler;

    /// <summary>
    /// 分辨率
    /// </summary>
    public static Vector2 resolution = Vector2.zero;

    /// <summary>
    /// 层级
    /// </summary>
    private static RectTransform[] layers = new RectTransform[8];
    
    void Awake()
    {
        //设置层级
        gameObject.layer = UNITY_UI_LAYER;

        root = gameObject.AddComponent<RectTransform>();

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

            RectTransform layer = (RectTransform)container.transform;
            layer.offsetMin = Vector2.zero;
            layer.offsetMax = Vector2.zero;
            layer.anchorMin = Vector2.zero;
            layer.anchorMax = Vector2.one;
            layer.localPosition = new Vector3(0f, 0f, -LAYER_DISTANCE * i * 100f);
            layers[i] = layer;
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
    public static void AddChild(GameObject go, int layer)
    {
        AddChild(go, layers[layer]);
    }

    public static void AddChild(GameObject go, Transform parent)
    {
        go.transform.SetParent(parent, false);
    }

    /// <summary>
    /// 删除某一层所有的子对象
    /// </summary>
    /// <param name="layer"></param>
    public static void RemoveChildren(int layer)
    {
        foreach (Transform child in layers[layer])
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// 从画布坐标转换到世界坐标
    /// </summary>
    /// <returns></returns>
    public static Vector3 TransformPoint(Vector3 canvasPosition)
    {
        float w = root.rect.width * 0.5f;
        float h = root.rect.height * 0.5f;
        Vector3 viewport = new Vector3((canvasPosition.x / w + 1f) / 2, (canvasPosition.y / h + 1f) / 2, 0f);
        return Camera.main.ViewportToWorldPoint(viewport);
    }

    /// <summary>
    /// 从世界坐标转换到画布坐标
    /// </summary>
    /// <returns></returns>
    public static Vector3 InverseTransformPoint(Vector3 worldPosition)
    {
        Vector2 viewport = Camera.main.WorldToViewportPoint(worldPosition);
        viewport = (viewport - root.pivot) * 2;
        float w = root.rect.width * 0.5f;
        float h = root.rect.height * 0.5f;
        return new Vector2(viewport.x * w, viewport.y * h);
    }
}
