using System;

namespace QuanLyBanHang
{
	/// <summary>
	/// Bảng Bảng giá
	/// </summary>
	public class BangGia
	{
		public string Tên { get; set; }
		public string Loại { get; set; }
		public int ĐơnGiá { get; set; }

		public BangGia()
		{

		}

		public BangGia(string tên, string loại, int đơnGiá)
		{
			Tên = tên;
			Loại = loại;
			ĐơnGiá = đơnGiá;
		}
	}

	/// <summary>
	/// Bảng Đơn hàng
	/// </summary>
	public class DonHang
	{
		public int ID { get; set; }
		public string MãHĐ { get; set; }
		public string Tên { get; set; }
		public string Loại { get; set; }
		public int ĐơnGiá { get; set; }
		public int SốLượng { get; set; }
		public string GhiChú { get; set; }

		public DonHang()
		{

		}

		/// <summary>
		/// Chuyển Bảng giá thành Đơn hàng
		/// </summary>
		/// <param name="bangGia"></param>
		public DonHang(BangGia bangGia)
		{
			this.Tên = bangGia.Tên;
			this.Loại = bangGia.Loại;
			this.ĐơnGiá = bangGia.ĐơnGiá;
			this.SốLượng = 1;
		}
	}

	/// <summary>
	/// Bảng Hoá đơn
	/// </summary>
	public class HoaDon
	{
		public string MãHĐ { get; set; } //HDddMMyyxxx, trong đó dd: ngày, MM: tháng, yy: năm, xxx: số thứ tự
		public string Ngày { get; set; }
		public double TổngTiền { get; set; }
		public double GiảmGiá { get; set; }
		public double ThànhTiền { get; set; }
		public double ĐưaTrước { get; set; }
		public double CònLại { get; set; }

		public HoaDon()
		{

		}

		/// <summary>
		/// Tạo mã hoá đơn dựa vào số thứ tự
		/// </summary>
		/// <param name="stt">Số thứ tự cần tạo mã hoá đơn</param>
		/// <returns>ddMMyyxxx với dd: ngày, MM: tháng, yy: năm, xxx là số thứ tự</returns>
		public static string TaoMaHD(int stt)
		{
			DateTime now = DateTime.Now;
			return now.ToString("ddMMyy") + stt.ToString("000");
		}

		public HoaDon(string mãHĐ, string ngày, double tổngTiền, double giảmGiá, double thànhTiền, double đưaTrước, double cònLại)
		{
			MãHĐ = mãHĐ;
			Ngày = ngày;
			TổngTiền = tổngTiền;
			GiảmGiá = giảmGiá;
			ThànhTiền = thànhTiền;
			ĐưaTrước = đưaTrước;
			CònLại = cònLại;
		}
	}
}
