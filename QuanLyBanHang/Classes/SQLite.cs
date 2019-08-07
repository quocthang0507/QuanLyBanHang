﻿using System.Data;
using System.Data.SQLite;

namespace QuanLyBanHang
{
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
		public void CreateTable()
		{
			SQLiteConnection.CreateFile(SqlFile);
			string tb_HD = "CREATE TABLE IF NOT EXISTS " + SQLite.tb_HD + "MAHD TEXT PRIMARY KEY, NGAY DATETIME, TONGTIEN INT, GIAMGIA FLOAT, THANHTIEN FLOAT, DUATRUOC FLOAT, CONLAI FLOAT";
			string tb_DH = "CREATE TABLE IF NOT EXISTS " + SQLite.tb_DH + "([ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, MAHD TEXT, TEN TEXT, LOAI TEXT, DONGIA INT, SOLUONG INT, GHICHU TEXT, FOREIGN KEY(MAHD) REFERENCES HOADON(MAHD)";
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

		//public void InsertValue()
		//{
		//	CreateConnection();
		//	string sql = "INSERT INTO " + tb_DH + " VALUES ";
		//	SQLiteCommand command = new SQLiteCommand(sql, connection);
		//	command.ExecuteNonQuery();
		//	CloseConnection();
		//}

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
	}
}