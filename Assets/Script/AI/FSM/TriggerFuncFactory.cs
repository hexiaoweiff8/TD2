using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// trigger判断条件工厂
/// </summary>
public class TriggerFuncFactory
{

    /// <summary>
    /// 获取对应行为类型的trigger检测方法
    /// </summary>
    /// <param name="triggerId">trigger类型</param>
    /// <param name="behaviorType">行为类型</param>
    /// <returns>检测方法</returns>
    public static Func<FSMSystem, bool> GetTriggerFuncByBehaviorType(FSMTriggerID triggerId, int behaviorType)
    {
        Func<FSMSystem, bool> result = null;

        switch (behaviorType)
        {
            // 常规士兵类型
            case FSMFactory.MemberType:
                {
                    // -------------------------------常规士兵类型-----------------------------
                    switch (triggerId)
                    {
                        case FSMTriggerID.Enter:
                            // -----------------------入场------------------------------
                            result = (fsm) =>
                            {
                                return false;
                            };
                            break;
                        case FSMTriggerID.Move:
                            // -----------------------行进------------------------------
                            result = (fsm) =>
                            {
                                if (fsm.Display.ClusterData.AllData.MemberData.CurrentHP <= 0)
                                {
                                    return false;
                                }
                                switch (fsm.CurrentStateID)
                                {
                                    case FSMStateID.Enter:
                                    case FSMStateID.Wait:
                                        return false;
                                    //case FSMStateID.Attack:
                                    //case FSMStateID.Skill:
                                    //    return fsm.TargetIsLoseEfficacy;
                                    //case FSMStateID.Pursue:
                                    //    return !fsm.IsPursue;
                                }
                                return false;
                            };
                            break;
                        case FSMTriggerID.PreFight:
                            // -----------------------准备战斗------------------------------
                            result = (fsm) =>
                            {
                                // 状态检查路由
                                switch (fsm.CurrentStateID)
                                {
                                    // 行进追击切准备战斗
                                    case FSMStateID.Move:
                                    case FSMStateID.Pursue:
                                    case FSMStateID.Wait:
                                        {
                                            return PreFightTrigger.CheckChangeState(fsm);
                                        }
                                    // 技能攻击/普通攻击切准备战斗
                                    case FSMStateID.Attack:
                                        {
                                            //Debug.Log("普通攻击检测技能释放");
                                            // 检测是否有技能可释放
                                            return PreFightTrigger.CheckSkillRelease(fsm);
                                        }
                                }
                                return false;
                            };
                            break;
                        case FSMTriggerID.Attack:
                            // -----------------------普通攻击------------------------------
                            result = (fsm) =>
                            {
                                switch (fsm.CurrentStateID)
                                {
                                    case FSMStateID.PreFight:
                                        return false;
                                }
                                return false;
                            };
                            break;
                        case FSMTriggerID.Skill:
                            // -----------------------技能攻击------------------------------
                            result = (fsm) =>
                            {
                                // 当前单位技能释放判断
                                switch (fsm.CurrentStateID)
                                {
                                    case FSMStateID.PreFight:
                                        return false;
                                    default:
                                        return false;
                                }
                            };
                            break;
                        case FSMTriggerID.Dead:
                            // -----------------------死亡------------------------------
                            result = (fsm) =>
                            {
                                return fsm.Display.ClusterData.AllData.MemberData.CurrentHP <= 0;
                            };
                            break;
                        case FSMTriggerID.Pursue:
                            result = (fsm) => { return false; };
                            break;

                        case FSMTriggerID.Wait:
                            result = (fsm) =>
                            {
                                // 状态检查路由
                                switch (fsm.CurrentStateID)
                                {
                                    // 行进追击切准备战斗
                                    case FSMStateID.Move:
                                    case FSMStateID.Pursue:
                                        {
                                            return PreFightTrigger.CheckChangeState(fsm);
                                        }
                                    // 技能攻击/普通攻击切准备战斗
                                    case FSMStateID.Attack:
                                    case FSMStateID.Skill:
                                        {
                                            return false;
                                        }
                                }
                                return false;
                            };
                            break;
                    }
                }
                break;
            // 防御塔类型
            case FSMFactory.TowerType:
                {
                    // --------------------------------防御塔类型------------------------------------
                    switch (triggerId)
                    {
                        case FSMTriggerID.Enter:
                            // -----------------------入场------------------------------
                            result = (fsm) =>
                            {
                                return false;
                            };
                            break;
                        case FSMTriggerID.PreFight:
                            // -----------------------准备战斗------------------------------
                            result = (fsm) =>
                            {
                                // 状态检查路由
                                switch (fsm.CurrentStateID)
                                {
                                    // 行进追击切准备战斗
                                    case FSMStateID.Wait:
                                        {
                                            return PreFightTrigger.CheckNormalAttack(fsm);
                                        }
                                    // 技能攻击/普通攻击切准备战斗
                                    case FSMStateID.Attack:
                                        {
                                            return false;
                                        }
                                }
                                return false;
                            };
                            break;
                        case FSMTriggerID.Attack:
                            // -----------------------普通攻击------------------------------
                            result = (fsm) =>
                            {
                                switch (fsm.CurrentStateID)
                                {
                                    case FSMStateID.PreFight:
                                        return false;
                                }
                                return false;
                            };
                            break;
                        case FSMTriggerID.Dead:
                            // -----------------------死亡------------------------------
                            result = (fsm) =>
                            {
                                return fsm.Display.ClusterData.AllData.MemberData.CurrentHP <= 0;
                            };
                            break;
                        case FSMTriggerID.Wait:
                            result = (fsm) =>
                            {
                                // 状态检查路由
                                switch (fsm.CurrentStateID)
                                {
                                    // 行进追击切准备战斗
                                    case FSMStateID.Enter:
                                    {
                                        return true;
                                    }
                                    // 技能攻击/普通攻击切准备战斗
                                    case FSMStateID.Attack:
                                    {
                                        return false;
                                    }
                                }
                                return false;
                            };
                            break;
                    }
                }
                break;
            //// 基地类型
            //case FSMFSMFactory.BaseType:
            //    {
            //        // -----------------------------------基地类型---------------------------------
            //        switch (triggerId)
            //        {
            //            case FSMTriggerID.Enter:
            //                // -----------------------入场------------------------------
            //                result = (fsm) =>
            //                {
            //                    return false;
            //                };
            //                break;
            //            case FSMTriggerID.Wait:
            //                //-----------------------待机------------------------------
            //                result = (fsm) =>
            //                {
            //                    // 切待机状态
            //                    return true;
            //                };
            //                break;
            //            case FSMTriggerID.Dead:
            //                // -----------------------死亡------------------------------
            //                result = (fsm) =>
            //                {
            //                    return fsm.Display.ClusterData.AllData.MemberData.CurrentHP <= 0;
            //                };
            //                break;
            //        }
            //    }
            //    break;
            // 地雷类型
            //case FSMFSMFactory.MineType:
            //    {
            //        // ----------------------------------地雷类型---------------------------------
            //        switch (triggerId)
            //        {
            //            case FSMTriggerID.Enter:
            //                // -----------------------入场------------------------------
            //                result = (fsm) =>
            //                {
            //                    return false;
            //                };
            //                break;
            //            case FSMTriggerID.PreFight:
            //                // -----------------------准备战斗------------------------------
            //                result = (fsm) =>
            //                {
            //                    // 状态检查路由
            //                    switch (fsm.CurrentStateID)
            //                    {
            //                        // 行进追击切准备战斗
            //                        case FSMStateID.Enter:
            //                        case FSMStateID.Wait:
            //                        case FSMStateID.Pursue:
            //                            {
            //                                return PreFightTrigger.CheckChangeState(fsm);
            //                            }
            //                        // 技能攻击/普通攻击切准备战斗
            //                        case FSMStateID.Skill:
            //                            {
            //                                return false;
            //                            }
            //                    }
            //                    return false;
            //                };
            //                break;
            //            case FSMTriggerID.Skill:
            //                // -----------------------技能攻击------------------------------
            //                result = (fsm) =>
            //                {
            //                    // 当前单位技能释放判断
            //                    switch (fsm.CurrentStateID)
            //                    {
            //                        case FSMStateID.PreFight:
            //                            return fsm.IsCanInSkill;
            //                        default:
            //                            return false;
            //                    }
            //                };
            //                break;
            //            case FSMTriggerID.Dead:
            //                // -----------------------死亡------------------------------
            //                result = (fsm) =>
            //                {
            //                    return fsm.Display.ClusterData.AllData.MemberData.CurrentHP <= 0;
            //                };
            //                break;
            //        }
            //    }
            //    break;

        }

        return result;
    }
}