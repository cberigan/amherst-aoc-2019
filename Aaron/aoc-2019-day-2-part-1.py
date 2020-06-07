# List of inputs
list_of_inputs = [1,12,2,3,1,1,2,3,1,3,4,3,1,5,0,3,2,1,9,19,1,19,5,23,2,6,23,27,1,6,27,31,2,31,9,35,1,35,6,39,1,10,39,43,2,9,43,47,1,5,47,51,2,51,6,55,1,5,55,59,2,13,59,63,1,63,5,67,2,67,13,71,1,71,9,75,1,75,6,79,2,79,6,83,1,83,5,87,2,87,9,91,2,9,91,95,1,5,95,99,2,99,13,103,1,103,5,107,1,2,107,111,1,111,5,0,99,2,14,0,0]

# Creates a list of the first input position indexes to use for the opcode operation
input_position_1_list = []
for value in list_of_inputs[1::4]:
	input_position_1_list.append(value)

# Creates a list of the second input position indexes to use for the opcode operation
input_position_2_list = []
for value in list_of_inputs[2::4]:
	input_position_2_list.append(value)

# Creates a list of the desginated index position for the opcode result
opcode_result_position_list = []
for value in list_of_inputs[3::4]:
	opcode_result_position_list.append(value) 

print(f'opcode input 1: {input_position_1_list}')
print(f'opcode input 2: {input_position_2_list}')
print(f'opcode result positions: {opcode_result_position_list}')

# Iterates through list of inputs, leverages the lists that contain the opcode, input position 1, input position 2, and opcode result position
for (opcode, input_1_index, input_2_index, result_destination) in zip(list_of_inputs[::4], input_position_1_list, input_position_2_list, opcode_result_position_list):
    if opcode == 99:
        break
    elif opcode == 1:
        list_of_inputs[result_destination] = list_of_inputs[input_1_index] + list_of_inputs[input_2_index]
    elif opcode == 2:
        list_of_inputs[result_destination] = list_of_inputs[input_1_index] * list_of_inputs[input_2_index]

print(f'Final List: {list_of_inputs}')