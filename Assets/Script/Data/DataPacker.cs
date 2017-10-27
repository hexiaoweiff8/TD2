using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Debug = UnityEngine.Debug;


/// <summary>
/// 数据类
/// </summary>
public class DataPacker : SingleItem<DataPacker>
{
    /// <summary>
    /// 数据字典
    /// </summary>
    private Dictionary<string, DataTable> dataDic = new Dictionary<string, DataTable>();


    /// <summary>
    /// 添加数据表
    /// </summary>
    /// <param name="name">数据Id</param>
    /// <param name="dataTable">数据类</param>
    public void SetDataItem([NotNull]string name, [NotNull]DataTable dataTable)
    {
        if (dataDic.ContainsKey(name))
        {
            throw new Exception("数据Id已存在:" + name);
        }
        dataDic.Add(name, dataTable);
    }

    /// <summary>
    /// 获取数据表
    /// </summary>
    /// <param name="name">数据表name</param>
    public DataTable GetDataItem([NotNull] string name)
    {
        return this[name];
    }

    /// <summary>
    /// 删除数据表
    /// </summary>
    /// <param name="name">数据Id</param>
    public void DelDataItem([NotNull] string name)
    {
        dataDic.Remove(name);
    }

    /// <summary>
    /// 清空数据表
    /// </summary>
    public void Clear()
    {
        dataDic.Clear();
    }

    /// <summary>
    /// 解析加载数据
    /// </summary>
    public void Load()
    {
        // 从指定路径获取数据
        // TODO 加载测试数据
        var mapTypeDataTable = new DataTable();

        // 填充测试数据
        var dataRow = new DataItem();
        // 地图数据
        dataRow.SetString("Resource", "Prefab/mapCell0001");
        mapTypeDataTable.AddDataItem("0", dataRow);
        dataRow = new DataItem();
        dataRow.SetString("Resource", "Prefab/empty");
        mapTypeDataTable.AddDataItem("-1", dataRow);
        dataRow = new DataItem();
        dataRow.SetString("Resource", "Prefab/obstacle0001");
        mapTypeDataTable.AddDataItem("1", dataRow);
        dataRow = new DataItem();
        dataRow.SetString("Resource", "Prefab/obstacle0002");
        mapTypeDataTable.AddDataItem("2", dataRow);
        dataRow = new DataItem();
        dataRow.SetString("Resource", "Prefab/member0001");
        mapTypeDataTable.AddDataItem("1001", dataRow);

        SetDataItem(UnitFictory.MapCellTableName, mapTypeDataTable);

    }

    /// <summary>
    /// 获取数据表
    /// </summary>
    /// <param name="tableName">数据表名称</param>
    public DataTable this[string tableName]
    {
        get
        {
            if (!dataDic.ContainsKey(tableName))
            {
                throw new Exception("数据Id不存在:" + tableName);
            }
            return dataDic[tableName];
        }
    }
}


/// <summary>
/// 数据类
/// </summary>
public class DataTable
{
    /// <summary>
    /// 数据名称
    /// </summary>
    public string DataName { get; private set; }

    /// <summary>
    /// 数据行字典
    /// </summary>
    private Dictionary<string, DataItem> dataRowDic = new Dictionary<string, DataItem>();


    /// <summary>
    /// 根据ID获取数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public DataItem GetRowById(string id)
    {
        return this[id];
    }

    /// <summary>
    /// 添加数据行
    /// </summary>
    /// <param name="id">数据ID</param>
    /// <param name="item">数据内容</param>
    public void AddDataItem(string id, DataItem item)
    {
        // 验证数据
        if (item == null)
        {
            Debug.LogError("数据内容为空.");
            return;
        }

        // 是否已包含数据
        if (dataRowDic.ContainsKey(id))
        {
            Debug.LogError("数据ID已存在.");
            return;
        }
        // 添加数据
        dataRowDic.Add(id, item);
    }

    /// <summary>
    /// 根据ID获取数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public DataItem this[string id]
    {
        get
        {
            if (!dataRowDic.ContainsKey(id))
            {
                throw new Exception("不存在数据ID:" + id);
            }
            return dataRowDic[id];
        }
    }
}


/// <summary>
/// 数据行
/// </summary>
public class DataItem
{

    /// <summary>
    /// 数据域-float
    /// </summary>
    private Dictionary<string, float> dataScopeFloat = new Dictionary<string, float>();

    /// <summary>
    /// 数据域-int
    /// </summary>
    private Dictionary<string, int> dataScopeInt = new Dictionary<string, int>();

    /// <summary>
    /// 数据域-long
    /// </summary>
    private Dictionary<string, long> dataScopeLong = new Dictionary<string, long>();

    /// <summary>
    /// 数据域-bool
    /// </summary>
    private Dictionary<string, bool> dataScopeBool = new Dictionary<string, bool>();

    /// <summary>
    /// 数据域-string
    /// </summary>
    private Dictionary<string, string> dataScopeString = new Dictionary<string, string>();



    public float GetFloat(string key)
    {
        if (dataScopeFloat.ContainsKey(key))
        {
            return dataScopeFloat[key];
        }
        return 0;
    }


    public int GetInt(string key)
    {
        if (dataScopeInt.ContainsKey(key))
        {
            return dataScopeInt[key];
        }
        return 0;
    }


    public long GetLong(string key)
    {
        if (dataScopeLong.ContainsKey(key))
        {
            return dataScopeLong[key];
        }
        return 0;
    }


    public bool GetBool(string key)
    {
        if (dataScopeBool.ContainsKey(key))
        {
            return dataScopeBool[key];
        }
        return false;
    }


    public string GetString(string key)
    {
        return dataScopeString[key];
    }


    public void SetFloat(string key, float value)
    {
        if (dataScopeFloat.ContainsKey(key))
        {
            dataScopeFloat[key] = value;
        }
        else
        {
            dataScopeFloat.Add(key, value);
        }
    }
    public void SetInt(string key, int value)
    {
        if (dataScopeInt.ContainsKey(key))
        {
            dataScopeInt[key] = value;
        }
        else
        {
            dataScopeInt.Add(key, value);
        }
    }
    public void SetLong(string key, long value)
    {
        if (dataScopeLong.ContainsKey(key))
        {
            dataScopeLong[key] = value;
        }
        else
        {
            dataScopeLong.Add(key, value);
        }
    }
    public void SetBool(string key, bool value)
    {
        if (dataScopeBool.ContainsKey(key))
        {
            dataScopeBool[key] = value;
        }
        else
        {
            dataScopeBool.Add(key, value);
        }
    }
    public void SetString(string key, string value)
    {
        if (dataScopeString.ContainsKey(key))
        {
            dataScopeString[key] = value;
        }
        else
        {
            dataScopeString.Add(key, value);
        }
    }
}