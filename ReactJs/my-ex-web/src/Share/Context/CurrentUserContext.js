import { createContext } from "react";


const CurrentUserContext = createContext({
    token:null,
    userId:null,
})

export default CurrentUserContext;