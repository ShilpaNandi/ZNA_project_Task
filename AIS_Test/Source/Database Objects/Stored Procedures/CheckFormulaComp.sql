
if exists (select 1 from sysobjects 
                where name = 'CheckFormulaComp' and type = 'P')
        drop procedure CheckFormulaComp
go
 
set ansi_nulls off
go

---------------------------------------------------------------------
-----
-----	Proc Name:	CheckFormulaComp
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	Checks whether the component is present in the formula or not
-----	
-----	On Exit:
-----	
-----
-----	Modified:	11/01/2008  Development Team
-----                   Created SP
-----
---------------------------------------------------------------------
create procedure CheckFormulaComp
      @Comp varchar(25),
      @prem_adj_pgm_id int,
	  @status bit output

as
 declare @comp_list varchar(750),
		@frmla_comp varchar(30),
		@identifier_pos smallint,
		@counter smallint,
		@decl_str varchar(1500),
		@init_str varchar(2000),
		@frmla nvarchar(1000),
		@frmla_id int,
		@trancount  int,
		@ent_tistmp datetime

select    @trancount  = @@trancount,
          @ent_tistmp = getdate( )

if @trancount = 0 
	begin
	    begin transaction 
	end


begin try

	select
	@frmla_id = fml.mstr_ernd_retro_prem_frmla_id,
	@frmla = fml.ernd_retro_prem_frmla_two_txt
	from dbo.PREM_ADJ_PGM pgm
	inner join dbo.MSTR_ERND_RETRO_PREM_FRMLA fml on (pgm.mstr_ernd_retro_prem_frmla_id = fml.mstr_ernd_retro_prem_frmla_id)
	where 
	pgm.prem_adj_pgm_id = @prem_adj_pgm_id
	and pgm.actv_ind = 1
	and fml.actv_ind = 1

	set @comp_list = 'PL,PALAE,LR,ALAER,IL,IALAE,Basic,CHF,LBA,'
	set @comp_list = @comp_list + 'PL,PALAE,RML,ELP,'
	set @comp_list =  @comp_list + 'OA,NCF,IBNR,LCF,LDF,TM,PASS'

	set @decl_str = ' '
	set @init_str = ' '

	;with Nums ( n ) AS (
    select 1 union all
    select 1 + n from Nums where n < (case when len(@frmla) > len(@comp_list) then len(@frmla) else len(@comp_list) end ) + 1 ) 
	select n  into #num from Nums
	option ( maxrecursion 2000 )

	set @counter = 1
	create table #frmla_parser
	(
	id int identity(1,1),
	frmla_comp varchar(30),
	identifier_pos smallint
	)

	create index ind ON #frmla_parser (id)

	insert into #frmla_parser(frmla_comp,identifier_pos)
	select 
	substring( ',' + @comp_list + ',', n + 1,charindex( ',', ',' + @comp_list + ',', n + 1 ) - n - 1 ) as "Comp",
	charindex( substring( ',' + @comp_list + ',', n + 1, charindex( ',', ',' + @comp_list + ',', n + 1 ) - n - 1 ), @frmla)
    from #num
    where substring( ',' + @comp_list + ',', n, 1 ) = ','
    and n < len( ',' + @comp_list + ',' )

	select @status = case when identifier_pos > 0 then 1 else 0 end from #frmla_parser where frmla_comp = @Comp

--     print @frmla_id
--	 print @frmla

    drop table #frmla_parser
	drop table #num

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
			@err_sev = error_severity()

	RAISERROR (@err_msg, -- Message text.
               @err_sev, -- Severity.
               1 -- State.
               )
end catch

go

if object_id('CheckFormulaComp') is not null
        print 'Created Procedure CheckFormulaComp'
else
        print 'Failed Creating Procedure CheckFormulaComp'
go

if object_id('CheckFormulaComp') is not null
        grant exec on CheckFormulaComp to  public
go
 

