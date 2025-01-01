using System.Diagnostics.Eventing.Reader;

namespace WebTool.Model
{
    public class TokenModel
    {
        public int length { get; set; } 
        public bool includeUppercase { get; set; }  
        public bool includeLowercase { get; set;}   
        public bool includeNumbers { get; set;} 
        public bool includeSpecialChars { get; set;}    
    }
}
