using System;
using System.Collections.Generic;

namespace LandMark.EF;

/// <summary>
/// 使用者操作紀錄(後台)
/// </summary>
public partial class AdminActionLog
{
    /// <summary>
    /// 流水號
    /// </summary>
    public long SerialNo { get; set; }

    /// <summary>
    /// 使用者Id
    /// </summary>
    public int AdminId { get; set; }

    /// <summary>
    /// 行為
    /// </summary>
    public string Action { get; set; } = null!;

    /// <summary>
    /// 參數
    /// </summary>
    public string? Params { get; set; }

    /// <summary>
    /// 行為時間
    /// </summary>
    public DateTime ActionTime { get; set; }
}
