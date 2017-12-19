using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// 状态工厂
/// </summary>
public class StateFuncFunction
{


    /// <summary>
    /// 获取进入状态Action
    /// </summary>
    /// <param name="stateId">状态ID</param>
    /// <param name="behaviorType">行为类型</param>
    /// <returns></returns>
    public static Action<FSMSystem> GetStateBeforeEnterFuncByBehaviorType(FSMStateID stateId, int behaviorType)
    {
        Action<FSMSystem> result = null;

        switch (behaviorType)
        {
            case FSMFactory.TowerType:
                {
                    // --------------------------------防御塔类型------------------------------------
                    switch (stateId)
                    {
                        case FSMStateID.Enter:
                            // -----------------------入场------------------------------
                            result = (fsm) =>
                            {
                                // 抛出入场事件
                                SkillManager.Single.SetTriggerData(new TriggerData()
                                {
                                    ReleaseMember = fsm.Display,
                                    ReceiveMember = fsm.Display,
                                    TypeLevel1 = TriggerLevel1.Fight,
                                    TypeLevel2 = TriggerLevel2.Enter
                                });
                            };
                            break;
                        case FSMStateID.PreFight:
                            // -----------------------准备战斗------------------------------
                            result = (fsm) =>
                            {
                            };
                            break;
                        case FSMStateID.Attack:
                            // -----------------------普通攻击------------------------------
                            result = (fsm) =>
                            {
                                // TODO 启动计时器
                                //_fireTimer = new Timer(fsm.Display.ClusterData.AllData.MemberData.AttackRate1, true);
                                //_fireTimer.OnCompleteCallback(() => { fsm.Display.ClusterData.MapCell.StepAction(fsm.Display.ClusterData.MapCell); }).Start();
                            };
                            break;
                        case FSMStateID.Skill:
                            // -----------------------普通攻击------------------------------
                            result = (fsm) =>
                            {
                                // TODO 启动计时器
                                //_fireTimer = new Timer(fsm.Display.ClusterData.AllData.MemberData.AttackRate1, true);
                                //_fireTimer.OnCompleteCallback(() => { fsm.Display.ClusterData.MapCell.StepAction(fsm.Display.ClusterData.MapCell); }).Start();
                            };
                            break;
                        case FSMStateID.Dead:
                            // -----------------------死亡------------------------------
                            result = (fsm) =>
                            {
                                // 抛出死亡事件
                                SkillManager.Single.SetTriggerData(new TriggerData()
                                {
                                    ReleaseMember = fsm.Display,
                                    ReceiveMember = fsm.Display,
                                    TypeLevel1 = TriggerLevel1.Fight,
                                    TypeLevel2 = TriggerLevel2.Death
                                });

                                // TODO 将目标推入销毁队列, 下一帧执行, 如有复活技能则可在下一帧将其拉出
                                FightUnitManager.Single.Destory(fsm.Display);
                            };
                            break;
                        case FSMStateID.Wait:
                            result = (fsm) =>
                            {
                            };
                            break;
                    }
                }
                break;
        }

        return result;
    }



    /// <summary>
    /// 获取运行状态Action
    /// </summary>
    /// <param name="stateId">状态ID</param>
    /// <param name="behaviorType">行为类型</param>
    /// <returns></returns>
    public static Action<FSMSystem> GetStateDoActionFuncByBehaviorType(FSMStateID stateId, int behaviorType)
    {
        Action<FSMSystem> result = null;

        
        switch (behaviorType)
        {
            case FSMFactory.TowerType:
                {
                    // --------------------------------防御塔类型------------------------------------
                    switch (stateId)
                    {
                        case FSMStateID.Enter:
                            // -----------------------入场------------------------------
                            result = (fsm) =>
                            {
                            };
                            break;
                        case FSMStateID.PreFight:
                            // -----------------------准备战斗------------------------------
                            result = (fsm) =>
                            {
                            };
                            break;
                        case FSMStateID.Attack:
                            // -----------------------普通攻击------------------------------
                            result = (fsm) =>
                            {
                                // TODO 检查范围内是否还有目标
                            };
                            break;
                        case FSMStateID.Dead:
                            // -----------------------死亡------------------------------
                            result = (fsm) =>
                            {
                            };
                            break;
                        case FSMStateID.Wait:
                            result = (fsm) =>
                            {
                                // TODO 检查范围内是否有目标
                            };
                            break;
                    }
                }
                break;
        }

        return result;
    }




    /// <summary>
    /// 获取离开状态Action
    /// </summary>
    /// <param name="stateId">状态ID</param>
    /// <param name="behaviorType">行为类型</param>
    /// <returns></returns>
    public static Action<FSMSystem> GetStateBeforeLeaveFuncByBehaviorType(FSMStateID stateId, int behaviorType)
    {
        Action<FSMSystem> result = null;
        
        switch (behaviorType)
        {
            case FSMFactory.TowerType:
                {
                    // --------------------------------防御塔类型------------------------------------
                    switch (stateId)
                    {
                        case FSMStateID.Enter:
                            // -----------------------入场------------------------------
                            result = (fsm) =>
                            {
                            };
                            break;
                        case FSMStateID.PreFight:
                            // -----------------------准备战斗------------------------------
                            result = (fsm) =>
                            {
                            };
                            break;
                        case FSMStateID.Attack:
                            // -----------------------普通攻击------------------------------
                            result = (fsm) =>
                            {
                                // 结束计时器
                            };
                            break;
                        case FSMStateID.Dead:
                            // -----------------------死亡------------------------------
                            result = (fsm) =>
                            {
                            };
                            break;
                        case FSMStateID.Wait:
                            result = (fsm) =>
                            {
                            };
                            break;
                    }
                }
                break;
        }

        return result;
    }
}
