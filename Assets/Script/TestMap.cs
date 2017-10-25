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

    /// <summary>
    /// 相机移动速度
    /// </summary>
    public int CameraMoveSpeed = 1;

    /// <summary>
    /// 绘制类型
    /// </summary>
    public int DrawType = 1;



    /// <summary>
    /// 绘制位置
    /// </summary>
    private Vector3 drawPos;


	

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
        MapManager.Single.Clear();
        // 加载地图绘制
        MapManager.Single.BeginMap(1, new Vector3(), UnitWidht, DrawType);
        MapDrawer.Single.ChangeDrawRect(Utils.GetShowRect(MainCamera.transform.position, MapDrawer.Single.MapWidth, MapDrawer.Single.MapHeight, Screen.width + MapDrawer.Single.UnitWidth * 2, Screen.height + MapDrawer.Single.UnitWidth * 2, MapDrawer.Single.UnitWidth));
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
        if (!Input.anyKey)
        {
            return;
        }

        if (Input.GetKey(KeyCode.R))
        {
            // 创建地图
            Init();
        }
        if (Input.GetKey(KeyCode.PageUp))
        {
            MainCamera.fieldOfView += 0.1f;
        }

        if (Input.GetKey(KeyCode.PageDown))
        {
            MainCamera.fieldOfView -= 0.1f;
        }

        // 地图绘制
        if (MapDrawer.Single.IsStarted)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                MainCamera.transform.position = new Vector3(MainCamera.transform.position.x,
                    MainCamera.transform.position.y + CameraMoveSpeed, MainCamera.transform.position.z);
                MapDrawer.Single.ChangeDrawRect(Utils.GetShowRect(MainCamera.transform.position, MapDrawer.Single.MapWidth, MapDrawer.Single.MapHeight, Screen.width + MapDrawer.Single.UnitWidth * 2, Screen.height + MapDrawer.Single.UnitWidth * 2, MapDrawer.Single.UnitWidth));

                drawPos = MainCamera.transform.position;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                MainCamera.transform.position = new Vector3(MainCamera.transform.position.x,
                    MainCamera.transform.position.y - CameraMoveSpeed, MainCamera.transform.position.z);
                MapDrawer.Single.ChangeDrawRect(Utils.GetShowRect(MainCamera.transform.position, MapDrawer.Single.MapWidth, MapDrawer.Single.MapHeight, Screen.width + MapDrawer.Single.UnitWidth * 2, Screen.height + MapDrawer.Single.UnitWidth * 2, MapDrawer.Single.UnitWidth));

                drawPos = MainCamera.transform.position;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                MainCamera.transform.position = new Vector3(MainCamera.transform.position.x - CameraMoveSpeed,
                    MainCamera.transform.position.y, MainCamera.transform.position.z);
                MapDrawer.Single.ChangeDrawRect(Utils.GetShowRect(MainCamera.transform.position, MapDrawer.Single.MapWidth, MapDrawer.Single.MapHeight, Screen.width + MapDrawer.Single.UnitWidth * 2, Screen.height + MapDrawer.Single.UnitWidth * 2, MapDrawer.Single.UnitWidth));

                drawPos = MainCamera.transform.position;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                MainCamera.transform.position = new Vector3(MainCamera.transform.position.x + CameraMoveSpeed,
                    MainCamera.transform.position.y, MainCamera.transform.position.z);
                MapDrawer.Single.ChangeDrawRect(Utils.GetShowRect(MainCamera.transform.position, MapDrawer.Single.MapWidth, MapDrawer.Single.MapHeight, Screen.width + MapDrawer.Single.UnitWidth * 2, Screen.height + MapDrawer.Single.UnitWidth * 2, MapDrawer.Single.UnitWidth));

                drawPos = MainCamera.transform.position;
            }
        }
    }
}