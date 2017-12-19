using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;


/// <summary>
/// 状态机管理器
/// </summary>
public class FSMManager : SingleItem<FSMManager>, ILoopItem
{

    /// <summary>
    /// 状态机列表
    /// </summary>
    private Dictionary<int, FSMControl> fsmDic = new Dictionary<int, FSMControl>();

    /// <summary>
    /// 运行中Fsm
    /// </summary>
    private Dictionary<int, FSMControl> runingFsmDic = new Dictionary<int, FSMControl>();

    /// <summary>
    /// looper编号
    /// </summary>
    private long looperId = -1;

    /// <summary>
    /// 是否在运行
    /// </summary>
    private bool isRuning = false;


    /// <summary>
    /// 启动运行
    /// </summary>
    public void Start()
    {
        if (looperId > 0)
        {
            LooperManager.Single.Remove(looperId);
        }
        looperId = LooperManager.Single.Add(this);
        isRuning = true;
    }


    /// <summary>
    /// 创建状态机
    /// </summary>
    /// <param name="displayOwner">显示对象</param>
    /// <returns>状态机控制器</returns>
    public FSMControl CreateFSMControl([NotNull]DisplayOwner displayOwner)
    {
        FSMControl result = null;
        if (!fsmDic.ContainsKey(displayOwner.Id))
        {
            result = new FSMControl();
            result.StartFSM(displayOwner);
            fsmDic.Add(displayOwner.Id, result);
        }
        return result;
    }

    /// <summary>
    /// 启动状态机
    /// </summary>
    /// <param name="displayOwner">单位DisplayOwner</param>
    public void BeginRunFSM([NotNull] DisplayOwner displayOwner)
    {
        var fsm = CreateFSMControl(displayOwner);
        if (runingFsmDic.ContainsKey(displayOwner.Id))
        {
            UnityEngine.Debug.LogError("该状态机正在运行:" + displayOwner.Id);
            return;
        }
        runingFsmDic.Add(displayOwner.Id, fsm);
    }


    /// <summary>
    /// 获取状态机
    /// </summary>
    /// <param name="id">FSM Id</param>
    /// <returns>FSM</returns>
    public FSMControl GetFSM(int id)
    {
        FSMControl result = null;

        if (fsmDic.ContainsKey(id))
        {
            result = fsmDic[id];
        }

        return result;
    }


    /// <summary>
    /// 清空
    /// </summary>
    public void Stop()
    {
        foreach (var fsm in fsmDic)
        {
            fsm.Value.Destory();
        }
        fsmDic.Clear();
        runingFsmDic.Clear();
        LooperManager.Single.Remove(looperId);
        isRuning = false;
    }


    /// <summary>
    /// 循环执行
    /// </summary>
    public void Do()
    {
        // 执行所有FSM
        foreach (var kv in runingFsmDic)
        {
            kv.Value.UpdateFSM();
        }
    }

    /// <summary>
    /// 结束判断
    /// </summary>
    /// <returns></returns>
    public bool IsEnd()
    {
        return false;
    }

    /// <summary>
    /// 销毁
    /// </summary>
    public void OnDestroy()
    {
        //Stop();
    }
}