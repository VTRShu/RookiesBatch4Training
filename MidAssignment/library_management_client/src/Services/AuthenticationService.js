import axios from "axios";
import AuthenticationConstant from "../Share/Constant/AuthenticationConstant";

export function LoginService({ userName, password, rememberMe }) {
  return axios.post(AuthenticationConstant.LoginURL, {
    userName: userName,
    password: password,
    rememberMe: rememberMe,
  });
}

export function ChangePasswordService({ oldPassword, newPassword }) {
  return axios.post(AuthenticationConstant.ChangePasswordURL, {
    oldPassword: oldPassword,
    newPassword: newPassword,
  });
}

