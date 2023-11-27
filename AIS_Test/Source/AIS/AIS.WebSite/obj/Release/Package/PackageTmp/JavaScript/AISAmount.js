var myRegExp = /,/g;
function onkeypressAmount(evt, control, strdec, strNeg) {

    txtvalue = control.value.replace(myRegExp, '');
    var keychar = String.fromCharCode(evt.keyCode);
    strRegx = '[0-9';
    if (strdec == 'True') {
        strRegx += '.';
    }
    if (strNeg == 'True') {
        strRegx += '-';
    }
    strRegx += ']';
    var reg = new RegExp(strRegx);
    if (!reg.test(keychar)) {
        event.returnValue = false;
    }
    if (strNeg != 'True' && evt.keyCode == 45) {
        return false;
    }
    NegSplit = txtvalue.split('-');
    Str = (NegSplit.length == 1) ? NegSplit[0] : NegSplit[1];
    decSplit = Str.split('.');
    Str1 = (decSplit.length == 1) ? decSplit[0] : decSplit[1];
    if (decSplit.length == 1 && decSplit[0].length > 10 && evt.keyCode != 46 && evt.keyCode != 45) {
        var txt = '';
        if (window.getSelection) {
            txt = window.getSelection();
        }
        else if (document.getSelection) // FireFox
        {
            txt = document.getSelection();
        }
        else if (document.selection)  // IE 6/7
        {
            txt = document.selection.createRange().text;
        }
        if (txtvalue.length == txt.length) {
            event.returnValue = true;
        }
        else {
            event.returnValue = false;
        }
    }
    decindex = txtvalue.indexOf('.');
    Result2 = txtvalue.indexOf('-');
    if (Result2 != -1 && evt.keyCode == 45) {
        event.returnValue = false;
    }
    else if (Result2 == -1 && evt.keyCode == 45) {
    if (myFlag != 0 && control.value == '0')
        control.value = "-";
    else {
        control.value = "-" + txtvalue;
        
    }
    if (control.value == '-0.00') {
        control.value = "-";
    } 
    event.returnValue = false;
    }
    if (decindex != -1 && evt.keyCode == 46) {
        event.returnValue = false;
    }
    if (Str.length > 13) {
        var txt = '';
        if (window.getSelection) {
            txt = window.getSelection();
        }
        else if (document.getSelection) // FireFox
        {
            txt = document.getSelection();
        }
        else if (document.selection)  // IE 6/7
        {
            txt = document.selection.createRange().text;
        }
        if (txtvalue.length == txt.length) {
            event.returnValue = true;
        }
        else {
            event.returnValue = false;
        }
    }
    myFlag = 0;
    return true;
}


var myFlag=0;
String.prototype.endsWith = function(str)
{ return (this.match(str + "$") == str) }
String.prototype.startsWith = function(str)
{ return (this.match("^" + str) == str) }
var myRegExp = /,/g;
function onkeyupAmount(evt, control, strdec) {
    myFlag = 0;
    Result2 = control.value.indexOf('-');
    if (control.value == "-") {
        var range = control.createTextRange();
        range.move("word", 1);
        range.select();
        return;
    }
    if (Result2 > 0) {
        control.value = control.value.substring(1, 16);
        var range = control.createTextRange();
        range.move("word", 0);
        range.select();
        return;
    }
    if (strdec == 'True') {
        a = control.value.split('.');
        max = Result2 != -1 ? 12 : 11;
        if (a.length > 1) {
            if (evt.keyCode != 8 && evt.keyCode != 37 && evt.keyCode != 39 && evt.keyCode != 46 && evt.keyCode != 16
        && evt.keyCode != 36) {
                if (a[1].length > 2)
                    control.value = a[0] + '.' + a[1].substring(0, 2);
                if (a[0].length > max)
                    control.value = a[0].substring(0, max) + '.' + a[1].substring(0, 2);
            }
        }
    }
}
function onblurAmount(control, strdec) {
    nStr = control.value;
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    a = nStr.split('-');
    abc = a.length > 1 ? a[1] : a[0];
    xyz = abc.split('.');
    Result2 = control.value.indexOf('-');
    max = Result2 > 0 ? 12 : 11;
    if (xyz[0].length > max) {
        if (a.length > 1)
            x1 = '-';
        else x1 = '';
        x1 = x1 + xyz[0].substring(0, 11) + '.' + xyz[0].substring(11, 15);
    }
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    if (x2.length > 1) {
        x2 = x2.slice(0, 3);
    }
    if (xyz[0].length > 12) {
        control.value = x1 + x2;
    }
    else {
        control.value = x1 + x2;
    }
    if (strdec == 'True') {
        if (control.value.endsWith("."))
            control.value += '00';

        if (control.value.indexOf('.') == -1)
            control.value += '.00';
        if (control.value.charAt(control.value.length - 2) == ".")
            control.value += '0';
    }
    control.value = control.value.replace(",.", ",");

}

function onfocusAmount(control) {
    myFlag = 1;
    control.value = control.value.replace(myRegExp, '')
    control.select();
}


