namespace SV20T1020544.BusinessLayers
{
    /// <summary>
    /// Khởi tạo, lưu trữ các thông tin cấu hình của BusinessLayers
    /// </summary>
    public static class Configuration
    {
        /// <summary>
        /// Chuỗi kết thông số kết nối đến CSDL
        /// </summary>
        public static string ConnectionString { get; private set; } = "";

        /// <summary>
        /// Khởi tạo cấu hình cho BusinessLayers
        /// (Hàm này phải được gọi trước khi ứng dụng chạy)
        /// </summary>
        /// <param name="connectionString"></param>
        public static void Initialize(string connectionString)
        {
            Configuration.ConnectionString = connectionString;
        }
    }
}
//Câu hỏi: static class là gì ? Khác với class thông thường chỗ nào ? 
