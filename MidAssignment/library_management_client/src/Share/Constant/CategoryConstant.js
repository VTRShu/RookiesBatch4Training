import CommonConstant from './CommonConstant';
const CategoryConstant = {
    ID : "Id",
    CategoryName:"Category Name",
    CreateAt:"Create At",


    NewCategoryURL: `${CommonConstant.Server}/Category`,
    CategoryEditURL : `${CommonConstant.Server}/Category?id=`,
    CategoryDetailURL:`${CommonConstant.Server}/Category?id=`,
    CategoryDeleteURL:`${CommonConstant.Server}/Category?id=`,
    CategoryListURL : `${CommonConstant.Server}/Category/list`,
    CategoryAllURL:`${CommonConstant.Server}/Category/all`
}

export default CategoryConstant;