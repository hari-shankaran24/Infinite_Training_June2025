use SQL_Assignments

--Query 1
declare @num int = 5; 
declare @fact bigint = 1;
declare @count int = 1;

while @count <= @num
begin
    set @fact = @fact * @count;
    set @count = @count + 1;
end
print 'factorial of ' + cast(@num as varchar) +
' is ' + cast(@fact as varchar);

--Query 2
create procedure mulTable
    @num int,
    @limit int
as
begin
    declare @count int = 1;

    while @count <= @limit
    begin
        print cast(@num as varchar) + 'x ' 
		+ cast(@count as varchar) + ' =' + cast(@num * @count as varchar);
        set @count = @count + 1;
    end
end;

exec mulTable @num=5, @limit=10;


--Query 3
create table student (
    sid int primary key,
    sname varchar(50)
);

create table marks (
    mid int primary key,
    sid int foreign key references student(sid),
    score int
);


insert into student (sid, sname) values
(1, 'jack'),
(2, 'rithvik'),
(3, 'jaspreeth'),
(4, 'praveen'),
(5, 'bisa'),
(6, 'suraj');

insert into marks (mid, sid, score) values
(1, 1, 23),
(2, 6, 95),
(3, 4, 98),
(4, 2, 17),
(5, 3, 53),
(6, 5, 13);

select * from student
select * from marks


create function getstat (@score int)
returns varchar(10)
as
begin
    return case 
        when @score >= 50 then 'pass'
        else 'fail'
    end
end;


select s.sid, s.sname, m.score, dbo.getstat(m.score) as status
from student s
join marks m on s.sid = m.sid;

