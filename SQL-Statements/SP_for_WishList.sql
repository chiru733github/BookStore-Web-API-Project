create procedure AddToWishList
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
	if((select count(*) from Users where userId=@userId)=1)
	begin
		if((select count(*) from Book where BookId=@BookId)=1)
		begin
			select @bookImg=bookImg,@BookName=bookName,@AuthorName=AuthorName,@MRP=MRP,@DiscountPrice=DiscountPrice from Book where BookId=@BookId;
			Insert into WishList(bookImg,bookName,AuthorName,MRP,Discountprice,userId,BookId) output Inserted.WishListId into @Identity
			values(@bookImg,@BookName,@AuthorName,@MRP,@DiscountPrice,@userId,@BookId);
			--Fetch cart details
			select * from WishList where WishListId=(select ID from @Identity);
		end
		else
		begin
			throw 50001,'Invalid book Id',1;
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
create proc GetAllWishListByUserId
(
	@userId int
)
as
begin
	begin try
	if((select count(*) from WishList where userId=@userId)=0)
	begin
		throw 50001,'No WishList are present by userID',1;
	end
	else
	begin
		select * from WishList where userId=@userId;
	end
	end try
	begin catch
		DECLARE @ErrorMessage NVARCHAR(4000);
		SELECT @ErrorMessage = ERROR_MESSAGE()
		RAISERROR(@ErrorMessage,16,1);
	end catch
end;
go
create proc RemoveFromWishList
(
	@WishListId int
)
as
begin
	begin try
		if exists(select 1 from WishList where WishListId=@WishListId)
		begin
			delete from WishList
			where WishListId=@WishListId;
		end
		else
		begin
			throw 50001,'Invalid WishList Id',1;
		end
	end try
	begin catch
		DECLARE @ErrorMessage NVARCHAR(4000);
		SELECT @ErrorMessage = ERROR_MESSAGE()
		RAISERROR(@ErrorMessage,16,1);
	end catch
end;
go
create proc ViewAllWishCart
as
begin
	begin try
	if((select count(*) from WishList)=0)
	begin
		throw 50001,'No WishList are presented',1;
	end
	else
	begin
		select * from WishList;
	end
	end try
	begin catch
		DECLARE @ErrorMessage NVARCHAR(4000);
		SELECT @ErrorMessage = ERROR_MESSAGE()
		RAISERROR(@ErrorMessage,16,1);
	end catch
end;