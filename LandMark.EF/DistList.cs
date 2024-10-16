using System;
using System.Collections.Generic;

namespace LandMark.EF;

/// <summary>
/// 鄉鎮市區
/// </summary>
public partial class DistList
{
    /// <summary>
    /// 城市區域代碼
    /// </summary>
    public long DistNo { get; set; }

    /// <summary>
    /// 城市代碼
    /// </summary>
    public long CityNo { get; set; }

    /// <summary>
    /// 城市區域名稱
    /// </summary>
    public string DistName { get; set; } = null!;

    /// <summary>
    /// 城市區域名稱(英文)
    /// </summary>
    public string? DistEngName { get; set; }

    /// <summary>
    /// 後台登入人員帳號
    /// </summary>
    public int UpdateUser { get; set; }

    /// <summary>
    /// 修改原因
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 此筆記錄汰舊時間
    /// </summary>
    public DateTime? RetireTime { get; set; }

    /// <summary>
    /// 此筆記錄新增時間
    /// </summary>
    public DateTime InsertTime { get; set; }

    /// <summary>
    /// 此筆記錄修改時間
    /// </summary>
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// 郵遞區號
    /// </summary>
    public int? PostalCode { get; set; }

    public virtual CityList CityNoNavigation { get; set; } = null!;
}
