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
			string str = "Data Source=" + SqlFile + ";Version=3;datetimeformat=CurrentCulture";
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
			string tb_HD = "CREATE TABLE IF NOT EXISTS " + SQLite.tb_HD + "(MãHĐ TEXT PRIMARY KEY, Ngày DATETIME, TổngTiền INT, GiảmGiá FLOAT, ThànhTiền FLOAT, ĐưaTrước FLOAT, CònLại FLOAT)";
			string tb_DH = "CREATE TABLE IF NOT EXISTS " + SQLite.tb_DH + "([ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, MãHĐ TEXT, Tên TEXT, Loại TEXT, ĐơnGiá INT, SốLượng INT, GhiChú TEXT, FOREIGN KEY(MãHĐ) REFERENCES HOADON(MãHĐ))";
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
			string sql = string.Format("INSERT INTO " + tb_DH + " (MãHĐ, Tên, Loại, ĐơnGiá, SốLượng, GhiChú) VALUES ('{0}', '{1}', '{2}', {3}, {4}, '{5}')", donHang.MãHĐ, donHang.Tên, donHang.Loại, donHang.ĐơnGiá, donHang.SốLượng, donHang.GhiChú);
			ExecuteCommand(sql);
		}

		/// <summary>
		/// Thêm Hoá đơn vào CSDL
		/// </summary>
		/// <param name="hoaDon">Hoá đơn cần thêm</param>
		public void InsertToHD(HoaDon hoaDon)
		{
			string sql = string.Format("INSERT INTO " + tb_HD + " (MãHĐ, Ngày, TổngTiền, GiảmGiá, ThànhTiền, ĐưaTrước, CònLại) VALUES ('{0}', '{1}', {2}, {3}, {4}, {5}, {6})", hoaDon.MãHĐ, hoaDon.Ngày, hoaDon.TổngTiền, hoaDon.GiảmGiá, hoaDon.ThànhTiền, hoaDon.ĐưaTrước, hoaDon.CònLại);
			ExecuteCommand(sql);
		}

		/// <summary>
		/// Cập nhật hoá đơn trong CSDL
		/// </summary>
		/// <param name="hoaDon">Hoá đơn cần cập nhật</param>
		public void UpdateHD(HoaDon hoaDon)
		{
			string sql = string.Format("UPDATE  " + tb_HD + " SET Ngày = '{0}', TổngTiền = '{1}', GiảmGiá = {2}, ThànhTiền = {3}, ĐưaTrước = {4}, CònLại = {5} WHERE MãHĐ = '" + hoaDon.MãHĐ + "'", hoaDon.Ngày, hoaDon.TổngTiền, hoaDon.GiảmGiá, hoaDon.ThànhTiền, hoaDon.ĐưaTrước, hoaDon.CònLại);
			ExecuteCommand(sql);
		}

		/// <summary>
		/// Cập nhật đơn hàng trong CSDL
		/// </summary>
		/// <param name="donHang">Đơn hàng cần cập nhật</param>
		public void UpdateDH(DonHang donHang)
		{
			string sql = string.Format("UPDATE " + tb_DH + " SET MãHĐ = '{0}', Tên = '{1}', Loại = '{2}', ĐơnGiá = {3}, SốLượng = {4}, GhiChú = '{5} WHERE [ID] = {6}'", donHang.MãHĐ, donHang.Tên, donHang.Loại, donHang.ĐơnGiá, donHang.SốLượng, donHang.GhiChú, donHang.ID);
			ExecuteCommand(sql);
		}

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
			return HoaDon.TaoMaHD(count + 1);
		}
	}
}
