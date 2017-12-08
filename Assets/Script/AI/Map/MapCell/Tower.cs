using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


/// <summary>
/// 塔基类
/// </summary>
public class Tower : MapCellBase
{

    /// <summary>
    /// 塔UI对象
    /// </summary>
    private GameObject TowerObj = null;


    /// <summary>
    /// 塔分布数据数组
    /// </summary>
    private int[,] towerCellDataArray = new int[,]
    {
        {100,0,0},
        {0,0,0},
        {0,0,101},
    };

    /// <summary>
    /// 塔单元数组
    /// </summary>
    private MapCellBase[,] towerCellArray = null;

    /// <summary>
    /// 塔cell高度
    /// </summary>
    private int height = 0;

    /// <summary>
    /// 塔cell宽度
    /// </summary>
    private int wight = 0;

    /// <summary>
    /// 塔的相对位置左下角
    /// </summary>
    private Vector2 towerLeftDown;

    /// <summary>
    /// 塔的单位宽度
    /// </summary>
    private int towerUnitWidth = 1;

    /// <summary>
    /// 初始化
    /// 该cell
    /// </summary>
    /// <param name="obj">游戏对象</param>
    /// <param name="dataId">cell的DataId</param>
    /// <param name="drawLayer">所在层</param>
    public Tower(GameObject obj, int dataId, int drawLayer) : base(obj, dataId, drawLayer)
    {
        MapCellType = UnitType.Tower;
    }

    /// <summary>
    /// 设置塔数据
    /// </summary>
    /// <param name="towerData">他单元数据</param>
    public void SetTowerData([NotNull]int[,] towerData)
    {
        towerCellDataArray = towerData;
        height = towerData.GetLength(0);
        wight = towerData.GetLength(1);
        // 加载地图Cell数据
    }

    /// <summary>
    /// 绘制当前单位
    /// </summary>
    /// <param name="leftdown">地图左下点位置</param>
    /// <param name="unitWidth">地图单位宽度</param>
    public override void Draw(Vector3 leftdown, int unitWidth)
    {
        base.Draw(leftdown, unitWidth);
        // 绘制塔内单位
        DrawTowerCell();
    }


    /// <summary>
    /// 绘制塔对象Cell
    /// </summary>
    public void DrawTowerCell()
    {
        if (towerCellArray != null)
        {
            // 遍历塔分布数据
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < wight; j++)
                {
                    towerCellArray[i, j].Draw(towerLeftDown, towerUnitWidth);
                }
            }
            // 将对应cell创建到对应位置

            // 如果绘制过了则判断变化, 否则不做操作


        }
    }


    // 读取模板设置塔的地板类型
    // 定义不同的位置点数据ID
    // 0: 可放置位置
    // -1: 不可放置位置
    // 1:金
    // 2:木
    // 3:水
    // 4:火
    // 5:土
    // 100 输入位置
    // 101 输出位置


}
