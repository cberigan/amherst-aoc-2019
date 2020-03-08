# Warning, this will take awhile to run but will produce both answers.  I was lazy, but could rewrite to use intersection
# math.  This would make it run nearly instantly.  Answer for question 2 is the smallest in list 6.

list1 = []
list2 = []

file = open('C:\\Users\\jvd50\\Desktop\\aoc3file1.txt')
for line in file:
    list1 = line.split(',')
file.close()

file2 = open('C:\\Users\\jvd50\\Desktop\\aoc3file2.txt')
for line in file2:
    list2 = line.split(',')
file2.close()


def movement(lst):
    coords = []
    x = 0
    y = 0
    for item in lst:
        if item[0] == 'R':
            for i in range(1,int(item[1:]) + 1):
                x += 1
                coords.append((x,y))
        elif item[0] == 'L':
            for i in range(1, int(item[1:]) + 1):
                x -= 1
                coords.append((x,y))
        elif item[0] == 'U':
            for i in range(1, int(item[1:]) + 1):
                y += 1
                coords.append((x,y))
        elif item[0] == 'D':
            for i in range(1, int(item[1:]) + 1):
                y -= 1

                coords.append((x,y))
        else:
            continue
    return coords

list3 = []
list4 = []
list5 = []
list6 = []
manval = 10000
newval = 0

list3 = movement(list1)
list4 = movement(list2)

for item in list3:
    if (item in list4):
        list5.append(item)
        list6.append(list3.index(item) + list4.index(item) + 2)


for item in list5:
    newval = abs(item[0]) + abs(item[1])
    if newval < manval:
        manval = newval
    else:
        continue

print(manval)
print(list5)
print(list6)




