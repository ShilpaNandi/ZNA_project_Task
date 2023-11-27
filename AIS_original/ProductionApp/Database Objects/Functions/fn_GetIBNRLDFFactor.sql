
if exists (select 1 from sysobjects 
		where name = 'fn_GetIBNRLDFFactor' and type = 'FN')
	drop function fn_GetIBNRLDFFactor
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetIBNRLDFFactor
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Dan Gojmerac
-----
-----	Description:	This function returns the IBNR or LDF factor.
----                    It assumes that callling module has determined that
----                    LDF/IBNR Incl Limit: indicator is checked and knows
----					which factor will be returned (stepped IBNR or stepped LDF)
-----					This stored procedure then determines if the stepped factor
-----					for the policy is checked
-----					If stepped factor indicator is not checked
-----					then function returns the ibnr or ldf factor from coml_agmt table
-----					If stepped factor indicator is checked then
-----					it identifies the appropriate rate to return.
-----					It subtracts a value of 1 from the calculate factor
-----	Modified:	
-----
---------------------------------------------------------------------
CREATE function [dbo].[fn_GetIBNRLDFFactor]
   (
	@custmr_id int, 
	@prem_adj_pgm_id int ,
	@coml_agmt_id int  
	)
returns decimal(15,8)
as
begin
declare @months_to_val int,
		@factor_rt decimal(15,8),
		@los_dev_fctr_rt decimal(15,8),
		@incur_but_not_rptd_fctr_rt decimal(15,8),
		@los_dev_fctr_incur_but_not_rptd_step_ind decimal(15,8),
		@prem_non_prem_cd char(2)

			select	@los_dev_fctr_rt = isnull(los_dev_fctr_rt, 0),
					@incur_but_not_rptd_fctr_rt = isnull(incur_but_not_rptd_fctr_rt,0),
					@los_dev_fctr_incur_but_not_rptd_step_ind = isnull(los_dev_fctr_incur_but_not_rptd_step_ind,0)
			from	COML_AGMT
			where	coml_agmt_id = @coml_agmt_id
			
			---- Check if stepped factor indicator is checked
			---- If it is not checked, then simply return the IBNR or LDF factor from coml_agmt table
			if (@los_dev_fctr_incur_but_not_rptd_step_ind = 0)
			begin
				if (@incur_but_not_rptd_fctr_rt > 0)
				begin
					set @factor_rt = (@incur_but_not_rptd_fctr_rt - 1)
				end
				else
				begin
					set @factor_rt = (@los_dev_fctr_rt - 1)
				end
			end
			else
			begin 
					-- Identify the stepped factor to return
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
								select @months_to_val =case when day(strt_dt)>15 then datediff(mm,strt_dt, nxt_valn_dt) else datediff(mm,strt_dt, nxt_valn_dt)+1 end
								from dbo.PREM_ADJ_PGM where prem_adj_pgm_id = @prem_adj_pgm_id
								end
								else
								begin
								select @months_to_val =case when day(strt_dt)>15 then datediff(mm,strt_dt, nxt_valn_dt_non_prem_dt) else datediff(mm,strt_dt, nxt_valn_dt_non_prem_dt)+1 end
								from dbo.PREM_ADJ_PGM where prem_adj_pgm_id = @prem_adj_pgm_id
								end
					
			
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
							and mms_to_valn_nbr = @months_to_val
							and actv_ind = 1
						)

					if (@incur_but_not_rptd_fctr_rt > 0)
						begin
							set @factor_rt = (@incur_but_not_rptd_fctr_rt - 1)
						end
						else
						begin
							set @factor_rt = (@los_dev_fctr_rt - 1)
						end
			end
	
		return @factor_rt
end

go

if object_id('fn_GetIBNRLDFFactor') is not null
	print 'Created function fn_GetIBNRLDFFactor'
else
	print 'Failed Creating Function fn_GetIBNRLDFFactor'
go

if object_id('fn_GetIBNRLDFFactor') is not null
	grant exec on fn_GetIBNRLDFFactor to public
go
