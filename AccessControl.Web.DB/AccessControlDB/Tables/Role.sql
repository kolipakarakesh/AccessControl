CREATE TABLE [Roles]
(
    RoleId INT IDENTITY(1,1) PRIMARY KEY,
    RoleName VARCHAR(100) NOT NULL,
    Description VARCHAR(255),
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedBy VARCHAR(100),
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedBy VARCHAR(100),
    ModifiedDate DATETIME
);
