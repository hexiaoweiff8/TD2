using UnityEngine;
using System.Collections;
using System;

public class WaitTrigger : FSMTrigger {

    //public override bool CheckTrigger(SoldierFSMSystem fsm)
    //{
    //    return false;
    //}

    public override void Init()
    {
        triggerId = FSMTriggerID.Wait;
    }
}