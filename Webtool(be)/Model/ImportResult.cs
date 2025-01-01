namespace WebTool.Model
{
    public class ImportResult
    {
        public List<int> ErrorRows {  get; set; }   
        public List<int> SuccessRows {  get; set; }
        public string? ErrorSheet { get; set; }
        //public String 
        public ImportResult()
        {
            ErrorRows = new List<int>();
            SuccessRows = new List<int>();
            ErrorSheet = string.Empty;
        }
    }
}
