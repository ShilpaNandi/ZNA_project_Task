
if exists (select 1 from sysobjects 
		where name = 'fn_GetIBNRStepInd' and type = 'FN')
	drop function fn_GetIBNRStepInd
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetIBNRStepInd
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Dan Gojmerac
-----
-----	Description:	This function determines if IBNR stepped factor will be used
-----					in the calculation of a record
-----	Modified:	
-----
---------------------------------------------------------------------
CREATE function [dbo].[fn_GetIBNRStepInd]
   (
	@custmr_id int, 
	@prem_adj_pgm_id int ,
	@coml_agmt_id int  
	)
returns bit
as
begin
declare @months_to_val int,
		@step_ind bit,
		@los_dev_fctr_rt decimal(15,8),
		@incur_but_not_rptd_fctr_rt decimal(15,8),
		@los_dev_fctr_incur_but_not_rptd_step_ind decimal(15,8),
		@prem_non_prem_cd char(2)

		select @step_ind = 0

			select @prem_non_prem_cd=pap.prem_non_prem_cd from prem_adj pa
								inner join prem_adj_perd pap on pap.prem_adj_id=pa.prem_adj_id
								inner join prem_adj_pgm pg on pg.prem_adj_pgm_id=pap.prem_adj_pgm_id
								where (pa.valn_dt=pg.nxt_valn_dt or pa.valn_dt=pg.nxt_valn_dt_non_prem_dt) 
								and pa.adj_can_ind<>1 
								and pa.adj_void_ind<>1
								and pa.adj_rrsn_ind<>1
								and substring(isnull(pa.fnl_invc_nbr_txt,''),1,3)<>'RTV'
								and pa.adj_sts_typ_id=346
								and pg.prem_adj_pgm_id=@prem_adj_pgm_id
				
			if(@prem_non_prem_cd='P' or @prem_non_prem_cd='B')
			begin
			select	@los_dev_fctr_incur_but_not_rptd_step_ind = isnull(los_dev_fctr_incur_but_not_rptd_step_ind,0),
					@months_to_val = case when day(strt_dt)>15 then datediff(mm,strt_dt, nxt_valn_dt) else datediff(mm,strt_dt, nxt_valn_dt)+1 end
			from	COML_AGMT ca, PREM_ADJ_PGM pap
			where	ca.coml_agmt_id = @coml_agmt_id
			and     pap.prem_adj_pgm_id = @prem_adj_pgm_id
			and		ca.prem_adj_pgm_id = pap.prem_adj_pgm_id
			end
			else
			begin
			select	@los_dev_fctr_incur_but_not_rptd_step_ind = isnull(los_dev_fctr_incur_but_not_rptd_step_ind,0),
					@months_to_val = case when day(strt_dt)>15 then datediff(mm,strt_dt, nxt_valn_dt_non_prem_dt) else datediff(mm,strt_dt, nxt_valn_dt_non_prem_dt)+1 end
			from	COML_AGMT ca, PREM_ADJ_PGM pap
			where	ca.coml_agmt_id = @coml_agmt_id
			and     pap.prem_adj_pgm_id = @prem_adj_pgm_id
			and		ca.prem_adj_pgm_id = pap.prem_adj_pgm_id

			end
			
			---- Check if IBNR stepped factor will be used for this record
			if (@los_dev_fctr_incur_but_not_rptd_step_ind = 1)
				begin 
			
					select	@los_dev_fctr_rt = isnull(los_dev_fctr_rt, 0),
							@incur_but_not_rptd_fctr_rt = isnull(incur_but_not_rptd_fctr_rt,0)
					from dbo.STEPPED_FCTR 
					where coml_agmt_id = @coml_agmt_id 
					and prem_adj_pgm_id = @prem_adj_pgm_id 
					and custmr_id = @custmr_id 
					and actv_ind = 1
					and mms_to_valn_nbr 
					=  (
							select max( mms_to_valn_nbr ) 
							from  dbo.STEPPED_FCTR 
							where coml_agmt_id = @coml_agmt_id 
							and prem_adj_pgm_id = @prem_adj_pgm_id 
							and custmr_id = @custmr_id 
							and mms_to_valn_nbr <= @months_to_val
							and actv_ind = 1
						)

					if (@incur_but_not_rptd_fctr_rt > 0)
						begin
							set @step_ind = 1
						end
			end
	
		return @step_ind
end

go

if object_id('fn_GetIBNRStepInd') is not null
	print 'Created function fn_GetIBNRStepInd'
else
	print 'Failed Creating Function fn_GetIBNRStepInd'
go

if object_id('fn_GetIBNRStepInd') is not null
	grant exec on fn_GetIBNRStepInd to public
go
