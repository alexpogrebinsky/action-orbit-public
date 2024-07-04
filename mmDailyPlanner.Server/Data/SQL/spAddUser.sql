CREATE PROCEDURE dbo.spAddUser
    @Username NVARCHAR(50),
    @Email NVARCHAR(50),
    @Password NVARCHAR(50),
    @AddressIP NVARCHAR(50),
    @FirstName NVARCHAR(50),
    @IsAuthenticated BIT,
    @LastName NVARCHAR(50),
    @PhoneNumber NVARCHAR(50),
    @Role NVARCHAR(50),
    @ProfileImage VARBINARY(MAX),
    @Salt VARBINARY(16)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Users (Username, Email, Password, AddressIP, FirstName, IsAuthenticated, LastName, PhoneNumber, Role, ProfileImage, Salt)
    VALUES (@Username, @Email, @Password, @AddressIP, @FirstName, @IsAuthenticated, @LastName, @PhoneNumber, @Role, @ProfileImage, @Salt)

    SELECT TOP 1 * 
    FROM Users
    WHERE Username = @Username AND Email = @Email
END
