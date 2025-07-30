use AssessmentsDB;

create table Employee_Details (
    EmpId int identity(1,1) primary key,
    Name varchar(100),
    Salary decimal(18,2),
    Gender varchar(10)
);

create or alter procedure InsertEmployee
    @Name varchar(100),
    @GivenSal decimal(18,2),
    @Gender varchar(10),
    @GeneratedEmpId int output,
    @CalculatedSal decimal(18,2) output
as
begin
    set @CalculatedSal = @GivenSal * 0.9;
    insert into Employee_Details (Name, Salary, Gender)
    values (@Name, @CalculatedSal, @Gender);
    set @GeneratedEmpId = scope_identity();
end;

declare @EmpId int;
declare @Salary decimal(18,2);

exec InsertEmployee
    @Name = 'Test User',
    @GivenSal = 75000,
    @Gender = 'Male',
    @GeneratedEmpId = @EmpId output,
    @CalculatedSal = @Salary output;

select @EmpId as GeneratedEmpId, @Salary as CalculatedSalary;

select * from Employee_Details



create or alter procedure UpdateSal
    @EmpId int,
    @UpdatedSalary decimal(18,2) output
as
begin
    update Employee_Details
    set Salary = Salary + 100
    where EmpId = @EmpId;

    select @UpdatedSalary = Salary
    from Employee_Details
    where EmpId = @EmpId;
end;
