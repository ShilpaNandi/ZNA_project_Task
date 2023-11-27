
if exists (select 1 from sysobjects 
		where name = 'ModAISDeleteReCalcData' and type = 'P')
	drop procedure ModAISDeleteReCalcData
go

set ansi_nulls off
go
---------------------------------------------------------------------
-----
-----	Proc Name:	ModAISDeleteReCalcData
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	This stored procedure is used for delete records from tables storing calculation results
-----					While doing the Re-Calculation.
-----
-----	On Exit:	
-----			
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------

create procedure [dbo].[ModAISDeleteReCalcData] 
@customer_id_Delete int,
@UnChecked_recalc_prem_adj_perds varchar(200),
@debug bit = 0
as

begin
	set nocount on

declare	@trancount int,
		@next_val_dt datetime,
		@conversion_dt datetime,
		@len_recalc_Unchecked int,
		@prem_adj_perd_id_Delete int,
		@prem_adj_id_Delete int,
		@prem_adj_pgm_id_Delete int
		


set @trancount = @@trancount

if @trancount = 0 
	begin
	    begin transaction 
	end


begin try

	/******************************************************************
		* Sequence generator that drives parsing of comma-separated strings
		******************************************************************/
		set @len_recalc_Unchecked = len(@UnChecked_recalc_prem_adj_perds)

		;with Nums ( n ) AS (
        select 1 union all
        select 1 + n from Nums where n < @len_recalc_Unchecked+1) 
		select n  into #num from Nums
		option ( maxrecursion 2000 )

		declare driver_prem_adj_perd_delete_cur cursor LOCAL FAST_FORWARD READ_ONLY 
		for 
		select 
				substring( ',' + @UnChecked_recalc_prem_adj_perds + ',', n + 1, 
				charindex( ',', ',' + @UnChecked_recalc_prem_adj_perds + ',', n + 1 ) - n - 1 ) as "DeletePremAdjPerds"
				from #num
				where substring( ',' + @UnChecked_recalc_prem_adj_perds + ',', n, 1 ) = ','
				and n < len( ',' + @UnChecked_recalc_prem_adj_perds + ',' ) 
		
		open driver_prem_adj_perd_delete_cur
		fetch driver_prem_adj_perd_delete_cur into @prem_adj_perd_id_Delete

		while @@Fetch_Status = 0
			begin
				begin
				
				if @debug = 1
				begin
				print '@prem_adj_perd_id_delete:' + convert(varchar(20),@prem_adj_perd_id_Delete)
				print '-----------'
				end
				--Retrieve @prem_adj_id_Delete
				select @prem_adj_id_Delete=prem_adj_id from prem_adj_perd
				where --custmr_id = @customer_id 
				--and 
				prem_adj_perd_id = @prem_adj_perd_id_Delete

				--Retrieve @prem_adj_pgm_id_Delete
				select @prem_adj_pgm_id_Delete=prem_adj_pgm_id from prem_adj_perd
				where --custmr_id = @customer_id and 
				prem_adj_perd_id = @prem_adj_perd_id_Delete

				delete from PREM_ADJ_PARMET_DTL WITH (ROWLOCK)
				where --custmr_id = @customer_id
				--and prem_adj_id = @premium_adjustment_id
				--and 
				prem_adj_perd_id = @prem_adj_perd_id_Delete

				delete from dbo.PREM_ADJ_PARMET_SETUP WITH (ROWLOCK)
				where --custmr_id = @customer_id
				--and prem_adj_id = @premium_adjustment_id
				--and 
				prem_adj_perd_id = @prem_adj_perd_id_Delete


				delete from dbo.PREM_ADJ_COMB_ELEMTS WITH (ROWLOCK)
				where --custmr_id = @customer_id
				--and prem_adj_id = @premium_adjustment_id
				--and 
				prem_adj_perd_id = @prem_adj_perd_id_Delete

				delete from dbo.PREM_ADJ_LOS_REIM_FUND_POST WITH (ROWLOCK)
				where --custmr_id = @customer_id
				--and prem_adj_id = @premium_adjustment_id
				--and 
				prem_adj_perd_id = @prem_adj_perd_id_Delete
	
	
				delete from dbo.PREM_ADJ_PAID_LOS_BIL WITH (ROWLOCK)
				where --custmr_id = @customer_id
				--and prem_adj_id = @premium_adjustment_id
				--and 
				prem_adj_perd_id = @prem_adj_perd_id_Delete
	
				delete from dbo.PREM_ADJ_MISC_INVC WITH (ROWLOCK)
				where --custmr_id = @customer_id
				--and prem_adj_id = @premium_adjustment_id
				--and 
				prem_adj_perd_id = @prem_adj_perd_id_Delete
--
				delete from dbo.PREM_ADJ_CMMNT WITH (ROWLOCK)
				where --custmr_id = @customer_id
				--and prem_adj_id = @premium_adjustment_id
				--and 
				prem_adj_perd_id = @prem_adj_perd_id_Delete

				delete from dbo.PREM_ADJ_NY_SCND_INJR_FUND WITH (ROWLOCK)
				where --custmr_id = @customer_id
				--and prem_adj_id = @premium_adjustment_id
				--and 
				prem_adj_perd_id = @prem_adj_perd_id_Delete

				delete from dbo.ARIES_TRNSMTL_HIST WITH (ROWLOCK)
				where --custmr_id = @customer_id
				--and 
				prem_adj_perd_id = @prem_adj_perd_id_Delete

				
				delete from dbo.PREM_ADJ_PERD_TOT WITH (ROWLOCK)
				where --custmr_id = @customer_id
				--and prem_adj_id = @premium_adjustment_id
				--and 
				prem_adj_perd_id = @prem_adj_perd_id_Delete

				delete from dbo.PREM_ADJ_RETRO_DTL WITH (ROWLOCK)
				where --custmr_id = @customer_id
				--and prem_adj_id = @premium_adjustment_id
				--and 
				prem_adj_perd_id = @prem_adj_perd_id_Delete

				delete from dbo.PREM_ADJ_RETRO WITH (ROWLOCK)
				where --custmr_id = @customer_id
				--and prem_adj_id = @premium_adjustment_id
				--and 
				prem_adj_perd_id = @prem_adj_perd_id_Delete

				delete from dbo.PREM_ADJ_PERD WITH (ROWLOCK)
				where --custmr_id = @customer_id
				--and prem_adj_id = @premium_adjustment_id
				--and 
				prem_adj_perd_id = @prem_adj_perd_id_Delete
				
					/****************************************************************************
					* Updating ARMIS_LOS_POL table prem_adj_id field with null
					*****************************************************************************/	
					if @debug = 1
					begin
					print 'Updating ARMIS_LOS_POL table prem_adj_id field with null'
					print '@prem_adj_pgm_id_delete:' + convert(varchar(20),@prem_adj_id_Delete)
					print '@prem_adj_id_delete:' + convert(varchar(20),@prem_adj_pgm_id_Delete)
					print '-----------'
					end
					--Updating ARMIS_LOS_POL table prem_adj_id field with null
					update dbo.ARMIS_LOS_POL  WITH (ROWLOCK)
					set prem_adj_id = NULL
					from dbo.ARMIS_LOS_POL
					where prem_adj_id=@prem_adj_id_Delete
					and prem_adj_pgm_id=@prem_adj_pgm_id_Delete
				
					/****************************************************************************
					* Converting from incurred to paid when Val date matches with conversion date
					*****************************************************************************/	
						select 
						@next_val_dt = nxt_valn_dt,
						@conversion_dt = 
							case when datepart(Day,strt_dt) >15
							then
							DATEADD(D,-1,DATEADD(mm, DATEDIFF(m,0,dateadd(month,incur_conv_mms_cnt, strt_dt))+1,0))  
							else
							DATEADD(D,-1,DATEADD(mm, DATEDIFF(m,0,dateadd(month,incur_conv_mms_cnt, strt_dt)),0))  
							end
						from dbo.PREM_ADJ_PGM
						where --custmr_id = @customer_id and
						prem_adj_pgm_id = @prem_adj_pgm_id_Delete
						and actv_ind = 1
						if @debug = 1
						begin
						print '@next_val_dt:' + convert(varchar(20),@next_val_dt)
						print '@conversion_dt:' + convert(varchar(20),@conversion_dt)
						print '-----------'
						end
					--if (datediff(d,@next_val_dt,@conversion_dt) = 0)
				if(convert(varchar,@next_val_dt,101) = convert(varchar,@conversion_dt,101))
				begin
					if @debug = 1
					begin
					print 'val date equal to conv date; perform conversion'
					end
					update dbo.PREM_ADJ_PGM WITH (ROWLOCK)
						set paid_incur_typ_id = 298, -- Lookup type: "PAID/INCURRED (LBA ADJUSTMENT TYPE)"; lookup: "PAID"
						updt_dt = getdate()
					where prem_adj_pgm_id = @prem_adj_pgm_id_Delete
					and paid_incur_typ_id = 297 -- Lookup type: "PAID/INCURRED (LBA ADJUSTMENT TYPE)"; lookup: "INCURRED"

					update dbo.COML_AGMT WITH (ROWLOCK)
						set adj_typ_id = 70, -- Paid Loss DEP
						updt_dt = getdate()
					where prem_adj_pgm_id = @prem_adj_pgm_id_Delete
					and adj_typ_id = 63 -- Incurred DEP
				
					update dbo.COML_AGMT WITH (ROWLOCK)
						set adj_typ_id = 71, -- Paid Loss Retro
						updt_dt = getdate()
					where prem_adj_pgm_id = @prem_adj_pgm_id_Delete
					and adj_typ_id = 65 -- Incurred Loss Retro

					update dbo.COML_AGMT WITH (ROWLOCK)
						set adj_typ_id = 72, -- Paid Loss Underlayer
						updt_dt = getdate()
					where prem_adj_pgm_id = @prem_adj_pgm_id_Delete
					and adj_typ_id = 67 -- Incurred Underlayer

					update dbo.COML_AGMT WITH (ROWLOCK)
						set adj_typ_id =  73, -- Paid Loss WA
						updt_dt = getdate()
					where prem_adj_pgm_id = @prem_adj_pgm_id_Delete
					and adj_typ_id =66 -- Incurred Loss WA

			--		update dbo.COML_AGMT WITH (ROWLOCK)
			--			set adj_typ_id =69 -- Paid Loss Deductible
			--		where prem_adj_pgm_id = @prem_adj_pgm_id_Delete
			--		and adj_typ_id =  64 -- Incurred Loss Deductible

				end

				--clear Premium Adjustment and Premium Adjustment Status table after clearing results output table
				if not exists(select * from dbo.PREM_ADJ_PERD where prem_adj_id = @prem_adj_id_Delete )
				begin
					
					
					if @debug = 1
					begin
					print 'prem_adj_id delete'
					print '@prem_adj_id_delete:' + convert(varchar(20),@prem_adj_id_Delete)
					print '-----------'
					end
					
					--Delete Premium Adjustment Status
					delete from dbo.PREM_ADJ_STS WITH (ROWLOCK)
					where --custmr_id = @customer_id
					--and 
					prem_adj_id = @prem_adj_id_Delete
					
									
						
					--Delete Premium Adjustment
					delete from dbo.PREM_ADJ WITH (ROWLOCK)
					where prem_adj_id = @prem_adj_id_Delete
				end

		end
				fetch driver_prem_adj_perd_delete_cur into @prem_adj_perd_id_Delete


			end --end of cursor driver_prem_adj_perd_delete_cur / while loop
		close driver_prem_adj_perd_delete_cur
		deallocate driver_prem_adj_perd_delete_cur


	print '@trancount: ' + convert(varchar(30),@trancount)
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
			+ convert(varchar(20),0) 
			+ ' for adjustment period number: ' 
			+ convert(varchar(20),isnull(@prem_adj_perd_id_Delete,0)) 
			+ ' associated with customer number: ' 
			+ convert(varchar(20),isnull(@customer_id_Delete,0))
			+ '. Error message: '
			+ @err_msg
	   ,@customer_id_Delete
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

if object_id('ModAISDeleteReCalcData') is not null
	print 'Created Procedure ModAISDeleteReCalcData'
else
	print 'Failed Creating Procedure ModAISDeleteReCalcData'
go

if object_id('ModAISDeleteReCalcData') is not null
	grant exec on ModAISDeleteReCalcData to public
go




