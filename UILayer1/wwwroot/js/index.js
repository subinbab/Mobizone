$(document).ready(function () {
    $("#Index").addClass("loading");
    getData(function () {
        $("#Index").removeClass("loading");
    })
    $("#sortLowToHigh").click(function () {
        //alert("clicked")
        document.getElementById("Index").innerHTML = '<div id="index"></div>'
        $("#Index").addClass("loading");
        getDataLowToHigh(function () {
            $("#Index").removeClass("loading");
        })

    })
    $("#sortHighToLow").click(function () {
        //alert("clicked")
        document.getElementById("Index").innerHTML = '<div id="index"></div>'
        $("#Index").addClass("loading");
        getDataHighToLow(function () {
            $("#Index").removeClass("loading");
        })

    })

    $("#filterSubmit").click(function () {
        //alert("clicked")
        document.getElementById("Index").innerHTML = '<div id="index"></div>'
        $("#Index").addClass("loading");
        filterByBrand(function () {
            $("#Index").removeClass("loading");
        })
    })
    $("#search-btn").click(function () {
        document.getElementById("Index").innerHTML = '<div id="Index"></div>'
        $("#Index").addClass("loading");
        search(function () {
            $("#Index").removeClass("loading");
        })
    })
})      
function getData(callback) {
    $.ajax({
        url: '/user/indexPartial',
        type: 'get',
        success: function (data) {
            //console.log(data);
            callback();
            document.getElementById("Index").innerHTML = '<div id="Index">' + data + '</div>';
        }
    })
}
function getDataLowToHigh(callback) {
    $.ajax({
        url: '/user/sortLowToHighPartial',
        type: 'get',
        success: function (data) {
            callback();
            document.getElementById("Index").innerHTML = '<div id="Index">' + data + '</data>';
        }
    })
}
function getDataHighToLow(callback) {
    $.ajax({
        url: '/user/SortHighToLowPartial',
        type: 'get',
        success: function (data) {
            callback()
            document.getElementById("Index").innerHTML = '<div id="Index">' + data + '</data>';
        }
    })
}
function filterByBrand(callback) {
    var brandname = $("#brandname").val();
    $.ajax({
        url: '/user/filterByBrandName',
        type: 'post',
        data: 'brandName=' + brandname,
        success: function (data) {
            //alert(data)
            callback()
            document.getElementById("Index").innerHTML = '<div id="Index">' + data + '</data>';
        },
        error: function (data) {
            alert(data)
        }
    })
}
function search(callback) {
    var name = $("#tags").val();
    if (name) {
        $.ajax({
            url: '/user/search',
            type: 'post',
            data: 'name=' + name,
            success: function (data) {
                //alert(data)
                callback()
                document.getElementById("Index").innerHTML = data;
            },
            error: function (data) {
                alert(data)
            }
        })
    }
    else {
        alert("Please type Here")
        getData(callback)
    }
    
}
function pagination(data) {
    document.getElementById("Index").innerHTML = '<div id="index"></div>'
    $("#Index").addClass("loading");
    IndexRequest(function () {
        $("#Index").removeClass("loading");
    },data)
}
function IndexRequest(callback,data) {
    $.ajax({
        url: '/user/indexPartial',
        type: 'post',
        data: 'count=' + data,
        success: function (data) {
            //alert(data)
            callback()
            document.getElementById("Index").innerHTML = '<div id="Index">' + data + '</data>';
        },
        error: function (data) {
            alert(data)
        }
    })
}