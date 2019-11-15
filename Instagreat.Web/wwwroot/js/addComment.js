function comment(username, id, profilePicture, typeToAdd) {
    let baseUrl = "/"
    let textBox = $(`#add${typeToAdd}${id}`);
    let comment = textBox.val();
    let data = {
        'comment': comment,
        'username': username,
        'id': id,
    };
    if (typeToAdd === "Reply") {
        baseUrl = baseUrl + "api/comments/AddReply";
    }
    else {
        baseUrl = baseUrl + "api/comments/AddComment";
    }
    $.ajax({
        url: baseUrl,
        method: "POST",
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val(),
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        data: JSON.stringify(data),
        success: function () {
            let div = $('<div>').addClass('row button');
            let h3 = $('<h3>');
            if (typeToAdd === 'reply') {
                h3.addClass('col-md-6 replyForComment');
            }
            else {
                h3.addClass('col-md-4');
            }
            let aTag = $('<a>').attr('href', baseUrl + `users/Profile/${username}`)
            aTag.text(username);
            let image = '<img src=\"' + profilePicture + '\" width="48px" height="48px" class="img-rounded"/>';
            h3.append(image);
            h3.append(aTag);
            h3.append(' : ' + comment);
            textBox.val(' ');
            $(`#${typeToAdd.toLowerCase()}TextBox${id}`).hide();
            div.append(h3);
            div.appendTo($('body'));
            notification('Successfully commented!', 'info', 'glyphicon glyphicon-comment');
            
        },
        error: function (msg) {
            console.dir(msg);
            notification('Your comment didnt meet the requirements.', 'danger', 'glyphicon glyphicon-remove');
        }   
    });
}
