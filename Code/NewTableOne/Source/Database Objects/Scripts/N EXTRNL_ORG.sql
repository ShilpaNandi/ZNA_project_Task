
SET IDENTITY_INSERT EXTRNL_ORG ON
GO

-- "Not Applicable" Broker
	INSERT INTO [dbo].[EXTRNL_ORG]
           ([EXTRNL_ORG_ID], [full_name], role_id, [updt_user_id],[updt_dt] ,[crte_user_id] ,[crte_dt]  ,[actv_ind])
 VALUES (1000000, 'Not Applicable', 233,	null,	null,	999999, '01/01/2008',	1 )
go


SET IDENTITY_INSERT EXTRNL_ORG OFF
go

declare @id int
select @id = max(extrnl_org_id) from EXTRNL_ORG where extrnl_org_id < 999999
print 'IDD' + convert(varchar(20), @id)
DBCC CHECKIDENT('EXTRNL_ORG', RESEED,@id)
go