
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
		@los_dev_fctr_incur_but_not_rptd_step_ind decimal(15,8)

		select @step_ind = 0

			select	@los_dev_fctr_incur_but_not_rptd_step_ind = isnull(los_dev_fctr_incur_but_not_rptd_step_ind,0),
					@months_to_val = datediff(mm,strt_dt, dateadd(d,1,nxt_valn_dt))
			from	COML_AGMT ca, PREM_ADJ_PGM pap
			where	ca.coml_agmt_id = @coml_agmt_id
			and     pap.prem_adj_pgm_id = @prem_adj_pgm_id
			and		ca.prem_adj_pgm_id = pap.prem_adj_pgm_id
			
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
