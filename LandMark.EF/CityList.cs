using System;
using System.Collections.Generic;

namespace LandMark.EF;

/// <summary>
/// 台灣縣市
/// </summary>
public partial class CityList
{
    /// <summary>
    /// 城市代碼
    /// </summary>
    public long CityNo { get; set; }

    /// <summary>
    /// 城市名稱
    /// </summary>
    public string CityName { get; set; } = null!;

    /// <summary>
    /// 城市名稱(英文)
    /// </summary>
    public string? CityEngName { get; set; }

    /// <summary>
    /// 後台登入人員帳號
    /// </summary>
    public int UpdateUser { get; set; }

    /// <summary>
    /// 顯示在前台0/不顯示1
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

    public virtual ICollection<DistList> DistLists { get; set; } = new List<DistList>();
}
