import axios from 'axios';
import UserConstant from '../Share/Constant/UserConstant'

export function GetUserProfileService({id})
{
    return axios.get(`${UserConstant.UserProfileURL}${id}`)
}
