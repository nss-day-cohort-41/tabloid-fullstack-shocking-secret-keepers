import React, { useContext, useEffect } from "react";
import { CommentContext } from "../../providers/CommentProvider";
import { PostContext } from "../../providers/PostProvider";
import Comment from "./Comment";
import { Col, Row, Button } from "reactstrap";
//accesses the route parameters
import { Link, useParams } from "react-router-dom";


const CommentList = () => {
    const { comments, getAllCommentsForPost } = useContext(CommentContext);
    const { post, getPost } = useContext(PostContext);
    // this will be the postId (once user clicks on view comments button on post details page--takes them to page to view comments)
    const { id } = useParams();

    useEffect(() => {
        getAllCommentsForPost(id);
        getPost(id);
    }, []);

    return (
        <>
            <h5> Comments for <strong><em>{post.title}</em></strong></h5>
            <Col sm="12" md={{ size: 6, offset: 3 }}>
                <Row className="justify-content-center">
                    <Link to={`/post/details/${id}`}>
                        <Button>Back To Post</Button>
                    </Link>
                </Row>
                {comments.length === 0 ? <p>There are currently no comments for this post</p> :
                    <div className="container">
                        <div className="row justify-content-center">
                            <div className="cards-column">
                                {comments && comments.map((comment) => {
                                    return (
                                        <Comment key={comment.id} comment={comment} />
                                    )
                                })}
                            </div>
                        </div>
                    </div>
                }
            </Col>
        </>
    );
};

export default CommentList;