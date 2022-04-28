import React,{useState, useEffect} from 'react';
import {useParams } from 'react-router-dom';
import {GetPostService} from '../../../Services/PostService';
import {Card} from 'antd';
import LoadingComponent from '../../../Components/LoadingComponent'
import './PostDetailPage.css'
const {Meta} = Card
const PostDetailPage = ()=>{
    const {id} = useParams();
    const [post, setPost] = useState({
        id:null,
        userId:null,
        title:null,
        body:null
    });
    const [isLoading,setIsLoading] = useState(false);
    useEffect(()=>{
        setIsLoading(true);
        let didCancel = false;
        GetPostService({id:id}).then(function(response){
            if(!didCancel)
            {
                setPost(response.data);
                setIsLoading(false);
            }
        }).catch(function(error){
            if (!didCancel) {
                setIsLoading(false);
              }
        });
        return () => {
            didCancel = true;
          };
    },[id])

    if (isLoading) {
        return (
          <LoadingComponent/>
        )
      } else {
          return(
                <div className='cardHold'>
                    <Card
                        hoverable
                        style={{ width: 300 }}
                        cover={<img alt="just a cover" src="https://1.bp.blogspot.com/-hGxE8Q3JTPo/XxwTcSpqt1I/AAAAAAAAEAc/CGWw5F4rXXIfTNQv4uFNjSdFWsBK-sATACLcBGAsYHQ/s1600/Best%2BMobile%2BWallpaper%2Bhd%2B4K%2B%25281%2529.jpg" />}
                    >
                        <Meta title={`Post ${post.id}: ${post.title}`} description={post.body} />
                    </Card>
                </div>
          )
      }
   
}

export default PostDetailPage;