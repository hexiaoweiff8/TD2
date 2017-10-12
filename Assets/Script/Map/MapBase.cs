using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 地图基类
/// </summary>
public class MapBase : MonoBehaviour
{
    /// <summary>
    /// 单例
    /// </summary>
    public static MapBase Single { get; private set; }

    // ------------------------------公共属性----------------------------------

    /// <summary>
    /// 地图位置标志物
    /// 该物体位于地图中心
    /// </summary>
    public GameObject MapCenter;

    /// <summary>
    /// 地图单位宽度
    /// </summary>
    public int MapCellWidth = 1;

    /// <summary>
    /// 是否显示格子
    /// </summary>
    public bool IsShow;


    /// <summary>
    /// 地图高度
    /// </summary>
    public int MapHeight;

    /// <summary>
    /// 地图宽度
    /// </summary>
    public int MapWidth;

    /// <summary>
    /// 地图单位宽度
    /// </summary>
    public int CellWidth { get; set; }

    /// <summary>
    /// 地图线绘制颜色
    /// </summary>
    public Color LineColor;



    // ------------------------------私有属性----------------------------------

    /// <summary>
    /// 地图中心点位置
    /// </summary>
    private Vector2 centerPos;

    /// <summary>
    /// 地图左上点
    /// </summary>
    private Vector2 leftup;

    /// <summary>
    /// 地图右上点
    /// </summary>
    private Vector2 rightup;

    /// <summary>
    /// 地图左下点
    /// </summary>
    private Vector2 leftdown;

    /// <summary>
    /// 地图右下点
    /// </summary>
    private Vector2 rightdown;

    /// <summary>
    /// 地图底板
    /// </summary>
    private MapCellBase[,] mapCellArray;


    // -------------------------------系统方法--------------------------------

    void Start()
    {
        Single = this;
        ResetMapPos();
        LineColor = Color.red;
    }


    /// <summary>
    /// update
    /// </summary>
    void Update()
    {
        if (mapCellArray != null)
        {
            // 不显示逻辑地图则返回
            if (!IsShow)
            {
                return;
            }
            // 在底板上画出格子
            // 画四边
            Debug.DrawLine(leftup, rightup, LineColor);
            Debug.DrawLine(leftup, leftdown, LineColor);
            Debug.DrawLine(rightdown, rightup, LineColor);
            Debug.DrawLine(rightdown, leftdown, LineColor);

            // 绘制格子
            for (var i = 1; i <= MapWidth; i++)
            {
                Debug.DrawLine(Utils.V2ToV3(leftup) + new Vector3(i * MapCellWidth, 0), Utils.V2ToV3(leftdown) + new Vector3(i * MapCellWidth, 0), LineColor);
            }
            for (var i = 1; i <= MapHeight; i++)
            {
                Debug.DrawLine(Utils.V2ToV3(leftdown) + new Vector3(0, i * MapCellWidth), Utils.V2ToV3(rightdown) + new Vector3(0, i * MapCellWidth), LineColor);
            }
        }
    }

    // ------------------------------公共方法-----------------------------------

    /// <summary>
    /// 重建地图数据
    /// </summary>
    /// <param name="width">地图宽度</param>
    /// <param name="height">地图高度</param>
    public void ReBuildMap(int width, int height)
    {
        if (width < 0 || height < 0)
        {
            throw new Exception("地图大小不合法:" + height + "," + width);
        }
        MapWidth = width;
        MapHeight = height;
        mapCellArray = new MapCellBase[height, width];
    }


    /// <summary>
    /// 推入地图单元
    /// </summary>
    /// <param name="mapCell">地图单元类</param>
    /// <param name="posX">所在位置X</param>
    /// <param name="posY">所在位置Y</param>
    public void PushMapCell(MapCellBase mapCell, int posX, int posY)
    {
        if (mapCell == null)
        {
            Debug.LogError("地图单元为空");
            return;
        }
        // 验证x与y是否越界
        if (posX + 1 > MapWidth || posX < 0)
        {
            Debug.LogError("地图X越界:" + posX);
            return;
        }
        if (posY + 1 > MapWidth || posY < 0)
        {
            Debug.LogError("地图Y越界:" + posY);
            return;
        }

        // 重建地图
        if (mapCellArray == null)
        {
            ReBuildMap(MapWidth, MapHeight);
        }

        mapCellArray[posY, posX] = mapCell;

    }


    /// <summary>
    /// 获取地图Array
    /// </summary>
    /// <returns>地图数据</returns>
    public MapCellBase[,] GetMapCellArray()
    {
        return mapCellArray;
    }


    /// <summary>
    /// 获取对应位置的Rect
    /// </summary>
    /// <param name="posX">位置X</param>
    /// <param name="posY">位置Y</param>
    /// <returns></returns>
    public Rect GetItemRect(int posX, int posY)
    {
        return new Rect(posX, posY, MapCellWidth, MapCellWidth);
    }

    // ------------------------------私有方法-----------------------------------

    /// <summary>
    /// 重置地图位置点
    /// </summary>
    private void ResetMapPos()
    {

        // 绘制格子
        // 重置地图位置点
        if (MapCenter != null)
        {
            centerPos = MapCenter.transform.position;
        }
        else
        {
            // 默认位置0,0,0
            centerPos = Vector2.zero;
        }

        var halfWidht = MapWidth*0.5f;
        var halfHeight = MapHeight*0.5f;
        // 取矩形四角点
        leftup = new Vector2(centerPos.x - halfWidht, centerPos.y + halfHeight);
        rightup = new Vector2(centerPos.x + halfWidht, centerPos.y + halfHeight);
        leftdown = new Vector2(centerPos.x - halfWidht, centerPos.y - halfHeight);
        rightdown = new Vector2(centerPos.x + halfWidht, centerPos.y - halfHeight);
    }



}


/// <summary>
/// 地图单元抽象类
/// </summary>
public abstract class MapCellBase
{

    /// <summary>
    /// 地图单元ID
    /// </summary>
    public long MapCellId { get; private set; }

    /// <summary>
    /// 地图单位类型
    /// </summary>
    public int MapCellType { get; set; }

    /// <summary>
    /// 自增唯一ID
    /// </summary>
    private static long addtionId = 1024;


    /// <summary>
    /// 基础初始化
    /// </summary>
    protected MapCellBase()
    {
        // 当前新类的ID并自增
        MapCellId = addtionId++;
    }
}


/// <summary>
/// 地图单元
/// </summary>
public class MapCell : MapCellBase
{
    public MapCell(int cellType) : base()
    {
        MapCellType = cellType;
    }
}