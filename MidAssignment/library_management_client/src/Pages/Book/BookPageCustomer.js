import React,{useState, useEffect,useContext} from "react";
import {Col,Layout,Row,Card,Pagination,Collapse,Modal,Button } from 'antd';
import LoadingComponent from "../../Components/LoadingComponent"
import BookConstant from '../../Share/Constant/BookConstant'
import {Cookies} from 'react-cookie';
import { useNavigate } from "react-router-dom";
import {GetListBookService,GetAllBookService} from '../../Services/BookService'
import CurrentUserContext from "../../Share/Context/CurrentUserContext"
import {TableConstant,itemRender} from '../../Share/Constant/TableConstant'
const { Content } = Layout;
const { Panel } = Collapse;
const {Meta} = Card;
const BookPageCustomer = () =>{
    const [pageIndex, setPageIndex] = useState(TableConstant.PageIndexDefault);
    const { currentUser, setCurrentUser } = useContext(CurrentUserContext);
    const [pageSizeOld, setPageSizeOld] = useState(12);
    const [isLoading, setIsLoading] = useState(false);
    const [books, setBooks] = useState([]);
    const [total,setTotal] = useState(0);
    const [categoryOfBook,setCategoryOfBook] = useState({
        categoryName: null,
    })
    const navigate = useNavigate();
    useEffect(()=>{
        setIsLoading(true);
        let didCancel = false;
        GetListBookService({index: pageIndex, size: pageSizeOld}).then((response) =>{
            if(!didCancel)
            {
                setBooks(response.data.items);
                setTotal(response.data.totalRecords);
                setIsLoading(false);
            }
        }).catch(function (err) {
            if (!didCancel) {
                console.error(err.message);
                setIsLoading(false)
            }
        })
        return () => didCancel = true;
    },[pageSizeOld, pageIndex])
    if (isLoading) {
        return (
          <LoadingComponent/>
        )
    } 
    else 
    {
        return(
            <Content className="ant-layout-content">
                <div style={{textAlign: 'center'}}>
                    <Button onClick = {(e)=> {
                         e.stopPropagation()
                         if(currentUser.role !==null)
                         {
                            navigate('/borrowing-request');
                         }else{
                             Modal.warning({
                                 title: 'You must login to borrow books!',
                                 okText:'Close'
                             })
                         }
                         }}>
                         Borrow Books
                    </Button>
                </div>
                <br/>
                {
                    books !==  undefined?
                    <>
                        <div>
                            <Row>
                                {
                                    books.map(book=>{
                                        return(
                                        <Col style={{ padding:'25px'}}>
                                            <Card 
                                            hoverable
                                            style={{width: 300}}
                                            cover={<img src={book.coverSrc} />}>
                                             <Meta title={book.name} description={`${book.publishedAt.substring(8, 10)}/${book.publishedAt.substring(5, 7)}/${book.publishedAt.substring(0, 4)}`} />
                                             <Collapse ghost>
                                                <Panel header="Description" key="1">
                                                    <p>{book.description}</p>
                                                </Panel>
                                            </Collapse>
                                            </Card>
                                        </Col>
                                        )
                                    })
                                }
                               
                            </Row>
                        </div>
                        <Row style={{ marginRight: '44%' }} justify="end">
                            <Col>
                                <Pagination size={'default'}  total={total} 
                                current={ pageIndex}
                                pageSize={ pageSizeOld}
                                onChange={(page, pageSize) => {
                                    if (page !== pageIndex) {
                                      setPageIndex(page);
                                    }
                                    if (pageSize !== pageSizeOld) {
                                      setPageSizeOld(pageSize);
                                    }
                                  }} />
                            </Col>
                        </Row>
                    </>
                    :""
                }
            </Content>
        )
    }
}

export default BookPageCustomer;