function like(username ,postId, isClicked, likesCount) {
    let likeButton = $(`#likeBtn${postId}`);
    let baseUrl = "https://localhost:44382/api/posts";
    let data = {
        'username': username,
        'postId': postId
    };

    if (!likeButton.hasClass('active')) {
        likeButton.addClass('active');
        $.ajax({
            url: baseUrl + "/AddLike",
            method: "POST",
            headers: {
                RequestVerificationToken:
                    $('input:hidden[name="__RequestVerificationToken"]').val(),
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(data),
            success: function () {
                $(`#likes${postId}`).text(`${likesCount + 1} Likes`)
            },
            error: function (msg) {
                console.dir(msg);
            }
        });

        isClicked = true;
    }
    else {
        likeButton.removeClass('active');
        $.ajax({
            url: baseUrl + "/RemoveLike",
            method: "POST",
            headers: {
                RequestVerificationToken:
                    $('input:hidden[name="__RequestVerificationToken"]').val(),
                'Content-Type': "application/json"
            },
            data: JSON.stringify(data),
            success: function (result) {
                $(`#likes${postId}`).text(`${likesCount} Likes`)
            },
            error: function (msg) {
                console.dir(msg);
            }
        });
        isClicked = false;
    }
}