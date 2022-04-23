CREATE TABLE [dbo].[Transactions](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Date] [date] NULL,
	[Category] [nvarchar](50) NULL,
	[Description] [nvarchar](255) NULL,
	[Price] [decimal](18, 2) NULL,
 CONSTRAINT [PK_Transactions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Transactions] ([Id], [Date], [Category], [Description], [Price]) VALUES (N'5072c3ee-8977-4113-883d-019530f6acf0', CAST(N'2022-04-05' AS Date), N'Transport', N'Petrol - 95', CAST(59.09 AS Decimal(18, 2)))
GO
INSERT [dbo].[Transactions] ([Id], [Date], [Category], [Description], [Price]) VALUES (N'1f8a80d7-026f-4c6a-9845-151ccd2c6e84', CAST(N'2022-04-27' AS Date), N'Transport', N'Petrol', CAST(59.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Transactions] ([Id], [Date], [Category], [Description], [Price]) VALUES (N'efe69b23-3ba6-43de-b252-1629efdbbe8b', CAST(N'2022-04-20' AS Date), N'Food', N'Food Corner', CAST(7.90 AS Decimal(18, 2)))
GO
INSERT [dbo].[Transactions] ([Id], [Date], [Category], [Description], [Price]) VALUES (N'898ba1c7-5333-4097-99d8-1739f78ed4bd', CAST(N'2022-04-22' AS Date), N'Health', N'Massage', CAST(60.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Transactions] ([Id], [Date], [Category], [Description], [Price]) VALUES (N'4647fc72-56a0-4a7e-84ff-2d4051191152', CAST(N'2022-04-23' AS Date), N'Bill', N'Petrol - 97', CAST(300.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Transactions] ([Id], [Date], [Category], [Description], [Price]) VALUES (N'7b0ebad3-a0a2-4dd0-8761-52a6ef166356', CAST(N'2022-04-20' AS Date), N'Beverage', N'Arabica Coffee', CAST(12.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Transactions] ([Id], [Date], [Category], [Description], [Price]) VALUES (N'bec91a8f-7803-48cc-ae94-67f025825e1d', CAST(N'2022-04-05' AS Date), N'Gadget', N'Iphone 13', CAST(3499.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Transactions] ([Id], [Date], [Category], [Description], [Price]) VALUES (N'd34056d2-902c-4733-9f6f-68de645d2ba8', CAST(N'2022-04-22' AS Date), N'Bill', N'Water', CAST(6.55 AS Decimal(18, 2)))
GO
INSERT [dbo].[Transactions] ([Id], [Date], [Category], [Description], [Price]) VALUES (N'75d6b375-992c-4cb3-8d2d-6bd52797df09', CAST(N'2022-04-23' AS Date), N'Beverage', N'Petrol - 97', CAST(13.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Transactions] ([Id], [Date], [Category], [Description], [Price]) VALUES (N'ef81810c-2418-44b3-9eb5-6ef046ee1563', CAST(N'2022-04-16' AS Date), N'Gadget', N'Mouse', CAST(200.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Transactions] ([Id], [Date], [Category], [Description], [Price]) VALUES (N'1699fd2c-8a03-4dfe-a311-726ef55f30b2', CAST(N'2022-04-11' AS Date), N'Food', N'Grocery', CAST(120.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Transactions] ([Id], [Date], [Category], [Description], [Price]) VALUES (N'a9ba12a3-5329-4e4d-a652-75701466188f', CAST(N'2022-04-04' AS Date), N'Beverage', N'Arabica Coffee', CAST(12.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Transactions] ([Id], [Date], [Category], [Description], [Price]) VALUES (N'4c89a274-92ca-447f-8e5f-786ba91a9735', CAST(N'2022-04-01' AS Date), N'Investment', N'Bitcoin', CAST(1000.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Transactions] ([Id], [Date], [Category], [Description], [Price]) VALUES (N'2499b807-2115-4e58-8096-7b1b46adcf1a', CAST(N'2022-04-09' AS Date), N'Beverage', N'Arabica Coffee', CAST(24.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Transactions] ([Id], [Date], [Category], [Description], [Price]) VALUES (N'f3c22c7e-6af5-434f-8439-7cb794cae003', CAST(N'2022-04-25' AS Date), N'Food', N'Food Corner', CAST(19.90 AS Decimal(18, 2)))
GO
INSERT [dbo].[Transactions] ([Id], [Date], [Category], [Description], [Price]) VALUES (N'38541ec1-fdaa-447f-9662-80e0c03e8b81', CAST(N'2022-04-09' AS Date), N'Food', N'Lavender Bakery', CAST(5.90 AS Decimal(18, 2)))
GO
INSERT [dbo].[Transactions] ([Id], [Date], [Category], [Description], [Price]) VALUES (N'3b7793d8-b23b-4eb4-b46f-94dca4413d16', CAST(N'2022-04-05' AS Date), N'Food', N'Grocery', CAST(300.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Transactions] ([Id], [Date], [Category], [Description], [Price]) VALUES (N'7b1354b9-50c4-42dc-bf6b-a622418f4d0d', CAST(N'2022-04-19' AS Date), N'Beverage', N'K bar', CAST(300.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Transactions] ([Id], [Date], [Category], [Description], [Price]) VALUES (N'0161ecee-e9e5-46a6-895d-bfb4a48c3499', CAST(N'2022-04-01' AS Date), N'Food', N'K-'' Fry Restaurant', CAST(59.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Transactions] ([Id], [Date], [Category], [Description], [Price]) VALUES (N'65beabae-9115-4108-a1df-c251162ab2a9', CAST(N'2022-04-11' AS Date), N'Entertainment', N'Movie', CAST(24.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Transactions] ([Id], [Date], [Category], [Description], [Price]) VALUES (N'f89b49dc-2af1-493b-b466-cbcc4ca1c196', CAST(N'2022-04-12' AS Date), N'Entertainment', N'Youtube - Premium', CAST(26.90 AS Decimal(18, 2)))
GO
INSERT [dbo].[Transactions] ([Id], [Date], [Category], [Description], [Price]) VALUES (N'8d9c9fde-039a-450d-9a08-cdf9e43cd1e3', CAST(N'2022-04-30' AS Date), N'Food', N'Food Corner', CAST(11.90 AS Decimal(18, 2)))
GO
INSERT [dbo].[Transactions] ([Id], [Date], [Category], [Description], [Price]) VALUES (N'a0c2edbb-20c4-4cc7-99c2-d72cba101aa8', CAST(N'2022-04-23' AS Date), N'Food', N'Dayone Restuarant', CAST(70.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Transactions] ([Id], [Date], [Category], [Description], [Price]) VALUES (N'ee917ad1-6cf8-4255-ad9d-f2fd70fc4540', CAST(N'2022-04-19' AS Date), N'Investment', N'Bitcoin', CAST(1000.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Transactions] ([Id], [Date], [Category], [Description], [Price]) VALUES (N'6f79c4ce-a645-4f80-8113-f50a28f3439c', CAST(N'2022-04-05' AS Date), N'Beverage', N'Arabica Coffee', CAST(12.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Transactions] ([Id], [Date], [Category], [Description], [Price]) VALUES (N'82793ecf-d045-4cd3-bd5c-f7a695e2ad46', CAST(N'2022-04-17' AS Date), N'Health', N'Facial', CAST(120.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Transactions] ([Id], [Date], [Category], [Description], [Price]) VALUES (N'a750bbb7-46d8-419c-86aa-f8d245e27579', CAST(N'2022-04-22' AS Date), N'Bill', N'Internet', CAST(100.00 AS Decimal(18, 2)))
GO
ALTER TABLE [dbo].[Transactions] ADD  CONSTRAINT [DF_Transactions_Id]  DEFAULT (newid()) FOR [Id]
GO
/****** Object:  StoredProcedure [dbo].[GetTransactionFromMonthAndYear]    Script Date: 23/4/2022 7:25:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetTransactionFromMonthAndYear]
  @year int,
  @month int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   SELECT * FROM Transactions WHERE [Date] >=datefromparts(@year, @month, 1) AND  [Date] <datefromparts(@year, @month+1, 1)
END
GO
/****** Object:  StoredProcedure [dbo].[NewTransaction]    Script Date: 23/4/2022 7:25:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[NewTransaction]
@date Date,
@category nvarchar(50),
@description nvarchar(255),
@price decimal(18,2),
@newId UNIQUEIDENTIFIER OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	SET @newId=NEWID();
	INSERT INTO Transactions(Id,[Date],Category,[Description],Price)
	VALUES(@newId,@date,@category,@description,@price)
	


END
GO