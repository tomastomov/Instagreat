function comment(username, postId, profilePicture) {
    let baseUrl = "https://localhost:44382/"
    let comment = $(`#addComment${postId}`).val();
    let data = {
        'comment': comment,
        'username': username,
        'postId': postId
    };
    let commentsDiv = $('#commentsDiv');
    $.ajax({
        url: baseUrl + "api/comments/AddComment",
        method: "POST",
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val(),
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        data: JSON.stringify(data),
        success: function () {
            let h3 = $('<h3>').addClass('col-md-4');
            let aTag = $('<a>').attr('href', baseUrl + `users/Profile/${username}`)
            aTag.text(username);
            let image = '<img src=\"' + profilePicture + '\" width="48px" height="48px" class="img-rounded"/>';
            h3.append(image);
            h3.append(aTag);
            h3.append(' : ' + comment);
            console.log(h3);
            commentsDiv.prepend(h3);
        },
        error: function (msg) {
            console.dir(msg);
        }
    });
}