using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace QuanLyBanHang
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		SQLite SQLiteHelper;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void Window_ContentRendered(object sender, EventArgs e)
		{
			LoadBangGia();
			TaoCSDL();
		}

		#region Events

		private void PART_TITLEBAR_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			DragMove();
		}

		private void PART_CLOSE_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void PART_MAXIMIZE_RESTORE_Click(object sender, RoutedEventArgs e)
		{
			if (this.WindowState == WindowState.Normal)
			{
				this.WindowState = WindowState.Maximized;
			}
			else
			{
				this.WindowState = WindowState.Normal;
			}
		}

		private void PART_MINIMIZE_Click(object sender, RoutedEventArgs e)
		{
			this.WindowState = WindowState.Minimized;
		}

		private void Menu_Thoat_Click(object sender, RoutedEventArgs e)
		{
			Environment.Exit(1);
		}

		private void Menu_LoadBangGia_Click(object sender, RoutedEventArgs e)
		{
			var thread = new Thread(() =>
			{
				string excelPath = Excel.OpenFile();
				string dataPath = Directory.GetCurrentDirectory().ToString() + "\\data.dat";
				if (excelPath != null)
				{
					// Update the main thread using Dispatcher
					dgBangGia.Dispatcher.Invoke(() => dgBangGia.ItemsSource = Excel.ReadExcel(excelPath));
					if (!File.Exists(dataPath))
					{
						File.Create(dataPath);
					}
					StreamWriter streamWriter = new StreamWriter(dataPath);
					streamWriter.Write(excelPath);
					streamWriter.Close();
				}
			});
			thread.Start();
		}

		private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			double newWidth = e.NewSize.Width;
			leftPanel.Width = (int)(newWidth / 4);
			middlePanel.Width = (int)(newWidth / 4);
		}

		private void Btn_Them_Click(object sender, RoutedEventArgs e)
		{
			BangGia selected = dgBangGia.SelectedItem as BangGia;
			if (selected != null)
			{
				DonHang donHang = new DonHang(selected);
				donHang.MãHĐ = SQLiteHelper.GetBillID();
				dgDanhMucChon.Items.Add(donHang);
			}
		}

		private void Btn_Xoa_Click(object sender, RoutedEventArgs e)
		{
			DonHang selected = dgDanhMucChon.SelectedItem as DonHang;
			if (selected != null)
			{
				dgDanhMucChon.Items.Remove(selected);
			}
		}

		private void Btn_Down_Click(object sender, RoutedEventArgs e)
		{
			int soLuong = 0;
			if (int.TryParse(tbx_SoLuong.Text, out soLuong))
			{
				if (soLuong > 0)
					soLuong--;
			}
			tbx_SoLuong.Text = soLuong.ToString();

		}

		private void Btn_Up_Click(object sender, RoutedEventArgs e)
		{
			int soLuong = 0;
			if (int.TryParse(tbx_SoLuong.Text, out soLuong))
			{
				soLuong++;
			}
			tbx_SoLuong.Text = soLuong.ToString();
		}

		private void Tbx_SoLuong_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key < Key.D0 || e.Key > Key.D9)
			{
				e.Handled = true;
			}
		}

		private void Tbx_GiamGia_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key < Key.D0 || e.Key > Key.D9)
			{
				e.Handled = true;
			}
		}

		private void Btn_TinhTien_Click(object sender, RoutedEventArgs e)
		{
			var danhMucChon = dgDanhMucChon.Items;
			int tong = 0;
			foreach (DonHang item in danhMucChon)
			{
				tong += item.ĐơnGiá * item.SốLượng;
			}
			tbx_TongTien.Text = tong.ToString();
			Tbx_TongTien_TextChanged(sender, null);
		}

		private void Tbx_TongTien_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			int tongTien, giamGia = 0;
			bool tt = int.TryParse(tbx_TongTien.Text, out tongTien);    //Nếu tổng tiền có số
			int.TryParse(tbx_GiamGia.Text, out giamGia);      //Mặc định là giảm giá 0%
			if (tt)
			{
				tbx_ThanhTien.Text = "" + tongTien * (100 - giamGia) / 100;
			}
		}

		private void Tbx_DuaTruoc_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			int thanhTien = 0, duaTruoc;
			int.TryParse(tbx_ThanhTien.Text, out thanhTien);    //Mặc định là thành tiền bằng 0
			bool dt = int.TryParse(tbx_DuaTruoc.Text, out duaTruoc);
			if (dt)
			{
				tbx_ConLai.Text = "" + (duaTruoc - thanhTien);
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Load Bảng giá từ file excel mỗi khi khởi động ứng dụng
		/// </summary>
		private void LoadBangGia()
		{
			var thread = new Thread(() =>
			{
				string path = Directory.GetCurrentDirectory().ToString() + "\\data.dat";
				if (File.Exists(path))
				{
					StreamReader streamReader = new StreamReader(path);
					string data = streamReader.ReadLine();
					if (File.Exists(data))
					{
						dgBangGia.Dispatcher.Invoke(() => dgBangGia.ItemsSource = Excel.ReadExcel(data));
					}
					streamReader.Close();
				}
			});
			thread.Start();
		}

		/// <summary>
		/// Tạo cơ sở dữ liệu SQLite
		/// </summary>
		private void TaoCSDL()
		{
			SQLiteHelper = new SQLite();
			var thread = new Thread(() =>
			  {
				  if (!File.Exists(Directory.GetCurrentDirectory() + "\\" + SQLite.SqlFile))
				  {
					  SQLiteHelper.CreateDatabase();
				  }
			  });
			thread.Start();
		}

		#endregion

	}
}
