using System.Data;
using System.Data.SQLite;

namespace QuanLyBanHang
{
	/// <summary>
	/// Lớp hỗ trợ thao tác trên CSDL SQLite
	/// </summary>
	public class SQLite
	{
		private SQLiteConnection connection = new SQLiteConnection();
		public static string SqlFile = "CSDL.sqlite";   //Tên CSDL
		public static string tb_DH = "DONHANG";
		public static string tb_HD = "HOADON";

		/// <summary>
		/// Tạo kết nối tới cơ sở dữ liệu
		/// </summary>
		public void CreateConnection()
		{
			string str = "Data Source=" + SqlFile + ";Version=3";
			connection.ConnectionString = str;
			connection.Open();
		}

		/// <summary>
		/// Đóng kết nối
		/// </summary>
		public void CloseConnection()
		{
			connection.Close();
		}

		/// <summary>
		/// Tạo một CSDL và tạo bảng HoaDon, DonHang vào CSDL đó
		/// </summary>
		public void CreateDatabase()
		{
			SQLiteConnection.CreateFile(SqlFile);
			string tb_HD = "CREATE TABLE IF NOT EXISTS " + SQLite.tb_HD + "(MAHD TEXT PRIMARY KEY, NGAY DATETIME, TONGTIEN INT, GIAMGIA FLOAT, THANHTIEN FLOAT, DUATRUOC FLOAT, CONLAI FLOAT)";
			string tb_DH = "CREATE TABLE IF NOT EXISTS " + SQLite.tb_DH + "([ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, MAHD TEXT, TEN TEXT, LOAI TEXT, DONGIA INT, SOLUONG INT, GHICHU TEXT, FOREIGN KEY(MAHD) REFERENCES HOADON(MAHD))";
			ExecuteCommand(tb_DH);
			ExecuteCommand(tb_HD);
		}

		/// <summary>
		/// Thực thi các câu lệnh SQLite
		/// </summary>
		/// <param name="sql">Câu lệnh DBCommand</param>
		public void ExecuteCommand(string sql)
		{
			CreateConnection();
			SQLiteCommand command = new SQLiteCommand(sql, connection);
			command.ExecuteNonQuery();
			CloseConnection();
		}

		/// <summary>
		/// Thêm Đơn hàng vào CSDL
		/// </summary>
		/// <param name="donHang">Đơn hàng cần thêm</param>
		public void InsertToDH(DonHang donHang)
		{
			string sql = string.Format("INSERT INTO " + tb_DH + " (MAHD, TEN, LOAI, DONGIA, SOLUONG, GHICHU) VALUES ('{0}', '{1}', '{2}', {3}, {4}, '{5}')", donHang.MãHĐ, donHang.Tên, donHang.Loại, donHang.ĐơnGiá, donHang.SốLượng, donHang.GhiChú);
			ExecuteCommand(sql);
		}

		/// <summary>
		/// Thêm Hoá đơn vào CSDL
		/// </summary>
		/// <param name="hoaDon">Hoá đơn cần thêm</param>
		public void InsertToHD(HoaDon hoaDon)
		{
			string sql = string.Format("INSERT INTO " + tb_HD + " (MAHD, NGAY, TONGTIEN, GIAMGIA, THANHTIEN, DUATRUOC, CONLAI) VALUES ('{0}', '{1}', {2}, {3}, {4}, {5}, {6}", hoaDon.MãHĐ, hoaDon.Ngày, hoaDon.TổngTiền, hoaDon.GiảmGiá, hoaDon.ThànhTiền, hoaDon.ĐưaTrước, hoaDon.CònLại);
			ExecuteCommand(sql);
		}

		public void UpdateHD(HoaDon hoaDon)
		{
			string sql = string.Format("UPDATE  " + tb_HD + " SET NGAY = '{0}', TONGTIEN = '{1}', GIAMGIA = {2}, THANHTIEN = {3}, DUATRUOC = {4}, CONLAI = {5} WHERE MAHD = '" + hoaDon.MãHĐ + "'", hoaDon.Ngày, hoaDon.TổngTiền, hoaDon.GiảmGiá, hoaDon.ThànhTiền, hoaDon.ĐưaTrước, hoaDon.CònLại);
			ExecuteCommand(sql);
		}

		public void UpdateDH(DonHang donHang)
		{
			string sql = string.Format("UPDATE " + tb_DH + " SET MAHD = '{0}', TEN = '{1}', LOAI = '{2}', DONGIA = {3}, SOLUONG = {4}, GHICHU = '{5} WHERE [ID] = {6}'", donHang.MãHĐ, donHang.Tên, donHang.Loại, donHang.ĐơnGiá, donHang.SốLượng, donHang.GhiChú, donHang.ID);
			ExecuteCommand(sql);
		}

		//public void UpdateValue()
		//{
		//	CreateConnection();
		//	string sql = "UPDATE " + tb_DH + " SET ... WHERE " + col_id + " = '0'";
		//	SQLiteCommand command = new SQLiteCommand(sql, connection);
		//	command.ExecuteNonQuery();
		//	CloseConnection();
		//}

		//public void DeleteValue()
		//{
		//	CreateConnection();
		//	string sql = "DELETE FROM " + tb_DH + " WHERE " + col_id + " = '0'";
		//	SQLiteCommand command = new SQLiteCommand(sql, connection);
		//	command.ExecuteNonQuery();
		//	CloseConnection();
		//}

		/// <summary>
		/// Trả về dữ liệu từ câu lệnh truy vấn
		/// </summary>
		/// <param name="query">Câu lệnh DBDataAdapter</param>
		/// <returns></returns>
		public DataSet GetValues(string query)
		{
			CreateConnection();
			DataSet dataSet = new DataSet();
			SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection);
			adapter.Fill(dataSet);
			CloseConnection();
			return dataSet;
		}

		/// <summary>
		/// Lấy mã hoá đơn khả dụng
		/// </summary>
		/// <returns>Mã hoá đơn</returns>
		public string GetBillID()
		{
			DataSet data = GetValues("SELECT * FROM " + tb_HD);
			var table = data.Tables[0];
			int count = table.Rows.Count;
			if (count == 0)
				return HoaDon.TaoMaHD(1);
		}
	}
}
