
if exists (select 1 from sysobjects 
		where name = 'fn_FindFormulaSectionERP' and type = 'FN')
	drop function fn_FindFormulaSectionERP
GO

set ansi_nulls off
GO
set quoted_identifier on
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_FindFormulaSectionERP
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Prabal Dhar 
-----
-----	Description:	This function  determines the formula section as applicable for an identifier.
-----
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------
create function [dbo].[fn_FindFormulaSectionERP]
   (
		@txtMainFormula varchar(1000),
		@identifier varchar(30)
	)
returns varchar(500)
as
begin
	declare @len int,
			@pos int,
			@st_pos int,
			@end_pos int,
			@clos_paran_cnt int,
			@st_paran_cnt int,
			@paran_matcher int,
			@chr char(1),
			@sub_formula varchar(1000)

set @clos_paran_cnt = 0
set @st_paran_cnt = 0
set @paran_matcher = -1

set @pos = charindex(@identifier, @txtMainFormula)
while @pos > 0
begin
	set @chr = substring(@txtMainFormula,@pos - 1,1)

	if @chr = ')'
	begin
		set @clos_paran_cnt = @clos_paran_cnt + 1

		if @clos_paran_cnt = 1
			set @end_pos = @pos

		set @paran_matcher = @paran_matcher + 1
	end

	if @chr = '('
	begin
		set @paran_matcher = @paran_matcher - 1

		if @paran_matcher = -1
		begin
			set @st_paran_cnt = @st_paran_cnt + 1
				
			if @st_paran_cnt = 1
				set @st_pos = @pos
		end
	end


	set @pos = @pos - 1
end

	set @sub_formula = substring(@txtMainFormula, @st_pos, (@end_pos - @st_pos) - 1  )
--	select @sub_formula = replace(@sub_formula, '@lcf','(@lcf - 1)')
	select @sub_formula = replace(@sub_formula, '@tm','(@tm - 1)')
--	select @sub_formula = replace(@sub_formula, '@ibnr','(@ibnr - 1)')
--	select @sub_formula = replace(@sub_formula, '@ldf','(@ldf - 1)')

    return @sub_formula --substring(@txtMainFormula, @st_pos, (@end_pos - @st_pos) - 1  )
end


go

if object_id('fn_FindFormulaSectionERP') is not null
	print 'Created function fn_FindFormulaSectionERP'
else
	print 'Failed Creating function fn_FindFormulaSectionERP'
go

if object_id('fn_FindFormulaSectionERP') is not null
	grant exec on fn_FindFormulaSectionERP to public
go


