create procedure AddBook
(
	@bookName varchar(50),
	@AuthorName varchar(50),
	@Description varchar(100),
	@MRP decimal,
	@DiscountPercentage float,
	@BookImg varchar(50),
	@quantity int
)
as
begin
	begin try
	Declare @Identity table (ID nvarchar(100));
	Declare @DiscountPrice decimal=0;
	if(len(@bookName)<3 or Len(@bookName)>50)
	begin
		throw 50001,'Book Name must be between 3 and 50 alphabetic characters',1;
	end
	if(LEN(@AuthorName)< 3 OR LEN(@AuthorName) > 50)
    begin
        THROW 50002, 'Author Name must be between 3 and 50 alphabetic characters', 1;
    end
	if(@DiscountPercentage<1 or @DiscountPercentage>100)
    begin
        THROW 50003, 'Discount percentage must be between 1 and 100 alphabetic characters', 1;
    end
	set @DiscountPrice = @MRP *(1-(@DiscountPercentage/100));
	Insert into Book(bookName,AuthorName,Description,MRP,DiscountPrice,rating,NoofRating,bookImg,Quantity) output Inserted.BookId into @Identity
	values(@bookName,@AuthorName,@Description,@MRP,@DiscountPrice,0,0,@BookImg,@quantity) 
	select * from Book where BookId=(select ID from @Identity);
	end try
	begin catch
		DECLARE @ErrorMessage NVARCHAR(4000);
		SELECT @ErrorMessage = ERROR_MESSAGE()
		RAISERROR(@ErrorMessage,16,1);
	end catch
end;
go
create proc GetByBookId
(
	@BookId int
)
as
begin
	begin try
	DECLARE @ErrorMessage NVARCHAR(4000);
	if exists(select 1 from Book where BookId=@BookId)
	begin
		select * from Book where BookId=@BookId;
	end
	else
	begin
		THROW 50000,'Invaild Book Id',1;
	end
	end try
	begin catch
		SELECT @ErrorMessage = ERROR_MESSAGE()
		RAISERROR(@ErrorMessage,16,1);
	end catch
end;
go
create procedure UpdateByBookId
(
	@BookId int,
	@bookName varchar(50),
	@AuthorName varchar(50),
	@Description varchar(100),
	@MRP decimal,
	@DiscountPercentage float,
	@BookImg varchar(50),
	@quantity int
)
as
begin
	begin try
	Declare @DiscountPrice decimal=0;
	if(len(@bookName)<3 or Len(@bookName)>50)
	begin
		throw 50001,'Book Name must be between 3 and 50 alphabetic characters',1;
	end
	if(LEN(@AuthorName)< 3 OR LEN(@AuthorName) > 50)
    begin
        THROW 50002, 'Author Name must be between 3 and 50 alphabetic characters', 1;
    end
	if(@DiscountPercentage<1 or @DiscountPercentage>100)
    begin
        THROW 50003, 'Discount percentage must be between 1 and 100 alphabetic characters', 1;
    end
	if((select count(*) from Book where BookId=@BookId)=1)
	begin
		set @DiscountPrice = @MRP *(1-(@DiscountPercentage/100));
		Update Book
		set BookName=@bookName,AuthorName=@AuthorName,Description=@Description,MRP=@MRP,DiscountPrice=@DiscountPrice,
		bookImg=@BookImg,Quantity=@quantity
		where BookId=@BookId;
	end
	else
	begin
		throw 50004,'Invaild book Id',1;
	end
	end try
	begin catch
		DECLARE @ErrorMessage NVARCHAR(4000);
		SELECT @ErrorMessage = ERROR_MESSAGE()
		RAISERROR(@ErrorMessage,16,1);
	end catch
end;
go
create proc GetAllBooks
as
begin
	begin try
	if((select count(*) from Book)=0)
	begin
		throw 50001,'No Books are present',1;
	end
	select * from Book;
	end try
	begin catch
		DECLARE @ErrorMessage NVARCHAR(4000);
		SELECT @ErrorMessage = ERROR_MESSAGE()
		RAISERROR(@ErrorMessage,16,1);
	end catch
end;
go
create procedure RemoveBook
(
	@BookId int
)
as
begin
	begin try
		if exists(select 1 from Book where BookId=@BookId)
		begin
			delete from Book where BookId=@BookId;
		end
		else
		begin
			throw 50001,'Invalid Book Id',1;
		end
	end try
	begin catch
		DECLARE @ErrorMessage NVARCHAR(4000);
		SELECT @ErrorMessage = ERROR_MESSAGE()
		RAISERROR(@ErrorMessage,16,1);
	end catch
end

go
----Review----
--1) Find the book using any two columns of table.
create procedure FindBook
(
	@BookName varchar(50),
	@AuthorName varchar(50)
)
as
begin
	begin try
	if exists(select 1 from Book where bookName=@BookName and AuthorName=@AuthorName)
	begin
		select * from Book where bookName=@BookName and AuthorName=@AuthorName
	end
	else
	begin
		throw 50001,'Invaild BookName or AuthorName',1;
	end
	end try
	begin catch
		DECLARE @ErrorMessage NVARCHAR(4000);
		SELECT @ErrorMessage = ERROR_MESSAGE()
		RAISERROR(@ErrorMessage,16,1);
	end catch
end
go
--2)Find the data using bookid, if it exst update the data else insert the new book record.
create procedure UpdateOrInsert
(
	@BookId int,
	@bookName varchar(50),
	@AuthorName varchar(50),
	@Description varchar(100),
	@MRP decimal,
	@DiscountPercentage float,
	@BookImg varchar(50),
	@quantity int
)
as
begin
	begin try
	Declare @Identity table (ID nvarchar(100));
	Declare @ActualPrice decimal=0;
	if(len(@bookName)<3 or Len(@bookName)>50)
	begin
		throw 50001,'Book Name must be between 3 and 50 alphabetic characters',1;
	end
	if(LEN(@AuthorName)< 3 OR LEN(@AuthorName) > 50)
    begin
        THROW 50002, 'Author Name must be between 3 and 50 alphabetic characters', 1;
    end
	if(@DiscountPercentage<1 or @DiscountPercentage>100)
    begin
        THROW 50003, 'Discount percentage must be between 1 and 100 alphabetic characters', 1;
    end
	--calculating ActualPrice
	set @ActualPrice = @MRP *(1-(@DiscountPercentage/100));
	if((select count(*) from Book where BookId=@BookId)=1)
	begin
		Update Book
		set BookName=@bookName,AuthorName=@AuthorName,Description=@Description,MRP=@MRP,DiscountPrice=@ActualPrice,
		bookImg=@BookImg,Quantity=@quantity
		where BookId=@BookId;
		select * from Book where BookId=@BookId;

	end
	else
	begin
		Insert into Book(bookName,AuthorName,Description,MRP,DiscountPrice,rating,NoofRating,bookImg,Quantity) output Inserted.BookId into @Identity
		values(@bookName,@AuthorName,@Description,@MRP,@ActualPrice,0,0,@BookImg,@quantity) 
		select * from Book where BookId=(select ID from @Identity);
	end
	end try
	begin catch
		DECLARE @ErrorMessage NVARCHAR(4000);
		SELECT @ErrorMessage = ERROR_MESSAGE()
		RAISERROR(@ErrorMessage,16,1);
	end catch
end;
go
--3) Display wishlist or cart details alongwith the user who has added it
create procedure WishListWithUserDetails
as
begin
	begin try
		select WL.*,U.*
		from WishList WL join Users U
		on WL.userId=U.userId;
	end try
	begin catch
		DECLARE @ErrorMessage NVARCHAR(4000);
		SELECT @ErrorMessage = ERROR_MESSAGE()
		RAISERROR(@ErrorMessage,16,1);
	end catch
end

select * from Book