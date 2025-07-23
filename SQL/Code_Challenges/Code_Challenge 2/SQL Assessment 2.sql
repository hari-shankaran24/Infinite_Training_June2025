use AssessmentsDB

--Query 1
--1. Write a query to display your birthday( day of week)
select datename(weekday, cast('2003-08-20' as date)) as bday


--Query 2
--2. Write a query to display your age in days
select datediff(day, '2003-08-20', getdate()) as age


--Query 3
--3. Write a query to display all employees information those who joined before 5 years in the current month
--(Hint : If required update some HireDates in your EMP table of the assignment)

use SQL_Assignments
select * from emp

update emp
set hire_date='15-jul-18'
where ename in ('allen','smith')

select * from emp
where month(hire_date) = month(getdate()) 
and datediff(year, hire_date, getdate()) >= 5

--Query 4

begin tran

--insert
insert into emp(empno, ename, job, mgr_id, hire_date,salary,comm,deptno) values
(8001,'john','clerk',7902,'01-jan-20',1000,null,20),
(8002,'jane','salesman',7698,'01-feb-20',2000,200,30),
(8003,'jake','analyst',7566,'01-mar-20',2500,null,20);
--update
update emp
set salary = salary + salary * 0.15
where empno = 8002;
--delete
delete from emp
where empno = 8001;

insert into emp (empno, ename, job, mgr_id, hire_date, salary, comm, deptno)
values (8001, 'john', 'clerk', 7902, '01-jan-20', 1000, null, 20);

commit tran
select * from emp

--Query 5
--Create a user defined function calculate Bonus for all employees of a  given dept using 	following conditions
--a. For Deptno 10 employees 15% of sal as bonus.
--b. For Deptno 20 employees  20% of sal as bonus
--c. For Others employees 5%of sal as bonus

create or alter function dbo.calculateBonus(@deptno int, @salary int)
returns decimal(10,2)
as
begin
  declare @bonus decimal(10,2)

  if @deptno = 10
    set @bonus = @salary * 0.15
  else if @deptno = 20
    set @bonus = @salary * 0.20
  else
    set @bonus = @salary * 0.05

  return @bonus
end

select empno, ename, deptno, salary, dbo.calculateBonus(deptno, salary) as Bonus
from emp

--Query 6
--Create a procedure to update the salary of employee by 500 
--whose dept name is Sales and current salary is below 1500 
--(use emp table)

create or alter procedure salUpdate
as
begin
  update emp 
  set salary = salary + 500
  from emp e 
  join dept d on e.deptno = d.deptno 
  where d.dname = 'sales' and e.salary < 1500;
end

exec salUpdate;

select * from emp

