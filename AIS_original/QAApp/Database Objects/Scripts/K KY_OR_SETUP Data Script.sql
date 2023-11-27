
truncate table KY_OR_SETUP
go

DBCC CHECKIDENT('KY_OR_SETUP', RESEED,1)

go


INSERT INTO KY_OR_SETUP(crte_dt, eff_dt,ky_fctr_rt,or_fctr_rt,crte_user_id)values('01/01/2008','01/01/92',0.1168,0.045,999999)
INSERT INTO KY_OR_SETUP(crte_dt, eff_dt,ky_fctr_rt,or_fctr_rt,crte_user_id)values('01/01/2008','01/01/94',0.123,0.045,999999)
INSERT INTO KY_OR_SETUP(crte_dt, eff_dt,ky_fctr_rt,or_fctr_rt,crte_user_id)values('01/01/2008','04/01/89',0.169,0.055,999999)
INSERT INTO KY_OR_SETUP(crte_dt, eff_dt,ky_fctr_rt,or_fctr_rt,crte_user_id)values('01/01/2008','01/01/90',0.169,0.045,999999)
INSERT INTO KY_OR_SETUP(crte_dt, eff_dt,ky_fctr_rt,or_fctr_rt,crte_user_id)values('01/01/2008','01/01/93',0.1168,0.045,999999)
INSERT INTO KY_OR_SETUP(crte_dt, eff_dt,ky_fctr_rt,or_fctr_rt,crte_user_id)values('01/01/2008','01/01/87',0.337,0.07,999999)
INSERT INTO KY_OR_SETUP(crte_dt, eff_dt,ky_fctr_rt,or_fctr_rt,crte_user_id)values('01/01/2008','11/01/87',0.232,0.07,999999)
INSERT INTO KY_OR_SETUP(crte_dt, eff_dt,ky_fctr_rt,or_fctr_rt,crte_user_id)values('01/01/2008','01/01/84',0,0.168,999999)
INSERT INTO KY_OR_SETUP(crte_dt, eff_dt,ky_fctr_rt,or_fctr_rt,crte_user_id)values('01/01/2008','01/01/95',0.097,0.045,999999)
INSERT INTO KY_OR_SETUP(crte_dt, eff_dt,ky_fctr_rt,or_fctr_rt,crte_user_id)values('01/01/2008','01/01/96',0.09,0.045,999999)
INSERT INTO KY_OR_SETUP(crte_dt, eff_dt,ky_fctr_rt,or_fctr_rt,crte_user_id)values('01/01/2008','01/01/97',0.09,0.045,999999)
INSERT INTO KY_OR_SETUP(crte_dt, eff_dt,ky_fctr_rt,or_fctr_rt,crte_user_id)values('01/01/2008','01/01/98',0.09,0.073,999999)
INSERT INTO KY_OR_SETUP(crte_dt, eff_dt,ky_fctr_rt,or_fctr_rt,crte_user_id)values('01/01/2008','01/01/99',0.09,0.073,999999)
INSERT INTO KY_OR_SETUP(crte_dt, eff_dt,ky_fctr_rt,or_fctr_rt,crte_user_id)values('01/01/2008','01/01/02',0.115,0.08,999999)
INSERT INTO KY_OR_SETUP(crte_dt, eff_dt,ky_fctr_rt,or_fctr_rt,crte_user_id)values('01/01/2008','01/01/04',0.115,0.07,999999)
INSERT INTO KY_OR_SETUP(crte_dt, eff_dt,ky_fctr_rt,or_fctr_rt,crte_user_id)values('01/01/2008','01/01/05',0.09,0.068,999999)
INSERT INTO KY_OR_SETUP(crte_dt, eff_dt,ky_fctr_rt,or_fctr_rt,crte_user_id)values('01/01/2008','01/01/06',0.065,0.055,999999)
INSERT INTO KY_OR_SETUP(crte_dt, eff_dt,ky_fctr_rt,or_fctr_rt,crte_user_id)values('01/01/2008','01/01/07',0.065,0.046,999999)
INSERT INTO KY_OR_SETUP(crte_dt, eff_dt,ky_fctr_rt,or_fctr_rt,crte_user_id)values('01/01/2008','01/01/08',0.065,0.046,999999)
go