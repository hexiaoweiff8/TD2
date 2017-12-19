﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 追击状态
/// </summary>
public class PursueState : FSMState
{
    /// <summary>
    /// 当前单位的行动类
    /// </summary>
    private ClusterData clusterData = null;

    /// <summary>
    /// 目标位置
    /// </summary>
    //private ClusterData targetClusterData = null;

    /// <summary>
    /// 当前目标点
    /// </summary>
    private Vector3 targetPos;

    /// <summary>
    /// 初始化
    /// </summary>
    public override void Init()
    {
        // 状态切换
        StateID = FSMStateID.Pursue;
    }

    ///// <summary>
    ///// 进入改状态时执行
    ///// </summary>
    ///// <param name="fsm"></param>
    //public override void DoBeforeEntering(FSMSystem fsm)
    //{
    //    // 加载当前数据
    //    //Debug.Log("追击状态:" + fsm.Display.GameObj.name);
    //    clusterData = fsm.Display.ClusterData as ClusterData;
    //    targetPos = fsm.EnemyTarget.ClusterData.MapCellObj.transform.position;

    //    // 切换动作
    //    //var myself = fsm.Display.RanderControl;
    //    //myself.ModelRander.SetClip("run".GetHashCode());

    //    ReFindPath(fsm);
    //    // 单位转向目标
    //    if (clusterData != null && fsm.EnemyTarget != null && fsm.EnemyTarget.ClusterData != null)
    //    {
    //        clusterData.RotateToWithoutYAxis(fsm.EnemyTarget.ClusterData.MapCellObj.transform.position);
    //    }
    //}

    ///// <summary>
    ///// 每帧检测
    ///// </summary>
    ///// <param name="fsm"></param>
    //public override void Action(FSMSystem fsm)
    //{

    //    // 继续查找最近单位, 如果有更近的选择更近的
    //    CheckChangeTarget(fsm);
    //    // 检测目标是否在追击范围内
    //    if (TargetIsInScope(fsm))
    //    {
    //        // 检测目标是否与当前目标点差距太大, 如果没有需用寻路, 如果有则重新寻路
    //        if (IsNeedReFindPath(fsm))
    //        {
    //            // 重巡路径
    //            ReFindPath(fsm);
    //        }
    //    }
    //    else
    //    {
    //        // 切换行进状态
    //        fsm.IsZhuiJi = false;
    //        fsm.TargetIsLoseEfficacy = true;
    //    }
    //}

    ///// <summary>
    ///// 改状态消失时执行
    ///// </summary>
    ///// <param name="fsm"></param>
    //public override void DoBeforeLeaving(FSMSystem fsm)
    //{
    //    fsm.IsZhuiJi = false;
    //    // 清空当前数据
    //    //Debug.Log("追击结束:" + fsm.Display.GameObj.name);
    //}


    ///// <summary>
    ///// 是否需要重新寻路
    ///// </summary>
    ///// <param name="fsm"></param>
    ///// <returns></returns>
    //private bool IsNeedReFindPath(FSMSystem fsm)
    //{
    //    var targetClusterData = fsm.EnemyTarget.ClusterData;
    //    // 判断目标点与当前目标的距离
    //    var distance = (Utils.WithOutZ(targetPos) - Utils.WithOutZ(targetClusterData.MapCellObj.transform.position)).magnitude;
    //    var minDistance = clusterData.Diameter * ClusterManager.Single.UnitWidth * 0.5f +
    //                      targetClusterData.Diameter * ClusterManager.Single.UnitWidth * 0.5f;

    //    return distance > minDistance;
    //}

    ///// <summary>
    ///// 重巡路径
    ///// </summary>
    ///// <param name="fsm"></param>
    //private void ReFindPath(FSMSystem fsm)
    //{
    //    //Debug.Log("");
    //    // 重新寻路设置目标点
    //    // 当前地图数据
    //    int[][] mapData = null;
    //    // 当前地图数据
    //    switch (clusterData.AllData.MemberData.GeneralType)
    //    {
    //        case Utils.GeneralTypeAir:
    //            mapData = LoadMap.Single.GetAirMapData();
    //            break;
    //        case Utils.GeneralTypeSurface:
    //            mapData = LoadMap.Single.GetSurfaceMapData();
    //            break;

    //    }
    //    // 当前单位位置映射
    //    var startPos = Utils.PositionToNum(LoadMap.Single.MapPlane.transform.position, clusterData.MapCellObj.transform.position, ClusterManager.Single.UnitWidth, ClusterManager.Single.MapWidth, ClusterManager.Single.MapHeight);
    //    // 当前目标位置映射
    //    var endPos = Utils.PositionToNum(LoadMap.Single.MapPlane.transform.position, fsm.EnemyTarget.ClusterData.MapCellObj.transform.position, ClusterManager.Single.UnitWidth, ClusterManager.Single.MapWidth, ClusterManager.Single.MapHeight);
    //    // 生成路径
    //    var path = AStarPathFinding.SearchRoad(mapData, startPos[0], startPos[1], endPos[0], endPos[1], (int)clusterData.Diameter, (int)clusterData.Diameter + 1);
    //    if (path != null && path.Count > 0)
    //    {
    //        // 清空当前目标点
    //        clusterData.ClearTarget();
    //        targetPos = fsm.EnemyTarget.ClusterData.MapCellObj.transform.position;
    //        clusterData.PushTargetList(Utils.NumToPostionByList(LoadMap.Single.MapPlane.transform.position, path,
    //            ClusterManager.Single.UnitWidth, ClusterManager.Single.MapWidth, ClusterManager.Single.MapHeight));
    //    }
    //    else
    //    {
    //        Debug.Log("zhuiji 目标点不可达:start:" + startPos[0] + "," + startPos[1] + " end:" + endPos[0] + "," + endPos[1]);
    //    }

    //}

    /// <summary>
    /// 是否超出追击范围
    /// </summary>
    /// <returns></returns>
    private bool TargetIsInScope(FSMSystem fsm)
    {
        var result = false;
        //var targetClusterData = fsm.EnemyTarget.ClusterData;
        //if (targetClusterData != null)
        //{
        //    var distance = (Utils.WithOutZ(targetClusterData.MapCellObj.transform.position) -
        //                    Utils.WithOutZ(clusterData.MapCellObj.transform.position)).magnitude;
        //    result = distance
        //             - targetClusterData.Diameter * ClusterManager.Single.UnitWidth * 0.5f
        //             - clusterData.Diameter * ClusterManager.Single.UnitWidth * 0.5f
        //             < clusterData.AllData.MemberData.SightRange;
        //    // 判断目标是否存活, 如果死亡切行进
        //    if (targetClusterData.AllData.MemberData.CurrentHP <= 0)
        //    {
        //        result = false;
        //    }
        //}
        return result;
    }

    /// <summary>
    /// 检测变更目标
    /// </summary>
    private void CheckChangeTarget(FSMSystem fsm)
    {
        var list = ClusterManager.Single.CheckRange(new Vector2(clusterData.X, clusterData.Y), clusterData.AllData.MemberData.SightRange, clusterData.AllData.MemberData.Camp, true);
        if (list != null && list.Count > 0)
        {
            var closeObj = list[0];
            if (closeObj == null) { }
            var closeDistance = float.MaxValue;
            foreach (var item in list)
            {
                if (!(item is ClusterData))
                {
                    continue;
                }
                var newDistance = GetDistance(clusterData, item);
                if (closeDistance > newDistance)
                {
                    closeObj = item;
                    closeDistance = newDistance;
                }
            }
            // 有变更
            //if (fsm.EnemyTarget == null ||
            //    fsm.EnemyTarget.ClusterData == null ||
            //    !closeObj.AllData.MemberData.ObjID.Equals(fsm.EnemyTarget.ClusterData.AllData.MemberData.ObjID))
            //{
            //    //Debug.Log("变更目标.");
            //    fsm.EnemyTarget = FightUnitManager.Single.GetElementById(closeObj.AllData.MemberData.ObjID);
            //}
        }

    }

    /// <summary>
    /// 计算两单位距离
    /// </summary>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    /// <returns></returns>
    private float GetDistance(PositionObject obj1, PositionObject obj2)
    {
        var result = float.MaxValue;
        if (obj1 != null && obj2 != null)
        {
            result = (new Vector2(obj1.X, obj1.Y) - new Vector2(obj2.X, obj2.Y)).magnitude;
        }

        return result;
    }

}
