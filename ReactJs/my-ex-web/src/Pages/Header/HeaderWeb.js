import React,{useContext,useState} from "react"
import { Link } from "react-router-dom";
import CurrentUserContext from '../../Share/Context/CurrentUserContext'
import Cookies from 'universal-cookie';
import {Layout,Menu, Modal} from 'antd'
const { Header } = Layout;
const { SubMenu } = Menu;
const HeaderWeb = ()=>{
    const { currentUser, setCurrentUser } = useContext(CurrentUserContext);
    const cookies = new Cookies();
    const showLogOutModal = ()=>{
        Modal.confirm({
            title: "Are you sure, you want to logout?",
            okText: "Yes",
            centered:true,
            okType: "danger",
            onOk: () => {
                setCurrentUser({
                    token: null,
                    userId:null,
            });
            cookies.remove('token');
            cookies.remove('userId')
            },
        })
    }
    
    return(
        <>
        <CurrentUserContext.Provider value={{ currentUser, setCurrentUser }}>
            <Header>
                <Menu theme="dark" mode="horizontal">
                {currentUser.token === null || currentUser.token === undefined ?
                <>  
                    <Menu.Item style={{ marginLeft:'auto'}}><Link to="/login">Login</Link></Menu.Item>
                </>
                :   
                <>
                    <Menu.Item key="1"><Link to="/home">Home</Link></Menu.Item>
                    <Menu.Item key="2"><Link to="/posts">Posts</Link></Menu.Item>
                    <SubMenu key="sub1" style={{ marginLeft: 'auto' }} title={`Welcome ${currentUser.userId}`}>
                        <Menu.Item key="3"><Link to="/profile">Your Profile</Link></Menu.Item>
                        <Menu.Item key="4" onClick={showLogOutModal}>Logout</Menu.Item>
                    </SubMenu>
                </>
                 } 
                </Menu>
            </Header>
        </CurrentUserContext.Provider>
        </>
    )
}

export default HeaderWeb;