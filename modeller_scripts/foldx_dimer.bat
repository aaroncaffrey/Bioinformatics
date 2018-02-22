set PATH=%PATH%;c:\foldx

IF NOT EXIST "rotabase.txt" (
	@copy /Y c:\foldx\rotabase.txt . >nul
)

IF NOT EXIST "model_monomer_Repair.pdb" (
	rem c:\foldx\foldx.exe -c RepairPDB --pdb "model_monomer.pdb" --repair_Interface ONLY > foldx_RP_model_monomer.log
	c:\foldx\foldx.exe -c RepairPDB --pdb "model_monomer.pdb" > foldx_RP_model_monomer.log
	c:\foldx\foldx.exe -c AnalyseComplex --pdb "model_monomer.pdb" > foldx_AC_model_monomer.log
	c:\foldx\foldx.exe -c AnalyseComplex --pdb "model_monomer_Repair.pdb" > foldx_AC_model_monomer_Repair.log
)
