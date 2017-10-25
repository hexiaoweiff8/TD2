using System;
using System.Collections;
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

    ///// <summary>
    ///// 地图底板层
    ///// </summary>
    //private MapCellBase[,] mapCellArray;

    /// <summary>
    /// 地图层字典
    /// </summary>
    private Dictionary<int, MapCellBase[,]> mapCellArrayDic = new Dictionary<int, MapCellBase[,]>();

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
    /// <param name="mapWidth">地图宽度</param>
    /// <param name="mapHeight">地图高度</param>
    /// <param name="unitWidth">单位宽度</param>
    /// <param name="newCenter">地图中心</param>
    public MapBase(int mapWidth, int mapHeight, Vector3 newCenter, int unitWidth)
    {
        ResetMapPos(newCenter, mapWidth, mapHeight, unitWidth);
        lineColor = Color.red;
    }
    
    /// <summary>
    /// 添加层数据
    /// </summary>
    /// <param name="mapCellArray">地图层数据</param>
    /// <param name="layer">数据所在层</param>
    public void AddMapCellArray([NotNull]MapCellBase[,] mapCellArray, int layer)
    {
        if (mapCellArrayDic.ContainsKey(layer))
        {
            throw new Exception("该层已存在:" + layer);
        }
        mapCellArrayDic.Add(layer, mapCellArray);
    }

    /// <summary>
    /// 绘制格子
    /// </summary>
    public void DrawLine()
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
            Debug.DrawLine(leftup + new Vector2(i*unitWidth, 0), leftdown + new Vector2(i*unitWidth, 0), lineColor);
        }
        for (var i = 1; i <= mapHeight; i++)
        {
            Debug.DrawLine(leftdown + new Vector2(0, i*unitWidth), rightdown + new Vector2(0, i*unitWidth), lineColor);
        }

    }


    /// <summary>
    /// 绘制地图
    /// </summary>
    public void DrawMap()
    {
        MapCellBase item = null;
        MapCellBase[,] itemArray = null;
        // 遍历地图
        foreach (var kv in mapCellArrayDic)
        {
            itemArray = kv.Value;
            for (var i = 0; i < mapHeight; i++)
            {
                for (var j = 0; j < mapWidth; j++)
                {
                    itemArray[i, j].Draw(leftdown, unitWidth);
                }
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
        mapCellArrayDic.Clear();
    }


    ///// <summary>
    ///// 推入地图单元
    ///// </summary>
    ///// <param name="mapCell">地图单元类</param>
    ///// <param name="posX">所在位置X</param>
    ///// <param name="posY">所在位置Y</param>
    //public void PushMapCell(MapCellBase mapCell, int posX, int posY)
    //{
    //    if (mapCell == null)
    //    {
    //        Debug.LogError("地图单元为空");
    //        return;
    //    }
    //    // 验证x与y是否越界
    //    if (posX + 1 > mapWidth || posX < 0)
    //    {
    //        Debug.LogError("地图X越界:" + posX);
    //        return;
    //    }
    //    if (posY + 1 > mapWidth || posY < 0)
    //    {
    //        Debug.LogError("地图Y越界:" + posY);
    //        return;
    //    }

    //    if (mapCellArray != null)
    //    {
    //        mapCellArray[posY, posX] = mapCell;
    //    }

    //}


    /// <summary>
    /// 获取地图Array
    /// </summary>
    /// <returns>地图数据</returns>
    public MapCellBase[,] GetMapCellArray(int layer)
    {
        if (!mapCellArrayDic.ContainsKey(layer))
        {
            throw new Exception("不存在地图层:" + layer);
        }
        return mapCellArrayDic[layer];
    }

    /// <summary>
    /// 清理数据
    /// </summary>
    public void Clear()
    {
        foreach (var kv in mapCellArrayDic)
        {
            var itemArray = kv.Value;
            var height = itemArray.GetLength(0);
            var width = itemArray.GetLength(1);
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    UnitFictory.Single.DestoryMapCell(itemArray[i, j]);
                }
            }
        }

        mapCellArrayDic.Clear();
    }


    ///// <summary>
    ///// 获取对应位置的Rect
    ///// </summary>
    ///// <param name="posX">位置X</param>
    ///// <param name="posY">位置Y</param>
    ///// <returns></returns>
    //public Rect GetItemRect(int posX, int posY)
    //{
    //    return new Rect(posX, posY, unitWidth, unitWidth);
    //}

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

    /// <summary>
    /// 遍历每个地图
    /// </summary>
    public void Foreach(Action<int, MapCellBase[,]>  action)
    {
        foreach (var kv in mapCellArrayDic)
        {
            action(kv.Key, kv.Value);
        }
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
    /// 自增唯一ID
    /// </summary>
    private static long addtionId = 1024;

    /// <summary>
    /// 绘制层级
    /// </summary>
    private int drawLayer = -1;

    /// <summary>
    /// 历史Rect
    /// </summary>
    private Rect historyRect;

    /// <summary>
    /// 历史位置X
    /// </summary>
    private int historyX = -1;

    /// <summary>
    /// 历史位置Y
    /// </summary>
    private int historyY = -1;

    /// <summary>
    /// 历史位置X
    /// </summary>
    private int historyXForDraw = -1;

    /// <summary>
    /// 历史位置Y
    /// </summary>
    private int historyYForDraw = -1;




    /// <summary>
    /// 基础初始化
    /// </summary>
    protected MapCellBase(GameObject obj, int drawLayer)
    {
        // 当前新类的ID并自增
        MapCellId = addtionId++;
        // 初始化模型
        this.GameObj = obj;
        this.drawLayer = drawLayer;
    }

    /// <summary>
    /// 绘制方法
    /// </summary>
    public void Draw(Vector3 leftdown, int unitWidth)
    {
        // 判断是否有变动

        if (X != historyXForDraw || Y != historyYForDraw)
        {
            GameObj.transform.position = new Vector3(leftdown.x + X*unitWidth + unitWidth*0.5f,
                leftdown.y + Y * unitWidth + unitWidth * 0.5f);
            historyXForDraw = X;
            historyYForDraw = Y;
        }
    }

    /// <summary>
    /// 显示物体
    /// </summary>
    public void Show()
    {
        GameObj.SetActive(true);
        // 设置显示层级

    }

    /// <summary>
    /// 隐藏物体
    /// </summary>
    public void Hide()
    {
        GameObj.SetActive(false);
    }

    /// <summary>
    /// 获取该位置的Rect
    /// </summary>
    /// <returns>该位置的Rect</returns>
    public Rect GetRect()
    {
        // 如果位置有变更则更新Rect
        if (X != historyX || Y != historyY)
        {
            var unitWidth = MapDrawer.Single.UnitWidth;
            historyX = X;
            historyY = Y;
            historyRect = new Rect(X * unitWidth, Y * unitWidth, unitWidth, unitWidth);
        }
        return historyRect;
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
    /// <param name="drawLayer">绘制层级</param>
    public MapCell(GameObject obj, int drawLayer)
        : base(obj, drawLayer)
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
    /// <param name="drawLayer">绘制层级</param>
    public Obstacle(GameObject obj, int drawLayer)
        : base(obj, drawLayer)
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
    /// <param name="drawLayer">绘制层级</param>
    public FightUnit(GameObject obj, int drawLayer)
        : base(obj, drawLayer)
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
    /// <param name="drawLayer">绘制层级</param>
    public NPC(GameObject obj, int drawLayer)
        : base(obj, drawLayer)
    {
        MapCellType = UnitType.NPC;
    }
}