import React from "react";
import "antd/dist/antd.css";
import './Login.css'
import { Form, Layout, Typography, Button, Input, Checkbox } from "antd";
import { useState } from "react";
import { LoginService } from "../../Services/AuthenticationService";
import {Cookies, useCookies } from "react-cookie";
import { UserOutlined, UnlockOutlined } from "@ant-design/icons";
import bcrypt from 'bcryptjs'
const { Content } = Layout;
const { Text } = Typography;
const LoginPage = () => {
  const [error, setError] = useState(true);
  const [userName, setUserName] = useState();
  const [password, setPassword] = useState();
  const [cookies, setCookie] = useCookies(["user"]);
  const salt = bcrypt.genSaltSync(10)
  const strongRegexPassword = new RegExp(
    "^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*])(?=.{8,})"
  );
  const onFinishFailed = (errorInfo) => {
    setError(errorInfo);
    console.log("Failed:", errorInfo);
  };
  const [form] = Form.useForm();
  const onFinish = (values) => {
    const hashedPassword = bcrypt.hashSync(values.Password, salt)
    LoginService({userName: values.Username, password: values.Password, rememberMe: values.Remember})
      .then((response) => {
        if (response.status === 200 && response.data.token !== null) {
          form.resetFields();
          if (values.Remember === true) {
            //cookies 
            setCookie('token', response.data.token, { maxAge:  7*24*60*60 });
          } else {
            setCookie('token', response.data.token);
          }
          window.location.replace("/home");
        } else if (response.data.token === null) {
          form.setFields([{
            name: 'Username',
            errors: [<b style={{ color: 'red' }}>Couldn't find user</b>],
          }])
        }
      }).catch((error) => {
        form.resetFields();
        if (error.response.data === "UserName or Password is incorrect.") {
          setError('UserName or Password is incorrect');
          form.setFields([{
            name: 'Username',
            errors: [<b style={{ color: 'red' }}>UserName or Password is incorrect.</b>],
          }])
          form.setFields([{
            name: 'Password',
            errors: [<b style={{ color: 'red' }}>UserName or Password is incorrect.</b>],
          }])
        };
      })
  };

  const handleChangeUsername = (e) => {
    setUserName(e.target.value);
  };
  const handleChangePassword = (e) => {
    setPassword(e.target.value);
  };
  return (
    <>
      <Content styles={{ padding: "50px 50px" }}>
        <div className="div-login">
          <h1 style={{ textAlign: "center", color: "black" }}>Login </h1>
          <Form
            form={form}
            name="basic"
            onFinish={onFinish}
            onFinishFailed={onFinishFailed}
          >
            <Form.Item
              style={{
                width: "70%",
                marginLeft: "15%",
                textAlign: "center",
                justifyContent: "center",
              }}
              name="Username"
              value={handleChangeUsername}
              rules={[
                {
                  required: true,
                  message: "Please input your Username!",
                  whitespace: false,
                }
              ]}
            >
              <Input
                placeholder="Username"
                value={userName}
                onChange={handleChangeUsername}
                prefix={<UserOutlined />}
              />
            </Form.Item>

            <Form.Item
              style={{
                width: "70%",
                marginLeft: "15%",
                textAlign: "center",
                justifyContent: "center",
              }}
              name="Password"
              values={setPassword}
              rules={[
                {
                  required: true,
                  message: "Please input your password!",
                  whitespace: false,
                  
                }
              ]}
            >
              <Input.Password
                placeholder="Password"
                onChange={handleChangePassword}
                value={password}
                prefix={<UnlockOutlined />}
              />
            </Form.Item>

            <Form.Item
              name="Remember"
              valuePropName="checked"
              style={{ marginRight: "10%" }}
              wrapperCol={{
                offset: 4,
                span: 16,
              }}
            >
              <Checkbox>
                <Text style={{ color: "black" }} strong>
                  Remember me
                </Text>
              </Checkbox>
            </Form.Item>

            <Form.Item style={{ textAlign: "center" }}>
              <Button
                disabled={
                  form.getFieldValue("Username") === undefined ||
                  form.getFieldValue("Username") === "" ||
                  form.getFieldValue("Password") === undefined ||
                  form.getFieldValue("Password") === "" ||
                  form.getFieldsError().filter(({ errors }) => errors.length)
                    .length > 0
                }
                htmlType="submit"
              >
                Login
              </Button>
            </Form.Item>
          </Form>
        </div>
      </Content>
    </>
  );
};
export default LoginPage;
