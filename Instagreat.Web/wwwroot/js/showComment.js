function showCommentTextBox(postId) {
    let div = $(`#commentTextBox${postId}`);
    let textArea = $(`#addComment${postId}`);

    console.dir(div);
    console.dir(textArea);

    if (div.css('display') === "none") {
        div.show();
        textArea.show();
    } else {
        textArea.val(' ');
        div.hide();
        textArea.hide();
    };
}
    
