using UnityEngine;
using System.Collections;
using System;

public class DeadTrigger : FSMTrigger
{
    public override void Init()
    {
        triggerId = FSMTriggerID.Dead;
    }
    //public override bool CheckTrigger(SoldierFSMSystem fsm)
    //{
    //    return fsm.Display.ClusterData.AllData.MemberData.CurrentHP <= 0;
    //}
}
