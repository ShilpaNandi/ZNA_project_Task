if exists (select 1 from sysobjects 
		where name = 'Add_CUST_PERS_REL_MASS_ASSIGN' and type = 'P')
	drop procedure Add_CUST_PERS_REL_MASS_ASSIGN
go

set ansi_nulls off
go

---------------------------------------------------------------------
-----
----- PROC NAME:  Add_CUST_PERS_REL_MASS_ASSIGN
-----
----- VERSION:  SQL SERVER 2008
-----
----- AUTHOR :  CSC
-----
----- DESCRIPTION: Insert or update the records in custmr_pers_rel table on maas reassigment
----
----- ON EXIT: 
-----   
-----
----- MODIFIED: 
-----   
----- 
---------------------------------------------------------------------

CREATE Procedure dbo.Add_CUST_PERS_REL_MASS_ASSIGN
(
@custmr_ids varchar(max),
@rol_ids varchar(max),
@pers_ids varchar(max),
@crte_user_id int 
)
AS
BEGIN

BEGIN TRY
Declare @custid int
Declare @RollId int
Declare @UserId int
Declare @CI int 
DEclare @i int
Declare @CustidLen int=len(@custmr_ids)
Declare @RollIdLen int=len(@rol_ids)
Declare @UserIdLen int=len(@pers_ids)

declare @TEMP table
(
ID INT IDENTITY(1,1), CUSTID int,ROLLID int,USRID int
)	

WHILE(LEN(@custmr_ids)>0)
BEGIN
	SET @CI=CHARINDEX(',',@custmr_ids) 
		IF(@CI=0) 
			BEGIN 
			SET @custid=@custmr_ids 
			SET @custmr_ids=''
			END 
		ELSE 
			BEGIN 
			SET @custid=SUBSTRING(@custmr_ids,0,@CI) 
			END 
	INSERT INTO @TEMP(CUSTID) SELECT @custid 
	SET @custmr_ids=SUBSTRING(@custmr_ids,@CI+1,@CustidLen) 
END

SET @I=1
WHILE(LEN(@rol_ids)>0)
BEGIN
	SET @CI=CHARINDEX(',',@rol_ids) 
		IF(@CI=0) 
			BEGIN 
			SET @RollId=@rol_ids 
			SET @rol_ids=''
			END 
		ELSE 
			BEGIN 
			SET @RollId=SUBSTRING(@rol_ids,0,@CI) 
			END 
	UPDATE  @TEMP SET ROLLID=@RollId  where ID=@i
	SET @rol_ids=SUBSTRING(@rol_ids,@CI+1,@RollIdLen) 
	SET @i=@i+1
END
SET @I=1
WHILE(LEN(@pers_ids)>0)
BEGIN
	SET @CI=CHARINDEX(',',@pers_ids) 
		IF(@CI=0) 
			BEGIN 
			SET @UserId=@pers_ids 
			SET @pers_ids=''
			END 
		ELSE 
			BEGIN 
			SET @UserId=SUBSTRING(@pers_ids,0,@CI) 
			END 
	UPDATE  @TEMP SET USRID=@UserId  where ID=@i
	SET @pers_ids=SUBSTRING(@pers_ids,@CI+1,@UserIdLen) 
	SET @i=@i+1
END

DECLARe @CNT int=(select count(*) from @TEMP)
SET @i=1
DECLARE @j int
WHILE (@CNT>=@i)
BEGIN
SET @j=1
SET @custid=(SELECT CUSTID FROM @TEMP where ID=@i)
WHILE ( @CNT>=@j)
	BEGIN
	SET @RollId=(SELECT ROLLID FROM @TEMP where ID=@j)
	SET @UserId=(SELECT USRID FROM @TEMP where ID=@j)

	IF((SELECT COUNT(*) FROM [CUSTMR_PERS_REL] where custmr_id=@custid and rol_id=@RollId)=0)
	BEGIN
		INSERT INTO [CUSTMR_PERS_REL](custmr_id,pers_id,rol_id,crte_dt,crte_user_id)
		SELECT @custid,CASE WHEN @UserId = 0 THEN NULL ELSE @UserId END,@RollId,GETDATE(),@crte_user_id
		--print @i
	END
	ELSE
	BEGIN
		UPDATE [CUSTMR_PERS_REL] SET pers_id=CASE WHEN @UserId = 0 THEN NULL ELSE @UserId END,
		[updt_user_id]=@crte_user_id,updt_dt=GETDATE() where custmr_id=@custid and  rol_id=@RollId
  
  
	END
	SET @j=@j+1
	END
SET @i=@i+1
END
 

END TRY
BEGIN CATCH


declare @err_msg varchar(500),@err_ln varchar(10),
			@err_proc varchar(30),@err_no varchar(10)
	select  @err_msg = error_message(),
		    @err_no = error_number(),
		    @err_proc = error_procedure(),
			@err_ln = error_line()
	set @err_msg = '- error no.:' + isnull(@err_no, ' ') + '; procedure:' 
		+ isnull(@err_proc, ' ') + ';error line:' + isnull(@err_ln, ' ') + ';description:' + isnull(@err_msg, ' ' )
	--set @err_msg_op = @err_msg

	insert into [dbo].[APLCTN_STS_LOG]
   (
		[src_txt]
	   ,[sev_cd]
	   ,[shrt_desc_txt]
	   ,[full_desc_txt]
	   ,[custmr_id]
	   ,[prem_adj_id]
	   ,[crte_user_id]
	)
	values
    (
		'AIS Mass Reassgin'
       ,'ERR'
       ,'Mass Reassign error'
       ,'Error encountered during Mass reassign ' 
			+ isnull(@err_msg, ' ')
	   ,@custmr_ids
	   ,@rol_ids
       ,isnull(@crte_user_id, 0)
	)


	select 
    error_number() AS ErrorNumber,
    error_severity() AS ErrorSeverity,
    error_state() as ErrorState,
    error_procedure() as ErrorProcedure,
    error_line() as ErrorLine,
    error_message() as ErrorMessage

	declare @err_sev varchar(10)

	select  @err_msg = error_message(),
		    @err_no = error_number(),
		    @err_proc = error_procedure(),
			@err_ln = error_line(),
			@err_sev = error_severity()

	RAISERROR (@err_msg, -- Message text.
               @err_sev, -- Severity.
               1 -- State.
               )


 
END CATCH


END

GO


if object_id('Add_CUST_PERS_REL_MASS_ASSIGN') is not null
	print 'Created Procedure Add_CUST_PERS_REL_MASS_ASSIGN'
else
	print 'Failed Creating Procedure Add_CUST_PERS_REL_MASS_ASSIGN'
go

if object_id('Add_CUST_PERS_REL_MASS_ASSIGN') is not null
	grant exec on Add_CUST_PERS_REL_MASS_ASSIGN to public
go
