import React,{useState,useEffect,useContext} from "react";
import { Table, Modal,Layout,Button,Input,Select,Row,Col} from "antd";
import {Link} from 'react-router-dom';
import LoadingComponent from "../../../Components/LoadingComponent"
import UserConstant from "../../../Share/Constant/UserConstant";
import {  SearchOutlined,UserAddOutlined,FilterFilled} from "@ant-design/icons";
import {Cookies} from 'react-cookie';
import {SearchConstant,TableConstant} from "../../../Share/Constant/TableConstant";
import {DisableUserService,GetListUserService,GetUserProfileService} from '../../../Services/UserService';
import CurrentUserContext from "../../../Share/Context/CurrentUserContext"
import './UserListPage.css'
const { Content } = Layout;
const {Search} = Input;
const UserListPage = ()=>{
    const cookies = new Cookies();
    const { currentUser, setCurrentUser } = useContext(CurrentUserContext);
    const [pageIndex, setPageIndex] = useState(TableConstant.PageIndexDefault);
    const [pageSizeOld, setPageSizeOld] = useState(TableConstant.PageSizeDefault);
    const [isLoading, setIsLoading] = useState(false);
    const [data, setData] = useState([]);
    const [total, setTotal] = useState(0);
    const [value,setValue] = useState([]);
    const [searchName,setSearchName] = useState();
    const [searchEmail,setSearchEmail] = useState();
    const [filterGender,setFilterGender] = useState();
    const [filterRole,setFilterRole] = useState();
    const [user, setUser] = useState({
        id:null,
        fullName:null,
        dob:null,
        email:null,
        gender:null,
        type:null,
        isDisabled:null,
    })
    const options = [{ label: 'NormalUser', value: "NormalUser" }, { label: 'SuperUser', value: "SuperUser" }];
    const selectProps = {
        suffixIcon: <FilterFilled />,
        style: {
          width: '100%',
        },
        mode: 'single',
        filterRole,
        options,
        onChange: (newValue) => {
          setFilterRole(newValue);
        },
        placeholder: 'Type',
        maxTagCount: 'responsive',
        showArrow: true,
        optionFilterProp: 'label'
      };
    const [showModal, setShowModal] = useState(false);
    const columns = [
        {
            title: 'No.',
            key: 'index',
            render:(text, record, index) => index+1
          },
        {
            title: UserConstant.FullName,
            dataIndex: "fullName",
            sorter: (a, b) => {
                return `${a.fullName}`.localeCompare(`${b.fullName}`);
            },
            filterDropdown: <Search placeholder="Search name" onSearch={(value)=>{setSearchName(value)}}/>,
            filterIcon: () => {
                return <SearchOutlined />;
            },
        },
        {
            title: UserConstant.Email,
            dataIndex : "email",
            sorter: (a, b) => {
                return `${a.email} ${a.email}`.localeCompare(`${b.email} ${b.email}`);
            },
            filterDropdown: <Search placeholder="Search email" onSearch={(value)=>{setSearchEmail(value)}}/>,
            filterIcon: () => {
                return <SearchOutlined />;
            },
        },
        {
            title: UserConstant.DoB,
            dataIndex : "dob",
            sorter: (a, b) => new Date(a.dob) - new Date(b.dob),
            render: (text) => {
                return(<>{`${text.substring(8, 10)}/${text.substring(5, 7)}/${text.substring(0, 4)}`}</>)
            },
        },
        {
            title: UserConstant.Genders,
            dataIndex : "gender",
        },
        {
            title: UserConstant.Type,
            dataIndex : "type",
        },
        {
            title: "Action",
            render: (user) => {
              return (
                <div>
                  {user.isDisabled === false?
                  <i className="bi bi-x-octagon-fill" onClick={(e) => {
                      e.stopPropagation();  
                      onDeleteUser(user);
                    }}style={{fontSize:'20px', color: "red" }} ></i>
                    :""
                }
                </div>
              ); 
            },
          },
    ]
    const onDeleteUser = (user) => {
        Modal.confirm({
          title: "Are you sure you want to disable this user?",
          okText: "Yes",
          centered:true,
          okType: "danger",
          onOk: () => {
            DisableUserService({id : user.id}).then((response) => {
                setData({...data,items:data.items.filter(x=>x.id !== user.id)})
            }).catch(function (error) {
                // handle error
                console.log(error);
            })
          },
        });
      };
      const getDetail = (record) => ({
        onClick: (e) => {
          GetUserProfileService({ id: record.id })
            .then(function (response) {
                response.data.dob = `${response.data.dob.substring(8, 10)}/${response.data.dob.substring(5, 7)}/${response.data.dob.substring(0, 4)}`
                setUser(response.data);
                setShowModal(true);
            })
            .catch(function () {
              console.log("Something went wrong");
            });
        },
      });
      useEffect(()=>{
          setIsLoading(true);
          let didCancel = false;
          GetListUserService({name:searchName,email:searchEmail,gender:filterGender,role:filterRole,index: pageIndex, size: pageSizeOld}).then(function (response) {
              if(!didCancel)
              {
                setData(response.data);
                setTotal(data.totalRecords)
                setIsLoading(false);
              }
          }).catch((err)=>{
            if (!didCancel) {
                console.log(err.message);
                if(err.message.includes("401"))
                {   
                    setCurrentUser({
                        fullName: null,
                        userId: null,
                        role: null
                    });
                    cookies.remove('token');
                }
                setIsLoading(false);
                console.log("Something went wrong");
              }
          });
          return () => didCancel = true;
      },[pageSizeOld, pageIndex,searchEmail,searchName,filterGender,filterRole]);
      const paginationProps = {
        current: pageIndex,
        pageSize: pageSizeOld,
        position: ["bottomCenter"],
        total : value.length !== 0 ?total.length:data.totalRecords,
        onChange: (page, pageSize) => {
          if (page !== pageIndex) {
            setPageIndex(page);
          }
          if (pageSize !== pageSizeOld) {
            setPageSizeOld(pageSize);
          }
        },
        showTotal: (total) => `Total ${total} items`,
      };
      if (isLoading) {
        return (
          <LoadingComponent/>
        )
       } else {
            if(currentUser.role !== "SuperUser")
            {   
                return(
                <div style={{textAlign: "center",height: "81vh",justifyContent: "center"}}>
                    <p  style={{  position:"relative",top: "calc(50% - 10px)", fontSize:"30px"}}>You don't have permission to access this page!</p>
                </div>
                )
            }
            else{
                return ( 
                <Content className="ant-layout-content">
                    <Button >
                        <Link to='/register'><UserAddOutlined />New User</Link>
                    </Button>
                    {user != null ? (
                <Modal
                    title="User Details"
                    centered
                    visible={showModal}
                    onOk={() => setShowModal(false)}
                    onCancel={() => setShowModal(false)}
                >
                    <table className="tableModal">
                    <tr>
                        <td>Full Name</td>                        
                        <td> {`${user.fullName}`}</td>
                    </tr>
                    <tr>
                        <td>Date Of Birth</td>
                        <td>{`${user.dob}`}</td>
                    </tr>
                    <tr>
                        <td> Gender</td>
                        <td>{`${user.gender}`}</td>
                    </tr>
                    <tr>
                        <td>Email</td>
                        <td>{`${user.email}`}</td>                                                                                                                                               
                    </tr>
                    <tr>
                        <td>Role</td>
                        <td>{`${user.type}`}</td>                                                                                                                                               
                    </tr>
                    </table>
                </Modal>
                ) : (
                ""
                )}
                    <Row gutter={{ xs: 8, sm: 16, md: 24, lg: 32 }} >
                        <Col span={5}>
                            <Select {...selectProps} />
                        </Col>
                   </Row>

                    <Table
                        size ="default"
                        loading={isLoading}
                        columns={columns}
                        dataSource={data.items}
                        pagination={paginationProps}
                        rowKey="id"
                        onRow={getDetail}
                    />
                </Content>
             )
        }
    }
}

export default UserListPage;