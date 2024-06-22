----Add Address----
create proc AddAddress
(
	@UserId int,
	@FullName varchar(50),
	@MobileNumber varchar(10),
	@Address varchar(150),
	@City nvarchar(50),
	@State nvarchar(50),
	@Type nvarchar(20)
)
as
begin
	begin try
	Declare @Identity table (ID nvarchar(100));
	Declare @IsDeleted bit = 0;
	if(len(@MobileNumber)!=10 and (@MobileNumber NOT LIKE '%[^0-9]%'))
	begin
		throw 50001,'Invaild Mobile Number',1;
	end
	if(LEN(@FullName)< 3 OR LEN(@FullName) > 50)
    begin
        THROW 50002, 'Full Name must be between 1 and 50 alphabetic characters', 1;
    end
	if((select count(*) from Users where userId=@UserId)=1)
	begin
		insert into AddressForOrder(FullName,MobileNumber,Address,City,State,Type,userId,IsDeleted) output Inserted.AddressId into @Identity
		values(@FullName,@MobileNumber,@Address,@City,@State,@Type,@UserId,@IsDeleted);
		select * from AddressForOrder where AddressId=(select ID from @Identity);
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
create proc EditAddress
(
	@AddressId int,
	@UserId int,
	@FullName varchar(50),
	@MobileNumber varchar(10),
	@Address varchar(150),
	@City nvarchar(50),
	@State nvarchar(50),
	@Type nvarchar(20)
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
        THROW 50002, 'Full Name must be between 1 and 50 alphabetic characters', 1;
    end
	if not Exists(select 1 from Users where userId=@UserId)
	begin
		throw 50003,'Invaild User Id',1;
	end
	if((select count(*) from AddressForOrder where AddressId=@AddressId and IsDeleted=0)=1)
	begin
		update AddressForOrder
		set FullName=@FullName,MobileNumber=@MobileNumber,Address=@Address,City=@City,State=@State,Type=@Type
		where AddressId=@AddressId;
		select * from AddressForOrder where AddressId=@AddressId;
	end
	else
	begin
		throw 50004,'Invaild Address Id',1;
	end
	end try
	begin catch
		DECLARE @ErrorMessage NVARCHAR(4000);
		SELECT @ErrorMessage = ERROR_MESSAGE()
		RAISERROR(@ErrorMessage,16,1);
	end catch
end;
go
----Get All Addresses By UserId----
create proc GetAllAddressByUserId
(
	@userId int
)
as
begin
	begin try
	if((select count(*) from AddressForOrder where userId=@userId and IsDeleted=0)=0)
	begin
		throw 50001,'No Addresses are present by userID',1;
	end
	else
	begin
		select * from AddressForOrder where userId=@userId and IsDeleted=0;
	end
	end try
	begin catch
		DECLARE @ErrorMessage NVARCHAR(4000);
		SELECT @ErrorMessage = ERROR_MESSAGE()
		RAISERROR(@ErrorMessage,16,1);
	end catch
end;
go
----Delete Address by AddressId----
create proc RemoveFromAddress
(
	@UserId int,
	@AddressId int
)
as
begin
	begin try
		if exists(select 1 from AddressForOrder where AddressId=@AddressId and userId=@UserId)
		begin
			update AddressForOrder
			set IsDeleted=1
			where AddressId=@AddressId;
		end
		else
		begin
			throw 50001,'Invalid Address Id or User Id',1;
		end
	end try
	begin catch
		DECLARE @ErrorMessage NVARCHAR(4000);
		SELECT @ErrorMessage = ERROR_MESSAGE()
		RAISERROR(@ErrorMessage,16,1);
	end catch
end;
go
----Get by Address by Address Id----
create proc GetAddressByAddressId
(
	@userId int,
	@AddressId int
)
as
begin
	begin try
	if((select count(*) from AddressForOrder where userId=@userId and IsDeleted=0)=0)
	begin
		throw 50001,'No Addresses are present by userID',1;
	end
	else
	begin
		select * from AddressForOrder where userId=@userId and AddressId=@AddressId and IsDeleted=0;
	end
	end try
	begin catch
		DECLARE @ErrorMessage NVARCHAR(4000);
		SELECT @ErrorMessage = ERROR_MESSAGE()
		RAISERROR(@ErrorMessage,16,1);
	end catch
end;
go