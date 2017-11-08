/*
 * Author:  Rick
 * Create:  2017/10/21 17:15:54
 * Email:   rickjiangshu@gmail.com
 * Follow:  https://github.com/RickJiangShu
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

/// <summary>
/// UIManager
/// </summary>
public class UIManager : MonoBehaviour
{
    #region 常量
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
    #endregion

    #region 自身组件
    private static RectTransform root;
    private static Camera camera;
    private static Canvas canvas;
    private static CanvasScaler scaler;
    private static GraphicRaycaster raycaster;
    #endregion

    /// <summary>
    /// 整个UI是否可交互
    /// </summary>
    public static bool interactable
    {
        get { return raycaster.enabled; }
        set { raycaster.enabled = value; }
    }

    /// <summary>
    /// 层级
    /// </summary>
    private static RectTransform[] layers = new RectTransform[8];

    /// <summary>
    /// 面板设置
    /// </summary>
    private static Dictionary<string, PanelSettings> panelSettings = new Dictionary<string, PanelSettings>();

    /// <summary>
    /// 所有当前存在的面板
    /// </summary>
    private static Dictionary<string, GameObject> panels = new Dictionary<string,GameObject>();

    void Awake()
    {
        //索引配置
        for (int i = 0, l = RedFramework.settings.ui.panels.Length; i < l; i++)
        {
            PanelSettings settings = RedFramework.settings.ui.panels[i];
            panelSettings[settings.name] = settings;
        }

        root = gameObject.AddComponent<RectTransform>();
        root.position = RedFramework.settings.ui.offset;

        //设置层级
        gameObject.layer = UNITY_UI_LAYER;

        //Canvas
        canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;

        scaler = gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = RedFramework.settings.ui.resolution;
        scaler.matchWidthOrHeight = 1f;

        raycaster = gameObject.AddComponent<GraphicRaycaster>();

        //Camera
        camera = Warehouser.NewObject("UICamera").AddComponent<Camera>();
        camera.gameObject.layer = UNITY_UI_LAYER;
        camera.transform.SetParent(root, false);
        camera.clearFlags = CameraClearFlags.Depth;
        camera.cullingMask = 1 << UNITY_UI_LAYER;
        camera.orthographic = true;
        camera.nearClipPlane = 0f;
        camera.farClipPlane = LAYER_DISTANCE * layers.Length + 0.3f;
        camera.transform.localPosition = new Vector3(0f, 0f, -LAYER_DISTANCE * layers.Length * 100f);
        canvas.worldCamera = camera;
        camera.orthographicSize = scaler.referenceResolution.y * 0.005f;

        //Instantiate Containers
        for (int i = 0, l = layers.Length; i < l; i++)
        {
            GameObject container = Warehouser.NewObject("Layer" + i, typeof(RectTransform));
            container.layer = UNITY_UI_LAYER;
            container.transform.SetParent(root, false);

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
    /// 显示面板
    /// </summary>
    /// <param name="name"></param>
    public static void ShowPanel(string name)
    {
        PanelSettings settings = panelSettings[name];
        
        //加载依赖图集
        foreach (string atlasName in settings.preloadAtlases)
        {
            Warehouser.GetAsset<SpriteAtlas>(atlasName);
        }

        //
        GameObject panel;
        if(settings.recyclable)
            panel = Warehouser.GetInstance(name);
        else 
            panel = Warehouser.Instantiate(name);

        AddChild(panel, RedFramework.settings.ui.panelLayer);
        panels.Add(name, panel);
    }

    public static void HidePanel(string name)
    {
        PanelSettings settings = panelSettings[name];
        
        GameObject panel = panels[name];
        if (settings.recyclable)
            Warehouser.Push(panel);
        else
            GameObject.Destroy(panel);

        panels.Remove(name);
    }

    /// <summary>
    /// 获取某个界面
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static GameObject GetPanel(string name)
    {
        return panels[name];
    }

    /// <summary>
    /// 隐藏所有界面
    /// </summary>
    public static void HideAllPanels()
    {
        string[] names = new string[panels.Count];
        panels.Keys.CopyTo(names, 0);
        foreach (string name in names)
        {
            HidePanel(name);
        }
    }

    /// <summary>
    /// 显示HUDText
    /// </summary>
    public static void HUD(string content, int fontSize, Color color, Vector3 target, HUDSettings settings)
    {
        GameObject go = Warehouser.GetObject(HUDText.NAME, typeof(RectTransform), typeof(HUDText));
        HUDText hudText = go.GetComponent<HUDText>();

        //设置位置
        go.transform.localPosition = MainCamera2Canvas(target) + new Vector2(0f, 100f);

        //添加到UI层
        AddChild(hudText.gameObject, settings.layer);

        //设置并播放
        hudText.Set(content, fontSize, color, target, settings);
        hudText.Play(settings);
    }

    /// <summary>
    /// 添加到层级
    /// </summary>
    /// <param name="go"></param>
    /// <param name="layer"></param>
    public static void AddChild(GameObject go, int layer, bool worldPositionStays = false)
    {
        go.transform.SetParent(layers[layer], worldPositionStays);
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

#region 坐标转换
    /// <summary>
    /// 从主摄像机中的坐标转换到画布坐标
    /// </summary>
    /// <returns></returns>
    public static Vector2 MainCamera2Canvas(Vector3 worldPosition)
    {
        Vector2 viewport = Camera.main.WorldToViewportPoint(worldPosition);
        viewport = (viewport - root.pivot) * 2;
        float w = root.rect.width * 0.5f;
        float h = root.rect.height * 0.5f;
        return new Vector2(viewport.x * w, viewport.y * h);
    }

    /// <summary>
    /// 从画布坐标转换到主摄像机中的坐标
    /// </summary>
    /// <returns></returns>
    public static Vector3 Canvas2MainCamera(Vector2 canvasPosition)
    {
        float w = root.rect.width * 0.5f;
        float h = root.rect.height * 0.5f;
        Vector3 viewport = new Vector3((canvasPosition.x / w + 1f) / 2, (canvasPosition.y / h + 1f) / 2, 0f);
        return Camera.main.ViewportToWorldPoint(viewport);
    }

    /// <summary>
    /// 世界坐标转换为相对于Canvas坐标
    /// </summary>
    /// <returns></returns>
    public static Vector2 World2Canvas(Vector2 position)
    {
        return root.InverseTransformPoint(position);
    }

    /// <summary>
    /// 相对于Canavs坐标转换为世界坐标
    /// </summary>
    /// <param name="localPosition"></param>
    /// <returns></returns>
    public static Vector2 Canvas2World(Vector2 localPosition)
    {
        return root.TransformPoint(localPosition);
    }
#endregion
}
