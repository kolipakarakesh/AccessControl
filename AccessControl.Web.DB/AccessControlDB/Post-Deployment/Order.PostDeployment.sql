/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

DECLARE @CustomerName VARCHAR(50);
DECLARE @CustomerEmail VARCHAR(50);
DECLARE @CustomerPhone VARCHAR(12);

DECLARE @OrderCounter INT;
DECLARE @OrderId INT;

DECLARE CustomerCursor CURSOR FOR
SELECT
    Name,
    Email,
    Phone
FROM [User]
WHERE Role = 'Customer';

OPEN CustomerCursor;

FETCH NEXT FROM CustomerCursor
INTO @CustomerName, @CustomerEmail, @CustomerPhone;

WHILE @@FETCH_STATUS = 0
BEGIN

    SET @OrderCounter = 1;

    WHILE @OrderCounter <= 10
    BEGIN

        INSERT INTO [Order]
        (
            OrderNumber,
            CustomerName,
            CustomerEmail,
            CustomerPhone,
            TotalAmount,
            OrderDate,
            Status,
            IsActive,
            CreatedBy,
            CreatedDate,
            ModifiedBy,
            ModifiedDate
        )
        VALUES
        (
            CONCAT('ORD-', REPLACE(@CustomerPhone,' ','') , '-', @OrderCounter),
            @CustomerName,
            @CustomerEmail,
            @CustomerPhone,
            0,
            DATEADD(DAY, -@OrderCounter, GETDATE()),
            'Delivered',
            1,
            'System',
            GETDATE(),
            'System',
            GETDATE()
        );

        SET @OrderId = SCOPE_IDENTITY();

        INSERT INTO OrderItem
        (
            OrderId,
            ProductId,
            Quantity,
            UnitPrice,
            TotalPrice,
            IsActive,
            CreatedBy,
            CreatedDate,
            ModifiedBy,
            ModifiedDate
        )
        SELECT TOP 5
            @OrderId,
            ProductId,
            1 + ABS(CHECKSUM(NEWID())) % 3,
            Price,
            Price * (1 + ABS(CHECKSUM(NEWID())) % 3),
            1,
            'System',
            GETDATE(),
            'System',
            GETDATE()
        FROM Product
        ORDER BY NEWID();

        UPDATE O
        SET TotalAmount =
        (
            SELECT SUM(TotalPrice)
            FROM OrderItem
            WHERE OrderId = O.OrderId
        )
        FROM [Order] O
        WHERE O.OrderId = @OrderId;

        SET @OrderCounter = @OrderCounter + 1;
    END

    FETCH NEXT FROM CustomerCursor
    INTO @CustomerName, @CustomerEmail, @CustomerPhone;

END

CLOSE CustomerCursor;
DEALLOCATE CustomerCursor;