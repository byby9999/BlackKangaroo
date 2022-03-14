exports = function(){
  var myVal = context.environment.values.MyValue;
  console.log("myVal");
  console.log(myVal);
  return myVal;
};