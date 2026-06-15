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
MERGE Product AS Target
USING
(
    VALUES
    -- Electronics
    (1, 'Samsung 55 Inch Smart TV', 'ELEC-001', '4K UHD Smart Television', 54999.00, 25),
    (1, 'Sony Home Theatre System', 'ELEC-002', '5.1 Channel Surround Sound System', 24999.00, 15),

    -- Computers
    (2, 'Dell Inspiron 15 Laptop', 'COMP-001', 'Intel Core i5 Laptop', 65999.00, 20),
    (2, 'HP Pavilion Desktop', 'COMP-002', 'Desktop Computer for Home Use', 52999.00, 10),

    -- Mobiles
    (3, 'Samsung Galaxy S25', 'MOB-001', 'Android Smartphone', 79999.00, 50),
    (3, 'Apple iPhone 17', 'MOB-002', 'Premium Smartphone', 119999.00, 40),

    -- Home Appliances
    (4, 'LG Double Door Refrigerator', 'HOME-001', 'Frost Free Refrigerator', 42999.00, 12),
    (4, 'IFB Front Load Washing Machine', 'HOME-002', 'Automatic Washing Machine', 35999.00, 8),

    -- Furniture
    (5, 'Wooden Dining Table Set', 'FURN-001', '6 Seater Dining Table', 28999.00, 10),
    (5, 'Office Executive Chair', 'FURN-002', 'Ergonomic Office Chair', 8999.00, 25),

    -- Books
    (31, 'Clean Code', 'BOOK-001', 'Software Development Book', 699.00, 100),
    (31, 'The Pragmatic Programmer', 'BOOK-002', 'Programming Best Practices', 799.00, 100),

    -- Fashion
    (41, 'Men Cotton Casual Shirt', 'FASH-001', 'Regular Fit Shirt', 999.00, 100),
    (41, 'Women Floral Kurti', 'FASH-002', 'Printed Kurti', 1299.00, 100),

    -- Beauty
    (51, 'Lakme Face Serum', 'BEAU-001', 'Skin Care Serum', 499.00, 200),
    (51, 'Maybelline Lipstick', 'BEAU-002', 'Long Lasting Lipstick', 399.00, 200),

    -- Sports
    (61, 'SS Cricket Bat', 'SPRT-001', 'English Willow Bat', 5999.00, 30),
    (61, 'Nivia Football', 'SPRT-002', 'Professional Football', 899.00, 50),

    -- Automotive
    (71, 'Bosch Car Battery', 'AUTO-001', '12V Car Battery', 6499.00, 20),
    (71, 'Michelin Car Tyre', 'AUTO-002', 'Tubeless Tyre', 7999.00, 40)

) AS Source
(
    CategoryId,
    ProductName,
    ProductCode,
    Description,
    Price,
    StockQuantity
)
ON Target.ProductCode = Source.ProductCode

WHEN NOT MATCHED THEN
INSERT
(
    CategoryId,
    ProductName,
    ProductCode,
    Description,
    Price,
    StockQuantity,
    IsActive,
    CreatedBy,
    CreatedDate,
    ModifiedBy,
    ModifiedDate
)
VALUES
(
    Source.CategoryId,
    Source.ProductName,
    Source.ProductCode,
    Source.Description,
    Source.Price,
    Source.StockQuantity,
    1,
    'System',
    GETDATE(),
     'System',
    GETDATE()
);