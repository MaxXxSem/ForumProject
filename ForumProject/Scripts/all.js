// JavaScript source code

/**
pageY and pageX - положение на странице
clientX and clientY - положение в окне - getBoundingClientRect
 */
function buttonUpSidebar() {
    var up = document.getElementById('up');

    if (up != null) {
        window.addEventListener("scroll", function (e) {
            if (document.documentElement.getBoundingClientRect().top != 0) {            //если документ не прокручен вниз
                up.style.visibility = "visible";
            } else {
                up.style.visibility = "hidden";
            }

            if (document.getElementById("info").getBoundingClientRect().top < window.innerHeight) {                 //если отступ от верха экрана до футера меньше высоты экрана (футер появился на экране)
                up.style.top = document.getElementById("info").getBoundingClientRect().top - 100 + "px";            //прилипнуть к футеру
            } else {
                up.style.bottom = 50 + "px";                                                                        //отлипнуть
            }
        });

        up.addEventListener("click", function (e) {
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
        /*прокрутка sidebar*/
        window.addEventListener("scroll", function (e) {
            var rect = sidebar.getBoundingClientRect();
            var header = document.getElementById('header_bottom') || document.getElementsByTagName('header')[0];
            var header_bottom = header.getBoundingClientRect().bottom;

            if (window.pageYOffset < lastPosition) {                        //движение вверх
                sidebar.style.bottom = "unset";
                if (header_bottom > 0) {                            //если header на экране
                    if (header.id == 'header_bottom') {     //проверка на наличие 'header_bottom' на странице
                        var main_style = getComputedStyle(document.getElementsByTagName("main")[0]);
                        sidebar.className = "sidebar_fixed";
                        sidebar.style.top = header_bottom + parseInt(main_style.marginTop) + parseInt(main_style.paddingTop) + "px";
                    } else {
                        sidebar.className = "sidebar_absolute";
                        sidebar.style.top = "unset";
                    }
                } else if (rect.top > 50) {                         //если header за экраном и видно верх sidebar
                    sidebar.className = "sidebar_fixed";
                    sidebar.style.top = 50 + "px";
                } else {                                            //если не видно верх sidebar
                    var top = rect.top;
                    sidebar.className = "sidebar_absolute";
                    sidebar.style.top = top + pageYOffset + "px";
                }
            } else if (window.pageYOffset > lastPosition) {                 //если движение вниз
                sidebar.style.top = "unset";
                if (header_bottom > 0) {                            //если header видно
                    sidebar.className = "sidebar_absolute";
                }
                else if (rect.bottom < window.innerHeight) {        //если не видно header и видно низ sidebar
                    sidebar.className = "sidebar_fixed";
                    sidebar.style.bottom = 0 + "px";
                } else {                                            //если не видно низ sidebar
                    var top = rect.top;
                    sidebar.className = "sidebar_absolute";
                    sidebar.style.top = top + pageYOffset + "px";
                }
            }

            if (document.getElementById("info").getBoundingClientRect().top < window.innerHeight) {                 //если отступ от верха экрана до футера меньше высоты экрана (футер появился на экране)
                sidebar.className = "sidebar_fixed";
                sidebar.style.top = "unset";
                sidebar.style.bottom = window.innerHeight - document.getElementById("info").getBoundingClientRect().top + "px";            //прилипнуть к футеру
            }

            lastPosition = window.pageYOffset;
        });
    }
}