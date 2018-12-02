function showProfilePicture() {
    let div = $('#profilePicture');

    if (div.css('display') === 'none') {
        div.show();
    }
    else {
        div.hide();
    }
}