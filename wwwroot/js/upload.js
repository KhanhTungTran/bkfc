var feedback = function(res) {
    if (res.success === true) {
        var get_link = res.data.link.replace(/^http:\/\//i, 'https://');
        img = document.getElementById("img-up");
        if (img==null){
            document.querySelector('.status').classList.add('bg-success');
            document.querySelector('.status').innerHTML =
                '<img id="img-up" class="img-up" alt="Imgur-Upload" src=\"' + get_link + '\"/>';
        }
        else{
            img.src=get_link;
        }
        document.getElementById("upload-img").value=get_link;
        document.getElementById("upload-img").readOnly=true;

    }
};

new Imgur({
    clientid: '4409588f10776f7', //You can change this ClientID
    callback: feedback
});