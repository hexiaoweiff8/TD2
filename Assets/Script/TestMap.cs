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
    /// 矩形绘制范围
    /// </summary>
    private RectGraphics drawRect = null;



	void Update () {
	    
        // 控制
	    Control();

	}


    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        var centerPos = new Vector3();
        drawRect = new RectGraphics(Vector2.zero, Screen.width, Screen.height, 0);
        // 开启章节
        FightManager.Single.StartChapter(1, centerPos, drawRect, UnitWidht, DrawType);

        // 创建player单位
    }

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
            MainCamera.orthographicSize += CameraMoveSpeed;
        }

        if (Input.GetKey(KeyCode.PageDown))
        {
            MainCamera.orthographicSize -= CameraMoveSpeed;
        }

        // 地图绘制
        if (MapDrawer.Single.IsStarted)
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    MainCamera.transform.position = new Vector3(MainCamera.transform.position.x,
                        MainCamera.transform.position.y + CameraMoveSpeed, MainCamera.transform.position.z);
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    MainCamera.transform.position = new Vector3(MainCamera.transform.position.x,
                        MainCamera.transform.position.y - CameraMoveSpeed, MainCamera.transform.position.z);
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    MainCamera.transform.position = new Vector3(MainCamera.transform.position.x - CameraMoveSpeed,
                        MainCamera.transform.position.y, MainCamera.transform.position.z);
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    MainCamera.transform.position = new Vector3(MainCamera.transform.position.x + CameraMoveSpeed,
                        MainCamera.transform.position.y, MainCamera.transform.position.z);
                }

                drawRect.Postion = MainCamera.transform.position;
                MapDrawer.Single.ChangeDrawGraphics(drawRect);
            }
           

        }
    }
}