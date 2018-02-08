// JavaScript source code

/**
pageY and pageX - ��������� �� ��������
clientX and clientY - ��������� � ���� - getBoundingClientRect
 */
function buttonUpSidebar() {
    var up = document.getElementById('up');

    if (up != null) {
        window.addEventListener("scroll", function (e) {
            if (document.documentElement.getBoundingClientRect().top != 0) {            //���� �������� �� ��������� ����
                up.style.visibility = "visible";
            } else {
                up.style.visibility = "hidden";
            }

            if (document.getElementById("info").getBoundingClientRect().top < window.innerHeight) {                 //���� ������ �� ����� ������ �� ������ ������ ������ ������ (����� �������� �� ������)
                up.style.top = document.getElementById("info").getBoundingClientRect().top - 100 + "px";            //���������� � ������
            } else {
                up.style.bottom = 50 + "px";                                                                        //���������
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
        /*��������� sidebar*/
        window.addEventListener("scroll", function (e) {
            var rect = sidebar.getBoundingClientRect();
            var header = document.getElementById('header_bottom') || document.getElementsByTagName('header')[0];
            var header_bottom = header.getBoundingClientRect().bottom;

            if (window.pageYOffset < lastPosition) {                        //�������� �����
                sidebar.style.bottom = "unset";
                if (header_bottom > 0) {                            //���� header �� ������
                    if (header.id == 'header_bottom') {     //�������� �� ������� 'header_bottom' �� ��������
                        var main_style = getComputedStyle(document.getElementsByTagName("main")[0]);
                        sidebar.className = "sidebar_fixed";
                        sidebar.style.top = header_bottom + parseInt(main_style.marginTop) + parseInt(main_style.paddingTop) + "px";
                    } else {
                        sidebar.className = "sidebar_absolute";
                        sidebar.style.top = "unset";
                    }
                } else if (rect.top > 50) {                         //���� header �� ������� � ����� ���� sidebar
                    sidebar.className = "sidebar_fixed";
                    sidebar.style.top = 50 + "px";
                } else {                                            //���� �� ����� ���� sidebar
                    var top = rect.top;
                    sidebar.className = "sidebar_absolute";
                    sidebar.style.top = top + pageYOffset + "px";
                }
            } else if (window.pageYOffset > lastPosition) {                 //���� �������� ����
                sidebar.style.top = "unset";
                if (header_bottom > 0) {                            //���� header �����
                    sidebar.className = "sidebar_absolute";
                }
                else if (rect.bottom < window.innerHeight) {        //���� �� ����� header � ����� ��� sidebar
                    sidebar.className = "sidebar_fixed";
                    sidebar.style.bottom = 0 + "px";
                } else {                                            //���� �� ����� ��� sidebar
                    var top = rect.top;
                    sidebar.className = "sidebar_absolute";
                    sidebar.style.top = top + pageYOffset + "px";
                }
            }

            if (document.getElementById("info").getBoundingClientRect().top < window.innerHeight) {                 //���� ������ �� ����� ������ �� ������ ������ ������ ������ (����� �������� �� ������)
                sidebar.className = "sidebar_fixed";
                sidebar.style.top = "unset";
                sidebar.style.bottom = window.innerHeight - document.getElementById("info").getBoundingClientRect().top + "px";            //���������� � ������
            }

            lastPosition = window.pageYOffset;
        });
    }
}