REM @echo off

set dimer_id=%RANDOM%
C:\CCP4-7\7.0\bin\pisa.exe model_dimer_%dimer_id% -analyse model_monomer.pdb 
C:\CCP4-7\7.0\bin\pisa.exe model_dimer_%dimer_id% -list monomers > pisa_monomers_model_monomer.log
C:\CCP4-7\7.0\bin\pisa.exe model_dimer_%dimer_id% -list interfaces > pisa_interfaces_model_monomer.log
C:\CCP4-7\7.0\bin\pisa.exe model_dimer_%dimer_id% -erase 

set dimer_id=%RANDOM%
C:\CCP4-7\7.0\bin\pisa.exe model_dimer_%dimer_id% -analyse model_monomer_Repair.pdb 
C:\CCP4-7\7.0\bin\pisa.exe model_dimer_%dimer_id% -list monomers > pisa_monomers_model_monomer_Repair.log
C:\CCP4-7\7.0\bin\pisa.exe model_dimer_%dimer_id% -list interfaces > pisa_interfaces_model_monomer_Repair.log
C:\CCP4-7\7.0\bin\pisa.exe model_dimer_%dimer_id% -erase

