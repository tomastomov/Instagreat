function showBiographyTextBox() {
    let div = $(`#biographyTextBox`);
    let textArea = $(`#addBiography`);

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
