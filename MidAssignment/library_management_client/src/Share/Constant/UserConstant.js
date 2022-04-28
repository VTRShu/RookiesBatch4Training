import CommonConstant from './CommonConstant';
const UserConstant = {
    Id: 'User Id',
    FullName: 'Full Name',
    Email: 'Email',
    Genders: 'Genders',
    DoB:"Date of Birth",
    Type:"Role",
    
    NewUserURL: `${CommonConstant.Server}/User`,
    UserProfileURL : `${CommonConstant.Server}/User?id=`,
    UserListURL :`${CommonConstant.Server}/User/list`,
    UserDisableURL:`${CommonConstant.Server}/User/disable?id=`,
    UserEdit :`${CommonConstant.Server}/User/edit?id=`,
    AllUser: `${CommonConstant.Server}/User/all`
}

export default UserConstant;