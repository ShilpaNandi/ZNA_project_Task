
if exists (select 1 from sysobjects 
                where name = 'ModAdjCpy' and type = 'P')
        drop procedure ModAdjCpy
go
 
set ansi_nulls off
go

---------------------------------------------------------------------
-----
-----	Proc Name:	ModAdjCpy
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	Procedure creates a copy of the PREM_ADJ record and its child records
-----	
-----	On Exit:
-----	
-----
-----	Modified:	11/01/2008  Development Team
-----                   Created SP
-----
-----               07/19/2010 --Venkat Kolimi
-----               Added the statements related to the AIS 21 surcharges project
---------------------------------------------------------------------
create procedure ModAdjCpy
      @select         smallint = 0, 
      @prem_adj_id    int,
		@New_Adj_id int output

as
declare @error      int,
	@trancount  int,
	@ent_tistmp datetime,
	@identity   int,
	@prem_adj_perd_id int,
	@prem_adj_parmet_setup_id int,
	@prem_adj_retro_id int,
	@new_prem_adj_id int,
	@new_prem_adj_perd_id int,
	@new_prem_adj_parmet_setup_id int,
	@new_prem_adj_retro_id int,
	@perd_fetch_status int

select    @trancount  = @@trancount,
          @ent_tistmp = getdate( )

if @trancount = 0 
	begin
	    begin transaction 
	end


begin try

	-- Make a copy of the PREM_ADJ record
	exec AddPREM_ADJCpy   @prem_adj_id =  @prem_adj_id , @identity=@new_prem_adj_id output

	print 'new_prem_adj_id: ' + convert(varchar(20), isnull(@new_prem_adj_id,0))
	select @New_Adj_id=@new_prem_adj_id
	-- Make a copy of the PREM_ADJ_CLRING record
	exec AddPREM_ADJ_ARIES_CLRINGCpy  @prem_adj_id=@prem_adj_id, @new_prem_adj_id=@new_prem_adj_id 

   -- Make a copy of ARMIS_LOS_POL and ARMIS_LOS_EXC records(not testedyet)
	exec ModARMIS_LOS_POLCpy 	@prem_adj_id=@prem_adj_id, 	@new_prem_adj_id=@new_prem_adj_id 
						
	declare prem_adj_perd_cur cursor LOCAL FAST_FORWARD READ_ONLY 
		for 
		select  
		prem_adj_perd_id
		from dbo.PREM_ADJ_PERD 
		where prem_adj_id = @prem_adj_id
		
		
	open prem_adj_perd_cur
	fetch next from prem_adj_perd_cur into @prem_adj_perd_id
	select @perd_fetch_status = @@Fetch_Status
	while @perd_fetch_status = 0
	begin
		exec  AddPREM_ADJ_PERDCpy 	@prem_adj_id=@prem_adj_id, 
						@prem_adj_perd_id=@prem_adj_perd_id, 
						@new_prem_adj_id=@new_prem_adj_id, 
						@identity=@new_prem_adj_perd_id output

		print 'new_prem_adj_perd_id:  ' +  convert(varchar(20), isnull(@new_prem_adj_perd_id,0))


		exec  AddPREM_ADJ_COMB_ELEMTSCpy 	@prem_adj_id=@prem_adj_id, 
							@prem_adj_perd_id=@prem_adj_perd_id, 
							@new_prem_adj_id=@new_prem_adj_id, 
							@new_prem_adj_perd_id=@new_prem_adj_perd_id
						
		exec AddPREM_ADJ_LOS_REIM_FUND_POSCpy  	@prem_adj_id=@prem_adj_id, 
							@prem_adj_perd_id=@prem_adj_perd_id, 
							@new_prem_adj_id=@new_prem_adj_id, 
							@new_prem_adj_perd_id=@new_prem_adj_perd_id
				
		--Texas Tax:Added fro Texas Tax					
		exec AddPREM_ADJ_LOS_REIM_FUND_POST_TAXCpy  	@prem_adj_id=@prem_adj_id, 
							@prem_adj_perd_id=@prem_adj_perd_id, 
							@new_prem_adj_id=@new_prem_adj_id, 
							@new_prem_adj_perd_id=@new_prem_adj_perd_id
						
		exec AddPREM_ADJ_MISC_INVCCpy  		@prem_adj_id=@prem_adj_id, 
							@prem_adj_perd_id=@prem_adj_perd_id, 
							@new_prem_adj_id=@new_prem_adj_id, 
							@new_prem_adj_perd_id=@new_prem_adj_perd_id
						
		exec AddPREM_ADJ_NY_SCND_INJR_FUNDCpy  	@prem_adj_id=@prem_adj_id, 
							@prem_adj_perd_id=@prem_adj_perd_id, 
							@new_prem_adj_id=@new_prem_adj_id, 
							@new_prem_adj_perd_id=@new_prem_adj_perd_id							

		exec AddPREM_ADJ_PAID_LOS_BILCpy	@prem_adj_id=@prem_adj_id, 
							@prem_adj_perd_id=@prem_adj_perd_id, 
							@new_prem_adj_id=@new_prem_adj_id, 
							@new_prem_adj_perd_id=@new_prem_adj_perd_id

		exec ModPREM_ADJ_PARMETCpy  		@prem_adj_id=@prem_adj_id, 
							@prem_adj_perd_id=@prem_adj_perd_id, 
							@new_prem_adj_id=@new_prem_adj_id, 
							@new_prem_adj_perd_id=@new_prem_adj_perd_id
		
		exec ModPREM_ADJ_RETROCpy  		@prem_adj_id=@prem_adj_id, 
							@prem_adj_perd_id=@prem_adj_perd_id, 
							@new_prem_adj_id=@new_prem_adj_id, 
							@new_prem_adj_perd_id=@new_prem_adj_perd_id
		
		--Texas Tax:added for Texas Tax				
		exec ModPREM_ADJ_TAXCpy  		@prem_adj_id=@prem_adj_id, 
							@prem_adj_perd_id=@prem_adj_perd_id, 
							@new_prem_adj_id=@new_prem_adj_id, 
							@new_prem_adj_perd_id=@new_prem_adj_perd_id
							
		--Surcharges:added for Surcharges	
		/**************************************************
		Surcharges and Assesments: Added below statements 
		as part of the AIS 21 surcharges project
		***************************************************/
		exec ModPREM_ADJ_SURCHRGCpy  		@prem_adj_id=@prem_adj_id, 
					@prem_adj_perd_id=@prem_adj_perd_id, 
					@new_prem_adj_id=@new_prem_adj_id, 
					@new_prem_adj_perd_id=@new_prem_adj_perd_id
							
		exec AddPREM_ADJ_PERD_TOTCpy  	@prem_adj_id=@prem_adj_id, 
							@prem_adj_perd_id=@prem_adj_perd_id, 
							@new_prem_adj_id=@new_prem_adj_id, 
							@new_prem_adj_perd_id=@new_prem_adj_perd_id
				
			
		fetch next from prem_adj_perd_cur into @prem_adj_perd_id
		select @perd_fetch_status = @@Fetch_Status
	end
	close prem_adj_perd_cur
	deallocate prem_adj_perd_cur
	
	
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

if object_id('ModAdjCpy') is not null
        print 'Created Procedure ModAdjCpy'
else
        print 'Failed Creating Procedure ModAdjCpy'
go

if object_id('ModAdjCpy') is not null
        grant exec on ModAdjCpy to  public
go
 

