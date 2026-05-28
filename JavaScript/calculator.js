function add(){
 var num1=document.getElementById("num1").value;
 var num2=document.getElementById("num2").value;
 var result= Number(num1)+Number(num2);

  document.getElementById("num3").value=result; 
}

function sub(){
 var num1=document.getElementById("num1").value;
 var num2=document.getElementById("num2").value;
 var result= Number(num1)-Number(num2);
console.log(result);
  document.getElementById("num3").value = result; 
}

function mult(){
 var num1=document.getElementById("num1").value;
 var num2=document.getElementById("num2").value;
 var result= Number(num1)*Number(num2);

  document.getElementById("num3").value=result; 
}
function div(){
 var num1=document.getElementById("num1").value;
 var num2=document.getElementById("num2").value;
 var result= Number(num1)/Number(num2);

  document.getElementById("num3").value=result; 
}
