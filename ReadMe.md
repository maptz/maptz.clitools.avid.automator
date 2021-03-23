# Maptz.Avid.Automation.Tool

A tool used to help automate the process of creating select sequences in Avid Media Composer based on Avid Markers. 

The tool is run from the CLI. When run, it asks for the path to a text based Avid marker file. It then runs simulates a sequences of keypresses to pull out 10 second clips around the marker point. 

## Instructions for use. 

1) Open up Avid Media Composer, and load the sequence you would like to pull in the source window. 
2) Still in Avid, open the Marker window and export the markers as a `text` file.
3) Run the `Maptz.Avid.Automation.Tool` in a CLI window. 
4) Navigate back to the Avid and create an empty target sequence in the record window. 
5) Now click on the source monitor to activate the source monitor. 
6) Press `Alt+A` to initiate the `Maptz.Avid.Automation.Tool`.
7) The tools will now show an file selection dialog. Select the file you exported in 2) above.
8) Press `Open` in the file open dialog.
9) The tool will now automate a series of keypresses designed to insert edit from the source sequence to the record sequence, encompassing each marker in the file. 
10) To cancel the operation at any time, press `Alt+B`.

## Bugs and Features

The Changelog can be found [here](CHANGELOG.md).

Please log any bugs on Github [here](https://github.com/maptz/maptz.clitools.avid.automator/issues).

## Source Code

The source code is available on GitHub [here](https://github.com/maptz/maptz.clitools.avid.automator).
