
SET IDENTITY_INSERT POST_ADDR ON
GO
-- System ID Postal Address
INSERT INTO [dbo].[POST_ADDR]
([post_addr_id] ,[pers_id],[addr_ln_1_txt],[addr_ln_2_txt],[city_txt],[post_cd_txt],[st_id],[updt_user_id],[updt_dt],[crte_user_id],[crte_dt])
 VALUES (999999, 999999, '1400 American Lane ', ' ', 'Schaumburg',	'60173', 16, null,	null,999999, '01/01/2008' )

-- "Not Applicable" Broker Postal Address
INSERT INTO [dbo].[POST_ADDR]
([post_addr_id] ,[pers_id],[addr_ln_1_txt],[addr_ln_2_txt],[city_txt],[post_cd_txt],[st_id],[updt_user_id],[updt_dt],[crte_user_id],[crte_dt])
 VALUES (1000000, 1000000, '1400 American Lane ', ' ', 'Schaumburg',	'60173', 16, null,	null,999999, '01/01/2008' )
go

SET IDENTITY_INSERT POST_ADDR OFF
GO

declare @id int
select @id = max(post_addr_id) from POST_ADDR where post_addr_id < 999999
print 'IDD' + convert(varchar(20), @id)
DBCC CHECKIDENT('POST_ADDR', RESEED,@id)
go