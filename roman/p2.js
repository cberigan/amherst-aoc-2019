let modules = [104042,
112116,
57758,
139018,
105580,
148312,
139340,
62939,
50925,
63881,
138725,
54735,
54957,
103075,
55457,
98808,
87206,
58868,
120829,
124551,
63631,
103358,
149501,
147414,
59731,
88284,
59034,
116206,
52299,
119619,
63648,
85456,
110391,
90254,
99360,
59529,
82628,
82693,
64331,
123779,
123064,
93600,
104226,
74068,
74354,
149707,
51503,
130433,
80778,
72279,
148782,
113454,
138409,
148891,
79257,
126927,
141696,
107136,
66200,
120929,
149350,
76952,
134002,
62354,
144559,
125186,
85169,
61662,
90252,
147774,
101960,
55254,
96885,
88249,
133866,
121809,
103675,
94407,
59078,
81498,
82547,
132599,
81181,
141685,
73476,
107700,
133314,
77982,
149270,
119176,
148255,
81023,
143938,
54348,
121790,
126521,
101123,
139921,
51152,
97943]

const getFuelCost = (mass) => {
  let unrounded = mass/3
  let remainder = (mass/3)%1
  return (unrounded-remainder)-2
}
  

const getFuelCosts = (modules) => modules.map(module => getFuelCost(module))
  
let fuelCosts = getFuelCosts(modules)

const getCostOfCost = (cost) =>{
  getCostOfCostRec = (cost, sum) => {
    if(cost < 1){
      return sum
    } else {
      let costOfCost = getFuelCost(cost)
      costOfCost = costOfCost > 0 ? costOfCost : 0
      return getCostOfCostRec(costOfCost, sum+costOfCost)
    }
  }

  return getCostOfCostRec(cost, cost)
}

console.log(fuelCosts.reduce((cost, currentModule) => cost += getCostOfCost(currentModule), 0))