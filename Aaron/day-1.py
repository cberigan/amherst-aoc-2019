main_list = []

with open('C:\\Users\\asang\\Downloads\\input.txt', 'r') as sum_file:
	for line in sum_file:
		stripped = line.rstrip('\n')
		main_list.append(stripped)

final_list = []

for item in main_list:
	a = int(item)
	a = int((a / 3) - 2)
	final_list.append(a)

print(sum(final_list))