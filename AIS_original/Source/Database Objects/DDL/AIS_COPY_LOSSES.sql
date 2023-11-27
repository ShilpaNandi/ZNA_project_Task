----------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------
GO
IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'ARMIS_LOS_POL' AND COLUMN_NAME = 'copy_ind')
BEGIN
	ALTER TABLE ARMIS_LOS_POL ADD copy_ind BIT NULL DEFAULT 0
END
GO
update ARMIS_LOS_POL set copy_ind=0

----------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------
GO
IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'ARMIS_LOS_EXC' AND COLUMN_NAME = 'copy_ind')
BEGIN
	ALTER TABLE ARMIS_LOS_EXC ADD copy_ind BIT NULL DEFAULT 0
END
GO
update ARMIS_LOS_EXC set copy_ind=0
----------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------