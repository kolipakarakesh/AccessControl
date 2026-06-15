CREATE TABLE [Product]
(
    ProductId       INT                 IDENTITY(1,1)   PRIMARY KEY,
    CategoryId      INT                 NULL,
    ProductName     VARCHAR(200)        NULL,
    ProductCode     VARCHAR(100)        NULL,
    Description     VARCHAR(500)        NULL,
    Price           DECIMAL(18,2)       NULL,
    StockQuantity   INT                 NULL,
    IsActive        BIT                 NULL DEFAULT 1,
    CreatedBy       VARCHAR(100)        NULL,
    CreatedDate     DATETIME            NULL DEFAULT GETDATE(),
    ModifiedBy      VARCHAR(100)        NULL,
    ModifiedDate    DATETIME            NULL
);