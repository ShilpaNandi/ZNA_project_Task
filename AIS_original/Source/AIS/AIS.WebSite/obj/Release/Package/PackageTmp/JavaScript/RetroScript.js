
//Function to Allow only single dot any amount field
function AmountValidation(evt, control) {

    txtvalue = control.value;

    Result = txtvalue.indexOf('.');
    if (evt.keyCode == 46) {
        if (Result != -1)
            return false;
        else
            return true;
    }
    Result2 = txtvalue.indexOf('-');
    if (evt.keyCode == 45) {
        if (Result2 != -1)
            return false;
        else
            return true;
    }
    //       count=txtvalue.split('.');

    //         if(count[1].length>1)
    //         {
    //             return false;
    //         }
    //         else
    //         {
    //             return true;
    //         }

    return true;
}


function setMouseOverColor(element) {
    oldgridSelectedColor = element.style.backgroundColor;
    element.style.backgroundColor = 'lightblue';
    element.style.cursor = 'hand';
    element.style.textDecoration = 'underline';
}

function setMouseOutColor(element) {
    element.style.backgroundColor = oldgridSelectedColor;
    element.style.textDecoration = 'none';
}
function doKeypress(control, max) {
    maxLength = parseInt(max);
    value = control.value;
    if (maxLength && value.length > maxLength - 1) {
        event.returnValue = false;
        maxLength = parseInt(maxLength);
    }
}
// Cancel default behavior
function doBeforePaste(control, max) {
    maxLength = parseInt(max);
    if (maxLength) {
        event.returnValue = false;
    }
}
// Cancel default behavior and create a new paste routine
//function doPaste(control, max) {      Edge changes - created new function
//    maxLength = parseInt(max);
//    value = control.value;
//    if (maxLength) {
//        event.returnValue = false;
//        maxLength = parseInt(maxLength);
//        var oTR = control.document.selection.createRange();
//        var iInsertLength = maxLength - value.length + oTR.text.length;
//        var sData = window.clipboardData.getData("Text").substr(0, iInsertLength);
//        oTR.text = sData;
//    }
//}

function doPaste(control, max) {
    maxLength = parseInt(max);
    var sDataLength = event.clipboardData.getData("Text").length;
    var sData = event.clipboardData.getData("Text");
    if (sDataLength > maxLength) {
        var fData = sData.substring(0, maxLength);
        document.getElementById("<%=txtComment.ClientID%>").value = fData;
        event.returnValue = false;
    }
}

function toUpperCase(evt, control) {
    if (control.value.length < 3) {
        var char = String.fromCharCode(evt.which || evt.keyCode);

        // Is it a lowercase character?    
        if (/[a-z]/.test(char)) {
            // Append its uppercase version
            control.value += char.toUpperCase();

            // Cancel the original event
            evt.cancelBubble = true;
            return false;
        }
    }
    else {
        return false;
    }

}

function RemoveCommas(e) {

    Num = e.value;
    if (isNaN(Num)) {
        Num = LTTrim(Num);
        e.value = Num; //.replace(/\-/g, '');
    }

}
///////////////////////////////////////////////////////////////////////////////////////////////////

// Function: FormatNumWithDecAmt
// This function will format amounts fields into a comma separated value with two digits 
// to the right of the decimal point.  Function assumes that only "1234567890-.," are passed as
// a variable
// The function will only allow maximum of MAX_LENGHT digits, the remaining numbers will be removed.
// The function will convert a number as follows
// 234234       to 234.234.00
// 4353.12      to 4,353.12
// 8911.1       to 8,911.10
// 28,,,,,23    to 2,823.00
// -2322        to -2,322.00
// -234----     to User receives a warning that a valid number was not entered
// 243..28      to User receives a warning that a valid number was not entered
function FormatNumWithDecAmt(e, MAX_LENGTH) {

    var temp1 = "";
    var temp2 = "";
    var decimalcount = 0;
    var decimalval1 = 0;
    var decimalval2 = 0;

    Num = e.value;

    // Validate that a valid digit is entered.
    // Main purpose of this code is to prevent a user from entering
    // multiple decimal points
    if (isNaN(Num.replace(/\,/g, ''))) {
        alert('You must enter a valid amount in the text field.');
        e.value = "";
        e.focus();
        return false;
    }

    // If user did not enter anything then make sure to leave
    // field blank by returning here
    if (Num.length == 0)
        return true;

    //Check for negative amount and store boolean indicator
    //Indicator is used at end of function
    var negFlag = false;
    if (Num.length > 1 && Num.charAt(0) == "-") {
        Num = Num.substring(1, Num.length);
        negFlag = true;
    }

    // Remove leading and trailing spaces, "," "$", and "-", and leading zeroes characters
    Num = LTTrim(Num);

    // If the leading char is the decimal (this way .00 => 0.00) or if all leading zero's 
    // were removed and nothing is left (this way 0 => 0.00, decimal places will be added later), 
    // add a zero in front of it
    if (Num.charAt(0) == "." || Num.length == 0)
        Num = "0" + Num;

    // Determine location of decimal point, if it exists
    // Remove the decimal point and digits to the right from the subsequent for loops.
    for (var k = 0; k <= Num.length - 1; k++) {
        var oneChar = Num.charAt(k);
        if (oneChar == ".") {
            break;
        }
        decimalcount++;
    }
    // Preserve the two digits to the right of the decimal point.
    // If nothing entered then variables default to zero
    if (Num.charAt(decimalcount + 1).length == 1) {
        decimalval1 = Num.charAt(decimalcount + 1)
    }
    if (Num.charAt(decimalcount + 2).length == 1) {
        decimalval2 = Num.charAt(decimalcount + 2)
    }

    // Navigate through the number backwards and add a comma every third digit
    // Stop navigating when all integers have been passed or maximum number
    // of allowed characters have been reached.
    var count = 0;
    var maxcount = 0;

    for (var k = decimalcount - 1; k >= 0; k--) {
        maxcount++;
        if (maxcount > MAX_LENGTH) {
            break;
        }
        var oneChar = Num.charAt(k);
        if (count == 3) {
            temp1 += ",";
            temp1 += oneChar;
            count = 1;
            continue;
        }
        else {
            temp1 += oneChar;
            count++;
        }
    }

    // Reverse the value 
    for (var k = temp1.length - 1; k >= 0; k--) {
        var oneChar = temp1.charAt(k);
        temp2 += oneChar;
    }

    // Append the two digits entered by the user to the right of the decimal point
    // If none were entered then fill with two zeroes
    e.value = temp2 + "." + decimalval1 + decimalval2;

    // Add a negative sign if one was entered.
    if (negFlag) { e.value = "-" + e.value; }
}

// Function: FormatNumNoDecAmt
// This function will format amounts fields into a comma separated value 
// Function assumes that only "1234567890-," are passed as
// a variable
// The function will only allow maximum of MAX_LENGHT digits, the remaining numbers will be removed.
// The function will convert a number as follows
// 234234       to 234,234
// 4353.12      to 4,353
// 8911.1       to 8,911
// 28,,,,,23    to 2,823
// -2322        to -2,322
// -234----     to User receives a warning that a valid number was not entered
// 243..28      to User receives a warning that a valid number was not entered
function FormatNumNoDecAmt(e, MAX_LENGTH) {

    var temp1 = "";
    var temp2 = "";
    var decimalcount = 0;
    var decimalval1 = 0;
    var decimalval2 = 0;

    Num = e.value;

    // Validate that a valid digit is entered.
    // Main purpose of this code is to prevent a user from entering
    // multiple decimal points
    if (isNaN(Num.replace(/\,/g, ''))) {
        alert('You must enter a valid amount in the text field.');
        e.value = "";
        e.focus();
        return false;
    }

    // If user did not enter anything then make sure to leave
    // field blank by returning here
    if (Num.length == 0)
        return true;

    //Check for negative amount and store boolean indicator
    //Indicator is used at end of function
    var negFlag = false;
    if (Num.length > 1 && Num.charAt(0) == "-") {
        Num = Num.substring(1, Num.length);
        negFlag = true;
    }

    // Remove leading and trailing spaces, "," "$", and "-", and leading zeroes characters
    Num = LTTrim(Num);

    // If the leading char is the decimal (this way .00 => 0.00) or if all leading zero's 
    // were removed and nothing is left (this way 0 => 0.00, decimal places will be added later), 
    // add a zero in front of it
    if (Num.charAt(0) == "." || Num.length == 0)
        Num = "0" + Num;

    // Determine location of decimal point, if it exists
    // Remove the decimal point and digits to the right from the subsequent for loops.
    for (var k = 0; k <= Num.length - 1; k++) {
        var oneChar = Num.charAt(k);
        if (oneChar == ".") {
            break;
        }
        decimalcount++;
    }

    // Navigate through the number backwards and add a comma every third digit
    // Stop navigating when all integers have been passed or maximum number
    // of allowed characters have been reached.
    var count = 0;
    var maxcount = 0;

    for (var k = decimalcount - 1; k >= 0; k--) {
        maxcount++;
        if (maxcount > MAX_LENGTH) {
            break;
        }
        var oneChar = Num.charAt(k);
        if (count == 3) {
            temp1 += ",";
            temp1 += oneChar;
            count = 1;
            continue;
        }
        else {
            temp1 += oneChar;
            count++;
        }
    }

    // Reverse the value 
    for (var k = temp1.length - 1; k >= 0; k--) {
        var oneChar = temp1.charAt(k);
        temp2 += oneChar;
    }

    // Set text field to formatted value
    e.value = temp2;

    // Add a negative sign if one was entered.
    if (negFlag) { e.value = "-" + e.value; }
}

// This Function is used for formatting rates.
// Params: MaxConstant - user by isValidRate function. Max constant allowed.
//         MaxDecimalLength - max number of decimal spaces.
// RB 9/10: Added
// RB 9/12: Updated to handle dollar numeric
function FormatRate(e, MaxConstant, MaxDecimalLength) {

    Rate = LTTrim(e.value);

    if (Rate.length != 0) {
        Rate = RemoveLeadZeros(Rate)
        if (Rate.length == 0) { e.value = "0.0"; }
        else if (isValidRate(Rate, MaxConstant)) {

            var Ar = Rate.split(".");
            if (Ar[1] == null || Ar[1].length == 0) { Ar[1] = "0"; }
            else if (Ar[1].length > MaxDecimalLength) {
                Ar[1] = Ar[1].substring(0, MaxDecimalLength);
            }

            if (Ar[0].length == 0) { Ar[0] = "0"; }
            e.value = Ar[0] + "." + Ar[1];
        }
        //else if (IsNumeric(Rate)) 
        else {
            alert("Please enter a valid rate < " + (MaxConstant + 1) + ".");
            e.value = "";
            e.focus();
        }
    }
}

// leading and trailing trim, also removes "," "$", and "-", and leading zeroes characters
function LTTrim(value) {
    return value.replace(/^[0]+|\,|\$/g, '');
    //         return value.replace(/\,|\$/g, '');
}

// Remove leading zeros from numeric string
// RB 9/10: Added
function RemoveLeadZeros(value) {
    return value.replace(/^[0]+/g, '');
}

// Determines if argument is a numeric
function IsNumeric(sText) {
    sText = LTTrim(sText);
    if (sText.length > 1 && (sText.charAt(0) == "-" || sText.charAt(0) == "+")) {
        sText = sText.substring(1, sText.length);
    }

    var ValidChars = "0123456789.";
    var IsNumber = true;
    var Char;

    for (i = 0; i < sText.length && IsNumber == true; i++) {
        Char = sText.charAt(i);
        if (ValidChars.indexOf(Char) == -1) {
            IsNumber = false;
        }
    }
    return IsNumber;
}

// This Function is used to validate rates.
// Params: MaxConstant - Max constant allowed.
// RB 9/10: Added
function isValidRate(sText, MaxConstant) {
    sText = LTTrim(sText);
    var Ar;
    if (sText.length != 0) {
        if (!IsNumeric(sText)) { return false; }
        Ar = sText.split(".");
        if (Ar[0] != null || Ar[0].length != 0) {
            var c = parseInt(Ar[0], 10);
            if (c > MaxConstant) {
                return false;
            }
        }
    }

    return true;
}      
