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
    /// 地图单位宽度
    /// </summary>
    public int UnitWidht = 1;


	void Start ()
	{
        // 初始化
	    Init();
	}
	

	void Update () {
	    
        // 控制
	    Control();
	}


    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        DataPacker.Single.Clear();
        // 加载数据
        DataPacker.Single.Load();
        // 加载地图绘制
        MapManager.Single.BeginMap(1, 1, new Vector3(), UnitWidht);
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
            //MapBase.Single.ReBuildMap(100,100);
        }

        if (Input.GetKey(KeyCode.PageUp))
        {
            MainCamera.fieldOfView += 0.1f;
        }

        if (Input.GetKey(KeyCode.PageDown))
        {
            MainCamera.fieldOfView -= 0.1f;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            MainCamera.fieldOfView -= 0.1f;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            MainCamera.fieldOfView -= 0.1f;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            MainCamera.transform.position = new Vector3(MainCamera.transform.position.x, MainCamera.transform.position.y, MainCamera.transform.position.z);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            MainCamera.fieldOfView -= 0.1f;
        }

        if (Input.GetKey(KeyCode.R))
        {
            // 创建地图
            Init();
        }
    }
}