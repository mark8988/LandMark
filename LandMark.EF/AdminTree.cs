using System;
using System.Collections.Generic;

namespace LandMark.EF;

/// <summary>
/// 網頁功能連結管理
/// </summary>
public partial class AdminTree
{
    /// <summary>
    /// 功能(連結)Id
    /// </summary>
    public int TreeId { get; set; }

    /// <summary>
    /// 功能(連結)Url
    /// </summary>
    public string TreeUrl { get; set; } = null!;

    /// <summary>
    /// 功能(連結)名稱
    /// </summary>
    public string TreeTitle { get; set; } = null!;

    /// <summary>
    /// 功能(連結)Root
    /// </summary>
    public int? TreeRoot { get; set; }

    public string TreeParam { get; set; } = null!;

    public int? DidoSenderId { get; set; }

    public string? Icon { get; set; }
}
