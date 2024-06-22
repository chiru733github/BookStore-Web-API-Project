create procedure AddTocart
(
	@userId int,
	@BookId int
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
	Declare @Quantity int =1;
	if((select count(*) from Users where userId=@userId)=1)
	begin
		if((select count(*) from Book where BookId=@BookId and Quantity>=1)=1)
		begin
			select @bookImg=bookImg,@BookName=bookName,@AuthorName=AuthorName,@MRP=MRP,@DiscountPrice=DiscountPrice from Book where BookId=@BookId;
			Insert into Cart(bookImg,bookName,AuthorName,MRP,Discountprice,Quantity,userId,BookId) output Inserted.cartId into @Identity
			values(@bookImg,@BookName,@AuthorName,@MRP,@DiscountPrice,@Quantity,@userId,@BookId);
			--Fetch cart details
			select * from Cart where cartId=(select ID from @Identity);
		end
		else
		begin
			throw 50001,'Invalid book Id or Out of Stock',1;
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

create proc GetAllCartByUserId
(
	@userId int
)
as
begin
	begin try
	if((select count(*) from Cart where userId=@userId)=0)
	begin
		throw 50001,'No Carts are present by userID',1;
	end
	else
	begin
		select * from Cart where userId=@userId;
	end
	end try
	begin catch
		DECLARE @ErrorMessage NVARCHAR(4000);
		SELECT @ErrorMessage = ERROR_MESSAGE()
		RAISERROR(@ErrorMessage,16,1);
	end catch
end;
go

create proc updateQuantity
(
	@cartId int,
	@Quantity int
)
as
begin
	begin try
		Declare @bookId int
		if((select 1 from Cart where cartId=@cartId)=1)
		begin
			update Cart
			set Quantity=@Quantity,
			MRP = MRP*@Quantity,
			Discountprice=Discountprice*@Quantity
			where cartId=@cartId;
			select * from Cart where cartId=@cartId;
		end
		else
		begin
			throw 50002,'Invalid cart Id',1;
		end
	end try
	begin catch
		DECLARE @ErrorMessage NVARCHAR(4000);
		SELECT @ErrorMessage = ERROR_MESSAGE()
		RAISERROR(@ErrorMessage,16,1);
	end catch
end;
go
create proc RemoveFromCart
(
	@cartId int
)
as
begin
	begin try
		if exists(select 1 from Cart where cartId=@cartId)
		begin
			delete from Cart
			where cartId=@cartId
		end
		else
		begin
			throw 50001,'Invalid cart Id',1;
		end
	end try
	begin catch
		DECLARE @ErrorMessage NVARCHAR(4000);
		SELECT @ErrorMessage = ERROR_MESSAGE()
		RAISERROR(@ErrorMessage,16,1);
	end catch
end;
go
create proc ViewAllCart
as
begin
	begin try
	if((select count(*) from Cart)=0)
	begin
		throw 50001,'No Carts are presented',1;
	end
	else
	begin
		select * from Cart;
	end
	end try
	begin catch
		DECLARE @ErrorMessage NVARCHAR(4000);
		SELECT @ErrorMessage = ERROR_MESSAGE()
		RAISERROR(@ErrorMessage,16,1);
	end catch
end;
go
create proc NoofCartsByUserId
(
	@userId int
)
as
begin
	begin try
	if((select count(*) from Cart where userId=@userId)=0)
	begin
		throw 50001,'No Carts are present by userID',1;
	end
	else
	begin
		select count(*) as CartCount from Cart where userId=@userId;
	end
	end try
	begin catch
		DECLARE @ErrorMessage NVARCHAR(4000);
		SELECT @ErrorMessage = ERROR_MESSAGE()
		RAISERROR(@ErrorMessage,16,1);
	end catch
end;