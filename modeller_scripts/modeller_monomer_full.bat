@echo off

echo.
echo.
echo --------------------------------------------
echo Making fragment monomer
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo Running modeller_fragment_monomer_salign.py
modeller_fragment_monomer_salign.py > modeller_fragment_monomer_salign.log
echo.
echo.
echo --------------------------------------------
echo Running modeller_fragment_monomer_align2D.py
modeller_fragment_monomer_align2D.py > modeller_fragment_monomer_align2D.log
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo Running modeller_fragment_monomer_model-single.py
modeller_fragment_monomer_model-single.py > modeller_fragment_monomer_model-single.log
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo Renaming to model_fragment_monomer.pdb
ren query.?????????.pdb model_fragment_monomer.pdb
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo Making locked monomer
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo Running modeller_locked_monomer_salign.py
modeller_locked_monomer_salign.py > modeller_locked_monomer_salign.log
echo.
echo.
echo --------------------------------------------
echo Running modeller_locked_monomer_align2D.py
modeller_locked_monomer_align2D.py > modeller_locked_monomer_align2D.log
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo Running modeller_locked_monomer_model-single.py
modeller_locked_monomer_model-single.py > modeller_locked_monomer_model-single.log
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo Renaming to model_locked_monomer.pdb
ren query.?????????.pdb model_locked_monomer.pdb
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo Making full monomer model
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo Running modeller_monomer_salign.py
modeller_monomer_salign.py > modeller_monomer_salign.log
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo Running modeller_monomer_align2D.py
modeller_monomer_align2D.py > modeller_monomer_align2D.log
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo Running modeller_monomer_model-single.py
modeller_monomer_model-single.py > modeller_monomer_model-single.log
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo Renaming to model_monomer.pdb
ren query.?????????.pdb model_monomer.pdb
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo Running fragment and full monomer assessment
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo Running modeller_monomer_assessment.py
modeller_monomer_assessment.py > modeller_monomer_assessment.log
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo Running pisa_monomer.bat
CALL pisa_monomer.bat > pisa_monomer.log
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo Finished
echo --------------------------------------------
echo.
echo.