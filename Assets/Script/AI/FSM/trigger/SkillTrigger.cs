﻿using UnityEngine;
using System.Collections;
using System;

public class SkillTrigger : FSMTrigger
{
    //public override bool CheckTrigger(SoldierFSMSystem fsm)
    //{
    //    // 当前单位技能释放判断
    //    switch (fsm.CurrentStateID)
    //    {
    //        case SoldierStateID.Zhunbeizhandou:
    //            return fsm.IsCanInJinenggongji;
    //        default :
    //            return false;
    //    }
    //}

    public override void Init()
    {
        triggerId = FSMTriggerID.Skill;
    }
}