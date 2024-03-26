use ChatDemo

select * from sys.tables
select * from AspNetRoles
select * from AspNetUsers
select * from AspNetUserRoles


insert into AspNetUserRoles values('a3a57a6f-c8d3-46f8-adb2-ea3361dd719a', '58e3e1ef-075a-41d9-b97d-05ec422bd4ef')
insert into AspNetUserRoles values('db59ddcc-3ee1-43c8-b581-a512a041eb47', '86ed554a-66e3-49c3-a024-1461eea1b465')


--delete from AspNetUsers

select * from dbo.[Messages]
select * from ChatRooms
select * from ChatRoomUser
insert into ChatRoomUser values('f3e9ef31-d7c6-4c9e-b3e7-af8ea16c8fe0',1)
delete from ChatRoomUser


select * from Biddings
select * from ProductImages
delete  from ProductImages where ProductId = 7
select * from Products
select * from ProductImages

select * from Products order by CURRENT_TIMESTAMP asc
select * from ProductInStatus join ProductStatuses on ProductStatuses.ProductStatusId = ProductInStatus.ProductStatusId
select * from ProductStatuses
select * from chatroomproducts

delete from ProductInStatus where ProductStatusId = 2

select * from ChatRooms
update chatrooms set StartDate = '2024-03-10 00:00:00.0000000'
update chatrooms set enddate = '2024-04-12 15:45:00.0000000'
select * from ChatRoomUser
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