import axios from 'axios'
import UserConstant from '../Share/Constant/UserConstant'

export function GetTokenLoginService(){
    return axios.get(UserConstant.UserTokenURL);
}
