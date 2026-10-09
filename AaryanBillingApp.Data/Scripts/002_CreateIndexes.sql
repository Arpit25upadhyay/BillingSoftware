USE AaryanRetail;
GO

CREATE UNIQUE INDEX IX_Product_Barcode
ON Products(Barcode)
WHERE Barcode IS NOT NULL;
GO

CREATE INDEX IX_Product_Name
ON Products(Name);
GO

CREATE INDEX IX_Product_Category
ON Products(CategoryId);
GO

CREATE UNIQUE INDEX IX_Category_Name
ON Categories(Name);
GO