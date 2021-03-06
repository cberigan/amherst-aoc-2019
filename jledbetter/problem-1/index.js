const masses = [
  132791,
  78272,
  114679,
  60602,
  59038,
  69747,
  61672,
  147972,
  92618,
  70186,
  125826,
  61803,
  78112,
  124864,
  58441,
  113062,
  105389,
  125983,
  90716,
  75544,
  148451,
  73739,
  127762,
  146660,
  128747,
  148129,
  138635,
  80095,
  60241,
  145455,
  98730,
  59139,
  146828,
  113550,
  91682,
  107415,
  129207,
  147635,
  104583,
  102245,
  73446,
  148657,
  96364,
  52033,
  69964,
  63609,
  98207,
  73401,
  65511,
  115034,
  126179,
  96664,
  85394,
  128472,
  79017,
  93222,
  55267,
  102446,
  133150,
  148985,
  95325,
  57713,
  77370,
  60879,
  111977,
  99362,
  91581,
  55201,
  137670,
  127159,
  128324,
  77217,
  86378,
  112847,
  108265,
  80355,
  75650,
  106222,
  67793,
  113891,
  74508,
  139463,
  69972,
  122753,
  135854,
  127770,
  101085,
  98304,
  61451,
  146719,
  61225,
  60468,
  83613,
  137436,
  126303,
  78759,
  70081,
  110671,
  113234,
  111563
];

const calc = val => {
  // Fuel required to launch a given module is based on its mass.
  // Specifically, to find the fuel required for a module, take its mass, divide by three, round down, and subtract 2.
  return Math.floor(val / 3) - 2;
};
const reducer = (accumulator, currentValue) => {
  return accumulator + currentValue;
};

const doItRecursiveLike = value => {
  let finalVal = 0;

  const recursiveCalc = val => {
    if (val >= 0) {
      finalVal = finalVal + val;
      recursiveCalc(calc(val));
    }
    return finalVal;
  };

  return recursiveCalc(value);
};

const runProblem1 = values => {
  return values.map(calc).reduce(reducer);
};

const runProblem2 = values => {
  return values
    .map(calc)
    .map(doItRecursiveLike)
    .reduce(reducer);
};

console.log(runProblem1(masses)); // 3317659
console.log(runProblem2(masses)); // 4973616
