import React from 'react';
import { FaSpinner } from 'react-icons/fa';

const LoadingComponent = ()=>{
    return(
    <div className="loadingIcon">
        <FaSpinner 
            icon="spinner" 
            className="spinner"  
            style={{  position:"relative",top: "calc(50% - 10px)"}}/>
    </div>
    )
}
export default LoadingComponent; 