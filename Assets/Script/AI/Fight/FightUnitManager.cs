using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


/// <summary>
/// 战斗单位工厂
/// </summary>
public class FightUnitManager : SingleItem<FightUnitManager>
{

    /// <summary>
    /// 寻路起始X key
    /// </summary>
    public const string FightItemStartX = "X";

    /// <summary>
    /// 寻路起始Y key
    /// </summary>
    public const string FightItemStartY = "Y";

    /// <summary>
    /// 寻路目标X key
    /// </summary>
    public const string FightItemTargetX = "targetX";
    
    /// <summary>
    /// 寻路目标Y key
    /// </summary>
    public const string FightItemTargetY = "targetY";


    /// <summary>
    /// 战斗单位字典
    /// </summary>
    private Dictionary<int, DisplayOwner> displayOwners = new Dictionary<int, DisplayOwner>();


    /// <summary>
    /// 加载单位
    /// </summary>
    /// <param name="type">单位类型</param>
    /// <param name="id">单位数据ID</param>
    /// <param name="dataItem">数据包装类</param>
    /// <returns></returns>
    public DisplayOwner LoadMember(UnitType type, int id, DataItem dataItem)
    {
        DisplayOwner result = null;
        switch (type)
        {
            case UnitType.FightUnit:
            {
                // TODO 临时测试数据
                // 获取怪数据
                // 创建怪模型
                var objId = new ObjectID(ObjectID.ObjectType.EnemySoldier);
                var mapCell = UnitFictory.Single.CreateUnit(type, id);
                var school = new ClusterData(new AllData()
                {
                    MemberData = new MemberData()
                    {
                        AttackRange = 20,
                        ObjID = objId,
                        MoveSpeed = 60,
                        GeneralType = 1,
                        Camp = 1,
                        Attack1 = 10,
                        BehaviorType = FSMFactory.MemberType,
                    },
                    UnitWidth = MapDrawer.Single.UnitWidth
                }, mapCell);

                var mapBase = FightManager.Single.MapBase;

                // 将单位放入地图数据中
                mapBase.AddMapCell(mapCell, MapManager.MapPlayerLayer);

                // 读取数据
                var startX = dataItem.GetInt(FightItemStartX);
                var startY = dataItem.GetInt(FightItemStartY);
                var targetX = dataItem.GetInt(FightItemTargetX);
                var targetY = dataItem.GetInt(FightItemTargetY);

                // 地图数据
                // 寻路 获取路径
                // 获取起始点位置
                // 获取目标点位置
                // 单位宽度
                school.MaxSpeed = 100;
                school.RotateSpeed = 10;
                school.Diameter = 1;
                school.MapCellObj.name = "item" + 1;
                school.MapCellObj.transform.localPosition = new Vector3(-700, -100, 0);

                var mapArray = mapBase.GetMapArray(MapManager.MapObstacleLayer);
                var posList = AStarPathFinding.SearchRoad(mapArray, startX, startY, targetX, targetY, 1, 1);
                school.PushTargetList(Utils.NumToPostionByList(mapBase.MapCenter, posList, mapBase.UnitWidth,
                    mapBase.MapWidth, mapBase.MapHeight));

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
                result = new DisplayOwner(school);
                // 缓存数据
                displayOwners.Add(result.Id, result);

                // 启动状态机
                FSMManager.Single.BeginRunFSM(result);
                // 加入状态机
            }
                break;
            case UnitType.MapCell:
            {

            }
                break;
            case UnitType.InPoint:
            {

            }
                break;
            case UnitType.OutPoint:
            {

            }
                break;
            case UnitType.NPC:
            {

            }
                break;
            case UnitType.Obstacle:
            {

            }
                break;
            case UnitType.Tower:
            {
                var objId = new ObjectID(ObjectID.ObjectType.MyTower);
                // 加载tower
                var tower = UnitFictory.Single.CreateUnit(UnitType.Tower, 1001) as Tower;

                tower.X = dataItem.GetInt(FightItemStartX);
                tower.Y = dataItem.GetInt(FightItemStartY);

                // TODO 临时数据
                tower.SetTowerData(new int[,]
                {
                    {90001, 10001, 10005, 90002},
                    {90001, 10004, 10004, 90002},
                    {90001, 10002, 10003, 90002},
                    {90001, 10002, 10003, 90002},
                });
                var fix = new FixtureData(new AllData()
                {
                    MemberData = new MemberData()
                    {
                        AttackRange = 20,
                        ObjID = objId,
                        MoveSpeed = 60,
                        GeneralType = 1,
                        Camp = 1,
                        Attack1 = 10,
                        TotalHp = 100,
                        CurrentHP = 100,
                        AttackRate1 = 1,
                        BehaviorType = FSMFactory.TowerType
                    }
                }, tower);
                fix.X = tower.GameObj.transform.position.x;
                fix.Y = tower.GameObj.transform.position.y;
                fix.StopMove();
                result = new DisplayOwner(fix);

                // 启动状态机
                FSMManager.Single.BeginRunFSM(result);
            }
                break;
            case UnitType.TowerCell:
            {

            }
                break;
            case UnitType.TowerPoint:
            {

            }
                break;
        }

        return result;
    }


    /// <summary>
    /// 获取单位
    /// </summary>
    /// <param name="objId">被获取单位ID</param>
    /// <returns>被获取单位(如果不存在返回Null</returns>
    public DisplayOwner GetElementById(ObjectID objId)
    {
        if (objId == null)
        {
            return null;
        }
        return GetElementById(objId.ObjType, objId.ID);
    }


    /// <summary>
    /// 获取单位
    /// </summary>
    /// <param name="objType">单位类型</param>
    /// <param name="id">单位唯一Id</param>
    /// <returns>被获取单位(如果不存在返回Null</returns>
    public DisplayOwner GetElementById(ObjectID.ObjectType objType, int id)
    {
        if (displayOwners.ContainsKey(id))
        {
            return displayOwners[id];
        }
        return null;
    }



    /// <summary>
    /// 获取Display
    /// </summary>
    /// <param name="pObj">display对应的pObj</param>
    /// <returns>pObj对应的DisplayOwner</returns>
    public DisplayOwner GetElementByPositionObject(PositionObject pObj)
    {
        DisplayOwner result = null;

        if (pObj != null)
        {
            result = GetElementById(pObj.AllData.MemberData.ObjID);
        }
        return result;
    }

    /// <summary>
    /// 根据PositionObject列表获取对应DisplayOwner列表
    /// </summary>
    /// <param name="pObjList">被获取列表</param>
    /// <returns>返回对应列表, 如果没有对应单位返回null</returns>
    public IList<DisplayOwner> GetElementsByPositionObjectList(IList<PositionObject> pObjList)
    {
        List<DisplayOwner> result = null;

        if (pObjList != null && pObjList.Count > 0)
        {
            result = new List<DisplayOwner>(pObjList.Count);
            foreach (var pObj in pObjList)
            {
                var display = GetElementByPositionObject(pObj);
                if (display != null)
                {
                    result.Add(display);
                }
            }
        }
        return result;
    }


    public void Destory([NotNull]DisplayOwner displayOwner)
    {
        // 删除引用
        displayOwners.Remove(displayOwner.Id);
        // 销毁对象
        displayOwner.Destroy();
    }


}