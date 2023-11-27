var timerId;
var running;
function ShowTimeOutDiv()
{
	var  div = document.getElementByID("TimeOutDiv");
	
	div.style.visisbility = "visible";
	startTimer();
	
  return;
}

function HideTimeOutDiv()
{

  var  div = document.getElementByID("TimeOutDiv");
	
  div.style.visisbility = "hidden";
 
  document.execCommand('Refresh');
  
  return;
}

function startTimer()
{
    running = true;

    now     = new Date();
    now     = now.getTime();
    
    endTime = now+(1000 * 60 * 2);

	var o = document.getElementById("TimeOutDivDisplay");
	o.innerHTML = "0.00"
	o.style.visibility = "visible"; 

    showCountDown();
}

function showCountDown()
{
    var now = new Date();
    now     = now.getTime();

    if(endTime - now <= 0)
    {
      stopTimer();
    }
    else
    {
		var delta   = new Date(endTime - now);
		var theMin  = delta.getMinutes();
		var theSec  = delta.getSeconds();
		var theTime = theMin;
		theTime += ((theSec < 10) ? ":0" : ":") + theSec;

		document.all.TimeOutDivDisplay.value = theTime;   
		var o = document.getElementById("TimeOutDivDisplay");
		o.innerHTML = theTime;
		o.style.visibility = "visible"; 
	    	
		if(running)
		{
			timerId = setTimeout("showCountDown()",1000);
		}
    }
}

function stopTimer()
{
    clearTimeout(timerId)
    running = false;
    
    var o = document.getElementById("TimeOutDivDisplay");
	o.innerHTML = "";
	o.style.visibility = "hidden"; 
	
	var o1 = document.getElementById("TimeOutDivMessage1");
	o1.innerHTML = "Your session has expired!";
	o1.style.visibility = "visible";
	o1.style.width = "100%";
	
	var o2 = document.getElementById("TimeOutDivMessage2");
	o2.innerHTML = ""
	o2.style.visibility = "hidden";	
}

function AddBodyOnLoadEvent(functn) {
    var prevOnload = window.onload;
    if (typeof window.onload != 'function') 
    {
        window.onload = functn;
    } else 
    {
        window.onload = function() 
        {
            if (prevOnload) 
            {
                prevOnload();
            }
            functn();
        }
    }
}
