import axios from 'axios';
import UserConstant from '../Share/Constant/UserConstant'

export function GetUserProfileService({id})
{
    return axios.get(`${UserConstant.UserProfileURL}${id}`)
}
export function GetListUserService({name,role,email,gender,index,size})
{
    return axios.get(UserConstant.UserListURL,{
        params:{
            name:name,
            email:email,
            gender:gender,
            role:role,
            pageSize : size,
            pageIndex : index,
        }
    })
}
export function DisableUserService({ id }) {
    return axios.put(`${UserConstant.UserDisableURL}${id}`);
}

export function CreateUserService({ fullName, dob, gender, type, userName, email,password}) {
    return axios.post(UserConstant.NewUserURL, {
      fullName: fullName,
      dob: dob,
      gender: gender,
      userName: userName,
      type: type,
      email: email,
      password: password,
    });
}

export function EditUserService({ id, fullName, dob, gender,email,userName,password }) {
    return axios.put(`${UserConstant.UserEdit}${id}`, {
      fullName: fullName,
      dob: dob,
      gender: gender,
      email: email,
      password: password,
      userName: userName,
    });
}

export function GetAllUserService(){
    return axios.get(`${UserConstant.AllUser}`)
}