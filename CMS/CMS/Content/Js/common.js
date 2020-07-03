var headers = ["H1", "H2", "H3", "H4", "H5", "H6"];
$(function () {

    $(".accordion").click(function (e) {
        var target = e.target,
            name = target.nodeName.toUpperCase();

        if ($.inArray(name, headers) > -1) {
            var subItem = $(target).next();
            $(name).removeClass("onn");

            //slideUp all elements (except target) at current depth or greater
            var depth = $(subItem).parents().length;
            var allAtDepth = $(".accordion p, .accordion div").filter(function () {
                if ($(this).parents().length >= depth && this !== subItem.get(0)) {

                    return true;
                }
            });
            $(allAtDepth).slideUp("fast");

            //slideToggle target content and adjust bottom border if necessary
            subItem.slideToggle("fast", function () {
                //$(".accordion :visible:last").css("border-radius","0 0 0px 0px");
            });
            //$(target).css({"border-bottom-right-radius":"0", "border-bottom-left-radius":"0"});
            $(target).addClass("onn");
        }
    });
});
jQuery.fn.addFavorite = function (l, h) {
    return this.click(function () {
        var t = jQuery(this);
        if (jQuery.browser.msie) {
            window.external.addFavorite(h, l);
        } else if (jQuery.browser.mozilla || jQuery.browser.opera) {
            t.attr("rel", "sidebar");
            t.attr("title", l);
            t.attr("href", h);
        } else {
            alert("请使用Ctrl+D将本页加入收藏夹！");
        }
    });
};