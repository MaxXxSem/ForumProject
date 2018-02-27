USE ForumDB
GO

CREATE TRIGGER User_DELETE
ON Users
INSTEAD OF DELETE
AS
DECLARE @DeletedUserId int
SET  @DeletedUserId = (SELECT TOP(1) Id FROM DELETED)

DELETE FROM RecordsLikes WHERE UserId = @DeletedUserId
DELETE FROM CommentsLikes WHERE UserId = @DeletedUserId
DELETE FROM Comments WHERE UserId = @DeletedUserId
DELETE FROM Subscribes WHERE SubscriberId = @DeletedUserId OR PublisherId = @DeletedUserId
DELETE FROM Users WHERE Id = @DeletedUserId
GO

CREATE TRIGGER ApplicationUser_DELETE
ON ApplicationUsers
AFTER DELETE
AS
DELETE FROM Users WHERE Users.Id = (SELECT TOP(1) UserId FROM DELETED)
GO