using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoldierFSMFactory
{
    /// <summary>
    /// 士兵类型
    /// </summary>
    public const int SoldierType = 1;

    /// <summary>
    /// 防御塔类型
    /// </summary>
    public const int TowerType = 2;

    /// <summary>
    /// 基地类型
    /// </summary>
    public const int BaseType = 3;

    /// <summary>
    /// 地雷类型
    /// </summary>
    public const int MineType = 4;

    /// <summary>
    /// 获取行为表结构
    /// </summary>
    /// <param name="behaviorType">行为编号</param>
    /// <returns></returns>
    public static Dictionary<FSMStateID, List<FSMStateID>> GetBehaviorMappingDicById(int behaviorType)
    {

        Dictionary<FSMStateID, List<FSMStateID>> result = null;
        switch (behaviorType)
        {
            //case SoldierType:
            //{
            //    // 常规士兵行为
            //    result = new Dictionary<SoldierStateID, List<SoldierStateID>>()
            //    {
            //        {SoldierStateID.RuChang, new List<SoldierStateID>()
            //        {
            //            SoldierStateID.SiWang,
            //            SoldierStateID.Xingjin
            //        }},
            //        {SoldierStateID.Xingjin, new List<SoldierStateID>()
            //        {
            //            SoldierStateID.SiWang,
            //            SoldierStateID.Zhunbeizhandou,
            //            SoldierStateID.ZhuiJi,
            //            SoldierStateID.DaiJi,
            //        }},
            //        {SoldierStateID.Zhunbeizhandou, new List<SoldierStateID>()
            //        {
            //            SoldierStateID.SiWang,
            //            SoldierStateID.JinengGongji,
            //            SoldierStateID.PutongGongji,
            //        }},
            //        {SoldierStateID.SiWang, new List<SoldierStateID>()},
            //        {SoldierStateID.PutongGongji, new List<SoldierStateID>()
            //        {
            //            SoldierStateID.SiWang,
            //            SoldierStateID.DaiJi,
            //            SoldierStateID.Zhunbeizhandou
            //        }},
            //        {SoldierStateID.JinengGongji, new List<SoldierStateID>()
            //        {
            //            SoldierStateID.SiWang,
            //            SoldierStateID.DaiJi,
            //        }},
            //        {SoldierStateID.ZhuiJi, new List<SoldierStateID>()
            //        {
            //            SoldierStateID.SiWang,
            //            SoldierStateID.Zhunbeizhandou,
            //            SoldierStateID.DaiJi,
            //        }},
            //        {SoldierStateID.DaiJi, new List<SoldierStateID>()
            //        {
            //            SoldierStateID.SiWang,
            //            SoldierStateID.Zhunbeizhandou,
            //            SoldierStateID.Xingjin,
            //        }},
            //    };
            //}
            //    break;
            case TowerType:
            {
                // 防御塔行为
                result = new Dictionary<FSMStateID, List<FSMStateID>>()
                {
                    {FSMStateID.Enter, new List<FSMStateID>()
                    {
                        FSMStateID.Dead,
                        FSMStateID.Wait,
                        FSMStateID.PreFight,
                    }},
                    {FSMStateID.PreFight, new List<FSMStateID>()
                    {
                        FSMStateID.Dead,
                        FSMStateID.Attack
                    }},
                    {FSMStateID.Attack, new List<FSMStateID>()
                    {
                        FSMStateID.Dead,
                        FSMStateID.PreFight,
                        FSMStateID.Wait,
                    }},
                    {FSMStateID.Wait, new List<FSMStateID>()
                    {
                        FSMStateID.Dead,
                        FSMStateID.PreFight,
                    }},
                    {FSMStateID.Dead, new List<FSMStateID>()},
                };
            }
                break;
            //case BaseType:
            //{
            //    // 基地行为
            //    result = new Dictionary<SoldierStateID, List<SoldierStateID>>()
            //    {
            //        {SoldierStateID.RuChang, new List<SoldierStateID>()
            //        {
            //            SoldierStateID.DaiJi,
            //            SoldierStateID.SiWang,
            //        }},
            //        {SoldierStateID.DaiJi, new List<SoldierStateID>()
            //        {
            //            SoldierStateID.SiWang,
            //        }},
            //        {SoldierStateID.SiWang, new List<SoldierStateID>()},
            //    };
            //}
            //    break;
            //case MineType:
            //{
            //    // 地雷行为
            //    result = new Dictionary<SoldierStateID, List<SoldierStateID>>()
            //    {
            //        {SoldierStateID.RuChang, new List<SoldierStateID>()
            //        {
            //            SoldierStateID.SiWang,
            //            SoldierStateID.Zhunbeizhandou,
            //        }},
            //        {SoldierStateID.Zhunbeizhandou, new List<SoldierStateID>()
            //        {
            //            SoldierStateID.SiWang,
            //            SoldierStateID.JinengGongji
            //        }},
            //        {SoldierStateID.JinengGongji, new List<SoldierStateID>()
            //        {
            //            SoldierStateID.SiWang,
            //            SoldierStateID.Zhunbeizhandou,
            //        }},
            //        {SoldierStateID.SiWang, new List<SoldierStateID>()},
            //    };
            //}
            //    break;
        }

        return result;
    }


    /// <summary>
    /// 获取行为表结构
    /// </summary>
    /// <param name="behaviorType">行为编号</param>
    /// <returns></returns>
    public static Dictionary<FSMTriggerID, Func<SoldierFSMSystem, bool>> GetTriggerFuncDicById(int behaviorType)
    {

        Dictionary<FSMTriggerID, Func<SoldierFSMSystem, bool>> result = new Dictionary<FSMTriggerID, Func<SoldierFSMSystem, bool>>();

        // 获取当前行为类型的行为列表
        var behaviorDic = GetBehaviorMappingDicById(behaviorType);
        // 遍历列表获取行为具体内容
        foreach (var kv in behaviorDic)
        {
            var triggerId = GetTriggerByStateId(kv.Key);
            result.Add(triggerId, TriggerFuncFactory.GetTriggerFuncByBehaviorType(triggerId, behaviorType));
        }

        // 构建trigger事件

        return result;
    }

    /// <summary>
    /// triggerId转stateId
    /// </summary>
    /// <param name="id">SoldierTriggerID</param>
    /// <returns>SoldierStateID</returns>
    public static FSMStateID GetStateIdByTrigger(FSMTriggerID id)
    {
        switch (id)
        {
            case FSMTriggerID.Enter:
                return FSMStateID.Enter;
            case FSMTriggerID.Move:
                return FSMStateID.Move;
            case FSMTriggerID.PreFight:
                return FSMStateID.PreFight;
            case FSMTriggerID.Attack:
                return FSMStateID.Attack;
            case FSMTriggerID.Skill:
                return FSMStateID.Skill;
            case FSMTriggerID.Dead:
                return FSMStateID.Dead;
            case FSMTriggerID.Pursue:
                return FSMStateID.Pursue;
            case FSMTriggerID.Wait:
                return FSMStateID.Wait;
        }
        return FSMStateID.NullState;
    }

    /// <summary>
    /// stateId转triggerId
    /// </summary>
    /// <param name="id">SoldierStateID</param>
    /// <returns>SoldierTriggerID</returns>
    public static FSMTriggerID GetTriggerByStateId(FSMStateID id)
    {
        switch (id)
        {
            case FSMStateID.Enter:
                return FSMTriggerID.Enter;
            case FSMStateID.Move:
                return FSMTriggerID.Move;
            case FSMStateID.PreFight:
                return FSMTriggerID.PreFight;
            case FSMStateID.Attack:
                return FSMTriggerID.Attack;
            case FSMStateID.Skill:
                return FSMTriggerID.Skill;
            case FSMStateID.Dead:
                return FSMTriggerID.Dead;
            case FSMStateID.Pursue:
                return FSMTriggerID.Pursue;
            case FSMStateID.Wait:
                return FSMTriggerID.Wait;
        }
        return FSMTriggerID.NullTri;
    }

    /// <summary>
    /// stateId转FSMState类type
    /// </summary>
    /// <param name="id">SoldierStateID</param>
    /// <returns>FSMState类Type</returns>
    public static Type GetStateTypeByStateId(FSMStateID id)
    {
        switch (id)
        {
            case FSMStateID.Enter:
                return typeof(EnterState);
            //case FSMStateID.Move:
            //    return typeof(MoveState);
            case FSMStateID.PreFight:
                return typeof(PreFightState);
            case FSMStateID.Attack:
                return typeof(AttackState);
            case FSMStateID.Skill:
                return typeof(SkillState);
            case FSMStateID.Dead:
                return typeof(DeadState);
            //case FSMStateID.Pursue:
            //    return typeof(PursueState);
            case FSMStateID.Wait:
                return typeof(WaitState);
        }
        return typeof(EnterState);
    }

    /// <summary>
    /// stateId转FSMState类type
    /// </summary>
    /// <param name="id">SoldierTriggerID</param>
    /// <returns>FSMState类Type</returns>
    public static Type GetTriggerTypeByTriggerId(FSMTriggerID id)
    {
        switch (id)
        {
            case FSMTriggerID.Enter:
                return typeof(EnterTrigger);
            case FSMTriggerID.Move:
                return typeof(MoveTrigger);
            case FSMTriggerID.PreFight:
                return typeof(PreFightTrigger);
            case FSMTriggerID.Attack:
                return typeof(AttackTrigger);
            case FSMTriggerID.Skill:
                return typeof(SkillTrigger);
            case FSMTriggerID.Dead:
                return typeof(DeadTrigger);
            case FSMTriggerID.Pursue:
                return typeof(PursueTrigger);
            case FSMTriggerID.Wait:
                return typeof(WaitTrigger);
        }
        return typeof(EnterTrigger);
    }
}