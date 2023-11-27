/**************************** Table Chnages *************************************************************/
----------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------
GO
IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'ARMIS_LOS_POL' AND COLUMN_NAME = 'copy_ind')
BEGIN
	ALTER TABLE ARMIS_LOS_POL ADD copy_ind BIT NULL DEFAULT 0
END
GO
update ARMIS_LOS_POL set copy_ind=0

----------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------
GO
IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'ARMIS_LOS_EXC' AND COLUMN_NAME = 'copy_ind')
BEGIN
	ALTER TABLE ARMIS_LOS_EXC ADD copy_ind BIT NULL DEFAULT 0
END
GO
update ARMIS_LOS_EXC set copy_ind=0
----------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------

/***************************** Index Changes ************************************************************/
GO
IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'COML_AGMT_NN8' AND object_id = OBJECT_ID('COML_AGMT'))
BEGIN
	CREATE NONCLUSTERED INDEX COML_AGMT_NN8
	ON [dbo].[COML_AGMT] ([pol_sym_txt],[pol_nbr_txt],[pol_modulus_txt])
	
	PRINT 'Created COML_AGMT_NN8 nonclustered index on COML_AGMT table successfully'
END
GO
IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'COML_AGMT_NN9' AND object_id = OBJECT_ID('COML_AGMT'))
BEGIN
	CREATE NONCLUSTERED INDEX COML_AGMT_NN9
	ON [dbo].[COML_AGMT] ([custmr_id],[pol_eff_dt],[planned_end_date],[pol_sym_txt],[pol_nbr_txt],[pol_modulus_txt])
	
	PRINT 'Created COML_AGMT_NN9 nonclustered index on COML_AGMT table successfully'
END
GO
IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'PERS_NN8' AND object_id = OBJECT_ID('PERS'))
BEGIN
	CREATE NONCLUSTERED INDEX PERS_NN8
	ON [dbo].[PERS] ([external_reference])
	INCLUDE ([pers_id])
	
	PRINT 'Created PERS_NN8 nonclustered index on PERS table successfully'
END
GO
IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'ARMIS_LOS_EXC_NN3' AND object_id = OBJECT_ID('ARMIS_LOS_EXC'))
BEGIN
	CREATE NONCLUSTERED INDEX ARMIS_LOS_EXC_NN3
	ON [dbo].[ARMIS_LOS_EXC] ([prem_adj_pgm_id],[custmr_id],[actv_ind])
	
	PRINT 'Created ARMIS_LOS_EXC_NN3 nonclustered index on ARMIS_LOS_EXC table successfully'
END
GO 

/***************************************** New Tables *******************************************************************/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LOSS_INFO_COPY_STAGE_EXEC]') AND type in (N'U'))
DROP TABLE [dbo].[LOSS_INFO_COPY_STAGE_EXEC]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING OFF
GO

CREATE TABLE [dbo].[LOSS_INFO_COPY_STAGE_EXEC](
	[Valuation_Date] [varchar](max) NULL,
	[LOB] [varchar](max) NULL,
	[POLICY_NO] [varchar](max) NULL,
	[CUSTMR_ID] [varchar](max) NULL,
	[PGM_EFF_DT] [varchar](max) NULL,
	[PGM_EXP_DT] [varchar](max) NULL,
	[PGM_TYPE] [varchar](max) NULL,
	[STATE] [varchar](max) NULL,
	[POL_EFF_DT] [varchar](max) NULL,
	[POL_EXP_DT] [varchar](max) NULL,
	[CLM_NBR_TXT] [varchar](max) NULL,
	[ADDN_CLM_IND] [varchar](max) NULL,
	[ADDN_CLM_TXT] [varchar](max) NULL,
	[CLMT_NM] [varchar](max) NULL,
	[CLM_STS_ID] [varchar](max) NULL,
	[COVG_TRIGR_DT] [varchar](max) NULL,
	[LIM2_AMT] [varchar](max) NULL,
	[LOS_PAID_IDNMTY_AMT] [varchar](max) NULL,
	[LOS_PAID_EXPS_AMT] [varchar](max) NULL,
	[LOS_RESRV_IDNMTY_AMT] [varchar](max) NULL,
	[LOS_RESRV_EXPS_AMT] [varchar](max) NULL,
	[NON_BILABL_PAID_IDNMTY_AMT] [varchar](max) NULL,
	[NON_BILABL_PAID_EXPS_AMT] [varchar](max) NULL,
	[NON_BILABL_RESRVD_IDNMTY_AMT] [varchar](max) NULL,
	[NON_BILABL_RESRVD_EXPS_AMT] [varchar](max) NULL,
	[LOS_SYS_GENRT_IND] [varchar](max) NULL,
	[CRTE_USER_ID] [varchar](100) NULL,
	[CRTE_DT] [datetime] NULL,
	[VALIDATE] [bit] NULL,
	[LOSS_INFO_COPY_STAGE_EXEC_ID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_LOSS_INFO_COPY_STAGE_EXEC] PRIMARY KEY CLUSTERED 
(
	[LOSS_INFO_COPY_STAGE_EXEC_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

PRINT 'Created LOSS_INFO_COPY_STAGE_EXEC table successfully'
GO

SET ANSI_PADDING OFF
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LOSS_INFO_COPY_STAGE_EXEC_STATUSLOG]') AND type in (N'U'))
DROP TABLE [dbo].[LOSS_INFO_COPY_STAGE_EXEC_STATUSLOG]
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[LOSS_INFO_COPY_STAGE_EXEC_STATUSLOG](
	[Valuation_Date] [varchar](max) NULL,
	[LOB] [varchar](max) NULL,
	[POLICY_NO] [varchar](max) NULL,
	[CUSTMR_ID] [varchar](max) NULL,
	[PGM_EFF_DT] [varchar](max) NULL,
	[PGM_EXP_DT] [varchar](max) NULL,
	[PGM_TYPE] [varchar](max) NULL,
	[STATE] [varchar](max) NULL,
	[POL_EFF_DT] [varchar](max) NULL,
	[POL_EXP_DT] [varchar](max) NULL,
	[CLM_NBR_TXT] [varchar](max) NULL,
	[ADDN_CLM_IND] [varchar](max) NULL,
	[ADDN_CLM_TXT] [varchar](max) NULL,
	[CLMT_NM] [varchar](max) NULL,
	[CLM_STS_ID] [varchar](max) NULL,
	[COVG_TRIGR_DT] [varchar](max) NULL,
	[LIM2_AMT] [varchar](max) NULL,
	[LOS_PAID_IDNMTY_AMT] [varchar](max) NULL,
	[LOS_PAID_EXPS_AMT] [varchar](max) NULL,
	[LOS_RESRV_IDNMTY_AMT] [varchar](max) NULL,
	[LOS_RESRV_EXPS_AMT] [varchar](max) NULL,
	[NON_BILABL_PAID_IDNMTY_AMT] [varchar](max) NULL,
	[NON_BILABL_PAID_EXPS_AMT] [varchar](max) NULL,
	[NON_BILABL_RESRVD_IDNMTY_AMT] [varchar](max) NULL,
	[NON_BILABL_RESRVD_EXPS_AMT] [varchar](max) NULL,
	[LOS_SYS_GENRT_IND] [varchar](max) NULL,
	[CRTE_USER_ID] [varchar](100) NULL,
	[CRTE_DT] [datetime] NULL,
	[VALIDATE] [bit] NULL,
	[TXTSTATUS] [varchar](50) NULL,
	[TXTERRORDESC] [varchar](max) NULL,
	[LOSS_INFO_COPY_STAGE_EXEC_ID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_LOSS_INFO_COPY_STAGE_EXEC_STATUSLOG] PRIMARY KEY CLUSTERED 
(
	[LOSS_INFO_COPY_STAGE_EXEC_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

PRINT 'Created LOSS_INFO_COPY_STAGE_EXEC_STATUSLOG table successfully'
GO

SET ANSI_PADDING OFF
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LOSS_INFO_COPY_STAGE_POL]') AND type in (N'U'))
DROP TABLE [dbo].[LOSS_INFO_COPY_STAGE_POL]
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[LOSS_INFO_COPY_STAGE_POL](
	[Valuation_Date] [varchar](max) NULL,
	[LOB] [varchar](max) NULL,
	[POLICY_NO] [varchar](max) NULL,
	[CUSTMR_ID] [varchar](max) NULL,
	[PGM_EFF_DT] [varchar](max) NULL,
	[PGM_EXP_DT] [varchar](max) NULL,
	[PGM_TYPE] [varchar](max) NULL,
	[STATE] [varchar](max) NULL,
	[POL_EFF_DT] [varchar](max) NULL,
	[POL_EXP_DT] [varchar](max) NULL,
	[SCGID] [varchar](max) NULL,
	[PAID_IDNMTY_AMT] [varchar](max) NULL,
	[PAID_EXPS_AMT] [varchar](max) NULL,
	[RESRV_IDNMTY_AMT] [varchar](max) NULL,
	[RESRV_EXPS_AMT] [varchar](max) NULL,
	[SYS_GENRT_IND] [varchar](max) NULL,
	[CRTE_USER_ID] [varchar](100) NULL,
	[CRTE_DT] [datetime] NULL,
	[VALIDATE] [bit] NULL,
	[LOSS_INFO_COPY_STAGE_ID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_LOSS_INFO_COPY_STAGE] PRIMARY KEY CLUSTERED 
(
	[LOSS_INFO_COPY_STAGE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

PRINT 'Created LOSS_INFO_COPY_STAGE_POL table successfully'
GO

SET ANSI_PADDING OFF
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LOSS_INFO_COPY_STAGE_POL_STATUSLOG]') AND type in (N'U'))
DROP TABLE [dbo].[LOSS_INFO_COPY_STAGE_POL_STATUSLOG]
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING OFF
GO

CREATE TABLE [dbo].[LOSS_INFO_COPY_STAGE_POL_STATUSLOG](
	[Valuation_Date] [varchar](max) NULL,
	[LOB] [varchar](max) NULL,
	[POLICY_NO] [varchar](max) NULL,
	[CUSTMR_ID] [varchar](max) NULL,
	[PGM_EFF_DT] [varchar](max) NULL,
	[PGM_EXP_DT] [varchar](max) NULL,
	[PGM_TYPE] [varchar](max) NULL,
	[STATE] [varchar](max) NULL,
	[POL_EFF_DT] [varchar](max) NULL,
	[POL_EXP_DT] [varchar](max) NULL,
	[SCGID] [varchar](max) NULL,
	[PAID_IDNMTY_AMT] [varchar](max) NULL,
	[PAID_EXPS_AMT] [varchar](max) NULL,
	[RESRV_IDNMTY_AMT] [varchar](max) NULL,
	[RESRV_EXPS_AMT] [varchar](max) NULL,
	[SYS_GENRT_IND] [varchar](max) NULL,
	[CRTE_USER_ID] [varchar](100) NULL,
	[CRTE_DT] [datetime] NULL,
	[VALIDATE] [bit] NULL,
	[TXTSTATUS] [varchar](50) NULL,
	[TXTERRORDESC] [varchar](max) NULL,
	[LOSS_INFO_COPY_STAGE_ID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_LOSS_INFO_COPY_STAGE_POL_STATUSLOG] PRIMARY KEY CLUSTERED 
(
	[LOSS_INFO_COPY_STAGE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

PRINT 'Created LOSS_INFO_COPY_STAGE_POL_STATUSLOG table successfully'
GO

SET ANSI_PADDING OFF
GO

/************************************* Function Changes **************************************************/
if exists (select 1 from sysobjects 
		where name = 'fn_GetPolCheck' and type = 'FN')
	drop function fn_GetPolCheck
GO
set ansi_nulls on
GO


SET QUOTED_IDENTIFIER ON
GO

  
---------------------------------------------------------------------  
-----  
----- Proc Name:  fn_GetPolCheck  
-----  
----- Version:  SQL Server 2005  
-----  
----- Created:  Suneel Kumar Mogali  
-----  
----- Description: Tells whether coml_agmt_id present in prem_adj_pgm_setup_pol table.  
-----  
----- Modified:   07/16/2015 : Added adj_parmet_typ_id 399 for fixing the issue with INC0862333(The Loss Fund exhibit is missing the policy Numbers)
-----  
---------------------------------------------------------------------  
CREATE FUNCTION [dbo].[fn_GetPolCheck]  
(@comlagmtid int,  
@adjid int,  
@perdid int,  
@typnum int)  
RETURNS int   
AS  
BEGIN  
  
declare @Return int  
set @Return = 0  
  
IF EXISTS(
select 1 from prem_adj_pgm_setup_pol Inner join  
prem_adj_pgm_setup on prem_adj_pgm_setup.prem_adj_pgm_setup_id =  
prem_adj_pgm_setup_pol.prem_adj_pgm_setup_id and  
prem_adj_pgm_setup.adj_parmet_typ_id IN (398,399,400)  
inner join prem_adj_pgm on prem_adj_pgm.prem_adj_pgm_id = prem_adj_pgm_setup_pol.  
prem_adj_pgm_id and prem_adj_pgm.custmr_id = prem_adj_pgm_setup_pol.custmr_id  
inner join prem_adj_perd on prem_adj_perd.prem_adj_pgm_id = prem_adj_pgm.prem_adj_pgm_id  
and prem_adj_perd.prem_adj_id = @adjid and prem_adj_perd.prem_adj_perd_id = @perdid  
where coml_agmt_id = @comlagmtid 
) 
BEGIN
	SET @Return = 1
END
  
  
return @Return  
end  
  

  
GO
if object_id('fn_GetPolCheck') is not null
	print 'Created function fn_GetPolCheck'
else
	print 'Failed function fn_GetPolCheck'
go

if object_id('fn_GetPolCheck') is not null
	grant exec on fn_GetPolCheck to public
go 

/*************************************** Stored Procedure Changes ************************************************************/
if exists (select 1 from sysobjects where name = 'PrgAISCalcResults' and type = 'P')
	drop procedure PrgAISCalcResults
go

set ansi_nulls off
go

---------------------------------------------------------------------
-----
-----	Proc Name:	PrgAISCalcResults
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	This stored procedure is used for delete records from tables storing calculation results.
-----
-----	On Exit:	
-----			
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification
-----               07/19/2010 --Venkat Kolimi
-----               Added the statements related to the AIS 21 surcharges project

---------------------------------------------------------------------

CREATE procedure [dbo].[PrgAISCalcResults] 
@customer_id int,
@premium_adjustment_id int,
@premium_adj_period_id int,
@delete_plb bit,
@delete_ilrf bit,
@delete_chf bit
as

begin
	set nocount on

declare	@trancount int


set @trancount = @@trancount
if @trancount = 0 
	begin
	    begin transaction 
	end


begin try

	delete from PREM_ADJ_PARMET_DTL WITH (ROWLOCK)
	where custmr_id = @customer_id
	and prem_adj_id = @premium_adjustment_id
	and prem_adj_perd_id = @premium_adj_period_id

	delete from dbo.PREM_ADJ_PARMET_SETUP WITH (ROWLOCK)
	where custmr_id = @customer_id
	and prem_adj_id = @premium_adjustment_id
	and prem_adj_perd_id = @premium_adj_period_id

	delete from dbo.PREM_ADJ_ARIES_CLRING WITH (ROWLOCK)
	where custmr_id = @customer_id
	and prem_adj_id = @premium_adjustment_id
	--and prem_adj_perd_id = @premium_adj_period_id

--	delete from dbo.PREM_ADJ_COMB_ELEMTS WITH (ROWLOCK)
--	where custmr_id = @customer_id
--	and prem_adj_id = @premium_adjustment_id
--	and prem_adj_perd_id = @premium_adj_period_id
	if(@delete_ilrf=1)
	begin
	delete from dbo.PREM_ADJ_LOS_REIM_FUND_POST WITH (ROWLOCK)
	where custmr_id = @customer_id
	and prem_adj_id = @premium_adjustment_id
	and prem_adj_perd_id = @premium_adj_period_id
	
	--Texas Tax:Deleting the tbales related to Texas Tax in Recalculation
	delete from dbo.PREM_ADJ_LOS_REIM_FUND_POST_TAX WITH (ROWLOCK)
	where custmr_id = @customer_id
	and prem_adj_id = @premium_adjustment_id
	and prem_adj_perd_id = @premium_adj_period_id
	and (dedtbl_tax_cmpnt_id in(535,536,537,538) OR dedtbl_tax_cmpnt_id is null)
	
	end


	if(@delete_chf=1)
	begin

	--Texas Tax:Deleting the tbales related to Texas Tax in Recalculation for CHF component
	delete from dbo.PREM_ADJ_LOS_REIM_FUND_POST_TAX WITH (ROWLOCK)
	where custmr_id = @customer_id
	and prem_adj_id = @premium_adjustment_id
	and prem_adj_perd_id = @premium_adj_period_id
	and dedtbl_tax_cmpnt_id=(select lkup_id from dbo.LKUP where lkup_typ_id=54 and lkup_txt='CHF')

	end
	
	if (@delete_plb = 1)
	begin
		delete from dbo.PREM_ADJ_PAID_LOS_BIL WITH (ROWLOCK)
		where custmr_id = @customer_id
		and prem_adj_id = @premium_adjustment_id
		and prem_adj_perd_id = @premium_adj_period_id
	end

--	delete from dbo.PREM_ADJ_MISC_INVC WITH (ROWLOCK)
--	where custmr_id = @customer_id
--	and prem_adj_id = @premium_adjustment_id
--	and prem_adj_perd_id = @premium_adj_period_id

--	delete from dbo.PREM_ADJ_CMMNT WITH (ROWLOCK)
--	where custmr_id = @customer_id
--	and prem_adj_id = @premium_adjustment_id
--	and prem_adj_perd_id = @premium_adj_period_id

--	delete from dbo.PREM_ADJ_NY_SCND_INJR_FUND WITH (ROWLOCK)
--	where custmr_id = @customer_id
--	and prem_adj_id = @premium_adjustment_id
--	and prem_adj_perd_id = @premium_adj_period_id

	delete from dbo.PREM_ADJ_PERD_TOT WITH (ROWLOCK)
	where custmr_id = @customer_id
	and prem_adj_id = @premium_adjustment_id
	and prem_adj_perd_id = @premium_adj_period_id
	
	/**************************************************
	Surcharges and Assesments: Added below statements 
	as part of the AIS 21 surcharges project
	***************************************************/
	delete from dbo.PREM_ADJ_SURCHRG_DTL WITH (ROWLOCK)
	where custmr_id = @customer_id
	and prem_adj_id = @premium_adjustment_id
	and prem_adj_perd_id = @premium_adj_period_id

	delete from dbo.PREM_ADJ_RETRO_DTL WITH (ROWLOCK)
	where custmr_id = @customer_id
	and prem_adj_id = @premium_adjustment_id
	and prem_adj_perd_id = @premium_adj_period_id

	delete from dbo.PREM_ADJ_RETRO WITH (ROWLOCK)
	where custmr_id = @customer_id
	and prem_adj_id = @premium_adjustment_id
	and prem_adj_perd_id = @premium_adj_period_id



	--clear Premium Adjustment Period table after clearing results output table
	-- This delete operation being performed in the driver
--	delete from dbo.PREM_ADJ_PERD WITH (ROWLOCK)
--	where custmr_id = @customer_id
--	and prem_adj_id = @premium_adjustment_id
--	and prem_adj_perd_id = @premium_adj_period_id

	--clear Premium Adjustment Status table after clearing results output table
--	delete from dbo.PREM_ADJ_STS WITH (ROWLOCK)
--	where custmr_id = @customer_id
--	and prem_adj_id = @premium_adjustment_id

	--clear Premium Adjustment table after clearing results output table
--	if not exists(select * from dbo.PREM_ADJ_PERD where custmr_id = @customer_id and prem_adj_id = @premium_adjustment_id )
--	begin
--		delete from dbo.PREM_ADJ WITH (ROWLOCK)
--		where reg_custmr_id = @customer_id
--		and prem_adj_id = @premium_adjustment_id
--	end
	
	--Texas Tax:Deleting the tbales related to Texas Tax in Recalculation
	delete from dbo.PREM_ADJ_TAX_DTL WITH (ROWLOCK)
	where custmr_id = @customer_id
	and prem_adj_id = @premium_adjustment_id
	and prem_adj_perd_id = @premium_adj_period_id

	delete from dbo.PREM_ADJ_TAX_SETUP WITH (ROWLOCK)
	where custmr_id = @customer_id
	and prem_adj_id = @premium_adjustment_id
	and prem_adj_perd_id = @premium_adj_period_id


	if @trancount = 0
		commit transaction

end try
begin catch

	if @trancount = 0  
	begin
		rollback transaction
	end

	declare @err_msg varchar(500),@err_ln varchar(10),
			@err_proc varchar(30),@err_no varchar(10)
	select  @err_msg = error_message(),
		    @err_no = error_number(),
		    @err_proc = error_procedure(),
			@err_ln = error_line()
	set @err_msg = '- error no.:' + isnull(@err_no,'') + '; procedure:' 
		+ isnull(@err_proc,'') + ';error line:' + isnull(@err_ln,'') + ';description:' + isnull(@err_msg,'') 

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
		'AIS Calculation Engine'
       ,'ERR'
       ,'Calculation error'
       ,'Error encountered during calculation of adjustment number: ' 
			+ convert(varchar(20),isnull(@premium_adjustment_id,0)) 
			+ ' for adjustment period number: ' 
			+ convert(varchar(20),isnull(@premium_adj_period_id,0)) 
			+ ' associated with customer number: ' 
			+ convert(varchar(20),isnull(@customer_id,0))
			+ '. Error message: '
			+ @err_msg
	   ,@customer_id
	   ,@premium_adjustment_id
       ,1
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

end catch


end
GO


if object_id('PrgAISCalcResults') is not null
	print 'Created Procedure PrgAISCalcResults'
else
	print 'Failed Creating Procedure PrgAISCalcResults'
go

if object_id('PrgAISCalcResults') is not null
	grant exec on PrgAISCalcResults to public
go

if exists (select 1 from sysobjects where name = 'ModAISCalcCHF' and type = 'P')
	drop procedure ModAISCalcCHF
go

set ansi_nulls off
go

---------------------------------------------------------------------
-----
-----	Proc Name:	ModAISCalcCHF
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	This stored procedure is used to insert CHF calculation results.
-----
-----	On Exit:	
-----			
-----
-----	Modified:	MM/DD/YY	first & last name of modifier 
-----			- Description of Modification
-----               02/02/2009 venkat kolimi
-----               inserted prem_adj_pgm_id into dbo.[PREM_ADJ_PARMET_DTL] table as it is not null field
-----			10/07/15	venkat kolimi
-----			Policy id added in Group by
---------------------------------------------------------------------

CREATE procedure [dbo].[ModAISCalcCHF] 
@premium_adj_period_id int,
@premium_adjustment_id int,
@customer_id int,
@premium_adj_prog_id int,
@updt_user_id int,
@create_user_id int,
@err_msg_op varchar(1000) output
as

begin
	set nocount on

declare @iden int,
		@count int,
		@counter int,
		@outer_count int,
		@outer_counter int,
		@cnt_prev_adjs int,
		@prev_valid_adj_id int,
		@setup_id int,
		@dep_amt decimal(15,2),
		@chf_prev_bil_amt decimal(15,2),
		@months_to_val int,
		@months_elapsed smallint,
		@fst_adj_dt datetime,
		@next_val_date datetime,
		@first_adj int,
		@freq smallint,
		@pgm_setup_id int,
		@incl_in_erp bit,
		@prem_adj_valn_dt datetime,
		@pgm_period_valn_dt datetime,
		@err_message varchar(500),
		@trancount int


--Check if CHF calc needs to be performed for this adjustment
select 
@prem_adj_valn_dt = valn_dt
from dbo.PREM_ADJ
where prem_adj_id = @premium_adjustment_id

select 
@pgm_period_valn_dt = nxt_valn_dt_non_prem_dt
from dbo.PREM_ADJ_PGM
where prem_adj_pgm_id = @premium_adj_prog_id

print 'Before CHF valuation date validation'

if (@prem_adj_valn_dt <> @pgm_period_valn_dt)
begin
				set @err_message = 'CHF: Valuation date for this adjustment and the valuation date specified in the program period setup do not match' 
				+ ';customer ID: ' + convert(varchar(20),@customer_id) 
				+ ';Program Period ID: ' + convert(varchar(20),@premium_adj_prog_id) 
				set @err_msg_op = @err_message
				exec [dbo].[AddAPLCTN_STS_LOG] 
					@premium_adjustment_id = @premium_adjustment_id,
					@customer_id = @customer_id,
					@premium_adj_prog_id = @premium_adj_prog_id,
					@err_msg = @err_message,
					@create_user_id = @create_user_id

	return
end

print 'CHF: valuation date validation PASSED; START OF CALC'


set @trancount = @@trancount;
--print @trancount
if @trancount >= 1
    save transaction ModAISCalcCHF
else
    begin transaction
	
begin try


/**************************
* Determine first adjustment
**************************/

exec @cnt_prev_adjs = [dbo].[fn_DetermineFirstAdjustment_ParmetType]
	@premium_adj_prog_id = @premium_adj_prog_id,
	@adj_parmet_typ_id = 398 -- Adj. Parameter type ID for ADJUSTMENT PARAMETER TYPE -> CHF


create table #pgm_setups
(
id int identity(1,1),
pgm_setup_id int,
incl_in_erp bit
)


create index ind ON #pgm_setups (id)

insert into #pgm_setups(pgm_setup_id,incl_in_erp)
select 
prem_adj_pgm_setup_id,
incld_ernd_retro_prem_ind
from dbo.PREM_ADJ_PGM_SETUP
where custmr_id = @customer_id
and prem_adj_pgm_id = @premium_adj_prog_id
and adj_parmet_typ_id = 398 -- Adj. Parameter type ID for ADJUSTMENT PARAMETER TYPE -> CHF

set @outer_counter = 1
select @outer_count = count(*) from #pgm_setups

while @outer_counter <= @outer_count
begin

select 
@pgm_setup_id = pgm_setup_id,
@incl_in_erp = incl_in_erp
from #pgm_setups 
where id = @outer_counter

print' @pgm_setup_id:- ' + convert(varchar(20), @pgm_setup_id)  
print' @incl_in_erp:- ' + convert(varchar(20), @incl_in_erp)  

if @incl_in_erp = 0 -- Not included in ERP
begin
	if (@cnt_prev_adjs = 0) -- No existing adjustments; this is initial adjustment
	begin
	print 'initial adjustment'
		/*************************************************************
		* Additional check required for program period setup ->FIRST ADJ NON-PREMIUM 
		* before initiating LBA calc. Only if inception to VAL DATE is 
		* more than this value, intiate LBA calc.
		*************************************************************/
		--select
		--@fst_adj_dt = fst_adj_non_prem_dt,
		--@next_val_date = nxt_valn_dt_non_prem_dt 
		--from dbo.PREM_ADJ_PGM 
		--where prem_adj_pgm_id = @premium_adj_prog_id

		--if (@fst_adj_dt <> @next_val_date)
		--begin
			--set @err_message = 'CHF: First Adjustment Date(NP) is not equal to Next Valuation Date(NP)' 
			--+ ';customer ID: ' + convert(varchar(20),@customer_id) 
			--+ ';Program Period ID: ' + convert(varchar(20),@premium_adj_prog_id) 
			--rollback transaction ModAISCalcCHF
			--set @err_msg_op = @err_message
			--exec [dbo].[AddAPLCTN_STS_LOG] 
				--@premium_adjustment_id = @premium_adjustment_id,
				--@customer_id = @customer_id,
				--@premium_adj_prog_id = @premium_adj_prog_id,
				--@err_msg = @err_message,
				--@create_user_id = @create_user_id
		--	return
	--	end

	end
	else
	begin -- Subsequent adjustment / this is not initial adjustment
		select @months_elapsed = datediff(mm,prev_valn_dt_non_prem_dt,nxt_valn_dt_non_prem_dt), 
			   @freq = freq_non_prem_mms_cnt -- Non-premium frequency
		from dbo.PREM_ADJ_PGM 
		where prem_adj_pgm_id = @premium_adj_prog_id

		if @months_elapsed <> @freq
		begin
			set @err_message = 'CHF: Difference between Next Valuation Date(NP) and Previous Valuation Date(NP) is not consistent with frequency for CHF'
			+ ';customer ID: ' + convert(varchar(20),@customer_id) 
			+ ';Program Period ID: ' + convert(varchar(20),@premium_adj_prog_id) 
			rollback transaction ModAISCalcCHF
			set @err_msg_op = @err_message
			exec [dbo].[AddAPLCTN_STS_LOG] 
				@premium_adjustment_id = @premium_adjustment_id,
				@customer_id = @customer_id,
				@premium_adj_prog_id = @premium_adj_prog_id,
				@err_msg = @err_message,
				@create_user_id = @create_user_id
			return
		end

	end

	select @next_val_date = nxt_valn_dt_non_prem_dt
	from dbo.PREM_ADJ_PGM 
	where prem_adj_pgm_id = @premium_adj_prog_id

	if(getdate() < @next_val_date)
	begin
		set @err_message = 'CHF: Current date is less than the Next Valuation Date(NP)'
		+ ';customer ID: ' + convert(varchar(20),@customer_id) 
		+ ';Program Period ID: ' + convert(varchar(20),@premium_adj_prog_id) 
		rollback transaction ModAISCalcCHF
		set @err_msg_op = @err_message
		exec [dbo].[AddAPLCTN_STS_LOG] 
			@premium_adjustment_id = @premium_adjustment_id,
			@customer_id = @customer_id,
			@premium_adj_prog_id = @premium_adj_prog_id,
			@err_msg = @err_message,
			@create_user_id = @create_user_id
		return
	end
end -- end of: if @incl_in_erp = 0
else if @incl_in_erp = 1 -- Included in ERP
begin
	if (@cnt_prev_adjs = 0) -- No existing adjustments; this is initial adjustment
	begin
	print 'initial adjustment check'
		/*************************************************************
		* Additional check required for program period setup ->FIRST ADJ NON-PREMIUM 
		* before initiating LBA calc. Only if inception to VAL DATE is 
		* more than this value, intiate LBA calc.
		*************************************************************/
		--select
		--@fst_adj_dt = fst_adj_dt,
		--@next_val_date = nxt_valn_dt 
		--from dbo.PREM_ADJ_PGM 
		--where prem_adj_pgm_id = @premium_adj_prog_id

		--if (@fst_adj_dt <> @next_val_date)
		--begin
			--set @err_message = 'CHF: First Adjustment Date(P) is not equal to Next Valuation Date(P)' 
			--+ ';customer ID: ' + convert(varchar(20),@customer_id) 
			--+ ';Program Period ID: ' + convert(varchar(20),@premium_adj_prog_id) 
			--rollback transaction ModAISCalcCHF
			--set @err_msg_op = @err_message
			--exec [dbo].[AddAPLCTN_STS_LOG] 
				--@premium_adjustment_id = @premium_adjustment_id,
				--@customer_id = @customer_id,
				--@premium_adj_prog_id = @premium_adj_prog_id,
				--@err_msg = @err_message,
				--@create_user_id = @create_user_id
			--return
		--end

	end
	else
	begin -- Subsequent adjustment / this is not initial adjustment
		select @months_elapsed = datediff(mm,prev_valn_dt,nxt_valn_dt), 
			   @freq = adj_freq_mms_intvrl_cnt -- Premium frequency
		from dbo.PREM_ADJ_PGM 
		where prem_adj_pgm_id = @premium_adj_prog_id

		if @months_elapsed <> @freq
		begin
			set @err_message = 'CHF: Difference between Next Valuation Date(P) and Previous Valuation Date(P) is not consistent with frequency for CHF'
			+ ';customer ID: ' + convert(varchar(20),@customer_id) 
			+ ';Program Period ID: ' + convert(varchar(20),@premium_adj_prog_id) 
			rollback transaction ModAISCalcCHF
			set @err_msg_op = @err_message
			exec [dbo].[AddAPLCTN_STS_LOG] 
				@premium_adjustment_id = @premium_adjustment_id,
				@customer_id = @customer_id,
				@premium_adj_prog_id = @premium_adj_prog_id,
				@err_msg = @err_message,
				@create_user_id = @create_user_id
			return
		end

	end

	select @next_val_date = nxt_valn_dt
	from dbo.PREM_ADJ_PGM 
	where prem_adj_pgm_id = @premium_adj_prog_id

	if(getdate() < @next_val_date)
	begin
		set @err_message = 'CHF: Current date is less than the Next Valuation Date(P)'
		+ ';customer ID: ' + convert(varchar(20),@customer_id) 
		+ ';Program Period ID: ' + convert(varchar(20),@premium_adj_prog_id) 
		rollback transaction ModAISCalcCHF
		set @err_msg_op = @err_message
		exec [dbo].[AddAPLCTN_STS_LOG] 
			@premium_adjustment_id = @premium_adjustment_id,
			@customer_id = @customer_id,
			@premium_adj_prog_id = @premium_adj_prog_id,
			@err_msg = @err_message,
			@create_user_id = @create_user_id
		return
	end

end --end of: if @incl_in_erp = 1 


		create table #t_adj_setup
		(
			[id] [int] identity(1,1),
			[prem_adj_perd_id] [int],
			[prem_adj_id] [int] ,
			[prem_adj_pgm_setup_id] [int],
			[prem_adj_pgm_id] [int],
			[custmr_id] [int] ,
			[prem_adj_cmmnt_id] [int] ,
			[tot_amt] [decimal](15, 2) ,
			[adj_parmet_typ_id] [int] ,
			[updt_user_id] [int] ,
			[updt_dt] [datetime] ,
			[crte_user_id] [int] ,
			[crte_dt] [datetime] 
		)

		insert into #t_adj_setup
        (
			[prem_adj_perd_id]
		   ,[prem_adj_id]
		   ,[prem_adj_pgm_setup_id]
		   ,[prem_adj_pgm_id]
		   ,[custmr_id]
		   ,[prem_adj_cmmnt_id]
		   ,[tot_amt]
		   ,[adj_parmet_typ_id]
		   ,[updt_user_id]
		   ,[updt_dt]
		   ,[crte_user_id]
		   ,[crte_dt]
		)
		select 
		@premium_adj_period_id,
		@premium_adjustment_id,
		s.prem_adj_pgm_setup_id,
		s.prem_adj_pgm_id,
		s.custmr_id,
		NULL,  
		sum(dbo.fn_ComputeCHFAmount(isnull(sdtl.clm_hndl_fee_clmt_nbr,0) , isnull(sdtl.clm_hndl_fee_clm_rt_nbr,0))) as chf_amt,
		398, -- Adjustment Parameter Type ID for CHF
		@updt_user_id,
		getdate(),
		@create_user_id,
		getdate()
		from dbo.PREM_ADJ_PGM_SETUP s  
		inner join dbo.PREM_ADJ_PGM_DTL sdtl on (s.prem_adj_pgm_setup_id = sdtl.prem_adj_pgm_setup_id) and (s.custmr_id = sdtl.custmr_id) and (s.prem_adj_pgm_id = sdtl.prem_adj_pgm_id)
		where s.custmr_id = @customer_id
		and s.prem_adj_pgm_id = @premium_adj_prog_id
		and s.actv_ind = 1 
		and sdtl.actv_ind = 1  
		and s.adj_parmet_typ_id = 398 -- Adj. Parameter type ID for ADJUSTMENT PARAMETER TYPE -> CHF
		and s.prem_adj_pgm_setup_id = @pgm_setup_id
		group by s.custmr_id, s.prem_adj_pgm_id, s.prem_adj_pgm_setup_id --sdtl.prem_adj_pgm_dtl_id

		--select * FROM #t_adj_setup
		set @counter = 1

		select @count = count(*) from #t_adj_setup
		print @count

		while @counter <= @count
			begin
				print @counter
				select 
				@setup_id = prem_adj_pgm_setup_id 
				from #t_adj_setup 
				where id = @counter

				--insert into the header output table
				insert into dbo.[PREM_ADJ_PARMET_SETUP]
				(
					[prem_adj_perd_id]
				   ,[prem_adj_id]
				   ,[prem_adj_pgm_setup_id]
				   ,[prem_adj_pgm_id]
				   ,[custmr_id]
				   --,[prem_adj_cmmnt_id]
				   ,[tot_amt]
				   ,[adj_parmet_typ_id]
				   ,[updt_user_id]
				   ,[updt_dt]
				   ,[crte_user_id]
				   ,[crte_dt]
				)
				select
					[prem_adj_perd_id]
				   ,[prem_adj_id]
				   ,[prem_adj_pgm_setup_id]
				   ,[prem_adj_pgm_id]
				   ,[custmr_id]
				   --,[prem_adj_cmmnt_id]
				   ,[tot_amt]
				   ,[adj_parmet_typ_id]
				   ,[updt_user_id]
				   ,[updt_dt]
				   ,[crte_user_id]
				   ,[crte_dt]
				from #t_adj_setup where id = @counter



				select @iden = @@identity

				--insert into the details table
				-- may need tweaking to specify policy info
				insert into dbo.[PREM_ADJ_PARMET_DTL]
				(
					[prem_adj_parmet_setup_id]
					,[prem_adj_perd_id]
					,[prem_adj_id]
					,[custmr_id]
					 --added by venkat
					,[prem_adj_pgm_id]
					,[coml_agmt_id]
					,[ln_of_bsn_id]
					,[st_id]
					,[ssst_st_id]
					,[ssst_amt]
					,[clm_hndl_fee_los_typ_id]
					,[clm_hndl_fee_clmt_nbr]
					,[clm_hndl_fee_clm_rt_nbr]
					,[tot_amt]
					,[updt_user_id]
					,[updt_dt]
					,[crte_user_id]
					,[crte_dt]
				
				)
				select 
				@iden,
				@premium_adj_period_id,
				@premium_adjustment_id,
				@customer_id,
				--added by venkat
                sdtl.prem_adj_pgm_id,
				ap.coml_agmt_id,
				[dbo].[fn_GetLOB](ap.coml_agmt_id),
				sdtl.st_id,
				sdtl.ssst_st_id,
				sdtl.ssst_amt,
				sdtl.clm_hndl_fee_los_typ_id,
				isnull(sdtl.clm_hndl_fee_clmt_nbr,0),
				isnull(sdtl.clm_hndl_fee_clm_rt_nbr,0),
				dbo.fn_ComputeCHFAmount(isnull(sdtl.clm_hndl_fee_clmt_nbr,0) , isnull(sdtl.clm_hndl_fee_clm_rt_nbr,0)) as chf_computed_by_fn,
				@updt_user_id,
				getdate(),
				@create_user_id,
				getdate()
				from dbo.PREM_ADJ_PGM_SETUP s  
				inner join dbo.PREM_ADJ_PGM_DTL sdtl on (s.prem_adj_pgm_setup_id = sdtl.prem_adj_pgm_setup_id) and (s.custmr_id = sdtl.custmr_id) and (s.prem_adj_pgm_id = sdtl.prem_adj_pgm_id)
				inner join PREM_ADJ_PGM_SETUP_POL ap on (s.prem_adj_pgm_setup_id = ap.prem_adj_pgm_setup_id ) and  (s.prem_adj_pgm_id = ap.prem_adj_pgm_id)  and (sdtl.prem_adj_pgm_setup_pol_id=ap.prem_adj_pgm_setup_pol_id)
				where s.custmr_id = @customer_id
				and s.prem_adj_pgm_id = @premium_adj_prog_id
				and s.actv_ind = 1 
				and sdtl.actv_ind = 1  
				and s.adj_parmet_typ_id = 398 -- Adj. Parameter type ID for ADJUSTMENT PARAMET CHF
				and s.prem_adj_pgm_setup_id = @setup_id
				
				set @counter = @counter + 1
			end --while loop
		drop table #t_adj_setup
	


		/*******************************************
		* Give credits for previously billed amounts
		********************************************/


		select @dep_amt = aps.depst_amt
		from 
		dbo.PREM_ADJ_PARMET_SETUP pas
		inner join dbo.PREM_ADJ_PGM_SETUP aps on aps.prem_adj_pgm_setup_id = pas.prem_adj_pgm_setup_id
		where
		pas.prem_adj_perd_id = @premium_adj_period_id
		and pas.prem_adj_id = @premium_adjustment_id
		and aps.incld_ernd_retro_prem_ind = 0
		and aps.adj_parmet_typ_id = 398 -- Adj. Parameter type ID for ADJUSTMENT PARAMET CHF

		if (@cnt_prev_adjs = 0) -- No existing adjustments this is initial adjustment
		begin
			set @dep_amt = isnull(@dep_amt,0)
		end
		else
		begin -- This is not initial adjustment
			set @dep_amt = 0
		end

	
		--retrieve amounts from the previous adjustment
		exec @prev_valid_adj_id = [dbo].[fn_DeterminePrevValidAdj_NonPremium]
 			@current_premium_adjustment_id = @premium_adjustment_id,
			@customer_id = @customer_id,
			@premium_adj_prog_id = @premium_adj_prog_id,
			@adj_parmet_typ_id = 398 -- Adj. Parameter type ID for ADJUSTMENT PARAMET CHF

		--Eliminate the policy level duplicate tot_amt
		select distinct  
		d.prem_adj_id,
		d.prem_adj_pgm_id,
		d.clm_hndl_fee_los_typ_id,
		d.st_id,
		d.tot_amt,
		d.ssst_st_id 
		into #Prior_PREM_ADJ_PARMET_DTL
		from 
		dbo.PREM_ADJ_PARMET_SETUP stp
		inner join dbo.PREM_ADJ_PARMET_DTL d on (d.prem_adj_parmet_setup_id = stp.prem_adj_parmet_setup_id) and (d.prem_adj_perd_id = stp.prem_adj_perd_id) and (d.prem_adj_id = stp.prem_adj_id)
		inner join dbo.PREM_ADJ_PERD prd on (stp.prem_adj_perd_id = prd.prem_adj_perd_id) and (stp.prem_adj_id = prd.prem_adj_id)
		inner join dbo.PREM_ADJ_PGM_SETUP adp on (stp.prem_adj_pgm_setup_id = adp.prem_adj_pgm_setup_id)
		where stp.prem_adj_id = @prev_valid_adj_id
		and prd.prem_adj_pgm_id = @premium_adj_prog_id
		and stp.adj_parmet_typ_id = 398 
		and adp.incld_ernd_retro_prem_ind = 0

		select @chf_prev_bil_amt = isnull(sum( d.tot_amt ),0) + isnull(@dep_amt,0) 
		from 
		dbo.#Prior_PREM_ADJ_PARMET_DTL d
		
		drop table #Prior_PREM_ADJ_PARMET_DTL

		print '@chf_prev_bil_amt: ' + convert(varchar(20), @chf_prev_bil_amt)
		print '@dep_amt: ' + convert(varchar(20), @dep_amt)

--		update dbo.PREM_ADJ_PARMET_SETUP WITH (ROWLOCK)
--		set clm_hndl_fee_prev_biled_amt = isnull(@chf_prev_bil_amt,0),
--			--los_base_asses_depst_amt = isnull(@dep_amt,0),
--			tot_amt = tot_amt - isnull(@dep_amt,0) - isnull(@chf_prev_bil_amt,0)
--		from 
--		dbo.PREM_ADJ_PARMET_SETUP pas
--		inner join dbo.PREM_ADJ_PGM_SETUP aps on aps.prem_adj_pgm_setup_id = pas.prem_adj_pgm_setup_id
--		where
--		pas.prem_adj_perd_id = @premium_adj_period_id
--		and prem_adj_id = @premium_adjustment_id
--		and aps.incld_ernd_retro_prem_ind = 0
--		and aps.adj_parmet_typ_id = 398 -- Adj. Parameter type ID for ADJUSTMENT PARAMET CHF
--		and pas.adj_parmet_typ_id = 398 -- Adj. Parameter type ID for ADJUSTMENT PARAMET CHF

		print '@pgm_setup_id:' + convert(varchar(20),@pgm_setup_id)
		print '---END pgm_setup while loop--------'
		

		set @outer_counter = @outer_counter + 1
end --end of pgm_setup while loop


		--update dbo.PREM_ADJ_PARMET_SETUP WITH (ROWLOCK)
		--set clm_hndl_fee_prev_biled_amt = isnull(@chf_prev_bil_amt,0),
		--	--los_base_asses_depst_amt = isnull(@dep_amt,0),
		--	tot_amt = tot_amt - isnull(@dep_amt,0) - isnull(@chf_prev_bil_amt,0)
		--from 
		--dbo.PREM_ADJ_PARMET_SETUP pas
		--inner join dbo.PREM_ADJ_PGM_SETUP aps on aps.prem_adj_pgm_setup_id = pas.prem_adj_pgm_setup_id
		--where
		--pas.prem_adj_perd_id = @premium_adj_period_id
		--and prem_adj_id = @premium_adjustment_id
		--and aps.incld_ernd_retro_prem_ind = 0
		--and aps.adj_parmet_typ_id = 398 -- Adj. Parameter type ID for ADJUSTMENT PARAMET CHF
		--and pas.adj_parmet_typ_id = 398 -- Adj. Parameter type ID for ADJUSTMENT PARAMET CHF

		update dbo.PREM_ADJ_PARMET_SETUP WITH (ROWLOCK)
		set clm_hndl_fee_prev_biled_amt = isnull(@chf_prev_bil_amt,0),
			tot_amt = tot_amt- (CASE [dbo].[fn_CheckFirstAdjustment_ParmetType](aps.PREM_ADJ_PGM_ID,398) 
								WHEN 0 THEN 0 ELSE isnull(@dep_amt,0) END) - isnull(@chf_prev_bil_amt,0)

		from 
		dbo.PREM_ADJ_PARMET_SETUP pas
		inner join dbo.PREM_ADJ_PGM_SETUP aps on aps.prem_adj_pgm_setup_id = pas.prem_adj_pgm_setup_id
		where
		pas.prem_adj_perd_id = @premium_adj_period_id
		and prem_adj_id = @premium_adjustment_id
		and aps.incld_ernd_retro_prem_ind = 0
		and aps.adj_parmet_typ_id = 398 -- Adj. Parameter type ID for ADJUSTMENT PARAMET CHF
		and pas.adj_parmet_typ_id = 398 -- Adj. Parameter type ID for ADJUSTMENT PARAMET CHF
	
		if @trancount = 0
			commit transaction 
end try
begin catch

	if @trancount = 0
	begin
		rollback transaction
	end
	else
	begin
		rollback transaction ModAISCalcCHF
	end

	
	declare @err_msg varchar(500),@err_ln varchar(10),
			@err_proc varchar(30),@err_no varchar(10)
	select  @err_msg = error_message(),
		    @err_no = error_number(),
		    @err_proc = error_procedure(),
			@err_ln = error_line()
	set @err_msg = '- error no.:' + isnull(@err_no, ' ') + '; procedure:' 
		+ isnull(@err_proc, ' ') + ';error line:' + isnull(@err_ln, ' ') + ';description:' + isnull(@err_msg, ' ' )
	set @err_msg_op = @err_msg

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
		'AIS Calculation Engine'
       ,'ERR'
       ,'Calculation error'
       ,'Error encountered during calculation of adjustment number: ' 
			+ convert(varchar(20),isnull(@premium_adjustment_id,0)) 
			+ ' for program number: ' 
			+ convert(varchar(20),isnull(@premium_adj_prog_id,0)) 
			+ ' associated with customer number: ' 
			+ convert(varchar(20),isnull(@customer_id,0))
			+ '. Error message: '
			+ isnull(@err_msg, ' ')
	   ,@customer_id
	   ,@premium_adjustment_id
       ,isnull(@create_user_id, 0)
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
end catch

end

GO


if object_id('ModAISCalcCHF') is not null
	print 'Created Procedure ModAISCalcCHF'
else
	print 'Failed Creating Procedure ModAISCalcCHF'
go

if object_id('ModAISCalcCHF') is not null
	grant exec on ModAISCalcCHF to public
go

if exists (select 1 from sysobjects where name = 'ModAdjRevisionDriver' and type = 'P')
           drop procedure ModAdjRevisionDriver
go
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER OFF
GO
---------------------------------------------------------------------
-----
-----	Proc Name:	ModAdjRevisionDriver
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	Driver for the Revision process
-----	
-----	On Exit:
-----	
-----
-----	Modified:	11/01/2008  Development Team
-----                   Created SP
-----	Modified:	05/13/2008  Siva Kumar Thangaraj
-----                   Solved the deadlock issue while concurrent user are accessing
-----   Modified:	 09/26/2010 Venkat Kolimi	
-----				 As per the TFS Bug 12801, we corrected the prior prem_adj_id logic
-----
-----               07/19/2010 --Venkat Kolimi
-----               Added the statements related to the AIS 21 surcharges project
-----
-----   Modified:	 09/26/2010 Venkat Kolimi	
-----				 As per the TFS Bug 12801, we corrected the prior prem_adj_id logic
-----

---------------------------------------------------------------------
CREATE procedure [dbo].[ModAdjRevisionDriver]
      @select         smallint = 0, 
      @prem_adj_id    int,
      @custmr_id int,
      @User_id int,
	  @err_msg_output varchar(1000) output


as
declare @error      int,
		@trancount  int,
		@New_Prem_Adj_id int,
		@historical_adj_ind int,
		@New_Adj_id int,
		@err_msg_check_key_parameters_output varchar(1000),
		@tryProc int

set @tryProc = 2
set deadlock_priority low

while(@tryProc>0)
begin

	select    @trancount  = @@trancount

	if @trancount = 0 
		begin
			begin transaction 
		end

	begin try
		---------------------- deleting prior Losses
		delete from ARMIS_LOS_EXC with(rowlock) where armis_los_pol_id
		in (
		select armis_los_pol_id from ARMIS_LOS_POL alp
		join
		(select perd.prem_adj_pgm_id, adj.Valn_dt from Prem_adj_perd perd,
		Prem_adj adj where adj.prem_adj_id=perd.Prem_adj_id and adj.Prem_adj_id=@prem_adj_id) prm
		on alp.prem_adj_pgm_id = prm.prem_adj_pgm_id and alp.valn_dt = prm.valn_dt
		and alp.prem_adj_id is null
		)

		delete from ARMIS_LOS_POL with(rowlock) where armis_los_pol_id in
		(select armis_los_pol_id from ARMIS_LOS_POL alp
		join
		(select perd.prem_adj_pgm_id, adj.Valn_dt from Prem_adj_perd perd,
		Prem_adj adj where adj.prem_adj_id=perd.Prem_adj_id and adj.Prem_adj_id=@prem_adj_id) prm
		on alp.prem_adj_pgm_id = prm.prem_adj_pgm_id and alp.valn_dt = prm.valn_dt
		and alp.prem_adj_id is null)
		---------------------------------------------

		-- step 1. Make a copy of adjustment and losses tables
		-- step 2. For the “new” adjustment record, set Related Premium Adjustment Id field in the Premium Adjustment table to the Primary Key of the revised adjustment record.
		exec ModAdjCpy   @select=0, @prem_adj_id =  @prem_adj_id ,@New_Adj_id=@New_Prem_Adj_id output

		-- step 1. Reset values not applicable to new adjustment record.
		update PREM_ADJ WITH (ROWLOCK)
		set adj_can_ind=0,
			adj_void_ind=0,
			adj_rrsn_ind = 0,
			adj_pendg_ind = 0,
			--historical_adj_ind=0,
			adj_void_rsn_id = null,
			adj_rrsn_rsn_id = null,
			adj_pendg_rsn_id = null,
			drft_invc_nbr_txt = null, 
			drft_invc_dt = null,
			invc_due_dt= null,
			calc_adj_sts_cd=null,
			drft_mailed_undrwrt_dt = null,
			drft_intrnl_pdf_zdw_key_txt = null,
			drft_extrnl_pdf_zdw_key_txt = null,
			drft_cd_wrksht_pdf_zdw_key_txt = null,
			fnl_invc_nbr_txt = null,  
			fnl_invc_dt = null,
			fnl_mailed_undrwrt_dt = null,
			fnl_intrnl_pdf_zdw_key_txt = null,
			fnl_extrnl_pdf_zdw_key_txt = null,
			fnl_cd_wrksht_pdf_zdw_key_txt = null,
			fnl_mailed_brkr_dt   = null,
			twenty_pct_qlty_cntrl_ind = null,
			twenty_pct_qlty_cntrl_pers_id = null,
			twenty_pct_qlty_cntrl_dt = null,
			adj_sts_typ_id = 346,   -- Set adjustment status to "CALC",
			adj_sts_eff_dt = getdate( ),
			twenty_pct_qlty_cntrl_reqr_ind = NULL,
			reconciler_revw_ind = 0,
			adj_qc_ind = 0,
			updt_user_id = @User_id,
			updt_dt = getdate( )
		where prem_adj_id = @New_Prem_Adj_id
		
		-- step 3. For the revised adjustment record, set the Adjustment Revision Indicator field in Premium Adjustment  table to true.
		update PREM_ADJ	WITH (ROWLOCK)
		set adj_rrsn_ind = 1, updt_user_id = @User_id, updt_dt = getdate()
		where prem_adj_id = @prem_adj_id



	--if not exists (select prem_adj_id
	--			from prem_adj
	--			where rel_prem_adj_id = @prem_adj_id 
	--			and	  adj_can_ind = 1)
	--begin		

		-- step 5. Identify the Previous Adjustment Number
		-- Need to locate the prior adjusment for each program period 
		-- This only applies for premium dates
		-- First, set the prior_prem_adj_id to null for all adjustment's program period
		-- Then find the prior adjustment id for each program periods
		-- Program period who end up with a null prior_prem_adj_id indicates that this is the 
		-- first adjustment for the program period.
		update PREM_ADJ_PGM WITH (ROWLOCK)
		set	prior_prem_adj_id = null
		from(
				SELECT prem_adj_pgm_id, prem_adj_id	FROM PREM_ADJ_PERD 
				WHERE PREM_ADJ_ID = @prem_adj_id and reg_custmr_id = @custmr_id)
		as t3
		inner join PREM_ADJ_PGM pag on ( t3.prem_adj_pgm_id = pag.prem_adj_pgm_id)
		and pag.actv_ind = 1

		/*----------------------------------------------------
		                  12801 Bug Fix
		----------------------------------------------------*/		
		update PREM_ADJ_PGM WITH (ROWLOCK)
		set	prior_prem_adj_id = t3.prem_adj_id
		from (
		select t1.prem_adj_id, t1.prem_adj_pgm_id
		from
			(	SELECT MAX(PREM_ADJ_ID) AS PREM_ADJ_ID, prem_adj_pgm_id, reg_custmr_id	FROM PREM_ADJ_PERD 
					WHERE PREM_ADJ_ID < @prem_adj_id and reg_custmr_id = @custmr_id
				and (prem_non_prem_cd = 'B' or prem_non_prem_cd = 'P' OR prem_non_prem_cd is null)
				and prem_adj_pgm_id in (select prem_adj_pgm_id from prem_adj_perd where PREM_ADJ_ID = @prem_adj_id )
				and prem_adj_id in (select prem_adj_id from prem_adj pa where (isnull(pa.adj_rrsn_ind,0) !=1 and isnull(pa.adj_void_ind,0) !=1 and isnull(pa.adj_can_ind,0) !=1)
					and	  pa.adj_sts_typ_id in (349,352) and PREM_ADJ_ID < @prem_adj_id and reg_custmr_id = @custmr_id and substring(pa.fnl_invc_nbr_txt,1,3)<>'RTV')
					group by  prem_adj_pgm_id, reg_custmr_id  
				) as t1
				inner join PREM_ADJ  pa on ( t1.prem_adj_id = pa.prem_adj_id)
				where	
					(isnull(pa.adj_rrsn_ind,0) !=1 and isnull(pa.adj_void_ind,0) !=1 and isnull(pa.adj_can_ind,0) !=1)
					and	  pa.adj_sts_typ_id in (349,352) and substring(pa.fnl_invc_nbr_txt,1,3)<>'RTV'
				
		) as t3
		inner join PREM_ADJ_PGM pag on ( t3.prem_adj_pgm_id = pag.prem_adj_pgm_id)
		and pag.actv_ind = 1


		
		-- Step 6: Adjustment applies to Premium and Non-premium then move both dates back
		-- In rare case, the prem_non_prem_cd is null assume that val dates are identical
		-- if prem_non_prem_cd is null in prem_adj_perd table then then there's an error with the
		-- module which updates this field.
				update PREM_ADJ_PGM WITH (ROWLOCK)
				set nxt_valn_dt=prev_valn_dt, 
					nxt_valn_dt_non_prem_dt=prev_valn_dt_non_prem_dt,
					prev_valn_dt = 
					CASE WHEN  DATEADD(D,-1,DATEADD(mm, DATEDIFF(m,0,dateadd(month,-adj_freq_mms_intvrl_cnt, prev_valn_dt))+1,0)) < fst_adj_dt
							THEN NULL
							ELSE DATEADD(D,-1,DATEADD(mm, DATEDIFF(m,0,dateadd(month,-adj_freq_mms_intvrl_cnt, prev_valn_dt))+1,0)) 
					END,
					prev_valn_dt_non_prem_dt=
					CASE WHEN DATEADD(D,-1,DATEADD(mm, DATEDIFF(m,0,dateadd(month,-freq_non_prem_mms_cnt, prev_valn_dt_non_prem_dt))+1,0))  < fst_adj_non_prem_dt
						THEN NULL
						 ELSE DATEADD(D,-1,DATEADD(mm, DATEDIFF(m,0,dateadd(month,-freq_non_prem_mms_cnt, prev_valn_dt_non_prem_dt))+1,0)) 
					END ,
					lsi_retrieve_from_dt=
					CASE WHEN DATEADD(D,-1,DATEADD(mm, DATEDIFF(m,0,dateadd(month,-adj_freq_mms_intvrl_cnt, prev_valn_dt))+1,0)) < fst_adj_dt
						THEN strt_dt
						ELSE dateadd(month,-adj_freq_mms_intvrl_cnt, prev_valn_dt)

					END,
					updt_dt = getdate(),
					updt_user_id = @User_id

				from PREM_ADJ_PGM pag inner join PREM_ADJ_PERD pap on ( pag.prem_adj_pgm_id = pap.prem_adj_pgm_id)
				where pap.prem_adj_id = @prem_adj_id
				and (prem_non_prem_cd = 'B' or  prem_non_prem_cd is null)
				and pag.actv_ind = 1

		--Step 7: Adjustment applies to Premium then only move Premium dates back
				update PREM_ADJ_PGM WITH (ROWLOCK)
				set nxt_valn_dt=prev_valn_dt, 
					prev_valn_dt = 
					CASE WHEN DATEADD(D,-1,DATEADD(mm, DATEDIFF(m,0,dateadd(month,-adj_freq_mms_intvrl_cnt, prev_valn_dt))+1,0)) < fst_adj_dt
							THEN NULL
							ELSE DATEADD(D,-1,DATEADD(mm, DATEDIFF(m,0,dateadd(month,-adj_freq_mms_intvrl_cnt, prev_valn_dt))+1,0)) 
					END,
					lsi_retrieve_from_dt=
					CASE WHEN DATEADD(D,-1,DATEADD(mm, DATEDIFF(m,0,dateadd(month,-adj_freq_mms_intvrl_cnt, prev_valn_dt))+1,0)) < fst_adj_dt
						THEN strt_dt
						ELSE dateadd(month,-adj_freq_mms_intvrl_cnt, prev_valn_dt)
					END,
					updt_dt = getdate(),
					updt_user_id = @User_id
					
				from PREM_ADJ_PGM pag inner join PREM_ADJ_PERD pap on ( pag.prem_adj_pgm_id = pap.prem_adj_pgm_id)
				where pap.prem_adj_id = @prem_adj_id
				and prem_non_prem_cd = 'P'
				and pag.actv_ind = 1

		-- Step 8: Adjustment applies to Non-premium then only move Non-Premium dates back
				update PREM_ADJ_PGM WITH (ROWLOCK)
				set nxt_valn_dt_non_prem_dt=prev_valn_dt_non_prem_dt,
					prev_valn_dt_non_prem_dt=
					CASE WHEN DATEADD(D,-1,DATEADD(mm, DATEDIFF(m,0,dateadd(month,-freq_non_prem_mms_cnt, prev_valn_dt_non_prem_dt))+1,0))  < fst_adj_non_prem_dt
						THEN NULL
						 ELSE DATEADD(D,-1,DATEADD(mm, DATEDIFF(m,0,dateadd(month,-freq_non_prem_mms_cnt, prev_valn_dt_non_prem_dt))+1,0)) 
					END ,
					lsi_retrieve_from_dt=
					CASE WHEN DATEADD(D,-1,DATEADD(mm, DATEDIFF(m,0,dateadd(month,-freq_non_prem_mms_cnt, prev_valn_dt_non_prem_dt))+1,0))  < fst_adj_non_prem_dt
						THEN strt_dt
						ELSE dateadd(month,-freq_non_prem_mms_cnt, prev_valn_dt_non_prem_dt)
					END,
					updt_dt = getdate(),
					updt_user_id = @User_id
					
				from PREM_ADJ_PGM pag inner join PREM_ADJ_PERD pap on ( pag.prem_adj_pgm_id = pap.prem_adj_pgm_id)
				where pap.prem_adj_id = @prem_adj_id
				and prem_non_prem_cd = 'NP'
				and pag.actv_ind = 1

		-- step 9
		-- Set the adj_ind to false for program periods for which its a first adjustment
				select @historical_adj_ind=historical_adj_ind from prem_adj where prem_adj_id=@prem_adj_id
				if(@historical_adj_ind=1)
				begin
				update COML_AGMT_AUDT WITH (ROWLOCK)
				set adj_ind=0,
					updt_dt = getdate(),
					updt_user_id = @User_id
				from COML_AGMT_AUDT caa,Prem_adj_PGM pgm
				where caa.prem_adj_id = @prem_adj_id and
				pgm.Prem_adj_pgm_id = caa.Prem_adj_pgm_id
				and caa.audt_revd_sts_ind<>1
				and pgm.prev_valn_dt_non_prem_dt is NULL and pgm.prev_valn_dt is NULL
				end
				else
				begin
				update COML_AGMT_AUDT WITH (ROWLOCK)
				set adj_ind=0,
					updt_dt = getdate(),
					updt_user_id = @User_id
				from COML_AGMT_AUDT caa
				where caa.prem_adj_id = @prem_adj_id and caa.audt_revd_sts_ind<>1
				end
				
	--end -- if not exists( )
	-- step 10. Set the status of the new adjustment record to “Calc” status
		insert into dbo.[PREM_ADJ_STS]([prem_adj_id],[custmr_id],[adj_sts_typ_id],[qlty_cntrl_dt],[crte_user_id])
		values (@New_Prem_Adj_id,@custmr_id,346,getdate(),@User_id)

	/***********************************************************************
		* Convert from INCURRED to PAID if applicable
		***********************************************************************/
		update PREM_ADJ_PGM WITH (ROWLOCK)
		set paid_incur_typ_id = 298 -- Lookup type: "PAID/INCURRED (LBA ADJUSTMENT TYPE)"; lookup: "PAID"
		from(
				SELECT prem_adj_pgm_id, prem_adj_id	FROM PREM_ADJ_PERD 
				WHERE PREM_ADJ_ID = @prem_adj_id and reg_custmr_id = @custmr_id)
		as t3
		inner join PREM_ADJ_PGM pag on ( t3.prem_adj_pgm_id = pag.prem_adj_pgm_id)
		and pag.actv_ind = 1
		and convert(varchar, nxt_valn_dt,101) = 
			case when datepart(Day,strt_dt) >15
			then
			convert(varchar,DATEADD(D,-1,DATEADD(mm, DATEDIFF(m,0,dateadd(month,incur_conv_mms_cnt,strt_dt))+1,0)),101)
			else
			convert(varchar,DATEADD(D,-1,DATEADD(mm, DATEDIFF(m,0,dateadd(month,incur_conv_mms_cnt,strt_dt)),0)),101)
			end
		and paid_incur_typ_id = 297 -- Lookup type: "PAID/INCURRED (LBA ADJUSTMENT TYPE)"; lookup: "INCURRED"

		update COML_AGMT WITH (ROWLOCK)
		set updt_dt = getdate(),
		adj_typ_id  = 
		CASE 
			WHEN adj_typ_id = 64 THEN 69 --  Incurred Loss Deductible to Paid Loss Deductible 
			WHEN adj_typ_id = 63 THEN 70 --  Incurred Loss DEP to Paid Loss DEP
			WHEN adj_typ_id = 65 THEN 71 --  Incurred Loss Retro to Paid Loss Retro
			WHEN adj_typ_id = 67 THEN 72 --  Incurred Loss Underlayer to Paid Loss Underlayer
			WHEN adj_typ_id = 66 THEN 73 --  Incurred Loss WA to Paid Loss WA
		END
		from(
				SELECT prem_adj_pgm_id, prem_adj_id	FROM PREM_ADJ_PERD 
				WHERE PREM_ADJ_ID = @prem_adj_id and reg_custmr_id = @custmr_id)
		as t3
		inner join PREM_ADJ_PGM pag on ( t3.prem_adj_pgm_id = pag.prem_adj_pgm_id)
		inner join COML_AGMT ca on ( t3.prem_adj_pgm_id = ca.prem_adj_pgm_id)
		and pag.actv_ind = 1
		and ca.actv_ind = 1
		and convert(varchar, nxt_valn_dt,101) = 
			case when datepart(Day,strt_dt) >15
			then
			convert(varchar,DATEADD(D,-1,DATEADD(mm, DATEDIFF(m,0,dateadd(month,incur_conv_mms_cnt,strt_dt))+1,0)),101)
			else
			convert(varchar,DATEADD(D,-1,DATEADD(mm, DATEDIFF(m,0,dateadd(month,incur_conv_mms_cnt,strt_dt)),0)),101)
			end
		and adj_typ_id in (63,64,65,66,67)


	--	Surcharges:
	-- step 11.Update the use_std_subj_prem_ind based on the previous adjustment value from the prem_adj_perd table

	update PREM_ADJ_PGM WITH (ROWLOCK)
	set use_std_subj_prem_ind=pap.use_std_subj_prem_ind
	from PREM_ADJ_PGM pag inner join PREM_ADJ_PERD pap on ( pag.prem_adj_pgm_id = pap.prem_adj_pgm_id)
	where pap.prem_adj_id = @prem_adj_id
	and pag.actv_ind = 1



	-- step 12. Invoke the Calc Engine Business Service for that adjustment’s Premium adjustment Period records as a recalculation
	declare @prem_adj_perd_ids varchar(1000)
	select @prem_adj_perd_ids = coalesce( @prem_adj_perd_ids + ',','') + cast(prem_adj_perd_id as varchar(100))
	from prem_adj_perd where prem_adj_id=@New_Prem_Adj_id


	/******************************************************************
			*			Open Adjustment Check Verification
	******************************************************************/
			exec [dbo].[ModAISCheckKeyParameters] 
			@check_calc_prog_perds = NULL,
			@check_recalc_prem_adj_perds = @prem_adj_perd_ids,
			@create_user_id = @User_id,
			@err_msg_op = @err_msg_check_key_parameters_output output,
			@debug = 0

	DECLARE	@return_value int,
			@err_msg_output_Calc varchar(1000)
			
	if(@err_msg_check_key_parameters_output is null)
	begin
	
	EXEC	@return_value = [dbo].[ModAISCalcDriver]
			@customer_id = @custmr_id,
			@calc_prog_perds = NULL,
			@recalc_prem_adj_perds = @prem_adj_perd_ids,
			@delete_plb = 1,
			@delete_ilrf=1,
			@delete_chf=1,
			@create_user_id = @User_id,
			@err_msg_output = @err_msg_output_Calc OUTPUT,
			@debug = 0
	SELECT	@err_msg_output_Calc as '@err_msg_output'
	SELECT	'Return Value' = @return_value
	
	end
	else
	begin
	update dbo.PREM_ADJ with(rowlock)
	set calc_adj_sts_cd = 'ERR',
		updt_user_id = @User_id,
		updt_dt = getdate()
	where prem_adj_id = @New_Prem_Adj_id

	end
	
	
	declare @err_msg_output2 varchar(1000)
	-- Call Aries Transmittal procedure
		exec [dbo].[ModAIS_TransmittalToARiES] 
		@prem_adj_id = @prem_adj_id,
		@rel_prem_adj_id= @New_Prem_Adj_id,
		@err_msg_output = @err_msg_output2 output,
		@Ind = 2 --Revision

		
	set @tryProc = 0

	if @trancount = 0  
		commit transaction 

	end try
	begin catch
		
		if (ERROR_NUMBER() = 1205)
			set @tryProc = @tryProc - 1
		else
			set @tryProc = -1 

		if @trancount = 0  or xact_state() <> 0
		begin
			rollback transaction
		end

		declare @err_msg varchar(500),
				@err_sev varchar(10),
				@err_no varchar(10)
				
		select 
		error_number() AS ErrorNumber,
		error_severity() AS ErrorSeverity,
		error_state() as ErrorState,
		error_procedure() as ErrorProcedure,
		error_line() as ErrorLine,
		error_message() as ErrorMessage
		
		select  @err_msg = error_message(),
				@err_no = error_number(),
				@err_sev = error_severity(),
				@err_msg_output=error_message()

		RAISERROR (@err_msg, -- Message text.
				   @err_sev, -- Severity.
				   1 -- State.
				   )

	end catch

end -- while loop
GO
if object_id('ModAdjRevisionDriver') is not null
        print 'Created Procedure ModAdjRevisionDriver'
else
        print 'Failed Creating Procedure ModAdjRevisionDriver'
go

if object_id('ModAdjRevisionDriver') is not null
        grant exec on ModAdjRevisionDriver to  public
go

if exists (select 1 from sysobjects where name = 'ModAISCalcLBA' and type = 'P')
	drop procedure ModAISCalcLBA
go

set ansi_nulls off
go
---------------------------------------------------------------------
-----
-----	Proc Name:	ModAISCalcLBA
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	This stored procedure is used to populate the output tables for LBA with calculation results.
-----
-----	On Exit:	
-----			
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification
-----	Modified:	07/14/2010  Venkat Kolimi
-----			- As per the TFS Bug 11658, when there are excess LDF/IBNR amount calculated for the losses , then
-----			  the LBA calculated amount is deducted by that amount.As discussed with users this functionality is
-----			  not valide we are commenting the code related to this functiolaity.
-----	Modified:	10/20/2010  Venkat Kolimi
-----			- As per the TFS Bug 12977, when there is the NP LBA calculations then we are verifying the nxt_val_dt_non_prem with the current date.
-----	Modified:	09/28/2015  Venkat Kolimi
-----			-As per the below requirement I have added the logic CASE WHEN @cnt_prev_adjs=0 THEN 0 ELSE isnull(@dep_amt,0) END
-----            The LBA Deposit must be recognized as being used by the system on the second adjustment, and not use the amount in the actual calculation.  Once the LBA deposit is entered into AIS, it can be used once in the next adjustment, after that, the field should be ignored. 
-----	Modified:	10/07/2015  Venkat Kolimi
-----			LBA Change


---------------------------------------------------------------------

CREATE procedure [dbo].[ModAISCalcLBA] 
@premium_adj_period_id int,
@premium_adjustment_id int,
@customer_id int,
@premium_adj_prog_id int,
@create_user_id int,
@err_msg_op varchar(1000) output,
@debug bit = 0
as

begin
	set nocount on

declare @lba_adj_typ_id int,
		@incl_erp bit,
		@dep_amt decimal(15,2),
		@lba_prev_bil_amt decimal(15,2),
		@incl_ibnr_ldf bit,
		@adj_fctr_rt decimal(15,8),
		@fnl_overrid_amt decimal(15,2),
		@subj_paid_idnmty_amt decimal(15,2),
		@subj_resrv_idnmty_amt decimal(15,2),
		@cnt_prev_adjs int,
		@com_agm_id decimal(15,2),
		@state_id int,
		@prem_adj_pgm_setup_id int,
		@prem_adj_pgm_setup_id_tracker int,
		@prem_adj_parmet_setup_id int,
		@loss_amt decimal(15,2),
		@ldf_amt decimal(15,2),
		@ibnr_amt decimal(15,2),
		@amt_subj_lba_ft decimal(15,2),
		@lba_amt decimal(15,2),
		@sum_loss_amt decimal(15,2),
		@sum_tot_amt decimal(15,2),
		@ibnr_rt decimal(15,8),
		@ldf_rt decimal(15,8),
		@count int,
		@counter int,
		@com_agr_id int,
		@is_ibnr bit,
		@ldf_ibnr_step_ind bit,
		@incl_in_erp bit,
		@months_to_val int,
		@months_elapsed smallint,
		@first_adj int,
		@prev_valid_adj_id int,
		@freq smallint,
		@fst_adj_dt datetime,
		@next_val_date datetime,
		@pgm_setup_id int,
		@prem_adj_valn_dt datetime,
		@pgm_period_valn_dt datetime,
		@exc_ratio decimal(15,8),
		@exc_ldf_ibnr_amt decimal(15,2),
		@paid_incurred_losses decimal(15,2),
		@pgm_lkup_txt varchar(20),
		@curr_prem_non_prem_cd char(2),
		@brkr_id int,
		@bsn_unt_ofc_id int,
		@err_message varchar(500),
		@trancount int




--Check if LBA calc needs to be performed for this adjustment
select 
@prem_adj_valn_dt = valn_dt,
@brkr_id=brkr_id, 
@bsn_unt_ofc_id=bu_office_id
from dbo.PREM_ADJ
where prem_adj_id = @premium_adjustment_id

exec @curr_prem_non_prem_cd = dbo.fn_GetDeterminePerdPremStatus
@CUSTOMER_ID=@customer_id,  
@VALN_MM_DT=@prem_adj_valn_dt, 
@BRKR_ID=@brkr_id, 
@BSN_UNT_OFC_ID=@bsn_unt_ofc_id,
@PGM_ID=@premium_adj_prog_id

select 
@pgm_period_valn_dt = CASE WHEN @curr_prem_non_prem_cd='NP' THEN nxt_valn_dt_non_prem_dt ELSE nxt_valn_dt END
from dbo.PREM_ADJ_PGM
where prem_adj_pgm_id = @premium_adj_prog_id

if @debug = 1
begin
print 'Before LBA valuation date validation'
end

if (@prem_adj_valn_dt <> @pgm_period_valn_dt)
begin
				set @err_message = 'LBA: Valuation date for this adjustment and the valuation date specified in the program period setup do not match' 
				+ ';customer ID: ' + convert(varchar(20),@customer_id) 
				+ ';Program Period ID: ' + convert(varchar(20),@premium_adj_prog_id) 
				set @err_msg_op = @err_message
				exec [dbo].[AddAPLCTN_STS_LOG] 
					@premium_adjustment_id = @premium_adjustment_id,
					@customer_id = @customer_id,
					@premium_adj_prog_id = @premium_adj_prog_id,
					@err_msg = @err_message,
					@create_user_id = @create_user_id

	return
end

if @debug = 1
begin
print 'LBA: valuation date validation PASSED; START OF CALC'
end

set @trancount = @@trancount
--print @trancount
if @trancount >= 1
    save transaction ModAISCalcLBA
else
    begin transaction


begin try

	
											


	/**************************
	* Determine first adjustment
	**************************/

exec @cnt_prev_adjs = [dbo].[fn_DetermineFirstAdjustment_ParmetType]
	@premium_adj_prog_id = @premium_adj_prog_id,
	@adj_parmet_typ_id = 401 -- Adj. Parameter type ID for ADJUSTMENT PARAMETER TYPE -> LBA


set @counter = 1
create table #pgm_setups
(
id int identity(1,1),
pgm_setup_id int,
incl_in_erp bit
)


create index ind ON #pgm_setups (id)

if(@curr_prem_non_prem_cd='P')
begin
insert into #pgm_setups(pgm_setup_id,incl_in_erp)
select 
prem_adj_pgm_setup_id,
incld_ernd_retro_prem_ind
from dbo.PREM_ADJ_PGM_SETUP
where custmr_id = @customer_id
and prem_adj_pgm_id = @premium_adj_prog_id
and adj_parmet_typ_id = 401 -- Adj. Parameter type ID for ADJUSTMENT PARAMETER TYPE -> LBA
and incld_ernd_retro_prem_ind=1
end

if(@curr_prem_non_prem_cd='NP')
begin
insert into #pgm_setups(pgm_setup_id,incl_in_erp)
select 
prem_adj_pgm_setup_id,
incld_ernd_retro_prem_ind
from dbo.PREM_ADJ_PGM_SETUP
where custmr_id = @customer_id
and prem_adj_pgm_id = @premium_adj_prog_id
and adj_parmet_typ_id = 401 -- Adj. Parameter type ID for ADJUSTMENT PARAMETER TYPE -> LBA
and incld_ernd_retro_prem_ind=0
end

if(@curr_prem_non_prem_cd='B')
begin
insert into #pgm_setups(pgm_setup_id,incl_in_erp)
select 
prem_adj_pgm_setup_id,
incld_ernd_retro_prem_ind
from dbo.PREM_ADJ_PGM_SETUP
where custmr_id = @customer_id
and prem_adj_pgm_id = @premium_adj_prog_id
and adj_parmet_typ_id = 401 -- Adj. Parameter type ID for ADJUSTMENT PARAMETER TYPE -> LBA
end

select @count = count(*) from #pgm_setups

while @counter <= @count
begin


select 
@pgm_setup_id = pgm_setup_id,
@incl_in_erp = incl_in_erp
from #pgm_setups 
where id = @counter
if @debug = 1
begin
print' @pgm_setup_id:- ' + convert(varchar(20), @pgm_setup_id)  
print' @incl_in_erp:- ' + convert(varchar(20), @incl_in_erp)  
print' @curr_prem_non_prem_cd:- ' + @curr_prem_non_prem_cd 
print' @count:- ' + convert(varchar(20), @count)  
end

if @incl_in_erp = 0 -- Not included in ERP
begin
	if (@cnt_prev_adjs = 0) -- No existing adjustments; this is initial adjustment
	begin
		print 'initial adjustment'
		/*************************************************************
		* Additional check required for program period setup ->FIRST ADJ NON-PREMIUM 
		* before initiating LBA calc. Only if inception to VAL DATE is 
		* more than this value, intiate LBA calc.
		*************************************************************/
--		select
--		@fst_adj_dt = fst_adj_non_prem_dt,
--		@next_val_date = nxt_valn_dt_non_prem_dt 
--		from dbo.PREM_ADJ_PGM 
--		where prem_adj_pgm_id = @premium_adj_prog_id
--
--		if (@fst_adj_dt <> @next_val_date)
--		begin
--			set @err_message = 'LBA: First Adjustment Date(NP) is not equal to Next Valuation Date(NP)' 
--			+ ';customer ID: ' + convert(varchar(20),@customer_id) 
--			+ ';Program Period ID: ' + convert(varchar(20),@premium_adj_prog_id) 
--			rollback transaction ModAISCalcLBA
--			set @err_msg_op = @err_message
--			exec [dbo].[AddAPLCTN_STS_LOG] 
--				@premium_adjustment_id = @premium_adjustment_id,
--				@customer_id = @customer_id,
--				@premium_adj_prog_id = @premium_adj_prog_id,
--				@err_msg = @err_message,
--				@create_user_id = @create_user_id
--			return
--		end

	end
	else
	begin -- Subsequent adjustment / this is not initial adjustment
		select @months_elapsed = datediff(mm,prev_valn_dt_non_prem_dt,nxt_valn_dt_non_prem_dt), 
			   @freq = freq_non_prem_mms_cnt -- Non-premium frequency
		from dbo.PREM_ADJ_PGM 
		where prem_adj_pgm_id = @premium_adj_prog_id

		if @months_elapsed <> @freq
		begin
			set @err_message = 'LBA: Difference between Next Valuation Date(NP) and Previous Valuation Date(NP) is not consistent with frequency for LBA'
			+ ';customer ID: ' + convert(varchar(20),@customer_id) 
			+ ';Program Period ID: ' + convert(varchar(20),@premium_adj_prog_id) 
			rollback transaction ModAISCalcLBA
			set @err_msg_op = @err_message
			exec [dbo].[AddAPLCTN_STS_LOG] 
				@premium_adjustment_id = @premium_adjustment_id,
				@customer_id = @customer_id,
				@premium_adj_prog_id = @premium_adj_prog_id,
				@err_msg = @err_message,
				@create_user_id = @create_user_id
			return
		end

	end

	select @next_val_date = nxt_valn_dt_non_prem_dt
	from dbo.PREM_ADJ_PGM 
	where prem_adj_pgm_id = @premium_adj_prog_id

	if(getdate() < @next_val_date AND (@curr_prem_non_prem_cd='NP' OR @curr_prem_non_prem_cd='B'))
	begin
		set @err_message = 'LBA: Current date is less than the Next Valuation(NP) Date'
		+ ';customer ID: ' + convert(varchar(20),@customer_id) 
		+ ';Program Period ID: ' + convert(varchar(20),@premium_adj_prog_id) 
		rollback transaction ModAISCalcLBA
		set @err_msg_op = @err_message
		exec [dbo].[AddAPLCTN_STS_LOG] 
			@premium_adjustment_id = @premium_adjustment_id,
			@customer_id = @customer_id,
			@premium_adj_prog_id = @premium_adj_prog_id,
			@err_msg = @err_message,
			@create_user_id = @create_user_id
		return
	end
end -- end of: if @incl_in_erp = 0
else if @incl_in_erp = 1 -- Included in ERP
begin
	if (@cnt_prev_adjs = 0) -- No existing adjustments; this is initial adjustment
	begin
		print 'initial adjustment'
		/*************************************************************
		* Additional check required for program period setup ->FIRST ADJ NON-PREMIUM 
		* before initiating LBA calc. Only if inception to VAL DATE is 
		* more than this value, intiate LBA calc.
		*************************************************************/
--		select
--		@fst_adj_dt = fst_adj_dt,
--		@next_val_date = nxt_valn_dt 
--		from dbo.PREM_ADJ_PGM 
--		where prem_adj_pgm_id = @premium_adj_prog_id
--
--		if (@fst_adj_dt <> @next_val_date)
--		begin
--			set @err_message = 'LBA: First Adjustment Date(P) is not equal to Next Valuation Date(P)'
--			+ ';customer ID: ' + convert(varchar(20),@customer_id) 
--			+ ';Program Period ID: ' + convert(varchar(20),@premium_adj_prog_id) 
--			rollback transaction ModAISCalcLBA
--			set @err_msg_op = @err_message
--			exec [dbo].[AddAPLCTN_STS_LOG] 
--				@premium_adjustment_id = @premium_adjustment_id,
--				@customer_id = @customer_id,
--				@premium_adj_prog_id = @premium_adj_prog_id,
--				@err_msg = @err_message,
--				@create_user_id = @create_user_id
--			return
--		end

	end
	else
	begin -- Subsequent adjustment / this is not initial adjustment
		select @months_elapsed = datediff(mm,prev_valn_dt,nxt_valn_dt), 
			   @freq = adj_freq_mms_intvrl_cnt -- Premium frequency
		from dbo.PREM_ADJ_PGM 
		where prem_adj_pgm_id = @premium_adj_prog_id

		if @months_elapsed <> @freq
		begin
			set @err_message = 'LBA: Difference between Next Valuation Date(P) and Previous Valuation Date(P) is not consistent with frequency for LBA'
			+ ';customer ID: ' + convert(varchar(20),@customer_id) 
			+ ';Program Period ID: ' + convert(varchar(20),@premium_adj_prog_id) 
			rollback transaction ModAISCalcLBA
			set @err_msg_op = @err_message
			exec [dbo].[AddAPLCTN_STS_LOG] 
				@premium_adjustment_id = @premium_adjustment_id,
				@customer_id = @customer_id,
				@premium_adj_prog_id = @premium_adj_prog_id,
				@err_msg = @err_message,
				@create_user_id = @create_user_id
			return
		end

	end

	select @next_val_date = nxt_valn_dt
	from dbo.PREM_ADJ_PGM 
	where prem_adj_pgm_id = @premium_adj_prog_id

	if(getdate() < @next_val_date AND (@curr_prem_non_prem_cd='P' OR @curr_prem_non_prem_cd='B'))
	begin
		set @err_message = 'LBA: Current date is less than the Next Valuation Date(P) for LBA'
		+ ';customer ID: ' + convert(varchar(20),@customer_id) 
		+ ';Program Period ID: ' + convert(varchar(20),@premium_adj_prog_id) 
		rollback transaction ModAISCalcLBA
		set @err_msg_op = @err_message
		exec [dbo].[AddAPLCTN_STS_LOG] 
			@premium_adjustment_id = @premium_adjustment_id,
			@customer_id = @customer_id,
			@premium_adj_prog_id = @premium_adj_prog_id,
			@err_msg = @err_message,
			@create_user_id = @create_user_id
		return
	end

end --end of: if @incl_in_erp = 1 


			
			
	declare lba_basic cursor LOCAL FAST_FORWARD READ_ONLY 
		for 
		select	distinct
		ls.coml_agmt_id,
		ls.st_id,
		d.prem_adj_pgm_setup_id,
		d.los_base_asses_adj_typ_id, -- LBA Adj. Type
		d.incld_ernd_retro_prem_ind, -- Included in ERP
		d.depst_amt, --Intitial deposit
		d.incld_incur_but_not_rptd_ind, -- Incl. IBNR/LDF
		d.adj_fctr_rt, --Factor
		d.fnl_overrid_amt, -- Final LBA Amt.
		ls.subj_paid_idnmty_amt ,
		ls.subj_resrv_idnmty_amt
		from
		(	
		select	
		ad.prem_adj_pgm_dtl_id,
		ap.coml_agmt_id,
		ad.st_id,
		s.prem_adj_pgm_setup_id,
		s.los_base_asses_adj_typ_id, -- LBA Adj. Type
		s.incld_ernd_retro_prem_ind, -- Included in ERP
		s.depst_amt, --Intitial deposit
		s.incld_incur_but_not_rptd_ind, -- Incl. IBNR/LDF
		ad.adj_fctr_rt, --Factor
		ad.fnl_overrid_amt, -- Final LBA Amt.
		al.subj_paid_idnmty_amt ,
		al.subj_resrv_idnmty_amt 
		from 
		(
			select 				
			prem_adj_pgm_id, 				
			custmr_id, 				
			coml_agmt_id, 				
			st_id, 				
			isnull(sum(subj_paid_idnmty_amt),0) as subj_paid_idnmty_amt , 				
			isnull(sum(subj_paid_exps_amt),0) as   subj_paid_exps_amt ,			
			isnull(sum(subj_resrv_idnmty_amt),0) as subj_resrv_idnmty_amt , 				
			isnull(sum(subj_resrv_exps_amt),0) as subj_resrv_exps_amt 				
			from 				
			dbo.ARMIS_LOS_POL 				
			where prem_adj_pgm_id =@premium_adj_prog_id 				
			and custmr_id = @customer_id 				
			and ((prem_adj_id is null) or (prem_adj_id = @premium_adjustment_id))
			and valn_dt	= @prem_adj_valn_dt -- Triage # 67			
			and actv_ind = 1  
			group by  st_id,coml_agmt_id,custmr_id,prem_adj_pgm_id
		) al
		inner join PREM_ADJ_PGM_SETUP_POL ap on (al.coml_agmt_id = ap.coml_agmt_id ) and (al.prem_adj_pgm_id = ap.prem_adj_pgm_id)
		inner join dbo.PREM_ADJ_PGM_SETUP s on (s.prem_adj_pgm_id = ap.prem_adj_pgm_id ) and (s.prem_adj_pgm_setup_id = ap.prem_adj_pgm_setup_id)
		inner join PREM_ADJ_PGM_DTL ad on  (ap.prem_adj_pgm_setup_id = ad.prem_adj_pgm_setup_id ) and (al.st_id =  ad.st_id )
		where 
		s.custmr_id = @customer_id 
		and s.prem_adj_pgm_id = @premium_adj_prog_id
		--and ((al.prem_adj_id is null) or (al.prem_adj_id = 6))
		--and s.prem_adj_pgm_setup_id = @pgm_setup_id
		and s.actv_ind = 1
		--and al.actv_ind = 1
		and ad.actv_ind  = 1 
		and s.adj_parmet_typ_id = 401 -- Adj. Parameter type ID for ADJUSTMENT PARAMETER TYPE -> LBA
		) as d
		right outer join
		(
			select 				
			prem_adj_pgm_id, 				
			custmr_id, 				
			coml_agmt_id, 				
			st_id, 				
			isnull(sum(subj_paid_idnmty_amt),0) as subj_paid_idnmty_amt , 				
			isnull(sum(subj_paid_exps_amt),0) as   subj_paid_exps_amt ,			
			isnull(sum(subj_resrv_idnmty_amt),0) as subj_resrv_idnmty_amt , 				
			isnull(sum(subj_resrv_exps_amt),0) as subj_resrv_exps_amt 				
			from 				
			dbo.ARMIS_LOS_POL 				
			where prem_adj_pgm_id =@premium_adj_prog_id 				
			and custmr_id = @customer_id 				
			and ((prem_adj_id is null) or (prem_adj_id = @premium_adjustment_id)) 
			and valn_dt	= @prem_adj_valn_dt -- Triage # 67			
			and actv_ind = 1  
			group by  st_id,coml_agmt_id,custmr_id,prem_adj_pgm_id
		) ls on (d.st_id = ls.st_id) --and (d.coml_agmt_id = ls.coml_agmt_id)
		where ls.prem_adj_pgm_id = @premium_adj_prog_id
		--and ls.coml_agmt_id = d.coml_agmt_id
		--and d.prem_adj_pgm_setup_id = @pgm_setup_id
		and ls.coml_agmt_id in 
		(
			select pol.coml_agmt_id from dbo.PREM_ADJ_PGM_SETUP_POL pol
			inner join PREM_ADJ_PGM_SETUP stp on (pol.prem_adj_pgm_setup_id = stp.prem_adj_pgm_setup_id) and (pol.prem_adj_pgm_id = stp.prem_adj_pgm_id)
			inner join coml_agmt on coml_agmt.coml_agmt_id = pol.coml_agmt_id and coml_agmt.actv_ind = 1 
			and coml_agmt.adj_typ_id not in (68) and coml_agmt.covg_typ_id in (85,92)
			where pol.prem_adj_pgm_id = @premium_adj_prog_id
			and stp.adj_parmet_typ_id = 401 -- adj parameter setup for LBA
		)

		order by d.prem_adj_pgm_setup_id

		open lba_basic
		fetch lba_basic into @com_agm_id, @state_id, @prem_adj_pgm_setup_id, @lba_adj_typ_id, @incl_erp, @dep_amt, @incl_ibnr_ldf, @adj_fctr_rt, @fnl_overrid_amt, @subj_paid_idnmty_amt, @subj_resrv_idnmty_amt /*@pd_indm_amt, @pd_exp_amt, @res_indm_amt, @res_ex
p_amt*/

		set @prem_adj_pgm_setup_id_tracker = 0


		while @@Fetch_Status = 0
			begin
				begin
					if @debug = 1
					begin
				    print'*******************LBA: START OF ITERATION*********' 
				    print'---------------Input Params-------------------' 
					print' @com_agm_id:- ' + convert(varchar(20), @com_agm_id)  
					print' @lba_adj_typ_id:- ' + convert(varchar(20), @lba_adj_typ_id)  
					print' @incl_erp: ' + convert(varchar(20), @incl_erp)  
					print' @dep_amt: ' + convert(varchar(20), @dep_amt)
					print' @incl_ibnr_ldf: ' + convert(varchar(20), @incl_ibnr_ldf )  
					print' @adj_fctr_rt: ' + convert(varchar(20), @adj_fctr_rt ) 
					print' @fnl_overrid_amt: ' + convert(varchar(20), @fnl_overrid_amt )  
					print' @subj_paid_idnmty_amt: ' + convert(varchar(20), isnull(@subj_paid_idnmty_amt,0)) 
					--print' @subj_paid_exps_amt: ' + convert(varchar(20), isnull(@subj_paid_exps_amt,0) )  
					print' @subj_resrv_idnmty_amt: ' + convert(varchar(20), isnull(@subj_resrv_idnmty_amt,0) )  
					--print' @subj_resrv_exps_amt: ' + convert(varchar(20), isnull(@subj_resrv_exps_amt,0) )  
					end

					set @loss_amt=0
					set @paid_incurred_losses=0
					-- Handle potential null values
					set @subj_paid_idnmty_amt = isnull(@subj_paid_idnmty_amt,0)
					set @subj_resrv_idnmty_amt = isnull(@subj_resrv_idnmty_amt,0)
					set @dep_amt = CASE WHEN @cnt_prev_adjs=0 THEN isnull(@dep_amt,0) ELSE 0 END
	
				
					if @prem_adj_pgm_setup_id_tracker <> @prem_adj_pgm_setup_id
					begin
						if @adj_fctr_rt is null
						begin
							select @prem_adj_pgm_setup_id = ps.prem_adj_pgm_setup_id 
							from dbo.PREM_ADJ_PGM_SETUP_POL spol
							inner join dbo.PREM_ADJ_PGM_SETUP ps on (ps.prem_adj_pgm_setup_id = spol.prem_adj_pgm_setup_id) and (ps.prem_adj_pgm_id = spol.prem_adj_pgm_id)
							where ps.prem_adj_pgm_id = @premium_adj_prog_id
							and ps.adj_parmet_typ_id = 401 -- Adjustment parameter type for LBA
							and coml_agmt_id = @com_agm_id
						end

						if (@pgm_setup_id <> @prem_adj_pgm_setup_id) 
						begin
							fetch lba_basic into @com_agm_id, @state_id, @prem_adj_pgm_setup_id, @lba_adj_typ_id, @incl_erp, @dep_amt, @incl_ibnr_ldf, @adj_fctr_rt, @fnl_overrid_amt, @subj_paid_idnmty_amt, @subj_resrv_idnmty_amt 
							continue
						end


						set @prem_adj_pgm_setup_id_tracker = @prem_adj_pgm_setup_id

						if not exists(select * from dbo.PREM_ADJ_PARMET_SETUP where prem_adj_perd_id = @premium_adj_period_id and prem_adj_id = @premium_adjustment_id and prem_adj_pgm_setup_id = @prem_adj_pgm_setup_id  and adj_parmet_typ_id = 401)
						begin

							if @debug = 1
							begin
							print 'before insert to header'
							print' @premium_adj_period_id: ' + convert(varchar(20), isnull(@premium_adj_period_id,0)) 
							print' @premium_adjustment_id: ' + convert(varchar(20), isnull(@premium_adjustment_id,0) )  
							print' @prem_adj_pgm_setup_id: ' + convert(varchar(20), isnull(@prem_adj_pgm_setup_id,0) )  
							print' @premium_adj_prog_id: ' + convert(varchar(20), isnull(@premium_adj_prog_id,0) )  
							print' @customer_id: ' + convert(varchar(20), isnull(@customer_id,0) )  
							end
							/*
							insert into [dbo].[PREM_ADJ_PARMET_SETUP]
							(
							[prem_adj_perd_id]
						   ,[prem_adj_id]
						   ,[prem_adj_pgm_setup_id]
						   ,[prem_adj_pgm_id]
						   ,[custmr_id]
						   ,[adj_parmet_typ_id]
						   ,[crte_user_id]
							)
							values
							(
							@premium_adj_period_id,
							@premium_adjustment_id,
							@prem_adj_pgm_setup_id,
							@premium_adj_prog_id,
							@customer_id,
							401, -- Lookup value for Adjustment Parameter type : "LBA"
							@create_user_id					
							)
							set @prem_adj_parmet_setup_id = @@identity
							*/

							exec [dbo].[AddPREM_ADJ_PARMET_SETUP] 
								@premium_adj_period_id ,
								@premium_adjustment_id ,
								@customer_id ,
								@prem_adj_pgm_setup_id ,
								@premium_adj_prog_id ,
								401, -- Lookup value for Adjustment Parameter type : "LBA"
								@create_user_id ,
								@prem_adj_parmet_setup_id_op = @prem_adj_parmet_setup_id output
								--print '@prem_adj_parmet_setup_id.........: ' + convert(varchar(20),@prem_adj_parmet_setup_id)
						end --end of: if not exists(select * from dbo.PREM_ADJ_PARMET_SETUP where prem_adj_perd_id = @premium_adj_period_id and prem_adj_id = @premium_adjustment_id and prem_adj_pgm_setup_id = @prem_adj_pgm_setup_id )
						else
						begin
							select @prem_adj_parmet_setup_id = prem_adj_parmet_setup_id from dbo.PREM_ADJ_PARMET_SETUP where prem_adj_perd_id = @premium_adj_period_id and prem_adj_id = @premium_adjustment_id and prem_adj_pgm_setup_id = @prem_adj_pgm_setup_id 
						end

						if @adj_fctr_rt is null
						begin
							select @prem_adj_pgm_setup_id = ps.prem_adj_pgm_setup_id 
							from dbo.PREM_ADJ_PGM_SETUP_POL spol
							inner join dbo.PREM_ADJ_PGM_SETUP ps on (ps.prem_adj_pgm_setup_id = spol.prem_adj_pgm_setup_id) and (ps.prem_adj_pgm_id = spol.prem_adj_pgm_id)
							where ps.prem_adj_pgm_id = @premium_adj_prog_id
							and ps.adj_parmet_typ_id = 401 -- Adjustment parameter type for LBA
							and coml_agmt_id = @com_agm_id

							select @dep_amt = CASE WHEN @cnt_prev_adjs=0 THEN isnull(depst_amt,0) ELSE 0  END, @lba_adj_typ_id = los_base_asses_adj_typ_id,
							@incl_ibnr_ldf = incld_incur_but_not_rptd_ind 
							from dbo.PREM_ADJ_PGM_SETUP 
							where prem_adj_pgm_setup_id = @prem_adj_pgm_setup_id


							select @adj_fctr_rt = adj_fctr_rt , 
							@fnl_overrid_amt = fnl_overrid_amt 
							from PREM_ADJ_PGM_DTL 
							where prem_adj_pgm_setup_id = @prem_adj_pgm_setup_id and st_id = 3 -- All other state
							and actv_ind = 1 
						end --end of: if @adj_fctr_rt is null

					end --end of: if @prem_adj_pgm_setup_id_tracker <> @prem_adj_pgm_setup_id

					if (@pgm_setup_id <> @prem_adj_pgm_setup_id) 
					begin
						fetch lba_basic into @com_agm_id, @state_id, @prem_adj_pgm_setup_id, @lba_adj_typ_id, @incl_erp, @dep_amt, @incl_ibnr_ldf, @adj_fctr_rt, @fnl_overrid_amt, @subj_paid_idnmty_amt, @subj_resrv_idnmty_amt 
						continue
					end
					if @debug = 1
					begin
					print'For AO state  @prem_adj_pgm_setup_id: ' + convert(varchar(20), @prem_adj_pgm_setup_id ) 
					print'For AO state  @adj_fctr_rt: ' + convert(varchar(20), @adj_fctr_rt ) 
					print'For AO state  @fnl_overrid_amt: ' + convert(varchar(20), @fnl_overrid_amt )  
					print'For AO state  @lba_adj_typ_id:- ' + convert(varchar(20), @lba_adj_typ_id)  
					print'For AO state  @incl_erp: ' + convert(varchar(20), @incl_erp)  
					print'For AO state  @dep_amt: ' + convert(varchar(20), @dep_amt)
					print'For AO state  @incl_ibnr_ldf: ' + convert(varchar(20), @incl_ibnr_ldf )  
					end

					if (@adj_fctr_rt is null) and (@fnl_overrid_amt is null)
					begin
						fetch lba_basic into @com_agm_id, @state_id, @prem_adj_pgm_setup_id, @lba_adj_typ_id, @incl_erp, @dep_amt, @incl_ibnr_ldf, @adj_fctr_rt, @fnl_overrid_amt, @subj_paid_idnmty_amt, @subj_resrv_idnmty_amt 
						continue
					end


					if @lba_adj_typ_id = 297 -- Lookup Type:"LBA Adj Type" Lookup Name: "Incurred"
					begin
						set @paid_incurred_losses = @subj_paid_idnmty_amt + @subj_resrv_idnmty_amt

						if @incl_ibnr_ldf = 1 --IBNR checked
						begin
							set @loss_amt = dbo.fn_ComputeLossIBNR_LDF(@lba_adj_typ_id , @com_agm_id , @premium_adj_prog_id ,	@customer_id, @subj_paid_idnmty_amt , @subj_resrv_idnmty_amt  )
						end --end: if @incl_ibnr_ldf = 1
						else --IBNR unchecked
						begin -- else: if @incl_ibnr_ldf = 1
							set @loss_amt = 0 --@subj_paid_idnmty_amt + @subj_resrv_idnmty_amt
						end --end: if @incl_ibnr_ldf = 0
					end -- end: if @lba_adj_typ_id = 297 
					else if @lba_adj_typ_id = 298 -- Lookup Type:"LBA Adj Type" : Lookup Name: "Paid"
					begin
						set @paid_incurred_losses = @subj_paid_idnmty_amt

						if @incl_ibnr_ldf = 1 --IBNR checked
						begin
							set @loss_amt = dbo.fn_ComputeLossIBNR_LDF(@lba_adj_typ_id , @com_agm_id , @premium_adj_prog_id ,	@customer_id, @subj_paid_idnmty_amt , @subj_resrv_idnmty_amt  )
						end --end: if @incl_ibnr_ldf = 1
						else --IBNR unchecked
						begin --else: if @incl_ibnr_ldf = 1
							set @loss_amt = 0 --@subj_paid_idnmty_amt 
							--set @amt_subj_lba_ft = @loss_amt 
						end --end:- else: to  if @incl_ibnr_ldf = 1
					end --end: if @lba_adj_typ_id = 298

					set @loss_amt = isnull(@loss_amt,0)
					set @adj_fctr_rt = isnull(@adj_fctr_rt,0)
					if @debug = 1
					begin
				    print'---------------Computed Values-------------------' 
					print' @loss_amt:- ' + convert(varchar(20), @loss_amt)
					print' @adj_fctr_rt:- ' + convert(varchar(20), @adj_fctr_rt)
					end
					
					set @lba_amt = (@paid_incurred_losses + @loss_amt) * (@adj_fctr_rt)

					/********************************************************
					As per the bug 11658 we have commneted the following code
												11658 FIX STRAT
					*********************************************************/
					/*******************************************
					* This code would remove the excess LDF/IBNR 
					* portion when LDF and IBNR is included 
					* in LBA.
					********************************************/
					--Determine excess LDF/IBNR amount
					--					declare @ldf_ibnr_lim_ind bit
--
--					select 
--					@ldf_ibnr_lim_ind = los_dev_fctr_incur_but_not_rptd_incld_lim_ind 
--					from dbo.COML_AGMT 
--					where coml_agmt_id = @com_agm_id
--
--					if ((@ldf_ibnr_lim_ind = 1) and (@incl_ibnr_ldf = 1))
--					begin
--						select @exc_ldf_ibnr_amt = isnull(sum(exc_ldf_ibnr_amt),0)  
--						from ARMIS_LOS_POL 
--						where prem_adj_pgm_id = @premium_adj_prog_id 
--						and coml_agmt_id = @com_agm_id 
--						and st_id = @state_id
--						and ((prem_adj_id is null) or (prem_adj_id = @premium_adjustment_id))
--
--						select @ibnr_rt = incur_but_not_rptd_fctr_rt  from dbo.COML_AGMT where coml_agmt_id = @com_agm_id and prem_adj_pgm_id = @premium_adj_prog_id and custmr_id = @customer_id
--						if @ibnr_rt is null -- not IBNR; should be LDF
--						begin
--							set @is_ibnr = 0
--						end
--						else -- should be IBNR
--						begin
--							set @is_ibnr = 1
--						end
--
--						exec @exc_ratio = [dbo].[fn_ComputeLBAExcessRatioyPolicy]
--  								@is_ibnr = @is_ibnr,
--								@customer_id = @customer_id,
--								@premium_adj_prog_id = @premium_adj_prog_id,
--								@coml_agmt_id = @com_agm_id,
--								@state_id = @state_id
--						
--						set @lba_amt = @lba_amt - (@exc_ldf_ibnr_amt * @exc_ratio)
--					end

					/********************************************************
					As per the bug 11658 we have commneted the above code
												11658 FIX END
					*********************************************************/

					--Handle Final Override Amount ( for MN )
					if @fnl_overrid_amt is not null
						set @lba_amt = @fnl_overrid_amt

					--This logic is for avoiding the duplicate records for the same state under DEP
					select 
					@pgm_lkup_txt = lk.lkup_txt
					from dbo.PREM_ADJ_PGM pgm
					inner join dbo.LKUP lk on (pgm.pgm_typ_id = lk.lkup_id)
					where 
					prem_adj_pgm_id = @premium_adj_prog_id
					and pgm.actv_ind = 1
					and lk.actv_ind = 1

					if(substring(isnull(@pgm_lkup_txt,''),1,3) = 'DEP')
					begin
					if @fnl_overrid_amt is not null
						set @lba_amt = @fnl_overrid_amt/(case when dbo.fn_GetDEPNumberPolicy(@premium_adjustment_id,@premium_adj_prog_id,
														@customer_id,@state_id) = 0 then 1 else dbo.fn_GetDEPNumberPolicy(@premium_adjustment_id,@premium_adj_prog_id,
														@customer_id,@state_id) end)
					end
					--End of logic
					

					set @lba_amt = isnull(@lba_amt ,0)
					set @lba_amt = round(@lba_amt ,0)
					if @debug = 1
					begin
					print' @lba_amt:- ' + convert(varchar(20), @lba_amt)
					end
						/*
						insert into PREM_ADJ_PARMET_DTL
						(
							[prem_adj_parmet_setup_id],
							[prem_adj_perd_id] ,
							[prem_adj_id],
							[custmr_id],
							[st_id],
							[los_amt]  ,
							[los_base_asses_rt] ,
							[tot_amt],
							[crte_user_id]
						)
						values
						(
							@prem_adj_parmet_setup_id,
							@premium_adj_period_id,
							@premium_adjustment_id,
							@customer_id,							
							@state_id,
							@loss_amt,
							@adj_fctr_rt,
							@lba_amt,
							@create_user_id
						)

						exec [dbo].[AddPREM_ADJ_PARMET_DTL]
							@prem_adj_parmet_setup_id , 
							@premium_adj_period_id ,
							@premium_adjustment_id ,
							@customer_id ,
							@premium_adj_prog_id ,
							@state_id ,
							@loss_amt ,
							@adj_fctr_rt ,
							@lba_amt ,
							@create_user_id 
						*/
						set @loss_amt = @paid_incurred_losses + @loss_amt

						exec [dbo].[AddPREM_ADJ_PARMET_DTL]
							@prem_adj_parmet_setup_id = @prem_adj_parmet_setup_id, 
							@premium_adj_period_id = @premium_adj_period_id,
							@premium_adjustment_id = @premium_adjustment_id,
							@customer_id = @customer_id,
							@premium_adj_prog_id = @premium_adj_prog_id,
							@coml_agmt_id = @com_agm_id,
							@state_id = @state_id,
							@lob_id = null,
							@loss_amt = @loss_amt,
							@paid_loss = null,
							@paid_alae = null,
							@resv_loss = null,
							@resv_alae = null,
							@lba_rt = @adj_fctr_rt,
							@lba_amt = @lba_amt,
							@lcf_rt = null,
							@lcf_amt = null,
							@ldf_rt = null,
							@ldf_amt = null,
							@total_amt = null,
							@create_user_id = @create_user_id


				end
				fetch lba_basic into @com_agm_id, @state_id, @prem_adj_pgm_setup_id, @lba_adj_typ_id, @incl_erp, @dep_amt, @incl_ibnr_ldf, @adj_fctr_rt, @fnl_overrid_amt, @subj_paid_idnmty_amt, @subj_resrv_idnmty_amt /*@pd_indm_amt, @pd_exp_amt, @res_indm_amt, @res_e
xp_amt*/
			end --end of cursor lba_basic / while loop
		close lba_basic
		deallocate lba_basic
		
		/***********************************************
		* DETERMINE PREVIOUS VALID ADJUSTMENT AND POPULATE
		* CORRESPONDING PREVIOUS LBA AMT
		***********************************************/
		exec @prev_valid_adj_id = [dbo].[fn_DeterminePrevValidAdj_NonPremium]
 			@current_premium_adjustment_id = @premium_adjustment_id,
			@customer_id = @customer_id,
			@premium_adj_prog_id = @premium_adj_prog_id,
			@adj_parmet_typ_id = 401 -- Adjustment Parameter Type for LBA

	--------begin: changes for bug # 10235---------------
		declare @cnt_states int
		declare @cnt_states_incld_erp int
		declare @cnt_paramet_setup int
		declare @cnt_paramet_setup_incld_erp int
		declare @prem_adj_paramet_setup_id_incld_erp int
		declare @prem_adj_paramet_setup_id int
		set @cnt_paramet_setup=0
		set @cnt_paramet_setup_incld_erp=0
		set @cnt_states=0
		set @cnt_states_incld_erp=0

		select  @cnt_states=count(st_id)
		from 
		dbo.PREM_ADJ_PARMET_DTL 
		where  prem_adj_id = @premium_adjustment_id
		and prem_adj_perd_id = @premium_adj_period_id
		and custmr_id = @customer_id
		and prem_adj_parmet_setup_id 
		in(select paps.prem_adj_parmet_setup_id from dbo.PREM_ADJ_PARMET_SETUP paps
		inner join PREM_ADJ_PGM_SETUP on paps.PREM_ADJ_PGM_SETUP_ID=PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_ID 
		where 
		paps.prem_adj_id=@premium_adjustment_id 
		and paps.adj_parmet_typ_id=401
		and paps.prem_adj_perd_id=@premium_adj_period_id
		and incld_ernd_retro_prem_ind=0
		)

		select  @cnt_states_incld_erp=count(st_id)
		from 
		dbo.PREM_ADJ_PARMET_DTL 
		where  prem_adj_id = @premium_adjustment_id
		and prem_adj_perd_id = @premium_adj_period_id
		and custmr_id = @customer_id
		and prem_adj_parmet_setup_id 
		in(select paps.prem_adj_parmet_setup_id from dbo.PREM_ADJ_PARMET_SETUP paps
		inner join PREM_ADJ_PGM_SETUP on paps.PREM_ADJ_PGM_SETUP_ID=PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_ID 
		where 
		paps.prem_adj_id=@premium_adjustment_id 
		and paps.adj_parmet_typ_id=401
		and paps.prem_adj_perd_id=@premium_adj_period_id
		and incld_ernd_retro_prem_ind=1

		)
			


if(@cnt_states=0 OR @cnt_states_incld_erp=0)
begin


select @cnt_paramet_setup = count(paps.prem_adj_parmet_setup_id)
from dbo.PREM_ADJ_PARMET_SETUP paps
inner join PREM_ADJ_PGM_SETUP on paps.PREM_ADJ_PGM_SETUP_ID=PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_ID
where  paps.prem_adj_id = @premium_adjustment_id
and paps.prem_adj_perd_id = @premium_adj_period_id
and paps.custmr_id = @customer_id
and paps.adj_parmet_typ_id=401 
and incld_ernd_retro_prem_ind=0

select @cnt_paramet_setup_incld_erp = count(paps.prem_adj_parmet_setup_id)
from dbo.PREM_ADJ_PARMET_SETUP paps
inner join PREM_ADJ_PGM_SETUP on paps.PREM_ADJ_PGM_SETUP_ID=PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_ID
where  paps.prem_adj_id = @premium_adjustment_id
and paps.prem_adj_perd_id = @premium_adj_period_id
and paps.custmr_id = @customer_id
and paps.adj_parmet_typ_id=401 
and incld_ernd_retro_prem_ind=1


if(@cnt_paramet_setup=0)
begin
			
		insert into dbo.PREM_ADJ_PARMET_SETUP
		(
		 [prem_adj_perd_id]
		,[prem_adj_id]
		,[custmr_id]
		,[prem_adj_pgm_setup_id]
		,[prem_adj_pgm_id]
		,[adj_parmet_typ_id]
		,[crte_user_id]
		,[crte_dt]
		)
		select
		@premium_adj_period_id,
		@premium_adjustment_id,
		@customer_id,
		paps.prem_adj_pgm_setup_id,
		@premium_adj_prog_id,
		401,
		@create_user_id,
		getdate()
		from
		PREM_ADJ_PARMET_SETUP paps
		inner join PREM_ADJ_PGM_SETUP on paps.PREM_ADJ_PGM_SETUP_ID=PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_ID
		where  paps.prem_adj_id = @prev_valid_adj_id
		and paps.prem_adj_pgm_id = @premium_adj_prog_id
		and paps.custmr_id = @customer_id
		and paps.adj_parmet_typ_id=401
		and incld_ernd_retro_prem_ind=0

end


if(@cnt_paramet_setup_incld_erp=0)
begin
			
		insert into dbo.PREM_ADJ_PARMET_SETUP
		(
		 [prem_adj_perd_id]
		,[prem_adj_id]
		,[custmr_id]
		,[prem_adj_pgm_setup_id]
		,[prem_adj_pgm_id]
		,[adj_parmet_typ_id]
		,[crte_user_id]
		,[crte_dt]
		)
		select
		@premium_adj_period_id,
		@premium_adjustment_id,
		@customer_id,
		paps.prem_adj_pgm_setup_id,
		@premium_adj_prog_id,
		401,
		@create_user_id,
		getdate()
		from
		PREM_ADJ_PARMET_SETUP paps
		inner join PREM_ADJ_PGM_SETUP on paps.PREM_ADJ_PGM_SETUP_ID=PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_ID
		where  paps.prem_adj_id = @prev_valid_adj_id
		and paps.prem_adj_pgm_id = @premium_adj_prog_id
		and paps.custmr_id = @customer_id
		and paps.adj_parmet_typ_id=401
		and incld_ernd_retro_prem_ind=1

end

select @prem_adj_paramet_setup_id = max(paps.prem_adj_parmet_setup_id)
from dbo.PREM_ADJ_PARMET_SETUP paps
inner join PREM_ADJ_PGM_SETUP on paps.PREM_ADJ_PGM_SETUP_ID=PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_ID
where  paps.prem_adj_id = @premium_adjustment_id
and paps.prem_adj_perd_id = @premium_adj_period_id
and paps.custmr_id = @customer_id
and paps.adj_parmet_typ_id=401 
and incld_ernd_retro_prem_ind=0

select @prem_adj_paramet_setup_id_incld_erp = max(prem_adj_parmet_setup_id)
from dbo.PREM_ADJ_PARMET_SETUP paps
inner join PREM_ADJ_PGM_SETUP on paps.PREM_ADJ_PGM_SETUP_ID=PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_ID
where  paps.prem_adj_id = @premium_adjustment_id
and paps.prem_adj_perd_id = @premium_adj_period_id
and paps.custmr_id = @customer_id
and paps.adj_parmet_typ_id=401 
and incld_ernd_retro_prem_ind=1


insert into dbo.PREM_ADJ_PARMET_DTL
		(
		[prem_adj_parmet_setup_id]
		,[prem_adj_perd_id]
		,[prem_adj_id]
		,[custmr_id]
		,[prem_adj_pgm_id]
		,[coml_agmt_id]
		,[st_id]
		,[crte_user_id]
		,[crte_dt]
		)
		select 
		@prem_adj_paramet_setup_id,
		@premium_adj_period_id,
		@premium_adjustment_id,
		@customer_id,
		@premium_adj_prog_id,
		coml_agmt_id,
		st_id,
		@create_user_id,
		getdate()
		from 
		(
			select
			papd.custmr_id,
			papd.prem_adj_pgm_id,
			papd.prem_adj_id,
			papd.prem_adj_perd_id,
			papd.coml_agmt_id,
			papd.st_id,
			PREM_ADJ_PARMET_SETUP.prem_adj_parmet_setup_id,
			PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_ID,
			PREM_ADJ_PGM_SETUP.incld_ernd_retro_prem_ind
			from dbo.PREM_ADJ_PARMET_DTL papd
			inner join  PREM_ADJ_PARMET_SETUP on PREM_ADJ_PARMET_SETUP.prem_adj_parmet_setup_id=papd.prem_adj_parmet_setup_id
			inner join PREM_ADJ_PGM_SETUP on PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_ID=PREM_ADJ_PARMET_SETUP.PREM_ADJ_PGM_SETUP_ID 
			where  papd.prem_adj_id = @prev_valid_adj_id
			and papd.custmr_id = @customer_id
			and papd.prem_adj_pgm_id = @premium_adj_prog_id
			and PREM_ADJ_PARMET_SETUP.adj_parmet_typ_id=401
			and incld_ernd_retro_prem_ind=0
			
		) as prev
		where not exists
		(
			select * 
			from 
			(
				select
				papd.custmr_id,
				papd.prem_adj_pgm_id,
				papd.prem_adj_id,
				papd.coml_agmt_id,
				papd.st_id,
				PREM_ADJ_PARMET_SETUP.prem_adj_parmet_setup_id,
				PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_ID,
				PREM_ADJ_PGM_SETUP.incld_ernd_retro_prem_ind
				from dbo.PREM_ADJ_PARMET_DTL papd
				inner join  PREM_ADJ_PARMET_SETUP on PREM_ADJ_PARMET_SETUP.prem_adj_parmet_setup_id=papd.prem_adj_parmet_setup_id
				inner join PREM_ADJ_PGM_SETUP on PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_ID=PREM_ADJ_PARMET_SETUP.PREM_ADJ_PGM_SETUP_ID
				where  papd.prem_adj_id = @premium_adjustment_id
				and papd.prem_adj_perd_id = @premium_adj_period_id
				and papd.custmr_id = @customer_id
				and PREM_ADJ_PARMET_SETUP.adj_parmet_typ_id=401 
				and incld_ernd_retro_prem_ind=0
			) as curr
			where prev.coml_agmt_id = curr.coml_agmt_id
			and prev.st_id = curr.st_id
			and prev.PREM_ADJ_PGM_SETUP_ID = curr.PREM_ADJ_PGM_SETUP_ID
		)

		insert into dbo.PREM_ADJ_PARMET_DTL
		(
		[prem_adj_parmet_setup_id]
		,[prem_adj_perd_id]
		,[prem_adj_id]
		,[custmr_id]
		,[prem_adj_pgm_id]
		,[coml_agmt_id]
		,[st_id]
		,[crte_user_id]
		,[crte_dt]
		)
		select 
		@prem_adj_paramet_setup_id_incld_erp,
		@premium_adj_period_id,
		@premium_adjustment_id,
		@customer_id,
		@premium_adj_prog_id,
		coml_agmt_id,
		st_id,
		@create_user_id,
		getdate()
		from 
		(
			select
			papd.custmr_id,
			papd.prem_adj_pgm_id,
			papd.prem_adj_id,
			papd.prem_adj_perd_id,
			papd.coml_agmt_id,
			papd.st_id,
			PREM_ADJ_PARMET_SETUP.prem_adj_parmet_setup_id,
			PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_ID,
			PREM_ADJ_PGM_SETUP.incld_ernd_retro_prem_ind
			from dbo.PREM_ADJ_PARMET_DTL papd
			inner join  PREM_ADJ_PARMET_SETUP on PREM_ADJ_PARMET_SETUP.prem_adj_parmet_setup_id=papd.prem_adj_parmet_setup_id
			inner join PREM_ADJ_PGM_SETUP on PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_ID=PREM_ADJ_PARMET_SETUP.PREM_ADJ_PGM_SETUP_ID 
			where  papd.prem_adj_id = @prev_valid_adj_id
			and papd.custmr_id = @customer_id
			and papd.prem_adj_pgm_id = @premium_adj_prog_id
			and PREM_ADJ_PARMET_SETUP.adj_parmet_typ_id=401
			and incld_ernd_retro_prem_ind=1
			
		) as prev
		where not exists
		(
			select * 
			from 
			(
				select
				papd.custmr_id,
				papd.prem_adj_pgm_id,
				papd.prem_adj_id,
				papd.coml_agmt_id,
				papd.st_id,
				PREM_ADJ_PARMET_SETUP.prem_adj_parmet_setup_id,
				PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_ID,
				PREM_ADJ_PGM_SETUP.incld_ernd_retro_prem_ind
				from dbo.PREM_ADJ_PARMET_DTL papd
				inner join  PREM_ADJ_PARMET_SETUP on PREM_ADJ_PARMET_SETUP.prem_adj_parmet_setup_id=papd.prem_adj_parmet_setup_id
				inner join PREM_ADJ_PGM_SETUP on PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_ID=PREM_ADJ_PARMET_SETUP.PREM_ADJ_PGM_SETUP_ID
				where  papd.prem_adj_id = @premium_adjustment_id
				and papd.prem_adj_perd_id = @premium_adj_period_id
				and papd.custmr_id = @customer_id
				and PREM_ADJ_PARMET_SETUP.adj_parmet_typ_id=401 
				and incld_ernd_retro_prem_ind=1
			) as curr
			where prev.coml_agmt_id = curr.coml_agmt_id
			and prev.st_id = curr.st_id
			and prev.PREM_ADJ_PGM_SETUP_ID = curr.PREM_ADJ_PGM_SETUP_ID
		)



end
--------end: changes for bug # 10235------------


		update dbo.PREM_ADJ_PARMET_SETUP
		set --los_amt = tm.sum_los_amt,
			los_base_asses_amt = tm.sum_lba_amt
		from dbo.PREM_ADJ_PARMET_SETUP as s
		join 
		(
			select 
			[prem_adj_parmet_setup_id],
			[prem_adj_perd_id] ,
			[prem_adj_id],
			sum([los_amt]) as sum_los_amt ,
			sum([los_base_asses_amt]) as sum_lba_amt 
			from 
			PREM_ADJ_PARMET_DTL
			where [prem_adj_id] = @premium_adjustment_id
			and [prem_adj_perd_id] = @premium_adj_period_id
			group by [prem_adj_parmet_setup_id], [prem_adj_perd_id],[prem_adj_id]
		) as tm
		on s.prem_adj_parmet_setup_id = tm.prem_adj_parmet_setup_id
		and s.prem_adj_perd_id = tm.prem_adj_perd_id
		and s.prem_adj_id = tm.prem_adj_id
		where s.adj_parmet_typ_id = 401
		and s.prem_adj_pgm_setup_id = @pgm_setup_id

		/*******************************************
		* Give credits for previously billed amounts
		********************************************/


		select @dep_amt = CASE WHEN @cnt_prev_adjs=0 THEN  isnull(aps.depst_amt,0) ELSE 0 END
		from 
		dbo.PREM_ADJ_PARMET_SETUP pas
		inner join dbo.PREM_ADJ_PGM_SETUP aps on aps.prem_adj_pgm_setup_id = pas.prem_adj_pgm_setup_id
		where
		pas.prem_adj_perd_id = @premium_adj_period_id
		and pas.prem_adj_id = @premium_adjustment_id
		and aps.incld_ernd_retro_prem_ind = 0
		and aps.adj_parmet_typ_id = 401 -- Adjustment Parameter Type for LBA

	
		--retrieve amounts from the previous adjustment
		
		select @lba_prev_bil_amt = isnull(stp.los_base_asses_amt,0) 
		from 
		dbo.PREM_ADJ_PARMET_SETUP stp
		inner join dbo.PREM_ADJ_PERD prd on (stp.prem_adj_perd_id = prd.prem_adj_perd_id) and (stp.prem_adj_id = prd.prem_adj_id)
		inner join dbo.PREM_ADJ_PGM_SETUP adp on (stp.prem_adj_pgm_setup_id = adp.prem_adj_pgm_setup_id)
		where stp.prem_adj_id = @prev_valid_adj_id
		/*
		stp.prem_adj_id in
		(
			select max(pa.prem_adj_id) from dbo.PREM_ADJ pa
			inner join dbo.PREM_ADJ_STS ps on (pa.prem_adj_id = ps.prem_adj_id) and (pa.custmr_id = ps.custmr_id)
			where 
			pa.valn_dt in
			(
				select max(valn_dt) 
				from dbo.PREM_ADJ 
				where valn_dt < (
									select 
									valn_dt 
									from PREM_ADJ 
									where prem_adj_id = @premium_adjustment_id
								)
								and custmr_id = @customer_id

			)
			and ps.adj_sts_typ_id in (349,352) -- Lookup IDs for applicable statuses: Final Invoice and Transmitted
			and pa.custmr_id = @customer_id
		)
		*/
		and prd.prem_adj_pgm_id = @premium_adj_prog_id
		and stp.adj_parmet_typ_id = 401 -- Adjustment Parameter Type for LBA
		and adp.incld_ernd_retro_prem_ind = 0 -- not included in ERP

		if @debug = 1
		begin
		print '@lba_prev_bil_amt: ' + convert(varchar(20), @lba_prev_bil_amt)
		print '@dep_amt: ' + convert(varchar(20), @dep_amt)
		end

		update dbo.PREM_ADJ_PARMET_SETUP 
		set los_base_asses_prev_biled_amt = round(isnull(@lba_prev_bil_amt,0),0) ,
			los_base_asses_depst_amt = round(isnull(@dep_amt,0),0) ,
			tot_amt = round(case when ((@dep_amt is not null and @dep_amt <> 0) 
						or (@lba_prev_bil_amt is not null and @lba_prev_bil_amt <> 0))
						and (los_base_asses_amt is null) then isnull(los_base_asses_amt,0) else los_base_asses_amt 
						end - isnull(@dep_amt,0) - isnull(@lba_prev_bil_amt,0),0)
		from 
		dbo.PREM_ADJ_PARMET_SETUP pas
		inner join dbo.PREM_ADJ_PGM_SETUP aps on aps.prem_adj_pgm_setup_id = pas.prem_adj_pgm_setup_id
		where
		pas.prem_adj_perd_id = @premium_adj_period_id
		and prem_adj_id = @premium_adjustment_id
		and aps.incld_ernd_retro_prem_ind = 0
		and aps.adj_parmet_typ_id = 401 -- Adjustment Parameter Type for LBA
		and pas.adj_parmet_typ_id = 401

		update dbo.PREM_ADJ_PARMET_SETUP 
		set tot_amt = los_base_asses_amt
		from 
		dbo.PREM_ADJ_PARMET_SETUP pas
		inner join dbo.PREM_ADJ_PGM_SETUP aps on aps.prem_adj_pgm_setup_id = pas.prem_adj_pgm_setup_id
		where
		pas.prem_adj_perd_id = @premium_adj_period_id
		and prem_adj_id = @premium_adjustment_id
		and aps.incld_ernd_retro_prem_ind = 1
		and aps.adj_parmet_typ_id = 401 -- Adjustment Parameter Type for LBA
		and pas.adj_parmet_typ_id = 401 -- Adjustment Parameter Type for LBA

		declare @rel_custmr_id int
		
		select @rel_custmr_id = custmr_id from prem_adj_pgm 
		where prem_adj_pgm_id = @premium_adj_prog_id
		
		--Update ARMIS LOS with Premium Adjustment ID
		update dbo.ARMIS_LOS_POL 
		set prem_adj_id = tm.prem_adj_id
		from dbo.ARMIS_LOS_POL as los
		join 
		(
			select
			h.custmr_id,
			h.prem_adj_pgm_id,
			h.prem_adj_id,
			d.coml_agmt_id,
			d.st_id
			from dbo.PREM_ADJ_PARMET_DTL d
			inner join dbo.PREM_ADJ_PARMET_SETUP h on (d.prem_adj_parmet_setup_id = h.prem_adj_parmet_setup_id) and (d.prem_adj_perd_id = h.prem_adj_perd_id) and (d.prem_adj_id = h.prem_adj_id) and (d.custmr_id = h.custmr_id)
			where d.prem_adj_perd_id = @premium_adj_period_id
			and d.prem_adj_id = @premium_adjustment_id
			and d.custmr_id = @rel_custmr_id
			and h.prem_adj_pgm_id = @premium_adj_prog_id
			and h.adj_parmet_typ_id = 401 -- Adjustment Parameter Type for LBA
		) as tm
		on los.custmr_id = tm.custmr_id
		and los.prem_adj_pgm_id = tm.prem_adj_pgm_id
		and los.coml_agmt_id = tm.coml_agmt_id
		and los.st_id = tm.st_id
		where los.custmr_id = @customer_id
		and los.prem_adj_pgm_id = @premium_adj_prog_id
		and ((los.prem_adj_id is null) or (los.prem_adj_id = @premium_adjustment_id))
		and los.valn_dt	= @prem_adj_valn_dt -- Triage # 67
		and los.actv_ind = 1  

		if @debug = 1
		begin
		print '@pgm_setup_id:' + convert(varchar(20),@pgm_setup_id)
		print '---END pgm_setup while loop--------'
		end

		set @counter = @counter + 1
end --end of pgm_setup while loop

		--print '@trancount: ' + convert(varchar(30),@trancount)
		if @trancount = 0
			commit transaction

end try
begin catch

	if @trancount = 0
	begin
		rollback transaction
	end
	else
	begin
		rollback transaction ModAISCalcLBA
	end

	
	declare @err_msg varchar(500),@err_ln varchar(10),
			@err_proc varchar(30),@err_no varchar(10)
	select  @err_msg = error_message(),
		    @err_no = error_number(),
		    @err_proc = error_procedure(),
			@err_ln = error_line()
	set @err_msg = '- error no.:' + isnull(@err_no, ' ') + '; procedure:' 
		+ isnull(@err_proc, ' ') + ';error line:' + isnull(@err_ln, ' ') + ';description:' + isnull(@err_msg, ' ' )
	set @err_msg_op = @err_msg

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
		'AIS Calculation Engine'
     ,'ERR'
       ,'Calculation error'
       ,'Error encountered during calculation of adjustment number: ' 
			+ convert(varchar(20),isnull(@premium_adjustment_id,0)) 
			+ ' for program number: ' 
			+ convert(varchar(20),isnull(@premium_adj_prog_id,0)) 
			+ ' associated with customer number: ' 
			+ convert(varchar(20),isnull(@customer_id,0))
			+ '. Error message: '
			+ isnull(@err_msg, ' ')
	   ,@customer_id
	   ,@premium_adjustment_id
       ,isnull(@create_user_id, 0)
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
end catch


end
GO


if object_id('ModAISCalcLBA') is not null
	print 'Created Procedure ModAISCalcLBA'
else
	print 'Failed Creating Procedure ModAISCalcLBA'
go

if object_id('ModAISCalcLBA') is not null
	grant exec on ModAISCalcLBA to public
go


if exists (select 1 from sysobjects 
                where name = 'AddARMIS_LOS_POLCpy' and type = 'P')
        drop procedure AddARMIS_LOS_POLCpy
go
 
set ansi_nulls off
go

---------------------------------------------------------------------
-----
----- Proc Name: AddARMIS_LOS_POLCpy
-----
----- Version: SQL Server 2005
-----
----- Description: Procedure creates a copy of the ARMIS_LOS_POL record.
----- 
----- On Exit:
----- 
-----
----- Modified: 11/01/2008  Development Team
-----                   Created SP
-----
---------------------------------------------------------------------
CREATE procedure [dbo].[AddARMIS_LOS_POLCpy]
      @select         smallint = 0, 
      @prem_adj_id    int,
      @armis_los_pol_id    int,
      @new_prem_adj_id    int,
      @identity       int output

as
declare   @error      int,
          @trancount  int,
          @ent_timestamp  datetime


select    @trancount  = @@trancount,
          @ent_timestamp = getdate( )

if @trancount = 0
begin 
 begin transaction 
end 
 
begin try

 insert into ARMIS_LOS_POL
 (
            
            coml_agmt_id
           ,prem_adj_pgm_id
           ,custmr_id
           ,st_id
           ,valn_dt
           ,prem_adj_id
           ,suprt_serv_custmr_gp_id
           ,paid_idnmty_amt
           ,paid_exps_amt
           ,resrv_idnmty_amt
           ,resrv_exps_amt
           ,non_bilabl_paid_idnmty_amt
           ,non_bilabl_paid_exps_amt
           ,non_bilabl_resrv_idnmty_amt
           ,non_bilabl_resrv_exps_amt
           ,subj_paid_idnmty_amt
           ,subj_paid_exps_amt
           ,subj_resrv_idnmty_amt
           ,subj_resrv_exps_amt
           ,exc_paid_idnmty_amt
           ,exc_paid_exps_amt
           ,exc_resrvd_idnmty_amt
           ,exc_resrv_exps_amt
           ,subj_ldf_ibnr_amt
           ,exc_ldf_ibnr_amt
           ,sys_genrt_ind
           ,updt_user_id
           ,updt_dt
           ,crte_user_id
           ,crte_dt
           ,actv_ind
		   ,copy_ind
        )
        select
            coml_agmt_id
           ,prem_adj_pgm_id
           ,custmr_id
           ,st_id
           ,valn_dt
           ,@new_prem_adj_id
           ,suprt_serv_custmr_gp_id
           ,paid_idnmty_amt
           ,paid_exps_amt
           ,resrv_idnmty_amt
           ,resrv_exps_amt
           ,non_bilabl_paid_idnmty_amt
           ,non_bilabl_paid_exps_amt
           ,non_bilabl_resrv_idnmty_amt
           ,non_bilabl_resrv_exps_amt
           ,subj_paid_idnmty_amt
           ,subj_paid_exps_amt
           ,subj_resrv_idnmty_amt
           ,subj_resrv_exps_amt
           ,exc_paid_idnmty_amt
           ,exc_paid_exps_amt
           ,exc_resrvd_idnmty_amt
           ,exc_resrv_exps_amt
           ,subj_ldf_ibnr_amt
           ,exc_ldf_ibnr_amt
           ,sys_genrt_ind
           ,NULL
           ,NULL
           ,crte_user_id
           ,@ent_timestamp
           ,actv_ind
		   ,copy_ind
     from ARMIS_LOS_POL
     where armis_los_pol_id = @armis_los_pol_id
 
 select  @identity = @@identity
       if @trancount = 0
       begin
                commit transaction 
       end
end try
begin catch

  if @trancount = 0
  begin
        rollback transaction 
  end

 declare @err_sev varchar(10), 
         @err_msg varchar(500), 
         @err_no varchar(10) 


  select 
  error_number() AS ErrorNumber,
  error_severity() AS ErrorSeverity,
  error_state() as ErrorState,
  error_procedure() as ErrorProcedure,
  error_line() as ErrorLine,
  error_message() as ErrorMessage


 select  @err_msg = error_message(),
         @err_no = error_number(),
      @err_sev = error_severity()

  RAISERROR ( @err_msg, -- Message text. 
                 @err_sev, -- Severity. 
                 1 -- State. 
                )
end catch
go

if object_id('AddARMIS_LOS_POLCpy') is not null
        print 'Created Procedure AddARMIS_LOS_POLCpy'
else
        print 'Failed Creating Procedure AddARMIS_LOS_POLCpy'
go

if object_id('AddARMIS_LOS_POLCpy') is not null
        grant exec on AddARMIS_LOS_POLCpy to  public
go
 
if exists (select 1 from sysobjects 
                where name = 'GetARMSLossesDetails' and type = 'P')
        drop procedure GetARMSLossesDetails
go

set ansi_nulls off
go         

---------------------------------------------------------------------            
-----            
----- Proc Name:  GetARMSLossesDetails            
-----            
----- Version:  SQL Server 2012            
-----            
----- Author :  Dheeraj Nadimpalli            
-----            
----- Description: Returns data for given different parameters in ARMIS_LOS_POL.            
-----               
----- Modified:                
-----               
-----             
---------------------------------------------------------------------            
          
CREATE PROCEDURE [dbo].[GetARMSLossesDetails]          
@VALDT VARCHAR(100)=NULL,            
@ADJNO INT=NULL,          
@PREM_ADJ_PGM_ID varchar(1000)=NULL,          
@COML_AGM_IDS VARCHAR(1000)=NULL,          
@SYS_GEN BIT=NULL,
@CUSTMR_ID INT=NULL          
AS            
BEGIN           
          
BEGIN TRY           
            
SELECT DISTINCT CONVERT(VARCHAR(10),POL.VALN_DT,101) AS [VALUATION DATE],LOBTYP.LKUP_TXT AS LOB,LTRIM(RTRIM(COML.POL_SYM_TXT)) + ' ' + LTRIM(RTRIM(COML.POL_NBR_TXT)) + ' ' + LTRIM(RTRIM(COML.POL_MODULUS_TXT)) AS [POLICY NO],          
POL.CUSTMR_ID AS [CUSTOMER ID],CONVERT(VARCHAR(10),PGM.STRT_DT,101) AS [PROGRAM PERIOD EFF DATE],CONVERT(VARCHAR(10),PGM.PLAN_END_DT,101) [PROGRAM PERIOD EXP DATE], --SCGID,         
PGMTYP.LKUP_TXT AS [PROGRAM TYPE],STTYP.ATTR_1_TXT AS [STATE],CONVERT(VARCHAR(10),COML.POL_EFF_DT,101) AS [POLICY EFF DATE],CONVERT(VARCHAR(10),COML.PLANNED_END_DATE,101) AS [POLICY EXP DATE],          
'' AS SCGID,'' [Claim Status],CASE       
  WHEN POL.SYS_GENRT_IND = 1 THEN 'Yes'          
  WHEN POL.SYS_GENRT_IND = 0 THEN 'No'          
  ELSE ''          
END AS  [System Generated] ,paid_idnmty_amt [Total Paid Indemnity],paid_exps_amt [Total Paid Expense],      
 resrv_idnmty_amt [Total Reserved Indemnity],resrv_exps_amt [Total Reserved Expense] ,     
--ORGIN_CLM_NBR_TXT AS [CLAIM NO],ADDN_CLM_IND AS [ADDITIONAL CLAIM IND],ADDN_CLM_TXT AS [ADDITIONAL CLAIM],CLMT_NM AS [CLAIMANT NAME],          
--CLMSTATUS.LKUP_TXT [CLAIM STATUS],CONVERT(VARCHAR(10),COVG_TRIGR_DT,101) [COVERAGE TRIGGER DATE],LIM2_AMT [LIMIT 2],EXC.PAID_IDNMTY_AMT [TOTAL PAID INDEMNITY]          
--,EXC.PAID_EXPS_AMT [TOTAL PAID EXPENSE],EXC.RESRVD_IDNMTY_AMT [TOTAL RESERVED INDEMNITY],EXC.RESRVD_EXPS_AMT [TOTAL RESERVED EXPENSE],          
--EXC.NON_BILABL_PAID_IDNMTY_AMT [NONBILLABlE PAID INDEMNITY],EXC.NON_BILABL_PAID_EXPS_AMT [NONBILLABlE PAID EXPENSE],          
--EXC.NON_BILABL_RESRVD_IDNMTY_AMT [NONBILLABlE RESERVED INDEMNITY],EXC.NON_BILABL_RESRVD_EXPS_AMT [NONBILLABlE RESERVED EXPENSE],          
 PGM.STRT_DT
  
FROM ARMIS_LOS_POL POL      
INNER JOIN COML_AGMT COML ON COML.COML_AGMT_ID=POL.COML_AGMT_ID          
INNER JOIN PREM_ADJ_PGM PGM ON PGM.PREM_ADJ_PGM_ID = POL.PREM_ADJ_PGM_ID          
INNER JOIN LKUP PGMTYP ON PGM.PGM_TYP_ID=PGMTYP.LKUP_ID          
INNER JOIN LKUP STTYP ON STTYP.LKUP_ID=POL.ST_ID    
INNER JOIN LKUP LOBTYP ON LOBTYP.LKUP_ID = COML.covg_typ_id        
--INNER JOIN LKUP CLMSTATUS ON CLMSTATUS.LKUP_ID=POL.CLM_STS_ID
INNER JOIN PREM_ADJ ADJ ON POL.prem_adj_id = ADJ.prem_adj_id          
WHERE POL.ACTV_IND=1           
AND  POL.CUSTMR_ID = ISNULL(@CUSTMR_ID, POL.CUSTMR_ID)  
AND  POL.VALN_DT = ISNULL(@VALDT, POL.VALN_DT)  
AND ADJ.adj_sts_typ_id <> 347   
AND ADJ.ADJ_CAN_IND<>1 
AND ADJ.ADJ_VOID_IND<>1
AND ADJ.ADJ_RRSN_IND<>1
AND SUBSTRING(ADJ.FNL_INVC_NBR_TXT,1,3)<>'RTV'     
--AND  POL.PREM_ADJ_ID = ISNULL(@ADJNO, POL.PREM_ADJ_ID)          
--AND PGM.PREM_ADJ_PGM_ID = ISNULL(@PREM_ADJ_PGM_ID, PGM.PREM_ADJ_PGM_ID)       
AND (ISNULL(@PREM_ADJ_PGM_ID, '')='' OR PGM.PREM_ADJ_PGM_ID IN (SELECT ITEMS FROM fn_Getidsfromstring(@PREM_ADJ_PGM_ID,',')))        
AND (ISNULL(@COML_AGM_IDS, '')='' OR COML.COML_AGMT_ID IN (SELECT ITEMS FROM fn_Getidsfromstring(@COML_AGM_IDS,',')))           
AND POL.SYS_GENRT_IND = ISNULL(@SYS_GEN,POL.SYS_GENRT_IND)        
ORDER BY   PGM.STRT_DT DESC,[PROGRAM TYPE] ASC,[POLICY NO] ASC    
          
END TRY            
          
BEGIN CATCH            
            
 SELECT             
    ERROR_NUMBER() AS ERRORNUMBER,            
    ERROR_SEVERITY() AS ERRORSEVERITY,            
    ERROR_STATE() AS ERRORSTATE,            
    ERROR_PROCEDURE() AS ERRORPROCEDURE,            
    ERROR_LINE() AS ERRORLINE,            
    ERROR_MESSAGE() AS ERRORMESSAGE;            
            
            
END CATCH            
          
END 
GO
if object_id('GetARMSLossesDetails') is not null
        print 'Created Procedure GetARMSLossesDetails'
else
        print 'Failed Creating Procedure GetARMSLossesDetails'
go

if object_id('GetARMSLossesDetails') is not null
        grant exec on GetARMSLossesDetails to  public
go 
  
if exists (select 1 from sysobjects 
		where name = 'GetExcessLossesDetails' and type = 'P')
	drop procedure GetExcessLossesDetails
go

set ansi_nulls off
go  

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-----              
----- Proc Name:  GetExcessLossesDetails              
-----              
----- Version:  SQL Server 2012              
-----              
----- Author :  Dheeraj Nadimpalli              
-----              
----- Description: Returns data for given different parameters in ARMIS_LOS_EXC.              
-----                 
----- Modified:                  
-----                 
-----               
---------------------------------------------------------------------              
            
CREATE PROCEDURE [dbo].[GetExcessLossesDetails]            
@VALDT VARCHAR(100)=NULL,              
@ADJNO INT=NULL,            
@PREM_ADJ_PGM_ID VARCHAR(1000)=NULL,            
@COML_AGM_IDS VARCHAR(1000)=NULL,            
@SYS_GEN BIT=NULL,
@CUSTMR_ID INT=NULL             
AS              
BEGIN             
            
BEGIN TRY             
              
SELECT DISTINCT CONVERT(VARCHAR(10),POL.VALN_DT,101) AS [VALUATION DATE],LOBTYP.LKUP_TXT AS LOB,LTRIM(RTRIM(COML.POL_SYM_TXT)) + ' ' + LTRIM(RTRIM(COML.POL_NBR_TXT)) + ' ' + LTRIM(RTRIM(COML.POL_MODULUS_TXT)) AS [POLICY NO],            
EXC.CUSTMR_ID AS [CUSTOMER ID],CONVERT(VARCHAR(10),PGM.STRT_DT,101) AS [PROGRAM PERIOD EFF DATE],CONVERT(VARCHAR(10),PGM.PLAN_END_DT,101) [PROGRAM PERIOD EXP DATE],            
PGMTYP.LKUP_TXT AS [PROGRAM TYPE],STTYP.ATTR_1_TXT AS [STATE],CONVERT(VARCHAR(10),COML.POL_EFF_DT,101) AS [POLICY EFF DATE],CONVERT(VARCHAR(10),COML.PLANNED_END_DATE,101) AS [POLICY EXP DATE],            
CLM_NBR_TXT AS [CLAIM NO],ADDN_CLM_IND AS [ADDITIONAL CLAIM IND],ADDN_CLM_TXT AS [ADDITIONAL CLAIM],CLMT_NM AS [CLAIMANT NAME],            
CLMSTATUS.LKUP_TXT [CLAIM STATUS],CONVERT(VARCHAR(10),COVG_TRIGR_DT,101) [COVERAGE TRIGGER DATE],LIM2_AMT [LIMIT 2],EXC.PAID_IDNMTY_AMT [TOTAL PAID INDEMNITY]            
,EXC.PAID_EXPS_AMT [TOTAL PAID EXPENSE],EXC.RESRVD_IDNMTY_AMT [TOTAL RESERVED INDEMNITY],EXC.RESRVD_EXPS_AMT [TOTAL RESERVED EXPENSE],            
EXC.NON_BILABL_PAID_IDNMTY_AMT [NONBILLABLE PAID INDEMNITY],EXC.NON_BILABL_PAID_EXPS_AMT [NONBILLABLE PAID EXPENSE],            
EXC.NON_BILABL_RESRVD_IDNMTY_AMT [NONBILLABLE RESERVED INDEMNITY],EXC.NON_BILABL_RESRVD_EXPS_AMT [NONBILLABLE RESERVED EXPENSE],            
CASE            
  WHEN EXC.SYS_GENRT_IND = 1 THEN 'Yes'            
  WHEN EXC.SYS_GENRT_IND = 0 THEN 'No'            
  ELSE ''            
END AS  [SYSTEM GENERATED],
PGM.STRT_DT            
FROM ARMIS_LOS_EXC EXC            
INNER JOIN ARMIS_LOS_POL POL ON POL.ARMIS_LOS_POL_ID=EXC.ARMIS_LOS_POL_ID            
INNER JOIN COML_AGMT COML ON COML.COML_AGMT_ID=EXC.COML_AGMT_ID            
INNER JOIN PREM_ADJ_PGM PGM ON PGM.PREM_ADJ_PGM_ID = EXC.PREM_ADJ_PGM_ID            
INNER JOIN LKUP PGMTYP ON PGM.PGM_TYP_ID=PGMTYP.LKUP_ID            
INNER JOIN LKUP STTYP ON STTYP.LKUP_ID=POL.ST_ID            
INNER JOIN LKUP CLMSTATUS ON CLMSTATUS.LKUP_ID=EXC.CLM_STS_ID       
INNER JOIN LKUP LOBTYP ON LOBTYP.LKUP_ID = COML.covg_typ_id
INNER JOIN PREM_ADJ ADJ ON POL.prem_adj_id = ADJ.prem_adj_id         
WHERE EXC.ACTV_IND=1             
AND  POL.VALN_DT = ISNULL(@VALDT, POL.VALN_DT)   
AND  POL.CUSTMR_ID = ISNULL(@CUSTMR_ID, POL.CUSTMR_ID)
AND ADJ.adj_sts_typ_id <> 347 
AND ADJ.ADJ_CAN_IND<>1 
AND ADJ.ADJ_VOID_IND<>1
AND ADJ.ADJ_RRSN_IND<>1
AND SUBSTRING(ADJ.FNL_INVC_NBR_TXT,1,3)<>'RTV'          
--AND  POL.PREM_ADJ_ID = ISNULL(@ADJNO, POL.PREM_ADJ_ID)            
--AND PGM.PREM_ADJ_PGM_ID = ISNULL(@PREM_ADJ_PGM_ID, PGM.PREM_ADJ_PGM_ID)          
AND (ISNULL(@PREM_ADJ_PGM_ID, '')='' OR PGM.PREM_ADJ_PGM_ID IN (SELECT ITEMS FROM fn_Getidsfromstring(@PREM_ADJ_PGM_ID,',')))         
AND (ISNULL(@COML_AGM_IDS, '')='' OR COML.COML_AGMT_ID IN (SELECT ITEMS FROM fn_Getidsfromstring(@COML_AGM_IDS,',')))             
AND EXC.SYS_GENRT_IND = ISNULL(@SYS_GEN,EXC.SYS_GENRT_IND)            
ORDER BY   PGM.STRT_DT DESC,[PROGRAM TYPE] ASC,[POLICY NO] ASC            
END TRY              
           
BEGIN CATCH              
              
 SELECT               
    ERROR_NUMBER() AS ERRORNUMBER,              
    ERROR_SEVERITY() AS ERRORSEVERITY,              
    ERROR_STATE() AS ERRORSTATE,              
    ERROR_PROCEDURE() AS ERRORPROCEDURE,              
    ERROR_LINE() AS ERRORLINE,              
    ERROR_MESSAGE() AS ERRORMESSAGE;              
              
              
END CATCH              
            
END          
GO

if object_id('GetExcessLossesDetails') is not null
	print 'Created Procedure GetExcessLossesDetails'
else
	print 'Failed Creating Procedure GetExcessLossesDetails'
go

if object_id('GetExcessLossesDetails') is not null
	grant exec on GetExcessLossesDetails to public
go

if exists (select 1 from sysobjects 
		where name = 'ModAIS_Process_Copy_Losses_Excess_Upload' and type = 'P')
	drop procedure ModAIS_Process_Copy_Losses_Excess_Upload
go

set ansi_nulls off
go

---------------------------------------------------------------------
-----
----- PROC NAME:  ModAIS_Process_Copy_Losses_Excess_Upload
-----
----- VERSION:  SQL SERVER 2008
-----
----- AUTHOR :  CSC
-----
----- DESCRIPTION: To validate and insert/update ARMIS  Excess losses details for copy losses.
----
----- ON EXIT: 
-----   
-----
----- CREATED: 
-----   
----- 
---------------------------------------------------------------------

CREATE PROCEDURE [dbo].[ModAIS_Process_Copy_Losses_Excess_Upload]      
@create_user_id   VARCHAR(500),      
@dtUploadDateTime DATETIME,      
@err_msg_op       VARCHAR(1000) OUTPUT,      
@debug            BIT = 0      
AS      
  BEGIN      
      SET NOCOUNT ON      
      
 DECLARE @Valuation_Date     VARCHAR(MAX),      
   @LOB       VARCHAR(MAX),      
   @POLICY_NO      VARCHAR(MAX),      
   @CUSTMR_ID      VARCHAR(MAX),      
   @PGM_EFF_DT      VARCHAR(MAX),      
   @PGM_EXP_DT      VARCHAR(MAX),      
   @PGM_TYPE      VARCHAR(MAX),      
   @STATE       VARCHAR(MAX),      
   @POL_EFF_DT      VARCHAR(MAX),      
   @POL_EXP_DT      VARCHAR(MAX),      
   @CLAIMNO      VARCHAR(MAX),      
   @ADDITIONALCLAIMIND    VARCHAR(MAX),      
   @ADDITIONALCLAIM    VARCHAR(MAX),      
   @CLAIMANTNAME     VARCHAR(MAX),      
   @CLAIMSTATUS     VARCHAR(MAX),      
   @COVERAGETRIGGERDATE   VARCHAR(MAX),      
   @LIMIT       VARCHAR(MAX),      
   @TOTALPAIDINDEMNITY    VARCHAR(MAX),      
   @TOTALPAIDEXPENSE    VARCHAR(MAX),      
   @TOTALRESERVEDINDEMNITY   VARCHAR(MAX),      
   @TOTALRESERVEDEXPENSE   VARCHAR(MAX),      
   @NONBILLABLEPAIDINDEMNITY  VARCHAR(MAX),      
   @NONBILLABLEPAIDEXPENSE   VARCHAR(MAX),      
   @NONBILLABLERESERVEDINDEMNITY VARCHAR(MAX),      
   @NONBILLABLERESERVEDEXPENSE  VARCHAR(MAX),      
   @SYSTEMGENERATED    VARCHAR(MAX),       
   @CRTE_USER_ID           VARCHAR(100),      
   @CRTE_DT            DATETIME,      
   @VALIDATE            BIT,      
   @LOSS_INFO_COPY_STAGE_EXEC_ID   INT,      
   @Armis_los_pol_id         INT,      
   @LOSS_INFO_COPY_STAGE_ID INT,      
   @Coml_Agmt_Id     INT,      
   @Count_PolicyNo     INT,      
   @CustmrID      INT,      
   @StateID      INT,      
   @Prem_Adj_Pgm_ID    INT,      
   @Prem_Adj_ID     INT,      
   @ClaimStatusID     INT,      
   @Adj_Status_ID     INT,     
   @Covg_Typ_ID     INT,      
   @Validation_Message       VARCHAR(MAX),      
   @Validation_Message_Valuation_Date    VARCHAR(MAX),      
   @Validation_Message_PolicyNo     VARCHAR(MAX),      
   @Validation_Message_CUSTMR_ID     VARCHAR(MAX),      
   @Validation_Message_PGM_EFF_DT     VARCHAR(MAX),      
   @Validation_Message_PGM_EXP_DT     VARCHAR(MAX),      
   @Validation_Message_State_ID     VARCHAR(MAX),      
   @Validation_Message_POL_EFF_DT     VARCHAR(MAX),      
   @Validation_Message_POL_EXP_DT     VARCHAR(MAX),      
   @Validation_Message_PGMPRD      VARCHAR(MAX),      
   @Validation_Message_COML_AGMT_ID    VARCHAR(MAX),      
   @Validation_Message_PREM_ADJ_ID     VARCHAR(MAX),      
   @Validation_Message_LOSS_INFO_COPY_STAGE_ID  VARCHAR(MAX),      
   @Validation_Message_AdditionalClaimInd      VARCHAR(MAX),      
   @Validation_Message_ClaimStatus           VARCHAR(MAX),      
   @Validation_Message_CoverageTriggerDate   VARCHAR(MAX),      
   @Validation_Message_Limit2      VARCHAR(MAX),      
   @Validation_Message_Total_Paid_Indemnity  VARCHAR(MAX),      
   @Validation_Message_Total_Paid_Expense   VARCHAR(MAX),      
   @Validation_Message_Total_Reserved_Indemnity VARCHAR(MAX),      
   @Validation_Message_Total_Reserved_Expense  VARCHAR(MAX),      
   @Validation_Message_NonBillable_Paid_Indemnity   VARCHAR(MAX),      
   @Validation_Message_NonBillable_Paid_Expense    VARCHAR(MAX),      
   @Validation_Message_NonBillable_Reserved_Indemnity VARCHAR(MAX),      
   @Validation_Message_NonBillable_Reserved_Expense   VARCHAR(MAX),      
   @Validation_Message_System_Generated        VARCHAR(MAX),      
   @trancount  INT,      
   @intUserID  INT,      
   @index   SMALLINT,      
   @index2   SMALLINT,    
   @LOBID      INT,      
   @Validation_Message_LOB_ID     VARCHAR(max)      
      
      IF @debug = 1      
        BEGIN      
            PRINT 'Upload: Loss Info Copy Stage Policy Processing started'      
        END      
      
      SET @trancount = @@trancount      
      
      --print @trancount                    
      IF @trancount >= 1      
        SAVE TRANSACTION Loss_Info_Copy_Stage_Excess      
      ELSE      
        BEGIN TRANSACTION      
      
      BEGIN try      
          IF @debug = 1      
            BEGIN      
                PRINT ' @create_user_id:- ' + CONVERT(VARCHAR(500), @create_user_id)      
       PRINT ' @dtUploadDateTime:- ' + CONVERT(VARCHAR(500), @dtUploadDateTime)      
            END      
      
          DECLARE LOSS_INFO_COPY_STAGE_EXCESS_Basic CURSOR LOCAL FAST_FORWARD READ_ONLY      
          FOR      
            SELECT Valuation_Date,      
     LOB,      
     POLICY_NO,      
     CUSTMR_ID,      
     PGM_EFF_DT,      
     PGM_EXP_DT,      
     PGM_TYPE,      
     [STATE],      
     POL_EFF_DT,      
     POL_EXP_DT,      
     CLM_NBR_TXT,      
     ADDN_CLM_IND,      
     ADDN_CLM_TXT,      
     CLMT_NM,      
     CLM_STS_ID,      
     COVG_TRIGR_DT,      
     LIM2_AMT,      
     LOS_PAID_IDNMTY_AMT,      
     LOS_PAID_EXPS_AMT,      
     LOS_RESRV_IDNMTY_AMT,      
     LOS_RESRV_EXPS_AMT,      
     NON_BILABL_PAID_IDNMTY_AMT,      
     NON_BILABL_PAID_EXPS_AMT,      
     NON_BILABL_RESRVD_IDNMTY_AMT,      
     NON_BILABL_RESRVD_EXPS_AMT,      
     LOS_SYS_GENRT_IND,      
     CRTE_USER_ID,      
     CRTE_DT,      
     VALIDATE,      
     LOSS_INFO_COPY_STAGE_EXEC_ID      
            FROM   [dbo].[LOSS_INFO_COPY_STAGE_EXEC]      
            WHERE  [CRTE_USER_ID] = @create_user_id      
                   AND [CRTE_DT] = @dtUploadDateTime      
      
          OPEN LOSS_INFO_COPY_STAGE_EXCESS_Basic      
      
          FETCH LOSS_INFO_COPY_STAGE_EXCESS_Basic INTO @Valuation_Date, @LOB,@POLICY_NO,@CUSTMR_ID,@PGM_EFF_DT,@PGM_EXP_DT,@PGM_TYPE,@STATE,      
          @POL_EFF_DT,@POL_EXP_DT,@CLAIMNO,@ADDITIONALCLAIMIND,@ADDITIONALCLAIM,@CLAIMANTNAME,@CLAIMSTATUS,@COVERAGETRIGGERDATE,@LIMIT,@TOTALPAIDINDEMNITY,      
          @TOTALPAIDEXPENSE,@TOTALRESERVEDINDEMNITY,@TOTALRESERVEDEXPENSE,@NONBILLABLEPAIDINDEMNITY,@NONBILLABLEPAIDEXPENSE,@NONBILLABLERESERVEDINDEMNITY,      
          @NONBILLABLERESERVEDEXPENSE,@SYSTEMGENERATED,@CRTE_USER_ID,@CRTE_DT,@VALIDATE,@LOSS_INFO_COPY_STAGE_EXEC_ID      
      
          WHILE @@Fetch_Status = 0      
            BEGIN      
                BEGIN      
                    IF @debug = 1      
                      BEGIN      
                          PRINT '******************* Uploads: START OF ITERATION *********'      
                          PRINT '---------------Input Params-------------------'      
                          PRINT ' @Valuation_Date:- ' + @Valuation_Date      
                          PRINT ' @LOB:- ' + @LOB      
                          PRINT ' @POLICY_NO:- ' + @POLICY_NO      
                          PRINT ' @CUSTMR_ID: ' + @CUSTMR_ID      
                          PRINT ' @PGM_EFF_DT: ' + @PGM_EFF_DT      
                          PRINT ' @PGM_EXP_DT: ' + @PGM_EXP_DT      
                          PRINT ' @PGM_TYPE: '  + @PGM_TYPE      
                          PRINT ' @STATE: ' + @STATE      
                          PRINT ' @POL_EFF_DT: ' + @POL_EFF_DT      
                          PRINT ' @POL_EXP_DT:- ' + @POL_EXP_DT      
                          PRINT ' @CLAIMNO:- ' + @CLAIMNO      
                          PRINT ' @ADDITIONALCLAIMIND:- ' + @ADDITIONALCLAIMIND      
    PRINT ' @ADDITIONALCLAIM:- ' + @ADDITIONALCLAIM      
                          PRINT ' @CLAIMANTNAME:- ' + @CLAIMANTNAME      
                          PRINT ' @CLAIMSTATUS:- ' + @CLAIMSTATUS      
                          PRINT ' @COVERAGETRIGGERDATE:- ' + @COVERAGETRIGGERDATE      
                          PRINT ' @LIMIT:- ' + @LIMIT      
                          PRINT ' @TOTALPAIDINDEMNITY:- ' + @TOTALPAIDINDEMNITY      
						  PRINT ' @TOTALPAIDEXPENSE:- ' + @TOTALPAIDEXPENSE      
						  PRINT ' @TOTALRESERVEDINDEMNITY:- ' + @TOTALRESERVEDINDEMNITY      
						  PRINT ' @TOTALRESERVEDEXPENSE:- ' + @TOTALRESERVEDEXPENSE      
						  PRINT ' @NONBILLABLEPAIDINDEMNITY:- ' + @NONBILLABLEPAIDINDEMNITY      
						  PRINT ' @NONBILLABLEPAIDEXPENSE:- ' + @NONBILLABLEPAIDEXPENSE      
						  PRINT ' @NONBILLABLERESERVEDINDEMNITY:- ' + @NONBILLABLERESERVEDINDEMNITY      
						  PRINT ' @NONBILLABLERESERVEDEXPENSE:- ' + @NONBILLABLERESERVEDEXPENSE      
						  PRINT ' @SYSTEMGENERATED:- ' + @SYSTEMGENERATED      
                          PRINT ' @CRTE_USER_ID: ' + CONVERT(VARCHAR(50), @CRTE_USER_ID)      
						  PRINT ' @CRTE_DT: ' + CONVERT(VARCHAR(50), @CRTE_DT)      
                          PRINT ' @VALIDATE: ' + CONVERT(VARCHAR(50), @VALIDATE)      
                          PRINT ' @LOSS_INFO_COPY_STAGE_EXEC_ID: ' + CONVERT(VARCHAR(50), @LOSS_INFO_COPY_STAGE_EXEC_ID)      
                      END      
      
                    SET @Validation_Message = ''      
                    SET @Validation_Message_Valuation_Date = NULL      
                    SET @Validation_Message_PolicyNo = NULL      
                    SET @Validation_Message_CUSTMR_ID = NULL      
                    SET @Validation_Message_PGM_EFF_DT = NULL      
                    SET @Validation_Message_PGM_EXP_DT = NULL      
                    SET @Validation_Message_State_ID = NULL      
                    SET @Validation_Message_POL_EFF_DT = NULL      
                    SET @Validation_Message_POL_EXP_DT = NULL      
                    SET @Validation_Message_PGMPRD = NULL      
					SET @Validation_Message_COML_AGMT_ID = NULL      
					SET @Validation_Message_PREM_ADJ_ID = NULL      
					SET @Validation_Message_LOSS_INFO_COPY_STAGE_ID = NULL      
					SET @Validation_Message_AdditionalClaimInd = NULL      
					SET @Validation_Message_ClaimStatus = NULL      
					SET @Validation_Message_CoverageTriggerDate = NULL      
					SET @Validation_Message_Limit2 = NULL      
					SET @Validation_Message_Total_Paid_Indemnity = NULL      
					SET @Validation_Message_Total_Paid_Expense = NULL      
					SET @Validation_Message_Total_Reserved_Indemnity = NULL      
					SET @Validation_Message_Total_Reserved_Expense = NULL      
					SET @Validation_Message_NonBillable_Paid_Indemnity = NULL      
					SET @Validation_Message_NonBillable_Paid_Expense = NULL      
					SET @Validation_Message_NonBillable_Reserved_Indemnity = NULL      
					SET @Validation_Message_NonBillable_Reserved_Expense = NULL      
					SET @Validation_Message_System_Generated = NULL      
					SET @Validation_Message_LOB_ID=NULL    
                    -----------------------------------------------------------------------------------------------------                
                    --Validation Rule 1:                
                    --Valuation Date should not be blank      
                    -----------------------------------------------------------------------------------------------------                 
                    IF(ISNULL(@Valuation_Date, '') = '' OR ISDATE(@Valuation_Date) = 0)      
                      SET @Validation_Message = 'Valuation Date'      
      
                    IF( @Validation_Message <> '' )      
                      BEGIN      
      IF(ISNULL(@Valuation_Date, '') = '')      
        BEGIN      
       SET @Validation_Message_Valuation_Date = @Validation_Message + ' is missing.'      
             
       IF @debug = 1      
         BEGIN      
        PRINT 'Valuation Date Blank Data Validation Failed :'      
        PRINT '@Validation_Message_Valuation_Date: ' + CONVERT(VARCHAR(max),@Validation_Message_Valuation_Date)      
         END      
        END        
      ELSE       
        BEGIN      
       SET @Validation_Message_Valuation_Date = @Validation_Message + ' is not a valid format.'      
            
       IF @debug = 1      
         BEGIN      
        PRINT @Validation_Message + ' Format Validation Failed :'      
        PRINT '@Validation_Message_Valuation_Date: ' + CONVERT(VARCHAR(max),@Validation_Message_Valuation_Date)      
         END      
        END      
                      END            
      
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 2:                
                    --The given Policy No. should not be blank and should be valid        
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
     SET @Count_PolicyNo = NULL      
     SET @index = 0      
                    SET @index2 = 0      
                        
                    SET @index = CHARINDEX(' ',@POLICY_NO)      
     SET @index2 = CHARINDEX(' ',@POLICY_NO, @index + 1)      
           
     IF(@index > 0 AND @index2 > 0)      
     BEGIN      
      SELECT @Count_PolicyNo = COUNT(coml_agmt_id) FROM COML_AGMT WHERE pol_sym_txt = SUBSTRING (@POLICY_NO, 1 , @index - 1)      
       AND pol_nbr_txt = SUBSTRING (@POLICY_NO, @index + 1 , @index2 - (@index + 1))       
       AND pol_modulus_txt = SUBSTRING (@POLICY_NO, @index2 + 1 , LEN(@POLICY_NO)- @index2)      
     END      
           
     IF(@POLICY_NO IS NULL OR @Count_PolicyNo IS NULL OR @Count_PolicyNo <= 0)      
                      SET @Validation_Message = 'Policy No'      
      
     IF(@Validation_Message <> '')      
     BEGIN      
      IF(@POLICY_NO IS NULL)      
        BEGIN      
          SET @Validation_Message_PolicyNo = @Validation_Message + ' is missing.'      
      
       IF @debug = 1      
         BEGIN      
        PRINT @Validation_Message + ' Blank Data Validation Failed :'      
        PRINT '@Validation_Message_PolicyNo: ' + CONVERT(VARCHAR(max),@Validation_Message_PolicyNo)      
         END      
        END      
      ELSE      
        BEGIN         
       SET @Validation_Message_PolicyNo = @Validation_Message + ' doesn''t exist.'      
      
       IF @debug = 1      
         BEGIN      
        PRINT @Validation_Message + ' Matched Validation Failed :'      
        PRINT '@Validation_Message_PolicyNo: ' + CONVERT(VARCHAR(max),@Validation_Message_PolicyNo)      
         END      
        END      
                      END              
      
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 3:                
                    -- • The Given Customer id is valid           
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
                    SET @CustmrID = NULL      
           
     IF(ISNUMERIC(@CUSTMR_ID) = 1)      
      SELECT @CustmrID = custmr_id FROM custmr WHERE  custmr_id = @CUSTMR_ID      
             
                    IF(@CUSTMR_ID IS NULL OR @CustmrID IS NULL OR @CustmrID <= 0)      
                      SET @Validation_Message = 'Customer ID'      
    
                IF(@Validation_Message <> '')      
                      BEGIN      
      IF(@CUSTMR_ID IS NULL)      
        BEGIN      
          SET @Validation_Message_CUSTMR_ID = @Validation_Message + ' is missing.'      
      
       IF @debug = 1      
         BEGIN      
        PRINT @Validation_Message + ' Blank Data Validation Failed :'      
        PRINT '@Validation_Message_CUSTMR_ID: ' + CONVERT(VARCHAR(max),@Validation_Message_CUSTMR_ID)      
         END      
        END      
      ELSE IF(ISNUMERIC(@CUSTMR_ID) = 0)        
        BEGIN      
          SET @Validation_Message_CUSTMR_ID = @Validation_Message + ' is not a valid format.'      
      
       IF @debug = 1      
         BEGIN      
        PRINT @Validation_Message + ' Format Validation Failed :'      
        PRINT '@Validation_Message_CUSTMR_ID: ' + CONVERT(VARCHAR(max),@Validation_Message_CUSTMR_ID)      
         END      
        END      
      ELSE      
        BEGIN         
       SET @Validation_Message_CUSTMR_ID = @Validation_Message + ' doesn''t exist.'      
      
       IF @debug = 1      
         BEGIN      
        PRINT @Validation_Message + ' Matched Validation Failed :'      
        PRINT '@Validation_Message_CUSTMR_ID: ' + CONVERT(VARCHAR(max),@Validation_Message_CUSTMR_ID)      
         END      
        END      
                      END      
                            
                    -----------------------------------------------------------------------------------------------------                
                    --Validation Rule 4:                
                    --Program Period Effective Date should not be blank      
                    -----------------------------------------------------------------------------------------------------        
                    --Variable initialization                    
                    SET @Validation_Message = ''      
                                   
                    IF(ISNULL(@PGM_EFF_DT, '') = '' OR ISDATE(@PGM_EFF_DT) = 0)      
                      SET @Validation_Message = 'Program Period Eff Date'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      IF(ISNULL(@PGM_EFF_DT, '') = '')      
       BEGIN      
        SET @Validation_Message_PGM_EFF_DT = @Validation_Message + ' is missing.'      
      
        IF @debug = 1      
          BEGIN      
         PRINT 'Program Period Effective Date Blank Data Validation Failed :'      
         PRINT '@Validation_Message_PGM_EFF_DT: ' + CONVERT(VARCHAR(max),@Validation_Message_PGM_EFF_DT)      
          END      
       END      
      ELSE      
       BEGIN      
        SET @Validation_Message_PGM_EFF_DT = @Validation_Message + ' is not a valid format.'      
      
        IF @debug = 1      
          BEGIN      
         PRINT 'Program Period Effective Date format Validation Failed :'      
         PRINT '@Validation_Message_PGM_EFF_DT: ' + CONVERT(VARCHAR(max),@Validation_Message_PGM_EFF_DT)      
          END      
       END      
                      END       
                            
                    -----------------------------------------------------------------------------------------------------                
                    --Validation Rule 5:                
                    --Program Period Expiration Date should not be blank      
                    -----------------------------------------------------------------------------------------------------        
                    --Variable initialization                    
                    SET @Validation_Message = ''      
                                   
                    IF(ISNULL(@PGM_EXP_DT, '') = '' OR ISDATE(@PGM_EXP_DT) = 0)      
                      SET @Validation_Message = 'Program Period Exp Date'      
      
           IF(@Validation_Message <> '')      
                      BEGIN      
      IF(ISNULL(@PGM_EXP_DT, '') = '')      
        BEGIN      
       SET @Validation_Message_PGM_EXP_DT = @Validation_Message + ' is missing.'      
      
       IF @debug = 1      
         BEGIN      
        PRINT 'Program Period Expiration Date Blank Data Validation Failed :'      
        PRINT '@Validation_Message_PGM_EXP_DT: ' + CONVERT(VARCHAR(max),@Validation_Message_PGM_EXP_DT)    
         END      
        END      
      ELSE      
        BEGIN      
       SET @Validation_Message_PGM_EXP_DT = @Validation_Message + ' is not a valid format.'      
      
       IF @debug = 1      
         BEGIN      
        PRINT 'Program Period Expiration Date format Validation Failed :'      
        PRINT '@Validation_Message_PGM_EXP_DT: ' + CONVERT(VARCHAR(max),@Validation_Message_PGM_EXP_DT)      
         END      
        END      
                      END      
                            
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 6:                
                    -- • The Given State id is valid           
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
                    SET @StateID = NULL      
              
                    SELECT @StateID = lkup_id FROM LKUP WHERE attr_1_txt = @State AND lkup_typ_id = 1      
                          
                    IF(@State IS NULL OR @StateID IS NULL OR @StateID <= 0)      
                      SET @Validation_Message = 'State'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      IF(@State IS NULL)      
        BEGIN      
          SET @Validation_Message_State_ID = @Validation_Message + ' is missing.'      
      
       IF @debug = 1      
         BEGIN     
        PRINT 'State Blank Data Validation Failed :'      
        PRINT '@Validation_Message_State_ID: ' + CONVERT(VARCHAR(max),@Validation_Message_State_ID)      
         END      
        END      
      ELSE      
        BEGIN         
       SET @Validation_Message_State_ID = @Validation_Message + ' doesn''t exist.'      
      
       IF @debug = 1      
         BEGIN      
        PRINT 'State Matched Validation Failed :'      
        PRINT '@Validation_Message_State_ID: ' + CONVERT(VARCHAR(max),@Validation_Message_State_ID)      
         END      
        END      
                      END      
                          
                    -----------------------------------------------------------------------------------------------------                
                    --Validation Rule 7:                
                    --Policy Effective Date should not be blank      
                    -----------------------------------------------------------------------------------------------------        
                    --Variable initialization                 
     SET @Validation_Message = ''      
                                   
                    IF(ISNULL(@POL_EFF_DT, '') = '' OR ISDATE(@POL_EFF_DT) = 0)      
                      SET @Validation_Message = 'Policy Eff Date'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
                      IF(ISNULL(@POL_EFF_DT, '') = '')      
        BEGIN      
                        SET @Validation_Message_POL_EFF_DT = @Validation_Message + ' is missing.'      
      
       IF @debug = 1      
         BEGIN      
        PRINT 'Policy Effective Date Blank Data Validation Failed :'      
        PRINT '@Validation_Message_POL_EFF_DT: ' + CONVERT(VARCHAR(max),@Validation_Message_POL_EFF_DT)      
         END      
        END      
                      ELSE      
                      BEGIN      
                       SET @Validation_Message_POL_EFF_DT = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT 'Policy Effective Date format Validation Failed :'      
       PRINT '@Validation_Message_POL_EFF_DT: ' + CONVERT(VARCHAR(max),@Validation_Message_POL_EFF_DT)      
        END      
                      END      
      
                      END       
                          
                    -----------------------------------------------------------------------------------------------------                
                    --Validation Rule 8:                
                    --Policy Expiration Date should not be blank      
                    -----------------------------------------------------------------------------------------------------       
                    --Variable initialization                    
                    SET @Validation_Message = ''      
                                    
                    IF(ISNULL(@POL_EXP_DT, '') = '' OR ISDATE(@POL_EFF_DT) = 0)      
                      SET @Validation_Message = 'Policy Exp Date'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
        IF(ISNULL(@POL_EXP_DT, '') = '')      
         BEGIN      
                         SET @Validation_Message_POL_EXP_DT = @Validation_Message + ' is missing.'      
      
        IF @debug = 1      
          BEGIN      
         PRINT 'Policy Expiration Date Blank Data Validation Failed :'      
         PRINT '@Validation_Message_POL_EXP_DT: ' + CONVERT(VARCHAR(max),@Validation_Message_POL_EXP_DT)      
          END      
         END      
        ELSE      
         BEGIN      
                         SET @Validation_Message_POL_EXP_DT = @Validation_Message + ' is not a valid format.'      
      
        IF @debug = 1      
          BEGIN      
         PRINT 'Policy Expiration Date format Validation Failed :'      
         PRINT '@Validation_Message_POL_EXP_DT: ' + CONVERT(VARCHAR(max),@Validation_Message_POL_EXP_DT)      
          END      
         END      
                      END      
                              
                    -----------------------------------------------------------------------------------------------------                
                    --Validation Rule 9:                
                    -- A valid Policy should exist with given data      
                    -----------------------------------------------------------------------------------------------------       
                    --Variable initialization                    
                    SET @Validation_Message = ''      
                    SET @Coml_Agmt_Id = NULL      
                    SET @Prem_Adj_Pgm_ID = NULL    
                    SET @Covg_Typ_ID = NULL     
                    SET @index = 0      
                    SET @index2 = 0      
                          
                    SET @index = CHARINDEX(' ',@POLICY_NO)      
     SET @index2 = CHARINDEX(' ',@POLICY_NO, @index + 1)      
            
     IF(@POL_EFF_DT IS NOT NULL AND ISDATE(@POL_EFF_DT) = 1 AND @POL_EXP_DT IS NOT NULL AND ISDATE(@POL_EXP_DT) = 1 AND @index > 0 AND @index2 > 0)      
     BEGIN          
      SELECT @Coml_Agmt_Id = coml_agmt_id, @Prem_Adj_Pgm_ID = prem_adj_pgm_id, @Covg_Typ_ID = covg_typ_id FROM COML_AGMT WHERE pol_sym_txt = SUBSTRING (@POLICY_NO, 1 , @index - 1)      
      AND pol_nbr_txt = SUBSTRING (@POLICY_NO, @index + 1 , @index2 - (@index + 1))       
      AND pol_modulus_txt = SUBSTRING (@POLICY_NO, @index2 + 1 , LEN(@POLICY_NO)- @index2)       
      AND custmr_id = @CustmrID AND pol_eff_dt = CONVERT(DATETIME,@POL_EFF_DT,101) AND planned_end_date = CONVERT(DATETIME,@POL_EXP_DT,101)       
     END      
                                    
                    IF((@POLICY_NO IS NOT NULL AND @Count_PolicyNo IS NOT NULL AND @Count_PolicyNo > 0) AND (@CustmrID IS NOT NULL AND @CustmrID > 0) AND      
                      (@Coml_Agmt_Id IS NULL OR @Coml_Agmt_Id <= 0))      
                      SET @Validation_Message = 'Policy'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_COML_AGMT_ID = @Validation_Message + ' doesn''t exist.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT 'Policy Matched Validation Failed :'      
       PRINT '@Validation_Message_COML_AGMT_ID: ' + CONVERT(VARCHAR(max),@Validation_Message_COML_AGMT_ID)      
        END      
                      END    
                          
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 2_New:                
                    -- • The Given LOB is valid           
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
                    SET @LOBID = NULL      
              
     SELECT @LOBID = LKUP.lkup_id FROM LKUP       
                    INNER JOIN LKUP_TYP TYP ON LKUP.lkup_typ_id = TYP.lkup_typ_id      
     WHERE TYP.lkup_typ_nm_txt = 'LOB COVERAGE' AND LKUP.lkup_txt = @LOB      
              
                    IF(@LOB IS NULL OR @LOBID IS NULL OR @LOBID <= 0 OR @Covg_Typ_ID <> @LOBID)      
                      SET @Validation_Message = 'LOB'      
      
                    IF(@Validation_Message <> '')      
                    BEGIN      
      IF(@LOB IS NULL)      
      BEGIN      
       SET @Validation_Message_LOB_ID = @Validation_Message + ' is missing.'      
    
       IF @debug = 1      
       BEGIN      
        PRINT 'LOB Blank Data Validation Failed :'      
        PRINT '@Validation_Message_LOB_ID: ' + CONVERT(VARCHAR(max),@Validation_Message_LOB_ID)      
       END      
      END      
      ELSE IF(@LOB IS NOT NULL AND (@LOBID IS NULL OR @LOBID <= 0))     
      BEGIN    
       SET @Validation_Message_LOB_ID = @Validation_Message + ' doesn''t exist.'      
    
       IF @debug = 1      
       BEGIN      
        PRINT 'LOB Matched Validation Failed :'      
        PRINT '@Validation_Message_LOB_ID: ' + CONVERT(VARCHAR(max),@Validation_Message_LOB_ID)      
       END      
      END    
      ELSE    
      BEGIN    
       SET @Validation_Message_LOB_ID = @Validation_Message + ' is not assosiated with policy.'      
    
       IF @debug = 1      
       BEGIN      
        PRINT 'LOB Assosiation with Policy Failed :'      
        PRINT '@Validation_Message_LOB_ID: ' + CONVERT(VARCHAR(max),@Validation_Message_LOB_ID)      
       END      
      END    
                    END    
                           
                    -----------------------------------------------------------------------------------------------------                
                    --Validation Rule 10:                
                    -- A valid Program Period should exist with given data      
                    -----------------------------------------------------------------------------------------------------       
                    --Variable initialization                    
                    SET @Validation_Message = ''      
           
     IF(@PGM_EFF_DT IS NOT NULL AND ISDATE(@PGM_EFF_DT) = 1 AND @PGM_EXP_DT IS NOT NULL AND ISDATE(@PGM_EXP_DT) = 1)      
     BEGIN      
      IF NOT EXISTS(SELECT 1 FROM PREM_ADJ_PGM PGM       
      WHERE PGM.strt_dt = CONVERT(DATETIME,@PGM_EFF_DT,101) AND PGM.plan_end_dt = CONVERT(DATETIME,@PGM_EXP_DT,101)       
      AND PGM.custmr_id = @CustmrID AND PGM.actv_ind=1 AND PGM.PREM_ADJ_PGM_ID = @Prem_Adj_Pgm_ID)      
      BEGIN      
       SET @Prem_Adj_Pgm_ID = NULL      
      END      
     END      
                                    
                    IF((@Coml_Agmt_Id IS NOT NULL OR @Coml_Agmt_Id > 0) AND (@Prem_Adj_Pgm_ID IS NULL OR @Prem_Adj_Pgm_ID <= 0))      
              SET @Validation_Message = 'Program Period'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_PGMPRD = @Validation_Message + ' doesn''t exist.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT 'Program Period Matched Validation Failed :'      
       PRINT '@Validation_Message_PGMPRD: ' + CONVERT(VARCHAR(max),@Validation_Message_PGMPRD)      
        END      
                      END      
                            
                    -----------------------------------------------------------------------------------------------------                
                    --Validation Rule 11:                
                    -- A valid Adjustment should exist with given data      
                    -----------------------------------------------------------------------------------------------------       
                    --Variable initialization                    
                    SET @Validation_Message = ''      
                    SET @Prem_Adj_ID = NULL      
                    SET @Adj_Status_ID = NULL      
            
     IF(@Valuation_Date IS NOT NULL AND ISDATE(@Valuation_Date) = 1)      
  BEGIN 
            
      SELECT @Prem_Adj_ID = prem_adj_id, @Adj_Status_ID = adj_sts_typ_id FROM PREM_ADJ       
      WHERE reg_custmr_id = @CustmrID 
	  AND valn_dt = CONVERT(DATETIME,@Valuation_Date,101) 
	  AND ADJ_CAN_IND<>1 
	  AND ADJ_VOID_IND<>1
	  AND ADJ_RRSN_IND<>1
	  AND SUBSTRING(FNL_INVC_NBR_TXT,1,3)<>'RTV'   
	     
     END      
                                    
                    IF(@Prem_Adj_ID IS NOT NULL AND @Prem_Adj_ID > 0 AND @Adj_Status_ID <> 346)      
                      SET @Validation_Message = 'Adjustment'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_PREM_ADJ_ID = @Validation_Message + ' not in CALC status.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT 'Adjustment''s Status Type Validation Failed :'      
       PRINT '@Validation_Message_PREM_ADJ_ID: ' + CONVERT(VARCHAR(max),@Validation_Message_PREM_ADJ_ID)      
        END      
                      END      
                            
                    -----------------------------------------------------------------------------------------------------               
                    --Validation Rule 12:                
                    -- A valid Loss Policy should exist with given data      
                    -----------------------------------------------------------------------------------------------------       
                    --Variable initialization                    
                    SET @Validation_Message = ''      
                    SET @Armis_los_pol_id = NULL      
                    SET @LOSS_INFO_COPY_STAGE_ID = NULL      
           
     IF(ISDATE(@Valuation_Date) = 1)      
     BEGIN           
      SELECT @Armis_los_pol_id = armis_los_pol_id FROM ARMIS_LOS_POL WHERE coml_agmt_id = @Coml_Agmt_Id AND prem_adj_pgm_id = @Prem_Adj_Pgm_ID      
      AND custmr_id = @CustmrID AND st_id = @StateID AND valn_dt = CONVERT(DATETIME,@Valuation_Date,101) AND isnull(prem_adj_id,0) = isnull(@Prem_Adj_ID,0)      
     END      
           
     IF(@Armis_los_pol_id IS NULL OR @Armis_los_pol_id <= 0)      
     BEGIN      
           
     SELECT @LOSS_INFO_COPY_STAGE_ID= LOSS_INFO_COPY_STAGE_ID FROM LOSS_INFO_COPY_STAGE_POL WHERE Valuation_Date=@Valuation_Date AND LOB=@LOB AND POLICY_NO=@POLICY_NO AND       
     CUSTMR_ID=@CUSTMR_ID AND PGM_EFF_DT=@PGM_EFF_DT AND PGM_EXP_DT=@PGM_EXP_DT AND PGM_TYPE=@PGM_TYPE AND [STATE]=@STATE      
     AND POL_EFF_DT=@POL_EFF_DT AND POL_EXP_DT=@POL_EXP_DT AND       
     NOT EXISTS (SELECT TOP 1 1 FROM LOSS_INFO_COPY_STAGE_POL_STATUSLOG WHERE Valuation_Date=@Valuation_Date AND LOB=@LOB AND POLICY_NO=@POLICY_NO AND       
     CUSTMR_ID=@CUSTMR_ID AND PGM_EFF_DT=@PGM_EFF_DT AND PGM_EXP_DT=@PGM_EXP_DT AND PGM_TYPE=@PGM_TYPE AND [STATE]=@STATE      
     AND POL_EFF_DT=@POL_EFF_DT AND POL_EXP_DT=@POL_EXP_DT AND CRTE_DT=@dtUploadDateTime)      
           
     END      
                                    
                    IF((@Armis_los_pol_id IS NULL OR @Armis_los_pol_id <= 0) AND (@LOSS_INFO_COPY_STAGE_ID IS NULL OR @LOSS_INFO_COPY_STAGE_ID <= 0))      
                      SET @Validation_Message = 'Loss Policy'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_LOSS_INFO_COPY_STAGE_ID = @Validation_Message + ' doesn''t exist.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT @Validation_Message + ' Matched Validation Failed :'      
       PRINT '@Validation_Message_LOSS_INFO_COPY_STAGE_ID: ' + CONVERT(VARCHAR(max),@Validation_Message_LOSS_INFO_COPY_STAGE_ID)      
        END      
                      END                          
                          
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 13:                
                    -- • The Given Additional Claim Ind is valid           
                    ---------------------------------------------------------------------------------------     
                    --Variable initialization                    
                    SET @Validation_Message = ''      
           
     IF(NOT(LOWER(@ADDITIONALCLAIMIND) = 'true' OR LOWER(@ADDITIONALCLAIMIND) = 'false' OR LOWER(@ADDITIONALCLAIMIND) = 'yes' OR      
        LOWER(@ADDITIONALCLAIMIND) = 'no' OR @ADDITIONALCLAIMIND = '1' OR @ADDITIONALCLAIMIND = '0'))      
      SET @Validation_Message = 'Additional Claim Ind'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_AdditionalClaimInd = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT 'Additional Claim Ind Format Validation Failed :'      
       PRINT '@Validation_Message_AdditionalClaimInd: ' + CONVERT(VARCHAR(max),@Validation_Message_AdditionalClaimInd)      
        END      
                      END         
                           
                    -----------------------------------------------------------------------------------------------------                
                    --Validation Rule 14:                
                    -- A valid Claim Status should exist with given data      
                    -----------------------------------------------------------------------------------------------------       
                    --Variable initialization                                      SET @Validation_Message = ''      
                    SET @ClaimStatusID = NULL      
           
     IF(@CLAIMSTATUS IS NOT NULL)      
     BEGIN           
      SELECT @ClaimStatusID = LKUP.lkup_id FROM LKUP       
      INNER JOIN LKUP_TYP TYP ON LKUP.lkup_typ_id = TYP.lkup_typ_id      
      WHERE TYP.lkup_typ_nm_txt = 'CLAIM STATUS' AND LKUP.lkup_txt = @CLAIMSTATUS      
     END      
                                    
                    IF(ISNULL(@CLAIMSTATUS,'') <> '' AND (@ClaimStatusID IS NULL OR @ClaimStatusID <= 0))      
                      SET @Validation_Message = 'Claim Status'      
      
         IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_ClaimStatus = @Validation_Message + ' doesn''t exist.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT @Validation_Message + ' Matched Validation Failed :'      
       PRINT '@Validation_Message_ClaimStatus: ' + CONVERT(VARCHAR(max),@Validation_Message_ClaimStatus)      
        END      
                      END       
                
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 15:                
                    -- • The Given Coverage Trigger Date is valid           
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
           
     IF(ISNULL(@COVERAGETRIGGERDATE,'') <> '' AND ISDATE(@COVERAGETRIGGERDATE) = 0)      
      SET @Validation_Message = 'Coverage Trigger Date'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_CoverageTriggerDate = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT 'Coverage Trigger Date Format Validation Failed :'      
       PRINT '@Validation_Message_CoverageTriggerDate: ' + CONVERT(VARCHAR(max),@Validation_Message_CoverageTriggerDate)      
        END      
                      END        
                            
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 16:                
                    -- • The Given Limit 2 is valid           
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
           
     IF(ISNULL(@LIMIT,'') <> '' AND ISNUMERIC(@LIMIT) = 0)      
      SET @Validation_Message = 'Limit 2'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_Limit2 = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT @Validation_Message + ' Format Validation Failed :'      
       PRINT '@Validation_Message_Limit2: ' + CONVERT(VARCHAR(max),@Validation_Message_Limit2)      
        END      
                      END       
                            
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 17:                
                    -- • The Given Total Paid Indemnity is valid           
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
           
     IF(@TOTALPAIDINDEMNITY IS NOT NULL AND @TOTALPAIDINDEMNITY <> '' AND ISNUMERIC(@TOTALPAIDINDEMNITY) = 0)      
      SET @Validation_Message = 'Total Paid Indemnity'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_Total_Paid_Indemnity = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT @Validation_Message + ' Format Validation Failed :'      
       PRINT '@Validation_Message_Total_Paid_Indemnity: ' + CONVERT(VARCHAR(max),@Validation_Message_Total_Paid_Indemnity)     
        END      
              END         
                           
             ---------------------------------------------------------------------------------------                
                    --Validation Rule 18:                
                    -- • The Given Total Paid Expense is valid           
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
           
     IF(@TOTALPAIDEXPENSE IS NOT NULL AND @TOTALPAIDEXPENSE <> '' AND ISNUMERIC(@TOTALPAIDEXPENSE) = 0)      
      SET @Validation_Message = 'Total Paid Expense'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_Total_Paid_Expense = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT @Validation_Message + ' Format Validation Failed :'      
       PRINT '@Validation_Message_Total_Paid_Expense: ' + CONVERT(VARCHAR(max),@Validation_Message_Total_Paid_Expense)      
        END      
                      END        
                            
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 19:                
                    -- • The Given Total Reserved Indemnity is valid           
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
           
     IF(@TOTALRESERVEDINDEMNITY IS NOT NULL AND @TOTALRESERVEDINDEMNITY <> '' AND ISNUMERIC(@TOTALRESERVEDINDEMNITY) = 0)      
      SET @Validation_Message = 'Total Reserved Indemnity'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_Total_Reserved_Indemnity = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT @Validation_Message + ' Format Validation Failed :'      
       PRINT '@Validation_Message_Total_Reserved_Indemnity: ' + CONVERT(VARCHAR(max),@Validation_Message_Total_Reserved_Indemnity)      
        END      
                      END       
                            
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 20:                
                    -- • The Given Total Reserved Expense is valid           
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
           
     IF(@TOTALRESERVEDEXPENSE IS NOT NULL AND @TOTALRESERVEDEXPENSE <> '' AND ISNUMERIC(@TOTALRESERVEDEXPENSE) = 0)      
      SET @Validation_Message = 'Total Reserved Expense'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_Total_Reserved_Expense = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT @Validation_Message + ' Format Validation Failed :'      
       PRINT '@Validation_Message_Total_Reserved_Expense: ' + CONVERT(VARCHAR(max),@Validation_Message_Total_Reserved_Expense)      
        END      
                      END      
                          
                    ---------------------------------------------------------------------------------------                
        --Validation Rule 21: 
                    -- • The Given NonBillable Paid Indemnity is valid           
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
           
     IF(@NONBILLABLEPAIDINDEMNITY IS NOT NULL AND @NONBILLABLEPAIDINDEMNITY <> '' AND ISNUMERIC(@NONBILLABLEPAIDINDEMNITY) = 0)      
      SET @Validation_Message = 'NonBillable Paid Indemnity'      
      
                    IF(@Validation_Message <> '')      
        BEGIN      
      SET @Validation_Message_NonBillable_Paid_Indemnity = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT @Validation_Message + ' Format Validation Failed :'      
       PRINT '@Validation_Message_NonBillable_Paid_Indemnity: ' + CONVERT(VARCHAR(max),@Validation_Message_NonBillable_Paid_Indemnity)      
        END      
                      END         
                           
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 22:                
                    -- • The Given NonBillable Paid Expense is valid           
                    ---------------------------------------------------------------------------------------                
     --Variable initialization         
                    SET @Validation_Message = ''      
           
     IF(@NONBILLABLEPAIDEXPENSE IS NOT NULL AND @NONBILLABLEPAIDEXPENSE <> '' AND ISNUMERIC(@NONBILLABLEPAIDEXPENSE) = 0)      
      SET @Validation_Message = 'NonBillable Paid Expense'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_NonBillable_Paid_Expense = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT @Validation_Message + ' Format Validation Failed :'      
       PRINT '@Validation_Message_NonBillable_Paid_Expense: ' + CONVERT(VARCHAR(max),@Validation_Message_NonBillable_Paid_Expense)      
        END      
                      END        
                            
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 23:                
                    -- • The Given NonBillable Reserved Indemnity is valid           
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
           
     IF(@NONBILLABLERESERVEDINDEMNITY IS NOT NULL AND @NONBILLABLERESERVEDINDEMNITY <> '' AND ISNUMERIC(@NONBILLABLERESERVEDINDEMNITY) = 0)      
      SET @Validation_Message = 'NonBillable Reserved Indemnity'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_NonBillable_Reserved_Indemnity = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT @Validation_Message + ' Format Validation Failed :'      
       PRINT '@Validation_Message_NonBillable_Reserved_Indemnity: ' + CONVERT(VARCHAR(max),@Validation_Message_NonBillable_Reserved_Indemnity)      
        END      
                      END       
                            
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 24:                
                    -- • The Given NonBillable Reserved Expense is valid   
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
           
     IF(@NONBILLABLERESERVEDEXPENSE IS NOT NULL AND @NONBILLABLERESERVEDEXPENSE <> '' AND ISNUMERIC(@NONBILLABLERESERVEDEXPENSE) = 0)      
      SET @Validation_Message = 'NonBillable Reserved Expense'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_NonBillable_Reserved_Expense = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT @Validation_Message + ' Format Validation Failed :'      
       PRINT '@Validation_Message_NonBillable_Reserved_Expense: ' + CONVERT(VARCHAR(max),@Validation_Message_NonBillable_Reserved_Expense)      
        END      
                      END      
                           
                           
                   
                    ---------------------------------------------------------------------------------------      
                    --Validation Rule 25:                
                    -- • The Given System Generated is valid           
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
           
     IF(NOT(LOWER(@SYSTEMGENERATED) = 'yes' OR LOWER(@SYSTEMGENERATED) = 'no' OR LOWER(@SYSTEMGENERATED) = 'true' OR LOWER(@SYSTEMGENERATED) = 'false'      
        OR @SYSTEMGENERATED = '1' OR @SYSTEMGENERATED = '0'))      
      SET @Validation_Message = 'System Generated'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_System_Generated = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT 'System Generated :'      
       PRINT '@Validation_Message_System_Generated: ' + CONVERT(VARCHAR(max),@Validation_Message_System_Generated)      
        END      
                      END      
                            
                    IF(ISNULL(@Validation_Message_Valuation_Date, '') <> '' OR ISNULL(@Validation_Message_PolicyNo, '') <> '' OR      
                       ISNULL(@Validation_Message_CUSTMR_ID, '') <> '' OR ISNULL(@Validation_Message_PGM_EFF_DT, '') <> '' OR      
                       ISNULL(@Validation_Message_PGM_EXP_DT, '') <> '' OR ISNULL(@Validation_Message_State_ID, '') <> '' OR      
                       ISNULL(@Validation_Message_POL_EFF_DT, '') <> '' OR ISNULL(@Validation_Message_POL_EXP_DT, '') <> '' OR      
                       ISNULL(@Validation_Message_PGMPRD, '') <> '' OR ISNULL(@Validation_Message_COML_AGMT_ID, '') <> '' OR      
                       ISNULL(@Validation_Message_PREM_ADJ_ID, '') <> '' OR ISNULL(@Validation_Message_LOSS_INFO_COPY_STAGE_ID,'') <> '' OR       
                       ISNULL(@Validation_Message_AdditionalClaimInd, '') <> '' OR ISNULL(@Validation_Message_ClaimStatus, '') <> '' OR      
                       ISNULL(@Validation_Message_CoverageTriggerDate, '') <> '' OR ISNULL(@Validation_Message_Limit2, '') <> '' OR      
                       ISNULL(@Validation_Message_Total_Paid_Indemnity, '') <> '' OR ISNULL(@Validation_Message_Total_Paid_Expense, '') <> '' OR       
                       ISNULL(@Validation_Message_Total_Reserved_Indemnity, '') <> '' OR ISNULL(@Validation_Message_Total_Reserved_Expense, '') <> '' OR       
                       ISNULL(@Validation_Message_NonBillable_Paid_Indemnity, '') <> '' OR ISNULL(@Validation_Message_NonBillable_Paid_Expense, '') <> '' OR       
                       ISNULL(@Validation_Message_NonBillable_Reserved_Indemnity, '') <> '' OR ISNULL(@Validation_Message_NonBillable_Reserved_Expense, '') <> ''       
                       OR ISNULL(@Validation_Message_System_Generated, '') <> '' OR ISNULL(@Validation_Message_LOB_ID, '') <> '')      
                      BEGIN                     
      INSERT INTO [dbo].[LOSS_INFO_COPY_STAGE_EXEC_STATUSLOG]      
      (      
       [Valuation_Date],[LOB],[POLICY_NO],[CUSTMR_ID],[PGM_EFF_DT],[PGM_EXP_DT],[PGM_TYPE],[STATE],[POL_EFF_DT],      
       [POL_EXP_DT],[CLM_NBR_TXT],[ADDN_CLM_IND],[ADDN_CLM_TXT],[CLMT_NM],[CLM_STS_ID],[COVG_TRIGR_DT],[LIM2_AMT],      
       [LOS_PAID_IDNMTY_AMT],[LOS_PAID_EXPS_AMT],[LOS_RESRV_IDNMTY_AMT],[LOS_RESRV_EXPS_AMT],[NON_BILABL_PAID_IDNMTY_AMT],      
       [NON_BILABL_PAID_EXPS_AMT],[NON_BILABL_RESRVD_IDNMTY_AMT],[NON_BILABL_RESRVD_EXPS_AMT],[LOS_SYS_GENRT_IND],      
       [CRTE_USER_ID],[CRTE_DT],[VALIDATE],[TXTSTATUS],[TXTERRORDESC]      
      )      
                        VALUES            
       (      
       @Valuation_Date, @LOB, @POLICY_NO, @CUSTMR_ID, @PGM_EFF_DT, @PGM_EXP_DT, @PGM_TYPE, @STATE, @POL_EFF_DT,       
       @POL_EXP_DT, @CLAIMNO,@ADDITIONALCLAIMIND,@ADDITIONALCLAIM,@CLAIMANTNAME,@CLAIMSTATUS,@COVERAGETRIGGERDATE,@LIMIT,      
       @TOTALPAIDINDEMNITY,@TOTALPAIDEXPENSE,@TOTALRESERVEDINDEMNITY,@TOTALRESERVEDEXPENSE,@NONBILLABLEPAIDINDEMNITY,      
  @NONBILLABLEPAIDEXPENSE,@NONBILLABLERESERVEDINDEMNITY,@NONBILLABLERESERVEDEXPENSE,@SYSTEMGENERATED,@CRTE_USER_ID,      
       @CRTE_DT,@VALIDATE, 'Error',      
       LTRIM(RTRIM(ISNULL(@Validation_Message_Valuation_Date + ' ', '') + ISNULL(@Validation_Message_PolicyNo + ' ','') +       
       ISNULL(@Validation_Message_CUSTMR_ID + ' ','') + ISNULL(@Validation_Message_PGM_EFF_DT + ' ','') +       
       ISNULL(@Validation_Message_PGM_EXP_DT + ' ','') + ISNULL(@Validation_Message_State_ID + ' ','') +       
       ISNULL(@Validation_Message_POL_EFF_DT  + ' ','') + ISNULL(@Validation_Message_POL_EXP_DT + ' ','') +       
       ISNULL(@Validation_Message_PGMPRD + ' ','') + ISNULL(@Validation_Message_COML_AGMT_ID + ' ','') +      
       ISNULL(@Validation_Message_LOB_ID + ' ','') + ISNULL(@Validation_Message_PREM_ADJ_ID  + ' ','') +     
       ISNULL(@Validation_Message_LOSS_INFO_COPY_STAGE_ID + ' ','') + ISNULL(@Validation_Message_AdditionalClaimInd + ' ','') +     
       ISNULL(@Validation_Message_ClaimStatus + ' ','') + ISNULL(@Validation_Message_CoverageTriggerDate + ' ','') +     
       ISNULL(@Validation_Message_Limit2 + ' ','') + ISNULL(@Validation_Message_Total_Paid_Indemnity + ' ','') +     
       ISNULL(@Validation_Message_Total_Paid_Expense + ' ','') + ISNULL(@Validation_Message_Total_Reserved_Indemnity + ' ','') +     
       ISNULL(@Validation_Message_Total_Reserved_Expense + ' ','') + ISNULL(@Validation_Message_NonBillable_Paid_Indemnity + ' ','') +     
       ISNULL(@Validation_Message_NonBillable_Paid_Expense + ' ','') + ISNULL(@Validation_Message_NonBillable_Reserved_Indemnity + ' ','') +     
       ISNULL(@Validation_Message_NonBillable_Reserved_Expense + ' ','') + ISNULL(@Validation_Message_System_Generated,'')))      
                         )      
      
                         ----Skip this record as it is having blank or NULL values                    
                         FETCH LOSS_INFO_COPY_STAGE_EXCESS_Basic INTO @Valuation_Date, @LOB,@POLICY_NO,@CUSTMR_ID,@PGM_EFF_DT,@PGM_EXP_DT,@PGM_TYPE,@STATE,      
       @POL_EFF_DT,@POL_EXP_DT,@CLAIMNO,@ADDITIONALCLAIMIND,@ADDITIONALCLAIM,@CLAIMANTNAME,@CLAIMSTATUS,@COVERAGETRIGGERDATE,@LIMIT,      
       @TOTALPAIDINDEMNITY,@TOTALPAIDEXPENSE,@TOTALRESERVEDINDEMNITY,@TOTALRESERVEDEXPENSE,@NONBILLABLEPAIDINDEMNITY,@NONBILLABLEPAIDEXPENSE,      
       @NONBILLABLERESERVEDINDEMNITY,@NONBILLABLERESERVEDEXPENSE,@SYSTEMGENERATED,@CRTE_USER_ID,@CRTE_DT,@VALIDATE,@LOSS_INFO_COPY_STAGE_EXEC_ID      
CONTINUE      
                      END       
                 
           SELECT @intUserID = pers_id FROM pers WHERE external_reference = @CRTE_USER_ID      
      
                    ---------------------------------------------------------------------------------------                
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message=''      
      
                    IF @debug = 1      
                      BEGIN      
                          PRINT 'Validation Completed successfully:Insert/Update progress'      
                          PRINT '@intUserID: ' + CONVERT(VARCHAR(max), @intUserID)      
                      END      
      
      
                    IF(@Validate = 0 AND @Armis_los_pol_id IS NOT NULL AND @Armis_los_pol_id > 0 AND @Coml_Agmt_Id IS NOT NULL AND @Coml_Agmt_Id > 0      
                    AND @CustmrID IS NOT NULL AND @CustmrID > 0 AND @Prem_Adj_Pgm_ID IS NOT NULL AND @Prem_Adj_Pgm_ID > 0)      
                      BEGIN      
                          IF((SELECT COUNT(1) FROM [ARMIS_LOS_EXC] WHERE armis_los_pol_id = @Armis_los_pol_id AND coml_agmt_id = @Coml_Agmt_Id       
                              AND prem_adj_pgm_id = @Prem_Adj_Pgm_ID AND custmr_id = @CustmrID AND clm_nbr_txt = @CLAIMNO AND clmt_nm = @CLAIMANTNAME) = 0)      
                            BEGIN      
                                IF @debug = 1      
                                  BEGIN      
                        PRINT 'ARMIS_LOS_EXC Insertion:'      
                                 PRINT '@Armis_los_pol_id: ' + CONVERT(VARCHAR(max), @Armis_los_pol_id)      
                                      PRINT '@Coml_Agmt_Id: ' + CONVERT(VARCHAR(max), @Coml_Agmt_Id)      
                                      PRINT '@Prem_Adj_Pgm_ID: ' + CONVERT(VARCHAR(max), @Prem_Adj_Pgm_ID)      
                                      PRINT '@CustmrID: ' + CONVERT(VARCHAR(max), @CustmrID)      
                                      PRINT '@CLAIMNO: ' + CONVERT(VARCHAR(max), @CLAIMNO)      
                                      PRINT '@ADDITIONALCLAIMIND: ' + CONVERT(VARCHAR(max), @ADDITIONALCLAIMIND)      
                                      PRINT '@ADDITIONALCLAIM: ' + CONVERT(VARCHAR(max), @ADDITIONALCLAIM)      
                                      PRINT '@LIMIT: ' + CONVERT(VARCHAR(max), @LIMIT)      
                                      PRINT '@COVERAGETRIGGERDATE: ' + CONVERT(VARCHAR(max), @COVERAGETRIGGERDATE)      
                                      PRINT '@CLAIMANTNAME: ' + CONVERT(VARCHAR(max), @CLAIMANTNAME)      
                                      PRINT '@TOTALPAIDINDEMNITY: ' + CONVERT(VARCHAR(max), @TOTALPAIDINDEMNITY)      
                                      PRINT '@TOTALPAIDEXPENSE: ' + CONVERT(VARCHAR(max), @TOTALPAIDEXPENSE)      
                                      PRINT '@TOTALRESERVEDINDEMNITY: ' + CONVERT(VARCHAR(max), @TOTALRESERVEDINDEMNITY)      
           PRINT '@TOTALRESERVEDEXPENSE: ' + CONVERT(VARCHAR(max), @TOTALRESERVEDEXPENSE)      
                      PRINT '@NONBILLABLEPAIDINDEMNITY: ' + CONVERT(VARCHAR(max), @NONBILLABLEPAIDINDEMNITY)      
                                      PRINT '@NONBILLABLEPAIDEXPENSE: ' + CONVERT(VARCHAR(max), @NONBILLABLEPAIDEXPENSE)      
                                      PRINT '@NONBILLABLERESERVEDINDEMNITY: ' + CONVERT(VARCHAR(max), @NONBILLABLERESERVEDINDEMNITY)      
                                      PRINT '@NONBILLABLERESERVEDEXPENSE: ' + CONVERT(VARCHAR(max), @NONBILLABLERESERVEDEXPENSE)      
                                      PRINT '@SYSTEMGENERATED: ' + CONVERT(VARCHAR(max), @SYSTEMGENERATED)      
                                      PRINT '@ClaimStatusID: ' + CONVERT(VARCHAR(max), @ClaimStatusID)      
                                      PRINT '@intUserID: ' + CONVERT(VARCHAR(max), @intUserID)      
                                  END      
      
        INSERT INTO ARMIS_LOS_EXC      
        (      
         armis_los_pol_id,      
         coml_agmt_id,      
         prem_adj_pgm_id,      
         custmr_id,      
         clm_nbr_txt,      
         addn_clm_ind,      
         addn_clm_txt,      
         lim2_amt,      
         covg_trigr_dt,      
         clmt_nm,      
         paid_idnmty_amt,      
         paid_exps_amt,      
         resrvd_idnmty_amt,      
         resrvd_exps_amt,      
         non_bilabl_paid_idnmty_amt,      
         non_bilabl_paid_exps_amt,      
         non_bilabl_resrvd_idnmty_amt,      
         non_bilabl_resrvd_exps_amt,      
         sys_genrt_ind,      
         clm_sts_id,      
         crte_user_id,      
         crte_dt,      
         actv_ind,  
         copy_ind      
        )      
        VALUES      
        (      
         @Armis_los_pol_id,      
         @Coml_Agmt_Id,      
         @Prem_Adj_Pgm_ID,      
         @CustmrID,      
         @CLAIMNO,      
         CASE WHEN LOWER(@ADDITIONALCLAIMIND) = 'true' OR LOWER(@ADDITIONALCLAIMIND) = 'yes' OR @ADDITIONALCLAIMIND = '1'       
         THEN CAST(1 AS BIT)      
         ELSE CAST(0 AS BIT) END,      
         @ADDITIONALCLAIM,      
         CAST(@LIMIT AS DECIMAL(15,2)),      
         CASE WHEN ISNULL(@COVERAGETRIGGERDATE,'') = '' THEN NULL      
         ELSE CONVERT(DATETIME,@COVERAGETRIGGERDATE,101) END,      
         @CLAIMANTNAME,      
         CASE WHEN ISNULL(@TOTALPAIDINDEMNITY,'') = '' THEN NULL      
         ELSE CAST(@TOTALPAIDINDEMNITY AS DECIMAL(15,2)) END,      
         CASE WHEN ISNULL(@TOTALPAIDEXPENSE,'') = '' THEN NULL      
         ELSE CAST(@TOTALPAIDEXPENSE AS DECIMAL(15,2)) END,      
         CASE WHEN ISNULL(@TOTALRESERVEDINDEMNITY,'') = '' THEN NULL      
         ELSE CAST(@TOTALRESERVEDINDEMNITY AS DECIMAL(15,2)) END,      
         CASE WHEN ISNULL(@TOTALRESERVEDEXPENSE,'') = '' THEN NULL      
         ELSE CAST(@TOTALRESERVEDEXPENSE AS DECIMAL(15,2)) END,      
         CASE WHEN ISNULL(@NONBILLABLEPAIDINDEMNITY,'') = '' THEN NULL      
         ELSE CAST(@NONBILLABLEPAIDINDEMNITY AS DECIMAL(15,2)) END,      
         CASE WHEN ISNULL(@NONBILLABLEPAIDEXPENSE,'') = '' THEN NULL      
         ELSE CAST(@NONBILLABLEPAIDEXPENSE AS DECIMAL(15,2)) END,      
         CASE WHEN ISNULL(@NONBILLABLERESERVEDINDEMNITY,'') = '' THEN NULL      
         ELSE CAST(@NONBILLABLERESERVEDINDEMNITY AS DECIMAL(15,2)) END,      
         CASE WHEN ISNULL(@NONBILLABLERESERVEDEXPENSE,'') = '' THEN NULL      
         ELSE CAST(@NONBILLABLERESERVEDEXPENSE AS DECIMAL(15,2)) END,      
         --CASE WHEN @SYSTEMGENERATED IS NULL THEN NULL       
         --  WHEN LOWER(@SYSTEMGENERATED) = 'true' OR LOWER(@SYSTEMGENERATED) = 'yes' OR @SYSTEMGENERATED = '1'       
         --  THEN CAST(1 AS BIT)      
         --  ELSE CAST(0 AS BIT) END,     
         CAST(0 AS BIT),  
         @ClaimStatusID,      
         ISNULL(@intUserID,9999),      
         GETDATE(),      
         CAST(1 AS BIT),  
         CAST(1 AS BIT)       
        )      
      
                                IF @debug = 1      
                                  BEGIN      
                        PRINT 'Record is created successfully for ARMIS_LOS_EXC table.'      
          END      
                            END      
                          ELSE      
                            BEGIN      
        IF @debug = 1      
          BEGIN      
           PRINT 'ARMIS_LOS_POL Updation:'      
                               PRINT '@Armis_los_pol_id: ' + CONVERT(VARCHAR(max), @Armis_los_pol_id)      
                                      PRINT '@Coml_Agmt_Id: ' + CONVERT(VARCHAR(max), @Coml_Agmt_Id)      
                                  PRINT '@Prem_Adj_Pgm_ID: ' + CONVERT(VARCHAR(max), @Prem_Adj_Pgm_ID)      
                                      PRINT '@CustmrID: ' + CONVERT(VARCHAR(max), @CustmrID)      
                                      PRINT '@CLAIMNO: ' + CONVERT(VARCHAR(max), @CLAIMNO)      
                                      PRINT '@ADDITIONALCLAIMIND: ' + CONVERT(VARCHAR(max), @ADDITIONALCLAIMIND)      
                                      PRINT '@ADDITIONALCLAIM: ' + CONVERT(VARCHAR(max), @ADDITIONALCLAIM)      
                                      PRINT '@LIMIT: ' + CONVERT(VARCHAR(max), @LIMIT)      
                                      PRINT '@COVERAGETRIGGERDATE: ' + CONVERT(VARCHAR(max), @COVERAGETRIGGERDATE)      
                                      PRINT '@CLAIMANTNAME: ' + CONVERT(VARCHAR(max), @CLAIMANTNAME)      
                                      PRINT '@TOTALPAIDINDEMNITY: ' + CONVERT(VARCHAR(max), @TOTALPAIDINDEMNITY)      
                                      PRINT '@TOTALPAIDEXPENSE: ' + CONVERT(VARCHAR(max), @TOTALPAIDEXPENSE)      
                                      PRINT '@TOTALRESERVEDINDEMNITY: ' + CONVERT(VARCHAR(max), @TOTALRESERVEDINDEMNITY)      
                                      PRINT '@TOTALRESERVEDEXPENSE: ' + CONVERT(VARCHAR(max), @TOTALRESERVEDEXPENSE)      
                                      PRINT '@NONBILLABLEPAIDINDEMNITY: ' + CONVERT(VARCHAR(max), @NONBILLABLEPAIDINDEMNITY)      
                                      PRINT '@NONBILLABLEPAIDEXPENSE: ' + CONVERT(VARCHAR(max), @NONBILLABLEPAIDEXPENSE)      
                                      PRINT '@NONBILLABLERESERVEDINDEMNITY: ' + CONVERT(VARCHAR(max), @NONBILLABLERESERVEDINDEMNITY)      
                                      PRINT '@NONBILLABLERESERVEDEXPENSE: ' + CONVERT(VARCHAR(max), @NONBILLABLERESERVEDEXPENSE)      
                                      PRINT '@SYSTEMGENERATED: ' + CONVERT(VARCHAR(max), @SYSTEMGENERATED)      
                                      PRINT '@ClaimStatusID: ' + CONVERT(VARCHAR(max), @ClaimStatusID)      
                                      PRINT '@intUserID: ' + CONVERT(VARCHAR(max), @intUserID)      
          END      
      
        UPDATE [ARMIS_LOS_EXC]       
        SET clm_nbr_txt = @CLAIMNO,      
         addn_clm_ind = CASE WHEN LOWER(@ADDITIONALCLAIMIND) = 'true' OR LOWER(@ADDITIONALCLAIMIND) = 'yes' OR       
         @ADDITIONALCLAIMIND = '1' THEN CAST(1 AS BIT)      
         ELSE CAST(0 AS BIT) END,      
         addn_clm_txt = @ADDITIONALCLAIM,      
         lim2_amt = CAST(@LIMIT AS DECIMAL(15,2)),      
         covg_trigr_dt = CASE WHEN ISNULL(@COVERAGETRIGGERDATE,'') = '' THEN NULL      
         ELSE CONVERT(DATETIME,@COVERAGETRIGGERDATE,101) END,      
         clmt_nm = @CLAIMANTNAME,      
         paid_idnmty_amt = CASE WHEN ISNULL(@TOTALPAIDINDEMNITY,'') = '' THEN NULL      
         ELSE CAST(@TOTALPAIDINDEMNITY AS DECIMAL(15,2)) END,      
         paid_exps_amt = CASE WHEN ISNULL(@TOTALPAIDEXPENSE,'') = '' THEN NULL      
         ELSE CAST(@TOTALPAIDEXPENSE AS DECIMAL(15,2)) END,      
         resrvd_idnmty_amt = CASE WHEN ISNULL(@TOTALRESERVEDINDEMNITY,'') = '' THEN NULL      
         ELSE CAST(@TOTALRESERVEDINDEMNITY AS DECIMAL(15,2)) END,      
         resrvd_exps_amt = CASE WHEN ISNULL(@TOTALRESERVEDEXPENSE,'') = '' THEN NULL      
         ELSE CAST(@TOTALRESERVEDEXPENSE AS DECIMAL(15,2)) END,      
         non_bilabl_paid_idnmty_amt = CASE WHEN ISNULL(@NONBILLABLEPAIDINDEMNITY,'') = '' THEN NULL      
         ELSE CAST(@NONBILLABLEPAIDINDEMNITY AS DECIMAL(15,2)) END,      
         non_bilabl_paid_exps_amt = CASE WHEN ISNULL(@NONBILLABLEPAIDEXPENSE,'') = '' THEN NULL      
         ELSE CAST(@NONBILLABLEPAIDEXPENSE AS DECIMAL(15,2)) END,      
         non_bilabl_resrvd_idnmty_amt = CASE WHEN ISNULL(@NONBILLABLERESERVEDINDEMNITY,'') = '' THEN NULL      
         ELSE CAST(@NONBILLABLERESERVEDINDEMNITY AS DECIMAL(15,2)) END,      
         non_bilabl_resrvd_exps_amt = CASE WHEN ISNULL(@NONBILLABLERESERVEDEXPENSE,'') = '' THEN NULL      
         ELSE CAST(@NONBILLABLERESERVEDEXPENSE AS DECIMAL(15,2)) END,      
         --sys_genrt_ind = CASE WHEN @SYSTEMGENERATED IS NULL THEN NULL       
         --      WHEN LOWER(@SYSTEMGENERATED) = 'true' OR LOWER(@SYSTEMGENERATED) = 'yes' OR @SYSTEMGENERATED = '1'       
         --      THEN CAST(1 AS BIT)      
         --      ELSE CAST(0 AS BIT) END,   
         sys_genrt_ind = CAST(0 AS BIT),     
         clm_sts_id = @ClaimStatusID,      
         updt_user_id = ISNULL(@intUserID,9999),      
         updt_dt = GETDATE(),      
         actv_ind = 1,  
         copy_ind=1      
        WHERE armis_los_pol_id = @Armis_los_pol_id AND coml_agmt_id = @Coml_Agmt_Id AND prem_adj_pgm_id = @Prem_Adj_Pgm_ID       
        AND custmr_id = @CustmrID AND clm_nbr_txt = @CLAIMNO AND clmt_nm = @CLAIMANTNAME     
      
        IF @debug = 1      
          BEGIN      
           PRINT 'Record is updated successfully for ARMIS_LOS_EXC table.'      
          END      
       END      
        
        EXEC ModAISLossLimitExcess @CustmrID,@Prem_Adj_Pgm_ID          
                      END               
                END      
      
                FETCH LOSS_INFO_COPY_STAGE_EXCESS_Basic INTO @Valuation_Date, @LOB,@POLICY_NO,@CUSTMR_ID,@PGM_EFF_DT,@PGM_EXP_DT,@PGM_TYPE,@STATE,      
    @POL_EFF_DT,@POL_EXP_DT,@CLAIMNO,@ADDITIONALCLAIMIND,@ADDITIONALCLAIM,@CLAIMANTNAME,@CLAIMSTATUS,@COVERAGETRIGGERDATE,@LIMIT,@TOTALPAIDINDEMNITY,      
    @TOTALPAIDEXPENSE,@TOTALRESERVEDINDEMNITY,@TOTALRESERVEDEXPENSE,@NONBILLABLEPAIDINDEMNITY,@NONBILLABLEPAIDEXPENSE,@NONBILLABLERESERVEDINDEMNITY,      
    @NONBILLABLERESERVEDEXPENSE,@SYSTEMGENERATED,@CRTE_USER_ID,@CRTE_DT,@VALIDATE,@LOSS_INFO_COPY_STAGE_EXEC_ID      
            END      
          --end of cursor Mass_Reassignments_basic / while loop                   
          CLOSE LOSS_INFO_COPY_STAGE_EXCESS_Basic      
      
          DEALLOCATE LOSS_INFO_COPY_STAGE_EXCESS_Basic      
      
          IF @debug = 1      
            BEGIN      
                PRINT 'Truncating the stage table LOSS_INFO_COPY_STAGE_EXCESS'      
            END      
      
          DELETE FROM [dbo].[LOSS_INFO_COPY_STAGE_EXEC]      
          WHERE  DATEDIFF(DAY, [CRTE_DT], GETDATE()) > 2      
      
          IF @trancount = 0      
            COMMIT TRANSACTION      
      END try      
      
      BEGIN catch      
          IF @trancount = 0      
            BEGIN      
                ROLLBACK TRANSACTION      
            END      
          ELSE      
            BEGIN      
                ROLLBACK TRANSACTION Loss_Info_Copy_Stage_Excess      
            END      
      
          DECLARE @err_msg  VARCHAR(500),      
                  @err_ln   VARCHAR(10),      
                  @err_proc VARCHAR(30),      
                  @err_no   VARCHAR(10)      
      
          SELECT @err_msg = Error_message(),      
        @err_no = Error_number(),      
                 @err_proc = Error_procedure(),      
                 @err_ln = Error_line()      
      
          SET @err_msg = '- error no.:' + Isnull(@err_no, ' ')      
                         + '; procedure:' + Isnull(@err_proc, ' ')      
                         + ';error line:' + Isnull(@err_ln, ' ')      
                         + ';description:' + Isnull(@err_msg, ' ' )      
          SET @err_msg_op = @err_msg      
      
          SELECT Error_number()    AS ErrorNumber,      
                 Error_severity()  AS ErrorSeverity,      
                 Error_state()     AS ErrorState,      
                 Error_procedure() AS ErrorProcedure,      
                 Error_line()      AS ErrorLine,      
                 Error_message()   AS ErrorMessage      
      
          DECLARE @err_sev VARCHAR(10)      
      
          SELECT @err_msg = Error_message(),      
         @err_no = Error_number(),      
                 @err_proc = Error_procedure(),      
                 @err_ln = Error_line(),      
                 @err_sev = Error_severity()      
      
          RAISERROR (@err_msg,-- Message text.                    
                     @err_sev,-- Severity.                    
                     1 -- State.                    
          )      
      END catch      
  END
GO


if object_id('ModAIS_Process_Copy_Losses_Excess_Upload') is not null
	print 'Created Procedure ModAIS_Process_Copy_Losses_Excess_Upload'
else
	print 'Failed Creating Procedure ModAIS_Process_Copy_Losses_Excess_Upload'
go

if object_id('ModAIS_Process_Copy_Losses_Excess_Upload') is not null
	grant exec on ModAIS_Process_Copy_Losses_Excess_Upload to public
go

if exists (select 1 from sysobjects 
		where name = 'ModAIS_Process_Copy_Losses_Policy_Upload' and type = 'P')
	drop procedure ModAIS_Process_Copy_Losses_Policy_Upload
go

set ansi_nulls off
go

---------------------------------------------------------------------
-----
----- PROC NAME:  ModAIS_Process_Copy_Losses_Policy_Upload
-----
----- VERSION:  SQL SERVER 2008
-----
----- AUTHOR :  CSC
-----
----- DESCRIPTION: To validate and insert/update ARMIS loss policies details for copy losses.
----
----- ON EXIT: 
-----   
-----
----- CREATED: 
-----   
----- 
---------------------------------------------------------------------

CREATE PROCEDURE [dbo].[ModAIS_Process_Copy_Losses_Policy_Upload]      
@create_user_id   VARCHAR(500),      
@dtUploadDateTime DATETIME,      
@err_msg_op       VARCHAR(1000) OUTPUT,      
@debug            BIT = 0      
AS      
  BEGIN      
      SET NOCOUNT ON      
      
 DECLARE @Valuation_Date     VARCHAR(MAX),      
   @LOB       VARCHAR(MAX),      
   @POLICY_NO      VARCHAR(MAX),      
   @CUSTMR_ID      VARCHAR(MAX),      
   @PGM_EFF_DT      VARCHAR(MAX),      
   @PGM_EXP_DT      VARCHAR(MAX),      
   @PGM_TYPE      VARCHAR(MAX),      
   @STATE       VARCHAR(MAX),      
   @POL_EFF_DT      VARCHAR(MAX),      
   @POL_EXP_DT      VARCHAR(MAX),      
   @SCGID       VARCHAR(MAX),      
   @PAID_IDNMTY_AMT    VARCHAR(MAX),      
   @PAID_EXPS_AMT     VARCHAR(MAX),      
   @RESRV_IDNMTY_AMT    VARCHAR(MAX),      
   @RESRV_EXPS_AMT     VARCHAR(MAX),      
   @SYS_GENRT_IND     VARCHAR(MAX),       
   @CRTE_USER_ID     VARCHAR(100),      
   @CRTE_DT      DATETIME,      
   @VALIDATE      BIT,      
   @LOSS_INFO_COPY_STAGE_ID  INT,      
   @Coml_Agmt_Id     INT,    
   @Covg_Typ_ID     INT,      
   @Count_PolicyNo     INT,      
   @CustmrID      INT,      
   @StateID      INT,      
   @Prem_Adj_Pgm_ID    INT,      
   @Prem_Adj_ID     INT,      
   @Adj_Status_ID     INT,      
   @Validation_Message       VARCHAR(max),      
   @Validation_Message_Valuation_Date    VARCHAR(max),      
   @Validation_Message_PolicyNo     VARCHAR(max),      
   @Validation_Message_CUSTMR_ID     VARCHAR(max),      
   @Validation_Message_PGM_EFF_DT     VARCHAR(max),      
   @Validation_Message_PGM_EXP_DT     VARCHAR(max),      
   @Validation_Message_State_ID     VARCHAR(max),      
   @Validation_Message_POL_EFF_DT     VARCHAR(max),      
   @Validation_Message_POL_EXP_DT     VARCHAR(max),      
   @Validation_Message_PGMPRD      VARCHAR(max),      
   @Validation_Message_COML_AGMT_ID    VARCHAR(max),      
   @Validation_Message_PREM_ADJ_ID     VARCHAR(max),      
   @Validation_Message_Total_Paid_Indemnity  VARCHAR(max),      
   @Validation_Message_Total_Paid_Expense   VARCHAR(max),      
   @Validation_Message_Total_Reserved_Indemnity VARCHAR(max),      
   @Validation_Message_Total_Reserved_Expense  VARCHAR(max),      
   @Validation_Message_System_Generated   VARCHAR(max),      
   @txtErrorDesc_OUT         VARCHAR(max),      
   @txtStatusMessage         VARCHAR(max),      
   @err_message              VARCHAR(500),      
   @trancount                INT,      
   @intUserID      INT,      
   @index       SMALLINT,      
   @index2       SMALLINT,       
   @LOBID      INT,      
   @Validation_Message_LOB_ID     VARCHAR(max)    
      
      IF @debug = 1      
        BEGIN      
            PRINT 'Upload: Loss Info Copy Stage Policy Processing started'      
        END      
      
      SET @trancount = @@trancount      
      
      --print @trancount                    
      IF @trancount >= 1      
        SAVE TRANSACTION Loss_Info_Copy_Stage_Policy      
      ELSE      
        BEGIN TRANSACTION      
      
      BEGIN try      
          IF @debug = 1      
            BEGIN      
                PRINT ' @create_user_id:- ' + CONVERT(VARCHAR(500), @create_user_id)      
                PRINT ' @dtUploadDateTime:- ' + CONVERT(VARCHAR(500), @dtUploadDateTime)      
            END      
      
          DECLARE LOSS_INFO_COPY_STAGE_POL_Basic CURSOR LOCAL FAST_FORWARD READ_ONLY      
          FOR      
            SELECT [Valuation_Date],      
     [LOB],      
     [POLICY_NO],      
     [CUSTMR_ID],      
     [PGM_EFF_DT],      
     [PGM_EXP_DT],      
     [PGM_TYPE],      
     [STATE],      
     [POL_EFF_DT],      
     [POL_EXP_DT],      
     [SCGID],      
     [PAID_IDNMTY_AMT],      
     [PAID_EXPS_AMT],      
     [RESRV_IDNMTY_AMT],      
     [RESRV_EXPS_AMT],      
     [SYS_GENRT_IND],      
     [CRTE_USER_ID],      
     [CRTE_DT],      
     [VALIDATE],      
     [LOSS_INFO_COPY_STAGE_ID]      
            FROM   [dbo].[LOSS_INFO_COPY_STAGE_POL]      
            WHERE  [CRTE_USER_ID] = @create_user_id      
                   AND [CRTE_DT] = @dtUploadDateTime      
      
          OPEN LOSS_INFO_COPY_STAGE_POL_Basic      
      
          FETCH LOSS_INFO_COPY_STAGE_POL_Basic INTO @Valuation_Date, @LOB, @POLICY_NO, @CUSTMR_ID, @PGM_EFF_DT, @PGM_EXP_DT,       
          @PGM_TYPE, @STATE, @POL_EFF_DT, @POL_EXP_DT, @SCGID, @PAID_IDNMTY_AMT, @PAID_EXPS_AMT, @RESRV_IDNMTY_AMT,       
          @RESRV_EXPS_AMT, @SYS_GENRT_IND, @CRTE_USER_ID, @CRTE_DT, @VALIDATE, @LOSS_INFO_COPY_STAGE_ID      
      
          WHILE @@Fetch_Status = 0      
            BEGIN      
                BEGIN      
                    IF @debug = 1      
                      BEGIN      
                          PRINT '******************* Uploads: START OF ITERATION *********'      
                          PRINT '---------------Input Params-------------------'      
                          PRINT ' @Valuation_Date:- ' + @Valuation_Date      
                          PRINT ' @LOB:- ' + @LOB      
                          PRINT ' @POLICY_NO: ' + @POLICY_NO      
                          PRINT ' @CUSTMR_ID: ' + @CUSTMR_ID      
                          PRINT ' @PGM_EFF_DT: ' + @PGM_EFF_DT      
                          PRINT ' @PGM_EXP_DT: ' + @PGM_EXP_DT      
                          PRINT ' @PGM_TYPE: '  + @PGM_TYPE      
                          PRINT ' @STATE: ' + @STATE      
                          PRINT ' @POL_EFF_DT: ' + @POL_EFF_DT      
                          PRINT ' @POL_EXP_DT: ' + @POL_EXP_DT      
                          PRINT ' @SCGID:- ' + @SCGID      
                          PRINT ' @PAID_IDNMTY_AMT: ' + @PAID_IDNMTY_AMT      
                          PRINT ' @PAID_EXPS_AMT: ' + @PAID_EXPS_AMT      
                          PRINT ' @RESRV_IDNMTY_AMT: ' + @RESRV_IDNMTY_AMT      
                          PRINT ' @RESRV_EXPS_AMT: ' + @RESRV_EXPS_AMT      
                          PRINT ' @SYS_GENRT_IND: '  + @SYS_GENRT_IND      
                          PRINT ' @CRTE_USER_ID: ' + CONVERT(VARCHAR(50), @CRTE_USER_ID)      
                          PRINT ' @CRTE_DT: ' + CONVERT(VARCHAR(50), @CRTE_DT)      
                          PRINT ' @VALIDATE: ' + CONVERT(VARCHAR(50), @VALIDATE)      
                          PRINT ' @LOSS_INFO_COPY_STAGE_ID: ' + CONVERT(VARCHAR(50), @LOSS_INFO_COPY_STAGE_ID)      
                      END      
      
                    SET @Validation_Message = ''      
                    SET @Validation_Message_Valuation_Date = NULL      
                    SET @Validation_Message_PolicyNo = NULL      
                    SET @Validation_Message_CUSTMR_ID = NULL      
                    SET @Validation_Message_PGM_EFF_DT = NULL      
                    SET @Validation_Message_PGM_EXP_DT = NULL      
                    SET @Validation_Message_State_ID = NULL      
                    SET @Validation_Message_POL_EFF_DT = NULL      
                    SET @Validation_Message_POL_EXP_DT = NULL      
                    SET @Validation_Message_PGMPRD = NULL      
					SET @Validation_Message_COML_AGMT_ID = NULL      
					SET @Validation_Message_PREM_ADJ_ID = NULL      
					SET @Validation_Message_Total_Paid_Indemnity = NULL      
					SET @Validation_Message_Total_Paid_Expense = NULL      
					SET @Validation_Message_Total_Reserved_Indemnity = NULL      
					SET @Validation_Message_Total_Reserved_Expense = NULL      
					SET @Validation_Message_System_Generated = NULL      
					SET @Validation_Message_LOB_ID=NULL    
      
                    -----------------------------------------------------------------------------------------------------                
                    --Validation Rule 1:                
                    --Valuation Date should not be blank      
                    -----------------------------------------------------------------------------------------------------                 
                    IF(ISNULL(@Valuation_Date, '') = '' OR ISDATE(@Valuation_Date) = 0)      
                      SET @Validation_Message = 'Valuation Date'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      IF(ISNULL(@Valuation_Date, '') = '')      
        BEGIN      
       SET @Validation_Message_Valuation_Date = @Validation_Message + ' is missing.'      
             
       IF @debug = 1      
         BEGIN      
      PRINT 'Valuation Date Blank Data Validation Failed :'      
        PRINT '@Validation_Message_Valuation_Date: ' + CONVERT(VARCHAR(max),@Validation_Message_Valuation_Date)      
         END      
        END        
      ELSE       
        BEGIN      
       SET @Validation_Message_Valuation_Date = @Validation_Message + ' is not a valid format.'      
             
       IF @debug = 1      
         BEGIN      
        PRINT 'Valuation Date Format Validation Failed :'      
        PRINT '@Validation_Message_Valuation_Date: ' + CONVERT(VARCHAR(max),@Validation_Message_Valuation_Date)      
         END      
        END      
                      END            
                          
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 2:                
                    --The given Policy No. should not be blank and should be valid        
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
     SET @Count_PolicyNo = NULL      
     SET @index = 0      
                    SET @index2 = 0      
                          
                    SET @index = CHARINDEX(' ',@POLICY_NO)      
     SET @index2 = CHARINDEX(' ',@POLICY_NO, @index + 1)      
           
     IF(@index > 0 AND @index2 > 0)      
     BEGIN      
      SELECT @Count_PolicyNo = COUNT(coml_agmt_id) FROM COML_AGMT WHERE pol_sym_txt = SUBSTRING (@POLICY_NO, 1 , @index - 1)      
       AND pol_nbr_txt = SUBSTRING (@POLICY_NO, @index + 1 , @index2 - (@index + 1))       
       AND pol_modulus_txt = SUBSTRING (@POLICY_NO, @index2 + 1 , LEN(@POLICY_NO)- @index2)      
     END      
           
     IF(@POLICY_NO IS NULL OR @Count_PolicyNo IS NULL OR @Count_PolicyNo <= 0)      
                      SET @Validation_Message = 'Policy No'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      IF(@POLICY_NO IS NULL)      
        BEGIN      
          SET @Validation_Message_PolicyNo = @Validation_Message + ' is missing.'      
      
       IF @debug = 1      
         BEGIN      
        PRINT 'Policy No Blank Data Validation Failed :'      
        PRINT '@Validation_Message_PolicyNo: ' + CONVERT(VARCHAR(max),@Validation_Message_PolicyNo)      
         END      
        END      
      ELSE      
        BEGIN         
       SET @Validation_Message_PolicyNo = @Validation_Message + ' doesn''t exist.'      
      
       IF @debug = 1      
         BEGIN      
        PRINT 'Policy No Matched Validation Failed :'      
        PRINT '@Validation_Message_PolicyNo: ' + CONVERT(VARCHAR(max),@Validation_Message_PolicyNo)      
         END      
        END      
                      END              
      
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 3:                
                    -- • The Given Customer id is valid           
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
                    SET @CustmrID = NULL      
           
     IF(ISNUMERIC(@CUSTMR_ID) = 1)      
      SELECT @CustmrID = custmr_id FROM custmr WHERE  custmr_id = @CUSTMR_ID      
                          
                    IF(@CUSTMR_ID IS NULL OR @CustmrID IS NULL OR @CustmrID <= 0)      
                      SET @Validation_Message = 'Customer ID'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      IF(@CUSTMR_ID IS NULL)      
        BEGIN      
          SET @Validation_Message_CUSTMR_ID = @Validation_Message + ' is missing.'      
      
       IF @debug = 1      
         BEGIN      
        PRINT 'Customer ID Blank Data Validation Failed :'      
        PRINT '@Validation_Message_CUSTMR_ID: ' + CONVERT(VARCHAR(max),@Validation_Message_CUSTMR_ID)      
         END      
        END      
      ELSE IF(ISNUMERIC(@CUSTMR_ID) = 0)        
        BEGIN      
          SET @Validation_Message_CUSTMR_ID = @Validation_Message + ' is not a valid format.'      
      
       IF @debug = 1      
         BEGIN      
        PRINT 'Customer ID Format Validation Failed :'      
        PRINT '@Validation_Message_CUSTMR_ID: ' + CONVERT(VARCHAR(max),@Validation_Message_CUSTMR_ID)      
         END      
        END      
      ELSE      
        BEGIN         
       SET @Validation_Message_CUSTMR_ID = @Validation_Message + ' doesn''t exist.'      
      
       IF @debug = 1      
         BEGIN      
        PRINT 'Customer ID Matched Validation Failed :'      
        PRINT '@Validation_Message_CUSTMR_ID: ' + CONVERT(VARCHAR(max),@Validation_Message_CUSTMR_ID)      
         END      
        END      
                      END      
                            
                    -----------------------------------------------------------------------------------------------------                
                    --Validation Rule 4:                
                    --Program Period Effective Date should not be blank      
                    -----------------------------------------------------------------------------------------------------        
                    --Variable initialization                    
                    SET @Validation_Message = ''      
                                   
                    IF(ISNULL(@PGM_EFF_DT, '') = '' OR ISDATE(@PGM_EFF_DT) = 0)      
                      SET @Validation_Message = 'Program Period Eff Date'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      IF(ISNULL(@PGM_EFF_DT, '') = '')      
       BEGIN      
        SET @Validation_Message_PGM_EFF_DT = @Validation_Message + ' is missing.'      
      
        IF @debug = 1      
          BEGIN      
         PRINT 'Program Period Effective Date Blank Data Validation Failed :'      
         PRINT '@Validation_Message_PGM_EFF_DT: ' + CONVERT(VARCHAR(max),@Validation_Message_PGM_EFF_DT)      
          END      
       END      
      ELSE      
       BEGIN      
        SET @Validation_Message_PGM_EFF_DT = @Validation_Message + ' is not a valid format.'      
      
   IF @debug = 1      
          BEGIN     
         PRINT 'Program Period Effective Date format Validation Failed :'      
         PRINT '@Validation_Message_PGM_EFF_DT: ' + CONVERT(VARCHAR(max),@Validation_Message_PGM_EFF_DT)      
          END      
       END      
                      END       
                            
                    -----------------------------------------------------------------------------------------------------                
  --Validation Rule 5:                
                    --Program Period Expiration Date should not be blank      
                    -----------------------------------------------------------------------------------------------------        
                    --Variable initialization                    
                    SET @Validation_Message = ''      
                                   
                    IF(ISNULL(@PGM_EXP_DT, '') = '' OR ISDATE(@PGM_EXP_DT) = 0)      
                      SET @Validation_Message = 'Program Period Exp Date'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN     
      IF(ISNULL(@PGM_EXP_DT, '') = '')      
        BEGIN      
       SET @Validation_Message_PGM_EXP_DT = @Validation_Message + ' is missing.'      
      
       IF @debug = 1      
         BEGIN      
        PRINT 'Program Period Expiration Date Blank Data Validation Failed :'      
        PRINT '@Validation_Message_PGM_EXP_DT: ' + CONVERT(VARCHAR(max),@Validation_Message_PGM_EXP_DT)      
         END      
        END      
      ELSE      
        BEGIN      
       SET @Validation_Message_PGM_EXP_DT = @Validation_Message + ' is not a valid format.'      
      
       IF @debug = 1      
         BEGIN      
        PRINT 'Program Period Expiration Date format Validation Failed :'      
        PRINT '@Validation_Message_PGM_EXP_DT: ' + CONVERT(VARCHAR(max),@Validation_Message_PGM_EXP_DT)      
         END      
        END      
                      END      
                            
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 6:                
                    -- • The Given State id is valid           
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
                    SET @StateID = NULL      
              
                    SELECT @StateID = LKUP.lkup_id FROM LKUP       
                    INNER JOIN LKUP_TYP TYP ON LKUP.lkup_typ_id = TYP.lkup_typ_id      
     WHERE TYP.lkup_typ_nm_txt = 'STATE' AND LKUP.attr_1_txt = @State      
              
                    IF(@State IS NULL OR @StateID IS NULL OR @StateID <= 0)      
                      SET @Validation_Message = 'State'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      IF(@State IS NULL)      
        BEGIN      
          SET @Validation_Message_State_ID = @Validation_Message + ' is missing.'      
      
       IF @debug = 1      
         BEGIN      
        PRINT 'State Blank Data Validation Failed :'      
        PRINT '@Validation_Message_State_ID: ' + CONVERT(VARCHAR(max),@Validation_Message_State_ID)      
         END      
        END      
      ELSE      
        BEGIN         
       SET @Validation_Message_State_ID = @Validation_Message + ' doesn''t exist.'      
      
       IF @debug = 1      
         BEGIN      
        PRINT 'State Matched Validation Failed :'      
        PRINT '@Validation_Message_State_ID: ' + CONVERT(VARCHAR(max),@Validation_Message_State_ID)      
         END      
        END      
                      END      
                          
                    -----------------------------------------------------------------------------------------------------                
                    --Validation Rule 7:                
                    --Policy Effective Date should not be blank      
                    -----------------------------------------------------------------------------------------------------        
                    --Variable initialization                    
                  SET @Validation_Message = ''      
                                   
                    IF(ISNULL(@POL_EFF_DT, '') = '' OR ISDATE(@POL_EFF_DT) = 0)      
                      SET @Validation_Message = 'Policy Eff Date'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
                      IF(ISNULL(@POL_EFF_DT, '') = '')      
        BEGIN      
                        SET @Validation_Message_POL_EFF_DT = @Validation_Message + ' is missing.'      
      
       IF @debug = 1      
         BEGIN      
        PRINT 'Policy Effective Date Blank Data Validation Failed :'      
        PRINT '@Validation_Message_POL_EFF_DT: ' + CONVERT(VARCHAR(max),@Validation_Message_POL_EFF_DT)      
         END      
        END      
     ELSE      
                      BEGIN      
                       SET @Validation_Message_POL_EFF_DT = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT 'Policy Effective Date format Validation Failed :'      
       PRINT '@Validation_Message_POL_EFF_DT: ' + CONVERT(VARCHAR(max),@Validation_Message_POL_EFF_DT)      
        END      
                      END      
      
                      END       
                          
                    -----------------------------------------------------------------------------------------------------                
                    --Validation Rule 8:                
                    --Policy Expiration Date should not be blank      
                    -----------------------------------------------------------------------------------------------------       
                    --Variable initialization                    
                    SET @Validation_Message = ''      
                                    
                    IF(ISNULL(@POL_EXP_DT, '') = '' OR ISDATE(@POL_EFF_DT) = 0)      
                      SET @Validation_Message = 'Policy Exp Date'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
        IF(ISNULL(@POL_EXP_DT, '') = '')      
         BEGIN      
                         SET @Validation_Message_POL_EXP_DT = @Validation_Message + ' is missing.'      
      
        IF @debug = 1      
          BEGIN      
         PRINT 'Policy Expiration Date Blank Data Validation Failed :'      
         PRINT '@Validation_Message_POL_EXP_DT: ' + CONVERT(VARCHAR(max),@Validation_Message_POL_EXP_DT)      
          END      
         END      
        ELSE      
         BEGIN      
                         SET @Validation_Message_POL_EXP_DT = @Validation_Message + ' is not a valid format.'      
      
        IF @debug = 1      
          BEGIN      
         PRINT 'Policy Expiration Date format Validation Failed :'      
         PRINT '@Validation_Message_POL_EXP_DT: ' + CONVERT(VARCHAR(max),@Validation_Message_POL_EXP_DT)      
          END      
         END      
                      END      
                            
                    -----------------------------------------------------------------------------------------------------                
                    --Validation Rule 9:                
                    -- A valid Policy should exist with given data      
                    -----------------------------------------------------------------------------------------------------       
     --Variable initialization                    
                    SET @Validation_Message = ''      
                    SET @Coml_Agmt_Id = NULL      
                    SET @Prem_Adj_Pgm_ID = NULL    
                    SET @Covg_Typ_ID = NULL      
                    SET @index = 0      
                    SET @index2 = 0      
                          
                    SET @index = CHARINDEX(' ',@POLICY_NO)   
     SET @index2 = CHARINDEX(' ',@POLICY_NO, @index + 1)      
            
     IF(@POL_EFF_DT IS NOT NULL AND ISDATE(@POL_EFF_DT) = 1 AND @POL_EXP_DT IS NOT NULL AND ISDATE(@POL_EXP_DT) = 1 AND @index > 0 AND @index2 > 0)      
     BEGIN          
      SELECT @Coml_Agmt_Id = coml_agmt_id, @Prem_Adj_Pgm_ID = prem_adj_pgm_id, @Covg_Typ_ID = covg_typ_id FROM COML_AGMT WHERE pol_sym_txt = SUBSTRING (@POLICY_NO, 1 , @index - 1)      
      AND pol_nbr_txt = SUBSTRING (@POLICY_NO, @index + 1 , @index2 - (@index + 1))       
      AND pol_modulus_txt = SUBSTRING (@POLICY_NO, @index2 + 1 , LEN(@POLICY_NO)- @index2)       
      AND custmr_id = @CustmrID AND pol_eff_dt = CONVERT(DATETIME,@POL_EFF_DT,101) AND planned_end_date = CONVERT(DATETIME,@POL_EXP_DT,101)       
     END      
                                    
                    IF((@POLICY_NO IS NOT NULL AND @Count_PolicyNo IS NOT NULL AND @Count_PolicyNo > 0) AND (@CustmrID IS NOT NULL AND @CustmrID > 0) AND      
                      (@Coml_Agmt_Id IS NULL OR @Coml_Agmt_Id <= 0))      
                      SET @Validation_Message = 'Policy'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_COML_AGMT_ID = @Validation_Message + ' doesn''t exist.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT 'Policy Matched Validation Failed :'      
       PRINT '@Validation_Message_COML_AGMT_ID: ' + CONVERT(VARCHAR(max),@Validation_Message_COML_AGMT_ID)      
        END      
                      END     
                          
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 2_New:                
                    -- • The Given LOB is valid           
                    ---------------------------------------------------------------------------------------                
                --Variable initialization                    
                    SET @Validation_Message = ''      
                    SET @LOBID = NULL      
              
     SELECT @LOBID = LKUP.lkup_id FROM LKUP       
                    INNER JOIN LKUP_TYP TYP ON LKUP.lkup_typ_id = TYP.lkup_typ_id      
     WHERE TYP.lkup_typ_nm_txt = 'LOB COVERAGE' AND LKUP.lkup_txt = @LOB      
              
                    IF(@LOB IS NULL OR @LOBID IS NULL OR @LOBID <= 0 OR @Covg_Typ_ID <> @LOBID)      
                      SET @Validation_Message = 'LOB'      
      
                    IF(@Validation_Message <> '')      
                    BEGIN      
      IF(@LOB IS NULL)      
      BEGIN      
       SET @Validation_Message_LOB_ID = @Validation_Message + ' is missing.'      
    
       IF @debug = 1      
       BEGIN      
        PRINT 'LOB Blank Data Validation Failed :'      
        PRINT '@Validation_Message_LOB_ID: ' + CONVERT(VARCHAR(max),@Validation_Message_LOB_ID)      
       END      
      END      
      ELSE IF(@LOB IS NOT NULL AND (@LOBID IS NULL OR @LOBID <= 0))     
      BEGIN    
       SET @Validation_Message_LOB_ID = @Validation_Message + ' doesn''t exist.'      
    
       IF @debug = 1      
       BEGIN      
        PRINT 'LOB Matched Validation Failed :'      
        PRINT '@Validation_Message_LOB_ID: ' + CONVERT(VARCHAR(max),@Validation_Message_LOB_ID)      
       END      
      END    
      ELSE    
      BEGIN    
       SET @Validation_Message_LOB_ID = @Validation_Message + ' is not assosiated with policy.'      
    
       IF @debug = 1      
       BEGIN      
        PRINT 'LOB Assosiation with Policy Failed :'      
        PRINT '@Validation_Message_LOB_ID: ' + CONVERT(VARCHAR(max),@Validation_Message_LOB_ID)      
       END      
      END    
                    END    
                            
                    -----------------------------------------------------------------------------------------------------                
                    --Validation Rule 10:                
                    -- A valid Program Period should exist with given data      
                    -----------------------------------------------------------------------------------------------------       
                    --Variable initialization                    
                    SET @Validation_Message = ''      
           
     IF(@PGM_EFF_DT IS NOT NULL AND ISDATE(@PGM_EFF_DT) = 1 AND @PGM_EXP_DT IS NOT NULL AND ISDATE(@PGM_EXP_DT) = 1)      
     BEGIN      
      IF NOT EXISTS(SELECT 1 FROM PREM_ADJ_PGM PGM       
      WHERE PGM.strt_dt = CONVERT(DATETIME,@PGM_EFF_DT,101) AND PGM.plan_end_dt = CONVERT(DATETIME,@PGM_EXP_DT,101)       
      AND PGM.custmr_id = @CustmrID AND PGM.actv_ind=1 AND PGM.PREM_ADJ_PGM_ID = @Prem_Adj_Pgm_ID)      
      BEGIN      
       SET @Prem_Adj_Pgm_ID = NULL      
      END      
     END      
                                    
                    IF((@Coml_Agmt_Id IS NOT NULL OR @Coml_Agmt_Id > 0) AND (@Prem_Adj_Pgm_ID IS NULL OR @Prem_Adj_Pgm_ID <= 0))      
                      SET @Validation_Message = 'Program Period'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_PGMPRD = @Validation_Message + ' doesn''t exist.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT 'Program Period Matched Validation Failed :'      
       PRINT '@Validation_Message_PGMPRD: ' + CONVERT(VARCHAR(max),@Validation_Message_PGMPRD)      
        END      
           END      
                            
                    -----------------------------------------------------------------------------------------------------                
                    --Validation Rule 11:                
     -- A valid Adjustment should exist with given data      
                    -----------------------------------------------------------------------------------------------------       
                    --Variable initialization                    
                    SET @Validation_Message = ''      
                    SET @Prem_Adj_ID = NULL      
                    SET @Adj_Status_ID = NULL      
            
     IF(@Valuation_Date IS NOT NULL AND ISDATE(@Valuation_Date) = 1)      
     BEGIN
	            
      SELECT @PREM_ADJ_ID = PREM_ADJ_ID, @ADJ_STATUS_ID = ADJ_STS_TYP_ID FROM PREM_ADJ       
      WHERE REG_CUSTMR_ID = @CUSTMRID 
	  AND VALN_DT = CONVERT(DATETIME,@VALUATION_DATE,101)      
	  AND ADJ_CAN_IND<>1 
	  AND ADJ_VOID_IND<>1
	  AND ADJ_RRSN_IND<>1
	  AND SUBSTRING(FNL_INVC_NBR_TXT,1,3)<>'RTV' 

     END      
                                    
                    IF(@Prem_Adj_ID IS NOT NULL AND @Prem_Adj_ID > 0 AND @Adj_Status_ID <> 346)      
                      SET @Validation_Message = 'Adjustment'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_PREM_ADJ_ID = @Validation_Message + ' not in CALC status.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT 'Adjustment''s Status Type Validation Failed :'      
       PRINT '@Validation_Message_PREM_ADJ_ID: ' + CONVERT(VARCHAR(max),@Validation_Message_PREM_ADJ_ID)      
        END      
                      END      
                            
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 12:                
                    -- • The Given Total Paid Indemnity is valid           
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
           
     IF(@PAID_IDNMTY_AMT IS NOT NULL AND @PAID_IDNMTY_AMT <> '' AND ISNUMERIC(@PAID_IDNMTY_AMT) = 0)      
      SET @Validation_Message = 'Total Paid Indemnity'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_Total_Paid_Indemnity = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT 'Total Paid Indemnity Format Validation Failed :'      
       PRINT' @Validation_Message_Total_Paid_Indemnity: ' + CONVERT(VARCHAR(max),@Validation_Message_Total_Paid_Indemnity)      
        END      
                      END         
                           
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 13:                
                    -- • The Given Total Paid Expense is valid           
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
           
     IF(@PAID_EXPS_AMT IS NOT NULL AND @PAID_EXPS_AMT <> '' AND ISNUMERIC(@PAID_EXPS_AMT) = 0)      
      SET @Validation_Message = 'Total Paid Expense'      
      
                    IF(@Validation_Message <> '')      
                    BEGIN      
      SET @Validation_Message_Total_Paid_Expense = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT 'Total Paid Expense Format Validation Failed :'      
       PRINT '@Validation_Message_Total_Paid_Expense: ' + CONVERT(VARCHAR(max),@Validation_Message_Total_Paid_Expense)      
        END      
                      END        
                            
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 14:                
                    -- • The Given Total Reserved Indemnity is valid           
                    ---------------------------------------------------------------------------------------                
        --Variable initialization                    
                    SET @Validation_Message = ''      
           
     IF(@RESRV_IDNMTY_AMT IS NOT NULL AND @RESRV_IDNMTY_AMT <> '' AND ISNUMERIC(@RESRV_IDNMTY_AMT) = 0)      
      SET @Validation_Message = 'Total Reserved Indemnity'      
      
     IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_Total_Reserved_Indemnity = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT 'Total Reserved Indemnity Format Validation Failed :'      
       PRINT '@Validation_Message_Total_Reserved_Indemnity: ' + CONVERT(VARCHAR(max),@Validation_Message_Total_Reserved_Indemnity)      
        END      
                      END       
                            
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 15:      
                    -- • The Given Total Reserved Expense is valid           
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
           
     IF(@RESRV_EXPS_AMT IS NOT NULL AND @RESRV_EXPS_AMT <> '' AND ISNUMERIC(@RESRV_EXPS_AMT) = 0)      
      SET @Validation_Message = 'Total Reserved Expense'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_Total_Reserved_Expense = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT 'Total Reserved Expense Format Validation Failed :'      
       PRINT '@Validation_Message_Total_Reserved_Expense: ' + CONVERT(VARCHAR(max),@Validation_Message_Total_Reserved_Expense)      
        END      
                      END      
                           
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 16:                
                    -- • The Given System Generated is valid           
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
           
     IF(NOT(LOWER(@SYS_GENRT_IND) = 'yes' OR LOWER(@SYS_GENRT_IND) = 'no' OR LOWER(@SYS_GENRT_IND) = 'true' OR LOWER(@SYS_GENRT_IND) = 'false'      
        OR @SYS_GENRT_IND = '1' OR @SYS_GENRT_IND = '0'))      
      SET @Validation_Message = 'System Generated'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_System_Generated = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT 'System Generated :'      
       PRINT '@Validation_Message_System_Generated: ' + CONVERT(VARCHAR(max),@Validation_Message_System_Generated)      
        END      
                      END      
                            
                    IF(ISNULL(@Validation_Message_Valuation_Date, '') <> '' OR ISNULL(@Validation_Message_PolicyNo, '') <> '' OR      
                       ISNULL(@Validation_Message_CUSTMR_ID, '') <> '' OR ISNULL(@Validation_Message_PGM_EFF_DT, '') <> '' OR      
                       ISNULL(@Validation_Message_PGM_EXP_DT, '') <> '' OR ISNULL(@Validation_Message_State_ID, '') <> '' OR      
                       ISNULL(@Validation_Message_POL_EFF_DT, '') <> '' OR ISNULL(@Validation_Message_POL_EXP_DT, '') <> '' OR      
                       ISNULL(@Validation_Message_PGMPRD, '') <> '' OR ISNULL(@Validation_Message_COML_AGMT_ID, '') <> '' OR      
                       ISNULL(@Validation_Message_PREM_ADJ_ID, '') <> '' OR ISNULL(@Validation_Message_Total_Paid_Indemnity, '') <> '' OR       
                       ISNULL(@Validation_Message_Total_Paid_Expense, '') <> '' OR ISNULL(@Validation_Message_Total_Reserved_Indemnity, '') <> '' OR      
                       ISNULL(@Validation_Message_Total_Reserved_Expense, '') <> '' OR ISNULL(@Validation_Message_System_Generated, '') <> ''    
                       OR ISNULL(@Validation_Message_LOB_ID, '') <> '')      
    BEGIN                     
      INSERT INTO [dbo].[LOSS_INFO_COPY_STAGE_POL_STATUSLOG]      
      (      
       [Valuation_Date], [LOB], [POLICY_NO], [CUSTMR_ID], [PGM_EFF_DT], [PGM_EXP_DT], [PGM_TYPE], [STATE],      
       [POL_EFF_DT], [POL_EXP_DT], [SCGID], [PAID_IDNMTY_AMT], [PAID_EXPS_AMT], [RESRV_IDNMTY_AMT], [RESRV_EXPS_AMT],      
       [SYS_GENRT_IND], [CRTE_USER_ID], [CRTE_DT], [VALIDATE], [TXTSTATUS], [TXTERRORDESC]      
      )      
                        VALUES            
                        (      
       @Valuation_Date, @LOB, @POLICY_NO, @CUSTMR_ID, @PGM_EFF_DT, @PGM_EXP_DT, @PGM_TYPE, @STATE, @POL_EFF_DT,       
       @POL_EXP_DT, @SCGID, @PAID_IDNMTY_AMT, @PAID_EXPS_AMT, @RESRV_IDNMTY_AMT, @RESRV_EXPS_AMT, @SYS_GENRT_IND,       
       @CRTE_USER_ID, @CRTE_DT, @VALIDATE, 'Error',      
       LTRIM(RTRIM(ISNULL(@Validation_Message_Valuation_Date + ' ','') + ISNULL(@Validation_Message_PolicyNo + ' ','') +       
       ISNULL(@Validation_Message_CUSTMR_ID + ' ','') + ISNULL(@Validation_Message_PGM_EFF_DT + ' ','') +       
       ISNULL(@Validation_Message_PGM_EXP_DT + ' ','') + ISNULL(@Validation_Message_State_ID + ' ','') +       
       ISNULL(@Validation_Message_POL_EFF_DT  + ' ','') + ISNULL(@Validation_Message_POL_EXP_DT + ' ','') +       
       ISNULL(@Validation_Message_PGMPRD + ' ','') + ISNULL(@Validation_Message_COML_AGMT_ID + ' ','') +      
       ISNULL(@Validation_Message_LOB_ID + ' ','') + ISNULL(@Validation_Message_PREM_ADJ_ID + ' ','') +     
       ISNULL(@Validation_Message_Total_Paid_Indemnity + ' ','') + ISNULL(@Validation_Message_Total_Paid_Expense + ' ','') +     
       ISNULL(@Validation_Message_Total_Reserved_Indemnity + ' ','') + ISNULL(@Validation_Message_Total_Reserved_Expense + ' ','') +     
       ISNULL(@Validation_Message_System_Generated,'')))      
                         )      
      
                         ----Skip this record as it is having blank or NULL values                    
                         FETCH LOSS_INFO_COPY_STAGE_POL_Basic INTO @Valuation_Date, @LOB, @POLICY_NO, @CUSTMR_ID, @PGM_EFF_DT, @PGM_EXP_DT,       
       @PGM_TYPE, @STATE, @POL_EFF_DT, @POL_EXP_DT, @SCGID, @PAID_IDNMTY_AMT, @PAID_EXPS_AMT, @RESRV_IDNMTY_AMT,       
       @RESRV_EXPS_AMT, @SYS_GENRT_IND, @CRTE_USER_ID, @CRTE_DT, @VALIDATE, @LOSS_INFO_COPY_STAGE_ID      
                          CONTINUE      
                      END       
                 
                    SELECT @intUserID = pers_id FROM pers WHERE external_reference = @CRTE_USER_ID      
      
                    ---------------------------------------------------------------------------------------                
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization      
                    SET @Validation_Message=''      
      
                    IF @debug = 1      
                      BEGIN      
                          PRINT 'Validation Completed successfully:Insert/Update progress'      
                          PRINT '@intUserID: ' + CONVERT(VARCHAR(max), @intUserID)      
                      END      
      
      
                    IF(@Validate = 0 AND @Coml_Agmt_Id IS NOT NULL AND @Coml_Agmt_Id > 0 AND @Prem_Adj_Pgm_ID IS NOT NULL AND @Prem_Adj_Pgm_ID > 0      
                    AND @CustmrID IS NOT NULL AND @CustmrID > 0 AND @StateID IS NOT NULL AND @StateID > 0)      
                      BEGIN      
                          IF((SELECT COUNT(*) FROM [ARMIS_LOS_POL] WHERE coml_agmt_id = @Coml_Agmt_Id AND prem_adj_pgm_id = @Prem_Adj_Pgm_ID AND      
        custmr_id = @CustmrID AND st_id = @StateID AND valn_dt = CONVERT(DATETIME,@Valuation_Date,101)       
        AND isnull(prem_adj_id,0) = isnull(@Prem_Adj_ID,0)) = 0)      
                            BEGIN      
                                IF @debug = 1      
                                  BEGIN      
                                      PRINT 'ARMIS_LOS_POL Insertion:'      
                                      PRINT '@Coml_Agmt_Id: ' + CONVERT(VARCHAR(max), @Coml_Agmt_Id)      
                                      PRINT '@Prem_Adj_Pgm_ID: ' + CONVERT(VARCHAR(max), @Prem_Adj_Pgm_ID)      
                              PRINT '@CustmrID: ' + CONVERT(VARCHAR(max), @CustmrID)      
                                      PRINT '@StateID: ' + CONVERT(VARCHAR(max), @StateID)      
                                      PRINT '@Prem_Adj_ID: ' + CONVERT(VARCHAR(max), @Prem_Adj_ID)      
                                  END      
      
        INSERT INTO [ARMIS_LOS_POL]      
        (      
         coml_agmt_id,      
         prem_adj_pgm_id,      
         custmr_id,      
         st_id,      
         valn_dt,      
         prem_adj_id,      
         suprt_serv_custmr_gp_id,      
         paid_idnmty_amt,      
         paid_exps_amt,      
         resrv_idnmty_amt,      
         resrv_exps_amt,      
         sys_genrt_ind,      
         crte_dt,      
         crte_user_id,      
         actv_ind,  
         copy_ind      
        )      
        VALUES      
        (      
         @Coml_Agmt_Id,      
         @Prem_Adj_Pgm_ID,      
         @CustmrID,      
         @StateID,      
         @Valuation_Date,      
         @Prem_Adj_ID,      
         CAST(@SCGID AS CHAR(10)),      
         CASE WHEN ISNULL(@PAID_IDNMTY_AMT,'') = '' THEN NULL      
         ELSE CAST(@PAID_IDNMTY_AMT AS DECIMAL(15,2)) END,      
         CASE WHEN ISNULL(@PAID_EXPS_AMT,'') = '' THEN NULL      
         ELSE CAST(@PAID_EXPS_AMT AS DECIMAL(15,2)) END,      
         CASE WHEN ISNULL(@RESRV_IDNMTY_AMT,'') = '' THEN NULL      
         ELSE CAST(@RESRV_IDNMTY_AMT AS DECIMAL(15,2)) END,      
         CASE WHEN ISNULL(@RESRV_EXPS_AMT,'') = '' THEN NULL      
         ELSE CAST(@RESRV_EXPS_AMT AS DECIMAL(15,2)) END,      
         --CASE WHEN @SYS_GENRT_IND IS NULL THEN NULL      
         --  WHEN LOWER(@SYS_GENRT_IND) = 'yes' OR LOWER(@SYS_GENRT_IND) = 'true' OR @SYS_GENRT_IND = '1'       
         --  THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END,   
         CAST(0 AS BIT),  
         GETDATE(),      
         ISNULL(@intUserID,9999),      
         CAST(1 AS BIT),  
         CAST(1 AS BIT)       
        )      
      
                                IF @debug = 1      
                                  BEGIN      
                                      PRINT 'Record is created successfully for ARMIS_LOS_POL table.'      
          END      
                            END      
                          ELSE      
                            BEGIN      
        IF @debug = 1      
          BEGIN      
           PRINT 'ARMIS_LOS_POL Updation:'      
                                      PRINT '@Coml_Agmt_Id: ' + CONVERT(VARCHAR(max), @Coml_Agmt_Id)      
  PRINT '@Prem_Adj_Pgm_ID: ' + CONVERT(VARCHAR(max), @Prem_Adj_Pgm_ID)      
                                      PRINT '@CustmrID: ' + CONVERT(VARCHAR(max), @CustmrID)      
                                      PRINT '@StateID: ' + CONVERT(VARCHAR(max), @StateID)      
                                      PRINT '@Prem_Adj_ID: ' + CONVERT(VARCHAR(max), @Prem_Adj_ID)      
          END      
      
        UPDATE [ARMIS_LOS_POL]      
        SET [suprt_serv_custmr_gp_id] = CAST(@SCGID AS CHAR(10)),      
         [paid_idnmty_amt] = CASE WHEN ISNULL(@PAID_IDNMTY_AMT,'') = '' THEN NULL      
                 ELSE CAST(@PAID_IDNMTY_AMT AS DECIMAL(15,2)) END,      
         [paid_exps_amt] = CASE WHEN ISNULL(@PAID_EXPS_AMT,'') = '' THEN NULL      
               ELSE CAST(@PAID_EXPS_AMT AS DECIMAL(15,2)) END,      
         [resrv_idnmty_amt] = CASE WHEN ISNULL(@RESRV_IDNMTY_AMT,'') = '' THEN NULL      
               ELSE CAST(@RESRV_IDNMTY_AMT AS DECIMAL(15,2)) END,      
         [resrv_exps_amt] = CASE WHEN ISNULL(@RESRV_EXPS_AMT,'') = '' THEN NULL      
                ELSE CAST(@RESRV_EXPS_AMT AS DECIMAL(15,2)) END,      
         --[sys_genrt_ind] = CASE WHEN @SYS_GENRT_IND IS NULL THEN NULL      
         --      WHEN LOWER(@SYS_GENRT_IND) = 'yes' OR LOWER(@SYS_GENRT_IND) = 'true' OR @SYS_GENRT_IND = '1'       
         --      THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END,    
         [sys_genrt_ind] = CAST(0 AS BIT),    
            [updt_user_id] = ISNULL(@intUserID, 9999),      
            [updt_dt] = GETDATE(),  
            copy_ind=1      
        WHERE coml_agmt_id = @Coml_Agmt_Id AND prem_adj_pgm_id = @Prem_Adj_Pgm_ID AND custmr_id = @CustmrID AND       
           st_id = @StateID AND isnull(prem_adj_id,0) = isnull(@Prem_Adj_ID,0)      
      
        IF @debug = 1      
          BEGIN      
           PRINT 'Record is updated successfully for ARMIS_LOS_POL table.'      
          END      
       END      
        
        EXEC ModAISLossLimitExcess @CustmrID,@Prem_Adj_Pgm_ID         
                      END               
                END      
      
                FETCH LOSS_INFO_COPY_STAGE_POL_Basic INTO @Valuation_Date, @LOB, @POLICY_NO, @CUSTMR_ID, @PGM_EFF_DT, @PGM_EXP_DT,       
    @PGM_TYPE, @STATE, @POL_EFF_DT, @POL_EXP_DT, @SCGID, @PAID_IDNMTY_AMT, @PAID_EXPS_AMT, @RESRV_IDNMTY_AMT,       
    @RESRV_EXPS_AMT, @SYS_GENRT_IND, @CRTE_USER_ID, @CRTE_DT, @VALIDATE, @LOSS_INFO_COPY_STAGE_ID      
            END      
          --end of cursor Mass_Reassignments_basic / while loop                   
          CLOSE LOSS_INFO_COPY_STAGE_POL_Basic      
      
          DEALLOCATE LOSS_INFO_COPY_STAGE_POL_Basic      
      
          IF @debug = 1      
            BEGIN      
                PRINT 'Truncating the stage table LOSS_INFO_COPY_STAGE_POL'      
            END      
      
          DELETE FROM [dbo].[LOSS_INFO_COPY_STAGE_POL]      
          WHERE  DATEDIFF(DAY, [CRTE_DT], GETDATE()) > 2      
         
    IF @debug = 1      
            BEGIN      
                PRINT '@trancount: ' + CAST(@trancount AS VARCHAR)      
            END      
          
          IF @trancount = 0      
            COMMIT TRANSACTION      
      END try      
      
      BEGIN catch      
          IF @trancount = 0      
            BEGIN      
                ROLLBACK TRANSACTION      
            END      
          ELSE      
            BEGIN      
                ROLLBACK TRANSACTION Loss_Info_Copy_Stage_Policy      
            END      
      
          DECLARE @err_msg  VARCHAR(500),      
                  @err_ln   VARCHAR(10),      
                  @err_proc VARCHAR(30),      
                  @err_no   VARCHAR(10)      
      
          SELECT @err_msg = Error_message(),      
                 @err_no = Error_number(),      
                 @err_proc = Error_procedure(),      
                 @err_ln = Error_line()      
      
          SET @err_msg = '- error no.:' + Isnull(@err_no, ' ')      
                         + '; procedure:' + Isnull(@err_proc, ' ')      
                         + ';error line:' + Isnull(@err_ln, ' ')      
                         + ';description:' + Isnull(@err_msg, ' ' )      
          SET @err_msg_op = @err_msg      
      
          SELECT Error_number()    AS ErrorNumber,      
                 Error_severity()  AS ErrorSeverity,      
                 Error_state()     AS ErrorState,      
                 Error_procedure() AS ErrorProcedure,      
                 Error_line()      AS ErrorLine,      
                 Error_message()   AS ErrorMessage      
      
          DECLARE @err_sev VARCHAR(10)      
      
          SELECT @err_msg = Error_message(),      
                 @err_no = Error_number(),      
                 @err_proc = Error_procedure(),      
                 @err_ln = Error_line(),      
                 @err_sev = Error_severity()      
      
          RAISERROR (@err_msg,-- Message text.                    
                     @err_sev,-- Severity.                    
                     1 -- State.                    
          )      
      END catch      
  END
GO


if object_id('ModAIS_Process_Copy_Losses_Policy_Upload') is not null
	print 'Created Procedure ModAIS_Process_Copy_Losses_Policy_Upload'
else
	print 'Failed Creating Procedure ModAIS_Process_Copy_Losses_Policy_Upload'
go

if object_id('ModAIS_Process_Copy_Losses_Policy_Upload') is not null
	grant exec on ModAIS_Process_Copy_Losses_Policy_Upload to public
go

/************************************ About AIS Page Insert script *******************************************************/
GO
IF NOT EXISTS(SELECT 1 FROM APLCTN_MENU WHERE menu_nm_txt = 'About AIS')
BEGIN
	DECLARE @aplctn_menu_id INT
	
	UPDATE APLCTN_MENU
	SET menu_nm_txt = 'About AIS',
		web_page_txt = 'AboutAIS.aspx',
		crte_dt = '10/12/2015',
		crte_user_id = 1,
		menu_tooltip_txt = 'AIS config details'
	WHERE aplctn_menu_id = 8
	
	UPDATE APLCTN_WEB_PAGE_AUTH
	SET crte_dt = '10/12/2015',
		crte_user_id = 1
	WHERE aplctn_menu_id = 8
	

	INSERT INTO APLCTN_MENU (crte_dt,menu_nm_txt, parnt_id, web_page_txt, menu_tooltip_txt, actv_ind, depnd_txt, page_catg_cd, crte_user_id)  VALUES('01/01/2008','Exit',	NULL,	'Javascript:LogUserOff();window.close();',	NULL,	1,	NULL, 'M', 1)
	SET @aplctn_menu_id = @@IDENTITY
	
	INSERT INTO [APLCTN_WEB_PAGE_AUTH](crte_dt,[secur_gp_id],[aplctn_menu_id],[authd_ind],[inquiry_full_acss_ind_cd],[crte_user_id])values('01/01/2008',354,@aplctn_menu_id,1,'I',1)
	INSERT INTO [APLCTN_WEB_PAGE_AUTH](crte_dt,[secur_gp_id],[aplctn_menu_id],[authd_ind],[inquiry_full_acss_ind_cd],[crte_user_id])values('01/01/2008',355,@aplctn_menu_id,1,'F',1)	
	INSERT INTO [APLCTN_WEB_PAGE_AUTH](crte_dt,[secur_gp_id],[aplctn_menu_id],[authd_ind],[inquiry_full_acss_ind_cd],[crte_user_id])values('01/01/2008',356,@aplctn_menu_id,1,'F',1)
	INSERT INTO [APLCTN_WEB_PAGE_AUTH](crte_dt,[secur_gp_id],[aplctn_menu_id],[authd_ind],[inquiry_full_acss_ind_cd],[crte_user_id])values('01/01/2008',357,@aplctn_menu_id,1,'F',1)
	INSERT INTO [APLCTN_WEB_PAGE_AUTH](crte_dt,[secur_gp_id],[aplctn_menu_id],[authd_ind],[inquiry_full_acss_ind_cd],[crte_user_id])values('01/01/2008',358,@aplctn_menu_id,1,'F',1)
END
GO