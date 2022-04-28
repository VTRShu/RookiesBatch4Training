import React from 'react';
import {Input} from 'antd';
const  SearchConstant = ({
    setSelectedKeys,
    selectedKeys,
    confirm,

  })=> {
    return (
      <>
        <div style={{ display: "flex", flex: 1, justifyContent: "center" }}>
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

export {SearchConstant}
