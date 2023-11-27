IF NOT EXISTS(SELECT 1 FROM APLCTN_MENU WHERE menu_nm_txt = 'About AIS')
BEGIN
	DECLARE @aplctn_menu_id INT
	
	UPDATE APLCTN_MENU
	SET menu_nm_txt = 'About AIS',
		web_page_txt = 'AboutAIS.aspx',
		crte_dt = '10/12/2015',
		crte_user_id = 1,
		menu_tooltip_txt = 'AIS config details'
	WHERE aplctn_menu_id = 8
	
	UPDATE APLCTN_WEB_PAGE_AUTH
	SET crte_dt = '10/12/2015',
		crte_user_id = 1
	WHERE aplctn_menu_id = 8
	

	INSERT INTO APLCTN_MENU (crte_dt,menu_nm_txt, parnt_id, web_page_txt, menu_tooltip_txt, actv_ind, depnd_txt, page_catg_cd, crte_user_id)  VALUES('01/01/2008','Exit',	NULL,	'Javascript:LogUserOff();window.close();',	NULL,	1,	NULL, 'M', 1)
	SET @aplctn_menu_id = @@IDENTITY
	
	INSERT INTO [APLCTN_WEB_PAGE_AUTH](crte_dt,[secur_gp_id],[aplctn_menu_id],[authd_ind],[inquiry_full_acss_ind_cd],[crte_user_id])values('01/01/2008',354,@aplctn_menu_id,1,'I',1)
	INSERT INTO [APLCTN_WEB_PAGE_AUTH](crte_dt,[secur_gp_id],[aplctn_menu_id],[authd_ind],[inquiry_full_acss_ind_cd],[crte_user_id])values('01/01/2008',355,@aplctn_menu_id,1,'F',1)	
	INSERT INTO [APLCTN_WEB_PAGE_AUTH](crte_dt,[secur_gp_id],[aplctn_menu_id],[authd_ind],[inquiry_full_acss_ind_cd],[crte_user_id])values('01/01/2008',356,@aplctn_menu_id,1,'F',1)
	INSERT INTO [APLCTN_WEB_PAGE_AUTH](crte_dt,[secur_gp_id],[aplctn_menu_id],[authd_ind],[inquiry_full_acss_ind_cd],[crte_user_id])values('01/01/2008',357,@aplctn_menu_id,1,'F',1)
	INSERT INTO [APLCTN_WEB_PAGE_AUTH](crte_dt,[secur_gp_id],[aplctn_menu_id],[authd_ind],[inquiry_full_acss_ind_cd],[crte_user_id])values('01/01/2008',358,@aplctn_menu_id,1,'F',1)
END