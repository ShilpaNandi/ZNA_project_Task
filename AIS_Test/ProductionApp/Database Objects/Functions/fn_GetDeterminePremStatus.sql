
if exists (select 1 from sysobjects 
		where name = 'fn_GetDeterminePremStatus' and type = 'FN')
	drop function fn_GetDeterminePremStatus
GO

set ansi_nulls off
GO
set quoted_identifier on
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		[fn_GetDeterminePremStatus]
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		CSC
-----
-----	Description:	Determine if the current adjustmetn is being created
-----					for a Premium only, Non-Premium only or both types of adjustments
-----
---------------------------------------------------------------------
create function [dbo].[fn_GetDeterminePremStatus]
(
    @CUSTOMER_ID int, 
    @VALN_MM_DT datetime, 
    @BRKR_ID int, 
    @BSN_UNT_OFC_ID int
)  
RETURNS char(2)
BEGIN  
DECLARE @indicator  CHAR(2)
set @indicator=' '

-- The adjustment is setup for both Premium and Non-Prmemium program period lines.
if exists (SELECT prem_adj_pgm_id FROM PREM_ADJ_PGM with(nolock)
	WHERE CUSTMR_ID = @customer_id
	AND nxt_valn_dt = @valn_mm_dt
	AND nxt_valn_dt_non_prem_dt = @valn_mm_dt
	AND brkr_id = @brkr_id
	AND bsn_unt_ofc_id = @bsn_unt_ofc_id)
begin
	set @indicator = 'B'
end
else
begin
	--Premium 
		if exists (SELECT prem_adj_pgm_id FROM PREM_ADJ_PGM with(nolock)
			WHERE CUSTMR_ID = @customer_id
			AND nxt_valn_dt = @valn_mm_dt
			AND brkr_id = @brkr_id
			AND bsn_unt_ofc_id = @bsn_unt_ofc_id)
		begin
			set @indicator = 'P'
		end
		else
		begin
			--Non-Premium
			if exists (SELECT prem_adj_pgm_id FROM PREM_ADJ_PGM with(nolock)
				WHERE CUSTMR_ID = @customer_id
				AND nxt_valn_dt_non_prem_dt = @valn_mm_dt
				AND brkr_id = @brkr_id
				AND bsn_unt_ofc_id = @bsn_unt_ofc_id)
			begin
				set @indicator = 'NP'
			end
			else
			begin
			-- May occur if its a child account 
				if exists (SELECT prem_adj_pgm_id FROM PREM_ADJ_PGM with(nolock)
						WHERE custmr_id IN (select custmr_id FROM custmr WHERE custmr_rel_id = @customer_id)
						AND nxt_valn_dt = @valn_mm_dt
						AND nxt_valn_dt_non_prem_dt = @valn_mm_dt
						AND brkr_id = @brkr_id
						AND bsn_unt_ofc_id = @bsn_unt_ofc_id) 
					begin
						set @indicator = 'B'
					end
					else
					begin
						--Premium  
							if exists (SELECT prem_adj_pgm_id FROM PREM_ADJ_PGM with(nolock)
								WHERE custmr_id IN (select custmr_id FROM custmr WHERE custmr_rel_id = @customer_id)
								AND nxt_valn_dt = @valn_mm_dt
								AND brkr_id = @brkr_id
								AND bsn_unt_ofc_id = @bsn_unt_ofc_id)
							begin
								set @indicator = 'P'
							end
							else
							begin
								--Non-Premium
								if exists (SELECT prem_adj_pgm_id FROM PREM_ADJ_PGM with(nolock)
									WHERE custmr_id IN (select custmr_id FROM custmr WHERE custmr_rel_id = @customer_id)
									AND nxt_valn_dt_non_prem_dt = @valn_mm_dt
									AND brkr_id = @brkr_id
									AND bsn_unt_ofc_id = @bsn_unt_ofc_id) 
								begin
									set @indicator = 'NP'
								end
							end
				end
			end
	end
end


return @indicator
 
END  
GO  
 
if object_id('fn_GetDeterminePremStatus') is not null
	print 'Created function fn_GetDeterminePremStatus'
else
	print 'Failed creating function fn_GetDeterminePremStatus'
go

if object_id('fn_GetDeterminePremStatus') is not null
	grant exec on fn_GetDeterminePremStatus to public
go
