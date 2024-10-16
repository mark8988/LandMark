namespace LandMark.Middleware.ViewModels
{
    public class Response
    {
        public bool result_status { get; set; }
        public string result_message { get; set; }
        public object result_content { get; set; }
        public string result_stacktrace { get; set; }
    }
}
