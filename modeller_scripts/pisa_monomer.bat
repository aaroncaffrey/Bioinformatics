@echo off
echo.
echo.
echo --------------------------------------------
echo ; pisa                template_fragment_ligand_all.pdb
C:\CCP4-7\7.0\bin\pisa.exe template_fragment_ligand_all -analyse template_fragment_ligand_all.pdb
C:\CCP4-7\7.0\bin\pisa.exe template_fragment_ligand_all -list monomers
C:\CCP4-7\7.0\bin\pisa.exe template_fragment_ligand_all -list interfaces
C:\CCP4-7\7.0\bin\pisa.exe template_fragment_ligand_all -erase
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo ; pisa                template_ligand_all.pdb
C:\CCP4-7\7.0\bin\pisa.exe template_ligand_all -analyse template_ligand_all.pdb
C:\CCP4-7\7.0\bin\pisa.exe template_ligand_all -list monomers
C:\CCP4-7\7.0\bin\pisa.exe template_ligand_all -list interfaces
C:\CCP4-7\7.0\bin\pisa.exe template_ligand_all -erase
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo ; pisa                model_fragment_monomer.pdb
C:\CCP4-7\7.0\bin\pisa.exe model_fragment_monomer -analyse model_fragment_monomer.pdb
C:\CCP4-7\7.0\bin\pisa.exe model_fragment_monomer -list monomers
C:\CCP4-7\7.0\bin\pisa.exe model_fragment_monomer -list interfaces
C:\CCP4-7\7.0\bin\pisa.exe model_fragment_monomer -erase
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo ; pisa                model_locked_monomer.pdb
C:\CCP4-7\7.0\bin\pisa.exe model_locked_monomer -analyse model_locked_monomer.pdb
C:\CCP4-7\7.0\bin\pisa.exe model_locked_monomer -list monomers
C:\CCP4-7\7.0\bin\pisa.exe model_locked_monomer -list interfaces
C:\CCP4-7\7.0\bin\pisa.exe model_locked_monomer -erase
echo --------------------------------------------
echo.
echo.
echo --------------------------------------------
echo ; pisa                model_monomer.pdb
C:\CCP4-7\7.0\bin\pisa.exe model_monomer -analyse model_monomer.pdb
C:\CCP4-7\7.0\bin\pisa.exe model_monomer -list monomers
C:\CCP4-7\7.0\bin\pisa.exe model_monomer -list interfaces
C:\CCP4-7\7.0\bin\pisa.exe model_monomer -erase
echo --------------------------------------------
echo.
echo.
