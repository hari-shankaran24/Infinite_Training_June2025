if db_id('RAILWAYS') is not null
begin
    alter database RAILWAYS set single_user with rollback immediate;
    drop database RAILWAYS;
end

create database RAILWAYS;
use RAILWAYS;

-- ============================
-- Tables
-- ============================
select * from Customer

create table Customer (
    CustID varchar(20) primary key,
    CustName varchar(100) not null,
    Phone varchar(15) not null,
    Email varchar(255) not null,
    Username varchar(100) unique not null,
    Password varchar(255) not null,
    Address varchar(255) not null,
    IsDeleted bit default 0
);

------------------------------------------------------------
select * from TrainDetails

create table TrainDetails (
    TrainNo varchar(10) not null,
    TrainName varchar(100) not null,
    Source varchar(100) not null,
    Destination varchar(100) not null,
    ClassType varchar(20) not null,
    SeatsAvailable int not null,
    Price decimal(10,2) not null,
    DepartureDateTime datetime not null,
    primary key (TrainNo, ClassType)
);

------------------------------------------------------------
select * from Booking

create table Booking (
    BookingID varchar(20) primary key,
    CustID varchar(20) not null,
    TrainNo varchar(10) not null,
    ClassType varchar(20) not null,
    SeatsBooked int not null,
    TotalCost decimal(10,2) not null,
    BookingDate datetime not null default getdate(),
    TravelDate date not null,
    foreign key (CustID) references Customer(CustID),
    foreign key (TrainNo, ClassType) references TrainDetails(TrainNo, ClassType)
);

------------------------------------------------------------
select * from Passenger

create table Passenger (
    PassengerID varchar(20) primary key,
    BookingID varchar(20) not null,
    PassengerName varchar(100) not null,
    Age int not null,
    Gender varchar(10) not null,
    BerthAllotment varchar(20) not null,
    IsCancelled bit default 0,
    foreign key (BookingID) references Booking(BookingID)
);

------------------------------------------------------------
select * from Cancellation

create table Cancellation (
    CancelID int primary key identity(1,1),
    BookingID varchar(20) not null,
    PassengerID varchar(20) null,
    RefundAmount decimal(10,2) not null,
    CancelDate datetime not null default getdate(),
    foreign key (BookingID) references Booking(BookingID),
    foreign key (PassengerID) references Passenger(PassengerID)
);

-- ============================
-- ID generator procs
-- ============================
create or alter procedure GetNextBookingID
as
begin
    set nocount on;
    declare @NextNum int;
    select @NextNum = isnull(max(cast(substring(BookingID,2,len(BookingID)-1) as int)),0) + 1 from Booking;
    if @NextNum >= 100000
        select 'B' + right('000000' + cast(@NextNum as varchar(20)), 6) as NextBookingID;
    else
        select 'B' + right('00000' + cast(@NextNum as varchar(20)), 5) as NextBookingID;
end;

------------------------------------------------------------
create or alter procedure GetNextCustomerID
as
begin
    set nocount on;
    declare @NextNum int;
    select @NextNum = isnull(max(cast(substring(CustID,2,len(CustID)-1) as int)),0) + 1 from Customer;
    if @NextNum >= 100000
        select 'C' + right('000000' + cast(@NextNum as varchar(20)), 6) as NextCustomerID;
    else
        select 'C' + right('00000' + cast(@NextNum as varchar(20)), 5) as NextCustomerID;
end;

------------------------------------------------------------
create or alter procedure GetNextPassengerID
as
begin
    set nocount on;
    declare @NextNum int;
    select @NextNum = isnull(max(cast(substring(PassengerID,2,len(PassengerID)-1) as int)),0) + 1 from Passenger;
    if @NextNum >= 100000
        select 'P' + right('000000' + cast(@NextNum as varchar(20)), 6) as NextPassengerID;
    else
        select 'P' + right('00000' + cast(@NextNum as varchar(20)), 5) as NextPassengerID;
end;

-- ============================
-- Stored Procedures
-- ============================
create or alter procedure sp_RegisterUser
    @CustName varchar(100),
    @Phone varchar(15),
    @Email varchar(255),
    @Username varchar(100),
    @Password varchar(255),
    @Address varchar(255)
as
begin
    set nocount on;
    declare @NewCustID varchar(20);
    declare @NextNum int;
    select @NextNum = isnull(max(cast(substring(CustID,2,len(CustID)-1) as int)),0) + 1 from Customer;
    if @NextNum >= 100000
        set @NewCustID = 'C' + right('000000' + cast(@NextNum as varchar(20)), 6);
    else
        set @NewCustID = 'C' + right('00000' + cast(@NextNum as varchar(20)), 5);
    insert into Customer (CustID, CustName, Phone, Email, Username, Password, Address)
    values (@NewCustID, @CustName, @Phone, @Email, @Username, @Password, @Address);
    select @NewCustID as NewCustID;
end;

------------------------------------------------------------
create or alter procedure sp_BookTickets
    @BookingID varchar(20),
    @CustID varchar(20),
    @TrainNo varchar(10),
    @ClassType varchar(20),
    @SeatsBooked int,
    @TotalCost decimal(10,2),
    @TravelDate date
as
begin
    set nocount on;
    begin transaction;
    begin try
        if exists (
            select 1 from TrainDetails with (updlock, rowlock)
            where TrainNo = @TrainNo and ClassType = @ClassType and SeatsAvailable >= @SeatsBooked
        )
        begin
            insert into Booking (BookingID, CustID, TrainNo, ClassType, SeatsBooked, TotalCost, TravelDate)
            values (@BookingID, @CustID, @TrainNo, @ClassType, @SeatsBooked, @TotalCost, @TravelDate);
            update TrainDetails
            set SeatsAvailable = SeatsAvailable - @SeatsBooked
            where TrainNo = @TrainNo and ClassType = @ClassType;
            commit transaction;
        end
        else
        begin
            rollback transaction;
            throw 50001, 'Not enough seats available. Added to Waiting List', 1;
        end
    end try
    begin catch
        if xact_state() <> 0
            rollback transaction;
        throw;
    end catch
end;

------------------------------------------------------------
create or alter procedure sp_InsertPassenger
    @PassengerID varchar(20),
    @BookingID varchar(20),
    @PassengerName varchar(100),
    @Age int,
    @Gender varchar(10),
    @BerthAllotment varchar(20)
as
begin
    set nocount on;
    insert into Passenger (PassengerID, BookingID, PassengerName, Age, Gender, BerthAllotment)
    values (@PassengerID, @BookingID, @PassengerName, @Age, @Gender, @BerthAllotment);
end;

------------------------------------------------------------
create or alter procedure sp_CancelFullBooking
    @BookingID varchar(20),
    @Phone varchar(15)
as
begin
    set nocount on;
    declare @CustID varchar(20), @Seats int, @ClassType varchar(20), @TrainNo varchar(10), @Amount decimal(10,2);
    select @CustID = b.CustID, @Seats = b.SeatsBooked, @ClassType = b.ClassType, @TrainNo = b.TrainNo, @Amount = b.TotalCost
    from Booking b join Customer c on b.CustID = c.CustID
    where b.BookingID = @BookingID and c.Phone = @Phone;
    if @CustID is null
        throw 50002, 'Booking not found for provided phone.', 1;
    update Passenger set IsCancelled = 1 where BookingID = @BookingID;
    insert into Cancellation (BookingID, PassengerID, RefundAmount)
    values (@BookingID, null, @Amount * 0.5);
    update TrainDetails set SeatsAvailable = SeatsAvailable + @Seats where TrainNo = @TrainNo and ClassType = @ClassType;
end;

------------------------------------------------------------
create or alter procedure sp_CancelPassengersByPassengerIDs
    @PassengerIDs varchar(max)
as
begin
    set nocount on;
    declare @xml xml = cast('<i>' + replace(@PassengerIDs, ',', '</i><i>') + '</i>' as xml);
    declare @Ids table (PassengerID varchar(20));
    insert into @Ids (PassengerID)
    select T.c.value('.', 'varchar(20)') from @xml.nodes('/i') T(c);
    declare @BookingID varchar(20), @TrainNo varchar(10), @ClassType varchar(20);
    declare @CancelledSeats int;
    select top 1 @BookingID = b.BookingID, @TrainNo = b.TrainNo, @ClassType = b.ClassType
    from Passenger p join @Ids i on p.PassengerID = i.PassengerID
    join Booking b on p.BookingID = b.BookingID;
    select @CancelledSeats = count(*)
    from Passenger p join @Ids i on p.PassengerID = i.PassengerID
    where p.IsCancelled = 0;
    if @CancelledSeats = 0
    begin
        throw 50003, 'No valid passengers to cancel.', 1;
        return;
    end
    update p set IsCancelled = 1
    from Passenger p join @Ids i on p.PassengerID = i.PassengerID
    where p.IsCancelled = 0;
    insert into Cancellation (BookingID, PassengerID, RefundAmount)
    select b.BookingID, p.PassengerID, (b.TotalCost / nullif(b.SeatsBooked, 0)) * 0.5
    from Passenger p join @Ids i on p.PassengerID = i.PassengerID
    join Booking b on p.BookingID = b.BookingID;
    update TrainDetails set SeatsAvailable = SeatsAvailable + @CancelledSeats
    where TrainNo = @TrainNo and ClassType = @ClassType;
end;

------------------------------------------------------------
create or alter procedure sp_SearchTrains
    @Source varchar(100),
    @Destination varchar(100),
    @TravelDate date = null
as
begin
    set nocount on;
    if exists (
        select 1 from TrainDetails
        where Source = @Source and Destination = @Destination
          and (@TravelDate is null or cast(DepartureDateTime as date) = @TravelDate)
    )
    begin
        select TrainNo, TrainName, Source, Destination, ClassType, SeatsAvailable, Price, DepartureDateTime
        from TrainDetails
        where Source = @Source and Destination = @Destination
          and (@TravelDate is null or cast(DepartureDateTime as date) = @TravelDate)
        order by DepartureDateTime;
    end
    else
    begin
        select TrainNo, TrainName, Source, Destination, ClassType, SeatsAvailable, Price, DepartureDateTime
        from TrainDetails
        where Source = @Source and Destination = @Destination
        order by DepartureDateTime;
    end
end;

------------------------------------------------------------
create or alter procedure sp_TicketReport
    @BookingID varchar(20)
as
begin
    set nocount on;
    select b.BookingID, b.BookingDate, b.TotalCost, b.TravelDate, t.TrainName, t.Source, t.Destination, t.DepartureDateTime,
           p.PassengerID, p.PassengerName, p.Age, p.Gender, p.BerthAllotment, p.IsCancelled
    from Booking b
    join TrainDetails t on b.TrainNo = t.TrainNo and b.ClassType = t.ClassType
    join Passenger p on b.BookingID = p.BookingID
    where b.BookingID = @BookingID;
end;

------------------------------------------------------------
create or alter procedure sp_SoftDeleteUser
    @CustID varchar(20)
as
begin
    update Customer set IsDeleted = 1 where CustID = @CustID;
end;

------------------------------------------------------------
create or alter procedure sp_LoginUser
    @Username varchar(100),
    @Password varchar(255)
as
begin
    select CustID, CustName, Phone, Email, Address from Customer
    where Username = @Username and Password = @Password and IsDeleted = 0;
end;

------------------------------------------------------------
create or alter procedure sp_InsertTrain
    @TrainNo varchar(10),
    @TrainName varchar(100),
    @Source varchar(100),
    @Destination varchar(100),
    @ClassType varchar(20),
    @SeatsAvailable int,
    @Price decimal(10,2),
    @DepartureDateTime datetime
as
begin
    insert into TrainDetails (TrainNo, TrainName, Source, Destination, ClassType, SeatsAvailable, Price, DepartureDateTime)
    values (@TrainNo, @TrainName, @Source, @Destination, @ClassType, @SeatsAvailable, @Price, @DepartureDateTime);
end;



-- Chennai <-> Bangalore
INSERT INTO TrainDetails (TrainNo, TrainName, Source, Destination, ClassType, SeatsAvailable, Price, DepartureDateTime) VALUES
('1001', 'Shatabdi Express', 'Chennai', 'Bangalore', 'Sleeper', 30, 200, DATEADD(HOUR,6,CAST(GETDATE() AS DATETIME))),
('1001', 'Shatabdi Express', 'Chennai', 'Bangalore', '1st AC', 30, 1000, DATEADD(HOUR,6,CAST(GETDATE() AS DATETIME))),
('1001', 'Shatabdi Express', 'Chennai', 'Bangalore', '2nd AC', 30, 800, DATEADD(HOUR,6,CAST(GETDATE() AS DATETIME))),
('1001', 'Shatabdi Express', 'Chennai', 'Bangalore', '3rd AC', 30, 600, DATEADD(HOUR,6,CAST(GETDATE() AS DATETIME))),

('1002', 'Kaveri Express', 'Chennai', 'Bangalore', 'Sleeper', 30, 200, DATEADD(HOUR,12,CAST(GETDATE() AS DATETIME))),
('1002', 'Kaveri Express', 'Chennai', 'Bangalore', '1st AC', 30, 1000, DATEADD(HOUR,12,CAST(GETDATE() AS DATETIME))),
('1002', 'Kaveri Express', 'Chennai', 'Bangalore', '2nd AC', 30, 800, DATEADD(HOUR,12,CAST(GETDATE() AS DATETIME))),
('1002', 'Kaveri Express', 'Chennai', 'Bangalore', '3rd AC', 30, 600, DATEADD(HOUR,12,CAST(GETDATE() AS DATETIME))),

('1003', 'Tamil Nadu Express', 'Chennai', 'Bangalore', 'Sleeper', 30, 200, DATEADD(HOUR,20,CAST(GETDATE() AS DATETIME))),
('1003', 'Tamil Nadu Express', 'Chennai', 'Bangalore', '1st AC', 30, 1000, DATEADD(HOUR,20,CAST(GETDATE() AS DATETIME))),
('1003', 'Tamil Nadu Express', 'Chennai', 'Bangalore', '2nd AC', 30, 800, DATEADD(HOUR,20,CAST(GETDATE() AS DATETIME))),
('1003', 'Tamil Nadu Express', 'Chennai', 'Bangalore', '3rd AC', 30, 600, DATEADD(HOUR,20,CAST(GETDATE() AS DATETIME)));

-- Bangalore to Chennai
INSERT INTO TrainDetails (TrainNo, TrainName, Source, Destination, ClassType, SeatsAvailable, Price, DepartureDateTime) VALUES
('1101', 'Shatabdi Express', 'Bangalore', 'Chennai', 'Sleeper', 30, 200, DATEADD(HOUR,10,CAST(GETDATE() AS DATETIME))),
('1101', 'Shatabdi Express', 'Bangalore', 'Chennai', '1st AC', 30, 1000, DATEADD(HOUR,10,CAST(GETDATE() AS DATETIME))),
('1101', 'Shatabdi Express', 'Bangalore', 'Chennai', '2nd AC', 30, 800, DATEADD(HOUR,10,CAST(GETDATE() AS DATETIME))),
('1101', 'Shatabdi Express', 'Bangalore', 'Chennai', '3rd AC', 30, 600, DATEADD(HOUR,10,CAST(GETDATE() AS DATETIME))),

('1102', 'Kaveri Express', 'Bangalore', 'Chennai', 'Sleeper', 30, 200, DATEADD(HOUR,16,CAST(GETDATE() AS DATETIME))),
('1102', 'Kaveri Express', 'Bangalore', 'Chennai', '1st AC', 30, 1000, DATEADD(HOUR,16,CAST(GETDATE() AS DATETIME))),
('1102', 'Kaveri Express', 'Bangalore', 'Chennai', '2nd AC', 30, 800, DATEADD(HOUR,16,CAST(GETDATE() AS DATETIME))),
('1102', 'Kaveri Express', 'Bangalore', 'Chennai', '3rd AC', 30, 600, DATEADD(HOUR,16,CAST(GETDATE() AS DATETIME))),

('1103', 'Tamil Nadu Express', 'Bangalore', 'Chennai', 'Sleeper', 30, 200, DATEADD(HOUR,24,CAST(GETDATE() AS DATETIME))),
('1103', 'Tamil Nadu Express', 'Bangalore', 'Chennai', '1st AC', 30, 1000, DATEADD(HOUR,24,CAST(GETDATE() AS DATETIME))),
('1103', 'Tamil Nadu Express', 'Bangalore', 'Chennai', '2nd AC', 30, 800, DATEADD(HOUR,24,CAST(GETDATE() AS DATETIME))),
('1103', 'Tamil Nadu Express', 'Bangalore', 'Chennai', '3rd AC', 30, 600, DATEADD(HOUR,24,CAST(GETDATE() AS DATETIME)));

-- Chennai <-> Hyderabad
INSERT INTO TrainDetails (TrainNo, TrainName, Source, Destination, ClassType, SeatsAvailable, Price, DepartureDateTime) VALUES
('1004', 'Godavari Express', 'Chennai', 'Hyderabad', 'Sleeper', 30, 220, DATEADD(HOUR,6,CAST(GETDATE() AS DATETIME))),
('1004', 'Godavari Express', 'Chennai', 'Hyderabad', '1st AC', 30, 1100, DATEADD(HOUR,6,CAST(GETDATE() AS DATETIME))),
('1004', 'Godavari Express', 'Chennai', 'Hyderabad', '2nd AC', 30, 850, DATEADD(HOUR,6,CAST(GETDATE() AS DATETIME))),
('1004', 'Godavari Express', 'Chennai', 'Hyderabad', '3rd AC', 30, 650, DATEADD(HOUR,6,CAST(GETDATE() AS DATETIME))),

('1005', 'Krishna Express', 'Chennai', 'Hyderabad', 'Sleeper', 30, 220, DATEADD(HOUR,12,CAST(GETDATE() AS DATETIME))),
('1005', 'Krishna Express', 'Chennai', 'Hyderabad', '1st AC', 30, 1100, DATEADD(HOUR,12,CAST(GETDATE() AS DATETIME))),
('1005', 'Krishna Express', 'Chennai', 'Hyderabad', '2nd AC', 30, 850, DATEADD(HOUR,12,CAST(GETDATE() AS DATETIME))),
('1005', 'Krishna Express', 'Chennai', 'Hyderabad', '3rd AC', 30, 650, DATEADD(HOUR,12,CAST(GETDATE() AS DATETIME))),

('1006', 'Hyderabad Express', 'Chennai', 'Hyderabad', 'Sleeper', 30, 220, DATEADD(HOUR,20,CAST(GETDATE() AS DATETIME))),
('1006', 'Hyderabad Express', 'Chennai', 'Hyderabad', '1st AC', 30, 1100, DATEADD(HOUR,20,CAST(GETDATE() AS DATETIME))),
('1006', 'Hyderabad Express', 'Chennai', 'Hyderabad', '2nd AC', 30, 850, DATEADD(HOUR,20,CAST(GETDATE() AS DATETIME))),
('1006', 'Hyderabad Express', 'Chennai', 'Hyderabad', '3rd AC', 30, 650, DATEADD(HOUR,20,CAST(GETDATE() AS DATETIME)));

-- Hyderabad to Chennai
INSERT INTO TrainDetails (TrainNo, TrainName, Source, Destination, ClassType, SeatsAvailable, Price, DepartureDateTime) VALUES
('1104', 'Godavari Express', 'Hyderabad', 'Chennai', 'Sleeper', 30, 220, DATEADD(HOUR,10,CAST(GETDATE() AS DATETIME))),
('1104', 'Godavari Express', 'Hyderabad', 'Chennai', '1st AC', 30, 1100, DATEADD(HOUR,10,CAST(GETDATE() AS DATETIME))),
('1104', 'Godavari Express', 'Hyderabad', 'Chennai', '2nd AC', 30, 850, DATEADD(HOUR,10,CAST(GETDATE() AS DATETIME))),
('1104', 'Godavari Express', 'Hyderabad', 'Chennai', '3rd AC', 30, 650, DATEADD(HOUR,10,CAST(GETDATE() AS DATETIME))),

('1105', 'Krishna Express', 'Hyderabad', 'Chennai', 'Sleeper', 30, 220, DATEADD(HOUR,16,CAST(GETDATE() AS DATETIME))),
('1105', 'Krishna Express', 'Hyderabad', 'Chennai', '1st AC', 30, 1100, DATEADD(HOUR,16,CAST(GETDATE() AS DATETIME))),
('1105', 'Krishna Express', 'Hyderabad', 'Chennai', '2nd AC', 30, 850, DATEADD(HOUR,16,CAST(GETDATE() AS DATETIME))),
('1105', 'Krishna Express', 'Hyderabad', 'Chennai', '3rd AC', 30, 650, DATEADD(HOUR,16,CAST(GETDATE() AS DATETIME))),

('1106', 'Hyderabad Express', 'Hyderabad', 'Chennai', 'Sleeper', 30, 220, DATEADD(HOUR,24,CAST(GETDATE() AS DATETIME))),
('1106', 'Hyderabad Express', 'Hyderabad', 'Chennai', '1st AC', 30, 1100, DATEADD(HOUR,24,CAST(GETDATE() AS DATETIME))),
('1106', 'Hyderabad Express', 'Hyderabad', 'Chennai', '2nd AC', 30, 850, DATEADD(HOUR,24,CAST(GETDATE() AS DATETIME))),
('1106', 'Hyderabad Express', 'Hyderabad', 'Chennai', '3rd AC', 30, 650, DATEADD(HOUR,24,CAST(GETDATE() AS DATETIME)));

-- Chennai <-> Visakhapatnam
INSERT INTO TrainDetails (TrainNo, TrainName, Source, Destination, ClassType, SeatsAvailable, Price, DepartureDateTime) VALUES
('1007', 'Visakha Express', 'Chennai', 'Visakhapatnam', 'Sleeper', 30, 240, DATEADD(HOUR,6,CAST(GETDATE() AS DATETIME))),
('1007', 'Visakha Express', 'Chennai', 'Visakhapatnam', '1st AC', 30, 1200, DATEADD(HOUR,6,CAST(GETDATE() AS DATETIME))),
('1007', 'Visakha Express', 'Chennai', 'Visakhapatnam', '2nd AC', 30, 900, DATEADD(HOUR,6,CAST(GETDATE() AS DATETIME))),
('1007', 'Visakha Express', 'Chennai', 'Visakhapatnam', '3rd AC', 30, 700, DATEADD(HOUR,6,CAST(GETDATE() AS DATETIME))),

('1008', 'East Coast Express', 'Chennai', 'Visakhapatnam', 'Sleeper', 30, 240, DATEADD(HOUR,12,CAST(GETDATE() AS DATETIME))),
('1008', 'East Coast Express', 'Chennai', 'Visakhapatnam', '1st AC', 30, 1200, DATEADD(HOUR,12,CAST(GETDATE() AS DATETIME))),
('1008', 'East Coast Express', 'Chennai', 'Visakhapatnam', '2nd AC', 30, 900, DATEADD(HOUR,12,CAST(GETDATE() AS DATETIME))),
('1008', 'East Coast Express', 'Chennai', 'Visakhapatnam', '3rd AC', 30, 700, DATEADD(HOUR,12,CAST(GETDATE() AS DATETIME))),

('1009', 'Kalinga Express', 'Chennai', 'Visakhapatnam', 'Sleeper', 30, 240, DATEADD(HOUR,20,CAST(GETDATE() AS DATETIME))),
('1009', 'Kalinga Express', 'Chennai', 'Visakhapatnam', '1st AC', 30, 1200, DATEADD(HOUR,20,CAST(GETDATE() AS DATETIME))),
('1009', 'Kalinga Express', 'Chennai', 'Visakhapatnam', '2nd AC', 30, 900, DATEADD(HOUR,20,CAST(GETDATE() AS DATETIME))),
('1009', 'Kalinga Express', 'Chennai', 'Visakhapatnam', '3rd AC', 30, 700, DATEADD(HOUR,20,CAST(GETDATE() AS DATETIME)));

-- Visakhapatnam to Chennai
INSERT INTO TrainDetails (TrainNo, TrainName, Source, Destination, ClassType, SeatsAvailable, Price, DepartureDateTime) VALUES
('1107', 'Visakha Express', 'Visakhapatnam', 'Chennai', 'Sleeper', 30, 240, DATEADD(HOUR,10,CAST(GETDATE() AS DATETIME))),
('1107', 'Visakha Express', 'Visakhapatnam', 'Chennai', '1st AC', 30, 1200, DATEADD(HOUR,10,CAST(GETDATE() AS DATETIME))),
('1107', 'Visakha Express', 'Visakhapatnam', 'Chennai', '2nd AC', 30, 900, DATEADD(HOUR,10,CAST(GETDATE() AS DATETIME))),
('1107', 'Visakha Express', 'Visakhapatnam', 'Chennai', '3rd AC', 30, 700, DATEADD(HOUR,10,CAST(GETDATE() AS DATETIME))),

('1108', 'East Coast Express', 'Visakhapatnam', 'Chennai', 'Sleeper', 30, 240, DATEADD(HOUR,16,CAST(GETDATE() AS DATETIME))),
('1108', 'East Coast Express', 'Visakhapatnam', 'Chennai', '1st AC', 30, 1200, DATEADD(HOUR,16,CAST(GETDATE() AS DATETIME))),
('1108', 'East Coast Express', 'Visakhapatnam', 'Chennai', '2nd AC', 30, 900, DATEADD(HOUR,16,CAST(GETDATE() AS DATETIME))),
('1108', 'East Coast Express', 'Visakhapatnam', 'Chennai', '3rd AC', 30, 700, DATEADD(HOUR,16,CAST(GETDATE() AS DATETIME))),

('1109', 'Kalinga Express', 'Visakhapatnam', 'Chennai', 'Sleeper', 30, 240, DATEADD(HOUR,24,CAST(GETDATE() AS DATETIME))),
('1109', 'Kalinga Express', 'Visakhapatnam', 'Chennai', '1st AC', 30, 1200, DATEADD(HOUR,24,CAST(GETDATE() AS DATETIME))),
('1109', 'Kalinga Express', 'Visakhapatnam', 'Chennai', '2nd AC', 30, 900, DATEADD(HOUR,24,CAST(GETDATE() AS DATETIME))),
('1109', 'Kalinga Express', 'Visakhapatnam', 'Chennai', '3rd AC', 30, 700, DATEADD(HOUR,24,CAST(GETDATE() AS DATETIME)));

-- Chennai <-> Mumbai
INSERT INTO TrainDetails (TrainNo, TrainName, Source, Destination, ClassType, SeatsAvailable, Price, DepartureDateTime) VALUES
('1010', 'Mumbai Express', 'Chennai', 'Mumbai', 'Sleeper', 30, 400, DATEADD(HOUR,6,CAST(GETDATE() AS DATETIME))),
('1010', 'Mumbai Express', 'Chennai', 'Mumbai', '1st AC', 30, 2000, DATEADD(HOUR,6,CAST(GETDATE() AS DATETIME))),
('1010', 'Mumbai Express', 'Chennai', 'Mumbai', '2nd AC', 30, 1600, DATEADD(HOUR,6,CAST(GETDATE() AS DATETIME))),
('1010', 'Mumbai Express', 'Chennai', 'Mumbai', '3rd AC', 30, 1200, DATEADD(HOUR,6,CAST(GETDATE() AS DATETIME))),

('1011', 'Konkan Express', 'Chennai', 'Mumbai', 'Sleeper', 30, 400, DATEADD(HOUR,12,CAST(GETDATE() AS DATETIME))),
('1011', 'Konkan Express', 'Chennai', 'Mumbai', '1st AC', 30, 2000, DATEADD(HOUR,12,CAST(GETDATE() AS DATETIME))),
('1011', 'Konkan Express', 'Chennai', 'Mumbai', '2nd AC', 30, 1600, DATEADD(HOUR,12,CAST(GETDATE() AS DATETIME))),
('1011', 'Konkan Express', 'Chennai', 'Mumbai', '3rd AC', 30, 1200, DATEADD(HOUR,12,CAST(GETDATE() AS DATETIME))),

('1012', 'Deccan Express', 'Chennai', 'Mumbai', 'Sleeper', 30, 400, DATEADD(HOUR,20,CAST(GETDATE() AS DATETIME))),
('1012', 'Deccan Express', 'Chennai', 'Mumbai', '1st AC', 30, 2000, DATEADD(HOUR,20,CAST(GETDATE() AS DATETIME))),
('1012', 'Deccan Express', 'Chennai', 'Mumbai', '2nd AC', 30, 1600, DATEADD(HOUR,20,CAST(GETDATE() AS DATETIME))),
('1012', 'Deccan Express', 'Chennai', 'Mumbai', '3rd AC', 30, 1200, DATEADD(HOUR,20,CAST(GETDATE() AS DATETIME)));

-- Mumbai to Chennai
INSERT INTO TrainDetails (TrainNo, TrainName, Source, Destination, ClassType, SeatsAvailable, Price, DepartureDateTime) VALUES
('1110', 'Mumbai Express', 'Mumbai', 'Chennai', 'Sleeper', 30, 400, DATEADD(HOUR,10,CAST(GETDATE() AS DATETIME))),
('1110', 'Mumbai Express', 'Mumbai', 'Chennai', '1st AC', 30, 2000, DATEADD(HOUR,10,CAST(GETDATE() AS DATETIME))),
('1110', 'Mumbai Express', 'Mumbai', 'Chennai', '2nd AC', 30, 1600, DATEADD(HOUR,10,CAST(GETDATE() AS DATETIME))),
('1110', 'Mumbai Express', 'Mumbai', 'Chennai', '3rd AC', 30, 1200, DATEADD(HOUR,10,CAST(GETDATE() AS DATETIME))),

('1111', 'Konkan Express', 'Mumbai', 'Chennai', 'Sleeper', 30, 400, DATEADD(HOUR,16,CAST(GETDATE() AS DATETIME))),
('1111', 'Konkan Express', 'Mumbai', 'Chennai', '1st AC', 30, 2000, DATEADD(HOUR,16,CAST(GETDATE() AS DATETIME))),
('1111', 'Konkan Express', 'Mumbai', 'Chennai', '2nd AC', 30, 1600, DATEADD(HOUR,16,CAST(GETDATE() AS DATETIME))),
('1111', 'Konkan Express', 'Mumbai', 'Chennai', '3rd AC', 30, 1200, DATEADD(HOUR,16,CAST(GETDATE() AS DATETIME))),

('1112', 'Deccan Express', 'Mumbai', 'Chennai', 'Sleeper', 30, 400, DATEADD(HOUR,24,CAST(GETDATE() AS DATETIME))),
('1112', 'Deccan Express', 'Mumbai', 'Chennai', '1st AC', 30, 2000, DATEADD(HOUR,24,CAST(GETDATE() AS DATETIME))),
('1112', 'Deccan Express', 'Mumbai', 'Chennai', '2nd AC', 30, 1600, DATEADD(HOUR,24,CAST(GETDATE() AS DATETIME))),
('1112', 'Deccan Express', 'Mumbai', 'Chennai', '3rd AC', 30, 1200, DATEADD(HOUR,24,CAST(GETDATE() AS DATETIME)));

-- Chennai <-> Delhi
INSERT INTO TrainDetails (TrainNo, TrainName, Source, Destination, ClassType, SeatsAvailable, Price, DepartureDateTime) VALUES
('1013', 'Rajdhani Express', 'Chennai', 'Delhi', 'Sleeper', 30, 450, DATEADD(HOUR,6,CAST(GETDATE() AS DATETIME))),
('1013', 'Rajdhani Express', 'Chennai', 'Delhi', '1st AC', 30, 2200, DATEADD(HOUR,6,CAST(GETDATE() AS DATETIME))),
('1013', 'Rajdhani Express', 'Chennai', 'Delhi', '2nd AC', 30, 1700, DATEADD(HOUR,6,CAST(GETDATE() AS DATETIME))),
('1013', 'Rajdhani Express', 'Chennai', 'Delhi', '3rd AC', 30, 1300, DATEADD(HOUR,6,CAST(GETDATE() AS DATETIME))),

('1014', 'Duronto Express', 'Chennai', 'Delhi', 'Sleeper', 30, 450, DATEADD(HOUR,12,CAST(GETDATE() AS DATETIME))),
('1014', 'Duronto Express', 'Chennai', 'Delhi', '1st AC', 30, 2200, DATEADD(HOUR,12,CAST(GETDATE() AS DATETIME))),
('1014', 'Duronto Express', 'Chennai', 'Delhi', '2nd AC', 30, 1700, DATEADD(HOUR,12,CAST(GETDATE() AS DATETIME))),
('1014', 'Duronto Express', 'Chennai', 'Delhi', '3rd AC', 30, 1300, DATEADD(HOUR,12,CAST(GETDATE() AS DATETIME))),

('1015', 'Garib Rath Express', 'Chennai', 'Delhi', 'Sleeper', 30, 450, DATEADD(HOUR,20,CAST(GETDATE() AS DATETIME))),
('1015', 'Garib Rath Express', 'Chennai', 'Delhi', '1st AC', 30, 2200, DATEADD(HOUR,20,CAST(GETDATE() AS DATETIME))),
('1015', 'Garib Rath Express', 'Chennai', 'Delhi', '2nd AC', 30, 1700, DATEADD(HOUR,20,CAST(GETDATE() AS DATETIME))),
('1015', 'Garib Rath Express', 'Chennai', 'Delhi', '3rd AC', 30, 1300, DATEADD(HOUR,20,CAST(GETDATE() AS DATETIME)));

-- Delhi to Chennai
INSERT INTO TrainDetails (TrainNo, TrainName, Source, Destination, ClassType, SeatsAvailable, Price, DepartureDateTime) VALUES
('1113', 'Rajdhani Express', 'Delhi', 'Chennai', 'Sleeper', 30, 450, DATEADD(HOUR,10,CAST(GETDATE() AS DATETIME))),
('1113', 'Rajdhani Express', 'Delhi', 'Chennai', '1st AC', 30, 2200, DATEADD(HOUR,10,CAST(GETDATE() AS DATETIME))),
('1113', 'Rajdhani Express', 'Delhi', 'Chennai', '2nd AC', 30, 1700, DATEADD(HOUR,10,CAST(GETDATE() AS DATETIME))),
('1113', 'Rajdhani Express', 'Delhi', 'Chennai', '3rd AC', 30, 1300, DATEADD(HOUR,10,CAST(GETDATE() AS DATETIME))),

('1114', 'Duronto Express', 'Delhi', 'Chennai', 'Sleeper', 30, 450, DATEADD(HOUR,16,CAST(GETDATE() AS DATETIME))),
('1114', 'Duronto Express', 'Delhi', 'Chennai', '1st AC', 30, 2200, DATEADD(HOUR,16,CAST(GETDATE() AS DATETIME))),
('1114', 'Duronto Express', 'Delhi', 'Chennai', '2nd AC', 30, 1700, DATEADD(HOUR,16,CAST(GETDATE() AS DATETIME))),
('1114', 'Duronto Express', 'Delhi', 'Chennai', '3rd AC', 30, 1300, DATEADD(HOUR,16,CAST(GETDATE() AS DATETIME))),

('1115', 'Garib Rath Express', 'Delhi', 'Chennai', 'Sleeper', 30, 450, DATEADD(HOUR,24,CAST(GETDATE() AS DATETIME))),
('1115', 'Garib Rath Express', 'Delhi', 'Chennai', '1st AC', 30, 2200, DATEADD(HOUR,24,CAST(GETDATE() AS DATETIME))),
('1115', 'Garib Rath Express', 'Delhi', 'Chennai', '2nd AC', 30, 1700, DATEADD(HOUR,24,CAST(GETDATE() AS DATETIME))),
('1115', 'Garib Rath Express', 'Delhi', 'Chennai', '3rd AC', 30, 1300, DATEADD(HOUR,24,CAST(GETDATE() AS DATETIME)));

-- Hyderabad <-> Chennai
INSERT INTO TrainDetails (TrainNo, TrainName, Source, Destination, ClassType, SeatsAvailable, Price, DepartureDateTime) VALUES
('1016', 'Hyderabad Express', 'Hyderabad', 'Chennai', 'Sleeper', 30, 350, DATEADD(HOUR,6,CAST(GETDATE() AS DATETIME))),
('1016', 'Hyderabad Express', 'Hyderabad', 'Chennai', '1st AC', 30, 1800, DATEADD(HOUR,6,CAST(GETDATE() AS DATETIME))),
('1016', 'Hyderabad Express', 'Hyderabad', 'Chennai', '2nd AC', 30, 1400, DATEADD(HOUR,6,CAST(GETDATE() AS DATETIME))),
('1016', 'Hyderabad Express', 'Hyderabad', 'Chennai', '3rd AC', 30, 1100, DATEADD(HOUR,6,CAST(GETDATE() AS DATETIME))),

('1017', 'Godavari Express', 'Hyderabad', 'Chennai', 'Sleeper', 30, 350, DATEADD(HOUR,12,CAST(GETDATE() AS DATETIME))),
('1017', 'Godavari Express', 'Hyderabad', 'Chennai', '1st AC', 30, 1800, DATEADD(HOUR,12,CAST(GETDATE() AS DATETIME))),
('1017', 'Godavari Express', 'Hyderabad', 'Chennai', '2nd AC', 30, 1400, DATEADD(HOUR,12,CAST(GETDATE() AS DATETIME))),
('1017', 'Godavari Express', 'Hyderabad', 'Chennai', '3rd AC', 30, 1100, DATEADD(HOUR,12,CAST(GETDATE() AS DATETIME))),

('1018', 'Krishna Express', 'Hyderabad', 'Chennai', 'Sleeper', 30, 350, DATEADD(HOUR,20,CAST(GETDATE() AS DATETIME))),
('1018', 'Krishna Express', 'Hyderabad', 'Chennai', '1st AC', 30, 1800, DATEADD(HOUR,20,CAST(GETDATE() AS DATETIME))),
('1018', 'Krishna Express', 'Hyderabad', 'Chennai', '2nd AC', 30, 1400, DATEADD(HOUR,20,CAST(GETDATE() AS DATETIME))),
('1018', 'Krishna Express', 'Hyderabad', 'Chennai', '3rd AC', 30, 1100, DATEADD(HOUR,20,CAST(GETDATE() AS DATETIME)));

-- Chennai to Hyderabad
INSERT INTO TrainDetails (TrainNo, TrainName, Source, Destination, ClassType, SeatsAvailable, Price, DepartureDateTime) VALUES
('1116', 'Hyderabad Express', 'Chennai', 'Hyderabad', 'Sleeper', 30, 350, DATEADD(HOUR,10,CAST(GETDATE() AS DATETIME))),
('1116', 'Hyderabad Express', 'Chennai', 'Hyderabad', '1st AC', 30, 1800, DATEADD(HOUR,10,CAST(GETDATE() AS DATETIME))),
('1116', 'Hyderabad Express', 'Chennai', 'Hyderabad', '2nd AC', 30, 1400, DATEADD(HOUR,10,CAST(GETDATE() AS DATETIME))),
('1116', 'Hyderabad Express', 'Chennai', 'Hyderabad', '3rd AC', 30, 1100, DATEADD(HOUR,10,CAST(GETDATE() AS DATETIME))),

('1117', 'Godavari Express', 'Chennai', 'Hyderabad', 'Sleeper', 30, 350, DATEADD(HOUR,16,CAST(GETDATE() AS DATETIME))),
('1117', 'Godavari Express', 'Chennai', 'Hyderabad', '1st AC', 30, 1800, DATEADD(HOUR,16,CAST(GETDATE() AS DATETIME))),
('1117', 'Godavari Express', 'Chennai', 'Hyderabad', '2nd AC', 30, 1400, DATEADD(HOUR,16,CAST(GETDATE() AS DATETIME))),
('1117', 'Godavari Express', 'Chennai', 'Hyderabad', '3rd AC', 30, 1100, DATEADD(HOUR,16,CAST(GETDATE() AS DATETIME))),

('1118', 'Krishna Express', 'Chennai', 'Hyderabad', 'Sleeper', 30, 350, DATEADD(HOUR,24,CAST(GETDATE() AS DATETIME))),
('1118', 'Krishna Express', 'Chennai', 'Hyderabad', '1st AC', 30, 1800, DATEADD(HOUR,24,CAST(GETDATE() AS DATETIME))),
('1118', 'Krishna Express', 'Chennai', 'Hyderabad', '2nd AC', 30, 1400, DATEADD(HOUR,24,CAST(GETDATE() AS DATETIME))),
('1118', 'Krishna Express', 'Chennai', 'Hyderabad', '3rd AC', 30, 1100, DATEADD(HOUR,24,CAST(GETDATE() AS DATETIME)));

-- Bangalore <-> Chennai
INSERT INTO TrainDetails (TrainNo, TrainName, Source, Destination, ClassType, SeatsAvailable, Price, DepartureDateTime) VALUES
('1019', 'Bangalore Express', 'Bangalore', 'Chennai', 'Sleeper', 30, 300, DATEADD(HOUR,6,CAST(GETDATE() AS DATETIME))),
('1019', 'Bangalore Express', 'Bangalore', 'Chennai', '1st AC', 30, 1600, DATEADD(HOUR,6,CAST(GETDATE() AS DATETIME))),
('1019', 'Bangalore Express', 'Bangalore', 'Chennai', '2nd AC', 30, 1200, DATEADD(HOUR,6,CAST(GETDATE() AS DATETIME))),
('1019', 'Bangalore Express', 'Bangalore', 'Chennai', '3rd AC', 30, 900, DATEADD(HOUR,6,CAST(GETDATE() AS DATETIME))),

('1020', 'Chamundi Express', 'Bangalore', 'Chennai', 'Sleeper', 30, 300, DATEADD(HOUR,12,CAST(GETDATE() AS DATETIME))),
('1020', 'Chamundi Express', 'Bangalore', 'Chennai', '1st AC', 30, 1600, DATEADD(HOUR,12,CAST(GETDATE() AS DATETIME))),
('1020', 'Chamundi Express', 'Bangalore', 'Chennai', '2nd AC', 30, 1200, DATEADD(HOUR,12,CAST(GETDATE() AS DATETIME))),
('1020', 'Chamundi Express', 'Bangalore', 'Chennai', '3rd AC', 30, 900, DATEADD(HOUR,12,CAST(GETDATE() AS DATETIME))),

('1021', 'Mysore Express', 'Bangalore', 'Chennai', 'Sleeper', 30, 300, DATEADD(HOUR,20,CAST(GETDATE() AS DATETIME))),
('1021', 'Mysore Express', 'Bangalore', 'Chennai', '1st AC', 30, 1600, DATEADD(HOUR,20,CAST(GETDATE() AS DATETIME))),
('1021', 'Mysore Express', 'Bangalore', 'Chennai', '2nd AC', 30, 1200, DATEADD(HOUR,20,CAST(GETDATE() AS DATETIME))),
('1021', 'Mysore Express', 'Bangalore', 'Chennai', '3rd AC', 30, 900, DATEADD(HOUR,20,CAST(GETDATE() AS DATETIME)));

-- Chennai to Bangalore
INSERT INTO TrainDetails (TrainNo, TrainName, Source, Destination, ClassType, SeatsAvailable, Price, DepartureDateTime) VALUES
('1119', 'Bangalore Express', 'Chennai', 'Bangalore', 'Sleeper', 30, 300, DATEADD(HOUR,10,CAST(GETDATE() AS DATETIME))),
('1119', 'Bangalore Express', 'Chennai', 'Bangalore', '1st AC', 30, 1600, DATEADD(HOUR,10,CAST(GETDATE() AS DATETIME))),
('1119', 'Bangalore Express', 'Chennai', 'Bangalore', '2nd AC', 30, 1200, DATEADD(HOUR,10,CAST(GETDATE() AS DATETIME))),
('1119', 'Bangalore Express', 'Chennai', 'Bangalore', '3rd AC', 30, 900, DATEADD(HOUR,10,CAST(GETDATE() AS DATETIME))),

('1120', 'Chamundi Express', 'Chennai', 'Bangalore', 'Sleeper', 30, 300, DATEADD(HOUR,16,CAST(GETDATE() AS DATETIME))),
('1120', 'Chamundi Express', 'Chennai', 'Bangalore', '1st AC', 30, 1600, DATEADD(HOUR,16,CAST(GETDATE() AS DATETIME))),
('1120', 'Chamundi Express', 'Chennai', 'Bangalore', '2nd AC', 30, 1200, DATEADD(HOUR,16,CAST(GETDATE() AS DATETIME))),
('1120', 'Chamundi Express', 'Chennai', 'Bangalore', '3rd AC', 30, 900, DATEADD(HOUR,16,CAST(GETDATE() AS DATETIME))),

('1121', 'Mysore Express', 'Chennai', 'Bangalore', 'Sleeper', 30, 300, DATEADD(HOUR,24,CAST(GETDATE() AS DATETIME))),
('1121', 'Mysore Express', 'Chennai', 'Bangalore', '1st AC', 30, 1600, DATEADD(HOUR,24,CAST(GETDATE() AS DATETIME))),
('1121', 'Mysore Express', 'Chennai', 'Bangalore', '2nd AC', 30, 1200, DATEADD(HOUR,24,CAST(GETDATE() AS DATETIME))),
('1121', 'Mysore Express', 'Chennai', 'Bangalore', '3rd AC', 30, 900, DATEADD(HOUR,24,CAST(GETDATE() AS DATETIME)));

-- Visakhapatnam <-> Bangalore
INSERT INTO TrainDetails (TrainNo, TrainName, Source, Destination, ClassType, SeatsAvailable, Price, DepartureDateTime) VALUES
('1022', 'Eastern Express', 'Visakhapatnam', 'Bangalore', 'Sleeper', 30, 600, DATEADD(HOUR,6,CAST(GETDATE() AS DATETIME))),
('1022', 'Eastern Express', 'Visakhapatnam', 'Bangalore', '1st AC', 30, 2500, DATEADD(HOUR,6,CAST(GETDATE() AS DATETIME))),
('1022', 'Eastern Express', 'Visakhapatnam', 'Bangalore', '2nd AC', 30, 2200, DATEADD(HOUR,6,CAST(GETDATE() AS DATETIME))),
('1022', 'Eastern Express', 'Visakhapatnam', 'Bangalore', '3rd AC', 30, 1800, DATEADD(HOUR,6,CAST(GETDATE() AS DATETIME))),

('1023', 'Southern Express', 'Visakhapatnam', 'Bangalore', 'Sleeper', 30, 600, DATEADD(HOUR,12,CAST(GETDATE() AS DATETIME))),
('1023', 'Southern Express', 'Visakhapatnam', 'Bangalore', '1st AC', 30, 2500, DATEADD(HOUR,12,CAST(GETDATE() AS DATETIME))),
('1023', 'Southern Express', 'Visakhapatnam', 'Bangalore', '2nd AC', 30, 2200, DATEADD(HOUR,12,CAST(GETDATE() AS DATETIME))),
('1023', 'Southern Express', 'Visakhapatnam', 'Bangalore', '3rd AC', 30, 1800, DATEADD(HOUR,12,CAST(GETDATE() AS DATETIME))),

('1024', 'Deccan Express', 'Visakhapatnam', 'Bangalore', 'Sleeper', 30, 600, DATEADD(HOUR,20,CAST(GETDATE() AS DATETIME))),
('1024', 'Deccan Express', 'Visakhapatnam', 'Bangalore', '1st AC', 30, 2500, DATEADD(HOUR,20,CAST(GETDATE() AS DATETIME))),
('1024', 'Deccan Express', 'Visakhapatnam', 'Bangalore', '2nd AC', 30, 2200, DATEADD(HOUR,20,CAST(GETDATE() AS DATETIME))),
('1024', 'Deccan Express', 'Visakhapatnam', 'Bangalore', '3rd AC', 30, 1800, DATEADD(HOUR,20,CAST(GETDATE() AS DATETIME)));

-- Bangalore to Visakhapatnam
INSERT INTO TrainDetails (TrainNo, TrainName, Source, Destination, ClassType, SeatsAvailable, Price, DepartureDateTime) VALUES
('1122', 'Eastern Express', 'Bangalore', 'Visakhapatnam', 'Sleeper', 30, 600, DATEADD(HOUR,10,CAST(GETDATE() AS DATETIME))),
('1122', 'Eastern Express', 'Bangalore', 'Visakhapatnam', '1st AC', 30, 2500, DATEADD(HOUR,10,CAST(GETDATE() AS DATETIME))),
('1122', 'Eastern Express', 'Bangalore', 'Visakhapatnam', '2nd AC', 30, 2200, DATEADD(HOUR,10,CAST(GETDATE() AS DATETIME))),
('1122', 'Eastern Express', 'Bangalore', 'Visakhapatnam', '3rd AC', 30, 1800, DATEADD(HOUR,10,CAST(GETDATE() AS DATETIME))),

('1123', 'Southern Express', 'Bangalore', 'Visakhapatnam', 'Sleeper', 30, 600, DATEADD(HOUR,16,CAST(GETDATE() AS DATETIME))),
('1123', 'Southern Express', 'Bangalore', 'Visakhapatnam', '1st AC', 30, 2500, DATEADD(HOUR,16,CAST(GETDATE() AS DATETIME))),
('1123', 'Southern Express', 'Bangalore', 'Visakhapatnam', '2nd AC', 30, 2200, DATEADD(HOUR,16,CAST(GETDATE() AS DATETIME))),
('1123', 'Southern Express', 'Bangalore', 'Visakhapatnam', '3rd AC', 30, 1800, DATEADD(HOUR,16,CAST(GETDATE() AS DATETIME))),

('1124', 'Deccan Express', 'Bangalore', 'Visakhapatnam', 'Sleeper', 30, 600, DATEADD(HOUR,24,CAST(GETDATE() AS DATETIME))),
('1124', 'Deccan Express', 'Bangalore', 'Visakhapatnam', '1st AC', 30, 2500, DATEADD(HOUR,24,CAST(GETDATE() AS DATETIME))),
('1124', 'Deccan Express', 'Bangalore', 'Visakhapatnam', '2nd AC', 30, 2200, DATEADD(HOUR,24,CAST(GETDATE() AS DATETIME))),
('1124', 'Deccan Express', 'Bangalore', 'Visakhapatnam', '3rd AC', 30, 1800, DATEADD(HOUR,24,CAST(GETDATE() AS DATETIME)));
