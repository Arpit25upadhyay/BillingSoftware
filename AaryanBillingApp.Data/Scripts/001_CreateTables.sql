USE AaryanRetail;
GO

--------------------------------------------------------
-- Categories
--------------------------------------------------------
CREATE TABLE Categories
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500) NULL,

    IsActive BIT NOT NULL DEFAULT(1),

    CreatedOn DATETIME2 NOT NULL DEFAULT(GETDATE()),
    UpdatedOn DATETIME2 NULL
);

--------------------------------------------------------
-- Products
--------------------------------------------------------
CREATE TABLE Products
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    CategoryId INT NOT NULL,

    Name NVARCHAR(200) NOT NULL,

    Barcode NVARCHAR(100) NULL,

    SKU NVARCHAR(100) NULL,

    PurchasePrice DECIMAL(18,2) NOT NULL,

    SellingPrice DECIMAL(18,2) NOT NULL,

    CurrentStock DECIMAL(18,2) NOT NULL DEFAULT(0),

    MinStock DECIMAL(18,2) NOT NULL DEFAULT(0),

    Units NVARCHAR(20) NOT NULL DEFAULT('PCS'),

    IsActive BIT NOT NULL DEFAULT(1),

    CreatedOn DATETIME2 NOT NULL DEFAULT(GETDATE()),

    UpdatedOn DATETIME2 NULL
);
GO

ALTER TABLE Products
ADD CONSTRAINT FK_Product_Category
FOREIGN KEY(CategoryId)
REFERENCES Categories(Id);
GO