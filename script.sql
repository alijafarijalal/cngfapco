USE [CNGFAPCO]
GO
/****** Object:  UserDefinedFunction [dbo].[Convert_utf8]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Convert_utf8](@utf8 VARBINARY(MAX))
RETURNS NVARCHAR(MAX)
AS
BEGIN
    DECLARE @rslt NVARCHAR(MAX);

    SELECT @rslt=
    CAST(
          --'<?xml version="1.0" encoding="UTF-8"?><![CDATA['
          0x3C3F786D6C2076657273696F6E3D22312E302220656E636F64696E673D225554462D38223F3E3C215B43444154415B
          --the content goes within CDATA
        + @utf8
        --']]>'
        + 0x5D5D3E
    AS XML).value('.', 'nvarchar(max)');

    RETURN @rslt;
END
GO
/****** Object:  UserDefinedFunction [dbo].[DigitToPersianWord]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
----------------------------------------------------------------------------

-- برای اینکه محدودیتی در تعداد ارقام صحیح و اعشاری نداشته باشیم، پارامتر ورودی را از نوع کاراکتری می گیریم

----------------------------------------------------------------------------

CREATE FUNCTION [dbo].[DigitToPersianWord](@Number AS NVARCHAR(100) )

RETURNS NVARCHAR(2500) 

AS

BEGIN

----------------------------------------------------------------------------

-- بررسی تهی یا خالی بودن رشته

----------------------------------------------------------------------------

       IF LEN(ISNULL(@Number, '')) = 0  RETURN NULL

 

----------------------------------------------------------------------------

-- بررسی رشته ورودی برای پیدا کردن کاراکتر غیر عددی، نقطه و منفی

-- بررسی تعداد علامت منفی و نقطه که بیشتر از یک مورد نباشند

-- بررسی اینکه علامت منفی در ابتدای رشته ورودی باشد

----------------------------------------------------------------------------

 

       IF (PATINDEX('%[^0-9.-]%', @Number) > 0)

          OR (LEN(@Number) - LEN(REPLACE(@Number, '-', '')) > 1)

          OR (LEN(@Number) - LEN(REPLACE(@Number, '.', '')) > 1)

          OR (CHARINDEX('-', @Number) > 1)

              RETURN N'عدد وارد شده معتبر نمی باشد'

----------------------------------------------------------------------------

-- بررسی صفر بودن ورودی

-- بررسی منفی بودن ورودی

----------------------------------------------------------------------------

       IF PATINDEX('%[^0]%', @Number) = 0  RETURN 'صفر'

       IF (CHARINDEX('.', @Number) = 1) SET @Number = '0' + @Number

      

       DECLARE @Negative  AS NVARCHAR(5) = '';

       IF LEFT(@Number, 1) = '-'

       BEGIN

           SET @Number = SUBSTRING(@Number, 2, 100)

           SET @Negative  = 'منفی '

       END

-----------------------------------------------------------------------------

-- درج نام اعداد به فارسی در جدول مربوطه

-----------------------------------------------------------------------------

 

       DECLARE @NumTitle TABLE (val  INT,Title NVARCHAR(100));      

       INSERT INTO @NumTitle (val,Title)

       VALUES(0, ''),(1, 'یک') ,(2, 'دو') ,(3, 'سه')    ,(4, 'چهار'),(5, 'پنج'),(6, 'شش'),(7, 'هفت'),(8, 'هشت')

              ,(9, 'نه'),(10, 'ده'),(11, 'یازده'),(12, 'دوازده'),(13, 'سیزده'),(14, 'چهارده')      ,(15, 'پانزده'),(16, 'شانزده')

              ,(17, 'هفده'),(18, 'هجده'),(19, 'نوزده'),(20, 'بیست'),(30, 'سی'),(40, 'چهل'),(50, 'پنجاه'),(60, 'شصت'),(70, 'هفتاد'),(80, 'هشتاد'),(90, 'نود'),(100, 'صد')

              ,(200, 'دویست'),(300, 'سیصد'),(400, 'چهارصد'),(500, 'پانصد'),(600, 'ششصد'),(700, 'هفتصد'),(800, 'هشتصد'),(900, 'نهصد')

      

       DECLARE @PositionTitle TABLE (id  INT,Title NVARCHAR(100));               

       INSERT INTO @PositionTitle (id,title)

       VALUES (1, '')       ,(2, 'هزار'),(3, 'میلیون'),(4, 'میلیارد'),(5, 'تریلیون')

              ,(6, 'کوادریلیون'),(7, 'کوینتیلیون'),(8, 'سیکستیلون'),(9, 'سپتیلیون'),(10, 'اکتیلیون'),(11, 'نونیلیون'),(12, 'دسیلیون')

              ,(13, 'آندسیلیون'),(14, 'دودسیلیون'),(15, 'تریدسیلیون'),(16, 'کواتردسیلیون'),(17, 'کویندسیلیون'),(18, 'سیکسدسیلیون'),(19, 'سپتندسیلیون'),(20, 'اکتودسیلیوم'),(21, 'نومدسیلیون')         

      

       DECLARE @DecimalTitle TABLE (id  INT,Title NVARCHAR(100));          

       INSERT INTO @DecimalTitle (id,Title)

       VALUES( 1 ,'دهم' ),(2 , 'صدم'),(3 , 'هزارم'),(4 , 'ده-هزارم'),(5 , 'صد-هزارم'),(6 , 'میلیون ام')

              ,(7 , 'ده-میلیون ام'),(8 , 'صد-میلیون ام'),(9 , 'میلیاردم'),(10 , 'ده-میلیاردم')

       ---------------------------------------------------------------------

       -- حذف صفرهای غیرضروری موجود در اعشار

       ---------------------------------------------------------------------

       DECLARE @IntegerNumber NVARCHAR(100),

                     @DecimalNumber NVARCHAR(100),

                     @PointPosition INT = CASE CHARINDEX('.', @Number) WHEN 0 THEN LEN(@Number) + 1 ELSE CHARINDEX('.', @Number) END

       SET @Number = replace(rtrim(replace(@Number,'0',' ')),' ','0');

       SET @IntegerNumber = LEFT(@Number, @PointPosition - 1)

       SET @DecimalNumber = SUBSTRING(@Number, @PointPosition + 1 , LEN(@Number))

 

 

       SET @Number= @IntegerNumber

----------------------------------------------------------------------------------

-- جداسازی رقم صحیح و اعشاری

----------------------------------------------------------------------------------

       DECLARE @Num AS INT

       DECLARE @MyNumbers TABLE (id INT IDENTITY(1, 1), Val1 INT, Val2 INT, Val3 INT)

      

       WHILE (@Number) <> '0'

       BEGIN

           SET @Num = CAST(SUBSTRING(@Number, LEN(@Number) -2, 3)AS INT)   

          

              INSERT INTO @MyNumbers

              SELECT (@Num % 1000) -(@Num % 100),

              CASE

                     WHEN @Num % 100 BETWEEN 10 AND 19 THEN @Num % 100

                     ELSE (@Num % 100) -(@Num % 10)

              END,

              CASE

                     WHEN @Num % 100 BETWEEN 10 AND 19 THEN 0

                     ELSE @Num % 10

              END

          

           IF LEN(@Number) > 2

               SET @Number = LEFT(@Number, LEN(@Number) -3)

           ELSE

               SET @Number = '0'

       END

-----------------------------------------------------------------------------------     

-- جدا کردن سه رقم سه رقم برای بدست آوردن یکان، دهگان و صدگان

-----------------------------------------------------------------------------------     

       DECLARE @PersianWord AS NVARCHAR(2000) = '';

 

       SELECT @PersianWord += REPLACE(REPLACE(LTRIM(RTRIM(nt1.Title + ' ' + nt2.Title + ' ' + nt3.title)),'  ',' '),' ', ' و ')

              + ' ' + pt.title + ' و '

       FROM   @MyNumbers  AS mn

              INNER JOIN @PositionTitle pt

                   ON  pt.id = mn.id

              INNER JOIN @NumTitle nt1

                   ON  nt1.val = mn.Val1

              INNER JOIN @NumTitle nt2

                   ON  nt2.val = mn.Val2

              INNER JOIN @NumTitle nt3

                   ON  nt3.val = mn.Val3

       WHERE  (nt1.val + nt2.val + nt3.val > 0)

       ORDER BY pt.id DESC

      

       IF @IntegerNumber ='0' 

              SET @PersianWord = CASE WHEN PATINDEX('%[^0]%', @DecimalNumber) > 0 THEN @Negative ELSE '' END + 'صفر'

       ELSE

              SET @PersianWord = @Negative  + LEFT (@PersianWord, LEN(@PersianWord) - 2)

             

    DECLARE @PTitle NVARCHAR(100) = ISNULL((SELECT Title FROM @DecimalTitle WHERE id=LEN(@DecimalNumber)),'')

       SET @PersianWord += ISNULL(' ممیز '+ [dbo].[DigitToPersianWord](@DecimalNumber) + ' ' + @PTitle,'')

       RETURN @PersianWord

END

 
GO
/****** Object:  UserDefinedFunction [dbo].[fn_FileExists]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_FileExists](@path NVARCHAR(512))
RETURNS BIT
AS
BEGIN
     DECLARE @result INT
     EXEC master.dbo.xp_fileexist @path, @result OUTPUT
     RETURN cast(@result as bit)
END;
GO
/****** Object:  UserDefinedFunction [dbo].[GetAuditsPrice]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetAuditsPrice](@id  nvarchar(max),@date DATETIME)
RETURNS float 
AS
BEGIN
return ( 
        select Price price from [dbo].[tbl_AuditsPrice] inner join tbl_AuditComponies on AuditCompanyID=tbl_AuditComponies.ID
					where AuditCompanyID=@id and  CONVERT(VARCHAR(10),@date,120) BETWEEN '' + CONVERT(VARCHAR(10),FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),ToDate,120)+ '' 
        );
END;


GO
/****** Object:  UserDefinedFunction [dbo].[GetCheckedPreInvoice]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetCheckedPreInvoice](@workshopId  nvarchar(max))
RETURNS float
AS
BEGIN
    RETURN (
		select SUM(tbl_FreeSaleInvoices.TotalAmountTaxandComplications) as Amount
		from tbl_FinallFreeSaleInvoices right outer join tbl_FreeSaleInvoices on tbl_FreeSaleInvoices.InvoiceCode = tbl_FinallFreeSaleInvoices.PreInvoiceCode
		where tbl_FreeSaleInvoices.WorkshopsID=@workshopId
		group by PreInvoiceCode--,tbl_FreeSaleInvoices.WorkshopsID,tbl_FreeSaleInvoices.SaleCondition
		having PreInvoiceCode is null
	)   
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetCheckInvoicePaied]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetCheckInvoicePaied](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select Status from tbl_Payments where PreInvoiceCode=@id );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetCurrentIP]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetCurrentIP] ()
RETURNS varchar(255)
AS
BEGIN
    DECLARE @IP_Address varchar(255);

    SELECT @IP_Address = client_net_address
    FROM sys.dm_exec_connections
    WHERE Session_id = @@SPID;

    Return @IP_Address;
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetCurrRemWarehouses]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetCurrRemWarehouses](@id nvarchar(max))
RETURNS float
AS
BEGIN

	return ( select SUM(CurrentRem) from tbl_Warehouses where FinancialCode=@id );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetCustomers]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetCustomers](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select FirstName + ' ' + LasstName from tbl_Customers 
			where ID=@id );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetCutoffValveModels]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetCutoffValveModels](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select Code from [dbo].[tbl_CutofValveConstractors] where ID=@id );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetCutofValveConstractors]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetCutofValveConstractors](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select CutofValve from tbl_CutofValveConstractors where ID=@id );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetCylinderBaseReturnofPartsNumber]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetCylinderBaseReturnofPartsNumber](@id nvarchar(max),@workshopid  nvarchar(max),@FromDate DATETIME,@ToDate DATETIME)
RETURNS int 
AS
BEGIN   
	return ( CASE
                WHEN @workshopid=0 THEN 
				(select ISNULL(SUM(NumberofSend),0) as 'Send' FROM  dbo.tbl_ReturnofParts
					where TypeofPiece= N'پایه مخزن' and VehicleTypeID=@id  and CONVERT(VARCHAR(10),Date,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
					GROUP BY VehicleTypeID)

                ELSE 
				(select ISNULL(SUM(NumberofSend),0) as 'Send' FROM  dbo.tbl_ReturnofParts
					where TypeofPiece=N'پایه مخزن' and VehicleTypeID=@id and WorkshopID=@workshopid  and CONVERT(VARCHAR(10),Date,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
					GROUP BY VehicleTypeID
				)
             END
        );

END

GO
/****** Object:  UserDefinedFunction [dbo].[GetCylinderBaseSendNumber]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetCylinderBaseSendNumber](@type nvarchar(max),@workshopid  nvarchar(max))
RETURNS int 
AS
BEGIN   
	return ( select SUM(NumberofSend) Send from tbl_TankBaseDivisionPlans inner join 
			tbl_TypeofTankBases on tbl_TankBaseDivisionPlans.TypeofTankBaseID=tbl_TypeofTankBases.ID inner join 
			tbl_DivisionPlans on tbl_TankBaseDivisionPlans.DivisionPlanID=tbl_DivisionPlans.ID
			where Send='1' and VehicleTypeId=@type and WorkshopID=@workshopid
			group by Type,WorkshopID
 );

END
GO
/****** Object:  UserDefinedFunction [dbo].[GetCylinderBaseUsedNumber]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetCylinderBaseUsedNumber](@type nvarchar(max),@workshopid  nvarchar(max))
RETURNS int 
AS
BEGIN   
	return ( select COUNT(*) Used 
			from tbl_VehicleRegistrations tbl1 inner join 
			tbl_TypeofTankBases tbl2 on tbl1.VehicleTypeID=tbl2.VehicleTypeId inner join 
			tbl_Workshops tbl3 on tbl1.WorkshopID=tbl3.ID
			where tbl2.VehicleTypeId=@type and tbl1.WorkshopID=@workshopid);

END

GO
/****** Object:  UserDefinedFunction [dbo].[GetCylinderBulk]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetCylinderBulk](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select top(1) [bulk] from tbl_BankTanks where REPLACE(serialNumber,' ','')=REPLACE(@id,' ','') );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetCylinderBulk_2]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetCylinderBulk_2](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select Type from tbl_TypeofTanks where  ID=@id);
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetCylinderConstractors]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetCylinderConstractors](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select Constractor from tbl_TankConstractors where ID=@id );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetCylinderCoverReturnofPartsNumber]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetCylinderCoverReturnofPartsNumber](@id nvarchar(max),@workshopid  nvarchar(max),@FromDate DATETIME,@ToDate DATETIME)
RETURNS int 
AS
BEGIN   
	return ( CASE
                WHEN @workshopid=0 THEN 
				(select ISNULL(SUM(NumberofSend),0) as 'Send' FROM  dbo.tbl_ReturnofParts
					where TypeofPiece= N'کاور مخزن' and VehicleTypeID=@id  and CONVERT(VARCHAR(10),Date,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
					GROUP BY VehicleTypeID)

                ELSE 
				(select ISNULL(SUM(NumberofSend),0) as 'Send' FROM  dbo.tbl_ReturnofParts
					where TypeofPiece=N'کاور مخزن' and VehicleTypeID=@id and WorkshopID=@workshopid  and CONVERT(VARCHAR(10),Date,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
					GROUP BY VehicleTypeID
				)
             END
        );

END

GO
/****** Object:  UserDefinedFunction [dbo].[GetCylinderCoverSendNumber]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetCylinderCoverSendNumber](@type nvarchar(max),@workshopid  nvarchar(max))
RETURNS int 
AS
BEGIN   
	return ( select SUM(NumberofSend) Send from tbl_TankCoverDivisionPlans inner join 
			tbl_TypeofTankCovers on tbl_TankCoverDivisionPlans.TypeofTankCoverID=tbl_TypeofTankCovers.ID inner join 
			tbl_DivisionPlans on tbl_TankCoverDivisionPlans.DivisionPlanID=tbl_DivisionPlans.ID
			where Send='1' and VehicleTypeId=@type and WorkshopID=@workshopid
			group by Type,WorkshopID
 );

END
GO
/****** Object:  UserDefinedFunction [dbo].[GetCylinderCoverUsedNumber]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetCylinderCoverUsedNumber](@type nvarchar(max),@workshopid  nvarchar(max))
RETURNS int 
AS
BEGIN   
	return ( select COUNT(*) Used 
			from tbl_VehicleRegistrations tbl1 inner join 
			tbl_TypeofTankCovers tbl2 on tbl1.VehicleTypeID=tbl2.VehicleTypeId inner join 
			tbl_Workshops tbl3 on tbl1.WorkshopID=tbl3.ID
			where tbl2.VehicleTypeId=@type and tbl1.WorkshopID=@workshopid);

END

GO
/****** Object:  UserDefinedFunction [dbo].[GetCylinderLiterage]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetCylinderLiterage](@Literage nvarchar(4))
RETURNS nvarchar(4) 
AS
BEGIN							

return ( CASE
                WHEN @Literage=20 THEN 20
				WHEN @Literage=28 THEN 28
				WHEN @Literage=35 THEN 28
				WHEN @Literage in (50,52,55,57,58) THEN 57
				WHEN @Literage in (60,62) THEN 62
				WHEN @Literage in (65,70,73,75) THEN 75
				WHEN @Literage in (77,80,82) THEN 82
				WHEN @Literage in (90,96,97,100,101,112) THEN 100
				else
				0
		END
        );
END;


GO
/****** Object:  UserDefinedFunction [dbo].[GetCylinderSendNumber]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetCylinderSendNumber](@type nvarchar(max),@workshopid  nvarchar(max))
RETURNS int 
AS
BEGIN   
	return ( select SUM(NumberofSend) Send from tbl_TankDivisionPlans inner join 
			tbl_TypeofTanks on tbl_TankDivisionPlans.TypeofTankID=tbl_TypeofTanks.ID inner join 
			tbl_DivisionPlans on tbl_TankDivisionPlans.DivisionPlanID=tbl_DivisionPlans.ID
			where Send='1' and Type=@type and WorkshopID=@workshopid
			group by Type,WorkshopID
 );

END

--select sum(tbl1.NumberofSend) Send
--			from tbl_TankDivisionPlans tbl1 RIGHT OUTER JOIN 
--					tbl_TypeofTanks tbl2 on tbl1.TypeofTankID=tbl2.ID inner join 
--					tbl_DivisionPlans tbl3 on tbl1.DivisionPlanID=tbl3.ID
--			where Send=1 and tbl2.Type=@type and WorkshopID=@workshopid
--			group by tbl2.Type,tbl3.WorkshopID
GO
/****** Object:  UserDefinedFunction [dbo].[GetCylinderTypeID]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetCylinderTypeID](@id nvarchar(max))
RETURNS int 
AS
BEGIN

	return ( select ID from tbl_TypeofTanks where VehicleTypeId=@id);
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetCylinderUsedNumber]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetCylinderUsedNumber](@type nvarchar(max),@workshopid  nvarchar(max))
RETURNS int 
AS
BEGIN   
	return ( select COUNT(*) Used 
			from tbl_VehicleRegistrations tbl1 inner join 
			tbl_TypeofTanks tbl2 on tbl1.VehicleTypeID=tbl2.VehicleTypeId inner join 
			tbl_Workshops tbl3 on tbl1.WorkshopID=tbl3.ID
			where tbl2.Type=@type and tbl1.WorkshopID=@workshopid);

END

GO
/****** Object:  UserDefinedFunction [dbo].[GetCylinderWeight]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetCylinderWeight](@Literage  nvarchar(4))
RETURNS float 
AS
BEGIN							

return ( CASE
                WHEN @Literage=20 THEN 21
				WHEN @Literage=28 THEN 29
				WHEN @Literage=57 THEN 55.5
				WHEN @Literage in (60,62) THEN 60.5
				WHEN @Literage=75 THEN 75.5
				WHEN @Literage=82 THEN 80.5
				WHEN @Literage=100 THEN 97.5
				else
				0
         END
        );
END;


GO
/****** Object:  UserDefinedFunction [dbo].[GetDamagesDoubleCylinderCount]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetDamagesDoubleCylinderCount](@Literage nvarchar(4),@workshopId int,@year nvarchar(4),@month nvarchar(2))
RETURNS int 
AS
BEGIN

return ( 
		select COUNT(*) from resultstable_IRNGVDamages  
		left outer join tbl_Workshops on WorkshopID=IRNGVCod
		where ID=@workshopId and Year=@Year and Month=@Month and (Literage=@Literage or Literage_2=@Literage)
        );
END;


GO
/****** Object:  UserDefinedFunction [dbo].[GetDamagesDoubleCylinderCount_SameLiterage]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetDamagesDoubleCylinderCount_SameLiterage](@Literage nvarchar(4),@workshopId int,@year nvarchar(4),@month nvarchar(2))
RETURNS int 
AS
BEGIN

return ( 
		select (COUNT(*) * 1 ) as Count from resultstable_IRNGVDamages  
		left outer join tbl_Workshops on WorkshopID=IRNGVCod
		where ID=@workshopId and Year=@Year and Month=@Month and (Literage=Literage_2) and (Literage=@Literage or Literage_2=@Literage)
        );
END;


GO
/****** Object:  UserDefinedFunction [dbo].[GetDamagesLiterage28]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetDamagesLiterage28](@workshopId int,@InvoiceCode nvarchar(max))
RETURNS int 
AS
BEGIN

return ( 
			select ServiceCode FROM  tbl_InvoicesDamages 		
			where WorkshopsID=@workshopId and InvoiceCode=@InvoiceCode and ServiceCode in (28)
        );
END;


GO
/****** Object:  UserDefinedFunction [dbo].[GetDefectsCount]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetDefectsCount](@workshopId  nvarchar(max),@fromdate nvarchar(max),	@todate nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN
    RETURN (
		select COUNT(*) as 'Count' from tbl_VehicleRegistrations
		where RegisterStatus=1 and RegistrationTypeID=1 and WorkshopID=@workshopId and CONVERT(VARCHAR(10),CreateDate,120) BETWEEN '' + CONVERT(VARCHAR(10),@fromdate,120) + '' AND '' +  CONVERT(VARCHAR(10),@todate,120)+ '' and 
		( ([dbo].[GetVehicleNationalCard](ID) IS NULL or [dbo].[GetVehicleNationalCard](ID)='') or ([dbo].[GetVehicleCard](ID) IS NULL or [dbo].[GetVehicleCard](ID)='') or ([dbo].[GetVehicleLicenseImage](ID) IS NULL or [dbo].[GetVehicleLicenseImage](ID)='') or ([dbo].[GetVehicleHealthCertificate](ID) IS NULL or [dbo].[GetVehicleHealthCertificate](ID)=''))
	)   
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetDefectsCount_2]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetDefectsCount_2](@workshopId  nvarchar(max),@fromdate nvarchar(max), @todate nvarchar(max))
RETURNS int 
AS
BEGIN
    RETURN (
		select count(*) as 'Count' from(
		select (dbo.fn_FileExists(N'D:\www.cngfapco.com\UploadedFiles\Vehicle\' + tbl2.Folder + '\' + tbl2.Image)) as 'isExist',Folder,Image,tbl1.ID,tbl1.CreateDate  from tbl_VehicleRegistrations tbl1 left outer join tbl_VehicleAttachments tbl2 on tbl1.ID=tbl2.VehicleRegistrationID
				where RegisterStatus=1 and RegistrationTypeID=1 and WorkshopID=@workshopId and CONVERT(VARCHAR(10),tbl1.CreateDate,120) BETWEEN '' + CONVERT(VARCHAR(10),@fromdate,120) + '' AND '' +  CONVERT(VARCHAR(10),@todate,120)+ '' 
				group by Folder,Image,tbl1.ID,tbl1.CreateDate) as level1
		where level1.isExist='0'
	)   
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetDifferenceRegistrationPrice]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetDifferenceRegistrationPrice](@workshopid  nvarchar(max),@type  nvarchar(max),@date DATETIME)
RETURNS float 
AS
BEGIN

return ( 
CASE
                WHEN @workshopid=8 THEN 
				(
					select Price-OldPrice from [dbo].[tbl_RegistrationPrice]
					where DepType='2' and Type=(case when @type like N'%وانت%' then '2' else '1' end) and  CONVERT(VARCHAR(10),@date,120) BETWEEN '' + CONVERT(VARCHAR(10),FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),ToDate,120)+ '' 
				)
				WHEN @workshopid=44 THEN 
				(
					select Price-OldPrice from [dbo].[tbl_RegistrationPrice]
					where DepType='3' and Type=(case when @type like N'%وانت%' then '2' else '1' end) and  CONVERT(VARCHAR(10),@date,120) BETWEEN '' + CONVERT(VARCHAR(10),FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),ToDate,120)+ '' 
				)
				else
				(
					select Price-OldPrice from [dbo].[tbl_RegistrationPrice]
					where DepType='1' and Type=(case when @type like N'%وانت%' then '2' else '1' end) and  CONVERT(VARCHAR(10),@date,120) BETWEEN '' + CONVERT(VARCHAR(10),FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),ToDate,120)+ '' 
				)
END
        );
END;


GO
/****** Object:  UserDefinedFunction [dbo].[GetEconomicalnumber]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetEconomicalnumber](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select Economicalnumber from tbl_Workshops where ID=@id );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetEquipmentGroupValue]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetEquipmentGroupValue](@id nvarchar(max),@type nvarchar(1))
RETURNS float
AS
BEGIN

	return ( 
		case when @type=1 then
			(select SUM(Value) [value] from tbl_EquipmentList where Pid=@id )
		else
			(select SUM(Value2) [value2] from tbl_EquipmentList where Pid=@id )
		end
	);
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetEquipmentListParent]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetEquipmentListParent](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( case
			when(@id is not null) then
				(SELECT top(1)  tbl_EquipmentList_1.Title AS Parent
				 FROM    dbo.tbl_EquipmentList INNER JOIN
                         dbo.tbl_EquipmentList AS tbl_EquipmentList_1 ON dbo.tbl_EquipmentList.Pid = tbl_EquipmentList_1.ID
						 where tbl_EquipmentList.Pid=@id)
			else
				(null)
			end
			);
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetExistBOM]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetExistBOM](@id nvarchar(max))
RETURNS int
AS
BEGIN

	return ( select top(1) DivisionPlanID from tbl_DivisionPlanBOMs where DivisionPlanID=@id );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetFillingValveConstractors]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetFillingValveConstractors](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select FillingValve from tbl_FillingValveConstractors where ID=@id );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetFillingValveModels]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetFillingValveModels](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select Code from tbl_FillingValveConstractors where ID=@id );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetFinalFreeSaleInvoiceCode]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetFinalFreeSaleInvoiceCode](@invoiceCode nvarchar(max))
RETURNS nvarchar(max)
AS
BEGIN

	return ( select top(1)InvoiceCode from tbl_FinallFreeSaleInvoices
			where PreInvoiceCode=@invoiceCode );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetFinancialCreditor]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetFinancialCreditor](@id nvarchar(max),@workshopid  nvarchar(max),@FromDate DATETIME,@ToDate DATETIME)
RETURNS float 
AS
BEGIN   
	return ( CASE
            WHEN @id=1 THEN 
				(select SUM(list2.Salary) as 'Salary' from(
					select list.Type,list.Count,list.Price,(CONVERT(float,list.Count) * list.Price) as 'Salary',(CONVERT(float,list.Count) * 600000) as 'Inspection', (CONVERT(float,list.Count) * 25000) as 'label', (CONVERT(float,list.Count) * 55000) as 'InsuranceDoc'  from (
					select Type, COUNT(*) as 'Count', [dbo].[GetRegistrationPrice](WorkshopID,Type,CreateDate) as 'Price' from tbl_VehicleRegistrations inner join tbl_VehicleTypes on tbl_VehicleRegistrations.VehicleTypeID=tbl_VehicleTypes.ID
					where RegisterStatus=1 and RegistrationTypeID=1 and (WorkshopID=@workshopid or @workshopid=0) and CONVERT(VARCHAR(10),CreateDate,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
					group by Type,WorkshopID,CreateDate) as list  ) as list2
					 )
			 WHEN @id=2 THEN 
				(SELECT SUM(CarryFare) as 'CarryFare' FROM tbl_RemittanceDetails inner join 
					tbl_Remittances on tbl_RemittanceDetails.RemittancesID=tbl_Remittances.ID inner join 
					tbl_DivisionPlans on tbl_Remittances.DivisionPlanID=tbl_DivisionPlans.ID
					where (WorkshopID=@workshopid or @workshopid=0) and CONVERT(VARCHAR(10),tbl_RemittanceDetails.Date,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
					--group by CarrierName
					 )
			 WHEN @id=3 THEN 
				(select SUM(list2.Inspection) as 'Inspection' from(
					select list.Type,list.Count,list.Price,(CONVERT(float,list.Count) * list.Price) as 'Salary',(CONVERT(float,list.Count) * CONVERT(float,list.AuditsPrice)) as 'Inspection', (CONVERT(float,list.Count) * 25000) as 'label', (CONVERT(float,list.Count) * 55000) as 'InsuranceDoc'  from (
					select Type, COUNT(*) as 'Count', [dbo].[GetRegistrationPrice](WorkshopID,Type,tbl_VehicleRegistrations.CreateDate) as 'Price',[dbo].[GetAuditsPrice](CompaniesID,tbl_VehicleRegistrations.CreateDate) as 'AuditsPrice' from tbl_VehicleRegistrations inner join tbl_VehicleTypes on tbl_VehicleRegistrations.VehicleTypeID=tbl_VehicleTypes.ID inner join 
					tbl_Workshops on tbl_VehicleRegistrations.WorkshopID=tbl_Workshops.ID
					where RegisterStatus=1 and RegistrationTypeID=1 and (WorkshopID=@workshopid or @workshopid=0) and CONVERT(VARCHAR(10),tbl_VehicleRegistrations.CreateDate,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
					group by Type,WorkshopID,tbl_VehicleRegistrations.CreateDate,CompaniesID) as list  ) as list2
					 )
			WHEN @id=4 THEN 
				(select SUM(list2.label) as 'label' from(
					select list.Type,list.Count,list.Price,(CONVERT(float,list.Count) * list.Price) as 'Salary',(CONVERT(float,list.Count) * 600000) as 'Inspection', (CONVERT(float,list.Count) * 25000) as 'label', (CONVERT(float,list.Count) * 55000) as 'InsuranceDoc'  from (
					select Type, COUNT(*) as 'Count', [dbo].[GetRegistrationPrice](WorkshopID,Type,CreateDate) as 'Price' from tbl_VehicleRegistrations inner join tbl_VehicleTypes on tbl_VehicleRegistrations.VehicleTypeID=tbl_VehicleTypes.ID
					where RegisterStatus=1 and RegistrationTypeID=1 and (WorkshopID=@workshopid or @workshopid=0) and CONVERT(VARCHAR(10),CreateDate,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
					group by Type,WorkshopID,CreateDate) as list  ) as list2
					 )
			WHEN @id=5 THEN  
				( select SUM(list2.InsuranceDoc) as 'InsuranceDoc' from(
					select list.Type,list.Count,list.Price,(CONVERT(float,list.Count) * list.Price) as 'Salary',(CONVERT(float,list.Count) * 600000) as 'Inspection', (CONVERT(float,list.Count) * 25000) as 'label', (CONVERT(float,list.Count) * 55000) as 'InsuranceDoc'  from (
					select Type, COUNT(*) as 'Count', [dbo].[GetRegistrationPrice](WorkshopID,Type,CreateDate) as 'Price' from tbl_VehicleRegistrations inner join tbl_VehicleTypes on tbl_VehicleRegistrations.VehicleTypeID=tbl_VehicleTypes.ID
					where RegisterStatus=1 and RegistrationTypeID=1 and (WorkshopID=@workshopid or @workshopid=0) and CONVERT(VARCHAR(10),CreateDate,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
					group by Type,WorkshopID,CreateDate) as list  ) as list2
					)
					
			WHEN @id=7 THEN  
				( select (ISNULL([dbo].[GetRegisterDamagedCount](@workshopid,@FromDate,@ToDate) * 3000000,0)) as 'DamagedPrice' 
					
					)
           ELSE 
				( '0')
             END
        );

END
GO
/****** Object:  UserDefinedFunction [dbo].[GetFinancialCreditor_Assesment]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetFinancialCreditor_Assesment](@id nvarchar(max),@workshopid  nvarchar(max),@FromDate DATETIME,@ToDate DATETIME)
RETURNS float 
AS
BEGIN   
	return ( CASE
            WHEN @id=1 THEN 
				(SELECT sum([TotalAmount]) as 'Salary'  FROM [CNGFAPCO].[dbo].[tbl_InvoicesFapa] 
					where FinancialStatus=1 and WorkshopsID=@workshopid  )
			 WHEN @id=2 THEN 
				(SELECT SUM(CarryFare) as 'CarryFare' FROM tbl_RemittanceDetails inner join 
					tbl_Remittances on tbl_RemittanceDetails.RemittancesID=tbl_Remittances.ID inner join 
					tbl_DivisionPlans on tbl_Remittances.DivisionPlanID=tbl_DivisionPlans.ID
					where (WorkshopID=@workshopid or @workshopid=0) and CONVERT(VARCHAR(10),tbl_RemittanceDetails.Date,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
					--group by CarrierName
					 )
			 WHEN @id=3 THEN 
				(select SUM(list2.Inspection) as 'Inspection' from(
					select list.Type,list.Count,list.Price,(CONVERT(float,list.Count) * list.Price) as 'Salary',(CONVERT(float,list.Count) * CONVERT(float,list.AuditsPrice)) as 'Inspection', (CONVERT(float,list.Count) * 25000) as 'label', (CONVERT(float,list.Count) * 55000) as 'InsuranceDoc'  from (
					select Type, COUNT(*) as 'Count', [dbo].[GetRegistrationPrice](WorkshopID,Type,tbl_VehicleRegistrations.CreateDate) as 'Price',[dbo].[GetAuditsPrice](CompaniesID,tbl_VehicleRegistrations.CreateDate) as 'AuditsPrice' from tbl_VehicleRegistrations inner join tbl_VehicleTypes on tbl_VehicleRegistrations.VehicleTypeID=tbl_VehicleTypes.ID inner join 
					tbl_Workshops on tbl_VehicleRegistrations.WorkshopID=tbl_Workshops.ID
					where RegisterStatus=1 and RegistrationTypeID=1 and (WorkshopID=@workshopid or @workshopid=0) and CONVERT(VARCHAR(10),tbl_VehicleRegistrations.CreateDate,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
					group by Type,WorkshopID,tbl_VehicleRegistrations.CreateDate,CompaniesID) as list  ) as list2
					 )
			WHEN @id=4 THEN 
				(select SUM(list2.label) as 'label' from(
					select list.Type,list.Count,list.Price,(CONVERT(float,list.Count) * list.Price) as 'Salary',(CONVERT(float,list.Count) * 600000) as 'Inspection', (CONVERT(float,list.Count) * 25000) as 'label', (CONVERT(float,list.Count) * 55000) as 'InsuranceDoc'  from (
					select Type, COUNT(*) as 'Count', [dbo].[GetRegistrationPrice](WorkshopID,Type,CreateDate) as 'Price' from tbl_VehicleRegistrations inner join tbl_VehicleTypes on tbl_VehicleRegistrations.VehicleTypeID=tbl_VehicleTypes.ID
					where RegisterStatus=1 and RegistrationTypeID=1 and (WorkshopID=@workshopid or @workshopid=0) and CONVERT(VARCHAR(10),CreateDate,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
					group by Type,WorkshopID,CreateDate) as list  ) as list2
					 )
			WHEN @id=5 THEN  
				( select SUM(list2.InsuranceDoc) as 'InsuranceDoc' from(
					select list.Type,list.Count,list.Price,(CONVERT(float,list.Count) * list.Price) as 'Salary',(CONVERT(float,list.Count) * 600000) as 'Inspection', (CONVERT(float,list.Count) * 25000) as 'label', (CONVERT(float,list.Count) * 55000) as 'InsuranceDoc'  from (
					select Type, COUNT(*) as 'Count', [dbo].[GetRegistrationPrice](WorkshopID,Type,CreateDate) as 'Price' from tbl_VehicleRegistrations inner join tbl_VehicleTypes on tbl_VehicleRegistrations.VehicleTypeID=tbl_VehicleTypes.ID
					where RegisterStatus=1 and RegistrationTypeID=1 and (WorkshopID=@workshopid or @workshopid=0) and CONVERT(VARCHAR(10),CreateDate,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
					group by Type,WorkshopID,CreateDate) as list  ) as list2)
           ELSE 
				( '0')
             END
        );

END

GO
/****** Object:  UserDefinedFunction [dbo].[GetFinancialDebtor]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetFinancialDebtor](@id nvarchar(max),@workshopid  nvarchar(max), @FromDate DATETIME,@ToDate DATETIME)
RETURNS float 
AS
BEGIN   
	return ( 
			select ISNULL(SUM(Value),0) as 'Debtor'
			FROM [CNGFAPCO].[dbo].[tbl_FinancialPayments]
			where FinancialDescID=@id and (WorkshopID=@workshopid or @workshopid=0) and CONVERT(VARCHAR(10),Date,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
	);

END

GO
/****** Object:  UserDefinedFunction [dbo].[GetHealtCareCount]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetHealtCareCount](@workshopid  nvarchar(max))
RETURNS int 
AS
BEGIN

	return ( CASE
                WHEN @workshopid=0 THEN 
				(	select top(1) SUM(Count) as 'Used' from  [CNGFAPCO].[dbo].[tbl_ContradictionTotals] where ContradictionTypeId='4'
					group by CreateDate
					order by CreateDate desc
				)
				ELSE 
				(
					select top(1) SUM(Count) as 'Used' from  [CNGFAPCO].[dbo].[tbl_ContradictionTotals] where ContradictionTypeId='4'					
					and WorkshopId=@workshopid
					group by CreateDate
					order by CreateDate desc
				)
             END
        );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetInsuranceNumber]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetInsuranceNumber](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select Number from tbl_Insurances where VehicleRegistrationID=@id );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetInvoiceAmount_ToWords]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetInvoiceAmount_ToWords](@code nvarchar(max),@workshopId nvarchar(max))
RETURNS float
AS
BEGIN

	return ( select SUM(TotalAmountTaxandComplications) as amount from tbl_InvoicesFapa
			where WorkshopsID=@workshopId and InvoiceCode=@code );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetInvoiceCheckedCount]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetInvoiceCheckedCount](@workshopid nvarchar(max),@tbl_Code nvarchar(1))
RETURNS nvarchar(max) 
AS
BEGIN
			return ( CASE
                WHEN @tbl_Code=1 THEN 
				(
					select COUNT(distinct InvoiceCode) as 'CheckedCount'  from tbl_InvoicesFapa
					where WorkshopsID=@workshopid and CheckStatus='1'
				)

                ELSE 
				(
					select COUNT(distinct InvoiceCode) as 'CheckedCount'  from tbl_InvoicesFapa_DamagesCylinder
					where WorkshopsID=@workshopid and CheckStatus='1'
				)
             END
        );

END


GO
/****** Object:  UserDefinedFunction [dbo].[GetInvoiceFapaCount]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetInvoiceFapaCount](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select SUM(CONVERT(float, [Number])) as [Number] from tbl_InvoicesFapa where WorkshopsID=@id and Description is null );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetInvoiceFinancialCount]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetInvoiceFinancialCount](@workshopid nvarchar(max),@tbl_Code nvarchar(1))
RETURNS nvarchar(max) 
AS
BEGIN
			return ( CASE
                WHEN @tbl_Code=1 THEN 
				(
					select COUNT(distinct InvoiceCode) as 'FinancialCount'  from tbl_InvoicesFapa
					where WorkshopsID=@workshopid and FinancialStatus='1'
				)

                ELSE 
				(
					select COUNT(distinct InvoiceCode) as 'FinancialCount'  from tbl_InvoicesFapa_DamagesCylinder
					where WorkshopsID=@workshopid and FinancialStatus='1'
				)
             END
        );
	
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetInvoiceReciveCount]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetInvoiceReciveCount](@workshopid nvarchar(max),@tbl_Code nvarchar(1))
RETURNS nvarchar(max) 
AS
BEGIN
			return ( CASE
                WHEN @tbl_Code=1 THEN 
				(
					select COUNT(distinct InvoiceCode) as 'ReciveCount'  from tbl_InvoicesFapa
					where WorkshopsID=@workshopid and ReciveStatus='1'
				)

                ELSE 
				(
					select COUNT(distinct InvoiceCode) as 'ReciveCount'  from tbl_InvoicesFapa_DamagesCylinder
					where WorkshopsID=@workshopid and ReciveStatus='1'
				)
             END
        );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetInvoiceStatusPaied]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetInvoiceStatusPaied](@InvoiceCode nvarchar(max),@WorkshopId nvarchar(max))
RETURNS int 
AS
BEGIN

return(0)
	--return ( select (case status when N'پرداخت شده' then 1 else 0 end) StatusPaied from tbl_Payments
	--			where InvoiceCode = @InvoiceCode and PayerCode=@WorkshopId);
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetInvoiceStatusRemittance]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetInvoiceStatusRemittance](@InvoiceCode nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select top(1)InvoiceCode  from [dbo].[tbl_FreeSaleRemittances]
				where InvoiceCode = @InvoiceCode );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetIRNGV_DiffRows]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetIRNGV_DiffRows](@nationalCode  nvarchar(max), @plate  nvarchar(max))
RETURNS @Diff_Result TABLE ( [VehicleType2]  nvarchar(max)
      ,[ConstructionYear2]  nvarchar(4)
      ,[EngineNumber2]  nvarchar(max)
      ,[ChassisNumber2]  nvarchar(max)
      ,[NationalCode2]  nvarchar(max)
      ,[Workshop2]  nvarchar(max)
      ,[Plate2]  nvarchar(max)
      ,[Plate22]  nvarchar(max)
      ,[HealthCertificate2]  nvarchar(max) )
AS
BEGIN
    Insert into @Diff_Result
    SELECT top(1) [VehicleType]
      ,[ConstructionYear]
      ,[EngineNumber]
      ,[ChassisNumber]
      ,[NationalCode]
      ,[Workshop]
      ,[Plate]
      ,[Plate2]
      ,[HealthCertificate] from [dbo].[vw_IRNGV] where NationalCode = @nationalCode and Plate2=@plate

    RETURN 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetIRNGV_DiffValue]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetIRNGV_DiffValue](@column  nvarchar(max), @condition nvarchar(max))
RETURNS nvarchar(max)      
AS
BEGIN
    	return (select top(1) @column from [dbo].[tbl_GCR] where @column = @condition)

END
GO
/****** Object:  UserDefinedFunction [dbo].[GetIRNGV_DiffVlues]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetIRNGV_DiffVlues](@nationalCode  nvarchar(max), @plate  nvarchar(max))
RETURNS nvarchar(max)
AS
BEGIN
    declare @result nvarchar(max)
	select @result=[VehicleType]
	  +'_'+[ConstructionYear]
      +'_'+''
      +'_'+[ChassisNumber]
	  +'_'+[NationalCode]
      +'_'+[Plate]
      +'_'+[Plate2]
      +'_'+[HealthCertificate] 
	  from vw_IRNGV where NationalCode=@nationalCode --and Plate2=@plate

	RETURN @result
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetIRNGV_HealthCertificate]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetIRNGV_HealthCertificate](@natinalCode  nvarchar(max), @plate nvarchar(max))
RETURNS nvarchar(max)      
AS
BEGIN
    	return (select top(1) HealthCertificate from [dbo].[vw_IRNGV] where NationalCode = @natinalCode and Plate2=@plate)

END
GO
/****** Object:  UserDefinedFunction [dbo].[GetIRNGV_Rows]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetIRNGV_Rows](@nationalCode  nvarchar(max))
RETURNS @Result TABLE ( [VehicleType]  nvarchar(max)
      ,[ConstructionYear]  nvarchar(4)
      ,[EngineNumber]  nvarchar(max)
      ,[ChassisNumber]  nvarchar(max)
      ,[NationalCode]  nvarchar(max)
      ,[Workshop]  nvarchar(max)
      ,[Plate2]  nvarchar(max)
      ,[Plate]  nvarchar(max)
      ,[HealthCertificate]  nvarchar(max) )
AS
BEGIN
    Insert into @Result 
    SELECT [VehicleType]
      ,[ConstructionYear]
      ,[EngineNumber]
      ,[ChassisNumber]
      ,[NationalCode]
      ,[Workshop]
      ,[Plate2]
      ,[Plate]
      ,[HealthCertificate] from [dbo].[vw_IRNGV] where NationalCode = @nationalCode

    RETURN 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetIRNGV_VehicleType]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetIRNGV_VehicleType](@plate  nvarchar(max))
RETURNS nvarchar(max)      
AS
BEGIN
    	return (select VehicleType from [dbo].[vw_IRNGV] where Plate = @plate)

END
GO
/****** Object:  UserDefinedFunction [dbo].[GetKitConstractors]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetKitConstractors](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select top(1) Constractor from tbl_BankKits where REPLACE(serialNumber,' ','')=REPLACE(@id,' ','') );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetKitReturnofPartsNumber]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetKitReturnofPartsNumber](@id nvarchar(max),@workshopid  nvarchar(max),@FromDate DATETIME,@ToDate DATETIME)
RETURNS int 
AS
BEGIN   
	return ( CASE
                WHEN @workshopid=0 THEN 
				(select ISNULL(SUM(NumberofSend),0) as 'Send' FROM  dbo.tbl_ReturnofParts
					where TypeofPiece= N'کیت' and VehicleTypeID=@id  and CONVERT(VARCHAR(10),Date,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
					GROUP BY VehicleTypeID)

                ELSE 
				(select ISNULL(SUM(NumberofSend),0) as 'Send' FROM  dbo.tbl_ReturnofParts
					where TypeofPiece=N'کیت' and VehicleTypeID=@id and WorkshopID=@workshopid  and CONVERT(VARCHAR(10),Date,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
					GROUP BY VehicleTypeID
				)
             END
        );

END

GO
/****** Object:  UserDefinedFunction [dbo].[GetKitSendNumber]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetKitSendNumber](@id nvarchar(max),@workshopid  nvarchar(max),@FromDate DATETIME,@ToDate DATETIME)
RETURNS int 
AS
BEGIN

	return ( CASE
                WHEN @workshopid=0 THEN 
				(select ISNULL(SUM(tbl_KitDivisionPlans.NumberofSend),0) as 'Send' from  dbo.tbl_KitDivisionPlans inner join tbl_DivisionPlans
				on tbl_KitDivisionPlans.DivisionPlanID=tbl_DivisionPlans.ID
				where tbl_DivisionPlans.Send=1 and tbl_KitDivisionPlans.VehicleTypeID=@id and CONVERT(VARCHAR(10),CreateDate,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
				GROUP BY tbl_KitDivisionPlans.VehicleTypeID)

                ELSE 
				(select ISNULL(SUM(tbl_KitDivisionPlans.NumberofSend),0) as 'Send' from  dbo.tbl_KitDivisionPlans inner join tbl_DivisionPlans
				on tbl_KitDivisionPlans.DivisionPlanID=tbl_DivisionPlans.ID
				where tbl_DivisionPlans.Send=1 and tbl_KitDivisionPlans.VehicleTypeID=@id and tbl_DivisionPlans.WorkshopID=@workshopid  and CONVERT(VARCHAR(10),CreateDate,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
				GROUP BY tbl_KitDivisionPlans.VehicleTypeID, dbo.tbl_DivisionPlans.WorkshopID)
             END
        );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetLisenceCount]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetLisenceCount](@workshopid  nvarchar(max))
RETURNS int 
AS
BEGIN

	return ( CASE
                WHEN @workshopid=0 THEN 
				(	select top(1) SUM(Count) as 'Used' from  [CNGFAPCO].[dbo].[tbl_ContradictionTotals] where ContradictionTypeId='3'
					group by CreateDate
					order by CreateDate desc
				)
				ELSE 
				(
					select top(1) SUM(Count) as 'Used' from  [CNGFAPCO].[dbo].[tbl_ContradictionTotals] where ContradictionTypeId='3'
					and WorkshopId=@workshopid
					group by CreateDate
					order by CreateDate desc
				)
             END
        );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetNIOPDC_Status]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetNIOPDC_Status](@healthcertificate  nvarchar(max))
RETURNS nvarchar(max)
AS
BEGIN
    return (select top(1) Status from tbl_GCR where HealthCertificateID=REPLACE(@healthcertificate,'F',''))
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetOfferedID]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetOfferedID](@id nvarchar(max))
RETURNS float 
AS
BEGIN

	return ( select top(1) ID from tbl_OfferedPrices where WorkshopID=@id order by ID desc );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetOfferedPrice]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetOfferedPrice](@id nvarchar(max))
RETURNS float 
AS
BEGIN

	return ( select top(1) Value from tbl_OfferedPrices where WorkshopID=@id and StatusPay=0 order by ID desc );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetOfferedPriceNoPaied]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetOfferedPriceNoPaied](@id nvarchar(max))
RETURNS float 
AS
BEGIN

	return ( select top(1) Value from tbl_OfferedPrices where WorkshopID=@id and StatusPay=0 order by ID desc );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetOfferedSerial]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetOfferedSerial]()
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select top(1) Serial from tbl_OfferedPrices order by ID desc );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetPreFreeSaleInvoiceCode]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetPreFreeSaleInvoiceCode](@invoiceCode nvarchar(max))
RETURNS nvarchar(max)
AS
BEGIN

	return ( select top(1)InvoiceCode from tbl_FreeSaleInvoices
			where RequestInvoiceCode=@invoiceCode );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetPreFreeSaleInvoiceCount]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetPreFreeSaleInvoiceCount](@invoiceCode nvarchar(max))
RETURNS int
AS
BEGIN

	return ( select COUNT(*) count  from tbl_FreeSaleInvoices
			where RequestInvoiceCode=@invoiceCode );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetPreInvoiceCode]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetPreInvoiceCode](@invoiceCode nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select top(1) PreInvoiceCode from tbl_FinallFreeSaleInvoices where InvoiceCode=@invoiceCode );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetPreInvoiceHint]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetPreInvoiceHint](@id nvarchar(max))
RETURNS float 
AS
BEGIN   
	return ( 
			select ISNULL(SUM(t1.Value),0) Value from (
			select ISNULL(SUM(tbl1.TotalAmountTaxandComplications),0) Value, tbl1.WorkshopsID,tbl1.InvoiceCode,tbl2.PreInvoiceCode,tbl1.RequestInvoiceCode from tbl_FreeSaleInvoices tbl1 left outer join tbl_FinallFreeSaleInvoices tbl2 on tbl1.InvoiceCode=tbl2.PreInvoiceCode
			where tbl1.WorkshopsID=@id and tbl1.SaleCondition=N'غیر نقدی'
			group by tbl1.WorkshopsID,tbl2.PreInvoiceCode,tbl1.InvoiceCode,tbl1.RequestInvoiceCode 
			having tbl2.PreInvoiceCode is null) as t1
	);

END

GO
/****** Object:  UserDefinedFunction [dbo].[GetRegisterCount]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetRegisterCount](@id nvarchar(max),@workshopid  nvarchar(max),@fromdate nvarchar(max),@todate nvarchar(max))
RETURNS int 
AS
BEGIN
    RETURN (
			select COUNT(*) from dbo.tbl_VehicleRegistrations
						where RegisterStatus=1 and RegistrationTypeID=1 and VehicleTypeID=@id and (WorkshopID=@workshopid or @workshopid=0) 
						and CONVERT(VARCHAR(10),CreateDate,120) BETWEEN '' + CONVERT(VARCHAR(10),[dbo].[PersianToMiladi](@fromdate),120) + '' AND '' +  CONVERT(VARCHAR(10),[dbo].[PersianToMiladi](@todate),120)+ ''
		);
END

GO
/****** Object:  UserDefinedFunction [dbo].[GetRegisterCount2]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetRegisterCount2](@workshopid  nvarchar(max),@fromdate nvarchar(max),@todate nvarchar(max))
RETURNS int 
AS
BEGIN
    RETURN (
			select COUNT(*) from dbo.tbl_VehicleRegistrations
						where RegisterStatus=1 and RegistrationTypeID=1 and (WorkshopID=@workshopid or @workshopid=0) 
						and CONVERT(VARCHAR(10),CreateDate,120) BETWEEN '' + CONVERT(VARCHAR(10),@fromdate,120) + '' AND '' +  CONVERT(VARCHAR(10),@todate,120)+ ''
		);
END

GO
/****** Object:  UserDefinedFunction [dbo].[GetRegisterDamagedCount]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetRegisterDamagedCount](@workshopid  nvarchar(max), @FromDate DATETIME ,	@ToDate DATETIME)
RETURNS nvarchar(max) 
AS
BEGIN
    RETURN (select COUNT(*) 
	from resultstable_IRNGVDamages left outer join tbl_Workshops
				on CAST(WorkshopID AS NVARCHAR(10)) = CAST(IRNGVCod AS NVARCHAR(10))
				where ID=@workshopid and 
				[dbo].[PersianToMiladi](CAST(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(Acceptance,N'۰',N'0'),N'۱',N'1'),N'۲',N'2'),N'۳',N'3'),N'۴',N'4'),N'۵',N'5'),N'۶',N'6'),N'۷',N'7'),N'۸',N'8'),N'۹',N'9') as nvarchar(10))) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
				)   
END

--(Year between LEFT(FORMAT(@FromDate,'yyyy/MM/dd','fa'),4) and LEFT(FORMAT(@ToDate,'yyyy/MM/dd','fa'),4))
--and (Month between substring(FORMAT(@FromDate,'yyyy/MM/dd','fa'), 6, 2) and substring(FORMAT(@ToDate,'yyyy/MM/dd','fa'), 6, 2))
GO
/****** Object:  UserDefinedFunction [dbo].[GetRegistrationCount]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetRegistrationCount](@workshopid  nvarchar(max))
RETURNS int 
AS
BEGIN

	return ( CASE
                WHEN @workshopid=0 THEN 
				(	select top(1) SUM(Count) as 'Used' from  [CNGFAPCO].[dbo].[tbl_ContradictionTotals] where ContradictionTypeId='1'
					group by CreateDate
					order by CreateDate desc
				)
				ELSE 
				(
					select top(1) SUM(Count) as 'Used' from  [CNGFAPCO].[dbo].[tbl_ContradictionTotals]
					where WorkshopId=@workshopid and ContradictionTypeId='1'
					group by CreateDate
					order by CreateDate desc
				)
             END
        );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetRegistrationnumber]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetRegistrationnumber](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select Registrationnumber from tbl_Workshops where ID=@id );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetRegistrationPrice]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetRegistrationPrice](@workshopid  nvarchar(max),@type  nvarchar(max),@date DATETIME)
RETURNS float 
AS
BEGIN
--(case @workshopid when 8 then (case when Type like N'%وانت%' then 2100000 else 1600000 end) else (case when Type like N'%وانت%' then 2500000 else 2000000 end) end)
return ( CASE
                WHEN @workshopid=8 THEN 
				(	
					--(case when @type like N'%وانت%' then 2100000 else 1600000 end)
					select Price price from [dbo].[tbl_RegistrationPrice]
					where DepType='2' and Type=(case when @type like N'%وانت%' then '2' else '1' end) and  CONVERT(VARCHAR(10),@date,120) BETWEEN '' + CONVERT(VARCHAR(10),FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),ToDate,120)+ '' 
				)
				
				WHEN @workshopid=44 THEN 
				(	
					--(case when @type like N'%وانت%' then 2100000 else 1600000 end)
					select Price price from [dbo].[tbl_RegistrationPrice]
					where DepType='3' and Type=(case when @type like N'%وانت%' then '2' else '1' end) and  CONVERT(VARCHAR(10),@date,120) BETWEEN '' + CONVERT(VARCHAR(10),FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),ToDate,120)+ '' 
				)

				--WHEN @workshopid=60 THEN 
				--(	
				--	--(case when @type like N'%وانت%' then 2100000 else 1600000 end)
				--	select Price price from [dbo].[tbl_RegistrationPrice]
				--	where DepType='4' and Type=(case when @type like N'%وانت%' then '2' else '1' end) and  CONVERT(VARCHAR(10),@date,120) BETWEEN '' + CONVERT(VARCHAR(10),FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),ToDate,120)+ '' 
				--)
				
				ELSE 
				(
					--(case when @type like N'%وانت%' then 2500000 else 2000000 end)
					------------for added invoce-----------------------------------------------------------------
					--select (case when @type like N'%وانت%' then Price-2500000 else Price-2000000 end) price from [dbo].[tbl_RegistrationPrice]
					select Price price from [dbo].[tbl_RegistrationPrice]
					where DepType='1' and Type=(case when @type like N'%وانت%' then '2' else '1' end) and  CONVERT(VARCHAR(10),@date,120) BETWEEN '' + CONVERT(VARCHAR(10),FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),ToDate,120)+ '' 
				)
             END
        );
END;


GO
/****** Object:  UserDefinedFunction [dbo].[GetRegulatorConstractors]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetRegulatorConstractors](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select top(1) Regulator from tbl_RegulatorConstractors where ID=@id );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetRegulatorSendNumber]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetRegulatorSendNumber](@type nvarchar(max),@workshopid  nvarchar(max))
RETURNS int 
AS
BEGIN   
	return ( select SUM(NumberofSend) Send from tbl_KitDivisionPlans inner join 
			tbl_VehicleTypes on tbl_KitDivisionPlans.VehicleTypeID=tbl_VehicleTypes.ID inner join 
			tbl_DivisionPlans on tbl_KitDivisionPlans.DivisionPlanID=tbl_DivisionPlans.ID
			where Send='1' and tbl_VehicleTypes.ID=@type and WorkshopID=@workshopid
			group by Type,WorkshopID
 );

END

--select sum(tbl1.NumberofSend) Send
--			from tbl_TankDivisionPlans tbl1 RIGHT OUTER JOIN 
--					tbl_TypeofTanks tbl2 on tbl1.TypeofTankID=tbl2.ID inner join 
--					tbl_DivisionPlans tbl3 on tbl1.DivisionPlanID=tbl3.ID
--			where Send=1 and tbl2.Type=@type and WorkshopID=@workshopid
--			group by tbl2.Type,tbl3.WorkshopID
GO
/****** Object:  UserDefinedFunction [dbo].[GetRegulatorUsedNumber]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetRegulatorUsedNumber](@type nvarchar(max),@workshopid  nvarchar(max))
RETURNS int 
AS
BEGIN   
	return ( select COUNT(*) Used 
			from tbl_VehicleRegistrations tbl1 inner join 
			tbl_VehicleTypes tbl2 on tbl1.VehicleTypeID=tbl2.ID inner join 
			tbl_Workshops tbl3 on tbl1.WorkshopID=tbl3.ID
			where tbl2.ID=@type and tbl1.WorkshopID=@workshopid);

END

GO
/****** Object:  UserDefinedFunction [dbo].[GetRemittanceDetailsCost]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetRemittanceDetailsCost](@id nvarchar(max))
RETURNS float 
AS
BEGIN

	return ( select SUM(CarryFare) from tbl_RemittanceDetails Where RemittancesID = @id );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetRemittanceDetailsCount]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetRemittanceDetailsCount](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select COUNT(*) from tbl_RemittanceDetails Where RemittancesID = @id );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetRemittanceUrl]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetRemittanceUrl](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( case when @id< 2373 then 'Remittance' else 'RemittanceWithBOMFixed' end);
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetRemWarehouses]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetRemWarehouses](@id nvarchar(max))
RETURNS float
AS
BEGIN

	return ( select SUM(Rem) from tbl_Warehouses where FinancialCode=@id );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetReplacementDamagesDiscountedPrice]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetReplacementDamagesDiscountedPrice](@Year  nvarchar(4),@Month  nvarchar(2))
RETURNS float 
AS
BEGIN

return ( 
		select DiscountedPrice from tbl_ReplacementPlanPrice
		where LEFT(FORMAT(FromDate,'yyyy/MM/dd','fa'),4)=@Year and substring(FORMAT(FromDate,'yyyy/MM/dd','fa'),6,2)=@Month
        );
END;


GO
/****** Object:  UserDefinedFunction [dbo].[GetReplacementDamagesPrice]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetReplacementDamagesPrice](@Year  nvarchar(4),@Month  nvarchar(2))
RETURNS float 
AS
BEGIN

return ( 
		select Price from tbl_ReplacementPlanPrice
		where LEFT(FORMAT(FromDate,'yyyy/MM/dd','fa'),4)=@Year and substring(FORMAT(FromDate,'yyyy/MM/dd','fa'),6,2)=@Month
        );
END;


GO
/****** Object:  UserDefinedFunction [dbo].[GetReturnofPartsNumber]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetReturnofPartsNumber](@id nvarchar(max),@workshopid  nvarchar(max),@FromDate DATETIME,@ToDate DATETIME)
RETURNS int 
AS
BEGIN   
	return ( CASE
                WHEN @workshopid=0 THEN 
				(select ISNULL(SUM(NumberofSend),0) as 'Send' FROM  dbo.tbl_ReturnofParts
					where TypeofPiece= N'مخزن' and VehicleTypeID=@id  and CONVERT(VARCHAR(10),Date,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
					GROUP BY VehicleTypeID)

                ELSE 
				(select ISNULL(SUM(NumberofSend),0) as 'Send' FROM  dbo.tbl_ReturnofParts
					where TypeofPiece=N'مخزن' and VehicleTypeID=@id and WorkshopID=@workshopid  and CONVERT(VARCHAR(10),Date,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
					GROUP BY VehicleTypeID
				)
             END
        );

END

GO
/****** Object:  UserDefinedFunction [dbo].[GetSaleCondition]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create FUNCTION [dbo].[GetSaleCondition](@id nvarchar(max))
RETURNS nvarchar(max)
AS
BEGIN

	return ( select top(1) SaleCondition from tbl_FreeSaleInvoices where RequestInvoiceCode=@id );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetServiceDesc]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetServiceDesc](@id nvarchar(max))
RETURNS nvarchar(max)
AS
BEGIN

	return ( select Title from tbl_ListofServices where ID=@id );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetServiceRent]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetServiceRent](@id nvarchar(max))
RETURNS float
AS
BEGIN

	return ( select ServiceRent from tbl_ListofServices where ID=@id );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetTankBaseSendNumber]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetTankBaseSendNumber](@id nvarchar(max),@workshopid  nvarchar(max),@FromDate DATETIME,@ToDate DATETIME)
RETURNS int 
AS
BEGIN
    return ( CASE
                WHEN @workshopid=0 THEN 
				(select ISNULL(SUM(tbl_TankBaseDivisionPlans.NumberofSend),0) as 'Send' FROM            dbo.tbl_DivisionPlans INNER JOIN
                         dbo.tbl_TankBaseDivisionPlans ON dbo.tbl_DivisionPlans.ID = dbo.tbl_TankBaseDivisionPlans.DivisionPlanID INNER JOIN
                         dbo.tbl_TypeofTankBases ON dbo.tbl_TankBaseDivisionPlans.TypeofTankBaseID = dbo.tbl_TypeofTankBases.ID INNER JOIN
                         dbo.tbl_VehicleTypes ON dbo.tbl_TypeofTankBases.VehicleTypeId = dbo.tbl_VehicleTypes.ID INNER JOIN
                         dbo.tbl_Workshops ON dbo.tbl_DivisionPlans.WorkshopID = dbo.tbl_Workshops.ID
				where tbl_DivisionPlans.Send=1 and tbl_VehicleTypes.ID=@id and CONVERT(VARCHAR(10),tbl_DivisionPlans.CreateDate,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
				GROUP BY tbl_TankBaseDivisionPlans.TypeofTankBaseID)

                ELSE 
				(select ISNULL(SUM(tbl_TankBaseDivisionPlans.NumberofSend),0) as 'Send' FROM            dbo.tbl_DivisionPlans INNER JOIN
                         dbo.tbl_TankBaseDivisionPlans ON dbo.tbl_DivisionPlans.ID = dbo.tbl_TankBaseDivisionPlans.DivisionPlanID INNER JOIN
                         dbo.tbl_TypeofTankBases ON dbo.tbl_TankBaseDivisionPlans.TypeofTankBaseID = dbo.tbl_TypeofTankBases.ID INNER JOIN
                         dbo.tbl_VehicleTypes ON dbo.tbl_TypeofTankBases.VehicleTypeId = dbo.tbl_VehicleTypes.ID INNER JOIN
                         dbo.tbl_Workshops ON dbo.tbl_DivisionPlans.WorkshopID = dbo.tbl_Workshops.ID
				where tbl_DivisionPlans.Send=1 and tbl_VehicleTypes.ID=@id and tbl_DivisionPlans.WorkshopID=@workshopid  and CONVERT(VARCHAR(10),tbl_DivisionPlans.CreateDate,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
				GROUP BY tbl_TankBaseDivisionPlans.TypeofTankBaseID, dbo.tbl_DivisionPlans.WorkshopID)
             END
        );

END

GO
/****** Object:  UserDefinedFunction [dbo].[GetTankCoverSendNumber]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetTankCoverSendNumber](@id nvarchar(max),@workshopid  nvarchar(max),@FromDate DATETIME,@ToDate DATETIME)
RETURNS int 
AS
BEGIN
    return ( CASE
                WHEN @workshopid=0 THEN 
				(select ISNULL(SUM(tbl_TankCoverDivisionPlans.NumberofSend),0) as 'Send' FROM            dbo.tbl_DivisionPlans INNER JOIN
                         dbo.tbl_TankCoverDivisionPlans ON dbo.tbl_DivisionPlans.ID = dbo.tbl_TankCoverDivisionPlans.DivisionPlanID INNER JOIN
                         dbo.tbl_TypeofTankCovers ON dbo.tbl_TankCoverDivisionPlans.TypeofTankCoverID = dbo.tbl_TypeofTankCovers.ID INNER JOIN
                         dbo.tbl_VehicleTypes ON dbo.tbl_TypeofTankCovers.VehicleTypeId = dbo.tbl_VehicleTypes.ID INNER JOIN
                         dbo.tbl_Workshops ON dbo.tbl_DivisionPlans.WorkshopID = dbo.tbl_Workshops.ID
				where tbl_DivisionPlans.Send=1 and tbl_VehicleTypes.ID=@id and CONVERT(VARCHAR(10),tbl_DivisionPlans.CreateDate,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
				GROUP BY tbl_TankCoverDivisionPlans.TypeofTankCoverID)

                ELSE 
				(select ISNULL(SUM(tbl_TankCoverDivisionPlans.NumberofSend),0) as 'Send' FROM            dbo.tbl_DivisionPlans INNER JOIN
                         dbo.tbl_TankCoverDivisionPlans ON dbo.tbl_DivisionPlans.ID = dbo.tbl_TankCoverDivisionPlans.DivisionPlanID INNER JOIN
                         dbo.tbl_TypeofTankCovers ON dbo.tbl_TankCoverDivisionPlans.TypeofTankCoverID = dbo.tbl_TypeofTankCovers.ID INNER JOIN
                         dbo.tbl_VehicleTypes ON dbo.tbl_TypeofTankCovers.VehicleTypeId = dbo.tbl_VehicleTypes.ID INNER JOIN
                         dbo.tbl_Workshops ON dbo.tbl_DivisionPlans.WorkshopID = dbo.tbl_Workshops.ID
				where tbl_DivisionPlans.Send=1 and tbl_VehicleTypes.ID=@id and tbl_DivisionPlans.WorkshopID=@workshopid  and CONVERT(VARCHAR(10),tbl_DivisionPlans.CreateDate,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
				GROUP BY tbl_TankCoverDivisionPlans.TypeofTankCoverID, dbo.tbl_DivisionPlans.WorkshopID)
             END
        );
END

GO
/****** Object:  UserDefinedFunction [dbo].[GetTankSendNumber]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetTankSendNumber](@id nvarchar(max),@workshopid  nvarchar(max),@FromDate DATETIME,@ToDate DATETIME)
RETURNS int 
AS
BEGIN   
	return ( CASE
                WHEN @workshopid=0 THEN 
				(select ISNULL(SUM(tbl_TankDivisionPlans.NumberofSend),0) as 'Send' FROM  dbo.tbl_DivisionPlans INNER JOIN
                         dbo.tbl_TankDivisionPlans ON dbo.tbl_DivisionPlans.ID = dbo.tbl_TankDivisionPlans.DivisionPlanID INNER JOIN
                         dbo.tbl_TypeofTanks ON dbo.tbl_TankDivisionPlans.TypeofTankID = dbo.tbl_TypeofTanks.ID INNER JOIN
                         dbo.tbl_VehicleTypes ON dbo.tbl_TypeofTanks.VehicleTypeId = dbo.tbl_VehicleTypes.ID INNER JOIN
                         dbo.tbl_Workshops ON dbo.tbl_DivisionPlans.WorkshopID = dbo.tbl_Workshops.ID
				where tbl_DivisionPlans.Send=1 and tbl_VehicleTypes.ID=@id  and CONVERT(VARCHAR(10),tbl_DivisionPlans.CreateDate,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
				GROUP BY tbl_TankDivisionPlans.TypeofTankID)

                ELSE 
				(select ISNULL(SUM(tbl_TankDivisionPlans.NumberofSend),0) as 'Send' FROM  dbo.tbl_DivisionPlans INNER JOIN
                         dbo.tbl_TankDivisionPlans ON dbo.tbl_DivisionPlans.ID = dbo.tbl_TankDivisionPlans.DivisionPlanID INNER JOIN
                         dbo.tbl_TypeofTanks ON dbo.tbl_TankDivisionPlans.TypeofTankID = dbo.tbl_TypeofTanks.ID INNER JOIN
                         dbo.tbl_VehicleTypes ON dbo.tbl_TypeofTanks.VehicleTypeId = dbo.tbl_VehicleTypes.ID INNER JOIN
                         dbo.tbl_Workshops ON dbo.tbl_DivisionPlans.WorkshopID = dbo.tbl_Workshops.ID
				where tbl_DivisionPlans.Send=1 and tbl_VehicleTypes.ID=@id and tbl_DivisionPlans.WorkshopID=@workshopid  and CONVERT(VARCHAR(10),tbl_DivisionPlans.CreateDate,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
				GROUP BY tbl_TankDivisionPlans.TypeofTankID, dbo.tbl_DivisionPlans.WorkshopID)
             END
        );

END

GO
/****** Object:  UserDefinedFunction [dbo].[GetTankSendNumber2]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetTankSendNumber2](@id nvarchar(max),@workshopid  nvarchar(max))
RETURNS int 
AS
BEGIN   
	return ( CASE
                WHEN @workshopid=0 THEN 
				(select ISNULL(SUM(tbl_TankDivisionPlans.NumberofSend),0) as 'Send' FROM  dbo.tbl_DivisionPlans INNER JOIN
                         dbo.tbl_TankDivisionPlans ON dbo.tbl_DivisionPlans.ID = dbo.tbl_TankDivisionPlans.DivisionPlanID INNER JOIN
                         dbo.tbl_TypeofTanks ON dbo.tbl_TankDivisionPlans.TypeofTankID = dbo.tbl_TypeofTanks.ID INNER JOIN
                         dbo.tbl_VehicleTypes ON dbo.tbl_TypeofTanks.VehicleTypeId = dbo.tbl_VehicleTypes.ID INNER JOIN
                         dbo.tbl_Workshops ON dbo.tbl_DivisionPlans.WorkshopID = dbo.tbl_Workshops.ID
				where tbl_VehicleTypes.ID=@id 
				GROUP BY tbl_TankDivisionPlans.TypeofTankID)

                ELSE 
				(select ISNULL(SUM(tbl_TankDivisionPlans.NumberofSend),0) as 'Send' FROM  dbo.tbl_DivisionPlans INNER JOIN
                         dbo.tbl_TankDivisionPlans ON dbo.tbl_DivisionPlans.ID = dbo.tbl_TankDivisionPlans.DivisionPlanID INNER JOIN
                         dbo.tbl_TypeofTanks ON dbo.tbl_TankDivisionPlans.TypeofTankID = dbo.tbl_TypeofTanks.ID INNER JOIN
                         dbo.tbl_VehicleTypes ON dbo.tbl_TypeofTanks.VehicleTypeId = dbo.tbl_VehicleTypes.ID INNER JOIN
                         dbo.tbl_Workshops ON dbo.tbl_DivisionPlans.WorkshopID = dbo.tbl_Workshops.ID
				where tbl_VehicleTypes.ID=@id and tbl_DivisionPlans.WorkshopID in (Select ParsedString  From [dbo].[ParseStringList](@workshopid))
				GROUP BY tbl_TankDivisionPlans.TypeofTankID, dbo.tbl_DivisionPlans.WorkshopID)
             END
        );

END

GO
/****** Object:  UserDefinedFunction [dbo].[GetTypeofTankBases]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetTypeofTankBases](@id  nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN
    RETURN (select Type from dbo.tbl_TypeofTankBases where VehicleTypeId=@id or @id=0)   
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetTypeofTankCovers]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetTypeofTankCovers](@id  nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN
    RETURN (select Type from dbo.tbl_TypeofTankCovers where VehicleTypeId=@id or @id=0)   
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetTypeofUse]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetTypeofUse](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select Type from tbl_TypeofUses where ID=@id );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetUsers]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetUsers](@id nvarchar(max))
RETURNS nvarchar(max)
AS
BEGIN

	return ( select Firstname + ' ' + Lastname from Users where UserID=@id );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetValueAdded]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetValueAdded](@Year nvarchar(max))
RETURNS float 
AS
BEGIN
return ( case
	
		when (convert(int, @Year) <= 1402) then
		(select (Value/100) from tbl_TaxandComplications
			where Year = 1402)
	
		else
			(select (Value/100) from tbl_TaxandComplications
			where Year = @Year)
		end

	);
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetValveConstractors]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetValveConstractors](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select Valve from tbl_ValveConstractors where ID=@id );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetValveReturnofPartsNumber]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetValveReturnofPartsNumber](@id nvarchar(max),@workshopid  nvarchar(max),@FromDate DATETIME,@ToDate DATETIME)
RETURNS int 
AS
BEGIN   
	return ( CASE
                WHEN @workshopid=0 THEN 
				(select ISNULL(SUM(NumberofSend),0) as 'Send' FROM  dbo.tbl_ReturnofParts
					where TypeofPiece= N'شیر مخزن' and VehicleTypeID=@id  and CONVERT(VARCHAR(10),Date,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
					GROUP BY VehicleTypeID)

                ELSE 
				(select ISNULL(SUM(NumberofSend),0) as 'Send' FROM  dbo.tbl_ReturnofParts
					where TypeofPiece=N'شیر مخزن' and VehicleTypeID=@id and WorkshopID=@workshopid  and CONVERT(VARCHAR(10),Date,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
					GROUP BY VehicleTypeID
				)
             END
        );

END

GO
/****** Object:  UserDefinedFunction [dbo].[GetValveSendNumber]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetValveSendNumber](@workshopid  nvarchar(max),@FromDate DATETIME,@ToDate DATETIME)
RETURNS int 
AS
BEGIN   
	return ( CASE
                WHEN @workshopid=0 THEN 
				(select ISNULL(SUM(tbl_ValveDivisionPlans.NumberofSend),0) as 'Send' FROM  dbo.tbl_DivisionPlans INNER JOIN
                         dbo.tbl_ValveDivisionPlans ON dbo.tbl_DivisionPlans.ID = dbo.tbl_ValveDivisionPlans.DivisionPlanID INNER JOIN                         
                         dbo.tbl_Workshops ON dbo.tbl_DivisionPlans.WorkshopID = dbo.tbl_Workshops.ID
				where tbl_DivisionPlans.Send=1 and CONVERT(VARCHAR(10),tbl_DivisionPlans.CreateDate,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
				)

                ELSE 
				(select ISNULL(SUM(tbl_ValveDivisionPlans.NumberofSend),0) as 'Send' FROM  dbo.tbl_DivisionPlans INNER JOIN
                         dbo.tbl_ValveDivisionPlans ON dbo.tbl_DivisionPlans.ID = dbo.tbl_ValveDivisionPlans.DivisionPlanID INNER JOIN                         
                         dbo.tbl_Workshops ON dbo.tbl_DivisionPlans.WorkshopID = dbo.tbl_Workshops.ID
				where tbl_DivisionPlans.Send=1 and tbl_DivisionPlans.WorkshopID=@workshopid  and CONVERT(VARCHAR(10),tbl_DivisionPlans.CreateDate,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
				GROUP BY dbo.tbl_DivisionPlans.WorkshopID)
             END
        );

END

GO
/****** Object:  UserDefinedFunction [dbo].[GetVehicleCard]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetVehicleCard](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select top(1) dbo.fn_FileExists(N'D:\www.cngfapco.com\UploadedFiles\Vehicle\VehicleCard\'+Image) from tbl_VehicleAttachments where VehicleRegistrationID=@id and Folder='VehicleCard' );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetVehicleCount]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetVehicleCount](@workshopid  nvarchar(max))
RETURNS int 
AS
BEGIN

	return ( CASE
                WHEN @workshopid=0 THEN 
				(select COUNT(*) as 'Used' from  dbo.tbl_VehicleRegistrations where RegisterStatus=1 and RegistrationTypeID=1)
				ELSE 
				(select COUNT(*) as 'Used' from  dbo.tbl_VehicleRegistrations
					where RegisterStatus=1 and RegistrationTypeID=1 and tbl_VehicleRegistrations.WorkshopID=@workshopid
				)
             END
        );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetVehicleCylinderContractors]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetVehicleCylinderContractors](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select top(1) TankConstractorID from tbl_VehicleTanks where VehicleRegistrationID=@id );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetVehicleCylinders]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetVehicleCylinders](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select top(1) Serial from tbl_VehicleTanks where VehicleRegistrationID=@id );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetVehicleHealthCertificate]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetVehicleHealthCertificate](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select top(1) dbo.fn_FileExists(N'D:\www.cngfapco.com\UploadedFiles\Vehicle\HealthCertificate\'+Image) from tbl_VehicleAttachments where VehicleRegistrationID=@id and Folder='HealthCertificate' );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetVehicleLicenseImage]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetVehicleLicenseImage](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select top(1) dbo.fn_FileExists(N'D:\www.cngfapco.com\UploadedFiles\Vehicle\LicenseImage\'+Image) from tbl_VehicleAttachments where VehicleRegistrationID=@id and Folder='LicenseImage' );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetVehicleNationalCard]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetVehicleNationalCard](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select top(1)  dbo.fn_FileExists(N'D:\www.cngfapco.com\UploadedFiles\Vehicle\NationalCard\'+Image) from tbl_VehicleAttachments where VehicleRegistrationID=@id and Folder='NationalCard');
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetVehicleRegisterDate]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetVehicleRegisterDate](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select CreateDate from tbl_VehicleRegistrations where ID=@id );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetVehicleRegistrationInfo]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetVehicleRegistrationInfo](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select N'نام و نام خ: '+ OwnerName+' ' + OwnerFamily + ' - ' + N'کد ملی: ' + NationalCode + ' - ' + N'شماره شاسی:  ' + ChassisNumber  from tbl_VehicleRegistrations
			where ID=@id );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetVehicleTankBaseCount]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetVehicleTankBaseCount](@id nvarchar(max),@workshopid  nvarchar(max))
RETURNS int 
AS
BEGIN
    RETURN (select ISNULL(SUM(tbl_TankBaseDivisionPlans.NumberofSend),0) as 'Send' from  tbl_TankBaseDivisionPlans inner join tbl_TypeofTankBases
			on tbl_TankBaseDivisionPlans.TypeofTankBaseID=tbl_TypeofTankBases.ID inner join tbl_DivisionPlans on tbl_TankBaseDivisionPlans.DivisionPlanID=tbl_DivisionPlans.ID
	where tbl_TypeofTankBases.VehicleTypeId=@id and (tbl_DivisionPlans.WorkshopID=@workshopid or @workshopid=0))   
END

GO
/****** Object:  UserDefinedFunction [dbo].[GetVehicleTankCount]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetVehicleTankCount](@id nvarchar(max),@workshopid  nvarchar(max))
RETURNS int 
AS
BEGIN
    RETURN (select COUNT(*) from dbo.tbl_VehicleTanks inner join dbo.tbl_VehicleRegistrations on tbl_VehicleTanks.VehicleRegistrationID=tbl_VehicleRegistrations.ID	
	where dbo.tbl_VehicleTanks.Volume=@id and (tbl_VehicleRegistrations.WorkshopID=@workshopid or @workshopid=0) )   
END

GO
/****** Object:  UserDefinedFunction [dbo].[GetVehicleTankCoverCount]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[GetVehicleTankCoverCount](@id nvarchar(max),@workshopid  nvarchar(max))
RETURNS int 
AS
BEGIN
    RETURN (select COUNT(*) from dbo.tbl_VehicleRegistrations
	where dbo.tbl_VehicleRegistrations.VehicleTypeID=@id)   
END

GO
/****** Object:  UserDefinedFunction [dbo].[GetVehicleTechnicalDiagnosis]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetVehicleTechnicalDiagnosis](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select top(1) dbo.fn_FileExists(N'D:\www.cngfapco.com\UploadedFiles\Vehicle\TechnicalDiagnosis\'+Image) from tbl_VehicleAttachments where VehicleRegistrationID=@id and Folder='TechnicalDiagnosis' );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetVehicleType]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetVehicleType](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select Type + ' ' + Description from tbl_VehicleTypes where ID=@id );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetVehicleTypeCount]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetVehicleTypeCount](@id nvarchar(max),@workshopid  nvarchar(max))
RETURNS int 
AS
BEGIN
    RETURN (
			( CASE
					WHEN @id=1 THEN 
					(select COUNT(*) from dbo.tbl_VehicleRegistrations
						where RegisterStatus=1 and RegistrationTypeID=1 and  (VehicleTypeID=@id or VehicleTypeID=6) and (WorkshopID=@workshopid or @workshopid=0))

					ELSE 
					(select COUNT(*) from dbo.tbl_VehicleRegistrations
						where RegisterStatus=1 and RegistrationTypeID=1 and VehicleTypeID=@id and (WorkshopID=@workshopid or @workshopid=0))
				 END
			)
		);
END

GO
/****** Object:  UserDefinedFunction [dbo].[GetVehicleUsed]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetVehicleUsed](@id nvarchar(max),@workshopid  nvarchar(max),@FromDate DATETIME,@ToDate DATETIME)--,@RegistrationTypeID nvarchar(max)
RETURNS int 
AS
BEGIN

	return ( CASE
                WHEN @workshopid=0 THEN 
				(select COUNT(*) as 'Used' from  dbo.tbl_VehicleRegistrations
					where RegisterStatus=1 and  tbl_VehicleRegistrations.VehicleTypeID=@id and CONVERT(VARCHAR(10),CreateDate,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
				)--and RegistrationTypeID=@RegistrationTypeID 

                ELSE 
				(select COUNT(*) as 'Used' from  dbo.tbl_VehicleRegistrations
					where RegisterStatus=1 and tbl_VehicleRegistrations.VehicleTypeID=@id and tbl_VehicleRegistrations.WorkshopID=@workshopid  and CONVERT(VARCHAR(10),CreateDate,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
				)--and RegistrationTypeID=@RegistrationTypeID
             END
        );
END

--RegistrationTypeID=1 and
GO
/****** Object:  UserDefinedFunction [dbo].[GetVehicleValveConstractors]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetVehicleValveConstractors](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select top(1) ValveConstractorID from tbl_VehicleTanks where VehicleRegistrationID=@id );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetVehicleValveCount]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetVehicleValveCount](@workshopid  nvarchar(max),@fromDate datetime,@toDate datetime)
RETURNS int 
AS
BEGIN
    RETURN (select COUNT(*) from dbo.tbl_VehicleRegistrations
			where (tbl_VehicleRegistrations.WorkshopID=@workshopid or @workshopid=0) and tbl_VehicleRegistrations.CreateDate between @fromDate and @toDate)   
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetVehicleValves]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetVehicleValves](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select top(1) SerialTankValve from tbl_VehicleTanks where VehicleRegistrationID=@id );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetWorkshopAddress]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetWorkshopAddress](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select (tbl_Cities.Title +' - ' + Address) from tbl_Workshops INNER JOIN
                         dbo.tbl_Cities ON dbo.tbl_Workshops.CityID = dbo.tbl_Cities.ID INNER JOIN
                         dbo.tbl_States ON dbo.tbl_Cities.StateID = dbo.tbl_States.ID
			where tbl_Workshops.ID=@id );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetWorkshopByVehicleRegistration]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetWorkshopByVehicleRegistration](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select [dbo].[GetWorkshops](WorkshopID) from tbl_VehicleRegistrations
			where ID=@id );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetWorkshopEconomic]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetWorkshopEconomic](@ID nvarchar(max))
RETURNS nvarchar(max)
AS
BEGIN
return ( select Economicalnumber from tbl_Workshops
		where ID=@ID
	);
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetWorkshopEconomicStatus]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetWorkshopEconomicStatus](@ID nvarchar(max))
RETURNS int
AS
BEGIN
return ( select case when
			convert(float,(case when [dbo].[GetWorkshopEconomic](@ID) IS null or [dbo].[GetWorkshopEconomic](@ID)='-' 
				then '0' else [dbo].[GetWorkshopEconomic](@ID) end))>0 
					then '1' 
						else '0' 
			end
	);
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetWorkshopPhone]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[GetWorkshopPhone](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select PhoneNumber from tbl_Workshops where ID=@id );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetWorkshops]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetWorkshops](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select REPLACE(tbl_Workshops.Title,N'مرکز خدمات CNG ','')+ ' - ' + tbl_Cities.Title from tbl_Workshops 
			inner join tbl_Cities on tbl_Workshops.CityID=tbl_Cities.ID 
			inner join tbl_States on tbl_Cities.StateID=tbl_States.ID 
			where tbl_Workshops.ID=@id );
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetWorkshopState]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetWorkshopState](@id nvarchar(max))
RETURNS nvarchar(max) 
AS
BEGIN

	return ( select tbl_States.Title from tbl_Workshops INNER JOIN
                         dbo.tbl_Cities ON dbo.tbl_Workshops.CityID = dbo.tbl_Cities.ID INNER JOIN
                         dbo.tbl_States ON dbo.tbl_Cities.StateID = dbo.tbl_States.ID
			where tbl_Workshops.ID=@id );
END


GO
/****** Object:  UserDefinedFunction [dbo].[InsertDoubleRow_DamagesInvoice]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      <Author,,Name>
-- Create date: <Create Date,,>
-- Description: <Description,,>
-- =============================================
CREATE FUNCTION  [dbo].[InsertDoubleRow_DamagesInvoice]
(
   @ID int
)
RETURNS 
@tbl_DoubleRowDamagesInvoice TABLE 
(
    -- Add the column definitions for the TABLE variable here
    [Literage] char(4),
	[Literage_2] char(4),
	[Weight] float,
	[Weight_2] float,
	[Literage_Text] varchar(max),
	[WorkshopID] int,
	[Year] int,
	[Month] int,
	Plate nvarchar(max),
	ChassisNumber nvarchar(max),
	VehicleType nvarchar(max),
	Acceptance nvarchar(max),
	UnitAmount float,
	TotalAmount float,
	DiscountAmount float,
	Amount float
)
AS
BEGIN
DECLARE @myTable table 
(
    -- Add the column definitions for the TABLE variable here
    [Literage] char(4),
	[Literage_2] char(4),
	[Weight] float,
	[Weight_2] float,
	[Literage_Text] varchar(max),
	[WorkshopID] int,
	[Year] int,
	[Month] int,
	Plate nvarchar(max),
	ChassisNumber nvarchar(max),
	VehicleType nvarchar(max),
	Acceptance nvarchar(max),
	UnitAmount float,
	TotalAmount float,
	DiscountAmount float,
	Amount float
)

insert into @myTable 
select literage_2,literage,weight_2,weight,literage_2,workshopID,Year,Month,Plate,ChassisNumber,VehicleType,Acceptance,UnitAmount,
		0,0,0
from resultstable_IRNGVDamages where uniqId=@ID


--This select returns data
insert into  @tbl_DoubleRowDamagesInvoice
select * from @myTable where Literage_2 in (20,28)

return
END
GO
/****** Object:  UserDefinedFunction [dbo].[Num_ToWords]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE FUNCTION [dbo].[Num_ToWords] (
 
    @Number Numeric (38, 0) -- Input number with as many as 18 digits
 
) RETURNS NVARCHAR(max)
 
AS BEGIN

DECLARE @inputNumber nVARCHAR(38)
DECLARE @NumbersTable TABLE (number int, word VARCHAR(10))
DECLARE @outputString nVARCHAR(max)
DECLARE @length INT
DECLARE @counter INT
DECLARE @loops INT
DECLARE @position INT
DECLARE @chunk CHAR(3)
DECLARE @tensones CHAR(2)
DECLARE @hundreds CHAR(1)
DECLARE @tens CHAR(1)
DECLARE @ones CHAR(1)
DECLARE @And nvarchar(3)
DECLARE @Neg nvarchar(10)
 
    IF @Number = 0 return 'صفر'
    
    IF  Left(@Number ,1) <> '-'
        SET  @Neg = ' '
    ELSE
    BEGIN
        SET  @Neg = 'منفی '
        SET  @Number = @Number * -1
    END
 
SELECT @inputNumber = CONVERT(varchar(38), @Number)
     , @outputString = ''
     , @counter = 1
SELECT @length   = LEN(@inputNumber)
     , @position = LEN(@inputNumber) - 2
     , @loops    = LEN(@inputNumber)/3
 
 
IF LEN(@inputNumber) % 3 <> 0 SET @loops = @loops + 1
 
 
INSERT INTO @NumbersTable   SELECT 0, ''
    UNION ALL SELECT 1, ' یک '      UNION ALL SELECT 2, ' دو '
    UNION ALL SELECT 3, ' سه '    UNION ALL SELECT 4, ' چهار '
    UNION ALL SELECT 5, ' پنج '     UNION ALL SELECT 6, ' شش '
    UNION ALL SELECT 7, ' هفت '    UNION ALL SELECT 8, ' هشت '
    UNION ALL SELECT 9, ' نه '     UNION ALL SELECT 10, ' ده '
    UNION ALL SELECT 11, ' یازده '   UNION ALL SELECT 12, ' دوازده '
    UNION ALL SELECT 13, ' سیزده ' UNION ALL SELECT 14, ' چهارده '
    UNION ALL SELECT 15, ' پانزده '  UNION ALL SELECT 16, ' شانزده '
    UNION ALL SELECT 17, ' هفده ' UNION ALL SELECT 18, ' هیجده '
    UNION ALL SELECT 19, ' نوزده ' UNION ALL SELECT 20, ' بیست '
    UNION ALL SELECT 30, ' سی '   UNION ALL SELECT 40, ' چهل '
    UNION ALL SELECT 50, ' پنجاه '    UNION ALL SELECT 60, ' شصت '
    UNION ALL SELECT 70, ' هفتاد '  UNION ALL SELECT 80, ' هشتاد '
    UNION ALL SELECT 90, ' نود '   UNION ALL SELECT 100, ' صد '
    UNION ALL SELECT 200, ' دویست '   UNION ALL SELECT 300, ' سیصد '
    UNION ALL SELECT 400, ' چهارصد '   UNION ALL SELECT 500, ' پانصد '
    UNION ALL SELECT 600, ' ششصد '   UNION ALL SELECT 700, ' هفتصد '
    UNION ALL SELECT 800, ' هشتصد '   UNION ALL SELECT 900, ' نهصد '
 
 
WHILE @counter <= @loops BEGIN
 
    SET @chunk = RIGHT('000' + SUBSTRING(@inputNumber, @position, 3), 3)
    
 
    IF @chunk <> '000' BEGIN
        SELECT @tensones = SUBSTRING(@chunk, 2, 2)
             , @hundreds = SUBSTRING(@chunk, 1, 1)
             , @tens = SUBSTRING(@chunk, 2, 1)
             , @ones = SUBSTRING(@chunk, 3, 1)
 
        IF CONVERT(INT, @tensones) <= 20 OR @Ones='0' BEGIN
                if len(@outputString)>0
                    begin
                    set @And='و '
                    end
                else
                    begin
                    set @And=''
                    end
                    
                SET @outputString = (SELECT word
                                    FROM @NumbersTable
                                    WHERE @hundreds+'00'  = number)
                            + case @hundreds when '0' then '' else
                                case @tensones when '00' then '' else 'و' end  end+
                            (
                                    SELECT word
                                      FROM @NumbersTable
                                      WHERE @tensones = number)
                   + CASE @counter WHEN 1 THEN '' -- No name
                       WHEN 2 THEN ' هزار ' WHEN 3 THEN ' میلیون '
                       WHEN 4 THEN ' میلیارد '  WHEN 5 THEN ' بیلیون '
                       WHEN 6 THEN ' بیلیارد ' WHEN 7 THEN ' کوانتیلیون '
                       WHEN 8 THEN ' سکستیلیون '  WHEN 9 THEN ' سپتیلیون '
                       WHEN 10 THEN ' اکتیلیون '  WHEN 11 THEN ' نونیلیون '
                       WHEN 12 THEN ' دسیلیون '  WHEN 13 THEN ' اندسیلیون '
                       ELSE '' END
                               + @And + @outputString
            END
 
         ELSE BEGIN
 
                    if len(@outputString)>0
                    begin
                    set @And='و '
                    end
                else
                    begin
                    set @And=''
                    end
                    
                
 
                SET @outputString = ' '
                            + (SELECT word
                                    FROM @NumbersTable
                                    WHERE @hundreds+'00'  = number)
                             + case @hundreds when '0' then '' else  'و' end
                            + (SELECT word
                                    FROM @NumbersTable
                                    WHERE   @tens+'0'  = number)
                             + 'و'
                             + (SELECT word
                                    FROM @NumbersTable
                                    WHERE  @ones = number)
                   + CASE @counter WHEN 1 THEN '' -- No name
                       WHEN 2 THEN ' هزار ' WHEN 3 THEN ' میلیون '
                       WHEN 4 THEN ' میلیارد '  WHEN 5 THEN ' بیلیون '
                       WHEN 6 THEN ' بیلیارد ' WHEN 7 THEN ' کوانتیلیون '
                       WHEN 8 THEN ' سکستیلیون '  WHEN 9 THEN ' سپتیلیون '
                       WHEN 10 THEN ' اکتیلیون '  WHEN 11 THEN ' نونیلیون '
                       WHEN 12 THEN ' دسیلیون '  WHEN 13 THEN ' اندسیلیون '
                       ELSE '' END
                            + @And + @outputString
        END
        
 
    END
 
    SELECT @counter = @counter + 1
         , @position = @position - 3
 
END
 
SET @outputString = LTRIM(RTRIM(REPLACE(@outputString, '  ', ' ')))
SET @outputstring = UPPER(LEFT(@outputstring, 1)) + SUBSTRING(@outputstring, 2, 8000)
SET @outputstring = @Neg + @outputstring
 
RETURN @outputString
END
GO
/****** Object:  UserDefinedFunction [dbo].[ParseStringList]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create Function [dbo].[ParseStringList]  (@StringArray nvarchar(max) )  
Returns @tbl_string Table  (ParsedString nvarchar(max))  As  

BEGIN 

DECLARE @end Int,
        @start Int

SET @stringArray =  @StringArray + ',' 
SET @start=1
SET @end=1

WHILE @end<Len(@StringArray)
    BEGIN
        SET @end = CharIndex(',', @StringArray, @end)
        INSERT INTO @tbl_string 
            SELECT
                Substring(@StringArray, @start, @end-@start)

        SET @start=@end+1
        SET @end = @end+1
    END

RETURN
END
GO
/****** Object:  UserDefinedFunction [dbo].[PersianToMiladi]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      MohammadSoori
-- Create date: 2021-06-21
-- Description: Convert Persian date to Miladi date.
-- =============================================
-- SELECT [dbo].[PersianToMiladi] ('1400/01/01')
-- =============================================
Create FUNCTION [dbo].[PersianToMiladi]
(
    @PersianDate VARCHAR(10)
)
RETURNS DATE
AS
BEGIN
    SET @PersianDate = RIGHT (@PersianDate, 9)
    DECLARE @Year INT = SUBSTRING(@PersianDate, 1, 3)
    DECLARE @Month INT = SUBSTRING(@PersianDate, 5, 2)
    DECLARE @Day INT = SUBSTRING(@PersianDate, 8, 2)
    DECLARE @DiffYear INT = @Year - 350

    DECLARE @Days INT = @DiffYear * 365.24 +
    CASE WHEN @Month < 7 THEN (@Month - 1) * 31
         ELSE 186 + (@Month - 7) * 30 END + @Day

    DECLARE @StartDate DATETIME = '03/21/1971'
    DECLARE @ResultDate DATE = @StartDate + @Days

    RETURN CONVERT(DATE, @ResultDate)  

END
GO
/****** Object:  UserDefinedFunction [dbo].[udf_GetNumeric]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create FUNCTION [dbo].[udf_GetNumeric]
(@strAlphaNumeric VARCHAR(256))
RETURNS VARCHAR(256)
AS
BEGIN
DECLARE @intAlpha INT
SET @intAlpha = PATINDEX('%[^0-9]%', @strAlphaNumeric)
BEGIN
WHILE @intAlpha > 0
BEGIN
SET @strAlphaNumeric = STUFF(@strAlphaNumeric, @intAlpha, 1, '' )
SET @intAlpha = PATINDEX('%[^0-9]%', @strAlphaNumeric )
END
END
RETURN ISNULL(@strAlphaNumeric,0)
END
GO
/****** Object:  Table [dbo].[tbl_KitDivisionPlans]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_KitDivisionPlans](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[VehicleTypeID] [int] NOT NULL,
	[Number] [int] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[NumberofSend] [int] NULL,
	[DivisionPlanID] [int] NULL,
	[GenarationID] [int] NULL,
	[RegistrationTypeID] [int] NULL,
 CONSTRAINT [PK_dbo.tbl_KitDivisionPlans] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_VehicleTypes]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_VehicleTypes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Image] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_VehicleTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[DivisionKits]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Function [dbo].[DivisionKits]  (@id nvarchar(max) )  

RETURNS TABLE
AS
RETURN(select N'کیت'+ ' ' + tt.Type as Title, N'عدد' as Unit, td.NumberofSend, td.Description from tbl_KitDivisionPlans td inner join 
		tbl_VehicleTypes tt on td.VehicleTypeID=tt.ID 
where td.DivisionPlanID=@id)
GO
/****** Object:  Table [dbo].[tbl_TankBaseDivisionPlans]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_TankBaseDivisionPlans](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DivisionPlanID] [int] NULL,
	[TypeofTankBaseID] [int] NOT NULL,
	[Number] [int] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[NumberofSend] [int] NULL,
	[GenarationID] [int] NULL,
	[RegistrationTypeID] [int] NULL,
 CONSTRAINT [PK_dbo.tbl_TankBaseDivisionPlans] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_TypeofTankBases]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_TypeofTankBases](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[VehicleTypeId] [int] NULL,
 CONSTRAINT [PK_dbo.tbl_TypeofTankBases] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[DivisionTankBase]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Function [dbo].[DivisionTankBase]  (@id nvarchar(max) )  

RETURNS TABLE
AS
RETURN(select N'پایه مخزن '+ '' + tt.Type as Title, N'عدد' as Unit, td.NumberofSend, td.Description from tbl_TankBaseDivisionPlans td inner join 
		tbl_TypeofTankBases tt on td.TypeofTankBaseID=tt.ID 
where td.DivisionPlanID=@id)
GO
/****** Object:  Table [dbo].[tbl_TankCoverDivisionPlans]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_TankCoverDivisionPlans](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DivisionPlanID] [int] NULL,
	[TypeofTankCoverID] [int] NOT NULL,
	[Number] [int] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[NumberofSend] [int] NULL,
	[GenarationID] [int] NULL,
	[RegistrationTypeID] [int] NULL,
 CONSTRAINT [PK_dbo.tbl_TankCoverDivisionPlans] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[DivisionTankCovers]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Function [dbo].[DivisionTankCovers]  (@id nvarchar(max) )  

RETURNS TABLE
AS
RETURN(select N'کاور مخزن '+ '' + tt.Type as Title, N'عدد' as Unit, td.NumberofSend, td.Description from tbl_TankCoverDivisionPlans td inner join 
		tbl_TypeofTankBases tt on td.TypeofTankCoverID=tt.ID 
where td.DivisionPlanID=@id)
GO
/****** Object:  Table [dbo].[tbl_TankConstractors]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_TankConstractors](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Constractor] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Code] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_TankConstractors] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_TankDivisionPlans]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_TankDivisionPlans](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DivisionPlanID] [int] NULL,
	[TypeofTankID] [int] NOT NULL,
	[Number] [int] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[NumberofSend] [int] NULL,
	[TankConstractorID] [int] NULL,
	[GenarationID] [int] NULL,
	[RegistrationTypeID] [int] NULL,
 CONSTRAINT [PK_dbo.tbl_TankDivisionPlans] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_TypeofTanks]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_TypeofTanks](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[VehicleTypeId] [int] NULL,
 CONSTRAINT [PK_dbo.tbl_TypeofTanks] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[DivisionTanks]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Function [dbo].[DivisionTanks]  (@id nvarchar(max) )  

RETURNS TABLE
AS
RETURN(select N'مخزن' + ' ' + tt.Type + ' ' + N'لیتری' +'- ' + tc.Constractor  as Title, N'عدد' as Unit, td.NumberofSend, td.Description from tbl_TankDivisionPlans td inner join 
		tbl_TypeofTanks tt on td.TypeofTankID=tt.ID inner join 
		tbl_TankConstractors tc on td.TankConstractorID=tc.ID
where td.DivisionPlanID=@id)
GO
/****** Object:  Table [dbo].[tbl_ValveConstractors]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_ValveConstractors](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Valve] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Code] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_ValveConstractors] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_ValveDivisionPlans]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_ValveDivisionPlans](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](max) NOT NULL,
	[ValveConstractorID] [int] NOT NULL,
	[Number] [int] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[NumberofSend] [int] NULL,
	[DivisionPlanID] [int] NULL,
	[GenarationID] [int] NULL,
	[RegistrationTypeID] [int] NULL,
 CONSTRAINT [PK_dbo.tbl_ValveDivisionPlans] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[DivisionTankValves]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Function [dbo].[DivisionTankValves]  (@id nvarchar(max) )  

RETURNS TABLE
AS
RETURN(select N'شیر مخزن'+ ' ' + vd.Type + '- ' + N' شرکت '+ vc.Valve as Title, N'عدد' as Unit, vd.NumberofSend, vd.Description from tbl_ValveDivisionPlans vd inner join tbl_ValveConstractors vc on vd.ValveConstractorID=vc.ID
where vd.DivisionPlanID=@id)
GO
/****** Object:  Table [dbo].[tbl_VehicleTanks]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_VehicleTanks](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[VehicleRegistrationID] [int] NULL,
	[Serial] [nvarchar](max) NOT NULL,
	[Volume] [float] NOT NULL,
	[TankConstractorID] [int] NULL,
	[ProductDate] [nvarchar](max) NOT NULL,
	[ExpirationDate] [nvarchar](max) NOT NULL,
	[SerialTankValve] [nvarchar](max) NOT NULL,
	[TypeTankValve] [nvarchar](max) NOT NULL,
	[Creator] [int] NULL,
	[CreateDate] [datetime] NOT NULL,
	[ValveConstractorID] [int] NOT NULL,
	[RegulatorSerial] [nvarchar](max) NOT NULL,
	[RegulatorConstractorID] [int] NULL,
	[CutofValveSerial] [nvarchar](max) NULL,
	[CutofValveConstractorID] [int] NULL,
	[FillingValveSerial] [nvarchar](max) NULL,
	[FillingValveConstractorID] [int] NULL,
	[FuelRelaySerial] [nvarchar](max) NULL,
	[FuelRelayConstractorID] [int] NULL,
	[GasECUSerial] [nvarchar](max) NULL,
	[GasECUConstractorID] [int] NULL,
	[GenarationID] [int] NULL,
 CONSTRAINT [PK_dbo.tbl_VehicleTanks] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Workshops]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Workshops](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[CityID] [int] NOT NULL,
	[PhoneNumber] [nvarchar](max) NOT NULL,
	[MobileNumber] [nvarchar](max) NOT NULL,
	[FaxNumber] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NOT NULL,
	[Creator] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
	[BusinessLicense] [nvarchar](max) NULL,
	[OwnerName] [nvarchar](max) NOT NULL,
	[OwnerFamily] [nvarchar](max) NOT NULL,
	[FapCode] [nvarchar](max) NULL,
	[isServices] [bit] NULL,
	[Economicalnumber] [nvarchar](max) NULL,
	[Registrationnumber] [nvarchar](max) NULL,
	[Postalcode] [nvarchar](max) NULL,
	[Logo] [nvarchar](max) NULL,
	[CompaniesID] [int] NULL,
	[Auditor] [nvarchar](max) NULL,
	[closedServices] [bit] NULL,
	[closedDate] [datetime] NULL,
	[IRNGVCod] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_Workshops] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_VehicleRegistrations]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_VehicleRegistrations](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[VehicleTypeID] [int] NULL,
	[System] [nvarchar](max) NOT NULL,
	[OwnerName] [nvarchar](max) NOT NULL,
	[OwnerFamily] [nvarchar](max) NOT NULL,
	[NationalCard] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[MobileNumber] [nvarchar](max) NOT NULL,
	[ConstructionYear] [nvarchar](4) NOT NULL,
	[LeftNumberPlate] [nvarchar](max) NOT NULL,
	[AlphaPlate] [nvarchar](max) NOT NULL,
	[RightNumberPlate] [nvarchar](max) NOT NULL,
	[IranNumberPlate] [nvarchar](max) NOT NULL,
	[EngineNumber] [nvarchar](max) NOT NULL,
	[ChassisNumber] [nvarchar](max) NOT NULL,
	[VehicleCard] [nvarchar](max) NULL,
	[Creator] [int] NULL,
	[CreateDate] [datetime] NOT NULL,
	[SerialSparkPreview] [nvarchar](max) NULL,
	[SerialKit] [nvarchar](max) NULL,
	[SerialKey] [nvarchar](max) NULL,
	[SerialRefuelingValve] [nvarchar](max) NULL,
	[RefuelingLable] [nvarchar](max) NULL,
	[TrackingCode] [nvarchar](max) NULL,
	[License] [nvarchar](max) NULL,
	[LicenseImage] [nvarchar](max) NULL,
	[InstallationStatus] [bit] NULL,
	[Description] [nvarchar](max) NULL,
	[VIN] [nvarchar](max) NOT NULL,
	[TypeofUseID] [int] NULL,
	[NationalCode] [nvarchar](max) NOT NULL,
	[Address] [nvarchar](max) NULL,
	[FuelCard] [nvarchar](max) NOT NULL,
	[WorkshopID] [int] NULL,
	[Editor] [int] NULL,
	[EditDate] [datetime] NULL,
	[HealthCertificate] [nvarchar](max) NULL,
	[CreatorIPAddress] [nvarchar](max) NULL,
	[EditorIPAddress] [nvarchar](max) NULL,
	[Status] [bit] NOT NULL,
	[InvoiceCode] [nvarchar](max) NULL,
	[Checked] [bit] NOT NULL,
	[CheckedDate] [datetime] NULL,
	[Checker] [nvarchar](max) NULL,
	[TechnicalDiagnosis] [nvarchar](max) NULL,
	[RegisterStatus] [bit] NOT NULL,
	[RegisterStatusDate] [datetime] NULL,
	[RegisterStatusUser] [nvarchar](max) NULL,
	[RegisterUniqueCode] [nvarchar](max) NULL,
	[FinancialStatus] [bit] NOT NULL,
	[FinancialStatusDate] [datetime] NULL,
	[FinancialStatusUser] [nvarchar](max) NULL,
	[RegistrationTypeID] [int] NULL,
 CONSTRAINT [PK_dbo.tbl_VehicleRegistrations] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_VehicleRegistrations]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_VehicleRegistrations]
AS
SELECT        dbo.tbl_VehicleTypes.Type + ' ' + dbo.tbl_VehicleTypes.Description AS VehicleType, dbo.tbl_VehicleRegistrations.ConstructionYear, dbo.tbl_VehicleRegistrations.EngineNumber, dbo.tbl_VehicleRegistrations.ChassisNumber, 
                         REPLACE(dbo.tbl_VehicleRegistrations.NationalCode, '-', '') AS NationalCode, dbo.tbl_Workshops.Title AS Workshop, 
                         dbo.tbl_VehicleRegistrations.IranNumberPlate + '-' + dbo.tbl_VehicleRegistrations.LeftNumberPlate + '' + dbo.tbl_VehicleRegistrations.AlphaPlate + '' + dbo.tbl_VehicleRegistrations.RightNumberPlate AS Plate, 
                         dbo.tbl_VehicleRegistrations.IranNumberPlate + '' + dbo.tbl_VehicleRegistrations.LeftNumberPlate + '' + dbo.tbl_VehicleRegistrations.RightNumberPlate AS Plate2, dbo.tbl_VehicleRegistrations.License AS HealthCertificate, 
                         dbo.tbl_VehicleRegistrations.ID, dbo.tbl_VehicleRegistrations.RegisterStatus
FROM            dbo.tbl_VehicleRegistrations INNER JOIN
                         dbo.tbl_VehicleTypes ON dbo.tbl_VehicleRegistrations.VehicleTypeID = dbo.tbl_VehicleTypes.ID LEFT OUTER JOIN
                         dbo.tbl_Workshops ON dbo.tbl_VehicleRegistrations.WorkshopID = dbo.tbl_Workshops.ID LEFT OUTER JOIN
                         dbo.tbl_VehicleTanks ON dbo.tbl_VehicleRegistrations.ID = dbo.tbl_VehicleTanks.VehicleRegistrationID
WHERE        (dbo.tbl_VehicleRegistrations.RegisterStatus = 1)
GO
/****** Object:  Table [dbo].[tbl_IRNGV]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_IRNGV](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Acceptance] [nvarchar](max) NULL,
	[Constractor] [nvarchar](max) NULL,
	[VehicleType] [nvarchar](max) NULL,
	[State] [nvarchar](max) NULL,
	[City] [nvarchar](max) NULL,
	[Plate] [nvarchar](max) NULL,
	[EngineNumber] [nvarchar](max) NULL,
	[ChassisNumber] [nvarchar](max) NULL,
	[ConstructionYear] [nvarchar](4) NULL,
	[Workshop] [nvarchar](max) NULL,
	[WorkshopID] [nvarchar](max) NULL,
	[InspectionCertificateNumber] [nvarchar](max) NULL,
	[DateofCertification] [nvarchar](max) NULL,
	[Column1] [nvarchar](max) NULL,
	[Insurance] [nvarchar](max) NULL,
	[InsuranceNumber] [nvarchar](max) NULL,
	[InspectionCompany] [nvarchar](max) NULL,
	[InspectorName] [nvarchar](max) NULL,
	[Cylinder] [nvarchar](max) NULL,
	[Valve] [nvarchar](max) NULL,
	[Regulator] [nvarchar](max) NULL,
	[NationalCode] [nvarchar](max) NULL,
	[Creator] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
	[CutoffValve] [nvarchar](max) NULL,
	[FillingValve] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_IRNGV] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_IRNGV]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_IRNGV]
AS
SELECT        TOP (100) PERCENT VehicleType, REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(ConstructionYear, N'۱', '1'), N'۲', '2'), N'۳', '3'), N'۴', '4'), N'۵', '5'), N'۶', '6'), N'۷', '7'), 
                         N'۸', '8'), N'۹', '9'), N'۰', '0') AS ConstructionYear, EngineNumber, ChassisNumber, REPLACE(NationalCode, '-', '') AS NationalCode, Workshop, Plate AS Plate, 
                         dbo.udf_GetNumeric(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(Plate, ' ', ''), N'۰', '0'), N'۱', '1'), N'۲', '2'), N'۳', '3'), N'۴', '4'), N'۵', '5'), N'۶', '6'), 
                         N'۷', '7'), N'۸', '8'), N'۹', '9'), N' ', '')) AS Plate2, 'F' + REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE([InspectionCertificateNumber ], N'۱', '1'), N'۲', '2'), N'۳', '3'), N'۴', '4'), 
                         N'۵', '5'), N'۶', '6'), N'۷', '7'), N'۸', '8'), N'۹', '9'), N'۰', '0') AS HealthCertificate
FROM            dbo.tbl_IRNGV
GO
/****** Object:  Table [dbo].[tbl_GCR]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_GCR](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IssueTracking] [nvarchar](max) NULL,
	[DateofTurn] [nvarchar](max) NULL,
	[Status] [nvarchar](max) NULL,
	[OwnerFullName] [nvarchar](max) NULL,
	[NationalCode] [nvarchar](max) NULL,
	[OwnerMobile] [nvarchar](max) NULL,
	[Plate] [nvarchar](max) NULL,
	[NajiPlate] [nvarchar](max) NULL,
	[TypeofUsed] [nvarchar](max) NULL,
	[VehicleType] [nvarchar](max) NULL,
	[Model] [nvarchar](max) NULL,
	[Contractor] [nvarchar](max) NULL,
	[Workshop] [nvarchar](max) NULL,
	[State] [nvarchar](max) NULL,
	[City] [nvarchar](max) NULL,
	[WorkshopAddress] [nvarchar](max) NULL,
	[WorkshopPhone] [nvarchar](max) NULL,
	[ConversionDate] [nvarchar](max) NULL,
	[ConversionID] [nvarchar](max) NULL,
	[CylinderBulk] [nvarchar](max) NULL,
	[CylinderSerial] [nvarchar](max) NULL,
	[ValveSerial] [nvarchar](max) NULL,
	[RegulatorSerial] [nvarchar](max) NULL,
	[ConversionCertificateNumber] [nvarchar](max) NULL,
	[HealthCertificateID] [nvarchar](max) NULL,
	[EngineNumber] [nvarchar](max) NULL,
	[panNumber] [nvarchar](max) NULL,
	[ChassisNumber] [nvarchar](max) NULL,
	[PersonalPassenger] [nvarchar](max) NULL,
	[Creator] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
	[ConstructionYear] [nvarchar](4) NULL,
 CONSTRAINT [PK_dbo.tbl_GCR] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_NIOPDC]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* SUBSTRING(dbo.udf_GetNumeric(REPLACE(REPLACE(Plate, N'ايران', ''), '-', '')), 1, 2) + '' + SUBSTRING(REPLACE(REPLACE(Plate, N'ايران', ''), '-', ''), 7, 2) + '' + SUBSTRING(REPLACE(REPLACE(Plate, N'ايران', ''), '-', ''), 3, 3) AS Plate,*/
CREATE VIEW [dbo].[vw_NIOPDC]
AS
SELECT        TOP (100) PERCENT VehicleType + ' ' + Model AS VehicleType, ConstructionYear, EngineNumber, ChassisNumber, REPLACE(NationalCode, '-', '') AS NationalCode, Workshop, Plate, SUBSTRING(Plate, 6, 2) 
                         + '' + SUBSTRING(Plate, 13, 2) + '' + SUBSTRING(Plate, 9, 3) AS Plate2, 'F' + HealthCertificateID AS HealthCertificate, Status
FROM            dbo.tbl_GCR
GO
/****** Object:  View [dbo].[vw_3TableDataDiff]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_3TableDataDiff]
AS
SELECT        dbo.vw_IRNGV.VehicleType AS IRNGV_VehicleType, dbo.vw_IRNGV.ConstructionYear AS IRNGV_ConstructionYear, dbo.vw_IRNGV.EngineNumber AS IRNGV_EngineNumber, 
                         dbo.vw_IRNGV.ChassisNumber AS IRNGV_ChassisNumber, dbo.vw_IRNGV.NationalCode AS IRNGV_NationalCode, dbo.vw_IRNGV.Workshop AS IRNGV_Workshop, dbo.vw_IRNGV.Plate2 AS IRNGV_Plate2, 
                         dbo.vw_IRNGV.Plate AS IRNGV_Plate, dbo.vw_IRNGV.HealthCertificate AS IRNGV_HealthCertificate, dbo.vw_NIOPDC.VehicleType AS NIOPDC_VehicleType, dbo.vw_NIOPDC.ConstructionYear AS NIOPDC_ConstructionYear, 
                         dbo.vw_NIOPDC.EngineNumber AS NIOPDC_EngineNumber, dbo.vw_NIOPDC.ChassisNumber AS NIOPDC_ChassisNumber, dbo.vw_NIOPDC.NationalCode AS NIOPDC_NationalCode, 
                         dbo.vw_NIOPDC.Workshop AS NIOPDC_Workshop, dbo.vw_NIOPDC.Plate2 AS NIOPDC_Plate2, dbo.vw_NIOPDC.Plate AS NIOPDC_Plate, dbo.vw_NIOPDC.HealthCertificate AS NIOPDC_HealthCertificate, 
                         dbo.vw_VehicleRegistrations.VehicleType, dbo.vw_VehicleRegistrations.ConstructionYear, dbo.vw_VehicleRegistrations.ChassisNumber, dbo.vw_VehicleRegistrations.NationalCode, 
                         dbo.vw_VehicleRegistrations.EngineNumber, dbo.vw_VehicleRegistrations.Workshop, dbo.vw_VehicleRegistrations.Plate2, dbo.vw_VehicleRegistrations.Plate, dbo.vw_VehicleRegistrations.HealthCertificate, 
                         dbo.vw_VehicleRegistrations.ID
FROM            dbo.vw_NIOPDC LEFT OUTER JOIN
                         dbo.vw_IRNGV ON dbo.vw_NIOPDC.NationalCode = dbo.vw_IRNGV.NationalCode LEFT OUTER JOIN
                         dbo.vw_VehicleRegistrations ON dbo.vw_NIOPDC.NationalCode = dbo.vw_VehicleRegistrations.NationalCode
GO
/****** Object:  Table [dbo].[tbl_DivisionPlanBOMs]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_DivisionPlanBOMs](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BOMID] [int] NULL,
	[Number] [float] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[NumberofSend] [float] NULL,
	[Creator] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
	[DivisionPlanID] [int] NULL,
	[GenarationID] [int] NULL,
	[RegistrationTypeID] [int] NULL,
 CONSTRAINT [PK_dbo.tbl_DivisionPlanBOMs] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_EquipmentList]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_EquipmentList](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[FinancialCode] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[Pid] [int] NULL,
	[Presentable] [bit] NOT NULL,
	[Creator] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
	[Value] [float] NULL,
	[Value2] [float] NULL,
	[RegistrationTypeID] [int] NULL,
 CONSTRAINT [PK_dbo.tbl_EquipmentList] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_BOMs]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_BOMs](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EquipmentListID] [int] NULL,
	[Ratio] [float] NULL,
	[Unit] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[Presentable] [bit] NOT NULL,
	[Creator] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
	[VehicleTypeID] [int] NULL,
	[GenerationID] [int] NULL,
 CONSTRAINT [PK_dbo.tbl_BOMs] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_DivisionPlans]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_DivisionPlans](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](max) NULL,
	[WorkshopID] [int] NULL,
	[CreateDate] [datetime] NOT NULL,
	[Creator] [int] NULL,
	[Confirmation] [bit] NULL,
	[ConfirmationUser] [int] NULL,
	[Send] [bit] NULL,
	[SendDate] [datetime] NULL,
	[Sender] [int] NULL,
	[Description] [nvarchar](max) NULL,
	[ConfirmationDate] [datetime] NULL,
	[FinalCheck] [bit] NULL,
	[FinalCheckDate] [datetime] NULL,
	[Attachment] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_DivisionPlans] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[Vw_DivisionPlan New query]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Vw_DivisionPlan New query]
AS
SELECT        TOP (2000) dbo.tbl_Workshops.Title, dbo.tbl_DivisionPlans.Send, dbo.tbl_DivisionPlanBOMs.DivisionPlanID, dbo.tbl_DivisionPlanBOMs.NumberofSend, dbo.tbl_VehicleTypes.Type, dbo.tbl_VehicleTypes.Description, 
                         dbo.tbl_EquipmentList.Title AS Expr1, dbo.tbl_EquipmentList.Pid, dbo.tbl_EquipmentList.Address, dbo.tbl_DivisionPlans.Code
FROM            dbo.tbl_DivisionPlans INNER JOIN
                         dbo.tbl_DivisionPlanBOMs ON dbo.tbl_DivisionPlans.ID = dbo.tbl_DivisionPlanBOMs.DivisionPlanID INNER JOIN
                         dbo.tbl_BOMs ON dbo.tbl_DivisionPlanBOMs.BOMID = dbo.tbl_BOMs.ID INNER JOIN
                         dbo.tbl_Workshops ON dbo.tbl_DivisionPlans.WorkshopID = dbo.tbl_Workshops.ID INNER JOIN
                         dbo.tbl_EquipmentList ON dbo.tbl_BOMs.EquipmentListID = dbo.tbl_EquipmentList.ID INNER JOIN
                         dbo.tbl_VehicleTypes ON dbo.tbl_BOMs.VehicleTypeID = dbo.tbl_VehicleTypes.ID
GO
/****** Object:  View [dbo].[vw_diffvalue]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_diffvalue]
AS
SELECT        dbo.vw_NIOPDC.VehicleType AS NIOPDC_VehicleType, dbo.vw_NIOPDC.ConstructionYear AS NIOPDC_ConstructionYear, dbo.vw_NIOPDC.EngineNumber AS NIOPDC_EngineNumber, 
                         dbo.vw_NIOPDC.ChassisNumber AS NIOPDC_ChassisNumber, dbo.vw_NIOPDC.NationalCode AS NIOPDC_NationalCode, dbo.vw_NIOPDC.Workshop AS NIOPDC_Workshop, dbo.vw_NIOPDC.Plate2 AS NIOPDC_Plate2, 
                         dbo.vw_NIOPDC.Plate AS NIOPDC_Plate, dbo.vw_NIOPDC.HealthCertificate AS NIOPDC_HealthCertificate, dbo.vw_VehicleRegistrations.VehicleType, dbo.vw_VehicleRegistrations.ConstructionYear, 
                         dbo.vw_VehicleRegistrations.ChassisNumber, dbo.vw_VehicleRegistrations.NationalCode, dbo.vw_VehicleRegistrations.EngineNumber, dbo.vw_VehicleRegistrations.Workshop, dbo.vw_VehicleRegistrations.Plate2, 
                         dbo.vw_VehicleRegistrations.Plate, dbo.vw_VehicleRegistrations.HealthCertificate, dbo.vw_VehicleRegistrations.ID
FROM            dbo.vw_NIOPDC LEFT OUTER JOIN
                         dbo.vw_VehicleRegistrations ON dbo.vw_NIOPDC.NationalCode = dbo.vw_VehicleRegistrations.NationalCode
GO
/****** Object:  Table [dbo].[tbl_InvoicesFapa]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_InvoicesFapa](
	[InvoiceID] [int] IDENTITY(1,1) NOT NULL,
	[VehicleTypesID] [int] NULL,
	[WorkshopsID] [int] NULL,
	[EquipmentsID] [int] NULL,
	[ServiceCode] [nvarchar](max) NULL,
	[InvoiceCode] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[EmployerEconomicalnumber] [nvarchar](max) NULL,
	[Employerregistrationnumber] [nvarchar](max) NULL,
	[EmployerState] [nvarchar](max) NULL,
	[EmployerAddress] [nvarchar](max) NULL,
	[EmployerPostalcode] [nvarchar](max) NULL,
	[EmployerPhone] [nvarchar](max) NULL,
	[EmployerFax] [nvarchar](max) NULL,
	[ServiceDesc] [nvarchar](max) NOT NULL,
	[Number] [nvarchar](max) NOT NULL,
	[UnitofMeasurement] [nvarchar](max) NULL,
	[UnitAmount] [float] NOT NULL,
	[TotalAmount] [float] NULL,
	[DiscountAmount] [float] NULL,
	[TotalAmountafterDiscount] [float] NULL,
	[Tax] [float] NULL,
	[Complications] [float] NULL,
	[AmountTaxandComplications] [float] NULL,
	[TotalAmountTaxandComplications] [float] NULL,
	[Description] [nvarchar](max) NULL,
	[SaleCondition] [nvarchar](max) NULL,
	[Comment] [nvarchar](max) NULL,
	[AcceptedAmount] [float] NULL,
	[AcceptedDate] [datetime] NULL,
	[Status] [bit] NULL,
	[CurrencyTypeID] [int] NULL,
	[CreatorUser] [int] NULL,
	[CheckStatus] [bit] NULL,
	[CkeckedDate] [datetime] NULL,
	[CkeckedUser] [nvarchar](max) NULL,
	[FinancialStatus] [bit] NULL,
	[FinancialDate] [datetime] NULL,
	[FinancialUser] [nvarchar](max) NULL,
	[ReciveStatus] [bit] NULL,
	[ReciveDate] [datetime] NULL,
	[ReciveUser] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_InvoicesFapa] PRIMARY KEY CLUSTERED 
(
	[InvoiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_Tabdil_MaboTafavot]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*مابه التفاوت دستمزد تبدیل*/
CREATE VIEW [dbo].[vw_Tabdil_MaboTafavot]
AS
SELECT        t2.ID, t2.Title, SUM(CONVERT(int, t1.Number)) AS 'Number', SUM(t1.TotalAmountTaxandComplications) AS 'TotalAmount', '1' AS 'TypeCode', 'Tabdil' AS 'Type'
FROM            dbo.tbl_InvoicesFapa AS t1 INNER JOIN
                         dbo.tbl_Workshops AS t2 ON t1.WorkshopsID = t2.ID
WHERE        (t1.FinancialStatus = 1) AND (t1.Description LIKE N'%مربوط به مابه التفاوت دستمزد تبدیل می باشد.%')
GROUP BY t2.ID, t2.Title
GO
/****** Object:  View [dbo].[vw_DastmozdeTabdil]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*دستمزد تبدیل*/
CREATE VIEW [dbo].[vw_DastmozdeTabdil]
AS
SELECT        t2.ID, t2.Title, SUM(CONVERT(int, t1.Number)) AS 'Number', SUM(t1.TotalAmountTaxandComplications) AS 'TotalAmount', '1' AS 'TypeCode', 'Tabdil' AS 'Type'
FROM            dbo.tbl_InvoicesFapa AS t1 INNER JOIN
                         dbo.tbl_Workshops AS t2 ON t1.WorkshopsID = t2.ID
WHERE        (t1.FinancialStatus = 1)
GROUP BY t2.ID, t2.Title
GO
/****** Object:  Table [dbo].[tbl_InvoicesFapa_DamagesCylinder]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_InvoicesFapa_DamagesCylinder](
	[InvoiceID] [int] IDENTITY(1,1) NOT NULL,
	[VehicleTypesID] [int] NULL,
	[WorkshopsID] [int] NULL,
	[EquipmentsID] [int] NULL,
	[ServiceCode] [nvarchar](max) NULL,
	[InvoiceCode] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[EmployerEconomicalnumber] [nvarchar](max) NULL,
	[Employerregistrationnumber] [nvarchar](max) NULL,
	[EmployerState] [nvarchar](max) NULL,
	[EmployerAddress] [nvarchar](max) NULL,
	[EmployerPostalcode] [nvarchar](max) NULL,
	[EmployerPhone] [nvarchar](max) NULL,
	[EmployerFax] [nvarchar](max) NULL,
	[ServiceDesc] [nvarchar](max) NOT NULL,
	[Number] [nvarchar](max) NOT NULL,
	[UnitofMeasurement] [nvarchar](max) NULL,
	[UnitAmount] [float] NOT NULL,
	[TotalAmount] [float] NULL,
	[DiscountAmount] [float] NULL,
	[TotalAmountafterDiscount] [float] NULL,
	[Tax] [float] NULL,
	[Complications] [float] NULL,
	[AmountTaxandComplications] [float] NULL,
	[TotalAmountTaxandComplications] [float] NULL,
	[Description] [nvarchar](max) NULL,
	[SaleCondition] [nvarchar](max) NULL,
	[Comment] [nvarchar](max) NULL,
	[AcceptedAmount] [float] NULL,
	[AcceptedDate] [datetime] NULL,
	[Status] [bit] NULL,
	[CurrencyTypeID] [int] NULL,
	[CreatorUser] [int] NULL,
	[CheckStatus] [bit] NULL,
	[CkeckedDate] [datetime] NULL,
	[CkeckedUser] [nvarchar](max) NULL,
	[FinancialStatus] [bit] NULL,
	[FinancialDate] [datetime] NULL,
	[FinancialUser] [nvarchar](max) NULL,
	[ReciveStatus] [bit] NULL,
	[ReciveDate] [datetime] NULL,
	[ReciveUser] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_InvoicesFapa_DamagesCylinder] PRIMARY KEY CLUSTERED 
(
	[InvoiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_DastmozdeTaaviz]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*دستمزد تعویض مخزن*/
CREATE VIEW [dbo].[vw_DastmozdeTaaviz]
AS
SELECT        t2.ID, t2.Title, SUM(CONVERT(int, t1.Number)) AS 'Number', SUM(t1.TotalAmountTaxandComplications) AS 'TotalAmount', '2' AS 'TypeCode', 'Taaviz' AS 'Type'
FROM            dbo.tbl_InvoicesFapa_DamagesCylinder AS t1 INNER JOIN
                         dbo.tbl_Workshops AS t2 ON t1.WorkshopsID = t2.ID
WHERE        (t1.FinancialStatus = 1)
GROUP BY t2.ID, t2.Title
GO
/****** Object:  Table [dbo].[tbl_InvoicesDamages]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_InvoicesDamages](
	[InvoiceID] [int] IDENTITY(1,1) NOT NULL,
	[VehicleTypesID] [int] NULL,
	[WorkshopsID] [int] NULL,
	[EquipmentsID] [int] NULL,
	[ServiceCode] [nvarchar](max) NULL,
	[InvoiceCode] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[EmployerEconomicalnumber] [nvarchar](max) NULL,
	[Employerregistrationnumber] [nvarchar](max) NULL,
	[EmployerState] [nvarchar](max) NULL,
	[EmployerAddress] [nvarchar](max) NULL,
	[EmployerPostalcode] [nvarchar](max) NULL,
	[EmployerPhone] [nvarchar](max) NULL,
	[EmployerFax] [nvarchar](max) NULL,
	[ServiceDesc] [nvarchar](max) NOT NULL,
	[Number] [nvarchar](max) NOT NULL,
	[UnitofMeasurement] [nvarchar](max) NULL,
	[UnitAmount] [float] NOT NULL,
	[TotalAmount] [float] NULL,
	[DiscountAmount] [float] NULL,
	[TotalAmountafterDiscount] [float] NULL,
	[Tax] [float] NULL,
	[Complications] [float] NULL,
	[AmountTaxandComplications] [float] NULL,
	[TotalAmountTaxandComplications] [float] NULL,
	[Description] [nvarchar](max) NULL,
	[SaleCondition] [nvarchar](max) NULL,
	[Comment] [nvarchar](max) NULL,
	[AcceptedAmount] [float] NULL,
	[AcceptedDate] [datetime] NULL,
	[Status] [bit] NULL,
	[CurrencyTypeID] [int] NULL,
	[CreatorUser] [int] NULL,
	[CheckStatus] [bit] NULL,
	[CkeckedDate] [datetime] NULL,
	[CkeckedUser] [nvarchar](max) NULL,
	[FinancialStatus] [bit] NULL,
	[FinancialDate] [datetime] NULL,
	[FinancialUser] [nvarchar](max) NULL,
	[ReciveStatus] [bit] NULL,
	[ReciveDate] [datetime] NULL,
	[ReciveUser] [nvarchar](max) NULL,
	[Year] [nvarchar](max) NULL,
	[Month] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_InvoicesDamages] PRIMARY KEY CLUSTERED 
(
	[InvoiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_ForusheMakhzan]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*فروش  مخزن ضایعاتی*/
CREATE VIEW [dbo].[vw_ForusheMakhzan]
AS
SELECT        t2.ID, t2.Title, SUM(CONVERT(int, t1.Number)) AS 'Number', SUM(t1.TotalAmountTaxandComplications) AS 'TotalAmount', '3' AS 'TypeCode', 'FroushMakhzan' AS 'Type'
FROM            dbo.tbl_InvoicesDamages AS t1 INNER JOIN
                         dbo.tbl_Workshops AS t2 ON t1.WorkshopsID = t2.ID
GROUP BY t2.ID, t2.Title
GO
/****** Object:  Table [dbo].[tbl_InvoicesValveDamages]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_InvoicesValveDamages](
	[InvoiceID] [int] IDENTITY(1,1) NOT NULL,
	[VehicleTypesID] [int] NULL,
	[WorkshopsID] [int] NULL,
	[EquipmentsID] [int] NULL,
	[ServiceCode] [nvarchar](max) NULL,
	[InvoiceCode] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[EmployerEconomicalnumber] [nvarchar](max) NULL,
	[Employerregistrationnumber] [nvarchar](max) NULL,
	[EmployerState] [nvarchar](max) NULL,
	[EmployerAddress] [nvarchar](max) NULL,
	[EmployerPostalcode] [nvarchar](max) NULL,
	[EmployerPhone] [nvarchar](max) NULL,
	[EmployerFax] [nvarchar](max) NULL,
	[ServiceDesc] [nvarchar](max) NOT NULL,
	[Number] [nvarchar](max) NOT NULL,
	[UnitofMeasurement] [nvarchar](max) NULL,
	[UnitAmount] [float] NOT NULL,
	[TotalAmount] [float] NULL,
	[DiscountAmount] [float] NULL,
	[TotalAmountafterDiscount] [float] NULL,
	[Tax] [float] NULL,
	[Complications] [float] NULL,
	[AmountTaxandComplications] [float] NULL,
	[TotalAmountTaxandComplications] [float] NULL,
	[Description] [nvarchar](max) NULL,
	[SaleCondition] [nvarchar](max) NULL,
	[Comment] [nvarchar](max) NULL,
	[AcceptedAmount] [float] NULL,
	[AcceptedDate] [datetime] NULL,
	[Status] [bit] NULL,
	[CurrencyTypeID] [int] NULL,
	[CreatorUser] [int] NULL,
	[CheckStatus] [bit] NULL,
	[CkeckedDate] [datetime] NULL,
	[CkeckedUser] [nvarchar](max) NULL,
	[FinancialStatus] [bit] NULL,
	[FinancialDate] [datetime] NULL,
	[FinancialUser] [nvarchar](max) NULL,
	[ReciveStatus] [bit] NULL,
	[ReciveDate] [datetime] NULL,
	[ReciveUser] [nvarchar](max) NULL,
	[Year] [nvarchar](max) NULL,
	[Month] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_InvoicesValveDamages] PRIMARY KEY CLUSTERED 
(
	[InvoiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_ForusheShirMakhzan]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_ForusheShirMakhzan]
AS
SELECT        t2.ID, t2.Title, SUM(CONVERT(int, t1.Number)) AS 'Number', SUM(t1.TotalAmountTaxandComplications) AS 'TotalAmount', '4' AS 'TypeCode', 'FroushShirMakhzan' AS 'Type'
FROM            dbo.tbl_InvoicesValveDamages AS t1 INNER JOIN
                         dbo.tbl_Workshops AS t2 ON t1.WorkshopsID = t2.ID
GROUP BY t2.ID, t2.Title
GO
/****** Object:  Table [dbo].[tbl_Remittances]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Remittances](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Number] [nvarchar](max) NOT NULL,
	[DivisionPlanID] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Creator] [int] NULL,
	[Status] [bit] NULL,
	[StatusDate] [datetime] NULL,
	[Description] [nvarchar](max) NULL,
	[Incomplete] [bit] NULL,
	[IncompleteDesc] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_Remittances] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_RemittanceDetails]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_RemittanceDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RemittancesID] [int] NULL,
	[Date] [datetime] NULL,
	[Vehicle] [nvarchar](max) NOT NULL,
	[Plate] [nvarchar](max) NULL,
	[BillofLading] [nvarchar](max) NULL,
	[Transferee] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
	[Creator] [nvarchar](max) NULL,
	[CarryFare] [float] NULL,
	[CarrierName] [nvarchar](max) NULL,
	[Attachment] [nvarchar](max) NULL,
	[CheckStatus] [bit] NULL,
	[CkeckedDate] [datetime] NULL,
	[CkeckedUser] [nvarchar](max) NULL,
	[FinancialDate] [datetime] NULL,
	[FinancialUser] [nvarchar](max) NULL,
	[ManagerDate] [datetime] NULL,
	[ManagerUser] [nvarchar](max) NULL,
	[FinancialCheckStatus] [bit] NULL,
	[ManagerCheckStatus] [bit] NULL,
 CONSTRAINT [PK_dbo.tbl_RemittanceDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_Barname]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Barname]
AS
SELECT        t4.ID, t4.Title, SUM(t1.CarryFare) AS 'RemittanceCost', '5' AS 'TypeCode', 'Baarnameh' AS 'Type'
FROM            dbo.tbl_RemittanceDetails AS t1 INNER JOIN
                         dbo.tbl_Remittances AS t2 ON t1.RemittancesID = t2.ID INNER JOIN
                         dbo.tbl_DivisionPlans AS t3 ON t2.DivisionPlanID = t3.ID INNER JOIN
                         dbo.tbl_Workshops AS t4 ON t3.WorkshopID = t4.ID
WHERE        (t1.FinancialDate IS NOT NULL) OR
                         (t1.FinancialUser IS NOT NULL)
GROUP BY t4.ID, t4.Title
GO
/****** Object:  View [dbo].[vw_Kosurat]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Kosurat]
AS
SELECT        ID, Title, ISNULL(dbo.GetFinancialCreditor(1, ID, '2019-01-01', '2025-04-20'), 0) * 0.2667 AS 'Deductions', '6' AS 'TypeCode', 'Kosurat' AS 'Type'
FROM            dbo.tbl_Workshops
GO
/****** Object:  Table [dbo].[tbl_FinancialDescs]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_FinancialDescs](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_FinancialDescs] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_FinancialPayments]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_FinancialPayments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FinancialDescID] [int] NULL,
	[WorkshopID] [int] NULL,
	[Date] [datetime] NOT NULL,
	[Value] [float] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
	[Creator] [nvarchar](max) NULL,
	[Status] [bit] NULL,
	[Comment] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_FinancialPayments] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_Pardakhtiha]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Pardakhtiha]
AS
SELECT        t3.ID, t3.Title, t2.Title AS 'DescTitle', t2.ID AS 'DescID', SUM(t1.Value) AS 'Payment', '7' AS 'TypeCode', 'Pardakhtiha' AS 'Type'
FROM            dbo.tbl_FinancialPayments AS t1 INNER JOIN
                         dbo.tbl_FinancialDescs AS t2 ON t1.FinancialDescID = t2.ID INNER JOIN
                         dbo.tbl_Workshops AS t3 ON t1.WorkshopID = t3.ID
GROUP BY t3.ID, t3.Title, t2.Title, t2.ID
GO
/****** Object:  Table [dbo].[tbl_Cities]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Cities](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NULL,
	[StateID] [int] NULL,
 CONSTRAINT [PK_dbo.tbl_Cities] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_States]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_States](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_States] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_Workshops]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Workshops]
AS
SELECT        dbo.tbl_Workshops.ID, dbo.tbl_Workshops.Title, dbo.tbl_States.Title AS State, dbo.tbl_Cities.Title AS City, dbo.tbl_Workshops.OwnerName, dbo.tbl_Workshops.OwnerFamily, dbo.tbl_Workshops.FapCode, 
                         dbo.tbl_Workshops.isServices, dbo.tbl_Workshops.closedServices, dbo.tbl_Workshops.IRNGVCod
FROM            dbo.tbl_Workshops INNER JOIN
                         dbo.tbl_Cities ON dbo.tbl_Workshops.CityID = dbo.tbl_Cities.ID INNER JOIN
                         dbo.tbl_States ON dbo.tbl_Cities.StateID = dbo.tbl_States.ID
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IRNGV__Update]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IRNGV__Update](
	[Acceptance] [nvarchar](255) NULL,
	[NationalCode] [nvarchar](255) NULL,
	[Constractor] [nvarchar](255) NULL,
	[VehicleType] [nvarchar](255) NULL,
	[State] [nvarchar](255) NULL,
	[City] [nvarchar](255) NULL,
	[Plate] [nvarchar](255) NULL,
	[ChassisNumber] [nvarchar](255) NULL,
	[ConstructionYear] [nvarchar](255) NULL,
	[Workshop] [nvarchar](255) NULL,
	[WorkshopID] [nvarchar](255) NULL,
	[InspectionCertificateNumber] [nvarchar](255) NULL,
	[DateofCertification] [nvarchar](255) NULL,
	[Column1] [nvarchar](255) NULL,
	[Insurance] [nvarchar](255) NULL,
	[InsuranceNumber] [nvarchar](255) NULL,
	[InspectionCompany] [nvarchar](255) NULL,
	[InspectorName] [nvarchar](255) NULL,
	[Cylinder] [nvarchar](255) NULL,
	[Valve] [nvarchar](255) NULL,
	[Regulator] [nvarchar](255) NULL,
	[CutoffValve] [nvarchar](255) NULL,
	[FillingValve] [nvarchar](255) NULL,
	[Creator] [nvarchar](max) NULL,
	[CreateDate] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LNK_Role_Permission]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LNK_Role_Permission](
	[Permission_Id] [int] NOT NULL,
	[Role_Id] [int] NOT NULL,
 CONSTRAINT [PK_dbo.LNK_Role_Permission] PRIMARY KEY CLUSTERED 
(
	[Permission_Id] ASC,
	[Role_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LNK_User_Organization]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LNK_User_Organization](
	[Organization_Id] [int] NOT NULL,
	[User_Id] [int] NOT NULL,
 CONSTRAINT [PK_dbo.LNK_User_Organization] PRIMARY KEY CLUSTERED 
(
	[Organization_Id] ASC,
	[User_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LNK_User_Role]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LNK_User_Role](
	[Role_Id] [int] NOT NULL,
	[User_Id] [int] NOT NULL,
 CONSTRAINT [PK_dbo.LNK_User_Role] PRIMARY KEY CLUSTERED 
(
	[Role_Id] ASC,
	[User_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LNK_USER_SideBarItem]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LNK_USER_SideBarItem](
	[SideBarItems_Id] [int] NOT NULL,
	[User_Id] [int] NOT NULL,
 CONSTRAINT [PK_dbo.LNK_USER_SideBarItem] PRIMARY KEY CLUSTERED 
(
	[SideBarItems_Id] ASC,
	[User_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LNK_User_Workshop]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LNK_User_Workshop](
	[Workshop_Id] [int] NOT NULL,
	[User_Id] [int] NOT NULL,
 CONSTRAINT [PK_dbo.LNK_User_Workshop] PRIMARY KEY CLUSTERED 
(
	[Workshop_Id] ASC,
	[User_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[myTable]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[myTable](
	[Count] [int] NULL,
	[Literage_Texe] [nvarchar](max) NULL,
	[Weight] [float] NULL,
	[Code] [int] NULL,
	[WorkshopID] [int] NULL,
	[Title] [nvarchar](max) NULL,
	[Year] [int] NULL,
	[Month] [int] NULL,
	[Price] [float] NULL,
	[TotalAmount] [float] NULL,
	[TotalAmountAfterDiscount] [float] NULL,
	[Tax] [float] NULL,
	[TotalAmountWithTax] [float] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NIOPDC_for_Update]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NIOPDC_for_Update](
	[ردیف] [float] NULL,
	[شماره پیگیری] [nvarchar](255) NULL,
	[تاریخ نوبت] [nvarchar](255) NULL,
	[وضعیت] [nvarchar](255) NULL,
	[نام و نام خانوادگی مالک] [nvarchar](255) NULL,
	[کد ملی مالک] [nvarchar](255) NULL,
	[شماره مالک] [nvarchar](255) NULL,
	[پلاک] [nvarchar](255) NULL,
	[پلاک ناجی] [nvarchar](255) NULL,
	[نوع کاربری] [nvarchar](255) NULL,
	[برند] [nvarchar](255) NULL,
	[سال ساخت] [nvarchar](255) NULL,
	[مدل] [nvarchar](255) NULL,
	[نام پیمانکار] [nvarchar](255) NULL,
	[نام کارگاه] [nvarchar](255) NULL,
	[استان کارگاه] [nvarchar](255) NULL,
	[شهر کارگاه] [nvarchar](255) NULL,
	[آدرس کارگاه] [nvarchar](255) NULL,
	[شماره تماس کارگاه] [nvarchar](255) NULL,
	[تاریخ تبدیل] [nvarchar](255) NULL,
	[شناسه تبدیل] [nvarchar](255) NULL,
	[حجم مخزن] [nvarchar](255) NULL,
	[سریال مخزن] [nvarchar](255) NULL,
	[سریال شیر مخزن] [nvarchar](255) NULL,
	[سریال رگولاتور] [nvarchar](255) NULL,
	[سریال ECU] [nvarchar](255) NULL,
	[شماره گواهی تبدیل] [nvarchar](255) NULL,
	[شناسه گواهی سلامت] [nvarchar](255) NULL,
	[شماره موتور] [nvarchar](255) NULL,
	[شماره pan] [nvarchar](255) NULL,
	[شماره شاسی] [nvarchar](255) NULL,
	[مسافربر شخصی] [nvarchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Permissions]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permissions](
	[Permission_Id] [int] IDENTITY(1,1) NOT NULL,
	[PermissionDescription] [nvarchar](500) NOT NULL,
	[PersianDescription] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_dbo.Permissions] PRIMARY KEY CLUSTERED 
(
	[Permission_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Permissions_old2]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permissions_old2](
	[Permission_Id] [int] IDENTITY(1,1) NOT NULL,
	[PermissionDescription] [nvarchar](500) NOT NULL,
	[PersianDescription] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_dbo.Permissions_old2] PRIMARY KEY CLUSTERED 
(
	[Permission_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Cost] [float] NULL,
	[EmployerID] [int] NULL,
	[CategoriesID] [int] NULL,
	[CitiesID] [int] NULL,
	[StartDate] [datetime] NULL,
	[FinishDate] [datetime] NULL,
	[Description] [nvarchar](max) NULL,
	[CreateDate] [datetime] NULL,
	[Creator] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Products] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[resultstable_IRNGVDamages]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[resultstable_IRNGVDamages](
	[uniqID] [int] NULL,
	[Literage] [varchar](4) NULL,
	[Literage_2] [varchar](4) NULL,
	[Weight] [float] NULL,
	[Weight_2] [float] NULL,
	[Literage_Text] [varchar](max) NULL,
	[WorkshopID] [int] NULL,
	[Year] [int] NULL,
	[Month] [int] NULL,
	[Day] [int] NULL,
	[Plate] [nvarchar](max) NULL,
	[ChassisNumber] [nvarchar](max) NULL,
	[VehicleType] [nvarchar](max) NULL,
	[Acceptance] [nvarchar](max) NULL,
	[UnitAmount] [float] NULL,
	[TotalAmount] [float] NULL,
	[DiscountAmount] [float] NULL,
	[Amount] [float] NULL,
	[persianDate] [nvarchar](max) NULL,
	[miladiDate] [date] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Role_Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](max) NOT NULL,
	[RoleDescription] [nvarchar](max) NULL,
	[IsSysAdmin] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.Roles] PRIMARY KEY CLUSTERED 
(
	[Role_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sheet1$]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sheet1$](
	[ID] [int] NOT NULL,
	[serialNumber] [float] NULL,
	[constractor] [nvarchar](255) NULL,
	[model] [nvarchar](255) NULL,
	[type] [nvarchar](255) NULL,
	[rezve] [nvarchar](255) NULL,
	[productDate] [nvarchar](255) NULL,
	[workshop] [nvarchar](255) NULL,
	[status] [nvarchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_AnswerQuestions]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_AnswerQuestions](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[QuestionId] [int] NULL,
	[Description] [nvarchar](max) NULL,
	[OkNotOk] [bit] NOT NULL,
	[FullName] [nvarchar](max) NULL,
	[Mobile] [nvarchar](max) NULL,
	[ip] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
	[RegistrationsId] [int] NULL,
 CONSTRAINT [PK_dbo.tbl_AnswerQuestions] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_AuditCategories]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_AuditCategories](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_AuditCategories] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_AuditComponies]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_AuditComponies](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[User_UserID] [int] NULL,
 CONSTRAINT [PK_dbo.tbl_AuditComponies] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_AuditFiles]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_AuditFiles](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CheckList] [nvarchar](max) NULL,
	[Picture] [nvarchar](max) NULL,
	[CategoryID] [int] NULL,
	[AuditID] [int] NULL,
 CONSTRAINT [PK_dbo.tbl_AuditFiles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Auditors]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Auditors](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](max) NULL,
	[Picture] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
	[Creator] [nvarchar](max) NULL,
	[CompaniesID] [int] NULL,
 CONSTRAINT [PK_dbo.tbl_Auditors] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Audits]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Audits](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AuditDate] [datetime] NOT NULL,
	[WorkshopID] [int] NULL,
	[Description] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
	[Creator] [nvarchar](max) NULL,
	[UserID] [int] NULL,
 CONSTRAINT [PK_dbo.tbl_Audits] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_AuditsPrice]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_AuditsPrice](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AuditCompanyID] [int] NULL,
	[Price] [float] NOT NULL,
	[FromDate] [datetime] NOT NULL,
	[ToDate] [datetime] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[OldPrice] [float] NULL,
	[Type] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_AuditsPrice] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_BankCutofValves]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_BankCutofValves](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[serialNumber] [nvarchar](max) NULL,
	[constractor] [nvarchar](max) NULL,
	[model] [nvarchar](max) NULL,
	[productDate] [nvarchar](max) NULL,
	[workshop] [nvarchar](max) NULL,
	[status] [nvarchar](max) NULL,
	[CreateDate] [datetime] NULL,
	[Creator] [nvarchar](max) NULL,
	[MaterailName] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_BankCutofValves] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_BankFillingValves]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_BankFillingValves](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[serialNumber] [nvarchar](max) NULL,
	[constractor] [nvarchar](max) NULL,
	[model] [nvarchar](max) NULL,
	[productDate] [nvarchar](max) NULL,
	[workshop] [nvarchar](max) NULL,
	[status] [nvarchar](max) NULL,
	[CreateDate] [datetime] NULL,
	[Creator] [nvarchar](max) NULL,
	[MaterailName] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_BankFillingValves] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_BankFuelRelays]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_BankFuelRelays](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MaterailName] [nvarchar](max) NULL,
	[serialNumber] [nvarchar](max) NULL,
	[constractor] [nvarchar](max) NULL,
	[generation] [nvarchar](max) NULL,
	[model] [nvarchar](max) NULL,
	[type] [nvarchar](max) NULL,
	[productDate] [nvarchar](max) NULL,
	[expireDate] [nvarchar](max) NULL,
	[workshop] [nvarchar](max) NULL,
	[status] [nvarchar](max) NULL,
	[CreateDate] [datetime] NULL,
	[Creator] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_BankFuelRelays] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_BankGasECU]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_BankGasECU](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MaterailName] [nvarchar](max) NULL,
	[serialNumber] [nvarchar](max) NULL,
	[constractor] [nvarchar](max) NULL,
	[generation] [nvarchar](max) NULL,
	[model] [nvarchar](max) NULL,
	[type] [nvarchar](max) NULL,
	[productDate] [nvarchar](max) NULL,
	[expireDate] [nvarchar](max) NULL,
	[workshop] [nvarchar](max) NULL,
	[status] [nvarchar](max) NULL,
	[CreateDate] [datetime] NULL,
	[Creator] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_BankGasECU] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_BankKits]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_BankKits](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[serialNumber] [nvarchar](max) NULL,
	[constractor] [nvarchar](max) NULL,
	[generation] [nvarchar](max) NULL,
	[model] [nvarchar](max) NULL,
	[type] [nvarchar](max) NULL,
	[productDate] [nvarchar](max) NULL,
	[workshop] [nvarchar](max) NULL,
	[status] [nvarchar](max) NULL,
	[CreateDate] [datetime] NULL,
	[Creator] [nvarchar](max) NULL,
	[MaterailName] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_BankKits] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_BankTanks]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_BankTanks](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[serialNumber] [nvarchar](max) NULL,
	[bulk] [nvarchar](max) NULL,
	[model] [nvarchar](max) NULL,
	[type] [nvarchar](max) NULL,
	[length] [nvarchar](max) NULL,
	[pressure] [nvarchar](max) NULL,
	[diameter] [nvarchar](max) NULL,
	[rezve] [nvarchar](max) NULL,
	[productDate] [nvarchar](max) NULL,
	[expireDate] [nvarchar](max) NULL,
	[gregorianPMonth] [nvarchar](max) NULL,
	[gregorianPYear] [nvarchar](max) NULL,
	[gregorianEMonth] [nvarchar](max) NULL,
	[gregorianEYear] [nvarchar](max) NULL,
	[constractor] [nvarchar](max) NULL,
	[workshop] [nvarchar](max) NULL,
	[status] [nvarchar](max) NULL,
	[CreateDate] [datetime] NULL,
	[Creator] [nvarchar](max) NULL,
	[MaterailName] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_BankTanks] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_BankTankValves]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_BankTankValves](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[serialNumber] [nvarchar](max) NULL,
	[constractor] [nvarchar](max) NULL,
	[model] [nvarchar](max) NULL,
	[type] [nvarchar](max) NULL,
	[rezve] [nvarchar](max) NULL,
	[productDate] [nvarchar](max) NULL,
	[workshop] [nvarchar](max) NULL,
	[status] [nvarchar](max) NULL,
	[CreateDate] [datetime] NULL,
	[Creator] [nvarchar](max) NULL,
	[MaterailName] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_BankTankValves] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Categories]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Categories](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Cat] [nvarchar](max) NOT NULL,
	[SubCategory] [int] NULL,
	[Description] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
	[Creator] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_Categories] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_CNGHandBooks]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_CNGHandBooks](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Image] [nvarchar](max) NULL,
	[Icon] [nvarchar](max) NULL,
	[Title] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Order] [int] NULL,
	[Presentable] [bit] NOT NULL,
	[Url] [nvarchar](max) NULL,
	[Page] [nvarchar](max) NULL,
	[Creator] [nvarchar](max) NULL,
	[CreatDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.tbl_CNGHandBooks] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_ContactUs]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_ContactUs](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Image] [nvarchar](max) NULL,
	[Icon] [nvarchar](max) NULL,
	[Title] [nvarchar](max) NULL,
	[Order] [int] NULL,
	[Presentable] [bit] NOT NULL,
	[Url] [nvarchar](max) NULL,
	[Section] [nvarchar](max) NULL,
	[Creator] [nvarchar](max) NULL,
	[CreatDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.tbl_ContactUs] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Contradictions]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Contradictions](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[WorkshopId] [int] NULL,
	[Description] [nvarchar](max) NULL,
	[VehicleType1] [int] NULL,
	[VehicleType2] [int] NULL,
	[VehicleType3] [int] NULL,
	[VehicleType4] [int] NULL,
	[Date] [datetime] NOT NULL,
	[VehicleType5] [int] NULL,
	[VehicleTypeOther] [int] NULL,
	[CreateDate] [datetime] NULL,
	[Creator] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_Contradictions] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_ContradictionTotals]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_ContradictionTotals](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[WorkshopId] [int] NULL,
	[Date] [datetime] NOT NULL,
	[CreateDate] [datetime] NULL,
	[Creator] [nvarchar](max) NULL,
	[ContradictionTypeId] [int] NULL,
	[Count] [float] NOT NULL,
 CONSTRAINT [PK_dbo.tbl_ContradictionTotals] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_ContradictionTypes]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_ContradictionTypes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_tbl_ContradictionTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_CRMDynamicForms]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_CRMDynamicForms](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NULL,
	[OkNotOk] [bit] NOT NULL,
	[Description] [bit] NOT NULL,
	[isShow] [bit] NOT NULL,
	[Creator] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.tbl_CRMDynamicForms] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_CRMIndexes]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_CRMIndexes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[PId] [int] NULL,
	[Presentable] [bit] NOT NULL,
	[Creator] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.tbl_CRMIndexes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_CRMs]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_CRMs](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IndexId] [int] NULL,
	[OwnersId] [int] NULL,
	[Point] [int] NOT NULL,
	[Description1] [nvarchar](max) NULL,
	[Description2] [nvarchar](max) NULL,
	[Suggestion] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.tbl_CRMs] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_CurrencyTypes]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_CurrencyTypes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_CurrencyTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_CustomerFinallSaleInvoices]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_CustomerFinallSaleInvoices](
	[InvoiceID] [int] IDENTITY(1,1) NOT NULL,
	[CustomersID] [int] NULL,
	[EquipmentsID] [int] NULL,
	[ServiceCode] [nvarchar](max) NULL,
	[InvoiceCode] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[Economicalnumber] [nvarchar](max) NULL,
	[Registrationnumber] [nvarchar](max) NULL,
	[State] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[Postalcode] [nvarchar](max) NULL,
	[Phone] [nvarchar](max) NULL,
	[Fax] [nvarchar](max) NULL,
	[ServiceDesc] [nvarchar](max) NOT NULL,
	[Number] [nvarchar](max) NOT NULL,
	[UnitofMeasurement] [nvarchar](max) NULL,
	[UnitAmount] [float] NOT NULL,
	[TotalAmount] [float] NULL,
	[DiscountAmount] [float] NULL,
	[TotalAmountafterDiscount] [float] NULL,
	[Tax] [float] NULL,
	[Complications] [float] NULL,
	[AmountTaxandComplications] [float] NULL,
	[TotalAmountTaxandComplications] [float] NULL,
	[Description] [nvarchar](max) NULL,
	[SaleCondition] [nvarchar](max) NULL,
	[Comment] [nvarchar](max) NULL,
	[AcceptedAmount] [float] NULL,
	[AcceptedDate] [datetime] NULL,
	[Status] [bit] NOT NULL,
	[CurrencyTypeID] [int] NULL,
	[CreatorUser] [int] NULL,
	[ViewStatus] [bit] NOT NULL,
	[ViewDate] [datetime] NULL,
	[Viewer] [nvarchar](max) NULL,
	[PreInvoiceCode] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_CustomerFinallSaleInvoices] PRIMARY KEY CLUSTERED 
(
	[InvoiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_CustomerPreSaleInvoices]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_CustomerPreSaleInvoices](
	[InvoiceID] [int] IDENTITY(1,1) NOT NULL,
	[CustomersID] [int] NULL,
	[EquipmentsID] [int] NULL,
	[ServiceCode] [nvarchar](max) NULL,
	[InvoiceCode] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[Economicalnumber] [nvarchar](max) NULL,
	[Registrationnumber] [nvarchar](max) NULL,
	[State] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[Postalcode] [nvarchar](max) NULL,
	[Phone] [nvarchar](max) NULL,
	[Fax] [nvarchar](max) NULL,
	[ServiceDesc] [nvarchar](max) NOT NULL,
	[Number] [nvarchar](max) NOT NULL,
	[UnitofMeasurement] [nvarchar](max) NULL,
	[UnitAmount] [float] NOT NULL,
	[TotalAmount] [float] NULL,
	[DiscountAmount] [float] NULL,
	[TotalAmountafterDiscount] [float] NULL,
	[Tax] [float] NULL,
	[Complications] [float] NULL,
	[AmountTaxandComplications] [float] NULL,
	[TotalAmountTaxandComplications] [float] NULL,
	[Description] [nvarchar](max) NULL,
	[SaleCondition] [nvarchar](max) NULL,
	[Comment] [nvarchar](max) NULL,
	[AcceptedAmount] [float] NULL,
	[AcceptedDate] [datetime] NULL,
	[Status] [bit] NOT NULL,
	[CurrencyTypeID] [int] NULL,
	[CreatorUser] [int] NULL,
	[ViewStatus] [bit] NOT NULL,
	[ViewDate] [datetime] NULL,
	[Viewer] [nvarchar](max) NULL,
	[RequestCode] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_CustomerPreSaleInvoices] PRIMARY KEY CLUSTERED 
(
	[InvoiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_CustomerRequests]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_CustomerRequests](
	[InvoiceID] [int] IDENTITY(1,1) NOT NULL,
	[CustomersID] [int] NULL,
	[EquipmentsID] [int] NULL,
	[ServiceCode] [nvarchar](max) NULL,
	[InvoiceCode] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[Economicalnumber] [nvarchar](max) NULL,
	[Registrationnumber] [nvarchar](max) NULL,
	[State] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[Postalcode] [nvarchar](max) NULL,
	[Phone] [nvarchar](max) NULL,
	[Fax] [nvarchar](max) NULL,
	[ServiceDesc] [nvarchar](max) NOT NULL,
	[Number] [nvarchar](max) NOT NULL,
	[UnitofMeasurement] [nvarchar](max) NULL,
	[UnitAmount] [float] NOT NULL,
	[TotalAmount] [float] NULL,
	[DiscountAmount] [float] NULL,
	[TotalAmountafterDiscount] [float] NULL,
	[Tax] [float] NULL,
	[Complications] [float] NULL,
	[AmountTaxandComplications] [float] NULL,
	[TotalAmountTaxandComplications] [float] NULL,
	[Description] [nvarchar](max) NULL,
	[SaleCondition] [nvarchar](max) NULL,
	[Comment] [nvarchar](max) NULL,
	[AcceptedAmount] [float] NULL,
	[AcceptedDate] [datetime] NULL,
	[Status] [bit] NOT NULL,
	[CurrencyTypeID] [int] NULL,
	[CreatorUser] [int] NULL,
	[ViewStatus] [bit] NOT NULL,
	[ViewDate] [datetime] NULL,
	[Viewer] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_CustomerRequests] PRIMARY KEY CLUSTERED 
(
	[InvoiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Customers]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Customers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](max) NULL,
	[LasstName] [nvarchar](max) NULL,
	[NationalCode] [nvarchar](max) NULL,
	[BirthDate] [datetime] NULL,
	[Economicalnumber] [nvarchar](max) NULL,
	[PostalCode] [nvarchar](max) NULL,
	[CitiesId] [int] NULL,
	[Address] [nvarchar](max) NULL,
	[Mobile] [nvarchar](max) NULL,
	[Phone] [nvarchar](max) NULL,
	[Fax] [nvarchar](max) NULL,
	[CreditLimit] [float] NOT NULL,
	[Status] [bit] NOT NULL,
	[Creator] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.tbl_Customers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_CutofValveConstractors]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_CutofValveConstractors](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CutofValve] [nvarchar](max) NOT NULL,
	[Code] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_CutofValveConstractors] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_CutofValves]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_CutofValves](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[serialNumber] [nvarchar](max) NULL,
	[constractor] [nvarchar](max) NULL,
	[model] [nvarchar](max) NULL,
	[productDate] [nvarchar](max) NULL,
	[workshop] [nvarchar](max) NULL,
	[status] [nvarchar](max) NULL,
	[CreateDate] [datetime] NULL,
	[Creator] [nvarchar](max) NULL,
	[RefreshDate] [datetime] NULL,
	[RefreshCreator] [nvarchar](max) NULL,
	[MaterailName] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_CutofValves] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_CylinderDetails]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_CylinderDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ConstractorId] [int] NULL,
	[Bulk] [nvarchar](max) NOT NULL,
	[Lenght] [nvarchar](max) NULL,
	[Pressure] [nvarchar](max) NULL,
	[Diameter] [nvarchar](max) NULL,
	[Rezve] [nvarchar](max) NULL,
	[Model] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_CylinderDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Deposits]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Deposits](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[WorkshopsID] [int] NULL,
	[NationalCode] [nvarchar](max) NULL,
	[FullName] [nvarchar](max) NULL,
	[TrackingCode] [nvarchar](max) NULL,
	[Value] [float] NOT NULL,
	[Bank] [nvarchar](max) NULL,
	[Branch] [nvarchar](max) NULL,
	[DifinitiveCode] [nvarchar](max) NULL,
	[InflectionCode] [nvarchar](max) NULL,
	[DepositorID] [nvarchar](max) NULL,
	[Serial] [nvarchar](max) NULL,
	[Depositor] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Creator] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_Deposits] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_DocHistories]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_DocHistories](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DocumentsID] [int] NULL,
	[ProductsID] [int] NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Attachments] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[CreateDate] [datetime] NULL,
	[Creator] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_DocHistories] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_DocTypes]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_DocTypes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CreateDate] [datetime] NULL,
	[Creator] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_DocTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Documents]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Documents](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DocTypeID] [int] NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Attachments] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[CreateDate] [datetime] NULL,
	[Creator] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_Documents] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Educations]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Educations](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[VehicleTypeId] [int] NULL,
	[File] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
	[Status] [bit] NOT NULL,
	[Version] [nvarchar](max) NULL,
	[Title] [nvarchar](max) NULL,
	[DownloadUrl] [nvarchar](max) NULL,
	[Cat] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_Educations] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Employers]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Employers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CreateDate] [datetime] NULL,
	[Creator] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_Employers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_FillingValveConstractors]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_FillingValveConstractors](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FillingValve] [nvarchar](max) NOT NULL,
	[Code] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_FillingValveConstractors] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_FillingValves]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_FillingValves](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[serialNumber] [nvarchar](max) NULL,
	[constractor] [nvarchar](max) NULL,
	[model] [nvarchar](max) NULL,
	[productDate] [nvarchar](max) NULL,
	[workshop] [nvarchar](max) NULL,
	[status] [nvarchar](max) NULL,
	[CreateDate] [datetime] NULL,
	[Creator] [nvarchar](max) NULL,
	[RefreshDate] [datetime] NULL,
	[RefreshCreator] [nvarchar](max) NULL,
	[QRCodeText] [nvarchar](max) NULL,
	[MaterailName] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_FillingValves] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_FinallFreeSaleInvoices]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_FinallFreeSaleInvoices](
	[InvoiceID] [int] IDENTITY(1,1) NOT NULL,
	[CustomersID] [int] NULL,
	[WorkshopsID] [int] NULL,
	[EquipmentsID] [int] NULL,
	[ServiceCode] [nvarchar](max) NULL,
	[InvoiceCode] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[EmployerEconomicalnumber] [nvarchar](max) NULL,
	[Employerregistrationnumber] [nvarchar](max) NULL,
	[EmployerState] [nvarchar](max) NULL,
	[EmployerAddress] [nvarchar](max) NULL,
	[EmployerPostalcode] [nvarchar](max) NULL,
	[EmployerPhone] [nvarchar](max) NULL,
	[EmployerFax] [nvarchar](max) NULL,
	[ServiceDesc] [nvarchar](max) NOT NULL,
	[Number] [int] NOT NULL,
	[UnitofMeasurement] [nvarchar](max) NULL,
	[UnitAmount] [float] NOT NULL,
	[TotalAmount] [float] NULL,
	[DiscountAmount] [float] NULL,
	[TotalAmountafterDiscount] [float] NULL,
	[Tax] [float] NULL,
	[Complications] [float] NULL,
	[AmountTaxandComplications] [float] NULL,
	[TotalAmountTaxandComplications] [float] NULL,
	[Description] [nvarchar](max) NULL,
	[SaleCondition] [nvarchar](max) NULL,
	[Comment] [nvarchar](max) NULL,
	[AcceptedAmount] [float] NULL,
	[AcceptedDate] [datetime] NULL,
	[Status] [bit] NOT NULL,
	[CurrencyTypeID] [int] NULL,
	[CreatorUser] [int] NULL,
	[ViewStatus] [bit] NOT NULL,
	[ViewDate] [datetime] NULL,
	[Viewer] [nvarchar](max) NULL,
	[PreInvoiceCode] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_FinallFreeSaleInvoices] PRIMARY KEY CLUSTERED 
(
	[InvoiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_FreeSaleInvoices]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_FreeSaleInvoices](
	[InvoiceID] [int] IDENTITY(1,1) NOT NULL,
	[WorkshopsID] [int] NULL,
	[EquipmentsID] [int] NULL,
	[ServiceCode] [nvarchar](max) NULL,
	[InvoiceCode] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[EmployerEconomicalnumber] [nvarchar](max) NULL,
	[Employerregistrationnumber] [nvarchar](max) NULL,
	[EmployerState] [nvarchar](max) NULL,
	[EmployerAddress] [nvarchar](max) NULL,
	[EmployerPostalcode] [nvarchar](max) NULL,
	[EmployerPhone] [nvarchar](max) NULL,
	[EmployerFax] [nvarchar](max) NULL,
	[ServiceDesc] [nvarchar](max) NOT NULL,
	[Number] [int] NOT NULL,
	[UnitofMeasurement] [nvarchar](max) NULL,
	[UnitAmount] [float] NOT NULL,
	[TotalAmount] [float] NULL,
	[DiscountAmount] [float] NULL,
	[TotalAmountafterDiscount] [float] NULL,
	[Tax] [float] NULL,
	[Complications] [float] NULL,
	[AmountTaxandComplications] [float] NULL,
	[TotalAmountTaxandComplications] [float] NULL,
	[Description] [nvarchar](max) NULL,
	[SaleCondition] [nvarchar](max) NULL,
	[Comment] [nvarchar](max) NULL,
	[AcceptedAmount] [float] NULL,
	[AcceptedDate] [datetime] NULL,
	[Status] [bit] NOT NULL,
	[CurrencyTypeID] [int] NULL,
	[CreatorUser] [int] NULL,
	[ViewStatus] [bit] NOT NULL,
	[ViewDate] [datetime] NULL,
	[Viewer] [nvarchar](max) NULL,
	[CustomersID] [int] NULL,
	[RequestInvoiceCode] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_FreeSaleInvoices] PRIMARY KEY CLUSTERED 
(
	[InvoiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_FreeSaleRemittanceDetails]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_FreeSaleRemittanceDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RemittancesID] [int] NULL,
	[Date] [datetime] NULL,
	[Vehicle] [nvarchar](max) NOT NULL,
	[Plate] [nvarchar](max) NULL,
	[BillofLading] [nvarchar](max) NULL,
	[Transferee] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
	[Creator] [nvarchar](max) NULL,
	[CarryFare] [float] NULL,
	[CarrierName] [nvarchar](max) NULL,
	[Attachment] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_FreeSaleRemittanceDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_FreeSaleRemittances]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_FreeSaleRemittances](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Number] [nvarchar](max) NOT NULL,
	[InvoiceCode] [nvarchar](max) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Creator] [nvarchar](max) NULL,
	[Status] [bit] NULL,
	[StatusDate] [datetime] NULL,
	[Description] [nvarchar](max) NULL,
	[Incomplete] [bit] NULL,
	[IncompleteDesc] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_FreeSaleRemittances] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_FuelRelayConstractors]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_FuelRelayConstractors](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FuelRelay] [nvarchar](max) NOT NULL,
	[Code] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_FuelRelayConstractors] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_FuelRelays]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_FuelRelays](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MaterailName] [nvarchar](max) NULL,
	[serialNumber] [nvarchar](max) NULL,
	[constractor] [nvarchar](max) NULL,
	[generation] [nvarchar](max) NULL,
	[model] [nvarchar](max) NULL,
	[type] [nvarchar](max) NULL,
	[productDate] [nvarchar](max) NULL,
	[expireDate] [nvarchar](max) NULL,
	[workshop] [nvarchar](max) NULL,
	[status] [nvarchar](max) NULL,
	[CreateDate] [datetime] NULL,
	[Creator] [nvarchar](max) NULL,
	[RefreshDate] [datetime] NULL,
	[RefreshCreator] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_FuelRelays] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_GasECU]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_GasECU](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MaterailName] [nvarchar](max) NULL,
	[serialNumber] [nvarchar](max) NULL,
	[constractor] [nvarchar](max) NULL,
	[generation] [nvarchar](max) NULL,
	[model] [nvarchar](max) NULL,
	[type] [nvarchar](max) NULL,
	[productDate] [nvarchar](max) NULL,
	[expireDate] [nvarchar](max) NULL,
	[workshop] [nvarchar](max) NULL,
	[status] [nvarchar](max) NULL,
	[CreateDate] [datetime] NULL,
	[Creator] [nvarchar](max) NULL,
	[RefreshDate] [datetime] NULL,
	[RefreshCreator] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_GasECU] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_GasECUConstractors]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_GasECUConstractors](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[GasECU] [nvarchar](max) NOT NULL,
	[Code] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_GasECUConstractors] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_GenerationofRegulators]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_GenerationofRegulators](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_GenerationofRegulators] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_InstructionForms]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_InstructionForms](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NULL,
	[Attachment] [nvarchar](max) NULL,
	[InstructionID] [int] NULL,
	[Creator] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.tbl_InstructionForms] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Instructions]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Instructions](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NULL,
	[DateofApproval] [datetime] NULL,
	[ValidityDate] [datetime] NULL,
	[Code] [nvarchar](max) NULL,
	[Status] [bit] NOT NULL,
	[Attachment] [nvarchar](max) NULL,
	[CategoriesID] [int] NULL,
	[Description] [nvarchar](max) NULL,
	[PublishLevel] [nvarchar](max) NULL,
	[Creator] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.tbl_Instructions] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_InsuranceCompanies]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_InsuranceCompanies](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_InsuranceCompanies] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Insurances]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Insurances](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[VehicleRegistrationID] [int] NOT NULL,
	[Number] [nvarchar](max) NOT NULL,
	[Creator] [nvarchar](max) NULL,
	[CreateDate] [datetime] NULL,
 CONSTRAINT [PK_dbo.tbl_Insurances] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_InsuranceTypes]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_InsuranceTypes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_InsuranceTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Invoices]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Invoices](
	[InvoiceID] [int] IDENTITY(1,1) NOT NULL,
	[OwnersID] [int] NULL,
	[WorkshopsID] [int] NULL,
	[EquipmentsID] [int] NULL,
	[ServiceCode] [nvarchar](max) NULL,
	[InvoiceCode] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[EmployerEconomicalnumber] [nvarchar](max) NULL,
	[Employerregistrationnumber] [nvarchar](max) NULL,
	[EmployerState] [nvarchar](max) NULL,
	[EmployerAddress] [nvarchar](max) NULL,
	[EmployerPostalcode] [nvarchar](max) NULL,
	[EmployerPhone] [nvarchar](max) NULL,
	[EmployerFax] [nvarchar](max) NULL,
	[ServiceDesc] [nvarchar](max) NOT NULL,
	[Number] [nvarchar](max) NOT NULL,
	[UnitofMeasurement] [nvarchar](max) NULL,
	[UnitAmount] [float] NOT NULL,
	[TotalAmount] [float] NULL,
	[DiscountAmount] [float] NULL,
	[TotalAmountafterDiscount] [float] NULL,
	[Tax] [float] NULL,
	[Complications] [float] NULL,
	[AmountTaxandComplications] [float] NULL,
	[TotalAmountTaxandComplications] [float] NULL,
	[Description] [nvarchar](max) NULL,
	[SaleCondition] [nvarchar](max) NULL,
	[Comment] [nvarchar](max) NULL,
	[AcceptedAmount] [float] NULL,
	[AcceptedDate] [datetime] NULL,
	[Status] [bit] NULL,
	[CurrencyTypeID] [int] NULL,
	[CreatorUser] [int] NULL,
	[RegistrationTypeID] [int] NULL,
 CONSTRAINT [PK_dbo.tbl_Invoices] PRIMARY KEY CLUSTERED 
(
	[InvoiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_IRNGVDamages]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_IRNGVDamages](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Acceptance] [nvarchar](max) NULL,
	[NationalCode] [nvarchar](max) NULL,
	[Constractor] [nvarchar](max) NULL,
	[VehicleType] [nvarchar](max) NULL,
	[State] [nvarchar](max) NULL,
	[City] [nvarchar](max) NULL,
	[Plate] [nvarchar](max) NULL,
	[EngineNumber] [nvarchar](max) NULL,
	[ChassisNumber] [nvarchar](max) NULL,
	[ConstructionYear] [nvarchar](4) NULL,
	[Workshop] [nvarchar](max) NULL,
	[WorkshopID] [nvarchar](max) NULL,
	[InspectionCertificateNumber] [nvarchar](max) NULL,
	[DateofCertification] [nvarchar](max) NULL,
	[Serial] [nvarchar](max) NULL,
	[Insurance] [nvarchar](max) NULL,
	[InsuranceNumber] [nvarchar](max) NULL,
	[InspectionCompany] [nvarchar](max) NULL,
	[InspectorName] [nvarchar](max) NULL,
	[Cylinder] [nvarchar](max) NULL,
	[Valve] [nvarchar](max) NULL,
	[Regulator] [nvarchar](max) NULL,
	[Generation] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Literage] [nvarchar](max) NULL,
	[Creator] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
	[Literage_2] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_IRNGVDamages] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Kits]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Kits](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[serialNumber] [nvarchar](max) NULL,
	[constractor] [nvarchar](max) NULL,
	[generation] [nvarchar](max) NULL,
	[model] [nvarchar](max) NULL,
	[type] [nvarchar](max) NULL,
	[productDate] [nvarchar](max) NOT NULL,
	[workshop] [nvarchar](max) NULL,
	[status] [nvarchar](max) NULL,
	[CreateDate] [datetime] NULL,
	[Creator] [nvarchar](max) NULL,
	[RefreshDate] [datetime] NULL,
	[RefreshCreator] [nvarchar](max) NULL,
	[MaterailName] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_Kits] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_ListofServices]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_ListofServices](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NULL,
	[ServiceRent] [float] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Type] [nvarchar](max) NULL,
	[Unit] [nvarchar](max) NULL,
	[Presentable] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.tbl_ListofServices] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_MessageForwards]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_MessageForwards](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MessageID] [int] NULL,
	[SenderID] [int] NULL,
	[SenderDate] [datetime] NOT NULL,
	[ReciverID] [int] NULL,
	[ReadStatus] [bit] NULL,
	[ReadDate] [datetime] NULL,
	[Attachment] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_MessageForwards] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_MessageReplies]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_MessageReplies](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MessageID] [int] NULL,
	[Subject] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[SenderID] [int] NULL,
	[SenderDate] [datetime] NOT NULL,
	[ReciverID] [int] NULL,
	[ReadStatus] [bit] NULL,
	[ReadDate] [datetime] NULL,
	[Attachment] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_MessageReplies] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Messages]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Messages](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Subject] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[SenderID] [int] NULL,
	[SenderDate] [datetime] NOT NULL,
	[WorkshopID] [int] NOT NULL,
	[ReadStatus] [bit] NULL,
	[ReadDate] [datetime] NULL,
	[Attachment] [nvarchar](max) NULL,
	[Priority] [nvarchar](max) NOT NULL,
	[LetterNumber] [nvarchar](max) NULL,
	[ReciverID] [int] NULL,
	[MessageID] [int] NULL,
	[Type] [int] NULL,
 CONSTRAINT [PK_dbo.tbl_Messages] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_OfferedPrices]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_OfferedPrices](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[WorkshopID] [int] NULL,
	[Date] [datetime] NOT NULL,
	[Value] [float] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
	[Creator] [nvarchar](max) NULL,
	[StatusPay] [bit] NOT NULL,
	[PayDate] [datetime] NULL,
	[PaiedValue] [float] NOT NULL,
	[Number] [int] NOT NULL,
	[Serial] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_OfferedPrices] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Operators]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Operators](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[WorkshopID] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Family] [nvarchar](max) NOT NULL,
	[PhoneNumber] [nvarchar](max) NOT NULL,
	[MobileNumber] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NOT NULL,
	[Image] [nvarchar](max) NULL,
	[Creator] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
	[Status] [bit] NULL,
 CONSTRAINT [PK_dbo.tbl_Operators] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Organizations]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Organizations](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[CityID] [int] NOT NULL,
	[OwnerName] [nvarchar](max) NOT NULL,
	[OwnerFamily] [nvarchar](max) NOT NULL,
	[PhoneNumber] [nvarchar](max) NOT NULL,
	[MobileNumber] [nvarchar](max) NOT NULL,
	[FaxNumber] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NOT NULL,
	[BusinessLicense] [nvarchar](max) NULL,
	[Creator] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
	[OrgCode] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_Organizations] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Otherthings]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Otherthings](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Title] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.tbl_Otherthings] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_OtherThingsDivisionPlans]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_OtherThingsDivisionPlans](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DivisionPlanID] [int] NULL,
	[Number] [int] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[NumberofSend] [int] NULL,
	[DiThingsID] [int] NOT NULL,
	[GenarationID] [int] NULL,
	[RegistrationTypeID] [int] NULL,
 CONSTRAINT [PK_dbo.tbl_OtherThingsDivisionPlans] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Payments]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Payments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](max) NULL,
	[NationalCode] [nvarchar](max) NULL,
	[MobileNumber] [nvarchar](max) NULL,
	[EMailAddress] [nvarchar](max) NULL,
	[Amount] [float] NOT NULL,
	[PayDate] [datetime] NOT NULL,
	[PayerIPAddress] [nvarchar](max) NULL,
	[Status] [nvarchar](max) NULL,
	[RefID] [nvarchar](max) NULL,
	[SaleReferenceId] [nvarchar](max) NULL,
	[OrderID] [nvarchar](max) NULL,
	[PreInvoiceCode] [nvarchar](max) NULL,
	[InvoiceCode] [nvarchar](max) NULL,
	[PayerCode] [int] NULL,
	[RequestInvoiceCode] [nvarchar](max) NULL,
	[PaymentMethod] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_Payments] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_ProCategories]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_ProCategories](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Cat] [nvarchar](max) NOT NULL,
	[ProTypeID] [int] NULL,
	[Description] [nvarchar](max) NULL,
	[CreateDate] [datetime] NULL,
	[Creator] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_ProCategories] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Process]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Process](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Attachments] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[CreateDate] [datetime] NULL,
	[Creator] [nvarchar](max) NULL,
	[ProCategoryID] [int] NULL,
 CONSTRAINT [PK_dbo.tbl_Process] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_ProHistories]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_ProHistories](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProcessID] [int] NULL,
	[ProductsID] [int] NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Attachments] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[CreateDate] [datetime] NULL,
	[Creator] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_ProHistories] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_ProTypes]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_ProTypes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CreateDate] [datetime] NULL,
	[Creator] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_ProTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_RegistrationPrice]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_RegistrationPrice](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](max) NULL,
	[Price] [float] NOT NULL,
	[FromDate] [datetime] NOT NULL,
	[ToDate] [datetime] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[OldPrice] [float] NULL,
	[DepType] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_RegistrationPrice] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_RegistrationPrice_Old]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_RegistrationPrice_Old](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [int] NOT NULL,
	[Price] [float] NOT NULL,
	[FromDate] [datetime] NOT NULL,
	[ToDate] [datetime] NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_RegistrationPrice] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_RegistrationTypes]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_RegistrationTypes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](max) NOT NULL,
	[Image] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_RegistrationTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_RegulatorConstractors]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_RegulatorConstractors](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Regulator] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Code] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_RegulatorConstractors] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_ReplacementPlanPrice]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_ReplacementPlanPrice](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EquipmentTypeID] [int] NULL,
	[Price] [float] NOT NULL,
	[DiscountedPrice] [float] NOT NULL,
	[FromDate] [datetime] NOT NULL,
	[ToDate] [datetime] NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_ReplacementPlanPrice] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_ReplacementPlanValvePrice]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_ReplacementPlanValvePrice](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EquipmentTypeID] [int] NULL,
	[Price] [float] NOT NULL,
	[DiscountedPrice] [float] NOT NULL,
	[FromDate] [datetime] NOT NULL,
	[ToDate] [datetime] NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_ReplacementPlanValvePrice] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_RequestFreeSales]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_RequestFreeSales](
	[InvoiceID] [int] IDENTITY(1,1) NOT NULL,
	[Owners] [nvarchar](max) NULL,
	[WorkshopsID] [int] NULL,
	[EquipmentsID] [int] NULL,
	[ServiceCode] [nvarchar](max) NULL,
	[InvoiceCode] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[EmployerEconomicalnumber] [nvarchar](max) NULL,
	[Employerregistrationnumber] [nvarchar](max) NULL,
	[EmployerState] [nvarchar](max) NULL,
	[EmployerAddress] [nvarchar](max) NULL,
	[EmployerPostalcode] [nvarchar](max) NULL,
	[EmployerPhone] [nvarchar](max) NULL,
	[EmployerFax] [nvarchar](max) NULL,
	[ServiceDesc] [nvarchar](max) NOT NULL,
	[Number] [int] NOT NULL,
	[UnitofMeasurement] [nvarchar](max) NULL,
	[UnitAmount] [float] NOT NULL,
	[TotalAmount] [float] NULL,
	[DiscountAmount] [float] NULL,
	[Description] [nvarchar](max) NULL,
	[SaleCondition] [nvarchar](max) NULL,
	[Comment] [nvarchar](max) NULL,
	[Status] [bit] NOT NULL,
	[CurrencyTypeID] [int] NULL,
	[CreatorUser] [int] NULL,
	[ViewStatus] [bit] NOT NULL,
	[ViewDate] [datetime] NULL,
	[Viewer] [nvarchar](max) NULL,
	[FinalStatus] [bit] NOT NULL,
	[RegistrationTypeID] [int] NULL,
 CONSTRAINT [PK_dbo.tbl_RequestFreeSales] PRIMARY KEY CLUSTERED 
(
	[InvoiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_ReturnofParts]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_ReturnofParts](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](max) NULL,
	[WorkshopID] [int] NULL,
	[VehicleTypeID] [int] NULL,
	[TypeofPiece] [nvarchar](max) NULL,
	[Transferee] [int] NULL,
	[CreateDate] [datetime] NOT NULL,
	[Creator] [int] NULL,
	[Action] [nvarchar](max) NULL,
	[NumberofSend] [int] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Date] [datetime] NOT NULL,
	[EquipmentID] [int] NULL,
 CONSTRAINT [PK_dbo.tbl_ReturnofParts] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_SaleWarehouses]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_SaleWarehouses](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[FinancialCode] [nvarchar](max) NULL,
	[Title] [nvarchar](max) NULL,
	[Units] [nvarchar](max) NULL,
	[Rem] [float] NULL,
	[CurrentRem] [float] NULL,
	[Creator] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.tbl_SaleWarehouses] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_SideBarItems]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_SideBarItems](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[nameOption] [nvarchar](max) NULL,
	[controller] [nvarchar](max) NULL,
	[action] [nvarchar](max) NULL,
	[imageClass] [nvarchar](max) NULL,
	[status] [bit] NOT NULL,
	[isParent] [bit] NOT NULL,
	[parentId] [int] NULL,
	[orderBy] [int] NULL,
	[Category] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_SideBarItems] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Sliders]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Sliders](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Image] [nvarchar](max) NULL,
	[Title] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Cat] [nvarchar](max) NULL,
	[Order] [int] NULL,
	[Presentable] [bit] NOT NULL,
	[DueDate] [int] NOT NULL,
	[Url] [nvarchar](max) NULL,
	[Section] [nvarchar](max) NULL,
	[Refrence] [nvarchar](max) NULL,
	[Creator] [nvarchar](max) NULL,
	[CreatDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.tbl_Sliders] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_SMSPanelResults]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_SMSPanelResults](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Mobile] [nvarchar](max) NULL,
	[FullName] [nvarchar](max) NULL,
	[Workshop] [nvarchar](max) NULL,
	[refID] [nvarchar](max) NULL,
	[Result] [nvarchar](max) NULL,
	[Sender] [nvarchar](max) NULL,
	[SenderIP] [nvarchar](max) NULL,
	[SendDate] [datetime] NOT NULL,
	[Section] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_SMSPanelResults] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Tanks]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Tanks](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[serialNumber] [nvarchar](max) NULL,
	[bulk] [nvarchar](max) NULL,
	[model] [nvarchar](max) NULL,
	[type] [nvarchar](max) NULL,
	[length] [nvarchar](max) NULL,
	[pressure] [nvarchar](max) NULL,
	[diameter] [nvarchar](max) NULL,
	[rezve] [nvarchar](max) NULL,
	[productDate] [nvarchar](max) NOT NULL,
	[expireDate] [nvarchar](max) NOT NULL,
	[gregorianPMonth] [nvarchar](max) NULL,
	[gregorianPYear] [nvarchar](max) NULL,
	[gregorianEMonth] [nvarchar](max) NULL,
	[gregorianEYear] [nvarchar](max) NULL,
	[constractor] [nvarchar](max) NULL,
	[workshop] [nvarchar](max) NULL,
	[status] [nvarchar](max) NULL,
	[CreateDate] [datetime] NULL,
	[Creator] [nvarchar](max) NULL,
	[RefreshDate] [datetime] NULL,
	[RefreshCreator] [nvarchar](max) NULL,
	[MaterailName] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_Tanks] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_TankValves]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_TankValves](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[serialNumber] [nvarchar](max) NULL,
	[constractor] [nvarchar](max) NULL,
	[model] [nvarchar](max) NULL,
	[type] [nvarchar](max) NULL,
	[rezve] [nvarchar](max) NULL,
	[productDate] [nvarchar](max) NOT NULL,
	[workshop] [nvarchar](max) NULL,
	[status] [nvarchar](max) NULL,
	[CreateDate] [datetime] NULL,
	[Creator] [nvarchar](max) NULL,
	[RefreshDate] [datetime] NULL,
	[RefreshCreator] [nvarchar](max) NULL,
	[MaterailName] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_TankValves_new]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_TankValves_new](
	[ID] [int] NOT NULL,
	[serialNumber] [nvarchar](max) NULL,
	[constractor] [nvarchar](max) NULL,
	[model] [nvarchar](max) NULL,
	[type] [nvarchar](max) NULL,
	[rezve] [nvarchar](max) NULL,
	[productDate] [nvarchar](max) NOT NULL,
	[workshop] [nvarchar](max) NULL,
	[status] [nvarchar](max) NULL,
	[CreateDate] [datetime] NULL,
	[Creator] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_TaxandComplications]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_TaxandComplications](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Value] [float] NOT NULL,
	[Year] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
	[Creator] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_TaxandComplications] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_TaxValueAdded]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_TaxValueAdded](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Year] [int] NOT NULL,
	[ValueAdded] [float] NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_TaxValueAdded] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_TwoTableDiff]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_TwoTableDiff](
	[VehicleType] [nvarchar](max) NULL,
	[ConstructionYear] [nvarchar](max) NULL,
	[EngineNumber] [nvarchar](max) NULL,
	[ChassisNumber] [nvarchar](max) NULL,
	[NationalCode] [nvarchar](max) NULL,
	[Workshop] [nvarchar](max) NULL,
	[Plate] [nvarchar](max) NULL,
	[Plate2] [nvarchar](max) NULL,
	[HealthCertificate] [nvarchar](max) NULL,
	[Status] [nvarchar](max) NULL,
	[IRNGV_Values] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_TypeofTankCovers]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_TypeofTankCovers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[VehicleTypeId] [int] NULL,
 CONSTRAINT [PK_dbo.tbl_TypeofTankCovers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_TypeofUses]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_TypeofUses](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tbl_TypeofUses] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_VehicleAttachments]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_VehicleAttachments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[VehicleRegistrationID] [int] NULL,
	[Image] [nvarchar](max) NOT NULL,
	[Folder] [nvarchar](max) NULL,
	[Creator] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.tbl_VehicleAttachments] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_VehicleInvoices]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_VehicleInvoices](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[VehicleRegistrationID] [int] NULL,
	[Number] [nvarchar](max) NOT NULL,
	[Amount] [float] NOT NULL,
	[Type] [nvarchar](max) NOT NULL,
	[Date] [datetime] NOT NULL,
	[InvoiceFile] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Creator] [int] NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.tbl_VehicleInvoices] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Warehouses]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Warehouses](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[FinancialCode] [nvarchar](max) NULL,
	[Title] [nvarchar](max) NULL,
	[Units] [nvarchar](max) NULL,
	[Rem] [float] NULL,
	[CurrentRem] [float] NULL,
	[Creator] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.tbl_Warehouses] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_WorkshopInsurance]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_WorkshopInsurance](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[WorkshopID] [int] NULL,
	[InsuranceCompaniesID] [int] NULL,
	[InsuranceTypesID] [int] NULL,
	[StartDate] [datetime] NULL,
	[FinishDate] [datetime] NULL,
	[Image] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Creator] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
	[Value] [float] NOT NULL,
 CONSTRAINT [PK_dbo.tbl_WorkshopInsurance] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[updateTank_13990430]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[updateTank_13990430](
	[ID] [float] NULL,
	[serialNumber] [nvarchar](max) NULL,
	[bulk] [float] NULL,
	[model] [nvarchar](255) NULL,
	[type] [nvarchar](255) NULL,
	[length] [float] NULL,
	[pressure] [float] NULL,
	[diameter] [float] NULL,
	[rezve] [nvarchar](255) NULL,
	[productDate] [nvarchar](255) NULL,
	[expireDate] [nvarchar](255) NULL,
	[constractor] [nvarchar](255) NULL,
	[workshop] [nvarchar](255) NULL,
	[status] [nvarchar](255) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[updateTankValve_13990430]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[updateTankValve_13990430](
	[ID] [float] NULL,
	[serialNumber] [nvarchar](max) NULL,
	[constractor] [nvarchar](255) NULL,
	[model] [nvarchar](255) NULL,
	[type] [nvarchar](255) NULL,
	[rezve] [nvarchar](255) NULL,
	[productDate] [nvarchar](255) NULL,
	[workshop] [nvarchar](255) NULL,
	[status] [nvarchar](255) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserEntranceInfo]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserEntranceInfo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EntranceDate] [datetime] NOT NULL,
	[IPAddress] [nvarchar](max) NULL,
	[UserID] [int] NULL,
	[Status] [bit] NULL,
 CONSTRAINT [PK_dbo.UserEntranceInfo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](30) NOT NULL,
	[Password] [nvarchar](30) NOT NULL,
	[LastModified] [datetime] NULL,
	[Inactive] [bit] NULL,
	[Firstname] [nvarchar](50) NULL,
	[Lastname] [nvarchar](50) NULL,
	[EMail] [nvarchar](100) NULL,
	[MobileNumber] [nvarchar](100) NULL,
	[DataPermission] [nvarchar](1000) NULL,
	[WorkshopID] [int] NULL,
	[Image] [nvarchar](max) NULL,
	[AuditCompaniesID] [int] NULL,
	[isAuditManager] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbl_ContradictionTotals] ADD  DEFAULT ((0)) FOR [Count]
GO
ALTER TABLE [dbo].[tbl_Deposits] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [CreateDate]
GO
ALTER TABLE [dbo].[tbl_FreeSaleInvoices] ADD  DEFAULT ((0)) FOR [ViewStatus]
GO
ALTER TABLE [dbo].[tbl_InstructionForms] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [CreateDate]
GO
ALTER TABLE [dbo].[tbl_Instructions] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [CreateDate]
GO
ALTER TABLE [dbo].[tbl_ListofServices] ADD  DEFAULT ((0)) FOR [Presentable]
GO
ALTER TABLE [dbo].[tbl_OfferedPrices] ADD  DEFAULT ((0)) FOR [Number]
GO
ALTER TABLE [dbo].[tbl_Otherthings] ADD  DEFAULT ('') FOR [Title]
GO
ALTER TABLE [dbo].[tbl_OtherThingsDivisionPlans] ADD  DEFAULT ((0)) FOR [DiThingsID]
GO
ALTER TABLE [dbo].[tbl_RequestFreeSales] ADD  DEFAULT ((0)) FOR [ViewStatus]
GO
ALTER TABLE [dbo].[tbl_RequestFreeSales] ADD  DEFAULT ((0)) FOR [FinalStatus]
GO
ALTER TABLE [dbo].[tbl_VehicleAttachments] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [CreateDate]
GO
ALTER TABLE [dbo].[tbl_VehicleRegistrations] ADD  CONSTRAINT [DF__tbl_Vehic__Creat__5EBF139D]  DEFAULT ('1900-01-01T00:00:00.000') FOR [CreateDate]
GO
ALTER TABLE [dbo].[tbl_VehicleRegistrations] ADD  CONSTRAINT [DF__tbl_Vehic__Seria__5FB337D6]  DEFAULT ('') FOR [SerialSparkPreview]
GO
ALTER TABLE [dbo].[tbl_VehicleRegistrations] ADD  CONSTRAINT [DF__tbl_Vehic__Seria__60A75C0F]  DEFAULT ('') FOR [SerialKit]
GO
ALTER TABLE [dbo].[tbl_VehicleRegistrations] ADD  CONSTRAINT [DF__tbl_Vehic__Seria__619B8048]  DEFAULT ('') FOR [SerialKey]
GO
ALTER TABLE [dbo].[tbl_VehicleRegistrations] ADD  CONSTRAINT [DF__tbl_Vehic__Seria__628FA481]  DEFAULT ('') FOR [SerialRefuelingValve]
GO
ALTER TABLE [dbo].[tbl_VehicleRegistrations] ADD  CONSTRAINT [DF__tbl_Vehic__Refue__6383C8BA]  DEFAULT ('') FOR [RefuelingLable]
GO
ALTER TABLE [dbo].[tbl_VehicleRegistrations] ADD  CONSTRAINT [DF__tbl_Vehic__Track__6477ECF3]  DEFAULT ('') FOR [TrackingCode]
GO
ALTER TABLE [dbo].[tbl_VehicleRegistrations] ADD  CONSTRAINT [DF__tbl_Vehic__Licen__656C112C]  DEFAULT ('') FOR [License]
GO
ALTER TABLE [dbo].[tbl_VehicleRegistrations] ADD  CONSTRAINT [DF__tbl_Vehic__Licen__66603565]  DEFAULT ('') FOR [LicenseImage]
GO
ALTER TABLE [dbo].[tbl_VehicleRegistrations] ADD  CONSTRAINT [DF__tbl_Vehicle__VIN__6754599E]  DEFAULT ('') FOR [VIN]
GO
ALTER TABLE [dbo].[tbl_VehicleRegistrations] ADD  CONSTRAINT [DF__tbl_Vehic__Typeo__68487DD7]  DEFAULT ((0)) FOR [TypeofUseID]
GO
ALTER TABLE [dbo].[tbl_VehicleRegistrations] ADD  DEFAULT ((0)) FOR [Status]
GO
ALTER TABLE [dbo].[tbl_VehicleRegistrations] ADD  DEFAULT ((0)) FOR [Checked]
GO
ALTER TABLE [dbo].[tbl_VehicleRegistrations] ADD  DEFAULT ((0)) FOR [RegisterStatus]
GO
ALTER TABLE [dbo].[tbl_VehicleRegistrations] ADD  DEFAULT ((0)) FOR [FinancialStatus]
GO
ALTER TABLE [dbo].[tbl_VehicleTanks] ADD  DEFAULT ((0)) FOR [ValveConstractorID]
GO
ALTER TABLE [dbo].[tbl_VehicleTanks] ADD  DEFAULT ('') FOR [RegulatorSerial]
GO
ALTER TABLE [dbo].[tbl_VehicleTanks] ADD  DEFAULT ('') FOR [CutofValveSerial]
GO
ALTER TABLE [dbo].[tbl_VehicleTanks] ADD  DEFAULT ('') FOR [FillingValveSerial]
GO
ALTER TABLE [dbo].[tbl_WorkshopInsurance] ADD  DEFAULT ((0)) FOR [Value]
GO
ALTER TABLE [dbo].[tbl_Workshops] ADD  DEFAULT ('') FOR [OwnerName]
GO
ALTER TABLE [dbo].[tbl_Workshops] ADD  DEFAULT ('') FOR [OwnerFamily]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [isAuditManager]
GO
ALTER TABLE [dbo].[LNK_Role_Permission]  WITH CHECK ADD  CONSTRAINT [FK_dbo.LNK_Role_Permission_dbo.Permissions_Permission_Id] FOREIGN KEY([Permission_Id])
REFERENCES [dbo].[Permissions] ([Permission_Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LNK_Role_Permission] CHECK CONSTRAINT [FK_dbo.LNK_Role_Permission_dbo.Permissions_Permission_Id]
GO
ALTER TABLE [dbo].[LNK_Role_Permission]  WITH CHECK ADD  CONSTRAINT [FK_dbo.LNK_Role_Permission_dbo.Roles_Role_Id] FOREIGN KEY([Role_Id])
REFERENCES [dbo].[Roles] ([Role_Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LNK_Role_Permission] CHECK CONSTRAINT [FK_dbo.LNK_Role_Permission_dbo.Roles_Role_Id]
GO
ALTER TABLE [dbo].[LNK_User_Organization]  WITH CHECK ADD  CONSTRAINT [FK_dbo.LNK_User_Organization_dbo.tbl_Organizations_Organization_Id] FOREIGN KEY([Organization_Id])
REFERENCES [dbo].[tbl_Organizations] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LNK_User_Organization] CHECK CONSTRAINT [FK_dbo.LNK_User_Organization_dbo.tbl_Organizations_Organization_Id]
GO
ALTER TABLE [dbo].[LNK_User_Organization]  WITH CHECK ADD  CONSTRAINT [FK_dbo.LNK_User_Organization_dbo.Users_User_Id] FOREIGN KEY([User_Id])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LNK_User_Organization] CHECK CONSTRAINT [FK_dbo.LNK_User_Organization_dbo.Users_User_Id]
GO
ALTER TABLE [dbo].[LNK_User_Role]  WITH CHECK ADD  CONSTRAINT [FK_dbo.LNK_User_Role_dbo.Roles_Role_Id] FOREIGN KEY([Role_Id])
REFERENCES [dbo].[Roles] ([Role_Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LNK_User_Role] CHECK CONSTRAINT [FK_dbo.LNK_User_Role_dbo.Roles_Role_Id]
GO
ALTER TABLE [dbo].[LNK_User_Role]  WITH CHECK ADD  CONSTRAINT [FK_dbo.LNK_User_Role_dbo.Users_User_Id] FOREIGN KEY([User_Id])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LNK_User_Role] CHECK CONSTRAINT [FK_dbo.LNK_User_Role_dbo.Users_User_Id]
GO
ALTER TABLE [dbo].[LNK_USER_SideBarItem]  WITH CHECK ADD  CONSTRAINT [FK_dbo.LNK_USER_SideBarItem_dbo.tbl_SideBarItems_SideBarItems_Id] FOREIGN KEY([SideBarItems_Id])
REFERENCES [dbo].[tbl_SideBarItems] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LNK_USER_SideBarItem] CHECK CONSTRAINT [FK_dbo.LNK_USER_SideBarItem_dbo.tbl_SideBarItems_SideBarItems_Id]
GO
ALTER TABLE [dbo].[LNK_USER_SideBarItem]  WITH CHECK ADD  CONSTRAINT [FK_dbo.LNK_USER_SideBarItem_dbo.Users_User_Id] FOREIGN KEY([User_Id])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LNK_USER_SideBarItem] CHECK CONSTRAINT [FK_dbo.LNK_USER_SideBarItem_dbo.Users_User_Id]
GO
ALTER TABLE [dbo].[LNK_User_Workshop]  WITH CHECK ADD  CONSTRAINT [FK_dbo.LNK_User_Workshop_dbo.tbl_Workshops_Workshop_Id] FOREIGN KEY([Workshop_Id])
REFERENCES [dbo].[tbl_Workshops] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LNK_User_Workshop] CHECK CONSTRAINT [FK_dbo.LNK_User_Workshop_dbo.tbl_Workshops_Workshop_Id]
GO
ALTER TABLE [dbo].[LNK_User_Workshop]  WITH CHECK ADD  CONSTRAINT [FK_dbo.LNK_User_Workshop_dbo.Users_User_Id] FOREIGN KEY([User_Id])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LNK_User_Workshop] CHECK CONSTRAINT [FK_dbo.LNK_User_Workshop_dbo.Users_User_Id]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Products_dbo.tbl_Cities_CitiesID] FOREIGN KEY([CitiesID])
REFERENCES [dbo].[tbl_Cities] ([ID])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_dbo.Products_dbo.tbl_Cities_CitiesID]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Products_dbo.tbl_Employers_EmployerID] FOREIGN KEY([EmployerID])
REFERENCES [dbo].[tbl_Employers] ([ID])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_dbo.Products_dbo.tbl_Employers_EmployerID]
GO
ALTER TABLE [dbo].[tbl_AnswerQuestions]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_AnswerQuestions_dbo.tbl_CRMDynamicForms_QuestionId] FOREIGN KEY([QuestionId])
REFERENCES [dbo].[tbl_CRMDynamicForms] ([ID])
GO
ALTER TABLE [dbo].[tbl_AnswerQuestions] CHECK CONSTRAINT [FK_dbo.tbl_AnswerQuestions_dbo.tbl_CRMDynamicForms_QuestionId]
GO
ALTER TABLE [dbo].[tbl_AnswerQuestions]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_AnswerQuestions_dbo.tbl_VehicleRegistrations_RegistrationsId] FOREIGN KEY([RegistrationsId])
REFERENCES [dbo].[tbl_VehicleRegistrations] ([ID])
GO
ALTER TABLE [dbo].[tbl_AnswerQuestions] CHECK CONSTRAINT [FK_dbo.tbl_AnswerQuestions_dbo.tbl_VehicleRegistrations_RegistrationsId]
GO
ALTER TABLE [dbo].[tbl_AuditComponies]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_AuditComponies_dbo.Users_User_UserID] FOREIGN KEY([User_UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[tbl_AuditComponies] CHECK CONSTRAINT [FK_dbo.tbl_AuditComponies_dbo.Users_User_UserID]
GO
ALTER TABLE [dbo].[tbl_AuditFiles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_AuditFiles_dbo.tbl_AuditCategories_CategoryID] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[tbl_AuditCategories] ([ID])
GO
ALTER TABLE [dbo].[tbl_AuditFiles] CHECK CONSTRAINT [FK_dbo.tbl_AuditFiles_dbo.tbl_AuditCategories_CategoryID]
GO
ALTER TABLE [dbo].[tbl_AuditFiles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_AuditFiles_dbo.tbl_Audits_AuditID] FOREIGN KEY([AuditID])
REFERENCES [dbo].[tbl_Audits] ([ID])
GO
ALTER TABLE [dbo].[tbl_AuditFiles] CHECK CONSTRAINT [FK_dbo.tbl_AuditFiles_dbo.tbl_Audits_AuditID]
GO
ALTER TABLE [dbo].[tbl_Auditors]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Auditors_dbo.tbl_AuditComponies_CompaniesID] FOREIGN KEY([CompaniesID])
REFERENCES [dbo].[tbl_AuditComponies] ([ID])
GO
ALTER TABLE [dbo].[tbl_Auditors] CHECK CONSTRAINT [FK_dbo.tbl_Auditors_dbo.tbl_AuditComponies_CompaniesID]
GO
ALTER TABLE [dbo].[tbl_Audits]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Audits_dbo.tbl_Workshops_WorkshopID] FOREIGN KEY([WorkshopID])
REFERENCES [dbo].[tbl_Workshops] ([ID])
GO
ALTER TABLE [dbo].[tbl_Audits] CHECK CONSTRAINT [FK_dbo.tbl_Audits_dbo.tbl_Workshops_WorkshopID]
GO
ALTER TABLE [dbo].[tbl_Audits]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Audits_dbo.Users_UserID] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[tbl_Audits] CHECK CONSTRAINT [FK_dbo.tbl_Audits_dbo.Users_UserID]
GO
ALTER TABLE [dbo].[tbl_AuditsPrice]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_AuditsPrice_dbo.tbl_AuditComponies_AuditCompanyID] FOREIGN KEY([AuditCompanyID])
REFERENCES [dbo].[tbl_AuditComponies] ([ID])
GO
ALTER TABLE [dbo].[tbl_AuditsPrice] CHECK CONSTRAINT [FK_dbo.tbl_AuditsPrice_dbo.tbl_AuditComponies_AuditCompanyID]
GO
ALTER TABLE [dbo].[tbl_BOMs]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_BOMs_dbo.tbl_EquipmentList_EquipmentListID] FOREIGN KEY([EquipmentListID])
REFERENCES [dbo].[tbl_EquipmentList] ([ID])
GO
ALTER TABLE [dbo].[tbl_BOMs] CHECK CONSTRAINT [FK_dbo.tbl_BOMs_dbo.tbl_EquipmentList_EquipmentListID]
GO
ALTER TABLE [dbo].[tbl_BOMs]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_BOMs_dbo.tbl_GenerationofRegulators_GenerationID] FOREIGN KEY([GenerationID])
REFERENCES [dbo].[tbl_GenerationofRegulators] ([ID])
GO
ALTER TABLE [dbo].[tbl_BOMs] CHECK CONSTRAINT [FK_dbo.tbl_BOMs_dbo.tbl_GenerationofRegulators_GenerationID]
GO
ALTER TABLE [dbo].[tbl_BOMs]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_BOMs_dbo.tbl_VehicleTypes_VehicleTypeID] FOREIGN KEY([VehicleTypeID])
REFERENCES [dbo].[tbl_VehicleTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_BOMs] CHECK CONSTRAINT [FK_dbo.tbl_BOMs_dbo.tbl_VehicleTypes_VehicleTypeID]
GO
ALTER TABLE [dbo].[tbl_Cities]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Cities_dbo.tbl_States_StateID] FOREIGN KEY([StateID])
REFERENCES [dbo].[tbl_States] ([ID])
GO
ALTER TABLE [dbo].[tbl_Cities] CHECK CONSTRAINT [FK_dbo.tbl_Cities_dbo.tbl_States_StateID]
GO
ALTER TABLE [dbo].[tbl_Contradictions]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Contradictions_dbo.tbl_Workshops_WorkshopId] FOREIGN KEY([WorkshopId])
REFERENCES [dbo].[tbl_Workshops] ([ID])
GO
ALTER TABLE [dbo].[tbl_Contradictions] CHECK CONSTRAINT [FK_dbo.tbl_Contradictions_dbo.tbl_Workshops_WorkshopId]
GO
ALTER TABLE [dbo].[tbl_ContradictionTotals]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_ContradictionTotals_dbo.tbl_ContradictionTypes_ContradictionTypeId] FOREIGN KEY([ContradictionTypeId])
REFERENCES [dbo].[tbl_ContradictionTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_ContradictionTotals] CHECK CONSTRAINT [FK_dbo.tbl_ContradictionTotals_dbo.tbl_ContradictionTypes_ContradictionTypeId]
GO
ALTER TABLE [dbo].[tbl_ContradictionTotals]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_ContradictionTotals_dbo.tbl_Workshops_WorkshopId] FOREIGN KEY([WorkshopId])
REFERENCES [dbo].[tbl_Workshops] ([ID])
GO
ALTER TABLE [dbo].[tbl_ContradictionTotals] CHECK CONSTRAINT [FK_dbo.tbl_ContradictionTotals_dbo.tbl_Workshops_WorkshopId]
GO
ALTER TABLE [dbo].[tbl_CRMIndexes]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_CRMIndexes_dbo.tbl_CRMIndexes_PId] FOREIGN KEY([PId])
REFERENCES [dbo].[tbl_CRMIndexes] ([ID])
GO
ALTER TABLE [dbo].[tbl_CRMIndexes] CHECK CONSTRAINT [FK_dbo.tbl_CRMIndexes_dbo.tbl_CRMIndexes_PId]
GO
ALTER TABLE [dbo].[tbl_CRMs]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_CRMs_dbo.tbl_CRMIndexes_IndexId] FOREIGN KEY([IndexId])
REFERENCES [dbo].[tbl_CRMIndexes] ([ID])
GO
ALTER TABLE [dbo].[tbl_CRMs] CHECK CONSTRAINT [FK_dbo.tbl_CRMs_dbo.tbl_CRMIndexes_IndexId]
GO
ALTER TABLE [dbo].[tbl_CRMs]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_CRMs_dbo.tbl_VehicleRegistrations_OwnersId] FOREIGN KEY([OwnersId])
REFERENCES [dbo].[tbl_VehicleRegistrations] ([ID])
GO
ALTER TABLE [dbo].[tbl_CRMs] CHECK CONSTRAINT [FK_dbo.tbl_CRMs_dbo.tbl_VehicleRegistrations_OwnersId]
GO
ALTER TABLE [dbo].[tbl_CustomerFinallSaleInvoices]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_CustomerFinallSaleInvoices_dbo.tbl_CurrencyTypes_CurrencyTypeID] FOREIGN KEY([CurrencyTypeID])
REFERENCES [dbo].[tbl_CurrencyTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_CustomerFinallSaleInvoices] CHECK CONSTRAINT [FK_dbo.tbl_CustomerFinallSaleInvoices_dbo.tbl_CurrencyTypes_CurrencyTypeID]
GO
ALTER TABLE [dbo].[tbl_CustomerFinallSaleInvoices]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_CustomerFinallSaleInvoices_dbo.tbl_Customers_CustomersID] FOREIGN KEY([CustomersID])
REFERENCES [dbo].[tbl_Customers] ([ID])
GO
ALTER TABLE [dbo].[tbl_CustomerFinallSaleInvoices] CHECK CONSTRAINT [FK_dbo.tbl_CustomerFinallSaleInvoices_dbo.tbl_Customers_CustomersID]
GO
ALTER TABLE [dbo].[tbl_CustomerFinallSaleInvoices]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_CustomerFinallSaleInvoices_dbo.tbl_EquipmentList_EquipmentsID] FOREIGN KEY([EquipmentsID])
REFERENCES [dbo].[tbl_EquipmentList] ([ID])
GO
ALTER TABLE [dbo].[tbl_CustomerFinallSaleInvoices] CHECK CONSTRAINT [FK_dbo.tbl_CustomerFinallSaleInvoices_dbo.tbl_EquipmentList_EquipmentsID]
GO
ALTER TABLE [dbo].[tbl_CustomerPreSaleInvoices]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_CustomerPreSaleInvoices_dbo.tbl_CurrencyTypes_CurrencyTypeID] FOREIGN KEY([CurrencyTypeID])
REFERENCES [dbo].[tbl_CurrencyTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_CustomerPreSaleInvoices] CHECK CONSTRAINT [FK_dbo.tbl_CustomerPreSaleInvoices_dbo.tbl_CurrencyTypes_CurrencyTypeID]
GO
ALTER TABLE [dbo].[tbl_CustomerPreSaleInvoices]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_CustomerPreSaleInvoices_dbo.tbl_Customers_CustomersID] FOREIGN KEY([CustomersID])
REFERENCES [dbo].[tbl_Customers] ([ID])
GO
ALTER TABLE [dbo].[tbl_CustomerPreSaleInvoices] CHECK CONSTRAINT [FK_dbo.tbl_CustomerPreSaleInvoices_dbo.tbl_Customers_CustomersID]
GO
ALTER TABLE [dbo].[tbl_CustomerPreSaleInvoices]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_CustomerPreSaleInvoices_dbo.tbl_EquipmentList_EquipmentsID] FOREIGN KEY([EquipmentsID])
REFERENCES [dbo].[tbl_EquipmentList] ([ID])
GO
ALTER TABLE [dbo].[tbl_CustomerPreSaleInvoices] CHECK CONSTRAINT [FK_dbo.tbl_CustomerPreSaleInvoices_dbo.tbl_EquipmentList_EquipmentsID]
GO
ALTER TABLE [dbo].[tbl_CustomerRequests]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_CustomerRequests_dbo.tbl_CurrencyTypes_CurrencyTypeID] FOREIGN KEY([CurrencyTypeID])
REFERENCES [dbo].[tbl_CurrencyTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_CustomerRequests] CHECK CONSTRAINT [FK_dbo.tbl_CustomerRequests_dbo.tbl_CurrencyTypes_CurrencyTypeID]
GO
ALTER TABLE [dbo].[tbl_CustomerRequests]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_CustomerRequests_dbo.tbl_Customers_CustomersID] FOREIGN KEY([CustomersID])
REFERENCES [dbo].[tbl_Customers] ([ID])
GO
ALTER TABLE [dbo].[tbl_CustomerRequests] CHECK CONSTRAINT [FK_dbo.tbl_CustomerRequests_dbo.tbl_Customers_CustomersID]
GO
ALTER TABLE [dbo].[tbl_CustomerRequests]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_CustomerRequests_dbo.tbl_EquipmentList_EquipmentsID] FOREIGN KEY([EquipmentsID])
REFERENCES [dbo].[tbl_EquipmentList] ([ID])
GO
ALTER TABLE [dbo].[tbl_CustomerRequests] CHECK CONSTRAINT [FK_dbo.tbl_CustomerRequests_dbo.tbl_EquipmentList_EquipmentsID]
GO
ALTER TABLE [dbo].[tbl_Customers]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Customers_dbo.tbl_Cities_CitiesId] FOREIGN KEY([CitiesId])
REFERENCES [dbo].[tbl_Cities] ([ID])
GO
ALTER TABLE [dbo].[tbl_Customers] CHECK CONSTRAINT [FK_dbo.tbl_Customers_dbo.tbl_Cities_CitiesId]
GO
ALTER TABLE [dbo].[tbl_CylinderDetails]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_CylinderDetails_dbo.tbl_TankConstractors_ConstractorId] FOREIGN KEY([ConstractorId])
REFERENCES [dbo].[tbl_TankConstractors] ([ID])
GO
ALTER TABLE [dbo].[tbl_CylinderDetails] CHECK CONSTRAINT [FK_dbo.tbl_CylinderDetails_dbo.tbl_TankConstractors_ConstractorId]
GO
ALTER TABLE [dbo].[tbl_Deposits]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Deposits_dbo.tbl_Workshops_WorkshopsID] FOREIGN KEY([WorkshopsID])
REFERENCES [dbo].[tbl_Workshops] ([ID])
GO
ALTER TABLE [dbo].[tbl_Deposits] CHECK CONSTRAINT [FK_dbo.tbl_Deposits_dbo.tbl_Workshops_WorkshopsID]
GO
ALTER TABLE [dbo].[tbl_DivisionPlanBOMs]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_DivisionPlanBOMs_dbo.tbl_BOMs_BOMID] FOREIGN KEY([BOMID])
REFERENCES [dbo].[tbl_BOMs] ([ID])
GO
ALTER TABLE [dbo].[tbl_DivisionPlanBOMs] CHECK CONSTRAINT [FK_dbo.tbl_DivisionPlanBOMs_dbo.tbl_BOMs_BOMID]
GO
ALTER TABLE [dbo].[tbl_DivisionPlanBOMs]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_DivisionPlanBOMs_dbo.tbl_DivisionPlans_DivisionPlanID] FOREIGN KEY([DivisionPlanID])
REFERENCES [dbo].[tbl_DivisionPlans] ([ID])
GO
ALTER TABLE [dbo].[tbl_DivisionPlanBOMs] CHECK CONSTRAINT [FK_dbo.tbl_DivisionPlanBOMs_dbo.tbl_DivisionPlans_DivisionPlanID]
GO
ALTER TABLE [dbo].[tbl_DivisionPlanBOMs]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_DivisionPlanBOMs_dbo.tbl_GenerationofRegulators_GenarationID] FOREIGN KEY([GenarationID])
REFERENCES [dbo].[tbl_GenerationofRegulators] ([ID])
GO
ALTER TABLE [dbo].[tbl_DivisionPlanBOMs] CHECK CONSTRAINT [FK_dbo.tbl_DivisionPlanBOMs_dbo.tbl_GenerationofRegulators_GenarationID]
GO
ALTER TABLE [dbo].[tbl_DivisionPlanBOMs]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_DivisionPlanBOMs_dbo.tbl_RegistrationTypes_RegistrationTypeID] FOREIGN KEY([RegistrationTypeID])
REFERENCES [dbo].[tbl_RegistrationTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_DivisionPlanBOMs] CHECK CONSTRAINT [FK_dbo.tbl_DivisionPlanBOMs_dbo.tbl_RegistrationTypes_RegistrationTypeID]
GO
ALTER TABLE [dbo].[tbl_DivisionPlans]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_DivisionPlans_dbo.tbl_Workshops_WorkshopID] FOREIGN KEY([WorkshopID])
REFERENCES [dbo].[tbl_Workshops] ([ID])
GO
ALTER TABLE [dbo].[tbl_DivisionPlans] CHECK CONSTRAINT [FK_dbo.tbl_DivisionPlans_dbo.tbl_Workshops_WorkshopID]
GO
ALTER TABLE [dbo].[tbl_DocHistories]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_DocHistories_dbo.Products_ProductsID] FOREIGN KEY([ProductsID])
REFERENCES [dbo].[Products] ([ID])
GO
ALTER TABLE [dbo].[tbl_DocHistories] CHECK CONSTRAINT [FK_dbo.tbl_DocHistories_dbo.Products_ProductsID]
GO
ALTER TABLE [dbo].[tbl_DocHistories]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_DocHistories_dbo.tbl_Documents_DocumentsID] FOREIGN KEY([DocumentsID])
REFERENCES [dbo].[tbl_Documents] ([ID])
GO
ALTER TABLE [dbo].[tbl_DocHistories] CHECK CONSTRAINT [FK_dbo.tbl_DocHistories_dbo.tbl_Documents_DocumentsID]
GO
ALTER TABLE [dbo].[tbl_Documents]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Documents_dbo.tbl_DocTypes_DocTypeID] FOREIGN KEY([DocTypeID])
REFERENCES [dbo].[tbl_DocTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_Documents] CHECK CONSTRAINT [FK_dbo.tbl_Documents_dbo.tbl_DocTypes_DocTypeID]
GO
ALTER TABLE [dbo].[tbl_Educations]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Educations_dbo.tbl_VehicleTypes_VehicleTypeId] FOREIGN KEY([VehicleTypeId])
REFERENCES [dbo].[tbl_VehicleTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_Educations] CHECK CONSTRAINT [FK_dbo.tbl_Educations_dbo.tbl_VehicleTypes_VehicleTypeId]
GO
ALTER TABLE [dbo].[tbl_EquipmentList]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_EquipmentList_dbo.tbl_EquipmentList_Pid] FOREIGN KEY([Pid])
REFERENCES [dbo].[tbl_EquipmentList] ([ID])
GO
ALTER TABLE [dbo].[tbl_EquipmentList] CHECK CONSTRAINT [FK_dbo.tbl_EquipmentList_dbo.tbl_EquipmentList_Pid]
GO
ALTER TABLE [dbo].[tbl_EquipmentList]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_EquipmentList_dbo.tbl_RegistrationTypes_RegistrationTypeID] FOREIGN KEY([RegistrationTypeID])
REFERENCES [dbo].[tbl_RegistrationTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_EquipmentList] CHECK CONSTRAINT [FK_dbo.tbl_EquipmentList_dbo.tbl_RegistrationTypes_RegistrationTypeID]
GO
ALTER TABLE [dbo].[tbl_FinallFreeSaleInvoices]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_FinallFreeSaleInvoices_dbo.tbl_CurrencyTypes_CurrencyTypeID] FOREIGN KEY([CurrencyTypeID])
REFERENCES [dbo].[tbl_CurrencyTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_FinallFreeSaleInvoices] CHECK CONSTRAINT [FK_dbo.tbl_FinallFreeSaleInvoices_dbo.tbl_CurrencyTypes_CurrencyTypeID]
GO
ALTER TABLE [dbo].[tbl_FinallFreeSaleInvoices]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_FinallFreeSaleInvoices_dbo.tbl_Customers_CustomersID] FOREIGN KEY([CustomersID])
REFERENCES [dbo].[tbl_Customers] ([ID])
GO
ALTER TABLE [dbo].[tbl_FinallFreeSaleInvoices] CHECK CONSTRAINT [FK_dbo.tbl_FinallFreeSaleInvoices_dbo.tbl_Customers_CustomersID]
GO
ALTER TABLE [dbo].[tbl_FinallFreeSaleInvoices]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_FinallFreeSaleInvoices_dbo.tbl_EquipmentList_EquipmentsID] FOREIGN KEY([EquipmentsID])
REFERENCES [dbo].[tbl_EquipmentList] ([ID])
GO
ALTER TABLE [dbo].[tbl_FinallFreeSaleInvoices] CHECK CONSTRAINT [FK_dbo.tbl_FinallFreeSaleInvoices_dbo.tbl_EquipmentList_EquipmentsID]
GO
ALTER TABLE [dbo].[tbl_FinallFreeSaleInvoices]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_FinallFreeSaleInvoices_dbo.tbl_Workshops_WorkshopsID] FOREIGN KEY([WorkshopsID])
REFERENCES [dbo].[tbl_Workshops] ([ID])
GO
ALTER TABLE [dbo].[tbl_FinallFreeSaleInvoices] CHECK CONSTRAINT [FK_dbo.tbl_FinallFreeSaleInvoices_dbo.tbl_Workshops_WorkshopsID]
GO
ALTER TABLE [dbo].[tbl_FinancialPayments]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_FinancialPayments_dbo.tbl_FinancialDescs_FinancialDescID] FOREIGN KEY([FinancialDescID])
REFERENCES [dbo].[tbl_FinancialDescs] ([ID])
GO
ALTER TABLE [dbo].[tbl_FinancialPayments] CHECK CONSTRAINT [FK_dbo.tbl_FinancialPayments_dbo.tbl_FinancialDescs_FinancialDescID]
GO
ALTER TABLE [dbo].[tbl_FinancialPayments]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_FinancialPayments_dbo.tbl_Workshops_WorkshopID] FOREIGN KEY([WorkshopID])
REFERENCES [dbo].[tbl_Workshops] ([ID])
GO
ALTER TABLE [dbo].[tbl_FinancialPayments] CHECK CONSTRAINT [FK_dbo.tbl_FinancialPayments_dbo.tbl_Workshops_WorkshopID]
GO
ALTER TABLE [dbo].[tbl_FreeSaleInvoices]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_FreeSaleInvoices_dbo.tbl_CurrencyTypes_CurrencyTypeID] FOREIGN KEY([CurrencyTypeID])
REFERENCES [dbo].[tbl_CurrencyTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_FreeSaleInvoices] CHECK CONSTRAINT [FK_dbo.tbl_FreeSaleInvoices_dbo.tbl_CurrencyTypes_CurrencyTypeID]
GO
ALTER TABLE [dbo].[tbl_FreeSaleInvoices]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_FreeSaleInvoices_dbo.tbl_Customers_CustomersID] FOREIGN KEY([CustomersID])
REFERENCES [dbo].[tbl_Customers] ([ID])
GO
ALTER TABLE [dbo].[tbl_FreeSaleInvoices] CHECK CONSTRAINT [FK_dbo.tbl_FreeSaleInvoices_dbo.tbl_Customers_CustomersID]
GO
ALTER TABLE [dbo].[tbl_FreeSaleInvoices]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_FreeSaleInvoices_dbo.tbl_EquipmentList_EquipmentsID] FOREIGN KEY([EquipmentsID])
REFERENCES [dbo].[tbl_EquipmentList] ([ID])
GO
ALTER TABLE [dbo].[tbl_FreeSaleInvoices] CHECK CONSTRAINT [FK_dbo.tbl_FreeSaleInvoices_dbo.tbl_EquipmentList_EquipmentsID]
GO
ALTER TABLE [dbo].[tbl_FreeSaleInvoices]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_FreeSaleInvoices_dbo.tbl_Workshops_WorkshopsID] FOREIGN KEY([WorkshopsID])
REFERENCES [dbo].[tbl_Workshops] ([ID])
GO
ALTER TABLE [dbo].[tbl_FreeSaleInvoices] CHECK CONSTRAINT [FK_dbo.tbl_FreeSaleInvoices_dbo.tbl_Workshops_WorkshopsID]
GO
ALTER TABLE [dbo].[tbl_FreeSaleRemittanceDetails]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_FreeSaleRemittanceDetails_dbo.tbl_FreeSaleRemittances_RemittancesID] FOREIGN KEY([RemittancesID])
REFERENCES [dbo].[tbl_FreeSaleRemittances] ([ID])
GO
ALTER TABLE [dbo].[tbl_FreeSaleRemittanceDetails] CHECK CONSTRAINT [FK_dbo.tbl_FreeSaleRemittanceDetails_dbo.tbl_FreeSaleRemittances_RemittancesID]
GO
ALTER TABLE [dbo].[tbl_InstructionForms]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_InstructionForms_dbo.tbl_Instructions_InstructionID] FOREIGN KEY([InstructionID])
REFERENCES [dbo].[tbl_Instructions] ([ID])
GO
ALTER TABLE [dbo].[tbl_InstructionForms] CHECK CONSTRAINT [FK_dbo.tbl_InstructionForms_dbo.tbl_Instructions_InstructionID]
GO
ALTER TABLE [dbo].[tbl_Insurances]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Insurances_dbo.tbl_VehicleRegistrations_VehicleRegistrationID] FOREIGN KEY([VehicleRegistrationID])
REFERENCES [dbo].[tbl_VehicleRegistrations] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbl_Insurances] CHECK CONSTRAINT [FK_dbo.tbl_Insurances_dbo.tbl_VehicleRegistrations_VehicleRegistrationID]
GO
ALTER TABLE [dbo].[tbl_Invoices]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Invoices_dbo.tbl_CurrencyTypes_CurrencyTypeID] FOREIGN KEY([CurrencyTypeID])
REFERENCES [dbo].[tbl_CurrencyTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_Invoices] CHECK CONSTRAINT [FK_dbo.tbl_Invoices_dbo.tbl_CurrencyTypes_CurrencyTypeID]
GO
ALTER TABLE [dbo].[tbl_Invoices]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Invoices_dbo.tbl_EquipmentList_EquipmentsID] FOREIGN KEY([EquipmentsID])
REFERENCES [dbo].[tbl_EquipmentList] ([ID])
GO
ALTER TABLE [dbo].[tbl_Invoices] CHECK CONSTRAINT [FK_dbo.tbl_Invoices_dbo.tbl_EquipmentList_EquipmentsID]
GO
ALTER TABLE [dbo].[tbl_Invoices]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Invoices_dbo.tbl_RegistrationTypes_RegistrationTypeID] FOREIGN KEY([RegistrationTypeID])
REFERENCES [dbo].[tbl_RegistrationTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_Invoices] CHECK CONSTRAINT [FK_dbo.tbl_Invoices_dbo.tbl_RegistrationTypes_RegistrationTypeID]
GO
ALTER TABLE [dbo].[tbl_Invoices]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Invoices_dbo.tbl_VehicleRegistrations_OwnersID] FOREIGN KEY([OwnersID])
REFERENCES [dbo].[tbl_VehicleRegistrations] ([ID])
GO
ALTER TABLE [dbo].[tbl_Invoices] CHECK CONSTRAINT [FK_dbo.tbl_Invoices_dbo.tbl_VehicleRegistrations_OwnersID]
GO
ALTER TABLE [dbo].[tbl_Invoices]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Invoices_dbo.tbl_Workshops_WorkshopsID] FOREIGN KEY([WorkshopsID])
REFERENCES [dbo].[tbl_Workshops] ([ID])
GO
ALTER TABLE [dbo].[tbl_Invoices] CHECK CONSTRAINT [FK_dbo.tbl_Invoices_dbo.tbl_Workshops_WorkshopsID]
GO
ALTER TABLE [dbo].[tbl_InvoicesDamages]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_InvoicesDamages_dbo.tbl_CurrencyTypes_CurrencyTypeID] FOREIGN KEY([CurrencyTypeID])
REFERENCES [dbo].[tbl_CurrencyTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_InvoicesDamages] CHECK CONSTRAINT [FK_dbo.tbl_InvoicesDamages_dbo.tbl_CurrencyTypes_CurrencyTypeID]
GO
ALTER TABLE [dbo].[tbl_InvoicesDamages]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_InvoicesDamages_dbo.tbl_EquipmentList_EquipmentsID] FOREIGN KEY([EquipmentsID])
REFERENCES [dbo].[tbl_EquipmentList] ([ID])
GO
ALTER TABLE [dbo].[tbl_InvoicesDamages] CHECK CONSTRAINT [FK_dbo.tbl_InvoicesDamages_dbo.tbl_EquipmentList_EquipmentsID]
GO
ALTER TABLE [dbo].[tbl_InvoicesDamages]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_InvoicesDamages_dbo.tbl_VehicleTypes_VehicleTypesID] FOREIGN KEY([VehicleTypesID])
REFERENCES [dbo].[tbl_VehicleTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_InvoicesDamages] CHECK CONSTRAINT [FK_dbo.tbl_InvoicesDamages_dbo.tbl_VehicleTypes_VehicleTypesID]
GO
ALTER TABLE [dbo].[tbl_InvoicesDamages]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_InvoicesDamages_dbo.tbl_Workshops_WorkshopsID] FOREIGN KEY([WorkshopsID])
REFERENCES [dbo].[tbl_Workshops] ([ID])
GO
ALTER TABLE [dbo].[tbl_InvoicesDamages] CHECK CONSTRAINT [FK_dbo.tbl_InvoicesDamages_dbo.tbl_Workshops_WorkshopsID]
GO
ALTER TABLE [dbo].[tbl_InvoicesFapa]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_InvoicesFapa_dbo.tbl_CurrencyTypes_CurrencyTypeID] FOREIGN KEY([CurrencyTypeID])
REFERENCES [dbo].[tbl_CurrencyTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_InvoicesFapa] CHECK CONSTRAINT [FK_dbo.tbl_InvoicesFapa_dbo.tbl_CurrencyTypes_CurrencyTypeID]
GO
ALTER TABLE [dbo].[tbl_InvoicesFapa]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_InvoicesFapa_dbo.tbl_EquipmentList_EquipmentsID] FOREIGN KEY([EquipmentsID])
REFERENCES [dbo].[tbl_EquipmentList] ([ID])
GO
ALTER TABLE [dbo].[tbl_InvoicesFapa] CHECK CONSTRAINT [FK_dbo.tbl_InvoicesFapa_dbo.tbl_EquipmentList_EquipmentsID]
GO
ALTER TABLE [dbo].[tbl_InvoicesFapa]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_InvoicesFapa_dbo.tbl_VehicleTypes_VehicleTypesID] FOREIGN KEY([VehicleTypesID])
REFERENCES [dbo].[tbl_VehicleTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_InvoicesFapa] CHECK CONSTRAINT [FK_dbo.tbl_InvoicesFapa_dbo.tbl_VehicleTypes_VehicleTypesID]
GO
ALTER TABLE [dbo].[tbl_InvoicesFapa]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_InvoicesFapa_dbo.tbl_Workshops_WorkshopsID] FOREIGN KEY([WorkshopsID])
REFERENCES [dbo].[tbl_Workshops] ([ID])
GO
ALTER TABLE [dbo].[tbl_InvoicesFapa] CHECK CONSTRAINT [FK_dbo.tbl_InvoicesFapa_dbo.tbl_Workshops_WorkshopsID]
GO
ALTER TABLE [dbo].[tbl_InvoicesFapa_DamagesCylinder]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_InvoicesFapa_DamagesCylinder_dbo.tbl_CurrencyTypes_CurrencyTypeID] FOREIGN KEY([CurrencyTypeID])
REFERENCES [dbo].[tbl_CurrencyTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_InvoicesFapa_DamagesCylinder] CHECK CONSTRAINT [FK_dbo.tbl_InvoicesFapa_DamagesCylinder_dbo.tbl_CurrencyTypes_CurrencyTypeID]
GO
ALTER TABLE [dbo].[tbl_InvoicesFapa_DamagesCylinder]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_InvoicesFapa_DamagesCylinder_dbo.tbl_EquipmentList_EquipmentsID] FOREIGN KEY([EquipmentsID])
REFERENCES [dbo].[tbl_EquipmentList] ([ID])
GO
ALTER TABLE [dbo].[tbl_InvoicesFapa_DamagesCylinder] CHECK CONSTRAINT [FK_dbo.tbl_InvoicesFapa_DamagesCylinder_dbo.tbl_EquipmentList_EquipmentsID]
GO
ALTER TABLE [dbo].[tbl_InvoicesFapa_DamagesCylinder]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_InvoicesFapa_DamagesCylinder_dbo.tbl_VehicleTypes_VehicleTypesID] FOREIGN KEY([VehicleTypesID])
REFERENCES [dbo].[tbl_VehicleTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_InvoicesFapa_DamagesCylinder] CHECK CONSTRAINT [FK_dbo.tbl_InvoicesFapa_DamagesCylinder_dbo.tbl_VehicleTypes_VehicleTypesID]
GO
ALTER TABLE [dbo].[tbl_InvoicesFapa_DamagesCylinder]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_InvoicesFapa_DamagesCylinder_dbo.tbl_Workshops_WorkshopsID] FOREIGN KEY([WorkshopsID])
REFERENCES [dbo].[tbl_Workshops] ([ID])
GO
ALTER TABLE [dbo].[tbl_InvoicesFapa_DamagesCylinder] CHECK CONSTRAINT [FK_dbo.tbl_InvoicesFapa_DamagesCylinder_dbo.tbl_Workshops_WorkshopsID]
GO
ALTER TABLE [dbo].[tbl_InvoicesValveDamages]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_InvoicesValveDamages_dbo.tbl_CurrencyTypes_CurrencyTypeID] FOREIGN KEY([CurrencyTypeID])
REFERENCES [dbo].[tbl_CurrencyTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_InvoicesValveDamages] CHECK CONSTRAINT [FK_dbo.tbl_InvoicesValveDamages_dbo.tbl_CurrencyTypes_CurrencyTypeID]
GO
ALTER TABLE [dbo].[tbl_InvoicesValveDamages]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_InvoicesValveDamages_dbo.tbl_EquipmentList_EquipmentsID] FOREIGN KEY([EquipmentsID])
REFERENCES [dbo].[tbl_EquipmentList] ([ID])
GO
ALTER TABLE [dbo].[tbl_InvoicesValveDamages] CHECK CONSTRAINT [FK_dbo.tbl_InvoicesValveDamages_dbo.tbl_EquipmentList_EquipmentsID]
GO
ALTER TABLE [dbo].[tbl_InvoicesValveDamages]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_InvoicesValveDamages_dbo.tbl_VehicleTypes_VehicleTypesID] FOREIGN KEY([VehicleTypesID])
REFERENCES [dbo].[tbl_VehicleTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_InvoicesValveDamages] CHECK CONSTRAINT [FK_dbo.tbl_InvoicesValveDamages_dbo.tbl_VehicleTypes_VehicleTypesID]
GO
ALTER TABLE [dbo].[tbl_InvoicesValveDamages]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_InvoicesValveDamages_dbo.tbl_Workshops_WorkshopsID] FOREIGN KEY([WorkshopsID])
REFERENCES [dbo].[tbl_Workshops] ([ID])
GO
ALTER TABLE [dbo].[tbl_InvoicesValveDamages] CHECK CONSTRAINT [FK_dbo.tbl_InvoicesValveDamages_dbo.tbl_Workshops_WorkshopsID]
GO
ALTER TABLE [dbo].[tbl_KitDivisionPlans]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_KitDivisionPlans_dbo.tbl_DivisionPlans_DivisionPlanID] FOREIGN KEY([DivisionPlanID])
REFERENCES [dbo].[tbl_DivisionPlans] ([ID])
GO
ALTER TABLE [dbo].[tbl_KitDivisionPlans] CHECK CONSTRAINT [FK_dbo.tbl_KitDivisionPlans_dbo.tbl_DivisionPlans_DivisionPlanID]
GO
ALTER TABLE [dbo].[tbl_KitDivisionPlans]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_KitDivisionPlans_dbo.tbl_GenerationofRegulators_GenarationID] FOREIGN KEY([GenarationID])
REFERENCES [dbo].[tbl_GenerationofRegulators] ([ID])
GO
ALTER TABLE [dbo].[tbl_KitDivisionPlans] CHECK CONSTRAINT [FK_dbo.tbl_KitDivisionPlans_dbo.tbl_GenerationofRegulators_GenarationID]
GO
ALTER TABLE [dbo].[tbl_KitDivisionPlans]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_KitDivisionPlans_dbo.tbl_RegistrationTypes_RegistrationTypeID] FOREIGN KEY([RegistrationTypeID])
REFERENCES [dbo].[tbl_RegistrationTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_KitDivisionPlans] CHECK CONSTRAINT [FK_dbo.tbl_KitDivisionPlans_dbo.tbl_RegistrationTypes_RegistrationTypeID]
GO
ALTER TABLE [dbo].[tbl_KitDivisionPlans]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_KitDivisionPlans_dbo.tbl_VehicleTypes_VehicleTypeID] FOREIGN KEY([VehicleTypeID])
REFERENCES [dbo].[tbl_VehicleTypes] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbl_KitDivisionPlans] CHECK CONSTRAINT [FK_dbo.tbl_KitDivisionPlans_dbo.tbl_VehicleTypes_VehicleTypeID]
GO
ALTER TABLE [dbo].[tbl_MessageForwards]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_MessageForwards_dbo.tbl_Messages_MessageID] FOREIGN KEY([MessageID])
REFERENCES [dbo].[tbl_Messages] ([ID])
GO
ALTER TABLE [dbo].[tbl_MessageForwards] CHECK CONSTRAINT [FK_dbo.tbl_MessageForwards_dbo.tbl_Messages_MessageID]
GO
ALTER TABLE [dbo].[tbl_MessageForwards]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_MessageForwards_dbo.Users_SenderID] FOREIGN KEY([SenderID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[tbl_MessageForwards] CHECK CONSTRAINT [FK_dbo.tbl_MessageForwards_dbo.Users_SenderID]
GO
ALTER TABLE [dbo].[tbl_MessageReplies]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_MessageReplies_dbo.tbl_Messages_MessageID] FOREIGN KEY([MessageID])
REFERENCES [dbo].[tbl_Messages] ([ID])
GO
ALTER TABLE [dbo].[tbl_MessageReplies] CHECK CONSTRAINT [FK_dbo.tbl_MessageReplies_dbo.tbl_Messages_MessageID]
GO
ALTER TABLE [dbo].[tbl_MessageReplies]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_MessageReplies_dbo.Users_SenderID] FOREIGN KEY([SenderID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[tbl_MessageReplies] CHECK CONSTRAINT [FK_dbo.tbl_MessageReplies_dbo.Users_SenderID]
GO
ALTER TABLE [dbo].[tbl_Messages]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Messages_dbo.Messages_MessageID] FOREIGN KEY([MessageID])
REFERENCES [dbo].[tbl_Messages] ([ID])
GO
ALTER TABLE [dbo].[tbl_Messages] CHECK CONSTRAINT [FK_dbo.tbl_Messages_dbo.Messages_MessageID]
GO
ALTER TABLE [dbo].[tbl_Messages]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Messages_dbo.tbl_Workshops_WorkshopID] FOREIGN KEY([WorkshopID])
REFERENCES [dbo].[tbl_Workshops] ([ID])
GO
ALTER TABLE [dbo].[tbl_Messages] CHECK CONSTRAINT [FK_dbo.tbl_Messages_dbo.tbl_Workshops_WorkshopID]
GO
ALTER TABLE [dbo].[tbl_Messages]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Messages_dbo.Users_SenderID] FOREIGN KEY([SenderID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[tbl_Messages] CHECK CONSTRAINT [FK_dbo.tbl_Messages_dbo.Users_SenderID]
GO
ALTER TABLE [dbo].[tbl_OfferedPrices]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_OfferedPrices_dbo.tbl_Workshops_WorkshopID] FOREIGN KEY([WorkshopID])
REFERENCES [dbo].[tbl_Workshops] ([ID])
GO
ALTER TABLE [dbo].[tbl_OfferedPrices] CHECK CONSTRAINT [FK_dbo.tbl_OfferedPrices_dbo.tbl_Workshops_WorkshopID]
GO
ALTER TABLE [dbo].[tbl_Operators]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Operators_dbo.tbl_Workshops_WorkshopID] FOREIGN KEY([WorkshopID])
REFERENCES [dbo].[tbl_Workshops] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbl_Operators] CHECK CONSTRAINT [FK_dbo.tbl_Operators_dbo.tbl_Workshops_WorkshopID]
GO
ALTER TABLE [dbo].[tbl_Organizations]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Organizations_dbo.tbl_Cities_CityID] FOREIGN KEY([CityID])
REFERENCES [dbo].[tbl_Cities] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbl_Organizations] CHECK CONSTRAINT [FK_dbo.tbl_Organizations_dbo.tbl_Cities_CityID]
GO
ALTER TABLE [dbo].[tbl_OtherThingsDivisionPlans]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_OtherThingsDivisionPlans_dbo.tbl_DivisionPlans_DivisionPlanID] FOREIGN KEY([DivisionPlanID])
REFERENCES [dbo].[tbl_DivisionPlans] ([ID])
GO
ALTER TABLE [dbo].[tbl_OtherThingsDivisionPlans] CHECK CONSTRAINT [FK_dbo.tbl_OtherThingsDivisionPlans_dbo.tbl_DivisionPlans_DivisionPlanID]
GO
ALTER TABLE [dbo].[tbl_OtherThingsDivisionPlans]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_OtherThingsDivisionPlans_dbo.tbl_GenerationofRegulators_GenarationID] FOREIGN KEY([GenarationID])
REFERENCES [dbo].[tbl_GenerationofRegulators] ([ID])
GO
ALTER TABLE [dbo].[tbl_OtherThingsDivisionPlans] CHECK CONSTRAINT [FK_dbo.tbl_OtherThingsDivisionPlans_dbo.tbl_GenerationofRegulators_GenarationID]
GO
ALTER TABLE [dbo].[tbl_OtherThingsDivisionPlans]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_OtherThingsDivisionPlans_dbo.tbl_Otherthings_DiThingsID] FOREIGN KEY([DiThingsID])
REFERENCES [dbo].[tbl_Otherthings] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbl_OtherThingsDivisionPlans] CHECK CONSTRAINT [FK_dbo.tbl_OtherThingsDivisionPlans_dbo.tbl_Otherthings_DiThingsID]
GO
ALTER TABLE [dbo].[tbl_OtherThingsDivisionPlans]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_OtherThingsDivisionPlans_dbo.tbl_RegistrationTypes_RegistrationTypeID] FOREIGN KEY([RegistrationTypeID])
REFERENCES [dbo].[tbl_RegistrationTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_OtherThingsDivisionPlans] CHECK CONSTRAINT [FK_dbo.tbl_OtherThingsDivisionPlans_dbo.tbl_RegistrationTypes_RegistrationTypeID]
GO
ALTER TABLE [dbo].[tbl_ProCategories]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_ProCategories_dbo.tbl_ProTypes_ProTypeID] FOREIGN KEY([ProTypeID])
REFERENCES [dbo].[tbl_ProTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_ProCategories] CHECK CONSTRAINT [FK_dbo.tbl_ProCategories_dbo.tbl_ProTypes_ProTypeID]
GO
ALTER TABLE [dbo].[tbl_Process]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Process_dbo.tbl_ProCategories_ProCategoryID] FOREIGN KEY([ProCategoryID])
REFERENCES [dbo].[tbl_ProCategories] ([ID])
GO
ALTER TABLE [dbo].[tbl_Process] CHECK CONSTRAINT [FK_dbo.tbl_Process_dbo.tbl_ProCategories_ProCategoryID]
GO
ALTER TABLE [dbo].[tbl_ProHistories]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_ProHistories_dbo.Products_ProductsID] FOREIGN KEY([ProductsID])
REFERENCES [dbo].[Products] ([ID])
GO
ALTER TABLE [dbo].[tbl_ProHistories] CHECK CONSTRAINT [FK_dbo.tbl_ProHistories_dbo.Products_ProductsID]
GO
ALTER TABLE [dbo].[tbl_ProHistories]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_ProHistories_dbo.tbl_Process_ProcessID] FOREIGN KEY([ProcessID])
REFERENCES [dbo].[tbl_Process] ([ID])
GO
ALTER TABLE [dbo].[tbl_ProHistories] CHECK CONSTRAINT [FK_dbo.tbl_ProHistories_dbo.tbl_Process_ProcessID]
GO
ALTER TABLE [dbo].[tbl_RemittanceDetails]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_RemittanceDetails_dbo.tbl_Remittances_RemittancesID] FOREIGN KEY([RemittancesID])
REFERENCES [dbo].[tbl_Remittances] ([ID])
GO
ALTER TABLE [dbo].[tbl_RemittanceDetails] CHECK CONSTRAINT [FK_dbo.tbl_RemittanceDetails_dbo.tbl_Remittances_RemittancesID]
GO
ALTER TABLE [dbo].[tbl_Remittances]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Remittances_dbo.tbl_DivisionPlans_DivisionPlanID] FOREIGN KEY([DivisionPlanID])
REFERENCES [dbo].[tbl_DivisionPlans] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbl_Remittances] CHECK CONSTRAINT [FK_dbo.tbl_Remittances_dbo.tbl_DivisionPlans_DivisionPlanID]
GO
ALTER TABLE [dbo].[tbl_Remittances]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Remittances_dbo.Users_Creator] FOREIGN KEY([Creator])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[tbl_Remittances] CHECK CONSTRAINT [FK_dbo.tbl_Remittances_dbo.Users_Creator]
GO
ALTER TABLE [dbo].[tbl_ReplacementPlanPrice]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_ReplacementPlanPrice_dbo.tbl_EquipmentList_EquipmentTypeID] FOREIGN KEY([EquipmentTypeID])
REFERENCES [dbo].[tbl_EquipmentList] ([ID])
GO
ALTER TABLE [dbo].[tbl_ReplacementPlanPrice] CHECK CONSTRAINT [FK_dbo.tbl_ReplacementPlanPrice_dbo.tbl_EquipmentList_EquipmentTypeID]
GO
ALTER TABLE [dbo].[tbl_ReplacementPlanValvePrice]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_ReplacementPlanValvePrice_dbo.tbl_EquipmentList_EquipmentTypeID] FOREIGN KEY([EquipmentTypeID])
REFERENCES [dbo].[tbl_EquipmentList] ([ID])
GO
ALTER TABLE [dbo].[tbl_ReplacementPlanValvePrice] CHECK CONSTRAINT [FK_dbo.tbl_ReplacementPlanValvePrice_dbo.tbl_EquipmentList_EquipmentTypeID]
GO
ALTER TABLE [dbo].[tbl_RequestFreeSales]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_RequestFreeSales_dbo.tbl_CurrencyTypes_CurrencyTypeID] FOREIGN KEY([CurrencyTypeID])
REFERENCES [dbo].[tbl_CurrencyTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_RequestFreeSales] CHECK CONSTRAINT [FK_dbo.tbl_RequestFreeSales_dbo.tbl_CurrencyTypes_CurrencyTypeID]
GO
ALTER TABLE [dbo].[tbl_RequestFreeSales]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_RequestFreeSales_dbo.tbl_EquipmentList_EquipmentsID] FOREIGN KEY([EquipmentsID])
REFERENCES [dbo].[tbl_EquipmentList] ([ID])
GO
ALTER TABLE [dbo].[tbl_RequestFreeSales] CHECK CONSTRAINT [FK_dbo.tbl_RequestFreeSales_dbo.tbl_EquipmentList_EquipmentsID]
GO
ALTER TABLE [dbo].[tbl_RequestFreeSales]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_RequestFreeSales_dbo.tbl_RegistrationTypes_RegistrationTypeID] FOREIGN KEY([RegistrationTypeID])
REFERENCES [dbo].[tbl_RegistrationTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_RequestFreeSales] CHECK CONSTRAINT [FK_dbo.tbl_RequestFreeSales_dbo.tbl_RegistrationTypes_RegistrationTypeID]
GO
ALTER TABLE [dbo].[tbl_RequestFreeSales]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_RequestFreeSales_dbo.tbl_Workshops_WorkshopsID] FOREIGN KEY([WorkshopsID])
REFERENCES [dbo].[tbl_Workshops] ([ID])
GO
ALTER TABLE [dbo].[tbl_RequestFreeSales] CHECK CONSTRAINT [FK_dbo.tbl_RequestFreeSales_dbo.tbl_Workshops_WorkshopsID]
GO
ALTER TABLE [dbo].[tbl_ReturnofParts]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_ReturnofParts_dbo.tbl_EquipmentList_EquipmentID] FOREIGN KEY([EquipmentID])
REFERENCES [dbo].[tbl_EquipmentList] ([ID])
GO
ALTER TABLE [dbo].[tbl_ReturnofParts] CHECK CONSTRAINT [FK_dbo.tbl_ReturnofParts_dbo.tbl_EquipmentList_EquipmentID]
GO
ALTER TABLE [dbo].[tbl_ReturnofParts]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_ReturnofParts_dbo.tbl_VehicleTypes_VehicleTypeID] FOREIGN KEY([VehicleTypeID])
REFERENCES [dbo].[tbl_VehicleTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_ReturnofParts] CHECK CONSTRAINT [FK_dbo.tbl_ReturnofParts_dbo.tbl_VehicleTypes_VehicleTypeID]
GO
ALTER TABLE [dbo].[tbl_ReturnofParts]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_ReturnofParts_dbo.tbl_Workshops_WorkshopID] FOREIGN KEY([WorkshopID])
REFERENCES [dbo].[tbl_Workshops] ([ID])
GO
ALTER TABLE [dbo].[tbl_ReturnofParts] CHECK CONSTRAINT [FK_dbo.tbl_ReturnofParts_dbo.tbl_Workshops_WorkshopID]
GO
ALTER TABLE [dbo].[tbl_SideBarItems]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_SideBarItems_dbo.tbl_SideBarItems_parentId] FOREIGN KEY([parentId])
REFERENCES [dbo].[tbl_SideBarItems] ([ID])
GO
ALTER TABLE [dbo].[tbl_SideBarItems] CHECK CONSTRAINT [FK_dbo.tbl_SideBarItems_dbo.tbl_SideBarItems_parentId]
GO
ALTER TABLE [dbo].[tbl_TankBaseDivisionPlans]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_TankBaseDivisionPlans_dbo.tbl_DivisionPlans_DivisionPlanID] FOREIGN KEY([DivisionPlanID])
REFERENCES [dbo].[tbl_DivisionPlans] ([ID])
GO
ALTER TABLE [dbo].[tbl_TankBaseDivisionPlans] CHECK CONSTRAINT [FK_dbo.tbl_TankBaseDivisionPlans_dbo.tbl_DivisionPlans_DivisionPlanID]
GO
ALTER TABLE [dbo].[tbl_TankBaseDivisionPlans]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_TankBaseDivisionPlans_dbo.tbl_GenerationofRegulators_GenarationID] FOREIGN KEY([GenarationID])
REFERENCES [dbo].[tbl_GenerationofRegulators] ([ID])
GO
ALTER TABLE [dbo].[tbl_TankBaseDivisionPlans] CHECK CONSTRAINT [FK_dbo.tbl_TankBaseDivisionPlans_dbo.tbl_GenerationofRegulators_GenarationID]
GO
ALTER TABLE [dbo].[tbl_TankBaseDivisionPlans]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_TankBaseDivisionPlans_dbo.tbl_RegistrationTypes_RegistrationTypeID] FOREIGN KEY([RegistrationTypeID])
REFERENCES [dbo].[tbl_RegistrationTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_TankBaseDivisionPlans] CHECK CONSTRAINT [FK_dbo.tbl_TankBaseDivisionPlans_dbo.tbl_RegistrationTypes_RegistrationTypeID]
GO
ALTER TABLE [dbo].[tbl_TankBaseDivisionPlans]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_TankBaseDivisionPlans_dbo.tbl_TypeofTankBases_TypeofTankBaseID] FOREIGN KEY([TypeofTankBaseID])
REFERENCES [dbo].[tbl_TypeofTankBases] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbl_TankBaseDivisionPlans] CHECK CONSTRAINT [FK_dbo.tbl_TankBaseDivisionPlans_dbo.tbl_TypeofTankBases_TypeofTankBaseID]
GO
ALTER TABLE [dbo].[tbl_TankCoverDivisionPlans]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_TankCoverDivisionPlans_dbo.tbl_DivisionPlans_DivisionPlanID] FOREIGN KEY([DivisionPlanID])
REFERENCES [dbo].[tbl_DivisionPlans] ([ID])
GO
ALTER TABLE [dbo].[tbl_TankCoverDivisionPlans] CHECK CONSTRAINT [FK_dbo.tbl_TankCoverDivisionPlans_dbo.tbl_DivisionPlans_DivisionPlanID]
GO
ALTER TABLE [dbo].[tbl_TankCoverDivisionPlans]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_TankCoverDivisionPlans_dbo.tbl_GenerationofRegulators_GenarationID] FOREIGN KEY([GenarationID])
REFERENCES [dbo].[tbl_GenerationofRegulators] ([ID])
GO
ALTER TABLE [dbo].[tbl_TankCoverDivisionPlans] CHECK CONSTRAINT [FK_dbo.tbl_TankCoverDivisionPlans_dbo.tbl_GenerationofRegulators_GenarationID]
GO
ALTER TABLE [dbo].[tbl_TankCoverDivisionPlans]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_TankCoverDivisionPlans_dbo.tbl_RegistrationTypes_RegistrationTypeID] FOREIGN KEY([RegistrationTypeID])
REFERENCES [dbo].[tbl_RegistrationTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_TankCoverDivisionPlans] CHECK CONSTRAINT [FK_dbo.tbl_TankCoverDivisionPlans_dbo.tbl_RegistrationTypes_RegistrationTypeID]
GO
ALTER TABLE [dbo].[tbl_TankCoverDivisionPlans]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_TankCoverDivisionPlans_dbo.tbl_TypeofTankCovers_TypeofTankCoverID] FOREIGN KEY([TypeofTankCoverID])
REFERENCES [dbo].[tbl_TypeofTankCovers] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbl_TankCoverDivisionPlans] CHECK CONSTRAINT [FK_dbo.tbl_TankCoverDivisionPlans_dbo.tbl_TypeofTankCovers_TypeofTankCoverID]
GO
ALTER TABLE [dbo].[tbl_TankDivisionPlans]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_TankDivisionPlans_dbo.tbl_DivisionPlans_DivisionPlanID] FOREIGN KEY([DivisionPlanID])
REFERENCES [dbo].[tbl_DivisionPlans] ([ID])
GO
ALTER TABLE [dbo].[tbl_TankDivisionPlans] CHECK CONSTRAINT [FK_dbo.tbl_TankDivisionPlans_dbo.tbl_DivisionPlans_DivisionPlanID]
GO
ALTER TABLE [dbo].[tbl_TankDivisionPlans]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_TankDivisionPlans_dbo.tbl_GenerationofRegulators_GenarationID] FOREIGN KEY([GenarationID])
REFERENCES [dbo].[tbl_GenerationofRegulators] ([ID])
GO
ALTER TABLE [dbo].[tbl_TankDivisionPlans] CHECK CONSTRAINT [FK_dbo.tbl_TankDivisionPlans_dbo.tbl_GenerationofRegulators_GenarationID]
GO
ALTER TABLE [dbo].[tbl_TankDivisionPlans]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_TankDivisionPlans_dbo.tbl_RegistrationTypes_RegistrationTypeID] FOREIGN KEY([RegistrationTypeID])
REFERENCES [dbo].[tbl_RegistrationTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_TankDivisionPlans] CHECK CONSTRAINT [FK_dbo.tbl_TankDivisionPlans_dbo.tbl_RegistrationTypes_RegistrationTypeID]
GO
ALTER TABLE [dbo].[tbl_TankDivisionPlans]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_TankDivisionPlans_dbo.tbl_TankConstractors_TankConstractorID] FOREIGN KEY([TankConstractorID])
REFERENCES [dbo].[tbl_TankConstractors] ([ID])
GO
ALTER TABLE [dbo].[tbl_TankDivisionPlans] CHECK CONSTRAINT [FK_dbo.tbl_TankDivisionPlans_dbo.tbl_TankConstractors_TankConstractorID]
GO
ALTER TABLE [dbo].[tbl_TankDivisionPlans]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_TankDivisionPlans_dbo.tbl_TypeofTanks_TypeofTankID] FOREIGN KEY([TypeofTankID])
REFERENCES [dbo].[tbl_TypeofTanks] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbl_TankDivisionPlans] CHECK CONSTRAINT [FK_dbo.tbl_TankDivisionPlans_dbo.tbl_TypeofTanks_TypeofTankID]
GO
ALTER TABLE [dbo].[tbl_TypeofTankBases]  WITH CHECK ADD  CONSTRAINT [FK_tbl_TypeofTankBases_VehicleTypeId] FOREIGN KEY([VehicleTypeId])
REFERENCES [dbo].[tbl_VehicleTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_TypeofTankBases] CHECK CONSTRAINT [FK_tbl_TypeofTankBases_VehicleTypeId]
GO
ALTER TABLE [dbo].[tbl_TypeofTankCovers]  WITH CHECK ADD  CONSTRAINT [FK_tbl_TypeofTankCovers_dbo.tbl_VehicleTypes_VehicleTypeId]]] FOREIGN KEY([VehicleTypeId])
REFERENCES [dbo].[tbl_VehicleTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_TypeofTankCovers] CHECK CONSTRAINT [FK_tbl_TypeofTankCovers_dbo.tbl_VehicleTypes_VehicleTypeId]]]
GO
ALTER TABLE [dbo].[tbl_TypeofTanks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_TypeofTanks_dbo.tbl_VehicleTypes_VehicleTypeId] FOREIGN KEY([VehicleTypeId])
REFERENCES [dbo].[tbl_VehicleTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_TypeofTanks] CHECK CONSTRAINT [FK_dbo.tbl_TypeofTanks_dbo.tbl_VehicleTypes_VehicleTypeId]
GO
ALTER TABLE [dbo].[tbl_ValveDivisionPlans]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_ValveDivisionPlans_dbo.tbl_DivisionPlans_DivisionPlanID] FOREIGN KEY([DivisionPlanID])
REFERENCES [dbo].[tbl_DivisionPlans] ([ID])
GO
ALTER TABLE [dbo].[tbl_ValveDivisionPlans] CHECK CONSTRAINT [FK_dbo.tbl_ValveDivisionPlans_dbo.tbl_DivisionPlans_DivisionPlanID]
GO
ALTER TABLE [dbo].[tbl_ValveDivisionPlans]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_ValveDivisionPlans_dbo.tbl_GenerationofRegulators_GenarationID] FOREIGN KEY([GenarationID])
REFERENCES [dbo].[tbl_GenerationofRegulators] ([ID])
GO
ALTER TABLE [dbo].[tbl_ValveDivisionPlans] CHECK CONSTRAINT [FK_dbo.tbl_ValveDivisionPlans_dbo.tbl_GenerationofRegulators_GenarationID]
GO
ALTER TABLE [dbo].[tbl_ValveDivisionPlans]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_ValveDivisionPlans_dbo.tbl_RegistrationTypes_RegistrationTypeID] FOREIGN KEY([RegistrationTypeID])
REFERENCES [dbo].[tbl_RegistrationTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_ValveDivisionPlans] CHECK CONSTRAINT [FK_dbo.tbl_ValveDivisionPlans_dbo.tbl_RegistrationTypes_RegistrationTypeID]
GO
ALTER TABLE [dbo].[tbl_ValveDivisionPlans]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_ValveDivisionPlans_dbo.tbl_ValveConstractors_ValveConstractorID] FOREIGN KEY([ValveConstractorID])
REFERENCES [dbo].[tbl_ValveConstractors] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbl_ValveDivisionPlans] CHECK CONSTRAINT [FK_dbo.tbl_ValveDivisionPlans_dbo.tbl_ValveConstractors_ValveConstractorID]
GO
ALTER TABLE [dbo].[tbl_VehicleAttachments]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_VehicleAttachments_dbo.tbl_VehicleRegistrations_VehicleRegistrationID] FOREIGN KEY([VehicleRegistrationID])
REFERENCES [dbo].[tbl_VehicleRegistrations] ([ID])
GO
ALTER TABLE [dbo].[tbl_VehicleAttachments] CHECK CONSTRAINT [FK_dbo.tbl_VehicleAttachments_dbo.tbl_VehicleRegistrations_VehicleRegistrationID]
GO
ALTER TABLE [dbo].[tbl_VehicleInvoices]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_VehicleInvoices_dbo.tbl_VehicleRegistrations_VehicleRegistrationID] FOREIGN KEY([VehicleRegistrationID])
REFERENCES [dbo].[tbl_VehicleRegistrations] ([ID])
GO
ALTER TABLE [dbo].[tbl_VehicleInvoices] CHECK CONSTRAINT [FK_dbo.tbl_VehicleInvoices_dbo.tbl_VehicleRegistrations_VehicleRegistrationID]
GO
ALTER TABLE [dbo].[tbl_VehicleRegistrations]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_VehicleRegistrations_dbo.tbl_RegistrationTypes_RegistrationTypeID] FOREIGN KEY([RegistrationTypeID])
REFERENCES [dbo].[tbl_RegistrationTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_VehicleRegistrations] CHECK CONSTRAINT [FK_dbo.tbl_VehicleRegistrations_dbo.tbl_RegistrationTypes_RegistrationTypeID]
GO
ALTER TABLE [dbo].[tbl_VehicleRegistrations]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_VehicleRegistrations_dbo.tbl_VehicleTypes_VehicleTypeID] FOREIGN KEY([VehicleTypeID])
REFERENCES [dbo].[tbl_VehicleTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_VehicleRegistrations] CHECK CONSTRAINT [FK_dbo.tbl_VehicleRegistrations_dbo.tbl_VehicleTypes_VehicleTypeID]
GO
ALTER TABLE [dbo].[tbl_VehicleRegistrations]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_VehicleRegistrations_dbo.tbl_Workshops_WorkshopID] FOREIGN KEY([WorkshopID])
REFERENCES [dbo].[tbl_Workshops] ([ID])
GO
ALTER TABLE [dbo].[tbl_VehicleRegistrations] CHECK CONSTRAINT [FK_dbo.tbl_VehicleRegistrations_dbo.tbl_Workshops_WorkshopID]
GO
ALTER TABLE [dbo].[tbl_VehicleTanks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_VehicleTanks_dbo.tbl_CutofValveConstractors_CutofValveConstractorID] FOREIGN KEY([CutofValveConstractorID])
REFERENCES [dbo].[tbl_CutofValveConstractors] ([ID])
GO
ALTER TABLE [dbo].[tbl_VehicleTanks] CHECK CONSTRAINT [FK_dbo.tbl_VehicleTanks_dbo.tbl_CutofValveConstractors_CutofValveConstractorID]
GO
ALTER TABLE [dbo].[tbl_VehicleTanks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_VehicleTanks_dbo.tbl_FillingValveConstractors_FillingValveConstractorID] FOREIGN KEY([FillingValveConstractorID])
REFERENCES [dbo].[tbl_FillingValveConstractors] ([ID])
GO
ALTER TABLE [dbo].[tbl_VehicleTanks] CHECK CONSTRAINT [FK_dbo.tbl_VehicleTanks_dbo.tbl_FillingValveConstractors_FillingValveConstractorID]
GO
ALTER TABLE [dbo].[tbl_VehicleTanks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_VehicleTanks_dbo.tbl_FuelRelayConstractors_FuelRelayConstractorID] FOREIGN KEY([FuelRelayConstractorID])
REFERENCES [dbo].[tbl_FuelRelayConstractors] ([ID])
GO
ALTER TABLE [dbo].[tbl_VehicleTanks] CHECK CONSTRAINT [FK_dbo.tbl_VehicleTanks_dbo.tbl_FuelRelayConstractors_FuelRelayConstractorID]
GO
ALTER TABLE [dbo].[tbl_VehicleTanks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_VehicleTanks_dbo.tbl_GasECUConstractors_GasECUConstractorID] FOREIGN KEY([GasECUConstractorID])
REFERENCES [dbo].[tbl_GasECUConstractors] ([ID])
GO
ALTER TABLE [dbo].[tbl_VehicleTanks] CHECK CONSTRAINT [FK_dbo.tbl_VehicleTanks_dbo.tbl_GasECUConstractors_GasECUConstractorID]
GO
ALTER TABLE [dbo].[tbl_VehicleTanks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_VehicleTanks_dbo.tbl_GenerationofRegulators_GenarationID] FOREIGN KEY([GenarationID])
REFERENCES [dbo].[tbl_GenerationofRegulators] ([ID])
GO
ALTER TABLE [dbo].[tbl_VehicleTanks] CHECK CONSTRAINT [FK_dbo.tbl_VehicleTanks_dbo.tbl_GenerationofRegulators_GenarationID]
GO
ALTER TABLE [dbo].[tbl_VehicleTanks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_VehicleTanks_dbo.tbl_RegulatorConstractors_RegulatorConstractorID] FOREIGN KEY([RegulatorConstractorID])
REFERENCES [dbo].[tbl_RegulatorConstractors] ([ID])
GO
ALTER TABLE [dbo].[tbl_VehicleTanks] CHECK CONSTRAINT [FK_dbo.tbl_VehicleTanks_dbo.tbl_RegulatorConstractors_RegulatorConstractorID]
GO
ALTER TABLE [dbo].[tbl_VehicleTanks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_VehicleTanks_dbo.tbl_TankConstractors_TankConstractorID] FOREIGN KEY([TankConstractorID])
REFERENCES [dbo].[tbl_TankConstractors] ([ID])
GO
ALTER TABLE [dbo].[tbl_VehicleTanks] CHECK CONSTRAINT [FK_dbo.tbl_VehicleTanks_dbo.tbl_TankConstractors_TankConstractorID]
GO
ALTER TABLE [dbo].[tbl_VehicleTanks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_VehicleTanks_dbo.tbl_ValveConstractors_ValveConstractorID] FOREIGN KEY([ValveConstractorID])
REFERENCES [dbo].[tbl_ValveConstractors] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbl_VehicleTanks] CHECK CONSTRAINT [FK_dbo.tbl_VehicleTanks_dbo.tbl_ValveConstractors_ValveConstractorID]
GO
ALTER TABLE [dbo].[tbl_VehicleTanks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_VehicleTanks_dbo.tbl_VehicleRegistrations_VehicleRegistrationID] FOREIGN KEY([VehicleRegistrationID])
REFERENCES [dbo].[tbl_VehicleRegistrations] ([ID])
GO
ALTER TABLE [dbo].[tbl_VehicleTanks] CHECK CONSTRAINT [FK_dbo.tbl_VehicleTanks_dbo.tbl_VehicleRegistrations_VehicleRegistrationID]
GO
ALTER TABLE [dbo].[tbl_WorkshopInsurance]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_WorkshopInsurance_dbo.tbl_InsuranceCompanies_InsuranceCompaniesID] FOREIGN KEY([InsuranceCompaniesID])
REFERENCES [dbo].[tbl_InsuranceCompanies] ([ID])
GO
ALTER TABLE [dbo].[tbl_WorkshopInsurance] CHECK CONSTRAINT [FK_dbo.tbl_WorkshopInsurance_dbo.tbl_InsuranceCompanies_InsuranceCompaniesID]
GO
ALTER TABLE [dbo].[tbl_WorkshopInsurance]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_WorkshopInsurance_dbo.tbl_InsuranceTypes_InsuranceTypesID] FOREIGN KEY([InsuranceTypesID])
REFERENCES [dbo].[tbl_InsuranceTypes] ([ID])
GO
ALTER TABLE [dbo].[tbl_WorkshopInsurance] CHECK CONSTRAINT [FK_dbo.tbl_WorkshopInsurance_dbo.tbl_InsuranceTypes_InsuranceTypesID]
GO
ALTER TABLE [dbo].[tbl_WorkshopInsurance]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_WorkshopInsurance_dbo.tbl_Workshops_WorkshopID] FOREIGN KEY([WorkshopID])
REFERENCES [dbo].[tbl_Workshops] ([ID])
GO
ALTER TABLE [dbo].[tbl_WorkshopInsurance] CHECK CONSTRAINT [FK_dbo.tbl_WorkshopInsurance_dbo.tbl_Workshops_WorkshopID]
GO
ALTER TABLE [dbo].[tbl_Workshops]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Workshops_dbo.tbl_AuditComponies_CompaniesID] FOREIGN KEY([CompaniesID])
REFERENCES [dbo].[tbl_AuditComponies] ([ID])
GO
ALTER TABLE [dbo].[tbl_Workshops] CHECK CONSTRAINT [FK_dbo.tbl_Workshops_dbo.tbl_AuditComponies_CompaniesID]
GO
ALTER TABLE [dbo].[tbl_Workshops]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Workshops_dbo.tbl_Cities_CityID] FOREIGN KEY([CityID])
REFERENCES [dbo].[tbl_Cities] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbl_Workshops] CHECK CONSTRAINT [FK_dbo.tbl_Workshops_dbo.tbl_Cities_CityID]
GO
ALTER TABLE [dbo].[UserEntranceInfo]  WITH CHECK ADD  CONSTRAINT [FK_dbo.UserEntranceInfo_dbo.Users_UserID] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[UserEntranceInfo] CHECK CONSTRAINT [FK_dbo.UserEntranceInfo_dbo.Users_UserID]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Users_dbo.tbl_AuditComponies_AuditCompaniesID] FOREIGN KEY([AuditCompaniesID])
REFERENCES [dbo].[tbl_AuditComponies] ([ID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_dbo.Users_dbo.tbl_AuditComponies_AuditCompaniesID]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Users_dbo.tbl_Workshops_WorkshopID] FOREIGN KEY([WorkshopID])
REFERENCES [dbo].[tbl_Workshops] ([ID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_dbo.Users_dbo.tbl_Workshops_WorkshopID]
GO
/****** Object:  StoredProcedure [dbo].[CreateWorkshopsInvoice]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1399/05/19>
-- Description:	<صدور دوره ای صورتحساب دستمزد تبدیل کارگاه ها>
-- =============================================
CREATE PROCEDURE [dbo].[CreateWorkshopsInvoice] 
	-- Add the parameters for the stored procedure here
	@UserId nvarchar(max),
	@workshopId nvarchar(max),
	@invoiceCode nvarchar(max),
	@fromdate nvarchar(max),
	@todate nvarchar(max)

AS


BEGIN

INSERT INTO dbo.tbl_InvoicesFapa([VehicleTypesID]
      ,[WorkshopsID]
      ,[EquipmentsID]
      ,[ServiceCode]
      ,[InvoiceCode]
      ,[CreatedDate]
      ,[EmployerEconomicalnumber]
      ,[Employerregistrationnumber]
      ,[EmployerState]
      ,[EmployerAddress]
      ,[EmployerPostalcode]
      ,[EmployerPhone]
      ,[EmployerFax]
      ,[ServiceDesc]
      ,[Number]
      ,[UnitofMeasurement]
      ,[UnitAmount]
      ,[TotalAmount]
      ,[DiscountAmount]
      ,[TotalAmountafterDiscount]
      ,[Tax]
      ,[Complications]
      ,[AmountTaxandComplications]
      ,[TotalAmountTaxandComplications]
      ,[Description]
      ,[SaleCondition]
      ,[Comment]
      ,[AcceptedAmount]
      ,[AcceptedDate]
      ,[Status]
      ,[CurrencyTypeID]
      ,[CreatorUser]) 
	  --- Columns of smaller table
        SELECT list2.VehicleTypeID,
		@workshopId,
		NULL,
		list2.VehicleTypeID,
		@invoiceCode,
		CONVERT(VARCHAR(10),@todate,120), --GETDATE(),
		CONVERT(VARCHAR(10),@fromdate,120),
		[dbo].[GetRegistrationnumber](@workshopId),
		[dbo].[GetEconomicalnumber](@workshopId),
		NULL,
		NULL,
		NULL,
		NULL,
		list2.Type,
		list2.Count,
		N'دستگاه',
		list2.Price,
		(CONVERT(float,list2.Count) * list2.Price) as 'Salary' ,
		'0',
		(CONVERT(float,list2.Count) * list2.Price),
		'0',
		'0',	
		(case when [dbo].[GetWorkshopEconomicStatus](@workshopId) > 0 then (CONVERT(float,list2.Count) * list2.Price * [dbo].[GetValueAdded](list2.CreateYear)) else 0 end),		
		(CONVERT(float,(case when [dbo].[GetWorkshopEconomicStatus](@workshopId) > 0 then (CONVERT(float,list2.Count) * list2.Price * [dbo].[GetValueAdded](list2.CreateYear)) else 0 end))) + (CONVERT(float,list2.Count) * list2.Price),
		--(CONVERT(float,list2.Count) * list2.Price * [dbo].[GetValueAdded](list2.CreateYear)),
		--(CONVERT(float,list2.Count) * list2.Price * [dbo].[GetValueAdded](list2.CreateYear)) + (CONVERT(float,list2.Count) * list2.Price),
		NULL,--N'مربوط به مابه التفاوت دستمزد تبدیل می باشد.',
		N'غیر نقدی',
		NULL,
		'0',
		NULL,
		NULL,
		'1',
		@UserId
		from (
		select list.VehicleTypeID,list.Type,SUM(list.Count) as Count,(list.Price) as Price,list.CreateYear from (
		select LEFT(FORMAT(CreateDate, 'yyyy/MM/dd-HH:mm:ss', 'fa'),4) as CreateYear ,VehicleTypeID,(Type + ' ' + tbl_VehicleTypes.Description) Type, COUNT(*) as 'Count',[dbo].[GetRegistrationPrice](WorkshopID,Type,CreateDate) as 'Price' from tbl_VehicleRegistrations inner join tbl_VehicleTypes on tbl_VehicleRegistrations.VehicleTypeID=tbl_VehicleTypes.ID
		where RegisterStatus=1 and RegistrationTypeID=1 and WorkshopID=@workshopId and CONVERT(VARCHAR(10),CreateDate,120) BETWEEN '' + CONVERT(VARCHAR(10),@fromdate,120) + '' AND '' +  CONVERT(VARCHAR(10),@todate,120)+ ''
		group by VehicleTypeID,Type,WorkshopID,CreateDate,tbl_VehicleTypes.Description,LEFT(CONVERT(VARCHAR(10),FORMAT(CreateDate, 'yyyy/MM/dd-HH:mm:ss', 'fa'),120),4)) as list
		group by list.VehicleTypeID,list.Type,list.Price,list.CreateYear) as list2


		--select VehicleTypeID,Type, COUNT(*) as 'Count',[dbo].[GetRegistrationPrice](WorkshopID,Type,CreateDate) as 'Price' from tbl_VehicleRegistrations inner join tbl_VehicleTypes on tbl_VehicleRegistrations.VehicleTypeID=tbl_VehicleTypes.ID
		--where WorkshopID=@workshopId and CONVERT(VARCHAR(10),CreateDate,120) BETWEEN '' + CONVERT(VARCHAR(10),@fromdate,120) + '' AND '' +  CONVERT(VARCHAR(10),@todate,120)+ ''
		--group by VehicleTypeID,Type,WorkshopID,CreateDate) as list 
END
GO
/****** Object:  StoredProcedure [dbo].[CylinderRemDivisionPlanDetails]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1399/01/21>
-- Description:	<مشاهده جزئیات طرح تقسیم به تفکیک کارگاه>
-- =============================================
CREATE PROCEDURE [dbo].[CylinderRemDivisionPlanDetails] 
	-- Add the parameters for the stored procedure here
	@Type nvarchar(max)
	
AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN
SET @PivotTableSQL = '
select *,(case when l2.Rem <0 then ''#ff766d'' else ''#63f576'' end)  TextColor from (
select *,(l1.Send-l1.Used) Rem from (
select [dbo].[GetWorkshops](ID) Title,'+ @Type+' Type,ISNULL([dbo].[GetCylinderSendNumber]('+@Type+',ID),0) Send,ISNULL([dbo].[GetCylinderUsedNumber]('+@Type+',ID),0) Used
from tbl_Workshops) as l1 )as l2
where l2.Send>0 or l2.Used>0
order by l2.Send Desc
'

EXECUTE(@PivotTableSQL)

END

------------------------old version---------------------------------------------------------------------------------------------------------------
--select *,(case when l2.Rem <0 then ''#ff766d'' else ''#63f576'' end)  TextColor from (
--select *,(l1.Send-l1.Used) Rem from (
--select [dbo].[GetWorkshops](tbl4.ID) Title, tbl3.Type,ISNULL([dbo].[GetCylinderSendNumber](tbl3.Type,tbl2.WorkshopID),0) Send,ISNULL([dbo].[GetCylinderUsedNumber](tbl3.Type,tbl2.WorkshopID),0) Used
--from tbl_VehicleTanks tbl1 inner join 
--		tbl_VehicleRegistrations tbl2 on tbl1.VehicleRegistrationID=tbl2.ID inner join 
--		tbl_TypeofTanks tbl3 on tbl1.Volume=tbl3.ID inner join 
--		tbl_Workshops tbl4 on tbl2.WorkshopID=tbl4.ID
--group by tbl3.Type,tbl2.WorkshopID,tbl4.Title,tbl4.ID) as l1 )as l2
--where l2.Type='+@Type+'
--order by l2.Send Desc
------------------------old2 version---------------------------------------------------------------------------------------------------------------
--select l2.Type,SUM(L2.Send) Send, SUM(L2.Used) Used, SUM(L2.Rem) Rem from (
--select [dbo].[GetWorkshops](L1.ID) Type,SUM(L1.Send) Send, SUM(L1.Used) Used,(SUM(L1.Send)-SUM(L1.Used)) Rem from
--(
--	select ID,ISNULL([dbo].[GetVehicleUsed]('+@vehicleType+',ID,''' + CONVERT(VARCHAR(10),@fromDate,120) + ''',''' + CONVERT(VARCHAR(10),@toDate,120) + '''),0) Used,ISNULL([dbo].[GetTankSendNumber]('+@vehicleType+',ID,''' + CONVERT(VARCHAR(10),@fromDate,120) + ''',''' + CONVERT(VARCHAR(10),@toDate,120) + '''),0) Send 
--	from tbl_Workshops
--	--where isServices=1
--	group by ID
--) as L1
--where L1.Send>0 or L1.Used>0
--group by L1.ID,L1.Send,L1.Used
--) as l2
--group by L2.Type,l2.Send,l2.Used
--order by L2.Send desc,L2.Used desc
-----------------------------------------
--@PivotTableSQL = '
--	select [dbo].[GetWorkshops](L1.WorkshopID) Type,SUM(L1.Send) Send, SUM(L1.Used) Used,(SUM(L1.Send)-SUM(L1.Used)) Rem from
--	(
--		select WorkshopID,ISNULL([dbo].[GetVehicleUsed](VehicleTypeID,WorkshopID,''' + CONVERT(VARCHAR(10),@fromDate,120) + ''',''' + CONVERT(VARCHAR(10),@toDate,120) + '''),0) Used,ISNULL([dbo].[GetKitSendNumber](tbl_VehicleTypes.ID,WorkshopID,''' + CONVERT(VARCHAR(10),@fromDate,120) + ''',''' + CONVERT(VARCHAR(10),@toDate,120) + '''),0) Send 
--		from tbl_KitDivisionPlans inner join tbl_DivisionPlans on tbl_KitDivisionPlans.DivisionPlanID=tbl_DivisionPlans.ID inner join tbl_VehicleTypes on tbl_KitDivisionPlans.VehicleTypeID=tbl_VehicleTypes.ID
--		where VehicleTypeID in ('+@vehicleType+')
--		group by VehicleTypeID,Type,tbl_VehicleTypes.ID,tbl_DivisionPlans.WorkshopID
--	) as L1
--	group by L1.WorkshopID'
GO
/****** Object:  StoredProcedure [dbo].[RemDivisionPlan]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1399/01/21>
-- Description:	<مشاهده اطلاعات موجودی طرح های تقسیم>
-- =============================================
CREATE PROCEDURE [dbo].[RemDivisionPlan] 
	-- Add the parameters for the stored procedure here
	@workshop nvarchar(max),
	@fromDate datetime,
	@toDate datetime,
	@Genaration nvarchar(max),
	@RegistrationType nvarchar(max)
	
AS
DECLARE @PivotTableKit NVARCHAR(MAX)

BEGIN

----------------------------------------------- Kit Division Plan -------------------------------------------------------------------------------------------------------------------
select REPLACE(L1.Type, N'انژکتوری', '') as 'Type', SUM(L1.Used) as 'Used',SUM(L1.Send) as 'Send',SUM(L1.KitReturnofParts) as 'KitReturnofParts',(SUM(L1.Send)-SUM(L1.Used)-SUM(L1.KitReturnofParts)) as 'Rem' from
(
	select case tbl_VehicleTypes.ID when 1 then 1 when 6 then 1 else tbl_VehicleTypes.ID end as 'TypeID',case tbl_VehicleTypes.ID when 1 then N'سمند/ پژو' when 6 then N'سمند/ پژو' else  tbl_VehicleTypes.Type end as 'Type',ISNULL([dbo].[GetVehicleUsed](tbl_VehicleTypes.ID,@workshop,@fromDate,@toDate),0) as 'Used',
	ISNULL([dbo].[GetKitSendNumber](tbl_VehicleTypes.ID,@workshop,@fromDate,@toDate),0) as 'Send',ISNULL([dbo].[GetKitReturnofPartsNumber](tbl_VehicleTypes.ID,@workshop,@fromDate,@toDate),0) as 'KitReturnofParts'
	from tbl_KitDivisionPlans RIGHT OUTER JOIN tbl_VehicleTypes on tbl_KitDivisionPlans.VehicleTypeID=tbl_VehicleTypes.ID
	group by tbl_VehicleTypes.ID,tbl_VehicleTypes.Type
) as L1
group by L1.Type

------------------------------------------------- Tank Division Plan -------------------------------------------------------------------------------------------------------------------
select  REPLACE(L1.Type, N'انژکتوری', '') as 'Type', SUM(L1.Used) as 'Used',SUM(L1.Send) as 'Send',SUM(L1.CylinderReturnofParts) as 'CylinderReturnofParts',(SUM(L1.Send)-SUM(L1.Used)-SUM(L1.CylinderReturnofParts)) as 'Rem' from
(
	select case tbl_TypeofTanks.VehicleTypeId when 1 then 1 when 6 then 1 when 3 then 3 when 11 then 3 when 4 then 4 when 8 then 4 when 9 then 4 when 10 then 4 else tbl_TypeofTanks.VehicleTypeId end as 'TypeID',case tbl_TypeofTanks.Type when 1 then N'75 لیتری' when 6 then N'75 لیتری' when 4 then N'62 لیتری' when 8 then N'62 لیتری' else  tbl_TypeofTanks.Type + N' لیتری' end as 'Type',
	ISNULL([dbo].[GetVehicleUsed](tbl_TypeofTanks.VehicleTypeId,@workshop,@fromDate,@toDate),0) as 'Used',ISNULL([dbo].[GetTankSendNumber](tbl_TypeofTanks.VehicleTypeId,@workshop,@fromDate,@toDate),0) as 'Send',
	ISNULL([dbo].[GetReturnofPartsNumber](tbl_TypeofTanks.VehicleTypeId,@workshop,@fromDate,@toDate),0) as 'CylinderReturnofParts'
	from tbl_TankDivisionPlans RIGHT OUTER JOIN tbl_TypeofTanks on tbl_TankDivisionPlans.TypeofTankID=tbl_TypeofTanks.ID inner join tbl_VehicleTypes on tbl_TypeofTanks.VehicleTypeID=tbl_VehicleTypes.ID	group by tbl_TypeofTanks.VehicleTypeId,tbl_TypeofTanks.Type
) as L1
group by L1.Type

------------------------------------------------- Valve Division Plan -------------------------------------------------------------------------------------------------------------------
select L1.Type, SUM(L1.Used) as 'Used',SUM(L1.Send) as 'Send', SUM(L1.ValveReturnofParts) as 'ValveReturnofParts',(SUM(L1.Send)-SUM(L1.Used)-SUM(L1.ValveReturnofParts)) as 'Rem' from
(

select N'شیر مخزن' as 'Type',ISNULL([dbo].[GetVehicleUsed](VehicleTypeID,@workshop,@fromDate,@toDate),0) as 'Used',ISNULL([dbo].[GetTankSendNumber](VehicleTypeID,@workshop,@fromDate,@toDate),0) as 'Send',ISNULL([dbo].[GetValveReturnofPartsNumber](VehicleTypeID,@workshop,@fromDate,@toDate),0) as 'ValveReturnofParts'
	from tbl_VehicleRegistrations RIGHT OUTER JOIN tbl_VehicleTypes on tbl_VehicleRegistrations.VehicleTypeID=tbl_VehicleTypes.ID
	where RegisterStatus=1
	--where WorkshopID=@workshop or @workshop=0
	group by VehicleTypeID,tbl_VehicleTypes.Type
) as L1
group by L1.Type

------------------------------------------------- Tank Base Division Plan -------------------------------------------------------------------------------------------------------------------
select  REPLACE(L1.Type, N'انژکتوری', '') as 'Type', SUM(L1.Used) as 'Used',SUM(L1.Send) as 'Send',SUM(L1.CylinderBaseReturnofParts) as 'CylinderBaseReturnofParts',(SUM(L1.Send)-SUM(L1.Used)-SUM(L1.CylinderBaseReturnofParts)) as 'Rem' from
(
	select case tbl_TypeofTankBases.VehicleTypeId when 1 then 1 when 6 then 1 else tbl_TypeofTankBases.VehicleTypeId end as 'TypeID',case tbl_TypeofTankBases.VehicleTypeId when 1 then N'سمند/ پژو ' when 6 then N'سمند/ پژو ' else  tbl_TypeofTankBases.Type end as 'Type',
	ISNULL([dbo].[GetVehicleUsed](tbl_TypeofTankBases.VehicleTypeId,@workshop,@fromDate,@toDate),0) as 'Used',ISNULL([dbo].[GetTankBaseSendNumber](tbl_TypeofTankBases.VehicleTypeId,@workshop,@fromDate,@toDate),0) as 'Send',
	ISNULL([dbo].[GetCylinderBaseReturnofPartsNumber](tbl_TypeofTankBases.VehicleTypeId,@workshop,@fromDate,@toDate),0) as 'CylinderBaseReturnofParts'
	from tbl_TankBaseDivisionPlans RIGHT OUTER JOIN tbl_TypeofTankBases on tbl_TankBaseDivisionPlans.TypeofTankBaseID=tbl_TypeofTankBases.ID inner join tbl_VehicleTypes on tbl_TypeofTankBases.VehicleTypeID=tbl_VehicleTypes.ID	
	group by tbl_TypeofTankBases.VehicleTypeId,tbl_TypeofTankBases.Type
) as L1
group by L1.Type

------------------------------------------------- Tank Cover Division Plan -------------------------------------------------------------------------------------------------------------------
select   REPLACE(L1.Type, N'انژکتوری', '') as 'Type', SUM(L1.Used) as 'Used',SUM(L1.Send) as 'Send',SUM(L1.CylinderCoverReturnofParts) as 'CylinderCoverReturnofParts',(SUM(L1.Send)-SUM(L1.Used)-SUM(L1.CylinderCoverReturnofParts)) as 'Rem' from
(
	select case tbl_TypeofTankCovers.VehicleTypeId when 1 then 1 when 6 then 1 else tbl_TypeofTankCovers.VehicleTypeId end as 'TypeID',case tbl_TypeofTankCovers.VehicleTypeId when 1 then N'سمند/ پژو ' when 6 then N'سمند/ پژو ' else  tbl_TypeofTankCovers.Type end as 'Type',
	ISNULL([dbo].[GetVehicleUsed](tbl_TypeofTankCovers.VehicleTypeId,@workshop,@fromDate,@toDate),0) as 'Used',ISNULL([dbo].[GetTankCoverSendNumber](tbl_TypeofTankCovers.VehicleTypeId,@workshop,@fromDate,@toDate),0) as 'Send',
	ISNULL([dbo].[GetCylinderCoverReturnofPartsNumber](tbl_TypeofTankCovers.VehicleTypeId,@workshop,@fromDate,@toDate),0) as 'CylinderCoverReturnofParts'
	from tbl_TankCoverDivisionPlans RIGHT OUTER JOIN tbl_TypeofTankCovers on tbl_TankCoverDivisionPlans.TypeofTankCoverID=tbl_TypeofTankCovers.ID inner join tbl_VehicleTypes on tbl_TypeofTankCovers.VehicleTypeID=tbl_VehicleTypes.ID
	group by tbl_TypeofTankCovers.VehicleTypeId,tbl_TypeofTankCovers.Type
) as L1
group by L1.Type

------------------------------------------------- Otherthings Division Plan -------------------------------------------------------------------------------------------------------------------

select *,(l1.Send-l1.Used) as 'Rem' from
(select (dbo.tbl_Otherthings.Title) as 'Type',SUM(NumberofSend) as 'Send',[dbo].[GetVehicleValveCount](@workshop,@fromDate,@toDate) as 'Used' from dbo.tbl_OtherThingsDivisionPlans 
	left outer join tbl_Otherthings on  dbo.tbl_OtherThingsDivisionPlans.DiThingsID=tbl_Otherthings.ID inner join tbl_DivisionPlans on dbo.tbl_OtherThingsDivisionPlans.DivisionPlanID=tbl_DivisionPlans.ID
	where (dbo.tbl_DivisionPlans.WorkshopID=@workshop or @workshop=0)  and CONVERT(VARCHAR(10),tbl_DivisionPlans.CreateDate,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
	group by tbl_Otherthings.Title
				) as l1

END

----old version befor 1403-11-19----------------------------------------

-----------------------------------------------14031120-------------------------------------------
------------------------------------------------- Kit Division Plan -------------------------------------------------------------------------------------------------------------------
--select REPLACE(L1.Type+' ('+l1.Description+')', N'انژکتوری', '') as 'Type', SUM(L1.Used) as 'Used',SUM(L1.Send) as 'Send',SUM(L1.KitReturnofParts) as 'KitReturnofParts',(SUM(L1.Send)-SUM(L1.Used)-SUM(L1.KitReturnofParts)) as 'Rem' from
--(
--	select case tbl_VehicleTypes.ID when 1 then 1 when 6 then 1 else tbl_VehicleTypes.ID end as 'TypeID',case tbl_VehicleTypes.ID when 1 then N'سمند/ پژو' when 6 then N'سمند/ پژو' else  tbl_VehicleTypes.Type end as 'Type',ISNULL([dbo].[GetVehicleUsed](tbl_VehicleTypes.ID,@workshop,@fromDate,@toDate,@RegistrationType),0) as 'Used',
--	ISNULL([dbo].[GetKitSendNumber](tbl_VehicleTypes.ID,@workshop,@fromDate,@toDate),0) as 'Send',ISNULL([dbo].[GetKitReturnofPartsNumber](tbl_VehicleTypes.ID,@workshop,@fromDate,@toDate),0) as 'KitReturnofParts',tbl_VehicleTypes.Description
--	from tbl_KitDivisionPlans right OUTER JOIN tbl_VehicleTypes on tbl_KitDivisionPlans.VehicleTypeID=tbl_VehicleTypes.ID
--	where (tbl_KitDivisionPlans.GenarationID=@Genaration or @Genaration=0) and (tbl_KitDivisionPlans.RegistrationTypeID=@RegistrationType or @RegistrationType=0)
--	group by tbl_VehicleTypes.ID,tbl_VehicleTypes.Type,tbl_VehicleTypes.Description
--) as L1
--group by l1.Type,l1.Description

--------------------------------------------------- Tank Division Plan -------------------------------------------------------------------------------------------------------------------
--select  REPLACE(N'مخزن ' + L1.Type, N'انژکتوری', '') as 'Type', SUM(L1.Used) as 'Used',SUM(L1.Send) as 'Send',SUM(L1.CylinderReturnofParts) as 'CylinderReturnofParts',(SUM(L1.Send)-SUM(L1.Used)-SUM(L1.CylinderReturnofParts)) as 'Rem' from
--(
--	select case tbl_TypeofTanks.VehicleTypeId when 1 then 1 when 6 then 1 when 3 then 3 when 11 then 3 when 4 then 4 when 8 then 4 when 9 then 4 when 10 then 4 else tbl_TypeofTanks.VehicleTypeId end as 'TypeID',case tbl_TypeofTanks.Type when 1 then N'75 لیتری' when 6 then N'75 لیتری' when 4 then N'62 لیتری' when 8 then N'62 لیتری' else  tbl_TypeofTanks.Type + N' لیتری' end as 'Type',
--	ISNULL([dbo].[GetVehicleUsed](tbl_TypeofTanks.VehicleTypeId,@workshop,@fromDate,@toDate,@RegistrationType),0) as 'Used',ISNULL([dbo].[GetTankSendNumber](tbl_TypeofTanks.VehicleTypeId,@workshop,@fromDate,@toDate),0) as 'Send',
--	ISNULL([dbo].[GetReturnofPartsNumber](tbl_TypeofTanks.VehicleTypeId,@workshop,@fromDate,@toDate),0) as 'CylinderReturnofParts'
--	from tbl_TankDivisionPlans right OUTER JOIN tbl_TypeofTanks on tbl_TankDivisionPlans.TypeofTankID=tbl_TypeofTanks.ID inner join tbl_VehicleTypes on tbl_TypeofTanks.VehicleTypeID=tbl_VehicleTypes.ID	
--	where (tbl_TankDivisionPlans.GenarationID=@Genaration or @Genaration=0) and (tbl_TankDivisionPlans.RegistrationTypeID=@RegistrationType or @RegistrationType=0)
--	group by tbl_TypeofTanks.VehicleTypeId,tbl_TypeofTanks.Type	
--) as L1
--group by L1.Type

--------------------------------------------------- Valve Division Plan -------------------------------------------------------------------------------------------------------------------

--select L1.Type, SUM(L1.Used) as 'Used',SUM(L1.Send) as 'Send', SUM(L1.ValveReturnofParts) as 'ValveReturnofParts',(SUM(L1.Send)-SUM(L1.Used)-SUM(L1.ValveReturnofParts)) as 'Rem' from
--(

--select N'شیر مخزن' as 'Type',ISNULL([dbo].[GetVehicleUsed](VehicleTypeID,@workshop,@fromDate,@toDate,@RegistrationType),0) as 'Used',ISNULL([dbo].[GetTankSendNumber](VehicleTypeID,@workshop,@fromDate,@toDate),0) as 'Send',ISNULL([dbo].[GetValveReturnofPartsNumber](VehicleTypeID,@workshop,@fromDate,@toDate),0) as 'ValveReturnofParts'
--	from tbl_VehicleRegistrations RIGHT OUTER JOIN tbl_VehicleTypes on tbl_VehicleRegistrations.VehicleTypeID=tbl_VehicleTypes.ID
--	where RegisterStatus=1 and (WorkshopID=@workshop or @workshop=0)
--	group by VehicleTypeID,tbl_VehicleTypes.Type
--) as L1
--group by L1.Type

--------------------------------------------------- Tank Base Division Plan -------------------------------------------------------------------------------------------------------------------

--select  REPLACE(N'پایه مخزن ' + L1.Type, N'انژکتوری', '') as 'Type', SUM(L1.Used) as 'Used',SUM(L1.Send) as 'Send',SUM(L1.CylinderBaseReturnofParts) as 'CylinderBaseReturnofParts',(SUM(L1.Send)-SUM(L1.Used)-SUM(L1.CylinderBaseReturnofParts)) as 'Rem' from
--(
--	select case tbl_TypeofTankBases.VehicleTypeId when 1 then 1 when 6 then 1 else tbl_TypeofTankBases.VehicleTypeId end as 'TypeID',case tbl_TypeofTankBases.VehicleTypeId when 1 then N'سمند/ پژو ' when 6 then N'سمند/ پژو ' else  tbl_TypeofTankBases.Type end as 'Type',
--	ISNULL([dbo].[GetVehicleUsed](tbl_TypeofTankBases.VehicleTypeId,@workshop,@fromDate,@toDate,@RegistrationType),0) as 'Used',ISNULL([dbo].[GetTankBaseSendNumber](tbl_TypeofTankBases.VehicleTypeId,@workshop,@fromDate,@toDate),0) as 'Send',
--	ISNULL([dbo].[GetCylinderBaseReturnofPartsNumber](tbl_TypeofTankBases.VehicleTypeId,@workshop,@fromDate,@toDate),0) as 'CylinderBaseReturnofParts'
--	from tbl_TankBaseDivisionPlans right OUTER JOIN tbl_TypeofTankBases on tbl_TankBaseDivisionPlans.TypeofTankBaseID=tbl_TypeofTankBases.ID inner join tbl_VehicleTypes on tbl_TypeofTankBases.VehicleTypeID=tbl_VehicleTypes.ID	
--	where (tbl_TankBaseDivisionPlans.GenarationID=@Genaration or @Genaration=0) and (tbl_TankBaseDivisionPlans.RegistrationTypeID=@RegistrationType or @RegistrationType=0)
--	group by tbl_TypeofTankBases.VehicleTypeId,tbl_TypeofTankBases.Type
--) as L1
--group by L1.Type

--------------------------------------------------- Tank Cover Division Plan -------------------------------------------------------------------------------------------------------------------

--select   REPLACE(N'کاور مخزن ' + L1.Type, N'انژکتوری', '') as 'Type', SUM(L1.Used) as 'Used',SUM(L1.Send) as 'Send',SUM(L1.CylinderCoverReturnofParts) as 'CylinderCoverReturnofParts',(SUM(L1.Send)-SUM(L1.Used)-SUM(L1.CylinderCoverReturnofParts)) as 'Rem' from
--(
--	select case tbl_TypeofTankCovers.VehicleTypeId when 1 then 1 when 6 then 1 else tbl_TypeofTankCovers.VehicleTypeId end as 'TypeID',case tbl_TypeofTankCovers.VehicleTypeId when 1 then N'سمند/ پژو ' when 6 then N'سمند/ پژو ' else  tbl_TypeofTankCovers.Type end as 'Type',
--	ISNULL([dbo].[GetVehicleUsed](tbl_TypeofTankCovers.VehicleTypeId,@workshop,@fromDate,@toDate,@RegistrationType),0) as 'Used',ISNULL([dbo].[GetTankCoverSendNumber](tbl_TypeofTankCovers.VehicleTypeId,@workshop,@fromDate,@toDate),0) as 'Send',
--	ISNULL([dbo].[GetCylinderCoverReturnofPartsNumber](tbl_TypeofTankCovers.VehicleTypeId,@workshop,@fromDate,@toDate),0) as 'CylinderCoverReturnofParts'
--	from tbl_TankCoverDivisionPlans RIGHT OUTER JOIN tbl_TypeofTankCovers on tbl_TankCoverDivisionPlans.TypeofTankCoverID=tbl_TypeofTankCovers.ID inner join tbl_VehicleTypes on tbl_TypeofTankCovers.VehicleTypeID=tbl_VehicleTypes.ID
--	where (tbl_TankCoverDivisionPlans.GenarationID=@Genaration or @Genaration=0) and (tbl_TankCoverDivisionPlans.RegistrationTypeID=@RegistrationType or @RegistrationType=0)
--	group by tbl_TypeofTankCovers.VehicleTypeId,tbl_TypeofTankCovers.Type
--) as L1
--group by L1.Type

--------------------------------------------------- Otherthings Division Plan -------------------------------------------------------------------------------------------------------------------

--select *,(l1.Send-l1.Used) as 'Rem' from
--(select (dbo.tbl_Otherthings.Title) as 'Type',SUM(NumberofSend) as 'Send',[dbo].[GetVehicleValveCount](@workshop,@fromDate,@toDate) as 'Used' from dbo.tbl_OtherThingsDivisionPlans 
--	left outer join tbl_Otherthings on  dbo.tbl_OtherThingsDivisionPlans.DiThingsID=tbl_Otherthings.ID inner join tbl_DivisionPlans on dbo.tbl_OtherThingsDivisionPlans.DivisionPlanID=tbl_DivisionPlans.ID
--	where (dbo.tbl_DivisionPlans.WorkshopID=@workshop or @workshop=0)  and CONVERT(VARCHAR(10),tbl_DivisionPlans.CreateDate,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
--	and (tbl_OtherThingsDivisionPlans.GenarationID=@Genaration or @Genaration=0) and (tbl_OtherThingsDivisionPlans.RegistrationTypeID=@RegistrationType or @RegistrationType=0)
--	group by tbl_Otherthings.Title
--				) as l1
GO
/****** Object:  StoredProcedure [dbo].[RemDivisionPlan_Old]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1399/01/21>
-- Description:	<مشاهده اطلاعات موجودی طرح های تقسیم>
-- =============================================
CREATE PROCEDURE [dbo].[RemDivisionPlan_Old] 
	-- Add the parameters for the stored procedure here
	@workshop nvarchar(max),
	@fromDate datetime,
	@toDate datetime
	
AS
DECLARE @PivotTableKit NVARCHAR(MAX)

BEGIN
	------------------------------------------------- Kit Division Plan -------------------------------------------------------------------------------------------------------------------

  select L1.Type, SUM(L1.Used) as 'Used',SUM(L1.Send) as 'Send',(SUM(L1.Send)-SUM(L1.Used)) as 'Rem' from
(
select case tbl_VehicleTypes.ID when 1 then N'سمند/ پژو انژکتوری' when 6 then N'سمند/ پژو انژکتوری' else tbl_VehicleTypes.Type end as 'Type',[dbo].[GetVehicleTypeCount](tbl_VehicleTypes.ID,@workshop) as 'Used',[dbo].[GetKitSendNumber](tbl_VehicleTypes.ID,@workshop) as 'Send' from tbl_DivisionPlans inner join tbl_KitDivisionPlans on tbl_DivisionPlans.ID=DivisionPlanID inner join tbl_VehicleTypes on tbl_KitDivisionPlans.VehicleTypeID=tbl_VehicleTypes.ID
  where WorkshopID=@workshop or @workshop=0
  group by tbl_VehicleTypes.Type,tbl_VehicleTypes.ID

) as L1
group by L1.Type

------------------------------------------------- Tank Division Plan -------------------------------------------------------------------------------------------------------------------

  select L1.Type, SUM(L1.Used) as 'Used',SUM(L1.Send) as 'Send',(SUM(L1.Send)-SUM(L1.Used)) as 'Rem' from
(
select case tbl_VehicleTypes.ID when 1 then N'75 لیتری' when 6 then N'75 لیتری'else tbl_VehicleTypes.Description end as 'Type',[dbo].[GetVehicleTypeCount](tbl_VehicleTypes.ID,@workshop) as 'Used',[dbo].[GetTankSendNumber](tbl_VehicleTypes.ID,@workshop) as 'Send' 
FROM            dbo.tbl_DivisionPlans INNER JOIN
                         dbo.tbl_TankDivisionPlans ON dbo.tbl_DivisionPlans.ID = dbo.tbl_TankDivisionPlans.DivisionPlanID INNER JOIN
                         dbo.tbl_TypeofTanks ON dbo.tbl_TankDivisionPlans.TypeofTankID = dbo.tbl_TypeofTanks.ID INNER JOIN
                         dbo.tbl_VehicleTypes ON dbo.tbl_TypeofTanks.VehicleTypeId = dbo.tbl_VehicleTypes.ID INNER JOIN
                         dbo.tbl_Workshops ON dbo.tbl_DivisionPlans.WorkshopID = dbo.tbl_Workshops.ID
  where WorkshopID=@workshop or @workshop=0
  group by tbl_VehicleTypes.Description,tbl_VehicleTypes.ID,tbl_TankDivisionPlans.TypeofTankID

) as L1
group by L1.Type

------------------------------------------------- Tank Base Division Plan -------------------------------------------------------------------------------------------------------------------

select L1.Type, SUM(L1.Used) as 'Used',SUM(L1.Send) as 'Send',(SUM(L1.Send)-SUM(L1.Used)) as 'Rem' from
(
select case tbl_TypeofTankBases.VehicleTypeId when 1 then N'سمند/ پژو' when 6 then N'سمند/ پژو' else tbl_TypeofTankBases.Type end as 'Type',[dbo].[GetVehicleTypeCount](tbl_TypeofTankBases.VehicleTypeId,@workshop) as 'Used',[dbo].[GetTankBaseSendNumber](tbl_TypeofTankBases.VehicleTypeId,@workshop) as 'Send' 
FROM            dbo.tbl_DivisionPlans INNER JOIN
                         dbo.tbl_TankBaseDivisionPlans ON dbo.tbl_DivisionPlans.ID = dbo.tbl_TankBaseDivisionPlans.DivisionPlanID INNER JOIN
                         dbo.tbl_TypeofTankBases ON dbo.tbl_TankBaseDivisionPlans.TypeofTankBaseID = dbo.tbl_TypeofTankBases.ID INNER JOIN
                         dbo.tbl_VehicleTypes ON dbo.tbl_TypeofTankBases.VehicleTypeId = dbo.tbl_VehicleTypes.ID INNER JOIN
                         dbo.tbl_Workshops ON dbo.tbl_DivisionPlans.WorkshopID = dbo.tbl_Workshops.ID
  where WorkshopID=@workshop or @workshop=0
  group by tbl_TypeofTankBases.Type,tbl_TypeofTankBases.VehicleTypeId

) as L1
group by L1.Type

------------------------------------------------- Valve Division Plan -------------------------------------------------------------------------------------------------------------------

select L1.Type, SUM(L1.Used) as 'Used',SUM(L1.Send) as 'Send',(SUM(L1.Send)-SUM(L1.Used)) as 'Rem' from
(
select N'شیر مخزن' as 'Type',[dbo].[GetVehicleTypeCount](tbl_TypeofTankCovers.VehicleTypeId,@workshop) as 'Used',[dbo].[GetKitSendNumber](tbl_TypeofTankCovers.VehicleTypeId,@workshop) as 'Send' from tbl_DivisionPlans inner join tbl_KitDivisionPlans on tbl_DivisionPlans.ID=DivisionPlanID inner join tbl_TypeofTankCovers on tbl_KitDivisionPlans.VehicleTypeID=tbl_TypeofTankCovers.VehicleTypeId
  where WorkshopID=@workshop or @workshop=0
  group by tbl_TypeofTankCovers.Type,tbl_TypeofTankCovers.VehicleTypeId

) as L1
group by L1.Type
------------------------------------------------- Tank Cover Division Plan -------------------------------------------------------------------------------------------------------------------

select L1.Type, SUM(L1.Used) as 'Used',SUM(L1.Send) as 'Send',(SUM(L1.Send)-SUM(L1.Used)) as 'Rem' from
(
select case tbl_TypeofTankCovers.VehicleTypeId when 1 then N'سمند/ پژو' when 6 then N'سمند/ پژو' else tbl_TypeofTankCovers.Type end as 'Type',[dbo].[GetVehicleTypeCount](tbl_TypeofTankCovers.VehicleTypeId,@workshop) as 'Used',[dbo].[GetTankCoverSendNumber](tbl_TypeofTankCovers.VehicleTypeId,@workshop) as 'Send' 
FROM            dbo.tbl_DivisionPlans INNER JOIN
                         dbo.tbl_TankCoverDivisionPlans ON dbo.tbl_DivisionPlans.ID = dbo.tbl_TankCoverDivisionPlans.DivisionPlanID INNER JOIN
                         dbo.tbl_TypeofTankCovers ON dbo.tbl_TankCoverDivisionPlans.TypeofTankCoverID = dbo.tbl_TypeofTankCovers.ID INNER JOIN
                         dbo.tbl_VehicleTypes ON dbo.tbl_TypeofTankCovers.VehicleTypeId = dbo.tbl_VehicleTypes.ID INNER JOIN
                         dbo.tbl_Workshops ON dbo.tbl_DivisionPlans.WorkshopID = dbo.tbl_Workshops.ID
  where WorkshopID=@workshop or @workshop=0
  group by tbl_TypeofTankCovers.Type,tbl_TypeofTankCovers.VehicleTypeId

) as L1
group by L1.Type

------------------------------------------------- Otherthings Division Plan -------------------------------------------------------------------------------------------------------------------

select *,(l1.Send-l1.Used) as 'Rem' from
(select (dbo.tbl_Otherthings.Title) as 'Type',SUM(NumberofSend) as 'Send',[dbo].[GetVehicleValveCount](@workshop,@fromDate,@toDate) as 'Used' from dbo.tbl_OtherThingsDivisionPlans 
	left outer join tbl_Otherthings on  dbo.tbl_OtherThingsDivisionPlans.DiThingsID=tbl_Otherthings.ID inner join tbl_DivisionPlans on dbo.tbl_OtherThingsDivisionPlans.DivisionPlanID=tbl_DivisionPlans.ID
	where (dbo.tbl_DivisionPlans.WorkshopID=@workshop or @workshop=0)  and tbl_DivisionPlans.CreateDate between @fromDate and @toDate
	group by tbl_Otherthings.Title
				) as l1

END

GO
/****** Object:  StoredProcedure [dbo].[RemDivisionPlanDetails]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1399/01/21>
-- Description:	<مشاهده جزئیات طرح تقسیم به تفکیک کارگاه>
-- =============================================
CREATE PROCEDURE [dbo].[RemDivisionPlanDetails] 
	-- Add the parameters for the stored procedure here
	@vehicleType nvarchar(max),
	@functionUsed nvarchar(max),
	@functionSend nvarchar(max),
	@fromDate datetime,
	@toDate datetime
	
AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN
SET @PivotTableSQL = '
select *,(case when l2.Rem <0 then ''#ff766d'' else ''#63f576'' end)  TextColor from (
select *,(l1.Send-l1.Used) Rem from (
select [dbo].[GetWorkshops](ID) Type,'+ @vehicleType +' Title,ISNULL([dbo].['+ @functionSend +']('+ @vehicleType +',ID),0) Send,ISNULL([dbo].['+ @functionUsed +']('+@vehicleType+',ID),0) Used
from tbl_Workshops) as l1 )as l2
where l2.Send>0 or l2.Used>0
order by l2.Send Desc
'

EXECUTE(@PivotTableSQL)

END
--------------------------------------------------------------old version-----------------------------------------------------------------
--select *,(case when l2.Rem <0 then ''#ff766d'' else ''#63f576'' end)  TextColor from (
--select [dbo].[GetWorkshops](L1.ID) Type,SUM(L1.Send) Send, SUM(L1.Used) Used,(SUM(L1.Send)-SUM(L1.Used)) Rem from
--(
--	select ID,ISNULL([dbo].[GetVehicleUsed]('+@vehicleType+',ID,''' + CONVERT(VARCHAR(10),@fromDate,120) + ''',''' + CONVERT(VARCHAR(10),@toDate,120) + '''),0) Used,ISNULL([dbo].['+@function+']('+@vehicleType+',ID,''' + CONVERT(VARCHAR(10),@fromDate,120) + ''',''' + CONVERT(VARCHAR(10),@toDate,120) + '''),0) Send 
--	from tbl_Workshops
--	--where isServices=1
--	group by ID
--) as L1
--where L1.Send>0 or L1.Used>0
--group by L1.ID,L1.Send,L1.Used) as l2
--order by l2.Send desc,l2.Used desc
--------------------------------------------------------------old2 version-----------------------------------------------------------------
--@PivotTableSQL = '
--	select [dbo].[GetWorkshops](L1.WorkshopID) Type,SUM(L1.Send) Send, SUM(L1.Used) Used,(SUM(L1.Send)-SUM(L1.Used)) Rem from
--	(
--		select WorkshopID,ISNULL([dbo].[GetVehicleUsed](VehicleTypeID,WorkshopID,''' + CONVERT(VARCHAR(10),@fromDate,120) + ''',''' + CONVERT(VARCHAR(10),@toDate,120) + '''),0) Used,ISNULL([dbo].[GetKitSendNumber](tbl_VehicleTypes.ID,WorkshopID,''' + CONVERT(VARCHAR(10),@fromDate,120) + ''',''' + CONVERT(VARCHAR(10),@toDate,120) + '''),0) Send 
--		from tbl_KitDivisionPlans inner join tbl_DivisionPlans on tbl_KitDivisionPlans.DivisionPlanID=tbl_DivisionPlans.ID inner join tbl_VehicleTypes on tbl_KitDivisionPlans.VehicleTypeID=tbl_VehicleTypes.ID
--		where VehicleTypeID in ('+@vehicleType+')
--		group by VehicleTypeID,Type,tbl_VehicleTypes.ID,tbl_DivisionPlans.WorkshopID
--	) as L1
--	group by L1.WorkshopID'
GO
/****** Object:  StoredProcedure [dbo].[sp_AnswerQuestionResults]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1399/11/26>
-- Description:	<مشاهده لیست نظر دهنده ها به سوالات نظر سنجی>
-- =============================================
CREATE PROCEDURE [dbo].[sp_AnswerQuestionResults] 
	-- Add the parameters for the stored procedure here
	--@id nvarchar(max)
AS


BEGIN

select distinct(ip),tbl2.CreateDate as RegisterDate,Mobile,FullName,CONVERT(date,tbl1.CreateDate) as CreateDate,ISNULL([dbo].[GetWorkshops](tbl2.WorkshopID),'') Workshop, ISNULL([dbo].[GetVehicleType](tbl2.VehicleTypeID),'') Type
from tbl_AnswerQuestions tbl1 
		left outer join tbl_VehicleRegistrations tbl2 on tbl1.RegistrationsId=tbl2.ID

END

GO
/****** Object:  StoredProcedure [dbo].[sp_ApprovedInvoices]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1401/05/05>
-- Description:	<نمایش جزئیات خودروهای تبدیل شده در صورت وضعیت عملکرد تایید شده بر اساس تایید مالی پروژه>
-- =============================================
CREATE PROCEDURE [dbo].[sp_ApprovedInvoices] 
	-- Add the parameters for the stored procedure here
	@workshops nvarchar(max)=null,
	@FromDate DATETIME ,
	@ToDate DATETIME
AS

BEGIN
-----------------------------------------------------------------------financial credit and debit ------------------------------------------------------------------------------
select *,(CONVERT(float,list.Creditor) - CONVERT(float,list.Debtor)) as 'Rem' from (
select ID,Title,ISNULL([dbo].[GetFinancialCreditor_Assesment](ID,@workshops,@FromDate,@ToDate),0) as 'Creditor',ISNULL([dbo].[GetFinancialDebtor](ID,@workshops,@FromDate,@ToDate),0) as 'Debtor'
FROM [CNGFAPCO].[dbo].[tbl_FinancialDescs] 
where ID in (1,2,3,6)
) as list
---------------------------------------------------------------------approved invoices-------------------------------------------------------------------------------------------
	SELECT 
	  0 as 'InvoiceCode'
	  ,tbl_VehicleTypes.Type + '  ' + tbl_VehicleTypes.Description as 'ServiceDesc'
      ,SUM(convert(float, [Number])) as [Number]
      ,[UnitofMeasurement]
	  ,'' as Description --tbl_InvoicesFapa.Description
      ,0  as 'UnitAmount'
      ,SUM([TotalAmount]) as [TotalAmount]
      ,SUM([AmountTaxandComplications]) as [AmountTaxandComplications]
      ,SUM([TotalAmountTaxandComplications]) as [TotalAmountTaxandComplications]
  FROM [CNGFAPCO].[dbo].[tbl_InvoicesFapa] inner join tbl_VehicleTypes on tbl_InvoicesFapa.VehicleTypesID=tbl_VehicleTypes.ID
  where FinancialStatus=1 and WorkshopsID=@workshops
  group by Type,tbl_VehicleTypes.Description,tbl_InvoicesFapa.UnitofMeasurement
  order by tbl_VehicleTypes.Type + '  ' + tbl_VehicleTypes.Description 

  --,tbl_InvoicesFapa.UnitAmount

  -----------------------------for sum total value -------------------------------------------------------------------------------
  SELECT [dbo].[GetInvoiceFapaCount](@workshops) as 'SumCount'
	  ,sum([TotalAmount]) as 'SumTotalAmount'
      ,sum([AmountTaxandComplications]) as 'SumAmountTax'
      ,sum([TotalAmountTaxandComplications]) as 'SumTotalAmountTax'
  FROM [CNGFAPCO].[dbo].[tbl_InvoicesFapa]
  where FinancialStatus=1 and WorkshopsID=@workshops
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CallBackRequestFreeSaleList]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1400/3/7>
-- Description:	صفحه ارجاع کاربر متقاضی خرید اقلام طرح فروش آزاد و مشاهده لیست درخواست ها
-- =============================================
CREATE PROCEDURE [dbo].[sp_CallBackRequestFreeSaleList] 
	-- Add the parameters for the stored procedure here
	@workshop nvarchar(max)=null 
	

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN

SET @PivotTableSQL = N'select Owners,[dbo].[GetWorkshops](WorkshopsID) as Workshops,InvoiceCode,CONVERT(date, CreatedDate) as CreatedDate,SUM(CONVERT(int, Number)) as Number,UnitofMeasurement,(SUM(CONVERT(float,TotalAmount))*0.09+SUM(CONVERT(float,TotalAmount))) as TotalAmount,SUM(CONVERT(float,DiscountAmount)) as DiscountAmount,
Status , (case when Status=0 then ''#f7818c'' else ''#82f59d'' end) ViewStatusColor,SaleCondition
FROM [CNGFAPCO].[dbo].[tbl_RequestFreeSales]
where WorkshopsID in ('+@workshop+')
GROUP BY Owners,WorkshopsID,InvoiceCode,CONVERT(date, CreatedDate),UnitofMeasurement,Status,SaleCondition
ORDER BY CONVERT(date, CreatedDate),InvoiceCode desc
'


EXECUTE(@PivotTableSQL)

END

------------1401/03/20 changed-----------------------------------
--CONVERT(VARCHAR(5), CreatedDate, 108) as Time,
--SUM(CONVERT(float,TotalAmount)) as TotalAmount
GO
/****** Object:  StoredProcedure [dbo].[sp_CheckedPreInvoice]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1400/04/04>
-- Description:	مشاهده اطلاعات پیش فاکتورهای صادر شده که هنوز نهایی و فاکتور نشده اند جهت مشاهده در گزارش مالی 
-- =============================================
Create PROCEDURE [dbo].[sp_CheckedPreInvoice] 
	-- Add the parameters for the stored procedure here
	@workshops nvarchar(max)=null
	

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN
SET @PivotTableSQL = N'
select PreInvoiceCode,tbl_FreeSaleInvoices.WorkshopsID, SUM(tbl_FreeSaleInvoices.TotalAmountTaxandComplications) as Amount, tbl_FreeSaleInvoices.SaleCondition
from tbl_FinallFreeSaleInvoices right outer join tbl_FreeSaleInvoices on tbl_FreeSaleInvoices.InvoiceCode = tbl_FinallFreeSaleInvoices.PreInvoiceCode
where tbl_FreeSaleInvoices.WorkshopsID in ('+@workshops+')
group by PreInvoiceCode,tbl_FreeSaleInvoices.WorkshopsID,tbl_FreeSaleInvoices.SaleCondition
having PreInvoiceCode is null
  '
EXECUTE(@PivotTableSQL)

END
GO
/****** Object:  StoredProcedure [dbo].[sp_Contradictions]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1399/04/15>
-- Description:	مشاهده اطلاعات مغایرت عملکرد سه سامانه و موجودی
-- =============================================
CREATE PROCEDURE [dbo].[sp_Contradictions] 
	-- Add the parameters for the stored procedure here
	@workshops nvarchar(max)=null,
	@Date DATETIME 
	

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)
DECLARE @PivotTableSQL2 NVARCHAR(MAX)
DECLARE @PivotTableSQL3 NVARCHAR(MAX)


BEGIN

SET @PivotTableSQL = N'
select * from (
SELECT (case when [Description] like N''%تعداد ثبت نامی%'' then 1 when [Description] like N''%تعداد تبدیل طبق سایت فن آوران%'' then 2 
		when [Description] like N''%تعداد تبدیل طبق سایت پخش%'' then 3 when [Description] like N''%تعداد گواهی سلامت در سایت اتحادیه%'' then 4
		when [Description] like N''%تعداد تبدیل طبق سایت اتحادیه%'' then 4
		when [Description] like N''%تعداد کیت و مخزن ارسالی%'' then 5 else 6 end) as ''rowId''
	  ,[Description]
      ,SUM([VehicleType1]) as [VehicleType1]
      ,SUM([VehicleType2]) as [VehicleType2]
      ,SUM([VehicleType3]) as [VehicleType3]
      ,SUM([VehicleType4]) as [VehicleType4]
	  ,SUM([VehicleType5]) as [VehicleType5]
	  ,SUM([VehicleTypeOther]) as [VehicleTypeOther]
	  ,sum([VehicleType1]+[VehicleType2]+[VehicleType3]+[VehicleType4]+[VehicleType5]+[VehicleTypeOther]) as ''Sum''
  FROM [CNGFAPCO].[dbo].[tbl_Contradictions]
  where WorkshopID in ('+@workshops+') and CONVERT(VARCHAR(10),Date,120) = ''' + CONVERT(VARCHAR(10),@Date,120) + '''
  GROUP BY [Description]) list
order by list.rowId'

EXECUTE(@PivotTableSQL)

SET @PivotTableSQL2 = N'
select * from (
SELECT  (case when [Description] like N''%تعداد ثبت نامی%'' then 1 when [Description] like N''%تعداد تبدیل طبق سایت فن آوران%'' then 2 
		when [Description] like N''%تعداد تبدیل طبق سایت پخش%'' then 3 when [Description] like N''%تعداد گواهی سلامت در سایت اتحادیه%'' then 4
		when [Description] like N''%تعداد تبدیل طبق سایت اتحادیه%'' then 4
		when [Description] like N''%تعداد کیت و مخزن ارسالی%'' then 5 else 6 end) as ''rowId'',
[Description],sum([VehicleType1]+[VehicleType2]+[VehicleType3]+[VehicleType4]+[VehicleType5]+[VehicleTypeOther]) as ''Sum'' FROM [CNGFAPCO].[dbo].[tbl_Contradictions]
where WorkshopID in ('+@workshops+') and CONVERT(VARCHAR(10),Date,120) = ''' + CONVERT(VARCHAR(10),@Date,120) + '''
GROUP BY [Description]) list2
order by list2.rowId'

EXECUTE(@PivotTableSQL2)

SET @PivotTableSQL3 = N'
select  sum(case list.rowId when 1 then -list.VehicleType1 else list.VehicleType1 end ) as DiffType1 ,sum(case list.rowId when 1 then -list.VehicleType2 else list.VehicleType2 end ) as DiffType2 ,
	sum(case list.rowId when 1 then -list.VehicleType3 else list.VehicleType3 end ) as DiffType3,sum(case list.rowId when 1 then -list.VehicleType4 else list.VehicleType4 end ) as DiffType4,
	sum(case list.rowId when 1 then -list.VehicleType5 else list.VehicleType5 end ) as DiffType5,sum(case list.rowId when 1 then -list.VehicleTypeOther else list.VehicleTypeOther end ) as DiffTypeOther,sum(case list.rowId when 1 then -list.Sum else list.Sum end ) as DiffSum 
from (
SELECT (case when [Description] like N''%تعداد ثبت نامی%'' then 1 when [Description] like N''%تعداد تبدیل طبق سایت فن آوران%'' then 2 
		when [Description] like N''%تعداد تبدیل طبق سایت پخش%'' then 3 when [Description] like N''%تعداد گواهی سلامت در سایت اتحادیه%'' then 4
		when [Description] like N''%تعداد تبدیل طبق سایت اتحادیه%'' then 4
		when [Description] like N''%تعداد کیت و مخزن ارسالی%'' then 5 else 6 end) as ''rowId''
      ,SUM([VehicleType1]) as [VehicleType1]
      ,SUM([VehicleType2]) as [VehicleType2]
      ,SUM([VehicleType3]) as [VehicleType3]
      ,SUM([VehicleType4]) as [VehicleType4]
	  ,SUM([VehicleType5]) as [VehicleType5]
	  ,SUM([VehicleTypeOther]) as [VehicleTypeOther]
	  ,sum([VehicleType1]+[VehicleType2]+[VehicleType3]+[VehicleType4]+[VehicleType5]+[VehicleTypeOther]) as ''Sum''
  FROM [CNGFAPCO].[dbo].[tbl_Contradictions]
  where WorkshopID in ('+@workshops+') and CONVERT(VARCHAR(10),Date,120) = ''' + CONVERT(VARCHAR(10),@Date,120) + '''
  GROUP BY [Description]) list
  where list.rowId in (1,2,6)'


EXECUTE(@PivotTableSQL3)


END
GO
/****** Object:  StoredProcedure [dbo].[sp_CreateWorkshopsDamagesInvoice]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1403/03/31>
-- Description:	<جهت ثبت اطلاعات صورتحساب فروش قطعات ضایعاتی در طرح تعویض به کارگاه ها>
-- =============================================
CREATE PROCEDURE [dbo].[sp_CreateWorkshopsDamagesInvoice] 
	-- Add the parameters for the stored procedure here
	@UserId nvarchar(max),
	@workshopId nvarchar(max),
	@invoiceCode nvarchar(max),
	@Year nvarchar(4),
	@Month nvarchar(2)
AS

DECLARE @tax_1402 float, @tax_1403 float
set @tax_1402 = 0.1;--for this query and request of project manager from 0.09 changed to 0.1
set @tax_1403 = 0.1;

BEGIN

INSERT INTO dbo.tbl_InvoicesDamages( 
	   [VehicleTypesID]
      ,[WorkshopsID]
      ,[EquipmentsID]
      ,[ServiceCode]
      ,[InvoiceCode]
      ,[CreatedDate]
      ,[EmployerEconomicalnumber]
      ,[Employerregistrationnumber]
      ,[EmployerState]
      ,[EmployerAddress]
      ,[EmployerPostalcode]
      ,[EmployerPhone]
      ,[EmployerFax]
      ,[ServiceDesc]
      ,[Number]
      ,[UnitofMeasurement]
      ,[UnitAmount]
      ,[TotalAmount]
      ,[DiscountAmount]
      ,[TotalAmountafterDiscount]
      ,[Tax]
      ,[Complications]
      ,[AmountTaxandComplications]
      ,[TotalAmountTaxandComplications]
      ,[Description]
      ,[SaleCondition]
      ,[Comment]
      ,[AcceptedAmount]
      ,[AcceptedDate]
      ,[Status]
      ,[CurrencyTypeID]
      ,[CreatorUser]
      ,[Year]
      ,[Month]) 
	  --- Columns of smaller table
        SELECT 
		NULL,
		@workshopId,
		NULL,
		list2.Literage_Text,
		@invoiceCode,		
		GETDATE(),
		NULL,
		[dbo].[GetRegistrationnumber](@workshopId),
		[dbo].[GetEconomicalnumber](@workshopId),
		NULL,
		NULL,
		NULL,
		NULL,
		N'مخزن' + ' ' + list2.Literage_Text + ' ' + N'لیتری ضایعاتی',
		list2.Count,
		N'عدد',
		list2.Price,
		list2.Price * Count,--TotalAmount
		(list2.Price * Count) * 0.2,--DiscountAmount
		((list2.Price * Count) - ((list2.Price * Count) * 0.2)),
		'0',
		'0',
		(case when @Year='1403' then Round((((list2.Price * Count) - ((list2.Price * Count) * 0.2)) * @tax_1403),0) else Round((((list2.Price * Count) - ((list2.Price * Count) * 0.2)) * @tax_1402),0) end),
		(case when @Year='1403' then Round((((list2.Price * Count) - ((list2.Price * Count) * 0.2)) * @tax_1403),0) else Round((((list2.Price * Count) - ((list2.Price * Count) * 0.2)) * @tax_1402),0) end) + ((list2.Price * Count) - ((list2.Price * Count) * 0.2)),
		N'پرداخت وجه از طریق شماره شبا IR190570034711014628807001 نزد بانک پاسارگاد بنام شرکت توسعه طرحهای صنعتی فن آوران پارسیان',
		N'نقدی',
		NULL,
		'0',
		NULL,
		NULL,
		'1',
		@UserId,
		@Year,
		@Month

		from(
		select list.Code,list.Literage_Text,list.Price,list.Title,list.WorkshopID,list.Count,list.TotalAmount,list.DiscountAmount from 
			(
				select (case when Literage=Literage_2 then [dbo].[GetDamagesDoubleCylinderCount_SameLiterage](Literage,@workshopId,@Year,@Month) + [dbo].[GetDamagesDoubleCylinderCount](Literage,@workshopId,@Year,@Month)
				else [dbo].[GetDamagesDoubleCylinderCount](Literage,@workshopId,@Year,@Month) end )as [Count]
				,Literage as Literage_Text,	WorkshopID as Code,ID as WorkshopID,Title,Year,Month,(UnitAmount * Weight) as 'Price',TotalAmount as TotalAmount, DiscountAmount as DiscountAmount
				from resultstable_IRNGVDamages left outer join tbl_Workshops
				on WorkshopID=IRNGVCod
				Group BY Literage,Literage_2,Weight,WorkshopID,ID,Title,Year,Month,UnitAmount,TotalAmount,DiscountAmount
				having ID=@workshopId and Year=@Year and Month=@Month
			) as list
				group by list.Code,list.Count,list.Literage_Text,list.Price,list.Title,list.WorkshopID,list.TotalAmount,list.DiscountAmount
			) as list2
		
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CreateWorkshopsDamagesInvoice_28L]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1403/03/31>
-- Description:	<جهت ثبت اطلاعات صورتحساب فروش قطعات ضایعاتی 28 لیتری شناسایی نشده در طرح تعویض به کارگاه ها>
-- =============================================
CREATE PROCEDURE [dbo].[sp_CreateWorkshopsDamagesInvoice_28L] 
	-- Add the parameters for the stored procedure here
	@UserId nvarchar(max),
	@workshopId nvarchar(max),
	@invoiceCode nvarchar(max),
	@Year nvarchar(4),
	@Month nvarchar(2)
AS

DECLARE @tax_1402 float, @tax_1403 float
set @tax_1402 = 0.1;--for this query and request of project manager from 0.09 changed to 0.1
set @tax_1403 = 0.1;

BEGIN

INSERT INTO dbo.tbl_InvoicesDamages( 
	   [VehicleTypesID]
      ,[WorkshopsID]
      ,[EquipmentsID]
      ,[ServiceCode]
      ,[InvoiceCode]
      ,[CreatedDate]
      ,[EmployerEconomicalnumber]
      ,[Employerregistrationnumber]
      ,[EmployerState]
      ,[EmployerAddress]
      ,[EmployerPostalcode]
      ,[EmployerPhone]
      ,[EmployerFax]
      ,[ServiceDesc]
      ,[Number]
      ,[UnitofMeasurement]
      ,[UnitAmount]
      ,[TotalAmount]
      ,[DiscountAmount]
      ,[TotalAmountafterDiscount]
      ,[Tax]
      ,[Complications]
      ,[AmountTaxandComplications]
      ,[TotalAmountTaxandComplications]
      ,[Description]
      ,[SaleCondition]
      ,[Comment]
      ,[AcceptedAmount]
      ,[AcceptedDate]
      ,[Status]
      ,[CurrencyTypeID]
      ,[CreatorUser]
      ,[Year]
      ,[Month]) 
	  --- Columns of smaller table
        SELECT 
		NULL,
		@workshopId,
		NULL,
		list2.ServiceCode,
		list2.InvoiceCode,		
		list2.CreatedDate,
		NULL,
		[dbo].[GetRegistrationnumber](@workshopId),
		[dbo].[GetEconomicalnumber](@workshopId),
		NULL,
		NULL,
		NULL,
		NULL,
		list2.ServiceDesc,
		list2.Count,
		N'عدد',
		list2.Price,
		list2.Price * Count,--TotalAmount
		(list2.Price * Count) * 0.2,--DiscountAmount
		((list2.Price * Count) - ((list2.Price * Count) * 0.2)),
		'0',
		'0',
		(case when @Year='1403' then Round((((list2.Price * Count) - ((list2.Price * Count) * 0.2)) * @tax_1403),0) else Round((((list2.Price * Count) - ((list2.Price * Count) * 0.2)) * @tax_1402),0) end),
		(case when @Year='1403' then Round((((list2.Price * Count) - ((list2.Price * Count) * 0.2)) * @tax_1403),0) else Round((((list2.Price * Count) - ((list2.Price * Count) * 0.2)) * @tax_1402),0) end) + ((list2.Price * Count) - ((list2.Price * Count) * 0.2)),
		N'پرداخت وجه از طریق شماره شبا IR190570034711014628807001 نزد بانک پاسارگاد بنام شرکت توسعه طرحهای صنعتی فن آوران پارسیان',
		N'نقدی',
		NULL,
		'0',
		NULL,
		NULL,
		'1',
		@UserId,
		@Year,
		@Month

		from(
				select VehicleTypesID, WorkshopsID, EquipmentsID,'28' as ServiceCode,InvoiceCode,CreatedDate,EmployerEconomicalnumber,Employerregistrationnumber
				,EmployerState,EmployerAddress,EmployerPostalcode,EmployerPhone,EmployerFax,N'مخزن' + ' ' + '28' + ' ' + N'لیتری ضایعاتی' as ServiceDesc
				,Number as Count,UnitofMeasurement,(Price * 29) as Price
				from
				(
				select [dbo].[GetReplacementDamagesPrice](@Year,@Month) as Price, * from tbl_InvoicesDamages
				where WorkshopsID=@workshopId and Year=@Year and Month=@Month and ServiceCode='20'
				) as list
			) as list2
		
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CreateWorkshopsDifferenceInvoice]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1401/06/11>
-- Description:	<صدور صورتحساب مابه التفاوت دستمزد تبدیل کارگاه ها>
-- =============================================
CREATE PROCEDURE [dbo].[sp_CreateWorkshopsDifferenceInvoice] 
	-- Add the parameters for the stored procedure here
	@UserId nvarchar(max),
	@workshopId nvarchar(max),
	@invoiceCode nvarchar(max),
	@fromdate nvarchar(max),
	@todate nvarchar(max)
AS


BEGIN

INSERT INTO dbo.tbl_InvoicesFapa([VehicleTypesID]
      ,[WorkshopsID]
      ,[EquipmentsID]
      ,[ServiceCode]
      ,[InvoiceCode]
      ,[CreatedDate]
      ,[EmployerEconomicalnumber]
      ,[Employerregistrationnumber]
      ,[EmployerState]
      ,[EmployerAddress]
      ,[EmployerPostalcode]
      ,[EmployerPhone]
      ,[EmployerFax]
      ,[ServiceDesc]
      ,[Number]
      ,[UnitofMeasurement]
      ,[UnitAmount]
      ,[TotalAmount]
      ,[DiscountAmount]
      ,[TotalAmountafterDiscount]
      ,[Tax]
      ,[Complications]
      ,[AmountTaxandComplications]
      ,[TotalAmountTaxandComplications]
      ,[Description]
      ,[SaleCondition]
      ,[Comment]
      ,[AcceptedAmount]
      ,[AcceptedDate]
      ,[Status]
      ,[CurrencyTypeID]
      ,[CreatorUser]) 
	  --- Columns of smaller table
        SELECT list2.VehicleTypeID,
		@workshopId,
		NULL,
		list2.VehicleTypeID,
		@invoiceCode,
		--[dbo].[PersianToMiladi](@todate),
		--[dbo].[PersianToMiladi](@fromdate),
		CONVERT(VARCHAR(10),@todate,120), --GETDATE(),
		CONVERT(VARCHAR(10),@fromdate,120),
		[dbo].[GetRegistrationnumber](@workshopId),
		[dbo].[GetEconomicalnumber](@workshopId),
		NULL,
		NULL,
		NULL,
		NULL,
		list2.Type,
		list2.Count,
		N'دستگاه',
		list2.Price,
		(CONVERT(float,list2.Count) * list2.Price) as 'Salary' ,
		'0',
		(CONVERT(float,list2.Count) * list2.Price),
		'0',
		'0',
		(case when [dbo].[GetWorkshopEconomicStatus](@workshopId) > 0 then (CONVERT(float,list2.Count) * list2.Price * [dbo].[GetValueAdded](list2.CreateYear)) else 0 end),		
		(CONVERT(float,(case when [dbo].[GetWorkshopEconomicStatus](@workshopId) > 0 then (CONVERT(float,list2.Count) * list2.Price * [dbo].[GetValueAdded](list2.CreateYear)) else 0 end))) + (CONVERT(float,list2.Count) * list2.Price),
		--(CONVERT(float,list2.Count) * list2.Price * 0.09),
		--(CONVERT(float,list2.Count) * list2.Price * 0.09) + (CONVERT(float,list2.Count) * list2.Price),
		N'مربوط به مابه التفاوت دستمزد تبدیل می باشد.',
		N'غیر نقدی',
		NULL,
		'0',
		NULL,
		NULL,
		'1',
		@UserId
		from (
		select list.VehicleTypeID,list.Type,SUM(list.Count) as Count,(list.Price) as Price,list.CreateYear from (
		select LEFT(FORMAT(CreateDate, 'yyyy/MM/dd-HH:mm:ss', 'fa'),4) as CreateYear,VehicleTypeID,(Type + ' ' + tbl_VehicleTypes.Description) Type, COUNT(*) as 'Count',[dbo].[GetDifferenceRegistrationPrice](WorkshopID,Type,CreateDate) as 'Price' from tbl_VehicleRegistrations inner join tbl_VehicleTypes on tbl_VehicleRegistrations.VehicleTypeID=tbl_VehicleTypes.ID
		where RegisterStatus=1 and WorkshopID=@workshopId and CONVERT(VARCHAR(10),CreateDate,120) BETWEEN '' + CONVERT(VARCHAR(10),@fromdate,120) + '' AND '' +  CONVERT(VARCHAR(10),@todate,120)+ ''
		group by VehicleTypeID,Type,WorkshopID,CreateDate,tbl_VehicleTypes.Description,LEFT(CONVERT(VARCHAR(10),FORMAT(CreateDate, 'yyyy/MM/dd-HH:mm:ss', 'fa'),120),4)) as list
		group by list.VehicleTypeID,list.Type,list.Price,list.CreateYear) as list2		
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CreateWorkshopsInvoices_DamagesCylinder]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1403/07/12>
-- Description:	<صدور دوره ای صورتحساب دستمزد تعویض کارگاه ها>
-- =============================================
CREATE PROCEDURE [dbo].[sp_CreateWorkshopsInvoices_DamagesCylinder] 
	-- Add the parameters for the stored procedure here
	@UserId nvarchar(max),
	@workshopId nvarchar(max),
	@invoiceCode nvarchar(max),
	@fromdate nvarchar(max),
	@todate nvarchar(max)

AS


BEGIN

INSERT INTO dbo.tbl_InvoicesFapa_DamagesCylinder([VehicleTypesID]
      ,[WorkshopsID]
      ,[EquipmentsID]
      ,[ServiceCode]
      ,[InvoiceCode]
      ,[CreatedDate]
      ,[EmployerEconomicalnumber]
      ,[Employerregistrationnumber]
      ,[EmployerState]
      ,[EmployerAddress]
      ,[EmployerPostalcode]
      ,[EmployerPhone]
      ,[EmployerFax]
      ,[ServiceDesc]
      ,[Number]
      ,[UnitofMeasurement]
      ,[UnitAmount]
      ,[TotalAmount]
      ,[DiscountAmount]
      ,[TotalAmountafterDiscount]
      ,[Tax]
      ,[Complications]
      ,[AmountTaxandComplications]
      ,[TotalAmountTaxandComplications]
      ,[Description]
      ,[SaleCondition]
      ,[Comment]
      ,[AcceptedAmount]
      ,[AcceptedDate]
      ,[Status]
      ,[CurrencyTypeID]
      ,[CreatorUser]) 
	  --- Columns of smaller table
        SELECT list2.VehicleTypeID,
		@workshopId,
		NULL,
		list2.VehicleTypeID,
		@invoiceCode,
		CONVERT(VARCHAR(10),@todate,120), --GETDATE(),
		CONVERT(VARCHAR(10),@fromdate,120),
		[dbo].[GetRegistrationnumber](@workshopId),
		[dbo].[GetEconomicalnumber](@workshopId),
		NULL,
		NULL,
		NULL,
		NULL,
		N'تعویض مخزن' + ' ' + list2.Type,
		list2.Count,
		N'دستگاه',
		list2.Price,
		(CONVERT(float,list2.Count) * list2.Price) as 'Salary' ,
		'0',
		(CONVERT(float,list2.Count) * list2.Price),
		'0',
		'0',		
		(case when [dbo].[GetWorkshopEconomicStatus](@workshopId) > 0 then (CONVERT(float,list2.Count) * list2.Price * [dbo].[GetValueAdded](list2.CreateYear)) else 0 end),		
		(CONVERT(float,(case when [dbo].[GetWorkshopEconomicStatus](@workshopId) > 0 then (CONVERT(float,list2.Count) * list2.Price * [dbo].[GetValueAdded](list2.CreateYear)) else 0 end))) + (CONVERT(float,list2.Count) * list2.Price),
		NULL,
		--(CONVERT(float,list2.Count) * list2.Price * [dbo].[GetValueAdded](list2.CreateYear)),
		--(CONVERT(float,list2.Count) * list2.Price * [dbo].[GetValueAdded](list2.CreateYear)) + (CONVERT(float,list2.Count) * list2.Price),
		--NULL,--list2.Description,
		N'غیر نقدی',
		NULL,
		'0',
		NULL,
		NULL,
		'1',
		@UserId
		from (
		select list.VehicleTypeID,list.Type,SUM(list.Count) as Count,(list.Price) as Price,list.CreateYear from (
		select Year as 'CreateYear','1' as VehicleTypeID,VehicleType as 'Type',COUNT(*) as 'Count', '3000000' as 'Price'
		FROM [CNGFAPCO].[dbo].[resultstable_IRNGVDamages] inner join tbl_Workshops on resultstable_IRNGVDamages.WorkshopID=tbl_Workshops.IRNGVCod
		where tbl_Workshops.ID = @workshopId and CONVERT(VARCHAR(10),miladiDate,120) BETWEEN '' + CONVERT(VARCHAR(10),@fromdate,120) + '' AND '' +  CONVERT(VARCHAR(10),@todate,120)+ ''
		group by VehicleType,WorkshopID,Year,LEFT(CONVERT(VARCHAR(10),FORMAT(miladiDate, 'yyyy/MM/dd-HH:mm:ss', 'fa'),120),4)) as list
		group by list.VehicleTypeID,list.Type,list.Price,list.CreateYear) as list2

END

--select list.VehicleTypeID,list.Type,SUM(list.Count) as Count,(list.Price) as Price,list.CreateYear,list.Description from (
--		select Year as 'CreateYear','1' as VehicleTypeID,VehicleType as 'Type',COUNT(*) as 'Count', '3000000' as 'Price',resultstable_IRNGVDamages.Literage_Text as 'Description'
--		FROM [CNGFAPCO].[dbo].[resultstable_IRNGVDamages] inner join tbl_Workshops on resultstable_IRNGVDamages.WorkshopID=tbl_Workshops.IRNGVCod
--		where tbl_Workshops.ID = @workshopId and CONVERT(VARCHAR(10),miladiDate,120) BETWEEN '' + CONVERT(VARCHAR(10),@fromdate,120) + '' AND '' +  CONVERT(VARCHAR(10),@todate,120)+ ''
--		group by Literage_Text,VehicleType,WorkshopID,Year,LEFT(CONVERT(VARCHAR(10),FORMAT(miladiDate, 'yyyy/MM/dd-HH:mm:ss', 'fa'),120),4)) as list
--		group by list.VehicleTypeID,list.Type,list.Price,list.CreateYear,list.Description) as list2
GO
/****** Object:  StoredProcedure [dbo].[sp_CRMOneView]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1400/07/15>
-- Description:	مشاهده اطلاعات نظرات شاخص ها در یک نگاه
-- =============================================
CREATE PROCEDURE [dbo].[sp_CRMOneView] 
	-- Add the parameters for the stored procedure here
	--@id nvarchar(max)
AS


BEGIN

DECLARE @PivotColumnHeaders NVARCHAR(MAX)

  SELECT @PivotColumnHeaders= ISNULL(@PivotColumnHeaders + ',','') 
       + QUOTENAME(ID)
	FROM (SELECT DISTINCT ID FROM tbl_CRMIndexes where Presentable=1 and PId>0) AS Titles

DECLARE @PivotTableSQL NVARCHAR(MAX)

SET @PivotTableSQL = 'select * from (
 select '' '''+ N'#'+'
	  ,[IndexId] as Title 
	  ,ISNULL(SUM(Point),0) as Value
	  ,[dbo].[GetWorkshops](v.WorkshopID)'+ N'کارگاه'+'
      ,(v.OwnerName+ '' '' + v.OwnerFamily) '+ N'مالک_خودرو'+'
	  ,(convert(date,v.CreateDate)) '+ N'تاریخ_تبدیل'+'
	  ,(convert(date,c.[CreateDate])) '+ N'تاریخ_نظر'+'
      ,SUBSTRING(Description1,0,1000) '+ N'توضیح1'+'
      ,SUBSTRING(Description2,0,1000) '+ N'توضیح2'+'
      ,SUBSTRING([Suggestion],0,1000) '+ N'پیشنهاد'+'
		FROM  tbl_CRMs as c
		inner join tbl_CRMIndexes on c.IndexId=tbl_CRMIndexes.ID
		inner join tbl_VehicleRegistrations as v on c.OwnersId=v.ID
		group by Description1,Description2,Suggestion,c.[CreateDate],IndexId,v.OwnerName,v.OwnerFamily,v.WorkshopID,v.CreateDate) as se
  PIVOT (
    SUM(Value)
    FOR se.Title IN (' + @PivotColumnHeaders +'
    )
  ) AS PivotTable
'

EXECUTE(@PivotTableSQL)

END

GO
/****** Object:  StoredProcedure [dbo].[sp_CRMViewComments]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1399/08/30>
-- Description:	<مشاهده لیست شرکت کنندگان در نظرسنجی>
-- =============================================
CREATE PROCEDURE [dbo].[sp_CRMViewComments] 
	-- Add the parameters for the stored procedure here
	--@id nvarchar(max)
AS


BEGIN

select OwnersId,(tbl_VehicleRegistrations.OwnerName + ' ' + tbl_VehicleRegistrations.OwnerFamily) as FullName,CONVERT(date, tbl_CRMs.CreateDate) as CreateDate,
		CONVERT(date, tbl_VehicleRegistrations.CreateDate) as RegistrationDate,[dbo].[GetWorkshops](WorkshopID) as Workshop, [dbo].[GetVehicleType](VehicleTypeID) as VehicleType
  FROM [CNGFAPCO].[dbo].[tbl_CRMs] inner join tbl_VehicleRegistrations on tbl_CRMs.OwnersId=tbl_VehicleRegistrations.ID
 -- where OwnersId=@id
  group by OwnersId,CONVERT(date, tbl_CRMs.CreateDate),tbl_VehicleRegistrations.OwnerName,tbl_VehicleRegistrations.OwnerFamily,tbl_VehicleRegistrations.WorkshopID,tbl_VehicleRegistrations.VehicleTypeID,CONVERT(date, tbl_VehicleRegistrations.CreateDate)
  order by CONVERT(date, tbl_CRMs.CreateDate) desc

END

GO
/****** Object:  StoredProcedure [dbo].[sp_CustomerRequestFreeSaleList]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1400/03/27>
-- Description:	مشاهده لیست درخواست های اشخاص حقیقی برای خرید اقلام در طرح فروش آزاد
-- =============================================
CREATE PROCEDURE [dbo].[sp_CustomerRequestFreeSaleList] 
	-- Add the parameters for the stored procedure here
	

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN

SET @PivotTableSQL = N'select ISNULL([dbo].[GetCustomers](CustomersID),'''') as Customers,InvoiceCode,CONVERT(date, CreatedDate) as CreatedDate,CONVERT(VARCHAR(5), CreatedDate, 108) as Time,SUM(CONVERT(int, Number)) as Number,UnitofMeasurement,SUM(CONVERT(float,TotalAmount)) as TotalAmount,SUM(CONVERT(float,DiscountAmount)) as DiscountAmount,
Status , (case when Status=0 then ''#f7818c'' else ''#82f59d'' end) ViewStatusColor,SaleCondition
FROM [CNGFAPCO].[dbo].[tbl_CustomerRequests]
GROUP BY CustomersID,InvoiceCode,CONVERT(date, CreatedDate),CONVERT(VARCHAR(5), CreatedDate, 108),UnitofMeasurement,Status,SaleCondition
ORDER BY CONVERT(date, CreatedDate),CONVERT(VARCHAR(5), CreatedDate, 108) desc
'


EXECUTE(@PivotTableSQL)

END
GO
/****** Object:  StoredProcedure [dbo].[sp_CutoffValveList]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1400/02/03>
-- Description:	مشاهده اطلاعات ثبت شده در بانک شیر ‍قطع کن
-- =============================================
CREATE PROCEDURE [dbo].[sp_CutoffValveList] 
	-- Add the parameters for the stored procedure here
	@workshops nvarchar(max)=null,
	@status nvarchar(max)=null,
	@FromDate DATETIME ,
	@ToDate DATETIME
	

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN
SET @PivotTableSQL = N'
SELECT * FROM(
SELECT [ID]
      ,[serialNumber]
      ,[constractor]
      ,[model]      
      ,[productDate]
      ,(CASE WHEN datalength(workshop)<10 THEN [dbo].[GetWorkshops]([workshop]) else [workshop] END) as [workshop]
      ,[status]
	  ,(case when [status]=N''نیاز به بررسی'' then 2 else 1 end) as [statusCode]
	  ,(case when [status]=N''نیاز به بررسی'' then ''red'' else ''green'' end) as [statusColor]
      ,[CreateDate]
      ,[Creator]
	  ,[RefreshDate]
  FROM [CNGFAPCO].[dbo].[tbl_CutofValves]
  where workshop in ('+@workshops+') and CONVERT(VARCHAR(10),CreateDate,120) BETWEEN ''' + CONVERT(VARCHAR(10),@FromDate,120) + ''' AND ''' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''' 
  ) AS List
  Where List.[statusCode] in ('+@status+')
  order by List.ID desc
  '

EXECUTE(@PivotTableSQL)

END
--(CASE WHEN datalength(constractor)<10 THEN [dbo].[GetCutofValveConstractors]([constractor]) else constractor END) as [constractor]
     -- ,(CASE WHEN datalength(model)<10 THEN [dbo].[GetCutoffValveModels]([constractor]) else model END) as [model]      
--smaple check numeric
--(CASE WHEN constractor LIKE '%[^0-9]%' THEN [dbo].[GetWorkshops]([workshop]) else constractor END) as [workshop]
GO
/****** Object:  StoredProcedure [dbo].[sp_CutoffValveList_Bank]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1400/02/03>
-- Description:	مشاهده اطلاعات ثبت شده در بانک شیر ‍قطع کن
-- =============================================
CREATE PROCEDURE [dbo].[sp_CutoffValveList_Bank] 
	-- Add the parameters for the stored procedure here
	@FromDate DATETIME ,
	@ToDate DATETIME
	

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN
SET @PivotTableSQL = N'
SELECT * FROM(
SELECT [ID]
      ,[serialNumber]
      ,[constractor]
      ,[model]      
      ,[productDate]
      ,[workshop]
      ,[status]	 
      ,[CreateDate]
      ,[Creator]
  FROM [CNGFAPCO].[dbo].[tbl_BankCutofValves]
  where CONVERT(VARCHAR(10),CreateDate,120) BETWEEN ''' + CONVERT(VARCHAR(10),@FromDate,120) + ''' AND ''' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''' 
  ) AS List
  order by List.ID desc
  '

EXECUTE(@PivotTableSQL)

END
--smaple check numeric
--(CASE WHEN constractor LIKE '%[^0-9]%' THEN [dbo].[GetWorkshops]([workshop]) else constractor END) as [workshop]
GO
/****** Object:  StoredProcedure [dbo].[sp_CylinderList]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1400/01/18>
-- Description:	مشاهده اطلاعات ثبت شده در بانک مخزن
-- =============================================
CREATE PROCEDURE [dbo].[sp_CylinderList] 
	-- Add the parameters for the stored procedure here
	@workshops nvarchar(max)=null,
	@status nvarchar(max)=null,
	@FromDate DATETIME ,
	@ToDate DATETIME
	

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN
SET @PivotTableSQL = N'
SELECT * FROM(
SELECT [ID]
	  ,[bulk]
      ,[model]
      ,[type]
      ,[length]
      ,[pressure]
      ,[diameter]
      ,[rezve]
      ,[productDate]
      ,[expireDate]
      ,[serialNumber]
      ,(CASE WHEN datalength(constractor)<10 THEN [dbo].[GetCylinderConstractors]([constractor]) else constractor END) as [constractor]      
      ,(CASE WHEN datalength(workshop)<10 THEN [dbo].[GetWorkshops]([workshop]) else [workshop] END) as [workshop]
      ,[status]
	  ,(case when [status]=N''نیاز به بررسی'' then 2 else 1 end) as [statusCode]
	  ,(case when [status]=N''نیاز به بررسی'' then ''red'' else ''green'' end) as [statusColor]
      ,[CreateDate]
      ,[Creator]
	  ,[RefreshDate]
  FROM [CNGFAPCO].[dbo].[tbl_Tanks]
  where workshop in ('+@workshops+') and CONVERT(VARCHAR(10),CreateDate,120) BETWEEN ''' + CONVERT(VARCHAR(10),@FromDate,120) + ''' AND ''' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''' 
  ) AS List
  Where List.[statusCode] in ('+@status+')
  order by List.ID desc
  '

EXECUTE(@PivotTableSQL)

END
GO
/****** Object:  StoredProcedure [dbo].[sp_CylinderList_Bank]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1400/01/18>
-- Description:	مشاهده اطلاعات ثبت شده در بانک مخزن
-- =============================================
CREATE PROCEDURE [dbo].[sp_CylinderList_Bank] 
	-- Add the parameters for the stored procedure here
	--@workshops nvarchar(max)=null,
	@FromDate DATETIME ,
	@ToDate DATETIME
	

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN
SET @PivotTableSQL = N'
SELECT [ID]
	  ,[bulk]
      ,[model]
      ,[type]
      ,[length]
      ,[pressure]
      ,[diameter]
      ,[rezve]
      ,[productDate]
      ,[expireDate]
      ,[serialNumber]
      ,[constractor]      
      ,[workshop]      
      ,[CreateDate]
      ,[Creator]
  FROM [CNGFAPCO].[dbo].[tbl_BankTanks]
  where  CONVERT(VARCHAR(10),CreateDate,120) BETWEEN ''' + CONVERT(VARCHAR(10),@FromDate,120) + ''' AND ''' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''' 
  order by ID desc
  '

EXECUTE(@PivotTableSQL)

END
--workshop in ('+@workshops+') and
GO
/****** Object:  StoredProcedure [dbo].[sp_DivisionPlan]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1399/05/19>
-- Description:	مشاهده فاکتور دستمزد تبدیل کارگاه ها
-- =============================================
CREATE PROCEDURE [dbo].[sp_DivisionPlan] 
	-- Add the parameters for the stored procedure here
	@Workshops nvarchar(max)=null
	

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN

SET @PivotTableSQL = N'
SELECT tbl1.[ID]
      ,tbl1.[Code]
      ,[dbo].[GetWorkshops](tbl1.[WorkshopID]) WorkshopTitle
      ,tbl1.[CreateDate]
      ,[dbo].[GetUsers](tbl1.[Creator]) Creator
      ,isnull(tbl1.[Confirmation],0) Confirmation
      ,[dbo].[GetUsers](tbl1.[ConfirmationUser]) ConfirmationUser
      ,isnull(tbl1.[Send],0) Send
      ,tbl1.[SendDate]
      ,[dbo].[GetUsers](tbl1.[Sender]) Sender
      ,tbl1.[Description]
      ,tbl1.[ConfirmationDate]
      ,isnull(tbl1.[FinalCheck],0) FinalCheck
      ,tbl1.[FinalCheckDate]
	  ,(CASE when tbl1.ID >=2373 then ''PreviewBOM'' else ''Preview'' end  ) ExistBOM
  FROM [CNGFAPCO].[dbo].[tbl_DivisionPlans] tbl1
  where tbl1.WorkshopID in ('+@Workshops+') 
  order by tbl1.ID desc'

EXECUTE(@PivotTableSQL)

END
---------------changed in 14010110---------------
---(CASE when [dbo].[GetExistBOM](tbl1.ID)>=2373 then ''PreviewBOM'' else ''Preview'' end  ) ExistBOM
-----changed in 13991121---------------------------
--(CASE when [dbo].[GetExistBOM](tbl1.ID)>0 and [dbo].[GetExistBOM](tbl1.ID)>=2373 then ''PreviewBOM'' else ''Preview'' end  ) ExistBOM
GO
/****** Object:  StoredProcedure [dbo].[sp_DivisionPlanwithBOMs]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1399/09/16>
-- Description:	<چک کردن موجودی کالا در انبار و صدور طرح تقسیم در مدل BOM>
-- =============================================
CREATE PROCEDURE [dbo].[sp_DivisionPlanwithBOMs] 
	-- Add the parameters for the stored procedure here
	@VehicleTypeID nvarchar(max),
	@GenerationID nvarchar(max)
AS


BEGIN

select tbl_EquipmentList.FinancialCode,[dbo].[GetEquipmentListParent](tbl_EquipmentList.Pid) ParentTitle,tbl_EquipmentList.Title, (ISNULL([dbo].[GetRemWarehouses](tbl_EquipmentList.FinancialCode),0)) Rem,(ISNULL([dbo].[GetCurrRemWarehouses](tbl_EquipmentList.FinancialCode),0)) CurrRem,
		tbl_BOMs.Ratio,tbl_BOMs.Unit
from tbl_BOMs inner join tbl_EquipmentList 
			on tbl_BOMs.EquipmentListID=tbl_EquipmentList.ID 
where (VehicleTypeID=@VehicleTypeID or @VehicleTypeID=0) and (GenerationID=@GenerationID or @GenerationID=0)
group by tbl_EquipmentList.FinancialCode,tbl_EquipmentList.Pid,tbl_EquipmentList.Title,tbl_BOMs.Ratio,tbl_BOMs.Unit
--------------------------------olde version with vehicle type-----------------------------------------------------------------------------------------
--select [dbo].[GetEquipmentListParent](tbl_EquipmentList.Pid) ParentTitle, tbl_EquipmentList.Title,tbl_EquipmentList.FinancialCode,[dbo].[GetVehicleType](VehicleTypeID) Type,tbl_BOMs.Ratio,tbl_BOMs.Unit,
--SUM(ISNULL([dbo].[GetRemWarehouses](tbl_EquipmentList.FinancialCode),0)) Rem,SUM(ISNULL([dbo].[GetCurrRemWarehouses](tbl_EquipmentList.FinancialCode),0)) CurrRem
--from tbl_BOMs inner join tbl_EquipmentList 
--			on tbl_BOMs.EquipmentListID=tbl_EquipmentList.ID 
--where VehicleTypeID=@VehicleTypeID or @VehicleTypeID=0
--group by tbl_EquipmentList.Pid,tbl_EquipmentList.Title,tbl_EquipmentList.FinancialCode,tbl_BOMs.VehicleTypeID,tbl_BOMs.Ratio,tbl_BOMs.Unit


END

GO
/****** Object:  StoredProcedure [dbo].[sp_FillingValveList]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1400/02/03>
-- Description:	مشاهده اطلاعات ثبت شده در بانک شیر ‍پرکن
-- =============================================
CREATE PROCEDURE [dbo].[sp_FillingValveList] 
	-- Add the parameters for the stored procedure here
	@workshops nvarchar(max)=null,
	@status nvarchar(max)=null,
	@FromDate DATETIME ,
	@ToDate DATETIME
	

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN
SET @PivotTableSQL = N'
SELECT * FROM(
SELECT [ID]
      ,[serialNumber]
      ,(CASE WHEN datalength(constractor)<10 THEN [dbo].[GetFillingValveConstractors]([constractor]) else constractor END) as [constractor]
      ,(CASE WHEN datalength(model)<10 THEN [dbo].[GetFillingValveModels]([constractor]) else model END) as [model]      
      ,[productDate]
      ,(CASE WHEN datalength(workshop)<10 THEN [dbo].[GetWorkshops]([workshop]) else [workshop] END) as [workshop]
      ,[status]
	  ,(case when [status]=N''نیاز به بررسی'' then 2 else 1 end) as [statusCode]
	  ,(case when [status]=N''نیاز به بررسی'' then ''red'' else ''green'' end) as [statusColor]
      ,[CreateDate]
      ,[Creator]
	  ,[RefreshDate]
  FROM [CNGFAPCO].[dbo].[tbl_FillingValves]
  where workshop in ('+@workshops+') and CONVERT(VARCHAR(10),CreateDate,120) BETWEEN ''' + CONVERT(VARCHAR(10),@FromDate,120) + ''' AND ''' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''' 
  ) AS List
  Where List.[statusCode] in ('+@status+')
  order by List.ID desc
  '

EXECUTE(@PivotTableSQL)

END
--smaple check numeric
--(CASE WHEN constractor LIKE '%[^0-9]%' THEN [dbo].[GetWorkshops]([workshop]) else constractor END) as [workshop]
GO
/****** Object:  StoredProcedure [dbo].[sp_FillingValveList_Bank]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1400/02/03>
-- Description:	مشاهده اطلاعات ثبت شده در بانک شیر ‍پر کن
-- =============================================
CREATE PROCEDURE [dbo].[sp_FillingValveList_Bank] 
	-- Add the parameters for the stored procedure here
	@FromDate DATETIME ,
	@ToDate DATETIME
	

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN
SET @PivotTableSQL = N'
SELECT * FROM(
SELECT [ID]
      ,[serialNumber]
      ,[constractor]
      ,[model]      
      ,[productDate]
      ,[workshop]
      ,[status]	 
      ,[CreateDate]
      ,[Creator]
  FROM [CNGFAPCO].[dbo].[tbl_BankFillingValves]
  where CONVERT(VARCHAR(10),CreateDate,120) BETWEEN ''' + CONVERT(VARCHAR(10),@FromDate,120) + ''' AND ''' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''' 
  ) AS List
  order by List.ID desc
  '

EXECUTE(@PivotTableSQL)

END
--smaple check numeric
--(CASE WHEN constractor LIKE '%[^0-9]%' THEN [dbo].[GetWorkshops]([workshop]) else constractor END) as [workshop]
GO
/****** Object:  StoredProcedure [dbo].[sp_FinallFreeSaleInvoiceList]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1400/03/15>
-- Description:	مشاهده لیست درخواست های نهایی شده و فاکتور شده کارگاه ها برای خرید اقلام در طرح فروش آزاد
-- =============================================
CREATE PROCEDURE [dbo].[sp_FinallFreeSaleInvoiceList] 
	-- Add the parameters for the stored procedure here
	

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN

SET @PivotTableSQL = N'select [dbo].[GetWorkshops](WorkshopsID) as Workshops,InvoiceCode,CONVERT(date, CreatedDate) as CreatedDate,CONVERT(VARCHAR(5), CreatedDate, 108) as Time,SUM(CONVERT(int, Number)) as Number,UnitofMeasurement,SUM(CONVERT(float,TotalAmountTaxandComplications)) as TotalAmount,
(case when Status=0 then ''#f7818c'' else ''#82f59d'' end) ViewStatusColor,SaleCondition,[dbo].[GetInvoiceStatusPaied](InvoiceCode,WorkshopsID) as StatusPaied,(CASE when [dbo].[GetInvoiceStatusRemittance](InvoiceCode) IS NULL then ''false'' else ''true'' end) as Status,[dbo].[GetPreInvoiceCode](InvoiceCode) as PreInvoiceCode
FROM [CNGFAPCO].[dbo].[tbl_FinallFreeSaleInvoices]
GROUP BY WorkshopsID,InvoiceCode,CONVERT(date, CreatedDate),CONVERT(VARCHAR(5), CreatedDate, 108),UnitofMeasurement,Status,SaleCondition
ORDER BY CONVERT(date, CreatedDate),CONVERT(VARCHAR(5), CreatedDate, 108) desc
'

EXECUTE(@PivotTableSQL)

END
GO
/****** Object:  StoredProcedure [dbo].[sp_FinancialAssesment]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1401/04/19>
-- Description:	خلاصه وضعیت عملکرد تایید شده 
-- =============================================
Create PROCEDURE [dbo].[sp_FinancialAssesment] 
	-- Add the parameters for the stored procedure here
	@workshops nvarchar(max)=null,
	@FromDate DATETIME ,
	@ToDate DATETIME
	

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN

-----------------------------------------------------------------------financial credit and debit ------------------------------------------------------------------------------
select *,(CONVERT(float,list.Creditor) - CONVERT(float,list.Debtor)) as 'Rem' from (
select ID,Title,ISNULL([dbo].[GetFinancialCreditor_Assesment](ID,@workshops,@FromDate,@ToDate),0) as 'Creditor',ISNULL([dbo].[GetFinancialDebtor](ID,@workshops,@FromDate,@ToDate),0) as 'Debtor'
FROM [CNGFAPCO].[dbo].[tbl_FinancialDescs] 
where ID in (1,2,3,6)
) as list
-----------------------------------------------------------------------vehicle registration count with details and type --------------------------------------------------------
select list.Type,SUM(list.Count) as Count,(list.Price) as Price,SUM((CONVERT(float,list.Count) * list.Price)) as 'Salary' from (
select (Type + ' - ' + tbl_VehicleTypes.Description) as 'Type', COUNT(*) as 'Count', [dbo].[GetRegistrationPrice](WorkshopID,Type,CreateDate) as 'Price' from tbl_VehicleRegistrations inner join tbl_VehicleTypes on tbl_VehicleRegistrations.VehicleTypeID=tbl_VehicleTypes.ID
where FinancialStatus=1 and (WorkshopID=@workshops or @workshops =0) and CONVERT(VARCHAR(10),CreateDate,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
group by Type,WorkshopID,CreateDate,tbl_VehicleTypes.Description) as list 
group by list.Type,list.Price
-----------------------------------------------------------------------financial credit and debit and Rem in OneView ------------------------------------------------------------
--select *, CEILING(((CONVERT(float,list3.Debtor) - CONVERT(float,list3.NonCash))/ (case CONVERT(float,list3.Creditor) when 0 then 1 else CONVERT(float,list3.Creditor) end)  * 100)) as 'NetPercent', CEILING(((CONVERT(float,list3.Debtor) - CONVERT(float,list3.NonCash))/ (  case (CONVERT(float,list3.Creditor)*0.7333 ) when 0 then 1 else (CONVERT(float,list3.Creditor)*0.7333 ) end )* 100)) as 'GrossPercent',
--	list3.PreInvoiceHint
--from (
--select *,(CONVERT(float,list2.Creditor) - CONVERT(float,list2.Deductions) - CONVERT(float,list2.Debtor) - CONVERT(float,list2.NonCash)) as 'RemWithDeductions' from(
--select *,(CONVERT(float,list.Creditor) - CONVERT(float,list.Debtor) - CONVERT(float,list.NonCash)) as 'Rem', ((CONVERT(float,list.Creditor) * 0.2667)) as 'Deductions' from (
--select ID,Title,ISNULL([dbo].[GetFinancialCreditor](1,ID,@FromDate,@ToDate),0) as 'Creditor',ISNULL([dbo].[GetFinancialDebtor](1,ID,@FromDate,@ToDate),0) as 'Debtor',
--		ISNULL([dbo].[GetFinancialDebtor](6,ID,@FromDate,@ToDate),0) as 'NonCash',ISNULL([dbo].[GetCheckedPreInvoice](ID),0) as 'Amount',ISNULL([dbo].[GetPreInvoiceHint](ID),0) as 'PreInvoiceHint',
--		ISNULL([dbo].[GetOfferedPrice](ID),0) as 'OfferedPrice',ISNULL([dbo].[GetOfferedSerial](),'-') as 'OfferedSerial',[dbo].[GetOfferedID](ID) as 'OfferedID'
--FROM [CNGFAPCO].[dbo].[tbl_Workshops]
--where isServices>0 and (ID=@workshops or @workshops =0)
--) as list ) as list2 ) as list3

-----------------------------------------------------------------------Audits Price registration count with details  --------------------------------------------------------
--select SUM(list.Count) as Count,(list.AuditsPrice) as AuditsPrice,SUM((CONVERT(float,list.Count) * list.AuditsPrice)) as 'Salary' from (
--select  COUNT(*) as 'Count', [dbo].[GetAuditsPrice](CompaniesID,tbl_VehicleRegistrations.CreateDate) as 'AuditsPrice' from tbl_VehicleRegistrations inner join tbl_VehicleTypes on tbl_VehicleRegistrations.VehicleTypeID=tbl_VehicleTypes.ID inner join
--tbl_Workshops on tbl_VehicleRegistrations.WorkshopID=tbl_Workshops.ID
--where RegisterStatus=1 and (WorkshopID=@workshops or @workshops =0) and CONVERT(VARCHAR(10),tbl_VehicleRegistrations.CreateDate,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
--group by Type,WorkshopID,tbl_VehicleRegistrations.CreateDate,tbl_VehicleTypes.Description,CompaniesID) as list 
--group by list.AuditsPrice

END
GO
/****** Object:  StoredProcedure [dbo].[sp_FinancialAssesment_Vehicle]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1399/08/14>
-- Changed in date:<1404/01/30> add @type parameter for suport two section registration and damage vehicle invoices....
-- Description:	<انتخاب خودروهایی که صورت حساب آنها توسط مالی پروژه تایید شده است>
-- =============================================
CREATE PROCEDURE [dbo].[sp_FinancialAssesment_Vehicle] 
	-- Add the parameters for the stored procedure here
	@invoiceCode nvarchar(max),
	@workshopId nvarchar(max),
	@fromdate nvarchar(max),
	@todate nvarchar(max),
	@user nvarchar(max),
	@type nvarchar(max)

AS

BEGIN
	update tbl_VehicleRegistrations
	set Status='1',FinancialStatus=level.st,FinancialStatusDate=GETDATE(),FinancialStatusUser=@user,[InvoiceCode]=@invoiceCode
	from
	(select
	ID,1 as st,CreateDate,WorkshopID from tbl_VehicleRegistrations 
	where RegistrationTypeID=@type and WorkshopID=@workshopId and CONVERT(VARCHAR(10),CreateDate,120) BETWEEN '' + CONVERT(VARCHAR(10),@fromdate,120) + '' AND '' +  CONVERT(VARCHAR(10),@todate,120)+ '') as level
	where tbl_VehicleRegistrations.ID=level.ID
END
GO
/****** Object:  StoredProcedure [dbo].[sp_freeproccache]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1401/04/17>
-- Description:	<خالی کردن کش دیتابیس>
-- =============================================
Create PROCEDURE [dbo].[sp_freeproccache] 
	-- Add the parameters for the stored procedure here
AS

BEGIN
	dbcc freeproccache
END
GO
/****** Object:  StoredProcedure [dbo].[sp_FreeSaleRemittance]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1400/03/15>
-- Description:	<مشاهده اطلاعات اقلام فروش در قالب حواله>
-- =============================================
CREATE PROCEDURE [dbo].[sp_FreeSaleRemittance] 
	-- Add the parameters for the stored procedure here
	@invoiceCode nvarchar(max)
AS


BEGIN
----------------------خواندن اطلاعات حواله ها و بارنامه مربوطه --------------------------------------------------------------
SELECT *,[tbl_FreeSaleRemittanceDetails].CreateDate Date
  FROM [CNGFAPCO].[dbo].[tbl_FreeSaleRemittances] left outer join [CNGFAPCO].[dbo].[tbl_FreeSaleRemittanceDetails]
  on [tbl_FreeSaleRemittances].ID=[tbl_FreeSaleRemittanceDetails].[RemittancesID]
  where invoiceCode=@invoiceCode

----------------------خواندن اطلاعات فاکتور های نهایی شده --------------------------------------------------------------
select * FROM [CNGFAPCO].[dbo].[tbl_FinallFreeSaleInvoices] inner join [CNGFAPCO].[dbo].[tbl_Workshops] on WorkshopsID=ID
  where invoiceCode=@invoiceCode

END

GO
/****** Object:  StoredProcedure [dbo].[sp_FreeSaleRequestAlarms]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1400/05/22>
-- Description:	مشاهده درخواست های خرید ثبت شده و مشاهده نشده توسط مدیر فروش
-- =============================================
Create PROCEDURE [dbo].[sp_FreeSaleRequestAlarms] 
	-- Add the parameters for the stored procedure here
	@workshops nvarchar(255)=null

AS

BEGIN
	-----------------------------------------------------------درخواست های خرید مشاهده نشده-----------------------------------------------------------------------------------
	select [dbo].[GetWorkshops](WorkshopsID) Workshop,InvoiceCode,CONVERT(date, CreatedDate) CreatedDate,sum(Number) Number FROM [CNGFAPCO].[dbo].[tbl_RequestFreeSales]
	where WorkshopsID=@workshops or @workshops=0 and ViewStatus=0
	group by WorkshopsID,InvoiceCode,CONVERT(date, CreatedDate)
	order by CONVERT(date, CreatedDate) desc
	-----------------------------------------------------------پیش فاکتورهای مشاهده نشده-----------------------------------------------------------------------------------
	select [dbo].[GetWorkshops](WorkshopsID) Workshop,RequestInvoiceCode,CONVERT(date, CreatedDate) CreatedDate,sum(Number) Number FROM [CNGFAPCO].[dbo].[tbl_FreeSaleInvoices]
	where WorkshopsID=@workshops and ViewStatus=0
	group by WorkshopsID,RequestInvoiceCode,CONVERT(date, CreatedDate)
	order by CONVERT(date, CreatedDate) desc

END
GO
/****** Object:  StoredProcedure [dbo].[sp_FuelRelayList]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1400/08/28>
-- Description:	مشاهده اطلاعات ثبت شده در بانک رله سوخت
-- =============================================
Create PROCEDURE [dbo].[sp_FuelRelayList] 
	-- Add the parameters for the stored procedure here
	@workshops nvarchar(max)=null,
	@status nvarchar(max)=null,
	@FromDate DATETIME ,
	@ToDate DATETIME
	

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN
SET @PivotTableSQL = N'
SELECT * FROM(
SELECT [ID]
      ,[serialNumber]
      ,[constractor]
      ,[model]
      ,[type]
      ,[generation]
      ,[productDate]
	  ,[expireDate]
      ,(CASE WHEN datalength(workshop)<10 THEN [dbo].[GetWorkshops]([workshop]) else [workshop] END) as [workshop]
      ,[status]
	  ,(case when [status]=N''نیاز به بررسی'' then 2 else 1 end) as [statusCode]
	  ,(case when [status]=N''نیاز به بررسی'' then ''red'' else ''green'' end) as [statusColor]
      ,[CreateDate]
      ,[Creator]
	  ,[RefreshDate]
	  ,[MaterailName]
  FROM [CNGFAPCO].[dbo].[tbl_FuelRelays]
  where workshop in ('+@workshops+') and CONVERT(VARCHAR(10),CreateDate,120) BETWEEN ''' + CONVERT(VARCHAR(10),@FromDate,120) + ''' AND ''' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''' 
  ) AS List
  Where List.[statusCode] in ('+@status+')
  order by List.ID desc
  '

EXECUTE(@PivotTableSQL)

END
--smaple check numeric
--(CASE WHEN datalength(constractor)<10 THEN [dbo].[GetRegulatorConstractors]([constractor]) else constractor END) as [constractor]
--(CASE WHEN constractor LIKE '%[^0-9]%' THEN [dbo].[GetWorkshops]([workshop]) else constractor END) as [workshop]
GO
/****** Object:  StoredProcedure [dbo].[sp_FuelRelayList_Bank]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1400/08/28>
-- Description:	مشاهده اطلاعات ثبت شده در بانک رله سوخت
-- =============================================
Create PROCEDURE [dbo].[sp_FuelRelayList_Bank] 
	-- Add the parameters for the stored procedure here	
	@FromDate DATETIME ,
	@ToDate DATETIME
	

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN
SET @PivotTableSQL = N'
SELECT [ID]
      ,[serialNumber]
      ,[constractor]
      ,[model]
      ,[type]
      ,[generation]
      ,[productDate]
	  ,[expireDate]
      ,[workshop]      
      ,[CreateDate]
      ,[Creator]
	  ,[MaterailName]
  FROM [CNGFAPCO].[dbo].[tbl_BankFuelRelays]
  where CONVERT(VARCHAR(10),CreateDate,120) BETWEEN ''' + CONVERT(VARCHAR(10),@FromDate,120) + ''' AND ''' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''' 
  order by ID desc
  '

EXECUTE(@PivotTableSQL)

END
--smaple check numeric
--(CASE WHEN constractor LIKE '%[^0-9]%' THEN [dbo].[GetWorkshops]([workshop]) else constractor END) as [workshop]
GO
/****** Object:  StoredProcedure [dbo].[sp_GasECUList]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1400/08/28>
-- Description:	مشاهده اطلاعات ثبت شده در بانک Gas ECU
-- =============================================
Create PROCEDURE [dbo].[sp_GasECUList] 
	-- Add the parameters for the stored procedure here
	@workshops nvarchar(max)=null,
	@status nvarchar(max)=null,
	@FromDate DATETIME ,
	@ToDate DATETIME
	

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN
SET @PivotTableSQL = N'
SELECT * FROM(
SELECT [ID]
      ,[serialNumber]
      ,[constractor]
      ,[model]
      ,[type]
      ,[generation]
      ,[productDate]
	  ,[expireDate]
      ,(CASE WHEN datalength(workshop)<10 THEN [dbo].[GetWorkshops]([workshop]) else [workshop] END) as [workshop]
      ,[status]
	  ,(case when [status]=N''نیاز به بررسی'' then 2 else 1 end) as [statusCode]
	  ,(case when [status]=N''نیاز به بررسی'' then ''red'' else ''green'' end) as [statusColor]
      ,[CreateDate]
      ,[Creator]
	  ,[RefreshDate]
	  ,[MaterailName]
  FROM [CNGFAPCO].[dbo].[tbl_GasECU]
  where workshop in ('+@workshops+') and CONVERT(VARCHAR(10),CreateDate,120) BETWEEN ''' + CONVERT(VARCHAR(10),@FromDate,120) + ''' AND ''' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''' 
  ) AS List
  Where List.[statusCode] in ('+@status+')
  order by List.ID desc
  '

EXECUTE(@PivotTableSQL)

END
--smaple check numeric
--(CASE WHEN datalength(constractor)<10 THEN [dbo].[GetRegulatorConstractors]([constractor]) else constractor END) as [constractor]
--(CASE WHEN constractor LIKE '%[^0-9]%' THEN [dbo].[GetWorkshops]([workshop]) else constractor END) as [workshop]
GO
/****** Object:  StoredProcedure [dbo].[sp_GasECUList_Bank]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1400/08/28>
-- Description:	مشاهده اطلاعات ثبت شده در بانک Gas ECU 
-- =============================================
Create PROCEDURE [dbo].[sp_GasECUList_Bank] 
	-- Add the parameters for the stored procedure here	
	@FromDate DATETIME ,
	@ToDate DATETIME
	

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN
SET @PivotTableSQL = N'
SELECT [ID]
      ,[serialNumber]
      ,[constractor]
      ,[model]
      ,[type]
      ,[generation]
      ,[productDate]
	  ,[expireDate]
      ,[workshop]      
      ,[CreateDate]
      ,[Creator]
	  ,[MaterailName]
  FROM [CNGFAPCO].[dbo].[tbl_BankGasECU]
  where CONVERT(VARCHAR(10),CreateDate,120) BETWEEN ''' + CONVERT(VARCHAR(10),@FromDate,120) + ''' AND ''' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''' 
  order by ID desc
  '

EXECUTE(@PivotTableSQL)

END
--smaple check numeric
--(CASE WHEN constractor LIKE '%[^0-9]%' THEN [dbo].[GetWorkshops]([workshop]) else constractor END) as [workshop]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetCheckSalaryRem]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1400/03/13>
-- Description:	محاسبه مانده طلب دستمزد تبدیل کارگاه ها برای چک کردن و مجوز خرید غیر نقدی کالا 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetCheckSalaryRem] 
	-- Add the parameters for the stored procedure here
	@workshops nvarchar(max)=null,
	@FromDate DATETIME ,
	@ToDate DATETIME
	

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN

-----------------------------------------------------------------------financial credit and debit --------------------------------------------------------
--select ((CONVERT(float,list.Creditor) - (CONVERT(float,list.Debtor) + CONVERT(float,list.NonCash))) * 0.7) as 'Rem' from (
--select ID,Title,ISNULL([dbo].[GetFinancialCreditor](ID,@workshops,@FromDate,@ToDate),0) as 'Creditor',ISNULL([dbo].[GetFinancialDebtor](ID,@workshops,@FromDate,@ToDate),0) as 'Debtor',
--ISNULL([dbo].[GetFinancialDebtor](6,@workshops,@FromDate,@ToDate),0) as 'NonCash'
--FROM [CNGFAPCO].[dbo].[tbl_FinancialDescs] 
--where ID in (1)
--) as list


select (CONVERT(float,list2.Creditor) - CONVERT(float,list2.Deductions) - CONVERT(float,list2.Debtor) - CONVERT(float,list2.NonCash)- CONVERT(float,list2.OfferedPrice)) as 'Rem' from(
select *,(CONVERT(float,list.Creditor) - CONVERT(float,list.Debtor) - CONVERT(float,list.NonCash)) as 'Rem', ((CONVERT(float,list.Creditor) * 0.2667)) as 'Deductions' from (
select ID,Title,ISNULL([dbo].[GetFinancialCreditor](ID,@workshops,@FromDate,@ToDate),0) as 'Creditor',ISNULL([dbo].[GetFinancialDebtor](ID,@workshops,@FromDate,@ToDate),0) as 'Debtor',
		ISNULL([dbo].[GetFinancialDebtor](6,@workshops,@FromDate,@ToDate),0) as 'NonCash',ISNULL([dbo].[GetCheckedPreInvoice](ID),0) as 'Amount',[dbo].[GetOfferedPriceNoPaied](@workshops) as 'OfferedPrice'
FROM [CNGFAPCO].[dbo].[tbl_FinancialDescs] 
where ID in (1)
) as list ) as list2

END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetDefectsCount]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal>
-- Create date: <1399/05/05>
-- Description:	<جهت مشاهده لیست صورتحساب فروش کارگاه به شرکت فن آوران پس از صدور توسط کاربر صادر کننده>
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetDefectsCount] 
	-- Add the parameters for the stored procedure here
	@permission nvarchar(max),
	@InvoiceCode nvarchar(1000)
AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN
	   SET @PivotTableSQL = 'SELECT  ISNULL([dbo].[GetDefectsCount]((' + @permission + '),cast(dbo.tbl_InvoicesFapa.EmployerEconomicalnumber as date),cast(dbo.tbl_InvoicesFapa.CreatedDate as date)),0) as DefectsCount
								FROM            dbo.tbl_InvoicesFapa
								where InvoiceCode in (' + @InvoiceCode + ')'
	   EXECUTE(@PivotTableSQL)
END
GO
/****** Object:  StoredProcedure [dbo].[sp_InsertPayment]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1399/09/27>
-- Description:	ثبت اطلاعات پرداخت هاي آنلاين درگاه بانک ملت
-- =============================================
CREATE PROCEDURE [dbo].[sp_InsertPayment] 
	-- Add the parameters for the stored procedure here
	@FullName nvarchar(max)=null,
	@PayerCode nvarchar(max)=null,
	@PreInvoiceCode nvarchar(max)=null,
	@InvoiceCode nvarchar(max)=null,
	@NationalCode nvarchar(max)=null,
	@MobileNumber nvarchar(max)=null ,
	@EMailAddress nvarchar(max)=null ,
	@Amount nvarchar(max)=null ,
	@PayerIPAddress nvarchar(max)=null ,
	@Status nvarchar(max)=null ,
	@RefID nvarchar(max)=null ,
	@OrderID nvarchar(max)=null ,
	@SaleReferenceId nvarchar(max)=null 
AS

BEGIN

insert into tbl_Payments(FullName,NationalCode,MobileNumber,EMailAddress,Amount,PayDate,PayerIPAddress,[Status],RefID,SaleReferenceId,OrderID,PreInvoiceCode,InvoiceCode,PayerCode,PaymentMethod) 
	   values(@FullName,@NationalCode,@MobileNumber,@EMailAddress,@Amount,GETDATE(),@PayerIPAddress,@status,@RefID,'',@OrderID,@PreInvoiceCode,@InvoiceCode,@PayerCode,N'درگاه بانکی')
	
END
GO
/****** Object:  StoredProcedure [dbo].[sp_InvoiceDamagedResultFilterView]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1403/06/31>
-- Description:	مشاهده نتایج تعدادی و ریالی مخازن تعویضی به صورت فیلتر شده و در قالب جدول
-- =============================================
CREATE PROCEDURE [dbo].[sp_InvoiceDamagedResultFilterView] 
	-- Add the parameters for the stored procedure here
	@workshopId nvarchar(max),
	@Year nvarchar(4),
	@Month nvarchar(2)
	
AS

BEGIN

SELECT [WorkshopsID]
	  ,[Title] as WorkshopTitle
      ,[ServiceCode]
      ,[CreatedDate]      
      ,[ServiceDesc]
      ,[Number]
      ,[UnitofMeasurement]
      ,[UnitAmount]
      ,[TotalAmount]
      ,[DiscountAmount]
      ,[TotalAmountafterDiscount]      
      ,[AmountTaxandComplications]
      ,[TotalAmountTaxandComplications]     
      ,[Year]
      ,[Month]
  FROM [CNGFAPCO].[dbo].[tbl_InvoicesDamages] inner join tbl_Workshops
		on tbl_InvoicesDamages.WorkshopsID=tbl_Workshops.ID
  where (WorkshopsID = @workshopId or @workshopId = 0) and (Year = @Year or @Year = 0) and (Month = @Month or @Month = 0)
  order by WorkshopsID,Year,Month,ServiceCode
END
GO
/****** Object:  StoredProcedure [dbo].[sp_InvoiceFapa_DamagesCylinderStatusList]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1404/02/29>
-- Description:	مشاهده وضعیت صورتحساب های دستمزد تعویض کارگاه ها بر اساس دریافت شده، تایید شده و ...
-- =============================================
CREATE PROCEDURE [dbo].[sp_InvoiceFapa_DamagesCylinderStatusList] 
	-- Add the parameters for the stored procedure here
	--@workshops nvarchar(max)=null	

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN
SET @PivotTableSQL = N'
SELECT *, CONVERT(float, list.ReciveCount)-CONVERT(float, list.CheckedCount) ''InCheckedProccess'',CONVERT(float, list.ReciveCount)-CONVERT(float, list.FinancialCount) ''InFinancialProccess'',
						 CONVERT(float, list.ReciveCount)-CONVERT(float, list.Count) ''Deliver'' from (
						 SELECT dbo.tbl_InvoicesFapa_DamagesCylinder.WorkshopsID,dbo.tbl_Workshops.Title, max(convert(int, InvoiceCode)) Count,[dbo].[GetInvoiceReciveCount](WorkshopsID,2) ''ReciveCount'',
						 [dbo].[GetInvoiceCheckedCount](WorkshopsID,2) ''CheckedCount'',[dbo].[GetInvoiceFinancialCount](WorkshopsID,2) ''FinancialCount''
						 FROM tbl_InvoicesFapa_DamagesCylinder INNER JOIN
                         dbo.tbl_VehicleTypes ON dbo.tbl_InvoicesFapa_DamagesCylinder.VehicleTypesID = dbo.tbl_VehicleTypes.ID LEFT OUTER JOIN
                         dbo.tbl_Workshops ON dbo.tbl_InvoicesFapa_DamagesCylinder.WorkshopsID= dbo.tbl_Workshops.ID
						 group by dbo.tbl_InvoicesFapa_DamagesCylinder.WorkshopsID,dbo.tbl_Workshops.Title) as list
  '

EXECUTE(@PivotTableSQL)

END
GO
/****** Object:  StoredProcedure [dbo].[sp_InvoiceFapaDiffWithRegisterCount]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1401/06/11>
-- Description:	مقایسه اطلاعات تعداد و نوع خودرد در جدول ثبت نام با جدول صورتحساب های دستمزد تبدیل شده
-- =============================================
CREATE PROCEDURE [dbo].[sp_InvoiceFapaDiffWithRegisterCount] 
	-- Add the parameters for the stored procedure here
	@workshops nvarchar(max)=null,
	@fromdate  nvarchar(max),
	@todate  nvarchar(max)

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN

SELECT tbl_VehicleTypes.Type + ' ' + tbl_VehicleTypes.Description as ServiceDesc
					  ,SUM(convert(float, [Number])) as InvoiceCount , [dbo].[GetRegisterCount](tbl_VehicleTypes.ID,WorkshopsID,@fromdate,@todate) as RegisterCount
					  FROM [CNGFAPCO].[dbo].[tbl_InvoicesFapa] inner join tbl_VehicleTypes on tbl_InvoicesFapa.VehicleTypesID=tbl_VehicleTypes.ID
					  where WorkshopsID in (@workshops) and tbl_InvoicesFapa.Description is null and CONVERT(VARCHAR(10),EmployerEconomicalnumber,120) >= '' + CONVERT(VARCHAR(10),[dbo].[PersianToMiladi](@fromdate),120) + '' AND '' +  CONVERT(VARCHAR(10),CreatedDate,120) <= CONVERT(VARCHAR(10),[dbo].[PersianToMiladi](@todate),120)+ ''
					  group by tbl_VehicleTypes.ID,Type,tbl_VehicleTypes.Description,tbl_InvoicesFapa.WorkshopsID
					  order by tbl_VehicleTypes.ID

END
GO
/****** Object:  StoredProcedure [dbo].[sp_InvoiceFapaDiffWithSalary]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1401/06/10>
-- Description:	مغایرت گیری فاکتورهای دستمزد تبدیل و مابه التفاوت با مبلغ دستمزد تبدیل
-- =============================================
CREATE PROCEDURE [dbo].[sp_InvoiceFapaDiffWithSalary] 
	-- Add the parameters for the stored procedure here
	@workshops nvarchar(max)=null,
	@FromDate DATETIME ,
	@ToDate DATETIME
	

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN

SET @PivotTableSQL = N' SELECT  sum(convert(int, dbo.tbl_InvoicesFapa.Number)) as TotalCount,sum(dbo.tbl_InvoicesFapa.TotalAmount) as TotalAmount,sum(dbo.tbl_InvoicesFapa.TotalAmountTaxandComplications) as TotalAmount2, dbo.tbl_Workshops.Title as Workshop,
						 N''فاکتور دستمزد تبدیل'' as Type,dbo.tbl_InvoicesFapa.Description
						 FROM            dbo.tbl_InvoicesFapa INNER JOIN
                         dbo.tbl_VehicleTypes ON dbo.tbl_InvoicesFapa.VehicleTypesID = dbo.tbl_VehicleTypes.ID LEFT OUTER JOIN
                         dbo.tbl_Workshops ON dbo.tbl_InvoicesFapa.WorkshopsID= dbo.tbl_Workshops.ID
						 where tbl_Workshops.ID in ('+@workshops+') and CONVERT(VARCHAR(10),EmployerEconomicalnumber,120) >= ''' + CONVERT(VARCHAR(10),@FromDate,120) + ''' AND CONVERT(VARCHAR(10),CreatedDate,120) <= ''' + CONVERT(VARCHAR(10),@ToDate,120)+ '''
						 group by dbo.tbl_InvoicesFapa.WorkshopsID, dbo.tbl_InvoicesFapa.Description,dbo.tbl_Workshops.Title

union all 
						select SUM(list.Count) as TotalCount,ISNULL(SUM((CONVERT(float,list.Count) * list.Price)),0) as ''TotalAmount2'','''','''',N''دستمزد تبدیل'' as Type,'''' from (
						select COUNT(*) as ''Count'', [dbo].[GetRegistrationPrice](WorkshopID,Type,CreateDate) as ''Price'' from tbl_VehicleRegistrations inner join tbl_VehicleTypes on tbl_VehicleRegistrations.VehicleTypeID=tbl_VehicleTypes.ID
						where RegisterStatus=1 and RegistrationTypeID=1 and WorkshopID in ('+@workshops+') and CONVERT(VARCHAR(10),CreateDate,120) BETWEEN ''' + CONVERT(VARCHAR(10),@FromDate,120) + ''' AND ''' +  CONVERT(VARCHAR(10),@ToDate,120)+ '''
						group by Type,WorkshopID,CreateDate,tbl_VehicleTypes.Description) as list 
'
EXECUTE(@PivotTableSQL)

END
GO
/****** Object:  StoredProcedure [dbo].[sp_InvoiceFapaStatusList]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1401/06/03>
-- Description:	مشاهده وضعیت صورتحساب های دستمزد تبدیل کارگاه ها بر اساس دریافت شده، تایید شده و ...
-- =============================================
CREATE PROCEDURE [dbo].[sp_InvoiceFapaStatusList] 
	-- Add the parameters for the stored procedure here
	--@workshops nvarchar(max)=null	

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN
SET @PivotTableSQL = N'
SELECT *, CONVERT(int, list.ReciveCount)-CONVERT(int, list.CheckedCount) ''InCheckedProccess'',CONVERT(int, list.ReciveCount)-CONVERT(int, list.FinancialCount) ''InFinancialProccess'',
						 CONVERT(int, list.ReciveCount)-CONVERT(int, list.Count) ''Deliver'' from (
						 SELECT dbo.tbl_InvoicesFapa.WorkshopsID,dbo.tbl_Workshops.Title, max(convert(int, InvoiceCode)) Count,[dbo].[GetInvoiceReciveCount](WorkshopsID,1) ''ReciveCount'',
						 [dbo].[GetInvoiceCheckedCount](WorkshopsID,1) ''CheckedCount'',[dbo].[GetInvoiceFinancialCount](WorkshopsID,1) ''FinancialCount''
						 FROM dbo.tbl_InvoicesFapa INNER JOIN
                         dbo.tbl_VehicleTypes ON dbo.tbl_InvoicesFapa.VehicleTypesID = dbo.tbl_VehicleTypes.ID LEFT OUTER JOIN
                         dbo.tbl_Workshops ON dbo.tbl_InvoicesFapa.WorkshopsID= dbo.tbl_Workshops.ID
						 group by dbo.tbl_InvoicesFapa.WorkshopsID,dbo.tbl_Workshops.Title) as list
  '

EXECUTE(@PivotTableSQL)

END
GO
/****** Object:  StoredProcedure [dbo].[sp_InvoicesDamages]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1403/04/27>
-- Description:	مشاهده اطلاعات کلی و جزئیات فاکتورهای فروش مخازن ضایعاتی
-- =============================================
Create PROCEDURE [dbo].[sp_InvoicesDamages] 
	-- Add the parameters for the stored procedure here
	@Year varchar(4),
	@Month varchar(2),
	@WorkshopsID varchar(5)

AS

BEGIN

----------------------------------------------for get sum of number Cilynder-------------------------------------------------------------------------------------------
select SUM(convert(int,Number)) as Count
from tbl_InvoicesDamages
where (WorkshopsID in (@WorkshopsID) or @WorkshopsID=0) and (Year in (@Year) or @Year=0) and (Month in (@Month) or @Month=0)
----------------------------------------------for get sum of Value Cilynder-------------------------------------------------------------------------------------------
select SUM(convert(float,TotalAmountTaxandComplications)) as TotalAmount
from tbl_InvoicesDamages
where (WorkshopsID in (@WorkshopsID) or @WorkshopsID=0) and (Year in (@Year) or @Year=0) and (Month in (@Month) or @Month=0)
----------------------------------------------for get Detaile of number Cilynder-------------------------------------------------------------------------------------------

select SUM(convert(int,Number)) as Count,WorkshopsID,Title,ServiceCode as Literage,InvoiceCode,Year,Month
from tbl_InvoicesDamages inner join tbl_Workshops on WorkshopsID=ID
group by Number,WorkshopsID,Title,ServiceCode,InvoiceCode,Year,Month
having (WorkshopsID in (@WorkshopsID) or @WorkshopsID=0) and (Year in (@Year) or @Year=0) and (Month in (@Month) or @Month=0)
order by InvoiceCode

END
GO
/****** Object:  StoredProcedure [dbo].[sp_InvoicesDamagesDetails]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1403/03/31>
-- Description:	مشاهده جزئیات فاکتورهای فروش اقلام ضایعاتی کارگاه‌ها
-- =============================================
CREATE PROCEDURE [dbo].[sp_InvoicesDamagesDetails] 
	-- Add the parameters for the stored procedure here
	@workshopId nvarchar(10),
	@Year nvarchar(4),
	@Month nvarchar(2)
AS

DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN

SET @PivotTableSQL = '
SELECT [Literage_Text] as [Literage]
      ,[WorkshopID]
	  ,ID
	  ,Title
      ,[Year]
      ,[Month]
      ,[Plate]
      ,[ChassisNumber]
	  ,VehicleType
	  ,Acceptance
  FROM [CNGFAPCO].[dbo].[resultstable_IRNGVDamages] inner join tbl_Workshops
  on WorkshopID=IRNGVCod
  where ID='+@workshopId+' and YEAR='+@Year+' and MONTH='+@Month+'
'

EXECUTE(@PivotTableSQL)

END
GO
/****** Object:  StoredProcedure [dbo].[sp_InvoicesDamagesFilterwithfromtoDate]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1404/02/18>
-- Description:	نمایش اطلاعات فروش مخازن ضایعاتی و شیر ضایعاتی مخزن کارگاه ها
-- =============================================
CREATE PROCEDURE [dbo].[sp_InvoicesDamagesFilterwithfromtoDate] 
	-- Add the parameters for the stored procedure here
	@table nvarchar(max)=null,
	@fromYear int,
	@toYear int,
	@fromMonth int,
	@toMonth int
	
AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN
    SET NOCOUNT ON;   
    -- اعتبارسنجی پارامترهای ورودی
    IF @fromMonth < 1 OR @fromMonth > 12 OR @toMonth < 1 OR @toMonth > 12
    BEGIN
        RAISERROR('محدوده ماه‌های انتخابی باید بین فروردین (1) تا اسفند (12) باشد', 16, 1);
        RETURN;
    END   
    IF (@fromYear > @toYear) OR (@fromYear = @toYear AND @fromMonth > @toMonth)
    BEGIN
        RAISERROR('محدوده تاریخ وارد شده معتبر نیست', 16, 1);
        RETURN;
    END    
    -- تبدیل سال و ماه به یک عدد ترکیبی برای مقایسه آسان‌تر
    DECLARE @FromDateNum INT = @fromYear * 100 + @fromMonth;
    DECLARE @ToDateNum INT = @toYear * 100 + @toMonth;
    
	SET @PivotTableSQL = N'
	SELECT 
		  (t2.OwnerName + '' '' + t2.OwnerFamily) as Owner
		  ,(t2.Title + '' '' + t3.Title) as WorkshopTitle
		  ,SUM(TotalAmountTaxandComplications) as TotalAmount
		  ,SUM(convert(int, t1.Number)) as Number
		  ,t1.WorkshopsID as WorkshopID
      
		  FROM ' + @table + ' t1
		  inner join tbl_Workshops t2 on t1.WorkshopsID=t2.ID
		  inner join tbl_Cities t3 on t2.CityID=t3.ID
		  WHERE (t1.Year * 100 + t1.Month) BETWEEN ' + CONVERT(nvarchar(max), @FromDateNum) +' AND '+ CONVERT(nvarchar(max), @ToDateNum) +'
		  GROUP BY t2.OwnerName,t2.OwnerFamily,t2.Title,t3.Title,t1.WorkshopsID'
	EXECUTE(@PivotTableSQL)
END
GO
/****** Object:  StoredProcedure [dbo].[sp_InvoicesDamagesList]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal>
-- Create date: <1403/03/31>
-- Description:	<جهت مشاهده لیست صورتحساب فروش اقلام ضایعاتی شرکت فن آوران به کارگاه‌ها >
-- =============================================
CREATE PROCEDURE [dbo].[sp_InvoicesDamagesList] 
	-- Add the parameters for the stored procedure here
	@permission nvarchar(max)
AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN
	   SET @PivotTableSQL = N'
	   SELECT  sum(convert(int, dbo.tbl_InvoicesDamages.Number)) as TotalCount,sum(dbo.tbl_InvoicesDamages.TotalAmountafterDiscount) as TotalAmount,sum(dbo.tbl_InvoicesDamages.TotalAmountTaxandComplications) as TotalAmount2, dbo.tbl_InvoicesDamages.InvoiceCode,dbo.tbl_InvoicesDamages.Description,cast(dbo.tbl_InvoicesDamages.CreatedDate as date) as CreatedDate, dbo.tbl_Workshops.Title, dbo.tbl_InvoicesDamages.Status,cast(dbo.tbl_InvoicesDamages.EmployerEconomicalnumber as date) as EmployerEconomicalnumber,
						 ''0'' as DefectsCount, (case dbo.tbl_InvoicesDamages.CheckStatus when 1 then N''صورتحساب تایید کارشناسی شد'' else N''در حال بررسی'' end) as CheckStatus,dbo.tbl_InvoicesDamages.Year,dbo.tbl_InvoicesDamages.Month,
						 (case dbo.tbl_InvoicesDamages.FinancialStatus when 1 then N''صورتحساب تایید مالی شد'' else N''در حال بررسی'' end) as FinancialStatus,
						  (case dbo.tbl_InvoicesDamages.ReciveStatus when 1 then N''صورتحساب دریافت شد'' else N''در حال بررسی'' end) as ReciveStatus
						 FROM   dbo.tbl_InvoicesDamages left outer JOIN
                         dbo.tbl_VehicleTypes ON dbo.tbl_InvoicesDamages.VehicleTypesID = dbo.tbl_VehicleTypes.ID LEFT OUTER JOIN
                         dbo.tbl_Workshops ON dbo.tbl_InvoicesDamages.WorkshopsID= dbo.tbl_Workshops.ID
						 where tbl_Workshops.ID in ('+@permission+')
						 group by dbo.tbl_InvoicesDamages.WorkshopsID, dbo.tbl_InvoicesDamages.InvoiceCode,dbo.tbl_InvoicesDamages.Description,cast(dbo.tbl_InvoicesDamages.CreatedDate as date),dbo.tbl_Workshops.Title,dbo.tbl_InvoicesDamages.Status,dbo.tbl_InvoicesDamages.EmployerEconomicalnumber,dbo.tbl_InvoicesDamages.CheckStatus,dbo.tbl_InvoicesDamages.FinancialStatus,dbo.tbl_InvoicesDamages.ReciveStatus,
						 dbo.tbl_InvoicesDamages.Year,dbo.tbl_InvoicesDamages.Month
						 order by CAST(dbo.tbl_InvoicesDamages.InvoiceCode as int) desc
						 '
	   EXECUTE(@PivotTableSQL)
END
GO
/****** Object:  StoredProcedure [dbo].[sp_InvoicesFapa_DamagesCylinder]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal>
-- Create date: <1403/07/12>
-- Description:	<جهت مشاهده لیست صورتحساب دستمزد تعویض ارسالی کارگاه به شرکت فن آوران پس از صدور توسط کاربر صادر کننده>
-- =============================================
CREATE PROCEDURE [dbo].[sp_InvoicesFapa_DamagesCylinder] 
	-- Add the parameters for the stored procedure here
	@permission nvarchar(max)
AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN
	   SET @PivotTableSQL = N'
	   SELECT  sum(convert(int, dbo.tbl_InvoicesFapa_DamagesCylinder.Number)) as TotalCount,sum(dbo.tbl_InvoicesFapa_DamagesCylinder.TotalAmount) as TotalAmount,sum(dbo.tbl_InvoicesFapa_DamagesCylinder.TotalAmountTaxandComplications) as TotalAmount2, dbo.tbl_InvoicesFapa_DamagesCylinder.InvoiceCode,dbo.tbl_InvoicesFapa_DamagesCylinder.Description,cast(dbo.tbl_InvoicesFapa_DamagesCylinder.CreatedDate as date) as CreatedDate, dbo.tbl_Workshops.Title, dbo.tbl_InvoicesFapa_DamagesCylinder.Status,cast(dbo.tbl_InvoicesFapa_DamagesCylinder.EmployerEconomicalnumber as date) as EmployerEconomicalnumber,
						 ''0'' as DefectsCount, (case dbo.tbl_InvoicesFapa_DamagesCylinder.CheckStatus when 1 then N''صورتحساب تایید کارشناسی شد'' else N''در حال بررسی'' end) as CheckStatus,
						 (case dbo.tbl_InvoicesFapa_DamagesCylinder.FinancialStatus when 1 then N''صورتحساب تایید مالی شد'' else N''در حال بررسی'' end) as FinancialStatus,
						  (case dbo.tbl_InvoicesFapa_DamagesCylinder.ReciveStatus when 1 then N''صورتحساب دریافت شد'' else N''در حال بررسی'' end) as ReciveStatus
						 FROM            dbo.tbl_InvoicesFapa_DamagesCylinder INNER JOIN
                         dbo.tbl_VehicleTypes ON dbo.tbl_InvoicesFapa_DamagesCylinder.VehicleTypesID = dbo.tbl_VehicleTypes.ID LEFT OUTER JOIN
                         dbo.tbl_Workshops ON dbo.tbl_InvoicesFapa_DamagesCylinder.WorkshopsID= dbo.tbl_Workshops.ID
						 where tbl_Workshops.ID in (' + @permission + ')
						 group by dbo.tbl_InvoicesFapa_DamagesCylinder.WorkshopsID, dbo.tbl_InvoicesFapa_DamagesCylinder.InvoiceCode,dbo.tbl_InvoicesFapa_DamagesCylinder.Description,dbo.tbl_InvoicesFapa_DamagesCylinder.CreatedDate,dbo.tbl_Workshops.Title,dbo.tbl_InvoicesFapa_DamagesCylinder.Status,dbo.tbl_InvoicesFapa_DamagesCylinder.EmployerEconomicalnumber,dbo.tbl_InvoicesFapa_DamagesCylinder.CheckStatus,dbo.tbl_InvoicesFapa_DamagesCylinder.FinancialStatus,dbo.tbl_InvoicesFapa_DamagesCylinder.ReciveStatus
						 order by CAST(dbo.tbl_InvoicesFapa_DamagesCylinder.InvoiceCode as int) desc
						 '
	   EXECUTE(@PivotTableSQL)
END
GO
/****** Object:  StoredProcedure [dbo].[sp_InvoicesFapaList]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal>
-- Create date: <1399/05/05>
-- Description:	<جهت مشاهده لیست صورتحساب فروش کارگاه به شرکت فن آوران پس از صدور توسط کاربر صادر کننده>
-- =============================================
CREATE PROCEDURE [dbo].[sp_InvoicesFapaList] 
	-- Add the parameters for the stored procedure here
	@permission nvarchar(max)
AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN
	   SET @PivotTableSQL = N'
	   SELECT  sum(convert(int, dbo.tbl_InvoicesFapa.Number)) as TotalCount,sum(dbo.tbl_InvoicesFapa.TotalAmount) as TotalAmount,sum(dbo.tbl_InvoicesFapa.TotalAmountTaxandComplications) as TotalAmount2, dbo.tbl_InvoicesFapa.InvoiceCode,dbo.tbl_InvoicesFapa.Description,cast(dbo.tbl_InvoicesFapa.CreatedDate as date) as CreatedDate, dbo.tbl_Workshops.Title, dbo.tbl_InvoicesFapa.Status,cast(dbo.tbl_InvoicesFapa.EmployerEconomicalnumber as date) as EmployerEconomicalnumber,
						 ''0'' as DefectsCount, (case dbo.tbl_InvoicesFapa.CheckStatus when 1 then N''صورتحساب تایید کارشناسی شد'' else N''در حال بررسی'' end) as CheckStatus,
						 (case dbo.tbl_InvoicesFapa.FinancialStatus when 1 then N''صورتحساب تایید مالی شد'' else N''در حال بررسی'' end) as FinancialStatus,
						  (case dbo.tbl_InvoicesFapa.ReciveStatus when 1 then N''صورتحساب دریافت شد'' else N''در حال بررسی'' end) as ReciveStatus
						 FROM            dbo.tbl_InvoicesFapa INNER JOIN
                         dbo.tbl_VehicleTypes ON dbo.tbl_InvoicesFapa.VehicleTypesID = dbo.tbl_VehicleTypes.ID LEFT OUTER JOIN
                         dbo.tbl_Workshops ON dbo.tbl_InvoicesFapa.WorkshopsID= dbo.tbl_Workshops.ID
						 where tbl_Workshops.ID in (' + @permission + ')
						 group by dbo.tbl_InvoicesFapa.WorkshopsID, dbo.tbl_InvoicesFapa.InvoiceCode,dbo.tbl_InvoicesFapa.Description,dbo.tbl_InvoicesFapa.CreatedDate,dbo.tbl_Workshops.Title,dbo.tbl_InvoicesFapa.Status,dbo.tbl_InvoicesFapa.EmployerEconomicalnumber,dbo.tbl_InvoicesFapa.CheckStatus,dbo.tbl_InvoicesFapa.FinancialStatus,dbo.tbl_InvoicesFapa.ReciveStatus
						 order by CAST(dbo.tbl_InvoicesFapa.InvoiceCode as int) desc
						 '
	   EXECUTE(@PivotTableSQL)
END

--ISNULL([dbo].[GetDefectsCount_2](WorkshopsID,cast(dbo.tbl_InvoicesFapa.EmployerEconomicalnumber as date),cast(dbo.tbl_InvoicesFapa.CreatedDate as date)),0) as DefectsCount
--ISNULL([dbo].[GetDefectsCount](WorkshopsID,cast(dbo.tbl_InvoicesFapa.EmployerEconomicalnumber as date),cast(dbo.tbl_InvoicesFapa.CreatedDate as date)),0) as DefectsCount
GO
/****** Object:  StoredProcedure [dbo].[sp_InvoicesList]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal>
-- Create date: <1399/05/03>
-- Description:	<جهت مشاهده لیست صورتحساب فروش پس از صدور توسط کاربر صادر کننده>
-- =============================================
CREATE PROCEDURE [dbo].[sp_InvoicesList] 
	-- Add the parameters for the stored procedure here
	@permission nvarchar(max)
AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN
	   SET @PivotTableSQL = 'SELECT  sum(dbo.tbl_Invoices.TotalAmount) as TotalAmount, dbo.tbl_Invoices.InvoiceCode,  CAST(dbo.tbl_Invoices.CreatedDate as date) as CreatedDate,case when dbo.tbl_Invoices.OwnersID is null then EmployerEconomicalnumber else  (dbo.tbl_VehicleRegistrations.OwnerName+'' '' + dbo.tbl_VehicleRegistrations.OwnerFamily) end as EmployerTitle, dbo.tbl_Workshops.Title, dbo.tbl_Invoices.Status
						  FROM            dbo.tbl_Invoices LEFT OUTER JOIN
                         dbo.tbl_VehicleRegistrations ON dbo.tbl_Invoices.OwnersID = dbo.tbl_VehicleRegistrations.ID LEFT OUTER JOIN
                         dbo.tbl_Workshops ON dbo.tbl_Invoices.WorkshopsID= dbo.tbl_Workshops.ID
						 where tbl_Workshops.ID in (' + @permission + ')
						 group by dbo.tbl_Invoices.InvoiceCode,CAST(dbo.tbl_Invoices.CreatedDate as date),dbo.tbl_VehicleRegistrations.OwnerName,dbo.tbl_VehicleRegistrations.OwnerFamily,dbo.tbl_Workshops.Title,dbo.tbl_Invoices.Status,dbo.tbl_Invoices.OwnersID,EmployerEconomicalnumber
						 order by CAST(dbo.tbl_Invoices.InvoiceCode as int) desc
						 '
	   EXECUTE(@PivotTableSQL)
END
GO
/****** Object:  StoredProcedure [dbo].[sp_InvoicesValveDamagesDetails]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1404/02/16>
-- Description:	مشاهده جزئیات فاکتورهای فروش شیر مخزنهای ضایعاتی کارگاه‌ها
-- =============================================
Create PROCEDURE [dbo].[sp_InvoicesValveDamagesDetails] 
	-- Add the parameters for the stored procedure here
	@workshopId nvarchar(10),
	@Year nvarchar(4),
	@Month nvarchar(2)
AS

DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN

SET @PivotTableSQL = '
SELECT [Literage_Text] as [Literage]
      ,[WorkshopID]
	  ,ID
	  ,Title
      ,[Year]
      ,[Month]
      ,[Plate]
      ,[ChassisNumber]
	  ,VehicleType
	  ,Acceptance
  FROM [CNGFAPCO].[dbo].[resultstable_IRNGVDamages] inner join tbl_Workshops
  on WorkshopID=IRNGVCod
  where ID='+@workshopId+' and YEAR='+@Year+' and MONTH='+@Month+'
'

EXECUTE(@PivotTableSQL)

END
GO
/****** Object:  StoredProcedure [dbo].[sp_InvoicesValveDamagesList]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal>
-- Create date: <1404/02/15>
-- Description:	<جهت مشاهده لیست صورتحساب فروش اقلام شیر مخزن های ضایعاتی شرکت فن آوران به کارگاه‌ها >
-- =============================================
CREATE PROCEDURE [dbo].[sp_InvoicesValveDamagesList] 
	-- Add the parameters for the stored procedure here
	@permission nvarchar(max)
AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN
	   SET @PivotTableSQL = N'
	   SELECT  sum(convert(int, dbo.tbl_InvoicesValveDamages.Number)) as TotalCount,sum(dbo.tbl_InvoicesValveDamages.TotalAmountafterDiscount) as TotalAmount,sum(dbo.tbl_InvoicesValveDamages.TotalAmountTaxandComplications) as TotalAmount2, dbo.tbl_InvoicesValveDamages.InvoiceCode,dbo.tbl_InvoicesValveDamages.Description,cast(dbo.tbl_InvoicesValveDamages.CreatedDate as date) as CreatedDate, dbo.tbl_Workshops.Title, dbo.tbl_InvoicesValveDamages.Status,cast(dbo.tbl_InvoicesValveDamages.EmployerEconomicalnumber as date) as EmployerEconomicalnumber,
						 ''0'' as DefectsCount, (case dbo.tbl_InvoicesValveDamages.CheckStatus when 1 then N''صورتحساب تایید کارشناسی شد'' else N''در حال بررسی'' end) as CheckStatus,dbo.tbl_InvoicesValveDamages.Year,dbo.tbl_InvoicesValveDamages.Month,
						 (case dbo.tbl_InvoicesValveDamages.FinancialStatus when 1 then N''صورتحساب تایید مالی شد'' else N''در حال بررسی'' end) as FinancialStatus,
						  (case dbo.tbl_InvoicesValveDamages.ReciveStatus when 1 then N''صورتحساب دریافت شد'' else N''در حال بررسی'' end) as ReciveStatus
						 FROM   dbo.tbl_InvoicesValveDamages left outer JOIN
                         dbo.tbl_VehicleTypes ON dbo.tbl_InvoicesValveDamages.VehicleTypesID = dbo.tbl_VehicleTypes.ID LEFT OUTER JOIN
                         dbo.tbl_Workshops ON dbo.tbl_InvoicesValveDamages.WorkshopsID= dbo.tbl_Workshops.ID
						 where tbl_Workshops.ID in ('+@permission+')
						 group by dbo.tbl_InvoicesValveDamages.WorkshopsID, dbo.tbl_InvoicesValveDamages.InvoiceCode,dbo.tbl_InvoicesValveDamages.Description,cast(dbo.tbl_InvoicesValveDamages.CreatedDate as date),dbo.tbl_Workshops.Title,dbo.tbl_InvoicesValveDamages.Status,dbo.tbl_InvoicesValveDamages.EmployerEconomicalnumber,dbo.tbl_InvoicesValveDamages.CheckStatus,dbo.tbl_InvoicesValveDamages.FinancialStatus,dbo.tbl_InvoicesValveDamages.ReciveStatus,
						 dbo.tbl_InvoicesValveDamages.Year,dbo.tbl_InvoicesValveDamages.Month
						 order by CAST(dbo.tbl_InvoicesValveDamages.InvoiceCode as int) desc
						 '
	   EXECUTE(@PivotTableSQL)
END
GO
/****** Object:  StoredProcedure [dbo].[sp_InvoicetoPrint]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1399/05/19>
-- Description:	مشاهده فاکتور دستمزد تبدیل کارگاه ها
-- =============================================
CREATE PROCEDURE [dbo].[sp_InvoicetoPrint] 
	-- Add the parameters for the stored procedure here
	@InvoiceCode nvarchar(max)=null,
	@WorkshopID nvarchar(max)=null
	

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN
SET @PivotTableSQL = N'
select * from (
SELECT [InvoiceID]
      ,[VehicleTypesID]
      ,[WorkshopsID]
	  ,[dbo].[GetWorkshops]([WorkshopsID]) [WorkshopsTitle]
	  ,[dbo].[GetWorkshopPhone]([WorkshopsID]) [WorkshopsPhoneNumber]
	  ,[dbo].[GetWorkshopState]([WorkshopsID]) [WorkshopsState]
	  ,[dbo].[GetWorkshopAddress]([WorkshopsID]) [WorkshopsAddress]
      ,[EquipmentsID]
      ,[ServiceCode]
      ,[InvoiceCode]
      ,[CreatedDate]
      ,[EmployerEconomicalnumber]
      ,[Employerregistrationnumber]
      ,[EmployerState]
      ,[EmployerAddress]
      ,[EmployerPostalcode]
      ,[EmployerPhone]
      ,[EmployerFax]
      ,[ServiceDesc]
      ,[Number]
      ,[UnitofMeasurement]
      ,[UnitAmount]
      ,[TotalAmount]
      ,[DiscountAmount]
      ,[TotalAmountafterDiscount]
      ,[Tax]
      ,[Complications]
      ,[AmountTaxandComplications]
      ,[TotalAmountTaxandComplications]
	  ,[dbo].[GetInvoiceAmount_ToWords]('+@InvoiceCode+','+@workshopId+') [Amount]
	  ,dbo.Num_ToWords([dbo].[GetInvoiceAmount_ToWords]('+@InvoiceCode+','+@workshopId+')) [AmountToWords]
      ,[Description]
      ,[SaleCondition]
      ,[Comment]
      ,[AcceptedAmount]
      ,[AcceptedDate]
      ,[Status]
      ,[CurrencyTypeID]
      ,[CreatorUser]
  FROM [CNGFAPCO].[dbo].[tbl_InvoicesFapa]
  where InvoiceCode='+@InvoiceCode+' and WorkshopsID='+@WorkshopID+') t'

  EXECUTE(@PivotTableSQL)

END
GO
/****** Object:  StoredProcedure [dbo].[sp_InvoiceValveDamagedResultFilterView]    Script Date: 5/30/2025 2:45:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1404/02/15>
-- Description:	مشاهده نتایج تعدادی و ریالی شیرهای مخزن تعویضی به صورت فیلتر شده و در قالب جدول
-- =============================================
CREATE PROCEDURE [dbo].[sp_InvoiceValveDamagedResultFilterView] 
	-- Add the parameters for the stored procedure here
	@workshopId nvarchar(max),
	@Year nvarchar(4),
	@Month nvarchar(2)
	
AS

BEGIN

SELECT [WorkshopsID]
	  ,[Title] as WorkshopTitle
      ,[ServiceCode]
      ,[CreatedDate]      
      ,[ServiceDesc]
      ,[Number]
      ,[UnitofMeasurement]
      ,[UnitAmount]
      ,[TotalAmount]
      ,[DiscountAmount]
      ,[TotalAmountafterDiscount]      
      ,[AmountTaxandComplications]
      ,[TotalAmountTaxandComplications]     
      ,[Year]
      ,[Month]
  FROM [CNGFAPCO].[dbo].[tbl_InvoicesValveDamages] inner join tbl_Workshops
		on tbl_InvoicesValveDamages.WorkshopsID=tbl_Workshops.ID
  where (WorkshopsID = @workshopId or @workshopId = 0) and (Year = @Year or @Year = 0) and (Month = @Month or @Month = 0)
  order by WorkshopsID,Year,Month,ServiceCode
END
GO
/****** Object:  StoredProcedure [dbo].[sp_LatestFinancialStatus]    Script Date: 5/30/2025 2:45:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1404/02/26>
-- Description:	<گزارش آخرین وضعیت مالی کارگاه ها>
-- =============================================
CREATE PROCEDURE [dbo].[sp_LatestFinancialStatus] 
	-- Add the parameters for the stored procedure here
	@condition nvarchar(max),
	@fromDate NVARCHAR(MAX),
	@toDate NVARCHAR(MAX)
	--@condition2 nvarchar(max)=null
	--430962

AS

DECLARE @PivotTableSQL NVARCHAR(MAX)
		

set @fromDate=@fromDate;--'2019-03-20';
set @toDate=@toDate;--'2025-05-21';


if @condition = '0'
BEGIN

SET @PivotTableSQL = N'
		select 
		wp.ID,
		wp.Title ''نام کارگاه'',
		wp.owner ''نام مالک'',

		 -- محاسبات جدید برای بدهکار، بستانکار و مانده
		 FORMAT(CASE WHEN (ISNULL(fMakh.TotalAmount,0) + ISNULL(fShirMakh.TotalAmount,0) + ISNULL(Pardakhtiha.DTab,0) + ISNULL(Pardakhtiha.DTaav,0) + ISNULL(Pardakhtiha.CreditSales,0) + ISNULL(Pardakhtiha.BarCost,0) + ISNULL(Pardakhtiha.HoAn,0) + ISNULL(Pardakhtiha.BimehBack,0)+ ISNULL(Tab.Amount*0.2667,0)) > 0 
					THEN (ISNULL(fMakh.TotalAmount,0) + ISNULL(fShirMakh.TotalAmount,0) + ISNULL(Pardakhtiha.DTab,0) + ISNULL(Pardakhtiha.DTaav,0) + ISNULL(Pardakhtiha.CreditSales,0) + ISNULL(Pardakhtiha.BarCost,0) + ISNULL(Pardakhtiha.HoAn,0) + ISNULL(Pardakhtiha.BimehBack,0)+ ISNULL(Tab.Amount*0.2667,0)) 
					ELSE 0 END, ''N0'') AS ''بدهکار'',

		FORMAT(CASE WHEN (ISNULL(Tab.Amount,0) + ISNULL(Taav.TotalAmount,0) + ISNULL(Varizi.Value,0)+ ISNULL(Baarnameh.RemittanceCost,0)) > 0
					THEN (ISNULL(Tab.Amount,0) + ISNULL(Taav.TotalAmount,0) + ISNULL(Varizi.Value,0)+ ISNULL(Baarnameh.RemittanceCost,0)) 
					ELSE 0 END, ''N0'') AS ''بستانکار'',
    
		FORMAT((CASE WHEN (ISNULL(Tab.Amount,0) + ISNULL(Taav.TotalAmount,0) + ISNULL(Varizi.Value,0)+ ISNULL(Baarnameh.RemittanceCost,0)) > 0
					THEN (ISNULL(Tab.Amount,0) + ISNULL(Taav.TotalAmount,0) + ISNULL(Varizi.Value,0)+ ISNULL(Baarnameh.RemittanceCost,0)) 
					ELSE 0 END)
			  - (CASE WHEN (ISNULL(fMakh.TotalAmount,0) + ISNULL(fShirMakh.TotalAmount,0) + ISNULL(Pardakhtiha.DTab,0) + ISNULL(Pardakhtiha.DTaav,0) + ISNULL(Pardakhtiha.CreditSales,0) + ISNULL(Pardakhtiha.BarCost,0) + ISNULL(Pardakhtiha.HoAn,0) + ISNULL(Pardakhtiha.BimehBack,0)+ ISNULL(Tab.Amount*0.2667,0)) > 0 
					THEN (ISNULL(fMakh.TotalAmount,0) + ISNULL(fShirMakh.TotalAmount,0) + ISNULL(Pardakhtiha.DTab,0) + ISNULL(Pardakhtiha.DTaav,0) + ISNULL(Pardakhtiha.CreditSales,0) + ISNULL(Pardakhtiha.BarCost,0) + ISNULL(Pardakhtiha.HoAn,0) + ISNULL(Pardakhtiha.BimehBack,0)+ ISNULL(Tab.Amount*0.2667,0)) 
					ELSE 0 END), ''N0'') AS ''طلب (بدهی)''

		from

		(
		select ID,Title,(OwnerName+ '' '' + OwnerFamily) owner from tbl_workshops
		where isServices=1) wp 

		LEFT OUTER JOIN
		--دستمزد تبدیل
		(select t2.ID, t2.Title,sum(convert(int, t1.Number)) ''Number'', SUM(TotalAmount) ''Amount'', SUM(TotalAmountTaxandComplications) ''TotalAmount'',''1'' as ''TypeCode'',''Tabdil'' as ''Type'' from tbl_InvoicesFapa t1
		inner join tbl_Workshops t2 on t1.WorkshopsID=t2.ID
		group by t2.ID,t2.Title) Tab 
		on wp.ID = Tab.ID

		LEFT OUTER JOIN
		--مابه التفاوت دستمزد تبدیل
		(select t2.ID, t2.Title,sum(convert(int, t1.Number)) ''Number'', SUM(TotalAmountTaxandComplications) ''TotalAmount'',''1'' as ''TypeCode'',''Tabdil'' as ''Type'' from tbl_InvoicesFapa t1
		inner join tbl_Workshops t2 on t1.WorkshopsID=t2.ID
		where t1.Description like N''%مربوط به مابه التفاوت دستمزد تبدیل می باشد.%''
		group by t2.ID,t2.Title) mTab
		on wp.ID=mTab.ID

		LEFT OUTER JOIN
		--دستمزد تعویض مخزن
		(select t2.ID, t2.Title,sum(convert(int, t1.Number)) ''Number'', SUM(TotalAmountTaxandComplications) ''TotalAmount'',''2'' as ''TypeCode'',''Taaviz'' as ''Type'' from tbl_InvoicesFapa_DamagesCylinder t1
		inner join tbl_Workshops t2 on t1.WorkshopsID=t2.ID
		group by t2.ID,t2.Title) Taav
		on wp.ID = Taav.ID

		LEFT OUTER JOIN
		--فروش  مخزن ضایعاتی
		(select t2.ID, t2.Title,sum(convert(int, t1.Number)) ''Number'', SUM(TotalAmountTaxandComplications) ''TotalAmount'',''3'' as ''TypeCode'',''FroushMakhzan'' as ''Type'' from tbl_InvoicesDamages t1
		inner join tbl_Workshops t2 on t1.WorkshopsID=t2.ID
		group by t2.ID,t2.Title) fMakh
		on wp.ID = fMakh.ID

		LEFT OUTER JOIN
		--فروش شیر ضایعاتی مخزن
		(select t2.ID, t2.Title,sum(convert(int, t1.Number)) ''Number'', SUM(TotalAmountTaxandComplications) ''TotalAmount'',''4'' as ''TypeCode'',''FroushShirMakhzan'' as ''Type'' from tbl_InvoicesValveDamages t1
		inner join tbl_Workshops t2 on t1.WorkshopsID=t2.ID
		group by t2.ID,t2.Title) fShirMakh
		on wp.ID = fShirMakh.ID

		LEFT OUTER JOIN
		--واریزی کارگاه‌ها بابت مخزن ضایعاتی و شیر ضایعاتی مخزن
		(select t2.ID, t2.Title,SUM([Value]) ''Value'',''6'' as ''TypeCode'',''Varizi'' as ''Type'' from [CNGFAPCO].[dbo].[tbl_Deposits] t1
		inner join tbl_Workshops t2 on t1.WorkshopsID=t2.ID
		where t1.Date BETWEEN ''' + CONVERT(VARCHAR(10),@fromDate,120) + ''' and ''' + CONVERT(VARCHAR(10),@toDate,120) + '''
		group by t2.ID,t2.Title) Varizi 
		on wp.ID = Varizi.ID

		LEFT OUTER JOIN
		--بارنامه
		(SELECT t4.ID,t4.Title, SUM(t1.CarryFare) ''RemittanceCost'',''5'' as ''TypeCode'',''Baarnameh'' as ''Type''
		FROM [CNGFAPCO].[dbo].[tbl_RemittanceDetails] t1
		inner join [CNGFAPCO].[dbo].[tbl_Remittances] t2 on t1.RemittancesID=t2.ID
		inner join [CNGFAPCO].[dbo].[tbl_DivisionPlans] t3 on t2.DivisionPlanID=t3.ID
		inner join [CNGFAPCO].[dbo].[tbl_Workshops] t4 on t3.WorkshopID=t4.ID
		where t1.Date BETWEEN ''' + CONVERT(VARCHAR(10),@fromDate,120) + ''' and ''' + CONVERT(VARCHAR(10),@toDate,120) + '''
		group by t4.ID,t4.Title) Baarnameh
		on wp.ID = Baarnameh.ID

		LEFT OUTER JOIN
		--پرداختی ها
		(SELECT 
			WorkshopID,
			WorkshopName AS N''نام کارگاه'',
			[1] AS ''DTab'',
			[2] AS ''BarCost'',
			[3] AS ''AudCost'',
			[6] AS ''CreditSales'',
			[7] AS ''DTaav'',
			[8] AS ''HoAn'',
			[9] AS ''BimehBack''
		FROM 
		(
			SELECT 
				t3.ID AS WorkshopID,
				t3.Title AS WorkshopName,
				t2.ID AS PaymentType,
				t1.Value AS Amount
			FROM 
				[CNGFAPCO].[dbo].[tbl_FinancialPayments] t1
				INNER JOIN tbl_FinancialDescs t2 ON t1.FinancialDescID = t2.ID
				INNER JOIN [CNGFAPCO].[dbo].[tbl_Workshops] t3 ON t1.WorkshopID = t3.ID
				where t1.Status=1 and t1.Date BETWEEN ''' + CONVERT(VARCHAR(10),@fromDate,120) + ''' and ''' + CONVERT(VARCHAR(10),@toDate,120) + '''
		) AS SourceTable
		PIVOT
		(
			SUM(Amount)
			FOR PaymentType IN (
				[1],
				[2],
				[3],
				[6],
				[7],
				[8],
				[9]
			)
		) AS PivotTable) Pardakhtiha

		on wp.ID=Pardakhtiha.WorkshopID
'

EXECUTE(@PivotTableSQL)

END

if @condition = '1'
BEGIN

SET @PivotTableSQL = N'
		select 
		wp.ID,
		wp.Title ''نام کارگاه'',
		wp.owner ''نام مالک'',
		
		 -- محاسبات جدید برای بدهکار، بستانکار و مانده
		 FORMAT(CASE WHEN (ISNULL(fMakh.TotalAmount,0) + ISNULL(fShirMakh.TotalAmount,0) + ISNULL(Pardakhtiha.DTab,0) + ISNULL(Pardakhtiha.DTaav,0) + ISNULL(Pardakhtiha.CreditSales,0) + ISNULL(Pardakhtiha.BarCost,0) + ISNULL(Pardakhtiha.HoAn,0) + ISNULL(Pardakhtiha.BimehBack,0)+ ISNULL(Tab.Amount*0.2667,0)) > 0 
					THEN (ISNULL(fMakh.TotalAmount,0) + ISNULL(fShirMakh.TotalAmount,0) + ISNULL(Pardakhtiha.DTab,0) + ISNULL(Pardakhtiha.DTaav,0) + ISNULL(Pardakhtiha.CreditSales,0) + ISNULL(Pardakhtiha.BarCost,0) + ISNULL(Pardakhtiha.HoAn,0) + ISNULL(Pardakhtiha.BimehBack,0)+ ISNULL(Tab.Amount*0.2667,0)) 
					ELSE 0 END, ''N0'') AS ''بدهکار'',

		FORMAT(CASE WHEN (ISNULL(Tab.Amount,0) + ISNULL(Taav.TotalAmount,0) + ISNULL(Varizi.Value,0)+ ISNULL(Baarnameh.RemittanceCost,0)) > 0
					THEN (ISNULL(Tab.Amount,0) + ISNULL(Taav.TotalAmount,0) + ISNULL(Varizi.Value,0)+ ISNULL(Baarnameh.RemittanceCost,0)) 
					ELSE 0 END, ''N0'') AS ''بستانکار'',
    
		FORMAT((CASE WHEN (ISNULL(Tab.Amount,0) + ISNULL(Taav.TotalAmount,0) + ISNULL(Varizi.Value,0)+ ISNULL(Baarnameh.RemittanceCost,0)) > 0
					THEN (ISNULL(Tab.Amount,0) + ISNULL(Taav.TotalAmount,0) + ISNULL(Varizi.Value,0)+ ISNULL(Baarnameh.RemittanceCost,0)) 
					ELSE 0 END)
			  - (CASE WHEN (ISNULL(fMakh.TotalAmount,0) + ISNULL(fShirMakh.TotalAmount,0) + ISNULL(Pardakhtiha.DTab,0) + ISNULL(Pardakhtiha.DTaav,0) + ISNULL(Pardakhtiha.CreditSales,0) + ISNULL(Pardakhtiha.BarCost,0) + ISNULL(Pardakhtiha.HoAn,0) + ISNULL(Pardakhtiha.BimehBack,0)+ ISNULL(Tab.Amount*0.2667,0)) > 0 
					THEN (ISNULL(fMakh.TotalAmount,0) + ISNULL(fShirMakh.TotalAmount,0) + ISNULL(Pardakhtiha.DTab,0) + ISNULL(Pardakhtiha.DTaav,0) + ISNULL(Pardakhtiha.CreditSales,0) + ISNULL(Pardakhtiha.BarCost,0) + ISNULL(Pardakhtiha.HoAn,0) + ISNULL(Pardakhtiha.BimehBack,0)+ ISNULL(Tab.Amount*0.2667,0)) 
					ELSE 0 END), ''N0'') AS ''طلب (بدهی)''

		from

		(
		select ID,Title,(OwnerName+ '' '' + OwnerFamily) owner from tbl_workshops
		where isServices=1) wp 

		LEFT OUTER JOIN
		--دستمزد تبدیل
		(select t2.ID, t2.Title,sum(convert(int, t1.Number)) ''Number'', SUM(TotalAmount) ''Amount'', SUM(TotalAmountTaxandComplications) ''TotalAmount'',''1'' as ''TypeCode'',''Tabdil'' as ''Type'' from tbl_InvoicesFapa t1
		inner join tbl_Workshops t2 on t1.WorkshopsID=t2.ID
		where t1.FinancialStatus = 1
		group by t2.ID,t2.Title) Tab 
		on wp.ID = Tab.ID

		LEFT OUTER JOIN
		--مابه التفاوت دستمزد تبدیل
		(select t2.ID, t2.Title,sum(convert(int, t1.Number)) ''Number'', SUM(TotalAmountTaxandComplications) ''TotalAmount'',''1'' as ''TypeCode'',''Tabdil'' as ''Type'' from tbl_InvoicesFapa t1
		inner join tbl_Workshops t2 on t1.WorkshopsID=t2.ID
		where t1.FinancialStatus = 1 and t1.Description like N''%مربوط به مابه التفاوت دستمزد تبدیل می باشد.%''
		group by t2.ID,t2.Title) mTab
		on wp.ID=mTab.ID

		LEFT OUTER JOIN
		--دستمزد تعویض مخزن
		(select t2.ID, t2.Title,sum(convert(int, t1.Number)) ''Number'', SUM(TotalAmountTaxandComplications) ''TotalAmount'',''2'' as ''TypeCode'',''Taaviz'' as ''Type'' from tbl_InvoicesFapa_DamagesCylinder t1
		inner join tbl_Workshops t2 on t1.WorkshopsID=t2.ID
		where t1.FinancialStatus = 1
		group by t2.ID,t2.Title) Taav
		on wp.ID = Taav.ID

		LEFT OUTER JOIN
		--فروش  مخزن ضایعاتی
		(select t2.ID, t2.Title,sum(convert(int, t1.Number)) ''Number'', SUM(TotalAmountTaxandComplications) ''TotalAmount'',''3'' as ''TypeCode'',''FroushMakhzan'' as ''Type'' from tbl_InvoicesDamages t1
		inner join tbl_Workshops t2 on t1.WorkshopsID=t2.ID
		group by t2.ID,t2.Title) fMakh
		on wp.ID = fMakh.ID

		LEFT OUTER JOIN
		--فروش شیر ضایعاتی مخزن
		(select t2.ID, t2.Title,sum(convert(int, t1.Number)) ''Number'', SUM(TotalAmountTaxandComplications) ''TotalAmount'',''4'' as ''TypeCode'',''FroushShirMakhzan'' as ''Type'' from tbl_InvoicesValveDamages t1
		inner join tbl_Workshops t2 on t1.WorkshopsID=t2.ID
		group by t2.ID,t2.Title) fShirMakh
		on wp.ID = fShirMakh.ID

		LEFT OUTER JOIN
		--واریزی کارگاه‌ها بابت مخزن ضایعاتی و شیر ضایعاتی مخزن
		(select t2.ID, t2.Title,SUM([Value]) ''Value'',''6'' as ''TypeCode'',''Varizi'' as ''Type'' from [CNGFAPCO].[dbo].[tbl_Deposits] t1
		inner join tbl_Workshops t2 on t1.WorkshopsID=t2.ID
		where t1.Date BETWEEN ''' + CONVERT(VARCHAR(10),@fromDate,120) + ''' and ''' + CONVERT(VARCHAR(10),@toDate,120) + '''
		group by t2.ID,t2.Title) Varizi 
		on wp.ID = Varizi.ID

		LEFT OUTER JOIN
		--بارنامه
		(SELECT t4.ID,t4.Title, SUM(t1.CarryFare) ''RemittanceCost'',''5'' as ''TypeCode'',''Baarnameh'' as ''Type''
		FROM [CNGFAPCO].[dbo].[tbl_RemittanceDetails] t1
		inner join [CNGFAPCO].[dbo].[tbl_Remittances] t2 on t1.RemittancesID=t2.ID
		inner join [CNGFAPCO].[dbo].[tbl_DivisionPlans] t3 on t2.DivisionPlanID=t3.ID
		inner join [CNGFAPCO].[dbo].[tbl_Workshops] t4 on t3.WorkshopID=t4.ID
		where (t1.FinancialDate is not Null) or (t1.FinancialUser is not Null) and (t1.Date BETWEEN ''' + CONVERT(VARCHAR(10),@fromDate,120) + ''' and ''' + CONVERT(VARCHAR(10),@toDate,120) + ''')
		group by t4.ID,t4.Title) Baarnameh
		on wp.ID = Baarnameh.ID

		LEFT OUTER JOIN
		--پرداختی ها
		(SELECT 
			WorkshopID,
			WorkshopName AS N''نام کارگاه'',
			[1] AS ''DTab'',
			[2] AS ''BarCost'',
			[3] AS ''AudCost'',
			[6] AS ''CreditSales'',
			[7] AS ''DTaav'',
			[8] AS ''HoAn'',
			[9] AS ''BimehBack''
		FROM 
		(
			SELECT 
				t3.ID AS WorkshopID,
				t3.Title AS WorkshopName,
				t2.ID AS PaymentType,
				t1.Value AS Amount
			FROM 
				[CNGFAPCO].[dbo].[tbl_FinancialPayments] t1
				INNER JOIN tbl_FinancialDescs t2 ON t1.FinancialDescID = t2.ID
				INNER JOIN [CNGFAPCO].[dbo].[tbl_Workshops] t3 ON t1.WorkshopID = t3.ID
				where t1.Status=1 and t1.Date BETWEEN ''' + CONVERT(VARCHAR(10),@fromDate,120) + ''' and ''' + CONVERT(VARCHAR(10),@toDate,120) + '''
		) AS SourceTable
		PIVOT
		(
			SUM(Amount)
			FOR PaymentType IN (
				[1],
				[2],
				[3],
				[6],
				[7],
				[8],
				[9]
			)
		) AS PivotTable) Pardakhtiha

		on wp.ID=Pardakhtiha.WorkshopID
'

EXECUTE(@PivotTableSQL)

END

GO
/****** Object:  StoredProcedure [dbo].[sp_LatestFinancialStatusDetails]    Script Date: 5/30/2025 2:45:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal>
-- Create date: <1404/02/28>
-- Description:	< رفتن به سطح جزئیات بیشتر از گزارش آخرین وضعیت مالی کارگاه ها>
-- =============================================
CREATE PROCEDURE [dbo].[sp_LatestFinancialStatusDetails] 
	-- Add the parameters for the stored procedure here
	@condition nvarchar(max),
	@fromDate NVARCHAR(MAX),
	@toDate NVARCHAR(MAX)
	--@condition2 nvarchar(max)=null
	--430962

AS

DECLARE @PivotTableSQL NVARCHAR(MAX)
		

set @fromDate=@fromDate;--'2019-03-20';
set @toDate=@toDate;--'2025-05-21';

if @condition = '0'
BEGIN

SET @PivotTableSQL = N'
		select 
		wp.ID,
		wp.Title ''نام کارگاه'',
		wp.owner ''نام مالک'',
		(isnull(convert(int, Tab.Number),0) - isnull(convert(int, mTab.Number),0)) ''تعداد تبدیل'',
		FORMAT(Tab.Amount,''N0'') ''دستمزد تبدیل'',
		Taav.Number ''تعداد تعویض'',
		FORMAT(Taav.TotalAmount,''N0'') ''دستمزد تعویض'',
		fMakh.Number ''تعداد مخزن'',
		FORMAT(fMakh.TotalAmount ,''N0'') ''فروش مخزن'',
		fShirMakh.Number ''تعداد شیر مخزن'',
		FORMAT(fShirMakh.TotalAmount,''N0'') ''فروش شیر مخزن'',
		FORMAT(Varizi.Value,''N0'') ''واریزی'',
		FORMAT(Baarnameh.RemittanceCost,''N0'') ''هزینه بارنامه'',
		--FORMAT((isnull(convert(int, Tab.Number),0) - isnull(convert(int, mTab.Number),0)) * 600000,''N0'') ''هزینه بازرسی'',
		FORMAT(Pardakhtiha.DTab,''N0'') ''دستمزد تبدیل پرداختی(ق)'',
		FORMAT(Pardakhtiha.DTaav,''N0'') ''دستمزد تعویض پرداختی(ق)'',
		FORMAT(Pardakhtiha.CreditSales,''N0'') ''فروش اعتباری'',
		FORMAT(Pardakhtiha.BarCost,''N0'') ''هزینه بارنامه پرداختی(ق)'',

		FORMAT(NGPardakhtiha.DTab,''N0'') ''دستمزد تبدیل پرداختی(غ-ق)'',
		FORMAT(NGPardakhtiha.DTaav,''N0'') ''دستمزد تعویض پرداختی(غ-ق)'',
		FORMAT(NGPardakhtiha.BarCost,''N0'') ''هزینه بارنامه پرداختی(غ-ق)'',

		--FORMAT(Pardakhtiha.AudCost,''N0'') ''هزینه بازرسی پرداختی(ق)'',
		FORMAT(Tab.Amount * 0.1,''N0'') ''کسورات حسن انجام'',
		FORMAT(Tab.Amount * 0.1667,''N0'') ''کسورات بیمه'',
		FORMAT(Pardakhtiha.HoAn,''N0'') ''آزادسازی حسن انجام'',
		FORMAT(Pardakhtiha.BimehBack,''N0'') ''استرداد کسورات بیمه'',
		
		 -- محاسبات جدید برای بدهکار، بستانکار و مانده
		 FORMAT(CASE WHEN (ISNULL(fMakh.TotalAmount,0) + ISNULL(fShirMakh.TotalAmount,0) + ISNULL(Pardakhtiha.DTab,0) + ISNULL(Pardakhtiha.DTaav,0) + ISNULL(Pardakhtiha.CreditSales,0) + ISNULL(Pardakhtiha.BarCost,0) + ISNULL(Pardakhtiha.HoAn,0) + ISNULL(Pardakhtiha.BimehBack,0)+ ISNULL(Tab.Amount*0.2667,0)) > 0 
					THEN (ISNULL(fMakh.TotalAmount,0) + ISNULL(fShirMakh.TotalAmount,0) + ISNULL(Pardakhtiha.DTab,0) + ISNULL(Pardakhtiha.DTaav,0) + ISNULL(Pardakhtiha.CreditSales,0) + ISNULL(Pardakhtiha.BarCost,0) + ISNULL(Pardakhtiha.HoAn,0) + ISNULL(Pardakhtiha.BimehBack,0)+ ISNULL(Tab.Amount*0.2667,0)) 
					ELSE 0 END, ''N0'') AS ''بدهکار'',

		FORMAT(CASE WHEN (ISNULL(Tab.Amount,0) + ISNULL(Taav.TotalAmount,0) + ISNULL(Varizi.Value,0)+ ISNULL(Baarnameh.RemittanceCost,0)) > 0
					THEN (ISNULL(Tab.Amount,0) + ISNULL(Taav.TotalAmount,0) + ISNULL(Varizi.Value,0)+ ISNULL(Baarnameh.RemittanceCost,0)) 
					ELSE 0 END, ''N0'') AS ''بستانکار'',
    
		FORMAT((CASE WHEN (ISNULL(Tab.Amount,0) + ISNULL(Taav.TotalAmount,0) + ISNULL(Varizi.Value,0)+ ISNULL(Baarnameh.RemittanceCost,0)) > 0
					THEN (ISNULL(Tab.Amount,0) + ISNULL(Taav.TotalAmount,0) + ISNULL(Varizi.Value,0)+ ISNULL(Baarnameh.RemittanceCost,0)) 
					ELSE 0 END)
			  - (CASE WHEN (ISNULL(fMakh.TotalAmount,0) + ISNULL(fShirMakh.TotalAmount,0) + ISNULL(Pardakhtiha.DTab,0) + ISNULL(Pardakhtiha.DTaav,0) + ISNULL(Pardakhtiha.CreditSales,0) + ISNULL(Pardakhtiha.BarCost,0) + ISNULL(Pardakhtiha.HoAn,0) + ISNULL(Pardakhtiha.BimehBack,0)+ ISNULL(Tab.Amount*0.2667,0)) > 0 
					THEN (ISNULL(fMakh.TotalAmount,0) + ISNULL(fShirMakh.TotalAmount,0) + ISNULL(Pardakhtiha.DTab,0) + ISNULL(Pardakhtiha.DTaav,0) + ISNULL(Pardakhtiha.CreditSales,0) + ISNULL(Pardakhtiha.BarCost,0) + ISNULL(Pardakhtiha.HoAn,0) + ISNULL(Pardakhtiha.BimehBack,0)+ ISNULL(Tab.Amount*0.2667,0)) 
					ELSE 0 END), ''N0'') AS ''طلب (بدهی)''

		from

		(
		select ID,Title,(OwnerName+ '' '' + OwnerFamily) owner from tbl_workshops
		where isServices=1) wp 

		LEFT OUTER JOIN
		--دستمزد تبدیل
		(select t2.ID, t2.Title,sum(convert(int, t1.Number)) ''Number'', SUM(TotalAmount) ''Amount'', SUM(TotalAmountTaxandComplications) ''TotalAmount'',''1'' as ''TypeCode'',''Tabdil'' as ''Type'' from tbl_InvoicesFapa t1
		inner join tbl_Workshops t2 on t1.WorkshopsID=t2.ID
		group by t2.ID,t2.Title) Tab 
		on wp.ID = Tab.ID

		LEFT OUTER JOIN
		--مابه التفاوت دستمزد تبدیل
		(select t2.ID, t2.Title,sum(convert(int, t1.Number)) ''Number'', SUM(TotalAmountTaxandComplications) ''TotalAmount'',''1'' as ''TypeCode'',''Tabdil'' as ''Type'' from tbl_InvoicesFapa t1
		inner join tbl_Workshops t2 on t1.WorkshopsID=t2.ID
		where t1.Description like N''%مربوط به مابه التفاوت دستمزد تبدیل می باشد.%''
		group by t2.ID,t2.Title) mTab
		on wp.ID=mTab.ID

		LEFT OUTER JOIN
		--دستمزد تعویض مخزن
		(select t2.ID, t2.Title,sum(convert(int, t1.Number)) ''Number'', SUM(TotalAmountTaxandComplications) ''TotalAmount'',''2'' as ''TypeCode'',''Taaviz'' as ''Type'' from tbl_InvoicesFapa_DamagesCylinder t1
		inner join tbl_Workshops t2 on t1.WorkshopsID=t2.ID
		group by t2.ID,t2.Title) Taav
		on wp.ID = Taav.ID

		LEFT OUTER JOIN
		--فروش  مخزن ضایعاتی
		(select t2.ID, t2.Title,sum(convert(int, t1.Number)) ''Number'', SUM(TotalAmountTaxandComplications) ''TotalAmount'',''3'' as ''TypeCode'',''FroushMakhzan'' as ''Type'' from tbl_InvoicesDamages t1
		inner join tbl_Workshops t2 on t1.WorkshopsID=t2.ID
		group by t2.ID,t2.Title) fMakh
		on wp.ID = fMakh.ID

		LEFT OUTER JOIN
		--فروش شیر ضایعاتی مخزن
		(select t2.ID, t2.Title,sum(convert(int, t1.Number)) ''Number'', SUM(TotalAmountTaxandComplications) ''TotalAmount'',''4'' as ''TypeCode'',''FroushShirMakhzan'' as ''Type'' from tbl_InvoicesValveDamages t1
		inner join tbl_Workshops t2 on t1.WorkshopsID=t2.ID
		group by t2.ID,t2.Title) fShirMakh
		on wp.ID = fShirMakh.ID

		LEFT OUTER JOIN
		--واریزی کارگاه‌ها بابت مخزن ضایعاتی و شیر ضایعاتی مخزن
		(select t2.ID, t2.Title,SUM([Value]) ''Value'',''6'' as ''TypeCode'',''Varizi'' as ''Type'' from [CNGFAPCO].[dbo].[tbl_Deposits] t1
		inner join tbl_Workshops t2 on t1.WorkshopsID=t2.ID
		where t1.Date BETWEEN ''' + CONVERT(VARCHAR(10),@fromDate,120) + ''' and ''' + CONVERT(VARCHAR(10),@toDate,120) + '''
		group by t2.ID,t2.Title) Varizi 
		on wp.ID = Varizi.ID

		LEFT OUTER JOIN
		--بارنامه
		(SELECT t4.ID,t4.Title, SUM(t1.CarryFare) ''RemittanceCost'',''5'' as ''TypeCode'',''Baarnameh'' as ''Type''
		FROM [CNGFAPCO].[dbo].[tbl_RemittanceDetails] t1
		inner join [CNGFAPCO].[dbo].[tbl_Remittances] t2 on t1.RemittancesID=t2.ID
		inner join [CNGFAPCO].[dbo].[tbl_DivisionPlans] t3 on t2.DivisionPlanID=t3.ID
		inner join [CNGFAPCO].[dbo].[tbl_Workshops] t4 on t3.WorkshopID=t4.ID
		where t1.Date BETWEEN ''' + CONVERT(VARCHAR(10),@fromDate,120) + ''' and ''' + CONVERT(VARCHAR(10),@toDate,120) + '''
		group by t4.ID,t4.Title) Baarnameh
		on wp.ID = Baarnameh.ID

		LEFT OUTER JOIN
		--پرداختی ها (قطعی)
		(SELECT 
			WorkshopID,
			WorkshopName AS N''نام کارگاه'',
			[1] AS ''DTab'',
			[2] AS ''BarCost'',
			[3] AS ''AudCost'',
			[6] AS ''CreditSales'',
			[7] AS ''DTaav'',
			[8] AS ''HoAn'',
			[9] AS ''BimehBack''
		FROM 
		(
			SELECT 
				t3.ID AS WorkshopID,
				t3.Title AS WorkshopName,
				t2.ID AS PaymentType,
				t1.Value AS Amount
			FROM 
				[CNGFAPCO].[dbo].[tbl_FinancialPayments] t1
				INNER JOIN tbl_FinancialDescs t2 ON t1.FinancialDescID = t2.ID
				INNER JOIN [CNGFAPCO].[dbo].[tbl_Workshops] t3 ON t1.WorkshopID = t3.ID
				where t1.Status=1 and t1.Date BETWEEN ''' + CONVERT(VARCHAR(10),@fromDate,120) + ''' and ''' + CONVERT(VARCHAR(10),@toDate,120) + '''
		) AS SourceTable
		PIVOT
		(
			SUM(Amount)
			FOR PaymentType IN (
				[1],
				[2],
				[3],
				[6],
				[7],
				[8],
				[9]
			)
		) AS PivotTable) Pardakhtiha

		on wp.ID=Pardakhtiha.WorkshopID

		LEFT OUTER JOIN
		--پرداختی ها (غیر قطعی)
		(SELECT 
			WorkshopID,
			WorkshopName AS N''نام کارگاه'',
			[1] AS ''DTab'',
			[2] AS ''BarCost'',
			[3] AS ''AudCost'',
			[6] AS ''CreditSales'',
			[7] AS ''DTaav'',
			[8] AS ''HoAn'',
			[9] AS ''BimehBack''
		FROM 
		(
			SELECT 
				t3.ID AS WorkshopID,
				t3.Title AS WorkshopName,
				t2.ID AS PaymentType,
				t1.Value AS Amount
			FROM 
				[CNGFAPCO].[dbo].[tbl_FinancialPayments] t1
				INNER JOIN tbl_FinancialDescs t2 ON t1.FinancialDescID = t2.ID
				INNER JOIN [CNGFAPCO].[dbo].[tbl_Workshops] t3 ON t1.WorkshopID = t3.ID
				where t1.Status=0 and  t1.Date BETWEEN ''' + CONVERT(VARCHAR(10),@fromDate,120) + ''' and ''' + CONVERT(VARCHAR(10),@toDate,120) + '''
		) AS SourceTable
		PIVOT
		(
			SUM(Amount)
			FOR PaymentType IN (
				[1],
				[2],
				[3],
				[6],
				[7],
				[8],
				[9]
			)
		) AS PivotTable) NGPardakhtiha

		on wp.ID=NGPardakhtiha.WorkshopID
'

EXECUTE(@PivotTableSQL)

END

if @condition = '1'
BEGIN

SET @PivotTableSQL = N'
		select 
		wp.ID,
		wp.Title ''نام کارگاه'',
		wp.owner ''نام مالک'',
		(isnull(convert(int, Tab.Number),0) - isnull(convert(int, mTab.Number),0)) ''تعداد تبدیل'',
		FORMAT(Tab.Amount,''N0'') ''دستمزد تبدیل'',
		Taav.Number ''تعداد تعویض'',
		FORMAT(Taav.TotalAmount,''N0'') ''دستمزد تعویض'',
		fMakh.Number ''تعداد مخزن'',
		FORMAT(fMakh.TotalAmount ,''N0'') ''فروش مخزن'',
		fShirMakh.Number ''تعداد شیر مخزن'',
		FORMAT(fShirMakh.TotalAmount,''N0'') ''فروش شیر مخزن'',
		FORMAT(Varizi.Value,''N0'') ''واریزی'',
		FORMAT(Baarnameh.RemittanceCost,''N0'') ''هزینه بارنامه'',
		--FORMAT((isnull(convert(int, Tab.Number),0) - isnull(convert(int, mTab.Number),0)) * 600000,''N0'') ''هزینه بازرسی'',
		FORMAT(Pardakhtiha.DTab,''N0'') ''دستمزد تبدیل پرداختی(ق)'',
		FORMAT(Pardakhtiha.DTaav,''N0'') ''دستمزد تعویض پرداختی(ق)'',
		FORMAT(Pardakhtiha.CreditSales,''N0'') ''فروش اعتباری'',
		FORMAT(Pardakhtiha.BarCost,''N0'') ''هزینه بارنامه پرداختی(ق)'',
	
		FORMAT(NGPardakhtiha.DTab,''N0'') ''دستمزد تبدیل پرداختی(غ-ق)'',
		FORMAT(NGPardakhtiha.DTaav,''N0'') ''دستمزد تعویض پرداختی(غ-ق)'',
		FORMAT(NGPardakhtiha.BarCost,''N0'') ''هزینه بارنامه پرداختی(غ-ق)'',

		--FORMAT(Pardakhtiha.AudCost,''N0'') ''هزینه بازرسی پرداختی'',
		FORMAT(Tab.Amount * 0.1,''N0'') ''کسورات حسن انجام'',
		FORMAT(Tab.Amount * 0.1667,''N0'') ''کسورات بیمه'',
		FORMAT(Pardakhtiha.HoAn,''N0'') ''آزادسازی حسن انجام'',
		FORMAT(Pardakhtiha.BimehBack,''N0'') ''استرداد کسورات بیمه'',

		-- محاسبات جدید برای بدهکار، بستانکار و مانده
		FORMAT(CASE WHEN (ISNULL(fMakh.TotalAmount,0) + ISNULL(fShirMakh.TotalAmount,0) + ISNULL(Pardakhtiha.DTab,0) + ISNULL(Pardakhtiha.DTaav,0) + ISNULL(Pardakhtiha.CreditSales,0) + ISNULL(Pardakhtiha.BarCost,0) + ISNULL(Pardakhtiha.HoAn,0) + ISNULL(Pardakhtiha.BimehBack,0)+ ISNULL(Tab.Amount*0.2667,0)) > 0 
					THEN (ISNULL(fMakh.TotalAmount,0) + ISNULL(fShirMakh.TotalAmount,0) + ISNULL(Pardakhtiha.DTab,0) + ISNULL(Pardakhtiha.DTaav,0) + ISNULL(Pardakhtiha.CreditSales,0) + ISNULL(Pardakhtiha.BarCost,0) + ISNULL(Pardakhtiha.HoAn,0) + ISNULL(Pardakhtiha.BimehBack,0)+ ISNULL(Tab.Amount*0.2667,0)) 
					ELSE 0 END, ''N0'') AS ''بدهکار'',

		FORMAT(CASE WHEN (ISNULL(Tab.Amount,0) + ISNULL(Taav.TotalAmount,0) + ISNULL(Varizi.Value,0)+ ISNULL(Baarnameh.RemittanceCost,0)) > 0
					THEN (ISNULL(Tab.Amount,0) + ISNULL(Taav.TotalAmount,0) + ISNULL(Varizi.Value,0)+ ISNULL(Baarnameh.RemittanceCost,0)) 
					ELSE 0 END, ''N0'') AS ''بستانکار'',
    
		FORMAT((CASE WHEN (ISNULL(Tab.Amount,0) + ISNULL(Taav.TotalAmount,0) + ISNULL(Varizi.Value,0)+ ISNULL(Baarnameh.RemittanceCost,0)) > 0
					THEN (ISNULL(Tab.Amount,0) + ISNULL(Taav.TotalAmount,0) + ISNULL(Varizi.Value,0)+ ISNULL(Baarnameh.RemittanceCost,0)) 
					ELSE 0 END)
			  - (CASE WHEN (ISNULL(fMakh.TotalAmount,0) + ISNULL(fShirMakh.TotalAmount,0) + ISNULL(Pardakhtiha.DTab,0) + ISNULL(Pardakhtiha.DTaav,0) + ISNULL(Pardakhtiha.CreditSales,0) + ISNULL(Pardakhtiha.BarCost,0) + ISNULL(Pardakhtiha.HoAn,0) + ISNULL(Pardakhtiha.BimehBack,0)+ ISNULL(Tab.Amount*0.2667,0)) > 0 
					THEN (ISNULL(fMakh.TotalAmount,0) + ISNULL(fShirMakh.TotalAmount,0) + ISNULL(Pardakhtiha.DTab,0) + ISNULL(Pardakhtiha.DTaav,0) + ISNULL(Pardakhtiha.CreditSales,0) + ISNULL(Pardakhtiha.BarCost,0) + ISNULL(Pardakhtiha.HoAn,0) + ISNULL(Pardakhtiha.BimehBack,0)+ ISNULL(Tab.Amount*0.2667,0)) 
					ELSE 0 END), ''N0'') AS ''طلب (بدهی)''

		from

		(
		select ID,Title,(OwnerName+ '' '' + OwnerFamily) owner from tbl_workshops
		where isServices=1) wp 

		LEFT OUTER JOIN
		--دستمزد تبدیل
		(select t2.ID, t2.Title,sum(convert(int, t1.Number)) ''Number'', SUM(TotalAmount) ''Amount'', SUM(TotalAmountTaxandComplications) ''TotalAmount'',''1'' as ''TypeCode'',''Tabdil'' as ''Type'' from tbl_InvoicesFapa t1
		inner join tbl_Workshops t2 on t1.WorkshopsID=t2.ID
		where t1.FinancialStatus = 1
		group by t2.ID,t2.Title) Tab 
		on wp.ID = Tab.ID

		LEFT OUTER JOIN
		--مابه التفاوت دستمزد تبدیل
		(select t2.ID, t2.Title,sum(convert(int, t1.Number)) ''Number'', SUM(TotalAmountTaxandComplications) ''TotalAmount'',''1'' as ''TypeCode'',''Tabdil'' as ''Type'' from tbl_InvoicesFapa t1
		inner join tbl_Workshops t2 on t1.WorkshopsID=t2.ID
		where t1.FinancialStatus = 1 and t1.Description like N''%مربوط به مابه التفاوت دستمزد تبدیل می باشد.%''
		group by t2.ID,t2.Title) mTab
		on wp.ID=mTab.ID

		LEFT OUTER JOIN
		--دستمزد تعویض مخزن
		(select t2.ID, t2.Title,sum(convert(int, t1.Number)) ''Number'', SUM(TotalAmountTaxandComplications) ''TotalAmount'',''2'' as ''TypeCode'',''Taaviz'' as ''Type'' from tbl_InvoicesFapa_DamagesCylinder t1
		inner join tbl_Workshops t2 on t1.WorkshopsID=t2.ID
		where t1.FinancialStatus = 1
		group by t2.ID,t2.Title) Taav
		on wp.ID = Taav.ID

		LEFT OUTER JOIN
		--فروش  مخزن ضایعاتی
		(select t2.ID, t2.Title,sum(convert(int, t1.Number)) ''Number'', SUM(TotalAmountTaxandComplications) ''TotalAmount'',''3'' as ''TypeCode'',''FroushMakhzan'' as ''Type'' from tbl_InvoicesDamages t1
		inner join tbl_Workshops t2 on t1.WorkshopsID=t2.ID
		group by t2.ID,t2.Title) fMakh
		on wp.ID = fMakh.ID

		LEFT OUTER JOIN
		--فروش شیر ضایعاتی مخزن
		(select t2.ID, t2.Title,sum(convert(int, t1.Number)) ''Number'', SUM(TotalAmountTaxandComplications) ''TotalAmount'',''4'' as ''TypeCode'',''FroushShirMakhzan'' as ''Type'' from tbl_InvoicesValveDamages t1
		inner join tbl_Workshops t2 on t1.WorkshopsID=t2.ID
		group by t2.ID,t2.Title) fShirMakh
		on wp.ID = fShirMakh.ID

		LEFT OUTER JOIN
		--واریزی کارگاه‌ها بابت مخزن ضایعاتی و شیر ضایعاتی مخزن
		(select t2.ID, t2.Title,SUM([Value]) ''Value'',''6'' as ''TypeCode'',''Varizi'' as ''Type'' from [CNGFAPCO].[dbo].[tbl_Deposits] t1
		inner join tbl_Workshops t2 on t1.WorkshopsID=t2.ID
		where t1.Date BETWEEN ''' + CONVERT(VARCHAR(10),@fromDate,120) + ''' and ''' + CONVERT(VARCHAR(10),@toDate,120) + '''
		group by t2.ID,t2.Title) Varizi 
		on wp.ID = Varizi.ID

		LEFT OUTER JOIN
		--بارنامه
		(SELECT t4.ID,t4.Title, SUM(t1.CarryFare) ''RemittanceCost'',''5'' as ''TypeCode'',''Baarnameh'' as ''Type''
		FROM [CNGFAPCO].[dbo].[tbl_RemittanceDetails] t1
		inner join [CNGFAPCO].[dbo].[tbl_Remittances] t2 on t1.RemittancesID=t2.ID
		inner join [CNGFAPCO].[dbo].[tbl_DivisionPlans] t3 on t2.DivisionPlanID=t3.ID
		inner join [CNGFAPCO].[dbo].[tbl_Workshops] t4 on t3.WorkshopID=t4.ID
		where (t1.FinancialDate is not Null) or (t1.FinancialUser is not Null) and (t1.Date BETWEEN ''' + CONVERT(VARCHAR(10),@fromDate,120) + ''' and ''' + CONVERT(VARCHAR(10),@toDate,120) + ''')
		group by t4.ID,t4.Title) Baarnameh
		on wp.ID = Baarnameh.ID

		LEFT OUTER JOIN
		--پرداختی ها
		(SELECT 
			WorkshopID,
			WorkshopName AS N''نام کارگاه'',
			[1] AS ''DTab'',
			[2] AS ''BarCost'',
			[3] AS ''AudCost'',
			[6] AS ''CreditSales'',
			[7] AS ''DTaav'',
			[8] AS ''HoAn'',
			[9] AS ''BimehBack''
		FROM 
		(
			SELECT 
				t3.ID AS WorkshopID,
				t3.Title AS WorkshopName,
				t2.ID AS PaymentType,
				t1.Value AS Amount
			FROM 
				[CNGFAPCO].[dbo].[tbl_FinancialPayments] t1
				INNER JOIN tbl_FinancialDescs t2 ON t1.FinancialDescID = t2.ID
				INNER JOIN [CNGFAPCO].[dbo].[tbl_Workshops] t3 ON t1.WorkshopID = t3.ID
				where t1.Status=1 and t1.Date BETWEEN ''' + CONVERT(VARCHAR(10),@fromDate,120) + ''' and ''' + CONVERT(VARCHAR(10),@toDate,120) + '''
		) AS SourceTable
		PIVOT
		(
			SUM(Amount)
			FOR PaymentType IN (
				[1],
				[2],
				[3],
				[6],
				[7],
				[8],
				[9]
			)
		) AS PivotTable) Pardakhtiha

		on wp.ID=Pardakhtiha.WorkshopID

		LEFT OUTER JOIN
		--پرداختی ها (غیر قطعی)
		(SELECT 
			WorkshopID,
			WorkshopName AS N''نام کارگاه'',
			[1] AS ''DTab'',
			[2] AS ''BarCost'',
			[3] AS ''AudCost'',
			[6] AS ''CreditSales'',
			[7] AS ''DTaav'',
			[8] AS ''HoAn'',
			[9] AS ''BimehBack''
		FROM 
		(
			SELECT 
				t3.ID AS WorkshopID,
				t3.Title AS WorkshopName,
				t2.ID AS PaymentType,
				t1.Value AS Amount
			FROM 
				[CNGFAPCO].[dbo].[tbl_FinancialPayments] t1
				INNER JOIN tbl_FinancialDescs t2 ON t1.FinancialDescID = t2.ID
				INNER JOIN [CNGFAPCO].[dbo].[tbl_Workshops] t3 ON t1.WorkshopID = t3.ID
				where t1.Status=0 and t1.Date BETWEEN ''' + CONVERT(VARCHAR(10),@fromDate,120) + ''' and ''' + CONVERT(VARCHAR(10),@toDate,120) + '''
		) AS SourceTable
		PIVOT
		(
			SUM(Amount)
			FOR PaymentType IN (
				[1],
				[2],
				[3],
				[6],
				[7],
				[8],
				[9]
			)
		) AS PivotTable) NGPardakhtiha

		on wp.ID=NGPardakhtiha.WorkshopID
		
'

EXECUTE(@PivotTableSQL)

END

GO
/****** Object:  StoredProcedure [dbo].[sp_LockedVehicleEdit]    Script Date: 5/30/2025 2:45:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1399/08/14>
-- Description:	<قفل کردن امکان ویرایش اطلاعات خودرو پس از چاپ صورت حساب>
-- =============================================
Create PROCEDURE [dbo].[sp_LockedVehicleEdit] 
	-- Add the parameters for the stored procedure here
	@invoiceCode nvarchar(max),
	@workshopId nvarchar(max),
	@fromdate nvarchar(max),
	@todate nvarchar(max)

AS


BEGIN
	update tbl_VehicleRegistrations
	set Status=level.st,[InvoiceCode]=@invoiceCode
	from
	(select
	ID,1 as st,CreateDate,WorkshopID from tbl_VehicleRegistrations 
	where WorkshopID=@workshopId and CONVERT(VARCHAR(10),CreateDate,120) BETWEEN '' + CONVERT(VARCHAR(10),@fromdate,120) + '' AND '' +  CONVERT(VARCHAR(10),@todate,120)+ '') as level
	where tbl_VehicleRegistrations.ID=level.ID
END
GO
/****** Object:  StoredProcedure [dbo].[sp_PayofOfferedList]    Script Date: 5/30/2025 2:45:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1400/12/06>
-- Description:	<مشاهده لیست دستور پرداخت علی الحساب دستمزد تبدیل به کارگاه ها>
-- =============================================
Create PROCEDURE [dbo].[sp_PayofOfferedList] 
	-- Add the parameters for the stored procedure here
AS


BEGIN

/****** Script for SelectTopNRows command from SSMS  ******/
SELECT CONVERT(date,date) as 'Date'
      ,SUM([Value]) as 'Sumvalue'
      ,SUM([PaiedValue]) as 'PaiedValue'
      ,[Serial]
      ,[Number]
  FROM [CNGFAPCO].[dbo].[tbl_OfferedPrices]
  group by CONVERT(date,date),Serial,Number

END

GO
/****** Object:  StoredProcedure [dbo].[sp_RegistraioninOneView]    Script Date: 5/30/2025 2:45:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1399/07/01>
-- Description:	<مشاهده اطلاعات ثبت خودرو و گواهی های صادر شده بر اساس کارگاه در یک نما>
-- =============================================
CREATE PROCEDURE [dbo].[sp_RegistraioninOneView] 
	-- Add the parameters for the stored procedure here
	
AS

BEGIN

select *,(case when list2.LisenceDiff < 0 then '#ff766d' else '#63f576' end) as 'LisenceDiffColor',(case when list2.HealtDiff < 0 then '#ff766d' else '#63f576' end) as 'HealtDiffColor',
(case when list2.FapDiff < 0 then '#ff766d' else '#63f576' end) as 'FapDiffColor',(case when list2.HLDiff < 0 then '#ff766d' else '#63f576' end) as 'HLDiffColor'
from(
select *,round((case list.RegistrCount when 0 then 0 else CONVERT(float,list.FapCount)/ (case convert(float,list.RegistrCount) when 0 then 1 else convert(float,list.RegistrCount) end) end) * 100,0) as 'FapPercent' ,
		round((case list.FapCount when 0 then 0 else CONVERT(float,list.LisenceCount)/ (case convert(float,list.FapCount) when 0 then 1 else convert(float,list.FapCount) end) end) * 100,0) as 'LisencePercent' ,
		round((case list.FapCount when 0 then 0 else CONVERT(float,list.HealtCount)/ (case convert(float,list.FapCount) when 0 then 1 else convert(float,list.FapCount) end) end) * 100,0) as 'HealtPercent',
		round((case list.LisenceCount when 0 then 0 else CONVERT(float,list.LisenceCount)/ (case convert(float,list.HealtCount) when 0 then 1 else convert(float,list.HealtCount) end)end) * 100,0) as 'HLPercent',
		(list.LisenceCount-list.FapCount) as 'LisenceDiff', (list.HealtCount-list.FapCount) as 'HealtDiff',(list.FapCount-list.RegistrCount) as 'FapDiff',(list.HealtCount-list.LisenceCount) as 'HLDiff'
from(
select ID,REPLACE(Title, N'مرکز خدمات CNG', '') as 'Title',ISNULL([dbo].[GetRegistrationCount](ID),0) as 'RegistrCount',[dbo].[GetVehicleCount](ID) as 'FapCount',
	   ISNULL([dbo].[GetLisenceCount](ID),0) as 'LisenceCount' ,ISNULL([dbo].[GetHealtCareCount](ID),0) as 'HealtCount' from tbl_Workshops
where  isServices>0 ) as list
) as list2
order by list2.RegistrCount desc

END

GO
/****** Object:  StoredProcedure [dbo].[sp_RegulatorList]    Script Date: 5/30/2025 2:45:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1400/01/19>
-- Description:	مشاهده اطلاعات ثبت شده در بانک کیت
-- =============================================
CREATE PROCEDURE [dbo].[sp_RegulatorList] 
	-- Add the parameters for the stored procedure here
	@workshops nvarchar(max)=null,
	@status nvarchar(max)=null,
	@FromDate DATETIME ,
	@ToDate DATETIME
	

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN
SET @PivotTableSQL = N'
SELECT * FROM(
SELECT [ID]
      ,[serialNumber]
      ,(CASE WHEN datalength(constractor)<10 THEN [dbo].[GetRegulatorConstractors]([constractor]) else constractor END) as [constractor]
      ,[model]
      ,[type]
      ,[generation]
      ,[productDate]
      ,(CASE WHEN datalength(workshop)<10 THEN [dbo].[GetWorkshops]([workshop]) else [workshop] END) as [workshop]
      ,[status]
	  ,(case when [status]=N''نیاز به بررسی'' then 2 else 1 end) as [statusCode]
	  ,(case when [status]=N''نیاز به بررسی'' then ''red'' else ''green'' end) as [statusColor]
      ,[CreateDate]
      ,[Creator]
	  ,[RefreshDate]
  FROM [CNGFAPCO].[dbo].[tbl_Kits]
  where workshop in ('+@workshops+') and CONVERT(VARCHAR(10),CreateDate,120) BETWEEN ''' + CONVERT(VARCHAR(10),@FromDate,120) + ''' AND ''' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''' 
  ) AS List
  Where List.[statusCode] in ('+@status+')
  order by List.ID desc
  '

EXECUTE(@PivotTableSQL)

END
--smaple check numeric
--(CASE WHEN constractor LIKE '%[^0-9]%' THEN [dbo].[GetWorkshops]([workshop]) else constractor END) as [workshop]
GO
/****** Object:  StoredProcedure [dbo].[sp_RegulatorList_Bank]    Script Date: 5/30/2025 2:45:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1400/01/19>
-- Description:	مشاهده اطلاعات ثبت شده در بانک کیت
-- =============================================
Create PROCEDURE [dbo].[sp_RegulatorList_Bank] 
	-- Add the parameters for the stored procedure here	
	@FromDate DATETIME ,
	@ToDate DATETIME
	

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN
SET @PivotTableSQL = N'
SELECT [ID]
      ,[serialNumber]
      ,[constractor]
      ,[model]
      ,[type]
      ,[generation]
      ,[productDate]
      ,[workshop]      
      ,[CreateDate]
      ,[Creator]
  FROM [CNGFAPCO].[dbo].[tbl_BankKits]
  where CONVERT(VARCHAR(10),CreateDate,120) BETWEEN ''' + CONVERT(VARCHAR(10),@FromDate,120) + ''' AND ''' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''' 
  order by ID desc
  '

EXECUTE(@PivotTableSQL)

END
--smaple check numeric
--(CASE WHEN constractor LIKE '%[^0-9]%' THEN [dbo].[GetWorkshops]([workshop]) else constractor END) as [workshop]
GO
/****** Object:  StoredProcedure [dbo].[sp_remittance]    Script Date: 5/30/2025 2:45:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1399/05/01>
-- Description:	<مشاهده اطلاعات طرح های تقسیم در حواله انبار>
-- =============================================
Create PROCEDURE [dbo].[sp_remittance] 
	-- Add the parameters for the stored procedure here
	@id nvarchar(max)
AS


BEGIN
------------------------------------------------- Tank Division Plan -------------------------------------------------------------------------------------------------------------------

select * from [dbo].[DivisionTanks](@id)

union all

------------------------------------------------- Valve Division Plan -------------------------------------------------------------------------------------------------------------------
select * from [dbo].[DivisionTankValves](@id)

union all

------------------------------------------------- Kit Division Plan -------------------------------------------------------------------------------------------------------------------

select * from [dbo].[DivisionKits](@id)

union all

------------------------------------------------- Tank Base Division Plan -------------------------------------------------------------------------------------------------------------------

select * from [dbo].[DivisionTankBase](@id)

union all
------------------------------------------------- Tank Cover Division Plan -------------------------------------------------------------------------------------------------------------------

select * from [dbo].[DivisionTankCovers](@id)

------------------------------------------------- Otherthings Division Plan -------------------------------------------------------------------------------------------------------------------


END

GO
/****** Object:  StoredProcedure [dbo].[sp_Remittances]    Script Date: 5/30/2025 2:45:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1401/02/09>
-- Description:	مشاهده حواله های انبار
-- =============================================
Create PROCEDURE [dbo].[sp_Remittances] 
	-- Add the parameters for the stored procedure here
	@Workshops nvarchar(max)=null
	

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN

SET @PivotTableSQL = N'
SELECT tbl_Remittances.[ID]
      ,[Number]
      ,[DivisionPlanID]
      ,tbl_Remittances.[CreateDate]
	  ,(Users.Firstname + '' '' + Users.Lastname) as ''Creator''
      ,[Status]
      ,[StatusDate]
      ,tbl_Remittances.[Description]
      ,[Incomplete]
      ,[IncompleteDesc]
	  ,tbl_DivisionPlans.Code	  
	  ,(tbl_Workshops.Title + '' - '' + tbl_Cities.Title) as ''Workshop''
	  ,[dbo].[GetRemittanceUrl](tbl_DivisionPlans.ID) as''Url''
	  ,[dbo].[GetRemittanceDetailsCount](tbl_Remittances.[ID]) as ''Count''
	  ,[dbo].[GetRemittanceDetailsCost](tbl_Remittances.[ID]) as ''Cost''
  FROM [CNGFAPCO].[dbo].[tbl_Remittances] inner join 
		Users on tbl_Remittances.Creator=Users.UserID inner join 
		tbl_DivisionPlans on tbl_Remittances.DivisionPlanID=tbl_DivisionPlans.ID inner join
		tbl_Workshops on tbl_DivisionPlans.WorkshopID=tbl_Workshops.ID inner join
		tbl_Cities on tbl_Workshops.CityID=tbl_Cities.ID
  where tbl_Workshops.ID in ('+@Workshops+') 
  order by tbl_Remittances.[ID] desc'

EXECUTE(@PivotTableSQL)

END
GO
/****** Object:  StoredProcedure [dbo].[sp_remittancewithBOM]    Script Date: 5/30/2025 2:45:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1399/05/01>
-- Description:	<مشاهده اطلاعات طرح های تقسیم در حواله انبار بر اساس BOM>
-- =============================================
CREATE PROCEDURE [dbo].[sp_remittancewithBOM] 
	-- Add the parameters for the stored procedure here
	@id nvarchar(max)
AS


BEGIN

	select FinancialCode,tbl_EquipmentList.Title,Unit,SUM(NumberofSend) as NumberofSend,ISNULL(tbl_DivisionPlanBOMs.Description,'') as [Description],tbl_EquipmentList.Pid,
			tbl_GenerationofRegulators.Title as Genaration,tbl_RegistrationTypes.Type as RegistrationType
	from tbl_DivisionPlanBOMs 
	inner join tbl_BOMs on tbl_DivisionPlanBOMs.BOMID=tbl_BOMs.ID 
	inner join tbl_EquipmentList on tbl_EquipmentList.ID=tbl_BOMs.EquipmentListID
	inner join tbl_GenerationofRegulators on tbl_DivisionPlanBOMs.GenarationID=tbl_GenerationofRegulators.ID
	inner join tbl_RegistrationTypes on tbl_DivisionPlanBOMs.RegistrationTypeID=tbl_RegistrationTypes.ID
	where DivisionPlanID=@id
	group by FinancialCode,tbl_EquipmentList.Title,Unit,tbl_DivisionPlanBOMs.Description,tbl_EquipmentList.Pid,tbl_GenerationofRegulators.Title,tbl_RegistrationTypes.Type
	order by tbl_EquipmentList.Pid

END

GO
/****** Object:  StoredProcedure [dbo].[sp_RequestFreeSaleList]    Script Date: 5/30/2025 2:45:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1399/05/19>
-- Description:	مشاهده لیست درخواست های کارگاه ها برای خرید اقلام در طرح فروش آزاد
-- =============================================
CREATE PROCEDURE [dbo].[sp_RequestFreeSaleList] 
	-- Add the parameters for the stored procedure here
	

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN

SET @PivotTableSQL = N'select *, ISNULL([dbo].[GetInvoiceStatusPaied](list2.FinalCode,list2.PayerCode),0) PaymentStatus from(
						select *,(list.Total + list.DiscountAmount) TotalAmount, ISNULL([dbo].[GetFinalFreeSaleInvoiceCode](list.PreCode),0) FinalCode from(
						select tbl_wo.ID PayerCode,[dbo].[GetWorkshops](tbl_re.WorkshopsID) Workshops,(tbl_wo.OwnerName +'' '' + tbl_wo.OwnerFamily) Owners,tbl_re.InvoiceCode,CONVERT(date, tbl_re.CreatedDate) CreatedDate,
								SUM(tbl_re.Number) Number,tbl_re.UnitofMeasurement,SUM(tbl_re.TotalAmount) Total,SUM(tbl_re.TotalAmount * 0.09) DiscountAmount,tbl_re.SaleCondition RequestSaleCondition,ISNULL([dbo].[GetSaleCondition](tbl_re.InvoiceCode),''-'') PreSaleCondition,
								[dbo].[GetPreFreeSaleInvoiceCount](tbl_re.InvoiceCode) PreCount,	ISNULL([dbo].[GetPreFreeSaleInvoiceCode](tbl_re.InvoiceCode),0) PreCode,tbl_re.Status , 
								(case when Status=0 then ''#f7818c'' else ''#82f59d'' end) ViewStatusColor
								FROM [tbl_RequestFreeSales] tbl_re
										inner join tbl_Workshops tbl_wo on tbl_re.WorkshopsID=tbl_wo.ID
						GROUP BY tbl_re.WorkshopsID,tbl_wo.OwnerName,tbl_wo.OwnerFamily,tbl_re.InvoiceCode,CONVERT(date, tbl_re.CreatedDate),tbl_re.UnitofMeasurement,tbl_re.SaleCondition,tbl_re.Status,tbl_wo.ID
						) list
						) list2
						ORDER BY list2.CreatedDate DESC
'


EXECUTE(@PivotTableSQL)

END
--select Owners,[dbo].[GetWorkshops](WorkshopsID) as Workshops,InvoiceCode,CONVERT(date, CreatedDate) as CreatedDate,SUM(CONVERT(int, Number)) as Number,UnitofMeasurement,(SUM(CONVERT(float,TotalAmount))*0.09+SUM(CONVERT(float,TotalAmount))) as TotalAmount,SUM(CONVERT(float,DiscountAmount)) as DiscountAmount,
--Status , (case when Status=0 then ''#f7818c'' else ''#82f59d'' end) ViewStatusColor,SaleCondition as RequestSaleCondition,[dbo].[GetSaleCondition](InvoiceCode) as SaleCondition
--FROM [CNGFAPCO].[dbo].[tbl_RequestFreeSales]
--where FinalStatus=1
--GROUP BY Owners,WorkshopsID,InvoiceCode,CONVERT(date, CreatedDate),UnitofMeasurement,Status,SaleCondition
--ORDER BY CONVERT(date, CreatedDate) desc
GO
/****** Object:  StoredProcedure [dbo].[sp_SearchInDiffInfo]    Script Date: 5/30/2025 2:45:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal>
-- Create date: <1400/01/11>
-- Description:	<جهت مشاهده لیست مغایرت اطلاعات ثبت شده در سایت ها سه گانه>
-- =============================================
CREATE PROCEDURE [dbo].[sp_SearchInDiffInfo] 
	-- Add the parameters for the stored procedure here
	@condition nvarchar(max),
	@column nvarchar(max),
	@Table1 nvarchar(max),
	@Table2 nvarchar(max)

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)
declare @tbl1 nvarchar(max)=null;
declare @tbl2 nvarchar(max)=null;
declare @col nvarchar(max)=null;
declare @col2 nvarchar(max)=null;
set @tbl1=REPLACE(@Table1,'vw_','');
set @tbl2=REPLACE(@Table2,'vw_','');
set @col=REPLACE(@column,' ','');

BEGIN
--if(@condition='Like')
--	   SET @PivotTableSQL = 'select tbl2.NationalCode '+@tbl2+'_NationalCode,REPLACE(tbl1.NationalCode,''-'','''') FAPCO_NationalCode,[dbo].[GetVehicleType](tbl1.VehicleTypeID) FAPCO_VehicleType,tbl2.VehicleType '+@tbl2+'_VehicleType,tbl1.ChassisNumber FAPCO_ChassisNumber,tbl2.ChassisNumber '+@tbl2+'_ChassisNumber,tbl1.EngineNumber FAPCO_EngineNumber,tbl2.EngineNumber '+@tbl2+'_EngineNumber,
--							tbl1.*,tbl3.Type as VehicleType,[dbo].[GetWorkshops](tbl1.WorkshopID) Workshops from '+@Table1+' tbl1
--								inner join '+@Table2+' tbl2 on REPLACE(tbl1.NationalCode,''-'','''')=tbl2.NationalCode left outer join tbl_VehicleTypes tbl3 on tbl1.VehicleTypeID=tbl3.ID
--								where tbl1.'+@col+' Like N''%tbl2.'+@col+'%''
--						 '
--else
	   --SET @PivotTableSQL = 'select tbl1.ConstructionYear '+@tbl1+'_ConstructionYear,tbl1.HealthCertificate '+@tbl1+'_HealthCertificate,tbl1.Plate2 '+@tbl1+'_Plate2,tbl1.Workshop '+@tbl1+'_Workshop,tbl1.NationalCode '+@tbl1+'_NationalCode,tbl1.VehicleType '+@tbl1+'_VehicleType,tbl1.ChassisNumber '+@tbl1+'_ChassisNumber,tbl1.EngineNumber '+@tbl1+'_EngineNumber								
				--				from vw_NIOPDC tbl1
				--				where  tbl1.HealthCertificate is null 
				--		 '

if(@condition='0' or @column='0')
SET @PivotTableSQL ='select tbl1.ConstructionYear '+@tbl1+'_ConstructionYear,tbl1.HealthCertificate '+@tbl1+'_HealthCertificate,tbl1.Plate2 '+@tbl1+'_Plate2,tbl1.Workshop '+@tbl1+'_Workshop,tbl1.NationalCode '+@tbl1+'_NationalCode,tbl1.VehicleType '+@tbl1+'_VehicleType,tbl1.ChassisNumber '+@tbl1+'_ChassisNumber,tbl1.EngineNumber '+@tbl1+'_EngineNumber,
						tbl2.ConstructionYear '+@tbl2+'_ConstructionYear,tbl2.HealthCertificate '+@tbl2+'_HealthCertificate,tbl2.Plate2 '+@tbl2+'_Plate2,tbl2.Workshop '+@tbl2+'_Workshop,tbl2.NationalCode '+@tbl2+'_NationalCode,tbl2.VehicleType '+@tbl2+'_VehicleType,tbl2.ChassisNumber '+@tbl2+'_ChassisNumber,tbl2.EngineNumber '+@tbl2+'_EngineNumber
						from '+@Table1+' tbl1 inner join '+@Table2+' tbl2 on tbl1.NationalCode=tbl2.NationalCode'
else
SET @PivotTableSQL = 'select tbl1.ConstructionYear '+@tbl1+'_ConstructionYear,tbl1.HealthCertificate '+@tbl1+'_HealthCertificate,tbl1.Plate2 '+@tbl1+'_Plate2,tbl1.Workshop '+@tbl1+'_Workshop,tbl1.NationalCode '+@tbl1+'_NationalCode,tbl1.VehicleType '+@tbl1+'_VehicleType,tbl1.ChassisNumber '+@tbl1+'_ChassisNumber,tbl1.EngineNumber '+@tbl1+'_EngineNumber,
						tbl2.ConstructionYear '+@tbl2+'_ConstructionYear,tbl2.HealthCertificate '+@tbl2+'_HealthCertificate,tbl2.Plate2 '+@tbl2+'_Plate2,tbl2.Workshop '+@tbl2+'_Workshop,tbl2.NationalCode '+@tbl2+'_NationalCode,tbl2.VehicleType '+@tbl2+'_VehicleType,tbl2.ChassisNumber '+@tbl2+'_ChassisNumber,tbl2.EngineNumber '+@tbl2+'_EngineNumber
						from '+@Table1+' tbl1 inner join '+@Table2+' tbl2 on tbl1.NationalCode=tbl2.NationalCode
						where tbl1.'+@col+''+@condition+ 'tbl2.'+@col+''
--(tbl1.HealthCertificate is null) and

	--SET @PivotTableSQL ='SELECT DISTINCT tbl1.ConstructionYear IRNGV_ConstructionYear,tbl1.HealthCertificate IRNGV_HealthCertificate,tbl1.Plate2 IRNGV_Plate2,tbl1.Workshop IRNGV_Workshop,tbl1.NationalCode IRNGV_NationalCode,tbl1.VehicleType IRNGV_VehicleType,tbl1.ChassisNumber IRNGV_ChassisNumber,tbl1.EngineNumber IRNGV_EngineNumber
 --          FROM '+@Table1+' tbl1
	--		WHERE COALESCE(tbl1.'+@col+', ''1'') NOT IN
	--		(SELECT COALESCE(tbl2.'+@col+', ''1'')
	--		FROM '+@Table2+' tbl2
	--		 WHERE COALESCE(tbl1.'+@col+', ''1'') = COALESCE(tbl2.'+@col+', ''1'')
	--		 );'
	   EXECUTE(@PivotTableSQL)
END

-----------------------------main code------------------------------------------------------------
 --SET @PivotTableSQL = 'select tbl1.ConstructionYear '+@tbl1+'_ConstructionYear,tbl1.HealthCertificate '+@tbl1+'_HealthCertificate,tbl1.Plate2 '+@tbl1+'_Plate2,tbl1.Workshop '+@tbl1+'_Workshop,tbl1.NationalCode '+@tbl1+'_NationalCode,tbl1.VehicleType '+@tbl1+'_VehicleType,tbl1.ChassisNumber '+@tbl1+'_ChassisNumber,tbl1.EngineNumber '+@tbl1+'_EngineNumber,
	--							tbl2.ConstructionYear '+@tbl2+'_ConstructionYear,tbl2.HealthCertificate '+@tbl2+'_HealthCertificate,tbl2.Plate2 '+@tbl2+'_Plate2,tbl2.Workshop '+@tbl2+'_Workshop,tbl2.NationalCode '+@tbl2+'_NationalCode,tbl2.VehicleType '+@tbl2+'_VehicleType,tbl2.ChassisNumber '+@tbl2+'_ChassisNumber,tbl2.EngineNumber '+@tbl2+'_EngineNumber
	--							from '+@Table1+' tbl1
	--							inner join '+@Table2+' tbl2 on tbl1.NationalCode = tbl2.NationalCode
	--							where tbl1.'+@col+''+@condition+ 'tbl2.'+@col+'
GO
/****** Object:  StoredProcedure [dbo].[sp_ShowBOMDivisionResults]    Script Date: 5/30/2025 2:45:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1399/09/16>
-- Description:	<مشاهده طرح تقسیم های صادر شده توسط کاربر در زمان صدور طرح بر اساس BOM>
-- =============================================
CREATE PROCEDURE [dbo].[sp_ShowBOMDivisionResults] 
	-- Add the parameters for the stored procedure here
	@id nvarchar(max),
	@GenerationId nvarchar(max)=null
AS

BEGIN

	select (t.Code) Code,(t.ID) DivisionPlanID,t.VehicleTypeID,(t.VehicleType) VehicleType,(t.VehicleTypeID) VehicleTypeID,(t.WorkshopTitle) WorkshopTitle,
	(CONVERT(date, t.CreateDate)) CreateDate,(t.Creator) Creator,(t.Number) Number, t.GenerationID,t.Generation
	from(select  tbl_DivisionPlans.Code,tbl_DivisionPlans.ID,tbl_BOMs.VehicleTypeID, [dbo].[GetVehicleType](tbl_BOMs.VehicleTypeID) VehicleType,
	SUM(CONVERT(float,Number) * CONVERT(float, (case when Ratio<>1 then 1 else 1 end) )) Number, tbl_Workshops.Title WorkshopTitle,tbl_DivisionPlanBOMs.CreateDate,
	tbl_DivisionPlanBOMs.Creator,tbl_GenerationofRegulators.ID as GenerationID,tbl_GenerationofRegulators.Title as Generation
	from tbl_DivisionPlanBOMs 
	inner join tbl_BOMs on tbl_DivisionPlanBOMs.BOMID=tbl_BOMs.ID
	inner join tbl_GenerationofRegulators on tbl_BOMs.GenerationID=tbl_GenerationofRegulators.ID
	inner join tbl_EquipmentList on tbl_BOMs.EquipmentListID=tbl_EquipmentList.ID
	inner join tbl_DivisionPlans on tbl_DivisionPlanBOMs.DivisionPlanID=tbl_DivisionPlans.ID
	inner join tbl_Workshops on tbl_DivisionPlans.WorkshopID=tbl_Workshops.ID
	where tbl_BOMs.Presentable=1 and tbl_DivisionPlanBOMs.DivisionPlanID=@id and (EquipmentListID='22' or EquipmentListID='23') --or GenerationID='5'
	group by tbl_BOMs.VehicleTypeID,tbl_Workshops.Title,tbl_DivisionPlanBOMs.CreateDate,tbl_DivisionPlanBOMs.Creator,tbl_BOMs.EquipmentListID,tbl_DivisionPlans.Code,
	tbl_DivisionPlans.ID,tbl_GenerationofRegulators.ID,tbl_GenerationofRegulators.Title
	) t
	group by t.VehicleTypeID,t.Code,t.ID,t.VehicleType,t.WorkshopTitle,t.CreateDate,t.Creator,t.Number,t.GenerationID,t.Generation
	order by t.GenerationID
END

----------------------------changed in date : 1403/10/28--------------------------old sp query---------------------------------------------
--if (@GenerationId in ('5'))
----------------------------------------------طرح تعویض خودروی فرسوده-------------------------------------------------------------
--	select MAX(t.Code) Code,MAX(t.ID) DivisionPlanID,t.VehicleTypeID,MAX(t.VehicleType) VehicleType,MAX(t.VehicleTypeID) VehicleTypeID,MAX(t.WorkshopTitle) WorkshopTitle,MAX(CONVERT(date, t.CreateDate)) CreateDate,MAX(t.Creator) Creator,MIN(t.Number) Number from(
--	select  tbl_DivisionPlans.Code,tbl_DivisionPlans.ID,tbl_BOMs.VehicleTypeID, [dbo].[GetVehicleType](tbl_BOMs.VehicleTypeID) VehicleType,SUM(CONVERT(float,Number) * CONVERT(float, (case when Ratio<>1 then 1 else 1 end) )) Number, tbl_Workshops.Title WorkshopTitle,tbl_DivisionPlanBOMs.CreateDate,tbl_DivisionPlanBOMs.Creator
--	from tbl_DivisionPlanBOMs 
--	inner join tbl_BOMs on tbl_DivisionPlanBOMs.BOMID=tbl_BOMs.ID
--	inner join tbl_DivisionPlans on tbl_DivisionPlanBOMs.DivisionPlanID=tbl_DivisionPlans.ID
--	inner join tbl_Workshops on tbl_DivisionPlans.WorkshopID=tbl_Workshops.ID
--	where tbl_BOMs.Presentable=1 and tbl_DivisionPlanBOMs.DivisionPlanID=@id
--	group by tbl_BOMs.VehicleTypeID,tbl_Workshops.Title,tbl_DivisionPlanBOMs.CreateDate,tbl_DivisionPlanBOMs.Creator,tbl_BOMs.EquipmentListID,tbl_DivisionPlans.Code,tbl_DivisionPlans.ID
--	) t
--	group by t.VehicleTypeID

--else
--	select MAX(t.Code) Code,MAX(t.ID) DivisionPlanID,t.VehicleTypeID,MAX(t.VehicleType) VehicleType,MAX(t.VehicleTypeID) VehicleTypeID,MAX(t.WorkshopTitle) WorkshopTitle,MAX(CONVERT(date, t.CreateDate)) CreateDate,MAX(t.Creator) Creator,MIN(t.Number) Number from(
--	select  tbl_DivisionPlans.Code,tbl_DivisionPlans.ID,tbl_BOMs.VehicleTypeID, [dbo].[GetVehicleType](tbl_BOMs.VehicleTypeID) VehicleType,SUM(CONVERT(float,Number) * CONVERT(float, (case when Ratio<>1 then 1 else 1 end) )) Number, tbl_Workshops.Title WorkshopTitle,tbl_DivisionPlanBOMs.CreateDate,tbl_DivisionPlanBOMs.Creator
--	from tbl_DivisionPlanBOMs 
--	inner join tbl_BOMs on tbl_DivisionPlanBOMs.BOMID=tbl_BOMs.ID
--	inner join tbl_DivisionPlans on tbl_DivisionPlanBOMs.DivisionPlanID=tbl_DivisionPlans.ID
--	inner join tbl_Workshops on tbl_DivisionPlans.WorkshopID=tbl_Workshops.ID
--	where tbl_BOMs.Presentable=1 and tbl_DivisionPlanBOMs.DivisionPlanID=@id and EquipmentListID='3' --or GenerationID='5'
--	group by tbl_BOMs.VehicleTypeID,tbl_Workshops.Title,tbl_DivisionPlanBOMs.CreateDate,tbl_DivisionPlanBOMs.Creator,tbl_BOMs.EquipmentListID,tbl_DivisionPlans.Code,tbl_DivisionPlans.ID
--	) t
--	group by t.VehicleTypeID
GO
/****** Object:  StoredProcedure [dbo].[sp_ShowBOMDivisionResults_TKhF]    Script Date: 5/30/2025 2:45:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1402/10/08>
-- Description:	<مشاهده طرح تقسیم های صادر شده توسط کاربر در زمان صدور طرح بر اساس BOM>
-- =============================================
CREATE PROCEDURE [dbo].[sp_ShowBOMDivisionResults_TKhF] 
	-- Add the parameters for the stored procedure here
	@id nvarchar(max),
	@GenerationId nvarchar(max)=null
AS


BEGIN

--------------------------------------------طرح تعویض خودروی فرسوده-------------------------------------------------------------
	select MAX(t.Code) Code,MAX(t.ID) DivisionPlanID,t.VehicleTypeID,MAX(t.VehicleType) VehicleType,MAX(t.VehicleTypeID) VehicleTypeID,MAX(t.WorkshopTitle) WorkshopTitle,MAX(CONVERT(date, t.CreateDate)) CreateDate,MAX(t.Creator) Creator,MIN(t.Number) Number from(
	select  tbl_DivisionPlans.Code,tbl_DivisionPlans.ID,tbl_BOMs.VehicleTypeID, [dbo].[GetVehicleType](tbl_BOMs.VehicleTypeID) VehicleType,SUM(CONVERT(float,Number) * CONVERT(float, (case when Ratio<>1 then 1 else 1 end) )) Number, tbl_Workshops.Title WorkshopTitle,tbl_DivisionPlanBOMs.CreateDate,tbl_DivisionPlanBOMs.Creator
	from tbl_DivisionPlanBOMs 
	inner join tbl_BOMs on tbl_DivisionPlanBOMs.BOMID=tbl_BOMs.ID
	inner join tbl_DivisionPlans on tbl_DivisionPlanBOMs.DivisionPlanID=tbl_DivisionPlans.ID
	inner join tbl_Workshops on tbl_DivisionPlans.WorkshopID=tbl_Workshops.ID
	where tbl_BOMs.Presentable=1 and tbl_DivisionPlanBOMs.DivisionPlanID=@id and tbl_BOMs.GenerationID=5
	group by tbl_BOMs.VehicleTypeID,tbl_Workshops.Title,tbl_DivisionPlanBOMs.CreateDate,tbl_DivisionPlanBOMs.Creator,tbl_BOMs.EquipmentListID,tbl_DivisionPlans.Code,tbl_DivisionPlans.ID
	) t
	group by t.VehicleTypeID
END
GO
/****** Object:  StoredProcedure [dbo].[sp_SMStoVehicleRegistrations]    Script Date: 5/30/2025 2:45:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1399/09/24>
-- Description:	مشاهده و فیلتر اطلاعات خودروهای ثبت شده برای ارسال پیام
-- =============================================
CREATE PROCEDURE [dbo].[sp_SMStoVehicleRegistrations] 
	-- Add the parameters for the stored procedure here
	@workshops nvarchar(max)=null,
	@VehicleTypeID nvarchar(max)=null,
	@FromDate DATETIME ,
	@ToDate DATETIME	

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN

SET @PivotTableSQL = '
select Top(2000) *,[dbo].[GetWorkshops](WorkshopID) as WorkshopTitle,[dbo].[GetInsuranceNumber](ID) as InsuranceNumber,[dbo].[GetTypeofUse](TypeofUseID) as TypeofUse ,[dbo].[GetVehicleType](VehicleTypeID) as VehicleType
from tbl_VehicleRegistrations
where WorkshopID in ('+@workshops+') and VehicleTypeID in ('+@VehicleTypeID+')   and CONVERT(VARCHAR(10),CreateDate,120) BETWEEN ''' + CONVERT(VARCHAR(10),@FromDate,120) + ''' AND ''' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''' 
order by ID desc
'

EXECUTE(@PivotTableSQL)

END
GO
/****** Object:  StoredProcedure [dbo].[sp_TankValveList]    Script Date: 5/30/2025 2:45:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1399/04/30>
-- Description:	مشاهده اطلاعات ثبت شده در بانک شیر مخزن
-- =============================================
CREATE PROCEDURE [dbo].[sp_TankValveList] 
	-- Add the parameters for the stored procedure here
	@workshops nvarchar(max)=null,
	@status nvarchar(max)=null,
	@FromDate DATETIME ,
	@ToDate DATETIME
	

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN
SET @PivotTableSQL = N'
SELECT * FROM(
SELECT tbl_TankValves.[ID]
      ,[serialNumber]
      ,(CASE WHEN datalength(constractor)<10 THEN [dbo].[GetValveConstractors]([constractor]) else constractor END) as [constractor]
      ,[model]
      ,[type]
      ,[rezve]
      ,[productDate]
      ,(CASE WHEN datalength(workshop)<10 THEN [dbo].[GetWorkshops]([workshop]) else [workshop] END) as [workshop]
      ,[status]
	  ,(case when [status]=N''نیاز به بررسی'' then 2 else 1 end) as [statusCode]
	  ,(case when [status]=N''نیاز به بررسی'' then ''red'' else ''green'' end) as [statusColor]
      ,[CreateDate]
      ,[Creator]
	  ,[RefreshDate]
  FROM [CNGFAPCO].[dbo].[tbl_TankValves]
  where workshop in ('+@workshops+') and CONVERT(VARCHAR(10),CreateDate,120) BETWEEN ''' + CONVERT(VARCHAR(10),@FromDate,120) + ''' AND ''' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''' 
   ) AS List
  Where List.[statusCode] in ('+@status+')
  order by List.ID desc
  '

EXECUTE(@PivotTableSQL)

END
--smaple check numeric
--(CASE WHEN constractor LIKE '%[^0-9]%' THEN [dbo].[GetWorkshops]([workshop]) else constractor END) as [workshop]
GO
/****** Object:  StoredProcedure [dbo].[sp_TankValveList_Bank]    Script Date: 5/30/2025 2:45:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1399/04/30>
-- Description:	مشاهده اطلاعات ثبت شده در بانک شیر مخزن
-- =============================================
Create PROCEDURE [dbo].[sp_TankValveList_Bank] 
	-- Add the parameters for the stored procedure here
	@FromDate DATETIME ,
	@ToDate DATETIME
	

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN
SET @PivotTableSQL = N'
SELECT [ID]
      ,[serialNumber]
      ,[constractor]
      ,[model]
      ,[type]
      ,[rezve]
      ,[productDate]
      ,[workshop]
      ,[CreateDate]
      ,[Creator]
  FROM [CNGFAPCO].[dbo].[tbl_BankTankValves]
  where CONVERT(VARCHAR(10),CreateDate,120) BETWEEN ''' + CONVERT(VARCHAR(10),@FromDate,120) + ''' AND ''' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''' 
  order by ID desc
  '

EXECUTE(@PivotTableSQL)

END
--smaple check numeric
--(CASE WHEN constractor LIKE '%[^0-9]%' THEN [dbo].[GetWorkshops]([workshop]) else constractor END) as [workshop]
GO
/****** Object:  StoredProcedure [dbo].[sp_tbl_CutoffValveList]    Script Date: 5/30/2025 2:45:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1400/02/03>
-- Description:	مشاهده اطلاعات ثبت شده در بانک شیر ‍قطع کن
-- =============================================
CREATE PROCEDURE [dbo].[sp_tbl_CutoffValveList] 
	-- Add the parameters for the stored procedure here
	@workshops nvarchar(max)=null,
	@status nvarchar(max)=null,
	@FromDate DATETIME ,
	@ToDate DATETIME
	

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN
SET @PivotTableSQL = N'
SELECT * FROM(
SELECT [ID]
      ,[serialNumber]
      ,(CASE WHEN datalength(constractor)<10 THEN [dbo].[GetCutofValveConstractors]([constractor]) else constractor END) as [constractor]
      ,(CASE WHEN datalength(model)<10 THEN [dbo].[GetCutoffValveModels]([constractor]) else model END) as [model]      
      ,[productDate]
      ,(CASE WHEN datalength(workshop)<10 THEN [dbo].[GetWorkshops]([workshop]) else [workshop] END) as [workshop]
      ,[status]
	  ,(case when [status]=N''نیاز به بررسی'' then 2 else 1 end) as [statusCode]
	  ,(case when [status]=N''نیاز به بررسی'' then ''red'' else ''green'' end) as [statusColor]
      ,[CreateDate]
      ,[Creator]
	  ,[RefreshDate]
  FROM [CNGFAPCO].[dbo].[tbl_CutofValves]
  where workshop in ('+@workshops+') and CONVERT(VARCHAR(10),CreateDate,120) BETWEEN ''' + CONVERT(VARCHAR(10),@FromDate,120) + ''' AND ''' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''' 
  ) AS List
  Where List.[statusCode] in ('+@status+')
  order by List.ID desc
  '

EXECUTE(@PivotTableSQL)

END
--smaple check numeric
--(CASE WHEN constractor LIKE '%[^0-9]%' THEN [dbo].[GetWorkshops]([workshop]) else constractor END) as [workshop]
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdatePayment]    Script Date: 5/30/2025 2:45:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1399/09/27>
-- Description:	ثبت اطلاعات پرداخت های آنلاین درگاه بانک ملت
-- =============================================
create PROCEDURE [dbo].[sp_UpdatePayment] 
	-- Add the parameters for the stored procedure here
	@Status nvarchar(max),
	@RefID nvarchar(max),
	@SaleReferenceId nvarchar(max),
	@PaymentId int

AS

BEGIN

update tbl_Payments set [Status]=@Status,RefID=@RefID,SaleReferenceId=@SaleReferenceId where ID=@PaymentId
	
END
GO
/****** Object:  StoredProcedure [dbo].[sp_VehicleBOMinOneView]    Script Date: 5/30/2025 2:45:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1399/09/05>
-- Description:	<مشاهده اطلاعات BOM خودروها در یک نگاه>
-- =============================================
CREATE PROCEDURE [dbo].[sp_VehicleBOMinOneView] 
	-- Add the parameters for the stored procedure here
	@generationId nvarchar(max),
	@vehicleTypeId nvarchar(max)
AS


BEGIN

DECLARE @PivotColumnHeaders NVARCHAR(MAX)

  SELECT @PivotColumnHeaders= ISNULL(@PivotColumnHeaders + ',','') 
       + QUOTENAME(Type)
	FROM (SELECT (Type+''+Description) as Type FROM tbl_VehicleTypes) AS Typee

DECLARE @PivotTableSQL NVARCHAR(MAX)

SET @PivotTableSQL = 'select * from (
 select	EquipmentListID '+ N'کد'+',tbl_EquipmentList.FinancialCode '+ N'کد_کالا'+',tbl_EquipmentList.Title '+ N'نام_کالا'+',Unit '+ N'واحد_سنجش'+', ISNULL(SUM(Ratio),0) as Value, (Type+''''+Description) as Subject
		FROM  tbl_BOMs 
		inner join tbl_VehicleTypes on tbl_BOMs.VehicleTypeID=tbl_VehicleTypes.ID
		inner join tbl_EquipmentList on tbl_BOMs.EquipmentListID =tbl_EquipmentList.ID
		where tbl_BOMs.Presentable=1 and (tbl_BOMs.GenerationID in ('+ @generationId +') or ' + @generationId + ' =0) and (tbl_BOMs.VehicleTypeID in ('+ @vehicleTypeId +') or ' + @vehicleTypeId + ' =0)
		group by Type,EquipmentListID,Unit,tbl_EquipmentList.Title,tbl_EquipmentList.FinancialCode,tbl_VehicleTypes.Description) as se
  PIVOT (
    SUM(Value)
    FOR se.Subject IN (' + @PivotColumnHeaders +'
    )
  ) AS PivotTable
'

EXECUTE(@PivotTableSQL)

END

GO
/****** Object:  StoredProcedure [dbo].[sp_VehicleConversionCertificate]    Script Date: 5/30/2025 2:45:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1400/09/05>
-- Description:	صدور گواهی تبدیل فن آوران پارسیان
-- ===========================================
CREATE PROCEDURE [dbo].[sp_VehicleConversionCertificate] 
	-- Add the parameters for the stored procedure here
	@selectedId nvarchar(max)=null	

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN

SELECT        tbl1.ID, tbl1.VehicleTypeID, tbl1.System, tbl1.OwnerName, tbl1.OwnerFamily, tbl1.NationalCard, tbl1.PhoneNumber, tbl1.MobileNumber, tbl1.ConstructionYear, tbl1.LeftNumberPlate, tbl1.AlphaPlate, tbl1.RightNumberPlate, 
                         tbl1.IranNumberPlate, tbl1.EngineNumber, tbl1.ChassisNumber, tbl1.VehicleCard, tbl1.Creator, tbl1.CreateDate, tbl1.SerialSparkPreview, tbl1.SerialKit, tbl1.SerialKey, tbl1.SerialRefuelingValve, tbl1.RefuelingLable, 
                         tbl1.TrackingCode, tbl1.License, tbl1.LicenseImage, tbl1.InstallationStatus, tbl1.Description, tbl1.VIN, tbl1.TypeofUseID, tbl1.NationalCode, tbl1.Address, tbl1.FuelCard, tbl1.WorkshopID, tbl1.Editor, tbl1.EditDate, 
                         tbl1.HealthCertificate, tbl1.CreatorIPAddress, tbl1.EditorIPAddress, tbl1.Status, tbl1.InvoiceCode, tbl1.Checked, tbl1.CheckedDate, tbl1.Checker, tbl1.TechnicalDiagnosis, tbl1.RegisterStatus, tbl1.RegisterStatusDate, 
                         tbl1.RegisterStatusUser, tbl2.ID AS Expr1, tbl2.Title as Workshop, tbl2.CityID, tbl2.PhoneNumber AS WPhoneNumber, tbl2.MobileNumber AS WMobileNumber, tbl2.FaxNumber, tbl2.Email, tbl2.Address AS WAddress, tbl2.Creator AS Expr5, 
                         tbl2.CreateDate AS Expr6, tbl2.BusinessLicense, tbl2.OwnerName AS WOwnerName, tbl2.OwnerFamily AS WOwnerFamily, tbl2.FapCode, tbl2.isServices, tbl2.Economicalnumber, tbl2.Registrationnumber, tbl2.Postalcode, 
                         tbl2.Logo, tbl2.CompaniesID, tbl2.Auditor, tbl3.ID AS Expr9, tbl3.Type AS VehicleType, tbl3.Description AS VehicleTypeDesc, tbl3.Image, tbl4.ID AS Expr11, tbl4.Type AS TypeofUse, tbl4.Description AS TypeofUseDesc, 
                         tbl5.ID AS Expr14, tbl5.VehicleRegistrationID, tbl5.serial as CylinderSerialNumber, tbl5.Volume, tbl5.TankConstractorID, tbl5.ProductDate, tbl5.ExpirationDate, tbl5.SerialTankValve as ValveSerialNumber, tbl5.TypeTankValve, tbl5.Creator AS Expr15, tbl5.CreateDate AS Expr16, 
                         tbl5.ValveConstractorID, tbl5.RegulatorSerial as RegulatorSerialNumber, tbl5.RegulatorConstractorID, tbl5.CutofValveSerial as CutoffValveSerialNumber, tbl5.CutofValveConstractorID, tbl5.FillingValveSerial as FillingValveSerialNumber, tbl5.FillingValveConstractorID, tbl6.ID AS Expr17, tbl6.Constractor as CylinderConstractor, 
                         tbl6.Description AS Expr18, tbl6.Code, tbl7.ID AS Expr19, tbl7.Valve as ValveConstractor, tbl7.Description AS Expr20, tbl7.Code AS Expr21, tbl8.ID AS Expr22, tbl8.Regulator as RegulatorConstractor, tbl8.Description AS Expr23, tbl8.Code AS Expr24, tbl9.ID AS Expr25, 
                         tbl9.CutofValve as CutoffValveConstractor, tbl9.Code AS Expr26, tbl9.Description AS Expr27, tbl10.ID AS Expr28, tbl10.FillingValve as FillingValveConstractor, tbl10.Code AS Expr29, tbl10.Description AS Expr30, tbl11.ID AS Expr31, tbl11.Type AS CylinderBulk, 
                         tbl11.Description AS Expr33, tbl11.VehicleTypeId AS Expr34, tbl12.diameter,tbl12.expireDate as CylinderExpireDate,tbl12.model,tbl12.pressure as CylinderPressure,tbl12.rezve,tbl12.type as CylinderType,tbl13.generation AS RegulatorGeneration, tbl13.model AS RegulatorModel, tbl13.type AS RegulatorType,
						 tbl14.model AS ValveModel, tbl14.type AS ValveType, tbl14.rezve AS ValveRezve, tbl15.model as FillingValveModel, tbl16.model as CutoffValveModel,tbl1.RegisterUniqueCode,
						 tbl17.Type AS RegistrationType
FROM            dbo.tbl_VehicleRegistrations AS tbl1 INNER JOIN
                         dbo.tbl_Workshops AS tbl2 ON tbl1.WorkshopID = tbl2.ID INNER JOIN
                         dbo.tbl_VehicleTypes AS tbl3 ON tbl1.VehicleTypeID = tbl3.ID INNER JOIN
                         dbo.tbl_TypeofUses AS tbl4 ON tbl1.TypeofUseID = tbl4.ID LEFT OUTER JOIN
                         dbo.tbl_VehicleTanks AS tbl5 ON tbl1.ID = tbl5.VehicleRegistrationID LEFT OUTER JOIN
                         dbo.tbl_TankConstractors AS tbl6 ON tbl5.TankConstractorID = tbl6.ID LEFT OUTER JOIN
                         dbo.tbl_ValveConstractors AS tbl7 ON tbl5.ValveConstractorID = tbl7.ID LEFT OUTER JOIN
                         dbo.tbl_RegulatorConstractors AS tbl8 ON tbl5.RegulatorConstractorID = tbl8.ID LEFT OUTER JOIN
                         dbo.tbl_CutofValveConstractors AS tbl9 ON tbl5.CutofValveConstractorID = tbl9.ID LEFT OUTER JOIN
                         dbo.tbl_FillingValveConstractors AS tbl10 ON tbl5.FillingValveConstractorID = tbl10.ID LEFT OUTER JOIN
                         dbo.tbl_TypeofTanks AS tbl11 ON tbl5.Volume = tbl11.ID LEFT OUTER JOIN
						 dbo.tbl_BankTanks AS tbl12 ON tbl5.Serial = tbl12.serialNumber LEFT OUTER JOIN						 
						 dbo.tbl_BankKits as tbl13 ON tbl5.RegulatorSerial=tbl13.serialNumber LEFT OUTER JOIN
						 dbo.tbl_BankTankValves AS tbl14 ON tbl5.SerialTankValve = tbl14.serialNumber LEFT OUTER JOIN
						 dbo.tbl_BankFillingValves AS tbl15 ON tbl5.FillingValveSerial = tbl15.serialNumber LEFT OUTER JOIN
						 dbo.tbl_BankCutofValves AS tbl16 ON tbl5.CutofValveSerial = tbl16.serialNumber LEFT OUTER JOIN
						 dbo.tbl_RegistrationTypes AS tbl17 ON tbl1.RegistrationTypeID = tbl17.ID
WHERE        (tbl1.ID = @selectedId)

END
GO
/****** Object:  StoredProcedure [dbo].[sp_VehicleRegistrationCount_Chart]    Script Date: 5/30/2025 2:45:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1399/04/18>
-- Description:	مشاهده نمودار وضعیت ثبت خودروها به صورت روزانه و کلی
-- =============================================
CREATE PROCEDURE [dbo].[sp_VehicleRegistrationCount_Chart] 
	-- Add the parameters for the stored procedure here
	@Date DATETIME,
	@RegistrationTypeID nvarchar(1)=null

AS

BEGIN

--declare @Date DATETIME
--set @RegistrationTypeID='1';

select x.Count,ISNULL(y.Count,0) as DayCount,x.Title from (

(select list.Count,REPLACE(list.Title, N'مرکز خدمات', '') as Title,list.ID from (
select COUNT(VehicleTypeID) as 'Count',REPLACE(tbl_Workshops.Title, 'CNG', '') as Title,tbl_Workshops.ID from tbl_VehicleRegistrations inner join tbl_Workshops on tbl_VehicleRegistrations.WorkshopID= tbl_Workshops.ID
where RegisterStatus=1 and RegistrationTypeID in (@RegistrationTypeID)
group by tbl_Workshops.Title,tbl_Workshops.ID) as list) as x left outer join

(select list2.Count,REPLACE(list2.Title, N'مرکز خدمات', '') as Title,list2.ID from (
select COUNT(VehicleTypeID) as 'Count',REPLACE(tbl_Workshops.Title, 'CNG', '') as Title, tbl_Workshops.ID from tbl_VehicleRegistrations inner join tbl_Workshops on tbl_VehicleRegistrations.WorkshopID= tbl_Workshops.ID
where RegisterStatus=1 and RegistrationTypeID in (@RegistrationTypeID) and CONVERT(VARCHAR(10),tbl_VehicleRegistrations.CreateDate,120) = '' + CONVERT(VARCHAR(10),@Date,120) + ''
group by tbl_Workshops.Title,tbl_Workshops.ID) as list2) as y on ISNULL(x.ID, 'none') = ISNULL(y.ID, 'none'))
order by x.Count desc

------------------------ Radar Charts data fro Registration ad Tabdil -----------------------------------------------------
select SUM(level1.count) as [count],level1.Type from (
SELECT case tbl_VehicleTypes.ID when 3 then N'وانت پیکان/ مزدا' when 11 then N'وانت پیکان/ مزدا' when 13 then N'وانت پیکان/ مزدا' when 15 then N'وانت پیکان/ مزدا' when 16 then N'وانت پیکان/ مزدا' when 4 then N'RD /پراید/ پیکان' when 8 then N'RD /پراید/ پیکان' when 10 then N'RD /پراید/ پیکان' when 1 then N'سمند/ پژو انژکتوری' when 6 then N'سمند/ پژو انژکتوری' else tbl_VehicleTypes.Type end as 'Type', COUNT(vehicleTypeID) as [count]
  FROM tbl_VehicleRegistrations inner join tbl_VehicleTypes on tbl_VehicleRegistrations.VehicleTypeID=tbl_VehicleTypes.ID
  where RegisterStatus=1 and RegistrationTypeID in (@RegistrationTypeID)
  group by tbl_VehicleTypes.Type,tbl_VehicleTypes.ID) level1
  group by level1.Type

END
GO
/****** Object:  StoredProcedure [dbo].[sp_VehicleRegistrationinContradictions]    Script Date: 5/30/2025 2:45:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1399/04/16>
-- Description:	مشاهده اطلاعات ثبت خودرو بر اساس نوع در جدول گزارش عملکرد(مغایرت)
-- =============================================
CREATE PROCEDURE [dbo].[sp_VehicleRegistrationinContradictions] 
	-- Add the parameters for the stored procedure here
	@workshops nvarchar(max)=null,
	@Date DATETIME 

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)
DECLARE @PivotTableSQL2 NVARCHAR(MAX)

BEGIN

SET @PivotTableSQL = '
select list.VehicleTypeID,sum(list.count) Count from (
select (case VehicleTypeID when 1 then 1 when 6 then 1 when 5 then 5 when 7 then 5 when 4 then 4 when 8 then 4 when 10 then 4 when 3 then 3 when 11 then 3 else VehicleTypeID end) VehicleTypeID,COUNT(*) as count from tbl_VehicleRegistrations
where WorkshopID in ('+@workshops+') and CONVERT(VARCHAR(10),CreateDate,120) <= ''' + CONVERT(VARCHAR(10),@Date,120) + '''
group by VehicleTypeID) as list
group by list.VehicleTypeID '

EXECUTE(@PivotTableSQL)

------------------------------------------------- Tank Division Plan -------------------------------------------------------------------------------------------------------------------
SET @PivotTableSQL2 = N'
  select L1.Type, SUM(L1.Send) as ''Send'' from
(
select case tbl_VehicleTypes.ID when 1 then N''75 لیتری'' when 6 then N''75 لیتری'' else tbl_VehicleTypes.Description end as ''Type'',[dbo].[GetTankSendNumber2](tbl_VehicleTypes.ID,'+@workshops+') as ''Send''
FROM            dbo.tbl_DivisionPlans INNER JOIN
                         dbo.tbl_TankDivisionPlans ON dbo.tbl_DivisionPlans.ID = dbo.tbl_TankDivisionPlans.DivisionPlanID INNER JOIN
                         dbo.tbl_TypeofTanks ON dbo.tbl_TankDivisionPlans.TypeofTankID = dbo.tbl_TypeofTanks.ID INNER JOIN
                         dbo.tbl_VehicleTypes ON dbo.tbl_TypeofTanks.VehicleTypeId = dbo.tbl_VehicleTypes.ID INNER JOIN
                         dbo.tbl_Workshops ON dbo.tbl_DivisionPlans.WorkshopID = dbo.tbl_Workshops.ID
  where dbo.tbl_DivisionPlans.WorkshopID in ('+@workshops+') and CONVERT(VARCHAR(10),dbo.tbl_DivisionPlans.CreateDate,120) <= ''' + CONVERT(VARCHAR(10),@Date,120) + '''
  group by tbl_VehicleTypes.Description,tbl_VehicleTypes.ID,tbl_TankDivisionPlans.TypeofTankID

) as L1
group by L1.Type'

EXECUTE(@PivotTableSQL2)


END
GO
/****** Object:  StoredProcedure [dbo].[sp_VehicleRegistrationinContradictions_Old]    Script Date: 5/30/2025 2:45:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1399/04/16>
-- Description:	مشاهده اطلاعات ثبت خودرو بر اساس نوع در جدول گزارش عملکرد(مغایرت)
-- =============================================
CREATE PROCEDURE [dbo].[sp_VehicleRegistrationinContradictions_Old] 
	-- Add the parameters for the stored procedure here
	@workshops nvarchar(max)=null,
	@Date DATETIME 

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)
DECLARE @PivotTableSQL2 NVARCHAR(MAX)

BEGIN

SET @PivotTableSQL = '
select list.VehicleTypeID,sum(list.count) Count from (
select (case VehicleTypeID when 1 then 1 when 6 then 1 when 5 then 5 when 7 then 5 when 4 then 4 when 8 then 4 else VehicleTypeID end) VehicleTypeID,COUNT(*) as count from tbl_VehicleRegistrations
where WorkshopID in ('+@workshops+') and CONVERT(VARCHAR(10),CreateDate,120) <= ''' + CONVERT(VARCHAR(10),@Date,120) + '''
group by VehicleTypeID) as list
group by list.VehicleTypeID '

EXECUTE(@PivotTableSQL)

------------------------------------------------- Tank Division Plan -------------------------------------------------------------------------------------------------------------------
SET @PivotTableSQL2 = N'
  select L1.Type, SUM(L1.Send) as ''Send'' from
(
select case tbl_VehicleTypes.ID when 1 then N''75 لیتری'' when 6 then N''75 لیتری'' else tbl_VehicleTypes.Description end as ''Type'',[dbo].[GetTankSendNumber2](tbl_VehicleTypes.ID,'+@workshops+') as ''Send''
FROM            dbo.tbl_DivisionPlans INNER JOIN
                         dbo.tbl_TankDivisionPlans ON dbo.tbl_DivisionPlans.ID = dbo.tbl_TankDivisionPlans.DivisionPlanID INNER JOIN
                         dbo.tbl_TypeofTanks ON dbo.tbl_TankDivisionPlans.TypeofTankID = dbo.tbl_TypeofTanks.ID INNER JOIN
                         dbo.tbl_VehicleTypes ON dbo.tbl_TypeofTanks.VehicleTypeId = dbo.tbl_VehicleTypes.ID INNER JOIN
                         dbo.tbl_Workshops ON dbo.tbl_DivisionPlans.WorkshopID = dbo.tbl_Workshops.ID
  where dbo.tbl_DivisionPlans.WorkshopID in ('+@workshops+') and CONVERT(VARCHAR(10),dbo.tbl_DivisionPlans.CreateDate,120) <= ''' + CONVERT(VARCHAR(10),@Date,120) + '''
  group by tbl_VehicleTypes.Description,tbl_VehicleTypes.ID,tbl_TankDivisionPlans.TypeofTankID

) as L1
group by L1.Type'

EXECUTE(@PivotTableSQL2)


END
GO
/****** Object:  StoredProcedure [dbo].[sp_VehicleRegistrations]    Script Date: 5/30/2025 2:45:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1399/04/11>
-- Description:	مشاهده اطلاعات خودرو به صورت جدولی و دسته بندی نوع خودرو در داشبورد
-- =============================================
CREATE PROCEDURE [dbo].[sp_VehicleRegistrations] 
	-- Add the parameters for the stored procedure here
	@workshops nvarchar(max)=null,
	@FromDate DATETIME ,
	@ToDate DATETIME,
	@VehicleTypeID nvarchar(max)=null,
	@RegistrationTypeID nvarchar(max)=null
	

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)
--DECLARE @FromDate DATETIME = '2011/02/25'
--DECLARE @ToDate DATETIME =  '2020/07/01'

BEGIN

SET @PivotTableSQL = 'select v.VehicleTypeID,vt.Description as ''Title'', COUNT(*) as ''Count'',vt.Image from tbl_VehicleRegistrations v inner join tbl_VehicleTypes vt on v.VehicleTypeID=vt.ID
where v.RegisterStatus=1 and v.RegistrationTypeID in ('+@RegistrationTypeID+') and v.WorkshopID in ('+@workshops+') and CONVERT(VARCHAR(10),v.CreateDate,120) BETWEEN ''' + CONVERT(VARCHAR(10),@FromDate,120) + ''' AND ''' +  CONVERT(VARCHAR(10),@ToDate,120)+ '''
group by v.VehicleTypeID,vt.Image,vt.Description
select *,vt.Description as vtDescription,(case [dbo].[GetVehicleNationalCard](v.ID) when 1 then ''isExist'' else ''noExist'' end ) as N_NationalCard,
											(case [dbo].[GetVehicleCard](v.ID) when 1 then ''isExist'' else ''noExist'' end ) as N_VehicleCard,
											(case [dbo].[GetVehicleLicenseImage](v.ID) when 1 then ''isExist'' else ''noExist'' end ) as N_LicenseImage,
											(case [dbo].[GetVehicleHealthCertificate](v.ID) when 1 then ''isExist'' else ''noExist'' end ) as N_HealthCertificate,
											(case [dbo].[GetVehicleTechnicalDiagnosis](v.ID) when 1 then ''isExist'' else ''noExist'' end ) as N_TechnicalDiagnosis
from tbl_VehicleRegistrations v inner join tbl_VehicleTypes vt on v.VehicleTypeID=vt.ID inner join tbl_Workshops w on v.WorkshopID=w.ID left outer join 
						 dbo.tbl_RegistrationTypes vty on v.RegistrationTypeID=vty.ID
where v.RegisterStatus=1 and v.RegistrationTypeID in ('+@RegistrationTypeID+') and v.WorkshopID in ('+@workshops+') and v.VehicleTypeID in ('+@VehicleTypeID+')  and CONVERT(VARCHAR(10),v.CreateDate,120) BETWEEN ''' + CONVERT(VARCHAR(10),@FromDate,120) + ''' AND ''' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''' 
order by v.ID desc
'

EXECUTE(@PivotTableSQL)

END
--and v.RegistrationTypeID=1
GO
/****** Object:  StoredProcedure [dbo].[sp_VehicleRegistrationSalary]    Script Date: 5/30/2025 2:45:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1399/04/30>
-- Description:	محاسبه دستمزد تبدیل کارگاه ها در بخش مالی 
-- =============================================
CREATE PROCEDURE [dbo].[sp_VehicleRegistrationSalary] 
	-- Add the parameters for the stored procedure here
	@workshops nvarchar(max)=null,
	@FromDate DATETIME ,
	@ToDate DATETIME
	

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN

-----------------------------------------------------------------------financial credit and debit ------------------------------------------------------------------------------
select *,(CONVERT(float,list.Creditor) - CONVERT(float,list.Debtor)) as 'Rem' from (
select ID,Title,ISNULL([dbo].[GetFinancialCreditor](ID,@workshops,@FromDate,@ToDate),0) as 'Creditor',ISNULL([dbo].[GetFinancialDebtor](ID,@workshops,@FromDate,@ToDate),0) as 'Debtor'
FROM [CNGFAPCO].[dbo].[tbl_FinancialDescs] 
where ID in (1,2,3,6,7)
) as list
-----------------------------------------------------------------------vehicle registration count with details and type --------------------------------------------------------
select list.Type,SUM(list.Count) as Count,ISNULL((list.Price),0) as Price,ISNULL(SUM((CONVERT(float,list.Count) * list.Price)),0) as 'Salary' from (
select (Type + ' - ' + tbl_VehicleTypes.Description) as 'Type', COUNT(*) as 'Count', [dbo].[GetRegistrationPrice](WorkshopID,Type,CreateDate) as 'Price' from tbl_VehicleRegistrations inner join tbl_VehicleTypes on tbl_VehicleRegistrations.VehicleTypeID=tbl_VehicleTypes.ID
where RegisterStatus=1 and RegistrationTypeID=1 and (WorkshopID=@workshops or @workshops =0) and CONVERT(VARCHAR(10),CreateDate,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
group by Type,WorkshopID,CreateDate,tbl_VehicleTypes.Description) as list 
group by list.Type,list.Price
-----------------------------------------------------------------------financial credit and debit and Rem in OneView ------------------------------------------------------------
select *, CEILING(((CONVERT(float,list3.Debtor) - CONVERT(float,list3.NonCash))/ (case CONVERT(float,list3.Creditor) when 0 then 1 else CONVERT(float,list3.Creditor) end)  * 100)) as 'NetPercent', CEILING(((CONVERT(float,list3.Debtor) - CONVERT(float,list3.NonCash))/ (  case (CONVERT(float,list3.Creditor)*0.7333 ) when 0 then 1 else (CONVERT(float,list3.Creditor)*0.7333 ) end )* 100)) as 'GrossPercent',
	list3.PreInvoiceHint
from (
select *,(CONVERT(float,list2.Creditor) - CONVERT(float,list2.Deductions) - CONVERT(float,list2.Debtor) - CONVERT(float,list2.NonCash)) as 'RemWithDeductions' from(
select *,(CONVERT(float,list.Creditor) - CONVERT(float,list.Debtor) - CONVERT(float,list.NonCash)) as 'Rem', ((CONVERT(float,list.Creditor) * 0.2667)) as 'Deductions' from (
select ID,Title,ISNULL([dbo].[GetFinancialCreditor](1,ID,@FromDate,@ToDate),0) as 'Creditor',ISNULL([dbo].[GetFinancialDebtor](1,ID,@FromDate,@ToDate),0) as 'Debtor',
		ISNULL([dbo].[GetFinancialDebtor](6,ID,@FromDate,@ToDate),0) as 'NonCash',ISNULL([dbo].[GetCheckedPreInvoice](ID),0) as 'Amount',ISNULL([dbo].[GetPreInvoiceHint](ID),0) as 'PreInvoiceHint',
		ISNULL([dbo].[GetOfferedPrice](ID),0) as 'OfferedPrice',ISNULL([dbo].[GetOfferedSerial](),'-') as 'OfferedSerial',[dbo].[GetOfferedID](ID) as 'OfferedID'
FROM [CNGFAPCO].[dbo].[tbl_Workshops]
where isServices>0 and (ID=@workshops or @workshops =0)
) as list ) as list2 ) as list3
order by list3.ID

-----------------------------------------------------------------------Audits Price registration count with details  --------------------------------------------------------
select SUM(list.Count) as Count,(list.AuditsPrice) as AuditsPrice,SUM((CONVERT(float,list.Count) * list.AuditsPrice)) as 'Salary' from (
select  COUNT(*) as 'Count', [dbo].[GetAuditsPrice](CompaniesID,tbl_VehicleRegistrations.CreateDate) as 'AuditsPrice' from tbl_VehicleRegistrations inner join tbl_VehicleTypes on tbl_VehicleRegistrations.VehicleTypeID=tbl_VehicleTypes.ID inner join
tbl_Workshops on tbl_VehicleRegistrations.WorkshopID=tbl_Workshops.ID
where RegisterStatus=1 and RegistrationTypeID=1 and (WorkshopID=@workshops or @workshops =0) and CONVERT(VARCHAR(10),tbl_VehicleRegistrations.CreateDate,120) BETWEEN '' + CONVERT(VARCHAR(10),@FromDate,120) + '' AND '' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''
group by Type,WorkshopID,tbl_VehicleRegistrations.CreateDate,tbl_VehicleTypes.Description,CompaniesID) as list 
group by list.AuditsPrice
-----------------------------------------------------------------------for cheched Pre Invoice not complete process hint in financial report ------------------------------------------------------------
--select PreInvoiceCode,tbl_FreeSaleInvoices.WorkshopsID, SUM(tbl_FreeSaleInvoices.TotalAmountTaxandComplications) as Amount, tbl_FreeSaleInvoices.SaleCondition
--from tbl_FinallFreeSaleInvoices right outer join tbl_FreeSaleInvoices on tbl_FreeSaleInvoices.InvoiceCode = tbl_FinallFreeSaleInvoices.PreInvoiceCode
--where tbl_FreeSaleInvoices.WorkshopsID in (@workshops) or @workshops=0
--group by PreInvoiceCode,tbl_FreeSaleInvoices.WorkshopsID,tbl_FreeSaleInvoices.SaleCondition
--having PreInvoiceCode is null and tbl_FreeSaleInvoices.SaleCondition='غیر نقدی'

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--SELECT SUM(CarryFare) as 'CarryFare',CarrierName FROM tbl_RemittanceDetails inner join 
--tbl_Remittances on tbl_RemittanceDetails.RemittancesID=tbl_Remittances.ID inner join 
--tbl_DivisionPlans on tbl_Remittances.DivisionPlanID=tbl_DivisionPlans.ID
--where WorkshopID=@workshops or @workshops=0
--group by CarrierName

END
GO
/****** Object:  StoredProcedure [dbo].[SP_VehicleRegistrationsRelatedData]    Script Date: 5/30/2025 2:45:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal>
-- Create date: <1398/12/07>
-- Description:	<جهت مشاهده اطلاعات سایر جداول مرتبط با ثبت نام خودرو>
-- =============================================
CREATE PROCEDURE [dbo].[SP_VehicleRegistrationsRelatedData]
	-- Add the parameters for the stored procedure here
	@VehicleRegistrationID int
AS
BEGIN

SELECT        ID, VehicleRegistrationID, Number, Amount, Type, Date, InvoiceFile, Description, Creator, CreateDate
FROM            dbo.tbl_VehicleInvoices
WHERE VehicleRegistrationID=@VehicleRegistrationID

SELECT        dbo.tbl_VehicleTanks.ID, dbo.tbl_VehicleTanks.VehicleRegistrationID, dbo.tbl_VehicleTanks.Serial, dbo.tbl_VehicleTanks.Volume, dbo.tbl_VehicleTanks.TankConstractorID,SUBSTRING(dbo.tbl_VehicleTanks.ProductDate,1,4) as 'ProductYear',SUBSTRING(dbo.tbl_VehicleTanks.ProductDate,6,2) as 'ProductMonth',
                         SUBSTRING(dbo.tbl_VehicleTanks.ExpirationDate,1,4) as 'ExpirationYear',SUBSTRING(dbo.tbl_VehicleTanks.ExpirationDate,6,2) as 'ExpirationMonth', dbo.tbl_VehicleTanks.SerialTankValve, dbo.tbl_VehicleTanks.TypeTankValve, dbo.tbl_ValveConstractors.Valve as 'ConstractorTankValve', dbo.tbl_TankConstractors.Constractor,
						 dbo.tbl_VehicleTanks.RegulatorSerial,(case when RegulatorConstractorID is null then '' else  [dbo].[GetRegulatorConstractors](dbo.tbl_VehicleTanks.RegulatorConstractorID) end) RegulatorConstractorID
FROM            dbo.tbl_VehicleTanks INNER JOIN
                         dbo.tbl_TankConstractors ON dbo.tbl_VehicleTanks.TankConstractorID = dbo.tbl_TankConstractors.ID INNER JOIN
                         dbo.tbl_ValveConstractors ON dbo.tbl_VehicleTanks.ValveConstractorID = dbo.tbl_ValveConstractors.ID
WHERE dbo.tbl_VehicleTanks.VehicleRegistrationID=@VehicleRegistrationID

END
GO
/****** Object:  StoredProcedure [dbo].[sp_VehicleRegistrationTable]    Script Date: 5/30/2025 2:45:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1399/04/16>
-- Description:	مشاهده اطلاعات در جدول ثبت نام
-- =============================================
CREATE PROCEDURE [dbo].[sp_VehicleRegistrationTable] 
	-- Add the parameters for the stored procedure here
	@workshops nvarchar(max)=null,
	@FromDate DATETIME ,
	@ToDate DATETIME	

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN

SET @PivotTableSQL = N'
SELECT        dbo.tbl_VehicleRegistrations.*, dbo.tbl_VehicleTypes.Type + '' '' + dbo.tbl_VehicleTypes.Description AS VehicleType, dbo.tbl_VehicleTanks.*, dbo.tbl_Workshops.Title as WorkshopTitle, dbo.tbl_TypeofTanks.Type AS CylinderBulk, 
                         dbo.tbl_TypeofTanks.Description AS Expr3, dbo.tbl_TankConstractors.Constractor, dbo.tbl_ValveConstractors.Valve,[dbo].[GetTypeofUse](TypeofUseID) TypeofUse, tbl_VehicleRegistrations.OwnerName + '' '' + tbl_VehicleRegistrations.OwnerFamily as FullName,
						 tbl_VehicleRegistrations.NationalCode,(case when RegulatorConstractorID is null then '' '' else  [dbo].[GetRegulatorConstractors](dbo.tbl_VehicleTanks.RegulatorConstractorID) end) KitConstractor, dbo.tbl_Insurances.Number as InsuranceNumber,
						 (case when CutofValveConstractorID is null then '' '' else  [dbo].[GetCutofValveConstractors](dbo.tbl_VehicleTanks.CutofValveConstractorID) end) CutofValveConstractor,
						 (case when FillingValveConstractorID is null then '' '' else  [dbo].[GetFillingValveConstractors](dbo.tbl_VehicleTanks.FillingValveConstractorID) end) FillingValveConstractor,
						 case  when [RegistrationTypeID] is null then 1 else [RegistrationTypeID] end as [RegistrationTypeID],
						 case  when tbl_RegistrationTypes.Type is null then N''طرح تبدیل'' else tbl_RegistrationTypes.Type end as RegistrationType
FROM            dbo.tbl_TypeofTanks INNER JOIN
                         dbo.tbl_VehicleRegistrations left outer JOIN
						 dbo.tbl_Insurances on tbl_VehicleRegistrations.ID= dbo.tbl_Insurances.VehicleRegistrationID left outer join
                         dbo.tbl_VehicleTypes ON dbo.tbl_VehicleRegistrations.VehicleTypeID = dbo.tbl_VehicleTypes.ID left outer JOIN
                         dbo.tbl_Workshops ON dbo.tbl_VehicleRegistrations.WorkshopID = dbo.tbl_Workshops.ID ON dbo.tbl_TypeofTanks.VehicleTypeId = dbo.tbl_VehicleTypes.ID LEFT OUTER JOIN
                         dbo.tbl_TankConstractors left outer JOIN
                         dbo.tbl_VehicleTanks ON dbo.tbl_TankConstractors.ID = dbo.tbl_VehicleTanks.TankConstractorID left outer JOIN
                         dbo.tbl_ValveConstractors ON dbo.tbl_VehicleTanks.ValveConstractorID = dbo.tbl_ValveConstractors.ID ON dbo.tbl_VehicleRegistrations.ID = dbo.tbl_VehicleTanks.VehicleRegistrationID LEFT OUTER JOIN
                         dbo.tbl_TypeofUses ON dbo.tbl_VehicleRegistrations.TypeofUseID = dbo.tbl_TypeofUses.ID left outer join 
						 dbo.tbl_RegistrationTypes on dbo.tbl_VehicleRegistrations.RegistrationTypeID=dbo.tbl_RegistrationTypes.ID
where RegisterStatus=1 and WorkshopID in ('+@workshops+') and CONVERT(VARCHAR(10),tbl_VehicleRegistrations.CreateDate,120) BETWEEN ''' + CONVERT(VARCHAR(10),@FromDate,120) + ''' AND ''' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''' 
order by tbl_VehicleRegistrations.ID desc

'

EXECUTE(@PivotTableSQL)

END

------------------------------------------------old version 1399-10-18-------------------------------------------------------
--select *,[dbo].[GetWorkshops](WorkshopID) as WorkshopTitle,[dbo].[GetInsuranceNumber](ID) as InsuranceNumber,[dbo].[GetTypeofUse](TypeofUseID) as TypeofUse ,[dbo].[GetVehicleType](VehicleTypeID) as VehicleType,
--[dbo].[GetValveConstractors]([dbo].[GetVehicleValveConstractors](ID)) ValveCostractor,[dbo].[GetVehicleValves](ID) ValveNumber,[dbo].[GetVehicleCylinders](ID) CylinderNumber,[dbo].[GetCylinderBulk]([dbo].[GetVehicleCylinders](ID)) CylinderBulk,
--[dbo].[GetCylinderConstractors]([dbo].[GetVehicleCylinderContractors](ID)) CylinderCostractor,[dbo].[GetKitConstractors](SerialKit) KitConstractor
--from tbl_VehicleRegistrations
--where WorkshopID in ('+@workshops+') and CONVERT(VARCHAR(10),CreateDate,120) BETWEEN ''' + CONVERT(VARCHAR(10),@FromDate,120) + ''' AND ''' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''' 
--order by ID desc
GO
/****** Object:  StoredProcedure [dbo].[sp_VehicleRegistrationTable_Copied_in_14020416]    Script Date: 5/30/2025 2:45:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ali jafari jalal  >
-- Create date: <1399/04/16>
-- Description:	مشاهده اطلاعات در جدول ثبت نام
-- =============================================
Create PROCEDURE [dbo].[sp_VehicleRegistrationTable_Copied_in_14020416] 
	-- Add the parameters for the stored procedure here
	@workshops nvarchar(max)=null,
	@FromDate DATETIME ,
	@ToDate DATETIME	

AS
DECLARE @PivotTableSQL NVARCHAR(MAX)

BEGIN

SET @PivotTableSQL = '
SELECT        dbo.tbl_VehicleRegistrations.*, dbo.tbl_VehicleTypes.Type + '' '' + dbo.tbl_VehicleTypes.Description AS VehicleType, dbo.tbl_VehicleTanks.*, dbo.tbl_Workshops.Title as WorkshopTitle, dbo.tbl_TypeofTanks.Type AS CylinderBulk, 
                         dbo.tbl_TypeofTanks.Description AS Expr3, dbo.tbl_TankConstractors.Constractor, dbo.tbl_ValveConstractors.Valve,[dbo].[GetTypeofUse](TypeofUseID) TypeofUse, tbl_VehicleRegistrations.OwnerName + '' '' + tbl_VehicleRegistrations.OwnerFamily as FullName,
						 tbl_VehicleRegistrations.NationalCode,(case when RegulatorConstractorID is null then '' '' else  [dbo].[GetRegulatorConstractors](dbo.tbl_VehicleTanks.RegulatorConstractorID) end) KitConstractor, [dbo].[GetInsuranceNumber](dbo.tbl_VehicleRegistrations.ID) as InsuranceNumber,
						 (case when CutofValveConstractorID is null then '' '' else  [dbo].[GetCutofValveConstractors](dbo.tbl_VehicleTanks.CutofValveConstractorID) end) CutofValveConstractor,
						 (case when FillingValveConstractorID is null then '' '' else  [dbo].[GetFillingValveConstractors](dbo.tbl_VehicleTanks.FillingValveConstractorID) end) FillingValveConstractor
FROM            dbo.tbl_TypeofTanks INNER JOIN
                         dbo.tbl_VehicleRegistrations INNER JOIN
                         dbo.tbl_VehicleTypes ON dbo.tbl_VehicleRegistrations.VehicleTypeID = dbo.tbl_VehicleTypes.ID INNER JOIN
                         dbo.tbl_Workshops ON dbo.tbl_VehicleRegistrations.WorkshopID = dbo.tbl_Workshops.ID ON dbo.tbl_TypeofTanks.VehicleTypeId = dbo.tbl_VehicleTypes.ID LEFT OUTER JOIN
                         dbo.tbl_TankConstractors INNER JOIN
                         dbo.tbl_VehicleTanks ON dbo.tbl_TankConstractors.ID = dbo.tbl_VehicleTanks.TankConstractorID INNER JOIN
                         dbo.tbl_ValveConstractors ON dbo.tbl_VehicleTanks.ValveConstractorID = dbo.tbl_ValveConstractors.ID ON dbo.tbl_VehicleRegistrations.ID = dbo.tbl_VehicleTanks.VehicleRegistrationID LEFT OUTER JOIN
                         dbo.tbl_TypeofUses ON dbo.tbl_VehicleRegistrations.TypeofUseID = dbo.tbl_TypeofUses.ID
where RegisterStatus=1 and WorkshopID in ('+@workshops+') and CONVERT(VARCHAR(10),tbl_VehicleRegistrations.CreateDate,120) BETWEEN ''' + CONVERT(VARCHAR(10),@FromDate,120) + ''' AND ''' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''' 
order by tbl_VehicleRegistrations.ID desc

'

EXECUTE(@PivotTableSQL)

END

------------------------------------------------old version 1399-10-18-------------------------------------------------------
--select *,[dbo].[GetWorkshops](WorkshopID) as WorkshopTitle,[dbo].[GetInsuranceNumber](ID) as InsuranceNumber,[dbo].[GetTypeofUse](TypeofUseID) as TypeofUse ,[dbo].[GetVehicleType](VehicleTypeID) as VehicleType,
--[dbo].[GetValveConstractors]([dbo].[GetVehicleValveConstractors](ID)) ValveCostractor,[dbo].[GetVehicleValves](ID) ValveNumber,[dbo].[GetVehicleCylinders](ID) CylinderNumber,[dbo].[GetCylinderBulk]([dbo].[GetVehicleCylinders](ID)) CylinderBulk,
--[dbo].[GetCylinderConstractors]([dbo].[GetVehicleCylinderContractors](ID)) CylinderCostractor,[dbo].[GetKitConstractors](SerialKit) KitConstractor
--from tbl_VehicleRegistrations
--where WorkshopID in ('+@workshops+') and CONVERT(VARCHAR(10),CreateDate,120) BETWEEN ''' + CONVERT(VARCHAR(10),@FromDate,120) + ''' AND ''' +  CONVERT(VARCHAR(10),@ToDate,120)+ ''' 
--order by ID desc
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1[41] 2[34] 3) )"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 2
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "vw_NIOPDC"
            Begin Extent = 
               Top = 17
               Left = 142
               Bottom = 147
               Right = 323
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "vw_IRNGV"
            Begin Extent = 
               Top = 0
               Left = 450
               Bottom = 130
               Right = 631
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "vw_VehicleRegistrations"
            Begin Extent = 
               Top = 135
               Left = 450
               Bottom = 265
               Right = 631
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      PaneHidden = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_3TableDataDiff'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_3TableDataDiff'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "t1"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 224
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "t2"
            Begin Extent = 
               Top = 6
               Left = 262
               Bottom = 136
               Right = 452
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "t3"
            Begin Extent = 
               Top = 6
               Left = 490
               Bottom = 136
               Right = 690
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "t4"
            Begin Extent = 
               Top = 6
               Left = 728
               Bottom = 136
               Right = 938
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Barname'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Barname'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "t1"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 328
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "t2"
            Begin Extent = 
               Top = 6
               Left = 366
               Bottom = 136
               Right = 576
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_DastmozdeTaaviz'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_DastmozdeTaaviz'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "t1"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 328
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "t2"
            Begin Extent = 
               Top = 6
               Left = 366
               Bottom = 136
               Right = 576
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_DastmozdeTabdil'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_DastmozdeTabdil'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "vw_NIOPDC"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 219
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "vw_VehicleRegistrations"
            Begin Extent = 
               Top = 6
               Left = 476
               Bottom = 136
               Right = 657
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_diffvalue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_diffvalue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tbl_DivisionPlans"
            Begin Extent = 
               Top = 6
               Left = 203
               Bottom = 200
               Right = 387
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tbl_DivisionPlanBOMs"
            Begin Extent = 
               Top = 5
               Left = 429
               Bottom = 206
               Right = 599
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tbl_BOMs"
            Begin Extent = 
               Top = 4
               Left = 649
               Bottom = 208
               Right = 825
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "tbl_Workshops"
            Begin Extent = 
               Top = 9
               Left = 0
               Bottom = 139
               Right = 194
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tbl_EquipmentList"
            Begin Extent = 
               Top = 6
               Left = 863
               Bottom = 210
               Right = 1033
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tbl_VehicleTypes"
            Begin Extent = 
               Top = 6
               Left = 1071
               Bottom = 136
               Right = 1241
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 10
         Width = 284
      ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Vw_DivisionPlan New query'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'   Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Vw_DivisionPlan New query'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Vw_DivisionPlan New query'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "t1"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 328
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "t2"
            Begin Extent = 
               Top = 6
               Left = 366
               Bottom = 136
               Right = 576
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_ForusheMakhzan'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_ForusheMakhzan'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "t1"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 328
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "t2"
            Begin Extent = 
               Top = 6
               Left = 366
               Bottom = 136
               Right = 576
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_ForusheShirMakhzan'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_ForusheShirMakhzan'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tbl_IRNGV"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 280
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_IRNGV'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_IRNGV'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tbl_Workshops"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 232
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Kosurat'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Kosurat'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tbl_GCR"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 285
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 11
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_NIOPDC'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_NIOPDC'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "t1"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 226
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "t2"
            Begin Extent = 
               Top = 6
               Left = 264
               Bottom = 102
               Right = 450
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "t3"
            Begin Extent = 
               Top = 6
               Left = 488
               Bottom = 136
               Right = 698
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Pardakhtiha'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Pardakhtiha'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "t1"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 328
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "t2"
            Begin Extent = 
               Top = 6
               Left = 366
               Bottom = 136
               Right = 576
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Tabdil_MaboTafavot'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Tabdil_MaboTafavot'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tbl_VehicleRegistrations"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 233
            End
            DisplayFlags = 280
            TopColumn = 45
         End
         Begin Table = "tbl_VehicleTypes"
            Begin Extent = 
               Top = 6
               Left = 271
               Bottom = 136
               Right = 441
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tbl_Workshops"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 268
               Right = 232
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tbl_VehicleTanks"
            Begin Extent = 
               Top = 270
               Left = 38
               Bottom = 400
               Right = 260
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 11
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_VehicleRegistrations'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_VehicleRegistrations'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_VehicleRegistrations'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tbl_Workshops"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 232
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tbl_Cities"
            Begin Extent = 
               Top = 6
               Left = 270
               Bottom = 119
               Right = 440
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tbl_States"
            Begin Extent = 
               Top = 6
               Left = 478
               Bottom = 102
               Right = 648
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Workshops'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Workshops'
GO
