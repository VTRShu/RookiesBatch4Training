import "antd/dist/antd.css";
import moment from "moment-timezone";
import { useState, useContext, useEffect } from 'react'
import { useNavigate } from "react-router-dom";
import { Layout, Form, Button, Select, Input, DatePicker, Radio, Upload, Space, Modal, Divider } from "antd";
import {CreateUserService} from "../../Services/UserService"
import { UnlockOutlined ,UserOutlined} from '@ant-design/icons';
import { Row, Col } from "antd";
import CurrentUserContext from "../../Share/Context/CurrentUserContext"
import React from "react";
const { Option } = Select;
const { Content } = Layout;
const RegisterPage = () => {
    const strongRegex = new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,})");
    const EmailRegex = new RegExp("^[a-z0-9][\-_\.\+\!\#\$\%\&\'\*\/\=\?\^\`\{\|]{0,1}([a-z0-9][\-_\.\+\!\#\$\%\&\'\*\/\=\?\^\`\{\|]{0,1})*[a-z0-9]@[a-z0-9][-\.]{0,1}([a-z][-\.]{0,1})*[a-z0-9]\.[a-z0-9]{1,}([\.\-]{0,1}[a-z]){0,}[a-z0-9]{0,}$");
    const dateFormat = "DD/MM/YYYY";
    let navigate = useNavigate();
    const [form] = Form.useForm();
    const onFinishFailed = () => {
        console.log("Failed:");
    };
    const { currentUser, setCurrentUser } = useContext(CurrentUserContext);
    const handleCancel = ()=>{
        if(currentUser.role === "SuperUser")
        {
            navigate(`/users`);
        }else{
            navigate(`/home`);
        }
    
    }
    const [dob, setDob] = useState([]);
    const onFinish = (values) => {
        var dobDate = moment.tz(dob, "Asia/Ho_Chi_Minh").format("MM/DD/YYYY");
        dobDate = new Date(dobDate).toDateString();
        if(currentUser.role === "SuperUser")
        {
            CreateUserService({
                fullName: values.fullName,
                dob:dobDate,
                gender: values.gender,
                type:values.type,
                email: values.email,
                userName:values.userName,
                password: values.password,
            }).then(function (response) {
                navigate(`/users`);
            }).catch(function (err) {
                console.error(err);
            })
        }else{
            CreateUserService({
                fullName: values.fullName,
                dob:dobDate,
                gender: values.gender,
                type:0,
                email: values.email,
                userName:values.userName,
                password: values.password,
            }).then(function (response) {
                navigate(`/home`);
                Modal.info({
                    title: "Register successfully!",
                    okText: "Close",
                    centered:true,
                })
            }).catch(function (err) {
                console.error(err);
            })
        }
    }
   
    return (
        <Content>
            <Form
                labelCol={{
                    span: 4,
                }}
                wrapperCol={{
                    span: 16,
                }}
                name="validate_other"
                form={form}
                onFinish={onFinish}
                onFinishFailed={onFinishFailed}>
                <h1 style={{ color: 'black', textAlign: 'center' }}>Register</h1>
                <Form.Item
                    label="Full Name"
                    name="fullName"
                    rules={[
                        {
                            required: true,
                            message: "Full Name is required. Please input First Name!",
                            whitespace: true,
                        },
                    ]}
                >
                    <Input />
                </Form.Item>
                <Form.Item
                    label="Date of Birth"
                    wrapperCol={{
                        span: 24,
                    }}
                    name="dob"
                    rules={[
                        {
                            required: true,
                            message: "Date of Birth is required. Should be a valid date in dd/mm/yyyy format",
                        },
                        ()=>{
                            
                        }
                    ]}
                >
                    <DatePicker onChange={(newValue) => { setDob(newValue) }} placeholder={'DD/MM/YYYY'} format={dateFormat} />
                </Form.Item>
                <Form.Item
                    name="gender"
                    label="Gender"
                    rules={[
                        {
                            required: true,
                            message: "Gender is required. Please select your Gender! ",
                        },
                    ]}
                >
                    <Radio.Group>
                        <Radio value="Male">Male</Radio>
                        <Radio value="Female">Female</Radio>
                    </Radio.Group>
                </Form.Item>
                {currentUser.role !== 'SuperUser'
                    // || valueSelect === 0 
                    ? "" :
                    <Form.Item
                    name="type"
                    label="Type"
                    hasFeedback
                    rules={[
                        {
                            required: true,
                            message: "Type is required. Please select user Type",
                        },
                    ]}
                >
                    <Select placeholder="Please select a Type"
                    >
                            <>
                                <Option value={0}>Normal User</Option>
                                <Option value={1}>Super User</Option>
                            </>
                    </Select>
                </Form.Item>
                }
                <Form.Item
                    label="Email"
                    name="email"
                    rules={[
                        {
                            required: true,
                            message: " Email is required. Please input email!",
                            whitespace: true,
                        },
                        () => ({
                            validator(_, value) {
                                if (EmailRegex.test(value)) {
                                    return Promise.resolve();
                                } else {
                                    return Promise.reject(
                                        new Error("Please enter a valid email address!")
                                    )
                                }
                            },
                        }),
                    ]}
                >
                    <Input />
                </Form.Item>
                <Form.Item
                    label="Username"
                    name="userName"
                    rules={[
                        {
                            required: true,
                            message: "Username is required. Please input First Name!",
                            whitespace: true,
                        },
                    ]}
                >
                    <Input 
                    prefix={<UserOutlined />}/>
                </Form.Item>
                <Form.Item
                        label="Password"
                        name="password"
                        values="password"
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
                            prefix={<UnlockOutlined />}
                        />
                    </Form.Item>
                <Row>
                    <Col span={18}>
                        <Form.Item
                            shouldUpdate
                            className="submit"
                            wrapperCol={{
                                span: 16,
                                offset: 21,
                            }}
                        >
                            {() => (
                                <Button
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
                    </Col>
                    <Col span={6}>
                        <Form.Item>
                            <Button onClick={() => handleCancel()}>
                                Cancel
                            </Button>
                        </Form.Item>
                    </Col>
                </Row>
            </Form>
        </Content>
    )

}
export default RegisterPage;