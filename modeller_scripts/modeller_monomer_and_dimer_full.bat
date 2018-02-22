@echo off
set HDF5_DISABLE_VERSION_CHECK=2
CALL modeller_monomer_full.bat
CALL modeller_dimer_full.bat
