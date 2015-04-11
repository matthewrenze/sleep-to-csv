Sleep to CSV
==============

A simple console application that converts Sleep as Android export files to .csv files.

Usage: SleepToCsv.exe source-file target-folder

The parameters are:
- source-file - the Sleep as Android export file
- target-folder - the folder to write the .csv files to

The output files are:
 - Summary.csv - contains the nightly summary data
 - Movements.csv - contains the minute-by-minute movement and noise data
 - Events.csv - contains a log of all events

Example: SleepToCsv.exe "C:\Source\sleep-export.csv" "C:\Target"

You can download the executable from the following URL:
http://www.matthewrenze.com/software/sleep-to-csv.zip
