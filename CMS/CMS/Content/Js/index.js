$(document).ready(function() {
    //响应式导航点击处理
    $('.btn-nav').click(function() {
        if ($('#screen').html() == undefined) {
            $('body').append(
                "<div id=\"screen\">" + "<div class=\"col-xs-2 z-index absolute\"style=\"color:#666;font-size:20px;left:10px;top:15px;\">" + "<div class=\"pointer btn-xs-nav\">" + "<span class=\"glyphicon glyphicon-remove\"></span>" + "</div>" + "</div>" + "<div class=\"col-xs-2 col-xs-offset-4\">" + "<img src=\"img/xs-logo.png\"class=\"center-block\"style=\"padding:15px 0px;\">" + "</div>" + "<div class=\"col-xs-4\"style=\"padding:20px 10px;font-weight:700;\">网达软件</div>" + "</div>"
            );
        }
        $('#screen').slideDown();
        $(document).scrollTop(0);
        $('.animate').css('top', 50 + $(document).scrollTop());
        $('.animate').animate({
            height: 'toggle'
        });
        $('body').css('overflow', 'hidden')
    });
    //底部响应式链接
    $('footer .panel-heading').click(function() {
        $(this).next().toggleClass('hidden-xs');
    });
    $('.hover').mouseenter(function() {
        var index = $(this).index();
        $('.hover').find('.font').removeClass('blue')
        $('.index-p-long1').hide();
        $('.index-p-long2').hide();
        $('.index-p-long3').hide();
        $('.index-p-long4').hide();
        $('.index-p-long').hide();
        $('#wrp').attr("src", "../Content/img/wrp.svg");
        $('#oms').attr("src", "../Content/img/oms.svg");
        $('#mam').attr("src", "../Content/img/mam.svg");
        $('#stp').attr("src", "../Content/img/stp.svg");
        $('#dma').attr("src", "../Content/img/dma.svg");
        switch (index) {
            case 0:
                $('.index-p-long').show();
                $('#wrp').attr("src", "../Content/img/wrp-s.svg");
                $(this).find('.font').addClass('blue');
                break;
            case 1:
                $('.index-p-long1').show();
                $('#oms').attr("src", "../Content/img/oms-s.svg");
                $(this).find('.font').addClass('blue');
                break;
            case 2:
                $('.index-p-long2').show();
                $('#mam').attr("src", "../Content/img/mam-s.svg");
                $(this).find('.font').addClass('blue');
                break;
            case 3:
                $('.index-p-long3').show();
                $('#stp').attr("src", "../Content/img/stp-s.svg");
                $(this).find('.font').addClass('blue');
                break;
            case 4:
                $('.index-p-long4').show();
                $('#dma').attr("src", "../Content/img/dma-s.svg");
                $(this).find('.font').addClass('blue');
                break;
        }
    });

    //菜单栏小屏展示关闭
    $(document).on('click', '.btn-xs-nav', function() {
        $('.animate').hide();
        $(this).parent().parent().slideUp();
        $('body').css('overflow', 'auto');
    });


    //服务页动画效果
    $(window).on('load',function(){
         var dfd=$.Deferred();
             dfd.then( $('.banner1').find('.img').fadeIn(2000)).then($('.banner1').find('.h_3').fadeIn(3500)).then($('.banner1').find('.p_').fadeIn(5000))  
    });

})
