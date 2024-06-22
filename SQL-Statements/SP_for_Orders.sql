create procedure AddOrder
(
	@CartId int,
	@AddressId int
)
as
begin
	begin try
	Declare @Identity table (ID nvarchar(100));
	Declare @bookImg varchar(50);
	Declare @BookName varchar(50);
	Declare @AuthorName varchar(50);
	Declare @MRP decimal;
	Declare @DiscountPrice decimal;
	Declare @Quantity int;
	Declare @userId int;
	Declare @BookId int;
	Declare @Address varchar(150);
	if((select count(*) from Cart where cartId=@CartId)=1)
	begin
		select @BookId=BookId,@userId=userId,@Quantity=Quantity from Cart where cartId=@CartId;
		if Not Exists(select 1 from AddressForOrder where AddressId=@AddressId)
		begin
			throw 50003,'Invaild Address Id',1;
		end
		else
		begin
			select @Address=Address from AddressForOrder where AddressId=@AddressId
			if((select count(*) from Book where BookId=@BookId and Quantity>=@Quantity)=1)
			begin
				select @bookImg=bookImg,@BookName=bookName,@AuthorName=AuthorName,@MRP=MRP,@DiscountPrice=DiscountPrice from Cart where cartId=@CartId;
				--insert into orders
				Insert into Orders(bookImg,bookName,AuthorName,TotalMRP,ActualPrice,Quantity,OrderedDateTime,Address,IsDeleted,UserId,BookId,AddressId) output Inserted.OrderId into @Identity
				values(@bookImg,@BookName,@AuthorName,@MRP,@DiscountPrice,@Quantity,GETDATE(),@Address,0,@userId,@BookId,@AddressId);
				--Update on Book quantity
				update Book
				set Quantity=Quantity-@Quantity
				where BookId=@BookId;
				--Fetch cart details
				select * from Orders where OrderId=(select ID from @Identity);
				--remove from cart
				delete from Cart
				where cartId=@CartId;
			end
			else
			begin
				throw 50001,'Invalid book Id or Out of Stock',1;
			end
		end	
	end
	else
	begin
		throw 50002,'Invalid user Id',1;
	end
	end try
	begin catch
		DECLARE @ErrorMessage NVARCHAR(4000);
		SELECT @ErrorMessage = ERROR_MESSAGE()
		RAISERROR(@ErrorMessage,16,1);
	end catch
end;
go
create proc GetAllOrdersByUserId
(
	@userId int
)
as
begin
	begin try
	if((select count(*) from Orders where userId=@userId)=0)
	begin
		throw 50001,'No Orders are present by userID',1;
	end
	else
	begin
		select * from Orders where userId=@userId;
	end
	end try
	begin catch
		DECLARE @ErrorMessage NVARCHAR(4000);
		SELECT @ErrorMessage = ERROR_MESSAGE()
		RAISERROR(@ErrorMessage,16,1);
	end catch
end;
go
create proc RemoveOrder
(
	@OrderId int
)
as
begin
	begin try
		if exists(select 1 from Orders where OrderId=@OrderId)
		begin
			if exists(select 1 from Orders where OrderedDateTime+1 > GETDATE())
			begin
				update Orders
				set IsDeleted=1
				where OrderId=@OrderId
			end
			else
			begin
				throw 50002,'Time Expired for removing order',1;
			end
		end
		else
		begin
			throw 50001,'Invalid Order Id',1;
		end
	end try
	begin catch
		DECLARE @ErrorMessage NVARCHAR(4000);
		SELECT @ErrorMessage = ERROR_MESSAGE()
		RAISERROR(@ErrorMessage,16,1);
	end catch
end;
go
create proc ViewAllOrder
as
begin
	begin try
	if((select count(*) from Orders)=0)
	begin
		throw 50001,'No Orders are presented',1;
	end
	else
	begin
		select * from Orders where IsDeleted=0;
	end
	end try
	begin catch
		DECLARE @ErrorMessage NVARCHAR(4000);
		SELECT @ErrorMessage = ERROR_MESSAGE()
		RAISERROR(@ErrorMessage,16,1);
	end catch
end;

