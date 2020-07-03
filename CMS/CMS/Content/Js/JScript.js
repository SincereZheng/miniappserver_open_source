
// JScript 文件  验证码点击刷新

    function change()
      {
          var img =document.getElementById("Image1");
          img.src=img.src+'?';
      }






// CheckBox全选
//<asp:CheckBox ID="CheckBox1" runat="server" onclick="javascript:SelectAll(this);" Text="全选" />

    function SelectAll(tempControl)
      {
           //将除头模板中的其它所有的CheckBox取反 
           var theBox=tempControl;
           xState=theBox.checked;    
           elem=theBox.form.elements;
           for(i=0;i<elem.length;i++)
           if(elem[i].type=="checkbox" && elem[i].id!=theBox.id)
            {
                 if(elem[i].checked!=xState)
                 elem[i].click();
            }
      }
    
    
    
    
    
    
    //只允许输入整数
    //使用方法
    // onkeydown="onlyNum();" onpaste="return false"//不允许粘贴
    //style="ime-mode:Disabled"禁用输入法
    function onlyNum() 
    { 
        if(!(event.keyCode==46)&&!(event.keyCode==8)&&!(event.keyCode==37)&&!(event.keyCode==39)) 
        if(!((event.keyCode>=48&&event.keyCode<=57)||(event.keyCode>=96&&event.keyCode<=105))) 
        event.returnValue=false; 
    } 
    
    
    
    
    
    
    
    
    
  