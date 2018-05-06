# Cheat Checker

# Team Members
Patrick Rottman
Andrew Kammerich
Nicole Dillon
Ethan Moss
Anand Kulkarni
Joe Timlin

## Description
#### This product is designed to help instructors catch people attempting to cheat on projects or assignments based around coding. The product takes in multiple files of code, compares them and checks to see if any of the students have copied or shared any code. 
## Features 
#### This product is able to decompress files that students have submitted. It downloads the file based around the names making it easier for the user. 

#### This product is able to check the code that the students have submitted. While the product runs it checks to see if there is similar code in any of the product that has been submitted by the students. The product runs through the files and is able to give the user a percentage of how much of the code has been copied and gives the names of the students who have copied each others work.  

## Process
#### The product first is designed to check the zip files that the user has downloaded. It creates a new folder with the names of the students names and is able to move the items from the zip file to the folders that have their name on it. This makes it easier for the user to distigush the files based on the names of the students. 

#### The product then opens the files of the students and looks for the .xaml.cs files which hold the code of the said students. Once it has opened the file the product looks for blank spaces, curly braces, comments etc. to ensure that it does not recognize those spaces as a something to compare with in the code. 

#### Once the product has opened it looks for the name of the student and stores it. This allows the product to give the user the name of the student that it compared. After this the product starts to look at the code in the format of strings and starts checking line by line to see which lines of code are the same. It compares one line at a time to all of the lines of the code of the other students products. The product compares the lines of code and is able to check if any of the lines are the same for many students. If the product recognizes any students that have the same code then it checks how much of the code is similar and outputs a percentage for the user. 

#### After it has finished doing this for all of the students the program then displays the name of the students who had similar code. With this the product displays the precentage of code that it noticed was the same for the students. 

#### The product also displays all of the lines that it has comapred to show the user how many lines of code were compared. 

## Image of Product 

![Image of Product](https://github.com/patrickrottman/4320FinalProject/blob/master/pastedImage.png)
