namespace WebTool.Model
{
    public class ImportModel
    {
        public string? ConnectionStrings { get; set; }    
        public string? TableName { get; set; }
        public int StartRow { get; set; }
        public int EndRow { get; set; }
        public string? SheetName { get; set; }   
    }
}
