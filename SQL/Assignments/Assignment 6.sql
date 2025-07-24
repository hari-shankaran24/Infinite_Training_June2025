--Query 1
create table client_rental (
  clientno varchar(10),
  cname varchar(100),
  propertyno varchar(10),
  paddress varchar(200),
  rentstart date,
  rentfinish date,
  rent decimal(10, 2),
  ownerno varchar(10),
  oname varchar(100)
);

insert into client_rental (clientno, cname, propertyno, paddress, rentstart, rentfinish, rent, ownerno, oname) values
('cr76', 'john kay', 'pg4', '6 lawrence st, glasgow', '2001-10-01', '2001-08-31', 350, 'co40', 'tina murphy'),
('cr76', 'john kay', 'pg16', '5 novar dr, glasgow', '2002-09-01', '2002-09-01', 450, 'co93', 'tony shaw'),
('cr76', 'john kay', 'pg4', '6 lawrence st, glasgow', '1999-09-01', '2000-06-10', 350, 'co40', 'tina murphy'),
('cr56', 'aline stewart', 'pg36', '2 manor rd, glasgow', '2000-10-10', '2001-12-01', 370, 'co93', 'tony shaw'),
('cr56', 'aline stewart', 'pg16', '5 novar dr, glasgow', '2002-11-01', '2003-08-01', 450, 'co93', 'tony shaw');

----------------------------------------Client Table
create table client (
  clientno varchar(10) primary key,
  cname varchar(100)
);

insert into client (clientno, cname) values
('cr76', 'john kay'),
('cr56', 'aline stewart');

----------------------------------------Property Table
create table prtydet (
  propertyno varchar(10) primary key,
  paddress varchar(200),
  rent decimal(10,2),
  ownerno varchar(10)
);

insert into prtydet (propertyno, paddress, rent, ownerno) values
('pg4', '6 lawrence st, glasgow', 350, 'co40'),
('pg16', '5 novar dr, glasgow', 450, 'co93'),
('pg36', '2 manor rd, glasgow', 370, 'co93');

----------------------------------------Clientrent2f
create table clientrent2f (
  clientno varchar(10),
  propertyno varchar(10),
  rentstart date,
  rentfinish date,
  primary key (clientno, propertyno, rentstart),
  foreign key (clientno) references client(clientno),
  foreign key (propertyno) references propertydetails(propertyno)
);

insert into clientrent2f (clientno, propertyno, rentstart, rentfinish) values
('cr76', 'pg4', '2001-10-01', '2001-08-31'),
('cr76', 'pg16', '2002-09-01', '2002-09-01'),
('cr76', 'pg4', '1999-09-01', '2000-06-10'),
('cr56', 'pg36', '2000-10-10', '2001-12-01'),
('cr56', 'pg16', '2002-11-01', '2003-08-01');

----------------------------------------
create table owner (
  ownerno varchar(10) primary key,
  oname varchar(100)
);

insert into owner (ownerno, oname) values
('co40', 'tina murphy'),
('co93', 'tony shaw');

----------------------------------------
create table prtydet3f (
  propertyno varchar(10) primary key,
  paddress varchar(200),
  rent decimal(10,2),
  ownerno varchar(10),
  foreign key (ownerno) references owner(ownerno)
);


insert into prtydet3f values
('pg4', '6 lawrence st, glasgow', 350, 'co40'),
('pg16', '5 novar dr, glasgow', 450, 'co93'),
('pg36', '2 manor rd, glasgow', 370, 'co93');
