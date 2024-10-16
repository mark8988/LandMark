using System;
using System.Collections.Generic;

namespace LandMark.EF;

/// <summary>
/// 使用者權限管理
/// </summary>
public partial class AdminRight
{
    /// <summary>
    /// 流水號
    /// </summary>
    public int AdminRightsId { get; set; }

    /// <summary>
    /// 群組ID
    /// </summary>
    public int GroupId { get; set; }

    /// <summary>
    /// TreeID
    /// </summary>
    public int TreeId { get; set; }

    /// <summary>
    /// 顯示排序
    /// </summary>
    public int TreeOrder { get; set; }

    /// <summary>
    /// 是否可執行新增
    /// </summary>
    public bool AllowInsert { get; set; }

    /// <summary>
    /// 是否可執行修改
    /// </summary>
    public bool AllowUpdate { get; set; }

    /// <summary>
    /// 是否可執行刪除
    /// </summary>
    public bool AllowDelete { get; set; }

    /// <summary>
    /// 是否顯示查詢的全部資訊
    /// </summary>
    public bool AllowShowAllData { get; set; }

    /// <summary>
    /// 是否顯示左側導航列
    /// </summary>
    public bool ShowInMenu { get; set; }

    /// <summary>
    /// 寄送通知
    /// </summary>
    public bool SendDido { get; set; }

    /// <summary>
    /// 接收通知
    /// </summary>
    public bool ReceiveDido { get; set; }
}
