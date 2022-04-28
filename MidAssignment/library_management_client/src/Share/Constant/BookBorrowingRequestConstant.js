import CommonConstant from './CommonConstant';
const BookBorrowingRequestConstant = {
    Id:"Id",
    Requester:"Requester",
    RequestDate: "Request Date",
    Status:"Status",
    BooksRequested:"Books requested",
    Responder:"Responder",
    ResponseDate: "Response At",
    
    BookBorrowingListURL:`${CommonConstant.Server}/BookBorrowingRequest`,
    BookBorrowingRequestURL:`${CommonConstant.Server}/BookBorrowingRequest`,
    BookBorrowingRespondURL:`${CommonConstant.Server}/BookBorrowingRequest`
}

export default BookBorrowingRequestConstant;