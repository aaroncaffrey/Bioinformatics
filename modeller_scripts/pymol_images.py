import os
import sys
import pymol
from pymol import cmd

print("saving images...")
cmd.set("ray_trace_fog",1)
cmd.set("ray_shadows",1)
cmd.set("antialias",1)
#cmd.viewport(1200, 1200)
#cmd.reset()
#cmd.refresh()
#cmd.rebuild()
#cmd.zoom()


cmd.set("use_shaders", 1)
colors = ['red', 'green', 'blue', 'yellow', 'violet', 'cyan',    \
           'salmon', 'lime', 'pink', 'slate', 'magenta', 'orange', 'marine', \
           'olive', 'purple', 'teal', 'forest', 'firebrick', 'chocolate',    \
           'wheat', 'white', 'grey' ]
           
for file in os.listdir("."):
    if file.endswith(".pdb"):
        n = os.path.splitext(file)[0]
        cmd.load(file)
        cmd.hide("all")
        cmd.show("cartoon")
        i = 4
        for c in cmd.get_chains():
            if (c and c.strip()):
                print(n + " has chain " + c)
                cmd.color(colors[i], "chain " + c)
            else:
                cmd.color(colors[i], "chain *")
            i += 1
        print("Saving " + file + " as " + n + ".png")
        #cmd.png(n + ".png", 0, 0, 800)
        ###cmd.png(n + ".png", "3cm", "3cm", 600)
        ###cmd.png(n + ".png", 1200, 900, 300, 0, 0) #, 300, 0, 0)
        cmd.delete(n)

#print("saved pngs")
#cmd.quit()
