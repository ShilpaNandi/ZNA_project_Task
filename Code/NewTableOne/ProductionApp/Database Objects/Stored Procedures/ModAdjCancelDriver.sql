if exists (select 1 from sysobjects 
                where name = 'ModAdjCancelDriver' and type = 'P')
        drop procedure ModAdjCancelDriver
go
 
set ansi_nulls off
go
---------------------------------------------------------------------
-----
-----	Proc Name:	ModAdjCancelDriver
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	cancels adjustment and sends entries to Aries
-----	
-----	On Exit:
-----	
-----
-----	Modified:	02/08/2008  Development Team
-----                   Created SP
-----
---------------------------------------------------------------------
create procedure [dbo].[ModAdjCancelDriver]
     @select         smallint = 0, 
     @prem_adj_id    int,
	  @err_msg_output varchar(1000) output

as
declare @error      int,
	@trancount  int
    

set @trancount = @@trancount  
  
if @trancount = 0 
	begin
	    begin transaction 
	end

begin try

		-- Per triage #67, upon a cancel make a copy of all losses
		-- and for the copied losses set the prem_adj_id to null
		exec ModARMIS_LOS_POLCpy 0, @prem_adj_id, null
	
		declare @err_msg_output2 varchar(1000),
				@rel_prem_adj_id int
				set @rel_prem_adj_id=(select rel_prem_adj_id from prem_adj where prem_adj_id=@prem_adj_id)
				if @rel_prem_adj_id > 0
				-- Cancel of Revision
				begin
					delete from ARIES_TRNSMTL_HIST
					where trnsmtl_sent_ind=0 and Post_cd=2
					and prem_adj_id=@rel_prem_adj_id
				--Generate a Cancel Invoice number and update FinalInvoice Number Field in Prem_Adj Table
					declare @count int,
					@Invoice_nbr varchar(15)
					select @Invoice_nbr= fnl_invc_nbr_txt from PREM_ADJ where prem_adj_id=@rel_prem_Adj_id
					select @count= count(*) from prem_adj where adj_can_ind=1 and
					fnl_invc_nbr_txt like 'RTC%'+substring(@Invoice_nbr,6,15)
									
					update PREM_ADJ WITH (ROWLOCK)
					set fnl_invc_nbr_txt='RTC'+(stuff('00', 2-len(@count)+1,len(@count),@count+1))+substring(@Invoice_nbr,6,15),updt_dt = getdate()
					where prem_adj_id=@prem_adj_id
					
					update PREM_ADJ set adj_rrsn_rsn_id=NULL , adj_rrsn_ind=0,updt_dt = getdate()
					where prem_adj_id=@rel_prem_adj_id

				-- Adjustment applies to Premium and Non-premium then move both dates forward
				-- In rare case, the prem_non_prem_cd is null assume that val dates are identical
				-- if prem_non_prem_cd is null in prem_adj_perd table then then there's an error with the
				-- module which updates this field.
						update PREM_ADJ_PGM WITH (ROWLOCK)
						set prev_valn_dt=nxt_valn_dt,
							nxt_valn_dt =DATEADD(D,-1,DATEADD(mm, DATEDIFF(m,0,dateadd(month,adj_freq_mms_intvrl_cnt, nxt_valn_dt))+1,0)),
							prev_valn_dt_non_prem_dt=nxt_valn_dt_non_prem_dt,
							nxt_valn_dt_non_prem_dt=DATEADD(D,-1,DATEADD(mm, DATEDIFF(m,0,dateadd(month,freq_non_prem_mms_cnt, nxt_valn_dt_non_prem_dt))+1,0)),
							lsi_retrieve_from_dt=nxt_valn_dt,
							prior_prem_adj_id=@rel_prem_adj_id,
							updt_dt = getdate()
						from PREM_ADJ_PGM pag inner join PREM_ADJ_PERD pap on ( pag.prem_adj_pgm_id = pap.prem_adj_pgm_id)
						where pap.prem_adj_id = @prem_adj_id
						and (prem_non_prem_cd = 'B' or  prem_non_prem_cd is null)
						and pag.actv_ind = 1

				-- Adjustment applies to Premium then only move Next Val Premium dates forward.
						update PREM_ADJ_PGM WITH (ROWLOCK)
						set prev_valn_dt=nxt_valn_dt,
							lsi_retrieve_from_dt=nxt_valn_dt,
							nxt_valn_dt =DATEADD(D,-1,DATEADD(mm, DATEDIFF(m,0,dateadd(month,adj_freq_mms_intvrl_cnt, nxt_valn_dt))+1,0)),
							prior_prem_adj_id=@rel_prem_adj_id,
							updt_dt = getdate()
						from PREM_ADJ_PGM pag inner join PREM_ADJ_PERD pap on ( pag.prem_adj_pgm_id = pap.prem_adj_pgm_id)
						where pap.prem_adj_id = @prem_adj_id
						and prem_non_prem_cd = 'P'
						and pag.actv_ind = 1
				-- Adjustment applies to Non-premium then only move Next Val Non-Premium dates forward
						update PREM_ADJ_PGM WITH (ROWLOCK)
						set prev_valn_dt_non_prem_dt=nxt_valn_dt_non_prem_dt,
							lsi_retrieve_from_dt=nxt_valn_dt_non_prem_dt,
							nxt_valn_dt_non_prem_dt=DATEADD(D,-1,DATEADD(mm, DATEDIFF(m,0,dateadd(month,freq_non_prem_mms_cnt, nxt_valn_dt_non_prem_dt))+1,0)) ,
							updt_dt = getdate()
						from PREM_ADJ_PGM pag inner join PREM_ADJ_PERD pap on ( pag.prem_adj_pgm_id = pap.prem_adj_pgm_id)
						where pap.prem_adj_id = @prem_adj_id
						and prem_non_prem_cd = 'NP'
						and pag.actv_ind = 1
				-- Set the adj_ind to true for program periods for which its a first adjustment
						update COML_AGMT_AUDT WITH (ROWLOCK)
						set adj_ind=1,
							updt_dt = getdate()
						from COML_AGMT_AUDT caa inner join PREM_ADJ_PERD pap on ( caa.prem_adj_pgm_id = pap.prem_adj_pgm_id)
						where pap.prem_adj_id = @prem_adj_id and caa.prem_adj_id is not null
				end
				-- Call Aries Transmittal procedure
				exec [dbo].[ModAIS_TransmittalToARiES] 
				@prem_adj_id = @prem_adj_id,
				@rel_prem_adj_id= null,
				@err_msg_output = @err_msg_output2 output,
				@Ind = 4 --Cancel INvoice


	/***********************************************************************
	* Convert from INCURRED to PAID if applicable
	***********************************************************************/
	update PREM_ADJ_PGM WITH (ROWLOCK)
	set paid_incur_typ_id = 298 -- Lookup type: "PAID/INCURRED (LBA ADJUSTMENT TYPE)"; lookup: "PAID"
	from(
			SELECT prem_adj_pgm_id, prem_adj_id	FROM PREM_ADJ_PERD 
			WHERE PREM_ADJ_ID = @prem_adj_id )
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
	set adj_typ_id  = 
	CASE 
		WHEN adj_typ_id = 64 THEN 69 --  Incurred Loss Deductible to Paid Loss Deductible 
		WHEN adj_typ_id = 63 THEN 70 --  Incurred Loss DEP to Paid Loss DEP
		WHEN adj_typ_id = 65 THEN 71 --  Incurred Loss Retro to Paid Loss Retro
		WHEN adj_typ_id = 67 THEN 72 --  Incurred Loss Underlayer to Paid Loss Underlayer
		WHEN adj_typ_id = 66 THEN 73 --  Incurred Loss WA to Paid Loss WA
	END
	from(
			SELECT prem_adj_pgm_id, prem_adj_id	FROM PREM_ADJ_PERD 
			WHERE PREM_ADJ_ID = @prem_adj_id )
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


	

	-----------------------------------
	
	if @trancount = 0  
		commit transaction 

end try
begin catch

	
	if @trancount = 0  
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


go

if object_id('ModAdjCancelDriver') is not null
        print 'Created Procedure ModAdjCancelDriver'
else
        print 'Failed Creating Procedure ModAdjCancelDriver'
go

if object_id('ModAdjCancelDriver') is not null
        grant exec on ModAdjCancelDriver to  public
go
 