import React from 'react';
import "./Counter.css";

const CounterComponent = ({ value, handleMinus, handlePlus }) => {
  return (
    <div className="counter">
      <button className="btn-counter" onClick={handleMinus}>
        -
      </button>
      <span className="counter-value">{value}</span>
      <button className="btn-counter" onClick={handlePlus}>
        +
      </button>
    
    </div>
  );
};

export default CounterComponent;
