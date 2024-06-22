create database BookStoreApplication
use BookStoreApplication
create table Users(
	userId int primary key identity(1,1),
	FullName varchar(50) not null,
	EmailId varchar(50) not null,
	Password varchar(30) not null,
	MobileNumber varchar(10),
	UpdatedAt datetime2,
	CreatedAt datetime2
	);

create table Book(
	BookId int primary key identity(1,1),
	bookName varchar(50) not null,
	AuthorName varchar(50) not null,
    Description varchar(100),
    MRP decimal not null,
    DiscountPrice decimal,
    rating float,
    NoofRating int,
    bookImg varchar(50),
    Quantity int
	);

create table Cart(
	cartId int primary key identity(1,1),
	bookImg varchar(50),
	bookName varchar(50),
	AuthorName varchar(50),
	MRP decimal,
	Discountprice decimal,
	Quantity int,
	userId int foreign key references Users(userId),
	BookId int foreign key references Book(BookId)
	);

create table WishList(
	WishListId int primary key Identity(1,1),
	bookImg varchar(50),
	bookName varchar(50),
	AuthorName varchar(50),
	MRP decimal,
	Discountprice decimal,
	userId int foreign key references Users(userId),
	BookId int foreign key references Book(BookId)
	);

create table AddressForOrder(
	AddressId int primary key identity(1,1),
	FullName varchar(50),
	MobileNumber varchar(10),
	Address varchar(150) not null,
	City varchar(50) not null,
	State varchar(50) not null,
	Type varchar(20),
	userId int foreign key references Users(userId),
	IsDeleted bit
	);

create table Orders(
    OrderId int primary key identity(1,1),
    BookName varchar(50),
    AuthorName varchar(50),
    BookImg varchar(50),
    Quantity int,  
    TotalMRP decimal,
    ActualPrice decimal,
    OrderedDateTime datetime,
	Address varchar(150),
    IsDeleted bit,
	UserId int foreign key references Users(userId),
    BookId int foreign key references Book(BookId),
	AddressId int foreign key references AddressForOrder(AddressId),
	);
create table FeedBack(
	FeedBackId int primary key Identity(1,1),
	UserName varchar(50),
	Comment varchar(150),
	rating int not null,
	userId int foreign key references Users(userId),
	BookId int foreign key references Book(BookId),
	IsDelete bit
	);