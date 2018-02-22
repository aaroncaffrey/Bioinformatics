set thiscd=%cd%

set atomtype=-
REM ca mc sc -

set contactdist=4.0
set flanking=0
set minlen=2
set density=0.5
set seqfile=%thiscd%\seq.fasta
set maxcluster_csv=-
set dsspfile=c:\dssp\
set stridefile=c:\stride\
set chainids=-
set overwrite=Y

set atomsdir=%thiscd%\atoms_%atomtype%
set contactsdir=%thiscd%\contacts_%atomtype%_%contactdist%
set interfacesdir=%thiscd%\interfaces_%atomtype%_%contactdist%_%flanking%_%minlen%_%density%

md %atomsdir%
md %contactsdir%
md %interfacesdir%

set extract_atoms=0
set extract_contacts=0
set extract_interfaces=1

REM EXTRACT ATOMS

if "%extract_atoms%"=="1" (
	cd C:\Users\aaron.PRMB2066-AA\Documents\ProteinBioinformaitcsToolkit\ComplexAtoms\bin\x64\Release\
	for /f %%f in ('dir /b %thiscd%\*.pdb') do ComplexAtoms %thiscd%\%%f %atomtype% %chainids% %atomsdir%\%%f"
)

REM EXTRACT CONTACTS

if "%extract_contacts%"=="1" (
	cd C:\Users\aaron.PRMB2066-AA\Documents\ProteinBioinformaitcsToolkit\ComplexContacts\bin\x64\Release\
	for /f %%f in ('dir /b %thiscd%\*.pdb') do ComplexContacts %atomsdir%\%%f %contactdist% "%contactsdir%\%%f" %overwrite%
)

REM EXTRACT INTERFACES

if "%extract_interfaces%"=="1" (
	cd C:\Users\aaron.PRMB2066-AA\Documents\ProteinBioinformaitcsToolkit\ComplexInterfaces\bin\x64\Release\
	for /f %%f in ('dir /b %thiscd%\*.pdb') do ComplexInterfaces %seqfile% %atomsdir%\%%f %contactsdir%\%%f %maxcluster_csv% %dsspfile% %stridefile% %chainids% %contactdist% %minlen% %flanking% %density% %interfacesdir%\interfaces_%%f.csv  %interfacesdir%\interface-interface_%%f.csv %overwrite%
)

pause
