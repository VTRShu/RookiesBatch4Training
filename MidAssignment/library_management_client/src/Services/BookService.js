import axios from 'axios';
import BookConstant from '../Share/Constant/BookConstant'

export function GetBookDetailService({id})
{
    return axios.get(`${BookConstant.BookDetailURL}${id}`)
}
export function GetListBookService({index,size})
{
    return axios.get(BookConstant.BookListURL,{
        params:{
            pageSize : size,
            pageIndex : index,
        }
    })
}
export function DeleteBookService({ id }) {
    return axios.delete(`${BookConstant.BookDeleteURL}${id}`);
}

export function CreateBookService({ categoryId,name,description,coverSrc}) {
    return axios.post(BookConstant.NewBookURL, {
        categoryId : categoryId,
        name: name,
        description: description,
        coverSrc: coverSrc
    });
}

export function EditBookService({ categoryId,name,description,id,coverSrc }) {
    return axios.put(`${BookConstant.BookEditURL}${id}`, {
        categoryId : categoryId,
        name: name,
        description: description,
        coverSrc: coverSrc
    });
}
export function GetAllBookService(){
    return axios.get(`${BookConstant.BookAllURL}`)
}