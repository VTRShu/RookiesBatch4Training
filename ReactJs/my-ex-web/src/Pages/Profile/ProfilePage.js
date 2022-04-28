import React,{useState, useEffect,useContext }from 'react';
import CurrentUserContext from '../../Share/Context/CurrentUserContext'
import {GetUserProfileService} from '../../Services/UserService';
import {Card} from 'antd';
import LoadingComponent from '../../Components/LoadingComponent';
import './ProfilePage.css'
const {Meta} = Card;
const ProfilePage = ()=>{
    const [user,setUser] = useState({
        id:null,
        name:null
    })
    const [isLoading,setIsLoading] = useState(false)
    const {currentUser,setCurrentUser} = useContext(CurrentUserContext);
    useEffect(()=>{
        setIsLoading(true);
        let didCancel = false;
        GetUserProfileService({id:currentUser.userId}).then(function (response){
            if(!didCancel)
            {
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
    console.log(user)
    if (isLoading) {
        return (
          <LoadingComponent/>
        )
      } else {
          return(
            <div className='cardUser'>
                <Card
                    className = "card"
                    hoverable
                    style={{ width: 240 }}
                    cover={<img alt="image" src="https://www.pngkey.com/png/full/312-3128144_ryuko-by-tasselcat-anime-profile-pictures-for-steam.png" />}
                >
                    <Meta title={`User ${user.id}`} description={user.name} />
                </Card>
          </div>
          )
      }
}
export default ProfilePage;