create database QLKH
go
use QLKH
go
create table LoaiKH(
	MaLoaiKH int primary key,
	TenLoai nvarchar(20),
)
create table KhachHang(
	MaKH int primary key,
	Ten nvarchar(50),
	Gender nchar(5),
	Phone varchar(10),
	MaLoaiKH int references LoaiKH(MaLoaiKH)
)
insert into LoaiKH values('1',N'VIP')
insert into LoaiKH values('2',N'PC')
insert into LoaiKH values('3',N'CC')
insert into LoaiKH values('4',N'LC')

select * from LoaiKH
select * from KhachHang
delete KhachHang