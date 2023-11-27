if exists (select 1 from sysobjects 
		where name = 'fn_RetrieveSurcharge_rt' and type = 'FN')
	drop function fn_RetrieveSurcharge_rt
GO
set ansi_nulls off
GO
set quoted_identifier on
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_RetrieveSurcharge_rt
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		CSC (Venkata Kolimi)
-----
-----	Description:    Function to Retrieve the Surcharge Rate based on the given parametrs such as State,LOB,Surcharge Type,policy,Surcharge Code and adjsutment number
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------
CREATE function [dbo].[fn_RetrieveSurcharge_rt]
   (
	@ln_of_bsn_id int,
	@state_id int,
	@surcharge_type_id int,
	@com_agm_id int,
	@surcharge_code_id int,
	@prem_adj_id int
	)
returns decimal(20, 8)
as
begin
	declare @surcharge_rt decimal(20,8),
			@surchrg_fctr_id int,
			@surchrg_fctr_lkup_id_R int,
			@surchrg_fctr_lkup_id_D int
					

			
					select @surchrg_fctr_lkup_id_R=lkup_id from lkup
					inner join lkup_typ on lkup.lkup_typ_id=lkup_typ.lkup_typ_id
					where lkup.lkup_txt='R'
					and lkup_typ_nm_txt='SURCHARGE DATE INDICATOR'

					select @surchrg_fctr_lkup_id_D=lkup_id from lkup
					inner join lkup_typ on lkup.lkup_typ_id=lkup_typ.lkup_typ_id
					where lkup.lkup_txt='D'
					and lkup_typ_nm_txt='SURCHARGE DATE INDICATOR'

					set @surcharge_rt=-1
					
					select @surcharge_rt=surchrg_rt,
					@surchrg_fctr_id=surchrg_fctr_id 
					from SURCHRG_ASSES_SETUP
					where ln_of_bsn_id=@ln_of_bsn_id
					and st_id=@state_id 
					and surchrg_typ_id=@surcharge_type_id
					and surchrg_eff_dt=(select max(surchrg_eff_dt)	from dbo.SURCHRG_ASSES_SETUP where surchrg_eff_dt <=
					(
					select
					pol_eff_dt
					from
					dbo.COML_AGMT
					where coml_agmt_id = @com_agm_id
					)
					and ln_of_bsn_id=@ln_of_bsn_id
					and st_id=@state_id 
					and surchrg_typ_id=@surcharge_type_id
					and surchrg_cd_id=@surcharge_code_id
					and actv_ind=1
					)		
					and actv_ind=1
					and surchrg_cd_id=@surcharge_code_id

					if(@surchrg_fctr_id=@surchrg_fctr_lkup_id_D)
					begin
					set @surcharge_rt=-1
					end 
					else if(@surchrg_fctr_id=@surchrg_fctr_lkup_id_R)
					begin
					set @surcharge_rt=-1
					select @surcharge_rt=surchrg_rt,
					@surchrg_fctr_id=surchrg_fctr_id 
					from SURCHRG_ASSES_SETUP
					where ln_of_bsn_id=@ln_of_bsn_id
					and st_id=@state_id 
					and surchrg_typ_id=@surcharge_type_id
					and surchrg_eff_dt=(select max(surchrg_eff_dt)	from dbo.SURCHRG_ASSES_SETUP where surchrg_eff_dt <=
					(
					select
					valn_dt
					from
					dbo.PREM_ADJ
					where prem_adj_id = @prem_adj_id
					)
					and ln_of_bsn_id=@ln_of_bsn_id
					and st_id=@state_id 
					and surchrg_typ_id=@surcharge_type_id
					and surchrg_cd_id=@surcharge_code_id
					and actv_ind=1
					)		
					and actv_ind=1
					and surchrg_cd_id=@surcharge_code_id
					
				    if(@surcharge_rt=0 AND @surchrg_fctr_id=@surchrg_fctr_lkup_id_D)
					begin
					SET @surcharge_rt=-1 
					end
	
					end
					



	set @surcharge_rt = isnull(@surcharge_rt,-1)

   return @surcharge_rt
end

go

if object_id('fn_RetrieveSurcharge_rt') is not null
	print 'Created function fn_RetrieveSurcharge_rt'
else
	print 'Failed Creating function fn_RetrieveSurcharge_rt'
go

if object_id('fn_RetrieveSurcharge_rt') is not null
	grant exec on fn_RetrieveSurcharge_rt to public
go