USE [master]
GO
/****** Object:  Database [BookEcommerceDB]    Script Date: 3/18/2025 1:11:33 PM ******/
CREATE DATABASE [BookEcommerceDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BookEcommerceDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\BookEcommerceDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'BookEcommerceDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\BookEcommerceDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [BookEcommerceDB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BookEcommerceDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BookEcommerceDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BookEcommerceDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BookEcommerceDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BookEcommerceDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BookEcommerceDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [BookEcommerceDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [BookEcommerceDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BookEcommerceDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BookEcommerceDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BookEcommerceDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BookEcommerceDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BookEcommerceDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BookEcommerceDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BookEcommerceDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BookEcommerceDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [BookEcommerceDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BookEcommerceDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BookEcommerceDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BookEcommerceDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BookEcommerceDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BookEcommerceDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [BookEcommerceDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BookEcommerceDB] SET RECOVERY FULL 
GO
ALTER DATABASE [BookEcommerceDB] SET  MULTI_USER 
GO
ALTER DATABASE [BookEcommerceDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BookEcommerceDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BookEcommerceDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BookEcommerceDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [BookEcommerceDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [BookEcommerceDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'BookEcommerceDB', N'ON'
GO
ALTER DATABASE [BookEcommerceDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [BookEcommerceDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [BookEcommerceDB]
GO
/****** Object:  Table [dbo].[Books]    Script Date: 3/18/2025 1:11:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Books](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](255) NOT NULL,
	[Author] [varchar](255) NOT NULL,
	[Description] [text] NULL,
	[ISBN] [varchar](20) NOT NULL,
	[Price] [decimal](10, 2) NOT NULL,
	[CategoryId] [int] NULL,
	[Publisher] [varchar](255) NULL,
	[PublicationDate] [date] NULL,
	[Language] [varchar](50) NULL,
	[Pages] [int] NULL,
	[CoverImage] [varchar](255) NULL,
	[Rating] [decimal](3, 1) NULL,
	[ReviewCount] [int] NULL,
	[IsNew] [bit] NULL,
	[IsFeatured] [bit] NULL,
	[InStock] [int] NULL,
	[CreatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 3/18/2025 1:11:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [text] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderItems]    Script Date: 3/18/2025 1:11:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[BookId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 3/18/2025 1:11:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[OrderNumber] [varchar](50) NOT NULL,
	[OrderDate] [datetime] NULL,
	[Status] [varchar](50) NULL,
	[ShippingFirstName] [varchar](50) NOT NULL,
	[ShippingLastName] [varchar](50) NOT NULL,
	[ShippingAddress] [varchar](255) NOT NULL,
	[ShippingCity] [varchar](100) NOT NULL,
	[ShippingState] [varchar](50) NOT NULL,
	[ShippingZipCode] [varchar](20) NOT NULL,
	[Subtotal] [decimal](10, 2) NOT NULL,
	[Tax] [decimal](10, 2) NOT NULL,
	[Shipping] [decimal](10, 2) NOT NULL,
	[Total] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reviews]    Script Date: 3/18/2025 1:11:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reviews](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BookId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[Rating] [int] NOT NULL,
	[Title] [varchar](255) NULL,
	[Comment] [text] NULL,
	[CreatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 3/18/2025 1:11:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[PasswordHash] [varchar](255) NOT NULL,
	[Address] [varchar](255) NULL,
	[City] [varchar](100) NULL,
	[State] [varchar](50) NULL,
	[ZipCode] [varchar](20) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Books] ADD  DEFAULT ((0)) FOR [Rating]
GO
ALTER TABLE [dbo].[Books] ADD  DEFAULT ((0)) FOR [ReviewCount]
GO
ALTER TABLE [dbo].[Books] ADD  DEFAULT ((0)) FOR [IsNew]
GO
ALTER TABLE [dbo].[Books] ADD  DEFAULT ((0)) FOR [IsFeatured]
GO
ALTER TABLE [dbo].[Books] ADD  DEFAULT ((0)) FOR [InStock]
GO
ALTER TABLE [dbo].[Books] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT (getdate()) FOR [OrderDate]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT ('Processing') FOR [Status]
GO
ALTER TABLE [dbo].[Reviews] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[Books]  WITH CHECK ADD FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([Id])
GO
ALTER TABLE [dbo].[OrderItems]  WITH CHECK ADD FOREIGN KEY([BookId])
REFERENCES [dbo].[Books] ([Id])
GO
ALTER TABLE [dbo].[OrderItems]  WITH CHECK ADD FOREIGN KEY([OrderId])
REFERENCES [dbo].[Orders] ([Id])
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Reviews]  WITH CHECK ADD FOREIGN KEY([BookId])
REFERENCES [dbo].[Books] ([Id])
GO
ALTER TABLE [dbo].[Reviews]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
USE [master]
GO
ALTER DATABASE [BookEcommerceDB] SET  READ_WRITE 
GO
