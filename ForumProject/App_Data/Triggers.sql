USE ForumDB
GO

-- REPLACE DELETED USER WITH DEFAULT ANONIM (Id = 37)
-- Connections: RecordsLikes -> Users, Comments -> Users, CommentsLikes -> Users, Subscribes -> Users
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

DELETE FROM Users WHERE Id = 36
