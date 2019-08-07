using System;

namespace QuanLyBanHang
{
	public class BangGia
	{
		public string Tên { get; set; }
		public string Loại { get; set; }
		public int ĐơnGiá { get; set; }
	}

	public class DonHang
	{
		public string MãHĐ { get; set; }
		public string Tên { get; set; }
		public string Loại { get; set; }
		public int ĐơnGiá { get; set; }
		public int SốLượng { get; set; }
		public string GhiChú { get; set; }

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

	public class HoaDon
	{
		public string MãHĐ { get; set; }
		public string Ngày { get; set; }
		public double TổngTiền { get; set; }
		public double GiảmGiá { get; set; }
		public double ThànhTiền { get; set; }
		public double ĐưaTrước { get; set; }
		public double CònLại { get; set; }
	}
}
