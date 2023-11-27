
if exists (select 1 from sysobjects 
		where name = 'fn_Getidsfromstring')
	drop function fn_Getidsfromstring
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
----- PROC NAME:  fn_Getidsfromstring
-----
----- VERSION:  SQL SERVER 2008
-----
----- CREATED:  CSC
-----
----- DESCRIPTION: RETRIEVES THE integer values from comma seperated varchar value.
-----
----- MODIFIED: 
-----
---------------------------------------------------------------------

CREATE FUNCTION [dbo].[fn_Getidsfromstring](@String varchar(MAX), @Delimiter char(1))       
returns @temptable TABLE (items varchar(MAX))       
as       
begin      
    declare @idx int       
    declare @slice varchar(8000)       

    select @idx = 1       
        if len(@String)<1 or @String is null  return       

    while @idx!= 0       
    begin       
        set @idx = charindex(@Delimiter,@String)       
        if @idx!=0       
            set @slice = left(@String,@idx - 1)       
        else       
            set @slice = @String       

        if(len(@slice)>0)  
            insert into @temptable(Items) values(@slice)       

        set @String = right(@String,len(@String) - @idx)       
        if len(@String) = 0 break       
    end   
return 
end

go

if object_id('fn_Getidsfromstring') is not null
	print 'Created function fn_Getidsfromstring'
else
	print 'Failed Creating Function fn_GetTotOfAuditPrem'
go

if object_id('fn_Getidsfromstring') is not null
	grant select on fn_Getidsfromstring to public


go
