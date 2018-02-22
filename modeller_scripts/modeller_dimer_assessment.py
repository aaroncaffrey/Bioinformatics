import os
import sys
from modeller import *
from modeller.scripts import complete_pdb
from modeller import soap_protein_od
import os.path

log.verbose()    # request verbose output
env = environ()
env.libs.topology.read(file='$(LIB)/top_heav.lib') # read topology
env.libs.parameters.read(file='$(LIB)/par.lib') # read parameters

# Set up SOAP-Protein-OD scoring (note: if assessing multiple models, it is
# best to create 'sp' just once and keep it around, since reading in the
# potential from disk can take a long time).
sp = soap_protein_od.Scorer()

# only calculate for single chains, because ga341 and dope only calculate first chains

models = ['model_dimer.pdb', 'model_locked_dimer.pdb', 'model_fragment_dimer.pdb']
templates = ['template_dimer_all.pdb', 'template_fragment_dimer_all.pdb']
files = models + templates

for f in files:
    if (not os.path.isfile(f)):
        continue
    print("; FILE " + f)
    # read model file
    mdl = complete_pdb(env, f)
    
    if mdl.seq_id < 0 or mdl.seq_id > 100:
        mdl.seq_id = 100
        
    # Assess all atoms with DOPE:
    #s = selection(mdl)
    #print("calling assess_ga341 mdl")
    #score_ga341 = mdl.assess_ga341()
    #print("; ENERGY assess_ga341 " + str(score_ga341))
    
    #print("calling assess_normalized_dope mdl")
    #score_normalized_dope = mdl.assess_normalized_dope()
    #print("; ENERGY assess_normalized_dope " + str(score_normalized_dope))
    
    for c in mdl.chains:
        print("; CHAIN " + c.name)
        atmsel = selection(c)
        
        #print("calling energy " + c.name)
        #(molpdf, terms) = atmsel.energy(edat=energy_data(dynamic_sphere=True))
        #print("; ENERGY molpdf " + str(molpdf))
        
        print("calling assess_dopehr " + c.name)
        score_dope_hr = atmsel.assess_dopehr()
        print("; ENERGY assess_dopehr " + str(score_dope_hr))
        
        print("calling assess_dope " + c.name)
        score_dope = atmsel.assess_dope()
        print("; ENERGY assess_dope " + str(score_dope))
                
        print("calculating soap for chain " + c.name)
        score_soap = atmsel.assess(sp)
        print("; ENERGY assess_soap_protein_od " + str(score_soap))
        
        print("; FINISHED " + c.name)
