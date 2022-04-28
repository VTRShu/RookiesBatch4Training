import React from 'react';
import {Input,Button} from 'antd';

const TableConstant ={
    PageIndexDefault: 1,
    PageSizeDefault: 10,
}

const  SearchConstant = ({
    setSelectedKeys,
    selectedKeys,
    confirm,

  })=> {
    return (
      <>
        <div>
          <Input
            autoFocus
            placeholder="Search"
            value={selectedKeys[0]}
            onChange={(e) => {
              setSelectedKeys(e.target.value ? [e.target.value] : []);
              confirm({ closeDropdown: false });
            }}
          
          ></Input>
        </div>
      </>
    );
}
function itemRender(current, type, originalElement) {
    if (type === 'prev') {
        return <Button size="small" style={{ fontSize: '14px', marginRight: '10px' }} >Previous</Button>;
    }
    if (type === 'next') {
        return <Button size="small" style={{ fontSize: '14px', marginLeft: '8px', marginRight: '10px' }}>Next</Button>;
    }
    return originalElement;
}
export {TableConstant,SearchConstant, itemRender}
