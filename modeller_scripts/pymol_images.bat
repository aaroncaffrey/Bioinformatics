@echo off
set PATH=%PATH%;C:\Program Files\PyMOL\PyMOL\;
set PYMOL_PATH=C:\Program Files\PyMOL\PyMOL\
set TCL_LIBRARY=C:\Program Files\PyMOL\PyMOL\py27\tcl\tcl8.4\
set PYTHONHOME=C:\Program Files\PyMOL\PyMOL\py27\
REM pymol -e -r pymol_images.py
pymol -r pymol_images.py
REM pause