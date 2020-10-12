import React, { useContext } from "react";
import { UserProfileContext } from "../../providers/UserProfileProvider";
import { Card, Button, Col, Row } from "reactstrap";

export default function UserProfile({ userProfile }) {
    const { activeUser } = useContext(UserProfileContext);

    return (
        <Col>
            <Card body>
                <Row>
                    <Col sm="9">
                        <strong>{userProfile.displayName}</strong>
                        <p>{userProfile.fullName}</p>
                        <p>{userProfile.userType.name}</p>
                    </Col>
                    {/* {activeUser.userTypeId === 1 &&
                        <>
                            <Col sm="1">
                                <Button type="button" id={category.id} href={`/category/edit/${category.id}`}>Edit</Button>
                            </Col>
                            <Col sm="1">
                                <Button type="button" color="danger" id={category.id} href={`/category/delete/${category.id}`}>Delete</Button>

                            </Col>
                        </>
                    } */}
                </Row>
            </Card >
        </Col >
    );
}