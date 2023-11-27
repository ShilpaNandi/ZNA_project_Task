
if exists (select 1 from sysobjects 
		where name = 'fn_GetLOB' and type = 'FN')
	drop function fn_GetLOB
GO

set ansi_nulls off
GO
set quoted_identifier on
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetLOB
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Prabal Dhar 
-----
-----	Description: This function determines line of business.
-----
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------
create function [dbo].[fn_GetLOB]
(
@com_agm_id int
--@premium_adj_prog_id int,
--@customer_id int
)
returns int
as
begin

	declare @lob varchar(10),
			@lob_id int

	select @lob = attr_1_txt 
	from dbo.LKUP 
	where lkup_id = (
						select 
						covg_typ_id 
						from dbo.COML_AGMT 
						where coml_agmt_id = @com_agm_id
--						and prem_adj_pgm_id = @premium_adj_prog_id
--						and custmr_id = @customer_id
					)
	and lkup_typ_id in (
							select 
							lkup_typ_id 
							from dbo.LKUP_TYP 
							where lkup_typ_nm_txt like 'LOB COVERAGE'
						)
	set @lob_id = dbo.fn_GetIDForLOB(@lob)
	return @lob_id
end



go

if object_id('fn_GetLOB') is not null
	print 'Created function fn_GetLOB'
else
	print 'Failed Creating function fn_GetLOB'
go

if object_id('fn_GetLOB') is not null
	grant exec on fn_GetLOB to public
go

