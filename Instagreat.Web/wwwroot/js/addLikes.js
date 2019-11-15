function like(username ,id, typeToLike) {
    let likeButton = $(`#${typeToLike}Btn${id}`);
    let baseUrl = "/api/posts";
    let data = {
        'username': username,
        'postId': id,
        'typeToLike': typeToLike
    };
    let likesDiv = $(`#${typeToLike}s${id}`);
           
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
                let likesText = likesDiv.text();
                likesDiv.text(`${+likesText + 1}`)
                notification('Successfully liked!', 'info', 'glyphicon glyphicon-thumbs-up');
            },
            error: function (msg) {
                console.dir(msg);
            }
        });
        
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
            success: function () {
                let likesText = likesDiv.text();
                likesDiv.text(`${+likesText - 1}`);
                notification('Successfully disliked!', 'info', 'glyphicon glyphicon-thumbs-up');
            },
            error: function (msg) {
                console.dir(msg);
            }
        });
    }
}
