create database Topokki
use Topokki

use master
drop database Topokki

create table Role(
	ID int identity(1,1) primary key,
	Name nvarchar(20) not null,
)

create table Account(
	ID int identity(1,1) primary key,
	UserName nvarchar(255) unique not null,
	[Password] nvarchar(255) not null default '1',
	[RoleID] int not null,
	Phone nvarchar(12) not null unique,
	[Name] nvarchar(255) not null,
	foreign KEY (RoleID) references Role(ID)
)

create table TableFood(
	ID int identity(1,1) primary key,
	Name Nvarchar(100),
	Status nvarchar(100) not null default N'Trống'
)

create table Category
(
	ID int identity(1,1) primary key,
	[Name] nvarchar(255) not null,
);

create table [Product]
(
	ID int identity(1,1) primary key,
	[Name] nvarchar(255) not null,
	Price decimal(18,2) not null default 0,
	CategoryID int,
	foreign KEY (categoryID) references Category(ID)
);

create table [Order]
(
	ID int identity(1,1) primary key,
	DateCheckIn Datetime not null default getdate(),
	DateCheckOut Datetime null,
	TableID int not null,
	Status int not null default 0, --1: da thanh toan; 0: chua thanh toan
	TotalPrice decimal(18,2),
	foreign key (TableID) references TableFood(ID)
);

create table OrderDetail
(
	ID int identity(1,1) primary key,
	OrderID int not null,
	ProductID int not null,
	Quantity int not null default 0,
	foreign key (OrderID) references [Order](ID),
	foreign key (ProductID) references Product(ID)
);

insert into Role
values 
	('admin'),
	('staff')

--them user
INSERT INTO Account
values 
	('admin1', '123111', '1', '0379616543', 'PocoB'),
	('staff1', '123111', '2', '0373424344', 'Herry')

--them ban
declare @i int = 1;
while @i <= 20
begin
	insert TableFood (Name)
	values (N'Bàn ' + cast(@i as nvarchar(100)))
	set @i = @i + 1
end

select * from OrderDetail

Update TableFood set Status = N'Có người' where ID = 9

--them category
insert Category
values
	(N'Hải sản'),
	(N'Nông sản'),
	(N'Lâm sản'),
	(N'Nước')

--them mon an
insert Product
values
	(N'Mực một năng nướng sa tế', 120000, 1),
	(N'Ngêu hấp xả', 50000, 1),
	(N'Dú dê nướng sữa', 60000, 2),
	(N'Heo rừng nướng muối ớt', 75000, 3),
	(N'7Up', 15000, 4),
	(N'Cafe', 12000, 4)

--them order
insert [dbo].[Order]
values
	(GETDATE(), null, 1, 0),
	(GETDATE(), null, 2, 0),
	(GETDATE(), GETDATE(), 3, 1)

--them od
insert OrderDetail
values
	(1, 1, 2),
	(1, 3, 4),
	(1, 5, 1),
	(2, 1, 2),
	(2, 6, 2),
	(3, 2, 5)

	select *
	from Category
	select* from
	Product
	select * from [dbo].[Order]
	select * from OrderDetail

select * from OrderDetail
select * from [dbo].[Order]
select * from TableFood
select * from Account
select * from Product
select * from Category
select * from Role

delete OrderDetail
delete [Order]

select *
from [Order] o
join TableFood tb on tb.ID = o.TableID
where o.Status = 0

create trigger UpdateOrderDetail on OrderDetail for insert, update
as
begin
	declare @orderID int
	select @orderID = OrderID from inserted
	declare @tableID int
	select @tableID = TableID from [Order] where ID = @orderID and Status = 0
	update TableFood set Status = N'Có người' where ID = @tableID
end

create trigger UpdateOrder on [Order] for update
as
begin
	declare @orderID int
	select @orderID = ID from inserted
	declare @tableID int
	select @tableID = TableID from [Order] where ID = @orderID
	declare @count int = 0
	select @count = count(*) from [Order] where ID = @orderID and Status = 0
	if(@count = 0)
		update TableFood set Status = N'Trống' where ID = @tableID
end

create trigger DeleteOrderDetail on OrderDetail for delete
as
begin
	declare @orderDetailID int
	declare @orderID int

	select @orderDetailID = ID, @orderID = OrderID from deleted

	declare @tableID int
	select @tableID = TableID from [Order] where ID = @orderID

	declare @count int = 0
	select @count = COUNT(*) from OrderDetail as od, [Order] as o where o.ID = @orderID and o.ID = od.OrderID and o.Status = 0

	if(@count = 0)
		Update TableFood set Status = N'Trống' where ID = @tableID
end

