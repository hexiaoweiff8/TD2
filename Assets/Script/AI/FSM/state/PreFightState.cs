using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PreFightState : FSMState
{


    public override void Init()
    {
        this.StateID = FSMStateID.PreFight;
    }
}