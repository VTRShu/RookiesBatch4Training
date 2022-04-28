import axios from 'axios';
import CategoryConstant from '../Share/Constant/CategoryConstant'

export function GetCategoryDetailService({id})
{
    return axios.get(`${CategoryConstant.CategoryDetailURL}${id}`)
}
export function GetListCategoryService({index,size})
{
    return axios.get(CategoryConstant.CategoryListURL,{
        params:{
            pageSize : size,
            pageIndex : index,
        }
    })
}
export function DeleteCategoryService({ id }) {
    return axios.delete(`${CategoryConstant.CategoryDeleteURL}${id}`);
}

export function CreateCategoryService({ categoryName}) {
    return axios.post(CategoryConstant.NewCategoryURL, {
      categoryName : categoryName
    });
}

export function EditCategoryService({ categoryName,id }) {
    return axios.put(`${CategoryConstant.CategoryEditURL}${id}`, {
      categoryName : categoryName
    });
}
export function GetAllCategoryService(){
    return axios.get(`${CategoryConstant.CategoryAllURL}`)
}
