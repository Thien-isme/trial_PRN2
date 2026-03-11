using Q1_SE182117;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

internal class Program
{
    static async Task Main(string[] args)
    {
        while (true)
        {
            // 1. Nhập Reader ID
            Console.Write("Enter Reader ID (or press Enter to exit): ");
            string input = Console.ReadLine() ?? "";
            // Thoát nếu nhấn Enter trống
            if (string.IsNullOrEmpty(input)) break;
            // 2. Kiểm tra tính hợp lệ (Phải là số nguyên dương > 0)
            if (!int.TryParse(input, out int readerId) || readerId <= 0)
            {
                Console.WriteLine("Invalid input! Please enter a valid Reader ID (positive integer).\n");
                continue;
            }
            // 3. Kết nối tới Server
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    // Cố gắng kết nối tới 127.0.0.1:3000
                    await client.ConnectAsync("127.0.0.1", 3000);

                    using (NetworkStream stream = client.GetStream())
                    {
                        // Gửi Reader ID dưới dạng String
                        byte[] data = Encoding.UTF8.GetBytes(input);
                        await stream.WriteAsync(data, 0, data.Length);
                        // Nhận dữ liệu phản hồi
                        byte[] buffer = new byte[8192];
                        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                        string jsonResponse = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        // 4. Giải mã JSON và hiển thị
                        var records = JsonSerializer.Deserialize<List<BorrowRecord>>(jsonResponse);
                        if (records == null || records.Count == 0)
                        {
                            Console.WriteLine("No borrow history found for this reader.");
                        }
                        else
                        {
                            Console.WriteLine($"{"Book ID",-10} | {"Title",-25} | {"Author",-20} | {"Status",-10}");
                            Console.WriteLine(new string('-', 75));
                            foreach (var r in records)
                            {
                                Console.WriteLine($"{r.BookID,-10} | {r.Title,-25} | {r.Author,-20} | {r.Status,-10}");
                            }
                        }
                    }
                }
            }
            catch (SocketException)
            {
                // Lỗi này xảy ra khi Server chưa bật hoặc cổng bị chặn
                Console.WriteLine("Library server is not running. Please try again later.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            Console.WriteLine(); // Xuống dòng cho lần nhập tiếp theo
        }
    }
}
