Create database QuanLy
use QuanLy

Create table PhongBan(
	IdPB int primary key not null identity (1,1),
	TenPhong nvarchar (50),
	SDT varchar (10),
);
Create table ChucVu (
	IdCV int primary key not null identity (1,1),
	TenCV nvarchar (20),
);

Create table NhanVien (
	IdNV int primary key not null identity (1,1),
	HoTen nvarchar (50),
	Email varchar (100),
	Password varchar (100),
	IsAdmin bit default 0,
	SDT varchar (10),
	GioiTinh nvarchar (3),
	NgaySinh date,
	QueQuan nvarchar (200),
	DanToc nvarchar (20),
	ChuyenNganh nvarchar (50),
	IdPB int,
	IdCV int,
	LuongCB float,
	CONSTRAINT fk_PhongBan foreign key (IdPB) references PhongBan (IdPB),
	CONSTRAINT fk_ChucVu foreign key (IdCV) references ChucVu (IdCV),
);
Create table TTChamCong (
	IdCC int primary key not null identity (1,1),
	IdNV int,
	NgayCC date,
	TVao time,
	TRa time,
	ViPham bit default 0,
	CONSTRAINT fk_NhanVienCC foreign key (IdNV) references NhanVien (IdNV), 
);
Create table HopDong (
	IdHD int primary key not null identity (1,1),
	IdNV int,
	TenHD nvarchar (100),
	TuNgay date,
	DenNgay date,
	CONSTRAINT fk_NhanVienHD foreign key (IdNV) references NhanVien (IdNV),
);
/*
drop table HopDong
drop table TTChamCong
drop table NhanVien
drop table PhongBan
drop table ChucVu
*/
/*
create proc sp_TongNgayCong(
		@IdNV int,
		@ThangCong int
)
as
SELECT COUNT(*) AS NgayCong FROM ChamCong where MONTH(NgayChamCong)= @ThangCong and IdNV = @IdNV GROUP BY MONTH(NgayChamCong)

create proc sp_TinhLuong (
	@IdNV int ,
	@ThangCong int
)
as
begin 
	declare @Luong float ,@NgayCong int
	set @NgayCong = (SELECT COUNT(*) AS NgayCong FROM ChamCong where MONTH(NgayChamCong)= @ThangCong and IdNV = @IdNV GROUP BY MONTH(NgayChamCong))
	set @Luong = (select LuongCB/26 from NhanVien where IdNV = @IdNV) * @NgayCong
	select Round(@Luong,3) 
end

-- dữ liệu đăng nhập
--1	Nguyễn Văn An	nguyenvanan1@gmail.com	8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92	True	0987564891	Nam	2002-08-04	Thành Phố Hồ Chí Minh	Kinh	Công Nghệ Thông Tin 	1	1	Đại Học	10000000
*/
--Phòng Ban
insert into PhongBan(TenPhong,SDT) values (N'Phòng Nhân Sự','18005291');
insert into PhongBan(TenPhong,SDT) values (N'Phòng Kế Toán','18005292');
insert into PhongBan(TenPhong,SDT) values (N'Phòng Kinh Doanh','18005293');
insert into PhongBan(TenPhong,SDT) values (N'Phòng Kỹ Thuật','18005294');
insert into PhongBan(TenPhong,SDT) values (N'Phòng Hành Chính','18005295');
insert into PhongBan(TenPhong,SDT) values (N'Phòng IT','18005296');
--Chức Vụ
insert into ChucVu(TenCV) values (N'Nhân Viên');
insert into ChucVu(TenCV) values (N'Trưởng Nhóm');
insert into ChucVu(TenCV) values (N'Giám Sát');
insert into ChucVu(TenCV) values (N'Chuyên Viên');
insert into ChucVu(TenCV) values (N'Trưởng Phòng');

 --Nhân Viên
 insert into NhanVien values (N'Nguyễn Văn An','nguyenvanan@gmail.com','123456',1,'0975463335',1,'1990-05-09',N'Hồ Chí Minh',N'Kinh',N'Quản Trị Kinh Doah',1,5,16000);
 insert into NhanVien values (N'Trần Thanh Vân','tranthanhvan@gmail.com','8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92',0,'0335467862',0,'1995-03-25',N'Long An',N'Kinh',N'Công Nghệ Thông Tin',1,1,5000);
 insert into NhanVien values (N'Bùi Ngọc Trân','buingoctran@gmail.com','8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92',0,'0755456824',0,'2000-06-24',N'Ninh Thuận',N'Kinh',N'Công Nghệ Thông Tin',1,1,5000);
 
 -- Thông Tin Chấm Công
insert into TTChamCong(IdNV,NgayCC,TVao,TRa) values (1,'2023-05-03','07:50:00','17:00:00');
insert into TTChamCong(IdNV,NgayCC,TVao,TRa) values (2,'2023-05-03','07:50:00','17:00:00');
insert into TTChamCong(IdNV,NgayCC,TVao,TRa) values (3,'2023-05-03','07:50:00','17:00:00');

insert into TTChamCong(IdNV,NgayCC,TVao,TRa) values (1,'2023-05-04','07:50:00','17:00:00');
insert into TTChamCong(IdNV,NgayCC,TVao,TRa) values (2,'2023-05-04','07:50:00','17:00:00');
insert into TTChamCong(IdNV,NgayCC,TVao,TRa) values (3,'2023-05-04','08:05:00','17:00:00');

insert into TTChamCong(IdNV,NgayCC,TVao,TRa) values (1,'2023-05-05','07:50:00','17:00:00');
insert into TTChamCong(IdNV,NgayCC,TVao,TRa) values (2,'2023-05-05','07:50:00','17:00:00');
insert into TTChamCong(IdNV,NgayCC,TVao,TRa) values (3,'2023-05-05','07:50:00','17:00:00');

insert into TTChamCong(IdNV,NgayCC,TVao,TRa) values (1,'2023-05-06','07:50:00','17:00:00');
insert into TTChamCong(IdNV,NgayCC,TVao,TRa) values (2,'2023-05-06','07:50:00','17:00:00');
insert into TTChamCong(IdNV,NgayCC,TVao,TRa) values (3,'2023-05-06','07:50:00','17:00:00');

insert into TTChamCong(IdNV,NgayCC,TVao,TRa) values (1,'2023-05-08','07:50:00','17:00:00');
insert into TTChamCong(IdNV,NgayCC,TVao,TRa) values (2,'2023-05-08','07:50:00','17:00:00');
insert into TTChamCong(IdNV,NgayCC,TVao,TRa) values (3,'2023-05-08','07:50:00','17:00:00');

insert into TTChamCong(IdNV,NgayCC,TVao,TRa) values (1,'2023-05-09','07:50:00','17:00:00');
insert into TTChamCong(IdNV,NgayCC,TVao,TRa) values (2,'2023-05-09','07:50:00','17:00:00');
insert into TTChamCong(IdNV,NgayCC,TVao,TRa) values (3,'2023-05-09','07:50:00','17:00:00');

insert into TTChamCong(IdNV,NgayCC,TVao,TRa) values (1,'2023-05-10','08:05:00','17:00:00');
insert into TTChamCong(IdNV,NgayCC,TVao,TRa) values (2,'2023-05-10','07:50:00','17:00:00');
insert into TTChamCong(IdNV,NgayCC,TVao,TRa) values (3,'2023-05-10','07:50:00','16:45:00');





-- Thông Tin hợp đồng
insert into HopDong values (1,N'Hợp đồng 2 năm','2023-05-15','2025-05-15');

-- Tạo trigger set ViPham = 1 khi nhân viên có thời gian vào trễ hơn 8h hoặc thời gian ra sớm hơn 17h và khi ViPham = 0
Create Trigger tr_CC On TTChamCong
for insert,update
AS
	Begin
		Declare @TVao time ,@TRa time ,@ViPham bit, @IdCC int
		select @TVao =  TVao From inserted 
		select @TRa =  TRa From inserted 
		select @ViPham =  ViPham From inserted 
		select @IdCC =  IdCC From inserted 
		if (@TVao > '08:00:00' or @TRa < '17:00:00' )
		begin
			update TTChamCong
			set ViPham = 1
			where IdCC = @IdCC and @ViPham = 0
		end
		
	End
/*-- trigger 1 nhân viên chỉ có thể chấm công 1 ngày 1 lần (Không chấm công trùng lặp ngày)
CREATE TRIGGER tr_CC1 ON TTChamCong
FOR INSERT, UPDATE
AS
BEGIN
    IF (
        SELECT count(c.NgayCC)
        FROM inserted i,TTChamCong c
		WHERE i.NgayCC = c.NgayCC and i.IdNV = c.IdNV
		GROUP BY c.NgayCC
    ) > 0
    BEGIN
        RAISERROR('Nhân viên không được phép chấm công cùng 1 ngày', 16, 1);
        ROLLBACK TRAN; 
    END
END;


CREATE TRIGGER tr_CC1 ON TTChamCong
FOR INSERT, UPDATE
AS
BEGIN
	Declare @iI int ,@cI int ,@iN date,@cN date
	Select @iI = c.IdNV from inserted i, TTChamCong c where (i.IdNV = c.IdNV and i.NgayCC = c.NgayCC)
	Select @iN = c.NgayCC from inserted i, TTChamCong c where (i.IdNV = c.IdNV and i.NgayCC = c.NgayCC)

    IF NOT Exists (Select c.IdNV from inserted i, TTChamCong c where i.IdNV = c.IdNV and i.NgayCC = c.NgayCC)
    BEGIN
        --RAISERROR('Nhân viên không được phép chấm công cùng 1 ngày', 16, 1);
		PRINT(@iI)
		PRINT(@iN)
        ROLLBACK TRAN; 
    END      
	/*ELSE 
	BEGIN
	COMMIT TRAN;
	END*/
END;*/


--Proc Tính ngày công của nhân viên 
 create proc sp_TongNgayCong(
		@IdNV int,
		@ThangCong int
)
as
SELECT ISNULL(COUNT(NgayCC), 0) FROM TTChamCong WHERE MONTH(NgayCC) = @ThangCong AND IdNV = @IdNV

--Proc tính lương của nhân viên theo tháng
CREATE PROCEDURE sp_TinhLuong (
    @IdNV int,
    @ThangCong int
)
AS
BEGIN
    DECLARE @Luong float, @NgayCong int, @SoVPham int, @TienPC float, @LuongCB float, @TienBH float

    SELECT @NgayCong = ISNULL(COUNT(NgayCC), 0) 
    FROM TTChamCong 
    WHERE MONTH(NgayCC) = @ThangCong AND IdNV = @IdNV

    SELECT @SoVPham = ISNULL(SUM(CASE WHEN ViPham = 1 THEN 1 ELSE 0 END), 0) 
    FROM TTChamCong 
    WHERE MONTH(NgayCC) = @ThangCong AND IdNV = @IdNV

    SELECT @LuongCB = ISNULL((SELECT nv.LuongCB FROM NhanVien nv WHERE nv.IdNV = @IdNV), 0)

    SET @Luong = ((@LuongCB / 26) * @NgayCong - 30 * @SoVPham )

    SELECT ROUND(@Luong, 3)
END
-- Proc tổng số lần vi phạm của 1 nhân viên trong 1 tháng được nhập từ bàn phím
 create proc sp_TongViPham(
		@IdNV int,
		@ThangCong int
)
as
BEGIN
    SELECT ISNULL(SUM(CASE WHEN ViPham = 1 THEN 1 ELSE 0 END), 0) FROM TTChamCong WHERE MONTH(NgayCC) = @ThangCong AND IdNV = @IdNV
END
/*
-- Drop 
drop Trigger tr_CC1
drop proc sp_TongNgayCong
drop proc sp_TinhLuong
drop proc sp_TongViPham

-- Delete
delete from TTChamCong where NgayCC = '2023-11-06'

--Select 
SELECT * FROM HopDong
SELECT * FROM TTChamCong
SELECT * FROM NhanVien
SELECT * FROM PhongBan
SELECT * FROM ChucVu
SELECT * FROM sys.triggers

--drop
drop table HopDong
drop table TTChamCong
drop table NhanVien
drop table PhongBan
drop table ChucVu
*/
select * from TTChamCong