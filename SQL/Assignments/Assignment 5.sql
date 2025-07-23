--Query 1

create or alter proc sp_Payslip(@empno int)
as 
begin
declare @ename varchar(30)
declare @sal float
declare @hra float
declare @da float
declare @pf float
declare @it float
declare @ded float
declare @gross float
declare @net float
select @ename=ename,@sal=salary from emp where empno=@empno
 
if @ename is null
begin
print 'Employee data not found.'
return
end
 
set @hra=@sal*0.10
set @da=@sal*0.20
set @pf=@sal*0.08
set @it=@sal*0.05
set @ded=@pf+@it
set @gross=@sal+@hra+@da
set @net=@gross-@ded
 
print 'Employee Number: ' + cast(@empno as varchar(10))
print 'Employee Name: ' + @ename
print 'Basic Salary: ' + cast(@sal as varchar(10))
print 'HRA (10%): ' + cast(@Hra as varchar(10))
print 'DA  (20%): ' + cast(@DA as varchar(10))
print 'PF  (8%): ' + cast(@pf as varchar(10))
print 'IT  (5%): ' + cast(@it as varchar(10))
print 'Deductions: ' + cast(@ded as varchar(10))
print 'Gross Salary: ' + cast(@gross as varchar(10))
print 'Net Salary: ' + cast(@net as varchar(10))
 
end
 
select * from emp
 
exec sp_payslip 7369
exec sp_payslip 7490
 

--Query 2

create table Holiday
(Holiday_Date date,
Holiday_Name varchar(20))
 
insert into holiday values
('2025-01-26', 'Republic Day'),
('2025-08-15', 'Independence Day'),
('2025-10-20', 'Diwali'),
('2025-12-25', 'Christmas')

select * from Holiday

create or alter trigger trg_On_Holiday
on emp for insert,delete,update 
as
begin
declare @hname varchar(20)
select @hname=holiday_name from holiday where Holiday_date=cast(getdate() as date)
 
if @hname is not null
begin
raiserror('Due to %s,you cannot manipulate data',16,1,@hname)
rollback
end
end
  
select cast(getdate() as date) as today;
update emp set salary = salary + 1000 where empno = 7369;
insert into holiday values (cast(getdate() as date), 'holiday');
update emp set salary = salary + 500 where empno = 7369;

