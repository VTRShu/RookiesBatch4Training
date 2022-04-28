import React,{useState, useEffect,useContext} from "react";
import {Table,Modal,Button,Layout,Form,Input} from 'antd';
import LoadingComponent from "../../Components/LoadingComponent"
import CategoryConstant from '../../Share/Constant/CategoryConstant'
import {SearchConstant,TableConstant} from '../../Share/Constant/TableConstant'
import {GetCategoryDetailService,GetListCategoryService,DeleteCategoryService,CreateCategoryService,EditCategoryService} from '../../Services/CategoryService'
import CurrentUserContext from "../../Share/Context/CurrentUserContext"
import {  SearchOutlined} from "@ant-design/icons";
import CategoryModalFormComponent from '../../Components/CategoryModalFormComponent'
import {Cookies} from 'react-cookie';
const { Content } = Layout;
const CategoryPage = ()=>{
    const cookies = new Cookies();
    const { currentUser, setCurrentUser } = useContext(CurrentUserContext);
    const [pageIndex, setPageIndex] = useState(TableConstant.PageIndexDefault);
    const [pageSizeOld, setPageSizeOld] = useState(TableConstant.PageSizeDefault);
    const [isLoading, setIsLoading] = useState(false);
    const [data, setData] = useState([]);
    const [total, setTotal] = useState(0);
    const [searchValue,setSearchValue] = useState([]);
    const [category, setCategory]= useState({
        categoryName:null,
        createdAt: null
    })
    const [categoryEdit, setCategoryEdit]= useState({
        categoryName:null,
    })
    const [showModal, setShowModal] = useState(false);
    const [showModalCreate, setShowModalCreate] = useState(false);
    const [showModalEdit, setShowModalEdit] = useState(false);
    const [categoryId,setCategoryId] = useState();
    const columns = [
        {
            title: 'No.',
            key: 'index',
            render:(text, record, index) => index+1
        },
        {
            title: CategoryConstant.CategoryName,
            dataIndex:"categoryName",
            sorter: (a, b) => {
                return `${a.categoryName}`.localeCompare(`${b.categoryName}`);
            },
            filterDropdown: SearchConstant,
            filterIcon: () => {
                return <SearchOutlined />;
            },
            onFilter: (value, record) => {
                setSearchValue(value);
                console.log(value.length)
                setTotal(record);
                return record.categoryName.toLowerCase().includes(value.toLowerCase());
            },
        },
        {
            title: CategoryConstant.CreateAt,
            dataIndex : "createdAt",
            sorter: (a, b) => new Date(a.dob) - new Date(b.dob),
            render: (text) => {
                return(<>{`${text.substring(8, 10)}/${text.substring(5, 7)}/${text.substring(0, 4)}`}</>)
            },
        },
        {
            title: "Action",
            render: (category) => {
              return (
                <div style={{display: 'flex'}}>
                  <i class="bi bi-pencil-square"
                  onClick = {(e) => {
                    setCategoryId(category.id);
                    setShowModalEdit(true);
                    e.stopPropagation();  
                  }}
                  title="Edit"                                                                              
                  style={{fontSize: '21px' , marginRight:'40%'}}
                  ></i>
                  <i className="bi bi-x-octagon-fill" onClick={(e) => {
                      e.stopPropagation();  
                      onDeleteCategory(category);
                    }}style={{fontSize:'20px', color: "red" }} ></i>
                </div>
              ); 
            },
        },
    ]
    const onDeleteCategory = (category) => {
        Modal.confirm({
          title: "Are you sure you want to delete this category?",
          okText: "Yes",
          centered:true,
          okType: "danger",
          onOk: () => {
            DeleteCategoryService({id : category.id}).then((response) => {
                setData({...data,items:data.items.filter(x=>x.id !== category.id)})
            }).catch(function (error) {
                // handle error
                console.log(error);
            })
          },
        });
    };
    const getCategoryDetail = (record) => ({
        onClick: (e) => {
          GetCategoryDetailService({ id: record.id })
            .then(function (response) {
                response.data.createdAt = `${response.data.createdAt.substring(8, 10)}/${response.data.createdAt.substring(5, 7)}/${response.data.createdAt.substring(0, 4)}`
                setCategory(response.data);
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
        GetListCategoryService({index: pageIndex, size: pageSizeOld}).then(function (response) {
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
        total : searchValue.length !== 0 ?total.length:data.totalRecords,
        onChange: (page, pageSize) => {
          if (page !== pageIndex) {
            setPageIndex(page);
          }
          if (pageSize !== pageSizeOld) {
            setPageSizeOld(pageSize);
          }
        },
    };
    //Create
    const [ formCreate ] = Form.useForm();
    const handleCreateOk = (value)=>{
        CreateCategoryService({
            categoryName : value.categoryName
        }).then((response) => {
            setShowModalCreate(false)
            Modal.info({
                title: "Create category successfully!",
                onOk: () =>{window.location.reload();},
                okText: "Close",
                centered:true,
            })
            formCreate.resetFields();
        }).catch(function (err){
            console.error(err);
        })
    }
    const handleCreateCancel=()=>{
        setShowModalCreate(false);
        formCreate.resetFields();
    }
    //Edit
    const [ formEdit ] = Form.useForm();
    const handleEditOk = (values)=>{
        EditCategoryService({id:categoryId, categoryName:values.CategoryName}).then((response)=>{
            setShowModalEdit(false)
            Modal.info({
                title: "Edit category successfully!",
                onOk: () =>{window.location.reload();},
                okText: "Close",
                centered:true,
            })
            formEdit.resetFields();
        }).catch((error)=>{
            console.log(error);
        })
    }
    const handleEditCancel=()=>{
        setShowModalEdit(false);
        formEdit.resetFields();
    }
    useEffect(()=>{
        GetCategoryDetailService({id: categoryId}).then((response)=>{
            setCategoryEdit(response.data);
            console.log(categoryEdit)
        }).catch((error)=>{
            console.log(error);
        })
    },[categoryId]);
    formEdit.setFieldsValue({
        CategoryName:categoryEdit.categoryName
    })
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
                     <Button onClick = {(e)=> {
                         e.stopPropagation()
                         setShowModalCreate(true)}}>
                        New Category
                    </Button>
                    <Modal
                        title="New Category"
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
                                    name="categoryName"
                                    rules={[
                                     {
                                         required: true,
                                         message: "Please input Category Name",
                                     },
                                    ]}>
                                    <Input placeholder="name"/>
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
                    {/* <CategoryModalFormComponent
                        form = {formCreate}
                        showModal= {false}
                        handleOk = {handleCreateOk}
                        handleCancel = {handleCreateCancel}
                    /> */}
                    <Modal
                        title="Edit Category"
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
                                    name="CategoryName"
                                    rules={[
                                     {
                                         required: true,
                                         message: "Please input Category Name",
                                         whitespace:true,
                                     },
                                    ]}>
                                    <Input placeholder="name"/>
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
                    {category != null ? (
                    <Modal
                        centered
                        visible={showModal}
                        onOk={() => setShowModal(false)}
                        onCancel={() => setShowModal(false)}
                    >
                        <table className="tableModal">
                        <tr>
                            <td>Category Name</td>
                            <td> {`${category.categoryName}`}</td>
                        </tr>
                        <tr>
                            <td>Created At</td>
                            <td>{`${category.createdAt}`}</td>
                        </tr>
                        </table>
                    </Modal>
                ) : (
                ""
                )}
                    <Table
                        size ="default"
                        loading={isLoading}
                        columns={columns}
                        dataSource={data.items}
                        pagination={paginationProps}
                        rowKey="id"
                        onRow={getCategoryDetail}
                    />
                </Content>
             )
        }
    }
}

export default CategoryPage;