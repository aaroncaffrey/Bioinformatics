import os
import sys
from modeller import * 

try:
    tt = 'all'
    log.minimal()
    env = environ()
    env.io.atom_files_directory = './:../atom_files/'

    aln = alignment(env)
    
    mdl = model(env, file=os.getcwd() + '\\template_locked_ligand_' + tt + '.pdb')
    aln.append_model(mdl, atom_files='template_locked_ligand_' + tt, align_codes='template_locked_ligand_' + tt)

    for (weights, write_fit, whole) in (((1., 0., 0., 0., 1., 0.), False, True),
                                        ((1., 0.5, 1., 1., 1., 0.), False, True),
                                        ((1., 1., 1., 1., 1., 0.), True, False)):
        aln.salign(rms_cutoff=3.5, normalize_pp_scores=False,
                   rr_file='$(LIB)/as1.sim.mat', overhang=30,
                   gap_penalties_1d=(-450, -50),
                   gap_penalties_3d=(0, 3), gap_gap_score=0, gap_residue_score=0,
                   dendrogram_file=os.getcwd() + '\\template_locked_ligand_salign.tree',
                   alignment_type='tree', # If 'progresive', the tree is not
                                          # computed and all structues will be
                                          # aligned sequentially to the first
                  feature_weights=weights, # For a multiple sequence alignment only
                                            # the first feature needs to be non-zero
                  improve_alignment=True, fit=True, write_fit=write_fit,
                  write_whole_pdb=whole, output='ALIGNMENT QUALITY')

    aln.write(file=os.getcwd() + '\\template_locked_ligand_salign.pap', alignment_format='PAP')
    aln.write(file=os.getcwd() + '\\template_locked_ligand_salign.ali', alignment_format='PIR')

    aln.salign(rms_cutoff=1.0, normalize_pp_scores=False,
               rr_file='$(LIB)/as1.sim.mat', overhang=30,
              gap_penalties_1d=(-450, -50), gap_penalties_3d=(0, 3),
               gap_gap_score=0, gap_residue_score=0, dendrogram_file=os.getcwd() + '\\template_locked_ligand_salign.tree',
               alignment_type='progressive', feature_weights=[0]*6,
               improve_alignment=False, fit=False, write_fit=True,
               write_whole_pdb=False, output='QUALITY')

except:
    FILE = open('run_status',"w")
    FILE.write(str(sys.exc_info()[0]))
    FILE.close()
    print("Unexpected error:", sys.exc_info()[0])
