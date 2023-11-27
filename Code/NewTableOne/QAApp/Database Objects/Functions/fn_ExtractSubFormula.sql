
if exists (select 1 from sysobjects 
		where name = 'fn_ExtractSubFormula' and type = 'FN')
	drop function fn_ExtractSubFormula
GO

set ansi_nulls off
GO
set quoted_identifier on
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_ExtractSubFormula
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Prabal Dhar 
-----
-----	Description:	This function  determines the formula section as applicable for an identifier. Replica of original function
-----					
-----
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------
create function [dbo].[fn_ExtractSubFormula]
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

--set @txtMainFormula = replace(@txtMainFormula, ' ','')

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
/*
--Multiple component processing

declare @sub_formula varchar(500), 
		@sub_formula_mod varchar(500),
		@original_formula varchar(5000),
		@ident varchar(100),
		@count_str int,
		@pos int,
		@sign_pos int

declare @sub_formulas table ( sub_formula_id int identity(1,1), sign_str varchar(5), sub_formula varchar(1000)  )


set @original_formula = '((IL    +      IALAE)*LCF + Basic)*TM - ((LR*LDF)*PA + LBA)*TM - ((ALAER + PL)*LCF + IBNR)*TM + (CHF + LBA) * TM'
set @ident = 'TM'

--set @original_formula = '((IL    +      IALAE)*LCF + Basic)*TM - (LDF)*LCF'
--set @ident = 'LCF'

set @original_formula = replace(@original_formula, ' ','')

exec @count_str = [dbo].[fn_CountOfStringInstances]
			@pInput = @original_formula,
			@pSearchString = @ident
print @count_str


print @original_formula
while @count_str > 0
begin
	exec @sub_formula = dbo.[fn_ExtractSubFormula]
		 @txtMainFormula = @original_formula,
		 @identifier = @ident

	set @sub_formula_mod = '(' + @sub_formula + ')'
	print 'in while loop retrieved @sub_formula:- ' + @sub_formula_mod

	set @pos = charindex(@ident, @original_formula)
	print 'in while loop @pos: ' + convert(varchar(20),@pos)

	set @sign_pos = charindex(@sub_formula, @original_formula)
	print 'in while loop @sign_pos: ' + convert(varchar(20),@sign_pos)
	print 'sign portion: ' + substring(@original_formula, @sign_pos -2, 1)

	insert into @sub_formulas(sign_str,sub_formula)
	values(substring(@original_formula, @sign_pos -2, 1),@sub_formula_mod)

	set @original_formula = right(@original_formula, len(@original_formula) - @pos - len(@ident) + 1)
	print 'Formula string to be processing in next loop: ' + @original_formula

	exec @count_str = [dbo].[fn_CountOfStringInstances]
				@pInput = @original_formula,
				@pSearchString = @ident

	print 'in while loop @count_str: ' + convert(varchar(20),@count_str)

end

select * from @sub_formulas


*/

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

if object_id('fn_ExtractSubFormula') is not null
	print 'Created function fn_ExtractSubFormula'
else
	print 'Failed Creating function fn_ExtractSubFormula'
go

if object_id('fn_ExtractSubFormula') is not null
	grant exec on fn_ExtractSubFormula to public
go


