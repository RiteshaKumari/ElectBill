use EBill;

create table BillDetail(
BillDetail_Id int primary key identity,
Cust_Name varchar(255),
Mobile varchar(255),
Address varchar(255),
total_Amount int,
);

create table BillItems(
BillItems_Id int primary key identity,
Prod_Name varchar(255),
Price int,
Quality int,
BillId int foreign key references BillDetail(BillDetail_Id)
);

create procedure saveBILLDetails
@Cust_Name varchar(255),
@Mobile varchar(255),
@Address varchar(255),
@total_Amount int,
@Id int output --this will store BillDetail(BillDetail_Id)
as
begin
   insert into BillDetail(Cust_Name,Mobile,Address,total_Amount) values (@Cust_Name,@Mobile,@Address,@total_Amount);

   select @Id = SCOPE_IDENTITY();---it will return id of last inserted id
end

create procedure getAllEbillDetails
as
begin
select * from BillDetail;
end

create procedure getOneEbillDetails
@Id int
as
begin
select BillDetail.BillDetail_Id as 'BillId', BillDetail.Cust_Name,BillDetail.Mobile,BillDetail.Address,BillDetail.total_Amount, BillItems.BillItems_Id as 'ItemId',
BillItems.Prod_Name,BillItems.Price,BillItems.Quality from
BillDetail inner join BillItems on BillDetail_Id = BillItems_Id where BillDetail_Id = @Id;

end

