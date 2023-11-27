
if exists (select 1 from sysobjects 
		where name = 'ModAISCheckKeyParameters' and type = 'P')
	drop procedure ModAISCheckKeyParameters
go

set ansi_nulls off
go
---------------------------------------------------------------------
-----
-----	Proc Name:	ModAISCheckKeyParameters
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	This stored procedure is used to verify for open adjustments
-----					prior to calculation and recalculation
-----
-----	On Exit:	
-----			
-----
-----	Modified:	01/09/09	Prabal Dhar
-----			

---------------------------------------------------------------------

create procedure [dbo].[ModAISCheckKeyParameters] 
@check_calc_prog_perds varchar(2000),
@check_recalc_prem_adj_perds varchar(2000),
@err_msg_op varchar(2000) output,
@create_user_id int,
@debug bit = 0
as

begin
	set nocount on


declare @len_calc int,
		@len_recalc int,
		@prem_adj_pgm_id int,
		@prem_adj_perd_id int,
		@open_adj_id int,
		@customer_id int,
		@trancount int,
		@err_message varchar(2000),
		@prog_adj_criteria_changed bit,
		@prem_adj_valn_dt datetime,
		@prem_adj_brkr_id int,
		@prem_adj_bu_office_id int,
		@prem_non_prem_cd char(2),
		@curr_valn_mm_dt_prem datetime,
		@curr_valn_mm_dt_non_prem datetime,
		@curr_brkr_id int,
		@curr_bsn_unt_ofc_id int,
		@curr_prem_non_prem_cd char(2),
		@cnt int


begin try
		/****************************************************************************
		* Open Adjustment Check for prem_adj_pgm_id's in case of new Calculation
		*****************************************************************************/
		/****************************************************************************
		* Sequence generator that drives parsing of comma-separated Prem_adj_pgm_id's
		*****************************************************************************/
		set @len_calc = len(@check_calc_prog_perds)
		
		if(@len_calc>0)
		begin
		
		;with Nums ( n ) AS (
        select 1 union all
        select 1 + n from Nums where n < @len_calc+1) 
		select n  into #num from Nums
		option ( maxrecursion 2000 )

		declare driver_prem_adj_pgm_cur cursor LOCAL FAST_FORWARD READ_ONLY 
		for 
		select 
				substring( ',' + @check_calc_prog_perds + ',', n + 1, 
				charindex( ',', ',' + @check_calc_prog_perds + ',', n + 1 ) - n - 1 ) as "ProgPerds"
				from #num
				where substring( ',' + @check_calc_prog_perds + ',', n, 1 ) = ','
				and n < len( ',' + @check_calc_prog_perds + ',' ) 
		
		open driver_prem_adj_pgm_cur
		fetch driver_prem_adj_pgm_cur into @prem_adj_pgm_id

		while @@Fetch_Status = 0
			begin
				begin
					if @debug = 1
					begin
					print '@prem_adj_pgm_id:' + convert(varchar(20),@prem_adj_pgm_id)
					end
					set @cnt = 0
					select @cnt=count(*) from prem_adj pa
					inner join prem_adj_perd pap on pap.prem_adj_id=pa.prem_adj_id
					inner join prem_adj_pgm pg on pg.prem_adj_pgm_id=pap.prem_adj_pgm_id
					where pa.adj_sts_typ_id  not in (349,352) 
					and pa.adj_can_ind<>1 
					and pa.adj_void_ind<>1
					and pa.adj_rrsn_ind<>1
					and substring(isnull(pa.fnl_invc_nbr_txt,' '),1,3)<>'RTV'
					and pg.prem_adj_pgm_id=@prem_adj_pgm_id
					
					if @debug = 1
					begin
					print '@cnt:' + convert(varchar(20),@cnt)
					end
					if @cnt > 0
								begin
									--retrieving open adjustment id on same val date
										select @open_adj_id=pa.prem_adj_id,@customer_id=pap.custmr_id from prem_adj pa
										inner join prem_adj_perd pap on pap.prem_adj_id=pa.prem_adj_id
										inner join prem_adj_pgm pg on pg.prem_adj_pgm_id=pap.prem_adj_pgm_id
										where pa.adj_sts_typ_id  not in (349,352)
										and pa.adj_can_ind<>1 
										and pa.adj_void_ind<>1
										and pa.adj_rrsn_ind<>1
										and substring(isnull(pa.fnl_invc_nbr_txt,' '),1,3)<>'RTV'
										and pg.prem_adj_pgm_id=@prem_adj_pgm_id
										set @err_message = 'A key field has been changed. You must cancel your invoice and perform another calculation in order for the changes to take effect.' 
										+ ';Customer ID: ' + convert(varchar(20),@customer_id) 
										+ ';Currently open adjustment ID: ' + convert(varchar(20),@open_adj_id) 
										+ ';Program Period ID: ' + convert(varchar(20),@prem_adj_pgm_id) 

										set @err_msg_op = @err_message
										exec [dbo].[AddAPLCTN_STS_LOG] 
											@premium_adjustment_id = @open_adj_id,
											@customer_id = @customer_id,
											@premium_adj_prog_id = @prem_adj_pgm_id,
											@err_msg = @err_message,
											@create_user_id = @create_user_id
										
										-- Write the validatin error to the application status log tabl
										
								end

							end

							
							fetch driver_prem_adj_pgm_cur into @prem_adj_pgm_id

						end --end of cursor driver_prem_adj_pgm_cur / while loop
					close driver_prem_adj_pgm_cur
					deallocate driver_prem_adj_pgm_cur
	
		end
		/****************************************************************************
		* Open Adjustment Check for prem_adj_perd_id's in case of Re-Calculation
		*****************************************************************************/	
		/****************************************************************************
		* Sequence generator that drives parsing of comma-separated Prem_adj_perd_id's
		*****************************************************************************/
		set @len_recalc = len(@check_recalc_prem_adj_perds)

		if(@len_recalc>0)
		begin
		
		;with Numbers ( i ) AS (
        select 1 union all
        select 1 + i from Numbers where i < @len_recalc+1) 
		select i  into #number from Numbers
		option ( maxrecursion 2000 )

		declare driver_prem_adj_perd_cur cursor LOCAL FAST_FORWARD READ_ONLY 
		for 
		select 
				substring( ',' + @check_recalc_prem_adj_perds + ',', i + 1, 
				charindex( ',', ',' + @check_recalc_prem_adj_perds + ',', i + 1 ) - i - 1 ) as "RecalcProgPerds"
				from #number
				where substring( ',' + @check_recalc_prem_adj_perds + ',', i, 1 ) = ','
				and i < len( ',' + @check_recalc_prem_adj_perds + ',' ) 
		
		open driver_prem_adj_perd_cur
		fetch driver_prem_adj_perd_cur into @prem_adj_perd_id

		while @@Fetch_Status = 0
			begin
				begin
					if @debug = 1
					begin
					print '@prem_adj_perd_id:' + convert(varchar(20),@prem_adj_perd_id)
					end
				
								set @prog_adj_criteria_changed = 0

								select 
								@prem_adj_valn_dt = pa.valn_dt,
								@prem_adj_brkr_id = pa.brkr_id,
								@prem_adj_bu_office_id = pa.bu_office_id,
								@prem_non_prem_cd=pap.prem_non_prem_cd
								from dbo.PREM_ADJ pa
								inner join dbo.PREM_ADJ_PERD pap on pap.prem_adj_id=pa.prem_adj_id
								where pap.prem_adj_perd_id = @prem_adj_perd_id
												

								select 
									@curr_valn_mm_dt_prem = pg.nxt_valn_dt,
									@curr_valn_mm_dt_non_prem=pg.nxt_valn_dt_non_prem_dt,
									@curr_brkr_id = pg.brkr_id,
									@curr_bsn_unt_ofc_id = pg.bsn_unt_ofc_id
									from dbo.PREM_ADJ_PGM pg
									inner join dbo.PREM_ADJ_PERD pap on pg.prem_adj_pgm_id=pap.prem_adj_pgm_id
									where pap.prem_adj_perd_id = @prem_adj_perd_id

						if(@curr_valn_mm_dt_prem=@curr_valn_mm_dt_non_prem)
						begin
								set @curr_prem_non_prem_cd='B'
						end
						if(@curr_valn_mm_dt_prem<>@curr_valn_mm_dt_non_prem)
						begin
								if(@curr_valn_mm_dt_prem <= getdate())
								begin
								set @curr_prem_non_prem_cd='P'
								end
								if(@curr_valn_mm_dt_non_prem <= getdate())
								begin
								set @curr_prem_non_prem_cd='NP'
								end
						end

						if(@curr_prem_non_prem_cd<>@prem_non_prem_cd)
						begin
							set @prog_adj_criteria_changed = 1
						end
						if(@curr_valn_mm_dt_prem<>@curr_valn_mm_dt_non_prem and @curr_valn_mm_dt_prem<= getdate() and @curr_valn_mm_dt_non_prem<= getdate())
						begin
							set @prog_adj_criteria_changed = 1
						end

						if(@prem_non_prem_cd='B')
						begin
								if( (@curr_valn_mm_dt_prem <> @prem_adj_valn_dt) or (@curr_brkr_id <> @prem_adj_brkr_id) or (@curr_bsn_unt_ofc_id <> @prem_adj_bu_office_id))
								begin
									set @prog_adj_criteria_changed = 1
								end
						end
						if(@prem_non_prem_cd='P')
						begin
								if( (@curr_valn_mm_dt_prem <> @prem_adj_valn_dt) or (@curr_brkr_id <> @prem_adj_brkr_id) or (@curr_bsn_unt_ofc_id <> @prem_adj_bu_office_id))
								begin
									set @prog_adj_criteria_changed = 1
								end
						end
						if(@prem_non_prem_cd='NP')
						begin
								if( (@curr_valn_mm_dt_non_prem <> @prem_adj_valn_dt) or (@curr_brkr_id <> @prem_adj_brkr_id) or (@curr_bsn_unt_ofc_id <> @prem_adj_bu_office_id))
								begin
									set @prog_adj_criteria_changed = 1
								end
						end
						if(@prog_adj_criteria_changed=1)
						begin
							select 
								@open_adj_id = pa.prem_adj_id,
								@customer_id = pap.custmr_id,
								@prem_adj_pgm_id = pap.prem_adj_pgm_id
								from dbo.PREM_ADJ pa
								inner join dbo.PREM_ADJ_PERD pap on pap.prem_adj_id=pa.prem_adj_id
								where pap.prem_adj_perd_id = @prem_adj_perd_id
								
							
							set @err_message = 'A key field has been changed. You must cancel your invoice and perform another calculation in order for the changes to take effect.' 
										+ ';Customer ID: ' + convert(varchar(20),@customer_id) 
										+ ';Currently open adjustment ID: ' + convert(varchar(20),@open_adj_id) 
										+ ';Program Period ID: ' + convert(varchar(20),@prem_adj_pgm_id) 

										set @err_msg_op = @err_message
										exec [dbo].[AddAPLCTN_STS_LOG] 
											@premium_adjustment_id = @open_adj_id,
											@customer_id = @customer_id,
											@premium_adj_prog_id = @prem_adj_pgm_id,
											@err_msg = @err_message,
											@create_user_id = @create_user_id
						end

						
							

				end

							
							fetch driver_prem_adj_perd_cur into @prem_adj_perd_id

						end --end of cursor driver_prem_adj_perd_cur / while loop
					close driver_prem_adj_perd_cur
					deallocate driver_prem_adj_perd_cur
		end
		if @trancount = 0
		commit transaction 

end try
begin catch

	if @trancount = 0  
	begin
		rollback transaction
	end
end catch


end

go

if object_id('ModAISCheckKeyParameters') is not null
	print 'Created Procedure ModAISCheckKeyParameters'
else
	print 'Failed Creating Procedure ModAISCheckKeyParameters'
go

if object_id('ModAISCheckKeyParameters') is not null
	grant exec on ModAISCheckKeyParameters to public
go
