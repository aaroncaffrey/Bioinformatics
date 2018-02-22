import os
import sys
from modeller import * 
from modeller.automodel import *
from modeller import soap_protein_od

try:
    tt = 'all'
    log.minimal()
    env = environ()
    a = automodel(env, alnfile=os.getcwd() + '\\template_locked_dimer_align2d.ali',knowns='template_locked_dimer_' + tt, sequence='query',assess_methods=None)
    a.starting_model = 1
    a.ending_model = 1
    a.make()
except:
    FILE = open('run_status',"w")
    FILE.write(str(sys.exc_info()[0]))
    FILE.close()
    print("Unexpected error:", sys.exc_info()[0])
