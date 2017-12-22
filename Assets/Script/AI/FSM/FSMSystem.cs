using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 状态机主类 管理各状态切换
/// </summary>
public class FSMSystem {


    /// <summary>
    /// 允许状态机持有显示对象
    /// 修改为外层持有对象
    /// </summary>
    public DisplayOwner Display;

    /// <summary>
    /// 数据域
    /// </summary>
    public DataScope DataScope = new DataScope();

    /// <summary>
    /// 当前状态ID
    /// 不要直接修改这个变量，之所以让他公有是因为得让其他脚本调用这个变量。
    /// </summary>
    public FSMStateID CurrentStateID { get { return _currentStateId; } }

    /// <summary>
    /// 当前状态
    /// </summary>
    public FSMState CurrentState { get { return _currentState; } }

    ///// <summary>
    ///// 来源状态
    ///// </summary>
    //public SoldierStateID SourceStateID { get; set; }

    // ----------------------------私有属性----------------------------

    /// <summary>
    /// 现有状态列表
    /// </summary>
    private List<FSMState> _states;

    /// <summary>
    /// 当前状态Id
    /// </summary>
    private FSMStateID _currentStateId;

    /// <summary>
    /// 当前状态
    /// </summary>
    private FSMState _currentState;

    // -----------------------------公共方法-----------------------------


    /// <summary>
    /// 初始化
    /// </summary>
    public FSMSystem()
    {
        _states = new List<FSMState>();
    }

    /// <summary>
    /// 增加状态转换对 第一次添加的状态是默认状态
    /// </summary>
    /// <param name="s"></param>
    public void AddState(FSMState s)
    {
        //_currentState = _states.Find((e) => e.StateID == s.StateID);
        if (s == null)
        {
            Debug.LogError("SoldierFSM ERROR: Null reference is not allowed");
            return;
        }

        if (_states.Count == 0)
        {
            _states.Add(s);
            ChangeState(s.StateID);
            return;
        }
        //排除相同的状态
        foreach (FSMState state in _states)
        {
            if (state.StateID == s.StateID)
            {
                Debug.LogError("改状态已存在: " + s.StateID);
                return;
            }
        }
        _states.Add(s);
    }

    /// <summary>
    /// 查找并切换对应id的状态 
    /// </summary>
    /// <param name="stateId"></param>
    public void ChangeState(FSMStateID stateId)
    {
        if (stateId == FSMStateID.NullState)
        {
            Debug.LogError("不允许切换空状态");
        }

        //遍历此状态容器 
        foreach (FSMState state in _states)
        {
            if (state.StateID == stateId)
            {
                if (null != _currentState && _currentState.DoBeforeLeavingAction != null)
                {
                    _currentState.DoBeforeLeavingAction(this);
                }
                // 设置前置状态
                //SourceStateID = _currentStateId;
                //只允许在这里切换状态
                _currentState = state;
                _currentStateId = state.StateID;
                //Debug.Log("执行状态切换-------------------" + state.StateID);
                if (null != _currentState && _currentState.DoBeforeEnterintAction != null)
                {
                    _currentState.DoBeforeEnterintAction(this);
                }
                break;
            }
        }
    }

    /// <summary>
    /// 销毁
    /// </summary>
    public void Destory()
    {
        foreach (var state in _states)
        {
            state.Destory();
        }
        _states.Clear();
    }

    ///// <summary>
    ///// 设置数据
    ///// TODO 这步操作之前应该还有一步将数据转化为本地数据
    ///// 如果收到的是入场则创建单位并入场
    ///// </summary>
    ///// <param name="fsm"></param>
    //public void SetData(SoldierFSMControl fsm)
    //{

    //}
}