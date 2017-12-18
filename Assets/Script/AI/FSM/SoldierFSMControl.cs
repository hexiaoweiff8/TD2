using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using System.Reflection;

public class SoldierFSMControl{

    private SoldierFSMSystem fsm;//内置一个fsm

    /// <summary>
    /// 标记状态机是否唤醒的 如果宿主在对象池中需要把它置为休眠
    /// </summary>
    private bool _iSAwake = false;

    /// <summary>
    /// 进入休眠状态
    /// </summary>
    public void Sleep()
    {
        _iSAwake = true;
    }

    /// <summary>
    /// 重新唤醒状态机 一般用在从对象池中pop出宿主的时候
    /// </summary>
    public void Awaken()
    {
        _iSAwake = false;
    }

    /// <summary>
    /// 启动状态机
    /// </summary>
    /// <param name="obj"></param>
    public void StartFSM([NotNull]DisplayOwner obj)
    {
        //初始化状态机
        fsm = new SoldierFSMSystem();
        fsm.Display = obj;

        // 初始化行为状态机
        InitState(obj.ClusterData.AllData.MemberData.BehaviorType);
    }

    /// <summary>
    /// 启动状态机
    /// </summary>
    /// <param name="objId">单位ObjId</param>
    public void StartFSM([NotNull] ObjectID objId)
    {
        StartFSM(FightUnitManager.Single.GetElementById(objId));
    }



    /// <summary>
    /// 初始化行为状态机
    /// </summary>
    /// <param name="behaviorType"></param>
    public void InitState(int behaviorType)
    {
        SetStateMappingConfig(SoldierFSMFactory.GetBehaviorMappingDicById(behaviorType),
            SoldierFSMFactory.GetTriggerFuncDicById(behaviorType));
    }


    /// <summary>
    /// 驱动状态机
    /// </summary>
    public void UpdateFSM()
    {
        if (_iSAwake) return;
        fsm.CurrentState.CheckTrigger(fsm);
        if (fsm.CurrentState.DoAction != null)
        {
            fsm.CurrentState.DoAction(fsm);
        }
    }


    /// <summary>
    /// 销毁
    /// </summary>
    public void Destory()
    {
        fsm.Destory();
    }


    /// <summary>
    /// 设置切换映射关系
    /// </summary>
    /// <param name="mapDic">切换关系列表</param>
    /// <param name="triggerFuncDic">节点具体行为列表</param>
    public void SetStateMappingConfig(Dictionary<FSMStateID, List<FSMStateID>> mapDic, Dictionary<FSMTriggerID, Func<SoldierFSMSystem, bool>> triggerFuncDic)
    {
        if (mapDic == null)
        {
            return;
        }

        foreach (var kv in mapDic)
        {
            var keyType = SoldierFSMFactory.GetStateTypeByStateId(kv.Key);
            var keyStateInvoke = (FSMState)keyType.InvokeMember("", BindingFlags.Public | BindingFlags.CreateInstance,
                null, null, null);

            foreach (var mapStateId in kv.Value)
            {
                // 设置映射关系
                keyStateInvoke.AddMappingTrigger(SoldierFSMFactory.GetTriggerByStateId(mapStateId),
                    triggerFuncDic[SoldierFSMFactory.GetTriggerByStateId(mapStateId)]);

            }
            // 添加状态
            fsm.AddState(keyStateInvoke);
        }
    }

    ///// <summary>
    ///// 停止攻击当前目标
    ///// </summary>
    //public void CleanTarget()
    //{
    //    // 终止普通攻击目标
    //    if (fsm.CurrentStateID == FSMStateID.Attack && fsm.CurrentState.DoBeforeLeavingAction != null)
    //    {
    //        fsm.CurrentState.DoBeforeLeavingAction(fsm);
    //    }
    //    // 终止技能攻击目标

    //}
}