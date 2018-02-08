// JavaScript source code

function likes() {
    var likes = document.getElementsByClassName("like");
    for (var i = 0; i < likes.length; i++) {
        likes[i].addEventListener("click", function (e) {
            var xhttp = new XMLHttpRequest();
            if (e.target.value == "false") {
                e.target.style.backgroundImage = "url(/Content/icons/star_blue24.png)";
                e.target.value = true;
                xhttp.open('GET', '/Profile/LikeRecord/' + e.target.name, true);
                xhttp.send();
            } else if (e.target.value == "true") {
                e.target.style.backgroundImage = "url(/Content/icons/star24.png)";
                e.target.value = false;
                xhttp.open('GET', '/Profile/UnlikeRecord/' + e.target.name, true);
                xhttp.send();
            }
        });
    }
}