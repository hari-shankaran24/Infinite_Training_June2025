create database SQL_Assignments

use SQL_Assignments


--Creating Clients Table
create table Clients
(
Client_ID numeric(4) primary key,
Cname varchar(40) not null,
Address varchar(30),
Email varchar(30) unique,
Phone numeric(10),
Buisness varchar(20) not null
);

--Inserting Values into the Table
insert into Clients values
(1001, 'ACME Utilities','Noida','contact@acmeutil.com', 9567880032,'manufacturing'),
(1002, 'Trackon Consultants', 'Mumbai', 'consult@trackon.com',8734210090,'Consultant'),
(1003, 'MoneySaver Distributors','Kolkata','save@moneysaver.com',779886655,'Reseller'),
(1004,'Lawful Corp','Chennai','justice@lawful.com',9210342219,'Professional')


--Creating Employees Table
create table Employees
(
Empno numeric(4) primary key,
Ename varchar(20) not null,
Job varchar(15),
Salary numeric(7) check(salary>0),
Deptno numeric(2) foreign key references Departments(Deptno)
);

--Inserting values into the Employees Table
insert into Employees values
(7001,'Sandeep','Analyst',25000,10),
(7002,'Rajest','Designer',30000,10),
(7003,'Madhav','Developer',40000,10),
(7004,'Manoj','Developer',40000,10),
(7005,'Abhay','Designer',35000,10),
(7006,'Uma','Tester',30000,10),
(7007,'Gita','Tech Writer',30000,10),
(7008,'Priya','Tester',35000,10),
(7009,'Nutan','Developer',45000,10),
(7010,'Smita','Analyst',20000,10),
(7011,'Anand','Project Mgr',65000,10)

--Creating Table Departments
create table Departments
(Deptno numeric(2) primary key,
Dname varchar(15) not null,
Loc varchar(20)
);

--Inserting values into the Departments
insert into Departments values 
(10, 'Design','Pune'),
(20, 'Development','Pune'),
(30, 'Testing','Mumbai'),
(40, 'Document','Mumbai')

--Creating Projects Table
 
Create table Projects (
Project_ID int primary key,
Descr varchar(30) not null,
Start_Date Date, 
Planned_End_Date Date, 
Actual_End_Date Date,
Budget bigint check (Budget > 0),
Client_ID numeric(4) references Clients(Client_ID),
Constraint Check_Actual_End_Date check (Actual_End_Date > Planned_End_Date));

--IN=erting values to Projects table
insert into Projects values
(401,'Inventory','2011-04-01','2011-10-01','2011-10-31',150000,1001),
(402,'Accounting','2011-08-01','2012-01-01',null,500000,1002),
(403,'PayRoll','2011-10-01','2011-12-31',null,75000,1003),
(404,'Contacct mgnt','2011-04-01','2011-10-01',null,50000,1004);


--Creating EmpProjectTasks Table
create table EmpProjectTasks(
Project_ID int,
Empno numeric(4),
Start_Date date,
End_Date date,
Task varchar(25)not null,
Status varchar(15)not null,
constraint pk primary key (Project_Id,Empno),
constraint fk_Pro foreign key(Project_ID) references projects(Project_ID),
constraint fk_Emp foreign key(Empno) references Employees(empno))
 
 --Inserting values into EmpProjectTasks Table
 insert into empprojecttasks values(401, 7001, '2011-04-01', '2011-04-20', 'system analysis', 'completed'),
(401, 7002, '2011-04-21', '2011-05-30', 'system design', 'completed'),
(401, 7003, '2011-06-01', '2011-07-15', 'coding', 'completed'),
(401, 7004, '2011-07-18', '2011-09-01', 'coding', 'completed'),
(401, 7006, '2011-09-03', '2011-09-15', 'testing', 'completed'),
(401, 7009, '2011-09-18', '2011-10-05', 'code change', 'completed'),
(401, 7008, '2011-10-06', '2011-10-16', 'testing', 'completed'),
(401, 7007, '2011-10-06', '2011-10-22', 'documentation', 'completed'),
(401, 7011, '2011-10-22', '2011-10-31', 'sign off', 'completed'),
(402, 7010, '2011-08-01', '2011-08-20', 'system analysis', 'completed'),
(402, 7002, '2011-08-22', '2011-09-30', 'system design', 'completed'),
(402, 7004, '2011-10-01', null, 'coding', 'in progress')
 

 --Selecting Tables

 select * from clients    
select * from employees
select * from departments
select * from projects   
select * from empprojecttasks 
 