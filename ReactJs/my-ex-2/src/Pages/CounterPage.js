import React, { useState, useEffect } from "react";
import CounterComponent from "../Components/Counter/CounterComponent";

const CounterPage = () => {
  const [value, setValue] = useState(0);
  const handleMinus = () => {
    if (value > 0) {
      setValue(value - 1);
    }
  };
  const handlePlus = () => {
    setValue(value + 1);
  };
  useEffect(()=>{
    document.title = `Counter value: ${value}`;
  })
  return (
    <CounterComponent
      value={value}
      handlePlus={handlePlus}
      handleMinus={handleMinus}
    />
  );
};

export default CounterPage;
