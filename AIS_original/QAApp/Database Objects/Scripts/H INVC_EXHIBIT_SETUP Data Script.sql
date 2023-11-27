
truncate table INVC_EXHIBIT_SETUP
go

DBCC CHECKIDENT('INVC_EXHIBIT_SETUP', RESEED,1)

go


INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2008','INV1',  'Broker Letter',1,'B',1,0,0)
INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2008','INV2',  'Adjustment Invoice',1,'B',2,0,0)
INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2008','INV3',  'Retrospective Premium Adjustment',1,'B',3,0,0)
INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2008','INV4',  'Retrospective Premium Adjustment Legend',1,'B',4,0,0)
INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2008','INV5',  'Loss Based Assessment Exhibit',1,'B',5,0,0)
INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2008','INV6',  'Claims Handling Fee', 1,'B',6,0,0)
INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2008','INV7',  'Excess Losses',1,'B',7,0,0)
INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2008','INV8',  'Cumulative Totals Worksheet',1,'B',8,0,0)
INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2008','INV9',  'Residual Market Subsidy Charge',1,'B',9,1,0)
INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2008','INV10', 'Workers Compensation Tax and Assessment Kentucky',1, 'B', 10,1,0)
INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2008','INV11', 'Adjustment of NY Second Injury Fund',1,'B',11,1,0)
INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2008','INV12', 'Loss Reimbursement Fund Adjustment -External',1,'B',12,0,0)
INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2008','INV13', 'Escrow Fund Adjustment',1,'B',13,0,0)
INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2008','INV14', 'Loss Reimbursement Fund Adjustment-Internal',1,'I',14, 0, 0)
INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2008','INV15', 'Aries Posting Details', 1,'I',15,0,0)
INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2008','INV16', 'Combined Elements Exhibit - Internal',1,'I',16,0,0)
INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2008','INV17', 'Processing and Distribution Checklist',1,'I',17,0,0)
INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2008','INV18', 'Cesar Coding Worksheet',1,null,18,1,0)

go