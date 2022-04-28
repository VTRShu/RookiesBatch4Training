import React, { useState, useEffect } from "react";
import CheckboxComponent from "../Components/Checkbox/CheckboxComponent";

const CheckboxPage = () => {
    const handleSubmit=(evt)=>{
        evt.preventDefault();
        console.log("Oh lala");
    }
    const [values,setValues] = useState({
        coding: false,
        music: false,
        reading: false,
    })
    const handleOnChange=(evt)=>{
        setValues({
            ...values,
            [evt.target.value] : evt.target.checked
        });
        console.log("values: ", values)
    }
    const all = values.coding && values.music && values.reading;
    const handleAllChange=(evt)=>{
        setValues({
            coding:evt.target.checked,
            music:evt.target.checked,
            reading:evt.target.checked
        })
    }
 return (
     <CheckboxComponent
        all = {all}
        handleSubmit = {handleSubmit}
        values={values}
        handleOnChange = {handleOnChange}
        handleAllChange = {handleAllChange}
     />
 )
};

export default CheckboxPage;
