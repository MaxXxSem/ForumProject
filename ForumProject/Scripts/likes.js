// JavaScript source code

//className == Record | Comment
function likes(className) {
    var likes = document.getElementsByClassName("like");
    for (var i = 0; i < likes.length; i++) {
        likes[i].addEventListener("click", function (e) {
            var xhttp = new XMLHttpRequest();
            if (e.target.value == "false") {                                                        //like
                e.target.style.backgroundImage = "url(/Content/icons/star_blue24.png)";
                e.target.value = true;
                xhttp.open('GET', '/Profile/Like' + className + '/' + e.target.name, true);
                xhttp.send();
            } else if (e.target.value == "true") {                                                  //unlike
                e.target.style.backgroundImage = "url(/Content/icons/star24.png)";
                e.target.value = false;
                xhttp.open('GET', '/Profile/Unlike' + className + '/' + e.target.name, true);
                xhttp.send();
            }
        });
    }
}