----Add FeedBack----
create proc AddFeedBack
(
	@Comment varchar(150),
	@rating int,
	@userId int,
	@BookId int
)
as
begin
	begin try
	Declare @Identity table (ID nvarchar(100));
	Declare @UserName varchar(50);
	Declare @IsDeleted bit = 0;
	
	if((select count(*) from Users where userId=@userId)=1)
	begin
		select @UserName=FullName from Users where userId=@userId
		if exists(select 1 from Book where BookId=@BookId)
		begin
			insert into FeedBack(UserName,Comment,rating,userId,BookId,IsDelected) output Inserted.FeedBackId into @Identity
			values(@UserName,@Comment,@rating,@userId,@BookId,@IsDeleted);
			--update on book
			update book
			set NoofRating=NoofRating+1,
				rating=(rating*NoofRating+@rating)/(NoofRating+1)
			where BookId=@BookId;
			--fetch feedback details
			select * from FeedBack where FeedBackId=(select ID from @Identity);
		end
		else
		begin
			throw 50001,'Invaild Book Id',1;
		end
	end
	else
	begin
		throw 50003,'Invaild User Id',1;
	end
	end try
	begin catch
		DECLARE @ErrorMessage NVARCHAR(4000);
		SELECT @ErrorMessage = ERROR_MESSAGE()
		RAISERROR(@ErrorMessage,16,1);
	end catch
end;
go

----Edit Address----
create proc EditFeedBack
(
	@FeedBackId int,
	@Comment varchar(150),
	@rating int,
	@userId int,
	@BookId int
)
as
begin
	begin try
	Declare @previousRating int;
	if((select count(*) from Users where userId=@userId)=1)
	begin
		if exists(select 1 from Book where BookId=@BookId)
		begin
			select @previousRating=rating from FeedBack where FeedBackId=@FeedBackId;
			--update on FeedBack
			update FeedBack
			set Comment=@Comment,rating=@rating
			where FeedBackId=@FeedBackId

			--update on book
			update book
			set rating=(rating*NoofRating+(@rating-@previousRating))/NoofRating
			where BookId=@BookId;
			--fetch feedback details
			select * from FeedBack where FeedBackId=@FeedBackId
		end
		else
		begin
			throw 50001,'Invaild Book Id',1;
		end
	end
	else
	begin
		throw 50003,'Invaild User Id',1;
	end
	end try
	begin catch
		DECLARE @ErrorMessage NVARCHAR(4000);
		SELECT @ErrorMessage = ERROR_MESSAGE()
		RAISERROR(@ErrorMessage,16,1);
	end catch
end;
go
----Delete FeedBack by FeedBackId----
create proc RemoveFeedBack
(
	@UserId int,
	@FeedBackId int
)
as
begin
	begin try
		Declare @previousRating int;
		if exists(select 1 from FeedBack where FeedBackId=@FeedBackId and userId=@UserId)
		begin
			select @previousRating=rating from FeedBack where FeedBackId=@FeedBackId;
			--update on book rating and No of rating column
			update Book
			set NoofRating=NoofRating-1,
				rating = (rating*NoofRating-@previousRating)/(NoofRating-1)
			where book
			--update on FeedBack
			update FeedBack
			set IsDelete=1
			where FeedBackId=@FeedBackId;
		end
		else
		begin
			throw 50001,'Invalid FeedBack Id or User Id',1;
		end
	end try
	begin catch
		DECLARE @ErrorMessage NVARCHAR(4000);
		SELECT @ErrorMessage = ERROR_MESSAGE()
		RAISERROR(@ErrorMessage,16,1);
	end catch
end;
go