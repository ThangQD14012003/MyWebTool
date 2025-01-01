using Dapper;
using ExcelDataReader;
using WebTool.Model;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using System.Data;
using WebTool.ProgressHub;
//SqlConnection: Kết nối với cơ sở dữ liệu SQL Server
//SqlCommand: Thực hiện các truy vấn SQL
//SqlTransaction: Quản lý các giao dịch
// SqlDataReader: được sử dụng để đọc kết quả từ truy vấn SQL
// ý tưởng là tạo 1 thằng để mở file sau đó đọc file gán cho datatable, rồi lấy kết quả từ datatable thêm vào csdl
namespace WebTool.Services
{
    public class ExcelServices
    {
        private readonly IHubContext<HubSignalR> _hubContext;
        public ExcelServices(IHubContext<HubSignalR> hubContext )
        {
            _hubContext = hubContext;
        }
        public async Task<IEnumerable<ColumnInfo>> GetColumnsInfoFromTable(ImportModel model)
        {
            using (var connection = new SqlConnection(model.ConnectionStrings))
            {
                await connection.OpenAsync();
                var sql = @"
            SELECT 
                column_name AS ColumnName, 
                data_type AS DataType, 
                is_nullable as IsNullable, 
                CASE WHEN COLUMNPROPERTY(object_id(@Table), column_name, 'IsIdentity') = 1 THEN 1 ELSE 0 END AS IsIdentity 
            FROM 
                information_schema.columns 
            WHERE 
                table_name = @Table";

                var columnsInfo = await connection.QueryAsync<ColumnInfo>(sql, new { Table = model.TableName });
                //Phương thức QueryAsync<T> của Dapper thực hiện một truy vấn và ánh xạ kết quả tới một danh
                //sách các đối tượng kiểu ColumnInfo
                return columnsInfo;
            }
        }

        public int ColumnIndexFromName(string columnName)
        {
            int columnIndex = 0;
            foreach (char c in columnName)
            {
                columnIndex = columnIndex * 26 + (c - 'A' + 1);
            }
            return columnIndex;
        }
        
        public async Task<ImportResult?> ImportExcelToDatabaseError(ImportModel model, IFormFile file, Dictionary<string, string> columnMappings)
        {
            var result = new ImportResult();
            using (var stream = file.OpenReadStream())
            //OpenReadStream() là phương thức của IFormFile để mở một luồng đọc từ file này.
            //stream là một đối tượng Stream có thể sử dụng để đọc dữ liệu từ file.
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            //ExcelReaderFactory.CreateReader(stream) trả về một đối tượng IExcelDataReader có khả năng đọc dữ liệu từ file Excel
            // ở đây là tạo thằng reader để đọc từ thằng stream(luồng dữ liệu từ file đã mở ở trên)
            {
                var dataSet = reader.AsDataSet();
                var dataTable = dataSet.Tables[model.SheetName];
                //Đọc dữ liệu từ file Excel vào DataSet.
                //DataSet là một tập hợp của các DataTable và các mối quan hệ giữa chúng.
                if (dataTable == null)
                {
                    var mes = "No";
                    result.ErrorSheet = mes;  
                    return result;
                }

                for (int i = 0; i < model.StartRow - 1; i++)
                {
                    dataTable.Rows.RemoveAt(0);
                }

                using (var connection = new SqlConnection(model.ConnectionStrings))
                {
                    await connection.OpenAsync();
                    // var dbColumns = (await GetColumnsFromTable(model)).ToList();
                    //var identitySeed = await connection.ExecuteScalarAsync<int>($"select ident_current('{model.TableName}')");
                    for (int i = 0; i <= (model.EndRow - model.StartRow); i++)
                    {
                       
                        var columns = new List<string>();
                        var parameters = new DynamicParameters();
                        
                        foreach (var mapping in columnMappings)
                        {
                            if (string.IsNullOrEmpty(mapping.Value)) continue;
                            var excelColumn = mapping.Value;
                           // key là tên cột trong file Exce
                            var dbColumn = mapping.Key;
                            // value là tên cột tương ứng trong csdl 
                            columns.Add(dbColumn);
                            var excelColumnIndex = ColumnIndexFromName(excelColumn) - 1;
                            parameters.Add($"@{dbColumn}", dataTable.Rows[i][excelColumnIndex]);
                            // chuỗi nội suy, tương đương templatestring trong js
                        }
                        var sql = $"INSERT INTO {model.TableName} ({string.Join(", ", columns)}) VALUES ({string.Join(", ", columns.Select(c => "@" + c))})";
                        // insert into mytable (id, name, age) values (@id, @name, @age) 
                        //Câu lệnh INSERT chỉ dùng để thêm các dòng mới vào bảng chứ k thêm vàoa 1 dòng đã có 1 cột chứa dữ liệu
                        using(var transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                await connection.ExecuteAsync(sql, parameters, transaction);
                                result.SuccessRows.Add(i + model.StartRow);
                                transaction.Rollback();
                                await _hubContext.Clients.All.SendAsync("ReceiveProgress", $"{i + 1}");
                            //await connection.ExecuteAsync($"DBCC CHECKIDENT ('{model.TableName}', RESEED, {identitySeed})");

                        }
                            catch (Exception)
                            {
                        result.ErrorRows.Add(i + model.StartRow);
                        await _hubContext.Clients.All.SendAsync("ReceiveProgress", $"{i + 1}");
                    }
                }
                        
                    }
                }
            }
            return result; 
        }
       
        public async Task<ImportResult> ImportExcelToDatabaseNoError(ImportModel model, IFormFile file, Dictionary<string, string> columnMappings)
        {
            var result = new ImportResult();
            using (var stream = file.OpenReadStream())
            //OpenReadStream() là phương thức của IFormFile để mở một luồng đọc từ file này.
            //stream là một đối tượng Stream có thể sử dụng để đọc dữ liệu từ file.
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            //ExcelReaderFactory.CreateReader(stream) trả về một đối tượng IExcelDataReader có khả năng đọc dữ liệu từ file Excel
            // ở đây là tạo thằng reader để đọc từ thằng stream(luồng dữ liệu từ file đã mở ở trên)
            {
                var dataSet = reader.AsDataSet();
                var dataTable = dataSet.Tables[model.SheetName];
                //Đọc dữ liệu từ file Excel vào DataSet.
                //DataSet là một tập hợp của các DataTable và các mối quan hệ giữa chúng.
                //if (dataTable == null)
                //{
                //    return; 
                //}
                
                for (int i = 0; i < model.StartRow - 1; i++)
                {
                    dataTable.Rows.RemoveAt(0);
                }

                using (var connection = new SqlConnection(model.ConnectionStrings))
                {
                    await connection.OpenAsync();
                   // var dbColumns = (await GetColumnsFromTable(model)).ToList();

                    for (int i = 0; i <= (model.EndRow - model.StartRow); i++)
                    {
                       
                        var columns = new List<string>();
                        var parameters = new DynamicParameters();
                        
                        foreach (var mapping in columnMappings)
                        {
                            if (string.IsNullOrEmpty(mapping.Value)) continue;
                            var excelColumn = mapping.Value;
                           // key là tên cột trong file Exce
                            var dbColumn = mapping.Key;
                            // value là tên cột tương ứng trong csdl 
                            columns.Add(dbColumn);
                            var excelColumnIndex = ColumnIndexFromName(excelColumn) - 1;
                            parameters.Add($"@{dbColumn}", dataTable.Rows[i][excelColumnIndex]);
                            // chuỗi nội suy, tương đương templatestring trong js
                        }
                        var sql = $"INSERT INTO {model.TableName} ({string.Join(", ", columns)}) VALUES ({string.Join(", ", columns.Select(c => "@" + c))})";
                        // insert into mytable (id, name, age) values (@id, @name, @age) 
                        //Câu lệnh INSERT chỉ dùng để thêm các dòng mới vào bảng chứ k thêm vào 1 dòng đã có 1 cột chứa dữ liệu
                        try
                        {
                            await connection.ExecuteAsync(sql, parameters);
                            result.SuccessRows.Add(i+model.StartRow);
                            // await _hubContext.Clients.All.SendAsync("ReceiveProgress", $"Row{i + model.StartRow} import successfully");

                            await _hubContext.Clients.All.SendAsync("ReceiveProgress", $"{i + 1}");
                        }
                        catch (Exception ex)
                        {   
                            Console.WriteLine(ex.ToString());
                            // await _hubContext.Clients.All.SendAsync("ReceiveProgress", $"Row {i + model.StartRow} import failed");
                            await _hubContext.Clients.All.SendAsync("ReceiveProgress", $"{i + 1}");
                        }        
                       
                    }
                }
            }
            return result;  

        }

        public async Task<IEnumerable<string>> GetTableAsync(ImportModel model)
        {
            using (var connection = new SqlConnection(model.ConnectionStrings))
            {
                await connection.OpenAsync();
                var table = await connection.QueryAsync<string>("select table_name from information_schema.tables where table_type = 'base table'");
                var listTable = table.ToString(); 
                return table; 
            }
        }

      
    }
}

// Data Source=MSI\SQLEXPRESS01;Initial Catalog=ImportExcelDapper;Integrated Security=True; TrustServerCertificate=True
// Data Source=MSI\SQLEXPRESS01;Initial Catalog=ImportExcelDapper2;Integrated Security=True; TrustServerCertificate=True

// có nghĩa là 2 cái kết nối với nhau, vthằng db column sẽ là tên cột và thằng excelColumnIndex lấy đúng tên cột đó