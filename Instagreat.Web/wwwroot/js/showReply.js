function showReplyTextBox(commentId) {
    let div = $(`#replyTextBox${commentId}`);
    let textArea = $(`#addReply${commentId}`);

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