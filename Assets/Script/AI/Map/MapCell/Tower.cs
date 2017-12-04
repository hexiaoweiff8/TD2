using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// 初始化
    /// 该cell
    /// </summary>
    /// <param name="obj">游戏对象</param>
    /// <param name="dataId">cell的DataId</param>
    /// <param name="drawLayer">所在层</param>
    public Tower(GameObject obj, int dataId, int drawLayer) : base(obj, dataId, drawLayer)
    {

    }


    /// <summary>
    /// 绘制塔对象Cell
    /// </summary>
    public void DrawTowerCell()
    {
        // 遍历塔分布数据
        // 将对应cell创建到对应位置
        // 如果绘制过了则判断变化, 否则不做操作

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
