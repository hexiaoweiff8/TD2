using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// 地图基类
/// </summary>
public class MapBase
{

    // ------------------------------公共属性----------------------------------

    /// <summary>
    /// 地图宽度
    /// </summary>
    public int MapWidth { get { return mapWidth; } }

    /// <summary>
    /// 地图高度
    /// </summary>
    public int MapHeight { get { return mapHeight; } }

    /// <summary>
    /// 地图单位宽度
    /// </summary>
    public int UnitWidth { get { return unitWidth; } }

    /// <summary>
    /// 地图中心位置
    /// </summary>
    public Vector3 MapCenter { get; private set; }


    // ------------------------------私有属性----------------------------------

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


    /// <summary>
    /// 地图高度
    /// </summary> 
    private int mapHeight;

    /// <summary>
    /// 地图宽度
    /// </summary>
    private int mapWidth;

    /// <summary>
    /// 地图单位宽度
    /// </summary>
    private int unitWidth;

    /// <summary>
    /// 地图线绘制颜色
    /// </summary>
    private Color lineColor;



    // ------------------------------公共方法-----------------------------------

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="mapData">地图数据</param>
    /// <param name="unitWidth">单位宽度</param>
    /// <param name="newCenter">地图中心</param>
    public MapBase([NotNull]MapCellBase[,] mapData, Vector3 newCenter, int unitWidth)
    {
        mapCellArray = mapData;
        var newMapWidth = mapCellArray.GetLength(1);
        var newMapHeight = mapCellArray.GetLength(0);
        ResetMapPos(newCenter, newMapWidth, newMapHeight, unitWidth);
        lineColor = Color.red;
    }

    /// <summary>
    /// 绘制格子
    /// </summary>
    public void DrawLine()
    {
        if (mapCellArray != null)
        {
            // 在底板上画出格子
            // 画四边
            Debug.DrawLine(leftup, rightup, lineColor);
            Debug.DrawLine(leftup, leftdown, lineColor);
            Debug.DrawLine(rightdown, rightup, lineColor);
            Debug.DrawLine(rightdown, leftdown, lineColor);

            // 绘制格子
            for (var i = 1; i <= mapWidth; i++)
            {
                Debug.DrawLine(leftup + new Vector2(i * unitWidth, 0), leftdown + new Vector2(i * unitWidth, 0), lineColor);
            }
            for (var i = 1; i <= mapHeight; i++)
            {
                Debug.DrawLine(leftdown + new Vector2(0, i * unitWidth), rightdown + new Vector2(0, i * unitWidth), lineColor);
            }
        }
    }


    /// <summary>
    /// 绘制地图
    /// </summary>
    public void DrawMap()
    {
        MapCellBase item = null;
        var halfCellWidth = unitWidth * 0.5f;
        // 遍历地图
        for (var i = 0; i < mapHeight; i++)
        {
            for (var j = 0; j < mapWidth; j++)
            {
                item = mapCellArray[i, j];
                item.GameObj.transform.position = new Vector3(leftdown.x + j * unitWidth + halfCellWidth, leftdown.y + i * unitWidth + halfCellWidth);
            }
        }
        // 判断变更
        // 绘制变更
        // 否则跳过
    }


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
        mapWidth = width;
        mapHeight = height;
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
        if (posX + 1 > mapWidth || posX < 0)
        {
            Debug.LogError("地图X越界:" + posX);
            return;
        }
        if (posY + 1 > mapWidth || posY < 0)
        {
            Debug.LogError("地图Y越界:" + posY);
            return;
        }

        // 重建地图
        if (mapCellArray == null)
        {
            ReBuildMap(mapWidth, mapHeight);
        }

        if (mapCellArray != null)
        {
            mapCellArray[posY, posX] = mapCell;
        }

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
        return new Rect(posX, posY, unitWidth, unitWidth);
    }

    // ------------------------------私有方法-----------------------------------

    /// <summary>
    /// 重置地图位置点
    /// </summary>
    public void ResetMapPos(Vector3 newCenter, int newMapWidth, int newMapHeight, int unitWidth)
    {

        // 验证x与y是否越界
        if (newMapWidth < 0)
        {
            Debug.LogError("地图宽度非法:" + newMapWidth);
            return;
        }
        if (newMapHeight < 0)
        {
            Debug.LogError("地图高度非法:" + newMapHeight);
            return;
        }

        // 重置中心位置与宽高
        MapCenter = newCenter;
        mapWidth = newMapWidth;
        mapHeight = newMapHeight;

        this.unitWidth = unitWidth;
        // 地图半宽高
        var halfWidht = mapWidth * unitWidth * 0.5f;
        var halfHeight = mapHeight * unitWidth * 0.5f;
        // 取矩形四角点
        leftup = new Vector2(MapCenter.x - halfWidht, MapCenter.y + halfHeight);
        rightup = new Vector2(MapCenter.x + halfWidht, MapCenter.y + halfHeight);
        leftdown = new Vector2(MapCenter.x - halfWidht, MapCenter.y - halfHeight);
        rightdown = new Vector2(MapCenter.x + halfWidht, MapCenter.y - halfHeight);
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
    public UnitType MapCellType { get; set; }

    /// <summary>
    /// 地图Obj单位
    /// </summary>
    public GameObject GameObj { get; set; }

    /// <summary>
    /// 位置X
    /// </summary>
    public int X { get; set; }

    /// <summary>
    /// 位置Y
    /// </summary>
    public int Y { get; set; }

    /// <summary>
    /// 历史Rect
    /// </summary>
    private Rect historyRect;


    public Rect GetRect()
    {
        
    }


    /// <summary>
    /// 自增唯一ID
    /// </summary>
    private static long addtionId = 1024;


    /// <summary>
    /// 基础初始化
    /// </summary>
    protected MapCellBase(GameObject obj)
    {
        // 当前新类的ID并自增
        MapCellId = addtionId++;
        // 初始化模型
        this.GameObj = obj;
    }
}


/// <summary>
/// 地图单元
/// </summary>
public class MapCell : MapCellBase
{

    /// <summary>
    /// 初始化地图单元
    /// </summary>
    /// <param name="obj">游戏物体</param>
    /// <param name="cellType">地图单元类型</param>
    public MapCell(GameObject obj)
        : base(obj)
    {
        MapCellType = UnitType.MapCell;
    }
}




/// <summary>
/// 障碍物
/// </summary>
public class Obstacle : MapCellBase
{
    /// <summary>
    /// 初始化障碍物
    /// </summary>
    /// <param name="obj">游戏物体</param>
    public Obstacle(GameObject obj)
        : base(obj)
    {
        MapCellType = UnitType.Obstacle;
    }
}



/// <summary>
/// FightUnit
/// </summary>
public class FightUnit : MapCellBase
{
    /// <summary>
    /// 初始化战斗单位
    /// </summary>
    /// <param name="obj">游戏物体</param>
    public FightUnit(GameObject obj)
        : base(obj)
    {
        MapCellType = UnitType.FightUnit;
    }
}



/// <summary>
/// NPC
/// </summary>
public class NPC : MapCellBase
{
    /// <summary>
    /// 初始化NPC
    /// </summary>
    /// <param name="obj">游戏物体</param>
    public NPC(GameObject obj)
        : base(obj)
    {
        MapCellType = UnitType.NPC;
    }
}