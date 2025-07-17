--Assignment 2

use SQL_Assignments


-- create department table
create table dept (
    deptno int primary key,
    dname varchar(30),
    loc varchar(30)
);

-- insert data into department
insert into dept values 
(10, 'accounting', 'new york'),
(20, 'research', 'dallas'),
(30, 'sales', 'chicago'),
(40, 'operations', 'boston');

-- creating employee table
create table emp (
    empno int primary key,
    ename varchar(30) not null,
    job varchar(30) not null,
    mgr_id int,
    hire_date varchar(30),
    salary int,
    comm int,
    deptno int references dept(deptno)
);

-- inserting data into employee
insert into emp values 
(7369, 'smith', 'clerk', 7902, '17-dec-80', 800, null, 20),
(7499, 'allen', 'salesman', 7698, '20-feb-81', 1600, 300, 30),
(7521, 'ward', 'salesman', 7698, '22-feb-81', 1250, 500, 30),
(7566, 'jones', 'manager', 7839, '02-apr-81', 2975, null, 20),
(7654, 'martin', 'salesman', 7698, '28-sep-81', 1250, 1400, 30),
(7698, 'blake', 'manager', 7839, '01-may-81', 2850, null, 30),
(7782, 'clark', 'manager', 7839, '09-jun-81', 2450, null, 10),
(7788, 'scott', 'analyst', 7566, '19-apr-87', 3000, null, 20),
(7839, 'king', 'president', null, '17-nov-81', 5000, null, 10),
(7844, 'turner', 'salesman', 7698, '08-sep-81', 1500, 0, 30),
(7876, 'adams', 'clerk', 7788, '23-may-87', 1100, null, 20),
(7900, 'james', 'clerk', 7698, '03-dec-81', 950, null, 30),
(7902, 'ford', 'analyst', 7566, '03-dec-81', 3000, null, 20),
(7934, 'miller', 'clerk', 7782, '23-jan-82', 1300, null, 10);

-- selecting  data
select * from dept;
select * from emp;

--Queries

--Query 1
select * from emp where ename like 'a%';


--Query 2
select * from emp where mgr_id is null;


--Query 3
select ename as employee_name, empno as employee_number, salary 
from emp 
where salary between 1200 and 1400;


--Query 4
update emp 
set salary = salary + (salary*0.10)
where deptno = (
    select deptno from dept where dname = 'research'
);

select empno, ename, salary 
from emp 
where deptno = (select deptno from dept where dname = 'research');


--Query 5
select count(job) as no_of_clerk 
from emp 
where job = 'Clerk role';


--Query 6
select job, count(*) as number_of_people, avg(salary) as avg_salary 
from emp 
group by job;


--Query 7
select ename, salary as max_salary 
from emp 
where salary = (select max(salary) from emp);

select ename, salary as min_salary 
from emp 
where salary = (select min(salary) from emp);


--Query 8
select * from dept d 
left join emp e on d.deptno = e.deptno 
where e.empno is null;


--Query 9
select ename, job, salary, deptno 
from emp 
where deptno = 20 and job = 'analyst' and salary > 1200 
order by ename;


--Query 10
select d.dname, d.deptno, count(e.empno) as total_emp, sum(e.salary) as total_salary 
from dept d 
left join emp e on d.deptno = e.deptno 
group by d.dname, d.deptno;


--Query 11
select ename, salary 
from emp 
where ename = 'miller' or ename = 'smith';


--Query 12
select * from emp 
where ename like 'a%' or ename like 'm%';


--Query 13
select ename, salary as monthly_salary, (12 * salary) as yearly_salary 
from emp 
where ename = 'smith';


--Query 14
select ename as name, salary as salary 
from emp 
where salary not between 1500 and 2850;


--Query 15
select e2.ename as manager_name, count(e1.empno) as reportees 
from emp e1 
join emp e2 on e1.mgr_id = e2.empno 
group by e2.ename 
having count(e1.empno) > 2;
