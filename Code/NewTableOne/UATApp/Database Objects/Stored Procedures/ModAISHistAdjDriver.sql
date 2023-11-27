
if exists (select 1 from sysobjects 
		where name = 'ModAISHistAdjDriver' and type = 'P')
	drop procedure ModAISHistAdjDriver
go

set ansi_nulls off
go

SET QUOTED_IDENTIFIER ON
GO
---------------------------------------------------------------------
-----
-----	Proc Name:	ModAISHistAdjDriver
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	This stored procedure is used to run historical adjustments
-----					in batch mode
-----					This stored proc is configured to only run at a time
-----                   when no adjustments have been created in the system
-----					or adjustments are only in "CALC" status
-----	On Exit:	
-----			
-----
-----	Modified:	01/09/09	Dan Gojmerac
---------------------------------------------------------------------

create procedure [dbo].[ModAISHistAdjDriver]
@low_custmr_id int,
@high_custmr_id int
as


declare @prem_adj_pgm_id_txt varchar(20),
		@prem_adj_perd_id_txt varchar(20),
		@custmr_id int,
		@err_msg_output varchar(1000)


begin try
SET NOCOUNT ON
		declare hist_adj_cur cursor LOCAL FAST_FORWARD READ_ONLY 
		for
		select 
		 papd.custmr_id, 
		 convert(varchar(20), papd.prem_adj_pgm_id),
		 convert(varchar(20), isnull(pape.prem_adj_perd_id,0))
		from prem_adj_pgm papd 
		inner JOIN PREM_ADJ_PGM_STS PAPS ON 
		( PAPS.prem_adj_pgm_id = papd.prem_adj_pgm_id
		AND pgm_perd_sts_typ_id = 342 AND STS_CHK_IND = 1)
		left outer join prem_adj_perd pape on (papd.prem_adj_pgm_id = pape.prem_adj_pgm_id)
		where 
		papd.custmr_id >= @low_custmr_id 
		and papd.custmr_id <= @high_custmr_id 
		order by papd.custmr_id, papd.prem_adj_pgm_id

		declare hist_adj_cur2 cursor LOCAL FAST_FORWARD READ_ONLY 
		for
		select 
		 papd.custmr_id, 
		 convert(varchar(20), papd.prem_adj_pgm_id),
		 convert(varchar(20), isnull(pape.prem_adj_perd_id,0))
		from prem_adj_pgm papd 
		inner JOIN PREM_ADJ_PGM_STS PAPS ON 
		( PAPS.prem_adj_pgm_id = papd.prem_adj_pgm_id
		AND pgm_perd_sts_typ_id = 342 AND STS_CHK_IND = 1)
		left outer join prem_adj_perd pape on (papd.prem_adj_pgm_id = pape.prem_adj_pgm_id)
		where 
		papd.custmr_id >= @low_custmr_id 
		and papd.custmr_id <= @high_custmr_id 
		order by papd.custmr_id, papd.prem_adj_pgm_id

				open hist_adj_cur
				fetch hist_adj_cur into @custmr_id, @prem_adj_pgm_id_txt, @prem_adj_perd_id_txt

				while @@Fetch_Status = 0
					begin
						print 'HISTORICAL DRIVER ' + convert(varchar(20), getdate( ), 120) + ' custmr_id:' + convert(varchar(20), @custmr_id )+ ' @prem_adj_pgm_id:' + @prem_adj_pgm_id_txt + ' @prem_adj_perd_id:'  + @prem_adj_perd_id_txt
						-- Check if this is a recalculation
						
						if @prem_adj_perd_id_txt = 0
						begin
							exec ModAISCalcDriver @custmr_id, @prem_adj_pgm_id_txt,	' ', 1,1,9999999,	  @err_msg_output=@err_msg_output, @debug=0
						end
						else
						begin
							exec ModAISCalcDriver @custmr_id, '', @prem_adj_perd_id_txt, 1,1,9999999,	  @err_msg_output=@err_msg_output, @debug=0
						end
						
					fetch hist_adj_cur into @custmr_id, @prem_adj_pgm_id_txt, @prem_adj_perd_id_txt
					end --end of cursor driver_adj_cur / while loop
			close hist_adj_cur
			deallocate hist_adj_cur


				open hist_adj_cur2
				fetch hist_adj_cur2 into @custmr_id, @prem_adj_pgm_id_txt, @prem_adj_perd_id_txt

				while @@Fetch_Status = 0
					begin
						print 'HISTORICAL DRIVER2 ' + convert(varchar(20), getdate( ), 120) + ' custmr_id:' + convert(varchar(20), @custmr_id )+ ' @prem_adj_pgm_id:' + @prem_adj_pgm_id_txt + ' @prem_adj_perd_id:'  + @prem_adj_perd_id_txt
						-- Check if this is a recalculation
						if @prem_adj_perd_id_txt > 0
						begin
							exec [dbo].[ModAISCalcProcessAdjNum] @prem_adj_perd_id_txt, @custmr_id
						end
						

					fetch hist_adj_cur2 into @custmr_id, @prem_adj_pgm_id_txt, @prem_adj_perd_id_txt
					end --end of cursor driver_adj_cur / while loop
			close hist_adj_cur2
			deallocate hist_adj_cur2

			update prem_adj
			set historical_adj_ind = 1
			where reg_custmr_id >= @low_custmr_id 
			and reg_custmr_id <= @high_custmr_id 

end try
begin catch

	declare @err_ln varchar(10),
			@err_proc varchar(30),@err_no varchar(10),
			@err_msg varchar(1000)
	select  @err_msg = error_message(),
		    @err_no = error_number(),
		    @err_proc = error_procedure(),
			@err_ln = error_line()
	set @err_msg = '- error no.:' + isnull(@err_no,'') + '; procedure:' 
		+ isnull(@err_proc,'') + ';error line:' + isnull(@err_ln,'') + ';description:' + isnull(@err_msg,'') 

	
	select 
    error_number() AS ErrorNumber,
    error_severity() AS ErrorSeverity,
    error_state() as ErrorState,
    error_procedure() as ErrorProcedure,
    error_line() as ErrorLine,
    error_message() as ErrorMessage

end catch


go

if object_id('ModAISHistAdjDriver') is not null
	print 'Created Procedure ModAISHistAdjDriver'
else
	print 'Failed Creating Procedure ModAISHistAdjDriver'
go

if object_id('ModAISHistAdjDriver') is not null
	grant exec on ModAISHistAdjDriver to public
go

