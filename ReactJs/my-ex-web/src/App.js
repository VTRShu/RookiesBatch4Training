import React,{useState} from 'react';
import './App.css';
import { Layout } from 'antd';
import { Route, Routes,} from "react-router-dom";
import PostsPage from './Pages/Post/Posts/PostsPage';
import LoginPage from './Pages/Login/LoginPage'
import PostDetailPage from './Pages/Post/PostDetail/PostDetailPage'
import ProfilePage from './Pages/Profile/ProfilePage'
import { Cookies,CookiesProvider } from "react-cookie";
import CurrentUserContext from './Share/Context/CurrentUserContext'
import HeaderWeb from './Pages/Header/HeaderWeb'
import HomePage from './Pages/Home/HomePage'
const { Footer,Content } = Layout;
function App() {
  const cookies = new Cookies();
  const initialValues = {
    token: cookies.get('token'),
    userId: cookies.get('userId')
  }
  const [currentUser,setCurrentUser] = useState(initialValues)
  return (
   <>
     <CurrentUserContext.Provider value={{ currentUser, setCurrentUser }}>
        <CookiesProvider>
          <Layout style={{ minHeight:'100vh'}}>
            <HeaderWeb /> 
              <Content  className="ant-layout-content">
              {(currentUser.token === null || currentUser.token === undefined)? 
              <>
                <LoginPage/>
              </>
              :
              <>
                <Routes>
                  <Route key="/" path="/" element={<HomePage/>}/>
                  <Route key="/home" path="/home" element={<HomePage/>}/>
                  <Route key="/posts" path="/posts" element={<PostsPage/>}/>
                  <Route key="/post/:id"path="/post/:id" element={<PostDetailPage/>}/>
                  <Route key="/profile"path="/profile" element={<ProfilePage/>}/>
                </Routes>
              </>
              }
             </Content>
            <Footer className="ant-layout-footer">Created by Le Trung Nghia</Footer>
          </Layout>
        </CookiesProvider>
    </CurrentUserContext.Provider>
   </>
  );
}

export default App;
