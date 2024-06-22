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
	Update Book
	set BookName=@bookName,AuthorName=@AuthorName,Description=@Description,MRP=@MRP,DiscountPrice=@DiscountPrice,
	bookImg=@BookImg,Quantity=@quantity
	where BookId=@BookId;
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
