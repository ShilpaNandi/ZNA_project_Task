if exists (select 1 from sysobjects 
		where name = 'GetILRFOtherAmounts' and type = 'P')
	drop procedure GetILRFOtherAmounts
go

set ansi_nulls off
go

---------------------------------------------------------------------
-----
-----	Proc Name:	GetILRFOtherAmounts
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	This stored procedure is used to verify for other amounts entered for the ILRF setup
-----                   Based on this stored procedure results we are going to show the vlaidation message to the
-----					Users while calculating the adjustment.(SR 325928)
-----
-----	On Exit:	
-----   
-----   Created  :   Venkat Kolimi			
-----
-----	Modified:	CSC
-----			

---------------------------------------------------------------------

CREATE procedure [dbo].[GetILRFOtherAmounts] 
@check_calc_prog_perds varchar(2000),
@check_recalc_prem_adj_perds varchar(2000)
as

begin
	set nocount on;


declare @len_calc int,
		@len_recalc int,
		@prem_adj_pgm_id int,
		@prem_adj_perd_id int,
		@customer_id int,
		@trancount int,
		@err_message varchar(2000),
		@cnt int


begin try
		
		/****************************************************************************
		* Sequence generator that drives parsing of comma-separated Prem_adj_pgm_id's
		*****************************************************************************/
		set @len_calc = len(@check_calc_prog_perds)
		
		if(@len_calc>0)
		begin
		
		;with Nums ( n ) AS (
        select 1 union all
        select 1 + n from Nums where n < @len_calc+1) 
		select n  into #num from Nums
		option ( maxrecursion 2000 )

		end
		/****************************************************************************
		* Sequence generator that drives parsing of comma-separated Prem_adj_perd_id's
		*****************************************************************************/
		set @len_recalc = len(@check_recalc_prem_adj_perds)

		if(@len_recalc>0)
		begin
		
		;with Numbers ( i ) AS (
        select 1 union all
        select 1 + i from Numbers where i < @len_recalc+1) 
		select i  into #number from Numbers
		option ( maxrecursion 2000 )

		end
		

		if (@len_calc>0 and @len_recalc>0)
		begin
		
		select pdp.prem_adj_pgm_id, 
		CONVERT(NVARCHAR(30), pdp.STRT_DT, 101) + '-' + CONVERT(NVARCHAR(30), pdp.PLAN_END_DT,101) "PROGRAM PERIOD",
		pdps.incur_los_reim_fund_othr_amt
	    from prem_adj_pgm pdp
		inner join prem_adj_pgm_setup pdps on pdp.prem_adj_pgm_id=pdps.prem_adj_pgm_id
		where pdp.prem_adj_pgm_id in
		(
			select substring( ',' + @check_calc_prog_perds + ',', n + 1, 
				charindex( ',', ',' + @check_calc_prog_perds + ',', n + 1 ) - n - 1 ) as ProgPerds
				from #num
				where substring( ',' + @check_calc_prog_perds + ',', n, 1 ) = ','
				and n < len( ',' + @check_calc_prog_perds + ',' ) 
		)
		and pdps.incur_los_reim_fund_othr_amt<>0

		union
		
		select pdp.prem_adj_pgm_id, 
		CONVERT(NVARCHAR(30), pdp.STRT_DT, 101) + '-' + CONVERT(NVARCHAR(30), pdp.PLAN_END_DT,101) "PROGRAM PERIOD",
		pdps.incur_los_reim_fund_othr_amt 
		from prem_adj_pgm pdp
		inner join prem_adj_perd papd on pdp.prem_adj_pgm_id=papd.prem_adj_pgm_id
		inner join prem_adj_pgm_setup pdps on pdp.prem_adj_pgm_id=pdps.prem_adj_pgm_id
		where papd.prem_adj_perd_id in
		(
			select 
				substring( ',' + @check_recalc_prem_adj_perds + ',', i + 1, 
				charindex( ',', ',' + @check_recalc_prem_adj_perds + ',', i + 1 ) - i - 1 ) as RecalcProgPerds
				from #number
				where substring( ',' + @check_recalc_prem_adj_perds + ',', i, 1 ) = ','
				and i < len( ',' + @check_recalc_prem_adj_perds + ',' ) 
		)
		and pdps.incur_los_reim_fund_othr_amt<>0

		end
		else
		if(@len_calc>0 and @len_recalc=0)		
		begin
		
		select pdp.prem_adj_pgm_id, 
		CONVERT(NVARCHAR(30), pdp.STRT_DT, 101) + '-' + CONVERT(NVARCHAR(30), pdp.PLAN_END_DT,101) "PROGRAM PERIOD",
		pdps.incur_los_reim_fund_othr_amt
		from prem_adj_pgm pdp
		inner join prem_adj_pgm_setup pdps on pdp.prem_adj_pgm_id=pdps.prem_adj_pgm_id
		where pdp.prem_adj_pgm_id in
		(
			select substring( ',' + @check_calc_prog_perds + ',', n + 1, 
				charindex( ',', ',' + @check_calc_prog_perds + ',', n + 1 ) - n - 1 ) as ProgPerds
				from #num
				where substring( ',' + @check_calc_prog_perds + ',', n, 1 ) = ','
				and n < len( ',' + @check_calc_prog_perds + ',' ) 
		)
		and pdps.incur_los_reim_fund_othr_amt<>0
		
		end
		else
		if(@len_calc=0 and @len_recalc>0)		
		begin
		
		select pdp.prem_adj_pgm_id, 
		CONVERT(NVARCHAR(30), pdp.STRT_DT, 101) + '-' + CONVERT(NVARCHAR(30), pdp.PLAN_END_DT,101) "PROGRAM PERIOD",
		pdps.incur_los_reim_fund_othr_amt 
		from prem_adj_pgm pdp
		inner join prem_adj_perd papd on pdp.prem_adj_pgm_id=papd.prem_adj_pgm_id
		inner join prem_adj_pgm_setup pdps on pdp.prem_adj_pgm_id=pdps.prem_adj_pgm_id
		where papd.prem_adj_perd_id in
		(
			select 
				substring( ',' + @check_recalc_prem_adj_perds + ',', i + 1, 
				charindex( ',', ',' + @check_recalc_prem_adj_perds + ',', i + 1 ) - i - 1 ) as RecalcProgPerds
				from #number
				where substring( ',' + @check_recalc_prem_adj_perds + ',', i, 1 ) = ','
				and i < len( ',' + @check_recalc_prem_adj_perds + ',' ) 
		)
		and pdps.incur_los_reim_fund_othr_amt<>0
		
		end
		
		
		
		
		END TRY
BEGIN CATCH

	SELECT 
    ERROR_NUMBER() AS ERRORNUMBER,
    ERROR_SEVERITY() AS ERRORSEVERITY,
    ERROR_STATE() AS ERRORSTATE,
    ERROR_PROCEDURE() AS ERRORPROCEDURE,
    ERROR_LINE() AS ERRORLINE,
    ERROR_MESSAGE() AS ERRORMESSAGE;


END CATCH
END

go

if object_id('GetILRFOtherAmounts') is not null
	print 'Created Procedure GetILRFOtherAmounts'
else
	print 'Failed Creating Procedure GetILRFOtherAmounts'
go

if object_id('GetILRFOtherAmounts') is not null
	grant exec on GetILRFOtherAmounts to public
go