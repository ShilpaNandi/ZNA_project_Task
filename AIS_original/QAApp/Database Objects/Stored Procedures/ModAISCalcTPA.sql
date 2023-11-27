
if exists (select 1 from sysobjects 
		where name = 'ModAISCalcTPA' and type = 'P')
	drop procedure ModAISCalcTPA
go

set ansi_nulls off
go
---------------------------------------------------------------------
-----
-----	Proc Name:	ModAISCalcTPA
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	This stored procedure is used to populate PREM_ADJ_PAID_LOS_BIL table
-----	with data extracted for third party administrator.
-----	On Exit:	
-----			
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

------  Modified: 02/26/2009 venkat kolimi
 --                 changed the post_trns_typ_id = 54
 --                 inserting lsi_src filed as 0
 
 ---- Modified:03/02/2009 venkat kolimi	
 ---                When PLB's are pulled for TPA Manual, 'lsi_pgm_typ_txt'  should get populated with "Retro"
 

---------------------------------------------------------------------

create procedure [dbo].[ModAISCalcTPA]
@premium_adj_period_id int,
@premium_adjustment_id int,
@customer_id int,
@premium_adj_prog_id int,
@create_user_id int,
@err_msg_op varchar(1000) output
as

begin
	set nocount on

declare @trancount int


set @trancount = @@trancount

if @trancount = 0 
	begin
	    begin transaction 
	end


begin try

   insert into dbo.PREM_ADJ_PAID_LOS_BIL
   (
		[prem_adj_perd_id]
	   ,[prem_adj_id]
	   ,[custmr_id]
	   ,[coml_agmt_id]
	   ,[lsi_src]
	   ,[lsi_pgm_typ_txt]
	   ,[lsi_valn_dt]
	   ,[ln_of_bsn_id]
	   ,[tot_paid_los_bil_amt]
	   ,[adj_tot_paid_los_bil_amt]
	   ,[crte_user_id]
	)
	select 
	@premium_adj_period_id ,
	@premium_adjustment_id ,
	@customer_id ,
	ca.coml_agmt_id,
	0, --the losses are pulled from TPA
	'Retro', --When PLB's are pulled for TPA Manual, 'lsi_pgm_typ_txt'  should get populated with "Retro"
	tpah.valn_dt,
	dbo.fn_GetLOB(ca.coml_agmt_id),
	sum(tpad.thrd_pty_admin_amt) AS TotalLossAmount,
	sum(tpad.thrd_pty_admin_amt) AS AdjTotalLossAmount,
	@create_user_id
	from dbo.THRD_PTY_ADMIN_MNL_INVC tpah  
	inner join dbo.THRD_PTY_ADMIN_MNL_INVC_DTL tpad on (tpah.custmr_id = tpad.custmr_id) and (tpah.thrd_pty_admin_mnl_invc_id = tpad.thrd_pty_admin_mnl_invc_id)
	inner join dbo.COML_AGMT ca on (tpad.custmr_id = ca.custmr_id) and (tpad.pol_sym_txt = ca.pol_sym_txt) and (tpad.pol_nbr_txt = ca.pol_nbr_txt) and (tpad.pol_modulus_txt = ca.pol_modulus_txt)
	where 
	ca.pol_eff_dt = tpad.eff_dt
	and ca.planned_end_date = tpad.expi_dt
	and tpad.post_trns_typ_id = 54 --Posting transaction type is Paid loss deductible as defined in the table POST_TRNS_TYP
	and tpah.actv_ind = 1
	and ca.prem_adj_pgm_id = @premium_adj_prog_id
	and ca.custmr_id = @customer_id
	and ca.actv_ind = 1
	and ca.thrd_pty_admin_ind = 1
	and ca.thrd_pty_admin_dir_ind = 0
	and tpah.fnl_ind=1
	and tpah.invc_nbr_txt is not null
	and (tpah.revise_ind <> 1 or tpah.revise_ind is null)
	and (tpah.void_ind <> 1 or tpah.void_ind is null)
	and (tpah.can_ind <> 1 or tpah.can_ind is null)
	group by /*pap.prem_adj_perd_id, pa.prem_adj_id, lc.custmr_id,*/ ca.coml_agmt_id , tpah.valn_dt
--	order by lc.custmr_id, ths.ValuationDate, pol.InceptionDate, ths.LOB 

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



go

if object_id('ModAISCalcTPA') is not null
	print 'Created Procedure ModAISCalcTPA'
else
	print 'Failed Creating Procedure ModAISCalcTPA'
go

if object_id('ModAISCalcTPA') is not null
	grant exec on ModAISCalcTPA to public
go





