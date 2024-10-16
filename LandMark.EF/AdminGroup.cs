using System;
using System.Collections.Generic;

namespace LandMark.EF;

/// <summary>
/// 使用者群組管理
/// </summary>
public partial class AdminGroup
{
    /// <summary>
    /// 資料流水號
    /// </summary>
    public int AdminGroupId { get; set; }

    /// <summary>
    /// 群組名稱
    /// </summary>
    public string GroupName { get; set; } = null!;

    /// <summary>
    /// 後台登入者帳號
    /// </summary>
    public int? UpdateUser { get; set; }

    /// <summary>
    /// 此筆記錄新增時間
    /// </summary>
    public DateTime? CreateTime { get; set; }

    /// <summary>
    /// 此筆記錄修改時間
    /// </summary>
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// 登入IP
    /// </summary>
    public string? LoginIp { get; set; }

    /// <summary>
    /// 允許登入時間(起)
    /// </summary>
    public TimeOnly? LoginTimeStart { get; set; }

    /// <summary>
    /// 允許登入時間(迄)
    /// </summary>
    public TimeOnly? LoginTimeEnd { get; set; }

    /// <summary>
    /// 是否為門店(0否/1是)
    /// </summary>
    public bool IsStoreManage { get; set; }

    /// <summary>
    /// 是否為總部(0否/1是)
    /// </summary>
    public bool IsHeadquarter { get; set; }

    /// <summary>
    /// 是否為督導(0否/1是)
    /// </summary>
    public bool IsSupervisor { get; set; }

    /// <summary>
    /// 組織管理ID
    /// </summary>
    public int OrganizeManageId { get; set; }
}
