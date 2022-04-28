import axios from 'axios';
import BookBorrowingRequestConstant from '../Share/Constant/BookBorrowingRequestConstant'


export function GetListBookBorrowingService({index,size})
{
    return axios.get(BookBorrowingRequestConstant.BookBorrowingListURL,{
        params:{
            pageSize : size,
            pageIndex : index,
        }
    })
}
export function RespondRequestService({ requestId,respond }) {
    return axios.put(`${BookBorrowingRequestConstant.BookBorrowingRespondURL}?requestId=${requestId}&respond=${respond}`);;
}

export function CreateRequestService({ books}) {
    return axios.post(BookBorrowingRequestConstant.BookBorrowingRequestURL, {
        books: books
    });
}
