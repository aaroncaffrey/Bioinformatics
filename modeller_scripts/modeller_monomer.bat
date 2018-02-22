@echo off
IF NOT EXIST "model_monomer.pdb" (

	IF EXIST "template_ligand_after_substitution.ali" (
		echo %date% %time%
		set HDF5_DISABLE_VERSION_CHECK=2
		C:\Anaconda3\python.exe c:\modeller_scripts\modeller_monomer_salign.py > modeller_monomer_salign.log
		C:\Anaconda3\python.exe c:\modeller_scripts\modeller_monomer_align2D.py > modeller_monomer_align2D.log
		C:\Anaconda3\python.exe c:\modeller_scripts\modeller_monomer_model-single.py > modeller_monomer_model-single.log
        	IF EXIST "model_monomer.pdb" (
                	ren model_monomer.pdb model_monomer_%random%.pdb
	        )
		ren query.?????????.pdb model_monomer.pdb
		rem C:\Anaconda3\python.exe c:\modeller_scripts\modeller_monomer_assessment.py > modeller_monomer_assessment.log
	)
)
rem cd %this_dir%
