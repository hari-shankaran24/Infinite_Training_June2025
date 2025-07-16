Create Database AssessmentsDB;

use AssessmentsDB



---------------------------------------------------------Question 1
Create table Books
(ID int primary key,
title varchar(20),
author varchar(20),
isbn varchar(20) Unique,
published_date datetime
);

insert into Books(ID, title, author, isbn, published_date) Values
(1, 'My First SQL book', 'Mary Parker', '98148309127', '2012-02-22 12:08:17'),
(2, 'My Second SQL book', 'John Mayer', '857300923713', '1972-07-03 09:22:45'),
(3, 'My Third SQL book', 'Cory Flint', '523120967812', '2015-10-18 14:05:44');

------------------------------------------------------------Query 1
select * from Books where author like '%er'


Create table Reviews
(id int primary key,
book_id int,
reviewer_name varchar(25),
content text,
rating int,
published_date datetime
foreign key (book_id) references books(ID)
);


insert into Reviews(id, book_id, reviewer_name, content, rating, published_date) Values
(1, 1, 'John Smith', 'My first review', 4, '2017-12-10 05:50:17'),
(2, 2, 'John Smith', 'My second review', 5, '2017-10-13 15:05:12'),
(3, 2, 'Alice Walker', 'Another review', 1, '2017-10-22 23:47:10');

--------------------------------------------------------------Query 2
Select
    b.title, b.author, r.reviewer_name 
From
    Books AS b
Join 
    Reviews AS r ON r.book_id = b.ID;

-----------------------------------------------------------------Query 3

Select reviewer_name, COUNT(book_id) AS 'books reviewed'
from Reviews
Group by reviewer_name
Having Count(book_id) > 1;

------------------------------------------------------------------------------Question 2
create table Customer 
(ID int primary key,
 Name varchar(20),
  Age int,
  Address varchar(20),
  Salary float)
 

insert into customer(ID, Name, Age, Address,Salary)
			values(1,'Ramesh',32,'Ahmedabad',2000.00),
 (2, 'Khilan', 25, 'Delhi', 1500.00),
(3, 'Kaushik', 23, 'Kota', 2000.00),
 (4, 'Chaitali', 25, 'Mumbai', 6500.00),
(5, 'Hardik', 27, 'Bhopal', 8500.00),
(6, 'Komal', 22, 'MP', 4500.00),
 (7, 'Muffy', 24, 'Indore', 10000.00)

 
create table Orders
(OID int primary key, 
Order_Date Datetime,
Customer_ID int,
Amount float,
constraint CID_fk foreign key (Customer_ID) references Customer(ID))

insert into Orders
(OID, Order_Date, Customer_ID, Amount)
values(102, '2009-10-08', 3, 3000),
(100, '2009-10-08', 3, 1500),
(101, '2009-11-20', 2, 1560),
(103, '2008-05-20', 4, 2060)
 
 select * from Orders

-------------------------------------------------------------Query 4
select name, address from customer 
where Address like '%o%'

-------------------------------------------------------------Query 5
Select Order_Date, COUNT(Distinct Customer_ID) AS Total_Customers
From Orders
Group By Order_Date;



------------------------------------------------------------------------------Question 3

create table employee (
    id int,
    name varchar(50),
    age int,
    address varchar(100),
    salary decimal(10, 2)
);

insert into employee (id, name, age, address, salary) values
(1, 'ramesh', 32, 'ahmedabad', 2000.00),
(2, 'khilan', 25, 'delhi', 1500.00),
(3, 'kaushik', 23, 'kota', null),
(4, 'chaitali', 25, 'mumbai', 6500.00),
(5, 'hardik', 27, 'bhopal', null),
(6, 'komal', 24, 'mp', null),
(7, 'muffy', 24, 'indore', null);

---------------------------------------------------------------Query 6

select lower(name) as lowercase_name
from employee
where salary is null;

------------------------------------------------------------------------------Question 4

create table StudentDetails
(RegisterNo int,Name varchar(30),
Age int,Qualification varchar(30),
MobileNo numeric,Mail_id varchar(60),
Location varchar(30),Gender varchar(1))
 
insert into StudentDetails values(2,'Sai',22,'B.E',9952836777,'Sai@gmail.com','Chennai','M'),
(3,'Kumar',20,'B.SC',7890125648,'Kumar@gmail.com','Madurai','M'),
(4,'Selvi',22,'B.Tech',890467342,'selvi@gmail.com','Salem','F'),
(5,'Nisha',25,'M.E',7834672310,'Nisha@gmail.com','Theni','F'),
(6,'SaiSaran',21,'B.A',7890345678,'saran@gmail.com','Madurai','F'),
(7,'Tom',23,'BCA',8901234675,'Tom@gmail.com','Pune','M')

---------------------------------------------------------------Query 7

select gender, count(*) as total_count
from studentdetails
group by gender;



select * from Books AssessmentsDB
select * from Reviews AssessmentsDB
select * from Customer AssessmentDB
select * from StudentDetails

