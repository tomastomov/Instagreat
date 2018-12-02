function showCommentTextBox(postId) {
    let div = $(`#commentTextBox${postId}`);
    let textArea = $(`#addComment${postId}`);

    if (div.css('display') === "none") {
        div.show();
        textArea.show();
    } else {
        textArea.val(' ');
        div.hide();
        textArea.hide();
    };
}
    
