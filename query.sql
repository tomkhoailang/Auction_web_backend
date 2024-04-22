--drop database ChatDemo
use ChatDemo

select * from sys.tables
select * from AspNetRoles
select * from AspNetUsers
select * from AspNetUserRoles
select * from BiddingFares


insert into AspNetUserRoles values('11d6e7bb-a58c-4e67-891f-5e02f7ffb39f', 'f863172d-0218-4220-8ea2-68136cecfd40')
insert into AspNetUserRoles values('db59ddcc-3ee1-43c8-b581-a512a041eb47', '86ed554a-66e3-49c3-a024-1461eea1b465')


--delete from AspNetUsers

select * from dbo.[Messages]
select * from ChatRooms
select * from ChatRoomUsers
insert into ChatRoomUser values('f3e9ef31-d7c6-4c9e-b3e7-af8ea16c8fe0',1)
delete from ChatRoomUser


select * from Biddings
select * from ProductImages
delete  from ProductImages where ProductId = 7
select * from Products
select * from ProductImages
update products set IsDeleted = 0, DeletedAt = null where ProductId = 2

select * from Products order by CURRENT_TIMESTAMP asc
select * from ProductInStatus join ProductStatuses on ProductStatuses.ProductStatusId = ProductInStatus.ProductStatusId
select * from ProductStatuses
select * from chatroomproducts
select * from ProductInStatuses
delete from ProductInStatus where ProductStatusId = 2

insert into ProductInStatuses ( Timestamp,ProductId, ProductStatusId, IsDeleted, DeletedAt) values(CURRENT_TIMESTAMP, 2,1, 0, null)
insert into ProductInStatuses ( Timestamp,ProductId, ProductStatusId, IsDeleted, DeletedAt) values(CURRENT_TIMESTAMP, 3,1, 0, null)

select * from BiddingFares

select * from Biddings
select * from ChatRooms
update chatrooms set StartDate = '2024-03-10 00:00:00.0000000'
update chatrooms set enddate = '2024-04-12 15:45:00.0000000'
select * from ChatRoomUsers
select * from chatroomproducts
select * from ChatRooms
--delete from ChatRoomProducts
--delete from ChatRoomUser
--delete from ChatRooms
update ChatRoomProducts set BiddingStartTime = '2024-03-13 14:34:00.0000000' where ChatRoomProductId = 2
update ChatRoomProducts set BiddingEndTime = '2024-03-13 20:00:00.0000000' where ChatRoomProductId = 2

update ChatRoomProducts set BiddingStartTime = '2024-03-14 14:05:00.0000000' where ChatRoomProductId = 1003
update ChatRoomProducts set BiddingEndTime = '2024-03-15 21:28:30.0000000' where ChatRoomProductId = 1003

--delete  from ChatRoomProducts

delete from Products where ProductId < 7

sp_help 'Products'

select ' alter table ' + schema_name(Schema_id)+'.'+ object_name(parent_object_id)
+ '  DROP CONSTRAINT  ' +  name   from sys.foreign_keys f1

--drop foreign key
DECLARE @dropScript NVARCHAR(MAX) = '';

SELECT @dropScript = @dropScript + 'ALTER TABLE ' + OBJECT_NAME(parent_object_id) + 
                     ' DROP CONSTRAINT ' + name + ';' + CHAR(13)
FROM sys.foreign_keys;

PRINT @dropScript; -- Print the script for verification

-- Execute the script
EXEC sp_executesql @dropScript;

--drop primary key of chatroomUsers
alter table ChatRoomUsers
drop constraint PK_ChatRoomUsers



 alter table dbo.AspNetRoleClaims  DROP CONSTRAINT  FK_AspNetRoleClaims_AspNetRoles_RoleId
 alter table dbo.AspNetUserClaims  DROP CONSTRAINT  FK_AspNetUserClaims_AspNetUsers_UserId
 alter table dbo.AspNetUserLogins  DROP CONSTRAINT  FK_AspNetUserLogins_AspNetUsers_UserId
 alter table dbo.AspNetUserRoles  DROP CONSTRAINT  FK_AspNetUserRoles_AspNetRoles_RoleId
 alter table dbo.AspNetUserRoles  DROP CONSTRAINT  FK_AspNetUserRoles_AspNetUsers_UserId
 alter table dbo.AspNetUserTokens  DROP CONSTRAINT  FK_AspNetUserTokens_AspNetUsers_UserId
 alter table dbo.ChatRooms  DROP CONSTRAINT  FK_ChatRooms_AspNetUsers_HostUserId
 alter table dbo.Products  DROP CONSTRAINT  FK_Products_AspNetUsers_SellerId
 alter table dbo.ChatRoomUsers  DROP CONSTRAINT  FK_ChatRoomUsers_AspNetUsers_UserId
 alter table dbo.ChatRoomUsers  DROP CONSTRAINT  FK_ChatRoomUsers_ChatRooms_ChatRoomId
 alter table dbo.Messages  DROP CONSTRAINT  FK_Messages_AspNetUsers_SenderId
 alter table dbo.Messages  DROP CONSTRAINT  FK_Messages_ChatRooms_ChatRoomId
 alter table dbo.Biddings  DROP CONSTRAINT  FK_Biddings_AspNetUsers_BiddingUserId
 alter table dbo.Biddings  DROP CONSTRAINT  FK_Biddings_Products_ProductId
 alter table dbo.ChatRoomProducts  DROP CONSTRAINT  FK_ChatRoomProducts_ChatRooms_ChatRoomId
 alter table dbo.ChatRoomProducts  DROP CONSTRAINT  FK_ChatRoomProducts_Products_ProductId
 alter table dbo.ProductImages  DROP CONSTRAINT  FK_ProductImages_Products_ProductId
 alter table dbo.ProductInStatuses  DROP CONSTRAINT  FK_ProductInStatuses_ProductStatuses_ProductStatusId
 alter table dbo.ProductInStatuses  DROP CONSTRAINT  FK_ProductInStatuses_Products_ProductId