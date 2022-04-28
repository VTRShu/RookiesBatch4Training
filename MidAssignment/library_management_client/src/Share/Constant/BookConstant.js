import CommonConstant from './CommonConstant';
const BookConstant = {
   Id: 'ID',
   CategoryId: 'Category',
   Name: 'Name',
   Description: 'Description',
   PublishedAt: 'Publish At',

   NewBookURL:`${CommonConstant.Server}/Book`,
   BookEditURL : `${CommonConstant.Server}/Book?id=`,
   BookDetailURL:`${CommonConstant.Server}/Book?id=`,
   BookDeleteURL:`${CommonConstant.Server}/Book?id=`,
   BookListURL : `${CommonConstant.Server}/Book/list`,
   BookAllURL:`${CommonConstant.Server}/Book/all`
}

export default BookConstant;