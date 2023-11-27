IF EXISTS (SELECT 1 FROM sysobjects 
	WHERE NAME = 'GetCustmrDetails_WithRoles' and TYPE = 'P')
	DROP PROC GetCustmrDetails_WithRoles
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
            
--   exec  [GetCustmrDetails_WithRoles] null,'407'        
---------------------------------------------------------------------            
-----            
----- PROC NAME:  GetCustmrDetails_WithRoles            
-----            
----- VERSION:  SQL SERVER 2008            
-----            
----- AUTHOR :  Dheeraj Nadimpalli            
-----            
----- DESCRIPTION: Retrieves the result set for the customer details with roles  (As part of mass reassignment WO)       
----            
----- ON EXIT:             
-----               
-----            
----- MODIFIED:             
-----               
-----             
---------------------------------------------------------------------            
            
CREATE PROCEDURE [dbo].[GetCustmrDetails_WithRoles]            
            
@custmr_id int=null,        
@custmr_ids varchar(max)=null,        
@buofficeid int=null,        
@bp_ids varchar(max)=null,        
@buname varchar(20)=null,        
@buoffice varchar(20)=null,        
@brokerid int =null,        
@roleid int=null,        
@userid int=null,        
@bpnumber varchar(20)=null,        
@acct_range varchar(20)=null,        
@buoffice_range varchar(20)=null,        
@broker_range varchar(20)=null           
AS            
BEGIN            
-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.            
SET NOCOUNT ON;            
BEGIN TRY    

CREATE TABLE #TMP
 (
		[Account No] VARCHAR(MAX),
		[BP Number] VARCHAR(MAX),
		AccountName VARCHAR(MAX),
		[Broker Name] VARCHAR(MAX),
		[BU/Office] VARCHAR(MAX),
		[ACCOUNT SETUP QC] VARCHAR(MAX),
		[ADJUSTMENT QC 100%] VARCHAR(MAX),
		[ADJUSTMENT QC 20%] VARCHAR(MAX),
		[ARiES QC] VARCHAR(MAX),
		[C&RM ADMIN ANALYST] VARCHAR(MAX),
		[C&RM COLLECTION SPECIALIST] VARCHAR(MAX),
		[CFS1] VARCHAR(MAX),
		[CFS2] VARCHAR(MAX),
		[LSS Admin] VARCHAR(MAX),
		[RECONCILER] VARCHAR(MAX) 
     
      )

        
  DECLARE @cols AS NVARCHAR(MAX),          
    @query  AS NVARCHAR(MAX)        
           
           
  Declare @collacecustid varchar(100 )=  COALESCE(cast(@custmr_id as varchar(100)) ,'c.custmr_id') ,    
  @collacebuofficeid varchar(100)=COALESCE(cast(@buofficeid as varchar(100)),'isnull(pgmprd.bsn_unt_ofc_id,1)')  ,    
  @collacebuname varchar(100)=COALESCE(@buname,'isnull(intorg.bsn_unt_cd,1)'),    
  @collacebuoffice varchar(100)=COALESCE(@buoffice,'isnull(intorg.city_nm,1)') ,    
  @collacebrokerid varchar(100)=COALESCE(cast(@brokerid as varchar(10)),'isnull(pgmprd.brkr_id,1)') ,    
  @collaceroleid varchar(100)=COALESCE(cast(@roleid as varchar(10)),'isnull(r.rol_id,1)') ,      
  @collaceuserid varchar(100)=COALESCE(cast(@userid as varchar(10)),'isnull(r.pers_id,1)') ,      
  @collacebpnumber varchar(100)=COALESCE(@bpnumber,'isnull(c.finc_pty_id,1)'),      
  @collaceacct_range varchar(100)=COALESCE(@acct_range,'c.full_nm')  ,    
  @collacebuoffice_range varchar(100)=COALESCE(@buoffice_range,'isnull(intorg.city_nm,1)') ,     
  @collacebroker_range  varchar(100)=COALESCE(@broker_range,'isnull(extorg.full_name,1)')      
        
        
select @cols = STUFF((SELECT ',' + QUOTENAME(lkup_txt)           
                    from LKUP where lkup_typ_id=36   and actv_ind=1       
                    FOR XML PATH(''), TYPE          
            ).value('.', 'NVARCHAR(MAX)')           
        ,1,1,'')          
         --print @cols          
          
      print @collacecustid    
set @query = 'SELECT [Account No],
[BP Number],AccountName,
[Broker Name],
bsn_unt_cd + ''/'' + city_nm [BU/Office],'+ @cols + ' 

from         
             (        
                select c.custmr_id [Account No],                finc_pty_id [BP Number] ,c.full_nm as AccountName,extorg.full_name [Broker Name],                isnull(p.forename,'''')+'' ''+isnull(p.surname,'''') as name ,
                l.lkup_txt as lkuptxt  ,
                intorg.bsn_unt_cd,
                intorg.city_nm      
from CUSTMR c left join CUSTMR_PERS_REL r on c.custmr_id=r.custmr_id        
left outer join PREM_ADJ_PGM pgmprd on pgmprd.custmr_id=c.custmr_id      
left join PERS p on r.pers_id=p.pers_id left join LKUP l on l.lkup_id=r.rol_id        
left outer join INT_ORG intorg on pgmprd.bsn_unt_ofc_id = intorg.int_org_id      
left outer join EXTRNL_ORG extorg on pgmprd.brkr_id = extorg.extrnl_org_id      
where --l.actv_ind=1 and l.lkup_typ_id=36     
 --and 
 c.custmr_id = '+@collacecustid+'      
and ( c.custmr_id in (select items from fn_Getidsfromstring('''+isnull(@custmr_ids,'''''')+''','','')) or '''+isnull(@custmr_ids,1)+'''=''1'')    
 and  pgmprd.bsn_unt_ofc_id ='+@collacebuofficeid+'    
 and ( c.finc_pty_id in (select items from fn_Getidsfromstring('''+isnull(@bp_ids,'''''')+''','','')) or '''+isnull(@bp_ids,1)+'''=''1'')      
and isnull(intorg.bsn_unt_cd,1)= '+case when @collacebuname=@buname then ''''+@buname+'''' else @collacebuname end +'    
and isnull(intorg.city_nm,1)='+case when @collacebuoffice=@buoffice then ''''+@buoffice+'''' else @collacebuoffice end+'    
and isnull(pgmprd.brkr_id,1) =  '+@collacebrokerid+'    
   
and isnull(c.finc_pty_id,1) = '+case when @collacebpnumber=@bpnumber then ''''+@bpnumber+'''' else @collacebpnumber end +'    
and c.full_nm like '+ case when @collaceacct_range=@acct_range then ''''+@acct_range+'''' else @collaceacct_range end +'     
and isnull(intorg.city_nm,1) like '+ case when @collacebuoffice_range=@buoffice_range then ''''+@buoffice_range+'''' else @collacebuoffice_range end +'    
and isnull(extorg.full_name,1) like '+case when @collacebroker_range=@broker_range then ''''+@broker_range+'''' else @collacebroker_range end+'     
      
             ) x        
            pivot         
            (        
                max(name)        
                for lkuptxt in (' + @cols + ')        
            ) p   
order by AccountName asc '          
  print @query 

--execute(@query)  

truncate table #TMP

INSERT INTO #TMP  EXEC sp_executesql @query

--select * from #TMP


IF(ISNULL(@roleid,'')<>'' AND @roleid=359)
Select * from #TMP WHERE [ACCOUNT SETUP QC] =(select isnull(forename,'')+' '+isnull(surname,'') from PERS where pers_id=@collaceuserid)

ELSE 
IF(ISNULL(@roleid,'')<>'' AND @roleid=360)
BEGIN
print '360'
Select * from #TMP WHERE [ADJUSTMENT QC 100%]=(select isnull(forename,'')+' '+isnull(surname,'') from PERS where pers_id=@collaceuserid)
END
ELSE IF(ISNULL(@roleid,'')<>'' AND @roleid=361)
Select * from #TMP WHERE [ADJUSTMENT QC 20%]=(select isnull(forename,'')+' '+isnull(surname,'') from PERS where pers_id=@collaceuserid)

ELSE IF(ISNULL(@roleid,'')<>'' AND @roleid=362)
Select * from #TMP WHERE [ARiES QC]=(select isnull(forename,'')+' '+isnull(surname,'') from PERS where pers_id=@collaceuserid)

ELSE IF(ISNULL(@roleid,'')<>'' AND @roleid=363)
Select * from #TMP WHERE [C&RM ADMIN ANALYST] =(select isnull(forename,'')+' '+isnull(surname,'') from PERS where pers_id=@collaceuserid)

ELSE IF(ISNULL(@roleid,'')<>'' AND @roleid=364)
Select * from #TMP WHERE [C&RM COLLECTION SPECIALIST]=(select isnull(forename,'')+' '+isnull(surname,'') from PERS where pers_id=@collaceuserid)

ELSE IF(ISNULL(@roleid,'')<>'' AND @roleid=365)
Select * from #TMP WHERE [CFS1]=(select isnull(forename,'')+' '+isnull(surname,'') from PERS where pers_id=@collaceuserid)

ELSE IF(ISNULL(@roleid,'')<>'' AND @roleid=366)
Select * from #TMP WHERE [CFS2]=(select isnull(forename,'')+' '+isnull(surname,'') from PERS where pers_id=@collaceuserid)

ELSE IF(ISNULL(@roleid,'')<>'' AND @roleid=367)
Select * from #TMP WHERE [LSS Admin] =(select isnull(forename,'')+' '+isnull(surname,'') from PERS where pers_id=@collaceuserid)

ELSE IF(ISNULL(@roleid,'')<>'' AND @roleid=368)
Select * from #TMP WHERE [RECONCILER] =(select isnull(forename,'')+' '+isnull(surname,'') from PERS where pers_id=@collaceuserid)

ELSE IF(ISNULL(@roleid,'') = '' AND ISNULL(@userid,'') <> '')
BEGIN

DECLARE @pers_name varchar(100)
 set @pers_name = (select isnull(forename,'')+' '+isnull(surname,'') from PERS where pers_id=@collaceuserid)
 print @pers_name
select * from #tmp where ([ACCOUNT SETUP QC]=@pers_name or [ADJUSTMENT QC 100%]=@pers_name or [ADJUSTMENT QC 20%]=@pers_name 
or [ARiES QC]=@pers_name or [C&RM ADMIN ANALYST]=@pers_name or [C&RM COLLECTION SPECIALIST]=@pers_name or [CFS1]=@pers_name
or [CFS2]=@pers_name or [LSS Admin]=@pers_name or [RECONCILER]=@pers_name)

END

ELSE
	
	select * from #tmp
	
            
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

GO

IF OBJECT_ID('GetCustmrDetails_WithRoles') IS NOT NULL
	PRINT 'CREATED PROCEDURE GetCustmrDetails_WithRoles'
ELSE
	PRINT 'FAILED CREATING PROCEDURE GetCustmrDetails_WithRoles'
GO

IF OBJECT_ID('GetCustmrDetails_WithRoles') IS NOT NULL
	GRANT EXEC ON GetCustmrDetails_WithRoles TO PUBLIC
GO