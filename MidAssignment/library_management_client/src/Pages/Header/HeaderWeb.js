import React,{useContext,useState} from "react"
import { Link } from "react-router-dom";
import CurrentUserContext from '../../Share/Context/CurrentUserContext'
import Cookies from 'universal-cookie';
import {ChangePasswordService} from '../../Services/AuthenticationService'
import { UnlockOutlined } from '@ant-design/icons';
import {Layout,Menu, Modal,Form,Input,Button} from 'antd';
const { Header } = Layout;
const { SubMenu } = Menu;
const HeaderWeb = ()=>{
    const { currentUser, setCurrentUser } = useContext(CurrentUserContext);
    const cookies = new Cookies();
    const [form] = Form.useForm();
    const showLogOutModal = ()=>{
        Modal.confirm({
            title: "Are you sure, you want to logout?",
            okText: "Yes",
            centered:true,
            okType: "danger",
            onOk: () => {
                setCurrentUser({
                    fullName: null,
                    userId: null,
                    role: null
            });
                cookies.remove('token');
            },
        })
    }
    const strongRegex = new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,})");
    const [isModalVisible, setIsModalVisible] = useState(false);
    const showModalChangePassword = () => {
        setIsModalVisible(true);
    }
    const [error, setError] = useState(true);
    const handleChange = (e) => e.target.value && setError(false);
    const handleOk = (values) => {
        ChangePasswordService({
            userCode: currentUser.code,
            oldPassword: values.OldPassword,
            newPassword: values.NewPassword
        })
            .then(function (response) {
                form.resetFields();
                setIsModalVisible(false);
                setCurrentUser({
                    fullName: null,
                    userId: null,
                    role: null
                });
                cookies.remove('token');
                Modal.info({
                    title: "Change Password successfully, please login again!",
                    okText: "Close",
                    centered:true,
                })
            }).catch(function (error) {
                if (error.response.data == "Password is incorrect") {
                    setError('Old password is incorrect!');
                    form.setFields([{
                        name: 'OldPassword',
                        errors: [<b style={{ color: 'red' }}>Old password is incorrect!</b>],
                    }])
                };
            })
    }
    const handleCancel = () => {
        form.resetFields();
        setIsModalVisible(false);
    };
    return(
        <>
        <CurrentUserContext.Provider value={{ currentUser, setCurrentUser }}>
        <Modal title="Change Password" visible={isModalVisible} onOk={handleOk} closable={false} onCancel={handleCancel} centered={true}
                footer={null}>
                <Form
                    form={form}
                    wrapperCol={{
                        span: 20,
                    }}
                    onFinish={handleOk}
                    onFinishFailed={handleCancel}
                >
                    <Form.Item
                        style={{ textAlign: 'center', justifyContent: 'center' }}
                        name="OldPassword"
                        values="OldPassword"
                        validateStatus={handleOk !== 'Password is incorrect' ? 'success' : 'Old Password is incorrect'}
                        rules={[
                            {
                                required: true,
                                message: "Please input your Old Password",
                            },
                        ]}

                    >
                        <Input.Password
                            placeholder="Old Password"
                            onChange={handleChange}
                            prefix={<UnlockOutlined />} />
                    </Form.Item>

                    <Form.Item
                        style={{ textAlign: 'center', justifyContent: 'center' }}
                        name="NewPassword"
                        values="NewPassword"
                        rules={[
                            {
                                required: true,
                                message: "Please input your New Password",
                            },
                            ({ getFieldValue }) => ({
                                validator(_, value) {
                                    if (!value ||getFieldValue("OldPassword") !== value) {
                                        return Promise.resolve();
                                    } else {
                                        return Promise.reject(
                                            new Error(
                                                "New Password and Old Password must not match!"
                                            )
                                        )
                                    }
                                }
                            }),
                            () => ({
                                validator(_, value) {
                                    if (!value || strongRegex.test(value)) {
                                        return Promise.resolve();
                                    } else {
                                        return Promise.reject(
                                            new Error(
                                                "Require lowercase , uppercase , numeric and special Character and at least 8 Characters"
                                            )
                                        );
                                    }
                                },
                            }),
                        ]}
                    >
                        <Input.Password
                            placeholder="New Password"
                            prefix={<UnlockOutlined />}
                        />
                    </Form.Item>
                    <Button onClick={handleCancel}
                        style={{ marginRight: '25%', marginLeft: '20%', color: "black", float: "right" }}>Cancel</Button>
                    <Form.Item
                        shouldUpdate
                        wrapperCol={{
                            span: 16,
                            offset: 21,
                        }}
                    >
                        {() => (
                            <Button
                                style={{ marginRight: '40%', paddingLeft: '20px', paddingRight: '20px', float: "right" }}
                                htmlType="submit"
                                disabled={
                                    !form.isFieldsTouched(true) ||
                                    form.getFieldsError().filter(({ errors }) => errors.length)
                                        .length > 0
                                }
                            >
                                Save
                            </Button>
                        )}
                    </Form.Item>
                </Form>
            </Modal>
            <Header>
                <Menu theme="dark" mode="horizontal"  defaultSelectedKeys={['/']}>
                <Menu.Item key="1"><Link to="/home">Home</Link></Menu.Item>
                <Menu.Item key="2"><Link to="/bookview">Book</Link></Menu.Item>
                {currentUser.role === null || currentUser.role === undefined ?
                <>  
                    <Menu.Item style={{ marginLeft:'80%'}}><Link to="/login">Login</Link></Menu.Item>
                    <Menu.Item style={{ marginLeft:'auto'}}><Link to="/register">Register</Link></Menu.Item>
                </>
                :currentUser.role ==="SuperUser"?   
                <>
                    <SubMenu key="sub1" title="Library Management">
                        <Menu.Item key="3"><Link to="/users">User</Link></Menu.Item>
                        <Menu.Item key="4"><Link to="/books">Book</Link></Menu.Item>
                        <Menu.Item key="5"><Link to ="/categories">Category</Link></Menu.Item>
                    </SubMenu>
                    <Menu.Item key="6"><Link to="/borrowing-request">Borrowing Request</Link></Menu.Item>
                    <SubMenu key="sub2" style={{ marginLeft: 'auto' }} title={`Welcome ${currentUser.fullName}`}>
                        <Menu.Item key="7" onClick={showModalChangePassword}>Change Password </Menu.Item>
                        <Menu.Item key="8"><Link to="/profile">Your Profile</Link></Menu.Item>
                        <Menu.Item key="9" onClick={showLogOutModal}>Logout</Menu.Item>
                    </SubMenu>
                </>
                :
                <>
                    <Menu.Item key="3"><Link to="/borrowing-request">Borrow Request</Link></Menu.Item>
                    <SubMenu key="sub1" style={{ marginLeft: 'auto' }} title={<img src="https://images.alphacoders.com/741/thumbbig-741098.webp" style={{ borderRadius: '30px', width: '60px', height: '60px' }} />}>
                        <Menu.Item key="5" onClick={showModalChangePassword}>Change Password </Menu.Item>
                        <Menu.Item key="6"><Link to="/profile">Your Profile</Link></Menu.Item>
                        <Menu.Item key="7" onClick={showLogOutModal}>Logout</Menu.Item>
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