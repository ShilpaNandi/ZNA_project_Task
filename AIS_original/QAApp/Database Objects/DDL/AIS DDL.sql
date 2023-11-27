
CREATE TABLE ADJ_NBR_LKUP
( 
	adj_nbr_lkup_id      integer IDENTITY ( 1,1 ) ,
	adj_numercal_nbr     integer  NULL ,
	adj_nbr_txt          varchar(20)  NULL ,
	updt_dt              datetime  NULL ,
	updt_user_id         integer  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_539091212
		 DEFAULT  CURRENT_TIMESTAMP
)
ON "AIS_Data"
go



ALTER TABLE ADJ_NBR_LKUP
	ADD CONSTRAINT PK_ADJ_NBR_LKUP PRIMARY KEY  CLUSTERED (adj_nbr_lkup_id ASC)
	 ON "AIS_Data"
go



CREATE TABLE APLCTN_MENU
( 
	aplctn_menu_id       integer IDENTITY ( 1,1 ) ,
	menu_nm_txt          varchar(30)  NULL ,
	parnt_id             integer  NULL ,
	web_page_txt         varchar(50)  NULL ,
	menu_tooltip_txt     varchar(50)  NULL ,
	depnd_txt            varchar(30)  NULL ,
	page_catg_cd         char(3)  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_1796793882
		 DEFAULT  CURRENT_TIMESTAMP,
	actv_ind             bit  NULL 
)
ON "AIS_Data"
go



ALTER TABLE APLCTN_MENU
	ADD CONSTRAINT PK_APLCTN_MENU PRIMARY KEY  CLUSTERED (aplctn_menu_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX APLCTN_MENU_NN1 ON APLCTN_MENU
( 
	parnt_id              ASC
)
ON "AIS_IDX"
go



CREATE TABLE APLCTN_STS_LOG
( 
	aplctn_sts_log_id    integer IDENTITY ( 1,1 ) ,
	custmr_id            integer  NULL ,
	prem_adj_id          integer  NULL ,
	src_txt              varchar(30)  NOT NULL ,
	sev_cd               char(3)  NOT NULL ,
	shrt_desc_txt        varchar(100)  NOT NULL ,
	full_desc_txt        varchar(2048)  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_250570271
		 DEFAULT  CURRENT_TIMESTAMP,
	crte_user_id         integer  NOT NULL 
)
ON "AIS_Data"
go



ALTER TABLE APLCTN_STS_LOG
	ADD CONSTRAINT PK_APLCTN_STS_LOG PRIMARY KEY  CLUSTERED (aplctn_sts_log_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX APLCTN_STS_LOG_NN1 ON APLCTN_STS_LOG
( 
	src_txt               ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX APLCTN_STS_LOG_NN2 ON APLCTN_STS_LOG
( 
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX APLCTN_STS_LOG_NN3 ON APLCTN_STS_LOG
( 
	crte_dt               ASC
)
ON "AIS_IDX"
go



CREATE TABLE APLCTN_WEB_PAGE_AUDT
( 
	aplctn_web_page_audt_id integer IDENTITY ( 1,1 ) ,
	custmr_id            integer  NULL ,
	prem_adj_pgm_id      integer  NULL ,
	web_page_id          integer  NOT NULL ,
	web_page_cntrl_txt   varchar(50)  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_1636417924
		 DEFAULT  CURRENT_TIMESTAMP
)
ON "AIS_Data"
go



ALTER TABLE APLCTN_WEB_PAGE_AUDT
	ADD CONSTRAINT PK_APLCTN_WEB_PAGE_AUDT PRIMARY KEY  CLUSTERED (aplctn_web_page_audt_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX APLCTN_WEB_PAGE_AUDT_NN1 ON APLCTN_WEB_PAGE_AUDT
( 
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX APLCTN_WEB_PAGE_AUDT_NN2 ON APLCTN_WEB_PAGE_AUDT
( 
	prem_adj_pgm_id       ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX APLCTN_WEB_PAGE_AUDT_NN3 ON APLCTN_WEB_PAGE_AUDT
( 
	web_page_id           ASC
)
ON "AIS_IDX"
go



CREATE TABLE APLCTN_WEB_PAGE_AUTH
( 
	aplctn_web_page_auth_id integer IDENTITY ( 1,1 ) ,
	aplctn_menu_id       integer  NOT NULL ,
	secur_gp_id          integer  NOT NULL ,
	authd_ind            bit  NULL ,
	inquiry_full_acss_ind_cd char(1)  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_1636422008
		 DEFAULT  CURRENT_TIMESTAMP
)
ON "AIS_Data"
go



ALTER TABLE APLCTN_WEB_PAGE_AUTH
	ADD CONSTRAINT PK_APLCTN_WEB_PAGE_AUTH PRIMARY KEY  CLUSTERED (aplctn_web_page_auth_id ASC,aplctn_menu_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX APLCTN_WEB_PAGE_AUTH_NN1 ON APLCTN_WEB_PAGE_AUTH
( 
	aplctn_menu_id        ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX APLCTN_WEB_PAGE_AUTH_NN2 ON APLCTN_WEB_PAGE_AUTH
( 
	secur_gp_id           ASC
)
ON "AIS_IDX"
go



CREATE TABLE ARIES_TRNSMTL_HIST
( 
	prem_adj_perd_id     integer  NULL ,
	prem_adj_id          integer  NULL ,
	rel_prem_adj_id      integer  NULL ,
	custmr_id            integer  NULL ,
	trnsmtl_sent_ind     bit  NULL ,
	thrd_pty_admin_mnl_invc_id integer  NULL ,
	post_cd              char(1)  NULL ,
	post_trns_typ_id     integer  NOT NULL ,
	adj_typ_txt          varchar(256)  NULL ,
	aktyp_txt            char(2)  NULL ,
	gpart_txt            char(10)  NULL ,
	gpart_ext_txt        char(20)  NULL ,
	vtref_txt            char(20)  NULL ,
	posnr_txt            char(12)  NULL ,
	psngl_txt            char(1)  NULL ,
	pmtfr_txt            char(8)  NULL ,
	pmtto_txt            char(8)  NULL ,
	rskfr_txt            char(8)  NULL ,
	rskto_txt            char(8)  NULL ,
	pmend_txt            char(8)  NULL ,
	pmend_time_txt       char(6)  NULL ,
	renew_txt            char(1)  NULL ,
	rnewx_txt            char(3)  NULL ,
	ccd_txt              char(4)  NULL ,
	opccd_txt            char(4)  NULL ,
	gsber_txt            char(4)  NULL ,
	opgsber_txt          char(4)  NULL ,
	prgrp_txt            char(2)  NULL ,
	vsarl_vx_txt         char(6)  NULL ,
	amt_tot_txt          char(16)  NULL ,
	amt_inst_txt         char(16)  NULL ,
	atfrd_txt            char(8)  NULL ,
	atfrd_time_txt       char(6)  NULL ,
	endtyp_txt           char(1)  NULL ,
	amt_ned_txt          char(16)  NULL ,
	amt_end_txt          char(16)  NULL ,
	curr_txt             char(5)  NULL ,
	hvorg_txt            char(4)  NULL ,
	tvorg_txt            char(4)  NULL ,
	ophvorg_txt          char(4)  NULL ,
	optvorg_txt          char(4)  NULL ,
	blart_txt            char(2)  NULL ,
	vtre2_txt            char(20)  NULL ,
	vtre3_txt            char(20)  NULL ,
	vgpart2_txt          char(10)  NULL ,
	vgpart3_txt          char(10)  NULL ,
	gsfnr_txt            char(16)  NULL ,
	belnr_txt            char(6)  NULL ,
	bltyp_txt            char(2)  NULL ,
	emgpa_txt            char(10)  NULL ,
	embvt_txt            char(4)  NULL ,
	emadr_txt            char(10)  NULL ,
	pymet_txt            char(1)  NULL ,
	pybuk_txt            char(4)  NULL ,
	faedn_txt            char(8)  NULL ,
	budat_txt            char(8)  NULL ,
	bldat_txt            char(8)  NULL ,
	dun_rsn_txt          char(1)  NULL ,
	dun_rsn_dd_txt       char(3)  NULL ,
	pay_rsn_txt          char(1)  NULL ,
	pay_rsn_dd_txt       char(3)  NULL ,
	clr_rsn_txt          char(1)  NULL ,
	clr_rsn_dd_txt       char(3)  NULL ,
	kostl_txt            char(10)  NULL ,
	prctr_txt            char(10)  NULL ,
	pygrp_txt            char(10)  NULL ,
	grkey_txt            char(3)  NULL ,
	endrev_txt           char(1)  NULL ,
	sto_from_txt         char(8)  NULL ,
	sto_to_txt           char(8)  NULL ,
	orgin_txt            char(30)  NULL ,
	chklvl_txt           char(1)  NULL ,
	vbund_txt            char(6)  NULL ,
	refgf_txt            char(16)  NULL ,
	refbl_txt            char(6)  NULL ,
	bewar_txt            char(3)  NULL ,
	optxt_txt            char(50)  NULL ,
	zzpolsym_txt         char(3)  NULL ,
	zzpol_txt            char(18)  NULL ,
	zzmod_txt            char(2)  NULL ,
	zzaiserr_txt         char(1)  NULL ,
	zzinvgrp_txt         char(10)  NULL ,
	zzpclnknr_txt        char(12)  NULL ,
	yylob_cd_txt         char(5)  NULL ,
	yyamy_cd_txt         char(4)  NULL ,
	zzst_cd_txt          char(5)  NULL ,
	zzlbv_cd_txt         char(6)  NULL ,
	zzztm_cd_txt         char(5)  NULL ,
	zzreins_cd_txt       char(10)  NULL ,
	zzlit_cd_txt         char(5)  NULL ,
	zzwwzdc_txt          char(5)  NULL ,
	zzcnvt_itm_txt       char(1)  NULL ,
	zzclring_ind_txt     char(2)  NULL ,
	zzextref_txt         char(20)  NULL ,
	zzgpart4_txt         char(10)  NULL ,
	zzvtref4_txt         char(20)  NULL ,
	zzrevisonind_txt     char(1)  NULL ,
	zzcbs_ind_txt        char(1)  NULL ,
	zzccd_txt            char(4)  NULL ,
	zzamt_comm_txt       char(16)  NULL ,
	zzeor_txt            char(3)  NULL ,
	zzrectyp_txt         char(1)  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_927051678
		 DEFAULT  CURRENT_TIMESTAMP,
	actv_ind             bit  NULL 
)
ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX ARIES_TRNSMTL_HIST_NN1 ON ARIES_TRNSMTL_HIST
( 
	prem_adj_perd_id      ASC,
	prem_adj_id           ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX ARIES_TRNSMTL_HIST_NN2 ON ARIES_TRNSMTL_HIST
( 
	post_trns_typ_id      ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX ARIES_TRNSMTL_HIST_NN3 ON ARIES_TRNSMTL_HIST
( 
	prem_adj_id           ASC
)
go



CREATE TABLE ARMIS_INTRFC37
( 
	armis_intrfc37_id    integer IDENTITY ( 1,1 ) ,
	valn_dt              datetime  NULL ,
	suprt_serv_custmr_gp_id char(10)  NULL ,
	cls1_txt             varchar(254)  NULL ,
	cls2_txt             varchar(254)  NULL ,
	cls3_txt             varchar(254)  NULL ,
	cls4_txt             varchar(254)  NULL ,
	pol_eff_dt           datetime  NULL ,
	pol_expiry_dt        datetime  NULL ,
	ln_of_bsn_txt        varchar(100)  NULL ,
	pol_nbr_txt          varchar(100)  NULL ,
	st_txt               varchar(100)  NULL ,
	paid_exps_amt        decimal(15,2)  NULL ,
	paid_los_bimed_amt   decimal(15,2)  NULL ,
	paid_los_lt_amt      decimal(15,2)  NULL ,
	resrvd_bimed_amt     decimal(15,2)  NULL ,
	resrvd_lt_amt        decimal(15,2)  NULL ,
	resrvd_exps_amt      decimal(15,2)  NULL ,
	wcd_undrln_ind       char(1)  NULL ,
	crte_dt              datetime  NULL 
)
ON "AIS_Data"
go



ALTER TABLE ARMIS_INTRFC37
	ADD CONSTRAINT PK_ARMIS_INTRFC37 PRIMARY KEY  CLUSTERED (armis_intrfc37_id ASC)
	 ON "AIS_Data"
go



CREATE TABLE ARMIS_INTRFC37_ERR
( 
	armis_intrfc37_err_id integer IDENTITY ( 1,1 ) ,
	valn_dt              datetime  NULL ,
	suprt_serv_custmr_gp_id char(10)  NULL ,
	cls1_txt             varchar(254)  NULL ,
	cls2_txt             varchar(254)  NULL ,
	cls3_txt             varchar(254)  NULL ,
	cls4_txt             varchar(254)  NULL ,
	pol_eff_dt           datetime  NULL ,
	pol_expiry_dt        datetime  NULL ,
	ln_of_bsn_txt        varchar(100)  NULL ,
	pol_nbr_txt          varchar(100)  NULL ,
	st_txt               varchar(100)  NULL ,
	paid_exps_amt        decimal(15,2)  NULL ,
	paid_los_bimed_amt   decimal(15,2)  NULL ,
	paid_los_lt_amt      decimal(15,2)  NULL ,
	resrvd_bimed_amt     decimal(15,2)  NULL ,
	resrvd_lt_amt        decimal(15,2)  NULL ,
	resrvd_exps_amt      decimal(15,2)  NULL ,
	wcd_undrln_ind       char(1)  NULL ,
	err_txt              varchar(100)  NULL ,
	crte_dt              datetime  NULL 
)
ON "AIS_Data"
go



ALTER TABLE ARMIS_INTRFC37_ERR
	ADD CONSTRAINT PK_ARMIS_INTRFC37_ERR PRIMARY KEY  CLUSTERED (armis_intrfc37_err_id ASC)
	 ON "AIS_Data"
go



CREATE TABLE ARMIS_INTRFC38
( 
	armis_intrfc38_id    integer IDENTITY ( 1,1 ) ,
	valn_dt              datetime  NULL ,
	suprt_serv_custmr_gp_id char(10)  NULL ,
	cls1_txt             varchar(254)  NULL ,
	cls2_txt             varchar(254)  NULL ,
	cls3_txt             varchar(254)  NULL ,
	cls4_txt             varchar(254)  NULL ,
	pol_eff_dt           datetime  NULL ,
	pol_expiry_dt        datetime  NULL ,
	data_src_txt         varchar(254)  NULL ,
	clm_nbr_txt          varchar(254)  NULL ,
	site_cd_txt          varchar(20)  NULL ,
	ln_of_bsn_txt        varchar(100)  NULL ,
	covg_txt             varchar(100)  NULL ,
	pol_nbr_txt          varchar(100)  NULL ,
	pol_eff_2_dt         datetime  NULL ,
	paid_los_bimed_amt   decimal(15,2)  NULL ,
	paid_los_lt_amt      decimal(15,2)  NULL ,
	paid_exps_amt        decimal(15,2)  NULL ,
	resrvd_exps_amt      decimal(15,2)  NULL ,
	resrvd_los_bimed_amt decimal(15,2)  NULL ,
	resrv_los_lt_amt     decimal(15,2)  NULL ,
	clmt_nm              varchar(100)  NULL ,
	st_txt               varchar(100)  NULL ,
	clm_sts_txt          varchar(254)  NULL ,
	covg_trigr_dt        datetime  NULL ,
	orgin_clm_nbr_txt    varchar(254)  NULL ,
	reop_clm_nbr_txt     varchar(254)  NULL ,
	crte_dt              datetime  NULL 
	CONSTRAINT CURRENT_TIMESTAMP_521713375
		 DEFAULT  CURRENT_TIMESTAMP
)
ON "AIS_Data"
go



ALTER TABLE ARMIS_INTRFC38
	ADD CONSTRAINT PK_ARMIS_INTRFC38 PRIMARY KEY  CLUSTERED (armis_intrfc38_id ASC)
	 ON "AIS_Data"
go



CREATE TABLE ARMIS_INTRFC38_ERR
( 
	armis_intrfc38_err_id integer IDENTITY ( 1,1 ) ,
	valn_dt              datetime  NULL ,
	suprt_serv_custmr_gp_id char(10)  NULL ,
	cls1_txt             varchar(254)  NULL ,
	cls2_txt             varchar(254)  NULL ,
	cls3_txt             varchar(254)  NULL ,
	cls4_txt             varchar(254)  NULL ,
	pol_eff_dt           datetime  NULL ,
	pol_expiry_dt        datetime  NULL ,
	data_src_txt         varchar(254)  NULL ,
	clm_nbr_txt          varchar(254)  NULL ,
	site_cd_txt          varchar(20)  NULL ,
	ln_of_bsn_txt        varchar(100)  NULL ,
	covg_txt             varchar(100)  NULL ,
	pol_nbr_txt          varchar(100)  NULL ,
	pol_eff_2_dt         datetime  NULL ,
	paid_los_bimed_amt   decimal(15,2)  NULL ,
	paid_los_lt_amt      decimal(15,2)  NULL ,
	paid_exps_amt        decimal(15,2)  NULL ,
	resrvd_exps_amt      decimal(15,2)  NULL ,
	resrvd_los_bimed_amt decimal(15,2)  NULL ,
	resrv_los_lt_amt     decimal(15,2)  NULL ,
	clmt_nm              varchar(100)  NULL ,
	st_txt               varchar(100)  NULL ,
	clm_sts_txt          varchar(254)  NULL ,
	covg_trigr_dt        datetime  NULL ,
	orgin_clm_nbr_txt    varchar(254)  NULL ,
	reop_clm_nbr_txt     varchar(254)  NULL ,
	err_txt              varchar(100)  NULL ,
	crte_dt              datetime  NULL 
	CONSTRAINT CURRENT_TIMESTAMP_1076665204
		 DEFAULT  CURRENT_TIMESTAMP
)
ON "AIS_Data"
go



ALTER TABLE ARMIS_INTRFC38_ERR
	ADD CONSTRAINT PK_ARMIS_INTRFC38_ERR PRIMARY KEY  CLUSTERED (armis_intrfc38_err_id ASC)
	 ON "AIS_Data"
go



CREATE TABLE ARMIS_LOS_EXC
( 
	armis_los_exc_id     integer IDENTITY ( 1,1 ) ,
	armis_los_pol_id     integer  NOT NULL ,
	coml_agmt_id         integer  NOT NULL ,
	prem_adj_pgm_id      integer  NOT NULL ,
	custmr_id            integer  NOT NULL ,
	orgin_clm_nbr_txt    varchar(20)  NULL ,
	clm_nbr_txt          varchar(100)  NULL ,
	addn_clm_ind         bit  NULL ,
	addn_clm_txt         varchar(50)  NULL ,
	lim2_amt             decimal(15,2)  NULL ,
	site_cd_txt          varchar(50)  NULL ,
	covg_trigr_dt        datetime  NULL ,
	clmt_nm              varchar(50)  NULL ,
	reop_clm_nbr_txt     varchar(20)  NULL ,
	paid_idnmty_amt      decimal(15,2)  NULL ,
	paid_exps_amt        decimal(15,2)  NULL ,
	resrvd_idnmty_amt    decimal(15,2)  NULL ,
	resrvd_exps_amt      decimal(15,2)  NULL ,
	non_bilabl_paid_idnmty_amt decimal(15,2)  NULL ,
	non_bilabl_paid_exps_amt decimal(15,2)  NULL ,
	non_bilabl_resrvd_idnmty_amt decimal(15,2)  NULL ,
	non_bilabl_resrvd_exps_amt decimal(15,2)  NULL ,
	subj_paid_idnmty_amt decimal(15,2)  NULL ,
	subj_paid_exps_amt   decimal(15,2)  NULL ,
	subj_resrvd_idnmty_amt decimal(15,2)  NULL ,
	subj_resrvd_exps_amt decimal(15,2)  NULL ,
	exc_paid_idnmty_amt  decimal(15,2)  NULL ,
	exc_paid_exps_amt    decimal(15,2)  NULL ,
	exc_resrvd_idnmty_amt decimal(15,2)  NULL ,
	exc_resrvd_exps_amt  decimal(15,2)  NULL ,
	sys_genrt_ind        bit  NULL ,
	clm_sts_id           integer  NULL ,
	exc_ldf_ibnr_amt     decimal(15,2)  NULL ,
	subj_ldf_ibnr_amt    decimal(15,2)  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_350952977
		 DEFAULT  CURRENT_TIMESTAMP,
	actv_ind             bit  NULL 
)
ON "AIS_Data"
go



ALTER TABLE ARMIS_LOS_EXC
	ADD CONSTRAINT PK_ARMIS_LOS_EXC PRIMARY KEY  CLUSTERED (armis_los_exc_id ASC,armis_los_pol_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX ARMIS_LOS_EXC_NN1 ON ARMIS_LOS_EXC
( 
	armis_los_pol_id      ASC
)
INCLUDE( coml_agmt_id,prem_adj_pgm_id,custmr_id )
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX ARMIS_LOS_EXC_NN2 ON ARMIS_LOS_EXC
( 
	clm_sts_id            ASC
)
ON "AIS_IDX"
go



CREATE TABLE ARMIS_LOS_POL
( 
	armis_los_pol_id     integer IDENTITY ( 1,1 ) ,
	coml_agmt_id         integer  NOT NULL ,
	prem_adj_pgm_id      integer  NOT NULL ,
	custmr_id            integer  NOT NULL ,
	st_id                integer  NOT NULL ,
	valn_dt              datetime  NULL ,
	prem_adj_id          integer  NULL ,
	suprt_serv_custmr_gp_id char(10)  NULL ,
	paid_idnmty_amt      decimal(15,2)  NULL ,
	paid_exps_amt        decimal(15,2)  NULL ,
	resrv_idnmty_amt     decimal(15,2)  NULL ,
	resrv_exps_amt       decimal(15,2)  NULL ,
	non_bilabl_paid_idnmty_amt decimal(15,2)  NULL ,
	non_bilabl_paid_exps_amt decimal(15,2)  NULL ,
	non_bilabl_resrv_idnmty_amt decimal(15,2)  NULL ,
	non_bilabl_resrv_exps_amt decimal(15,2)  NULL ,
	subj_paid_idnmty_amt decimal(15,2)  NULL ,
	subj_paid_exps_amt   decimal(15,2)  NULL ,
	subj_resrv_idnmty_amt decimal(15,2)  NULL ,
	subj_resrv_exps_amt  decimal(15,2)  NULL ,
	exc_paid_idnmty_amt  decimal(15,2)  NULL ,
	exc_paid_exps_amt    decimal(15,2)  NULL ,
	exc_resrvd_idnmty_amt decimal(15,2)  NULL ,
	exc_resrv_exps_amt   decimal(15,2)  NULL ,
	subj_ldf_ibnr_amt    decimal(15,2)  NULL ,
	exc_ldf_ibnr_amt     decimal(15,2)  NULL ,
	sys_genrt_ind        bit  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_351671578
		 DEFAULT  CURRENT_TIMESTAMP,
	actv_ind             bit  NULL 
)
ON "AIS_Data"
go



ALTER TABLE ARMIS_LOS_POL
	ADD CONSTRAINT PK_ARMIS_LOS_POL PRIMARY KEY  CLUSTERED (armis_los_pol_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX ARMIS_LOS_POL_NN1 ON ARMIS_LOS_POL
( 
	coml_agmt_id          ASC,
	prem_adj_pgm_id       ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX ARMIS_LOS_POL_NN2 ON ARMIS_LOS_POL
( 
	st_id                 ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX ARMIS_LOS_POL_NN4 ON ARMIS_LOS_POL
( 
	prem_adj_pgm_id       ASC,
	custmr_id             ASC,
	actv_ind              ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX ARMIS_LOS_POL_NN5 ON ARMIS_LOS_POL
( 
	prem_adj_pgm_id       ASC,
	custmr_id             ASC,
	non_bilabl_paid_idnmty_amt  ASC,
	non_bilabl_paid_exps_amt  ASC,
	non_bilabl_resrv_idnmty_amt  ASC,
	non_bilabl_resrv_exps_amt  ASC,
	exc_paid_idnmty_amt   ASC,
	exc_paid_exps_amt     ASC,
	exc_resrvd_idnmty_amt  ASC,
	exc_resrv_exps_amt    ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX ARMIS_LOS_POL_NN3 ON ARMIS_LOS_POL
( 
	prem_adj_id           ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE TABLE COMB_ELEMTS
( 
	comb_elemts_id       integer IDENTITY ( 1,1 ) ,
	coml_agmt_id         integer  NOT NULL ,
	prem_adj_pgm_id      integer  NOT NULL ,
	custmr_id            integer  NOT NULL ,
	dvsr_nbr_id          integer  NULL ,
	expo_typ_id          integer  NULL ,
	actv_ind             bit  NULL ,
	tot_amt              decimal(15,2)  NULL ,
	adj_rt               decimal(15,8)  NULL ,
	audt_expo_amt        decimal(15,2)  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_1946413614
		 DEFAULT  CURRENT_TIMESTAMP
)
ON "AIS_Data"
go



ALTER TABLE COMB_ELEMTS
	ADD CONSTRAINT PK_COMB_ELEMTS PRIMARY KEY  CLUSTERED (comb_elemts_id ASC,coml_agmt_id ASC,prem_adj_pgm_id ASC,custmr_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX COMB_ELEMTS_NN3 ON COMB_ELEMTS
( 
	coml_agmt_id          ASC,
	prem_adj_pgm_id       ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX COMB_ELEMTS_NN1 ON COMB_ELEMTS
( 
	expo_typ_id           ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX COMB_ELEMTS_NN2 ON COMB_ELEMTS
( 
	dvsr_nbr_id           ASC
)
ON "AIS_IDX"
go



CREATE TABLE COML_AGMT
( 
	coml_agmt_id         integer IDENTITY ( 1,1 ) ,
	prem_adj_pgm_id      integer  NOT NULL ,
	custmr_id            integer  NOT NULL ,
	pol_sym_txt          char(3)  NULL ,
	pol_nbr_txt          char(15)  NULL ,
	pol_modulus_txt      char(2)  NULL ,
	pol_eff_dt           datetime  NULL ,
	planned_end_date     datetime  NOT NULL ,
	adj_typ_id           integer  NULL ,
	aloc_los_adj_exps_typ_id integer  NULL ,
	dedtbl_pol_lim_amt   decimal(15,2)  NULL ,
	nonconv_amt          decimal(15,2)  NULL ,
	los_dev_fctr_rt      decimal(15,8)  NULL ,
	incur_but_not_rptd_fctr_rt decimal(15,8)  NULL ,
	aloc_los_adj_exps_capped_amt decimal(15,2)  NULL ,
	unlim_dedtbl_pol_lim_ind bit  NULL ,
	unlim_overrid_dedtbl_lim_ind bit  NULL ,
	overrid_dedtbl_lim_amt decimal(15,2)  NULL ,
	los_dev_fctr_incur_but_not_rptd_incld_lim_ind bit  NULL ,
	los_dev_fctr_incur_but_not_rptd_step_ind bit  NULL ,
	thrd_pty_admin_ind   bit  NULL ,
	los_sys_src_id       integer  NULL ,
	thrd_pty_admin_dir_ind bit  NULL ,
	othr_pol_adj_amt     decimal(15,2)  NULL ,
	dedtbl_prot_pol_st_id integer  NULL ,
	dedtbl_prot_pol_max_amt decimal(15,2)  NULL ,
	dedtbl_prot_pol_ind  bit  NULL ,
	covg_typ_id          integer  NULL ,
	parnt_coml_agmt_id   integer  NULL ,
	mstr_peo_pol_ind     bit  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_1801402660
		 DEFAULT  CURRENT_TIMESTAMP,
	actv_ind             bit  NULL 
)
ON "AIS_Data"
go



ALTER TABLE COML_AGMT
	ADD CONSTRAINT PK_COML_AGMT PRIMARY KEY  CLUSTERED (coml_agmt_id ASC,prem_adj_pgm_id ASC,custmr_id ASC)
	 ON "AIS_Data"
go



CREATE INDEX COML_AGMT_NN4 ON COML_AGMT
( 
	prem_adj_pgm_id       ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE INDEX COML_AGMT_NN7 ON COML_AGMT
( 
	prem_adj_pgm_id       ASC,
	custmr_id             ASC,
	parnt_coml_agmt_id    ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX COML_AGMT_NN6 ON COML_AGMT
( 
	dedtbl_prot_pol_st_id  ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX COML_AGMT_NN1 ON COML_AGMT
( 
	adj_typ_id            ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX COML_AGMT_NN2 ON COML_AGMT
( 
	aloc_los_adj_exps_typ_id  ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX COML_AGMT_NN3 ON COML_AGMT
( 
	los_sys_src_id        ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX COML_AGMT_NN5 ON COML_AGMT
( 
	covg_typ_id           ASC
)
ON "AIS_IDX"
go



CREATE TABLE COML_AGMT_AUDT
( 
	coml_agmt_audt_id    integer IDENTITY ( 1,1 ) ,
	coml_agmt_id         integer  NOT NULL ,
	prem_adj_pgm_id      integer  NOT NULL ,
	custmr_id            integer  NOT NULL ,
	expo_amt             decimal(15,2)  NULL ,
	strt_dt              datetime  NOT NULL ,
	subj_audt_prem_amt   decimal(15,2)  NULL ,
	non_subj_audt_prem_amt decimal(15,2)  NULL ,
	subj_depst_prem_amt  decimal(15,2)  NULL ,
	defr_depst_prem_amt  decimal(15,2)  NULL ,
	non_subj_depst_prem_amt decimal(15,2)  NULL ,
	audt_revd_sts_ind    bit  NULL ,
	audt_rslt_amt        decimal(15,2)  NULL ,
	adj_ind              bit  NULL ,
	Prem_adj_id			 int NULL,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_637835223
		 DEFAULT  CURRENT_TIMESTAMP
)
ON "AIS_Data"
go



ALTER TABLE COML_AGMT_AUDT
	ADD CONSTRAINT PK_COML_AGMT_AUDT PRIMARY KEY  NONCLUSTERED (coml_agmt_audt_id ASC)
	 ON "AIS_Data"
go



CREATE INDEX COML_AGMT_AUDT_NN1 ON COML_AGMT_AUDT
( 
	coml_agmt_id          ASC,
	prem_adj_pgm_id       ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX COML_AGMT_AUDT_NN2 ON COML_AGMT_AUDT
( 
	prem_adj_pgm_id       ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE TABLE CUSTMR
( 
	custmr_id            integer IDENTITY ( 1,1 ) ,
	custmr_rel_id        integer  NULL ,
	custmr_rel_actv_ind  bit  NULL ,
	full_nm              varchar(100)  NOT NULL ,
	finc_pty_id          char(10)  NULL ,
	suprt_serv_custmr_gp_id char(10)  NULL ,
	bnkrpt_buyout_eff_dt datetime  NULL ,
	md_retro_adj_ind     bit  NULL ,
	thrd_pty_admin_funded_ind bit  NULL ,
	peo_ind              bit  NULL ,
	bnkrpt_buyout_id     integer  NULL ,
	mstr_acct_ind        bit  NULL ,
	thrd_pty_admin_funded_dt datetime  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_1465957773
		 DEFAULT  CURRENT_TIMESTAMP,
	actv_ind             bit  NULL 
)
ON "AIS_Data"
go



ALTER TABLE CUSTMR
	ADD CONSTRAINT PK_CUSTMR PRIMARY KEY  CLUSTERED (custmr_id ASC)
	 ON "AIS_Data"
go



CREATE UNIQUE NONCLUSTERED INDEX CUSTMR_NN3 ON CUSTMR
( 
	full_nm               ASC
)
ON "AIS_IDX"
go



CREATE INDEX CUSTMR_NN1 ON CUSTMR
( 
	custmr_rel_id         ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX CUSTMR_NN2 ON CUSTMR
( 
	bnkrpt_buyout_id      ASC
)
ON "AIS_IDX"
go



CREATE TABLE CUSTMR_CMMNT
( 
	custmr_cmmnt_id      integer IDENTITY ( 1,1 ) ,
	custmr_id            integer  NOT NULL ,
	cmmnt_txt            varchar(4096)  NULL ,
	cmmnt_catg_id        integer  NOT NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_503239215
		 DEFAULT  CURRENT_TIMESTAMP
)
ON "AIS_Data"
go



ALTER TABLE CUSTMR_CMMNT
	ADD CONSTRAINT PK_CUSTMR_CMMNT PRIMARY KEY  CLUSTERED (custmr_cmmnt_id ASC,custmr_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX CUSTMR_CMMNT_NN1 ON CUSTMR_CMMNT
( 
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX CUSTMR_CMMNT_NN2 ON CUSTMR_CMMNT
( 
	cmmnt_catg_id         ASC
)
ON "AIS_IDX"
go



CREATE TABLE CUSTMR_DOC
( 
	custmr_doc_id        integer IDENTITY ( 1,1 ) ,
	custmr_id            integer  NULL ,
	frm_id               integer  NOT NULL ,
	recd_dt              datetime  NULL ,
	pgm_eff_dt           datetime  NULL ,
	pgm_expi_dt          datetime  NULL ,
	ent_dt               datetime  NULL ,
	qlty_cntrl_dt        datetime  NULL ,
	valn_dt              datetime  NULL ,
	md_retro_adj_amt     decimal(15,2)  NULL ,
	qlty_cntrl_pers_id   integer  NOT NULL ,
	twenty_pct_qlty_cntrl_ind bit  NULL ,
	cash_flw_splist_pers_id integer  NOT NULL ,
	cmmnt_txt            varchar(512)  NULL ,
	bu_office_id         integer  NULL ,
	nonais_custmr_id     integer  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_1230697264
		 DEFAULT  CURRENT_TIMESTAMP,
	actv_ind             bit  NULL 
)
ON "AIS_Data"
go



ALTER TABLE CUSTMR_DOC
	ADD CONSTRAINT PK_CUSTMR_DOC PRIMARY KEY  CLUSTERED (custmr_doc_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX CUSTMR_DOC_NN2 ON CUSTMR_DOC
( 
	qlty_cntrl_pers_id    ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX CUSTMR_DOC_NN1 ON CUSTMR_DOC
( 
	frm_id                ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX CUSTMR_DOC_NN3 ON CUSTMR_DOC
( 
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX CUSTMR_DOC_NN4 ON CUSTMR_DOC
( 
	cash_flw_splist_pers_id  ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX CUSTMR_DOC_NN5 ON CUSTMR_DOC
( 
	bu_office_id          ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX CUSTMR_DOC_NN6 ON CUSTMR_DOC
( 
	nonais_custmr_id      ASC
)
ON "AIS_IDX"
go



CREATE TABLE CUSTMR_DOC_ISSUS
( 
	custmr_doc_issus_id  integer IDENTITY ( 1,1 ) ,
	custmr_doc_id        integer  NOT NULL ,
	traking_issu_id      integer  NOT NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_927634459
		 DEFAULT  CURRENT_TIMESTAMP
)
ON "AIS_Data"
go



ALTER TABLE CUSTMR_DOC_ISSUS
	ADD CONSTRAINT PK_CUSTMR_DOC_ISSUS PRIMARY KEY  CLUSTERED (custmr_doc_issus_id ASC,custmr_doc_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX CUSTMR_DOC_ISSUS_NN1 ON CUSTMR_DOC_ISSUS
( 
	custmr_doc_id         ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX CUSTMR_DOC_ISSUS_NN2 ON CUSTMR_DOC_ISSUS
( 
	traking_issu_id       ASC
)
ON "AIS_IDX"
go



CREATE TABLE CUSTMR_PERS_REL
( 
	custmr_pers_rel_id   integer IDENTITY ( 1,1 ) ,
	custmr_id            integer  NOT NULL ,
	pers_id              integer  NULL ,
	rol_id               integer  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_137653304
		 DEFAULT  CURRENT_TIMESTAMP
)
ON "AIS_Data"
go



ALTER TABLE CUSTMR_PERS_REL
	ADD CONSTRAINT PK_CUSTMR_PERS_REL PRIMARY KEY  NONCLUSTERED (custmr_pers_rel_id ASC)
	 ON "AIS_Data"
go



CREATE UNIQUE NONCLUSTERED INDEX CUSTMR_PERS_REL_NN4 ON CUSTMR_PERS_REL
( 
	custmr_id             ASC,
	pers_id               ASC,
	rol_id                ASC
)
ON "AIS_IDX"
go



CREATE INDEX CUSTMR_PERS_REL_NN1 ON CUSTMR_PERS_REL
( 
	pers_id               ASC
)
ON "AIS_IDX"
go



CREATE INDEX CUSTMR_PERS_REL_NN2 ON CUSTMR_PERS_REL
( 
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX CUSTMR_PERS_REL_NN3 ON CUSTMR_PERS_REL
( 
	rol_id                ASC
)
ON "AIS_IDX"
go



CREATE TABLE EXTRNL_ORG
( 
	extrnl_org_id        integer IDENTITY ( 1,1 ) ,
	full_name            varchar(100)  NULL ,
	role_id              integer  NOT NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_1213328687
		 DEFAULT  CURRENT_TIMESTAMP,
	actv_ind             bit  NULL 
)
ON "AIS_Data"
go



ALTER TABLE EXTRNL_ORG
	ADD CONSTRAINT PK_EXTRNL_ORG PRIMARY KEY  CLUSTERED (extrnl_org_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX EXTRNL_ORG_NN1 ON EXTRNL_ORG
( 
	role_id               ASC
)
ON "AIS_IDX"
go



CREATE TABLE INCUR_LOS_REIM_FUND_FRMLA
( 
	incur_los_reim_fund_frmla_id integer IDENTITY ( 1,1 ) ,
	prem_adj_pgm_id      integer  NOT NULL ,
	custmr_id            integer  NOT NULL ,
	los_reim_fund_fctr_typ_id integer  NOT NULL ,
	use_paid_los_ind     bit  NULL ,
	use_paid_aloc_los_adj_exps_ind bit  NULL ,
	use_resrv_los_ind    bit  NULL ,
	use_resrv_aloc_los_adj_exps_ind bit  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_18937606
		 DEFAULT  CURRENT_TIMESTAMP
)
ON "AIS_Data"
go



ALTER TABLE INCUR_LOS_REIM_FUND_FRMLA
	ADD CONSTRAINT PK_INCUR_LOS_REIM_FUND_FRMLA PRIMARY KEY  CLUSTERED (incur_los_reim_fund_frmla_id ASC,prem_adj_pgm_id ASC,custmr_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX INCUR_LOS_REIM_FUND_FRMLA_NN1 ON INCUR_LOS_REIM_FUND_FRMLA
( 
	prem_adj_pgm_id       ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX INCUR_LOS_REIM_FUND_FRMLA_NN2 ON INCUR_LOS_REIM_FUND_FRMLA
( 
	los_reim_fund_fctr_typ_id  ASC
)
ON "AIS_IDX"
go



CREATE TABLE INT_ORG
( 
	int_org_id           integer IDENTITY ( 1,1 ) ,
	full_name            varchar(50)  NULL ,
	bsn_unt_cd           char(3)  NULL ,
	city_nm              varchar(50)  NULL ,
	ofc_cd               char(3)  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL
	CONSTRAINT CURRENT_TIMESTAMP_1671744897
		 DEFAULT  CURRENT_TIMESTAMP
)
ON "AIS_Data"
go



ALTER TABLE INT_ORG
	ADD CONSTRAINT PK_INT_ORG PRIMARY KEY  CLUSTERED (int_org_id ASC)
	 ON "AIS_Data"
go



CREATE TABLE INVC_EXHIBIT_SETUP
( 
	invc_exhibit_setup_id integer IDENTITY ( 1,1 ) ,
	atch_cd              char(6)  NULL ,
	atch_nm              varchar(50)  NULL ,
	sts_ind              bit  NULL ,
	intrnl_flag_cd       char(1)  NULL ,
	seq_nbr              integer  NULL ,
	cesar_cd_ind         bit  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_1094767455
		 DEFAULT  CURRENT_TIMESTAMP
)
ON "AIS_Data"
go



ALTER TABLE INVC_EXHIBIT_SETUP
	ADD CONSTRAINT PK_INVC_EXHIBIT_SETUP PRIMARY KEY  CLUSTERED (invc_exhibit_setup_id ASC)
	 ON "AIS_Data"
go



CREATE TABLE KY_OR_SETUP
( 
	ky_or_setup_id       integer IDENTITY ( 1,1 ) ,
	eff_dt               datetime  NULL ,
	ky_fctr_rt           decimal(15,8)  NULL ,
	or_fctr_rt           decimal(15,8)  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_1728169240
		 DEFAULT  CURRENT_TIMESTAMP
)
ON "AIS_Data"
go



ALTER TABLE KY_OR_SETUP
	ADD CONSTRAINT PK_KY_OR_SETUP PRIMARY KEY  CLUSTERED (ky_or_setup_id ASC)
	 ON "AIS_Data"
go



CREATE TABLE LKUP
( 
	lkup_id              integer IDENTITY ( 1,1 ) ,
	lkup_typ_id          integer  NOT NULL ,
	lkup_txt             varchar(256)  NULL ,
	attr_1_txt           varchar(256)  NULL ,
	eff_dt               datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_246715845
		 DEFAULT  CURRENT_TIMESTAMP,
	expi_dt              datetime  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_1347912246
		 DEFAULT  CURRENT_TIMESTAMP,
	actv_ind             bit  NULL 
)
ON "AIS_Data"
go



ALTER TABLE LKUP
	ADD CONSTRAINT PK_LKUP PRIMARY KEY  CLUSTERED (lkup_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX LKUP_NN1 ON LKUP
( 
	lkup_typ_id           ASC
)
ON "AIS_IDX"
go



CREATE TABLE LKUP_TYP
( 
	lkup_typ_id          integer IDENTITY ( 1,1 ) ,
	lkup_typ_nm_txt      varchar(50)  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_1347691642
		 DEFAULT  CURRENT_TIMESTAMP,
	actv_ind             bit  NULL 
)
ON "AIS_Data"
go



ALTER TABLE LKUP_TYP
	ADD CONSTRAINT PK_LKUP_TYP PRIMARY KEY  CLUSTERED (lkup_typ_id ASC)
	 ON "AIS_Data"
go



CREATE TABLE LSI_CUSTMR
( 
	lsi_custmr_id        integer IDENTITY ( 1,1 ) ,
	custmr_id            integer  NOT NULL ,
	lsi_acct_id          integer  NOT NULL ,
	full_nm              varchar(100)  NULL ,
	prim_ind             bit  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_1598027296
		 DEFAULT  CURRENT_TIMESTAMP,
	actv_ind             bit  NULL 
)
ON "AIS_Data"
go



ALTER TABLE LSI_CUSTMR
	ADD CONSTRAINT PK_LSI_CUSTMR PRIMARY KEY  CLUSTERED (lsi_custmr_id ASC,custmr_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX LSI_CUSTMR_NN1 ON LSI_CUSTMR
( 
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE TABLE MSTR_ERND_RETRO_PREM_FRMLA
( 
	mstr_ernd_retro_prem_frmla_id integer IDENTITY ( 1,1 ) ,
	ernd_retro_prem_frmla_one_txt varchar(256)  NULL ,
	ernd_retro_prem_frmla_two_txt varchar(256)  NULL ,
	ernd_retro_prem_frmla_desc_txt varchar(256)  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_471044599
		 DEFAULT  CURRENT_TIMESTAMP,
	actv_ind             bit  NULL 
)
ON "AIS_Data"
go



ALTER TABLE MSTR_ERND_RETRO_PREM_FRMLA
	ADD CONSTRAINT PK_MSTR_ERND_RETRO_PREM_FRMLA PRIMARY KEY  CLUSTERED (mstr_ernd_retro_prem_frmla_id ASC)
	 ON "AIS_Data"
go



CREATE TABLE NON_SUBJ_PREM_AUDT
( 
	non_subj_prem_audt_id integer IDENTITY ( 1,1 ) ,
	coml_agmt_audt_id    integer  NOT NULL ,
	coml_agmt_id         integer  NOT NULL ,
	prem_adj_pgm_id      integer  NOT NULL ,
	custmr_id            integer  NOT NULL ,
	nsa_typ_id           integer  NOT NULL ,
	non_subj_audt_prem_amt decimal(15,2)  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_676380558
		 DEFAULT  CURRENT_TIMESTAMP,
	actv_ind             bit  NULL 
)
ON "AIS_Data"
go



ALTER TABLE NON_SUBJ_PREM_AUDT
	ADD CONSTRAINT PK_NON_SUBJ_PREM_AUDT PRIMARY KEY  CLUSTERED (non_subj_prem_audt_id ASC,coml_agmt_audt_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX NON_SUBJ_PREM_AUDT_NN2 ON NON_SUBJ_PREM_AUDT
( 
	coml_agmt_audt_id     ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX NON_SUBJ_PREM_AUDT_NN1 ON NON_SUBJ_PREM_AUDT
( 
	nsa_typ_id            ASC
)
ON "AIS_IDX"
go



CREATE TABLE NONAIS_CUSTMR
( 
	nonais_custmr_id     integer IDENTITY ( 1,1 ) ,
	full_nm              varchar(100)  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_99746012
		 DEFAULT  CURRENT_TIMESTAMP
)
ON "AIS_Data"
go



ALTER TABLE NONAIS_CUSTMR
	ADD CONSTRAINT PK_NONAIS_CUSTMR PRIMARY KEY  CLUSTERED (nonais_custmr_id ASC)
	 ON "AIS_Data"
go



CREATE TABLE PERS
( 
	pers_id              integer IDENTITY ( 1,1 ) ,
	mgr_id               integer  NULL ,
	external_reference   varchar(20)  NULL ,
	forename             varchar(35)  NOT NULL ,
	surname              varchar(35)  NOT NULL ,
	prefx_ttl_id         integer  NULL ,
	phone_nbr_1_txt      char(10)  NULL ,
	phone_nbr_2_txt      char(10)  NULL ,
	fax_nbr_txt          char(10)  NULL ,
	email_txt            varchar(100)  NULL ,
	conctc_typ_id        integer  NULL ,
	acct_qlty_cntrl_pct_id integer  NULL ,
	adj_qlty_cntrl_pct_id integer  NULL ,
	aries_qlty_cntrl_pct_id integer  NULL ,
	extrnl_org_id        integer  NULL ,
	eff_dt               datetime  NULL ,
	expi_dt              datetime  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_1414627129
		 DEFAULT  CURRENT_TIMESTAMP,
	actv_ind             bit  NULL 
)
ON "AIS_Data"
go



ALTER TABLE PERS
	ADD CONSTRAINT PK_PERS PRIMARY KEY  CLUSTERED (pers_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX PERS_NN1 ON PERS
( 
	extrnl_org_id         ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PERS_NN2 ON PERS
( 
	mgr_id                ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PERS_NN6 ON PERS
( 
	prefx_ttl_id          ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PERS_NN3 ON PERS
( 
	conctc_typ_id         ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PERS_NN4 ON PERS
( 
	acct_qlty_cntrl_pct_id  ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PERS_NN5 ON PERS
( 
	adj_qlty_cntrl_pct_id  ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PERS_NN7 ON PERS
( 
	aries_qlty_cntrl_pct_id  ASC
)
ON "AIS_IDX"
go



CREATE TABLE POST_ADDR
( 
	post_addr_id         integer IDENTITY ( 1,1 ) ,
	pers_id              integer  NOT NULL ,
	addr_ln_1_txt        varchar(50)  NOT NULL ,
	addr_ln_2_txt        varchar(50)  NULL ,
	city_txt             varchar(50)  NOT NULL ,
	post_cd_txt          varchar(20)  NOT NULL ,
	st_id                integer  NOT NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_1801206297
		 DEFAULT  CURRENT_TIMESTAMP
)
ON "AIS_Data"
go



ALTER TABLE POST_ADDR
	ADD CONSTRAINT PK_POST_ADDR PRIMARY KEY  CLUSTERED (post_addr_id ASC,pers_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX POST_ADDR_NN2 ON POST_ADDR
( 
	st_id                 ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX POST_ADDR_NN1 ON POST_ADDR
( 
	pers_id               ASC
)
ON "AIS_IDX"
go



CREATE TABLE POST_TRNS_TYP
( 
	post_trns_typ_id     integer IDENTITY ( 1,1 ) ,
	trns_typ_id          integer  NULL ,
	trns_nm_txt          varchar(256)  NULL ,
	main_nbr_txt         char(5)  NULL ,
	sub_nbr_txt          char(5)  NULL ,
	comp_txt             char(5)  NULL ,
	invoicbl_ind         bit  NULL ,
	post_ind             bit  NULL ,
	thrd_pty_admin_mnl_ind bit  NULL ,
	adj_sumry_ind        bit  NULL ,
	pol_reqr_ind         bit  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_117844281
		 DEFAULT  CURRENT_TIMESTAMP,
	actv_ind             bit  NULL 
)
ON "AIS_Data"
go



ALTER TABLE POST_TRNS_TYP
	ADD CONSTRAINT PK_POST_TRNS_TYP PRIMARY KEY  CLUSTERED (post_trns_typ_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX POST_TRNS_TYP_NN1 ON POST_TRNS_TYP
( 
	trns_typ_id           ASC
)
ON "AIS_IDX"
go



CREATE TABLE PREM_ADJ
( 
	prem_adj_id          integer IDENTITY ( 1,1 ) ,
	reg_custmr_id        integer  NOT NULL ,
	rel_prem_adj_id      integer  NULL ,
	valn_dt              datetime  NOT NULL ,
	drft_invc_nbr_txt    varchar(20)  NULL ,
	drft_invc_dt         datetime  NULL ,
	drft_mailed_undrwrt_dt datetime  NULL ,
	drft_intrnl_pdf_zdw_key_txt varchar(50)  NULL ,
	drft_extrnl_pdf_zdw_key_txt varchar(50)  NULL ,
	drft_cd_wrksht_pdf_zdw_key_txt varchar(50)  NULL ,
	fnl_invc_nbr_txt     varchar(20)  NULL ,
	fnl_invc_dt          datetime  NULL ,
	fnl_mailed_undrwrt_dt datetime  NULL ,
	fnl_intrnl_pdf_zdw_key_txt varchar(50)  NULL ,
	fnl_extrnl_pdf_zdw_key_txt varchar(50)  NULL ,
	fnl_cd_wrksht_pdf_zdw_key_txt varchar(50)  NULL ,
	fnl_mailed_brkr_dt   datetime  NULL ,
	undrwrt_not_reqr_ind bit  NULL ,
	invc_due_dt          datetime  NULL ,
	historical_adj_ind   bit  NULL ,
	twenty_pct_qlty_cntrl_reqr_ind bit  NULL ,
	twenty_pct_qlty_cntrl_ind bit  NULL ,
	twenty_pct_qlty_cntrl_pers_id integer  NULL ,
	twenty_pct_qlty_cntrl_dt datetime  NULL ,
	calc_adj_sts_cd      char(3)  NULL ,
	adj_pendg_ind        bit  NULL ,
	adj_pendg_rsn_id     integer  NULL ,
	adj_rrsn_rsn_id      integer  NULL ,
	adj_void_rsn_id      integer  NULL ,
	adj_can_ind          bit  NULL ,
	adj_void_ind         bit  NULL ,
	adj_rrsn_ind         bit  NULL ,
	void_rrsn_cmmnt_txt  varchar(256)  NULL ,
	brkr_id              integer  NULL ,
	bu_office_id         integer  NULL ,
	adj_sts_typ_id       integer  NULL ,
	adj_sts_eff_dt       datetime  NULL ,
	reconciler_revw_ind  bit  NULL ,
	adj_qc_ind           bit  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_1281378691
		 DEFAULT  CURRENT_TIMESTAMP
)
ON "AIS_Data"
go



ALTER TABLE PREM_ADJ
	ADD CONSTRAINT PK_PREM_ADJ PRIMARY KEY  CLUSTERED (prem_adj_id ASC,reg_custmr_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_NN1 ON PREM_ADJ
( 
	twenty_pct_qlty_cntrl_pers_id  ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_NN2 ON PREM_ADJ
( 
	adj_pendg_rsn_id      ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_NN3 ON PREM_ADJ
( 
	adj_void_rsn_id       ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_NN4 ON PREM_ADJ
( 
	adj_rrsn_rsn_id       ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_NN5 ON PREM_ADJ
( 
	rel_prem_adj_id       ASC,
	reg_custmr_id         ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_NN6 ON PREM_ADJ
( 
	reg_custmr_id         ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_NN7 ON PREM_ADJ
( 
	brkr_id               ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_NN8 ON PREM_ADJ
( 
	bu_office_id          ASC
)
ON "AIS_IDX"
go



CREATE TABLE PREM_ADJ_ARIES_CLRING
( 
	prem_adj_aries_clring_id integer IDENTITY ( 1,1 ) ,
	prem_adj_id          integer  NOT NULL ,
	custmr_id            integer  NOT NULL ,
	qlty_cntrl_pers_id   integer  NOT NULL ,
	recon_due_dt         datetime  NULL ,
	recon_dt             datetime  NULL ,
	qlty_cntrl_dt        datetime  NULL ,
	aries_post_dt        datetime  NULL ,
	chk_nbr_txt          varchar(20)  NULL ,
	aries_paymt_amt      decimal(15,2)  NULL ,
	biled_itm_clring_dt  datetime  NULL ,
	qlty_cntrl_ind       bit  NULL ,
	cmmnt_txt            varchar(4096)  NULL ,
	aries_cmplt_ind      bit  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_2123407309
		 DEFAULT  CURRENT_TIMESTAMP
)
ON "AIS_Data"
go



ALTER TABLE PREM_ADJ_ARIES_CLRING
	ADD CONSTRAINT PK_PREM_ADJ_ARIES_CLRING PRIMARY KEY  CLUSTERED (prem_adj_aries_clring_id ASC,prem_adj_id ASC,custmr_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_ARIES_CLRING_NN1 ON PREM_ADJ_ARIES_CLRING
( 
	qlty_cntrl_pers_id    ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_ARIES_CLRING_NN2 ON PREM_ADJ_ARIES_CLRING
( 
	prem_adj_id           ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE TABLE PREM_ADJ_CMMNT
( 
	prem_adj_cmmnt_id    integer IDENTITY ( 1,1 ) ,
	prem_adj_id          integer  NULL ,
	prem_adj_perd_id     integer  NULL ,
	reg_custmr_id        integer  NULL ,
	custmr_id            integer  NULL ,
	cmmnt_catg_id        integer  NULL ,
	cmmnt_txt            varchar(512)  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_621704688
		 DEFAULT  CURRENT_TIMESTAMP
)
ON "AIS_Data"
go



ALTER TABLE PREM_ADJ_CMMNT
	ADD CONSTRAINT PK_PREM_ADJ_CMMNT PRIMARY KEY  CLUSTERED (prem_adj_cmmnt_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_CMMNT_NN1 ON PREM_ADJ_CMMNT
( 
	cmmnt_catg_id         ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_CMMNT_NN2 ON PREM_ADJ_CMMNT
( 
	prem_adj_id           ASC,
	reg_custmr_id         ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_CMMNT_NN3 ON PREM_ADJ_CMMNT
( 
	prem_adj_perd_id      ASC,
	prem_adj_id           ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE TABLE PREM_ADJ_COMB_ELEMTS
( 
	prem_adj_comb_elemts_id integer IDENTITY ( 1,1 ) ,
	prem_adj_perd_id     integer  NOT NULL ,
	prem_adj_id          integer  NOT NULL ,
	custmr_id            integer  NOT NULL ,
	retro_basic_prem_amt decimal(15,2)  NULL ,
	retro_los_fctr_amt   decimal(15,2)  NULL ,
	retro_tax_multi_rt   decimal(15,8)  NULL ,
	retro_subtot_amt     decimal(15,2)  NULL ,
	dedtbl_max_amt       decimal(15,2)  NULL ,
	dedtbl_min_amt       decimal(15,2)  NULL ,
	dedtbl_subtot_amt    decimal(15,2)  NULL ,
	dedtbl_max_less_amt  decimal(15,2)  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_1701863574
		 DEFAULT  CURRENT_TIMESTAMP
)
ON "AIS_Data"
go



ALTER TABLE PREM_ADJ_COMB_ELEMTS
	ADD CONSTRAINT PK_PREM_ADJ_COMB_ELEMTS PRIMARY KEY  CLUSTERED (prem_adj_comb_elemts_id ASC,prem_adj_perd_id ASC,prem_adj_id ASC,custmr_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_COMB_ELEMTS_NN1 ON PREM_ADJ_COMB_ELEMTS
( 
	prem_adj_perd_id      ASC,
	prem_adj_id           ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE TABLE PREM_ADJ_LOS_REIM_FUND_POST
( 
	prem_adj_los_reim_fund_post_id integer IDENTITY ( 1,1 ) ,
	prem_adj_perd_id     integer  NOT NULL ,
	prem_adj_id          integer  NOT NULL ,
	custmr_id            integer  NOT NULL ,
	recv_typ_id          integer  NULL ,
	curr_amt             decimal(15,2)  NULL ,
	aggr_amt             decimal(15,2)  NULL ,
	lim_amt              decimal(15,2)  NULL ,
	prior_yy_amt         decimal(15,2)  NULL ,
	adj_prior_yy_amt     decimal(15,2)  NULL ,
	post_amt             decimal(15,2)  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_563784195
		 DEFAULT  CURRENT_TIMESTAMP,
	crte_user_id         integer  NOT NULL 
)
ON "AIS_Data"
go



ALTER TABLE PREM_ADJ_LOS_REIM_FUND_POST
	ADD CONSTRAINT PK_PREM_ADJ_LOS_REIM_FUND_PST PRIMARY KEY  CLUSTERED (prem_adj_los_reim_fund_post_id ASC,prem_adj_perd_id ASC,prem_adj_id ASC,custmr_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_LOS_REIM_FUND_PST_NN2 ON PREM_ADJ_LOS_REIM_FUND_POST
( 
	prem_adj_perd_id      ASC,
	prem_adj_id           ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_LOS_REIM_FUND_PST_NN1 ON PREM_ADJ_LOS_REIM_FUND_POST
( 
	recv_typ_id           ASC
)
ON "AIS_IDX"
go



CREATE TABLE PREM_ADJ_MISC_INVC
( 
	prem_adj_misc_invc_id integer IDENTITY ( 1,1 ) ,
	prem_adj_id          integer  NOT NULL ,
	prem_adj_perd_id     integer  NOT NULL ,
	custmr_id            integer  NOT NULL ,
	post_amt             decimal(15,2)  NULL ,
	pol_sym_txt          char(3)  NULL ,
	pol_nbr_txt          char(15)  NULL ,
	pol_modulus_txt      char(2)  NULL ,
	post_trns_typ_id     integer  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_541447529
		 DEFAULT  CURRENT_TIMESTAMP,
	actv_ind             bit  NULL 
)
ON "AIS_Data"
go



ALTER TABLE PREM_ADJ_MISC_INVC
	ADD CONSTRAINT PK_PREM_ADJ_MISC_INVC PRIMARY KEY  CLUSTERED (prem_adj_misc_invc_id ASC,prem_adj_id ASC,prem_adj_perd_id ASC,custmr_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_MISC_INVC_NN1 ON PREM_ADJ_MISC_INVC
( 
	prem_adj_perd_id      ASC,
	prem_adj_id           ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_MISC_INVC_NN2 ON PREM_ADJ_MISC_INVC
( 
	post_trns_typ_id      ASC
)
ON "AIS_IDX"
go



CREATE TABLE PREM_ADJ_NY_SCND_INJR_FUND
( 
	prem_adj_ny_scnd_injr_fund_id integer IDENTITY ( 1,1 ) ,
	prem_adj_perd_id     integer  NOT NULL ,
	prem_adj_id          integer  NOT NULL ,
	custmr_id            integer  NOT NULL ,
	coml_agmt_id         integer  NULL ,
	prem_adj_pgm_id      integer  NULL ,
	incur_los_amt        decimal(15,2)  NULL ,
	los_conv_fctr_rt     decimal(15,8)  NULL ,
	cnvt_los_amt         decimal(15,2)  NULL ,
	basic_dedtbl_prem_amt decimal(15,2)  NULL ,
	tax_multi_rt         decimal(15,8)  NULL ,
	cnvt_tot_los_amt     decimal(15,2)  NULL ,
	ny_prem_disc_amt     decimal(15,2)  NULL ,
	ny_scnd_injr_fund_rt decimal(15,8)  NULL ,
	revd_ny_scnd_injr_fund_amt decimal(15,2)  NULL ,
	ny_tax_due_amt       decimal(15,2)  NULL ,
	prev_rslt_amt        decimal(15,2)  NULL ,
	ny_scnd_injr_fund_audt_amt decimal(15,2)  NULL ,
	curr_adj_amt         decimal(15,2)  NULL ,
	basic_cnvt_los_amt   decimal(15,2)  NULL ,
	ny_ernd_retro_prem_amt decimal(15,2)  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_907571465
		 DEFAULT  CURRENT_TIMESTAMP
)
ON "AIS_Data"
go



ALTER TABLE PREM_ADJ_NY_SCND_INJR_FUND
	ADD CONSTRAINT PK_PREM_ADJ_NY_SCND_INJR_FUND PRIMARY KEY  CLUSTERED (prem_adj_ny_scnd_injr_fund_id ASC,prem_adj_perd_id ASC,prem_adj_id ASC,custmr_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_NY_SCND_INJR_FUND_NN1 ON PREM_ADJ_NY_SCND_INJR_FUND
( 
	prem_adj_perd_id      ASC,
	prem_adj_id           ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_NY_SCND_INJR_FUND_NN2 ON PREM_ADJ_NY_SCND_INJR_FUND
( 
	coml_agmt_id          ASC,
	prem_adj_pgm_id       ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE TABLE PREM_ADJ_PAID_LOS_BIL
( 
	prem_adj_paid_los_bil_id integer IDENTITY ( 1,1 ) ,
	prem_adj_perd_id     integer  NULL ,
	prem_adj_id          integer  NULL ,
	custmr_id            integer  NULL ,
	lsi_pgm_typ_txt      varchar(30)  NULL ,
	lsi_src              bit  NULL ,
	lsi_valn_dt          datetime  NULL ,
	idnmty_amt           decimal(15,2)  NULL ,
	adj_idnmty_amt       decimal(15,2)  NULL ,
	exps_amt             decimal(15,2)  NULL ,
	adj_exps_amt         decimal(15,2)  NULL ,
	tot_paid_los_bil_amt decimal(15,2)  NULL ,
	adj_tot_paid_los_bil_amt decimal(15,2)  NULL ,
	ln_of_bsn_id         integer  NULL ,
	coml_agmt_id         integer  NULL ,
	cmmnt_txt            varchar(4096)  NULL ,
	prem_adj_pgm_id      integer  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_1503287592
		 DEFAULT  CURRENT_TIMESTAMP
)
ON "AIS_Data"
go



ALTER TABLE PREM_ADJ_PAID_LOS_BIL
	ADD CONSTRAINT PK_PREM_ADJ_PAID_LOS_BIL PRIMARY KEY  CLUSTERED (prem_adj_paid_los_bil_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PAID_LOS_BIL_NN2 ON PREM_ADJ_PAID_LOS_BIL
( 
	ln_of_bsn_id          ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PAID_LOS_BIL_NN3 ON PREM_ADJ_PAID_LOS_BIL
( 
	coml_agmt_id          ASC,
	prem_adj_pgm_id       ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PAID_LOS_BIL_NN1 ON PREM_ADJ_PAID_LOS_BIL
( 
	prem_adj_perd_id      ASC,
	prem_adj_id           ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE TABLE PREM_ADJ_PARMET_DTL
( 
	prem_adj_parmet_dtl_id integer IDENTITY ( 1,1 ) ,
	prem_adj_parmet_setup_id integer  NOT NULL ,
	prem_adj_perd_id     integer  NOT NULL ,
	prem_adj_id          integer  NOT NULL ,
	custmr_id            integer  NOT NULL ,
	st_id                integer  NULL ,
	ln_of_bsn_id         integer  NULL ,
	clm_hndl_fee_los_typ_id integer  NULL ,
	los_amt              decimal(15,2)  NULL ,
	los_base_asses_rt    decimal(15,8)  NULL ,
	tot_amt              decimal(15,2)  NULL ,
	paid_los_amt         decimal(15,2)  NULL ,
	paid_aloc_los_adj_exps_amt decimal(15,2)  NULL ,
	resrv_los_amt        decimal(15,2)  NULL ,
	resrv_aloc_los_adj_exps_amt decimal(15,2)  NULL ,
	los_dev_fctr_amt     decimal(15,2)  NULL ,
	los_dev_fctr_rt      decimal(15,8)  NULL ,
	los_base_asses_amt   decimal(15,2)  NULL ,
	los_conv_fctr_rt     decimal(15,8)  NULL ,
	los_conv_fctr_amt    decimal(15,2)  NULL ,
	coml_agmt_id         integer  NOT NULL ,
	prem_adj_pgm_id      integer  NOT NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_1303161425
		 DEFAULT  CURRENT_TIMESTAMP
)
ON "AIS_Data"
go



ALTER TABLE PREM_ADJ_PARMET_DTL
	ADD CONSTRAINT PK_PREM_ADJ_PARMET_DTL PRIMARY KEY  CLUSTERED (prem_adj_parmet_dtl_id ASC,prem_adj_parmet_setup_id ASC,prem_adj_perd_id ASC,prem_adj_id ASC,custmr_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PARMET_DTL_NN1 ON PREM_ADJ_PARMET_DTL
( 
	prem_adj_parmet_setup_id  ASC,
	prem_adj_perd_id      ASC,
	prem_adj_id           ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PARMET_DTL_NN2 ON PREM_ADJ_PARMET_DTL
( 
	ln_of_bsn_id          ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PARMET_DTL_NN3 ON PREM_ADJ_PARMET_DTL
( 
	st_id                 ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PARMET_DTL_NN4 ON PREM_ADJ_PARMET_DTL
( 
	clm_hndl_fee_los_typ_id  ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PARMET_DTL_NN5 ON PREM_ADJ_PARMET_DTL
( 
	coml_agmt_id          ASC,
	prem_adj_pgm_id       ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE TABLE PREM_ADJ_PARMET_SETUP
( 
	prem_adj_parmet_setup_id integer IDENTITY ( 1,1 ) ,
	prem_adj_perd_id     integer  NOT NULL ,
	prem_adj_id          integer  NOT NULL ,
	custmr_id            integer  NOT NULL ,
	prem_adj_pgm_setup_id integer  NOT NULL ,
	prem_adj_pgm_id      integer  NOT NULL ,
	los_base_asses_amt   decimal(15,2)  NULL ,
	los_base_asses_depst_amt decimal(15,2)  NULL ,
	los_base_asses_prev_biled_amt decimal(15,2)  NULL ,
	escr_adj_paid_los_amt decimal(15,2)  NULL ,
	escr_prevly_biled_amt decimal(15,2)  NULL ,
	escr_amt             decimal(15,2)  NULL ,
	escr_adj_amt         decimal(15,2)  NULL ,
	tot_amt              decimal(15,2)  NULL ,
	incur_los_reim_fund_amt decimal(15,2)  NULL ,
	incur_los_reim_fund_lim_amt decimal(15,2)  NULL ,
	incur_los_reim_fund_prevly_biled_amt decimal(15,2)  NULL ,
	clm_hndl_fee_prev_biled_amt decimal(15,2)  NULL ,
	adj_parmet_typ_id    integer  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_1920977893
		 DEFAULT  CURRENT_TIMESTAMP
)
ON "AIS_Data"
go



ALTER TABLE PREM_ADJ_PARMET_SETUP
	ADD CONSTRAINT PK_PREM_ADJ_PARMET_SETUP PRIMARY KEY  CLUSTERED (prem_adj_parmet_setup_id ASC,prem_adj_perd_id ASC,prem_adj_id ASC,custmr_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PARMET_SETUP_NN1 ON PREM_ADJ_PARMET_SETUP
( 
	prem_adj_perd_id      ASC,
	prem_adj_id           ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PARMET_SETUP_NN2 ON PREM_ADJ_PARMET_SETUP
( 
	adj_parmet_typ_id     ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PARMET_SETUP_NN4 ON PREM_ADJ_PARMET_SETUP
( 
	prem_adj_pgm_setup_id  ASC,
	prem_adj_pgm_id       ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE TABLE PREM_ADJ_PERD
( 
	prem_adj_perd_id     integer IDENTITY ( 1,1 ) ,
	prem_adj_id          integer  NOT NULL ,
	custmr_id            integer  NOT NULL ,
	reg_custmr_id        integer  NOT NULL ,
	prem_adj_pgm_id      integer  NOT NULL ,
	prem_non_prem_cd     char(2)  NULL ,
	adj_nbr              integer  NULL ,
	adj_nbr_txt          varchar(50)  NULL ,
	ernd_retro_prem_min_amt decimal(15,2)  NULL ,
	ernd_retro_prem_max_amt decimal(15,2)  NULL ,
	ernd_retro_prem_min_max_cd char(3)  NULL ,
	ernd_retro_prem_amt  decimal(15,2)  NULL ,
	ernd_retro_prem_unlim_ind bit  NULL ,
	adj_nbr_mnl_overrid_ind bit  NULL ,
	escr_mnl_overrid_ind bit  NULL ,
	escr_tot_amt         decimal(15,2)  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_405072072
		 DEFAULT  CURRENT_TIMESTAMP
)
ON "AIS_Data"
go



ALTER TABLE PREM_ADJ_PERD
	ADD CONSTRAINT PK_PREM_ADJ_PERD PRIMARY KEY  CLUSTERED (prem_adj_perd_id ASC,prem_adj_id ASC,custmr_id ASC)
	 ON "AIS_Data"
go



CREATE INDEX PREM_ADJ_PERD_NN1 ON PREM_ADJ_PERD
( 
	prem_adj_id           ASC,
	reg_custmr_id         ASC
)
ON "AIS_IDX"
go



CREATE INDEX PREM_ADJ_PERD_NN2 ON PREM_ADJ_PERD
( 
	prem_adj_pgm_id       ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE TABLE PREM_ADJ_PERD_TOT
( 
	prem_adj_perd_tot_id integer IDENTITY ( 1,1 ) ,
	prem_adj_perd_id     integer  NOT NULL ,
	prem_adj_id          integer  NOT NULL ,
	custmr_id            integer  NOT NULL ,
	invc_adj_typ_txt     varchar(256)  NULL ,
	tot_amt              decimal(15,2)  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_1194288781
		 DEFAULT  CURRENT_TIMESTAMP,
	updt_dt              datetime  NULL ,
	updt_user_id         integer  NULL 
)
ON "AIS_Data"
go



ALTER TABLE PREM_ADJ_PERD_TOT
	ADD CONSTRAINT PK_PREM_ADJ_PERD_TOT PRIMARY KEY  CLUSTERED (prem_adj_perd_tot_id ASC,prem_adj_perd_id ASC,prem_adj_id ASC,custmr_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PERD_TOT_NN1 ON PREM_ADJ_PERD_TOT
( 
	prem_adj_perd_id      ASC,
	prem_adj_id           ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PERD_TOT_NN2 ON PREM_ADJ_PERD_TOT
( 
	custmr_id             ASC
)
INCLUDE( prem_adj_id,prem_adj_perd_id )
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PERD_TOT_NN3 ON PREM_ADJ_PERD_TOT
( 
	prem_adj_perd_id      ASC
)
INCLUDE( prem_adj_id,custmr_id,tot_amt )
ON "AIS_IDX"
go



CREATE TABLE PREM_ADJ_PGM
( 
	prem_adj_pgm_id      integer IDENTITY ( 1,1 ) ,
	custmr_id            integer  NOT NULL ,
	brkr_conctc_id       integer  NULL ,
	mstr_ernd_retro_prem_frmla_id integer  NULL ,
	strt_dt              datetime  NULL ,
	plan_end_dt          datetime  NULL ,
	valn_mm_dt           datetime  NULL ,
	incur_conv_mms_cnt   integer  NULL ,
	adj_freq_mms_intvrl_cnt integer  NULL ,
	incld_captv_payknd_ind bit  NULL ,
	avg_tax_multi_ind    bit  NULL ,
	tax_multi_fctr_rt    decimal(15,8)  NULL ,
	los_sens_info_strt_dt datetime  NULL ,
	los_sens_info_end_dt datetime  NULL ,
	lsi_retrieve_from_dt datetime  NULL ,
	fst_adj_mms_from_incp_cnt integer  NULL ,
	fnl_adj_dt           datetime  NULL ,
	brkr_id              integer  NULL ,
	nxt_valn_dt          datetime  NULL ,
	prev_valn_dt         datetime  NULL ,
	comb_elemts_max_amt  decimal(15,2)  NULL ,
	peo_pay_in_amt       decimal(15,2)  NULL ,
	fst_adj_non_prem_mms_cnt integer  NULL ,
	freq_non_prem_mms_cnt integer  NULL ,
	fst_adj_non_prem_dt  datetime  NULL ,
	fnl_adj_non_prem_dt  datetime  NULL ,
	nxt_valn_dt_non_prem_dt datetime  NULL ,
	prev_valn_dt_non_prem_dt datetime  NULL ,
	zna_serv_comp_clm_hndl_fee_ind bit  NULL ,
	pgm_typ_id           integer  NULL ,
	bnkrpt_buyout_id     integer  NULL ,
	bnkrpt_buyout_eff_dt datetime  NULL ,
	paid_incur_typ_id    integer  NULL ,
	agmt_aloc_los_adj_exps_ind bit  NULL ,
	agmt_unaloctd_los_adj_ind bit  NULL ,
	agmt_los_base_asses_ind bit  NULL ,
	lsi_aloc_los_adj_exps_ind bit  NULL ,
	lsi_unaloctd_los_adj_ind bit  NULL ,
	lsi_los_base_asses_ind bit  NULL ,
	agmt_paid_incur_ind  bit  NULL ,
	qlty_cntrl_dt        datetime  NULL ,
	bsn_unt_ofc_id       integer  NULL ,
	qlty_cmmnt_txt       varchar(512)  NULL ,
	qlty_cntrl_pers_id   integer  NULL ,
	incld_legend_ind     bit  NULL ,
	curr_pgm_perd_adj_nbr integer  NULL ,
	fst_adj_dt           datetime  NULL ,
	prior_prem_adj_id    integer  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_317717963
		 DEFAULT  CURRENT_TIMESTAMP,
	actv_ind             bit  NULL 
)
ON "AIS_Data"
go



ALTER TABLE PREM_ADJ_PGM
	ADD CONSTRAINT PK_PREM_ADJ_PGM PRIMARY KEY  CLUSTERED (prem_adj_pgm_id ASC,custmr_id ASC)
	 ON "AIS_Data"
go



CREATE INDEX PREM_ADJ_PGM_NN7 ON PREM_ADJ_PGM
( 
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE INDEX PREM_ADJ_PGM_NN8 ON PREM_ADJ_PGM
( 
	brkr_id               ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PGM_NN10 ON PREM_ADJ_PGM
( 
	brkr_conctc_id        ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PGM_NN4 ON PREM_ADJ_PGM
( 
	mstr_ernd_retro_prem_frmla_id  ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PGM_NN6 ON PREM_ADJ_PGM
( 
	bsn_unt_ofc_id        ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PGM_NN9 ON PREM_ADJ_PGM
( 
	pgm_typ_id            ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PGM_NN1 ON PREM_ADJ_PGM
( 
	bnkrpt_buyout_id      ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PGM_NN2 ON PREM_ADJ_PGM
( 
	paid_incur_typ_id     ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PGM_NN11 ON PREM_ADJ_PGM
( 
	qlty_cntrl_pers_id    ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PGM_NN14 ON PREM_ADJ_PGM
( 
	prior_prem_adj_id     ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PGM_NN12 ON PREM_ADJ_PGM
( 
	strt_dt               ASC
)
INCLUDE( brkr_conctc_id )
ON "AIS_IDX"
go



CREATE TABLE PREM_ADJ_PGM_DTL
( 
	prem_adj_pgm_dtl_id  integer IDENTITY ( 1,1 ) ,
	prem_adj_pgm_setup_id integer  NOT NULL ,
	prem_adj_pgm_id      integer  NOT NULL ,
	custmr_id            integer  NOT NULL ,
	st_id                integer  NULL ,
	clm_hndl_fee_los_typ_id integer  NULL ,
	clm_hndl_fee_clmt_nbr decimal(15,2)  NULL ,
	clm_hndl_fee_clm_rt_nbr decimal(15,2)  NULL ,
	prem_asses_rt        decimal(15,8)  NULL ,
	adj_fctr_rt          decimal(15,8)  NULL ,
	cmmnt_txt            varchar(512)  NULL ,
	fnl_overrid_amt      decimal(15,2)  NULL ,
	ln_of_bsn_id         integer  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_1916031511
		 DEFAULT  CURRENT_TIMESTAMP,
	actv_ind             bit  NULL 
)
ON "AIS_Data"
go



ALTER TABLE PREM_ADJ_PGM_DTL
	ADD CONSTRAINT PK_PREM_ADJ_PGM_DTL PRIMARY KEY  CLUSTERED (prem_adj_pgm_dtl_id ASC,prem_adj_pgm_setup_id ASC,prem_adj_pgm_id ASC,custmr_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PGM_DTL_NN1 ON PREM_ADJ_PGM_DTL
( 
	prem_adj_pgm_setup_id  ASC,
	prem_adj_pgm_id       ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PGM_DTL_NN2 ON PREM_ADJ_PGM_DTL
( 
	clm_hndl_fee_los_typ_id  ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PGM_DTL_NN3 ON PREM_ADJ_PGM_DTL
( 
	st_id                 ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PGM_DTL_NN4 ON PREM_ADJ_PGM_DTL
( 
	ln_of_bsn_id          ASC
)
ON "AIS_IDX"
go



CREATE TABLE PREM_ADJ_PGM_PERS_REL
( 
	prem_adj_pgm_pers_rel_id integer IDENTITY ( 1,1 ) ,
	prem_adj_pgm_id      integer  NOT NULL ,
	custmr_id            integer  NOT NULL ,
	pers_id              integer  NOT NULL ,
	snd_invc_ind         bit  NULL ,
	commu_medum_id       integer  NOT NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_1753961997
		 DEFAULT  CURRENT_TIMESTAMP,
	actv_ind             bit  NULL 
)
ON "AIS_Data"
go



ALTER TABLE PREM_ADJ_PGM_PERS_REL
	ADD CONSTRAINT PK_PREM_ADJ_PERS_REL PRIMARY KEY  CLUSTERED (prem_adj_pgm_pers_rel_id ASC,prem_adj_pgm_id ASC,custmr_id ASC)
	 ON "AIS_Data"
go



CREATE INDEX PREM_ADJ_PERS_REL_NN1 ON PREM_ADJ_PGM_PERS_REL
( 
	prem_adj_pgm_id       ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE INDEX PREM_ADJ_PERS_REL_NN2 ON PREM_ADJ_PGM_PERS_REL
( 
	pers_id               ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PERS_REL_NN3 ON PREM_ADJ_PGM_PERS_REL
( 
	commu_medum_id        ASC
)
ON "AIS_IDX"
go



CREATE TABLE PREM_ADJ_PGM_RETRO
( 
	prem_adj_pgm_retro_id integer IDENTITY ( 1,1 ) ,
	prem_adj_pgm_id      integer  NOT NULL ,
	custmr_id            integer  NOT NULL ,
	retro_adj_fctr_aplcbl_ind bit  NULL ,
	no_lim_ind           bit  NULL ,
	expo_typ_id          integer  NULL ,
	expo_agmt_amt        decimal(15,2)  NULL ,
	retro_adj_fctr_rt    decimal(15,8)  NULL ,
	tot_agmt_amt         decimal(15,2)  NULL ,
	audt_expo_amt        decimal(15,2)  NULL ,
	aggr_fctr_pct        decimal(15,8)  NULL ,
	tot_audt_amt         decimal(15,2)  NULL ,
	retro_elemt_typ_id   integer  NULL ,
	expo_typ_incremnt_nbr_id integer  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_440790379
		 DEFAULT  CURRENT_TIMESTAMP,
	actv_ind             bit  NULL 
)
ON "AIS_Data"
go



ALTER TABLE PREM_ADJ_PGM_RETRO
	ADD CONSTRAINT PK_PREM_ADJ_PGM_RETRO PRIMARY KEY  NONCLUSTERED (prem_adj_pgm_retro_id ASC,prem_adj_pgm_id ASC,custmr_id ASC)
	 ON "AIS_Data"
go



CREATE INDEX PREM_ADJ_PGM_RETRO_NN1 ON PREM_ADJ_PGM_RETRO
( 
	prem_adj_pgm_id       ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PGM_RETRO_NN2 ON PREM_ADJ_PGM_RETRO
( 
	retro_elemt_typ_id    ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PGM_RETRO_NN3 ON PREM_ADJ_PGM_RETRO
( 
	expo_typ_incremnt_nbr_id  ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PGM_RETRO_NN4 ON PREM_ADJ_PGM_RETRO
( 
	expo_typ_id           ASC
)
ON "AIS_IDX"
go



CREATE TABLE PREM_ADJ_PGM_SETUP
( 
	prem_adj_pgm_setup_id integer IDENTITY ( 1,1 ) ,
	prem_adj_pgm_id      integer  NOT NULL ,
	custmr_id            integer  NOT NULL ,
	incld_ernd_retro_prem_ind bit  NULL ,
	depst_amt            decimal(15,2)  NULL ,
	clm_hndl_fee_basis_id integer  NULL ,
	incld_incur_but_not_rptd_ind bit  NULL ,
	los_base_asses_adj_typ_id integer  NULL ,
	adj_parmet_typ_id    integer  NULL ,
	escr_paid_los_bil_mms_nbr integer  NULL ,
	escr_dvsr_nbr        integer  NULL ,
	escr_mms_held_amt    decimal(15,2)  NULL ,
	los_conv_fctr_clm_cap_amt decimal(15,2)  NULL ,
	los_conv_fctr_aggr_cap_amt decimal(15,2)  NULL ,
	los_conv_fctr_lyr_insd_pays_amt decimal(15,2)  NULL ,
	los_conv_fctr_lyr_zna_pays_amt decimal(15,2)  NULL ,
	incur_but_not_rptd_los_dev_fctr_id integer  NULL ,
	incur_los_reim_fund_initl_fund_amt decimal(15,2)  NULL ,
	incur_los_reim_fund_unlim_agmt_lim_ind bit  NULL ,
	incur_los_reim_fund_aggr_lim_amt decimal(15,2)  NULL ,
	incur_los_reim_fund_unlim_minimium_lim_ind bit  NULL ,
	incur_los_reim_fund_min_lim_amt decimal(15,2)  NULL ,
	incur_los_reim_fund_invc_lsi_ind bit  NULL ,
	escr_prev_amt        decimal(15,2)  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_440791149
		 DEFAULT  CURRENT_TIMESTAMP,
	actv_ind             bit  NULL 
)
ON "AIS_Data"
go



ALTER TABLE PREM_ADJ_PGM_SETUP
	ADD CONSTRAINT PK_PREM_ADJ_PGM_SETUP PRIMARY KEY  CLUSTERED (prem_adj_pgm_setup_id ASC,prem_adj_pgm_id ASC,custmr_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PGM_SETUP_NN1 ON PREM_ADJ_PGM_SETUP
( 
	prem_adj_pgm_id       ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PGM_SETUP_NN2 ON PREM_ADJ_PGM_SETUP
( 
	clm_hndl_fee_basis_id  ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PGM_SETUP_NN3 ON PREM_ADJ_PGM_SETUP
( 
	los_base_asses_adj_typ_id  ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PGM_SETUP_NN4 ON PREM_ADJ_PGM_SETUP
( 
	adj_parmet_typ_id     ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PGM_SETUP_NN5 ON PREM_ADJ_PGM_SETUP
( 
	incur_but_not_rptd_los_dev_fctr_id  ASC
)
ON "AIS_IDX"
go



CREATE TABLE PREM_ADJ_PGM_SETUP_POL
( 
	prem_adj_pgm_setup_pol_id integer IDENTITY ( 1,1 ) ,
	prem_adj_pgm_setup_id integer  NOT NULL ,
	prem_adj_pgm_id      integer  NOT NULL ,
	custmr_id            integer  NOT NULL ,
	coml_agmt_id         integer  NOT NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_2039889849
		 DEFAULT  CURRENT_TIMESTAMP
)
ON "AIS_Data"
go



ALTER TABLE PREM_ADJ_PGM_SETUP_POL
	ADD CONSTRAINT PK_PREM_ADJ_PGM_POL PRIMARY KEY  CLUSTERED (prem_adj_pgm_setup_pol_id ASC,prem_adj_pgm_setup_id ASC,prem_adj_pgm_id ASC,custmr_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PGM_POL_NN1 ON PREM_ADJ_PGM_SETUP_POL
( 
	prem_adj_pgm_setup_id  ASC,
	prem_adj_pgm_id       ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PGM_POL_NN2 ON PREM_ADJ_PGM_SETUP_POL
( 
	coml_agmt_id          ASC,
	prem_adj_pgm_id       ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE TABLE PREM_ADJ_PGM_STS
( 
	prem_adj_pgm_sts_id  integer IDENTITY ( 1,1 ) ,
	prem_adj_pgm_id      integer  NOT NULL ,
	custmr_id            integer  NOT NULL ,
	pgm_perd_sts_typ_id  integer  NULL ,
	sts_chk_ind          bit  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_1917014558
		 DEFAULT  CURRENT_TIMESTAMP
)
ON "AIS_Data"
go



ALTER TABLE PREM_ADJ_PGM_STS
	ADD CONSTRAINT PK_PREM_ADJ_PGM_STS PRIMARY KEY  CLUSTERED (prem_adj_pgm_sts_id ASC,prem_adj_pgm_id ASC,custmr_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PGM_STS_NN1 ON PREM_ADJ_PGM_STS
( 
	pgm_perd_sts_typ_id   ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PGM_STS_NN2 ON PREM_ADJ_PGM_STS
( 
	prem_adj_pgm_id       ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_PGM_STS_NN3 ON PREM_ADJ_PGM_STS
( 
	custmr_id             ASC,
	pgm_perd_sts_typ_id   ASC
)
INCLUDE( prem_adj_pgm_sts_id )
ON "AIS_IDX"
go



CREATE TABLE PREM_ADJ_RETRO
( 
	prem_adj_retro_id    integer IDENTITY ( 1,1 ) ,
	prem_adj_perd_id     integer  NOT NULL ,
	prem_adj_id          integer  NOT NULL ,
	custmr_id            integer  NOT NULL ,
	coml_agmt_id         integer  NOT NULL ,
	prem_adj_pgm_id      integer  NOT NULL ,
	subj_depst_prem_amt  decimal(15,2)  NULL ,
	non_subj_audt_prem_amt decimal(15,2)  NULL ,
	non_subj_depst_prem_amt decimal(15,2)  NULL ,
	paid_los_bil_amt     decimal(15,2)  NULL ,
	prev_ernd_retro_prem_amt decimal(15,2)  NULL ,
	misc_amt             decimal(15,2)  NULL ,
	ky_tot_due_amt       decimal(15,2)  NULL ,
	or_tot_due_amt       decimal(15,2)  NULL ,
	rsdl_mkt_load_tot_amt decimal(15,2)  NULL ,
	invc_amt             decimal(15,2)  NULL ,
	peo_pay_in_amt       decimal(15,2)  NULL ,
	adj_cash_flw_ben_amt decimal(15,2)  NULL ,
	post_idnmty_amt      decimal(15,2)  NULL ,
	post_resrv_idnmty_amt decimal(15,2)  NULL ,
	aries_tot_amt        decimal(15,2)  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_755462630
		 DEFAULT  CURRENT_TIMESTAMP
)
ON "AIS_Data"
go



ALTER TABLE PREM_ADJ_RETRO
	ADD CONSTRAINT PK_PREM_ADJ_RETRO PRIMARY KEY  CLUSTERED (prem_adj_retro_id ASC,prem_adj_perd_id ASC,prem_adj_id ASC,custmr_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_RETRO_NN1 ON PREM_ADJ_RETRO
( 
	prem_adj_perd_id      ASC,
	prem_adj_id           ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_RETRO_NN2 ON PREM_ADJ_RETRO
( 
	coml_agmt_id          ASC,
	prem_adj_pgm_id       ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE TABLE PREM_ADJ_RETRO_DTL
( 
	prem_adj_retro_dtl_id integer IDENTITY ( 1,1 ) ,
	prem_adj_retro_id    integer  NOT NULL ,
	prem_adj_perd_id     integer  NOT NULL ,
	prem_adj_id          integer  NOT NULL ,
	custmr_id            integer  NOT NULL ,
	coml_agmt_id         integer  NOT NULL ,
	prem_adj_pgm_id      integer  NOT NULL ,
	st_id                integer  NULL ,
	ln_of_bsn_id         integer  NULL ,
	subj_paid_idnmty_amt decimal(15,2)  NULL ,
	subj_paid_exps_amt   decimal(15,2)  NULL ,
	subj_resrv_idnmty_amt decimal(15,2)  NULL ,
	subj_resrv_exps_amt  decimal(15,2)  NULL ,
	prev_subj_paid_idnmty_amt decimal(15,2)  NULL ,
	prev_subj_resrv_idnmty_amt decimal(15,2)  NULL ,
	adj_dedtbl_wrk_comp_los_amt decimal(15,2)  NULL ,
	basic_amt            decimal(15,2)  NULL ,
	clm_hndl_fee_amt     decimal(15,2)  NULL ,
	los_base_asessment_amt decimal(15,2)  NULL ,
	los_conv_fctr_amt    decimal(15,2)  NULL ,
	los_conv_fctr_rt     decimal(15,8)  NULL ,
	non_conv_fee_amt     decimal(15,2)  NULL ,
	prem_tax_amt         decimal(15,2)  NULL ,
	othr_amt             decimal(15,2)  NULL ,
	incur_ernd_retro_prem_amt decimal(15,2)  NULL ,
	adj_incur_ernd_retro_prem_amt decimal(15,2)  NULL ,
	paid_ernd_retro_prem_amt decimal(15,2)  NULL ,
	adj_paid_ernd_retro_prem_amt decimal(15,2)  NULL ,
	cash_flw_ben_amt     decimal(15,2)  NULL ,
	prior_cash_flw_ben_amt decimal(15,2)  NULL ,
	exc_los_prem_amt     decimal(15,2)  NULL ,
	los_dev_resrv_amt    decimal(15,2)  NULL ,
	std_subj_prem_amt    decimal(15,2)  NULL ,
	ernd_retro_prem_amt  decimal(15,2)  NULL ,
	ky_or_tax_asses_amt  decimal(15,2)  NULL ,
	ky_or_prev_tax_asses_amt decimal(15,2)  NULL ,
	rsdl_mkt_load_basic_fctr_rt decimal(15,8)  NULL ,
	rsdl_mkt_load_fctr_rt decimal(15,8)  NULL ,
	rsdl_mkt_load_ernd_amt decimal(15,2)  NULL ,
	rsdl_mkt_load_prev_amt decimal(15,2)  NULL ,
	rsdl_mkt_load_tot_amt decimal(15,2)  NULL ,
	rsdl_mkt_load_paid_amt decimal(15,2)  NULL ,
	ky_or_tot_due_amt    decimal(15,2)  NULL ,
	biled_ernd_retro_prem_amt decimal(15,2)  NULL ,
	adj_cash_flw_ben_amt decimal(15,2)  NULL ,
	prev_biled_ernd_retro_prem_amt decimal(15,2)  NULL ,
	prem_asses_amt       decimal(15,2)  NULL ,
	prev_std_subj_prem_amt decimal(15,2)  NULL ,
	cesar_cd_tot_amt     decimal(15,2)  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_842850919
		 DEFAULT  CURRENT_TIMESTAMP
)
ON "AIS_Data"
go



ALTER TABLE PREM_ADJ_RETRO_DTL
	ADD CONSTRAINT PK_PREM_ADJ_RETRO_DTL PRIMARY KEY  CLUSTERED (prem_adj_retro_dtl_id ASC,prem_adj_retro_id ASC,prem_adj_perd_id ASC,prem_adj_id ASC,custmr_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_RETRO_DTL_NN1 ON PREM_ADJ_RETRO_DTL
( 
	prem_adj_retro_id     ASC,
	prem_adj_perd_id      ASC,
	prem_adj_id           ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_RETRO_DTL_NN2 ON PREM_ADJ_RETRO_DTL
( 
	st_id                 ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_RETRO_DTL_NN3 ON PREM_ADJ_RETRO_DTL
( 
	coml_agmt_id          ASC,
	prem_adj_pgm_id       ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_RETRO_DTL_NN4 ON PREM_ADJ_RETRO_DTL
( 
	ln_of_bsn_id          ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_RETRO_DTL_NN5 ON PREM_ADJ_RETRO_DTL
( 
	custmr_id             ASC
)
INCLUDE( prem_adj_id,prem_adj_perd_id )
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_RETRO_DTL_NN6 ON PREM_ADJ_RETRO_DTL
( 
	prem_adj_perd_id      ASC
)
INCLUDE( prem_adj_id,custmr_id,coml_agmt_id,st_id )
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_RETRO_DTL_NN7 ON PREM_ADJ_RETRO_DTL
( 
	prem_adj_id           ASC
)
INCLUDE( prem_adj_perd_id,biled_ernd_retro_prem_amt )
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_RETRO_DTL_NN8 ON PREM_ADJ_RETRO_DTL
( 
	prem_adj_perd_id      ASC
)
INCLUDE( prem_adj_id,custmr_id,st_id )
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_RETRO_DTL_NN9 ON PREM_ADJ_RETRO_DTL
( 
	st_id                 ASC,
	prem_adj_perd_id      ASC,
	prem_adj_id           ASC,
	custmr_id             ASC,
	prem_adj_retro_dtl_id  ASC,
	prem_adj_retro_id     ASC
)
INCLUDE( coml_agmt_id,prem_adj_pgm_id,subj_paid_idnmty_amt,subj_paid_exps_amt,subj_resrv_idnmty_amt,subj_resrv_exps_amt,adj_dedtbl_wrk_comp_los_amt,basic_amt,clm_hndl_fee_amt,los_base_asessment_amt,los_conv_fctr_amt,non_conv_fee_amt,prem_tax_amt,othr_amt,incur_ernd_retro_prem_amt,adj_incur_ernd_retro_prem_amt,paid_ernd_retro_prem_amt,adj_paid_ernd_retro_prem_amt,cash_flw_ben_amt,prior_cash_flw_ben_amt,exc_los_prem_amt,los_dev_resrv_amt,std_subj_prem_amt,ernd_retro_prem_amt,ky_or_tax_asses_amt,ky_or_prev_tax_asses_amt,rsdl_mkt_load_basic_fctr_rt,rsdl_mkt_load_fctr_rt,rsdl_mkt_load_ernd_amt,rsdl_mkt_load_prev_amt,rsdl_mkt_load_tot_amt,ky_or_tot_due_amt,biled_ernd_retro_prem_amt,adj_cash_flw_ben_amt,prev_biled_ernd_retro_prem_amt,updt_user_id,updt_dt,crte_user_id,crte_dt,prem_asses_amt,prev_std_subj_prem_amt,rsdl_mkt_load_paid_amt,ln_of_bsn_id,los_conv_fctr_rt,prev_subj_paid_idnmty_amt,prev_subj_resrv_idnmty_amt )
go



CREATE NONCLUSTERED INDEX PREM_ADJ_RETRO_DTL_NN10 ON PREM_ADJ_RETRO_DTL
( 
	prem_adj_perd_id      ASC,
	prem_adj_id           ASC,
	custmr_id             ASC
)
go



CREATE NONCLUSTERED INDEX PREM_ADJ_RETRO_DTL_NN11 ON PREM_ADJ_RETRO_DTL
( 
	prem_adj_id           ASC,
	prem_adj_pgm_id       ASC
)
go



CREATE NONCLUSTERED INDEX PREM_ADJ_RETRO_DTL_NN12 ON PREM_ADJ_RETRO_DTL
( 
	prem_adj_id           ASC,
	prem_adj_pgm_id       ASC
)
INCLUDE( los_conv_fctr_rt )
go



CREATE NONCLUSTERED INDEX PREM_ADJ_RETRO_DTL_NN13 ON PREM_ADJ_RETRO_DTL
( 
	prem_adj_id           ASC,
	custmr_id             ASC,
	prem_adj_pgm_id       ASC
)
go



CREATE NONCLUSTERED INDEX PREM_ADJ_RETRO_DTL_NN14 ON PREM_ADJ_RETRO_DTL
( 
	prem_adj_id           ASC,
	prem_adj_pgm_id       ASC
)
INCLUDE( subj_resrv_idnmty_amt,subj_resrv_exps_amt,prem_asses_amt )
go



CREATE TABLE PREM_ADJ_STS
( 
	prem_adj_sts_id      integer IDENTITY ( 1,1 ) ,
	prem_adj_id          integer  NOT NULL ,
	custmr_id            integer  NOT NULL ,
	qlty_cntrl_dt        datetime  NULL ,
	adj_sts_typ_id       integer  NULL ,
	qlty_cntrl_pers_id   integer  NULL ,
	eff_dt               datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_1945238076
		 DEFAULT  CURRENT_TIMESTAMP,
	cmmnt_txt            varchar(512)  NULL ,
	expi_dt              datetime  NULL ,
	aprv_ind             bit  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_317917905
		 DEFAULT  CURRENT_TIMESTAMP
)
ON "AIS_Data"
go



ALTER TABLE PREM_ADJ_STS
	ADD CONSTRAINT PK_PREM_ADJ_STS PRIMARY KEY  CLUSTERED (prem_adj_sts_id ASC,prem_adj_id ASC,custmr_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_STS_NN3 ON PREM_ADJ_STS
( 
	adj_sts_typ_id        ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_STS_NN2 ON PREM_ADJ_STS
( 
	qlty_cntrl_pers_id    ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX PREM_ADJ_STS_NN1 ON PREM_ADJ_STS
( 
	prem_adj_id           ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE TABLE QLTY_CNTRL_LIST
( 
	qlty_cntrl_list_id   integer IDENTITY ( 1,1 ) ,
	chk_list_itm_id      integer  NOT NULL ,
	prem_adj_pgm_id      integer  NULL ,
	custmr_id            integer  NULL ,
	custmr_rel_id        integer  NULL ,
	prem_adj_sts_id      integer  NULL ,
	prem_adj_aries_clring_id integer  NULL ,
	chklist_sts_cd       char(3)  NULL ,
	prem_adj_id          integer  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_27763653
		 DEFAULT  CURRENT_TIMESTAMP,
	actv_ind             bit  NULL 
)
ON "AIS_Data"
go



ALTER TABLE QLTY_CNTRL_LIST
	ADD CONSTRAINT PK_QLTY_CNTRL_LIST PRIMARY KEY  CLUSTERED (qlty_cntrl_list_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX QLTY_CNTRL_LIST_NN1 ON QLTY_CNTRL_LIST
( 
	prem_adj_pgm_id       ASC,
	custmr_rel_id         ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX QLTY_CNTRL_LIST_NN3 ON QLTY_CNTRL_LIST
( 
	prem_adj_aries_clring_id  ASC,
	prem_adj_id           ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX QLTY_CNTRL_LIST_NN4 ON QLTY_CNTRL_LIST
( 
	chk_list_itm_id       ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX QLTY_CNTRL_LIST_NN2 ON QLTY_CNTRL_LIST
( 
	prem_adj_sts_id       ASC,
	prem_adj_id           ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE TABLE QLTY_CNTRL_MSTR_ISSU_LIST
( 
	qlty_cntrl_mstr_issu_list_id integer IDENTITY ( 1,1 ) ,
	issu_catg_id         integer  NULL ,
	issu_txt             varchar(256)  NULL ,
	finc_ind             bit  NULL ,
	srt_nbr              integer  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_738381768
		 DEFAULT  CURRENT_TIMESTAMP,
	actv_ind             bit  NULL 
)
ON "AIS_Data"
go



ALTER TABLE QLTY_CNTRL_MSTR_ISSU_LIST
	ADD CONSTRAINT PK_QLTY_CNTRL_MSTR_ISSU_LIST PRIMARY KEY  CLUSTERED (qlty_cntrl_mstr_issu_list_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX QLTY_CNTRL_MSTR_ISSU_LIST_NN1 ON QLTY_CNTRL_MSTR_ISSU_LIST
( 
	issu_catg_id          ASC
)
ON "AIS_IDX"
go



CREATE TABLE STEPPED_FCTR
( 
	stepped_fctr_id      integer IDENTITY ( 1,1 ) ,
	coml_agmt_id         integer  NOT NULL ,
	prem_adj_pgm_id      integer  NOT NULL ,
	custmr_id            integer  NOT NULL ,
	mms_to_valn_nbr      integer  NULL ,
	los_dev_fctr_rt      decimal(15,8)  NULL ,
	incur_but_not_rptd_fctr_rt decimal(15,8)  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_303494425
		 DEFAULT  CURRENT_TIMESTAMP,
	actv_ind             bit  NULL 
)
ON "AIS_Data"
go



ALTER TABLE STEPPED_FCTR
	ADD CONSTRAINT PK_STEPPED_FCTR PRIMARY KEY  CLUSTERED (stepped_fctr_id ASC,coml_agmt_id ASC,prem_adj_pgm_id ASC,custmr_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX STEPPED_FCTR_NN1 ON STEPPED_FCTR
( 
	coml_agmt_id          ASC,
	prem_adj_pgm_id       ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE TABLE SUBJ_PREM_AUDT
( 
	subj_prem_audt_id    integer IDENTITY ( 1,1 ) ,
	coml_agmt_audt_id    integer  NOT NULL ,
	coml_agmt_id         integer  NOT NULL ,
	prem_adj_pgm_id      integer  NOT NULL ,
	custmr_id            integer  NOT NULL ,
	st_id                integer  NOT NULL ,
	prem_amt             decimal(15,2)  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_638488258
		 DEFAULT  CURRENT_TIMESTAMP,
	actv_ind             bit  NULL 
)
ON "AIS_Data"
go



ALTER TABLE SUBJ_PREM_AUDT
	ADD CONSTRAINT PK_SUBJ_PREM_AUDT PRIMARY KEY  CLUSTERED (subj_prem_audt_id ASC,coml_agmt_audt_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX SUBJ_PREM_AUDT_NN2 ON SUBJ_PREM_AUDT
( 
	coml_agmt_audt_id     ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX SUBJ_PREM_AUDT_NN1 ON SUBJ_PREM_AUDT
( 
	st_id                 ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX SUBJ_PREM_AUDT_NN3 ON SUBJ_PREM_AUDT
( 
	coml_agmt_id          ASC,
	prem_adj_pgm_id       ASC,
	custmr_id             ASC,
	st_id                 ASC,
	actv_ind              ASC
)
ON "AIS_IDX"
go



CREATE TABLE THRD_PTY_ADMIN_MNL_INVC
( 
	thrd_pty_admin_mnl_invc_id integer IDENTITY ( 1,1 ) ,
	custmr_id            integer  NOT NULL ,
	related_invoice_id   integer  NULL ,
	thrd_pty_admin_id    integer  NULL ,
	bil_cycl_id          integer  NULL ,
	thrd_pty_admin_los_src_id integer  NULL ,
	thrd_pty_admin_invc_typ_id integer  NULL ,
	invc_nbr_txt         varchar(20)  NULL ,
	invc_dt              datetime  NULL ,
	due_dt               datetime  NULL ,
	valn_dt              datetime  NULL ,
	invc_amt             decimal(15,2)  NULL ,
	pol_yy_nbr           integer  NULL ,
	fnl_ind              bit  NULL ,
	cmmnt_txt            varchar(4000)  NULL ,
	bsn_unt_ofc_id       integer  NULL ,
	end_dt               datetime  NULL ,
	can_ind              bit  NULL ,
	revise_ind           bit  NULL ,
	void_ind             bit  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL 
	CONSTRAINT CURRENT_TIMESTAMP_1744640031
		 DEFAULT  CURRENT_TIMESTAMP,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL ,
	actv_ind             bit  NULL 
)
ON "AIS_Data"
go



ALTER TABLE THRD_PTY_ADMIN_MNL_INVC
	ADD CONSTRAINT PK_THRD_PTY_ADMIN_MNL_INVC PRIMARY KEY  CLUSTERED (thrd_pty_admin_mnl_invc_id ASC,custmr_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX THRD_PTY_ADMIN_MNL_INVC_NN2 ON THRD_PTY_ADMIN_MNL_INVC
( 
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX THRD_PTY_ADMIN_MNL_INVC_NN1 ON THRD_PTY_ADMIN_MNL_INVC
( 
	bsn_unt_ofc_id        ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX THRD_PTY_ADMIN_MNL_INVC_NN3 ON THRD_PTY_ADMIN_MNL_INVC
( 
	thrd_pty_admin_invc_typ_id  ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX THRD_PTY_ADMIN_MNL_INVC_NN6 ON THRD_PTY_ADMIN_MNL_INVC
( 
	thrd_pty_admin_los_src_id  ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX THRD_PTY_ADMIN_MNL_INVC_NN4 ON THRD_PTY_ADMIN_MNL_INVC
( 
	bil_cycl_id           ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX THRD_PTY_ADMIN_MNL_INVC_NN5 ON THRD_PTY_ADMIN_MNL_INVC
( 
	thrd_pty_admin_id     ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX THRD_PTY_ADMIN_MNL_INVC_NN7 ON THRD_PTY_ADMIN_MNL_INVC
( 
	related_invoice_id    ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE TABLE THRD_PTY_ADMIN_MNL_INVC_DTL
( 
	thrd_pty_admin_mnl_invc_dtl_id integer IDENTITY ( 1,1 ) ,
	thrd_pty_admin_mnl_invc_id integer  NOT NULL ,
	custmr_id            integer  NOT NULL ,
	post_trns_typ_id     integer  NOT NULL ,
	due_dt               datetime  NULL ,
	aries_main_nbr_txt   char(5)  NULL ,
	aries_sub_nbr_txt    char(5)  NULL ,
	comp_cd_txt          char(5)  NULL ,
	pol_sym_txt          char(3)  NULL ,
	pol_nbr_txt          char(15)  NULL ,
	pol_modulus_txt      char(2)  NULL ,
	eff_dt               datetime  NULL ,
	expi_dt              datetime  NULL ,
	thrd_pty_admin_amt   decimal(15,2)  NULL ,
	updt_user_id         integer  NULL ,
	updt_dt              datetime  NULL ,
	crte_user_id         integer  NOT NULL ,
	crte_dt              datetime  NOT NULL 
	CONSTRAINT CURRENT_TIMESTAMP_448181218
		 DEFAULT  CURRENT_TIMESTAMP
)
ON "AIS_Data"
go



ALTER TABLE THRD_PTY_ADMIN_MNL_INVC_DTL
	ADD CONSTRAINT PK_THRD_PTY_ADMIN_MNL_INVC_DTL PRIMARY KEY  CLUSTERED (thrd_pty_admin_mnl_invc_dtl_id ASC,thrd_pty_admin_mnl_invc_id ASC,custmr_id ASC)
	 ON "AIS_Data"
go



CREATE NONCLUSTERED INDEX THRD_PTY_ADMIN_MNL_INVC_DT_NN1 ON THRD_PTY_ADMIN_MNL_INVC_DTL
( 
	thrd_pty_admin_mnl_invc_id  ASC,
	custmr_id             ASC
)
ON "AIS_IDX"
go



CREATE NONCLUSTERED INDEX THRD_PTY_ADMIN_MNL_INVC_DT_NN2 ON THRD_PTY_ADMIN_MNL_INVC_DTL
( 
	post_trns_typ_id      ASC
)
ON "AIS_IDX"
go




ALTER TABLE APLCTN_MENU
	ADD CONSTRAINT Fk_Aplctn_Menu_Aplctn_Menu FOREIGN KEY (parnt_id) REFERENCES APLCTN_MENU(aplctn_menu_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE APLCTN_WEB_PAGE_AUDT
	ADD CONSTRAINT Fk_Aplctn_Web_Page_Audt_Custmr FOREIGN KEY (custmr_id) REFERENCES CUSTMR(custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE APLCTN_WEB_PAGE_AUDT
	ADD CONSTRAINT Fk_Aplctn_Web_Page_Audt_Prem_Adj_Pgm FOREIGN KEY (prem_adj_pgm_id,custmr_id) REFERENCES PREM_ADJ_PGM(prem_adj_pgm_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE APLCTN_WEB_PAGE_AUDT
	ADD CONSTRAINT Fk_Lkup_Aplctn_Web_Page_Audt1 FOREIGN KEY (web_page_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE APLCTN_WEB_PAGE_AUTH
	ADD CONSTRAINT Fk_Aplctn_Web_Page_Auth_Aplctn_Menu FOREIGN KEY (aplctn_menu_id) REFERENCES APLCTN_MENU(aplctn_menu_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE APLCTN_WEB_PAGE_AUTH
	ADD CONSTRAINT Fk_Lkup_Aplctn_Web_Page_Auth2 FOREIGN KEY (secur_gp_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE ARIES_TRNSMTL_HIST
	ADD CONSTRAINT Fk_Aries_Trnsmtl_Hist_Prem_Adj_Perd FOREIGN KEY (prem_adj_perd_id,prem_adj_id,custmr_id) REFERENCES PREM_ADJ_PERD(prem_adj_perd_id,prem_adj_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE ARIES_TRNSMTL_HIST
	ADD CONSTRAINT Fk_Aries_Trnsmtl_Hist_Post_Trns_Typ FOREIGN KEY (post_trns_typ_id) REFERENCES POST_TRNS_TYP(post_trns_typ_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE ARMIS_LOS_EXC
	ADD CONSTRAINT Fk_Armis_Los_Exc_Armis_Los_Pol FOREIGN KEY (armis_los_pol_id) REFERENCES ARMIS_LOS_POL(armis_los_pol_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE ARMIS_LOS_EXC
	ADD CONSTRAINT Fk_Lkup_Armis_Los_Exc1 FOREIGN KEY (clm_sts_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE ARMIS_LOS_POL
	ADD CONSTRAINT Fk_Armis_Los_Pol_Coml_Agmt FOREIGN KEY (coml_agmt_id,prem_adj_pgm_id,custmr_id) REFERENCES COML_AGMT(coml_agmt_id,prem_adj_pgm_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE ARMIS_LOS_POL
	ADD CONSTRAINT Fk_Lkup_Armis_Los_Pol FOREIGN KEY (st_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE COMB_ELEMTS
	ADD CONSTRAINT Fk_Comb_Elemts_Coml_Agmt FOREIGN KEY (coml_agmt_id,prem_adj_pgm_id,custmr_id) REFERENCES COML_AGMT(coml_agmt_id,prem_adj_pgm_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE COMB_ELEMTS
	ADD CONSTRAINT Fk_Lkup_Comb_Elemts2 FOREIGN KEY (expo_typ_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE COMB_ELEMTS
	ADD CONSTRAINT Fk_Lkup_Comb_Elemts1 FOREIGN KEY (dvsr_nbr_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE COML_AGMT
	ADD CONSTRAINT Fk_Coml_Agmt_Prem_Adj_Pgm FOREIGN KEY (prem_adj_pgm_id,custmr_id) REFERENCES PREM_ADJ_PGM(prem_adj_pgm_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE COML_AGMT
	ADD CONSTRAINT Fk_Coml_Agmt_Coml_Agmt FOREIGN KEY (parnt_coml_agmt_id,prem_adj_pgm_id,custmr_id) REFERENCES COML_AGMT(coml_agmt_id,prem_adj_pgm_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE COML_AGMT
	ADD CONSTRAINT Fk_Lkup_Coml_Agmt5 FOREIGN KEY (dedtbl_prot_pol_st_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE COML_AGMT
	ADD CONSTRAINT Fk_Lkup_Coml_Agmt4 FOREIGN KEY (adj_typ_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE COML_AGMT
	ADD CONSTRAINT Fk_Lkup_Coml_Agmt1 FOREIGN KEY (aloc_los_adj_exps_typ_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE COML_AGMT
	ADD CONSTRAINT Fk_Lkup_Coml_Agmt3 FOREIGN KEY (los_sys_src_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE COML_AGMT
	ADD CONSTRAINT Fk_Lkup_Coml_Agmt2 FOREIGN KEY (covg_typ_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE COML_AGMT_AUDT
	ADD CONSTRAINT Fk_Coml_Agmt_Audt_Coml_Agmt FOREIGN KEY (coml_agmt_id,prem_adj_pgm_id,custmr_id) REFERENCES COML_AGMT(coml_agmt_id,prem_adj_pgm_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE CUSTMR
	ADD CONSTRAINT Fk_Custmr_Custmr FOREIGN KEY (custmr_rel_id) REFERENCES CUSTMR(custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE CUSTMR
	ADD CONSTRAINT Fk_Lkup_Custmr FOREIGN KEY (bnkrpt_buyout_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE CUSTMR_CMMNT
	ADD CONSTRAINT Fk_Custmr_Cmmnt_Custmr FOREIGN KEY (custmr_id) REFERENCES CUSTMR(custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE CUSTMR_CMMNT
	ADD CONSTRAINT Fk_Lkup_Custmr_Cmmnt FOREIGN KEY (cmmnt_catg_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE CUSTMR_DOC
	ADD CONSTRAINT Fk_Custmr_Doc_Pers1 FOREIGN KEY (qlty_cntrl_pers_id) REFERENCES PERS(pers_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE CUSTMR_DOC
	ADD CONSTRAINT Fk_Lkup_Custmr_Doc1 FOREIGN KEY (frm_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE CUSTMR_DOC
	ADD CONSTRAINT Fk_Custmr_Doc_Custmr FOREIGN KEY (custmr_id) REFERENCES CUSTMR(custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE CUSTMR_DOC
	ADD CONSTRAINT Fk_Custmr_Doc_Pers2 FOREIGN KEY (cash_flw_splist_pers_id) REFERENCES PERS(pers_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE CUSTMR_DOC
	ADD CONSTRAINT Fk_Custmr_Doc_Int_Org FOREIGN KEY (bu_office_id) REFERENCES INT_ORG(int_org_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE CUSTMR_DOC
	ADD CONSTRAINT Fk_Custmr_Doc_Nonais_Custmr FOREIGN KEY (nonais_custmr_id) REFERENCES NONAIS_CUSTMR(nonais_custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE CUSTMR_DOC_ISSUS
	ADD CONSTRAINT Fk_Custmr_Doc_Issus_Custmr_Doc FOREIGN KEY (custmr_doc_id) REFERENCES CUSTMR_DOC(custmr_doc_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE CUSTMR_DOC_ISSUS
	ADD CONSTRAINT Fk_Lkup_Custmr_Doc_Issus FOREIGN KEY (traking_issu_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE CUSTMR_PERS_REL
	ADD CONSTRAINT Fk_Custmr_Pers_Rel_Pers FOREIGN KEY (pers_id) REFERENCES PERS(pers_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE CUSTMR_PERS_REL
	ADD CONSTRAINT Fk_Custmr_Pers_Rel_Custmr FOREIGN KEY (custmr_id) REFERENCES CUSTMR(custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE CUSTMR_PERS_REL
	ADD CONSTRAINT Fk_Lkup_Custmr_Pers_Rel FOREIGN KEY (rol_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE EXTRNL_ORG
	ADD CONSTRAINT Fk_Lkup_Extrnl_Org FOREIGN KEY (role_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE INCUR_LOS_REIM_FUND_FRMLA
	ADD CONSTRAINT Fk_Incur_Los_Reim_Fund_Frmla_Prem_Adj_Pgm FOREIGN KEY (prem_adj_pgm_id,custmr_id) REFERENCES PREM_ADJ_PGM(prem_adj_pgm_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE INCUR_LOS_REIM_FUND_FRMLA
	ADD CONSTRAINT Fk_Lkup_Incur_Los_Reim_Fund_Frmla FOREIGN KEY (los_reim_fund_fctr_typ_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE LKUP
	ADD CONSTRAINT Fk_Lkup_Typ_Lkup FOREIGN KEY (lkup_typ_id) REFERENCES LKUP_TYP(lkup_typ_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE LSI_CUSTMR
	ADD CONSTRAINT Fk_Lsi_Custmr_Custmr FOREIGN KEY (custmr_id) REFERENCES CUSTMR(custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE NON_SUBJ_PREM_AUDT
	ADD CONSTRAINT Fk_Non_Subj_Prem_Audt_Coml_Agmt_Audt FOREIGN KEY (coml_agmt_audt_id) REFERENCES COML_AGMT_AUDT(coml_agmt_audt_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE NON_SUBJ_PREM_AUDT
	ADD CONSTRAINT Fk_Lkup_Audt_Non_Subj_Prem FOREIGN KEY (nsa_typ_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PERS
	ADD CONSTRAINT Fk_Pers_Extrnl_Org FOREIGN KEY (extrnl_org_id) REFERENCES EXTRNL_ORG(extrnl_org_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PERS
	ADD CONSTRAINT Fk_Pers_Pers FOREIGN KEY (mgr_id) REFERENCES PERS(pers_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PERS
	ADD CONSTRAINT Fk_Lkup_Pers4 FOREIGN KEY (prefx_ttl_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PERS
	ADD CONSTRAINT Fk_Lkup_Pers1 FOREIGN KEY (conctc_typ_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PERS
	ADD CONSTRAINT Fk_Lkup_Pers3 FOREIGN KEY (acct_qlty_cntrl_pct_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PERS
	ADD CONSTRAINT Fk_Lkup_Pers2 FOREIGN KEY (adj_qlty_cntrl_pct_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PERS
	ADD CONSTRAINT Fk_Lkup_Pers5 FOREIGN KEY (aries_qlty_cntrl_pct_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE POST_ADDR
	ADD CONSTRAINT Fk_Lkup_Post_Addr FOREIGN KEY (st_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE POST_ADDR
	ADD CONSTRAINT Fk_Post_Addr_Pers FOREIGN KEY (pers_id) REFERENCES PERS(pers_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE POST_TRNS_TYP
	ADD CONSTRAINT Fk_Lkup_Post_Trns_Typ1 FOREIGN KEY (trns_typ_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ
	ADD CONSTRAINT Fk_Prem_Adj_Pers FOREIGN KEY (twenty_pct_qlty_cntrl_pers_id) REFERENCES PERS(pers_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ
	ADD CONSTRAINT Fk_Lkup_Prem_Adj3 FOREIGN KEY (adj_pendg_rsn_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ
	ADD CONSTRAINT Fk_Lkup_Prem_Adj2 FOREIGN KEY (adj_void_rsn_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ
	ADD CONSTRAINT Fk_Lkup_Prem_Adj1 FOREIGN KEY (adj_rrsn_rsn_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ
	ADD CONSTRAINT Fk_Prem_Adj_Prem_Adj FOREIGN KEY (rel_prem_adj_id,reg_custmr_id) REFERENCES PREM_ADJ(prem_adj_id,reg_custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ
	ADD CONSTRAINT Fk_Prem_Adj_Custmr FOREIGN KEY (reg_custmr_id) REFERENCES CUSTMR(custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ
	ADD CONSTRAINT Fk_Prem_Adj_Extrnl_Org FOREIGN KEY (brkr_id) REFERENCES EXTRNL_ORG(extrnl_org_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ
	ADD CONSTRAINT Fk_Prem_Adj_Int_Org FOREIGN KEY (bu_office_id) REFERENCES INT_ORG(int_org_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_ARIES_CLRING
	ADD CONSTRAINT Fk_Prem_Adj_Aries_Clring_Pers1 FOREIGN KEY (qlty_cntrl_pers_id) REFERENCES PERS(pers_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_ARIES_CLRING
	ADD CONSTRAINT Fk_Prem_Adj_Aries_Clring_Prem_Adj FOREIGN KEY (prem_adj_id,custmr_id) REFERENCES PREM_ADJ(prem_adj_id,reg_custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_CMMNT
	ADD CONSTRAINT Fk_Lkup_Prem_Adj_Cmmnt FOREIGN KEY (cmmnt_catg_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_CMMNT
	ADD CONSTRAINT Fk_Prem_Adj_Cmmnt_Prem_Adj FOREIGN KEY (prem_adj_id,reg_custmr_id) REFERENCES PREM_ADJ(prem_adj_id,reg_custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_CMMNT
	ADD CONSTRAINT Fk_Prem_Adj_Cmmnt_Prem_Adj_Perd FOREIGN KEY (prem_adj_perd_id,prem_adj_id,custmr_id) REFERENCES PREM_ADJ_PERD(prem_adj_perd_id,prem_adj_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_COMB_ELEMTS
	ADD CONSTRAINT Fk_Prem_Adj_Comb_Elemts_Prem_Adj_Perd FOREIGN KEY (prem_adj_perd_id,prem_adj_id,custmr_id) REFERENCES PREM_ADJ_PERD(prem_adj_perd_id,prem_adj_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_LOS_REIM_FUND_POST
	ADD CONSTRAINT Fk_Prem_Adj_Los_Reim_Fund_Post_Prem_Adj_Perd FOREIGN KEY (prem_adj_perd_id,prem_adj_id,custmr_id) REFERENCES PREM_ADJ_PERD(prem_adj_perd_id,prem_adj_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_LOS_REIM_FUND_POST
	ADD CONSTRAINT Fk_Prem_Adj_Los_Reim_Fund_Post_Post_Trns_Typ FOREIGN KEY (recv_typ_id) REFERENCES POST_TRNS_TYP(post_trns_typ_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_MISC_INVC
	ADD CONSTRAINT Fk_Prem_Adj_Misc_Invc_Prem_Adj_Perd FOREIGN KEY (prem_adj_perd_id,prem_adj_id,custmr_id) REFERENCES PREM_ADJ_PERD(prem_adj_perd_id,prem_adj_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_MISC_INVC
	ADD CONSTRAINT Fk_Prem_Adj_Misc_Invc_Post_Trns_Typ FOREIGN KEY (post_trns_typ_id) REFERENCES POST_TRNS_TYP(post_trns_typ_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_NY_SCND_INJR_FUND
	ADD CONSTRAINT Fk_Prem_Adj_Ny_Scnd_Injr_Fund_Prem_Adj_Perd FOREIGN KEY (prem_adj_perd_id,prem_adj_id,custmr_id) REFERENCES PREM_ADJ_PERD(prem_adj_perd_id,prem_adj_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_NY_SCND_INJR_FUND
	ADD CONSTRAINT Fk_Prem_Adj_Ny_Scnd_Injr_Fund_Coml_Agmt FOREIGN KEY (coml_agmt_id,prem_adj_pgm_id,custmr_id) REFERENCES COML_AGMT(coml_agmt_id,prem_adj_pgm_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PAID_LOS_BIL
	ADD CONSTRAINT Fk_Lkup_Prem_Adj_Paid_Los_Bil FOREIGN KEY (ln_of_bsn_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PAID_LOS_BIL
	ADD CONSTRAINT Fk_Prem_Adj_Paid_Los_Bil_Coml_Agmt FOREIGN KEY (coml_agmt_id,prem_adj_pgm_id,custmr_id) REFERENCES COML_AGMT(coml_agmt_id,prem_adj_pgm_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PAID_LOS_BIL
	ADD CONSTRAINT Fk_Prem_Adj_Paid_Los_Bil_Prem_Adj_Perd FOREIGN KEY (prem_adj_perd_id,prem_adj_id,custmr_id) REFERENCES PREM_ADJ_PERD(prem_adj_perd_id,prem_adj_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PARMET_DTL
	ADD CONSTRAINT Fk_Prem_Adj_Parmet_Dtl_Prem_Adj_Parmet_Setup FOREIGN KEY (prem_adj_parmet_setup_id,prem_adj_perd_id,prem_adj_id,custmr_id) REFERENCES PREM_ADJ_PARMET_SETUP(prem_adj_parmet_setup_id,prem_adj_perd_id,prem_adj_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PARMET_DTL
	ADD CONSTRAINT Fk_Lkup_Prem_Adj_Parmet_Dtl1 FOREIGN KEY (ln_of_bsn_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PARMET_DTL
	ADD CONSTRAINT Fk_Lkup_Prem_Adj_Parmet_Dtl2 FOREIGN KEY (st_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PARMET_DTL
	ADD CONSTRAINT Fk_Lkup_Prem_Adj_Parmet_Dtl3 FOREIGN KEY (clm_hndl_fee_los_typ_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PARMET_DTL
	ADD CONSTRAINT Fk_Prem_Adj_Dtl_Coml_Agmt FOREIGN KEY (coml_agmt_id,prem_adj_pgm_id,custmr_id) REFERENCES COML_AGMT(coml_agmt_id,prem_adj_pgm_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PARMET_SETUP
	ADD CONSTRAINT Fk_Prem_Adj_Parmet_Setup_Prem_Adj_Perd FOREIGN KEY (prem_adj_perd_id,prem_adj_id,custmr_id) REFERENCES PREM_ADJ_PERD(prem_adj_perd_id,prem_adj_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PARMET_SETUP
	ADD CONSTRAINT Fk_Lkup_Prem_Adj_Parmet_Setup1 FOREIGN KEY (adj_parmet_typ_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PARMET_SETUP
	ADD CONSTRAINT Fk_Prem_Adj_Parmet_Setup_Prem_Adj_Pgm_Setup FOREIGN KEY (prem_adj_pgm_setup_id,prem_adj_pgm_id,custmr_id) REFERENCES PREM_ADJ_PGM_SETUP(prem_adj_pgm_setup_id,prem_adj_pgm_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PERD
	ADD CONSTRAINT Fk_Prem_Adj_Perd_Prem_Adj FOREIGN KEY (prem_adj_id,reg_custmr_id) REFERENCES PREM_ADJ(prem_adj_id,reg_custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PERD
	ADD CONSTRAINT Fk_Prem_Adj_Perd_Prem_Adj_Pgm FOREIGN KEY (prem_adj_pgm_id,custmr_id) REFERENCES PREM_ADJ_PGM(prem_adj_pgm_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PERD_TOT
	ADD CONSTRAINT Fk_Prem_Adj_Perd_Tot_Prem_Adj_Perd FOREIGN KEY (prem_adj_perd_id,prem_adj_id,custmr_id) REFERENCES PREM_ADJ_PERD(prem_adj_perd_id,prem_adj_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PGM
	ADD CONSTRAINT Fk_Prem_Adj_Pgm_Custmr FOREIGN KEY (custmr_id) REFERENCES CUSTMR(custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PGM
	ADD CONSTRAINT Fk_Prem_Adj_Pgm_Extrnl_Org FOREIGN KEY (brkr_id) REFERENCES EXTRNL_ORG(extrnl_org_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PGM
	ADD CONSTRAINT Fk_Prem_Adj_Pgm_Pers2 FOREIGN KEY (brkr_conctc_id) REFERENCES PERS(pers_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PGM
	ADD CONSTRAINT Fk_Prem_Adj_Pgm_Mstr_Ernd_Retro_Prem_Frmla FOREIGN KEY (mstr_ernd_retro_prem_frmla_id) REFERENCES MSTR_ERND_RETRO_PREM_FRMLA(mstr_ernd_retro_prem_frmla_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PGM
	ADD CONSTRAINT Fk_Prem_Adj_Pgm_Int_Org FOREIGN KEY (bsn_unt_ofc_id) REFERENCES INT_ORG(int_org_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PGM
	ADD CONSTRAINT Fk_Lkup_Prem_Adj_Pgm4 FOREIGN KEY (pgm_typ_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PGM
	ADD CONSTRAINT Fk_Lkup_Prem_Adj_Pgm3 FOREIGN KEY (bnkrpt_buyout_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PGM
	ADD CONSTRAINT Fk_Lkup_Prem_Adj_Pgm2 FOREIGN KEY (paid_incur_typ_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PGM
	ADD CONSTRAINT Fk_Prem_Adj_Pgm_Pers1 FOREIGN KEY (qlty_cntrl_pers_id) REFERENCES PERS(pers_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PGM_DTL
	ADD CONSTRAINT Fk_Prem_Adj_Pgm_Dtl_Prem_Adj_Pgm_Setup FOREIGN KEY (prem_adj_pgm_setup_id,prem_adj_pgm_id,custmr_id) REFERENCES PREM_ADJ_PGM_SETUP(prem_adj_pgm_setup_id,prem_adj_pgm_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PGM_DTL
	ADD CONSTRAINT Fk_Lkup_Prem_Adj_Pgm_Dtl1 FOREIGN KEY (clm_hndl_fee_los_typ_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PGM_DTL
	ADD CONSTRAINT Fk_Lkup_Prem_Adj_Pgm_Dtl3 FOREIGN KEY (st_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PGM_DTL
	ADD CONSTRAINT Fk_Lkup_Prem_Adj_Pgm_Dtl2 FOREIGN KEY (ln_of_bsn_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PGM_PERS_REL
	ADD CONSTRAINT Fk_Prem_Adj_Pgm_Pers_Rel_Prem_Adj_Pgm FOREIGN KEY (prem_adj_pgm_id,custmr_id) REFERENCES PREM_ADJ_PGM(prem_adj_pgm_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PGM_PERS_REL
	ADD CONSTRAINT Fk_Prem_Adj_Pgm_Pers_Rel_Pers FOREIGN KEY (pers_id) REFERENCES PERS(pers_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PGM_PERS_REL
	ADD CONSTRAINT Fk_Lkup_Prem_Adj_Pgm_Pers_Rel FOREIGN KEY (commu_medum_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PGM_RETRO
	ADD CONSTRAINT Fk_Prem_Adj_Pgm_Retro_Prem_Adj_Pgm FOREIGN KEY (prem_adj_pgm_id,custmr_id) REFERENCES PREM_ADJ_PGM(prem_adj_pgm_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PGM_RETRO
	ADD CONSTRAINT Fk_Lkup_Prem_Adj_Pgm_Retro3 FOREIGN KEY (retro_elemt_typ_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PGM_RETRO
	ADD CONSTRAINT Fk_Lkup_Prem_Adj_Pgm_Retro2 FOREIGN KEY (expo_typ_incremnt_nbr_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PGM_RETRO
	ADD CONSTRAINT Fk_Lkup_Prem_Adj_Pgm_Retro1 FOREIGN KEY (expo_typ_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PGM_SETUP
	ADD CONSTRAINT Fk_Prem_Adj_Pgm_Setup_Prem_Adj_Pgm FOREIGN KEY (prem_adj_pgm_id,custmr_id) REFERENCES PREM_ADJ_PGM(prem_adj_pgm_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PGM_SETUP
	ADD CONSTRAINT Fk_Lkup_Prem_Adj_Pgm_Setup3 FOREIGN KEY (clm_hndl_fee_basis_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PGM_SETUP
	ADD CONSTRAINT Fk_Lkup_Prem_Adj_Pgm_Setup2 FOREIGN KEY (los_base_asses_adj_typ_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PGM_SETUP
	ADD CONSTRAINT Fk_Lkup_Prem_Adj_Pgm_Setup1 FOREIGN KEY (adj_parmet_typ_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PGM_SETUP
	ADD CONSTRAINT Fk_Lkup_Prem_Adj_Pgm_Setup4 FOREIGN KEY (incur_but_not_rptd_los_dev_fctr_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PGM_SETUP_POL
	ADD CONSTRAINT Fk_Prem_Adj_Pgm_Setup_Pol_Prem_Adj_Pgm_Setup FOREIGN KEY (prem_adj_pgm_setup_id,prem_adj_pgm_id,custmr_id) REFERENCES PREM_ADJ_PGM_SETUP(prem_adj_pgm_setup_id,prem_adj_pgm_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PGM_SETUP_POL
	ADD CONSTRAINT Fk_Prem_Adj_Pgm_Setup_Pol_Coml_Agmt FOREIGN KEY (coml_agmt_id,prem_adj_pgm_id,custmr_id) REFERENCES COML_AGMT(coml_agmt_id,prem_adj_pgm_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PGM_STS
	ADD CONSTRAINT Fk_Lkup_Prem_Adj_Pgm_Sts1 FOREIGN KEY (pgm_perd_sts_typ_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_PGM_STS
	ADD CONSTRAINT Fk_Prem_Adj_Pgm_Sts_Prem_Adj_Pgm FOREIGN KEY (prem_adj_pgm_id,custmr_id) REFERENCES PREM_ADJ_PGM(prem_adj_pgm_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_RETRO
	ADD CONSTRAINT Fk_Prem_Adj_Retro_Prem_Adj_Perd FOREIGN KEY (prem_adj_perd_id,prem_adj_id,custmr_id) REFERENCES PREM_ADJ_PERD(prem_adj_perd_id,prem_adj_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_RETRO
	ADD CONSTRAINT Fk_Prem_Adj_Retro_Coml_Agmt FOREIGN KEY (coml_agmt_id,prem_adj_pgm_id,custmr_id) REFERENCES COML_AGMT(coml_agmt_id,prem_adj_pgm_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_RETRO_DTL
	ADD CONSTRAINT Fk_Prem_Adj_Retro_Prem_Adj_Retro_Dtl FOREIGN KEY (prem_adj_retro_id,prem_adj_perd_id,prem_adj_id,custmr_id) REFERENCES PREM_ADJ_RETRO(prem_adj_retro_id,prem_adj_perd_id,prem_adj_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_RETRO_DTL
	ADD CONSTRAINT Fk_Lkup_Prem_Adj_Retro_Dtl1 FOREIGN KEY (st_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_RETRO_DTL
	ADD CONSTRAINT Fk_Prem_Adj_Retro_Dtl_Coml_Agmt FOREIGN KEY (coml_agmt_id,prem_adj_pgm_id,custmr_id) REFERENCES COML_AGMT(coml_agmt_id,prem_adj_pgm_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_RETRO_DTL
	ADD CONSTRAINT Fk_Lkup_Prem_Adj_Retro_Dtl2 FOREIGN KEY (ln_of_bsn_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_STS
	ADD CONSTRAINT Fk_Lkup_Prem_Adj_Sts1 FOREIGN KEY (adj_sts_typ_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_STS
	ADD CONSTRAINT Fk_Prem_Adj_Sts_Pers FOREIGN KEY (qlty_cntrl_pers_id) REFERENCES PERS(pers_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE PREM_ADJ_STS
	ADD CONSTRAINT Fk_Prem_Adjst_Sts_Prem_Adj FOREIGN KEY (prem_adj_id,custmr_id) REFERENCES PREM_ADJ(prem_adj_id,reg_custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE QLTY_CNTRL_LIST
	ADD CONSTRAINT Fk_Qlty_Cntrl_List_Prem_Adj_Pgm FOREIGN KEY (prem_adj_pgm_id,custmr_rel_id) REFERENCES PREM_ADJ_PGM(prem_adj_pgm_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE QLTY_CNTRL_LIST
	ADD CONSTRAINT Fk_Qlty_Cntrl_List_Prem_Adj_Aries_Clring FOREIGN KEY (prem_adj_aries_clring_id,prem_adj_id,custmr_id) REFERENCES PREM_ADJ_ARIES_CLRING(prem_adj_aries_clring_id,prem_adj_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE QLTY_CNTRL_LIST
	ADD CONSTRAINT Fk_Qlty_Cntrl_List_Mstr_Issu_List FOREIGN KEY (chk_list_itm_id) REFERENCES QLTY_CNTRL_MSTR_ISSU_LIST(qlty_cntrl_mstr_issu_list_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE QLTY_CNTRL_LIST
	ADD CONSTRAINT Fk_Qlty_Cntrl_List_Prem_Adj_Sts FOREIGN KEY (prem_adj_sts_id,prem_adj_id,custmr_id) REFERENCES PREM_ADJ_STS(prem_adj_sts_id,prem_adj_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE QLTY_CNTRL_MSTR_ISSU_LIST
	ADD CONSTRAINT Fk_Lkup_Qlty_Cntrl_Mstr_Issu_List1 FOREIGN KEY (issu_catg_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE STEPPED_FCTR
	ADD CONSTRAINT Fk_Stepped_Fctr_Coml_Agmt FOREIGN KEY (coml_agmt_id,prem_adj_pgm_id,custmr_id) REFERENCES COML_AGMT(coml_agmt_id,prem_adj_pgm_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE SUBJ_PREM_AUDT
	ADD CONSTRAINT Fk_Subj_Prem_Audt_Coml_Agmt_Audt FOREIGN KEY (coml_agmt_audt_id) REFERENCES COML_AGMT_AUDT(coml_agmt_audt_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE SUBJ_PREM_AUDT
	ADD CONSTRAINT Fk_Lkup_Subj_Prem_Audt FOREIGN KEY (st_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE THRD_PTY_ADMIN_MNL_INVC
	ADD CONSTRAINT Fk_Thrd_Pty_Admin_Mnl_Invc_Custmr FOREIGN KEY (custmr_id) REFERENCES CUSTMR(custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE THRD_PTY_ADMIN_MNL_INVC
	ADD CONSTRAINT Fk_Thrd_Pty_Admin_Mnl_Invc_Int_Org FOREIGN KEY (bsn_unt_ofc_id) REFERENCES INT_ORG(int_org_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE THRD_PTY_ADMIN_MNL_INVC
	ADD CONSTRAINT Fk_Lkup_Thrd_Pty_Admin_Mnl_Invc3 FOREIGN KEY (thrd_pty_admin_invc_typ_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE THRD_PTY_ADMIN_MNL_INVC
	ADD CONSTRAINT Fk_Lkup_Thrd_Pty_Admin_Mnl_Invc2 FOREIGN KEY (thrd_pty_admin_los_src_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE THRD_PTY_ADMIN_MNL_INVC
	ADD CONSTRAINT Fk_Lkup_Thrd_Pty_Admin_Mnl_Invc1 FOREIGN KEY (bil_cycl_id) REFERENCES LKUP(lkup_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE THRD_PTY_ADMIN_MNL_INVC
	ADD CONSTRAINT Fk_Thrd_Pty_Admin_Mnl_Invc_Extrnl_Org FOREIGN KEY (thrd_pty_admin_id) REFERENCES EXTRNL_ORG(extrnl_org_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE THRD_PTY_ADMIN_MNL_INVC
	ADD CONSTRAINT Fk_Thrd_Pty_Admin_Manl_Invc_Thrd_Pty_Admin_Manl_Invc FOREIGN KEY (related_invoice_id,custmr_id) REFERENCES THRD_PTY_ADMIN_MNL_INVC(thrd_pty_admin_mnl_invc_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE THRD_PTY_ADMIN_MNL_INVC_DTL
	ADD CONSTRAINT Fk_Thrd_Pty_Admin_Mnl_Invc_Dtl_Thrd_Pty_Administrator_Mnl_Invc FOREIGN KEY (thrd_pty_admin_mnl_invc_id,custmr_id) REFERENCES THRD_PTY_ADMIN_MNL_INVC(thrd_pty_admin_mnl_invc_id,custmr_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE THRD_PTY_ADMIN_MNL_INVC_DTL
	ADD CONSTRAINT Fk_Thrd_Pty_Admin_Mnl_Invc_Dtl_Post_Trns_Typ FOREIGN KEY (post_trns_typ_id) REFERENCES POST_TRNS_TYP(post_trns_typ_id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go


ALTER TABLE int_org ADD actv_ind bit default 1
go
