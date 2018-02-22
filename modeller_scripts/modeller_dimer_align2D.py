import os
import sys
from modeller import * 

log.minimal()
try:
    tt = 'all'
    env = environ()
    aln = alignment(env)
    mdl = model(env, file=os.getcwd() + '\\template_dimer_' + tt + '.pdb')
    aln.append_model(mdl, align_codes='template_dimer_' + tt, atom_files='template_dimer_' + tt + '.pdb')
    aln.append(file=os.getcwd() + '\\template_dimer_after_substitution.ali', align_codes='query')

    aln.align2d()
    aln.write(file=os.getcwd() + '\\template_dimer_align2d.ali', alignment_format='PIR')
    aln.write(file=os.getcwd() + '\\template_dimer_align2d.pap', alignment_format='PAP')

except:
    FILE = open('run_status',"w")
    FILE.write(str(sys.exc_info()[0]))
    FILE.close()
    print("Unexpected error:", sys.exc_info()[0])
