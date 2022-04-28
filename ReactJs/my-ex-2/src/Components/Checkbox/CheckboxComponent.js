import React from "react";

const CheckboxComponent = ({ handleSubmit,values,handleOnChange,all,handleAllChange }) => {
  return (
    <div className ="checkbox">
      <p>Choose your interest</p>
      <form>
        <div><label ><input type="checkbox" onChange={handleAllChange} checked={all}  value="all" /> All </label></div><br/>

        <div><label ><input type="checkbox" checked={values.coding} name="coding" value="coding" onChange={handleOnChange} />Coding</label></div><br/>

        <div><label ><input type="checkbox" checked={values.music} name="music" value="music" onChange={handleOnChange} /> Music</label></div><br/>

        <div><label ><input type="checkbox" checked={values.reading} name="reading" value="reading" onChange={handleOnChange} /> Reading Books</label></div><br/>
        <button type="submit" onClick={handleSubmit}>Submit</button>
      </form>
    </div>
  );
};

export default CheckboxComponent;
