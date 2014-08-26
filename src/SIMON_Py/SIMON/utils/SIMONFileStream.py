__author__ = 'PARKJINSANG'


import os
import csv


def TextReadStream(fileName):
    fIn = open(fileName, 'r')
    text = fIn.read()
    fIn.close()
    return text

def TextWriteStream(fileName, text):
    fOut = open(fileName, 'w')
    fOut.write(text)
    fOut.close()

def TextAppendStream(fileName, text):
    fOut = open(fileName, 'a')
    fOut.write(text)
    fOut.close()

def ReadHistory(fileName):
    history = []
    csv_file = open(fileName, 'r')
    reader = csv.reader(csv_file)
    iter = 0
    for row in reader:
        if(iter is not 0):
            history.append(row)
        iter += 1
    return history

def WriteHistory(fileName, simonObject):
    if(not os.path.isfile(fileName)):
        nameList = []
        for key in simonObject.Properties.keys():
            nameList.append(key)
        csv_file = open(fileName, 'w')
        cwriter = csv.writer(csv_file, delimiter=',', quotechar='|', lineterminator='\n')
        cwriter.writerow(nameList)
    dataList = []
    for value in simonObject.Properties.values():
        dataList.append(value.PropertyValue)
    csv_file = open(fileName, 'a')
    cwriter = csv.writer(csv_file, delimiter=',', quotechar='|', lineterminator="\n")
    cwriter.writerow(dataList)
