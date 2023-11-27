
select * into [post_addr_Sep09_2016] from [post_addr]

UPDATE [dbo].[post_addr] 
SET [addr_ln_1_txt]  = '1299 Zurich Way'
    ,[addr_ln_2_txt] = ''                   
    ,[city_txt]      = 'Schaumburg'
	,post_cd_txt = '60196'
    ,[updt_dt]	     =getdate()
    
WHERE 
    (addr_ln_1_txt like '%140%amer%' AND (city_txt LIKE '%sch%' OR city_txt LIke '%mburg%' Or city_txt Like '%mberg%'))
OR  (addr_ln_2_txt like '%140%amer%' AND (city_txt LIKE '%sch%' OR city_txt LIke '%mburg%' Or city_txt Like '%mberg%'))
OR  (addr_ln_1_txt like '1400%' AND (city_txt LIKE '%sch%' OR city_txt LIke '%mburg%' Or city_txt Like '%mberg%'))
OR  (addr_ln_1_txt LIKE '%amer%' AND post_cd_txt like '6019_')

GO