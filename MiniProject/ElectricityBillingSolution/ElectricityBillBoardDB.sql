CREATE DATABASE ElectricityBillBoardDB;

DROP TABLE dbo.AdminUsers;


CREATE TABLE dbo.AdminUsers(
    Username NVARCHAR(50) NOT NULL PRIMARY KEY,
    Password NVARCHAR(50) NOT NULL
);

INSERT INTO dbo.AdminUsers (Username, Password)
VALUES ('admin', 'admin@123');

DROP TABLE dbo.ElectricityBill;

USE ElectricityBillBoardDB;
CREATE TABLE dbo.ElectricityBill(
    bill_id INT IDENTITY(1,1) PRIMARY KEY,
    consumer_number VARCHAR(20) NOT NULL,
    consumer_name VARCHAR(50) NOT NULL,
    units_consumed INT NOT NULL,
    bill_amount DECIMAL(18,2) NOT NULL
);

select * from ElectricityBill




 