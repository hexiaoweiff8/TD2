using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// 战斗管理器
/// </summary>
public class FightManager : SingleItem<FightManager>
{

    /// <summary>
    /// 战斗单位字典
    /// </summary>
    private Dictionary<uint, DisplayOwner> displayOwners = new Dictionary<uint, DisplayOwner>();

    /// <summary>
    /// 出兵点位置
    /// </summary>
    private List<Vector2> outMonsterPointList = new List<Vector2>();

    /// <summary>
    /// 地图基类
    /// </summary>
    public MapBase MapBase = null;



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
        MapBase = MapManager.Single.GetMapBase(chapterId, centerPos, unitWidth, drawType);

        // 初始化地图绘制
        MapDrawer.Single.Init(MapBase, centerPos, drawRect, drawType);

        MapDrawer.Single.Begin();

        // 获得地图宽高
        var mapWidth = MapBase.MapWidth;
        var mapHeight = MapBase.MapHeight;


        ClusterManager.Single.Init(centerPos.x, centerPos.y, mapWidth, mapHeight, unitWidth);


        // 获得地图障碍列表
        LoadObstacleLayer(MapBase);
        // ---- 初始化的时候只初始化显示范围内单位, 其他部分协程加载
        // TODO 获得地图NPC列表
        // TODO 获取玩家数据
        // 启动战斗单元
        // TODO 创建玩家层
        // TODO 路标
        // TODO 敌方数据
        // TODO 出兵位置产兵
        //LoadPlayer(MapBase);


        var outPointList = MapBase.GetMapCellList(MapManager.MapNpcLayer, MapManager.OutMosterPointId);
        MapBase.DrawMap();

        var outMonsterPoint = (outPointList[0] as OutMonsterPoint);
        outMonsterPoint.AddData(new List<MonsterData>()
        {
            new MonsterData()
            {
                MonsterId = 1001,
                Count = 3,
                Interval = 1
            },
            new MonsterData()
            {
                MonsterId = 1001,
                Count = 1,
                Interval = 1
            },
            new MonsterData()
            {
                MonsterId = 1001,
                Count = 1,
                Interval = 1
            },
            new MonsterData()
            {
                MonsterId = 1001,
                Count = 1,
                Interval = 1
            },
            new MonsterData()
            {
                MonsterId = 1001,
                Count = 1,
                Interval = 1
            },
            new MonsterData()
            {
                MonsterId = 2,
                Count = 1,
                Interval = 1
            },
            new MonsterData()
            {
                MonsterId = 2,
                Count = 1,
                Interval = 1
            },
            new MonsterData()
            {
                MonsterId = 2,
                Count = 1,
                Interval = 1
            },
            new MonsterData()
            {
                MonsterId = 2,
                Count = 1,
                Interval = 1
            },
            new MonsterData()
            {
                MonsterId = 2,
                Count = 1,
                Interval = 1
            },
            new MonsterData()
            {
                MonsterId = 2,
                Count = 1,
                Interval = 1
            },
            new MonsterData()
            {
                MonsterId = 2,
                Count = 1,
                Interval = 1
            },
        });
        outMonsterPoint.Start();

    }

    /// <summary>
    /// 加载障碍层
    /// </summary>
    public void LoadObstacleLayer([NotNull]MapBase mapBase)
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
                if (cell != null && cell.DataId > 0)
                {
                    if (cell.DataId == MapManager.OutMosterPointId)
                    {
                        // 出兵点位置
                    }
                    else if (cell.DataId == MapManager.InMonsterPointId)
                    {
                        // 入兵点位置
                    }
                    else
                    {
                        // 从数据中获取该障碍物的信息
                        var dataRow = DataPacker.Single.GetDataItem(UnitFictory.ObstacleTableName).GetRowById(cell.DataId.ToString());
                        var diameter = dataRow.GetInt("Diameter");
                        var ob = new FixtureData(new AllData
                        {
                            MemberData = new MemberData
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
    }

    /// <summary>
    /// 加载怪
    /// </summary>
    /// <param name="id">怪Id</param>
    /// <param name="startX">怪起始位置x</param>
    /// <param name="startY">怪起始位置y</param>
    /// <param name="targetX">怪目标位置x</param>
    /// <param name="targetY">怪目标位置y</param>
    public DisplayOwner LoadMonster(int id, int startX, int startY, int targetX, int targetY)
    {
        DisplayOwner result = null;
        // 获取怪数据
        // 创建怪模型
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
        }, UnitFictory.Single.CreateUnit(UnitType.FightUnit, id));

        // 地图数据
        // 寻路 获取路径
        // 获取起始点位置
        // 获取目标点位置
        // 单位宽度
        var mapArray = MapBase.GetMapArray(MapManager.MapObstacleLayer);
        var posList = AStarPathFinding.SearchRoad(mapArray, startX, startY, targetX, targetY, 1, 1);
        school.MaxSpeed = 100;
        school.RotateSpeed = 10;
        school.MapCellObj.transform.localPosition = new Vector3(-700, -100, 0);
        school.MapCellObj.name = "item" + 1;
        //school.TargetPos = new Vector3(-700, -100, 0);
        school.PushTargetList(Utils.NumToPostionByList(MapBase.MapCenter, posList, MapBase.UnitWidth, MapBase.MapWidth, MapBase.MapHeight));
        school.Diameter = 1;

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
        //school.StartMove();

        result = new DisplayOwner(school.MapCell, school);

        // 缓存数据
        displayOwners.Add(result.Id, result);
        return result;
    }


    public void Destory([NotNull]DisplayOwner displayOwner)
    {
        // 删除引用
        displayOwners.Remove(displayOwner.Id);
        // 销毁对象
        displayOwner.Destroy();
    }



    /// <summary>
    /// TODO test创建玩家
    /// </summary>
    public void LoadPlayer(MapBase mapBase)
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
        }, UnitFictory.Single.CreateUnit(UnitType.FightUnit, 1001));

        // 地图数据
        // 寻路 获取路径
        // 获取起始点位置
        // 获取目标点位置
        // 单位宽度
        var mapArray = mapBase.GetMapArray(MapManager.MapObstacleLayer);
        var posList = AStarPathFinding.SearchRoad(mapArray, 0, 0, 1, 8, 1, 1);
        school.MaxSpeed = 100;
        school.RotateSpeed = 10;
        school.MapCellObj.transform.localPosition = new Vector3(-700, -100, 0);
        school.MapCellObj.name = "item" + 1;
        //school.TargetPos = new Vector3(-700, -100, 0);
        school.PushTargetList(Utils.NumToPostionByList(mapBase.MapCenter, posList, mapBase.UnitWidth, mapBase.MapWidth, mapBase.MapHeight));
        school.Diameter = 1;
        
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