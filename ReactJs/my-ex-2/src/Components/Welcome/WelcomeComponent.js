import React from 'react'

const WelcomeComponent = ({id, name, age, className,kind , getId, getGreeting}) => {
  
  // const { name, age } = props;
  return (
    <div className={className} key ={id} onClick={getId}>
      <h1>Hello {name} </h1>
      <h2>Age: {age}</h2>
      <h2>U are: {kind}</h2>
      <h2>{getGreeting(age)}</h2>
    </div>
  );
};



export default WelcomeComponent;
