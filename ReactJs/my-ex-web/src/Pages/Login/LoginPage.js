import React from "react";
import "antd/dist/antd.css";
import './Login.css'
import { Form, Layout, Typography, Button, Input, Checkbox } from "antd";
import { useState } from "react";
import { GetTokenLoginService } from "../../Services/AuthService";
import { useCookies } from "react-cookie";
import Cookies from "universal-cookie";
import { UserOutlined, UnlockOutlined } from "@ant-design/icons";
const { Content } = Layout;
const { Text } = Typography;
const LoginPage = () => {
  const [error, setError] = useState(true);
  const [email, setEmail] = useState();
  const [password, setPassword] = useState();
  const [cookies, setCookie] = useCookies(["user"]);
  const strongRegexPassword = new RegExp(
    "^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*])(?=.{8,})"
  );
  const emailRegex =new RegExp(/^(("[\w-\s]+")|([\w-]+(?:\.[\w-]+)*)|("[\w-\s]+")([\w-]+(?:\.[\w-]+)*))(@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$)|(@\[?((25[0-5]\.|2[0-4][0-9]\.|1[0-9]{2}\.|[0-9]{1,2}\.))((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\.){2}(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\]?$)/i)
  const onFinishFailed = (errorInfo) => {
    setError(errorInfo);
    console.log("Failed:", errorInfo);
  };
  const handleChange = (e) => e.target.value && setError(false);
  const [form] = Form.useForm();
  const onFinish = (values) => {
    GetTokenLoginService()
      .then((response) => {
        form.resetFields();
        if (values.Remember === true) {
          setCookie("token", response.data.token, { maxAge: 604800 });
          setCookie("userId", response.data.userId, { maxAge: 604800 });
          window.location.replace("/home")
        } else {
          setCookie("token", response.data.token);
          setCookie("userId", response.data.userId);
          window.location.replace("/home")
        }
      })
      .catch((error) => {
        console.log("Something went wrong");
      });
  };

  const handleChangeEmail = (e) => {
    setEmail(e.target.value);
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
            initialValues={{
              remember: true,
            }}
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
              name="Email"
              value={setEmail}
              rules={[
                {
                  required: true,
                  message: "Please input your Email!",
                  whitespace: false,
                },
                () => ({
                    validator(_, value) {
                        if (!value || emailRegex.test(value) ) {
                            return Promise.resolve();
                        } 
                            return Promise.reject(
                                new Error(
                                    "Email is invalid!"
                                )
                            ); 
                        
                    },
                }),
              ]}
            >
              <Input
                placeholder="Email"
                value={email}
                onChange={handleChangeEmail}
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
                  
                },
                () => ({
                    validator(_, value) {
                        if ( !value || strongRegexPassword.test(value)) {
                            return Promise.resolve();
                        } else {
                            return Promise.reject(
                                new Error(
                                    "Require lowercase , uppercase , numeric and special Character and at least 8 characters "
                                )
                            );
                        }
                    },
                }),
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
                  form.getFieldValue("Email") === undefined ||
                  form.getFieldValue("Email") === "" ||
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
