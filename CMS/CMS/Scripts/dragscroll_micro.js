/**
 * @fileoverview dragscroll - scroll area by dragging
 * @version 0.0.4 micro
 * 
 * @license MIT, see http://github.com/asvd/intence
 * @copyright 2015 asvd <heliosframework@gmail.com> 
 */


$$$_isdrag=false;

window.addEventListener("load", function() {
    var addEventListener = 'addEventListener';
    var elems = document.getElementsByClassName('fixed-table-body');
    for (var i = 0; i < elems.length;) {
        (function(elem, lastClientX, lastClientY, pushed) {
            elem[addEventListener]('mousedown', function(e) {
				$$$_isdrag=false;
                pushed = 1;
                lastClientX = e.clientX;
                lastClientY = e.clientY;

                e.preventDefault();
                e.stopPropagation();
            }, 0);
            
            window[addEventListener]('mousemove', function(e) {
                if (pushed) {
                    elem.scrollLeft -=
                        (- lastClientX + (lastClientX=e.clientX));
                    elem.scrollTop -=
                        (- lastClientY + (lastClientY=e.clientY));

					$$$_isdrag=true;
                }
            }, 0);
             
            window[addEventListener]('mouseup', function(){
				if(pushed==1)
				{
				    pushed = 0;
					e.stopPropagation();  
				}
                
            }, 0);

         })(elems[i++]);
    }
}, 0);

