import React from 'react';

const LoadingComponent = (name)=>{
    return(
    <div className="loadingIcon">
        <div class="spinner-border text-primary" role="status">
        <span class="sr-only">Loading...</span>
        </div>
    </div>
    )
}
export default LoadingComponent;