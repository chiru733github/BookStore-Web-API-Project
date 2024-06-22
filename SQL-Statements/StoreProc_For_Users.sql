create proc UserLogin
(
	@Email varchar(50),
	@Password varchar(50)
)
as
begin
	begin try
	DECLARE @ErrorMessage NVARCHAR(4000);
	if exists(select 1 from Users where EmailId=@Email)
	begin
		if exists(select 1 from Users where Password=@Password)
		begin 
			select EmailId,userId from Users where EmailId=@Email and Password=@Password;
		end
		else
		begin
			throw 50001,'Incorrect Password',1;
		end
	end
	else
	begin
		THROW 50000,'Invaild Email Id',1;
	end
	end try
	begin catch
		
		SELECT @ErrorMessage = ERROR_MESSAGE()
		RAISERROR(@ErrorMessage,16,1);
	end catch
end;
go

create procedure signIn
(
	@FullName varchar(50),
	@Email varchar(50),
	@password varchar(50),
	@MobileNumber varchar(10)
)
as
begin
	begin try
	Declare @Identity table (ID nvarchar(100));
	if ((select count(EmailId) from Users where EmailId=@Email)=1)
	begin
		throw 50000,'Email Id already exists',1;
	end
	if(len(@MobileNumber)!=10 and (@MobileNumber NOT LIKE '%[^0-9]%'))
	begin
		throw 50001,'Invaild Mobile Number',1;
	end
	if(LEN(@FullName)< 3 OR LEN(@FullName) > 50)
    begin
        THROW 50002, 'Full Name must be between 1 and 50 alphabetic characters', 1;
    end
	Insert into Users(FullName,EmailId,Password,MobileNumber) output Inserted.userId into @Identity
	values(@FullName,@Email,@password,@MobileNumber);
	select * from Users where userId=(select ID from @Identity);
	end try
	begin catch
		DECLARE @ErrorMessage NVARCHAR(4000);
		SELECT @ErrorMessage = ERROR_MESSAGE()
		RAISERROR(@ErrorMessage,16,1);
	end catch
end;
go
create proc checkEmail
(
	@Email varchar(50)
)
as
begin
	begin try
	DECLARE @ErrorMessage NVARCHAR(4000);
	if exists(select 1 from Users where EmailId=@Email)
	begin
		select userId,EmailId from Users where EmailId=@Email;
	end
	else
	begin
		THROW 50000,'Invaild Email Id',1;
	end
	end try
	begin catch
		SELECT @ErrorMessage = ERROR_MESSAGE()
		RAISERROR(@ErrorMessage,16,1);
	end catch
end;
go
create procedure EditProfile
(
	@UserId int,
	@FullName varchar(50),
	@Email varchar(50),
	@password varchar(50),
	@MobileNumber varchar(10)
)
as
begin
	begin try
	if(len(@MobileNumber)!=10 and (@MobileNumber NOT LIKE '%[^0-9]%'))
	begin
		throw 50001,'Invaild Mobile Number',1;
	end
	if(LEN(@FullName)< 3 OR LEN(@FullName) > 50)
    begin
        THROW 50002, 'Full Name must be between 1 and 100 alphabetic characters', 1;
    end
	update Users
	set FullName=@FullName,EmailId=@Email,Password=@password,MobileNumber=@MobileNumber
	where userId=@UserId;
	end try
	begin catch
		DECLARE @ErrorMessage NVARCHAR(4000);
		SELECT @ErrorMessage = ERROR_MESSAGE()
		RAISERROR(@ErrorMessage,16,1);
	end catch
end;
go
create proc GetById
(
	@UserId int
)
as
begin
	begin try
	DECLARE @ErrorMessage NVARCHAR(4000);
	if exists(select 1 from Users where userId=@UserId)
	begin
		select * from Users where userId=@UserId;
	end
	else
	begin
		THROW 50000,'Invaild User Id',1;
	end
	end try
	begin catch
		SELECT @ErrorMessage = ERROR_MESSAGE()
		RAISERROR(@ErrorMessage,16,1);
	end catch
end;
go
create proc ResetPassword
(
	@Email varchar(50),
	@password varchar(50)
)
as
begin
	begin try
	DECLARE @ErrorMessage NVARCHAR(4000);
	if exists(select 1 from Users where EmailId=@Email)
	begin
		update Users
		set Password=@password
		where EmailId=@Email;
	end
	else
	begin
		THROW 50000,'Invaild Email Id',1;
	end
	end try
	begin catch
		SELECT @ErrorMessage = ERROR_MESSAGE()
		RAISERROR(@ErrorMessage,16,1);
	end catch
end;
go
CREATE TRIGGER trgUsersInsertUpdate
ON Users
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Update for inserted rows
    UPDATE Users
    SET CreatedAt = CASE 
                          WHEN i.CreatedAt IS NULL THEN GETDATE() 
                          ELSE U.CreatedAt 
                      END,
        UpdatedAt = GETDATE()
    FROM Users U
    INNER JOIN inserted i ON U.UserId = i.UserId;
END;

