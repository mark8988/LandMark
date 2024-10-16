using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LandMark.Middleware
{
    public class GlobalSettings
    {
        public ExceptionFilterSetting ExceptionFilterSetting { get; set; }
        public EmailSetting EmailSetting { get; set; }
    
    }

    /// <summary>
    /// ExceptionFilter參數
    /// </summary>
    public class ExceptionFilterSetting
    {
        public string IsEnable { get; set; }
        public string TargetEmail { get; set; }
    }

    /// <summary>
    /// 寄信參數
    /// </summary>
    public class EmailSetting
    {
        public string Url { get; set; }
        public string sysName { get; set; }

    }
}
