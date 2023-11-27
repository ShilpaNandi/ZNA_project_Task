
if exists (select 1 from sysobjects 
		where name = 'fn_FindFormulaSectionERP_Multi_Comp' and type = 'FN')
	drop function fn_FindFormulaSectionERP_Multi_Comp
GO

set ansi_nulls off
GO
set quoted_identifier on
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_FindFormulaSectionERP_Multi_Comp
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Prabal Dhar 
-----
-----	Description:	This function  determines the formula section as applicable for an identifier. May be
-----					used to process formulas with multiple occurrences of a component.	
-----
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification
/*
-- Sample usage
declare @sub_formula varchar(1000)

exec @sub_formula = dbo.[fn_FindFormulaSectionERP_Multi_Comp]
	 @txtMainFormula = '((IL    +      IALAE)*LCF + Basic)*TM + (IBNR + LBA*LCF) - ((LR*LDF)*PA + LBA)*TM - ((ALAER + PL)*LCF + IBNR)*TM + (CHF + LBA) * TM',
	 @identifier = 'TM'
print 'retrieved multiple comp @sub_formula mu:- ' + @sub_formula 


Output :- ((IL+IALAE)*LCF+Basic)-((LR*LDF)*PA+LBA)-((ALAER+PL)*LCF+IBNR)+(CHF+LBA)


*/
---------------------------------------------------------------------
create function [dbo].[fn_FindFormulaSectionERP_Multi_Comp]
  (
		@txtMainFormula varchar(1000),
		@identifier varchar(30)
	)
returns varchar(500)
as
begin

    
declare @sub_formula varchar(500), 
		@sub_formula_mod varchar(500),
		@original_formula varchar(5000),
		@final_str varchar(5000),
		@ident varchar(100),
		--@counter smallint,
		--@count smallint,
		@count_str int,
		@pos int,
		@sign_pos int

declare @sub_formulas table ( sub_formula_id int identity(1,1), sign_str varchar(5), sub_formula varchar(1000)  )

set @final_str = ''

--set @original_formula = '(((IL + IALAE) * LDF + LBA + Basic +  OA) * TM)   - ((((IL+IALAE) * LDF) * TM-((IL+IALAE) * LDF)) - ((IL+IALAE)*TM-(IL+IALAE)))'
--set @ident = 'TM'

set @original_formula = @txtMainFormula
set @ident = @identifier

--set @original_formula = '((IL    +      IALAE)*LCF + Basic)*TM'
--set @ident = 'TM'

--set @original_formula = '((IL    +      IALAE)*LCF + Basic)*TM - (LDF)*LCF'
--set @ident = 'LCF'

set @original_formula = replace(@original_formula, ' ','')

exec @count_str = [dbo].[fn_CountOfStringInstances]
			@pInput = @original_formula,
			@pSearchString = @ident
--print @count_str


--print @original_formula
while @count_str > 0
begin
	exec @sub_formula = dbo.[fn_ExtractSubFormula]
		 @txtMainFormula = @original_formula,
		 @identifier = @ident

	set @sub_formula_mod = '(' + @sub_formula + ')'
	--print 'in while loop retrieved @sub_formula:- ' + @sub_formula_mod

	set @pos = charindex(@ident, @original_formula)
	--print 'in while loop @pos: ' + convert(varchar(20),@pos)

	set @sign_pos = charindex(@sub_formula, @original_formula)
	--print 'in while loop @sign_pos: ' + convert(varchar(20),@sign_pos)
	--print 'sign portion: ' + substring(@original_formula, @sign_pos -2, 1)

	insert into @sub_formulas(sign_str,sub_formula)
	values(substring(@original_formula, @sign_pos -2, 1),@sub_formula_mod)

	set @original_formula = right(@original_formula, len(@original_formula) - @pos - len(@ident) + 1)
	--print 'Formula string to be processing in next loop: ' + @original_formula

	exec @count_str = [dbo].[fn_CountOfStringInstances]
				@pInput = @original_formula,
				@pSearchString = @ident

	--print 'in while loop @count_str: ' + convert(varchar(20),@count_str)

end


--select * from @sub_formulas

select 
@final_str = @final_str + sign_str,
@final_str = @final_str + sub_formula
from @sub_formulas 
--where sub_formula_id = @counter

return @final_str

end
GO

if object_id('fn_FindFormulaSectionERP_Multi_Comp') is not null
	print 'Created function fn_FindFormulaSectionERP_Multi_Comp'
else
	print 'Failed Creating function fn_FindFormulaSectionERP_Multi_Comp'
go

if object_id('fn_FindFormulaSectionERP_Multi_Comp') is not null
	grant exec on fn_FindFormulaSectionERP_Multi_Comp to public
go


