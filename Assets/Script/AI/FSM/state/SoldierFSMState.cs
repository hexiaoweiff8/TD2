using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// 抽象类，所有状态必须继承它 并在子类中实例化
/// </summary>
public abstract class FSMState
{

    public FSMStateID StateID
    {
        get { return _stateId; }
        set { _stateId = value; }
    }

    /// <summary>
    /// 进入改状态
    /// </summary>
    public Action<SoldierFSMSystem> DoBeforeEnterintAction;

    /// <summary>
    /// 执行Action
    /// </summary>
    public Action<SoldierFSMSystem> DoAction;

    /// <summary>
    /// 离开改状态Action
    /// </summary>
    public Action<SoldierFSMSystem> DoBeforeLeavingAction;

    /// <summary>
    /// 状态ID
    /// </summary>
    protected FSMStateID _stateId;


    /// <summary>
    /// 当前状态可切换状态的trigger检测列表
    /// </summary>
    private List<FSMTrigger> _fsmTrriggerList = new List<FSMTrigger>();

    /// <summary>
    /// 初始化
    /// </summary>
    public FSMState()
    {
        Init();
    }

    /// <summary>
    /// 添加映射trigger
    /// </summary>
    /// <param name="triggerId">被映射TriggerId</param>
    /// <param name="triggerFunc">trigger具体行为</param>
    public void AddMappingTrigger(FSMTriggerID triggerId, Func<SoldierFSMSystem, bool> triggerFunc)
    {
        var triggerType = SoldierFSMFactory.GetTriggerTypeByTriggerId(triggerId);
        var triggerInvoke = (FSMTrigger)triggerType.InvokeMember("", BindingFlags.Public | BindingFlags.CreateInstance,
               null, null, null);
        triggerInvoke.CheckTriggerFunc = triggerFunc;
        _fsmTrriggerList.Add(triggerInvoke);
    }

    /// <summary>
    /// 状态的改变发生在这里 
    /// </summary>
    public void CheckTrigger(SoldierFSMSystem fsm)
    {
        for (int i = 0; i < _fsmTrriggerList.Count; i++)
        {
            //if (_fsmTrriggerList[i].CheckTrigger(fsm))
            if (_fsmTrriggerList[i].CheckTriggerFunc(fsm))
            {
                var state = SoldierFSMFactory.GetStateIdByTrigger(_fsmTrriggerList[i].triggerId);
                fsm.ChangeState(state);
                //Debug.Log("切换状态:" + state);
                break;
            }
        }
    }

    /// <summary>
    /// 模型切换动作 往往配合状态切换
    /// </summary>
    public void SwitchAnim(SoldierFSMSystem fsm, string aniName, WrapMode mode)
    {
        Debug.Log("切换动画:" + aniName);
        //var myself = fsm.Display.RanderControl;
        //myself.PlayAni(aniName, mode);
    }

    /// <summary>
    /// 销毁
    /// </summary>
    public void Destory()
    {
        _fsmTrriggerList.Clear();
        //dict.Clear();
    }



    /// <summary>
    /// 初始化方法
    /// </summary>
    public abstract void Init();
}


/// <summary>
/// 士兵的各个状态
/// </summary>
public enum FSMStateID
{
    //入场
    Enter,
    //待机
    Wait,
    //行进
    Move,
    //死亡
    Dead,
    //尸体
    ShiTi,
    //准备战斗状态 因为攻击分为普通攻击和技能攻击 所以扩展准备战斗状态 在这个状态里决定是哪种战斗
    PreFight,
    //隐身
    Hide,
    //普通攻击中状态
    Attack,
    //技能攻击中状态
    Skill,
    // 追击状态
    Pursue,
    //受击状态
    BeAttack,
    NullState = -10001
}

/// <summary>
/// 定义Transition(转换)类型的枚举变量，以后根据需要扩展
/// </summary>
public enum FSMTriggerID
{
    //入场
    Enter,
    //待机
    Wait,
    //行进
    Move,
    //死亡
    Dead,
    //尸体
    ShiTi,
    //准备战斗
    PreFight,
    //隐身
    Hide,
    //普通攻击
    Attack,
    //技能攻击
    Skill,
    // 追击
    Pursue,
    //受击
    BeAttack,
    NullTri = -10001
}
