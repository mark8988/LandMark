namespace LandMark.Middleware.ViewModels.CloudFlare
{
    public class ResCloudFlare<T>
    {
        public Result<T> result { get; set; }
        public bool success { get; set; }
        public object[] errors { get; set; }
        public object[] messages { get; set; }
    }

    public class Result<T>
    {
        public string id { get; set; }
        public T value { get; set; }
        public DateTime modified_on { get; set; }
        public bool editable { get; set; }
    }



    public class Minify
    {
        public string js { get; set; }
        public string css { get; set; }
        public string html { get; set; }
    }
}
