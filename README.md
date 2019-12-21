# Katrisky

A simple project to read in an aggregate report that summarizes output from Katrisk runs to facilitate the comparison of various experiments that facilitate the understanding of how Katrisk models levees.

To make this program run please follow these steps:
Launch cmd - go to the search icon and type in cmd, select cmd.exe and double click.

You should see an old fashioned command prompt. 

This program is written to do two different tasks, Simplify and Compare.

## Simplify

Simplify takes the csv files produced in post processing the output from Katrisk. It expects the data to have 4 columns, Name, return interval, AEP, and OEP. It gathers all records with the same name and converts the return interval into an exceedence probability, then it stores the OEP with that exceedence probability.

Then in a parallel process the program uses visvalingham Whyatt simplification to reduce the number of points in the EP curve to 500 or less (less if the EP curve started with less than 500 points). For each name in the file a single .csv file is written out. The file is named based on the name in the original csv, and has two columns of data x (probability) and y (OEP).

To run simplify, place the path to the Katrisk.exe (poor name choice on my part... sorry) compiled from this project - if there are spaces in the path be sure to put quotes around the path. then a space, then the word Simplify, then a path to the .csv file you wish to simplify, and finally an output directory. if the directory does not exist this program will create it.

in command prompt put:
"path to exe" Simplify "path to csv to simplify" "directory to store output"

## Compare
Compare takes two different Katrisk runs - each of which must be simplified - and compares any named outputs they have in common. It compares them at standard frequencies by computing a difference and dividing that by the larger of the two values at that probability (to avoid divide by zero). it then reports in the console the probabilty with the largest difference for each named location that has a difference. 

To run this process (which only takes a few seconds) make sure that the output directory has two simplified folders inside it. Then in command prompt put:
"path to exe" Compare "directory containing two sub directories"

if there are more than 2 sub directories the program only compares the first two it processes - so make sure there are only the directories you want to compare in the directory you specify.
