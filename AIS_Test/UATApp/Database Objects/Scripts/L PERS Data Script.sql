
SET IDENTITY_INSERT PERS ON
GO

-- System ID 
INSERT INTO [dbo].[PERS]
           ([pers_id] ,[mgr_id] ,[external_reference] ,[forename] ,[surname] ,[prefx_ttl_id] ,[phone_nbr_1_txt]  ,[phone_nbr_2_txt]
           ,[fax_nbr_txt]  ,[email_txt] ,[conctc_typ_id] ,[acct_qlty_cntrl_pct_id] ,[adj_qlty_cntrl_pct_id] ,[aries_qlty_cntrl_pct_id]
           ,[extrnl_org_id] ,[eff_dt] ,[expi_dt] ,[updt_user_id],[updt_dt] ,[crte_user_id] ,[crte_dt]  ,[actv_ind])
 VALUES (999999,null, ' ',	'SystemID',	'SystemID',	116,'9999999999',	'9999999999',
	'9999999999 ',	' ',	233,	NULL,	NULL,	NULL,
	NULL,	'01/01/2008',	null,	null,	null,	999999, '01/01/2008',	1 )


-- "Not Applicable" Broker
	INSERT INTO [dbo].[PERS]
           ([pers_id] ,[mgr_id] ,[external_reference] ,[forename] ,[surname] ,[prefx_ttl_id] ,[phone_nbr_1_txt]  ,[phone_nbr_2_txt]
           ,[fax_nbr_txt]  ,[email_txt] ,[conctc_typ_id] ,[acct_qlty_cntrl_pct_id] ,[adj_qlty_cntrl_pct_id] ,[aries_qlty_cntrl_pct_id]
           ,[extrnl_org_id] ,[eff_dt] ,[expi_dt] ,[updt_user_id],[updt_dt] ,[crte_user_id] ,[crte_dt]  ,[actv_ind])
 VALUES (1000000,null, ' ',	'Not',	'Applicable',	NULL,'9999999999',	'9999999999',
	'9999999999 ',	' ',	233,	NULL,	NULL,	NULL,
	NULL,	'01/01/2008',	null,	null,	null,	999999, '01/01/2008',	1 )
go


SET IDENTITY_INSERT PERS OFF
go

declare @id int
select @id = max(pers_id) from pers where pers_id < 999999
print 'IDD' + convert(varchar(20), @id)
DBCC CHECKIDENT('pers', RESEED,@id)
go