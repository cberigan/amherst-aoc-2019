import math
import pandas as pd
import xlrd

fileLocation = "C:\\Users\\jvd50\\Downloads\\"
fileName = 'Untitled Spreadsheet.xlsx'
masslist = pd.ExcelFile(fileLocation + fileName)
book = xlrd.open_workbook(masslist)
sheet = book.sheet_by_index(0)

def fuel(mass):
    count = 0
    while mass > 0:
        mass = mass // 3 - 2
        count += mass
        fuel(mass)
    return count
print(fuel(141923))

count = 0
for cell in sheet.col(0):
    mass = cell.value
    count += fuel(mass)

print(int(count))
