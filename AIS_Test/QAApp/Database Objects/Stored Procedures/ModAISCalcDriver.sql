
if exists (select 1 from sysobjects 
		where name = 'ModAISCalcDriver' and type = 'P')
	drop procedure ModAISCalcDriver
go

set ansi_nulls off
go
---------------------------------------------------------------------
-----
-----	Proc Name:	ModAISCalcDriver
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	This stored procedure is used to drive the individual subordinate stored procedure.
-----
-----	On Exit:	
-----			
-----
-----	Modified:	01/09/09	Prabal Dhar
-----			- Solved the issue with duplicate premium adjustments in QA env.
-----	Modified:	05/13/09	Siva Kumar Thangaraj
-----			- Solved deadlock issue while concurrent users are accessing

---------------------------------------------------------------------

create procedure [dbo].[ModAISCalcDriver]
@customer_id int,
@calc_prog_perds varchar(200),
@recalc_prem_adj_perds varchar(200),
@delete_plb bit,
@delete_ilrf bit,
@create_user_id int,
@err_msg_output varchar(1000) output,
@debug bit = 0
as

begin
	set deadlock_priority low
	set nocount on

declare @trancount int,
		@valn_mm_dt datetime,
		@curr_valn_mm_dt datetime,
		@val_dt datetime,
 		@brkr_id int,
		@bsn_unt_ofc_id int,
 		@curr_brkr_id int,
		@curr_bsn_unt_ofc_id int,
		@prem_adj_id int,
		@prem_adj_pgm_id int,
		@delete_prem_adj_pgm_id int,
		@prem_adj_perd_id int,
		@prem_adj_perd_id_delete int,
		@prem_adj_id_delete int,
		@customer_id_delete int,
		@exist_adj_id int,
		@diff_prem_nonprem_val int,
		@exist_prem_adj_perd_id int,
		@processed_prem_adj_perd_id int,
		@processed_related_customer_id int,
		@len_calc int,
		@len_recalc int,
		@count int,
		@related_cust_id int,
		@delete_related_cust_id int,
		@err_msg varchar(500),
		@adj_nbr_txt varchar(30),
		@prem_adj_valn_dt datetime,
		@pgm_perd_sts_typ_id int,
		@prem_adj_brkr_id int,
		@prem_adj_bu_office_id int,
		@open_adj_id int,
		@open_adj_id_on_same_valdate int,
		@historical_adj_id int,
		@temp_open_adj_id int,
		@prog_adj_criteria_changed bit,
		@adj_pendg_ind bit,
		@err_message varchar(1000),
		@calc_prog_perds_after_validation varchar(2000),
		@recalc_prem_adj_perds_after_validation varchar(2000),
		@open_adjustments_after_validation varchar(2000),
		@recalc_prem_adj_perds_Delete varchar(2000),
		@open_adj_id_err int,
		@len_openAdjustments int,
		@counter int,
		@counter_delete int,
		@count_delete int,
		@delete_prem_adj_perd_id int,
		@len_recalc_delete int,
		@cnt_prev_adjs int,
		@historical_adj_ind bit,
		@tryProc int

set @tryProc = 4
set @prog_adj_criteria_changed = 0

--while(@tryProc>0)
--begin
	set @trancount = @@trancount
	--print @trancount
	if @trancount = 0 
		begin
			begin transaction 
		end


	begin try
			
			/******************************************************************
			*			Open Adjustment Check Verification
			******************************************************************/
			exec [dbo].[ModAISCheckOpenAdjustments] 
									@check_calc_prog_perds = @calc_prog_perds,
									@check_recalc_prem_adj_perds = @recalc_prem_adj_perds,
									@calc_prog_perds_from_driver = @calc_prog_perds_after_validation output,
									@recalc_prem_adj_perds_from_driver=@recalc_prem_adj_perds_after_validation output,
									@open_adjustments_from_driver=@open_adjustments_after_validation output,
									@create_user_id = @create_user_id,
									@err_msg_op = @err_msg_output output,
									@debug = @debug

			set @calc_prog_perds=isnull(@calc_prog_perds_after_validation,'')
			set @recalc_prem_adj_perds= isnull(@recalc_prem_adj_perds_after_validation,'')
			
			/******************************************************************
			* Sequence generator that drives parsing of comma-separated strings
			******************************************************************/
			set @len_calc = len(@calc_prog_perds)
			set @len_recalc = len(@recalc_prem_adj_perds)

			;with Nums ( n ) AS (
			select 1 union all
			select 1 + n from Nums where n < (case when @len_calc > @len_recalc then @len_calc else @len_recalc end)+1) 
			select n  into #num from Nums
			option ( maxrecursion 2000 )

			--select * from #num

			/******************************************************************
			* Delete data associated with program periods indicated for recalc
			* or disabled
			******************************************************************/

			set @counter = 1
			
			create table #adj_perds
			(
			id int identity(1,1),
			prem_adj_perd_id int,
			prem_adj_pgm_id int
			)
			create index ind ON #adj_perds (id)

			create table #processed_adj_perds
			(
			id int identity(1,1),
			prem_adj_perd_id int,
			related_customer_id int
			)
			create index ind ON #processed_adj_perds (id)

			
			create table #deleted_adj_perds
			(
			id int identity(1,1),
			prem_adj_perd_id_delete int,
			)
			create index ind ON #deleted_adj_perds (id)

			insert into #adj_perds(prem_adj_perd_id)
			-- prem adj period ids sent by UI marked for recalc; 
			-- first clear existing results in all output tables.
			select 
			substring( ',' + @recalc_prem_adj_perds + ',', n + 1, 
			charindex( ',', ',' + @recalc_prem_adj_perds + ',', n + 1 ) - n - 1 ) as "RecalcProgPerds"
			from #num
			where substring( ',' + @recalc_prem_adj_perds + ',', n, 1 ) = ','
			and n < len( ',' + @recalc_prem_adj_perds + ',' )
			--union
			-- internal check: if a program period is disabled in parameter setup
			-- clear existing results in output tables
			--select prd.prem_adj_perd_id 
			--from dbo.PREM_ADJ_PERD prd
			--inner join dbo.PREM_ADJ_PGM pgm on (prd.custmr_id = pgm.custmr_id) and (prd.prem_adj_pgm_id = pgm.prem_adj_pgm_id)
			--where prd.reg_custmr_id = @customer_id
			--and pgm.actv_ind = 0
			
			update #adj_perds 
			set prem_adj_pgm_id = prd.prem_adj_pgm_id
			from #adj_perds t
			inner join dbo.PREM_ADJ_PERD prd on t.prem_adj_perd_id = prd.prem_adj_perd_id
			where prd.reg_custmr_id = @customer_id

			select 'before delete'
			select * from #adj_perds

			select @count = count(*) from #adj_perds
			--print @count

			while @counter <= @count
			begin
					
				
				
			--	Clear output tables
				select @prem_adj_perd_id_delete = prem_adj_perd_id
				from #adj_perds 
				where id = @counter
				
				select @prem_adj_id_delete = prem_adj_id,
					   @delete_prem_adj_pgm_id =  prem_adj_pgm_id,
						@customer_id_delete=reg_custmr_id
				from dbo.PREM_ADJ_PERD
				where prem_adj_perd_id = @prem_adj_perd_id_delete
				
				insert into #deleted_adj_perds(prem_adj_perd_id_delete)
				select prem_adj_perd_id
				from dbo.PREM_ADJ_PERD
				where prem_adj_id = @prem_adj_id_delete 
				and prem_adj_perd_id not in(select prem_adj_perd_id from #adj_perds)
				and prem_adj_perd_id not in(select prem_adj_perd_id_delete from #deleted_adj_perds)

				select 
				@delete_related_cust_id = custmr_id
				from dbo.PREM_ADJ_PGM
				where prem_adj_pgm_id = @delete_prem_adj_pgm_id
				
				select @adj_pendg_ind=adj_pendg_ind from prem_adj where prem_adj_id=@prem_adj_id_delete
				if(isnull(@adj_pendg_ind,0)<>1)
				begin
				if @debug = 1
				begin
				print '@prem_adj_perd_id_delete:' + convert(varchar(20),@prem_adj_perd_id_delete)
				print '@prem_adj_id_delete:' + convert(varchar(20),@prem_adj_id_delete)
				print '-----------'
				end

				exec dbo.[PrgAISCalcResults] 
				@customer_id = @delete_related_cust_id,
				@premium_adjustment_id = @prem_adj_id_delete,
				@premium_adj_period_id = @prem_adj_perd_id_delete,
				@delete_plb = @delete_plb,
				@delete_ilrf =@delete_ilrf
				end
				else
				begin
					-- Write the validatin error to the application status log tabl
											-- and skip this adjustment from recalculation
				--delete the prem_adj_perd_id from temp table to resrict the recalc
				delete from #adj_perds
				where prem_adj_perd_id=@prem_adj_perd_id_delete
				set @err_message = 'Adjustment is in Pending status and this status needs to be cleared before proceeding with CALC.' 
											+ ';Customer ID: ' + convert(varchar(20),@customer_id_delete) 
											+ ';Premium Adjustment ID: ' + convert(varchar(20),@prem_adj_id_delete) 
											--rollback transaction ModAISCalcDriver
											set @err_msg_output = @err_message
											exec [dbo].[AddAPLCTN_STS_LOG] 
												@premium_adjustment_id = @prem_adj_id_delete,
												@customer_id = @customer_id_delete,
												@premium_adj_prog_id = @delete_prem_adj_pgm_id,
												@err_msg = @err_message,
												@create_user_id = @create_user_id

										
				end
				set @counter = @counter + 1
			end

			delete from #adj_perds
			where prem_adj_perd_id in
				(
					select prd.prem_adj_perd_id 
					from dbo.PREM_ADJ_PERD prd
					inner join dbo.PREM_ADJ_PGM pgm on (prd.custmr_id = pgm.custmr_id) and (prd.prem_adj_pgm_id = pgm.prem_adj_pgm_id)
					where prd.reg_custmr_id = @customer_id
					and pgm.actv_ind = 0
				)

			/******************************************************************
			*			Deleting Unchecked prem_adj_perd's data
			******************************************************************/
			
			set @counter_delete=1
			select @count_delete = count(*) from #deleted_adj_perds
			while @counter_delete <= @count_delete
			begin
				select @delete_prem_adj_perd_id = prem_adj_perd_id_delete
				from #deleted_adj_perds 
				where id = @counter_delete
				
					if(@recalc_prem_adj_perds_Delete is null)
						begin
						set	@recalc_prem_adj_perds_Delete=convert(varchar(20),@delete_prem_adj_perd_id) 
						end
					else
						begin
						set	@recalc_prem_adj_perds_Delete=@recalc_prem_adj_perds_Delete+','+convert(varchar(20),@delete_prem_adj_perd_id)
						end
					set @counter_delete = @counter_delete + 1
				end
				set @len_recalc_delete=len(@recalc_prem_adj_perds_Delete)
				if(@len_recalc_delete>0)
				begin
				exec [dbo].[ModAISDeleteReCalcData] 
							@customer_id_Delete = @customer_id,
							@UnChecked_recalc_prem_adj_perds = @recalc_prem_adj_perds_Delete,
							@debug = @debug
				end

				
			/***************************************************************************
			* Start calculation process. Populates output tables for calculation results
			***************************************************************************/

			declare driver_adj_cur cursor LOCAL FAST_FORWARD READ_ONLY 
			for 
			select
			val_date,
			brkr_id,
			bsn_unt_ofc_id
			from
			(	--Premium
				select 
				prem_adj_pgm_id,
				nxt_valn_dt as val_date,
				brkr_id,
				bsn_unt_ofc_id 
				from dbo.PREM_ADJ_PGM 
				where  custmr_id in
				(
					--Get the master account and the related accounts
					select 
					custmr_id 
					from dbo.CUSTMR
					where 
					custmr_id = @customer_id
					and actv_ind = 1
					union
					select 
					cb.custmr_id 
					from dbo.CUSTMR ca
					inner join dbo.CUSTMR cb on (ca.custmr_id = cb.custmr_rel_id)
					where 
					ca.custmr_id = @customer_id 
					and ca.mstr_acct_ind = 1 -- master account indicator
					and ca.actv_ind = 1 -- active status indicator
					and cb.actv_ind = 1 -- active status indicator
					and cb.custmr_rel_actv_ind = 1 -- customer relation active status indicator
				)
				and prem_adj_pgm_id in 
				(	
					 --brand new calc
					select 
					substring( ',' + @calc_prog_perds + ',', n + 1, 
					charindex( ',', ',' + @calc_prog_perds + ',', n + 1 ) - n - 1 ) as "ProgPerds"
					from #num
					where substring( ',' + @calc_prog_perds + ',', n, 1 ) = ','
					and n < len( ',' + @calc_prog_perds + ',' ) 
					union
					--recalculate prog periods marked for recalc
					select
					prem_adj_pgm_id
					from #adj_perds 
				)
				and nxt_valn_dt <= getdate()
				and actv_ind = 1
				union
				--Non-Premium
				select 
				prem_adj_pgm_id,
				nxt_valn_dt_non_prem_dt as val_date,
				brkr_id,
				bsn_unt_ofc_id
				from dbo.PREM_ADJ_PGM 
				where  custmr_id in
				(
					--Get the master account and the related accounts
					select 
					custmr_id 
					from dbo.CUSTMR
					where 
					custmr_id = @customer_id
					and actv_ind = 1
					union
					select 
					cb.custmr_id 
					from dbo.CUSTMR ca
					inner join dbo.CUSTMR cb on (ca.custmr_id = cb.custmr_rel_id)
					where 
					ca.custmr_id = @customer_id 
					and ca.mstr_acct_ind = 1 -- master account indicator
					and ca.actv_ind = 1 -- active status indicator
					and cb.actv_ind = 1 -- active status indicator
					and cb.custmr_rel_actv_ind = 1 -- customer relation active status indicator
				)
				and prem_adj_pgm_id in 
				(	
					 --brand new calc
					select 
					substring( ',' + @calc_prog_perds + ',', n + 1, 
					charindex( ',', ',' + @calc_prog_perds + ',', n + 1 ) - n - 1 ) as "ProgPerds"
					from #num
					where substring( ',' + @calc_prog_perds + ',', n, 1 ) = ','
					and n < len( ',' + @calc_prog_perds + ',' ) 
					union
					--recalculate prog periods marked for recalc
					select
					prem_adj_pgm_id
					from #adj_perds 
				)
				and nxt_valn_dt <> nxt_valn_dt_non_prem_dt
				and nxt_valn_dt_non_prem_dt <= getdate()
				and actv_ind = 1
			) as res
			group by val_date,brkr_id,bsn_unt_ofc_id



			open driver_adj_cur
			fetch driver_adj_cur into @valn_mm_dt, @brkr_id, @bsn_unt_ofc_id


			--Perform calculation and insertion of output results associated with Premium Adjustment ID and Premium Adjustment Period IDs
			while @@Fetch_Status = 0
				begin
					begin

						if @debug = 1
						begin
						print'*******************OUTER LOOP*********' 

						print' @valn_mm_dt: ' + convert(varchar(20), @valn_mm_dt)  
						print' @brkr_id:- ' + convert(varchar(20), @brkr_id)  
						print' @bsn_unt_ofc_id:- ' + convert(varchar(20), @bsn_unt_ofc_id)  
						end
						/***************************************************
						* Determine if premium adjustment ID already exists 
						* for next_val_dt,brkr_id,bsn_unt_ofc_id. Associate
						* with pre-existing adjustment ID for both: RE-CALC & NEW CALC
						* which is not in transmitted or final invoiced status (incomplete status)
						****************************************************/

						set @exist_adj_id = null

									
						select 
						@exist_adj_id = pa.prem_adj_id
						from dbo.PREM_ADJ pa
						where reg_custmr_id = @customer_id
						and brkr_id = @brkr_id
						and bu_office_id = @bsn_unt_ofc_id
						and valn_dt = @valn_mm_dt
						and pa.adj_sts_typ_id not in (349,352) -- Lookup IDs for applicable statuses: Final Invoice and Transmitted
						and isnull(pa.adj_can_ind,0) = 0 -- not cancelled
						and isnull(pa.adj_void_ind,0) = 0 -- not voided
						and isnull(pa.adj_rrsn_ind,0) = 0 -- not revised
						

						if @debug = 1
						begin
						print '@exist_adj_id: ' + convert(varchar(20),@exist_adj_id)
						end

						if @exist_adj_id is null
						begin
							if @debug = 1
							begin
							print 'Creating a new adjustment'
							end
							insert into dbo.[PREM_ADJ]
						   (
								[reg_custmr_id],
								historical_adj_ind,
								adj_pendg_ind,
								adj_void_ind,
								adj_rrsn_ind,
								adj_can_ind,
							   [valn_dt]
							   ,[brkr_id]
							   ,[bu_office_id]
							   ,[crte_user_id]
							)
							 values
							(
							   @customer_id,
								0,
								0,
								0,
								0,
								0,
							   @valn_mm_dt,
							   @brkr_id,
							   @bsn_unt_ofc_id,
							   @create_user_id--,
							)

							set @prem_adj_id = @@identity
						
						end --end of: if @exist_adj_id is null
						else
						begin
							select @prem_adj_id = @exist_adj_id

						end
						 
		
						update dbo.PREM_ADJ with(rowlock)
						set calc_adj_sts_cd = 'INP',
							adj_sts_typ_id = 346,
							adj_sts_eff_dt = getdate (),
							updt_user_id = @create_user_id,
							updt_dt = getdate()						
						where prem_adj_id = @prem_adj_id
						
						if @debug = 1
						begin
						print '@prem_adj_id: ' + convert(varchar(20),@prem_adj_id)
						end
						--TODO: CREATE NEW LOOKUP FOR CALC-INP STATUS;update adjustment status table correctly

						insert into dbo.[PREM_ADJ_STS]
					   (
						[prem_adj_id]
					   ,[custmr_id]
					   ,[adj_sts_typ_id]
					   ,[qlty_cntrl_dt]
					   ,[crte_user_id]
						)
						 values
					   (
						@prem_adj_id,
						@customer_id,
						346, --Lookup Type: Adjustment statuses lookup value: CALC
						getdate(),
						@create_user_id --TODO
						)

						
						
						--inner loop for adjustment periods

						declare adj_perd_cur cursor LOCAL FAST_FORWARD READ_ONLY 
						for 
						select 
						prem_adj_pgm_id,0 
						from dbo.PREM_ADJ_PGM 
						where custmr_id in
						(
							select 
							custmr_id 
							from dbo.CUSTMR
							where 
							custmr_id = @customer_id
							and actv_ind = 1
							union
							select 
							cb.custmr_id 
							from dbo.CUSTMR ca
							inner join dbo.CUSTMR cb on (ca.custmr_id = cb.custmr_rel_id)
							where 
							ca.custmr_id = @customer_id 
							and ca.mstr_acct_ind = 1 -- master account indicator
							and ca.actv_ind = 1 -- active status indicator
							and cb.actv_ind = 1 -- active status indicator
							and cb.custmr_rel_actv_ind = 1 -- customer relation active status indicator
						)
						and ((nxt_valn_dt = @valn_mm_dt) or (nxt_valn_dt_non_prem_dt = @valn_mm_dt) )
						and brkr_id = @brkr_id
						and bsn_unt_ofc_id = @bsn_unt_ofc_id
						and actv_ind = 1
						and prem_adj_pgm_id in 
						(
							--brand new calc
							select 
							substring( ',' + @calc_prog_perds + ',', n + 1, 
							charindex( ',', ',' + @calc_prog_perds + ',', n + 1 ) - n - 1 ) as "ProgPerds"
							from #num
							where substring( ',' + @calc_prog_perds + ',', n, 1 ) = ','
							and n < len( ',' + @calc_prog_perds + ',' ) 
						)
						--recalculate prog periods marked for recalc
						union
						select
						pap.prem_adj_pgm_id,tp.prem_adj_perd_id
						from #adj_perds tp
						inner join dbo.PREM_ADJ_PGM pap on (tp.prem_adj_pgm_id = pap.prem_adj_pgm_id)
						where pap.custmr_id in
						(
							--Get the master account and the related accounts
							select 
							custmr_id 
							from dbo.CUSTMR
							where 
							custmr_id = @customer_id
							and actv_ind = 1
							union
							select 
							cb.custmr_id 
							from dbo.CUSTMR ca
							inner join dbo.CUSTMR cb on (ca.custmr_id = cb.custmr_rel_id)
							where 
							ca.custmr_id = @customer_id 
							and ca.mstr_acct_ind = 1 -- master account indicator
							and ca.actv_ind = 1 -- active status indicator
							and cb.actv_ind = 1 -- active status indicator
							and cb.custmr_rel_actv_ind = 1 -- customer relation active status indicator
						)
						--and pap.nxt_valn_dt = @valn_mm_dt 
						and ((pap.nxt_valn_dt = @valn_mm_dt) or (pap.nxt_valn_dt_non_prem_dt = @valn_mm_dt) )
						and pap.brkr_id = @brkr_id
						and pap.bsn_unt_ofc_id = @bsn_unt_ofc_id
						and actv_ind = 1


						open adj_perd_cur
						fetch adj_perd_cur into @prem_adj_pgm_id,@exist_prem_adj_perd_id

						while @@Fetch_Status = 0
							begin
								begin

									if @debug = 1
									begin
									print'*******************INNER LOOP FOR ADJ PERIOD*********' 

									print'***************************** @prem_adj_pgm_id: ' + convert(varchar(20), @prem_adj_pgm_id)  
									print'***************************** @exist_prem_adj_perd_id: ' + convert(varchar(20), @exist_prem_adj_perd_id)  
									end
									--Check if the program period is in active status
									select 
									@pgm_perd_sts_typ_id = pgm_perd_sts_typ_id
									from dbo.PREM_ADJ_PGM_STS
									where prem_adj_pgm_sts_id
									in
									(
										--select 
										--max(prem_adj_pgm_sts_id)
										--from dbo.PREM_ADJ_PGM_STS
										--where prem_adj_pgm_id = @prem_adj_pgm_id
										select top 1 prem_adj_pgm_sts_id
										from dbo.PREM_ADJ_PGM_STS 
										where prem_adj_pgm_id = @prem_adj_pgm_id and sts_chk_ind=1 order by pgm_perd_sts_typ_id
									)

									if(@pgm_perd_sts_typ_id <> 342) -- If any status other than ACTIVE status, skip iteration for this program period
									begin
										fetch adj_perd_cur into @prem_adj_pgm_id,@exist_prem_adj_perd_id
										continue
									end
							
									/****************************************************
									* Validation to check premium and non premium dates
									If those two dates are not equal and less than current date
									Roll back the transaction.
									****************************************************/
									declare @cnt int
									set @cnt = 0
									select 
										@cnt = count(*)
										from dbo.PREM_ADJ_PGM 
										where  
										prem_adj_pgm_id = @prem_adj_pgm_id
										and nxt_valn_dt <= getdate()
										and nxt_valn_dt <> nxt_valn_dt_non_prem_dt
										and nxt_valn_dt_non_prem_dt <= getdate()
									
									if @cnt > 0
									begin
										set @err_message = 'Calculation Engine Driver: Valuation dates of premium and non premium are different and both are less than current date.' 
											+ ';Customer ID: ' + convert(varchar(20),@customer_id) 
											+ ';Program Period ID: ' + convert(varchar(20),@prem_adj_pgm_id) 
											--rollback transaction ModAISCalcDriver
											set @err_msg_output = @err_message
											exec [dbo].[AddAPLCTN_STS_LOG] 
												@premium_adjustment_id = @prem_adj_id,
												@customer_id = @customer_id,
												@premium_adj_prog_id = @prem_adj_pgm_id,
												@err_msg = @err_message,
												@create_user_id = @create_user_id

											-- Write the validatin error to the application status log tabl
											-- and skip this program period

											GOTO EndOfPeriodCursorLoop
									end

									/****************************************************
									* Validation to check premium and non premium dates with 
									current dates
									****************************************************/
									set @cnt = 0
									select 
										@cnt = count(*)
										from dbo.PREM_ADJ_PGM 
										where  
										prem_adj_pgm_id = @prem_adj_pgm_id
										and nxt_valn_dt > getdate()
										and nxt_valn_dt_non_prem_dt > getdate()
									
									if @cnt > 0
									begin
										set @err_message = 'Calculation Engine Driver: Premium and Non Premium Valuation dates are greater than current date.' 
											+ ';Customer ID: ' + convert(varchar(20),@customer_id) 
											+ ';Program Period ID: ' + convert(varchar(20),@prem_adj_pgm_id) 
											--rollback transaction ModAISCalcDriver
											set @err_msg_output = @err_message
											exec [dbo].[AddAPLCTN_STS_LOG] 
												@premium_adjustment_id = @prem_adj_id,
												@customer_id = @customer_id,
												@premium_adj_prog_id = @prem_adj_pgm_id,
												@err_msg = @err_message,
												@create_user_id = @create_user_id
											
											-- Write the validatin error to the application status log tabl
											-- and skip this program period
											GOTO EndOfPeriodCursorLoop  
									end
									/****************************************************
									* Validation to check Policy setup
									****************************************************/
									set @cnt = 0
									select 
										@cnt = count(*)
										from dbo.COML_AGMT 
										where  
										prem_adj_pgm_id = @prem_adj_pgm_id
										
									
									if @cnt = 0
									begin
										set @err_message = 'Calculation Engine Driver: Policy setup is incomplete for the ' 
											+ 'Customer ID: ' + convert(varchar(20),@customer_id) 
											+ ';Program Period ID: ' + convert(varchar(20),@prem_adj_pgm_id) 
											--rollback transaction ModAISCalcDriver
											set @err_msg_output = @err_message
											exec [dbo].[AddAPLCTN_STS_LOG] 
												@premium_adjustment_id = @prem_adj_id,
												@customer_id = @customer_id,
												@premium_adj_prog_id = @prem_adj_pgm_id,
												@err_msg = @err_message,
												@create_user_id = @create_user_id
											
											-- Write the validatin error to the application status log tabl
											-- and skip this program period
											GOTO EndOfPeriodCursorLoop  
									end
									/****************************************************
									* Validation to check for adjustmet with current  
									val date
									****************************************************/
									set @cnt = 0
									select @cnt=count(*) from prem_adj pa
									inner join prem_adj_perd pap on pap.prem_adj_id=pa.prem_adj_id
									inner join prem_adj_pgm pg on pg.prem_adj_pgm_id=pap.prem_adj_pgm_id
									where (pa.valn_dt=pg.nxt_valn_dt or pa.valn_dt=pg.nxt_valn_dt_non_prem_dt) 
									and pa.adj_can_ind<>1 
									and pa.adj_void_ind<>1
									and pa.adj_rrsn_ind<>1
									and substring(pa.fnl_invc_nbr_txt,1,3)<>'RTV'
									and pg.prem_adj_pgm_id=@prem_adj_pgm_id
								
									if @cnt > 0
									begin
										--retrieving open adjustment id on same val date
									select @open_adj_id_on_same_valdate=pa.prem_adj_id from prem_adj pa
									inner join prem_adj_perd pap on pap.prem_adj_id=pa.prem_adj_id
									inner join prem_adj_pgm pg on pg.prem_adj_pgm_id=pap.prem_adj_pgm_id
									where (pa.valn_dt=pg.nxt_valn_dt or pa.valn_dt=pg.nxt_valn_dt_non_prem_dt) 
									and pa.adj_can_ind<>1 
									and pa.adj_void_ind<>1
									and pa.adj_rrsn_ind<>1
									and substring(pa.fnl_invc_nbr_txt,1,3)<>'RTV'
									and pg.prem_adj_pgm_id=@prem_adj_pgm_id

										set @err_message = 'Calculation Engine Driver: Already an adjustment exisists with this val date for this program period.' 
											+ ';Customer ID: ' + convert(varchar(20),@customer_id) 
											+ ';Program Period ID: ' + convert(varchar(20),@prem_adj_pgm_id)
											+ ';Current open adjustment ID: ' + convert(varchar(20),@open_adj_id_on_same_valdate)  
											--rollback transaction ModAISCalcDriver
											set @err_msg_output = @err_message
											exec [dbo].[AddAPLCTN_STS_LOG] 
												@premium_adjustment_id = @prem_adj_id,
												@customer_id = @customer_id,
												@premium_adj_prog_id = @prem_adj_pgm_id,
												@err_msg = @err_message,
												@create_user_id = @create_user_id
											
											-- Write the validatin error to the application status log tabl
											-- and skip this program period
											GOTO EndOfPeriodCursorLoop  
									end
									
									/****************************************************
									* Validation for open adjustment for a program period
									****************************************************/
									if @debug = 1
									begin
									print 'start open adjustment check'
									end
									set @prog_adj_criteria_changed = 0

									select 
									@prem_adj_valn_dt = valn_dt,
									@prem_adj_brkr_id = brkr_id,
									@prem_adj_bu_office_id = bu_office_id
									from dbo.PREM_ADJ
									where prem_adj_id = (select 
										prem_adj_id 
										from dbo.PREM_ADJ_PERD
										where prem_adj_perd_id = @exist_prem_adj_perd_id
														)
									if @debug = 1
									begin
									print '@prem_adj_valn_dt ' + convert(varchar(25),@prem_adj_valn_dt)
									print '@prem_adj_brkr_id ' + convert(varchar(25),@prem_adj_brkr_id)
									print '@prem_adj_bu_office_id ' + convert(varchar(25),@prem_adj_bu_office_id)
									end
									select 
									@curr_valn_mm_dt = valn_dt,
									@curr_brkr_id = brkr_id,
									@curr_bsn_unt_ofc_id = bu_office_id
									from dbo.PREM_ADJ
									where prem_adj_id = @prem_adj_id

									if @debug = 1
									begin
									print '@prem_adj_id ' + convert(varchar(25),@prem_adj_id)
									print '@curr_valn_mm_dt ' + convert(varchar(25),@curr_valn_mm_dt)
									print '@curr_brkr_id ' + convert(varchar(25),@curr_brkr_id)
									print '@curr_bsn_unt_ofc_id ' + convert(varchar(25),@curr_bsn_unt_ofc_id)
									end

									if( (@curr_valn_mm_dt <> @prem_adj_valn_dt) or (@curr_brkr_id <> @prem_adj_brkr_id) or (@curr_bsn_unt_ofc_id <> @prem_adj_bu_office_id))
									begin
										if @debug = 1
										begin
										print 'set @prog_adj_criteria_changed = 1'
										end
										set @prog_adj_criteria_changed = 1
									end

									if (
											(not exists (select 1 from #adj_perds where prem_adj_pgm_id = @prem_adj_pgm_id))
											or
											((exists (select 1 from #adj_perds where prem_adj_pgm_id = @prem_adj_pgm_id)) and (@prog_adj_criteria_changed = 1))

										)
									begin --start open adjustment check
										if @debug = 1
										begin
										print 'Open adj check code block begins'
										end
										select @temp_open_adj_id = pra.prem_adj_id
										from dbo.PREM_ADJ pra
										inner join dbo.PREM_ADJ_PERD prd on (pra.reg_custmr_id = prd.reg_custmr_id) and (pra.prem_adj_id = prd.prem_adj_id)
										where prd.prem_adj_pgm_id = @prem_adj_pgm_id
										and pra.prem_adj_id not in
										(
											--adjustments completed for a program period
											select prem_adj_id from dbo.PREM_ADJ_STS where prem_adj_id in
											(
												select pa.prem_adj_id
												from dbo.PREM_ADJ pa
												inner join dbo.PREM_ADJ_STS ps on (pa.prem_adj_id = ps.prem_adj_id) and (pa.reg_custmr_id = ps.custmr_id)
												inner join dbo.PREM_ADJ_PERD prd on (pa.reg_custmr_id = prd.reg_custmr_id) and (pa.prem_adj_id = prd.prem_adj_id)
												where 
												prd.prem_adj_pgm_id = @prem_adj_pgm_id
											)
											and adj_sts_typ_id  in (349,352) -- Lookup IDs for applicable statuses: Final Invoice and Transmitted
										)
										and isnull(pra.adj_can_ind,0) = 0 -- not cancelled
										and isnull(pra.adj_void_ind,0) = 0 -- not voided
										and isnull(pra.adj_rrsn_ind,0) = 0 -- not revised


										if @debug = 1
										begin
										print 'In open adj check before verification - @temp_open_adj_id ' + convert(varchar(25),  isnull(@temp_open_adj_id,0)  )
										end
										select 
										@diff_prem_nonprem_val = datediff(d ,nxt_valn_dt,nxt_valn_dt_non_prem_dt) 
										from dbo.PREM_ADJ_PGM 
										where  
										prem_adj_pgm_id = @prem_adj_pgm_id
										if @debug = 1
										begin
										print 'In open adj final check - @diff_prem_nonprem_val ' + convert(varchar(25),  isnull(@diff_prem_nonprem_val,0)  )
										end
										if(@exist_prem_adj_perd_id <> 0)  -- Re-calc of existing adj
										begin

											if(@diff_prem_nonprem_val <> 0) -- Premium and non-premium val dates differ
											begin
												-- Ignore the case of extra iteration 
												if(@prem_adj_id <> (select 
																	prem_adj_id 
																	from dbo.PREM_ADJ_PERD
																	where prem_adj_perd_id = @exist_prem_adj_perd_id
																	))
												begin
													set @temp_open_adj_id = null
												end
											end

											if(@diff_prem_nonprem_val = 0) -- Premium and non-premium val dates same
											begin
												if(@prog_adj_criteria_changed = 1) -- Adjustment creation criteria changed
												begin
													-- If current adjustment determined to be open adj then ignore 
													if(@prem_adj_id = @temp_open_adj_id)
													begin
														set @temp_open_adj_id = null
													end
												end
											end

										end -- end of: if(@exist_prem_adj_perd_id <> 0)
										else
										begin -- Not re-calc
											if(@diff_prem_nonprem_val <> 0) -- Premium and non-premium val dates differ
											begin
	--											if exists 
	--											(select *  
	--											from dbo.PREM_ADJ_PGM pag
	--											inner join dbo.PREM_ADJ pa on (	(pag.nxt_valn_dt = pa.valn_dt) or (pag.nxt_valn_dt_non_prem_dt= pa.valn_dt) )
	--											where prem_adj_pgm_id = @prem_adj_pgm_id
	--											and prem_adj_id = @temp_open_adj_id
	--											) and (@temp_open_adj_id is not null)
	--											begin
												set @temp_open_adj_id = null
	--											end

											end
										end

										if @debug = 1
										begin
										print 'In open adj check after verification - @temp_open_adj_id ' + convert(varchar(25),  isnull(@temp_open_adj_id,0)  )
										end

										-- Program Periods considered for this adjustment

										-- Premium included in current adjustment
										if exists 
										(
											select 1
											from
											(
												select
												prem_adj_pgm_id 
												from dbo.PREM_ADJ_PGM 
												where ((nxt_valn_dt = @valn_mm_dt) or (nxt_valn_dt_non_prem_dt = @valn_mm_dt) )
												and brkr_id = @brkr_id
												and bsn_unt_ofc_id = @bsn_unt_ofc_id
												and actv_ind = 1
												and prem_adj_pgm_id in 
												(
													--brand new calc
													select 
													substring( ',' + @calc_prog_perds + ',', n + 1, 
													charindex( ',', ',' + @calc_prog_perds + ',', n + 1 ) - n - 1 ) as "ProgPerds"
													from #num
													where substring( ',' + @calc_prog_perds + ',', n, 1 ) = ','
													and n < len( ',' + @calc_prog_perds + ',' ) 
												)
												--recalculate prog periods marked for recalc
												/*union
												select
												pap.prem_adj_pgm_id
												from #adj_perds tp
												inner join dbo.PREM_ADJ_PGM pap on (tp.prem_adj_pgm_id = pap.prem_adj_pgm_id)
												where ((pap.nxt_valn_dt = @valn_mm_dt) or (pap.nxt_valn_dt_non_prem_dt = @valn_mm_dt) )
												and pap.brkr_id = @brkr_id
												and pap.bsn_unt_ofc_id = @bsn_unt_ofc_id
												and actv_ind = 1 */
											) as app
											inner join dbo.PREM_ADJ_PGM pgm on (app.prem_adj_pgm_id = pgm.prem_adj_pgm_id)
											where pgm.nxt_valn_dt = @valn_mm_dt
											and pgm.actv_ind = 1
										)
										begin
											if exists(select 1 from dbo.PREM_ADJ_RETRO where prem_adj_id = @temp_open_adj_id )
												set @open_adj_id = @temp_open_adj_id
											if @debug = 1
											begin
											print 'In PREMIUM open adj check - @open_adj_id ' + convert(varchar(25),  isnull(@open_adj_id,0)  )
											end
										end
										--Non-premium included in current adjustment
										if exists 
										(
											select 1
											from
											(
												select
												prem_adj_pgm_id 
												from dbo.PREM_ADJ_PGM 
												where ((nxt_valn_dt = @valn_mm_dt) or (nxt_valn_dt_non_prem_dt = @valn_mm_dt) )
												and brkr_id = @brkr_id
												and bsn_unt_ofc_id = @bsn_unt_ofc_id
												and actv_ind = 1
												and prem_adj_pgm_id in 
												(
													--brand new calc
													select 
													substring( ',' + @calc_prog_perds + ',', n + 1, 
													charindex( ',', ',' + @calc_prog_perds + ',', n + 1 ) - n - 1 ) as "ProgPerds"
													from #num
													where substring( ',' + @calc_prog_perds + ',', n, 1 ) = ','
													and n < len( ',' + @calc_prog_perds + ',' ) 
												)
												--recalculate prog periods marked for recalc
												/*union
												select
												pap.prem_adj_pgm_id
												from #adj_perds tp
												inner join dbo.PREM_ADJ_PGM pap on (tp.prem_adj_pgm_id = pap.prem_adj_pgm_id)
												where ((pap.nxt_valn_dt = @valn_mm_dt) or (pap.nxt_valn_dt_non_prem_dt = @valn_mm_dt) )
												and pap.brkr_id = @brkr_id
												and pap.bsn_unt_ofc_id = @bsn_unt_ofc_id
												and actv_ind = 1*/
											) as app
											inner join dbo.PREM_ADJ_PGM pgm on (app.prem_adj_pgm_id = pgm.prem_adj_pgm_id)
											where pgm.nxt_valn_dt_non_prem_dt = @valn_mm_dt
											and pgm.actv_ind = 1
										)
										begin
											if exists(select 1 from dbo.PREM_ADJ_PARMET_SETUP where prem_adj_id = @temp_open_adj_id)
												set @open_adj_id = @temp_open_adj_id
											if @debug = 1
											begin
											print 'In NON-PREMIUM open adj check - @open_adj_id ' + convert(varchar(25),  isnull(@open_adj_id,0)  )
											end
										end

										if(@exist_prem_adj_perd_id <> 0)
										begin
											if(@prem_adj_id <> (select 
																prem_adj_id 
																from dbo.PREM_ADJ_PERD
																where prem_adj_perd_id = @exist_prem_adj_perd_id
																))
											begin

												if((exists (select 1 from #adj_perds where prem_adj_pgm_id = @prem_adj_pgm_id)) and (@prog_adj_criteria_changed = 1))
													set @open_adj_id = @temp_open_adj_id
											end
										end -- end of: if(@exist_prem_adj_perd_id <> 0)
										if @debug = 1
										begin
										print 'In open adj final check - @open_adj_id ' + convert(varchar(25),  isnull(@open_adj_id,0)  )
										end

										if(@open_adj_id is not null)
										begin
											set @err_message = 'Calculation Engine Driver: A new adjustment for a program period cannot be started when an associated adjustment is in open status.' 
											+ ';Customer ID: ' + convert(varchar(20),@customer_id) 
											+ ';Currently open adjustment ID: ' + convert(varchar(20),@open_adj_id) 
											+ ';Program Period ID: ' + convert(varchar(20),@prem_adj_pgm_id) 
											--rollback transaction ModAISCalcDriver
											set @err_msg_output = @err_message
											exec [dbo].[AddAPLCTN_STS_LOG] 
												@premium_adjustment_id = @prem_adj_id,
												@customer_id = @customer_id,
												@premium_adj_prog_id = @prem_adj_pgm_id,
												@err_msg = @err_message,
												@create_user_id = @create_user_id

											-- Write the validatin error to the application status log tabl
											-- and skip this program period
											GOTO EndOfPeriodCursorLoop

										end
									end --end of: open adjustment check
									if @debug = 1
									begin
									print 'end of open adjustment check'
									end
									-- Find the related account id in case the prog period is associated
									-- with related / child account.
									select 
									@related_cust_id = custmr_id
									from
									dbo.PREM_ADJ_PGM
									where prem_adj_pgm_id = @prem_adj_pgm_id

									/*******************************************************
									* Validation to check for adjustmet with historical ind
									
									********************************************************/							
									if @exist_adj_id is not null
									begin
									select @historical_adj_ind=historical_adj_ind from prem_adj where prem_adj_id=@exist_adj_id
									if(isnull(@historical_adj_ind,0)=1)
									begin
				
									/**************************
									* Determine first adjustment
									**************************/

									exec @cnt_prev_adjs = [dbo].[fn_DetermineFirstAdjustment]
										@premium_adj_prog_id = @prem_adj_pgm_id,
										@adj_parmet_typ_id = 399 


									if (@cnt_prev_adjs > 0) -- No existing adjustments; this is initial adjustment
									begin
									set @cnt = 0
									select @cnt=count(*) from prem_adj pa
									inner join prem_adj_perd pap on pap.prem_adj_id=pa.prem_adj_id
									where pa.adj_sts_typ_id in (349,352)
									and pa.adj_can_ind<>1 
									and pa.adj_void_ind<>1
									and pa.adj_rrsn_ind<>1
									and substring(pa.fnl_invc_nbr_txt,1,3)<>'RTV'
									and pap.prem_adj_pgm_id=@prem_adj_pgm_id
									and historical_adj_ind=1
									end

									if @cnt > 0
									begin
										--retrieving Historical adjustment id on same prem_adj_pgm_id
									select @historical_adj_id=pa.prem_adj_id from prem_adj pa
									inner join prem_adj_perd pap on pap.prem_adj_id=pa.prem_adj_id
									where pa.adj_sts_typ_id in (349,352)
									and pa.adj_can_ind<>1 
									and pa.adj_void_ind<>1
									and pa.adj_rrsn_ind<>1
									and substring(pa.fnl_invc_nbr_txt,1,3)<>'RTV'
									and pap.prem_adj_pgm_id=@prem_adj_pgm_id
									and historical_adj_ind=1

										set @err_message = 'Calculation Engine Driver: Already an historical adjustment exists for this program period.' 
											+ ';Customer ID: ' + convert(varchar(20),@customer_id) 
											+ ';Program Period ID: ' + convert(varchar(20),@prem_adj_pgm_id)
											+ ';Current historical adjustment ID: ' + convert(varchar(20),@historical_adj_id)  
											--rollback transaction ModAISCalcDriver
											set @err_msg_output = @err_message
											exec [dbo].[AddAPLCTN_STS_LOG] 
												@premium_adjustment_id = @prem_adj_id,
												@customer_id = @customer_id,
												@premium_adj_prog_id = @prem_adj_pgm_id,
												@err_msg = @err_message,
												@create_user_id = @create_user_id
											
											-- Write the validatin error to the application status log tabl
											-- and skip this program period
											GOTO EndOfPeriodCursorLoop  


									end
									
									end

									end

									--insert into adjust period table for each prog period
									if (@exist_prem_adj_perd_id = 0) -- No existing prem_adj_perd_id associated
									begin
										insert into dbo.[PREM_ADJ_PERD]
									   (
											[prem_adj_id]
										   ,[reg_custmr_id]
										   ,[custmr_id]
										   ,[prem_adj_pgm_id]
										   --,[adj_nbr_txt]
										   ,[crte_user_id]
										)
										 values
									   (
											@prem_adj_id,
											@customer_id,
											@related_cust_id,--@customer_id,
											@prem_adj_pgm_id,
											--@adj_nbr_txt,
											@create_user_id
										)
										
										-- with the returned identity adjust period id call sub-ordinate stored procedures
										set @prem_adj_perd_id = @@identity
									end
									else
									begin -- With existing prem_adj_perd_id
										set @prem_adj_perd_id = @exist_prem_adj_perd_id


										select 
										@prem_adj_valn_dt = valn_dt,
										@prem_adj_brkr_id = brkr_id,
										@prem_adj_bu_office_id = bu_office_id
										from dbo.PREM_ADJ
										where prem_adj_id = (select 
											prem_adj_id 
											from dbo.PREM_ADJ_PERD
											where prem_adj_perd_id = @exist_prem_adj_perd_id
															)
										if @debug = 1
										begin
										print '@prem_adj_valn_dt ' + convert(varchar(25),@prem_adj_valn_dt)
										print '@prem_adj_brkr_id ' + convert(varchar(25),@prem_adj_brkr_id)
										print '@prem_adj_bu_office_id ' + convert(varchar(25),@prem_adj_bu_office_id)
										end

										select 
										@curr_valn_mm_dt = valn_dt,
										@curr_brkr_id = brkr_id,
										@curr_bsn_unt_ofc_id = bu_office_id
										from dbo.PREM_ADJ
										where prem_adj_id = @prem_adj_id
										if @debug = 1
										begin
										print '@prem_adj_id ' + convert(varchar(25),@prem_adj_id)
										print '@curr_valn_mm_dt ' + convert(varchar(25),@curr_valn_mm_dt)
										print '@curr_brkr_id ' + convert(varchar(25),@curr_brkr_id)
										print '@curr_bsn_unt_ofc_id ' + convert(varchar(25),@curr_bsn_unt_ofc_id)
										end

										if( (@curr_valn_mm_dt <> @prem_adj_valn_dt) or (@curr_brkr_id <> @prem_adj_brkr_id) or (@curr_bsn_unt_ofc_id <> @prem_adj_bu_office_id))
										begin
											if @debug = 1
											begin
											print 'set @prog_adj_criteria_changed = 1'
											end
											set @prog_adj_criteria_changed = 1
										end

										select 
										@diff_prem_nonprem_val = datediff(d ,nxt_valn_dt,nxt_valn_dt_non_prem_dt) 
										from dbo.PREM_ADJ_PGM 
										where  
										prem_adj_pgm_id = @prem_adj_pgm_id
										if @debug = 1
										begin
										print '@diff_prem_nonprem_val: ' + convert(varchar(25),@diff_prem_nonprem_val)
										end

	--									if(@valn_mm_dt <> @prem_adj_valn_dt)
										if(@diff_prem_nonprem_val <> 0)
										begin
											if(
												@prem_adj_id <> (select 
												prem_adj_id 
												from dbo.PREM_ADJ_PERD
												where prem_adj_perd_id = @exist_prem_adj_perd_id
																)
											   )
											begin
												if @debug = 1
												begin
												print 'skipping iteration'
												end
												fetch adj_perd_cur into @prem_adj_pgm_id,@exist_prem_adj_perd_id
												continue
											end
										end

										if(@prog_adj_criteria_changed = 1)
										begin
											print 'delete @prog_adj_criteria_changed'
	--										delete from dbo.PREM_ADJ_PAID_LOS_BIL
	--										where prem_adj_perd_id = @exist_prem_adj_perd_id
	--
	--										delete from dbo.PREM_ADJ_COMB_ELEMTS
	--										where prem_adj_perd_id = @exist_prem_adj_perd_id
	--
	--										delete from dbo.PREM_ADJ_MISC_INVC
	--										where prem_adj_perd_id = @exist_prem_adj_perd_id
	--
	--										delete from dbo.PREM_ADJ_PERD
	--										where prem_adj_perd_id = @exist_prem_adj_perd_id
	--										
	--										if not exists(select * from dbo.PREM_ADJ_PERD where custmr_id = @related_cust_id and prem_adj_id = @prem_adj_id and prem_adj_pgm_id = @prem_adj_pgm_id)
	--										begin
	--											insert into dbo.[PREM_ADJ_PERD]
	--										   (
	--												[prem_adj_id]
	--											   ,[reg_custmr_id]
	--											   ,[custmr_id]
	--											   ,[prem_adj_pgm_id]
	--											   --,[adj_nbr_txt]
	--											   ,[crte_user_id]
	--											)
	--											 values
	--										   (
	--												@prem_adj_id,
	--												@customer_id,
	--												@related_cust_id,--@customer_id,
	--												@prem_adj_pgm_id,
	--												--@adj_nbr_txt,
	--												@create_user_id
	--											)
	--											
	--											-- with the returned identity adjust period id call sub-ordinate stored procedures
	--											set @prem_adj_perd_id = @@identity
	--										end
	--										else
	--										begin
	--											select @prem_adj_perd_id = prem_adj_perd_id from dbo.PREM_ADJ_PERD where custmr_id = @related_cust_id and prem_adj_id = @prem_adj_id and prem_adj_pgm_id = @prem_adj_pgm_id
	--										end

										end --end of: if( (@valn_mm_dt <> @prem_adj_valn_dt) or (@brkr_id <> @prem_adj_brkr_id) or (@bsn_unt_ofc_id <> @prem_adj_bu_office_id))
										else
										begin
											select 
											@prem_adj_id = prem_adj_id 
											from dbo.PREM_ADJ_PERD
											where prem_adj_perd_id = @exist_prem_adj_perd_id
										end



									end --end: With existing prem_adj_perd_id
									
									if @debug = 1
									begin
									print 'FINAL @prem_adj_id: ' + convert(varchar(20),@prem_adj_id)
									print 'FINAL @exist_prem_adj_perd_id: ' + convert(varchar(20),@exist_prem_adj_perd_id)
									print 'FINAL @prem_adj_perd_id: ' + convert(varchar(20),@prem_adj_perd_id)
									end
									insert into #processed_adj_perds
									(
									[prem_adj_perd_id],
									[related_customer_id]
									)
									values
									(
									@prem_adj_perd_id,
									@related_cust_id
									)


									/**************************************
									* Invoke sub-ordinate stored procedures
									**************************************/

									exec dbo.ModAISLossLimitExcess
									@custmr_id = @related_cust_id, -- @customer_id,
									@prem_adj_pgm_id = @prem_adj_pgm_id
									
									if (@delete_plb = 1)
									begin							
										exec dbo.ModAISCalcLSIPaidLossBilling
										@premium_adjustment_id = @prem_adj_id,
										@customer_id = @related_cust_id,-- @customer_id,
										@premium_adj_prog_id = @prem_adj_pgm_id,
										@create_user_id = @create_user_id,
										@err_msg_op = @err_msg_output output


										exec dbo.ModAISCalcTPA
										@premium_adj_period_id = @prem_adj_perd_id,
										@premium_adjustment_id = @prem_adj_id,
										@customer_id = @related_cust_id,-- @customer_id,
										@premium_adj_prog_id = @prem_adj_pgm_id,
										@create_user_id = @create_user_id,
										@err_msg_op = @err_msg_output output
									end


									if exists
									(
										select 1
										from dbo.PREM_ADJ_PGM_SETUP
										where custmr_id = @related_cust_id
										and prem_adj_pgm_id = @prem_adj_pgm_id
										and adj_parmet_typ_id = 398 -- Adj. Parameter type ID for ADJUSTMENT PARAMETER TYPE -> CHF
										and actv_ind = 1
									)
									begin
										exec dbo.ModAISCalcCHF --AddPremAdjCHF 
										@premium_adj_period_id = @prem_adj_perd_id,
										@premium_adjustment_id = @prem_adj_id,
										@customer_id = @related_cust_id,-- @customer_id,
										@premium_adj_prog_id = @prem_adj_pgm_id,
										@updt_user_id = @create_user_id,
										@create_user_id = @create_user_id,
										@err_msg_op = @err_msg_output output
									end

									if exists
									(
										select 1
										from dbo.PREM_ADJ_PGM_SETUP
										where custmr_id = @related_cust_id
										and prem_adj_pgm_id = @prem_adj_pgm_id
										and adj_parmet_typ_id = 401 -- Adj. Parameter type ID for ADJUSTMENT PARAMETER TYPE -> LBA
										and actv_ind = 1
									)
									begin
										exec dbo.ModAISCalcLBA 
										@premium_adj_period_id = @prem_adj_perd_id,
										@premium_adjustment_id = @prem_adj_id,
										@customer_id = @related_cust_id,-- @customer_id,
										@premium_adj_prog_id = @prem_adj_pgm_id,
										@create_user_id = @create_user_id,
										@err_msg_op = @err_msg_output output,
										@debug = @debug
									end
									
									if exists
									(
										select 1
										from dbo.PREM_ADJ_PGM_SETUP
										where custmr_id = @related_cust_id
										and prem_adj_pgm_id = @prem_adj_pgm_id
										and adj_parmet_typ_id = 400 -- Adj. Parameter type ID for ADJUSTMENT PARAMETER TYPE -> ILRF
										and actv_ind = 1
									)
									begin
										exec dbo.ModAISCalcILRF 
										@premium_adj_period_id = @prem_adj_perd_id,
										@premium_adjustment_id = @prem_adj_id,
										@customer_id = @related_cust_id,-- @customer_id,
										@delete_ilrf=@delete_ilrf,
										@premium_adj_prog_id = @prem_adj_pgm_id,
										@create_user_id = @create_user_id,
										@err_msg_op = @err_msg_output output,
										@debug = @debug
									end
									
									if exists
									(
										select 1
										from dbo.PREM_ADJ_PGM_SETUP
										where custmr_id = @related_cust_id
										and prem_adj_pgm_id = @prem_adj_pgm_id
										and adj_parmet_typ_id = 399 -- Adj. Parameter type ID for ADJUSTMENT PARAMETER TYPE -> ESCROW
										and actv_ind = 1
									)
									begin
										exec ModAISCalcEscrow 
										@premium_adj_period_id = @prem_adj_perd_id,
										@premium_adjustment_id = @prem_adj_id,
										@customer_id = @related_cust_id,-- @customer_id,
										@premium_adj_prog_id = @prem_adj_pgm_id,
										@create_user_id = @create_user_id,
										@err_msg_op = @err_msg_output output
									end

									exec [dbo].[ModAISCalcERP] 
									@premium_adj_period_id = @prem_adj_perd_id,
									@premium_adjustment_id = @prem_adj_id,
									@customer_id = @related_cust_id,-- @customer_id,
									@premium_adj_prog_id = @prem_adj_pgm_id,
									@create_user_id = @create_user_id,
									@err_msg_op = @err_msg_output output,
									@debug = @debug


									exec [dbo].[AddPREM_ADJ_PERD_TOT] 
									@premium_adj_period_id = @prem_adj_perd_id,
									@premium_adjustment_id = @prem_adj_id,
									@customer_id = @related_cust_id,-- @customer_id,
									@premium_adj_prog_id = @prem_adj_pgm_id,
									@create_user_id = @create_user_id

									--This update statement will update P,NP and B in code of perd table.
									update prem_adj_perd with(rowlock)
											set prem_non_prem_cd = dbo.fn_GetDeterminePerdPremStatus(@related_cust_id,  @valn_mm_dt, @brkr_id, @bsn_unt_ofc_id,@prem_adj_pgm_id)
									where prem_adj_perd_id = @prem_adj_perd_id
									

								end

								EndOfPeriodCursorLoop:			-- GOTO Statement
								fetch adj_perd_cur into @prem_adj_pgm_id,@exist_prem_adj_perd_id

							end --end of cursor adj_perd_cur / while loop
						close adj_perd_cur
						deallocate adj_perd_cur

						--TODO: Check for first adjustment

						update dbo.PREM_ADJ_PERD with(rowlock)
							set adj_nbr_txt = txt.adj_text,
								adj_nbr = txt.row_no
						from dbo.PREM_ADJ_PERD pap
						inner join
						(
							select 
							d.prem_adj_id,
							d.prem_adj_pgm_id,
							d.strt_dt,
							d.row_no,
							nl.adj_nbr_txt + ' Adjustment' as adj_text
							from
							(
								select 
								prd.prem_adj_id,pgm.prem_adj_pgm_id,pgm.strt_dt,dt.row_no 
								from dbo.PREM_ADJ_PERD prd
								inner join dbo.PREM_ADJ_PGM pgm on (prd.prem_adj_pgm_id = pgm.prem_adj_pgm_id) and (prd.custmr_id = pgm.custmr_id)
								inner join
								(
									select prd.prem_adj_id,pgm.strt_dt,row_number() over(order by pgm.strt_dt desc) as row_no 
									from dbo.PREM_ADJ_PERD prd
									inner join dbo.PREM_ADJ_PGM pgm on (prd.prem_adj_pgm_id = pgm.prem_adj_pgm_id) and (prd.custmr_id = pgm.custmr_id)
									where prd.prem_adj_id = @prem_adj_id
									and pgm.actv_ind = 1
									and isnull(prd.adj_nbr_mnl_overrid_ind,0)<>1
									group by prd.prem_adj_id,pgm.strt_dt
								) dt
								on (prd.prem_adj_id = dt.prem_adj_id) and (pgm.strt_dt = dt.strt_dt)
								where prd.prem_adj_id = @prem_adj_id
								and pgm.actv_ind = 1
								and isnull(prd.adj_nbr_mnl_overrid_ind,0)<>1
							) d
							inner join dbo.ADJ_NBR_LKUP nl on (d.row_no = nl.adj_numercal_nbr)					
						) txt
						on (pap.prem_adj_id = txt.prem_adj_id) and (pap.prem_adj_pgm_id = txt.prem_adj_pgm_id)
						where pap.prem_adj_id = @prem_adj_id
						and isnull(pap.adj_nbr_mnl_overrid_ind,0)<>1

						 -- Call Aries Transmittal procedure
						exec [dbo].[ModAIS_TransmittalToARiES] 
						@prem_adj_id = @prem_adj_id,
						@rel_prem_adj_id = null,
						@err_msg_output = @err_msg_output output,
						@Ind = 1 --Normal

						-- Completed processing adjustment ID; update status for the adjustment ID
						-- from INP to CMP
						 
						update dbo.PREM_ADJ with(rowlock)
						set calc_adj_sts_cd = 'CMP',
							updt_user_id = @create_user_id,
							updt_dt = getdate()
						where prem_adj_id = @prem_adj_id
						
						--------------------------------------------------
						--start of setup validation
						--------------------------------------------------
						
						declare @cnt_paramet_setup int
						declare @cnt_retro_setup int
						declare @cnt_PLB_setup int
						declare @invalid_setup_prem_adj_pgm_id int
						declare @invalid_setup_prem_adj_perd_id int
						set @cnt_paramet_setup=0
						set @cnt_retro_setup=0
						set @cnt_PLB_setup=0
				

					
						declare perd_cur_invalid_setup cursor LOCAL FAST_FORWARD READ_ONLY 
						for 
						select 
						prem_adj_pgm_id,
						prem_adj_perd_id
						from dbo.PREM_ADJ_PERD 
						where  prem_adj_id=@prem_adj_id

						open perd_cur_invalid_setup
						fetch perd_cur_invalid_setup into @invalid_setup_prem_adj_pgm_id,@invalid_setup_prem_adj_perd_id

						while @@Fetch_Status = 0
							begin
								begin
								select @cnt_paramet_setup=count(*) from dbo.PREM_ADJ_PARMET_SETUP where prem_adj_perd_id=@invalid_setup_prem_adj_perd_id
								select @cnt_retro_setup=count(*) from dbo.PREM_ADJ_RETRO where prem_adj_perd_id=@invalid_setup_prem_adj_perd_id
								select @cnt_PLB_setup=count(*) from PREM_ADJ_PAID_LOS_BIL where prem_adj_perd_id=@invalid_setup_prem_adj_perd_id
								if (@cnt_paramet_setup=0 and @cnt_retro_setup=0 and @cnt_PLB_setup=0)
								begin
								set @err_message = 'Calculation Engine Driver: Setup is incomplete for the ' 
								+ 'customer ID: ' + convert(varchar(20),@customer_id) 
								+ ';Program Period ID: ' + convert(varchar(20),@invalid_setup_prem_adj_pgm_id) 
								set @err_msg_output = @err_message
								exec [dbo].[AddAPLCTN_STS_LOG] 
									@premium_adjustment_id = @prem_adj_id,
									@customer_id = @customer_id,
									@premium_adj_prog_id = @invalid_setup_prem_adj_pgm_id,
									@err_msg = @err_message,
									@create_user_id = @create_user_id
									
								delete from dbo.PREM_ADJ_LOS_REIM_FUND_POST WITH (ROWLOCK)
								where prem_adj_perd_id = @invalid_setup_prem_adj_perd_id

								delete from dbo.PREM_ADJ_PERD WITH(ROWLOCK)
								where prem_adj_perd_id = @invalid_setup_prem_adj_perd_id
								if not exists(select * from dbo.PREM_ADJ_PERD where prem_adj_id = @prem_adj_id )
								begin
								--Delete Premium Adjustment Status
								delete from dbo.PREM_ADJ_STS WITH (ROWLOCK)
								where prem_adj_id = @prem_adj_id
																
								--Delete Premium Adjustment
								delete from dbo.PREM_ADJ WITH (ROWLOCK)
								where prem_adj_id = @prem_adj_id
								end
					
								end

							 end


			
					fetch perd_cur_invalid_setup into @invalid_setup_prem_adj_pgm_id,@invalid_setup_prem_adj_perd_id
				end --end of cursor perd_cur_invalid_setup / while loop
			close perd_cur_invalid_setup
			deallocate perd_cur_invalid_setup

					
--				--------------------------------------------------
--						--end of setup validation
--						--------------------------------------------------

					end
					fetch driver_adj_cur into @valn_mm_dt, @brkr_id, @bsn_unt_ofc_id

				end --end of cursor driver_adj_cur / while loop
			close driver_adj_cur
			deallocate driver_adj_cur


			select * from #processed_adj_perds

			select @count = count(*) from #processed_adj_perds
			set @counter = 1

			while @counter <= @count
			begin
				select @processed_prem_adj_perd_id = prem_adj_perd_id,
					   @processed_related_customer_id = related_customer_id
				from #processed_adj_perds 
				where id = @counter
				if @debug = 1
				begin
				print '----processed_prem_adj_perd_id table for update of adj no. -------'
				print '@processed_prem_adj_perd_id:' + convert(varchar(20),@processed_prem_adj_perd_id)
				print '@processed_related_customer_id:' + convert(varchar(20),@processed_related_customer_id)
				end

				exec [dbo].[ModAISCalcProcessAdjNum] 
				@premium_adj_period_id = @processed_prem_adj_perd_id,
				@customer_id = @processed_related_customer_id

				set @counter = @counter + 1
			end
			
	--		if(@err_msg_output is not null)
	--			set @err_msg_output = 'Error encountered during execution of calculation process. Please review the error log and take corrective action as appropriate.'

			drop table #num
			drop table #adj_perds
			drop table #processed_adj_perds
			drop table #deleted_adj_perds
			
			
			/****************************************************************************
			* Updating Open Adjustments with Error
			*****************************************************************************/
			/****************************************************************************
			* Sequence generator that drives parsing of comma-separated open_adj_id's
			*****************************************************************************/
			
			set @len_openAdjustments = len(@open_adjustments_after_validation)
			
			;with Numbers ( i) AS (
			select 1 union all
			select 1 + i from Numbers where i < @len_openAdjustments+1) 
			select i  into #number from Numbers
			option ( maxrecursion 2000 )

			declare driver_open_adj_err_cur cursor LOCAL FAST_FORWARD READ_ONLY 
			for 
			select 
					substring( ',' + @open_adjustments_after_validation + ',', i + 1, 
					charindex( ',', ',' + @open_adjustments_after_validation + ',', i + 1 ) - i - 1 ) as "OpenAdjustments"
					from #number
					where substring( ',' + @open_adjustments_after_validation + ',', i, 1 ) = ','
					and i < len( ',' + @open_adjustments_after_validation + ',' ) 
			
			open driver_open_adj_err_cur
			fetch driver_open_adj_err_cur into @open_adj_id_err

			while @@Fetch_Status = 0
				begin
					begin

					if exists(select 1 from dbo.PREM_ADJ where prem_adj_id = @open_adj_id_err)
								begin
									update dbo.PREM_ADJ with(rowlock)
									set calc_adj_sts_cd = 'ERR',
										updt_user_id = @create_user_id,
										updt_dt = getdate()
									where prem_adj_id = @open_adj_id_err
								end

				end
						
								fetch driver_open_adj_err_cur into @open_adj_id_err

							end --end of cursor driver_open_adj_err_cur / while loop
						close driver_open_adj_err_cur
						deallocate driver_open_adj_err_cur
			
			/****************************************************************************
			* End of Updating Open Adjustments with Error
			*****************************************************************************/

		set @tryProc = 0

		if @trancount = 0
			commit transaction 

	end try
	begin catch

		if (error_number()=1205)
			set @tryProc = @tryProc - 1
		else
			set @tryProc = -1

		if @trancount = 0 or xact_state() <> 0  
		begin
			rollback transaction
		end

		if exists(select 1 from dbo.PREM_ADJ where prem_adj_id = @prem_adj_id)
		begin
			update dbo.PREM_ADJ with(rowlock)
			set calc_adj_sts_cd = 'ERR',
				updt_user_id = @create_user_id,
				updt_dt = getdate()
			where prem_adj_id = @prem_adj_id
		end

		declare @err_ln varchar(10),
				@err_proc varchar(30),@err_no varchar(10),
				@start_date varchar(30),
				@end_date varchar(30)

		select  @err_msg = error_message(),
				@err_no = error_number(),
				@err_proc = error_procedure(),
				@err_ln = error_line(),
				@err_msg_output=error_message()
		set @err_msg = '- error no.:' + isnull(@err_no,'') + '; procedure:' 
			+ isnull(@err_proc,'') + ';error line:' + isnull(@err_ln,'') + ';description:' + isnull(@err_msg,'') 

		select 
		@start_date = isnull(convert(varchar(30), strt_dt,101),''),
		@end_date = isnull(convert(varchar(30),plan_end_dt,101 ),'')
		from dbo.PREM_ADJ_PGM
		where prem_adj_pgm_id = @prem_adj_pgm_id

		set @start_date = isnull(@start_date,' ')
		set @end_date = isnull(@end_date,' ')

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
				+ convert(varchar(20),isnull(@prem_adj_id,0)) 
				+ ' for program period: ' 
				+ convert(varchar(20),isnull(@prem_adj_pgm_id,0)) + ' (' + @start_date + '-' +  @end_date + ')' 
				+ ' associated with customer number: ' 
				+ convert(varchar(20),isnull(@customer_id,0))
				+ '. Error message: '
				+ isnull(@err_msg,'')
		   ,@customer_id
		   ,@prem_adj_id
		   ,isnull(@create_user_id,0)
		)

		select 
		error_number() AS ErrorNumber,
		error_severity() AS ErrorSeverity,
		error_state() as ErrorState,
		error_procedure() as ErrorProcedure,
		error_line() as ErrorLine,
		error_message() as ErrorMessage

		if(@err_msg_output is not null)
			set @err_msg_output = 'Error encountered during execution of calculation process. Please review the error log and take corrective action as appropriate.'


	end catch


	end
--end		--while loop

go

if object_id('ModAISCalcDriver') is not null
	print 'Created Procedure ModAISCalcDriver'
else
	print 'Failed Creating Procedure ModAISCalcDriver'
go

if object_id('ModAISCalcDriver') is not null
	grant exec on ModAISCalcDriver to public
go



