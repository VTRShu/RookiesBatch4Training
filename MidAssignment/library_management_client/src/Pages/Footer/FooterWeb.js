import React from "react";
import { Link } from "react-router-dom";
import { AiFillFacebook, AiFillYoutube, AiFillInstagram, AiFillLinkedin } from 'react-icons/ai';
import { Layout } from "antd";
import './FooterWeb.css'
const { Footer } = Layout;
const FooterWeb =() =>{
    return (
        <Footer className="ant-layout-footer" style={{backgroundColor: 'black', color: 'white' }}>
            <div>
                    ©2022 Created by VTR.Shu (Le Nghia)
            </div>
             <div className="menu1">
                <div className="submenu">
                    <div > <Link to="#">Copyright</Link></div>
                    <div > <Link to="#">Settings</Link></div>
                    <div> <Link to="#">Privacy statement</Link></div>
                    <div> <Link to="#">Legal notice</Link></div>
                </div>
                <div className="iconSocial">
                    <div><a title="Face Book" href="https://www.facebook.com" target="_blank"> <AiFillFacebook /></a></div>
                    <div><a title="Youtube" href="https://www.youtube.com" target="_blank">  <AiFillYoutube /></a></div>
                    <div><a title="Instagram" href="https://www.instagram.com" target="_blank">  <AiFillInstagram /></a></div>
                    <div><a title="Linkedin" href="https://www.linkedin.com" target="_blank"> <AiFillLinkedin /></a></div>
                </div>
            </div>
        </Footer>
    )
}

export default FooterWeb;