// JavaScript source code

/**
pageY and pageX - position on page
clientX and clientY - position in window (func - getBoundingClientRect)
 */
function buttonUpSidebar() {
    var up = document.getElementById('up');

    if (up != null) {
        window.addEventListener("scroll", function (e) {
            if (document.documentElement.getBoundingClientRect().top != 0) {                                //if document is not scrolled down
                up.style.visibility = "visible";
            } else {
                up.style.visibility = "hidden";
            }

            if (document.getElementById("info").getBoundingClientRect().top < window.innerHeight) {         //if offset from top of screen to footer less than screen height (footer has appeared on the screen)
                up.style.top = document.getElementById("info").getBoundingClientRect().top - 100 + "px";    //stick to footer
            } else {
                up.style.bottom = 50 + "px";                                                                //unstick
            }
        });

        up.addEventListener("click", function (e) {                                                         //button "up" click
            var pos = window.pageYOffset;
            inter = setInterval(function () {
                if (pos <= 0) {
                    clearInterval(inter);
                }
                pos -= 30;
                window.scrollTo(0, pos);
            }, 1);
        });
    }

    var sidebar = document.getElementById('sidebar');
    var lastPosition = 0;

    if (sidebar != null) {
        /*sidebar scrolling*/
        window.addEventListener("scroll", function (e) {
            var rect = sidebar.getBoundingClientRect();
            var header = document.getElementById('header_bottom') || document.getElementsByTagName('header')[0];
            var header_bottom = header.getBoundingClientRect().bottom;

            if (window.pageYOffset < lastPosition) {                            //moving up
                sidebar.style.bottom = "unset";
                if (header_bottom > 0) {                                        //if header on the screen
                    if (header.id == 'header_bottom') {                         //check if 'header_bottom' is on page
                        var main_style = getComputedStyle(document.getElementsByTagName("main")[0]);
                        sidebar.className = "sidebar_fixed";
                        sidebar.style.top = header_bottom + parseInt(main_style.marginTop) + parseInt(main_style.paddingTop) + "px";
                    } else {
                        sidebar.className = "sidebar_absolute";
                        sidebar.style.top = "unset";
                    }
                } else if (rect.top > 50) {                                     //if header is out of screen and sidebar top has appeared
                    sidebar.className = "sidebar_fixed";
                    sidebar.style.top = 50 + "px";
                } else {                                                        //if can't see sidebar top
                    var top = rect.top;
                    sidebar.className = "sidebar_absolute";
                    sidebar.style.top = top + pageYOffset + "px";
                }
            } else if (window.pageYOffset > lastPosition) {                     //moving down
                sidebar.style.top = "unset";
                if (header_bottom > 0) {                                        //if can see header
                    sidebar.className = "sidebar_absolute";
                }
                else if (rect.bottom < window.innerHeight) {                    //if can't see header and can see sidebar bottom
                    sidebar.className = "sidebar_fixed";
                    sidebar.style.bottom = 0 + "px";
                } else {                                                        //if can't see sidebar bottom
                    var top = rect.top;
                    sidebar.className = "sidebar_absolute";
                    sidebar.style.top = top + pageYOffset + "px";
                }
            }

            if (document.getElementById("info").getBoundingClientRect().top < window.innerHeight) {     //if offset from top of screen to footer less than screen height (footer has appeared on the screen)
                sidebar.className = "sidebar_fixed";
                sidebar.style.top = "unset";
                sidebar.style.bottom = window.innerHeight - document.getElementById("info").getBoundingClientRect().top + "px";            //stick to the footer
            }

            lastPosition = window.pageYOffset;
        });
    }
}