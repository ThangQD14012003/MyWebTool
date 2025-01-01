namespace WebTool.Model
{
    public class QrcodeDto
    {
        // public string Url { get; set; } 
        public string? TextData { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Message { get; set; }
        public string? ForegroundColor { set; get; }
        public string? BackgroundColor { set; get; }
        public string? CornerColor { set; get; }
        public string? DotColor { set; get; }
        public string? Logo { get; set; }
        public string? Frame { get; set; }
        public string? FrameText { get; set; }
        public string? ColorText { set; get; }
        public string? TextFont { set; get; }
        public string? ColorFrame { set; get; }
        public int FramePosition { set; get; }
        public string? CornerShape { get; set; }
        public string? DotShape { get; set; }
    }
}
