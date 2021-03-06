Create Database QuanLyHang
Use QuanLyHang
-- Tài Khoản
Begin
	Create Table TaiKhoan -- Dùng đăng nhập sử dụng chương trình.
	(
	TenDangNhap nchar(20) Primary Key,
	MatKhau nchar(100)
	)
	-- Tạo Proc thêm Tài khoản
	Create Proc SP_ThemTK
		@TenDangNhap nchar(20),
		@MatKhau nchar(100)
	as Insert into TaiKhoan values (@TenDangNhap, @MatKhau)
	-- Tạo Proc sửa Tài khoản
	Create Proc SP_SuaTK
		@TenDangNhap nchar(20),
		@MatKhau nchar(100)
	as Update TaiKhoan Set MatKhau = @MatKhau where TenDangNhap = @TenDangNhap
	-- Tạo Proc xoá Tài khoản	1. Xoá Lịch Sử Tài Khoản 2. Xoá Tài Khoản
	Create Proc SP_XoaTK
		@TenDangNhap nchar(20)
	as	Delete LsTaiKhoan Where TenDangNhap = @TenDangNhap
		Delete TaiKhoan Where TenDangNhap = @TenDangNhap
End

Create Table LsTaiKhoan
(
	MaLS int Identity(1,1),
	TenDangNhap nchar(20) foreign key references TaiKhoan,
	SignIn datetime,
	SignOut datetime
	Constraint PK_LS primary key (MaLS, TenDangNhap)
)
--Khách
Begin
	Create Table Khach -- Bao gồm Người mua hàng và Người bán hàng.
	(
	MaKhach nchar(10) Primary Key,
	TenKhach nvarchar(30),
	SDT nchar(30), -- Lưu nhiều số đt (ĐT bàn, ĐT di động 10-11 số).
	DiaChi nvarchar(100),
	GhiChu nvarchar(1000)
	)
	-- Tạo Proc thêm Khách
	Create Proc SP_ThemKhach
		@MaKhach nchar(10),
		@TenKhach nvarchar(30),
		@SDT nchar(30),
		@DiaChi nvarchar(100),
		@GhiChu nvarchar(1000)
	as Insert into Khach values (@MaKhach, @TenKhach, @SDT, @DiaChi, @GhiChu)
	-- Tạo Proc chỉnh sửa thông tin Khách
	Create Proc SP_SuaKhach
		@MaKhach nchar(10),
		@TenKhach nvarchar(30),
		@SDT nchar(30),
		@DiaChi nvarchar(100),
		@GhiChu nvarchar(1000)
	as
		Update Khach Set TenKhach=@TenKhach, SDT=@SDT, DiaChi=@DiaChi, GhiChu=@GhiChu where MaKhach=@MaKhach
	-- Tạo Proc xoá Khách	1. Cập nhật Mã Khách ở Nhập Xuất = null và Tên Khách vào Ghi Chú trong Nhập Xuất hàng.
	Create Proc SP_XoaKhach	
		@MaKhach nchar(10)
	as	
		Declare @TenKhach nvarchar(30) = (select TenKhach from Khach where MaKhach = @MaKhach)
		Update NhapXuat set MaKhach= Null, GhiChu = GhiChu + CHAR(13) + CHAR(10) + N'Tên Khách: ' + @TenKhach where MaKhach = @MaKhach
		Delete Khach Where MaKhach = @MaKhach
	-- Tạo Proc Tìm kiếm
	Create Proc SP_TimKhach
		@CachTim nchar(10),
		@TuTim nvarchar(100)
	as
		if(@CachTim = 'makhach')
			Select MaKhach, TenKhach, SDT, DiaChi, GhiChu From Khach 
				Where MaKhach like '%' + @TuTim + '%' Order by MaKhach 
		if(@CachTim = 'tenkhach')
			Select MaKhach, TenKhach, SDT, DiaChi, GhiChu From Khach 
				Where TenKhach like N'%' + @TuTim + '%' Order by MaKhach
		if(@CachTim = 'sdt')
			Select MaKhach, TenKhach, SDT, DiaChi, GhiChu From Khach 
				Where SDT like '%' + @TuTim + '%' Order by MaKhach
		if(@CachTim = 'diachi')
			Select MaKhach, TenKhach, SDT, DiaChi, GhiChu From Khach 
				Where DiaChi like N'%' + @TuTim + '%' Order by MaKhach
		if(@CachTim = 'ghichu')
			Select MaKhach, TenKhach, SDT, DiaChi, GhiChu From Khach 
				Where GhiChu like N'%' + @TuTim + '%' Order by MaKhach
End	
-- Hàng
Begin
	Create Table Hang
	(
	MaHang nchar(10) Primary Key,
	TenHang nvarchar(100),
	SoLuong int,
	DonViTinh nvarchar(20),
	DonGiaNhap float,
	DonGiaXuat float,
	GhiChu nvarchar(1000)
	)
	-- Tạo Proc thêm Hàng
	Create Proc SP_ThemHang
		@MaHang nchar(10),
		@TenHang nvarchar(100),
		@SoLuong int,
		@DonViTinh nvarchar(20),
		@DonGiaNhap float,
		@DonGiaXuat float,
		@GhiChu nvarchar(1000)
	as 
		Declare @GhiChuMoi nvarchar(1000) = @GhiChu + char(13)+ char(10)+ CONVERT(VARCHAR(10), GETDATE(), 103) 
											+ N' Tạo hàng có số lượng: ' + Convert(char, @SoLuong)
		Insert into Hang Values (@MaHang, @TenHang, @SoLuong, @DonViTinh, @DonGiaNhap, @DonGiaXuat, @GhiChuMoi)
	-- Tạo Proc chỉnh sửa Hàng	1.Cập nhật các thông tin về Tên Hàng và Số Lượng và Giá Nhập và Giá Xuất nếu có thay đổi vào Ghi Chú
	Create Proc SP_SuaHang
		@MaHang nchar(10),
		@TenHang nvarchar(100),
		@SoLuong int,
		@DonViTinh nvarchar(20),
		@DonGiaNhap float,
		@DonGiaXuat float,
		@GhiChu nvarchar(1000)
	as
		Declare @TenHangCu nvarchar (100) = (select TenHang from Hang where MaHang = @MaHang)
		Declare @SoLuongCu int  = (select SoLuong from Hang where MaHang = @MaHang)
		Declare @GiaNhapCu float = (select DonGiaNhap from Hang where MaHang = @MaHang)
		Declare @GiaXuatCu float = (select DonGiaXuat from Hang where MaHang = @MaHang)
		Declare @GhiChuMoi nvarchar(1000) = @GhiChu
		If ((@TenHang not like @TenHangCu) or (@SoLuong != @SoLuongCu) or (@DonGiaNhap != @GiaNhapCu) or (@DonGiaXuat != @GiaXuatCu))
			set @GhiChuMoi = @GhiChuMoi + char(13) + char(10) + CONVERT(VARCHAR(10), GETDATE(), 103)
		If (@TenHang not like @TenHangCu)
			set @GhiChuMoi = @GhiChuMoi + ' ' + N' Tên hàng: ' +  @TenHangCu + '=>' + @TenHang
		If (@SoLuong != @SoLuongCu) 
			set @GhiChuMoi = @GhiChuMoi + ' ' + N' Số lượng: ' +  LTRIM(STR(@SoLuongCu)) + '=>' + LTRIM(STR(@SoLuong))
		If (@DonGiaNhap != @GiaNhapCu) 
			set @GhiChuMoi = @GhiChuMoi + ' ' + N' Giá nhập: ' + LTRIM(STR(@GiaNhapCu)) + '=>' + LTRIM(STR(@DonGiaNhap))
		If (@DonGiaXuat != @GiaXuatCu) 
			set @GhiChuMoi = @GhiChuMoi + ' ' + N' Giá xuất: ' + LTRIM(STR(@GiaXuatCu)) + '=>' + LTRIM(STR(@DonGiaXuat))
		Update Hang Set TenHang=@TenHang, SoLuong=@SoLuong, DonViTinh=@DonViTinh, 
				DonGiaNhap=@DonGiaNhap, DonGiaXuat=@DonGiaXuat, GhiChu = @GhiChuMoi Where MaHang = @MaHang
	-- Tạo Proc xoá Hàng	1.Cập nhật Tên hàng, Số Lượng, Đơn Giá đã nhập hoặc xuất của Chi Tiết NX vào Ghi Chú Nhập Xuất
		--2. Xoá Chi Tiết NX. 3. Xoá Hàng
	Create Proc SP_XoaHang
		@MaHang nchar(10)
	as	
		Delete ChiTiet where MaHang =@MaHang
		Delete Hang Where MaHang = @MaHang
	-- Tạo Proc tìm Hàng
	Create Proc SP_TimHang
		@CachTim nchar(10),
		@TuTim nvarchar(100)
	as
		if(@CachTim = 'mahang')
			Select MaHang, TenHang, SoLuong, DonViTinh, DonGiaNhap, DonGiaXuat, GhiChu From Hang 
				Where MaHang like '%' + @TuTim + '%' Order by MaHang
		if(@CachTim = 'tenhang')
			Select MaHang, TenHang, SoLuong, DonViTinh, DonGiaNhap, DonGiaXuat, GhiChu From Hang 
				Where TenHang like N'%' + @TuTim + '%' Order by MaHang
		if(@CachTim = 'soluong')
			Select MaHang, TenHang, SoLuong, DonViTinh, DonGiaNhap, DonGiaXuat, GhiChu From Hang 
				Where SoLuong like '%' + @TuTim + '%' Order by MaHang
		if(@CachTim = 'donvitinh')
			Select MaHang, TenHang, SoLuong, DonViTinh, DonGiaNhap, DonGiaXuat, GhiChu From Hang 
				Where DonViTinh like '%' + @TuTim + '%' Order by MaHang
		if(@CachTim = 'dongianhap')
			Select MaHang, TenHang, SoLuong, DonViTinh, DonGiaNhap, DonGiaXuat, GhiChu From Hang 
				Where DonGiaNhap like '%' + @TuTim + '%' Order by MaHang
		if(@CachTim = 'dongiaxuat')
			Select MaHang, TenHang, SoLuong, DonViTinh, DonGiaNhap, DonGiaXuat, GhiChu From Hang 
				Where DonGiaXuat like '%' + @TuTim + '%' Order by MaHang
		if(@CachTim = 'ghichu')
			Select MaHang, TenHang, SoLuong, DonViTinh, DonGiaNhap, DonGiaXuat, GhiChu From Hang 
				Where GhiChu like N'%' + @TuTim + '%' Order by MaHang
End
-- Nhập, Xuất Hàng
Begin
	Create Table NhapXuat
	(
	MaNX nchar(15) Primary Key,-- Phân luôn loại nhập xuất bằng ký tự đầu: Nhập-N, Xuất-X
	MaKhach nchar(10) Foreign Key References Khach,
	Ngay date,
	GhiChu nvarchar(1000)
	)
	-- Tạo Proc Thêm Nhập Xuất
	Create Proc SP_ThemNX
		@MaNX nchar(15),
		@MaKhach nchar(10),
		@Ngay date,
		@GhiChu nvarchar(1000)
	as Insert into NhapXuat values (@MaNX, @MaKhach, @Ngay, @GhiChu)
	-- Tạo Proc Chỉnh sửa Nhập Xuất
	Create Proc SP_SuaNX
		@MaNX nchar(15),
		@MaKhach nchar(10),
		@Ngay date,
		@GhiChu nvarchar(1000)
	as	Update NhapXuat set MaKhach=@MaKhach, Ngay = @Ngay, GhiChu = @GhiChu Where MaNX = @MaNX
	-- Tạo Proc Xoá Nhập Xuất
	Create Proc SP_XoaNX
		@MaNX nchar(10)
	as Delete NhapXuat Where MaNX = @MaNX
	-- Tạo Proc Tìm Nhập Xuất
	Create Proc SP_TimNX
		@CachTim nchar(10),
		@TuTim nvarchar(100)
	as
		if(@CachTim='manx')
			Select MaNX, MaKhach, Ngay, GhiChu From NhapXuat 
				Where MaNX like '%' + @TuTim + '%' Order by MaNX
		if(@CachTim='makhach')
			Select MaNX, MaKhach, Ngay, GhiChu From NhapXuat 
				Where MaKhach like '%' + @TuTim + '%' Order by MaNX
		if(@CachTim='ngay') -- Tìm theo ngày chính xác.
			Select MaNX, MaKhach, Ngay, GhiChu From NhapXuat 
				Where day(Ngay) = LEFT(@TuTim, 2) and month(Ngay) = SUBSTRING(@TuTim,4,2) 
						and Year(Ngay) = RIGHT(@TuTim, 4) Order by MaNX
		if(@CachTim='ghichu')
			Select MaNX, MaKhach, Ngay, GhiChu From NhapXuat 
				Where GhiChu like N'%' + @TuTim + '%' Order by MaNX
	-- Tạo Proc tìm Nhập hàng giữa ngày này và ngày khác
	Create Proc SP_TimNXTheoNgay
		@TuNgay nvarchar(10),
		@DenNgay nvarchar(10)
	as
		Select MaNX, MaKhach, Ngay, GhiChu from NhapXuat Where Convert(nvarchar(10), Ngay, 103) 
		Between @TuNgay and @DenNgay Order By MaNX
End
-- Chi tiết Nhập Xuất
Begin
	Create Table ChiTiet
	(
	MaNX nchar(15) Foreign Key References NhapXuat,
	MaHang nchar(10) Foreign Key References Hang,
	SoLuong int,
	DonGia float
	Constraint PK_CT Primary Key (MaNX, MaHang)
	)
	-- Tạo Proc thêm Chi tiết
	Create Proc SP_ThemCT
		@MaNX nchar(15),
		@MaHang nchar(10),
		@SoLuong int,
		@DonGia float
	as
		Declare @soluongton int = (select soluong from hang where MaHang = @MaHang)
		Insert Into ChiTiet values (@MaNX, @MaHang, @SoLuong, @DonGia)
			-- Cập nhật Số lượng ở Hàng nếu là N thì thêm vào còn xuất thì trừ đi.
		if(LEFT(@MaNX,1) = 'N')
			Update Hang set SoLuong = (@soluongton + @SoLuong) Where MaHang = @MaHang
		else 
			Update Hang set SoLuong = (@soluongton - @SoLuong) Where MaHang = @MaHang
	-- Tạo Proc Sửa Chi tiết 
	Create Proc SP_SuaCT
		@MaNX nchar(15),
		@MaHang nchar(10),
		@SoLuong int,
		@DonGia float
	as	Declare @SoLuongTon int = (Select SoLuong From Hang Where MaHang = @MaHang)
		Declare @SoLuongCu int = (Select SoLuong From ChiTiet Where MaNX = @MaNX and MaHang = @MaHang)
		Update ChiTiet Set SoLuong = @SoLuong, DonGia = @DonGia Where MaNX = @MaNX and MaHang = @MaHang
		If(LEFT(@MaNX, 1) = 'N')
			Update Hang Set SoLuong = @SoLuongTon - @SoLuongCu + @SoLuong Where MaHang = @MaHang
		Else
			Update Hang Set SoLuong = @SoLuongTon + @SoLuongCu - @SoLuong Where MaHang = @MaHang
	--Tạo Proc Xoá Chi tiết
	Create Proc SP_XoaCT
		@MaNX nchar(15),
		@MaHang nchar(10)
	as	
		Declare @SoLuong int = (Select SoLuong From ChiTiet Where MaNX=@MaNX and MaHang =@MaHang)
		--Cập nhật số lượng ở hàng.
		if(LEFT(@MaNX,1)='N') Update Hang Set SoLuong = SoLuong - @SoLuong Where MaHang = @MaHang
		else Update Hang Set SoLuong = SoLuong + @SoLuong Where MaHang = @MaHang
		Delete ChiTiet Where MaNX =@MaNX and MaHang = @MaHang
		--Xoá NX nếu không còn Chi tiết
		if((Select COUNT(MaHang) From ChiTiet Where MaNX= @MaNX) = 0)
			Delete NhapXuat Where MaNX=@MaNX
		
	-- Tạo Proc Tìm kiếm Chi tiết
	Create Proc SP_TimCT
		@CachTim nchar(10),
		@TuTim nvarchar(100)
	as
		if(@CachTim='manx')
			Select MaNX, MaHang, Soluong, DonGia From ChiTiet
			Where MaNX like '%' + @TuTim + '%' Order by MaNX
		if(@CachTim='mahang')
			Select MaNX, MaHang, Soluong, DonGia From ChiTiet
			Where MaHang like '%' + @TuTim + '%' Order by MaNX
End


Create Proc SP_XemNhapXuat
	as
	Select NhapXuat.MaNX, Khach.MaKhach, TenKhach, SDT, DiaChi, Ngay, NhapXuat.GhiChu, TongTien = SUM(SoLuong*DonGia)
	From NhapXuat left join Khach on Khach.MaKhach = NhapXuat.MaKhach left join ChiTiet on NhapXuat.MaNX = ChiTiet.MaNX
	Group By NhapXuat.MaNX, Khach.MaKhach, TenKhach, SDT, DiaChi, Ngay, NhapXuat.GhiChu

Create Proc SP_XemCTNX
	@manx nchar(15)
	as
	Select ROW_NUMBER() over (order by Hang.MaHang desc) STT, Hang.MaHang, TenHang, ChiTiet.SoLuong, ChiTiet.DonGia, ThanhTien = ChiTiet.SoLuong * DonGia
	From Hang, ChiTiet
	Where Hang.MaHang = ChiTiet.MaHang and MaNX = @manx
	
Create Proc SP_XemKhachGiaoDich
	@makhach nchar(10),
	@loaiphieu nchar(1)
	as
	if((Select count(MaKhach) From NhapXuat Where MaKhach = @makhach and LEFT(NhapXuat.MaNX, 1)= 'N') > 0)
	begin
		Select Khach.MaKhach, TenKhach, SDT, DiaChi, Khach.GhiChu as GhiChuKhach,
			NhapXuat.MaNX, Ngay, NhapXuat.GhiChu as GhiChuNX
		From Khach inner join NhapXuat on Khach.MaKhach = NhapXuat.MaKhach
		Where Khach.MaKhach = @makhach and LEFT(NhapXuat.MaNX, 1)=@loaiphieu
	end
	else
	begin
		Select MaKhach, TenKhach, SDT, DiaChi, Khach.GhiChu as GhiChuKhach, 
		null as MaNX, null as Ngay,null as GhiChuNX 
		From Khach Where MaKhach = @MaKhach
	end

Create Proc SP_XemHangNX
	@mahang nchar(10),
	@ngaybd nchar(10),
	@ngaykt nchar(10)
	as
	if(@ngaybd is null and @ngaykt is null )
	begin
		Select Hang.MaHang, TenHang, Hang.SoLuong as SoLuongTon, DonViTinh, DonGiaNhap, DonGiaXuat, Hang.GhiChu,
			NhapXuat.MaNX, Ngay, ChiTiet.SoLuong as SoLuongNX, DonGia
		From Hang, NhapXuat, ChiTiet
		Where Hang.MaHang = ChiTiet.MaHang and ChiTiet.MaNX = NhapXuat.MaNX and Hang.MaHang = @mahang
		Order by Ngay
	end
	else 
	begin
		Select Hang.MaHang, TenHang, Hang.SoLuong as SoLuongTon, DonViTinh, DonGiaNhap, DonGiaXuat, Hang.GhiChu,
			NhapXuat.MaNX, Ngay, ChiTiet.SoLuong as SoLuongNX, DonGia
		From Hang, NhapXuat, ChiTiet
		Where Hang.MaHang = ChiTiet.MaHang and ChiTiet.MaNX = NhapXuat.MaNX and Hang.MaHang = @mahang
			and Ngay between @ngaybd and  @ngaykt
		Order by Ngay
	end
