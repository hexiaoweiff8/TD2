using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Util;

/// <summary>
/// 出兵点单位
/// </summary>
public class OutMonsterPoint : MapCellBase
{

    /// <summary>
    /// 刷怪列表
    /// </summary>
    private Queue<MonsterData> monsterQueue = new Queue<MonsterData>();


    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="obj">对象</param>
    /// <param name="dataId">数据ID</param>
    /// <param name="drawLayer">绘制层级</param>
    public OutMonsterPoint(GameObject obj, int dataId, int drawLayer) : base(obj, dataId, drawLayer)
    {
        // 读取数据
        // 生成创建事件链
        // 多个事件链
        // 数据格式 : 类型Id, 数量, Id,
    }

    /// <summary>
    /// 开始创建单位
    /// </summary>
    public void Begin()
    {
        // TODO 时间间隔创建
        // 创建单位有大量DrawCall上升问题, 待研究解决
        new Timer(5, true).OnCompleteCallback(CreateUnit).Start();
    }

    /// <summary>
    ///  创建单位
    /// </summary>
    private void CreateUnit()
    {
        // 从列表中将单位弹出
        //if (monsterQueue.Count > 0)
        //{
        //var monsterData = monsterQueue.Dequeue();

        var monsterData = new MonsterData()
        {
            MonsterId = 2
        };

        // 获取目标位置
        var targetMapCellList = FightManager.Single.MapBase.GetMapCellList(MapManager.MapNpcLayer,
            MapManager.InMonsterPointId);
        if (targetMapCellList != null && targetMapCellList.Count > 0)
        {
            // 创建怪单位
            var displayOwner = FightManager.Single.LoadMonster(monsterData.MonsterId, X, Y, targetMapCellList[0].X,
                targetMapCellList[0].Y);
            // 开始移动
            displayOwner.ClusterData.StartMove();
            // 设置到达目标事件
            displayOwner.MapCell.GameObj.transform.position = GameObj.transform.position;
            // 
        }
        else
        {
            Debug.LogError("没有目标位置");
        }

        //}
    }
}


/// <summary>
/// 刷怪单位信息
/// </summary>
public class MonsterData
{
    // 波次
    public int TurnId = 0;
    // 类型
    public int MonsterId = 0;
    // 数量
    public int Count = 0;
    // 与下一波的时间间隔
    public float Interval = 0f;
}