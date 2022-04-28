import React,{useState, useEffect,useContext} from "react";
import {Table,Modal,Button,Layout,Form,Input,Select,Divider} from 'antd';
import LoadingComponent from "../../Components/LoadingComponent"
import BookConstant from '../../Share/Constant/BookConstant'
import {Cookies} from 'react-cookie';
import {GetBookDetailService,GetListBookService,DeleteBookService,CreateBookService,EditBookService,GetAllBookService} from '../../Services/BookService'
import {GetCategoryDetailService,GetAllCategoryService} from '../../Services/CategoryService';
import {SearchConstant,TableConstant} from '../../Share/Constant/TableConstant'
import {SearchOutlined} from "@ant-design/icons";
import CurrentUserContext from "../../Share/Context/CurrentUserContext"
const { Content } = Layout;
const { Option } = Select;
const { TextArea } = Input;
const BookPage = ()=>{
    const cookies = new Cookies();
    const { currentUser, setCurrentUser } = useContext(CurrentUserContext);
    const [pageIndex, setPageIndex] = useState(TableConstant.PageIndexDefault);
    const [pageSizeOld, setPageSizeOld] = useState(TableConstant.PageSizeDefault);
    const [isLoading, setIsLoading] = useState(false);
    const [data, setData] = useState([]);
    const [total, setTotal] = useState(0);
    const [value,setValue] = useState([]);
    const [categoryOfBook,setCategoryOfBook] = useState({
        categoryName: null,
    })
    const [book,setBook] = useState({
        id:null,
        categoryId:null,
        name:null,
        description:null,
        publishedAt:null,
    });
    const [bookEdit, setBookEdit]= useState({
        categoryId:null,
        name:null,
        description:null,
        coverSrc:null,
    });
    const [categoryEdit,setCategoryEdit]= useState({
        categoryName:null,
    })
    const [showModal, setShowModal] = useState(false);
    const [showModalCreate, setShowModalCreate] = useState(false);
    const [showModalEdit, setShowModalEdit] = useState(false);
    const [bookId,setBookId] = useState();
    const columns = [
        {
            title: 'No.',
            key: 'index',
            render:(text, record, index) => index+1,
            width:'5%',
        },
        {
            title: BookConstant.Name,
            dataIndex:"name",
            sorter: (a, b) => {
                return `${a.name}`.localeCompare(`${b.name}`);
            },
            filterDropdown: SearchConstant,
            filterIcon: () => {
                return <SearchOutlined />;
            },
            onFilter: (value, record) => {
                setValue(value);
                setTotal(record);
                return record.name.toLowerCase().includes(value.toLowerCase());
            },
            width:'10%'
        },
        {
            title: BookConstant.PublishedAt,
            dataIndex : "publishedAt",
            sorter: (a, b) => new Date(a.publishedAt) - new Date(b.publishedAt),
            render: (text) => {
                return(<>{`${text.substring(8, 10)}/${text.substring(5, 7)}/${text.substring(0, 4)}`}</>)
            },
            width:'10%'
        },
        {
            title: BookConstant.Description,
            dataIndex : "description",
            sorter: (a, b) => {
                return `${a.description}`.localeCompare(`${b.description}`);
            },
            filterDropdown: SearchConstant,
            filterIcon: () => {
                return <SearchOutlined />;
            },
            onFilter: (value, record) => {
                setValue(value);
                setTotal(record);
                return record.description.toLowerCase().includes(value.toLowerCase());
            },
            width:'60%'
        },
        {
            title: "Action",
            render: (book) => {
              return (
                <div style={{display: 'flex'}}>
                  <i class="bi bi-pencil-square"
                  onClick = {(e) => {
                    setBookId(book.id);
                    setShowModalEdit(true);
                    e.stopPropagation();  
                  }}
                  title="Edit"
                  style={{fontSize: '21px' , marginRight:'40%'}}
                  ></i>
                  <i className="bi bi-x-octagon-fill" onClick={(e) => {
                      e.stopPropagation();  
                      onDeleteBook(book);
                    }}style={{fontSize:'20px',marginRight:'5%', color: "red" }} ></i>
                </div>
              ); 
            },
        },
    ]
    const onDeleteBook = (book) => {
        Modal.confirm({
          title: "Are you sure you want to delete this book?",
          okText: "Yes",
          centered:true,
          okType: "danger",
          onOk: () => {
            DeleteBookService({id : book.id}).then((response) => {
                setData({...data,items:data.items.filter(x=>x.id !== book.id)})
            }).catch(function (error) {
                // handle error
                console.log(error);
            })
          },
        });
    };
    const getBookDetail = (record) => ({
        onClick: (e) => {
          GetBookDetailService({ id: record.id })
            .then(function (response) {
                response.data.publishedAt = `${response.data.publishedAt.substring(8, 10)}/${response.data.publishedAt.substring(5, 7)}/${response.data.publishedAt.substring(0, 4)}`
                setBook(response.data);
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
        GetListBookService({index: pageIndex, size: pageSizeOld}).then(function (response) {
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
    useEffect(()=>{
        GetBookDetailService({id: bookId}).then((response)=>{
            setBookEdit(response.data);
            GetCategoryDetailService({id:bookEdit.categoryId}).then((res)=>{
                setCategoryEdit(res.data);
            }).catch((err)=>{
                console.log(err);
            })
        }).catch((error)=>{
            console.log(error);
        })
    },[bookId,bookEdit.categoryId]);
    useEffect(()=>{
        if(book.categoryId === null || book.categoryId !== undefined)
        {
            GetCategoryDetailService({id:book.categoryId}).then(function (res) {
                setCategoryOfBook(res.data);
                console.log(book.categoryId);
            }).catch(function (err) {
                console.error(err);
            })
        }else{
            setCategoryOfBook(null);
        }
    },[book.categoryId])
    //Get Category modal select
    const[categoryData,setCategoryData] = useState()
    useEffect(()=>{
        GetAllCategoryService().then((response)=>{
            setCategoryData(response.data)
        }).catch((error)=>{
            console.log(error);
        })
    },[])
    //Create
    const [ formCreate ] = Form.useForm();
    const handleCreateOk = (values) => {
        CreateBookService({
            categoryId: values.category,
            name: values.name,
            description: values.description,
            coverSrc:values.coverSrc
        }).then((response)=>{
            setShowModalCreate(false)
            Modal.info({
                title: "Create book successfully!",
                onOk: () =>{window.location.reload();},
                okText: "Close",
                centered:true,
            })
            formCreate.resetFields();
        }).catch(function (err){
            console.error(err);
            formCreate.setFields([{
                name: 'name',
                errors: [<b style={{ color: 'red' }}>This book is already existed!</b>],
            }])
        })
    }

    const handleCreateCancel=()=>{
        setShowModalCreate(false);
        formCreate.resetFields();
    }
    //Edit
    const [ formEdit ] = Form.useForm();
    const handleEditOk = (values)=>{
        EditBookService({id:bookId,
        categoryId:values.category,
        name:values.name,
        coverSrc:values.coverSrc,
        description: values.description}).then((response)=>{
            setShowModalEdit(false)
            Modal.info({
                title: "Edit book successfully!",
                onOk: () =>{window.location.reload();},
                okText: "Close",
                centered:true,
            })
            formEdit.resetFields();
        }).catch((error)=>{
            console.log(error);
        })
    }
    formEdit.setFieldsValue({
        name:bookEdit.name,
        description:bookEdit.description,
        category:categoryEdit.categoryName,
        coverSrc: bookEdit.coverSrc
    })
    const handleEditCancel=()=>{
        setShowModalEdit(false);
        formEdit.resetFields();
    }
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
                    {/* MODAL CREATE NEW */}
                    <Modal
                        title="New Book"
                        centered
                        visible={showModalCreate}
                        footer={null}>
                            <Form
                             wrapperCol={{
                                span: 23,
                            }}
                            name="validate_other"
                            form={formCreate}
                            initialValues={{
                                remember: true,
                            }}
                            onFinish={handleCreateOk}>
                                <Form.Item
                                style={{ textAlign: 'center', justifyContent: 'center' }}
                                    name="name"
                                    rules={[
                                     {
                                         required: true,
                                         message: "Please input Book Name",
                                     },
                                    ]}>
                                    <Input placeholder="Name"/>
                                </Form.Item>
                                <Form.Item
                                    style={{ textAlign: 'center', justifyContent: 'center' }}
                                    name="category"
                                    rules={[
                                        {
                                            required: true,
                                            message: "Please select Category!",
                                        }
                                    ]}>
                                    <Select
                                        style={{ width: "100%",textAlign: 'left'}}
                                        placeholder="Choose Category"
                                        dropdownRender={(menu) => (
                                            <div>
                                                {menu}
                                                <Divider style={{ margin: "4px 0" }} />
                                            </div>
                                        )}
                                    >
                                        {categoryData &&
                                            categoryData.length > 0 &&
                                            categoryData.map((item) => (
                                                <Option value={item.id} key={item.id}>
                                                    {item.categoryName}
                                                </Option>
                                            ))}
                                    </Select>
                                </Form.Item>
                                <Form.Item
                                style={{ textAlign: 'center', justifyContent: 'center' }}
                                    name="coverSrc"
                                    rules={[
                                     {
                                         required: true,
                                         message: "Please input coverSrc",
                                     },
                                     () => ({
                                        validator(_, value) {
                                            if (!value || value.includes("https://")) {
                                                return Promise.resolve();
                                            } else {
                                                return Promise.reject(
                                                    new Error(
                                                        "HTTPS link required!"
                                                    )
                                                );
                                            }
                                        },
                                    }),
                                    ]}>
                                    <Input placeholder="Paste a img link for the book"/>
                                </Form.Item>
                                <Form.Item
                                    name="description">
                                    <TextArea
                                        style={{ width: "93%", marginLeft: '6%' }}
                                        placeholder="Description" />
                                </Form.Item>
                               
                                <Button onClick={handleCreateCancel}
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
                                                !formCreate.isFieldsTouched(true) ||
                                                formCreate.getFieldsError().filter(({ errors }) => errors.length)
                                                    .length > 0
                                            }
                                        >
                                            Save
                                        </Button>
                                    )}
                                </Form.Item>
                            </Form>
                    </Modal>

                    {/* MODAL EDIT */}

                    <Modal
                        title="Edit Book"
                        centered
                        visible={showModalEdit}
                        footer={null}>
                            <Form
                             wrapperCol={{
                                span: 23,
                            }}
                            name="validate_other"
                            form={formEdit}
                            initialValues={{
                                remember: true,
                            }}
                            onFinish={handleEditOk}>
                                <Form.Item
                                style={{ textAlign: 'center', justifyContent: 'center' }}
                                    name="name"
                                    rules={[
                                     {
                                         required: true,
                                         message: "Please input Book Name",
                                     },
                                    ]}>
                                    <Input placeholder="Name"/>
                                </Form.Item>
                                <Form.Item
                                    style={{ textAlign: 'center', justifyContent: 'center' }}
                                    name="category"
                                    hasFeedback
                                    rules={[
                                        {
                                            required: true,
                                            message: "Please select Category!",
                                        }
                                    ]}>
                                    <Select
                                        style={{ width: "100%" ,textAlign: 'left'}}
                                        placeholder="Choose Category"
                                        dropdownRender={(menu) => (
                                            <div>
                                                {menu}
                                                <Divider style={{ margin: "4px 0" }} />
                                            </div>
                                        )}
                                    >
                                        {categoryData &&
                                            categoryData.length > 0 &&
                                            categoryData.map((item) => (
                                                <Option value={item.id} key={item.id}>
                                                    {item.categoryName}
                                                </Option>
                                            ))}
                                    </Select>
                                </Form.Item>
                                <Form.Item
                                style={{ textAlign: 'center', justifyContent: 'center' }}
                                    name="coverSrc"
                                    rules={[
                                     {
                                         required: true,
                                         message: "Please input coverSrc",
                                     },
                                    ]}>
                                    <Input placeholder="Paste a img link for the book"/>
                                </Form.Item>
                                <Form.Item
                                    name="description"
                                    hasFeedback>
                                    <TextArea
                                        style={{ width: "93%", marginLeft: '6%' }}
                                        placeholder="Description" />
                                </Form.Item>
                               
                                <Button onClick={handleEditCancel}
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
                                                !formCreate.isFieldsTouched(true) ||
                                                formCreate.getFieldsError().filter(({ errors }) => errors.length)
                                                    .length > 0
                                            }
                                        >
                                            Save
                                        </Button>
                                    )}
                                </Form.Item>
                            </Form>
                    </Modal>

                    {/* MODAL DETAIL */}

                    {book != null ? (
                    <Modal
                        centered
                        visible={showModal}
                        onOk={() => setShowModal(false)}
                        onCancel={() => setShowModal(false)}
                    >
                        <table className="tableModal">
                        <tr>
                            <td>Name</td>
                            <td> {`${book.name}`}</td>
                        </tr>
                        <tr>
                            <td>Of Category</td>
                            <td>{book.categoryId === null?"Unknown":`${categoryOfBook.categoryName}`}</td>
                        </tr>
                        <tr>
                            <td>Published At</td>
                            <td>{book.publishedAt}</td>
                        </tr>
                        <tr>
                            <td>Description</td>
                            <td>{`${book.description}`}</td>
                        </tr>
                        </table>
                    </Modal>
                ) : (
                ""
                )}
                    {/* View */}
                     <Button onClick = {(e)=> {
                         e.stopPropagation()
                         setShowModalCreate(true)}}>
                       <i class="bi bi-book"/> New Book
                    </Button>
            
                    <Table
                        size ="default"
                        loading={isLoading}
                        columns={columns}
                        dataSource={data.items}
                        pagination={paginationProps}
                        rowKey="id"
                        onRow={getBookDetail}
                    />
                </Content>
             )
        }
    }
}

export default BookPage;