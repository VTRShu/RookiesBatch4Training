import {Modal,Button,Form,Input} from 'antd';

const CategoryModalFormComponent = (form,showModal,handleOk,setShowModal,handleCancel) =>{
    [form] = Form.useForm();
    return(
        <Modal
        title= "Category"
        centered
        visible={showModal}
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
            onFinish={handleOk}>
                <Form.Item
                    name="categoryName"
                    rules={[
                     {
                         required: true,
                         message: "Please input Category Name",
                     },
                    ]}>
                    <Input placeholder="name"/>
                </Form.Item>
                <Button onClick={handleCancel}
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
                                !form.isFieldsTouched(true) ||
                                form.getFieldsError().filter(({ errors }) => errors.length)
                                    .length > 0
                            }
                        >
                            Save
                        </Button>
                    )}
                </Form.Item>
            </Form>
    </Modal>
    )
}

export default CategoryModalFormComponent;