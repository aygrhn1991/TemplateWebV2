/**
 * luara
 * Jquery图片切换插件 基于Jquery1.8.3
 * Version 0.0.1 2014-8-21
 * By Mr.肥鹅 Practice makes perfect.
 * 代码整理：大头网 www.datouwang.com

 于 2015.7.1 由 pk 修改

luara="yes"       开启焦点图模式(必填)
width="500"       设置焦点图宽度(必填)
height="300"      设置焦点图高度(必填)
interval="2000"   设置图片切换间隔时间(选填，默认3000)
deriction="left"  设置焦点图切换方向(选填，可选择left或top，默认渐隐渐显)
select="seleted"  设置焦点图选中的样式(选填，默认为seleted)
onbg="true"       设置焦点图是否为背景模式(选填，默认关闭)

使用范例:
<div class="default" luara="yes" width="100%" height="300" interval="2000" deriction="left" select="seleted" onbg="true"> 
<div class="default" luara="yes" width="400" height="300" interval="2000" deriction="top" select="seleted"> 
<div class="default" luara="yes" width="100%" height="300"> 
<div class="default" luara="yes" width="500" height="300" deriction="left" select="seleted"> 
<div class="default" luara="yes" width="500" height="300" deriction="top" select="seleted"> 

 */
jQuery.fn.extend({
    /*
     配置器
     config = {
        width : '容器宽度',
        height :  '容器高度',
        deriction : '图片滑动方向' || top || left
        interval : '图片切换间隔时间' 默认3000毫秒
        speed : '动画时长' 指定毫秒数
        selected : '导航器选中样式'
     }
     */
    luara : function(config){
        var
            //容器及元素相关变量
            $warpElement = $(this).eq(0),
            $containerElement = $(this).find('ul').eq(0),
            $containerUlLiElements = $containerElement.find('li'),
            $containerOlLiElements = $(this).find('ol').eq(0).find('li'),
            $imgElements = $warpElement.find('img'),
            imgCount = $imgElements.length,

            //配置相关变量
            config = config || {},
            width = config.width || getImageSize().width,
            height = config.height || getImageSize().height,
            deriction = config.deriction || '',
            className = 'luara-'+ deriction,
            interval = (config.interval>0?config.interval:-config.interval) || 3000,
            speed = getSpeed(),
            selected = config.selected,

            //开启背景模式
            onbgimg = config.onbgimg || false,

            //循环控制相关变量
            index = 0,
            isStop = false,
            timer,

            //函数相关变量
            animateHandler;


        //初始化外围容器大小
        //初始化图片容器大小
        //初始化图片大小
        //初始化导航器样式
        $warpElement.width(width).height(height).addClass(className);
        $containerElement.width(getContainerWidth(deriction)).height(height);
        $containerUlLiElements.width(width).height(height);
        $containerOlLiElements.eq(0).addClass(selected);

        //初始函数销毁
        (function(){
            getContainerWidth = null;
            getImageSize = null;
            getSpeed = null;
        })();

        //根据指定的方向设置图片容器的宽度
        //未指定该值时，设置为第一张图片宽度
        function getContainerWidth(){
            var temp;
            switch (deriction){
                case 'top':
                    temp = width;
                    break;
                case 'left':
                    temp = width * imgCount;
                    break;
                default :
                    temp = width;
                    break;
            }
            return temp;
        }

        //当没有指定外围包装器的宽度时
        //获取第一张图片的宽度值
        function getImageSize(){
            var $img = $warpElement.find('img').eq(0),
                size = {};
            size.width = $img.width();
            size.height = $img.height();
            return size;
        }

        //获得动画时长数
        //默认动画时长为切换间隔时长一半
        function getSpeed(speed){
            var speed = speed || config.speed || (interval/6);
            if(speed>interval){
                speed = interval;
            }else if(speed<interval&&speed<0){
                speed = arguments.callee(-speed);
            }
            return speed;
        }

        //根据指定值返回图片切换效果函数
        //默认返回的函数为淡进淡出切换
        animateHandler = (function(){
            switch (deriction){
                case  'top':
                    return function(){
                        $containerElement.animate({top:-height*index+'px'},speed);
                    };
                case 'left':
                    return function(){
                        $containerElement.animate({left:-width*index+'px'},speed);
                    };
                default :
                    return function(){
                        $containerUlLiElements.hide().eq(index).fadeIn(speed);
                    }
            }
        })();

        //给导航器绑定click事件
        $containerOlLiElements.mouseover(function(){
             $containerOlLiElements.eq(index).removeClass(selected);
             index = $containerOlLiElements.index($(this));
             $(this).addClass(selected);
             animateHandler();
        });

        //给外围容器绑定鼠标进入事件
        //给外围容器绑定鼠标离开事件
        $warpElement.mouseenter(function(){
            clearTimeout(timer);
        }).mouseleave(function(){
            loop();
        });

        //循环体函数
        function loop(){
            timer = setTimeout(function(){
                index++;
                $containerOlLiElements.eq(index-1).removeClass(selected);
                if(index==imgCount){
                    index = 0;
                }
                animateHandler();
                $containerOlLiElements.eq(index).addClass(selected);
                loop();
            },interval);
        };

        if (onbgimg){
            $containerUlLiElements.each(function(idx){
                $li = $(this);
                var li_img = $li.find('img');
                var li_dis = (idx > 0) ? 'display:none;' : 'display:block;';
                $li.attr('style','background:url('+ $(li_img).attr('src') +') 50% 100%;'+ li_dis);
                $(li_img).hide();
            });
        }

        loop();
    }
});

//add by pk at 2015.7.1
$(function(){
    $('div[luara="yes"]').each(function(){
        var settings = {
            width    : $(this).attr('width'),
            height   : $(this).attr('height'),
            interval : $(this).attr('interval'),
            selected : $(this).attr('select') || "seleted",
            deriction: $(this).attr('deriction') || '',
            onbgimg  : $(this).attr('onbg') || false
        };
        $(this).luara(settings);
    });

});

document.write('<style>');

document.write('/*默认*/');
document.write('.default{}');
document.write('.default ol{position:relative;width: 80px;height: 20px;top:-30px;left:60px;}');
document.write('.default ol li{float:left;width: 10px;height: 10px;margin: 5px;background: #fff;}');
document.write('.default ol li.seleted{background: #1AA4CA;}');

document.write('/*渐隐*/');
document.write('.luara-{position:relative;padding:0;overflow: hidden;}');
document.write('.luara- ul{padding: inherit;margin: 0;}');
document.write('.luara- ul li{display: none;padding: inherit;margin: inherit;list-style: none;}');
document.write('.luara- ul li:first-child{display:block;}');
document.write('.luara- ul li img{width: inherit;height: inherit;}');

document.write('/*上滑*/');
document.write('.luara-top{position:relative;padding:0;overflow: hidden;}');
document.write('.luara-top ul{position: relative;padding: inherit;margin: 0;}');
document.write('.luara-top ul li{padding: inherit;margin: inherit;list-style: none;}');
document.write('.luara-top ul li img{width: inherit;height: inherit;}');

document.write('/*左滑*/');
document.write('.luara-left{position:relative;padding:0;overflow: hidden;}');
document.write('.luara-left ul{position: relative;padding: inherit;margin: 0;}');
document.write('.luara-left ul li{float: left;padding: inherit;margin: inherit;list-style: none;}');
document.write('.luara-left ul li img{width: inherit;height: inherit;}');

document.write('</style>');