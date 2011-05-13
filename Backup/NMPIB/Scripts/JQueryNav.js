$(document).ready(function() {
    //selected nav items
    var topnavName = "";
    var subnavName = "";

    if (GetSelectedTopNavName() != "") {

        topnavName = GetSelectedTopNavName();
        selectSubNav(GetTopNavIndex(topnavName));
    }
    if (GetSelectedSubNavName() != "") {

        subnavName = GetSelectedSubNavName();
        topnavName = "topnav_" + GetSelectedSubNavIndex($("#" + subnavName).parent()[0].id);
        selectSubNav(GetSelectedSubNavIndex($("#" + subnavName).parent()[0].id));
        selectTopNav(GetSelectedSubNavIndex($("#" + subnavName).parent()[0].id));
    }



    $("div[id^=topnav_]").hover(
                function() {
                    selectTopNav($(this).attr("id").replace(/topnav_/, ""));
                },
                function() {
                    if ($(this).attr("id") != topnavName) { // do not deselect the current top nav item
                        $(this).find("div[class^=topnav_item_left]").attr("class", "topnav_item_left");
                        $(this).find("div[class^=topnav_item_right]").attr("class", "topnav_item_right");
                        $(this).find("div[class^=topnav_item_middle]").attr("class", "topnav_item_middle");
                    }
                }
            );

    $("div[id^=subnavItem]").hover(
                function() {
                    $(this).find(".newSubNnav_item_left").attr("class", "newSubNnav_item_left_selected");
                    $(this).find(".newSubNnav_item_right").attr("class", "newSubNnav_item_right_selected");
                    $(this).find(".newSubNnav_item_middle").attr("class", "newSubNnav_item_middle_selected");
                },
                function() {
                    //alert(subnavName);
                    if ($(this).attr("id") != subnavName) { // do not deselect the current top nav item

                        $(this).find("div[class^=newSubNnav_item_left]").attr("class", "newSubNnav_item_left");
                        $(this).find("div[class^=newSubNnav_item_right]").attr("class", "newSubNnav_item_right");
                        $(this).find("div[class^=newSubNnav_item_middle]").attr("class", "newSubNnav_item_middle");
                    }
                }
            );

});


function GetSelectedSubNavName() {

    if ($(".newSubNnav_item_left_selected").parent()[0]) {                
        return $(".newSubNnav_item_left_selected").parent()[0].id;
    }

    return "";

}

function GetSelectedSubNavIndex(subnavName) {
    return subnavName.replace(/subnav_/, "");   
}

function GetSelectedTopNavName() {
    if ($(".topnav_item_left_selected").parent()[0]) {                              //if a top nav is selected 
        return $(".topnav_item_left_selected").parent()[0].id;                //Get the selected top nav id                                      
    }
    return "";
}

function GetTopNavIndex(topnavName) {
    return topnavName.replace(/topnav_/, "");                        //Get the index from the id string
}

function selectTopNav(topnavindex) {
    $("#topnav_" + topnavindex).find(".topnav_item_left").attr("class", "topnav_item_left_selected");
    $("#topnav_" + topnavindex).find(".topnav_item_right").attr("class", "topnav_item_right_selected");
    $("#topnav_" + topnavindex).find(".topnav_item_middle").attr("class", "topnav_item_middle_selected");
}

function selectsubNav(subnavindex) {
    $("#subnav_" + subnavindex).find(".newSubNnav_item_left").attr("class", "newSubNnav_item_left_selected");
    $("#subnav_" + subnavindex).find(".newSubNnav_item_right").attr("class", "newSubNnav_item_right_selected");
    $("#subnav_" + subnavindex).find(".newSubNnav_item_middle").attr("class", "newSubNnav_item_middle_selected");
}

function selectSubNav(subnavindex) {
    $("#subnav_" + subnavindex).show();
}

function goToLink(link) {
    try {
        if (link != "" || link != null)
            document.location.href = link;
    }
    catch (e) {
        alert("goToLink: " + e.value);
    }
}