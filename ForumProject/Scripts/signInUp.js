//check SignIn/SignUp data after submitting
function checkData() {
    document.getElementsByName("_form")[0].onsubmit = function (e) {
        var fields = document.getElementsByClassName("input_text");
        for (var i = 0; i < fields.length; i++) {
            if (fields[i].value != "") {
                var regexp = /[^\d\w]+/i;                               //anything except of characters and numbers
                if (fields[i].value.trim().search(regexp) != -1) {
                    fields[i].style.borderColor = "red";
                    e.preventDefault();
                    window.event.preventDefault();
                    return;
                } else {
                    fields[i].style.borderColor = "#c0c0c0";
                }
            } else {
                fields[i].style.borderColor = "red";
                e.preventDefault();
                return;
            }
        }
    }
}