using WebTool.Model;
using System.CodeDom.Compiler;
using System.Linq;
using System.Text;
namespace WebTool.Services
{
    public class TokenServices
    {
        private static readonly Random random = new Random();
        // không cần async await cũng được vì không thực hiện các công việc tốn thời gian như I/O (gọi API, đọc tệp, truy vấn cơ sở dữ liệu
        public async Task<string> GenerateToken(TokenModel model)
        {
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string number = "0123456789";
            const string special = "!@#$%^&*()-_=+[]{}|;:'\",.<>?/`~";
            var characterSet = new StringBuilder();
            var random = new Random();
            var token = new List<char>();

            if (model.includeUppercase)
            {
                characterSet.Append(upper);
                token.Add(upper[random.Next(upper.Length)]);
                //random.Next(maxValue) trả về một số nguyên ngẫu nhiên trong khoảng từ 0 đến maxValue - 1.
                //Ví dụ, nếu maxValue là 10, random.Next(10) sẽ trả về các giá trị từ 0 đến 9.
            }
            if (model.includeLowercase)
            {
                characterSet.Append(lower);
                token.Add(lower[random.Next(lower.Length)]);
            }
            if (model.includeNumbers)
            {
                characterSet.Append(number);
                token.Add(number[random.Next(number.Length)]);
            }
          if(model.includeSpecialChars)
            {
                token.Add(special[random.Next(special.Length)]);
                characterSet.Append(special); 
            }
            while (token.Count < model.length)
            {
                token.Add(characterSet[random.Next(characterSet.Length)]);  
            }
            return new string(token.OrderBy(_ => random.Next()).ToArray());
        }


        public string EncodeBase64(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(bytes);
        }

        public string EncodeSha256(string input)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToHexString(hash);
            }
        }

        public string EncodeSha512(string input)
        {
            using (var sha512 = System.Security.Cryptography.SHA512.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hash = sha512.ComputeHash(bytes);
                return Convert.ToHexString(hash);
            }
        }

        public string EncodeMd5(string input)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hash = md5.ComputeHash(bytes);
                return Convert.ToHexString(hash);
            }
        }
    }
}
