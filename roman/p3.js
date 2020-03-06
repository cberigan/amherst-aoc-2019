let program = [1,0,0,3,1,1,2,3,1,3,4,3,1,5,0,3,2,6,1,19,1,19,10,23,
  2,13,23,27,1,5,27,31,2,6,31,35,1,6,35,39,2,39,9,43,1,5,43,47,1,13,
  47,51,1,10,51,55,2,55,10,59,2,10,59,63,1,9,63,67,2,67,13,71,1,71,6,
  75,2,6,75,79,1,5,79,83,2,83,9,87,1,6,87,91,2,91,6,95,1,95,6,99,2,99,
  13,103,1,6,103,107,1,2,107,111,1,111,9,0,99,2,14,0,0]

const ADD = 1
const MULT = 2
const HALT = 99

const RUN_PROGRAM = (INPUT, NOUN, VERB) => {
  let OPCODE_ADDR = 0
  let OPCODE
  let ADDR1, ADDR2, RESULT_ADDR

  INPUT[1] = NOUN
  INPUT[2] = VERB
  
  while(OPCODE_ADDR <= INPUT.length-1){
    OPCODE = INPUT[OPCODE_ADDR]
    ADDR1 = INPUT[OPCODE_ADDR + 1]
    ADDR2 = INPUT[OPCODE_ADDR + 2]
    RESULT_ADDR = INPUT [OPCODE_ADDR + 3]

    if(OPCODE == HALT){
      break
    } else if(OPCODE == ADD){
     INPUT[RESULT_ADDR] = INPUT[ADDR1] + INPUT[ADDR2]
    } else if(OPCODE == MULT){
      INPUT[RESULT_ADDR] = INPUT[ADDR1] * INPUT[ADDR2]
    }

    OPCODE_ADDR += 4
  }

  return INPUT
}

console.log(RUN_PROGRAM(program, 12, 2)[0])

module.exports = RUN_PROGRAM