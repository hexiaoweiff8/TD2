using System;
using UnityEngine;
using System.Collections;

public abstract class FSMTrigger {
    /// <summary>
    /// 触发器id 对应状态转换
    /// </summary>
    public FSMTriggerID triggerId;

    public FSMTrigger()
    {
        Init();
    }

    public abstract void Init();

    //public abstract bool CheckTrigger(SoldierFSMSystem fsm);

    /// <summary>
    /// 检测方法
    /// </summary>
    public Func<SoldierFSMSystem, bool> CheckTriggerFunc;
}