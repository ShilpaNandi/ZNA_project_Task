
if exists (select 1 from sysobjects 
		where name = 'ModAISCalcProcessAdjNum' and type = 'P')
	drop procedure ModAISCalcProcessAdjNum
go

set ansi_nulls off
go
---------------------------------------------------------------------
-----
-----	Proc Name:	ModAISCalcProcessAdjNum
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	This stored procedure is used for processing adjustment numbers.
-----
-----	On Exit:	
-----			
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------

create procedure [dbo].[ModAISCalcProcessAdjNum] 
@premium_adj_period_id int,
@customer_id int
as

begin
	set nocount on

declare @prev_valid_adj_id int,
		@premium_adj_prog_id int,
		@prev_valid_adj_perd_id int,
		@current_premium_adjustment_id int,
		@prev_adj_nbr int,
		@curr_adj_nbr int,
		@curr_pgm_perd_adj_nbr int,
		@adj_nbr_txt varchar(50),
		@trancount int


set @trancount = @@trancount
--print @trancount
if @trancount = 0 
	begin
	    begin transaction 
	end


begin try

	select 
	@premium_adj_prog_id = prem_adj_pgm_id,
	@current_premium_adjustment_id = prem_adj_id
	from dbo.PREM_ADJ_PERD
	where prem_adj_perd_id = @premium_adj_period_id
	and isnull(adj_nbr_mnl_overrid_ind,0)<>1

	/**************************
	* Determine previous valid adjustment
	**************************/

	select @prev_valid_adj_id =  max(pa.prem_adj_id)
	from dbo.PREM_ADJ pa
	inner join dbo.PREM_ADJ_STS ps on (pa.prem_adj_id = ps.prem_adj_id) and (pa.reg_custmr_id = ps.custmr_id)
	inner join dbo.PREM_ADJ_PERD prd on (pa.reg_custmr_id = prd.reg_custmr_id) and (pa.prem_adj_id = prd.prem_adj_id)
	where pa.valn_dt in
	(
		select max(valn_dt) 
		from dbo.PREM_ADJ adj
		--inner join dbo.PREM_ADJ_PARMET_SETUP op on (adj.reg_custmr_id = op.custmr_id) and (adj.prem_adj_id = op.prem_adj_id )
		inner join prem_adj_perd pap on pap.prem_adj_id=adj.prem_adj_id
		where adj.valn_dt < 
		(
			select 
			valn_dt 
			from PREM_ADJ 
			where prem_adj_id = @current_premium_adjustment_id
		)
		--and adj.reg_custmr_id = @customer_id
		and pap.custmr_id = @customer_id
		and pap.prem_adj_pgm_id=@premium_adj_prog_id
		and adj.adj_sts_typ_id in (349,352)
		and adj.adj_can_ind<>1 
		and adj.adj_void_ind<>1
		and adj.adj_rrsn_ind<>1
		and substring(isnull(adj.fnl_invc_nbr_txt,''),1,3)<>'RTV'
	)
	and ps.adj_sts_typ_id in (349,352) -- Lookup IDs for applicable statuses: Final Invoice and Transmitted
	and prd.prem_adj_pgm_id = @premium_adj_prog_id
	and pa.reg_custmr_id = @customer_id
	and pa.adj_can_ind<>1 
    and pa.adj_void_ind<>1
    and pa.adj_rrsn_ind <>1
	and substring(isnull(pa.fnl_invc_nbr_txt,''),1,3)<>'RTV'

	if (@prev_valid_adj_id is not null) -- Has valid previous adjustment; not first adjustment
	begin
		print '@prev_valid_adj_id is not null'

		select
		@prev_valid_adj_perd_id = prem_adj_perd_id
		from dbo.PREM_ADJ_PERD
		where prem_adj_id = @prev_valid_adj_id
		and prem_adj_pgm_id = @premium_adj_prog_id

		select 
		@prev_adj_nbr = adj_nbr
		from dbo.PREM_ADJ_PERD
		where prem_adj_perd_id = @prev_valid_adj_perd_id

		set @curr_adj_nbr = @prev_adj_nbr + 1

		select 
		@adj_nbr_txt = adj_nbr_txt + ' Adjustment'
		from dbo.ADJ_NBR_LKUP
		where adj_numercal_nbr = @curr_adj_nbr

		update dbo.PREM_ADJ_PERD WITH (ROWLOCK)
		set adj_nbr_txt = @adj_nbr_txt,
			adj_nbr = @curr_adj_nbr,
			updt_dt = getdate()
		where prem_adj_perd_id = @premium_adj_period_id
		and isnull(adj_nbr_mnl_overrid_ind,0)<>1

	end --end of: if (@prev_valid_adj_id is not null)
	else
	begin -- No valid previous adjustment; first adjustment

		print '@prev_valid_adj_id is null'

		select
		@curr_pgm_perd_adj_nbr = curr_pgm_perd_adj_nbr
		from dbo.PREM_ADJ_PGM
		where prem_adj_pgm_id = @premium_adj_prog_id

		if (@curr_pgm_perd_adj_nbr is not null)
		begin
			select 
			@adj_nbr_txt = adj_nbr_txt + ' Adjustment'
			from dbo.ADJ_NBR_LKUP
			where adj_numercal_nbr = @curr_pgm_perd_adj_nbr

			update dbo.PREM_ADJ_PERD WITH (ROWLOCK)
			set adj_nbr_txt = @adj_nbr_txt,
				adj_nbr = @curr_pgm_perd_adj_nbr,
				updt_dt = getdate()
			where prem_adj_perd_id = @premium_adj_period_id
			and isnull(adj_nbr_mnl_overrid_ind,0)<>1
		end -- end of: if (@curr_pgm_perd_adj_nbr is not null)
	end

	--print '@trancount: ' + convert(varchar(30),@trancount)
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
		'AIS Calculation Engine'
       ,'ERR'
       ,'Calculation error'
       ,'Error encountered during calculation of adjustment number: ' 
			+ convert(varchar(20),0) 
			+ ' for program number: ' 
			+ convert(varchar(20),isnull(@premium_adj_prog_id,0)) 
			+ ' associated with customer number: ' 
			+ convert(varchar(20),isnull(@customer_id,0))
			+ '. Error message: '
			+ isnull(@err_msg, ' ')
	   ,@customer_id
	   ,0
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



go

if object_id('ModAISCalcProcessAdjNum') is not null
	print 'Created Procedure ModAISCalcProcessAdjNum'
else
	print 'Failed Creating Procedure ModAISCalcProcessAdjNum'
go

if object_id('ModAISCalcProcessAdjNum') is not null
	grant exec on ModAISCalcProcessAdjNum to public
go







