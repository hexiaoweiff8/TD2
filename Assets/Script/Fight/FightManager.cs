using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 战斗管理器
/// </summary>
public class FightManager : SingleItem<FightManager>
{
    /// <summary>
    /// 开启章节
    /// </summary>
    /// <param name="centerPos">地图中心位置</param>
    /// <param name="chapterId">地图章节ID</param>
    /// <param name="drawRect">绘制范围</param>
    /// <param name="unitWidth">绘制单元大小</param>
    /// <param name="drawType">绘制类型</param>
    public void StartChapter(int chapterId, Vector3 centerPos, Rect drawRect, int unitWidth, int drawType)
    {

        DataPacker.Single.Clear();
        // 加载数据
        DataPacker.Single.Load();
        MapManager.Single.Clear();

        // 加载地图
        var mapBase = MapManager.Single.GetMapBase(chapterId, centerPos, unitWidth, drawType);

        // 初始化地图绘制
        MapDrawer.Single.Init(mapBase, centerPos, drawRect, drawType);

        MapDrawer.Single.Begin();

        // 获得地图宽高
        var mapWidth = mapBase.MapWidth;
        var mapHeight = mapBase.MapHeight;


        ClusterManager.Single.Init(centerPos.x, centerPos.y, mapWidth, mapHeight, unitWidth);


        // 获得地图障碍列表
        LoadObstacle(mapBase);
        // TODO 获得地图NPC列表
        // TODO 获得地图敌人列表
        // TODO 获取玩家数据
        // 启动战斗单元
        // TODO 创建玩家层
        LoadPlayer();
    }

    /// <summary>
    /// 加载障碍曾
    /// </summary>
    public void LoadObstacle(MapBase mapBase)
    {
        // 获取障碍层
        var obLayer = mapBase.GetMapCellArray(MapManager.MapObstacleLayer);
        if (obLayer == null)
        {
            Debug.LogError("障碍层不存在!");
            return;
        }
        // 遍历数据创建障碍
        var height = obLayer.GetLength(0);
        var width = obLayer.GetLength(1);
        for (var i = 0; i < height; i++)
        {
            for (var j = 0; j < width; j++)
            {
                var cell = obLayer[i, j];
                // 判断该位置是否有障碍物
                if (cell.DataId >= 0)
                {
                    // 从数据中获取该障碍物的信息
                    var dataRow = DataPacker.Single.GetDataItem(UnitFictory.ObstacleTableName).GetRowById(cell.DataId.ToString());
                    var diameter = dataRow.GetInt("Diameter");
                    var ob = new FixtureData(new AllData()
                    {
                        MemberData = new MemberData()
                        {
                            SpaceSet = diameter
                        },
                        UnitWidth = MapDrawer.Single.UnitWidth,
                        GraphicType = GraphicType.Rect
                    }, cell);

                    ClusterManager.Single.Add(ob);
                }

            }
        }


    }

    /// <summary>
    /// 创建玩家
    /// </summary>
    public void LoadPlayer()
    {

        // TODO 创建测试单位
        var objId = new ObjectID(ObjectID.ObjectType.MySoldier);
        var school = new ClusterData(new AllData()
        {
            MemberData = new MemberData()
            {
                AttackRange = 20,
                ObjID = objId,
                MoveSpeed = 60,
                GeneralType = 1,
                Camp = 1,
                Attack1 = 10
            },
            UnitWidth = MapDrawer.Single.UnitWidth
        }, UnitFictory.Single.GetUnit(UnitType.FightUnit, 1001));


        school.MaxSpeed = 10;
        school.RotateSpeed = 10;
        school.ItemObj.transform.localPosition = new Vector3(-700, -100, 0);
        school.ItemObj.name = "item" + 1;
        school.TargetPos = new Vector3(-700, -100, 0);
        school.Diameter = 1;
        //school.PushTargetList(Utils.NumToPostionByList(LoadMap.transform.position, cloneList, UnitWidth, MapWidth, MapHeight));
        // 目标选择权重
        var fightData = new SelectWeightData();
        // 选择目标数据
        fightData.AirWeight = 100;
        fightData.BuildWeight = 100;
        fightData.SurfaceWeight = 100;

        fightData.HumanWeight = 10;
        fightData.OrcWeight = 10;
        fightData.OmnicWeight = 10;

        fightData.HideWeight = -1;
        fightData.TauntWeight = 1000;

        fightData.HealthMaxWeight = 0;
        fightData.HealthMinWeight = 10;
        fightData.DistanceMaxWeight = 0;
        fightData.DistanceMinWeight = 10;
        school.AllData.SelectWeightData = fightData;
        school.AllData.MemberData.CurrentHP = 99;
        school.AllData.MemberData.TotalHp = 100;

        ClusterManager.Single.Add(school);
        school.StartMove();
    }
}