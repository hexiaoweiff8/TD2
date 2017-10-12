using UnityEngine;
using System.Collections;

/// <summary>
/// 测试地图
/// </summary>
public class TestMap : MonoBehaviour
{

    /// <summary>
    /// 游戏主相机
    /// </summary>
    public Camera MainCamera;

    /// <summary>
    /// 地图宽度
    /// </summary>
    public int MapWidth = 100;

    /// <summary>
    /// 地图高度
    /// </summary>
    public int MapHeight = 100;


	void Start () {
	}
	

	void Update () {

        // 绘制地图

	    
        // 控制
	    Control();
	}


    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        // 初始化
        // 创建地图
        for (var i = 0; i < MapHeight; i++)
        {
            for (var j = 0; j < MapWidth; j++)
            {
                MapBase.Single.PushMapCell(new MapCell(1), j, i);
            }
        }
    }

    ///// <summary>
    ///// 绘制地图
    ///// 如果该位置没有变更
    ///// </summary>
    //private void Draw()
    //{
    //    var mapArray = MapBase.Single.GetMapCellArray();
    //    for (var i = 0; i < MapHeight; i++)
    //    {
    //        for (var j = 0; j < MapWidth; j++)
    //        {
    //            var item = mapArray[i, j];
    //            if (item != null)
    //            {
    //                switch (item.MapCellType)
    //                {
    //                    case 1:

    //                        break;
    //                }
    //            }
    //        }
    //    }
    //}

    /// <summary>
    /// 控制方法
    /// </summary>
    private void Control()
    {

        if (Input.GetMouseButtonUp(1))
        {
            // 创建地图
            MapBase.Single.ReBuildMap(100,100);
        }

        if (Input.GetKey(KeyCode.PageUp))
        {
            MainCamera.orthographicSize += 0.1f;
        }

        if (Input.GetKey(KeyCode.PageDown))
        {
            MainCamera.orthographicSize -= 0.1f;
        }

        if (Input.GetKey(KeyCode.R))
        {
            // 创建地图
            Init();
        }
    }
}