use ChatDemo

select * from sys.tables
select * from AspNetRoles
select * from AspNetUsers
select * from AspNetUserRoles

insert into AspNetUserRoles values('7a16dca0-6cf7-4514-949f-5c21e290c471', 'c06f4997-08ee-486c-b2fc-fda7923b33b5')
insert into AspNetUserRoles values('b2d74338-b011-4363-ac28-44e21c821405', '69c2abb4-3227-4fcb-a63b-14b4f505e02f')


--delete from AspNetUsers

select * from dbo.[Messages]
select * from ChatRoom
select * from ChatRoomUser
insert into ChatRoomUser values('f3e9ef31-d7c6-4c9e-b3e7-af8ea16c8fe0',1)



select * from Biddings
select * from ProductImages
delete  from ProductImages where ProductId = 7
select * from Products
select * from ProductImages

select * from ProductInStatus
select * from ProductStatuses
select * from chatroomproducts


select * from ChatRooms
update chatrooms set StartDate = '2024-03-10 00:00:00.0000000'
update chatrooms set enddate = '2024-04-12 15:45:00.0000000'
select * from ChatRoomUser
select * from chatroomproducts
update ChatRoomProducts set BiddingStartTime = '2024-03-13 14:34:00.0000000' where ChatRoomProductId = 2
update ChatRoomProducts set BiddingEndTime = '2024-03-13 20:00:00.0000000' where ChatRoomProductId = 2

update ChatRoomProducts set BiddingStartTime = '2024-03-14 14:05:00.0000000' where ChatRoomProductId = 1003
update ChatRoomProducts set BiddingEndTime = '2024-03-15 21:28:30.0000000' where ChatRoomProductId = 1003

--delete  from ChatRoomProducts

delete from Products where ProductId < 7

sp_help 'Products'