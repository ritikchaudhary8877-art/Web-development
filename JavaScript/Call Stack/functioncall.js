function one() {
   two();
}

function two() {
   three();
}

function three() {
   console.log("Hello");
}

one();