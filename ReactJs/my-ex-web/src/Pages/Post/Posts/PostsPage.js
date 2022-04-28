import React, { useState, useEffect } from "react";
import PostConstant from "../../../Share/Constant/PostConstant";
import { GetPostsService, GetPostService } from "../../../Services/PostService";
import {  SearchOutlined } from "@ant-design/icons";
import { Table, Modal,Layout } from "antd";
import {SearchConstant} from "../../../Share/Constant/TableConstant";
import { useNavigate } from "react-router-dom";
import LoadingComponent from '../../../Components/LoadingComponent'
import './PostsPage.css'
const { Content } = Layout;
const PostsPage = () => {
  const [pageIndex, setPageIndex] = useState(PostConstant.PageIndexDefault);
  const [pageSizeOld, setPageSizeOld] = useState(PostConstant.PageSizeDefault);
  const [isLoading, setIsLoading] = useState(false);
  const [data, setData] = useState([]);
  const [total, setTotal] = useState(0);
  const [post, setPost] = useState({
    id:null,
    userId:null,
    title:null,
    body:null
  });
  let navigate = useNavigate();
  const [showModal, setShowModal] = useState(false);
  const columns = [
    {
      title: PostConstant.ID,
      dataIndex: "id",
      sorter: (a, b) => {
        return a.id - b.id;
      },
      filterDropdown: SearchConstant,
      filterIcon: () => {
        return <SearchOutlined />;
      },
      onFilter: (value, record) => {
        return record.id == value;
      },
      width: "20%",
    },
    {
      title: PostConstant.Title,
      dataIndex: "title",
      sorter: (a, b) => {
        return `${a.title} ${a.title}`.localeCompare(`${b.title} ${b.title}`);
      },
      width: "60%",
      filterDropdown: SearchConstant,
      filterIcon: () => {
        return <SearchOutlined />;
      },
      onFilter: (value, record) => {
        return record.title.toLowerCase().includes(value.toLowerCase());
      },
    },
    {
      title: "Action",
      render: (post) => {
        return (
          <div style={{display: 'flex'}}>
            <>
            <i class="bi bi-card-heading"
            onClick = {(e) => {
              e.stopPropagation();  
              navigate(`/post/${post.id}`)
            }}
            title="View Detail"
            style={{fontSize: '21px' , marginRight:'40%'}}
            ></i>
            </>
            <i className="bi bi-trash" onClick={(e) => {
                e.stopPropagation();  
                onDeletePost(post);
              }}style={{fontSize:'16px', color: "red" }} ></i>
          </div>
        ); 
      },
    },
  ];
  const onDeletePost = (post) => {
    Modal.confirm({
      title: "Are you sure, you want to delete this  record?",
      okText: "Yes",
      centered:true,
      okType: "danger",
      onOk: () => {
        setData((posts) => {
          return posts.filter((postDeleted) => postDeleted.id !== post.id);
        });
      },
    });
  };
  useEffect(() => {
    setIsLoading(true);
    let didCancel = false;
    GetPostsService()
      .then(function (response) {
        if (!didCancel) {
          setData(response.data);
          setIsLoading(false);
          setTotal(data.length);
        }
      })
      .catch(function () {
        if (!didCancel) {
          setIsLoading(false);
          console.log("Something went wrong");
        }
      });
    return () => didCancel = true;
  }, []);
  const paginationProps = {
    current: pageIndex,
    pageSize: pageSizeOld,
    position: ["bottomCenter"],
    total: total,
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
  const getDetail = (record) => ({
    onClick: (e) => {
      GetPostService({ id: record.id })
        .then(function (response) {
          setPost(response.data);
          setShowModal(true);
        })
        .catch(function () {
          console.log("Something went wrong");
        });
    },
  });
  if (isLoading) {
    return (
      <LoadingComponent/>
    )
  } else {
    return (
      <Content className="ant-layout-content">
        {post != null ? (
          <Modal
            title="Post Information"
            centered
            visible={showModal}
            onOk={() => setShowModal(false)}
            onCancel={() => setShowModal(false)}
          >
            <table className="tableModal">
              <tr>
                <td>UserId</td>
                <td> {`${post.userId}`}</td>
              </tr>
              <tr>
                <td> Post Id</td>
                <td>{`${post.id}`}</td>
              </tr>
              <tr>
                <td> Title</td>
                <td>{`${post.title}`}</td>
              </tr>
              <tr>
                <td>Body</td>
                <td>{`${post.body}`}</td>
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
          dataSource={data}
          pagination={paginationProps}
          rowKey="id"
          onRow={getDetail}
        />
      </Content>
    );
  }
};

export default PostsPage;
