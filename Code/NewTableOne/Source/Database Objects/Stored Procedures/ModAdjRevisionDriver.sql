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

