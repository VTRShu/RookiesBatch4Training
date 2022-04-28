import React,{useState, useEffect,useContext} from "react";
import {Table,Modal,Button,Layout,Form,Input,Select,Divider} from 'antd';
import LoadingComponent from "../../Components/LoadingComponent"
import BookBorrowingRequestConstant from '../../Share/Constant/BookBorrowingRequestConstant'
import {Cookies} from 'react-cookie';
import {GetListBookBorrowingService,RespondRequestService,CreateRequestService  } from '../../Services/BookBorrowingService';
import { GetAllBookService } from "../../Services/BookService";
import {SearchConstant,TableConstant} from '../../Share/Constant/TableConstant'
import {SearchOutlined} from "@ant-design/icons";
import CurrentUserContext from "../../Share/Context/CurrentUserContext"
const { Content } = Layout;
const { Option } = Select;

const BookBorrowingPage = () =>{
    const cookies = new Cookies();
    const { currentUser, setCurrentUser } = useContext(CurrentUserContext);
    const [pageIndex, setPageIndex] = useState(TableConstant.PageIndexDefault);
    const [pageSizeOld, setPageSizeOld] = useState(TableConstant.PageSizeDefault);
    const [isLoading, setIsLoading] = useState(false);
    const [data, setData] = useState([]);
    const [total, setTotal] = useState(0);
    const [value,setValue] = useState([]);

    const [showModalRequest, setShowModalRequest] = useState(false);
    const columns = [
        {
            title: 'No.',
            key: 'index',
            render:(text, record, index) => index+1
        },
        currentUser.role === "SuperUser"?
        {
            title:BookBorrowingRequestConstant.Requester,
            dataIndex : "requestedName",
            sorter: (a, b) => {
                return `${a.requestedName}`.localeCompare(`${b.requestedName}`);
            },
            filterDropdown: SearchConstant,
            filterIcon: () => {
                return <SearchOutlined />;
            },
            onFilter: (value, record) => {
                setValue(value);
                setTotal(record);
                return record.requestedName.toLowerCase().includes(value.toLowerCase());
            },
        }:{},
        {   
            title:BookBorrowingRequestConstant.RequestDate,
            dataIndex:'requestedAt',
            sorter: (a, b) => new Date(a.requestedAt) - new Date(b.requestedAt),
            render: (text) => {
                return(<>{`${text.substring(8, 10)}/${text.substring(5, 7)}/${text.substring(0, 4)}`}</>)
            },
        },
        { 
            title:BookBorrowingRequestConstant.BooksRequested,
            dataIndex:'booksRequested',
            render: books => (
                <>
                    {   
                        
                        books.map(book=>{
                            return(<p>{book}</p>)
                        })
                    }
                </>
            ),
        },
        {   
            title:BookBorrowingRequestConstant.Responder,
            dataIndex:'responseByName',
            sorter: (a, b) => {
                return `${a.responseByName}`.localeCompare(`${b.responseByName}`);
            },
        },
        {
            title:BookBorrowingRequestConstant.Status,
            dataIndex:'status',
            filters: [
                { text: 'Approve', value: 'Approve' },
                { text: 'Reject', value: 'Reject' },
                { text: 'Waiting', value: 'Waiting'}
                ],
            onFilter: (value, record) => {
                setTotal(record);
                setValue(value);
                return record.status === value;
            },
        },
        {
            title:BookBorrowingRequestConstant.ResponseDate,
            dataIndex:'responseAt',
            sorter: (a, b) => new Date(a.responseAt) - new Date(b.responseAt),
            render: (text) => {
                return(<>{text === null?"":`${text.substring(8, 10)}/${text.substring(5, 7)}/${text.substring(0, 4)}`}</>)
            },
        },
        currentUser.role === "SuperUser"?
        {
            title: "Action",
            render: (request) => {
              return (
                  request.status ==="Waiting"?
                <div style={{display: 'flex'}}>
                  <i class="bi bi-check-circle-fill"
                  onClick = {(e) => {
                    onApproveRequest(request);
                    e.stopPropagation();  
                  }}
                  title="Approve"
                  style={{fontSize: '21px' , marginRight:'40%',color:'green'}}
                  ></i>
                  <i className="bi bi-x-octagon-fill" onClick={(e) => {
                      e.stopPropagation();  
                      onRejectRequest(request);
                    }}
                    title="Reject"
                    style={{fontSize:'20px', color: "red" }} ></i>
                </div>:""
              ); 
            },
        }:{}
    ];
    const onRejectRequest = (request) =>{
        Modal.confirm({
            title: "Are you sure you want to Reject this request?",
            okText: "Yes",
            centered:true,
            okType: "danger",
            onOk: () => {
              RespondRequestService({requestId : request.id, respond:"Reject"}).then((response) => {
                  window.location.reload();
              }).catch(function (error) {
                  console.log(error);
                  Modal.info({
                    title: "This request is already responded!",
                    okText: "Close",
                    centered:true,
                    okType: "danger",
                  });
              })
            },
        });
    }
    const onApproveRequest = (request) =>{
        Modal.confirm({
            title: "Are you sure you want to Approve this request?",
            okText: "Yes",
            centered:true,
            okType: "danger",
            onOk: () => {
              RespondRequestService({requestId : request.id, respond:"Approve"}).then((response) => {
                  window.location.reload();
              }).catch(function (error) {
                  console.log(error);
                  Modal.info({
                    title: "This request is already responded!",
                    okText: "Close",
                    centered:true,
                    okType: "danger",
                  });
              })
            },
        });
    }
    useEffect(()=>{
        setIsLoading(true);
        let didCancel = false;
        GetListBookBorrowingService({index: pageIndex, size: pageSizeOld}).then(function (response) {
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
    },[pageSizeOld, pageIndex]);
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
    //Create Request 
    const [showModalCreate,setShowModalCreate] = useState(false);
    const [form] = Form.useForm();
    const handleCreateRequest = (values)=>{
        CreateRequestService({books:values.books}).then((response) => {
            setShowModalCreate(false);
            console.log(response.data)
            Modal.info({
                title: 
                response.data==="You can only make 3 request per month, pls wait for the next month!"?"You can only make 3 request per month, pls wait for the next month!":
                response.data==="You have react the limit of book request!(Max 5)"?"You have react the limit of book request(MAX 5)!":
                response.data==="Create request successfully!"?"Create request successfully!":"Something went wrong!",
                onOk: () =>{window.location.reload();},
                okText: "Close",
                centered:true,
            })
            form.resetFields();
        }).catch(function (err){
            console.log(err.message)
        })
    }
    const handleCreateCancel=()=>{
        setShowModalCreate(false);
        form.resetFields();
    }
    const [bookData,setBookData] = useState();
    useEffect(()=>{
        GetAllBookService().then((response)=>{
            setBookData(response.data)
        }).catch((error)=>{
            console.log(error);
        })
    },[])
    if (isLoading) {
        return (
          <LoadingComponent/>
        )
    } 
    else 
    {
        return ( 
            <Content className="ant-layout-content">
                <Modal 
                 title="Make Request"
                 centered
                 visible={showModalCreate}
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
                    onFinish={handleCreateRequest}>
                        <Form.Item
                            style={{ textAlign: 'center', justifyContent: 'center' }}
                            name="books"
                            rules={[
                            {
                                required: true,
                                message: "Please select books!",
                            },
                            () => ({
                                validator(_, value) {
                                    if (value.length <= 5) {
                                        return Promise.resolve();
                                    } else {
                                        return Promise.reject(
                                            new Error("The maximum number of books to be selected is 5!")
                                        )
                                    }
                                },
                            }),
                        ]}>
                            <Select
                            mode="multiple"
                            style={{ width: "100%",textAlign: 'left'}}
                            placeholder="Choose books"
                            dropdownRender={(menu) => (
                                <div>
                                    {menu}
                                    <Divider style={{ margin: "4px 0" }} />
                                </div>
                            )}>
                                {bookData && bookData.length > 0 &&
                                    bookData.map((item) => (
                                        <Option value={item.id} key={item.id}>
                                            {item.name}
                                        </Option>
                                ))}
                            </Select>
                        </Form.Item>
                        <Button onClick={handleCreateCancel}
                                style={{ marginRight: '25%', marginLeft: '20%', color: "black", float: "right" }}>Cancel
                        </Button>
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
                                    disabled={
                                        !form.isFieldsTouched(true) ||
                                        form.getFieldsError().filter(({ errors }) => errors.length).length > 0
                                    }>
                                        Save
                                </Button>
                            )}
                        </Form.Item>
                    </Form>
                </Modal>
                {currentUser.role === "NormalUser"?
                 <Button onClick = {(e)=> {
                         e.stopPropagation()
                         setShowModalCreate(true)}}>
                         Create Request
                    </Button>:""
                }
                    <Table
                        size ="default"
                        loading={isLoading}
                        columns={columns}
                        dataSource={data.items}
                        pagination={paginationProps}
                        rowKey="id"
                    />
            </Content>
        )
    }
}

export default BookBorrowingPage;