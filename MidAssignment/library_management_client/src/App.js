import logo from './logo.svg';
import './App.css';
import React, {useState} from 'react'
import { Layout } from 'antd';
import { Route, Routes,HashRouter} from "react-router-dom";
import { Cookies,CookiesProvider } from "react-cookie";
import { parseJwt } from './Share/parseJwt/parseJwt'
import CurrentUserContext from './Share/Context/CurrentUserContext'
import HeaderWeb from './Pages/Header/HeaderWeb';
import HomePage from './Pages/Home/HomePage';
import LoginPage from './Pages/Login/LoginPage';
import UserListPage from './Pages/User/List/UserListPage';
import RegisterPage from './Pages/User/RegisterPage';
import UserProfilePage from './Pages/User/UserProfilePage';
import CategoryPage from './Pages/Category/CategoryPage';
import BookPage from './Pages/Book/BookPage';
import BookBorrowingPage from './Pages/BookBorrowingRequest/BookBorrowingPage'
import BookPageCustomer from './Pages/Book/BookPageCustomer'
import FooterWeb from './Pages/Footer/FooterWeb'
const { Footer,Content } = Layout;
function App() {
  const cookies = new Cookies();
  const tokenDecryption = parseJwt(cookies.get('token'))
  const initialValues = {
    role: tokenDecryption ? tokenDecryption["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] : null,
    userId: tokenDecryption ? tokenDecryption["http://schemas.microsoft.com/ws/2008/06/identity/claims/userdata"].split(';')[0] : null,
    fullName : tokenDecryption ? tokenDecryption["http://schemas.microsoft.com/ws/2008/06/identity/claims/userdata"].split(';')[1] : null,
  }
 
  const [currentUser, setCurrentUser] = useState(initialValues);
  console.log(currentUser)
  return (

       
    <CurrentUserContext.Provider value={{ currentUser, setCurrentUser }}>
       <CookiesProvider>
         
         <Layout style={{ minHeight:'100vh'}}>
           <HeaderWeb /> 
             <Content  className="ant-layout-content">
               
          <Routes>
                <Route key="/" path="/" element={<HomePage/>}/>
                <Route key="/home" path="/home" element={<HomePage/>}/>
                <Route key="/bookview" path="/bookview" element={<BookPageCustomer/>}/>
             {(currentUser.role === null || currentUser.role === undefined)? 
             <>
                <Route key="/login" path="/login" element={<LoginPage/>}/>
                <Route key="/register" path="/register" element={<RegisterPage/>}/>
                <Route key="/categories" path="/categories" element={<LoginPage/>}/>
                <Route key="/users" path="/users" element={<LoginPage/>}/>
                <Route key="/books" path="/books" element={<LoginPage/>}/>
                <Route key="/borrowing-request" path="/borrowing-request" element={<LoginPage/>}/>
                <Route key="/profile" path="/profile" element={<LoginPage/>}/>
             </>
             :
             <>
                 <Route key="/users" path="/users" element={<UserListPage/>}/>
                 <Route key="/register" path="/register" element={<RegisterPage/>}/>
                 <Route key="/" path="/" element={<HomePage/>}/>
                 <Route key="/home" path="/home" element={<HomePage/>}/>
                 <Route key="/profile" path="/profile" element={<UserProfilePage/>}/>
                 <Route key="/categories" path="/categories" element={<CategoryPage/>}/>
                 <Route key="/books" path="/books" element={<BookPage/>}/>
                 <Route key="/borrowing-request" path="/borrowing-request" element={<BookBorrowingPage/>}/>
             </>
             }
            </Routes>

            </Content>
            <FooterWeb/>
         </Layout>
       </CookiesProvider>
   </CurrentUserContext.Provider>  
  );
}

export default App;
