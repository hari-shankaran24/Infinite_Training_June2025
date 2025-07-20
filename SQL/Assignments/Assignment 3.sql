use SQL_Assignments

select * from emp
select * from dept

--Query 1
select ename as managers from emp
where job='manager';

--Query 2
select ename,salary from emp
where salary>1000;

--Query 3
select ename,salary from emp
where ename<>'james';

--Query 4
select * from emp
where ename like 's%';

--Query 5
select ename from emp
where ename like '%a%';

--Query 6
select ename from emp
where ename like '__l%';

--Query 7
select ename,salary/30 as [daily salary] from emp
where ename='jones';

--Query 8
select sum(salary) as [total monthly salary] from emp;

--Query 9
select avg(salary*12) as [total monthly salary] from emp;

--Query 10
select ename,job,salary,deptno from emp
where job!='salesman' AND deptno=30;

--Query 11
select distinct deptno from emp;

--Query 12
select ename,salary from emp
where salary>1500 and (deptno=10 or deptno=30);

--Query 13
select ename, job, salary from emp
where job='manager' or job='analyst' and salary not in(1000,3000,5000);

--Query 14
select ename,salary,comm from emp
where comm>salary*1.10;

--Query 15
select ename from emp
where (ename like '%l%l%' and deptno in (30)) or mgr_id=7782;

--Query 16
select ename,
round(datediff(month, cast(hire_date as date), getdate()) / 12.0, 1) as experience_years
from emp
where datediff(year, cast(hire_date as date), getdate()) between 30 and 40;

select count(*)
from emp
where datediff(year, cast(hire_date as date), getdate()) between 30 and 40;

--Query 17
select d.dname, e.ename 
from dept d
join emp e on d.deptno=e.deptno
order by d.dname asc, e.ename desc;

--Query 18
select ename,
round(datediff(month, cast(hire_date as date), getdate()) / 12.0, 1) as experience_years
from emp
where ename='miller';