import axios from "axios";
import { Cookies } from "react-cookie";
function Authorization() {
  const cookies = new Cookies();
  axios.interceptors.request.use(
    function (config) {
      let token = cookies.get('token');
      if (token !== null) {
        config.headers = {
          'Authorization': `Bearer ${token}`,
        }
      }
      return config;
    }
  );
}

export default Authorization;