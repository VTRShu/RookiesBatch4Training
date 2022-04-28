import axios from 'axios'
import PostConstant from '../Share/Constant/PostConstant'

export function GetPostsService(){
    return axios.get(PostConstant.PostsListURL);
}

export function GetPostService({id}){
    return axios.get(`${PostConstant.PostsListURL}${id}`);
}



