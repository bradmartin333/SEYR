import os
import glob
from PIL import Image
import glob
import os

row = 1
col = 1
square = 10

files = glob.glob(os.getcwd() + '/*.png')
for file in files:
    if col == square+1:
        row += 1
        col = 1
    path = file.split('\\')
    path = '\\'.join(path[:-1])
    newpath = path + '\\output\_R' + str(row) + '_C' + str(col) + '.png'
    col += 1
    baseheight = 600
    img = Image.open(file)
    hpercent = (baseheight/float(img.size[1]))
    wsize = int((float(img.size[0])*float(hpercent)))
    img = img.resize((wsize,baseheight), Image.ANTIALIAS)
    img.save(newpath)
    print(newpath)
