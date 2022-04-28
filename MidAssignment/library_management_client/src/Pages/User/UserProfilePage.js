import React,{useState, useEffect,useContext }from 'react';
import CurrentUserContext from '../../Share/Context/CurrentUserContext'
import {GetUserProfileService,EditUserService} from "../../Services/UserService";
import {Card,Button,Modal,Form,Input,DatePicker,Radio} from 'antd';
import {Link} from 'react-router-dom';
import {EditOutlined} from '@ant-design/icons'
import moment from 'moment-timezone';
import LoadingComponent from '../../Components/LoadingComponent';
const {Meta} = Card;
const UserProfilePage = ()=>{
    const [user, setUser] = useState({
        id:null,
        fullName:null,
        dob:null,
        email:null,
        gender:null,
        type:null,
        isDisabled:null,
    })
    const dateFormat = "DD/MM/YYYY";
    const [isLoading,setIsLoading] = useState(false);
    const {currentUser,setCurrentUser} = useContext(CurrentUserContext);
    useEffect(()=>{
        setIsLoading(true);
        let didCancel = false;
        GetUserProfileService({id:currentUser.userId}).then(function (response){
            if(!didCancel)
            {   
                response.data.dob = `${response.data.dob.substring(8, 10)}/${response.data.dob.substring(5, 7)}/${response.data.dob.substring(0, 4)}`
                setUser(response.data);
                setIsLoading(false);
            }
        }).catch(function (error) {
            if(!didCancel)
            {
                setIsLoading(false);
                console.log(`Something went wrong! (${error})`);
            }
        })
        return () => didCancel = true;
    },[currentUser.userId])
    //Edit Modal
    const [showModalEdit,setShowModalEdit] = useState(false);
    const [form] = Form.useForm();
    const handleEditCancel = () =>{
        setShowModalEdit(false);
    }
    const onFinishFailed = () => {
        console.log("Edit Failed!");
    };
    const [dob, setDob] = useState([]);
    const onFinish = (values) =>{
        EditUserService({
            id:currentUser.userId,
            fullName:values.fullName,
            dob: values.dob == null ? user.dob : values.dob,
            gender: values.gender,
            email:user.email,
            userName:user.userName,
            password:"aaaaa",
        }).then(function (response) {
            setShowModalEdit(false);
            Modal.info({
                title:"Change profile successfully!",
                okText: "Close",
                onOk: () => {setUser(response.data)},
                centered:true,
            })
        }).catch(function (error) {
            console.log(error);
        });
    }
    form.setFieldsValue({
        fullName:user.fullName,
        dob : moment(user.dob),
        gender: user.gender
    })
    if (isLoading) {
        return (
          <LoadingComponent/>
        )
      } else {
          return(
            <div style={{display: 'flex',
                justifyContent: 'center'}}>
                    <Modal 
                        title="Edit Profile" 
                        visible={showModalEdit} 
                        centered
                        onCancel = {()=>setShowModalEdit(false)}
                        footer={null}>
                        <Form
                         wrapperCol={{
                            span: 23,
                        }}
                        name="validate_other"
                        form={form}
                        initialValues={{
                            remember: true,
                        }}
                        onFinish={onFinish}
                        onFinishFailed={onFinishFailed}
                        >
                            <Form.Item
                                style={{ textAlign: 'center', justifyContent: 'center' }}
                                name="fullName"
                                label="Full Name"
                                rules={[
                                {
                                    required: true,
                                    message: "Please input your name",
                                },
                                ]}>
                                <Input placeholder="Full Name"/>
                            </Form.Item>
                            <Form.Item
                                style={{ textAlign: 'center', justifyContent: 'center' }}
                                name="dob"
                                label="DOB"
                                rules={[
                                    {
                                        required: true,
                                        message: "Date of Birth is required. Should be a valid date in dd/mm/yyyy format",
                                    },
                                    () => ({
                                        validator(_, value) {
                                            var today = new Date();
                                            console.log(value.year());
                                            var temp = today.getFullYear() - value.year();
                                            if (temp > 10) {
                                                return Promise.resolve();
                                            }
                                            return Promise.reject(
                                                new Error("User is under 10. Please select a different date")
                                            );
                                        },
                                    }),
                                ]}
                            >
                                <DatePicker placeholder={'DD/MM/YYYY'} format={dateFormat} />
                            </Form.Item>
                            <Form.Item
                                style={{ textAlign: 'center', justifyContent: 'center' }}
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
                            <Button onClick={handleEditCancel}
                                style={{ marginRight: '25%', marginLeft: '20%', color: "black", float: "right" }}>Cancel</Button>
                            <Form.Item
                                shouldUpdate
                                wrapperCol={{
                                    span: 16,
                                     offset: 21,
                                }}>
                                {() => (
                                    <Button
                                        style={{ marginRight: '40%', paddingLeft: '20px', paddingRight: '20px', float: "right" }}
                                        htmlType="submit"
                                        >
                                        Save
                                    </Button>
                                )}
                            </Form.Item>
                        </Form>
                    </Modal>
                <Card
                    hoverable
                    style={{ width: 400,position:'absolute', top: '20%' }}
                    cover={<img alt="image" src="https://images.alphacoders.com/741/thumbbig-741098.webp" />}
                    actions={[
                        <EditOutlined key="edit" title="Edit Your Profile" onClick={()=>{
                            setShowModalEdit(true);
                        }} />,
                      ]}
                >
                    <Meta title={`${user.fullName} (${user.type})`} description={user.userName} />
                    <Meta description={user.dob} />
                    <Meta description={user.gender} />
                    <Meta description={user.email} />
                </Card>
          </div>
          )
      }
}
export default UserProfilePage;