import React from "react";
import WelcomeComponent from "../Components/Welcome/WelcomeComponent";

const WelcomePage = ({ className }) => {
  const getId = (event) => {
    console.log(event.target.id);
  }
  const people = [
    {id: 1,age: 17, name: "Nghia", kind: "weeboo", className: "color-red"},
    {id: 2, age: 20, name: "Son Tung", kind: "Singer", className: "color-green"},
  ]
  const getGreeting = (age) => {
    return age > 18 ? "Men make house, women make home" : "You're only young once";
  };
  return (
    <div>
      {
        people.map(person =>{
          return(
            <WelcomeComponent
            getId = {getId}
            name={person.name}
            kind={person.kind}
            age={person.age}
            className={person.className}
            key = {person.id}
            getGreeting = {getGreeting}/>
          )
      })
    }
  </div>);
};

export default WelcomePage;
