GO
CREATE NONCLUSTERED INDEX COML_AGMT_NN8
ON [dbo].[COML_AGMT] ([pol_sym_txt],[pol_nbr_txt],[pol_modulus_txt])
GO
CREATE NONCLUSTERED INDEX COML_AGMT_NN9
ON [dbo].[COML_AGMT] ([custmr_id],[pol_eff_dt],[planned_end_date],[pol_sym_txt],[pol_nbr_txt],[pol_modulus_txt])
GO
CREATE NONCLUSTERED INDEX PERS_NN8
ON [dbo].[PERS] ([external_reference])
INCLUDE ([pers_id])
GO
CREATE NONCLUSTERED INDEX ARMIS_LOS_EXC_NN3
ON [dbo].[ARMIS_LOS_EXC] ([prem_adj_pgm_id],[custmr_id],[actv_ind])
GO 
