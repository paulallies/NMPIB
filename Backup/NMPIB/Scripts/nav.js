function goToLink(link)
{
    try {
        if (link != "" || link != null)
            document.location.href = link;
    }
    catch (e) {
        alert("goToLink: " + e.value);
    }
}

function getSub(s) {
    try {
        return document.getElementById(s);
    }
    catch (e) {
        alert("getSub: " + e.value);
    }
}

function ToggleElement(id)
{
    var elm = document.getElementById(id);
    
    if(elm.style.display == "none")
        elm.style.display = "block";
    else
        elm.style.display = "none";
}

function topHover(sender, child, over) {

	try {
        var divs = sender.getElementsByTagName("div");
        var positions = new Array("left", "middle", "right");
        if (divs[0].className.indexOf("selected") < 0) {
            for (i = 0; i < divs.length; i++) {
                for (j = 0; j < positions.length; j++) {
                    if (divs[i].className.indexOf(positions[j]) > -1) { 
                        if(over)
                            divs[i].className = "topnav_item_" + positions[j] + "_hover";
                        else
                            divs[i].className = "topnav_item_" + positions[j];
                    }
                }
            }
        }
        if(child != null || child != '')
        {
        	toggleSubnav(child, over);
        }
    }
    catch (e) {
        alert("topHover: " + e.value);
    }
}

function toggleSubnav(el, onoff) {
    try {
//        if (onoff)
//            el.style.display = 'block';
//        else
//            el.style.display = 'none';
    }
    catch (e) {
        alert("togglesubnav: " + e.value);
    }
}

function subnavHover(sender, parent, over){
    try{
        topHover(parent, sender, over);
        toggleSubnav(sender, over);
    }
    catch(e){
        alert("subnavHover: " + e.value);
    }
}

function sublinkToggle(el, over) {
    try {
        var divs = el.getElementsByTagName("div");
        var positions = new Array("left", "middle", "right");
        if (divs[0].className.indexOf("selected") < 0) {
            
	        for (i = 0; i < divs.length; i++) {
	            for (j = 0; j < positions.length; j++) {
	                if (divs[i].className.indexOf(positions[j]) > -1) {
	                    if (over) {
	                        divs[i].className = "newSubNnav_item_" + positions[j] + "_hover";
	                    }
	                    else {
	                        divs[i].className = "newSubNnav_item_" + positions[j];
	                    }
	                }
	            }
	        }
	     }
    }
    catch (e) {
        alert("sublinkToggle: " + e.value);
    }
}

/*menu & submenu javascript*/



/*topics javascript*/

function setSelectedTopic(senderid, q) {

    //try {
        var header = document.getElementById("topics_headers_container");
        var topic_headers = header.getElementsByTagName("div");
        var content = document.getElementById("topics_contents_container");
        var topic_contents = content.childNodes;

        var positions = new Array("left", "center", "right");

        // turn off all topic tabs
        for (i = 0; i < topic_headers.length; i++) {            
            if (topic_headers[i].className.indexOf("selected") > -1) {
                for (j = 0; j < positions.length; j++) {
                    if (topic_headers[i].className.indexOf(positions[j]) > -1) {
                        topic_headers[i].className = "topic_header_" + positions[j];
                    }
                }
            }
        }
        
        //hide selected content
        for (i = 0; i < topic_contents.length; i++) {
                topic_contents[i].className = "topic_content";
                //break;
        }

        // select sender tab 
        var child = document.getElementById(senderid).getElementsByTagName("div");
        for (i = 0; i < child.length; i++) {
            for (j = 0; j < positions.length; j++) {
                if (child[i].className.indexOf(positions[j]) > -1) {
                    child[i].className = "topic_header_" + positions[j] + "_selected";
                }
            }
        }

        //now reappear proper content
        //alert(q);
        document.getElementById(q).className = "topic_content_selected";
//    }
//    catch (e) {
//        alert(e.value);
//    }
}

function setSelectedSubTopicEx(sender,container, q) {
    //try 
    //{    
       var contents = document.getElementById(container).getElementsByTagName("div");
       var headers = sender.parentNode.parentNode.getElementsByTagName("div");
       
       // hide all headers
       for(var i=0;i<headers.length;i++)
       {
            if(headers[i].className.indexOf("subtopic_header") > -1)
                headers[i].className = "subtopic_header";
       }
       
       //hide all contents
       for(var i=0;i<contents.length;i++)
       {
            if(contents[i].id.indexOf("_subtopiccontent") > -1)
            {
                //alert(contents[i].id);
                contents[i].className = "subtopic_content";
            }
       }
       
       // show selected
       sender.parentNode.className = "subtopic_header_selected";
       document.getElementById(q).className = "subtopic_content_selected";                

    //}
    //catch (e) {
        //alert("setSelectedSubTopicEx: " + e.value);
    //}
}