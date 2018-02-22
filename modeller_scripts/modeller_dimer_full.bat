@echo off
set HDF5_DISABLE_VERSION_CHECK=2
echo.
echo.
echo --------------------------------------------
echo Making fragment dimer model
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo Running modeller_fragment_dimer_salign.py
modeller_fragment_dimer_salign.py > modeller_fragment_dimer_salign.log
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo Running modeller_fragment_dimer_align2D.py
modeller_fragment_dimer_align2D.py > modeller_fragment_dimer_align2D.log
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo Running modeller_fragment_dimer_model-single.py
modeller_fragment_dimer_model-single.py > modeller_fragment_dimer_model-single.log
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo Renaming to model_fragment_dimer.pdb
ren query.?????????.pdb model_fragment_dimer.pdb
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo Making locked dimer model
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo Running modeller_locked_dimer_salign.py
modeller_locked_dimer_salign.py > modeller_locked_dimer_salign.log
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo Running modeller_locked_dimer_align2D.py
modeller_locked_dimer_align2D.py > modeller_locked_dimer_align2D.log
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo Running modeller_locked_dimer_model-single.py
modeller_locked_dimer_model-single.py > modeller_locked_dimer_model-single.log
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo Renaming to model_locked_dimer.pdb
ren query.?????????.pdb model_locked_dimer.pdb
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo Making full dimer model
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo Running modeller_dimer_salign.py
modeller_dimer_salign.py > modeller_dimer_salign.log
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo Running modeller_dimer_align2D.py
modeller_dimer_align2D.py > modeller_dimer_align2D.log
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo Running modeller_dimer_model-single.py
modeller_dimer_model-single.py > modeller_dimer_model-single.log
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo Renaming to model_dimer.pdb
ren query.?????????.pdb model_dimer.pdb
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo Running fragment and full monomer assessment
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo Running modeller_dimer_assessment.py
modeller_dimer_assessment.py > modeller_dimer_assessment.log
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo Running pisa_dimer.bat
CALL pisa_dimer.bat > pisa_dimer.log
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo Finished
echo --------------------------------------------
echo.
echo.