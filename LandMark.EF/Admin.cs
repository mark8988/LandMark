using System;
using System.Collections.Generic;

namespace LandMark.EF;

/// <summary>
/// 登入者帳號資訊
/// </summary>
public partial class Admin
{
    /// <summary>
    /// 帳號ID
    /// </summary>
    public int AdminId { get; set; }

    /// <summary>
    /// 群組ID
    /// </summary>
    public int AdminGroupId { get; set; }

    /// <summary>
    /// 登入帳號
    /// </summary>
    public string AdminAccount { get; set; } = null!;

    /// <summary>
    /// 登入密碼
    /// </summary>
    public string AdminPassWord { get; set; } = null!;

    /// <summary>
    /// 使用者EMAIL
    /// </summary>
    public string AdminEmail { get; set; } = null!;

    /// <summary>
    /// 使否啟用
    /// </summary>
    public bool IsEnable { get; set; }

    /// <summary>
    /// 修改人
    /// </summary>
    public int? UpdateUser { get; set; }

    /// <summary>
    /// 此筆記錄新增時間
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 此筆記錄修改時間
    /// </summary>
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// 使用者姓名
    /// </summary>
    public string? AdminName { get; set; }

    /// <summary>
    /// 使用者描述(備註)
    /// </summary>
    public string? AdminPhone { get; set; }

    /// <summary>
    /// LineID
    /// </summary>
    public string? AdminLineId { get; set; }
}
