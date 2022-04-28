import React from 'react';
const PokemonComponent = ({values, currentId,handleNext,handlePrevious})=>{
    return(
        <div>
            <div>Pokemon Page</div>
            <ul>
                <li> Pokemon ID: {values.id},</li>
                <li> Pokemon Name: {values.name},</li>
                <li> Pokemon height: {values.height},</li>
                <li> Pokemon weight: {values.weight},</li>
                <li><img src={values.frontImage}/>  <img src={values.backImage}/></li>
            </ul>
            <div>
                {currentId === 1 ?   <button onClick={handleNext}>Next</button>
                :   currentId === 896 ? <button onClick={handlePrevious}> Previous</button> :
                <> 
                    <button style={{marginLeft: "10px"}} onClick={handlePrevious}> Previous</button>
                    <button style={{marginLeft: "10px"}} onClick={handleNext}>Next</button>  
                </>
                }
            </div>
        </div>
    )
}
export default PokemonComponent;