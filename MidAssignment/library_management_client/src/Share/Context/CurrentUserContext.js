import { createContext } from "react";


const CurrentUserContext = createContext({
    fullName: null,
    userId: null,
    role: null
})

export default CurrentUserContext;